using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Radar Image")]
public class RadarImage : BaseImage
{
	private List<Vector3> _vertices;

	public int demensions;

	public List<float> values;

	protected override void Awake()
	{
		base.Awake();
		_vertices = new List<Vector3>();
	}

	public void SetValues(List<float> value)
	{
		values = value;
		SetVerticesDirty();
	}

	public void SetDemensions(int demension)
	{
		demensions = demension;
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
		_vertices.Clear();
		if (values.Count >= demensions)
		{
			float num = MathF.PI * 2f / (float)demensions;
			int num2 = demensions;
			float width = base.rectTransform.rect.width;
			float num3 = base.rectTransform.pivot.x * width;
			float num4 = 0f;
			Vector2 zero = Vector2.zero;
			int num5 = num2 + 1;
			vh.AddVert(new UIVertex
			{
				color = color,
				position = zero,
				uv0 = Vector2.zero
			});
			for (int i = 1; i < num5; i++)
			{
				float num6 = values[i - 1];
				float num7 = Mathf.Cos(num4);
				float num8 = Mathf.Sin(num4);
				zero = new Vector2(num7 * num6 * num3, num8 * num6 * num3);
				num4 += num;
				vh.AddVert(new UIVertex
				{
					color = color,
					position = zero,
					uv0 = Vector2.zero
				});
				_vertices.Add(zero);
			}
			int num9 = num2 * 3;
			int num10 = 0;
			int num11 = 1;
			while (num10 < num9 - 3)
			{
				vh.AddTriangle(num11, 0, num11 + 1);
				num10 += 3;
				num11++;
			}
			vh.AddTriangle(num5 - 1, 0, 1);
		}
	}
}
