using System.Collections.Generic;
using UnityEngine;

namespace Box2DSharp.Testbed.Unity.Inspection;

[ExecuteInEditMode]
public class UnityDraw : MonoBehaviour
{
	private const string SceneCameraName = "SceneCamera";

	private const string MainCameraTag = "MainCamera";

	private bool withTransform;

	private bool useCustomColor;

	private Color customColor;

	private Vector3 scalePhysicsToGraphics = Vector3.one;

	private static Material _lineMaterial;

	private static UnityDraw ms_instance;

	private readonly List<Color> _colors = new List<Color>();

	private readonly List<List<(Vector3 begin, Vector3 end)>> _lines = new List<List<(Vector3, Vector3)>>();

	private readonly List<(Vector3 Center, float Radius, Color color)> _points = new List<(Vector3, float, Color)>();

	private static readonly int _srcBlend = Shader.PropertyToID("_SrcBlend");

	private static readonly int _dstBlend = Shader.PropertyToID("_DstBlend");

	private static readonly int _cull = Shader.PropertyToID("_Cull");

	private static readonly int _zWrite = Shader.PropertyToID("_ZWrite");

	public static UnityDraw GetDraw()
	{
		if (ms_instance == null)
		{
			ms_instance = Camera.main.gameObject.AddComponent<UnityDraw>();
			ms_instance.withTransform = false;
		}
		return ms_instance;
	}

	public static UnityDraw GetDraw(GameObject gameObject, float scale, bool useCustomColor = false, Color color = default(Color))
	{
		if (!gameObject.TryGetComponent<UnityDraw>(out var component))
		{
			component = gameObject.AddComponent<UnityDraw>();
		}
		component.withTransform = true;
		component.scalePhysicsToGraphics = new Vector3(scale, scale, scale);
		component.useCustomColor = useCustomColor;
		component.customColor = color;
		return component;
	}

	private void OnRenderObject()
	{
		if (Camera.current == null || Camera.current.name != "SceneCamera")
		{
			return;
		}
		CreateLineMaterial();
		GL.PushMatrix();
		if (withTransform)
		{
			GL.MultMatrix(base.transform.localToWorldMatrix * Matrix4x4.Scale(scalePhysicsToGraphics));
		}
		_lineMaterial.SetPass(0);
		GL.Begin(1);
		for (int i = 0; i < _lines.Count; i++)
		{
			GL.Color(useCustomColor ? customColor : _colors[i]);
			foreach (var item in _lines[i])
			{
				GL.Vertex(item.begin);
				GL.Vertex(item.end);
			}
		}
		GL.End();
		GL.Begin(7);
		for (int j = 0; j < _points.Count; j++)
		{
			(Vector3, float, Color) tuple = _points[j];
			GL.Color(useCustomColor ? customColor : tuple.Item3);
			GL.Vertex(new Vector2(tuple.Item1.x - tuple.Item2, tuple.Item1.y - tuple.Item2));
			GL.Vertex(new Vector2(tuple.Item1.x - tuple.Item2, tuple.Item1.y + tuple.Item2));
			GL.Vertex(new Vector2(tuple.Item1.x + tuple.Item2, tuple.Item1.y + tuple.Item2));
			GL.Vertex(new Vector2(tuple.Item1.x + tuple.Item2, tuple.Item1.y - tuple.Item2));
		}
		GL.End();
		GL.PopMatrix();
	}

	public void PostLines(List<(Vector3 begin, Vector3 end)> lines, Color color)
	{
		_lines.Add(lines);
		_colors.Add(color);
	}

	public void PostPoint((Vector3 Center, float Radius, Color color) point)
	{
		_points.Add(point);
	}

	private void Update()
	{
		_lines.Clear();
		_colors.Clear();
		_points.Clear();
	}

	private static void CreateLineMaterial()
	{
		if (!_lineMaterial)
		{
			_lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"))
			{
				hideFlags = HideFlags.HideAndDontSave
			};
			_lineMaterial.SetInt(_srcBlend, 5);
			_lineMaterial.SetInt(_dstBlend, 10);
			_lineMaterial.SetInt(_cull, 0);
			_lineMaterial.SetInt(_zWrite, 0);
		}
	}
}
