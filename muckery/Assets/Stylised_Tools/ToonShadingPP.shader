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

    // --- UPGRADED POSTERIZE MATH ---
    // Now accepts a "softness" parameter to blend the color bands
    float posterize(float In, float steps, float softness)
    {
        float scaled = In * steps;
        float base = floor(scaled);
        float fractional = frac(scaled);
        
        // Clamp softness to prevent math errors
        softness = clamp(softness, 0.001, 0.999); 
        
        // A wider smoothstep creates a visual gradient between color bands
        float transition = smoothstep(1.0 - softness, 1.0, fractional);
        
        return (base + transition) / steps;
    }

    // Properties exposed to C#
    float _PosterizeAmount;
    float _OutlineThickness;
    
    TEXTURE2D_X(_InputTexture);

    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        // ==========================================
        // 🛠️ TWEAK THESE VALUES TO CHANGE THE LOOK 🛠️
        // ==========================================
        
        // 1. BAND SOFTNESS (0.01 to 0.99)
        // 0.01 = Sharp, harsh anime bands
        // 0.50 = Soft, painted gradients
        float bandSoftness = 0.4; 
        
        // 2. HIGHLIGHT THRESHOLD (0.8 to 3.0+)
        // Any brightness above this number will ignore the toon effect and stay smooth.
        // Lower this if highlights aren't showing up. Raise it if everything looks too smooth.
        float highlightThreshold = 1.2; 
        
        // 3. SHADOW MINIMUM (0.0 to 1.0)
        // Prevents shadows from becoming pure black.
        float shadowMinimum = 0.05;
        // ==========================================

        uint2 positionSS = uint2(input.positionCS.xy);
        float3 sourceColor = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;

        // --- COLOR SEPARATION MATH ---
        float luminance = Luminance(sourceColor);
        float3 normalizedColor = sourceColor / (luminance + 0.0001);

        // Posterize the luminance using our new Softness variable
        float posterizedLuma = posterize(luminance, max(_PosterizeAmount, 2.0), bandSoftness);
        
        // Shadow Lift
        posterizedLuma = max(posterizedLuma, shadowMinimum); 

        // --- HIGHLIGHT PROTECTION ---
        // Create a smooth mask based on how bright the pixel is.
        // If the pixel is brighter than our threshold, it smoothly blends back to its original un-banded brightness!
        float highlightMask = smoothstep(highlightThreshold * 0.8, highlightThreshold * 1.2, luminance);
        posterizedLuma = lerp(posterizedLuma, luminance, highlightMask);
        // ----------------------------

        // Recombine the protected brightness with the pure color
        sourceColor = normalizedColor * posterizedLuma;

        // --- DEPTH OUTLINES ---
        uint t = (uint)max(1.0, _OutlineThickness);

        float dUp    = Linear01Depth(LoadCameraDepth(positionSS + uint2(0, t)), _ZBufferParams);
        float dDown  = Linear01Depth(LoadCameraDepth(positionSS - uint2(0, t)), _ZBufferParams);
        float dLeft  = Linear01Depth(LoadCameraDepth(positionSS - uint2(t, 0)), _ZBufferParams);
        float dRight = Linear01Depth(LoadCameraDepth(positionSS + uint2(t, 0)), _ZBufferParams);

        float depthDifference = abs(dUp - dDown) + abs(dLeft - dRight);
        float centerDepth = Linear01Depth(LoadCameraDepth(positionSS), _ZBufferParams);

        float nearThreshold = 0.003; 
        float farThreshold = 0.01;
        float currentThreshold = lerp(nearThreshold, farThreshold, centerDepth);
        
        float isEdge = step(currentThreshold, depthDifference); 
        
        float outlineFade = 1.0 - smoothstep(0.05, 0.3, centerDepth);
        float rawDepth = LoadCameraDepth(positionSS);
        float isNotSkybox = step(0.000001, rawDepth);
        
        isEdge = isEdge * outlineFade * isNotSkybox;
        
        float3 outlineColor = float3(0.05, 0.05, 0.05); 
        sourceColor = lerp(sourceColor, outlineColor, isEdge);

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