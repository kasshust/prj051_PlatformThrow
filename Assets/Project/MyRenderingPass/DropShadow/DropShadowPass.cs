using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DropShadowPass : ScriptableRenderPass
{
    // FrameDebuggerやProfiler用の名前
    private const string ProfilerTag = nameof(DropShadowPass);
    private readonly ProfilingSampler _profilingSampler = new ProfilingSampler(ProfilerTag);

    // どのタイミングでレンダリングするか
    private readonly RenderPassEvent _renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;

    // 対象とするRenderQueue
    private readonly RenderQueueRange _renderQueueRange = RenderQueueRange.all;

    // ShaderのTagsでLightModeがこれになっているシェーダのみをレンダリング対象とする
    private readonly ShaderTagId _shaderTagId = new ShaderTagId("DropShadow");

    private FilteringSettings _filteringSettings;

    public DropShadowPass()
    {
        _filteringSettings = new FilteringSettings(_renderQueueRange);
        renderPassEvent = _renderPassEvent;
    }

    // レンダリング処理前に呼ばれる
    // レンダーターゲットを変えたりできる
    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
    }

    // レンダリング処理を書く
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

    // レンダリング処理後に呼ばれる
    // レンダリング処理に使用したリソースを片づけたりする
    public override void FrameCleanup(CommandBuffer cmd)
    {
    }
}