using System;
using System.Collections.Generic;
using UnityEngine.Sprites;

namespace UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class ArabicImageMirror : BaseMeshEffect
{
	public enum MirrorType
	{
		Horizontal
	}

	[SerializeField]
	private MirrorType m_MirrorType;

	[NonSerialized]
	private RectTransform m_RectTransform;

	public MirrorType mirrorType
	{
		get
		{
			return m_MirrorType;
		}
		set
		{
			if (m_MirrorType != value)
			{
				m_MirrorType = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}
	}

	public RectTransform rectTransform => m_RectTransform ?? (m_RectTransform = GetComponent<RectTransform>());

	public override void ModifyMesh(VertexHelper vh)
	{
		if (!IsActive())
		{
			return;
		}
		List<UIVertex> list = new List<UIVertex>();
		vh.GetUIVertexStream(list);
		int count = list.Count;
		if (base.graphic is Image)
		{
			switch ((base.graphic as Image).type)
			{
			case Image.Type.Simple:
				DrawSimple(list, count);
				break;
			case Image.Type.Sliced:
				DrawSliced(list, count);
				break;
			case Image.Type.Tiled:
				DrawTiled(list, count);
				break;
			case Image.Type.Filled:
				DrawSimple(list, count);
				break;
			}
		}
		else
		{
			DrawSimple(list, count);
		}
		vh.Clear();
		vh.AddUIVertexTriangleStream(list);
	}

	protected void DrawSimple(List<UIVertex> output, int count)
	{
		Rect pixelAdjustedRect = base.graphic.GetPixelAdjustedRect();
		if (m_MirrorType == MirrorType.Horizontal)
		{
			MirrorVerts(pixelAdjustedRect, output, count);
		}
	}

	protected void DrawSliced(List<UIVertex> output, int count)
	{
		if (!(base.graphic as Image).hasBorder)
		{
			DrawSimple(output, count);
			return;
		}
		Rect pixelAdjustedRect = base.graphic.GetPixelAdjustedRect();
		count = SliceExcludeVerts(output, count);
		if (m_MirrorType == MirrorType.Horizontal)
		{
			MirrorVerts(pixelAdjustedRect, output, count);
		}
	}

	protected void DrawTiled(List<UIVertex> verts, int count)
	{
		Sprite overrideSprite = (base.graphic as Image).overrideSprite;
		if (overrideSprite == null)
		{
			return;
		}
		Rect pixelAdjustedRect = base.graphic.GetPixelAdjustedRect();
		Vector4 innerUV = DataUtility.GetInnerUV(overrideSprite);
		float num = overrideSprite.rect.width / (base.graphic as Image).pixelsPerUnit;
		_ = overrideSprite.rect.height / (base.graphic as Image).pixelsPerUnit;
		int num2 = count / 3;
		for (int i = 0; i < num2; i++)
		{
			UIVertex value = verts[i * 3];
			UIVertex value2 = verts[i * 3 + 1];
			UIVertex value3 = verts[i * 3 + 2];
			float center = GetCenter(value.position.x, value2.position.x, value3.position.x);
			GetCenter(value.position.y, value2.position.y, value3.position.y);
			if (m_MirrorType == MirrorType.Horizontal && Mathf.FloorToInt((center - pixelAdjustedRect.xMin) / num) % 2 == 1)
			{
				value.uv0 = GetOverturnUV(value.uv0, innerUV.x, innerUV.z);
				value2.uv0 = GetOverturnUV(value2.uv0, innerUV.x, innerUV.z);
				value3.uv0 = GetOverturnUV(value3.uv0, innerUV.x, innerUV.z);
			}
			verts[i * 3] = value;
			verts[i * 3 + 1] = value2;
			verts[i * 3 + 2] = value3;
		}
	}

	protected void SimpleScale(Rect rect, List<UIVertex> verts, int count)
	{
		for (int i = 0; i < count; i++)
		{
			UIVertex value = verts[i];
			Vector3 position = value.position;
			if (m_MirrorType == MirrorType.Horizontal)
			{
				position.x = (position.x + rect.x) * 0.5f;
			}
			value.position = position;
			verts[i] = value;
		}
	}

	protected void SlicedScale(Rect rect, List<UIVertex> verts, int count)
	{
		Vector4 adjustedBorders = GetAdjustedBorders(rect);
		float num = rect.width * 0.5f;
		_ = rect.height;
		for (int i = 0; i < count; i++)
		{
			UIVertex value = verts[i];
			Vector3 position = value.position;
			if (m_MirrorType == MirrorType.Horizontal)
			{
				if (num < adjustedBorders.x && position.x >= rect.center.x)
				{
					position.x = rect.center.x;
				}
				else if (position.x >= adjustedBorders.x)
				{
					position.x = (position.x + rect.x) * 0.5f;
				}
			}
			value.position = position;
			verts[i] = value;
		}
	}

	protected void MirrorVerts(Rect rect, List<UIVertex> verts, int count, bool isHorizontal = true)
	{
		List<UIVertex> list = new List<UIVertex>();
		for (int i = 0; i < count; i++)
		{
			UIVertex item = verts[i];
			Vector3 position = item.position;
			position.x = rect.center.x * 2f - position.x;
			item.position = position;
			list.Add(item);
		}
		verts.Clear();
		verts.AddRange(list);
	}

	protected int SliceExcludeVerts(List<UIVertex> verts, int count)
	{
		int num = count;
		int num2 = 0;
		while (num2 < num)
		{
			UIVertex uIVertex = verts[num2];
			UIVertex uIVertex2 = verts[num2 + 1];
			UIVertex uIVertex3 = verts[num2 + 2];
			if (uIVertex.position == uIVertex2.position || uIVertex2.position == uIVertex3.position || uIVertex3.position == uIVertex.position)
			{
				verts[num2] = verts[num - 3];
				verts[num2 + 1] = verts[num - 2];
				verts[num2 + 2] = verts[num - 1];
				num -= 3;
			}
			else
			{
				num2 += 3;
			}
		}
		if (num < count)
		{
			verts.RemoveRange(num, count - num);
		}
		return num;
	}

	protected Vector4 GetAdjustedBorders(Rect rect)
	{
		Vector4 border = (base.graphic as Image).overrideSprite.border;
		border /= (base.graphic as Image).pixelsPerUnit;
		for (int i = 0; i <= 1; i++)
		{
			float num = border[i] + border[i + 2];
			if (rect.size[i] < num && num != 0f)
			{
				float num2 = rect.size[i] / num;
				border[i] *= num2;
				border[i + 2] *= num2;
			}
		}
		return border;
	}

	protected float GetCenter(float p1, float p2, float p3)
	{
		float num = Mathf.Max(Mathf.Max(p1, p2), p3);
		float num2 = Mathf.Min(Mathf.Min(p1, p2), p3);
		return (num + num2) / 2f;
	}

	protected Vector2 GetOverturnUV(Vector2 uv, float start, float end, bool isHorizontal = true)
	{
		if (isHorizontal)
		{
			uv.x = end - uv.x + start;
		}
		else
		{
			uv.y = end - uv.y + start;
		}
		return uv;
	}
}
