using System;
using UnityEngine;

namespace ParkourStage.Runtime.Util;

public interface IStagePrefabGenerator
{
	Transform GetSceneRoot();

	Transform GetMonsterBornRoot();

	GameObject InstantiatePrefab(string prefabPath, Transform parent = null);

	void InstantiatePrefabAsync(string prefabPath, Transform parent = null, Action<GameObject> complete = null);

	void DestroyInstantiatedPrefab(GameObject instantiatedPrefab);

	void OnUpdate(float deltaTime);

	void Clear();
}
