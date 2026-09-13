using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : MaskableGraphic
{
	[SerializeField]
	private Texture m_Texture;

	[SerializeField]
	private Rect m_UVRect = new Rect(0f, 0f, 1f, 1f);

	public float LineThickness = 2f;

	public bool UseMargins;

	public Vector2 Margin;

	public Vector2[] Points;

	public bool relativeSize;

	public override Texture mainTexture
	{
		get
		{
			if (!(m_Texture == null))
			{
				return m_Texture;
			}
			return Graphic.s_WhiteTexture;
		}
	}

	public Texture texture
	{
		get
		{
			return m_Texture;
		}
		set
		{
			if (!(m_Texture == value))
			{
				m_Texture = value;
				SetVerticesDirty();
				SetMaterialDirty();
			}
		}
	}

	public Rect uvRect
	{
		get
		{
			return m_UVRect;
		}
		set
		{
			if (!(m_UVRect == value))
			{
				m_UVRect = value;
				SetVerticesDirty();
			}
		}
	}

	protected override void OnPopulateMesh(Mesh toFill)
	{
		if (Points == null || Points.Length < 2)
		{
			Points = new Vector2[2]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 0f)
			};
		}
		int num = 24;
		float num2 = base.rectTransform.rect.width;
		float num3 = base.rectTransform.rect.height;
		float num4 = (0f - base.rectTransform.pivot.x) * base.rectTransform.rect.width;
		float num5 = (0f - base.rectTransform.pivot.y) * base.rectTransform.rect.height;
		if (!relativeSize)
		{
			num2 = 1f;
			num3 = 1f;
		}
		List<Vector2> list = new List<Vector2>();
		list.Add(Points[0]);
		Vector2 item = Points[0] + (Points[1] - Points[0]).normalized * num;
		list.Add(item);
		for (int i = 1; i < Points.Length - 1; i++)
		{
			list.Add(Points[i]);
		}
		item = Points[Points.Length - 1] - (Points[Points.Length - 1] - Points[Points.Length - 2]).normalized * num;
		list.Add(item);
		list.Add(Points[Points.Length - 1]);
		Vector2[] array = list.ToArray();
		if (UseMargins)
		{
			num2 -= Margin.x;
			num3 -= Margin.y;
			num4 += Margin.x / 2f;
			num5 += Margin.y / 2f;
		}
		toFill.Clear();
		VertexHelper vertexHelper = new VertexHelper(toFill);
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = Vector2.zero;
		for (int j = 1; j < array.Length; j++)
		{
			Vector2 vector3 = array[j - 1];
			Vector2 vector4 = array[j];
			vector3 = new Vector2(vector3.x * num2 + num4, vector3.y * num3 + num5);
			vector4 = new Vector2(vector4.x * num2 + num4, vector4.y * num3 + num5);
			float z = Mathf.Atan2(vector4.y - vector3.y, vector4.x - vector3.x) * 180f / MathF.PI;
			Vector2 vector5 = vector3 + new Vector2(0f, (0f - LineThickness) / 2f);
			Vector2 vector6 = vector3 + new Vector2(0f, LineThickness / 2f);
			Vector2 vector7 = vector4 + new Vector2(0f, LineThickness / 2f);
			Vector2 vector8 = vector4 + new Vector2(0f, (0f - LineThickness) / 2f);
			vector5 = RotatePointAroundPivot(vector5, vector3, new Vector3(0f, 0f, z));
			vector6 = RotatePointAroundPivot(vector6, vector3, new Vector3(0f, 0f, z));
			vector7 = RotatePointAroundPivot(vector7, vector4, new Vector3(0f, 0f, z));
			vector8 = RotatePointAroundPivot(vector8, vector4, new Vector3(0f, 0f, z));
			Vector2 zero = Vector2.zero;
			Vector2 vector9 = new Vector2(0f, 1f);
			Vector2 vector10 = new Vector2(0.5f, 0f);
			Vector2 vector11 = new Vector2(0.5f, 1f);
			Vector2 vector12 = new Vector2(1f, 0f);
			Vector2 vector13 = new Vector2(1f, 1f);
			Vector2[] uvs = new Vector2[4] { vector10, vector11, vector11, vector10 };
			if (j > 1)
			{
				SetVbo(vertexHelper, new Vector2[4] { vector, vector2, vector5, vector6 }, uvs);
			}
			if (j == 1)
			{
				uvs = new Vector2[4] { zero, vector9, vector11, vector10 };
			}
			else if (j == array.Length - 1)
			{
				uvs = new Vector2[4] { vector10, vector11, vector13, vector12 };
			}
			vertexHelper.AddUIVertexQuad(SetVbo(vertexHelper, new Vector2[4] { vector5, vector6, vector7, vector8 }, uvs));
			vertexHelper.FillMesh(toFill);
			vector = vector7;
			vector2 = vector8;
		}
	}

	protected UIVertex[] SetVbo(VertexHelper vbo, Vector2[] vertices, Vector2[] uvs)
	{
		UIVertex[] array = new UIVertex[4];
		for (int i = 0; i < vertices.Length; i++)
		{
			UIVertex simpleVert = UIVertex.simpleVert;
			simpleVert.color = color;
			simpleVert.position = vertices[i];
			simpleVert.uv0 = uvs[i];
			array[i] = simpleVert;
		}
		return array;
	}

	public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		Vector3 vector = point - pivot;
		vector = Quaternion.Euler(angles) * vector;
		point = vector + pivot;
		return point;
	}
}
