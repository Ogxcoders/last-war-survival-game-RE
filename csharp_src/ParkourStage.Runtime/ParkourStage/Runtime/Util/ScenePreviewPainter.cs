using System;
using UnityEngine;

namespace ParkourStage.Runtime.Util;

public static class ScenePreviewPainter
{
	private static GameObject ms_PreviewObject;

	private static Material ms_previewMaterial;

	private static bool ms_Active = false;

	private static float ms_PreviewScale = 1f;

	private static bool ms_Movable = true;

	private static Action<Vector3> ms_PreviewPuttedCallback;

	public static void SetActive(bool active)
	{
		ms_Active = active;
		if (ms_PreviewObject != null)
		{
			ms_PreviewObject.SetActive(ms_Active);
		}
	}

	private static void CreatePreviewMaterial()
	{
		ms_previewMaterial = new Material(Shader.Find("LastWar/Baked_Ramp"));
		ms_previewMaterial.SetColor("_BaseColor", new Color(0.2f, 0.8f, 0.3f, 0.5f));
		ms_previewMaterial.SetInt("_SrcBlend", 5);
		ms_previewMaterial.SetInt("_DstBlend", 10);
		ms_previewMaterial.SetInt("_ZWrite", 0);
		ms_previewMaterial.SetInt("_ZTest", 1);
		ms_previewMaterial.renderQueue = 3000;
	}

	public static void SetPreviewObjectPath(string previewObjectPath, float size = 1f)
	{
		ms_PreviewScale = size;
		if (ms_PreviewObject != null)
		{
			StagePrefabUtil.GetStagePrefabGenerator().DestroyInstantiatedPrefab(ms_PreviewObject);
		}
		StagePrefabUtil.GetStagePrefabGenerator().InstantiatePrefabAsync(previewObjectPath, null, OnPreviewObjectLoaded);
	}

	private static void OnPreviewObjectLoaded(GameObject previewObject)
	{
		ms_PreviewObject = previewObject;
		if (ms_PreviewObject != null)
		{
			SetPreviewMaterial(ms_PreviewObject);
			ms_PreviewObject.SetActive(ms_Active);
			Quaternion localRotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
			ms_PreviewObject.transform.localRotation = localRotation;
			ms_PreviewObject.transform.localScale = new Vector3(ms_PreviewScale, ms_PreviewScale, ms_PreviewScale);
		}
	}

	private static void SetPreviewMaterial(GameObject previewObject)
	{
		if (previewObject == null)
		{
			Debug.LogWarning("Preview object or material is null");
			return;
		}
		if (ms_previewMaterial == null)
		{
			CreatePreviewMaterial();
		}
		Renderer[] componentsInChildren = previewObject.GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer renderer in componentsInChildren)
		{
			if ((object)renderer == null)
			{
				continue;
			}
			if (!(renderer is MeshRenderer materialsForRenderer))
			{
				if (renderer is SkinnedMeshRenderer materialsForRenderer2)
				{
					SetMaterialsForRenderer(materialsForRenderer2);
				}
			}
			else
			{
				SetMaterialsForRenderer(materialsForRenderer);
			}
		}
	}

	private static void SetMaterialsForRenderer(Renderer renderer)
	{
		Material[] array = new Material[renderer.sharedMaterials.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = ms_previewMaterial;
		}
		renderer.sharedMaterials = array;
	}

	public static void SetMovable(bool movable)
	{
		ms_Movable = movable;
	}

	public static bool DrawPreview(Ray ray, out Vector3 hitPoint)
	{
		hitPoint = Vector3.zero;
		if (!ms_Active)
		{
			return false;
		}
		if (ms_PreviewObject == null)
		{
			return false;
		}
		if (!ms_Movable)
		{
			return false;
		}
		RaycastHit hitInfo;
		bool num = Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 1 << LayerMask.NameToLayer("Terrain"));
		Vector3 position = (hitPoint = (num ? hitInfo.point : ray.GetPoint(10f)));
		ms_PreviewObject.transform.position = position;
		return num;
	}

	public static void SetPreviewPutCallBack(Action<Vector3> previewPuttedCallback)
	{
		ms_PreviewPuttedCallback = previewPuttedCallback;
	}

	public static void PutPreview(Vector3 position)
	{
		ms_PreviewPuttedCallback?.Invoke(position);
	}

	public static void Clear()
	{
		if (ms_PreviewObject != null)
		{
			StagePrefabUtil.GetStagePrefabGenerator().DestroyInstantiatedPrefab(ms_PreviewObject);
		}
		ms_PreviewPuttedCallback = null;
		ms_Movable = true;
		ms_PreviewObject = null;
		if (ms_previewMaterial != null)
		{
			UnityEngine.Object.Destroy(ms_previewMaterial);
			ms_previewMaterial = null;
		}
	}
}
