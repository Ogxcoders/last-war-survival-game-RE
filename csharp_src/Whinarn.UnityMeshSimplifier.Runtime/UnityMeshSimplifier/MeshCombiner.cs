using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityMeshSimplifier;

public static class MeshCombiner
{
	public static Mesh CombineMeshes(Transform rootTransform, MeshRenderer[] renderers, out Material[] resultMaterials)
	{
		if (rootTransform == null)
		{
			throw new ArgumentNullException("rootTransform");
		}
		if (renderers == null)
		{
			throw new ArgumentNullException("renderers");
		}
		Mesh[] array = new Mesh[renderers.Length];
		Matrix4x4[] array2 = new Matrix4x4[renderers.Length];
		Material[][] array3 = new Material[renderers.Length][];
		for (int i = 0; i < renderers.Length; i++)
		{
			MeshRenderer meshRenderer = renderers[i];
			if (meshRenderer == null)
			{
				throw new ArgumentException($"The renderer at index {i} is null.", "renderers");
			}
			Transform transform = meshRenderer.transform;
			MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
			if (component == null)
			{
				throw new ArgumentException($"The renderer at index {i} has no mesh filter.", "renderers");
			}
			if (component.sharedMesh == null)
			{
				throw new ArgumentException($"The mesh filter for renderer at index {i} has no mesh.", "renderers");
			}
			if (!CanReadMesh(component.sharedMesh))
			{
				throw new ArgumentException($"The mesh in the mesh filter for renderer at index {i} is not readable.", "renderers");
			}
			array[i] = component.sharedMesh;
			array2[i] = rootTransform.worldToLocalMatrix * transform.localToWorldMatrix;
			array3[i] = meshRenderer.sharedMaterials;
		}
		return CombineMeshes(array, array2, array3, out resultMaterials);
	}

	public static Mesh CombineMeshes(Transform rootTransform, SkinnedMeshRenderer[] renderers, out Material[] resultMaterials, out Transform[] resultBones)
	{
		if (rootTransform == null)
		{
			throw new ArgumentNullException("rootTransform");
		}
		if (renderers == null)
		{
			throw new ArgumentNullException("renderers");
		}
		Mesh[] array = new Mesh[renderers.Length];
		Matrix4x4[] array2 = new Matrix4x4[renderers.Length];
		Material[][] array3 = new Material[renderers.Length][];
		Transform[][] array4 = new Transform[renderers.Length][];
		for (int i = 0; i < renderers.Length; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = renderers[i];
			if (skinnedMeshRenderer == null)
			{
				throw new ArgumentException($"The renderer at index {i} is null.", "renderers");
			}
			if (skinnedMeshRenderer.sharedMesh == null)
			{
				throw new ArgumentException($"The renderer at index {i} has no mesh.", "renderers");
			}
			if (!CanReadMesh(skinnedMeshRenderer.sharedMesh))
			{
				throw new ArgumentException($"The mesh in the renderer at index {i} is not readable.", "renderers");
			}
			Transform transform = skinnedMeshRenderer.transform;
			array[i] = skinnedMeshRenderer.sharedMesh;
			array2[i] = transform.worldToLocalMatrix * transform.localToWorldMatrix;
			array3[i] = skinnedMeshRenderer.sharedMaterials;
			array4[i] = skinnedMeshRenderer.bones;
		}
		return CombineMeshes(array, array2, array3, array4, out resultMaterials, out resultBones);
	}

	public static Mesh CombineMeshes(Mesh[] meshes, Matrix4x4[] transforms, Material[][] materials, out Material[] resultMaterials)
	{
		if (meshes == null)
		{
			throw new ArgumentNullException("meshes");
		}
		if (transforms == null)
		{
			throw new ArgumentNullException("transforms");
		}
		if (materials == null)
		{
			throw new ArgumentNullException("materials");
		}
		Transform[] resultBones;
		return CombineMeshes(meshes, transforms, materials, null, out resultMaterials, out resultBones);
	}

