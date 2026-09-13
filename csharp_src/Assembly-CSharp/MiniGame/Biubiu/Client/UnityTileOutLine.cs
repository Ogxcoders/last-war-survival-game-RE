using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameKit.Base;
using UnityEngine;
using VEngine;

namespace MiniGame.Biubiu.Client;

public class UnityTileOutLine : MonoBehaviour
{
	public float OutLineLenght = 0.2f;

	private CompositeCollider2D CompositeCollider2D;

	private MeshRenderer MeshRenderer;

	private MeshFilter MeshFilter;

	public Mesh Mesh;

	private Asset MaterialAsset;

	public TileOutLineType OutLineType;

	public void SetToGenMesh()
	{
		StartCoroutine(WaitComponentToMesh());
	}

	private IEnumerator WaitComponentToMesh()
	{
		if (CompositeCollider2D == null)
		{
			yield return null;
			CompositeCollider2D = GetComponent<CompositeCollider2D>();
		}
		yield return new WaitForFixedUpdate();
		Debug.Log("[BiuBiu] WaitComponentToMesh Start");
		RefreshMesh();
		Debug.Log("[BiuBiu] WaitComponentToMesh End");
	}

	private void Start()
	{
		Transform transform = base.transform.Find("collider_mesh");
		if (transform == null)
		{
			transform = new GameObject("collider_mesh").transform;
			transform.gameObject.layer = LayerMask.NameToLayer("UIObject3D");
		}
		transform.transform.SetParent(base.transform);
		transform.localPosition = new Vector3(0f, 0f, 30f);
		transform.localRotation = Quaternion.identity;
		transform.localScale = 1f / base.transform.lossyScale.x * Vector3.one;
		CompositeCollider2D = GetComponent<CompositeCollider2D>();
		MeshFilter = transform.gameObject.GetOrAddComponent<MeshFilter>();
		MeshRenderer = transform.gameObject.GetOrAddComponent<MeshRenderer>();
		MaterialAsset = ResourceManager.LoadAssetAsyncStatic("Assets/_Art_LastWar/ArtAssetIncrement/Seasons/S5/NPC/CommonRes/material/ShootingGameScnInnerShadow.mat", typeof(Material));
		Asset materialAsset = MaterialAsset;
		materialAsset.completed = (Action<Asset>)Delegate.Combine(materialAsset.completed, new Action<Asset>(MaterialFinish));
	}

	private void OnDestroy()
	{
		if (Mesh != null)
		{
			UnityEngine.Object.Destroy(Mesh);
			Mesh = null;
		}
		if (MaterialAsset != null)
		{
			if (MaterialAsset.isDone)
			{
				MaterialAsset.Release();
			}
			else
			{
				Asset materialAsset = MaterialAsset;
				materialAsset.completed = (Action<Asset>)Delegate.Remove(materialAsset.completed, new Action<Asset>(MaterialFinish));
				Asset materialAsset2 = MaterialAsset;
				materialAsset2.completed = (Action<Asset>)Delegate.Combine(materialAsset2.completed, new Action<Asset>(Release));
			}
		}
		StopAllCoroutines();
	}

	private void RefreshMesh()
	{
		Mesh = CreateTileWallMesh(CompositeCollider2D);
		MeshFilter.mesh = Mesh;
	}

	private void MaterialFinish(Asset asset)
	{
		if (!asset.isError && asset.asset != null)
		{
			MeshRenderer.sharedMaterial = asset.asset as Material;
		}
	}

	private void Release(Asset asset)
	{
		asset.completed = (Action<Asset>)Delegate.Remove(asset.completed, new Action<Asset>(Release));
		asset.Release();
	}

	private Mesh CreateTileWallMesh(CompositeCollider2D collider2D)
	{
		Mesh mesh = new Mesh();
		List<Vector3> list = new List<Vector3>();
		List<Color> colors = new List<Color>();
		List<int> triangles = new List<int>();
		int pathCount = collider2D.pathCount;
		for (int i = 0; i < pathCount; i++)
		{
			Vector2[] array = new Vector2[collider2D.GetPathPointCount(i)];
			collider2D.GetPath(i, array);
			List<Vector2> list2 = array.ToList();
			List<Vector2> list3 = new List<Vector2>();
			for (int num = list2.Count - 1; num >= 0; num--)
			{
				Vector2 item = list2[num];
				item.x = Mathf.Min(item.x, 3.0375001f);
				item.x = Mathf.Max(item.x, -3.0375001f);
				item.y = Mathf.Min(item.y, 6.75f);
				item.y = Mathf.Max(item.y, -6.75f);
				list3.Add(item);
			}
			list2 = list3;
			List<Vector2> waiPointList = GenerateOffsetPoints(list2, OutLineLenght, isOuter: true);
			List<Vector2> neiPointList = GenerateOffsetPoints(list2, OutLineLenght, isOuter: false);
			GenerateMeshInfo(list, triangles, colors, list2, neiPointList, waiPointList);
		}
		if (list.Count > 0)
		{
			mesh.SetVertices(list);
			mesh.SetTriangles(triangles, 0);
			mesh.SetColors(colors);
			mesh.RecalculateNormals();
		}
		return mesh;
	}

