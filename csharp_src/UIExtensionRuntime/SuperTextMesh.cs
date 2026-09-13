using System;
using System.Collections.Generic;
using ArabicSupport;
using UnityEngine;
using UnityEngine.Rendering;

[AddComponentMenu("Mesh/Super Text Mesh", 3)]
[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SuperTextMesh : MonoBehaviour
{
	public enum Alignment
	{
		TopLeft,
		TopCenter,
		TopRight,
		MidLeft,
		MidCenter,
		MidRight,
		BotLeft,
		BotCenter,
		BotRight
	}

	private Transform t;

	private MeshFilter f;

	private MeshRenderer r;

	private bool isInit;

	private const int ReserveCount = 10;

	private List<STMCharInfo> info = new List<STMCharInfo>(10);

	private List<Vector3> endVerts = new List<Vector3>(40);

	private List<Vector2> endUv = new List<Vector2>(40);

	private List<Color32> endCol32 = new List<Color32>(40);

	private List<int> tris = new List<int>(60);

	private List<STMCharInfo> charInfoPool = new List<STMCharInfo>(10);

	private int allocIndex;

	public float alpha = 1f;

	public static bool IsArabicLanguage = false;

	[TextArea(3, 10)]
	public string _text = string.Empty;

	private Action _callBack;

	private string hyphenedText;

	[Tooltip("Font to be used by this text mesh. .rtf, .otf, and Unity fonts are supported.")]
	public Font font;

	[Tooltip("Default color of the text mesh. This can be changed with the <c> tag! See the docs for more info.")]
	public Color32 color = Color.white;

	[Tooltip("Size in local space for letters, by default. Can be changed with the <s> tag.")]
	public float size = 1f;

	[Range(1f, 500f)]
	[Tooltip("Point size of text. Try to keep it as small as possible while looking crisp!")]
	public int quality = 32;

	[Tooltip("Default letter style. Can be changed with the <i> and <b> tags, using rich text.")]
	public FontStyle style;

	[Tooltip("Adjust line spacing between multiple lines of text. 1 is the default for the font.")]
	public float lineSpacing = 1f;

	[Tooltip("Adjust additional spacing between characters. 0 is default.")]
	public float characterSpacing;

	[Tooltip("How far tabs indent.")]
	public float tabSize = 4f;

	[Tooltip("Distance in local space before a line break is automatically inserted at the previous space. Disabled if set to 0.")]
	public float autoWrap;

	[Tooltip("With auto wrap, should large words be split to fit in the box?")]
	public bool breakText = true;

	[Tooltip("When large words are split, Should a hyphen be inserted?")]
	public bool insertHyphens = true;

	[Tooltip("The material to be used by this text mesh. Look under 'STM' in the shader menu for more compatible shaders.")]
	public Material textMat;

	public bool gradient;

	[SerializeField]
	private Gradient effectGradient = new Gradient
	{
		colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(Color.black, 0f),
			new GradientColorKey(Color.white, 1f)
		}
	};

	public bool ztest = true;

	public bool outline;

	public Color outlineColor = Color.black;

	public Vector2 outlineDistance = new Vector2(0.01f, 0.01f);

	public Alignment alignment;

	[SerializeField]
	private float skewX;

	[SerializeField]
	private float skewY;

	[SerializeField]
	private string sortingLayer;

	[SerializeField]
	private int orderInLayer;

	private Mesh textMesh;

	private Vector3 rawTopLeftBounds;

	private Vector3 rawBottomRightBounds;

	private Vector3 topLeftBounds;

	private Vector3 topRightBounds;

	private Vector3 bottomLeftBounds;

	private Vector3 bottomRightBounds;

	private Vector3 centerBounds;

	private float lowestVert;

	private float rightestVert;

	private float _lineWidth;

	private int _lineCount;

	private float _width;

	private static int mainTexId = Shader.PropertyToID("_MainTex");

	private static int mixAlphaID = Shader.PropertyToID("mixAlpha");

	private static int keyAlphaID = Shader.PropertyToID("_Alpha");

	private MaterialPropertyBlock matBlock;

	public string text
	{
		get
		{
			return _text;
		}
		set
		{
			if (IsRtl(value))
			{
				value = ArabicFixer.FixText(value, PopulateWithErrors);
			}
			if (_text != value)
			{
				_text = value;
				Rebuild();
			}
		}
	}

	public Color32 color32
	{
		get
		{
			return color;
		}
		set
		{
			if (!color.Equals(value))
			{
				color = value;
				Rebuild();
			}
		}
	}

	public Gradient EffectGradient
	{
		get
		{
			return effectGradient;
		}
		set
		{
			effectGradient = value;
		}
	}

	public string MaterialKey { get; set; }

	private float AutoWrap => autoWrap;

	public int LineCount => _lineCount;

	private STMCharInfo NewCharInfo(SuperTextMesh stm)
	{
		STMCharInfo sTMCharInfo;
		if (allocIndex == charInfoPool.Count)
		{
			sTMCharInfo = new STMCharInfo(stm);
			charInfoPool.Add(sTMCharInfo);
			allocIndex = charInfoPool.Count;
			return sTMCharInfo;
		}
		sTMCharInfo = charInfoPool[allocIndex++];
		sTMCharInfo.Reset(stm);
		return sTMCharInfo;
	}

	private STMCharInfo NewCharInfo(STMCharInfo clone, CharacterInfo ch)
	{
		STMCharInfo sTMCharInfo;
		if (allocIndex == charInfoPool.Count)
		{
			sTMCharInfo = new STMCharInfo(clone, ch);
			charInfoPool.Add(sTMCharInfo);
			allocIndex = charInfoPool.Count;
			return sTMCharInfo;
		}
		sTMCharInfo = charInfoPool[allocIndex++];
		sTMCharInfo.Reset(clone, ch);
		return sTMCharInfo;
	}

	private void ResetCharInfoPool()
	{
		allocIndex = 0;
	}

	public void FontTextureChanged()
	{
		Rebuild();
	}

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		if (!isInit)
		{
			t = base.transform;
			f = GetComponent<MeshFilter>();
			r = GetComponent<MeshRenderer>();
			r.sortingLayerName = sortingLayer;
			r.sortingOrder = orderInLayer;
			for (int i = 0; i < 10; i++)
			{
				charInfoPool.Add(new STMCharInfo(this));
			}
			isInit = true;
		}
	}

	public void SetOrderInLayer(int orderInLayer)
	{
		this.orderInLayer = orderInLayer;
		if (r != null)
		{
			r.sortingOrder = orderInLayer;
		}
	}

	private void OnEnable()
	{
		r.shadowCastingMode = ShadowCastingMode.Off;
		r.hideFlags = HideFlags.HideInInspector;
		f.hideFlags = HideFlags.HideInInspector;
		FontUpdateTracker.TrackText(this);
		Rebuild();
	}

	private void OnDisable()
	{
		FontUpdateTracker.UntrackText(this);
	}

	private void OnDestroy()
	{
		if (textMesh != null)
		{
			UnityEngine.Object.Destroy(textMesh);
		}
	}

	public void SetCallBack(Action callBack)
	{
		_callBack = callBack;
	}

	public void Rebuild()
	{
		if (font == null)
		{
			font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		}
		Init();
		ResetCharInfoPool();
		RebuildTextInfo();
		ApplyAlignment();
		SetMesh();
		ApplyMaterial();
		RecalculateBounds();
		_callBack?.Invoke();
	}

	private void RebuildTextInfo()
	{
		lowestVert = size;
		rightestVert = 0f;
		_lineCount = 0;
		info.Clear();
		for (int i = 0; i < text.Length; i++)
		{
			info.Add(NewCharInfo(this));
		}
		font.RequestCharactersInTexture(text, quality, style);
		for (int j = 0; j < text.Length; j++)
		{
			font.GetCharacterInfo(text[j], out info[j].ch, quality, style);
		}
		Vector3 pos = new Vector3(0f, 0f, 0f);
		if (AutoWrap > 0f)
		{
			hyphenedText = string.Copy(text);
			float num = ((info.Count > 0) ? info[0].indent : 0f);
			int num2 = 0;
			int k = 0;
			for (int num3 = hyphenedText.Length; k < num3; k++)
			{
				font.GetCharacterInfo(' ', out var ch, quality, style);
				font.RequestCharactersInTexture("-", quality, style);
				font.GetCharacterInfo('-', out var ch2, quality, style);
				num = ((hyphenedText[k] == '\n') ? 0f : ((hyphenedText[k] != '\t') ? (num + info[k].Advance(characterSpacing, info[k].ch.size).x) : (num + 0.5f * tabSize * info[k].size)));
				if (!(num > AutoWrap))
				{
					continue;
				}
				int num4 = hyphenedText.LastIndexOf(' ', k);
				int num5 = hyphenedText.LastIndexOf('-', k);
				int num6 = hyphenedText.LastIndexOf('\t', k);
				int num7 = Mathf.Max(num4, num5, num6);
				int num8 = hyphenedText.LastIndexOf('\n', k);
				if (!breakText && num7 != -1 && num7 > num8)
				{
					if (num7 == num5)
					{
						hyphenedText = hyphenedText.Insert(num7 + 1, '\n'.ToString());
						info.Insert(num7 + 1, NewCharInfo(info[num7], ch));
						k = num7 + 1;
						num3++;
						num2++;
					}
					else
					{
						hyphenedText = hyphenedText.Remove(num7, 1);
						hyphenedText = hyphenedText.Insert(num7, '\n'.ToString());
						k = num7;
					}
					num = info[k].indent;
				}
				else
				{
					if (k == 0)
					{
						continue;
					}
					if (insertHyphens)
					{
						hyphenedText = hyphenedText.Insert(k, "-\n");
						info.Insert(k, NewCharInfo(info[k - num2], ch));
						info.Insert(k, NewCharInfo(info[k - num2], ch2));
						if (AutoWrap < info[k - num2].size)
						{
							k += 2;
						}
						num3 += 2;
						num2 += 2;
					}
					else
					{
						hyphenedText = hyphenedText.Insert(k, "\n");
						info.Insert(k, NewCharInfo(info[k - num2], ch));
						if (AutoWrap < info[k - num2].size)
						{
							k++;
						}
						num3++;
						num2++;
					}
					num = info[k].indent;
				}
			}
		}
		else
		{
			hyphenedText = text;
		}
		int num9 = 0;
		int l = 0;
		for (int length = hyphenedText.Length; l < length; l++)
		{
			info[l].pos = pos;
			info[l].line = num9;
			if (hyphenedText[l] == '\n')
			{
				pos = new Vector3(0f, pos.y, 0f);
				pos -= new Vector3(0f, (float)quality * lineSpacing, 0f) * (size / (float)quality);
				num9++;
			}
			else if (hyphenedText[l] == '\t')
			{
				pos += new Vector3((float)quality * 0.5f * tabSize, 0f, 0f) * (info[l].size / (float)quality);
			}
			else
			{
				pos += info[l].Advance(characterSpacing, quality);
			}
			lowestVert = Mathf.Min(lowestVert, pos.y);
			rightestVert = Mathf.Max(rightestVert, info[l].BottomRightVert.x);
		}
		_lineCount = num9 + 1;
	}

	private void RecalculateBounds()
	{
		topLeftBounds = rawTopLeftBounds;
		topRightBounds = new Vector3(rawBottomRightBounds.x, rawTopLeftBounds.y, rawTopLeftBounds.z);
		bottomLeftBounds = new Vector3(rawTopLeftBounds.x, rawBottomRightBounds.y, rawBottomRightBounds.z);
		bottomRightBounds = rawBottomRightBounds;
		Quaternion rotation = t.rotation;
		topLeftBounds = rotation * topLeftBounds;
		topRightBounds = rotation * topRightBounds;
		bottomLeftBounds = rotation * bottomLeftBounds;
		bottomRightBounds = rotation * bottomRightBounds;
		Vector3 lossyScale = t.lossyScale;
		topLeftBounds.Scale(lossyScale);
		topRightBounds.Scale(lossyScale);
		bottomLeftBounds.Scale(lossyScale);
		bottomRightBounds.Scale(lossyScale);
		Vector3 position = t.position;
		topLeftBounds = position - topLeftBounds;
		topRightBounds = position - topRightBounds;
		bottomLeftBounds = position - bottomLeftBounds;
		bottomRightBounds = position - bottomRightBounds;
		centerBounds = Vector3.Lerp(topLeftBounds, bottomRightBounds, 0.5f);
	}

	private void ApplyAlignment()
	{
		float num = 0f;
		for (int i = 0; i < info.Count; i++)
		{
			float x = info[i].TopRightVert.x;
			if (x > num)
			{
				num = x;
			}
		}
		_width = num;
		Vector3 zero = Vector3.zero;
		switch (alignment)
		{
		case Alignment.BotLeft:
			zero += new Vector3(0f, lowestVert, 0f);
			break;
		case Alignment.BotCenter:
			zero += new Vector3(num * 0.5f, lowestVert, 0f);
			break;
		case Alignment.BotRight:
			zero += new Vector3(num, lowestVert, 0f);
			break;
		case Alignment.MidLeft:
			zero += new Vector3(0f, lowestVert * 0.5f, 0f);
			break;
		case Alignment.MidCenter:
			zero += new Vector3(num * 0.5f, lowestVert * 0.5f, 0f);
			break;
		case Alignment.MidRight:
			zero += new Vector3(num, lowestVert * 0.5f, 0f);
			break;
		case Alignment.TopLeft:
			zero += new Vector3(0f, 0f, 0f);
			break;
		case Alignment.TopCenter:
			zero += new Vector3(num * 0.5f, 0f, 0f);
			break;
		case Alignment.TopRight:
			zero += new Vector3(num, 0f, 0f);
			break;
		}
		int j = 0;
		for (int count = info.Count; j < count; j++)
		{
			info[j].pos -= zero;
		}
		rawTopLeftBounds = new Vector3(zero.x, zero.y - size, zero.z);
		rawBottomRightBounds = new Vector3((AutoWrap > 0f) ? (zero.x - AutoWrap) : (zero.x - rightestVert), zero.y - lowestVert, zero.z);
	}

	private void SetMesh()
	{
		if (textMesh == null)
		{
			textMesh = new Mesh();
			textMesh.MarkDynamic();
		}
		textMesh.Clear();
		if (text.Length > 0)
		{
			endVerts.Clear();
			endUv.Clear();
			endCol32.Clear();
			tris.Clear();
			int num = hyphenedText.Length * 4;
			int num2 = hyphenedText.Length * 6;
			if (outline)
			{
				num *= 5;
				num2 *= 5;
			}
			if (endVerts.Capacity < num)
			{
				endVerts.Capacity = num;
			}
			if (endUv.Capacity < num)
			{
				endUv.Capacity = num;
			}
			if (endCol32.Capacity < num)
			{
				endCol32.Capacity = num;
			}
			if (tris.Capacity < num2)
			{
				tris.Capacity = num2;
			}
			int i = 0;
			for (int length = hyphenedText.Length; i < length; i++)
			{
				STMCharInfo sTMCharInfo = info[i];
				endVerts.Add(sTMCharInfo.TopLeftVert);
				endVerts.Add(sTMCharInfo.TopRightVert);
				endVerts.Add(sTMCharInfo.BottomRightVert);
				endVerts.Add(sTMCharInfo.BottomLeftVert);
				endUv.Add(sTMCharInfo.ch.uvTopLeft);
				endUv.Add(sTMCharInfo.ch.uvTopRight);
				endUv.Add(sTMCharInfo.ch.uvBottomRight);
				endUv.Add(sTMCharInfo.ch.uvBottomLeft);
				endCol32.Add(color);
				endCol32.Add(color);
				endCol32.Add(color);
				endCol32.Add(color);
			}
			ModityMesh(endVerts, endUv, endCol32);
			for (int j = 0; j < endVerts.Count; j++)
			{
				Vector3 vector = endVerts[j];
				endVerts[j] = new Vector3(vector.x + vector.y * skewX, vector.y + vector.x * skewY, vector.z);
			}
			for (int k = 0; k < endVerts.Count / 4; k++)
			{
				tris.Add(4 * k);
				tris.Add(4 * k + 1);
				tris.Add(4 * k + 2);
				tris.Add(4 * k);
				tris.Add(4 * k + 2);
				tris.Add(4 * k + 3);
			}
			textMesh.SetVertices(endVerts);
			textMesh.SetUVs(0, endUv);
			textMesh.SetColors(endCol32);
			textMesh.subMeshCount = 1;
			textMesh.SetTriangles(tris, 0);
		}
		f.sharedMesh = textMesh;
	}

	private void ApplyMaterial()
	{
		Material material = FontUpdateTracker.GetMaterial(this);
		if (material != null)
		{
			material.SetTexture(mainTexId, font.material.mainTexture);
			alpha = Mathf.Clamp(alpha, 0f, 1f);
			material.SetFloat(mixAlphaID, alpha);
			r.sharedMaterial = material;
		}
	}

	public void OnUpdate(float alpha)
	{
		if (matBlock == null)
		{
			matBlock = new MaterialPropertyBlock();
		}
		matBlock.SetFloat(keyAlphaID, alpha);
		r.SetPropertyBlock(matBlock);
	}

	private void ModityMesh(List<Vector3> verts, List<Vector2> uvs, List<Color32> colors)
	{
		if (gradient)
		{
			int count = verts.Count;
			float num = verts[0].y;
			float num2 = verts[0].y;
			for (int num3 = count - 1; num3 >= 1; num3--)
			{
				float y = verts[num3].y;
				if (y > num2)
				{
					num2 = y;
				}
				else if (y < num)
				{
					num = y;
				}
			}
			float num4 = 1f / (num2 - num);
			for (int i = 0; i < count; i++)
			{
				colors[i] *= effectGradient.Evaluate((verts[i].y - num) * num4);
			}
		}
		if (outline)
		{
			int start = 0;
			int count2 = verts.Count;
			ApplyShadow(verts, uvs, colors, outlineColor, start, verts.Count, outlineDistance.x, outlineDistance.y);
			start = count2;
			int count3 = verts.Count;
			ApplyShadow(verts, uvs, colors, outlineColor, start, verts.Count, outlineDistance.x, 0f - outlineDistance.y);
			start = count3;
			int count4 = verts.Count;
			ApplyShadow(verts, uvs, colors, outlineColor, start, verts.Count, 0f - outlineDistance.x, outlineDistance.y);
			start = count4;
			_ = verts.Count;
			ApplyShadow(verts, uvs, colors, outlineColor, start, verts.Count, 0f - outlineDistance.x, 0f - outlineDistance.y);
		}
	}

	private void ApplyShadow(List<Vector3> verts, List<Vector2> uvs, List<Color32> colors, Color32 c, int start, int end, float x, float y)
	{
		for (int i = start; i < end; i++)
		{
			Vector3 vector = verts[i];
			verts.Add(vector);
			colors.Add(colors[i]);
			uvs.Add(uvs[i]);
			vector.x += x;
			vector.y += y;
			colors[i] = c;
			verts[i] = vector;
		}
	}

	public float GetHeight()
	{
		return lowestVert;
	}

	public float GetWidth()
	{
		return _width;
	}

	public void SetColorAlpha(float a)
	{
		if (matBlock == null)
		{
			matBlock = new MaterialPropertyBlock();
		}
		matBlock.SetFloat(keyAlphaID, a);
		r.SetPropertyBlock(matBlock);
	}

	public UGUITextLines[] PopulateWithErrors(string finalTxt)
	{
		return new UGUITextLines[1]
		{
			new UGUITextLines
			{
				startIndex = 0,
				endIndex = finalTxt.Length - 1
			}
		};
	}

	private static bool IsRtl(string str)
	{
		bool result = false;
		if (IsArabicLanguage)
		{
			foreach (char c in str)
			{
				if ((c >= '\u0600' && c <= 'ۿ') || (c >= 'ﹰ' && c <= '\ufeff'))
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}
}
