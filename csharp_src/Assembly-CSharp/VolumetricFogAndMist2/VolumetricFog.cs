using System.Collections.Generic;
using UnityEngine;

namespace VolumetricFogAndMist2;

[ExecuteInEditMode]
public class VolumetricFog : MonoBehaviour
{
	private struct FogOfWarTransition
	{
		public bool enabled;

		public int x;

		public int y;

		public float startTime;

		public float startDelay;

		public float duration;

		public int initialAlpha;

		public int targetAlpha;
	}

	public bool enableFogOfWar;

	public Vector3 fogOfWarCenter;

	public Vector3 fogOfWarSize = new Vector3(1024f, 0f, 1024f);

	[Range(32f, 2048f)]
	public int fogOfWarTextureSize = 256;

	[Range(0f, 100f)]
	public float fogOfWarRestoreDelay;

	[Range(0f, 25f)]
	public float fogOfWarRestoreDuration = 2f;

	[Range(0f, 1f)]
	public float fogOfWarSmoothness = 1f;

	public bool fogOfWarBlur;

	private const int MAX_SIMULTANEOUS_TRANSITIONS = 10000;

	private bool canDestroyFOWTexture;

	public bool maskEditorEnabled;

	public MASK_TEXTURE_BRUSH_MODE maskBrushMode = MASK_TEXTURE_BRUSH_MODE.RemoveFog;

	public Color maskBrushColor = Color.white;

	[Range(1f, 128f)]
	public int maskBrushWidth = 20;

	[Range(0f, 1f)]
	public float maskBrushFuzziness = 0.5f;

	[Range(0f, 1f)]
	public float maskBrushOpacity = 0.15f;

	[SerializeField]
	private Texture2D _fogOfWarTexture;

	private Color32[] fogOfWarColorBuffer;

	private FogOfWarTransition[] fowTransitionList;

	private int lastTransitionPos;

	private Dictionary<int, int> fowTransitionIndices;

	private bool requiresTextureUpload;

	private Material fowBlur;

	private RenderTexture fowBlur1;

	private RenderTexture fowBlur2;

	private readonly int _FogOfWar = Shader.PropertyToID("_FogOfWar");

	private static readonly int m_Params = Shader.PropertyToID("_Params");

	private FOWSystem fogSystem;

	public bool enablePointLights;

	public bool enableVoids;

	private const string SKW_SHAPE_BOX = "V2F_SHAPE_BOX";

	private const string SKW_SHAPE_SPHERE = "V2F_SHAPE_SPHERE";

	private const string SKW_POINT_LIGHTS = "VF2_POINT_LIGHTS";

	private const string SKW_VOIDS = "VF2_VOIDS";

	private const string SKW_FOW = "VF2_FOW";

	private const string SKW_RECEIVE_SHADOWS = "VF2_RECEIVE_SHADOWS";

	private const string SKW_DISTANCE = "VF2_DISTANCE";

	private const string SKW_DETAIL_NOISE = "V2F_DETAIL_NOISE";

	private Renderer r;

	private Material fogMat;

	private Material noiseMat;

	private Material turbulenceMat;

	private Material fogMat2D;

	private Material noiseMat2D;

	private Material turbulenceMat2D;

	private RenderTexture rtNoise;

	private RenderTexture rtTurbulence;

	private float turbAcum;

	private Vector3 windDirectionAcum;

	private Vector3 sunDir;

	private float dayLight;

	private List<string> shaderKeywords;

	private Texture3D detailTex;

	private Texture3D refDetailTex;

	private MaterialPropertyBlock materBlock;

	private static VolumetricFog instance;

	public Texture2D fogOfWarTexture
	{
		get
		{
			return _fogOfWarTexture;
		}
		set
		{
			if (_fogOfWarTexture != value && value != null)
			{
				if (value.width != value.height)
				{
					Debug.LogError("Fog of war texture must be square.");
					return;
				}
				_fogOfWarTexture = value;
				canDestroyFOWTexture = false;
				ReloadFogOfWarTexture();
			}
		}
	}

