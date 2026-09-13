using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FibMatrix.Rendering;

[ExecuteAlways]
[HelpURL("https://rivergame.feishu.cn/wiki/wikcnTdzKLv9KPCqxQjY9aMx2Je")]
public class MistComponent : MonoBehaviour
{
	public static MistComponent instance;

	public int edgeBlurIteration = 1;

	public float edgeBlurSize = 2f;

	public Material blurMaterial;

	public const string BlurMaterialPixDistMulAddPropertyName = "_PixDistMulAdd";

	public const string BlurMaterialMainTexPropertyName = "_MainTex";

	public readonly int BlurMaterialPixDistMulAddPropertyId = Shader.PropertyToID("_PixDistMulAdd");

	public readonly int BlurMaterialMainTexPropertyId = Shader.PropertyToID("_MainTex");

	public const string GLOBAL_MIST_MASK_TEX_NAME = "_MistMaskTex";

	public const string GLOBAL_MIST_MASK_SIZE_INFO_NAME = "_MistMaskSize";

	public static readonly int GLOBAL_MIST_MASK_TEX_ID = Shader.PropertyToID("_MistMaskTex");

	public static readonly int GLOBAL_MIST_MASK_SIZE_INFO_ID = Shader.PropertyToID("_MistMaskSize");

	private MistMaskData m_MistMaskData;

	private Texture2D m_MistMaskTex;

	private RenderTexture[] m_MistMaskBlured;

	private int m_MistMaskBluredIndex;

	private Color[] RawMistTexColors;

	private Color[] CurrentMistTexColors;

	private float[,] RawMistData;

	private bool m_PixelChanged;

	private const uint Border = 3u;

	private GameObject m_Spheres;

	private Coroutine coroutineFading;

	public bool enableFading { get; set; }

	public float duration { get; set; } = 1f;

	public bool defaultBlock { get; set; } = true;

	private int m_DataWidth => m_MistMaskData.mapSizeX;

	private int m_DataHeight => m_MistMaskData.mapSizeZ;

	private int m_TextureWidth => m_MistMaskData.mapSizeX;

	private int m_TextureHeight => m_MistMaskData.mapSizeZ;

	private bool EdgeBlurEnable
	{
		get
		{
			if (edgeBlurSize > 1E-05f)
			{
				return edgeBlurIteration > 0;
			}
			return false;
		}
	}

	private string DevelopDataDirectory { get; set; } = "Packages/com.rivergame.rendering/Samples/mist";

	private bool Develop { get; set; }

	private Vector3 Tp3dGetWorldPosByPosDevelop(Vector2 pos)
	{
		float num = 90f;
		float num2 = (0f - 70f) / 2f;
		float num3 = num / 2f;
		Vector3 zero = Vector3.zero;
		zero.x = num2 + pos.x * 1f / 2f + 0.5f;
		zero.z = num3 - pos.y * 1f / 2f - 0.5f;
		return Quaternion.AngleAxis(45f, Vector3.up) * zero;
	}

