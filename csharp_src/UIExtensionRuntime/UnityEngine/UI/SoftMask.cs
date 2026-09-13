using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UnityEngine.UI;

public class SoftMask : Mask, IMeshModifier
{
	public enum DownSamplingRate
	{
		None = 0,
		x1 = 1,
		x2 = 2,
		x4 = 4,
		x8 = 8
	}

	private static readonly List<SoftMask>[] s_TmpSoftMasks = new List<SoftMask>[4]
	{
		new List<SoftMask>(),
		new List<SoftMask>(),
		new List<SoftMask>(),
		new List<SoftMask>()
	};

	private static readonly Color[] s_ClearColors = new Color[4]
	{
		new Color(0f, 0f, 0f, 0f),
		new Color(1f, 0f, 0f, 0f),
		new Color(1f, 1f, 0f, 0f),
		new Color(1f, 1f, 1f, 0f)
	};

	private static bool s_UVStartsAtTop;

	private static bool s_IsMetal;

	private static Shader s_SoftMaskShader;

	private static Texture2D s_ReadTexture;

	private static readonly List<SoftMask> s_ActiveSoftMasks = new List<SoftMask>();

	private static readonly List<SoftMask> s_TempRelatables = new List<SoftMask>();

	private static readonly Dictionary<int, Matrix4x4> s_PreviousViewProjectionMatrices = new Dictionary<int, Matrix4x4>();

	private static readonly Dictionary<int, Matrix4x4> s_NowViewProjectionMatrices = new Dictionary<int, Matrix4x4>();

	private static int s_StencilCompId;

	private static int s_ColorMaskId;

	private static int s_MainTexId;

	private static int s_SoftnessId;

	private static int s_Alpha;

	private static int s_PreviousWidth;

	private static int s_PreviousHeight;

	private MaterialPropertyBlock _mpb;

	private CommandBuffer _cb;

	private RenderTexture _softMaskBuffer;

	private int _stencilDepth;

	private Mesh _mesh;

	private SoftMask _parent;

	internal readonly List<SoftMask> _children = new List<SoftMask>();

	private bool _hasChanged;

	private bool _hasStencilStateChanged;

	[FormerlySerializedAs("m_DesamplingRate")]
	[SerializeField]
	[Tooltip("The down sampling rate for soft mask buffer.")]
	private DownSamplingRate m_DownSamplingRate = DownSamplingRate.x1;

	[SerializeField]
	[Range(0f, 1f)]
	[Tooltip("The value used by the soft mask to select the area of influence defined over the soft mask's graphic.")]
	private float m_Softness = 1f;

	[SerializeField]
	[Range(0f, 1f)]
	[Tooltip("The transparency of the whole masked graphic.")]
	private float m_Alpha = 1f;

	[Header("Advanced Options")]
	[SerializeField]
	[Tooltip("Should the soft mask ignore parent soft masks?")]
	private bool m_IgnoreParent;

	[SerializeField]
	[Tooltip("Is the soft mask a part of parent soft mask?")]
	private bool m_PartOfParent;

	[SerializeField]
	[Tooltip("Self graphic will not be drawn to soft mask buffer.")]
	private bool m_IgnoreSelfGraphic;

	[SerializeField]
	[Tooltip("Self graphic will not be written to stencil buffer.")]
	private bool m_IgnoreSelfStencil;

	[SerializeField]
	private Material m_material;

	[SerializeField]
	private Material m_materialable;

	[SerializeField]
	private Material m_materialableGray;

	[SerializeField]
	private Material m_materialableText;

	public DownSamplingRate downSamplingRate
	{
		get
		{
			return m_DownSamplingRate;
		}
		set
		{
			if (m_DownSamplingRate != value)
			{
				m_DownSamplingRate = value;
				hasChanged = true;
			}
		}
	}

	public float softness
	{
		get
		{
			return m_Softness;
		}
		set
		{
			value = Mathf.Clamp01(value);
			if (!Mathf.Approximately(m_Softness, value))
			{
				m_Softness = value;
				hasChanged = true;
			}
		}
	}