	public Color32[] fogOfWarTextureData
	{
		get
		{
			return fogOfWarColorBuffer;
		}
		set
		{
			enableFogOfWar = true;
			fogOfWarColorBuffer = value;
			if (value != null && !(_fogOfWarTexture == null) && value.Length == _fogOfWarTexture.width * _fogOfWarTexture.height)
			{
				_fogOfWarTexture.SetPixels32(fogOfWarColorBuffer);
				_fogOfWarTexture.Apply();
			}
		}
	}

	public static VolumetricFog Instance => instance;

	private void FogOfWarInit()
	{
		if (fowTransitionList == null || fowTransitionList.Length != 10000)
		{
			fowTransitionList = new FogOfWarTransition[10000];
		}
		if (fowTransitionIndices == null)
		{
			fowTransitionIndices = new Dictionary<int, int>(10000);
		}
		else
		{
			fowTransitionIndices.Clear();
		}
		lastTransitionPos = -1;
		if (_fogOfWarTexture == null)
		{
			FogOfWarUpdateTexture();
		}
		else if (enableFogOfWar && (fogOfWarColorBuffer == null || fogOfWarColorBuffer.Length == 0))
		{
			ReloadFogOfWarTexture();
		}
	}

	private void FogOfWarDestroy()
	{
		if (canDestroyFOWTexture)
		{
			Object.DestroyImmediate(_fogOfWarTexture);
		}
		if (fowBlur1 != null)
		{
			fowBlur1.Release();
		}
		if (fowBlur2 != null)
		{
			fowBlur2.Release();
		}
	}

	public void ReloadFogOfWarTexture()
	{
		fogOfWarTextureSize = _fogOfWarTexture.width;
		fogOfWarColorBuffer = _fogOfWarTexture.GetPixels32();
		lastTransitionPos = -1;
		fowTransitionIndices.Clear();
		enableFogOfWar = true;
	}

	private void FogOfWarUpdateTexture()
	{
		if (!enableFogOfWar && Application.isPlaying)
		{
			int scaledSize = GetScaledSize(fogOfWarTextureSize, 1f);
			if (_fogOfWarTexture == null || _fogOfWarTexture.width != scaledSize || _fogOfWarTexture.height != scaledSize)
			{
				_fogOfWarTexture = new Texture2D(scaledSize, scaledSize, TextureFormat.RGBA32, mipChain: false, linear: true);
				_fogOfWarTexture.hideFlags = HideFlags.DontSave;
				_fogOfWarTexture.filterMode = FilterMode.Bilinear;
				_fogOfWarTexture.wrapMode = TextureWrapMode.Clamp;
				canDestroyFOWTexture = true;
				ResetFogOfWar();
			}
		}
	}

	private int GetScaledSize(int size, float factor)
	{
		size = (int)((float)size / factor);
		size /= 4;
		if (size < 1)
		{
			size = 1;
		}
		return size * 4;
	}

	public void UpdateFogOfWar(bool forceUpload = false)
	{
		if (!enableFogOfWar || _fogOfWarTexture == null)
		{
			return;
		}
		if (forceUpload)
		{
			requiresTextureUpload = true;
		}
		int width = _fogOfWarTexture.width;
		for (int i = 0; i <= lastTransitionPos; i++)
		{
			FogOfWarTransition fogOfWarTransition = fowTransitionList[i];
			if (!fogOfWarTransition.enabled)
			{
				continue;
			}
			float num = Time.time - fogOfWarTransition.startTime - fogOfWarTransition.startDelay;
			if (!(num > 0f))
			{
				continue;
			}
			float num2 = ((fogOfWarTransition.duration <= 0f) ? 1f : (num / fogOfWarTransition.duration));
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			else if (num2 > 1f)
			{
				num2 = 1f;
			}
			int num3 = (int)((float)fogOfWarTransition.initialAlpha + (float)(fogOfWarTransition.targetAlpha - fogOfWarTransition.initialAlpha) * num2);
			int num4 = fogOfWarTransition.y * width + fogOfWarTransition.x;
			fogOfWarColorBuffer[num4].a = (byte)num3;
			requiresTextureUpload = true;
			if (num2 >= 1f)
			{
				fowTransitionList[i].enabled = false;
				if (fogOfWarTransition.targetAlpha < 255 && fogOfWarRestoreDelay > 0f)
				{
					AddFogOfWarTransitionSlot(fogOfWarTransition.x, fogOfWarTransition.y, (byte)fogOfWarTransition.targetAlpha, byte.MaxValue, fogOfWarRestoreDelay, fogOfWarRestoreDuration);
				}
			}
		}
		if (requiresTextureUpload)
		{
			requiresTextureUpload = false;
			_fogOfWarTexture.SetPixels32(fogOfWarColorBuffer);
			_fogOfWarTexture.Apply();
			if (fogOfWarBlur)
			{
				SetFowBlurTexture();
			}
		}
	}

