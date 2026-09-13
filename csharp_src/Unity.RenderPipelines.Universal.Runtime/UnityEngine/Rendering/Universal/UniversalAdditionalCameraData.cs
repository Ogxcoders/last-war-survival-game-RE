using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace UnityEngine.Rendering.Universal;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
[ImageEffectAllowedInSceneView]
[MovedFrom("UnityEngine.Rendering.LWRP")]
public class UniversalAdditionalCameraData : MonoBehaviour, ISerializationCallbackReceiver
{
	[Tooltip("If enabled shadows will render for this camera.")]
	[FormerlySerializedAs("renderShadows")]
	[SerializeField]
	private bool m_RenderShadows = true;

	[Tooltip("If enabled depth texture will render for this camera bound as _CameraDepthTexture.")]
	[SerializeField]
	private CameraOverrideOption m_RequiresDepthTextureOption = CameraOverrideOption.UsePipelineSettings;

	[Tooltip("If enabled opaque color texture will render for this camera and bound as _CameraOpaqueTexture.")]
	[SerializeField]
	private CameraOverrideOption m_RequiresOpaqueTextureOption = CameraOverrideOption.UsePipelineSettings;

	[SerializeField]
	private CameraRenderType m_CameraType;

	[SerializeField]
	private List<Camera> m_Cameras = new List<Camera>();

	[SerializeField]
	private int m_RendererIndex = -1;

	[SerializeField]
	private LayerMask m_VolumeLayerMask = 1;

	[SerializeField]
	private Transform m_VolumeTrigger;

	[SerializeField]
	private bool m_RenderPostProcessing;

	[SerializeField]
	private AntialiasingMode m_Antialiasing;

	[SerializeField]
	private AntialiasingQuality m_AntialiasingQuality = AntialiasingQuality.High;

	[SerializeField]
	private bool m_StopNaN;

	[SerializeField]
	private bool m_Dithering;

	[SerializeField]
	private bool m_ClearDepth = true;

	[FormerlySerializedAs("requiresDepthTexture")]
	[SerializeField]
	private bool m_RequiresDepthTexture;

	[FormerlySerializedAs("requiresColorTexture")]
	[SerializeField]
	private bool m_RequiresColorTexture;

	[HideInInspector]
	[SerializeField]
	private float m_Version = 2f;

	private static UniversalAdditionalCameraData s_DefaultAdditionalCameraData;

	public bool disableRender;

	public float version => m_Version;

	internal static UniversalAdditionalCameraData defaultAdditionalCameraData
	{
		get
		{
			if (s_DefaultAdditionalCameraData == null)
			{
				s_DefaultAdditionalCameraData = new UniversalAdditionalCameraData();
			}
			return s_DefaultAdditionalCameraData;
		}
	}

	public bool renderShadows
	{
		get
		{
			return m_RenderShadows;
		}
		set
		{
			m_RenderShadows = value;
		}
	}

	public CameraOverrideOption requiresDepthOption
	{
		get
		{
			return m_RequiresDepthTextureOption;
		}
		set
		{
			m_RequiresDepthTextureOption = value;
		}
	}

	public CameraOverrideOption requiresColorOption
	{
		get
		{
			return m_RequiresOpaqueTextureOption;
		}
		set
		{
			m_RequiresOpaqueTextureOption = value;
		}
	}

	public CameraRenderType renderType
	{
		get
		{
			return m_CameraType;
		}
		set
		{
			m_CameraType = value;
		}
	}

