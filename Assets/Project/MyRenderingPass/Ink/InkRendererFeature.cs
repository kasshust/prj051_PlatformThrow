using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu]
public sealed class InkRendererFeature : ScriptableRendererFeature
{
    private InkRenderPass _currentPass;

    public RenderTexture renderTexture;

    public override void Create()
    {
        // �p�X�𐶐�����
        _currentPass = new InkRenderPass(renderTexture);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // �p�X�������_�����O�p�C�v���C���ɒǉ�
        renderer.EnqueuePass(_currentPass);
    }
}