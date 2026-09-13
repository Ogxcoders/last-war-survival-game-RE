using System;
using UnityEngine.Sprites;

namespace UnityEngine.UI.Extensions;

[ExecuteInEditMode]
[AddComponentMenu("UI/Effects/Extensions/SoftMaskScript")]
public class SoftMaskScript : MonoBehaviour
{
	private Material mat;

	private Canvas cachedCanvas;

	private Transform cachedCanvasTransform;

	private readonly Vector3[] m_WorldCorners = new Vector3[4];

	private readonly Vector3[] m_CanvasCorners = new Vector3[4];

	[Tooltip("The area that is to be used as the container.")]
	public RectTransform MaskArea;

	[Tooltip("Sprite to be used to do the soft alpha (supports Slice mode if border is set).")]
	public Sprite AlphaMaskSprite;

	[Tooltip("How to sample the alpha mask.")]
	public MaskSamplingMode SamplingMode;

	[Tooltip("In Slice mode works for the soft alpha mask.")]
	public float PixelsPerUnitMultiplier = 1f;

	[Tooltip("At what point to apply the alpha min range 0-1")]
	[Range(0f, 1f)]
	public float CutOff;

	[Tooltip("Implement a hard blend based on the Cutoff")]
	public bool HardBlend;

	[Tooltip("Flip the masks alpha value")]
	public bool FlipAlphaMask;

	[Tooltip("If a different Mask Scaling Rect is given, and this value is true, the area around the mask will not be clipped")]
	public bool DontClipMaskScalingRect;

	private Vector2 maskOffset = Vector2.zero;

	private Vector2 maskScale = Vector2.one;

	private void Start()
	{
		if (MaskArea == null)
		{
			MaskArea = GetComponent<RectTransform>();
		}
		Text component = GetComponent<Text>();
		if (component != null)
		{
			mat = new Material(ShaderLibrary.GetShaderInstance("UI Extensions/SoftMaskShader"));
			component.material = mat;
			cachedCanvas = component.canvas;
			cachedCanvasTransform = cachedCanvas.transform;
			if (base.transform.parent.GetComponent<Mask>() == null)
			{
				base.transform.parent.gameObject.AddComponent<Mask>();
			}
			base.transform.parent.GetComponent<Mask>().enabled = false;
		}
		else
		{
			Graphic component2 = GetComponent<Graphic>();
			if (component2 != null)
			{
				mat = new Material(ShaderLibrary.GetShaderInstance("UI Extensions/SoftMaskShader"));
				component2.material = mat;
				cachedCanvas = component2.canvas;
				cachedCanvasTransform = cachedCanvas.transform;
			}
		}
	}

	private void Update()
	{
		if (cachedCanvas != null)
		{
			SetMask();
		}
	}

	private void SetMask()
	{
		Rect canvasRect = GetCanvasRect();
		Vector2 vector = canvasRect.size;
		Vector2 min = canvasRect.min;
		Texture value = (AlphaMaskSprite ? AlphaMaskSprite.texture : null);
		mat.SetTexture("_AlphaMask", value);
		if (AlphaMaskSprite != null)
		{
			Vector4 padding = DataUtility.GetPadding(AlphaMaskSprite);
			Vector4 border = AlphaMaskSprite.border;
			float num = Mathf.Max(PixelsPerUnitMultiplier, float.Epsilon);
			padding /= num;
			border = border / num - padding;
			border.x = Math.Max(border.x, 0f);
			border.y = Math.Max(border.y, 0f);
			border.z = Math.Max(border.z, 0f);
			border.w = Math.Max(border.w, 0f);
			min += new Vector2(padding.x, padding.y);
			vector = new Vector2(Math.Max(0f, vector.x - padding.x - padding.z), Math.Max(0f, vector.y - padding.y - padding.w));
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			if (vector.x > float.Epsilon)
			{
				if (border.x > float.Epsilon)
				{
					zero.x = border.x / vector.x;
				}
				if (border.z > float.Epsilon)
				{
					zero2.x = border.z / vector.x;
				}
			}
			if (vector.y > float.Epsilon)
			{
				if (border.y > float.Epsilon)
				{
					zero.y = border.y / vector.y;
				}
				if (border.w > float.Epsilon)
				{
					zero2.y = border.w / vector.y;
				}
			}
			float num2 = zero.x;
			float num3 = zero2.x;
			if (num2 + num3 > 1f)
			{
				num2 = (num3 = 0.5f);
			}
			float num4 = zero.y;
			float num5 = zero2.y;
			if (num4 + num5 > 1f)
			{
				num4 = (num5 = 0.5f);
			}
			Vector4 value2 = new Vector4(num2, num4, 1f - num3, 1f - num5);
			mat.SetVector("_MaskBorderMinMaxPoints", value2);
			Vector4 outerUV = DataUtility.GetOuterUV(AlphaMaskSprite);
			mat.SetVector("_MaskOuterUV", outerUV);
			Vector4 innerUV = DataUtility.GetInnerUV(AlphaMaskSprite);
			mat.SetVector("_MaskInnerUV", innerUV);
		}
		else
		{
			mat.SetVector("_MaskBorderMinMaxPoints", new Vector4(0f, 0f, 1f, 1f));
			mat.SetVector("_MaskOuterUV", new Vector4(0f, 0f, 1f, 1f));
			mat.SetVector("_MaskInnerUV", new Vector4(0f, 0f, 1f, 1f));
		}
		maskScale.Set(1f / vector.x, 1f / vector.y);
		maskOffset = -min;
		maskOffset.Scale(maskScale);
		mat.SetTextureOffset("_AlphaMask", maskOffset);
		mat.SetTextureScale("_AlphaMask", maskScale);
		mat.SetFloat("_HardBlend", HardBlend ? 1 : 0);
		mat.SetInt("_FlipAlphaMask", FlipAlphaMask ? 1 : 0);
		mat.SetInt("_NoOuterClip", DontClipMaskScalingRect ? 1 : 0);
		mat.SetFloat("_CutOff", CutOff);
		mat.SetInt("_SamplingMode", (int)SamplingMode);
	}

	public Rect GetCanvasRect()
	{
		if (cachedCanvas == null)
		{
			return default(Rect);
		}
		MaskArea.GetWorldCorners(m_WorldCorners);
		for (int i = 0; i < 4; i++)
		{
			m_CanvasCorners[i] = cachedCanvasTransform.InverseTransformPoint(m_WorldCorners[i]);
		}
		return new Rect(m_CanvasCorners[0].x, m_CanvasCorners[0].y, m_CanvasCorners[2].x - m_CanvasCorners[0].x, m_CanvasCorners[2].y - m_CanvasCorners[0].y);
	}

	private void OnDestroy()
	{
		if (mat != null)
		{
			Graphic component = GetComponent<Graphic>();
			if (component != null)
			{
				component.material = null;
			}
			Object.Destroy(mat);
			mat = null;
		}
	}
}
