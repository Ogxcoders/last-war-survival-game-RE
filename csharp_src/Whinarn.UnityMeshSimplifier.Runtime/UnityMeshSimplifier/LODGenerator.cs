using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityMeshSimplifier;

public static class LODGenerator
{
	private struct RendererInfo
	{
		public string name;

		public bool isStatic;

		public bool isNewMesh;

		public Transform transform;

		public Mesh mesh;

		public Material[] materials;

		public Transform rootBone;

		public Transform[] bones;
	}

	public static readonly string LODParentGameObjectName = "_UMS_LODs_";

	public static readonly string LODAssetParentPath = "Assets/UMS_LODs/";

	public static LODGroup GenerateLODs(LODGeneratorHelper generatorHelper)
	{
		if (generatorHelper == null)
		{
			throw new ArgumentNullException("generatorHelper");
		}
		GameObject gameObject = generatorHelper.gameObject;
		LODLevel[] levels = generatorHelper.Levels;
		bool autoCollectRenderers = generatorHelper.AutoCollectRenderers;
		SimplificationOptions simplificationOptions = generatorHelper.SimplificationOptions;
		string saveAssetsPath = generatorHelper.SaveAssetsPath;
		LODGroup lODGroup = GenerateLODs(gameObject, levels, autoCollectRenderers, simplificationOptions, saveAssetsPath);
		if (lODGroup == null)
		{
			return null;
		}
		lODGroup.animateCrossFading = generatorHelper.AnimateCrossFading;
		lODGroup.fadeMode = generatorHelper.FadeMode;
		return lODGroup;
	}

	public static LODGroup GenerateLODs(GameObject gameObject, LODLevel[] levels, bool autoCollectRenderers, SimplificationOptions simplificationOptions)
	{
		return GenerateLODs(gameObject, levels, autoCollectRenderers, simplificationOptions, null);
	}

	public static LODGroup GenerateLODs(GameObject gameObject, LODLevel[] levels, bool autoCollectRenderers, SimplificationOptions simplificationOptions, string saveAssetsPath)
	{
		if (gameObject == null)
		{
			throw new ArgumentNullException("gameObject");
		}
		if (levels == null)
		{
			throw new ArgumentNullException("levels");
		}
		Transform transform = gameObject.transform;
		if (transform.Find(LODParentGameObjectName) != null)
		{
			throw new InvalidOperationException("The game object already appears to have LODs. Please remove them first.");
		}
		if (gameObject.GetComponent<LODGroup>() != null)
		{
			throw new InvalidOperationException("The game object already appears to have a LOD Group. Please remove it first.");
		}
		saveAssetsPath = ValidateSaveAssetsPath(saveAssetsPath);
		Transform transform2 = new GameObject(LODParentGameObjectName).transform;
		ParentAndResetTransform(transform2, transform);
		LODGroup lODGroup = gameObject.AddComponent<LODGroup>();
		Renderer[] array = null;
		if (autoCollectRenderers)
		{
			array = GetChildRenderersForLOD(gameObject);
		}
		List<Renderer> list = new List<Renderer>((array != null) ? array.Length : 10);
		LOD[] array2 = new LOD[levels.Length];
		for (int i = 0; i < levels.Length; i++)
		{
			LODLevel level = levels[i];
			Transform transform3 = new GameObject($"Level{i:00}").transform;
			ParentAndResetTransform(transform3, transform2);
			Renderer[] array3 = array ?? level.Renderers;
			List<Renderer> list2 = new List<Renderer>((array3 != null) ? array3.Length : 0);
			if (array3 != null && array3.Length != 0)
			{
				MeshRenderer[] renderers = (from renderer3 in array3
					let meshFilter = renderer3.GetComponent<MeshFilter>()
					let renderer = renderer3
					where renderer.enabled && renderer as MeshRenderer != null && meshFilter != null && meshFilter.sharedMesh != null
					select renderer as MeshRenderer).ToArray();
				SkinnedMeshRenderer[] renderers2 = (from renderer3 in array3
					where renderer3.enabled && renderer3 as SkinnedMeshRenderer != null && (renderer3 as SkinnedMeshRenderer).sharedMesh != null
					select renderer3 as SkinnedMeshRenderer).ToArray();
				RendererInfo[] array4;
				RendererInfo[] array5;
				if (level.CombineMeshes)
				{
					array4 = CombineStaticMeshes(transform, i, renderers);
					array5 = CombineSkinnedMeshes(transform, i, renderers2);
				}
				else
				{
					array4 = GetStaticRenderers(renderers);
					array5 = GetSkinnedRenderers(renderers2);
				}
				if (array4 != null)
				{
					for (int num = 0; num < array4.Length; num++)
					{
						RendererInfo renderer = array4[num];
						Renderer item = CreateLevelRenderer(gameObject, i, ref level, transform3, num, renderer, ref simplificationOptions, saveAssetsPath);
						list2.Add(item);
					}
				}
				if (array5 != null)
				{
					for (int num2 = 0; num2 < array5.Length; num2++)
					{
						RendererInfo renderer2 = array5[num2];
						Renderer item2 = CreateLevelRenderer(gameObject, i, ref level, transform3, num2, renderer2, ref simplificationOptions, saveAssetsPath);
						list2.Add(item2);
					}
				}
				Renderer[] array6 = array3;
				foreach (Renderer item3 in array6)
				{
					if (!list.Contains(item3))
					{
						list.Add(item3);
					}
				}
			}
			array2[i] = new LOD(level.ScreenRelativeTransitionHeight, list2.ToArray());
		}
		CreateBackup(gameObject, list.ToArray());
		foreach (Renderer item4 in list)
		{
			item4.enabled = false;
		}
		lODGroup.animateCrossFading = false;
		lODGroup.SetLODs(array2);
		return lODGroup;
	}

