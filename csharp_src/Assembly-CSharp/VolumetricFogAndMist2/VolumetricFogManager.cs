using UnityEngine;

namespace VolumetricFogAndMist2;

[ExecuteInEditMode]
public class VolumetricFogManager : MonoBehaviour, IVolumetricFogManager
{
	private static PointLightManager _pointLightManager;

	private static FogVoidManager _fogVoidManager;

	private static VolumetricFogManager _instance;

	public Camera mainCamera;

	public Light sun;

	[Tooltip("Layer to be used for fog elements. This layer will be excluded from the depth pre-pass.")]
	public int fogLayer = 1;

	[Tooltip("Flip depth texture. Use only as a workaround to a bug in URP if the depth shows inverted in GameView. Alternatively you can enable MSAA or HDR instead of using this option.")]
	public bool flipDepthTexture;

	[Tooltip("Optionally specify which transparent layers must be included in the depth prepass. Use only to avoid fog clipping with certain transparent objects.")]
	public LayerMask includeTransparent;

	private const string SKW_FLIP_DEPTH_TEXTURE = "VF2_FLIP_DEPTH_TEXTURE";

	public string managerName => "Volumetric Fog Manager";

	public static VolumetricFogManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Tools.CheckMainManager();
			}
			return _instance;
		}
	}

	public static PointLightManager pointLightManager
	{
		get
		{
			Tools.CheckManager(ref _pointLightManager);
			return _pointLightManager;
		}
	}

	public static FogVoidManager fogVoidManager
	{
		get
		{
			Tools.CheckManager(ref _fogVoidManager);
			return _fogVoidManager;
		}
	}

	private void OnEnable()
	{
		SetupLights();
		SetupDepthPrePass();
		Tools.CheckManager(ref _pointLightManager);
		Tools.CheckManager(ref _fogVoidManager);
	}

	private void OnValidate()
	{
		SetupDepthPrePass();
	}

	private void SetupCamera()
	{
		Tools.CheckCamera(ref mainCamera);
		if (mainCamera != null)
		{
			mainCamera.depthTextureMode |= DepthTextureMode.Depth;
		}
	}

	private void SetupLights()
	{
		Light[] array = Object.FindObjectsOfType<Light>();
		foreach (Light light in array)
		{
			if (light.type == LightType.Directional)
			{
				if (sun == null)
				{
					sun = light;
				}
				break;
			}
		}
	}

	private void SetupDepthPrePass()
	{
		Shader.SetGlobalInt("VF2_FLIP_DEPTH_TEXTURE", flipDepthTexture ? 1 : 0);
		DepthRenderPrePassFeature.DepthRenderPass.layerMask = (int)includeTransparent & ~(1 << fogLayer);
	}

	public static GameObject CreateFogVolume(string name)
	{
		GameObject obj = Object.Instantiate(Resources.Load<GameObject>("Prefabs/FogVolume2D"));
		obj.name = name;
		return obj;
	}

	public static GameObject CreateFogVoid(string name)
	{
		return new GameObject(name, typeof(FogVoid));
	}
}
