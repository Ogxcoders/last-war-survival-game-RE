using UnityEngine;

public class BattlefieldHandle : MonoBehaviour
{
	[SerializeField]
	private Renderer[] renderers;

	[SerializeField]
	private Material[] materials;

	[SerializeField]
	private GameObject[] gameObjects;

	[SerializeField]
	private Transform[] transforms;

	public Renderer GetRenderer(int index)
	{
		if (renderers == null || index < 0 || index >= renderers.Length)
		{
			return null;
		}
		return renderers[index];
	}

	public Material GetMaterial(int index)
	{
		if (materials == null || index < 0 || index >= materials.Length)
		{
			return null;
		}
		return materials[index];
	}

	public GameObject GetGameObject(int index)
	{
		if (gameObjects == null || index < 0 || index >= gameObjects.Length)
		{
			return null;
		}
		return gameObjects[index];
	}

	public Transform GetTransform(int index)
	{
		if (transforms == null || index < 0 || index >= transforms.Length)
		{
			return null;
		}
		return transforms[index];
	}
}
