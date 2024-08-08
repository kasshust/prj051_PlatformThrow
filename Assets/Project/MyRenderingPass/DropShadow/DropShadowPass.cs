using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DropShadowPass : ScriptableRenderPass
{
    // FrameDebugger��Profiler�p�̖��O
    private const string ProfilerTag = nameof(DropShadowPass);
    private readonly ProfilingSampler _profilingSampler = new ProfilingSampler(ProfilerTag);

    // �ǂ̃^�C�~���O�Ń����_�����O���邩
    private readonly RenderPassEvent _renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;

    // �ΏۂƂ���RenderQueue
    private readonly RenderQueueRange _renderQueueRange = RenderQueueRange.all;

    // Shader��Tags��LightMode������ɂȂ��Ă���V�F�[�_�݂̂������_�����O�ΏۂƂ���
    private readonly ShaderTagId _shaderTagId = new ShaderTagId("DropShadow");

    private FilteringSettings _filteringSettings;

    public DropShadowPass()
    {
        _filteringSettings = new FilteringSettings(_renderQueueRange);
        renderPassEvent = _renderPassEvent;
    }

    // �����_�����O�����O�ɌĂ΂��
    // �����_�[�^�[�Q�b�g��ς�����ł���
    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
    }

    // �����_�����O����������
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var cmd = CommandBufferPool.Get(ProfilerTag);
        using (new ProfilingScope(cmd, _profilingSampler))
        {
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);

            var drawingSettings =
                CreateDrawingSettings(_shaderTagId, ref renderingData, SortingCriteria.CommonTransparent);
            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref _filteringSettings);
        }
    }

    // �����_�����O������ɌĂ΂��
    // �����_�����O�����Ɏg�p�������\�[�X��ЂÂ����肷��
    public override void FrameCleanup(CommandBuffer cmd)
    {
    }
}