	private void SetFowBlurTexture()
	{
		if (fowBlur == null)
		{
			fowBlur = new Material(Shader.Find("VolumetricFog2/FoWBlur"));
			fowBlur.hideFlags = HideFlags.DontSave;
		}
		if (!(fowBlur == null))
		{
			if (fowBlur1 == null || fowBlur1.width != _fogOfWarTexture.width || fowBlur2 == null || fowBlur2.width != _fogOfWarTexture.width)
			{
				CreateFoWBlurRTs();
			}
			fowBlur1.DiscardContents();
			Graphics.Blit(_fogOfWarTexture, fowBlur1, fowBlur, 0);
			fowBlur2.DiscardContents();
			Graphics.Blit(fowBlur1, fowBlur2, fowBlur, 1);
			fogMat.SetTexture("_FogOfWar", fowBlur2);
		}
	}

	private void CreateFoWBlurRTs()
	{
		if (fowBlur1 != null)
		{
			fowBlur1.Release();
		}
		if (fowBlur2 != null)
		{
			fowBlur2.Release();
		}
		RenderTextureDescriptor desc = new RenderTextureDescriptor(_fogOfWarTexture.width, _fogOfWarTexture.height, RenderTextureFormat.ARGB32, 0);
		fowBlur1 = new RenderTexture(desc);
		fowBlur2 = new RenderTexture(desc);
	}

	public void SetFogOfWarAlpha(Vector3 worldPosition, float radius, float fogNewAlpha)
	{
		SetFogOfWarAlpha(worldPosition, radius, fogNewAlpha, 1f);
	}

	public void SetFogOfWarAlpha(Vector3 worldPosition, float radius, float fogNewAlpha, float duration)
	{
		SetFogOfWarAlpha(worldPosition, radius, fogNewAlpha, blendAlpha: true, duration, fogOfWarSmoothness, fogOfWarRestoreDelay, fogOfWarRestoreDuration);
	}

	public void SetFogOfWarAlpha(Vector3 worldPosition, float radius, float fogNewAlpha, float duration, float smoothness)
	{
		SetFogOfWarAlpha(worldPosition, radius, fogNewAlpha, blendAlpha: true, duration, smoothness, fogOfWarRestoreDelay, fogOfWarRestoreDuration);
	}

