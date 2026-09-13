using UnityEngine;

namespace VolumetricFogAndMist2;

public static class Tools
{
	public static Color ColorBlack = Color.black;

	public static void CheckCamera(ref Camera cam)
	{
		if (cam != null)
		{
			return;
		}
		cam = Camera.main;
		if (!(cam == null))
		{
			return;
		}
		Camera[] array = Object.FindObjectsOfType<Camera>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].isActiveAndEnabled && array[i].gameObject.activeInHierarchy)
			{
				cam = array[i];
				break;
			}
		}
	}

	public static VolumetricFogManager CheckMainManager()
	{
		VolumetricFogManager volumetricFogManager = Object.FindObjectOfType<VolumetricFogManager>();
		if (volumetricFogManager == null)
		{
			GameObject gameObject = new GameObject();
			volumetricFogManager = gameObject.AddComponent<VolumetricFogManager>();
			gameObject.name = volumetricFogManager.managerName;
		}
		return volumetricFogManager;
	}

	public static void CheckManager<T>(ref T manager) where T : Component
	{
		if (!(manager == null))
		{
			return;
		}
		manager = Object.FindObjectOfType<T>();
		if (manager == null)
		{
			VolumetricFogManager volumetricFogManager = CheckMainManager();
			if (volumetricFogManager != null)
			{
				manager = Object.FindObjectOfType<T>();
			}
			if (manager == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.transform.SetParent(volumetricFogManager.transform, worldPositionStays: false);
				manager = gameObject.AddComponent<T>();
				gameObject.name = ((IVolumetricFogManager)manager).managerName;
			}
		}
	}
}
