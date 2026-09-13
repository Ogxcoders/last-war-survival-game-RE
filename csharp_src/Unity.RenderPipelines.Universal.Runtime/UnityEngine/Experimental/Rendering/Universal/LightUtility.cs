using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Experimental.Rendering.Universal.LibTessDotNet;

namespace UnityEngine.Experimental.Rendering.Universal;

internal static class LightUtility
{
	public static bool CheckForChange(int a, ref int b)
	{
		bool result = a != b;
		b = a;
		return result;
	}

	public static bool CheckForChange(float a, ref float b)
	{
		bool result = a != b;
		b = a;
		return result;
	}

	public static bool CheckForChange(bool a, ref bool b)
	{
		bool result = a != b;
		b = a;
		return result;
	}

	public static bool CheckForChange(Vector2 a, ref Vector2 b)
	{
		bool result = a != b;
		b = a;
		return result;
	}

	public static bool CheckForChange(Sprite a, ref Sprite b)
	{
		bool result = !object.Equals(a, b);
		b = a;
		return result;
	}

	public static Bounds CalculateBoundingSphere(ref Vector3[] vertices, ref Color[] colors, float falloffDistance)
	{
		Bounds result = default(Bounds);
		Vector3 min = new Vector3(float.MaxValue, float.MaxValue);
		Vector3 max = new Vector3(float.MinValue, float.MinValue);
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vector = vertices[i];
			vector.x += falloffDistance * colors[i].r;
			vector.y += falloffDistance * colors[i].g;
			min.x = ((vector.x < min.x) ? vector.x : min.x);
			min.y = ((vector.y < min.y) ? vector.y : min.y);
			max.x = ((vector.x > max.x) ? vector.x : max.x);
			max.y = ((vector.y > max.y) ? vector.y : max.y);
		}
		result.max = max;
		result.min = min;
		return result;
	}

	public static Bounds GenerateParametricMesh(ref Mesh mesh, float radius, float falloffDistance, float angle, int sides)
	{
		if (mesh == null)
		{
			mesh = new Mesh();
		}
		float num = MathF.PI / 2f + MathF.PI / 180f * angle;
		if (sides < 3)
		{
			radius = 0.70710677f * radius;
			sides = 4;
		}
		if (sides == 4)
		{
			num = MathF.PI / 4f + MathF.PI / 180f * angle;
		}
		Vector3[] vertices = new Vector3[1 + 2 * sides];
		Color[] colors = new Color[1 + 2 * sides];
		int[] array = new int[9 * sides];
		int num2 = 2 * sides;
		Color color = new Color(0f, 0f, 0f, 1f);
		vertices[num2] = Vector3.zero;
		colors[num2] = color;
		float num3 = MathF.PI * 2f / (float)sides;
		for (int i = 0; i < sides; i++)
		{
			float num4 = (float)(i + 1) * num3;
			Vector3 vector = new Vector3(Mathf.Cos(num4 + num), Mathf.Sin(num4 + num), 0f);
			Vector3 vector2 = radius * vector;
			int num5 = (2 * i + 2) % (2 * sides);
			vertices[num5] = vector2;
			vertices[num5 + 1] = vector2;
			colors[num5] = new Color(vector.x, vector.y, 0f, 0f);
			colors[num5 + 1] = color;
			int num6 = 9 * i;
			array[num6] = num5 + 1;
			array[num6 + 1] = 2 * i + 1;
			array[num6 + 2] = num2;
			array[num6 + 3] = num5;
			array[num6 + 4] = 2 * i;
			array[num6 + 5] = 2 * i + 1;
			array[num6 + 6] = num5 + 1;
			array[num6 + 7] = num5;
			array[num6 + 8] = 2 * i + 1;
		}
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.colors = colors;
		mesh.triangles = array;
		return CalculateBoundingSphere(ref vertices, ref colors, falloffDistance);
	}

	public static Bounds GenerateSpriteMesh(ref Mesh mesh, Sprite sprite, float scale)
	{
		if (mesh == null)
		{
			mesh = new Mesh();
		}
		if (sprite != null)
		{
			Vector2[] vertices = sprite.vertices;
			Vector3[] vertices2 = new Vector3[vertices.Length];
			Color[] colors = new Color[vertices.Length];
			_ = new Vector4[vertices.Length];
			ushort[] triangles = sprite.triangles;
			int[] array = new int[triangles.Length];
			Vector3 vector = 0.5f * scale * (sprite.bounds.min + sprite.bounds.max);
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector3 vector2 = new Vector3(vertices[i].x, vertices[i].y) - vector;
				vertices2[i] = scale * vector2;
				colors[i] = new Color(0f, 0f, 0f, 1f);
			}
			for (int j = 0; j < triangles.Length; j++)
			{
				array[j] = triangles[j];
			}
			mesh.Clear();
			mesh.vertices = vertices2;
			mesh.uv = sprite.uv;
			mesh.triangles = array;
			mesh.colors = colors;
			return CalculateBoundingSphere(ref vertices2, ref colors, 0f);
		}
		return new Bounds(Vector3.zero, Vector3.zero);
	}

	private static void GetFalloffExtrusion(ContourVertex[] contourPoints, int contourPointCount, ref List<Vector2> extrusionDir)
	{
		for (int i = 0; i < contourPointCount; i++)
		{
			int num = ((i == 0) ? (contourPointCount - 1) : (i - 1));
			int num2 = (i + 1) % contourPointCount;
			Vector2 vector = new Vector2(contourPoints[num].Position.X, contourPoints[num].Position.Y);
			Vector2 vector2 = new Vector2(contourPoints[i].Position.X, contourPoints[i].Position.Y);
			Vector2 vector3 = new Vector2(contourPoints[num2].Position.X, contourPoints[num2].Position.Y);
			Vector2 vector4 = vector2 - vector;
			Vector2 vector5 = vector3 - vector2;
			if (!(vector4.magnitude < 0.001f) && !(vector5.magnitude < 0.001f))
			{
				Vector2 normalized = vector4.normalized;
				Vector2 normalized2 = vector5.normalized;
				normalized = new Vector2(0f - normalized.y, normalized.x);
				normalized2 = new Vector2(0f - normalized2.y, normalized2.x);
				Vector2 vector6 = normalized.normalized + normalized2.normalized;
				Vector2 vector7 = -vector6.normalized;
				if (vector6.magnitude > 0f && vector7.magnitude > 0f)
				{
					Vector2 item = new Vector2(vector7.x, vector7.y);
					extrusionDir.Add(item);
				}
			}
		}
	}

	private static object InterpCustomVertexData(Vec3 position, object[] data, float[] weights)
	{
		return data[0];
	}

	public static void GetFalloffShape(Vector3[] shapePath, ref List<Vector2> extrusionDir)
	{
		int num = shapePath.Length;
		ContourVertex[] array = new ContourVertex[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = new ContourVertex
			{
				Position = new Vec3
				{
					X = shapePath[i].x,
					Y = shapePath[i].y
				},
				Data = null
			};
		}
		GetFalloffExtrusion(array, num, ref extrusionDir);
	}

	public static Bounds GenerateShapeMesh(ref Mesh mesh, Vector3[] shapePath, float falloffDistance)
	{
		Color color = new Color(0f, 0f, 0f, 1f);
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		List<Color> list3 = new List<Color>();
		int num = shapePath.Length;
		ContourVertex[] array = new ContourVertex[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = new ContourVertex
			{
				Position = new Vec3
				{
					X = shapePath[i].x,
					Y = shapePath[i].y
				},
				Data = color
			};
		}
		Tess tess = new Tess();
		tess.AddContour(array, ContourOrientation.Original);
		tess.Tessellate(WindingRule.EvenOdd, ElementType.Polygons, 3, InterpCustomVertexData);
		int[] collection = tess.Elements.Select((int result) => result).ToArray();
		Vector3[] collection2 = tess.Vertices.Select((ContourVertex v) => new Vector3(v.Position.X, v.Position.Y, 0f)).ToArray();
		Color[] collection3 = tess.Vertices.Select((ContourVertex v) => new Color(((Color)v.Data).r, ((Color)v.Data).g, ((Color)v.Data).b, ((Color)v.Data).a)).ToArray();
		list.AddRange(collection2);
		list2.AddRange(collection);
		list3.AddRange(collection3);
		List<Vector2> extrusionDir = new List<Vector2>();
		GetFalloffShape(shapePath, ref extrusionDir);
		num = list.Count;
		int num2 = 2 * shapePath.Length;
		for (int num3 = 0; num3 < shapePath.Length; num3++)
		{
			int num4 = 2 * num3;
			int item = num + num4;
			int item2 = num + num4 + 1;
			int item3 = num + (num4 + 2) % num2;
			int item4 = num + (num4 + 3) % num2;
			Vector3 item5 = shapePath[num3];
			list.Add(item5);
			list.Add(item5);
			list2.Add(item);
			list2.Add(item2);
			list2.Add(item4);
			list2.Add(item4);
			list2.Add(item3);
			list2.Add(item);
			Color item6 = new Color(0f, 0f, 0f, 1f);
			Color item7 = new Color(extrusionDir[num3].x, extrusionDir[num3].y, 0f, 0f);
			list3.Add(item6);
			list3.Add(item7);
		}
		Color[] colors = list3.ToArray();
		Vector3[] vertices = list.ToArray();
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.colors = colors;
		mesh.SetIndices(list2.ToArray(), MeshTopology.Triangles, 0);
		return CalculateBoundingSphere(ref vertices, ref colors, falloffDistance);
	}

	public static void AddShadowCasterGroupToList(ShadowCasterGroup2D shadowCaster, List<ShadowCasterGroup2D> list)
	{
		int num = 0;
		for (num = 0; num < list.Count && shadowCaster.GetShadowGroup() != list[num].GetShadowGroup(); num++)
		{
		}
		list.Insert(num, shadowCaster);
	}

	public static void RemoveShadowCasterGroupFromList(ShadowCasterGroup2D shadowCaster, List<ShadowCasterGroup2D> list)
	{
		list.Remove(shadowCaster);
	}

	private static CompositeShadowCaster2D FindTopMostCompositeShadowCaster(ShadowCaster2D shadowCaster)
	{
		CompositeShadowCaster2D result = null;
		Transform parent = shadowCaster.transform.parent;
		while (parent != null)
		{
			CompositeShadowCaster2D component = parent.GetComponent<CompositeShadowCaster2D>();
			if (component != null)
			{
				result = component;
			}
			parent = parent.parent;
		}
		return result;
	}

	public static bool AddToShadowCasterGroup(ShadowCaster2D shadowCaster, ref ShadowCasterGroup2D shadowCasterGroup)
	{
		ShadowCasterGroup2D shadowCasterGroup2D = FindTopMostCompositeShadowCaster(shadowCaster);
		if (shadowCasterGroup2D == null)
		{
			shadowCasterGroup2D = shadowCaster.GetComponent<ShadowCaster2D>();
		}
		if (shadowCasterGroup2D != null && shadowCasterGroup != shadowCasterGroup2D)
		{
			shadowCasterGroup2D.RegisterShadowCaster2D(shadowCaster);
			shadowCasterGroup = shadowCasterGroup2D;
			return true;
		}
		return false;
	}

	public static void RemoveFromShadowCasterGroup(ShadowCaster2D shadowCaster, ShadowCasterGroup2D shadowCasterGroup)
	{
		if (shadowCasterGroup != null)
		{
			shadowCasterGroup.UnregisterShadowCaster2D(shadowCaster);
		}
	}
}