	public void SetFogOfWarAlpha(Vector3 worldPosition, float radius, float fogNewAlpha, bool blendAlpha, float duration, float smoothness, float restoreDelay, float restoreDuration)
	{
		if (_fogOfWarTexture == null || fogOfWarColorBuffer == null || fogOfWarColorBuffer.Length == 0)
		{
			return;
		}
		float num = (worldPosition.x - fogOfWarCenter.x) / fogOfWarSize.x + 0.5f;
		if (num < 0f || num > 1f)
		{
			return;
		}
		float num2 = (worldPosition.z - fogOfWarCenter.z) / fogOfWarSize.z + 0.5f;
		if (num2 < 0f || num2 > 1f)
		{
			return;
		}
		int width = _fogOfWarTexture.width;
		int height = _fogOfWarTexture.height;
		int num3 = (int)(num * (float)width);
		int num4 = (int)(num2 * (float)height);
		float num5 = 0.0001f + smoothness;
		byte b = (byte)(fogNewAlpha * 255f);
		float num6 = radius / fogOfWarSize.z;
		int num7 = (int)((float)height * num6);
		int num8 = num7 * num7;
		for (int i = num4 - num7; i <= num4 + num7; i++)
		{
			if (i <= 0 || i >= height - 1)
			{
				continue;
			}
			for (int j = num3 - num7; j <= num3 + num7; j++)
			{
				if (j <= 0 || j >= width - 1)
				{
					continue;
				}
				int num9 = (num4 - i) * (num4 - i) + (num3 - j) * (num3 - j);
				if (num9 > num8)
				{
					continue;
				}
				int num10 = i * width + j;
				Color32 color = fogOfWarColorBuffer[num10];
				if (!blendAlpha)
				{
					color.a = byte.MaxValue;
				}
				num9 = num8 - num9;
				float num11 = (float)num9 / ((float)num8 * num5);
				num11 = 1f - num11;
				if (num11 < 0f)
				{
					num11 = 0f;
				}
				else if (num11 > 1f)
				{
					num11 = 1f;
				}
				byte b2 = (byte)((float)(int)b + (float)(color.a - b) * num11);
				if (b2 >= byte.MaxValue)
				{
					continue;
				}
				if (duration > 0f)
				{
					AddFogOfWarTransitionSlot(j, i, color.a, b2, 0f, duration);
					continue;
				}
				color.a = b2;
				fogOfWarColorBuffer[num10] = color;
				requiresTextureUpload = true;
				if (restoreDelay > 0f)
				{
					AddFogOfWarTransitionSlot(j, i, b2, byte.MaxValue, restoreDelay, restoreDuration);
				}
			}
		}
	}

	public void SetFogOfWarAlpha(Bounds bounds, float fogNewAlpha, float duration)
	{
		SetFogOfWarAlpha(bounds, fogNewAlpha, blendAlpha: true, duration, fogOfWarSmoothness, fogOfWarRestoreDelay, fogOfWarRestoreDuration);
	}

	public void SetFogOfWarAlpha(Bounds bounds, float fogNewAlpha, float duration, float smoothness)
	{
		SetFogOfWarAlpha(bounds, fogNewAlpha, blendAlpha: true, duration, smoothness, fogOfWarRestoreDelay, fogOfWarRestoreDuration);
	}

	public void SetFogOfWarAlpha(Bounds bounds, float fogNewAlpha, bool blendAlpha, float duration, float smoothness, float restoreDelay, float restoreDuration)
	{
		if (_fogOfWarTexture == null || fogOfWarColorBuffer == null || fogOfWarColorBuffer.Length == 0)
		{
			return;
		}
		Vector3 center = bounds.center;
		float num = (center.x - fogOfWarCenter.x) / fogOfWarSize.x + 0.5f;
		if (num < 0f || num > 1f)
		{
			return;
		}
		float num2 = (center.z - fogOfWarCenter.z) / fogOfWarSize.z + 0.5f;
		if (num2 < 0f || num2 > 1f)
		{
			return;
		}
		int width = _fogOfWarTexture.width;
		int height = _fogOfWarTexture.height;
		int num3 = (int)(num * (float)width);
		int num4 = (int)(num2 * (float)height);
		byte b = (byte)(fogNewAlpha * 255f);
		float num5 = bounds.extents.z / fogOfWarSize.z;
		float num6 = bounds.extents.x / fogOfWarSize.x;
		float num7 = ((num6 > num5) ? 1f : (num5 / num6));
		float num8 = ((num6 > num5) ? (num6 / num5) : 1f);
		int num9 = (int)((float)height * num5);
		int num10 = num9 * num9;
		int num11 = (int)((float)width * num6);
		int num12 = num11 * num11;
		float num13 = 0.0001f + smoothness;
		for (int i = num4 - num9; i <= num4 + num9; i++)
		{
			if (i <= 0 || i >= height - 1)
			{
				continue;
			}
			int num14 = (num4 - i) * (num4 - i);
			num14 = num10 - num14;
			float num15 = (float)num14 * num7 / ((float)num10 * num13);
			for (int j = num3 - num11; j <= num3 + num11; j++)
			{
				if (j <= 0 || j >= width - 1)
				{
					continue;
				}
				int num16 = (num3 - j) * (num3 - j);
				int num17 = i * width + j;
				Color32 color = fogOfWarColorBuffer[num17];
				if (!blendAlpha)
				{
					color.a = byte.MaxValue;
				}
				num16 = num12 - num16;
				float num18 = (float)num16 * num8 / ((float)num12 * num13);
				float num19 = ((num15 < num18) ? num15 : num18);
				num19 = 1f - num19;
				if (num19 < 0f)
				{
					num19 = 0f;
				}
				else if (num19 > 1f)
				{
					num19 = 1f;
				}
				byte b2 = (byte)((float)(int)b + (float)(color.a - b) * num19);
				if (b2 >= byte.MaxValue)
				{
					continue;
				}
				if (duration > 0f)
				{
					AddFogOfWarTransitionSlot(j, i, color.a, b2, 0f, duration);
					continue;
				}
				color.a = b2;
				fogOfWarColorBuffer[num17] = color;
				requiresTextureUpload = true;
				if (restoreDelay > 0f)
				{
					AddFogOfWarTransitionSlot(j, i, b2, byte.MaxValue, restoreDelay, restoreDuration);
				}
			}
		}
	}