	public static Mesh CombineMeshes(Mesh[] meshes, Matrix4x4[] transforms, Material[][] materials, Transform[][] bones, out Material[] resultMaterials, out Transform[] resultBones)
	{
		if (meshes == null)
		{
			throw new ArgumentNullException("meshes");
		}
		if (transforms == null)
		{
			throw new ArgumentNullException("transforms");
		}
		if (materials == null)
		{
			throw new ArgumentNullException("materials");
		}
		if (transforms.Length != meshes.Length)
		{
			throw new ArgumentException("The array of transforms doesn't have the same length as the array of meshes.", "transforms");
		}
		if (materials.Length != meshes.Length)
		{
			throw new ArgumentException("The array of materials doesn't have the same length as the array of meshes.", "materials");
		}
		if (bones != null && bones.Length != meshes.Length)
		{
			throw new ArgumentException("The array of bones doesn't have the same length as the array of meshes.", "bones");
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < meshes.Length; i++)
		{
			Mesh mesh = meshes[i];
			if (mesh == null)
			{
				throw new ArgumentException($"The mesh at index {i} is null.", "meshes");
			}
			if (!CanReadMesh(mesh))
			{
				throw new ArgumentException($"The mesh at index {i} is not readable.", "meshes");
			}
			num += mesh.vertexCount;
			num2 += mesh.subMeshCount;
			Material[] array = materials[i];
			if (array == null)
			{
				throw new ArgumentException($"The materials for mesh at index {i} is null.", "materials");
			}
			if (array.Length != mesh.subMeshCount)
			{
				throw new ArgumentException($"The materials for mesh at index {i} doesn't match the submesh count ({array.Length} != {mesh.subMeshCount}).", "materials");
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] == null)
				{
					throw new ArgumentException($"The material at index {j} for mesh at index {i} is null.", "materials");
				}
			}
			if (bones == null)
			{
				continue;
			}
			Transform[] array2 = bones[i];
			if (array2 == null)
			{
				throw new ArgumentException($"The bones for mesh at index {i} is null.", "meshBones");
			}
			for (int k = 0; k < array2.Length; k++)
			{
				if (array2[k] == null)
				{
					throw new ArgumentException($"The bone at index {k} for mesh at index {i} is null.", "meshBones");
				}
			}
		}
		List<Vector3> list = new List<Vector3>(num);
		List<int[]> list2 = new List<int[]>(num2);
		List<Vector3> dest = null;
		List<Vector4> dest2 = null;
		List<Color> dest3 = null;
		List<BoneWeight> dest4 = null;
		List<Vector4>[] array3 = new List<Vector4>[MeshUtils.UVChannelCount];
		List<Matrix4x4> list3 = null;
		List<Transform> list4 = null;
		List<Material> list5 = new List<Material>(num2);
		Dictionary<Material, int> dictionary = new Dictionary<Material, int>(num2);
		int num3 = 0;
		for (int l = 0; l < meshes.Length; l++)
		{
			Mesh mesh2 = meshes[l];
			Matrix4x4 transform = transforms[l];
			Material[] array4 = materials[l];
			Transform[] array5 = ((bones != null) ? bones[l] : null);
			int subMeshCount = mesh2.subMeshCount;
			int vertexCount = mesh2.vertexCount;
			Vector3[] vertices = mesh2.vertices;
			Vector3[] normals = mesh2.normals;
			Vector4[] tangents = mesh2.tangents;
			List<Vector4>[] meshUVs = MeshUtils.GetMeshUVs(mesh2);
			Color[] colors = mesh2.colors;
			BoneWeight[] boneWeights = mesh2.boneWeights;
			Matrix4x4[] bindposes = mesh2.bindposes;
			if (array5 != null && boneWeights != null && boneWeights.Length != 0 && bindposes != null && bindposes.Length != 0 && array5.Length == bindposes.Length)
			{
				if (list3 == null)
				{
					list3 = new List<Matrix4x4>(bindposes);
					list4 = new List<Transform>(array5);
				}
				int[] array6 = new int[array5.Length];
				for (int m = 0; m < array5.Length; m++)
				{
					int num4 = list4.IndexOf(array5[m]);
					if (num4 == -1 || bindposes[m] != list3[num4])
					{
						num4 = list4.Count;
						list4.Add(array5[m]);
						list3.Add(bindposes[m]);
					}
					array6[m] = num4;
				}
				RemapBones(boneWeights, array6);
			}
			TransformVertices(vertices, ref transform);
			TransformNormals(normals, ref transform);
			TransformTangents(tangents, ref transform);
			CopyVertexPositions(list, vertices);
			CopyVertexAttributes(ref dest, normals, num3, vertexCount, num, new Vector3(1f, 0f, 0f));
			CopyVertexAttributes(ref dest2, tangents, num3, vertexCount, num, new Vector4(0f, 0f, 1f, 1f));
			CopyVertexAttributes(ref dest3, colors, num3, vertexCount, num, new Color(1f, 1f, 1f, 1f));
			CopyVertexAttributes(ref dest4, boneWeights, num3, vertexCount, num, default(BoneWeight));
			for (int n = 0; n < meshUVs.Length; n++)
			{
				CopyVertexAttributes(ref array3[n], meshUVs[n], num3, vertexCount, num, new Vector4(0f, 0f, 0f, 0f));
			}
			for (int num5 = 0; num5 < subMeshCount; num5++)
			{
				Material material = array4[num5];
				int[] triangles = mesh2.GetTriangles(num5, applyBaseVertex: true);
				if (num3 > 0)
				{
					for (int num6 = 0; num6 < triangles.Length; num6++)
					{
						triangles[num6] += num3;
					}
				}
				if (dictionary.TryGetValue(material, out var value))
				{
					list2[value] = MergeArrays(list2[value], triangles);
					continue;
				}
				int count = list2.Count;
				dictionary.Add(material, count);
				list5.Add(material);
				list2.Add(triangles);
			}
			num3 += vertexCount;
		}
		Vector3[] vertices2 = list.ToArray();
		int[][] indices = list2.ToArray();
		Vector3[] normals2 = dest?.ToArray();
		Vector4[] tangents2 = dest2?.ToArray();
		Color[] colors2 = dest3?.ToArray();
		BoneWeight[] boneWeights2 = dest4?.ToArray();
		List<Vector4>[] uvs = array3.ToArray();
		Matrix4x4[] bindposes2 = list3?.ToArray();
		resultMaterials = list5.ToArray();
		resultBones = list4?.ToArray();
		return MeshUtils.CreateMesh(vertices2, indices, normals2, tangents2, colors2, boneWeights2, uvs, bindposes2, null);
	}

	private static void CopyVertexPositions(ICollection<Vector3> list, Vector3[] arr)
	{
		if (arr != null && arr.Length != 0)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				list.Add(arr[i]);
			}
		}
	}

	private static void CopyVertexAttributes<T>(ref List<T> dest, IEnumerable<T> src, int previousVertexCount, int meshVertexCount, int totalVertexCount, T defaultValue)
	{
		if (src == null || src.Count() == 0)
		{
			if (dest != null)
			{
				for (int i = 0; i < meshVertexCount; i++)
				{
					dest.Add(defaultValue);
				}
			}
			return;
		}
		if (dest == null)
		{
			dest = new List<T>(totalVertexCount);
			for (int j = 0; j < previousVertexCount; j++)
			{
				dest.Add(defaultValue);
			}
		}
		dest.AddRange(src);
	}

	private static T[] MergeArrays<T>(T[] arr1, T[] arr2)
	{
		T[] array = new T[arr1.Length + arr2.Length];
		Array.Copy(arr1, 0, array, 0, arr1.Length);
		Array.Copy(arr2, 0, array, arr1.Length, arr2.Length);
		return array;
	}

	private static void TransformVertices(Vector3[] vertices, ref Matrix4x4 transform)
	{
		for (int i = 0; i < vertices.Length; i++)
		{
			vertices[i] = transform.MultiplyPoint3x4(vertices[i]);
		}
	}

	private static void TransformNormals(Vector3[] normals, ref Matrix4x4 transform)
	{
		if (normals != null)
		{
			for (int i = 0; i < normals.Length; i++)
			{
				normals[i] = transform.MultiplyVector(normals[i]);
			}
		}
	}

	private static void TransformTangents(Vector4[] tangents, ref Matrix4x4 transform)
	{
		if (tangents != null)
		{
			for (int i = 0; i < tangents.Length; i++)
			{
				Vector3 vector = transform.MultiplyVector(new Vector3(tangents[i].x, tangents[i].y, tangents[i].z));
				tangents[i] = new Vector4(vector.x, vector.y, vector.z, tangents[i].w);
			}
		}
	}

	private static void RemapBones(BoneWeight[] boneWeights, int[] boneIndices)
	{
		for (int i = 0; i < boneWeights.Length; i++)
		{
			if (boneWeights[i].weight0 > 0f)
			{
				boneWeights[i].boneIndex0 = boneIndices[boneWeights[i].boneIndex0];
			}
			if (boneWeights[i].weight1 > 0f)
			{
				boneWeights[i].boneIndex1 = boneIndices[boneWeights[i].boneIndex1];
			}
			if (boneWeights[i].weight2 > 0f)
			{
				boneWeights[i].boneIndex2 = boneIndices[boneWeights[i].boneIndex2];
			}
			if (boneWeights[i].weight3 > 0f)
			{
				boneWeights[i].boneIndex3 = boneIndices[boneWeights[i].boneIndex3];
			}
		}
	}

	private static bool CanReadMesh(Mesh mesh)
	{
		return mesh.isReadable;
	}
}
