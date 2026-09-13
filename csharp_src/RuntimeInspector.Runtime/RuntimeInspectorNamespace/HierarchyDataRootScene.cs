using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RuntimeInspectorNamespace;

public class HierarchyDataRootScene : HierarchyDataRoot
{
	private readonly List<GameObject> rootObjects = new List<GameObject>();

	public override string Name => Scene.name;

	public override int ChildCount => rootObjects.Count;

	public Scene Scene { get; private set; }

	public HierarchyDataRootScene(RuntimeHierarchy hierarchy, Scene target)
		: base(hierarchy)
	{
		Scene = target;
	}

	public override void RefreshContent()
	{
		rootObjects.Clear();
		if (Scene.isLoaded)
		{
			Scene.GetRootGameObjects(rootObjects);
		}
	}

	public override Transform GetChild(int index)
	{
		return rootObjects[index].transform;
	}

	public override Transform GetNearestRootOf(Transform target)
	{
		if (!(target.gameObject.scene == Scene))
		{
			return null;
		}
		return target.root;
	}
}
