using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.PerfTools;

[ExecuteAlways]
public abstract class OverdrawMonitor : MonoBehaviour
{
	private OverdrawMonitorCfg m_Config;

	protected Camera m_TargetCamera;

	protected Camera m_OverdrawCamera;

	public const RenderTextureFormat overdrawRTFormat = RenderTextureFormat.Default;

	private ComputeShader m_ComputeShader;

	protected const int k_RenderTextureSize = 256;

	private const int k_CSGroupThreadSize = 16;

	private const int k_DataSize = 256;

	private float[] m_InputData = new float[256];

	private float[] m_ResultData = new float[256];

	private ComputeBuffer m_ResultBuffer;

	private Queue<AsyncGPUReadbackRequest> m_Requests = new Queue<AsyncGPUReadbackRequest>();

	public Camera overdrawCamera => m_OverdrawCamera;

	[HideInInspector]
	public RenderTexture overdrawTexture { get; protected set; }

	public float LastFrameOverdraw { get; private set; }

	public float MaxOverdraw { get; private set; }

	public static OverdrawMonitor GetOverdrawMonitor(Camera targetCam, OverdrawMonitorCfg config)
	{
		string text = targetCam.name + "-OverdrawMonitor";
		GameObject gameObject = GameObject.Find(text);
		OverdrawMonitor overdrawMonitor = null;
		if (gameObject == null)
		{
			gameObject = new GameObject(text);
		}
		else
		{
			overdrawMonitor = gameObject.GetComponent<OverdrawMonitor>();
			if (overdrawMonitor == null || !overdrawMonitor.IsTargetCamera(targetCam))
			{
				gameObject = new GameObject(text);
			}
		}
		overdrawMonitor = gameObject.GetComponent<OverdrawMonitor>();
		if (overdrawMonitor == null)
		{
			gameObject.SetActive(value: false);
			overdrawMonitor = gameObject.AddComponent<OverdrawMonitorURPUsingRenderData>();
			overdrawMonitor.m_TargetCamera = targetCam;
			overdrawMonitor.m_Config = config;
			gameObject.SetActive(value: true);
		}
		return overdrawMonitor;
	}

	public bool IsDead()
	{
		return DoesCameraSkipRendering(m_TargetCamera);
	}