	public void ResetFogOfWarAlpha(Vector3 worldPosition, float radius)
	{
		if (_fogOfWarTexture == null || fogOfWarColorBuffer == null || fogOfWarColorBuffer.Length == 0)
		{
			return;
		}
		float num = (worldPosition.x - fogOfWarCenter.x) / fogOfWarSize.x + 0.5f;
		if (num < 0f || num > 1f)
		{
			return;
		}
		float num2 = (worldPosition.z - fogOfWarCenter.z) / fogOfWarSize.z + 0.5f;
		if (num2 < 0f || num2 > 1f)
		{
			return;
		}
		int width = _fogOfWarTexture.width;
		int height = _fogOfWarTexture.height;
		int num3 = (int)(num * (float)width);
		int num4 = (int)(num2 * (float)height);
		float num5 = radius / fogOfWarSize.z;
		int num6 = (int)((float)height * num5);
		int num7 = num6 * num6;
		for (int i = num4 - num6; i <= num4 + num6; i++)
		{
			if (i <= 0 || i >= height - 1)
			{
				continue;
			}
			for (int j = num3 - num6; j <= num3 + num6; j++)
			{
				if (j > 0 && j < width - 1 && (num4 - i) * (num4 - i) + (num3 - j) * (num3 - j) <= num7)
				{
					int num8 = i * width + j;
					Color32 color = fogOfWarColorBuffer[num8];
					color.a = byte.MaxValue;
					fogOfWarColorBuffer[num8] = color;
					requiresTextureUpload = true;
				}
			}
		}
	}

	public void ResetFogOfWarAlpha(Bounds bounds)
	{
		ResetFogOfWarAlpha(bounds.center, bounds.extents.x, bounds.extents.z);
	}

	public void ResetFogOfWarAlpha(Vector3 position, Vector3 size)
	{
		ResetFogOfWarAlpha(position, size.x * 0.5f, size.z * 0.5f);
	}

	public void ResetFogOfWarAlpha(Vector3 position, float extentsX, float extentsZ)
	{
		if (_fogOfWarTexture == null || fogOfWarColorBuffer == null || fogOfWarColorBuffer.Length == 0)
		{
			return;
		}
		float num = (position.x - fogOfWarCenter.x) / fogOfWarSize.x + 0.5f;
		if (num < 0f || num > 1f)
		{
			return;
		}
		float num2 = (position.z - fogOfWarCenter.z) / fogOfWarSize.z + 0.5f;
		if (num2 < 0f || num2 > 1f)
		{
			return;
		}
		int width = _fogOfWarTexture.width;
		int height = _fogOfWarTexture.height;
		int num3 = (int)(num * (float)width);
		int num4 = (int)(num2 * (float)height);
		float num5 = extentsZ / fogOfWarSize.z;
		float num6 = extentsX / fogOfWarSize.x;
		int num7 = (int)((float)height * num5);
		int num8 = (int)((float)width * num6);
		for (int i = num4 - num7; i <= num4 + num7; i++)
		{
			if (i <= 0 || i >= height - 1)
			{
				continue;
			}
			for (int j = num3 - num8; j <= num3 + num8; j++)
			{
				if (j > 0 && j < width - 1)
				{
					int num9 = i * width + j;
					Color32 color = fogOfWarColorBuffer[num9];
					color.a = byte.MaxValue;
					fogOfWarColorBuffer[num9] = color;
					requiresTextureUpload = true;
				}
			}
		}
	}

