using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu]
public sealed class DropShadowRendererFeature : ScriptableRendererFeature
{
    private DropShadowPass _currentPass;

    public override void Create()
    {
        // �p�X�𐶐�����
        _currentPass = new DropShadowPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // �p�X�������_�����O�p�C�v���C���ɒǉ�
        renderer.EnqueuePass(_currentPass);
    }
}