using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu]
public sealed class InkRendererFeature : ScriptableRendererFeature
{
    private InkRenderPass _currentPass;

    public RenderTexture renderTexture;

    public override void Create()
    {
        // パスを生成する
        _currentPass = new InkRenderPass(renderTexture);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // パスをレンダリングパイプラインに追加
        renderer.EnqueuePass(_currentPass);
    }
}