	public void ResetFogOfWar(byte alpha = byte.MaxValue)
	{
		if (!(_fogOfWarTexture == null))
		{
			int height = _fogOfWarTexture.height;
			int width = _fogOfWarTexture.width;
			int num = height * width;
			if (fogOfWarColorBuffer == null || fogOfWarColorBuffer.Length != num)
			{
				fogOfWarColorBuffer = new Color32[num];
			}
			Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, alpha);
			for (int i = 0; i < num; i++)
			{
				fogOfWarColorBuffer[i] = color;
			}
			_fogOfWarTexture.SetPixels32(fogOfWarColorBuffer);
			_fogOfWarTexture.Apply();
			lastTransitionPos = -1;
			fowTransitionIndices.Clear();
		}
	}

	private void AddFogOfWarTransitionSlot(int x, int y, byte initialAlpha, byte targetAlpha, float delay, float duration)
	{
		int key = y * 64000 + x;
		if (!fowTransitionIndices.TryGetValue(key, out var value))
		{
			value = -1;
			for (int i = 0; i <= lastTransitionPos; i++)
			{
				if (!fowTransitionList[i].enabled)
				{
					value = i;
					fowTransitionIndices[key] = value;
					break;
				}
			}
		}
		if (value >= 0 && fowTransitionList[value].enabled && (fowTransitionList[value].x != x || fowTransitionList[value].y != y))
		{
			value = -1;
		}
		if (value < 0)
		{
			if (lastTransitionPos >= 9999)
			{
				return;
			}
			value = ++lastTransitionPos;
			fowTransitionIndices[key] = value;
		}
		fowTransitionList[value].x = x;
		fowTransitionList[value].y = y;
		fowTransitionList[value].duration = duration;
		fowTransitionList[value].startTime = Time.time;
		fowTransitionList[value].startDelay = delay;
		fowTransitionList[value].initialAlpha = initialAlpha;
		fowTransitionList[value].targetAlpha = targetAlpha;
		fowTransitionList[value].enabled = true;
	}

	public float GetFogOfWarAlpha(Vector3 worldPosition)
	{
		if (fogOfWarColorBuffer == null || fogOfWarColorBuffer.Length == 0 || _fogOfWarTexture == null)
		{
			return 1f;
		}
		float num = (worldPosition.x - fogOfWarCenter.x) / fogOfWarSize.x + 0.5f;
		if (num < 0f || num > 1f)
		{
			return 1f;
		}
		float num2 = (worldPosition.z - fogOfWarCenter.z) / fogOfWarSize.z + 0.5f;
		if (num2 < 0f || num2 > 1f)
		{
			return 1f;
		}
		int width = _fogOfWarTexture.width;
		int height = _fogOfWarTexture.height;
		int num3 = (int)(num * (float)width);
		int num4 = (int)(num2 * (float)height) * width + num3;
		if (num4 < 0 || num4 >= fogOfWarColorBuffer.Length)
		{
			return 1f;
		}
		return (float)(int)fogOfWarColorBuffer[num4].a / 255f;
	}

	private void Awake()
	{
		instance = this;
		fogMat = GetComponent<MeshRenderer>().sharedMaterial;
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void LateUpdate()
	{
		base.transform.rotation = Quaternion.identity;
		if (fogSystem == null)
		{
			fogSystem = FOWSystem.instance;
		}
		if (fogSystem != null)
		{
			float num = 1f / (float)fogSystem.worldSize;
			Transform obj = fogSystem.transform;
			float num2 = obj.position.x - (float)fogSystem.worldSize * 0.5f;
			float num3 = obj.position.z - (float)fogSystem.worldSize * 0.5f;
			Vector4 value = new Vector4((0f - num2) * num, (0f - num3) * num, num, fogSystem.blendFactor);
			fogMat.SetVector(m_Params, value);
			fogMat.SetTexture(_FogOfWar, fogSystem.texture0);
		}
	}
}
