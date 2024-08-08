using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu]
public sealed class DropShadowRendererFeature : ScriptableRendererFeature
{
    private DropShadowPass _currentPass;

    public override void Create()
    {
        // パスを生成する
        _currentPass = new DropShadowPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // パスをレンダリングパイプラインに追加
        renderer.EnqueuePass(_currentPass);
    }
}