	public static bool DoesCameraSkipRendering(Camera cam)
	{
		bool flag = cam == null || !cam.enabled || !cam.gameObject.activeInHierarchy;
		if (!flag)
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = cam.GetUniversalAdditionalCameraData();
			FieldInfo field = universalAdditionalCameraData.GetType().GetField("disableRender");
			if (field != null)
			{
				flag = (bool)field.GetValue(universalAdditionalCameraData);
			}
		}
		return flag;
	}

	protected virtual void AwakeInternal()
	{
		if (Application.isPlaying)
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
		base.gameObject.hideFlags = HideFlags.DontSave;
		m_OverdrawCamera = GetComponent<Camera>();
		if (m_OverdrawCamera == null)
		{
			m_OverdrawCamera = base.gameObject.AddComponent<Camera>();
		}
		m_ComputeShader = null;
		for (int i = 0; i < m_InputData.Length; i++)
		{
			m_InputData[i] = 0f;
		}
	}

	protected virtual void OnEnableInternal()
	{
		m_Requests.Clear();
		m_OverdrawCamera.enabled = true;
	}

	protected virtual void OnDisableInternal()
	{
		m_OverdrawCamera.enabled = false;
		LastFrameOverdraw = -1f;
		if (overdrawTexture != null)
		{
			overdrawTexture.Release();
			Object.DestroyImmediate(overdrawTexture);
			overdrawTexture = null;
		}
	}

	protected virtual void LateUpdateInternal()
	{
		if (m_TargetCamera == null)
		{
			base.enabled = false;
		}
		else if ((bool)m_OverdrawCamera)
		{
			CopyCameraFrom(m_TargetCamera);
			RecreateTexture();
			SetCameraTarget();
			RenderingAsCanvasCamera();
		}
	}

	protected virtual void OnDestroyInternal()
	{
		if (m_OverdrawCamera != null)
		{
			m_OverdrawCamera.targetTexture = null;
		}
		m_ComputeShader = null;
		if (m_ResultBuffer != null)
		{
			m_ResultBuffer.Release();
			m_ResultBuffer = null;
		}
		if (overdrawTexture != null)
		{
			overdrawTexture.Release();
			Object.DestroyImmediate(overdrawTexture);
			overdrawTexture = null;
		}
	}

	public void Awake()
	{
		AwakeInternal();
	}

	public void OnEnable()
	{
		OnEnableInternal();
	}

	public void OnDisable()
	{
		OnDisableInternal();
	}

	public void LateUpdate()
	{
		LateUpdateInternal();
	}

	public void OnDestroy()
	{
		OnDestroyInternal();
	}

	protected virtual void CopyCameraFrom(Camera target)
	{
		if (!(target == null) && !(m_OverdrawCamera == target))
		{
			if (PerfToolsRuntimeCfg.Instance.overdrawMonitorConfig.syncCameraWithCopyFrom)
			{
				m_OverdrawCamera.CopyFrom(target);
			}
			else
			{
				m_OverdrawCamera.transform.localScale = m_TargetCamera.transform.localScale;
				m_OverdrawCamera.transform.position = m_TargetCamera.transform.position;
				m_OverdrawCamera.transform.rotation = m_TargetCamera.transform.rotation;
				overdrawCamera.cullingMask = m_TargetCamera.cullingMask;
				overdrawCamera.fieldOfView = m_TargetCamera.fieldOfView;
				overdrawCamera.orthographic = m_TargetCamera.orthographic;
			}
			m_OverdrawCamera.clearFlags = CameraClearFlags.Color;
			m_OverdrawCamera.orthographicSize = m_TargetCamera.orthographicSize;
			m_OverdrawCamera.nearClipPlane = m_TargetCamera.nearClipPlane;
			m_OverdrawCamera.farClipPlane = m_TargetCamera.farClipPlane;
			m_OverdrawCamera.backgroundColor = Color.black;
		}
	}

	protected virtual void RecreateTexture()
	{
	}

	protected virtual void SetCameraTarget()
	{
	}

	private void RecreateComputeBuffer()
	{
		if (m_ResultBuffer == null)
		{
			m_ResultBuffer = new ComputeBuffer(m_ResultData.Length, 4);
		}
	}

	public void Dispose()
	{
		if (this != null)
		{
			Object.DestroyImmediate(base.gameObject);
		}
	}

	private void RenderingAsCanvasCamera()
	{
		if (OverdrawMonitorManager.Instance.TryGetUIRootCanvas(m_TargetCamera, out var canvas) && !(canvas == null))
		{
			RenderMode renderMode = canvas.renderMode;
			float scaleFactor = canvas.scaleFactor;
			canvas.renderMode = RenderMode.WorldSpace;
			m_OverdrawCamera.Render();
			m_OverdrawCamera.enabled = false;
			canvas.renderMode = renderMode;
			canvas.scaleFactor = scaleFactor;
		}
	}

	private void CheckReqQueue()
	{
		while (m_Requests.Count > 0)
		{
			AsyncGPUReadbackRequest asyncGPUReadbackRequest = m_Requests.Peek();
			if (asyncGPUReadbackRequest.hasError)
			{
				Debug.LogError("overdraw result GPU readback error detected.");
				m_Requests.Dequeue();
				continue;
			}
			if (asyncGPUReadbackRequest.done)
			{
				asyncGPUReadbackRequest.GetData<float>().CopyTo(m_ResultData);
				CalculateOverdraw(m_ResultData);
				m_Requests.Dequeue();
				continue;
			}
			break;
		}
	}

	private void CalculateOverdraw(float[] rstData)
	{
		double num = 0.0;
		for (int i = 0; i < rstData.Length; i++)
		{
			num += (double)rstData[i];
		}
		LastFrameOverdraw = (float)(num / 65536.0);
		if (LastFrameOverdraw > MaxOverdraw)
		{
			MaxOverdraw = LastFrameOverdraw;
		}
	}

	protected void CalculateOverdrawFromRT()
	{
		int num = ((m_Config == null) ? 8 : m_Config.maxRequestQueueLength);
		if (m_Requests.Count < num && overdrawTexture != null && m_ComputeShader != null)
		{
			int kernelIndex = m_ComputeShader.FindKernel("CSMain");
			RecreateComputeBuffer();
			int num2 = 16;
			int threadGroupsY = 16;
			m_ResultBuffer.SetData(m_InputData);
			m_ComputeShader.SetTexture(kernelIndex, "Overdraw", overdrawTexture);
			m_ComputeShader.SetBuffer(kernelIndex, "Output", m_ResultBuffer);
			m_ComputeShader.SetInt("GroupX", num2);
			m_ComputeShader.Dispatch(kernelIndex, num2, threadGroupsY, 1);
			if (!OverdrawMonitorManager.ForceGetODResultInSyncMode && Application.isPlaying && SystemInfo.supportsAsyncGPUReadback)
			{
				m_Requests.Enqueue(AsyncGPUReadback.Request(m_ResultBuffer));
			}
			else
			{
				m_ResultBuffer.GetData(m_ResultData);
				CalculateOverdraw(m_ResultData);
			}
		}
		else
		{
			Debug.Log("overdraw 回读队列满, Too many requests.");
		}
		if (Application.isPlaying && SystemInfo.supportsAsyncGPUReadback)
		{
			CheckReqQueue();
		}
	}

	public void StartMeasurement()
	{
		base.enabled = true;
		m_OverdrawCamera.enabled = true;
	}

	public void Stop()
	{
		base.enabled = false;
		m_OverdrawCamera.enabled = false;
	}

	public void ResetExtreemes()
	{
		MaxOverdraw = 0f;
	}

	public void Restart()
	{
		Stop();
		StartMeasurement();
		ResetExtreemes();
	}

	public bool IsTargetCamera(Camera cam)
	{
		return m_TargetCamera == cam;
	}

	public bool IsSelfCamera(Camera cam)
	{
		return m_OverdrawCamera == cam;
	}

	public string GetTargetCameraName()
	{
		if (!(m_TargetCamera != null))
		{
			return "Unknow";
		}
		return m_TargetCamera.name;
	}

	public virtual void SetReplacementTag(string tag)
	{
	}

	public virtual void ResetReplacementTag()
	{
	}

	public virtual string GetCurReplacementTag()
	{
		return "";
	}
}
