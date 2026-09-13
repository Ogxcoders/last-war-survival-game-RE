using System.Collections.Generic;
using BitBenderGames;
using UnityEngine;

public class CameraGFXPanel : BaseGFXPanel
{
	private Camera _mainCamera;

	private string camZoomInit;

	private string camZoomMin;

	private string camZoomMax;

	private string camZoom;

	private string camZoomBuild;

	private string camZoomFocusRotation;

	private string camZoomFarmPlant;

	private string camZoomFarmPlantRotation;

	private string camZoomFormation;

	private string camZoomFormationRotation;

	private List<MobileTouchCamera.ZoomParam> zoomParams;

	private bool camToggle;

	public CameraGFXPanel()
		: base("摄像机")
	{
	}

	public override void Init()
	{
		_mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		camToggle = false;
		MobileTouchCamera component = _mainCamera.GetComponent<MobileTouchCamera>();
		camZoomInit = component.CamZoomInit.ToString();
		camZoomMin = component.CamZoomMin.ToString();
		camZoomMax = component.CamZoomMax.ToString();
		zoomParams = component.GetZoomParams();
		camZoomBuild = component.CamZoomBuild.ToString();
		camZoomFocusRotation = component.CamZoomFocusRotation.ToString();
		camZoomFarmPlant = component.CamZoomFarmPlant.ToString();
		camZoomFarmPlantRotation = component.CamZoomFarmPlantRotation.ToString();
		camZoomFormation = component.CamZoomFormation.ToString();
		camZoomFormationRotation = component.CamZoomFocusFormationRotation.ToString();
	}

	public override void DrawGUI()
	{
		if (_mainCamera == null)
		{
			GUILayout.Label("MainCamera为null，请检查");
			return;
		}
		camToggle = GUILayout.Toggle(camToggle, "视锥体");
		if (camToggle)
		{
			_mainCamera.farClipPlane = DrawSlider("FarClip", _mainCamera.farClipPlane, 2f, 600f);
			_mainCamera.nearClipPlane = DrawSlider("NearClip", _mainCamera.nearClipPlane, 0f, 30f);
			_mainCamera.fieldOfView = DrawSlider("FOV", _mainCamera.fieldOfView, 30f, 80f);
		}
		GUILayout.Space(30f);
		DrawZoomParams();
	}

	private void DrawZoomParams()
	{
		MobileTouchCamera component = _mainCamera.GetComponent<MobileTouchCamera>();
		GUILayout.Label("镜头Zoom参数：");
		float x = component.transform.eulerAngles.x;
		GUILayout.BeginHorizontal();
		GUILayout.Label("当前高：" + component.CamZoom);
		GUILayout.Label("俯仰角：" + x);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		camZoomInit = DrawInputField("初始高：", camZoomInit);
		camZoomMin = DrawInputField("最小高: ", camZoomMin);
		camZoomMax = DrawInputField("最大高: ", camZoomMax);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		camZoomBuild = DrawInputField("建筑-高", camZoomBuild);
		camZoomFocusRotation = DrawInputField("建筑-俯仰角", camZoomFocusRotation);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		camZoomFarmPlant = DrawInputField("农场-高", camZoomFarmPlant);
		camZoomFarmPlantRotation = DrawInputField("农场-俯仰角", camZoomFarmPlantRotation);
		camZoomFormation = DrawInputField("出征-高", camZoomFormation);
		camZoomFormationRotation = DrawInputField("出征-俯仰角", camZoomFormationRotation);
		GUILayout.EndHorizontal();
		GUILayout.BeginVertical();
		for (int i = 0; i < zoomParams.Count; i++)
		{
			MobileTouchCamera.ZoomParam zoomParam = zoomParams[i];
			GUILayout.BeginHorizontal();
			zoomParam.posY = float.Parse(DrawInputField($"[{i + 1}] 高：", zoomParam.posY.ToString()));
			zoomParam.offsetZ = float.Parse(DrawInputField("向后偏移：", zoomParam.offsetZ.ToString()));
			zoomParam.sensitivity = float.Parse(DrawInputField("灵敏度：", zoomParam.sensitivity.ToString()));
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
		if (GUILayout.Button("确定"))
		{
			component.CamZoomInit = float.Parse(camZoomInit);
			component.CamZoomMin = float.Parse(camZoomMin);
			component.CamZoomMax = float.Parse(camZoomMax);
			component.SetZoomParams(zoomParams);
			component.CamZoom = float.Parse(camZoomInit);
			component.CamZoomBuild = float.Parse(camZoomBuild);
			component.CamZoomFocusRotation = float.Parse(camZoomFocusRotation);
			component.CamZoomFarmPlant = float.Parse(camZoomFarmPlant);
			component.CamZoomFarmPlantRotation = float.Parse(camZoomFarmPlantRotation);
			component.CamZoomFormation = float.Parse(camZoomFormation);
			component.CamZoomFocusFormationRotation = float.Parse(camZoomFormationRotation);
		}
	}
}
