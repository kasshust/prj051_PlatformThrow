using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StencilToTexRendererFeature : ScriptableRendererFeature
{
    class StencilToTexturePass : ScriptableRenderPass
    {
        private readonly Material _stencilToTextureMaterial;
        private readonly Material _postEffectMaterial;
        private readonly RenderTexture _renderTexture;
        private RenderTargetIdentifier _cameraRenderTargetIdentifier;

        public StencilToTexturePass(Material stencilToTextureMaterial, Material postEffectMaterial)
        {
            _stencilToTextureMaterial = stencilToTextureMaterial;
            _postEffectMaterial = postEffectMaterial;
            _renderTexture = new RenderTexture(100, 100, 0, RenderTextureFormat.ARGB32);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            if (cameraTextureDescriptor.width != _renderTexture.width ||
                cameraTextureDescriptor.height != _renderTexture.height)
            {
                _renderTexture.Release();
                _renderTexture.width = cameraTextureDescriptor.width;
                _renderTexture.height = cameraTextureDescriptor.height;
                _renderTexture.Create();
            }
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.camera.cameraType == CameraType.Preview)
            {
                return;
            }
            CommandBuffer cmd = CommandBufferPool.Get("StencilToTexture");


            using (new ProfilingScope(cmd, new ProfilingSampler("StencilToTexture")))
            {
                var camera = renderingData.cameraData.camera;
                _cameraRenderTargetIdentifier = renderingData.cameraData.renderer.cameraColorTarget;

                // 新たに用意したテクスチャをRTに設定
                CoreUtils.SetRenderTarget(cmd, _renderTexture, renderingData.cameraData.renderer.cameraDepthTarget);

                // 全てをコピー
                cmd.Blit(_cameraRenderTargetIdentifier, _renderTexture);
                cmd.Blit(_cameraRenderTargetIdentifier, _renderTexture, _stencilToTextureMaterial);

                // Matrix4x4 PlaneTransform = Matrix4x4.TRS(Camera.main.transform.TransformPoint(Vector3.forward), Camera.main.transform.rotation, Vector3.one);
                // cmd.DrawMesh(RenderingUtils.fullscreenMesh, PlaneTransform, _stencilToTextureMaterial, 0, 4);
            }

            /*

                // 3. ポストエフェクトにStencilToTextureを渡し，カメラにレンダリングする
                cmd.SetRenderTarget(_cameraRenderTargetHandle);
                _postEffectMaterial.SetTexture(Shader.PropertyToID("_OcclusionTex"), _renderTexture);
                Blit(cmd, ref renderingData, _postEffectMaterial);

            */

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);


        }

        public override void FrameCleanup(CommandBuffer cmd)
        {

        }

    }

    private Material _stencilToTextureMaterial;
    public Material postEffectMaterial;

    private StencilToTexturePass _stencilToTexturePass;

    public override void Create()
    {
        _stencilToTextureMaterial = new Material(Shader.Find("Custom/StencilToTexture"));
        _stencilToTexturePass = new StencilToTexturePass(_stencilToTextureMaterial, postEffectMaterial)
        {
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_stencilToTexturePass);
    }
}