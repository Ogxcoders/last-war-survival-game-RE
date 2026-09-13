using System;

namespace UnityEngine.Rendering;

public class DynamicResolutionHandler
{
	private bool m_Enabled;

	private float m_MinScreenFraction = 1f;

	private float m_MaxScreenFraction = 1f;

	private float m_CurrentFraction = 1f;

	private float m_PrevFraction = -1f;

	private bool m_ForcingRes;

	private bool m_CurrentCameraRequest = true;

	private bool m_ForceSoftwareFallback;

	private float m_PrevHWScaleWidth = 1f;

	private float m_PrevHWScaleHeight = 1f;

	private Vector2Int m_LastScaledSize = new Vector2Int(0, 0);

	private DynamicResScalePolicyType m_ScalerType = DynamicResScalePolicyType.ReturnsMinMaxLerpFactor;

	private Vector2Int cachedOriginalSize;

	private DynamicResolutionType type;

	private PerformDynamicRes m_DynamicResMethod;

	private static DynamicResolutionHandler s_Instance = new DynamicResolutionHandler();

	public DynamicResUpscaleFilter filter { get; set; }

	public static DynamicResolutionHandler instance => s_Instance;

	private DynamicResolutionHandler()
	{
		m_DynamicResMethod = DefaultDynamicResMethod;
		filter = DynamicResUpscaleFilter.Bilinear;
	}

	private static float DefaultDynamicResMethod()
	{
		return 1f;
	}

	private void ProcessSettings(GlobalDynamicResolutionSettings settings)
	{
		m_Enabled = settings.enabled;
		if (!m_Enabled)
		{
			m_CurrentFraction = 1f;
			return;
		}
		type = settings.dynResType;
		float minScreenFraction = Mathf.Clamp(settings.minPercentage / 100f, 0.1f, 1f);
		m_MinScreenFraction = minScreenFraction;
		float maxScreenFraction = Mathf.Clamp(settings.maxPercentage / 100f, m_MinScreenFraction, 3f);
		m_MaxScreenFraction = maxScreenFraction;
		filter = settings.upsampleFilter;
		m_ForcingRes = settings.forceResolution;
		if (m_ForcingRes)
		{
			float currentFraction = Mathf.Clamp(settings.forcedPercentage / 100f, 0.1f, 1.5f);
			m_CurrentFraction = currentFraction;
		}
	}

	public static void SetDynamicResScaler(PerformDynamicRes scaler, DynamicResScalePolicyType scalerType = DynamicResScalePolicyType.ReturnsMinMaxLerpFactor)
	{
		s_Instance.m_ScalerType = scalerType;
		s_Instance.m_DynamicResMethod = scaler;
	}

	public void SetCurrentCameraRequest(bool cameraRequest)
	{
		m_CurrentCameraRequest = cameraRequest;
	}

	public void Update(GlobalDynamicResolutionSettings settings, Action OnResolutionChange = null)
	{
		ProcessSettings(settings);
		if (!m_Enabled)
		{
			return;
		}
		if (!m_ForcingRes)
		{
			if (m_ScalerType == DynamicResScalePolicyType.ReturnsMinMaxLerpFactor)
			{
				float t = Mathf.Clamp(m_DynamicResMethod(), 0f, 1f);
				m_CurrentFraction = Mathf.Lerp(m_MinScreenFraction, m_MaxScreenFraction, t);
			}
			else if (m_ScalerType == DynamicResScalePolicyType.ReturnsPercentage)
			{
				float num = Mathf.Max(m_DynamicResMethod(), 5f);
				m_CurrentFraction = Mathf.Clamp(num / 100f, m_MinScreenFraction, m_MaxScreenFraction);
			}
		}
		if (m_CurrentFraction != m_PrevFraction)
		{
			m_PrevFraction = m_CurrentFraction;
			if (!m_ForceSoftwareFallback && type == DynamicResolutionType.Hardware)
			{
				ScalableBufferManager.ResizeBuffers(m_CurrentFraction, m_CurrentFraction);
			}
			OnResolutionChange();
		}
		else if (!m_ForceSoftwareFallback && type == DynamicResolutionType.Hardware && (ScalableBufferManager.widthScaleFactor != m_PrevHWScaleWidth || ScalableBufferManager.heightScaleFactor != m_PrevHWScaleHeight))
		{
			OnResolutionChange();
		}
		m_PrevHWScaleWidth = ScalableBufferManager.widthScaleFactor;
		m_PrevHWScaleHeight = ScalableBufferManager.heightScaleFactor;
	}

	public bool SoftwareDynamicResIsEnabled()
	{
		if (m_CurrentCameraRequest && m_Enabled && m_CurrentFraction != 1f)
		{
			if (!m_ForceSoftwareFallback)
			{
				return type == DynamicResolutionType.Software;
			}
			return true;
		}
		return false;
	}

	public bool HardwareDynamicResIsEnabled()
	{
		if (!m_ForceSoftwareFallback && m_CurrentCameraRequest && m_Enabled)
		{
			return type == DynamicResolutionType.Hardware;
		}
		return false;
	}

	public bool RequestsHardwareDynamicResolution()
	{
		if (m_ForceSoftwareFallback)
		{
			return false;
		}
		return type == DynamicResolutionType.Hardware;
	}

	public bool DynamicResolutionEnabled()
	{
		if (m_CurrentCameraRequest && m_Enabled)
		{
			return m_CurrentFraction != 1f;
		}
		return false;
	}

	public void ForceSoftwareFallback()
	{
		m_ForceSoftwareFallback = true;
	}

	public Vector2Int GetScaledSize(Vector2Int size)
	{
		cachedOriginalSize = size;
		if (!m_Enabled || !m_CurrentCameraRequest)
		{
			return size;
		}
		float num = m_CurrentFraction;
		float num2 = m_CurrentFraction;
		if (!m_ForceSoftwareFallback && type == DynamicResolutionType.Hardware)
		{
			num = ScalableBufferManager.widthScaleFactor;
			num2 = ScalableBufferManager.heightScaleFactor;
		}
		Vector2Int vector2Int = new Vector2Int(Mathf.CeilToInt((float)size.x * num), Mathf.CeilToInt((float)size.y * num2));
		if (m_ForceSoftwareFallback || type != DynamicResolutionType.Hardware)
		{
			vector2Int.x += 1 & vector2Int.x;
			vector2Int.y += 1 & vector2Int.y;
		}
		m_LastScaledSize = vector2Int;
		return vector2Int;
	}

	public float GetCurrentScale()
	{
		if (!m_Enabled || !m_CurrentCameraRequest)
		{
			return 1f;
		}
		return m_CurrentFraction;
	}

	public Vector2Int GetLastScaledSize()
	{
		return m_LastScaledSize;
	}
}