	public float alpha
	{
		get
		{
			return m_Alpha;
		}
		set
		{
			value = Mathf.Clamp01(value);
			if (!Mathf.Approximately(m_Alpha, value))
			{
				m_Alpha = value;
				hasChanged = true;
			}
		}
	}

	public bool ignoreParent
	{
		get
		{
			return m_IgnoreParent;
		}
		set
		{
			if (m_IgnoreParent != value)
			{
				m_IgnoreParent = value;
				hasChanged = true;
				OnTransformParentChanged();
			}
		}
	}

	public bool partOfParent
	{
		get
		{
			return m_PartOfParent;
		}
		set
		{
			if (m_PartOfParent != value)
			{
				m_PartOfParent = value;
				hasChanged = true;
				OnTransformParentChanged();
			}
		}
	}

	public RenderTexture softMaskBuffer
	{
		get
		{
			if ((bool)_parent)
			{
				ReleaseRt(ref _softMaskBuffer);
				return _parent.softMaskBuffer;
			}
			GetDownSamplingSize(m_DownSamplingRate, out var w, out var h);
			if ((bool)_softMaskBuffer && (_softMaskBuffer.width != w || _softMaskBuffer.height != h))
			{
				ReleaseRt(ref _softMaskBuffer);
			}
			if (!_softMaskBuffer)
			{
				_softMaskBuffer = RenderTexture.GetTemporary(w, h, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1, RenderTextureMemoryless.Depth);
				hasChanged = true;
				_hasStencilStateChanged = true;
			}
			return _softMaskBuffer;
		}
	}

	public bool hasChanged
	{
		get
		{
			if (!_parent)
			{
				return _hasChanged;
			}
			return _parent.hasChanged;
		}
		private set
		{
			if ((bool)_parent)
			{
				_parent.hasChanged = value;
			}
			_hasChanged = value;
		}
	}

	public SoftMask parent => _parent;

	public bool ignoreSelfGraphic
	{
		get
		{
			return m_IgnoreSelfGraphic;
		}
		set
		{
			if (m_IgnoreSelfGraphic != value)
			{
				m_IgnoreSelfGraphic = value;
				hasChanged = true;
				base.graphic.SetVerticesDirtyEx();
			}
		}
	}

	public bool ignoreSelfStencil
	{
		get
		{
			return m_IgnoreSelfStencil;
		}
		set
		{
			if (m_IgnoreSelfStencil != value)
			{
				m_IgnoreSelfStencil = value;
				hasChanged = true;
				base.graphic.SetVerticesDirtyEx();
				base.graphic.SetMaterialDirtyEx();
			}
		}
	}

	private Material material => m_material;

	public Material materialable => m_materialable;

	public Material materialableGray => m_materialableGray;

	public Material materialableText => m_materialableText;

	private Mesh mesh
	{
		get
		{
			if (!_mesh)
			{
				Mesh obj = new Mesh
				{
					hideFlags = HideFlags.HideAndDontSave
				};
				Mesh result = obj;
				_mesh = obj;
				return result;
			}
			return _mesh;
		}
	}

	public override Material GetModifiedMaterial(Material baseMaterial)
	{
		hasChanged = true;
		if (ignoreSelfStencil)
		{
			return baseMaterial;
		}
		Material modifiedMaterial = base.GetModifiedMaterial(baseMaterial);
		if (m_IgnoreParent && modifiedMaterial != baseMaterial)
		{
			modifiedMaterial.SetInt(s_StencilCompId, 8);
		}
		return modifiedMaterial;
	}

	void IMeshModifier.ModifyMesh(Mesh mesh)
	{
		hasChanged = true;
		_mesh = mesh;
	}

