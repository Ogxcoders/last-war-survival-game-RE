using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class QualitySettingGFXPanel : BaseGFXPanel
{
	private Camera mainCamera;

	private int initScreenWidth;

	private int initScreenHeight;

	private int pixelWidthMax = 1920;

	private int pixelHeightMax = 1080;

	private int smaaQuality;

	private string[] smaaQualityString = new string[3] { "低", "中", "高" };

	public QualitySettingGFXPanel()
		: base("品质设置")
	{
		initScreenWidth = Mathf.Max(Screen.width, Screen.height);
		initScreenHeight = Mathf.Min(Screen.width, Screen.height);
		mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
	}

	public override void Init()
	{
		pixelWidthMax = Mathf.RoundToInt((float)initScreenWidth * 1f / (float)initScreenHeight * (float)pixelHeightMax);
		UniversalAdditionalCameraData component = mainCamera.GetComponent<UniversalAdditionalCameraData>();
		smaaQuality = (int)component.antialiasingQuality;
	}

	public override void DrawGUI()
	{
		GUILayout.Label("贴图分辨率Full,HalfWidth,QuadWidth");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("1"))
		{
			QualitySettings.masterTextureLimit = 0;
		}
		if (GUILayout.Button("1/2"))
		{
			QualitySettings.masterTextureLimit = 1;
		}
		if (GUILayout.Button("1/4"))
		{
			QualitySettings.masterTextureLimit = 2;
		}
		if (GUILayout.Button("1/8"))
		{
			QualitySettings.masterTextureLimit = 3;
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(30f);
		UniversalRenderPipelineAsset universalRenderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
		if (GUILayout.Button("URPAsset HDR:" + universalRenderPipelineAsset.supportsHDR))
		{
			universalRenderPipelineAsset.supportsHDR = !universalRenderPipelineAsset.supportsHDR;
		}
		GUILayout.Space(30f);
		GUILayout.BeginHorizontal();
		int num = pixelWidthMax;
		pixelWidthMax = Convert.ToInt32(DrawInputField("分辨率上限-宽：", pixelWidthMax.ToString()));
		if (pixelWidthMax != num)
		{
			pixelHeightMax = Mathf.RoundToInt((float)initScreenHeight * 1f / (float)initScreenWidth * (float)pixelWidthMax);
		}
		int num2 = pixelHeightMax;
		pixelHeightMax = Convert.ToInt32(DrawInputField("高: ", pixelHeightMax.ToString()));
		if (pixelHeightMax != num2)
		{
			pixelWidthMax = Mathf.RoundToInt((float)initScreenWidth * 1f / (float)initScreenHeight * (float)pixelHeightMax);
		}
		GUILayout.EndHorizontal();
		if (GUILayout.Button("确定切换"))
		{
			SceneQualitySetting.SetPixelHeightMax(pixelHeightMax);
			SceneQualitySetting.SetResolutionQuality();
		}
		GUILayout.Space(30f);
		GUILayout.Space(20f);
		GUILayout.BeginHorizontal();
		UniversalAdditionalCameraData component = mainCamera.GetComponent<UniversalAdditionalCameraData>();
		GUILayout.Label("抗锯齿模式:" + GetAntialiasingModeText(component.antialiasing));
		if (GUILayout.Button("None"))
		{
			component.antialiasing = AntialiasingMode.None;
		}
		if (GUILayout.Button("FXAA"))
		{
			component.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
		}
		if (GUILayout.Button("SMAA"))
		{
			component.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
		}
		GUILayout.EndHorizontal();
		if (component.antialiasing == AntialiasingMode.SubpixelMorphologicalAntiAliasing)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("SMAA质量:" + GetSMAAQualityText((AntialiasingQuality)smaaQuality));
			int num3 = smaaQuality;
			smaaQuality = GUILayout.Toolbar(smaaQuality, smaaQualityString);
			if (smaaQuality != num3)
			{
				component.antialiasingQuality = (AntialiasingQuality)smaaQuality;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.Space(30f);
		GUILayout.Label("MSAA级别:" + universalRenderPipelineAsset.msaaSampleCount);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("关闭"))
		{
			universalRenderPipelineAsset.msaaSampleCount = 1;
		}
		if (GUILayout.Button("2x"))
		{
			universalRenderPipelineAsset.msaaSampleCount = 2;
		}
		if (GUILayout.Button("4x"))
		{
			universalRenderPipelineAsset.msaaSampleCount = 4;
		}
		if (GUILayout.Button("8x"))
		{
			universalRenderPipelineAsset.msaaSampleCount = 8;
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label("Grading Mode:" + universalRenderPipelineAsset.colorGradingMode);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("LDR"))
		{
			universalRenderPipelineAsset.colorGradingMode = ColorGradingMode.LowDynamicRange;
		}
		if (GUILayout.Button("HDR"))
		{
			universalRenderPipelineAsset.colorGradingMode = ColorGradingMode.HighDynamicRange;
		}
		GUILayout.EndHorizontal();
	}

	private string GetAntialiasingModeText(AntialiasingMode mode)
	{
		return mode switch
		{
			AntialiasingMode.None => "None", 
			AntialiasingMode.FastApproximateAntialiasing => "FXAA", 
			AntialiasingMode.SubpixelMorphologicalAntiAliasing => "SMAA", 
			_ => "", 
		};
	}

	private string GetSMAAQualityText(AntialiasingQuality quality)
	{
		return smaaQualityString[(int)quality];
	}
}
