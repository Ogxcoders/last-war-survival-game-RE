using System;
using UnityEngine;

namespace FibMatrix.Rendering;

[ExecuteAlways]
[HelpURL("https://rivergame.feishu.cn/wiki/wikcnJK0zo4Kv3dvBC93AXQIcPg")]
internal class WindWaveMotionData : MonoBehaviour
{
	[SerializeField]
	[Tooltip("海浪后往前运动的距离")]
	private PreviewableTextureCurve WaveMotionPrimaryTexture = new PreviewableTextureCurve();

	public static string WaveMotionPrimaryPropertyName = "_WaveMotionPrimary";

	public static int WaveMotionPrimaryPropertyId = Shader.PropertyToID(WaveMotionPrimaryPropertyName);

	[SerializeField]
	[Tooltip("奔涌噪波（前突）的强度")]
	private PreviewableTextureCurve WaveMotionNoiseStrengthTexture = new PreviewableTextureCurve();

	public static string WaveMotionNoiseStrengthPropertyName = "_WaveMotionNoiseStrength";

	public static int WaveMotionNoiseStrengthPropertyId = Shader.PropertyToID(WaveMotionNoiseStrengthPropertyName);

	[SerializeField]
	[Tooltip("比如：由浅变深再变浅")]
	private PreviewableTextureCurve WaveMotionAlphaTexture = new PreviewableTextureCurve();

	public static string WaveMotionAlphaPropertyName = "_WaveMotionAlpha";

	public static int WaveMotionAlphaPropertyId = Shader.PropertyToID(WaveMotionAlphaPropertyName);

	[SerializeField]
	[Tooltip("海浪横截面形状，一般无需修改")]
	private PreviewableTextureCurve WaveShapeTexture = new PreviewableTextureCurve();

	public static string WaveShapePropertyName = "_WaveShape";

	public static int WaveShapePropertyId = Shader.PropertyToID(WaveShapePropertyName);

	private void OnEnable()
	{
		RefreshCallback.Callback = (Action)Delegate.Remove(RefreshCallback.Callback, new Action(Init));
		RefreshCallback.Callback = (Action)Delegate.Combine(RefreshCallback.Callback, new Action(Init));
		Init();
	}

	private void OnDisable()
	{
		Clear();
	}

	private void OnDestroy()
	{
		RefreshCallback.Callback = (Action)Delegate.Remove(RefreshCallback.Callback, new Action(Init));
		RefreshCallback.Invoke(base.gameObject);
	}

	private void Init()
	{
		Clear();
		WaveMotionPrimaryTexture.shaderPropertyName = WaveMotionPrimaryPropertyName;
		Shader.SetGlobalTexture(WaveMotionPrimaryPropertyId, WaveMotionPrimaryTexture.GetTexture2D());
		WaveMotionNoiseStrengthTexture.shaderPropertyName = WaveMotionNoiseStrengthPropertyName;
		Shader.SetGlobalTexture(WaveMotionNoiseStrengthPropertyId, WaveMotionNoiseStrengthTexture.GetTexture2D());
		WaveMotionAlphaTexture.shaderPropertyName = WaveMotionAlphaPropertyName;
		Shader.SetGlobalTexture(WaveMotionAlphaPropertyId, WaveMotionAlphaTexture.GetTexture2D());
		WaveShapeTexture.shaderPropertyName = WaveShapePropertyName;
		Shader.SetGlobalTexture(WaveShapePropertyId, WaveShapeTexture.GetTexture2D());
	}

	private void Clear()
	{
		WaveMotionPrimaryTexture.Release();
		WaveMotionNoiseStrengthTexture.Release();
		WaveMotionAlphaTexture.Release();
		WaveShapeTexture.Release();
	}
}
