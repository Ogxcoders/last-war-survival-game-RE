using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessGFXPanel : BaseGFXPanel
{
	private Camera _mainCamera;

	private Volume _ppVolume;

	public PostProcessGFXPanel()
		: base("后期")
	{
	}

	public override void Init()
	{
		_mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		_ppVolume = Object.FindObjectOfType(typeof(Volume)) as Volume;
	}

	public override void DrawGUI()
	{
		if (_mainCamera == null || _ppVolume == null)
		{
			GUILayout.Label("MainCamera或者Volum为null，请检查");
			return;
		}
		GUILayout.Space(30f);
		if (GUILayout.Button("开关后期:" + _ppVolume.enabled))
		{
			_ppVolume.enabled = !_ppVolume.enabled;
			_mainCamera.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = _ppVolume.enabled;
		}
		GUILayout.Space(30f);
		if (!_ppVolume.enabled)
		{
			return;
		}
		VolumeProfile profile = _ppVolume.profile;
		List<VolumeComponent> list = new List<VolumeComponent>();
		profile.TryGetAllSubclassOf(typeof(VolumeComponent), list);
		foreach (VolumeComponent item in list)
		{
			DoDrawPostProcessingModle(item);
		}
	}

	private void DoDrawPostProcessingModle(VolumeComponent com)
	{
		if (GUILayout.Button(com.GetType().Name + " 开关:" + com.active))
		{
			com.active = !com.active;
		}
	}
}