	public static bool DestroyLODs(LODGeneratorHelper generatorHelper)
	{
		if (generatorHelper == null)
		{
			throw new ArgumentNullException("generatorHelper");
		}
		return DestroyLODs(generatorHelper.gameObject);
	}

	public static bool DestroyLODs(GameObject gameObject)
	{
		if (gameObject == null)
		{
			throw new ArgumentNullException("gameObject");
		}
		RestoreBackup(gameObject);
		Transform transform = gameObject.transform.Find(LODParentGameObjectName);
		if (transform == null)
		{
			return false;
		}
		DestroyObject(transform.gameObject);
		LODGroup component = gameObject.GetComponent<LODGroup>();
		if (component != null)
		{
			DestroyObject(component);
		}
		return true;
	}

	private static RendererInfo[] GetStaticRenderers(MeshRenderer[] renderers)
	{
		List<RendererInfo> list = new List<RendererInfo>(renderers.Length);
		foreach (MeshRenderer meshRenderer in renderers)
		{
			MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
			if (component == null)
			{
				Debug.LogWarning("A renderer was missing a mesh filter and was ignored.", meshRenderer);
				continue;
			}
			Mesh sharedMesh = component.sharedMesh;
			if (sharedMesh == null)
			{
				Debug.LogWarning("A renderer was missing a mesh and was ignored.", meshRenderer);
				continue;
			}
			list.Add(new RendererInfo
			{
				name = meshRenderer.name,
				isStatic = true,
				isNewMesh = false,
				transform = meshRenderer.transform,
				mesh = sharedMesh,
				materials = meshRenderer.sharedMaterials
			});
		}
		return list.ToArray();
	}

