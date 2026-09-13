namespace UnityEngine.Rendering.Universal;

public class Shadow3SDFeature : ScriptableRendererFeature
{
	[Range(0.1f, 2f)]
	public float shadowRTWidthScale = 0.9f;

	[Range(0.1f, 2f)]
	[Tooltip("垂直方向的精度容易被受影面放大出现瑕疵，所以独立放大")]
	public float shadowRTHeightScale = 1.2f;

	[Range(0.1f, 2f)]
	[Tooltip("保存扩大的投影区域解决边缘阴影问题，大于1时比屏幕区域保存的多")]
	public float shadowScreenRangeScale = 1.2f;

	[Range(1f, 64f)]
	[Tooltip("在基准受影面上下高度范围；太大会降低精度，容易出现acne；太小会使caster超过高度部分被裁切，receiver中显示为阴影")]
	public float heightRange = 16f;

	public bool rtFormatUseShadowmap = true;

	public bool rtPrecision16Bit = true;

	public bool pointFilter;

	[Space(10f)]
	[Tooltip("false时物体是否投影受默认SM机制控制；true时可见opaque物体满足filter等条件可投影")]
	public bool customFilter;

	public LayerMask LayerMask;

	[Space(10f)]
	public Material debugOverrideMaterial;

	private Shadow3SDPass _pass;

	public override void Create()
	{
		_pass = new Shadow3SDPass(LayerMask);
		_pass.debugOverrideMaterial = debugOverrideMaterial;
		_pass.shadowRTSWidthcale = Mathf.Max(0.1f, shadowRTWidthScale);
		_pass.shadowRTHeightScale = Mathf.Max(0.1f, shadowRTHeightScale);
		_pass.shadowScreenRangeScale = Mathf.Max(0.1f, shadowScreenRangeScale);
		_pass.rtFormatUseShadowmap = rtFormatUseShadowmap;
		_pass.rtPrecision16Bit = rtPrecision16Bit;
		_pass.pointFilter = pointFilter;
		_pass.heightRange = heightRange;
		_pass.customFilter = customFilter;
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (renderingData.shadowData.supportsMainLightShadows || renderingData.cameraData.renderType != CameraRenderType.Base || renderingData.cameraData.maxShadowDistance <= 0f)
		{
			return;
		}
		int mainLightIndex = renderingData.lightData.mainLightIndex;
		if (mainLightIndex == -1)
		{
			return;
		}
		VisibleLight visibleLight = renderingData.lightData.visibleLights[mainLightIndex];
		if (visibleLight.light.shadows != LightShadows.None)
		{
			Bounds outBounds;
			if (visibleLight.lightType != LightType.Directional)
			{
				Debug.LogWarning("Only directional lights are supported as main light.");
			}
			else if (renderingData.cullResults.GetShadowCasterBounds(mainLightIndex, out outBounds))
			{
				renderer.EnqueuePass(_pass);
			}
		}
	}
}
