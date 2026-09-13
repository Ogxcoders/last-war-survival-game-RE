using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScreenGFXPanel : BaseGFXPanel
{
	private float urpResolutionScale = 1f;

	private float fullResolutionScale = 1f;

	private int initScreenWidth;

	private int initScreenHeight;

	private int targetFramerate = 60;

	public ScreenGFXPanel()
		: base("屏幕")
	{
		initScreenWidth = Mathf.Max(Screen.width, Screen.height);
		initScreenHeight = Mathf.Min(Screen.width, Screen.height);
		targetFramerate = Application.targetFrameRate;
	}

	public override void Init()
	{
		UniversalRenderPipelineAsset universalRenderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
		urpResolutionScale = universalRenderPipelineAsset.renderScale;
	}

	public override void DrawGUI()
	{
		GUILayout.Label($"原始屏幕分辨率:{initScreenWidth} {initScreenHeight}");
		GUILayout.Label($"URP主相机分辨率:{(int)((float)Camera.main.pixelWidth * urpResolutionScale)} {(int)((float)Camera.main.pixelHeight * urpResolutionScale)}");
		urpResolutionScale = DrawSlider("URP主相机缩放", urpResolutionScale, 0.25f, 1f);
		if (GUILayout.Button("确定切换"))
		{
			(QualitySettings.renderPipeline as UniversalRenderPipelineAsset).renderScale = urpResolutionScale;
		}
		GUILayout.Space(30f);
		fullResolutionScale = DrawSlider("总屏幕缩放", fullResolutionScale, 0.25f, 1f);
		if (GUILayout.Button("确定切换"))
		{
			int width = (int)((float)initScreenWidth * fullResolutionScale);
			int height = (int)((float)initScreenHeight * fullResolutionScale);
			Screen.SetResolution(width, height, fullscreen: true);
		}
		GUILayout.Space(30f);
		GUILayout.Label($"帧频:{Application.targetFrameRate}");
		targetFramerate = (int)GUILayout.HorizontalSlider(targetFramerate, 5f, 60f);
		if (GUI.changed)
		{
			Application.targetFrameRate = targetFramerate;
		}
		GUILayout.Space(30f);
	}
}