	private List<Vector2> GenerateOffsetPoints(List<Vector2> points, float offset, bool isOuter)
	{
		List<Vector2> list = new List<Vector2>();
		for (int i = 0; i < points.Count; i++)
		{
			Vector2 vector = points[i];
			Vector2 vector2 = points[(i - 1 + points.Count) % points.Count];
			Vector2 vector3 = points[(i + 1) % points.Count];
			Vector2 normalized = (vector - vector2).normalized;
			Vector2 normalized2 = (vector3 - vector).normalized;
			Vector2 normalized3 = (normalized + normalized2).normalized;
			Vector2 vector4 = new Vector2(0f - normalized3.y, normalized3.x);
			if (!isOuter)
			{
				vector4 = -vector4;
			}
			float num = Mathf.Sin(Vector2.Angle(normalized, -normalized2) * (MathF.PI / 180f) / 2f);
			if (num == 0f)
			{
				list.Add(vector);
			}
			else
			{
				list.Add(vector + vector4 * offset / num);
			}
		}
		return list;
	}

	public Vector2 GetFootOfPerpendicular(Vector2 A, Vector2 B, Vector2 C)
	{
		double num = C.x - B.x;
		double num2 = C.y - B.y;
		double num3 = ((double)(A.x - B.x) * num + (double)(A.y - B.y) * num2) / (num * num + num2 * num2);
		double num4 = (double)B.x + num3 * num;
		double num5 = (double)B.y + num3 * num2;
		return new Vector2((float)num4, (float)num5);
	}

	private void GenerateMeshInfo(List<Vector3> vertices, List<int> triangles, List<Color> colors, List<Vector2> points, List<Vector2> neiPointList, List<Vector2> waiPointList)
	{
		List<Vector2> list = new List<Vector2>();
		List<Vector2> list2 = new List<Vector2>();
		List<Vector2> list3 = new List<Vector2>();
		for (int i = 0; i < points.Count; i++)
		{
			int index = (i + 1) % points.Count;
			Vector2 vector = points[i];
			Vector2 a = points[index];
			Vector2 vector2 = waiPointList[i];
			Vector2 vector3 = waiPointList[index];
			Vector2 footOfPerpendicular = GetFootOfPerpendicular(vector, vector2, vector3);
			Vector2 vector4 = vector2 - footOfPerpendicular;
			Vector2 vector5 = vector3 - footOfPerpendicular;
			Vector2 vector6 = vector3 - vector2;
			float magnitude = (vector4 + vector5).magnitude;
			float magnitude2 = vector6.magnitude;
			bool num = magnitude <= magnitude2;
			footOfPerpendicular = (num ? footOfPerpendicular : vector2);
			list.Add(footOfPerpendicular);
			if (num)
			{
				float num2 = Vector2.Distance(vector, footOfPerpendicular);
				Vector2 item = (vector2 - vector).normalized * num2 + vector;
				list3.Add(item);
			}
			else
			{
				list3.Add(footOfPerpendicular);
			}
			Vector2 footOfPerpendicular2 = GetFootOfPerpendicular(a, vector2, vector3);
			Vector2 vector7 = vector2 - footOfPerpendicular2;
			Vector2 vector8 = vector3 - footOfPerpendicular2;
			footOfPerpendicular2 = (((vector7 + vector8).magnitude <= magnitude2) ? footOfPerpendicular2 : vector3);
			list2.Add(footOfPerpendicular2);
		}
		int count = vertices.Count;
		int num3 = count + points.Count;
		int num4 = num3 + neiPointList.Count;
		int num5 = num4 + list3.Count;
		int num6 = num5 + list.Count;
		for (int j = 0; j < points.Count; j++)
		{
			int num7 = (j + 1) % points.Count;
			int item2 = count + j;
			int item3 = count + num7;
			int item4 = num3 + j;
			int item5 = num3 + num7;
			int item6 = num4 + j;
			int item7 = num4 + num7;
			int item8 = num5 + j;
			int item9 = num6 + j;
			triangles.Add(item2);
			triangles.Add(item3);
			triangles.Add(item4);
			triangles.Add(item4);
			triangles.Add(item3);
			triangles.Add(item5);
			triangles.Add(item2);
			triangles.Add(item8);
			triangles.Add(item6);
			triangles.Add(item2);
			triangles.Add(item3);
			triangles.Add(item8);
			triangles.Add(item3);
			triangles.Add(item9);
			triangles.Add(item8);
			triangles.Add(item3);
			triangles.Add(item7);
			triangles.Add(item9);
		}
		foreach (Vector2 point in points)
		{
			vertices.Add(new Vector3(point.x, point.y, 0f));
			colors.Add(Color.black);
		}
		foreach (Vector2 neiPoint in neiPointList)
		{
			vertices.Add(new Vector3(neiPoint.x, neiPoint.y, 0f));
			colors.Add(Color.white);
		}
		foreach (Vector2 item10 in list3)
		{
			vertices.Add(new Vector3(item10.x, item10.y, 0f));
			colors.Add(Color.white);
		}
		foreach (Vector2 item11 in list)
		{
			vertices.Add(new Vector3(item11.x, item11.y, 0f));
			colors.Add(new Color(1f, 1f, 1f, 0.8f));
		}
		foreach (Vector2 item12 in list2)
		{
			vertices.Add(new Vector3(item12.x, item12.y, 0f));
			colors.Add(new Color(1f, 1f, 1f, 0.8f));
		}
	}
}