	void IMeshModifier.ModifyMesh(VertexHelper verts)
	{
		if (base.isActiveAndEnabled)
		{
			if (ignoreSelfGraphic)
			{
				verts.Clear();
				verts.FillMesh(mesh);
			}
			else if (ignoreSelfStencil)
			{
				verts.FillMesh(mesh);
				verts.Clear();
			}
			else
			{
				verts.FillMesh(mesh);
			}
		}
		hasChanged = true;
	}

	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera, Graphic g, int[] interactions)
	{
		if (!base.isActiveAndEnabled || (g == base.graphic && !g.raycastTarget))
		{
			return true;
		}
		int x = (int)((float)(softMaskBuffer.width - 1) * Mathf.Clamp01(sp.x / (float)Screen.width));
		int y = ((s_UVStartsAtTop && !s_IsMetal) ? ((int)((float)(softMaskBuffer.height - 1) * (1f - Mathf.Clamp01(sp.y / (float)Screen.height)))) : ((int)((float)(softMaskBuffer.height - 1) * Mathf.Clamp01(sp.y / (float)Screen.height))));
		return 0.5f < GetPixelValue(x, y, interactions);
	}

	public override bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		return true;
	}

	protected override void OnEnable()
	{
		hasChanged = true;
		if (s_ActiveSoftMasks.Count == 0)
		{
			Canvas.willRenderCanvases += UpdateMaskTextures;
			if (s_StencilCompId == 0)
			{
				s_UVStartsAtTop = SystemInfo.graphicsUVStartsAtTop;
				s_IsMetal = SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal;
				s_StencilCompId = Shader.PropertyToID("_StencilComp");
				s_ColorMaskId = Shader.PropertyToID("_ColorMask");
				s_MainTexId = Shader.PropertyToID("_MainTex");
				s_SoftnessId = Shader.PropertyToID("_Softness");
				s_Alpha = Shader.PropertyToID("_Alpha");
			}
		}
		s_ActiveSoftMasks.Add(this);
		GetComponentsInChildren(includeInactive: false, s_TempRelatables);
		int num = s_TempRelatables.Count - 1;
		while (0 <= num)
		{
			s_TempRelatables[num].OnTransformParentChanged();
			num--;
		}
		s_TempRelatables.Clear();
		_mpb = new MaterialPropertyBlock();
		_cb = new CommandBuffer();
		base.graphic.SetVerticesDirtyEx();
		base.OnEnable();
		_hasStencilStateChanged = false;
	}

	protected override void OnDisable()
	{
		s_ActiveSoftMasks.Remove(this);
		if (s_ActiveSoftMasks.Count == 0)
		{
			Canvas.willRenderCanvases -= UpdateMaskTextures;
		}
		int num = _children.Count - 1;
		while (0 <= num)
		{
			_children[num].SetParent(_parent);
			num--;
		}
		_children.Clear();
		SetParent(null);
		_mpb.Clear();
		_mpb = null;
		_cb.Release();
		_cb = null;
		ReleaseObject(_mesh);
		_mesh = null;
		ReleaseRt(ref _softMaskBuffer);
		base.OnDisable();
		_hasStencilStateChanged = false;
	}

	protected override void OnTransformParentChanged()
	{
		hasChanged = true;
		SoftMask softMask = null;
		if (base.isActiveAndEnabled && !m_IgnoreParent)
		{
			Transform transform = base.transform.parent;
			while ((bool)transform && (!softMask || !softMask.enabled))
			{
				softMask = transform.GetComponent<SoftMask>();
				transform = transform.parent;
			}
		}
		SetParent(softMask);
		hasChanged = true;
	}

	protected override void OnRectTransformDimensionsChange()
	{
		hasChanged = true;
	}

	private static void UpdateMaskTextures()
	{
		foreach (SoftMask s_ActiveSoftMask in s_ActiveSoftMasks)
		{
			if (!s_ActiveSoftMask || s_ActiveSoftMask._hasChanged)
			{
				continue;
			}
			Canvas canvas = s_ActiveSoftMask.graphic.canvas;
			if (!canvas)
			{
				continue;
			}
			if (canvas.renderMode == RenderMode.WorldSpace)
			{
				Camera worldCamera = canvas.worldCamera;
				if (!worldCamera)
				{
					continue;
				}
				Matrix4x4 matrix4x = worldCamera.projectionMatrix * worldCamera.worldToCameraMatrix;
				Matrix4x4 value = default(Matrix4x4);
				int instanceID = worldCamera.GetInstanceID();
				s_PreviousViewProjectionMatrices.TryGetValue(instanceID, out value);
				s_NowViewProjectionMatrices[instanceID] = matrix4x;
				if (value != matrix4x)
				{
					s_ActiveSoftMask.hasChanged = true;
				}
			}
			RectTransform rectTransform = s_ActiveSoftMask.rectTransform;
			if (rectTransform.hasChanged)
			{
				rectTransform.hasChanged = false;
				s_ActiveSoftMask.hasChanged = true;
			}
		}
		foreach (SoftMask s_ActiveSoftMask2 in s_ActiveSoftMasks)
		{
			if (!s_ActiveSoftMask2 || !s_ActiveSoftMask2._hasChanged)
			{
				continue;
			}
			s_ActiveSoftMask2._hasChanged = false;
			if (!s_ActiveSoftMask2._parent)
			{
				s_ActiveSoftMask2.UpdateMaskTexture();
				if (s_ActiveSoftMask2._hasStencilStateChanged)
				{
					s_ActiveSoftMask2._hasStencilStateChanged = false;
					MaskUtilities.NotifyStencilStateChanged(s_ActiveSoftMask2);
				}
			}
		}
		s_PreviousViewProjectionMatrices.Clear();
		foreach (KeyValuePair<int, Matrix4x4> s_NowViewProjectionMatrix in s_NowViewProjectionMatrices)
		{
			s_PreviousViewProjectionMatrices.Add(s_NowViewProjectionMatrix.Key, s_NowViewProjectionMatrix.Value);
		}
		s_NowViewProjectionMatrices.Clear();
	}

	private void UpdateMaskTexture()
	{
		if (!base.graphic || !base.graphic.canvas)
		{
			return;
		}
		_stencilDepth = MaskUtilities.GetStencilDepth(base.transform, MaskUtilities.FindRootSortOverrideCanvas(base.transform));
		int i = 0;
		s_TmpSoftMasks[0].Add(this);
		for (; _stencilDepth + i < 3; i++)
		{
			int count = s_TmpSoftMasks[i].Count;
			for (int j = 0; j < count; j++)
			{
				List<SoftMask> children = s_TmpSoftMasks[i][j]._children;
				int count2 = children.Count;
				for (int k = 0; k < count2; k++)
				{
					SoftMask softMask = children[k];
					int num = (softMask.m_PartOfParent ? i : (i + 1));
					s_TmpSoftMasks[num].Add(softMask);
				}
			}
		}
		_cb.Clear();
		_cb.SetRenderTarget(softMaskBuffer);
		_cb.ClearRenderTarget(clearDepth: false, clearColor: true, s_ClearColors[_stencilDepth]);
		Canvas rootCanvas = base.graphic.canvas.rootCanvas;
		Camera camera = rootCanvas.worldCamera ?? Camera.main;
		if ((bool)rootCanvas && rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay && (bool)camera)
		{
			Matrix4x4 gPUProjectionMatrix = GL.GetGPUProjectionMatrix(camera.projectionMatrix, renderIntoTexture: false);
			_cb.SetViewProjectionMatrices(camera.worldToCameraMatrix, gPUProjectionMatrix);
		}
		else
		{
			Vector3 position = rootCanvas.transform.position;
			Matrix4x4 view = Matrix4x4.TRS(new Vector3(0f - position.x, 0f - position.y, -1000f), Quaternion.identity, new Vector3(1f, 1f, -1f));
			Matrix4x4 proj = Matrix4x4.TRS(new Vector3(0f, 0f, -1f), Quaternion.identity, new Vector3(1f / position.x, 1f / position.y, -0.0002f));
			_cb.SetViewProjectionMatrices(view, proj);
		}
		for (int l = 0; l < s_TmpSoftMasks.Length; l++)
		{
			int count3 = s_TmpSoftMasks[l].Count;
			for (int m = 0; m < count3; m++)
			{
				SoftMask softMask2 = s_TmpSoftMasks[l][m];
				if (l != 0)
				{
					softMask2._stencilDepth = MaskUtilities.GetStencilDepth(softMask2.transform, MaskUtilities.FindRootSortOverrideCanvas(softMask2.transform));
				}
				softMask2.material.SetInt(s_ColorMaskId, 1 << 3 - _stencilDepth - l);
				softMask2._mpb.SetTexture(s_MainTexId, softMask2.graphic.mainTexture);
				softMask2._mpb.SetFloat(s_SoftnessId, softMask2.m_Softness);
				softMask2._mpb.SetFloat(s_Alpha, softMask2.m_Alpha);
				_cb.DrawMesh(softMask2.mesh, softMask2.transform.localToWorldMatrix, softMask2.material, 0, 0, softMask2._mpb);
			}
			s_TmpSoftMasks[l].Clear();
		}
		Graphics.ExecuteCommandBuffer(_cb);
	}

	private static void GetDownSamplingSize(DownSamplingRate rate, out int w, out int h)
	{
		if (Screen.fullScreenMode == FullScreenMode.Windowed)
		{
			w = Screen.width;
			h = Screen.height;
		}
		else
		{
			w = Screen.currentResolution.width;
			h = Screen.currentResolution.height;
		}
		if (rate != DownSamplingRate.None)
		{
			float num = (float)w / (float)h;
			if (w < h)
			{
				h = Mathf.ClosestPowerOfTwo(h / (int)rate);
				w = Mathf.CeilToInt((float)h * num);
			}
			else
			{
				w = Mathf.ClosestPowerOfTwo(w / (int)rate);
				h = Mathf.CeilToInt((float)w / num);
			}
		}
	}

	private static void ReleaseRt(ref RenderTexture tmpRT)
	{
		if ((bool)tmpRT)
		{
			tmpRT.Release();
			RenderTexture.ReleaseTemporary(tmpRT);
			tmpRT = null;
		}
	}

	private static void ReleaseObject(Object obj)
	{
		if ((bool)obj)
		{
			Object.Destroy(obj);
		}
	}

	private void SetParent(SoftMask newParent)
	{
		if (_parent != newParent && this != newParent)
		{
			if ((bool)_parent && _parent._children.Contains(this))
			{
				_parent._children.Remove(this);
				_parent._children.RemoveAll((SoftMask x) => x == null);
			}
			_parent = newParent;
		}
		if ((bool)_parent && !_parent._children.Contains(this))
		{
			_parent._children.Add(this);
		}
	}

	private float GetPixelValue(int x, int y, int[] interactions)
	{
		if (!s_ReadTexture)
		{
			s_ReadTexture = new Texture2D(1, 1, TextureFormat.ARGB32, mipChain: false);
		}
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = softMaskBuffer;
		s_ReadTexture.ReadPixels(new Rect(x, y, 1f, 1f), 0, 0);
		s_ReadTexture.Apply(updateMipmaps: false, makeNoLongerReadable: false);
		RenderTexture.active = active;
		byte[] rawTextureData = s_ReadTexture.GetRawTextureData();
		for (int i = 0; i < 4; i++)
		{
			switch (interactions[(i + 3) % 4])
			{
			case 0:
				rawTextureData[i] = byte.MaxValue;
				break;
			case 2:
				rawTextureData[i] = (byte)(255 - rawTextureData[i]);
				break;
			}
		}
		return _stencilDepth switch
		{
			0 => (float)(int)rawTextureData[1] / 255f, 
			1 => (float)(int)rawTextureData[1] / 255f * ((float)(int)rawTextureData[2] / 255f), 
			2 => (float)(int)rawTextureData[1] / 255f * ((float)(int)rawTextureData[2] / 255f) * ((float)(int)rawTextureData[3] / 255f), 
			3 => (float)(int)rawTextureData[1] / 255f * ((float)(int)rawTextureData[2] / 255f) * ((float)(int)rawTextureData[3] / 255f) * ((float)(int)rawTextureData[0] / 255f), 
			_ => 0f, 
		};
	}
}
