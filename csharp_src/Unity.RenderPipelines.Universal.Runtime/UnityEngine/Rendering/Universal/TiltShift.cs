using System;

namespace UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("Post-processing/Tilt Shift")]
public sealed class TiltShift : VolumeComponent, IPostProcessComponent
{
	[Tooltip("模糊半径")]
	public ClampedFloatParameter radius = new ClampedFloatParameter(0f, 0f, 2f);

	[Tooltip("迭代次数")]
	public ClampedIntParameter iteration = new ClampedIntParameter(1, 1, 3);

	[Tooltip("渐变起始")]
	public ClampedFloatParameter blurStart = new ClampedFloatParameter(0.8f, 0f, 1f);

	[Tooltip("渲染延迟(勾选，渲染将延迟到 Tone Mapping 之后进行，解决直接对 HDR 模糊出现异常颜色问题)")]
	public BoolParameter deffered = new BoolParameter(value: true, overrideState: true);

	[Tooltip("0 Mesh; 1 ViewPort; 2 Scissor")]
	public ClampedIntParameter debug = new ClampedIntParameter(2, 0, 2);

	public bool IsActive()
	{
		return (double)radius.value > 0.0;
	}

	public bool IsTileCompatible()
	{
		return true;
	}
}
