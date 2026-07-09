using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ToonShadingRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public Shader shader;
        [Range(1, 256)]
        public float posterizeAmount = 1f;
        public bool isEnabled = true;

        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public Settings settings = new Settings();

    class ToonShadingPass : ScriptableRenderPass
    {
        private Material material;
        private Settings settings;

        private RTHandle temporaryColorTexture;

        public ToonShadingPass(Settings settings)
        {
            this.settings = settings;
        }

        public void Setup(Material material)
        {
            this.material = material;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;

            RenderingUtils.ReAllocateIfNeeded(
                ref temporaryColorTexture,
                descriptor,
                FilterMode.Bilinear,
                TextureWrapMode.Clamp,
                name: "_ToonTemp");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!settings.isEnabled || material == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("Toon Shading");

            material.SetFloat("_PosterizeAmount", settings.posterizeAmount);

            RTHandle source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            Blitter.BlitCameraTexture(cmd, source, temporaryColorTexture, material, 0);
            Blitter.BlitCameraTexture(cmd, temporaryColorTexture, source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }

        public void Dispose()
        {
            temporaryColorTexture?.Release();
        }
    }

    private ToonShadingPass pass;
    private Material material;

    public override void Create()
    {
        if (settings.shader == null)
            return;

        material = CoreUtils.CreateEngineMaterial(settings.shader);

        pass = new ToonShadingPass(settings)
        {
            renderPassEvent = settings.renderPassEvent
        };

        pass.Setup(material);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.isEnabled && material != null)
            renderer.EnqueuePass(pass);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(material);
        pass?.Dispose();
    }
}