	[Obsolete("CameraOutput has been deprecated. Use Camera.targetTexture instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public CameraOutput cameraOutput
	{
		get
		{
			base.gameObject.TryGetComponent<Camera>(out var component);
			if (component?.targetTexture == null)
			{
				return CameraOutput.Screen;
			}
			return CameraOutput.Texture;
		}
		set
		{
		}
	}

	[Obsolete("cameras property has been deprecated. Use cameraStack property instead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public List<Camera> cameras => cameraStack;

	public List<Camera> cameraStack
	{
		get
		{
			if (renderType != CameraRenderType.Base)
			{
				Camera component = base.gameObject.GetComponent<Camera>();
				Debug.LogWarning($"{component.name}: This camera is of {renderType} type. Only Base cameras can have a camera stack.");
				return null;
			}
			if (!scriptableRenderer.supportedRenderingFeatures.cameraStacking)
			{
				Camera component2 = base.gameObject.GetComponent<Camera>();
				Debug.LogWarning($"{component2.name}: This camera has a ScriptableRenderer that doesn't support camera stacking. Camera stack is null.");
				return null;
			}
			return m_Cameras;
		}
	}

	public bool clearDepth => m_ClearDepth;

	public bool requiresDepthTexture
	{
		get
		{
			if (m_RequiresDepthTextureOption == CameraOverrideOption.UsePipelineSettings)
			{
				if (!UniversalRenderPipeline.asset.supportsCameraDepthTexture)
				{
					return UniversalRenderPipeline.asset.allowSoftParticles;
				}
				return true;
			}
			return m_RequiresDepthTextureOption == CameraOverrideOption.On;
		}
		set
		{
			m_RequiresDepthTextureOption = (value ? CameraOverrideOption.On : CameraOverrideOption.Off);
		}
	}

	public bool requiresColorTexture
	{
		get
		{
			if (m_RequiresOpaqueTextureOption == CameraOverrideOption.UsePipelineSettings)
			{
				return UniversalRenderPipeline.asset.supportsCameraOpaqueTexture;
			}
			return m_RequiresOpaqueTextureOption == CameraOverrideOption.On;
		}
		set
		{
			m_RequiresOpaqueTextureOption = (value ? CameraOverrideOption.On : CameraOverrideOption.Off);
		}
	}

	public ScriptableRenderer scriptableRenderer => UniversalRenderPipeline.asset.GetRenderer(m_RendererIndex);

	public LayerMask volumeLayerMask
	{
		get
		{
			return m_VolumeLayerMask;
		}
		set
		{
			m_VolumeLayerMask = value;
		}
	}

	public Transform volumeTrigger
	{
		get
		{
			return m_VolumeTrigger;
		}
		set
		{
			m_VolumeTrigger = value;
		}
	}

	public bool renderPostProcessing
	{
		get
		{
			return m_RenderPostProcessing;
		}
		set
		{
			m_RenderPostProcessing = value;
		}
	}

	public AntialiasingMode antialiasing
	{
		get
		{
			return m_Antialiasing;
		}
		set
		{
			m_Antialiasing = value;
		}
	}

	public AntialiasingQuality antialiasingQuality
	{
		get
		{
			return m_AntialiasingQuality;
		}
		set
		{
			m_AntialiasingQuality = value;
		}
	}

	public bool stopNaN
	{
		get
		{
			return m_StopNaN;
		}
		set
		{
			m_StopNaN = value;
		}
	}

	public bool dithering
	{
		get
		{
			return m_Dithering;
		}
		set
		{
			m_Dithering = value;
		}
	}

	[Obsolete("AddCamera has been deprecated. You can add cameras to the stack by calling <c>cameraStack</c> property and modifying the camera stack list.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void AddCamera(Camera camera)
	{
		m_Cameras.Add(camera);
	}

	internal void UpdateCameraStack()
	{
		int count = m_Cameras.Count;
		m_Cameras.RemoveAll((Camera cam) => cam == null);
		int count2 = m_Cameras.Count;
		int num = count - count2;
		if (num != 0)
		{
			Debug.LogWarning(base.name + ": " + num + " camera overlay" + ((num > 1) ? "s" : "") + " no longer exists and will be removed from the camera stack.");
		}
	}

	public void SetRenderer(int index)
	{
		m_RendererIndex = index;
	}

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		if (version <= 1f)
		{
			m_RequiresDepthTextureOption = (m_RequiresDepthTexture ? CameraOverrideOption.On : CameraOverrideOption.Off);
			m_RequiresOpaqueTextureOption = (m_RequiresColorTexture ? CameraOverrideOption.On : CameraOverrideOption.Off);
		}
	}

	public void OnDrawGizmos()
	{
		string text = "Packages/com.unity.render-pipelines.universal/Editor/Gizmos/";
		string value = "";
		Color white = Color.white;
		if (m_CameraType == CameraRenderType.Base)
		{
			value = text + "Camera_Base.png";
		}
		else if (m_CameraType == CameraRenderType.Overlay)
		{
			value = text + "Camera_Overlay.png";
		}
		else if (m_CameraType == CameraRenderType.UI)
		{
			value = text + "Camera_UI.png";
		}
		if (!string.IsNullOrEmpty(value))
		{
			Gizmos.DrawIcon(base.transform.position, value, allowScaling: true, white);
		}
		if (renderPostProcessing)
		{
			Gizmos.DrawIcon(base.transform.position, text + "Camera_PostProcessing.png", allowScaling: true, white);
		}
	}
}