	private void MistDevelop()
	{
		Clean();
		if (!Develop)
		{
			return;
		}
		ReleaseMist();
		List<Vector3> points = new List<Vector3>();
		Action<string> obj = delegate(string filename)
		{
			points.Clear();
			string[] array = File.ReadAllLines(DevelopDataDirectory.TrimEnd('/', '\\') + "/" + filename + ".txt");
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Trim('(', ')').Split(new char[1] { ',' });
				points.Add(Tp3dGetWorldPosByPosDevelop(new Vector2(int.Parse(array2[0]), int.Parse(array2[1]))));
			}
		};
		float num = Mathf.Sqrt(2f) * 0.5f;
		float num2 = Mathf.Sqrt(2f) * 0.5f;
		float num3 = -70f;
		float num4 = -90f;
		MistMaskData mapDataSize = new MistMaskData
		{
			mapSizeX = Convert.ToInt32(140f / num),
			mapSizeZ = Convert.ToInt32(180f / num2),
			mapOffsetX = 0f - num3 + 1f,
			mapOffsetZ = 0f - num4,
			mapSizeScaleX = num,
			mapSizeScaleZ = num2
		};
		ResetMist(mapDataSize, 1f);
		obj("frees");
		foreach (Vector3 item in points)
		{
			SetMist(item, 0f);
		}
		m_Spheres = new GameObject();
		m_Spheres.hideFlags = HideFlags.DontSave;
		foreach (Vector3 item2 in points)
		{
			GameObject obj2 = GameObject.CreatePrimitive(PrimitiveType.Quad);
			obj2.transform.SetParent(m_Spheres.transform, worldPositionStays: true);
			obj2.transform.position = item2 + Vector3.up * 0.1f;
			obj2.transform.localEulerAngles = Vector3.right * 90f;
			obj2.transform.localScale = Vector3.one * 0.5f;
		}
		m_PixelChanged = true;
		FlushPixels();
	}

	private void Clean()
	{
		if (m_Spheres != null)
		{
			DestroyInternal(m_Spheres);
			m_Spheres = null;
		}
	}

	protected void OnEnable()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Debug.LogError("multi instance for MistComponent");
		}
	}

	protected void OnDisable()
	{
		instance = null;
		Clean();
	}

	protected void OnDestroy()
	{
		ReleaseMist();
	}

	private void CreateMist()
	{
		RawMistData = new float[m_TextureWidth, m_TextureHeight];
		RawMistTexColors = new Color[m_TextureWidth * m_TextureHeight];
		CurrentMistTexColors = new Color[m_TextureWidth * m_TextureHeight];
		CreateBaseMask();
		CreateBlurMask();
		UpdateMistMaskTexture();
	}

	private void ReleaseMist()
	{
		StopFading();
		ReleaseBaseMask();
		ReleaseBlurMask();
	}

	private void CreateBaseMask()
	{
		TextureFormat textureFormat = (SystemInfo.SupportsTextureFormat(TextureFormat.R8) ? TextureFormat.R8 : (SystemInfo.SupportsTextureFormat(TextureFormat.Alpha8) ? TextureFormat.Alpha8 : (SystemInfo.SupportsTextureFormat(TextureFormat.RHalf) ? TextureFormat.RHalf : TextureFormat.RGBA32)));
		m_MistMaskTex = new Texture2D(m_TextureWidth, m_TextureHeight, textureFormat, mipChain: false, linear: false);
		m_MistMaskTex.wrapMode = TextureWrapMode.Clamp;
		m_MistMaskTex.filterMode = FilterMode.Bilinear;
	}

	private void ReleaseBaseMask()
	{
		if ((bool)m_MistMaskTex)
		{
			DestroyInternal(m_MistMaskTex);
			m_MistMaskTex = null;
		}
	}

	private void CreateBlurMask()
	{
		if (EdgeBlurEnable)
		{
			if (m_MistMaskBlured == null)
			{
				m_MistMaskBlured = new RenderTexture[2];
			}
			for (int i = 0; i < Math.Min(2, edgeBlurIteration); i++)
			{
				m_MistMaskBlured[i] = RenderTexture.GetTemporary(Mathf.Max(m_TextureWidth, 4), Mathf.Max(m_TextureHeight, 4), 0);
				m_MistMaskBlured[i].wrapMode = TextureWrapMode.Clamp;
				m_MistMaskBlured[i].name = $"MistMaskBluredSwapChain [{i}]";
			}
		}
	}

	private void ReleaseBlurMask()
	{
		if (m_MistMaskBlured == null)
		{
			return;
		}
		for (int i = 0; i < m_MistMaskBlured.Length; i++)
		{
			if (m_MistMaskBlured[i] != null)
			{
				RenderTexture.ReleaseTemporary(m_MistMaskBlured[i]);
				m_MistMaskBlured[i] = null;
			}
		}
	}

	private void UpdateMistMaskTexture()
	{
		if (!EdgeBlurEnable)
		{
			Shader.SetGlobalTexture(GLOBAL_MIST_MASK_TEX_ID, m_MistMaskTex);
		}
		else
		{
			Shader.SetGlobalTexture(GLOBAL_MIST_MASK_TEX_ID, m_MistMaskBlured[m_MistMaskBluredIndex]);
		}
	}

	private void DestroyInternal(UnityEngine.Object obj)
	{
		UnityEngine.Object.Destroy(obj);
	}

	public void ResetMist(MistMaskData mapDataSize, float fillWithMistValue = -1f)
	{
		m_MistMaskData = new MistMaskData
		{
			mapSizeX = mapDataSize.mapSizeX + 6,
			mapSizeZ = mapDataSize.mapSizeZ + 6,
			mapOffsetX = mapDataSize.mapOffsetX,
			mapOffsetZ = mapDataSize.mapOffsetZ,
			mapSizeScaleX = mapDataSize.mapSizeScaleX,
			mapSizeScaleZ = mapDataSize.mapSizeScaleZ
		};
		CreateMist();
		float num = (float)m_MistMaskData.mapSizeX * m_MistMaskData.mapSizeScaleX;
		float num2 = (float)m_MistMaskData.mapSizeZ * m_MistMaskData.mapSizeScaleZ;
		float y = m_MistMaskData.mapOffsetZ + 3f * m_MistMaskData.mapSizeScaleZ;
		float x = m_MistMaskData.mapOffsetX + 3f * m_MistMaskData.mapSizeScaleX;
		Shader.SetGlobalVector(GLOBAL_MIST_MASK_SIZE_INFO_ID, new Vector4(x, y, 1f / num, 1f / num2));
		if (fillWithMistValue > -0.9f)
		{
			m_PixelChanged = true;
			for (int i = 0; i < m_DataWidth; i++)
			{
				for (int j = 0; j < m_DataHeight; j++)
				{
					if ((long)i < 3L || i >= (long)m_DataWidth - 3L || (long)j < 3L || j >= (long)m_DataHeight - 3L)
					{
						RawMistData[i, j] = Convert.ToSingle(defaultBlock);
						RawMistTexColors[j * m_TextureWidth + i] = (defaultBlock ? Color.white : Color.clear);
					}
					else
					{
						RawMistData[i, j] = fillWithMistValue;
						RawMistTexColors[j * m_TextureWidth + i] = new Color(fillWithMistValue, fillWithMistValue, fillWithMistValue, fillWithMistValue);
					}
				}
			}
		}
		for (int k = 0; k < m_DataWidth; k++)
		{
			for (int l = 0; l < m_DataHeight; l++)
			{
				CurrentMistTexColors[l * m_TextureWidth + k] = Color.white;
			}
		}
	}

	public Vector2Int GetMistCoordByPositionWS(Vector3 positionWS)
	{
		return new Vector2Int((int)((positionWS.x + m_MistMaskData.mapOffsetX) / m_MistMaskData.mapSizeScaleX), (int)((positionWS.z + m_MistMaskData.mapOffsetZ) / m_MistMaskData.mapSizeScaleZ));
	}

	public Vector3 GetPositionWSByMistCoord(Vector2Int coord)
	{
		float x = (float)coord.x * m_MistMaskData.mapSizeScaleX - m_MistMaskData.mapOffsetX;
		float z = (float)coord.y * m_MistMaskData.mapSizeScaleZ - m_MistMaskData.mapOffsetZ;
		return new Vector3(x, 0f, z);
	}

	public void SetMist(Vector3 positionWS, float a)
	{
		Vector2Int mistCoordByPositionWS = GetMistCoordByPositionWS(positionWS);
		SetMist(mistCoordByPositionWS.x, mistCoordByPositionWS.y, a);
	}

	public void SetMist(int coordX, int coordZ, float a)
	{
		coordX += 3;
		coordZ += 3;
		if ((long)coordX >= 3L && coordX < (long)m_DataWidth - 3L && (long)coordZ >= 3L && coordZ < (long)m_DataHeight - 3L)
		{
			bool flag = false;
			if (RawMistData[coordX, coordZ] != a)
			{
				RawMistData[coordX, coordZ] = a;
				RawMistTexColors[coordZ * m_TextureWidth + coordX] = new Color(a, a, a, a);
				flag = true;
			}
			m_PixelChanged |= flag;
		}
	}

	public void FlushPixels()
	{
		if (m_PixelChanged)
		{
			m_PixelChanged = false;
			ApplyToRenderState();
		}
	}

	private void ApplyToRenderState()
	{
		if (enableFading)
		{
			StartFading();
			return;
		}
		Array.Copy(RawMistTexColors, CurrentMistTexColors, RawMistTexColors.Length);
		DoFlush();
	}

	private void DoFlush()
	{
		if (m_MistMaskTex != null)
		{
			m_MistMaskTex.SetPixels(CurrentMistTexColors);
			m_MistMaskTex.Apply();
			DoBlur();
		}
	}

	private void DoBlur()
	{
		if (EdgeBlurEnable && blurMaterial != null)
		{
			m_MistMaskBluredIndex = 0;
			DoBlur(m_MistMaskTex, m_MistMaskBlured[m_MistMaskBluredIndex], edgeBlurSize);
			for (int i = 0; i < edgeBlurIteration - 1; i++)
			{
				DoBlur(m_MistMaskBlured[m_MistMaskBluredIndex], m_MistMaskBlured[1 - m_MistMaskBluredIndex], edgeBlurSize);
				m_MistMaskBluredIndex = 1 - m_MistMaskBluredIndex;
			}
		}
	}

	private void DoBlur(Texture src, RenderTexture dst, float blurSize)
	{
		Vector4 vector = blurMaterial.GetVector(BlurMaterialPixDistMulAddPropertyId);
		vector.x = edgeBlurSize;
		vector.y = edgeBlurSize;
		blurMaterial.SetVector(BlurMaterialPixDistMulAddPropertyId, vector);
		blurMaterial.SetTexture(BlurMaterialMainTexPropertyId, m_MistMaskTex);
		Graphics.Blit(src, dst, blurMaterial);
	}

	private void ReBlur()
	{
		ReleaseBlurMask();
		CreateBlurMask();
		DoBlur();
		UpdateMistMaskTexture();
	}

	public void SetEdgeBlurSize(float size)
	{
		if (!(size < 1f))
		{
			edgeBlurSize = size;
			ReBlur();
		}
	}

	public void StartFading()
	{
		coroutineFading = StartCoroutine(CoroutineFading(duration));
	}

	public void StopFading()
	{
		if (coroutineFading != null)
		{
			StopCoroutine(coroutineFading);
			coroutineFading = null;
		}
	}

	private bool FadingAction(float deltaTime)
	{
		bool flag = true;
		for (int i = 0; i < RawMistTexColors.Length; i++)
		{
			if (CurrentMistTexColors[i].r > RawMistTexColors[i].r)
			{
				flag = false;
				CurrentMistTexColors[i] = Color.white * (CurrentMistTexColors[i].r - deltaTime);
			}
			else
			{
				CurrentMistTexColors[i] = RawMistTexColors[i];
			}
		}
		if (flag)
		{
			StopFading();
		}
		else
		{
			DoFlush();
		}
		return flag;
	}

	private IEnumerator CoroutineFading(float duration)
	{
		float time = 0f;
		while (time < duration && !FadingAction(Time.deltaTime / duration))
		{
			time += Time.deltaTime;
			yield return null;
		}
	}
}
