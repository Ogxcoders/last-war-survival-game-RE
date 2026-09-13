using System.Collections.Generic;
using GameKit.Base;
using UnityEngine;

public static class ModelEquipCombineHelper
{
	public static void ApplyModelEquipCombine(GameObject skeleton, List<GameObject> equips)
	{
		if (skeleton == null || equips == null || equips.Count <= 0)
		{
			return;
		}
		List<Transform> list = new List<Transform>(skeleton.GetComponentsInChildren<Transform>(includeInactive: true));
		List<Transform> list2 = new List<Transform>();
		List<Material> list3 = new List<Material>();
		List<CombineInstance> list4 = new List<CombineInstance>();
		for (int i = 0; i < equips.Count; i++)
		{
			GameObject gameObject = equips[i];
			if (gameObject == null)
			{
				continue;
			}
			SkinnedMeshRenderer componentInChildren = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
			if (componentInChildren == null)
			{
				continue;
			}
			Transform[] bones = componentInChildren.bones;
			foreach (Transform bone in bones)
			{
				Transform transform = list.Find((Transform t) => bone.name.Equals(t.name));
				if ((bool)transform)
				{
					list2.Add(transform);
				}
			}
			list4.Add(new CombineInstance
			{
				mesh = componentInChildren.sharedMesh
			});
			list3.Add(componentInChildren.sharedMaterial);
		}
		Mesh mesh = new Mesh();
		mesh.CombineMeshes(list4.ToArray(), mergeSubMeshes: false, useMatrices: false);
		SkinnedMeshRenderer orAddComponent = skeleton.GetOrAddComponent<SkinnedMeshRenderer>();
		orAddComponent.sharedMesh = mesh;
		orAddComponent.materials = list3.ToArray();
		orAddComponent.bones = list2.ToArray();
	}
}