	private static RendererInfo[] GetSkinnedRenderers(SkinnedMeshRenderer[] renderers)
	{
		List<RendererInfo> list = new List<RendererInfo>(renderers.Length);
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in renderers)
		{
			Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
			if (sharedMesh == null)
			{
				Debug.LogWarning("A renderer was missing a mesh and was ignored.", skinnedMeshRenderer);
				continue;
			}
			list.Add(new RendererInfo
			{
				name = skinnedMeshRenderer.name,
				isStatic = false,
				isNewMesh = false,
				transform = skinnedMeshRenderer.transform,
				mesh = sharedMesh,
				materials = skinnedMeshRenderer.sharedMaterials,
				rootBone = skinnedMeshRenderer.rootBone,
				bones = skinnedMeshRenderer.bones
			});
		}
		return list.ToArray();
	}

	private static RendererInfo[] CombineStaticMeshes(Transform transform, int levelIndex, MeshRenderer[] renderers)
	{
		if (renderers.Length == 0)
		{
			return null;
		}
		List<RendererInfo> list = new List<RendererInfo>(renderers.Length);
		Material[] resultMaterials;
		Mesh mesh = MeshCombiner.CombineMeshes(transform, renderers, out resultMaterials);
		mesh.name = $"{transform.name}_static{levelIndex:00}";
		string name = $"{transform.name}_combined_static";
		list.Add(new RendererInfo
		{
			name = name,
			isStatic = true,
			isNewMesh = true,
			transform = null,
			mesh = mesh,
			materials = resultMaterials,
			rootBone = null,
			bones = null
		});
		return list.ToArray();
	}

	private static RendererInfo[] CombineSkinnedMeshes(Transform transform, int levelIndex, SkinnedMeshRenderer[] renderers)
	{
		if (renderers.Length == 0)
		{
			return null;
		}
		List<RendererInfo> list = new List<RendererInfo>(renderers.Length);
		IEnumerable<SkinnedMeshRenderer> enumerable = renderers.Where((SkinnedMeshRenderer renderer) => renderer.sharedMesh != null && renderer.sharedMesh.blendShapeCount > 0);
		IEnumerable<SkinnedMeshRenderer> enumerable2 = renderers.Where((SkinnedMeshRenderer renderer) => renderer.sharedMesh == null);
		SkinnedMeshRenderer[] array = renderers.Where((SkinnedMeshRenderer renderer) => renderer.sharedMesh != null && renderer.sharedMesh.blendShapeCount == 0).ToArray();
		foreach (SkinnedMeshRenderer item in enumerable2)
		{
			Debug.LogWarning("A renderer was missing a mesh and was ignored.", item);
		}
		foreach (SkinnedMeshRenderer item2 in enumerable)
		{
			list.Add(new RendererInfo
			{
				name = item2.name,
				isStatic = false,
				isNewMesh = false,
				transform = item2.transform,
				mesh = item2.sharedMesh,
				materials = item2.sharedMaterials,
				rootBone = item2.rootBone,
				bones = item2.bones
			});
		}
		if (array.Length != 0)
		{
			Material[] resultMaterials;
			Transform[] resultBones;
			Mesh mesh = MeshCombiner.CombineMeshes(transform, array, out resultMaterials, out resultBones);
			mesh.name = $"{transform.name}_skinned{levelIndex:00}";
			Transform rootBone = FindBestRootBone(transform, array);
			string name = $"{transform.name}_combined_skinned";
			list.Add(new RendererInfo
			{
				name = name,
				isStatic = false,
				isNewMesh = false,
				transform = null,
				mesh = mesh,
				materials = resultMaterials,
				rootBone = rootBone,
				bones = resultBones
			});
		}
		return list.ToArray();
	}

	private static void ParentAndResetTransform(Transform transform, Transform parentTransform)
	{
		transform.SetParent(parentTransform);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	private static void ParentAndOffsetTransform(Transform transform, Transform parentTransform, Transform originalTransform)
	{
		transform.position = originalTransform.position;
		transform.rotation = originalTransform.rotation;
		transform.localScale = originalTransform.lossyScale;
		transform.SetParent(parentTransform, worldPositionStays: true);
	}

	private static Renderer CreateLevelRenderer(GameObject gameObject, int levelIndex, ref LODLevel level, Transform levelTransform, int rendererIndex, RendererInfo renderer, ref SimplificationOptions simplificationOptions, string saveAssetsPath)
	{
		Mesh mesh = renderer.mesh;
		if (level.Quality < 1f)
		{
			mesh = SimplifyMesh(mesh, level.Quality, simplificationOptions);
			if (renderer.isNewMesh)
			{
				DestroyObject(renderer.mesh);
			}
		}
		if (renderer.isStatic)
		{
			return CreateStaticLevelRenderer($"{rendererIndex:000}_static_{renderer.name}", levelTransform, renderer.transform, mesh, renderer.materials, ref level);
		}
		return CreateSkinnedLevelRenderer($"{rendererIndex:000}_skinned_{renderer.name}", levelTransform, renderer.transform, mesh, renderer.materials, renderer.rootBone, renderer.bones, ref level);
	}

	private static MeshRenderer CreateStaticLevelRenderer(string name, Transform parentTransform, Transform originalTransform, Mesh mesh, Material[] materials, ref LODLevel level)
	{
		GameObject gameObject = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
		Transform transform = gameObject.transform;
		if (originalTransform != null)
		{
			ParentAndOffsetTransform(transform, parentTransform, originalTransform);
		}
		else
		{
			ParentAndResetTransform(transform, parentTransform);
		}
		gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
		MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
		component.sharedMaterials = materials;
		SetupLevelRenderer(component, ref level);
		return component;
	}

	private static SkinnedMeshRenderer CreateSkinnedLevelRenderer(string name, Transform parentTransform, Transform originalTransform, Mesh mesh, Material[] materials, Transform rootBone, Transform[] bones, ref LODLevel level)
	{
		GameObject gameObject = new GameObject(name, typeof(SkinnedMeshRenderer));
		Transform transform = gameObject.transform;
		if (originalTransform != null)
		{
			ParentAndOffsetTransform(transform, parentTransform, originalTransform);
		}
		else
		{
			ParentAndResetTransform(transform, parentTransform);
		}
		SkinnedMeshRenderer component = gameObject.GetComponent<SkinnedMeshRenderer>();
		component.sharedMesh = mesh;
		component.sharedMaterials = materials;
		component.rootBone = rootBone;
		component.bones = bones;
		SetupLevelRenderer(component, ref level);
		return component;
	}

	private static Transform FindBestRootBone(Transform transform, SkinnedMeshRenderer[] skinnedMeshRenderers)
	{
		if (skinnedMeshRenderers == null || skinnedMeshRenderers.Length == 0)
		{
			return null;
		}
		Transform result = null;
		float num = float.MaxValue;
		for (int i = 0; i < skinnedMeshRenderers.Length; i++)
		{
			if (!(skinnedMeshRenderers[i] == null) && !(skinnedMeshRenderers[i].rootBone == null))
			{
				Transform rootBone = skinnedMeshRenderers[i].rootBone;
				float sqrMagnitude = (rootBone.position - transform.position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					result = rootBone;
					num = sqrMagnitude;
				}
			}
		}
		return result;
	}

	private static void SetupLevelRenderer(Renderer renderer, ref LODLevel level)
	{
		renderer.shadowCastingMode = level.ShadowCastingMode;
		renderer.receiveShadows = level.ReceiveShadows;
		renderer.motionVectorGenerationMode = level.MotionVectorGenerationMode;
		renderer.lightProbeUsage = level.LightProbeUsage;
		renderer.reflectionProbeUsage = level.ReflectionProbeUsage;
		SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
		if (skinnedMeshRenderer != null)
		{
			skinnedMeshRenderer.quality = level.SkinQuality;
			skinnedMeshRenderer.skinnedMotionVectors = level.SkinnedMotionVectors;
		}
	}

	private static Renderer[] GetChildRenderersForLOD(GameObject gameObject)
	{
		List<Renderer> list = new List<Renderer>();
		CollectChildRenderersForLOD(gameObject.transform, list);
		return list.ToArray();
	}

	private static void CollectChildRenderersForLOD(Transform transform, List<Renderer> resultRenderers)
	{
		Renderer[] components = transform.GetComponents<Renderer>();
		resultRenderers.AddRange(components);
		int childCount = transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (child.gameObject.activeSelf && !string.Equals(child.name, LODParentGameObjectName) && !(child.GetComponent<LODGroup>() != null) && !(child.GetComponent<LODGeneratorHelper>() != null))
			{
				CollectChildRenderersForLOD(child, resultRenderers);
			}
		}
	}

	private static Mesh SimplifyMesh(Mesh mesh, float quality, SimplificationOptions options)
	{
		MeshSimplifier meshSimplifier = new MeshSimplifier();
		meshSimplifier.PreserveBorderEdges = options.PreserveBorderEdges;
		meshSimplifier.PreserveUVSeamEdges = options.PreserveUVSeamEdges;
		meshSimplifier.PreserveUVFoldoverEdges = options.PreserveUVFoldoverEdges;
		meshSimplifier.PreserveSurfaceCurvature = options.PreserveSurfaceCurvature;
		meshSimplifier.EnableSmartLink = options.EnableSmartLink;
		meshSimplifier.VertexLinkDistance = options.VertexLinkDistance;
		meshSimplifier.MaxIterationCount = options.MaxIterationCount;
		meshSimplifier.Agressiveness = options.Agressiveness;
		meshSimplifier.Initialize(mesh);
		meshSimplifier.SimplifyMesh(quality);
		Mesh mesh2 = meshSimplifier.ToMesh();
		mesh2.bindposes = mesh.bindposes;
		return mesh2;
	}

	private static void DestroyObject(UnityEngine.Object obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		UnityEngine.Object.Destroy(obj);
	}

	private static void CreateBackup(GameObject gameObject, Renderer[] originalRenderers)
	{
		LODBackupComponent lODBackupComponent = gameObject.AddComponent<LODBackupComponent>();
		lODBackupComponent.hideFlags = HideFlags.HideInInspector;
		lODBackupComponent.OriginalRenderers = originalRenderers;
	}

	private static void RestoreBackup(GameObject gameObject)
	{
		LODBackupComponent[] components = gameObject.GetComponents<LODBackupComponent>();
		foreach (LODBackupComponent lODBackupComponent in components)
		{
			Renderer[] originalRenderers = lODBackupComponent.OriginalRenderers;
			if (originalRenderers != null)
			{
				Renderer[] array = originalRenderers;
				foreach (Renderer renderer in array)
				{
					if (renderer != null)
					{
						renderer.enabled = true;
					}
				}
			}
			DestroyObject(lODBackupComponent);
		}
	}

	private static string ValidateSaveAssetsPath(string saveAssetsPath)
	{
		if (string.IsNullOrEmpty(saveAssetsPath))
		{
			return null;
		}
		Debug.LogWarning("Unable to save assets when not running in the Unity Editor.");
		return null;
	}
}
