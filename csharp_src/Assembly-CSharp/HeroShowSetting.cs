using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class HeroShowSetting : MonoBehaviour
{
	private int level;

	private float shadowDistance;

	public bool IsAddGrabCamera;

	private string[] unitySHParamNames = new string[7] { "_Boyan_SHAr", "_Boyan_SHAg", "_Boyan_SHAb", "_Boyan_SHBr", "_Boyan_SHBg", "_Boyan_SHBb", "_Boyan_SHC" };

	private void Awake()
	{
		Setting();
	}

	private void Start()
	{
	}

	private void Setting()
	{
		Vector4 value = new Vector4(-0.002500065f, 0.1566966f, 0.2403159f, 0.2535352f);
		Vector4 value2 = new Vector4(-0.0007448478f, 0.1495781f, 0.2342949f, 0.259032f);
		Vector4 value3 = new Vector4(-0.0006305838f, 0.1381056f, 0.2189651f, 0.2619695f);
		Vector4 value4 = new Vector4(0.0009808665f, 0.1244143f, 0.1215409f, 0.007772579f);
		Vector4 value5 = new Vector4(0.002275236f, 0.1206006f, 0.1166272f, 0.01075001f);
		Vector4 value6 = new Vector4(0.003193089f, 0.1160733f, 0.1080668f, 0.01155533f);
		Vector4 value7 = new Vector4(0.0118142f, 0.01242505f, 0.01204208f, 1f);
		Shader.SetGlobalVector(unitySHParamNames[0], value3);
		Shader.SetGlobalVector(unitySHParamNames[1], value2);
		Shader.SetGlobalVector(unitySHParamNames[2], value);
		Shader.SetGlobalVector(unitySHParamNames[3], value6);
		Shader.SetGlobalVector(unitySHParamNames[4], value5);
		Shader.SetGlobalVector(unitySHParamNames[5], value4);
		Shader.SetGlobalVector(unitySHParamNames[6], value7);
		UniversalRenderPipelineAsset universalRenderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
		shadowDistance = universalRenderPipelineAsset.shadowDistance;
		universalRenderPipelineAsset.shadowDistance = 8f;
	}

	private void OnDestroy()
	{
		(QualitySettings.renderPipeline as UniversalRenderPipelineAsset).shadowDistance = shadowDistance;
	}

	private void Update()
	{
		if (!IsAddGrabCamera)
		{
			return;
		}
		GameObject gameObject = null;
		for (int i = 0; i < 7; i++)
		{
			gameObject = GameObject.Find("Camp_" + i);
			if (gameObject != null)
			{
				break;
			}
		}
		if (gameObject != null)
		{
			Camera componentInChildren = gameObject.GetComponentInChildren<Camera>();
			UniversalAdditionalCameraData component = componentInChildren.gameObject.GetComponent<UniversalAdditionalCameraData>();
			componentInChildren.cullingMask = 262144;
			if (componentInChildren != null && componentInChildren.transform.Find("GrabCamera") == null)
			{
				GameObject obj = new GameObject("GrabCamera");
				Camera camera = obj.AddComponent<Camera>();
				UniversalAdditionalCameraData universalAdditionalCameraData = obj.AddComponent<UniversalAdditionalCameraData>();
				obj.transform.SetParent(componentInChildren.transform);
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localRotation = Quaternion.identity;
				obj.transform.localScale = Vector3.one;
				universalAdditionalCameraData.renderType = CameraRenderType.Overlay;
				camera.fieldOfView = componentInChildren.fieldOfView;
				camera.cullingMask = 32768;
				camera.usePhysicalProperties = true;
				camera.focalLength = 21f;
				camera.sensorSize = new Vector2(35.99993f, 23.99995f);
				camera.nearClipPlane = 0.01f;
				camera.farClipPlane = 6000f;
				camera.gateFit = componentInChildren.gateFit;
				component.cameraStack.Add(camera);
			}
		}
		else
		{
			Debug.LogError("没有挂载场景");
		}
		IsAddGrabCamera = false;
	}
}
