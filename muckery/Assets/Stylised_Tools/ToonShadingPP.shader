Shader "Hidden/Shader/ToonShadingPP"
{
    HLSLINCLUDE
    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float2 texcoord : TEXCOORD0;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    Varyings Vert(Attributes input)
    {
        Varyings output;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
        return output;
    }

    // --- CLEAN ANTI-CRAWLING MATH (NO NOISE) ---
    float posterize(float In, float steps)
    {
        float scaled = In * steps;
        float base = floor(scaled);
        float fractional = frac(scaled);
        
        // Microscopic smooth gradient instead of static noise
        float bandSmoothness = 0.05; 
        float transition = smoothstep(1.0 - bandSmoothness, 1.0, fractional);
        
        return (base + transition) / steps;
    }

    // Properties exposed to C#
    float _PosterizeAmount;
    float _OutlineThickness;
    
    TEXTURE2D_X(_InputTexture);

    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        uint2 positionSS = uint2(input.positionCS.xy);
        float3 sourceColor = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;

        // --- COLOR SEPARATION MATH ---
        // 1. Get the actual brightness of the pixel
        float luminance = Luminance(sourceColor);
        
        // 2. Separate the raw color from the HDR brightness
        float3 normalizedColor = sourceColor / (luminance + 0.0001);

        // 3. Posterize ONLY the luminance using the clean math
        float posterizedLuma = posterize(luminance, max(_PosterizeAmount, 2.0));
        
        // 4. Shadow Lift: Prevent pure black shadows
        posterizedLuma = max(posterizedLuma, 0.05); 

        // 5. Recombine the posterized brightness with the pure color
        sourceColor = normalizedColor * posterizedLuma;

        // --- DEPTH OUTLINES ---
        uint t = (uint)max(0.0, _OutlineThickness);

        float dUp    = Linear01Depth(LoadCameraDepth(positionSS + uint2(0, t)), _ZBufferParams);
        float dDown  = Linear01Depth(LoadCameraDepth(positionSS - uint2(0, t)), _ZBufferParams);
        float dLeft  = Linear01Depth(LoadCameraDepth(positionSS - uint2(t, 0)), _ZBufferParams);
        float dRight = Linear01Depth(LoadCameraDepth(positionSS + uint2(t, 0)), _ZBufferParams);

        float depthDifference = abs(dUp - dDown) + abs(dLeft - dRight);
        float centerDepth = Linear01Depth(LoadCameraDepth(positionSS), _ZBufferParams);

        // Dynamic Threshold
        float nearThreshold = 0.003; 
        float farThreshold = 0.01;
        float currentThreshold = lerp(nearThreshold, farThreshold, centerDepth);
        
        float isEdge = step(currentThreshold, depthDifference); 
        
        // Distance Fade
        float outlineFade = 1.0 - smoothstep(0.05, 0.3, centerDepth);
        float rawDepth = LoadCameraDepth(positionSS);
        float isNotSkybox = step(0.000001, rawDepth);
        
        isEdge = isEdge * outlineFade * isNotSkybox;
        
        float3 outlineColor = float3(0.05, 0.05, 0.05); 
        sourceColor = lerp(sourceColor, outlineColor, isEdge);

        // Final saturate
        sourceColor = saturate(sourceColor);
        return float4(sourceColor, 1);
    }

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            Name "New Post Process Volume"

            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
                #pragma fragment CustomPostProcess
                #pragma vertex Vert
            ENDHLSL
        }
    }
    Fallback Off
}