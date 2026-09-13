using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace FibMatrix.Rendering;

[ExecuteAlways]
public class VerticalDepthData : MonoBehaviour
{
	public List<GameObject> targets;

	public bool overrideTopY;

	public float topY;

	public bool overrideBottomY;

	public float bottomY = -1f;

	public int textureMaxResolution = 256;

	[Tooltip("解决贴图双线性采样时边缘问题")]
	public int textureExpand = 4;

	public int depthBufferBit = 16;

	public bool autoDisable = true;

	private Camera verticalDepthCamera;

	private RenderTexture rt;

	public string VerticalDepthTextureName = "_VerticalDepthTexture";

	public string VerticalDepthTextureParameter = "_VerticalDepthTextureParameter";

	public string VerticalDepthParameter = "_VerticalDepthParameter";

	private RenderPipelineCallback renderPipelineCallback;

	private Bounds bounds;

	private const string TextureSaveBoxGroupName = "保存贴图";

	public Texture2D saved;

	private void OnEnable()
	{
		if (verticalDepthCamera == null)
		{
			verticalDepthCamera = GetComponentInChildren<Camera>(includeInactive: true);
		}
		RenderPipelineCallbackUtilities.GetOrCreateRenderPipelineCallback(base.gameObject, ref renderPipelineCallback);
		RefreshCallback.Callback = (Action)Delegate.Remove(RefreshCallback.Callback, new Action(UpdateRenderingSetting));
		RefreshCallback.Callback = (Action)Delegate.Combine(RefreshCallback.Callback, new Action(UpdateRenderingSetting));
		renderPipelineCallback.ActionEndCameraRendering += ActionPostRender;
		UpdateRenderingSetting();
	}

	private void OnDisable()
	{
		renderPipelineCallback.ActionEndCameraRendering -= ActionPostRender;
	}

	private void ActionPostRender(ScriptableRenderContext context, Camera camera)
	{
		if (autoDisable && camera == verticalDepthCamera)
		{
			verticalDepthCamera.enabled = false;
		}
	}

	private void OnDestroy()
	{
		RefreshCallback.Callback = (Action)Delegate.Remove(RefreshCallback.Callback, new Action(UpdateRenderingSetting));
		RefreshCallback.Invoke(base.gameObject);
		Release();
	}

	private void Release()
	{
		if (rt != null)
		{
			verticalDepthCamera.targetTexture = null;
			RenderTexture.ReleaseTemporary(rt);
			rt = null;
		}
	}

	private void Update()
	{
		if (base.transform.hasChanged)
		{
			UpdateRenderingSetting();
			StartCoroutine(CoroutineWaitForEndOfFrame());
			Debug.Log("transform changed");
		}
	}

	private IEnumerator CoroutineWaitForEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		base.transform.hasChanged = false;
	}

	private void UpdateRenderingSetting()
	{
		bounds = BoundsUtilities.CalculateBounds(targets);
		float num = (float)(textureExpand / textureMaxResolution) * Mathf.Max(bounds.extents.x, bounds.extents.z);
		bounds.Expand(new Vector3(num, 0f, num));
		if (!((double)bounds.extents.z < 0.0001))
		{
			float num2 = bounds.extents.x / bounds.extents.z;
			Release();
			Vector2Int vector2Int = Vector2Int.Max(rhs: (!(num2 >= 1f)) ? new Vector2Int((int)((float)textureMaxResolution * num2) >> 2 << 2, textureMaxResolution) : new Vector2Int(textureMaxResolution, (int)((float)textureMaxResolution / num2) >> 2 << 2), lhs: Vector2Int.one * 4);
			rt = RenderTexture.GetTemporary(vector2Int.x, vector2Int.y, depthBufferBit, RenderTextureFormat.R8);
			rt.filterMode = FilterMode.Bilinear;
			rt.name = "VerticalDepthTexture";
			float num3 = (overrideTopY ? topY : (bounds.center.y + bounds.extents.y));
			float num4 = (overrideBottomY ? bottomY : (bounds.center.y - bounds.extents.y));
			verticalDepthCamera.enabled = true;
			verticalDepthCamera.targetTexture = rt;
			verticalDepthCamera.clearFlags = CameraClearFlags.Color;
			verticalDepthCamera.backgroundColor = Color.white;
			verticalDepthCamera.orthographic = true;
			verticalDepthCamera.aspect = num2;
			verticalDepthCamera.orthographicSize = bounds.extents.z;
			verticalDepthCamera.nearClipPlane = 0f - num3;
			verticalDepthCamera.farClipPlane = 0f - num4;
			verticalDepthCamera.transform.position = new Vector3(bounds.center.x, 0f, bounds.center.z);
			verticalDepthCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
			Vector2 vector = new Vector2(verticalDepthCamera.transform.position.x, verticalDepthCamera.transform.position.z);
			Vector2 vector2 = new Vector2(verticalDepthCamera.orthographicSize * num2, verticalDepthCamera.orthographicSize);
			Vector4 value = new Vector4(vector.x - vector2.x, vector.y - vector2.y, vector2.x * 2f, vector2.y * 2f);
			Vector4 value2 = new Vector4(num3, num4, num3, num4);
			if (!string.IsNullOrEmpty(VerticalDepthTextureName) && !string.IsNullOrEmpty(VerticalDepthTextureParameter) && !string.IsNullOrEmpty(VerticalDepthParameter))
			{
				Shader.SetGlobalVector(VerticalDepthTextureParameter, value);
				Shader.SetGlobalVector(VerticalDepthParameter, value2);
				Shader.SetGlobalTexture(VerticalDepthTextureName, rt);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Vector3 center = bounds.center;
		Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
		Gizmos.DrawCube(center, bounds.size);
		Gizmos.color = new Color(1f, 0f, 1f, 0.25f);
		Gizmos.DrawWireCube(center, bounds.size);
	}

	private void Save()
	{
	}
}
