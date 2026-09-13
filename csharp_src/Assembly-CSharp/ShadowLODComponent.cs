using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ShadowLODComponent : MonoBehaviour
{
	private static HashSet<ShadowLODComponent> msShadows = new HashSet<ShadowLODComponent>();

	private static bool msEnabled = true;

	private static List<Material> msSharedMaterialCache = new List<Material>();

	public Renderer renderer;

	public Material material;

	public void Awake()
	{
		if (!(material != null) || !(renderer != null))
		{
			if (renderer == null)
			{
				renderer = GetComponent<Renderer>();
			}
			if (!(renderer == null))
			{
				renderer.GetSharedMaterials(msSharedMaterialCache);
				material = msSharedMaterialCache[msSharedMaterialCache.Count - 1];
			}
		}
	}

	public void OnEnable()
	{
		msShadows.Add(this);
		UpdateShadow();
	}

	public void OnDisable()
	{
		msShadows.Remove(this);
	}

	private void UpdateShadow()
	{
		if (!(renderer != null) || !(material != null))
		{
			return;
		}
		msSharedMaterialCache.Clear();
		renderer.GetSharedMaterials(msSharedMaterialCache);
		int num = msSharedMaterialCache.IndexOf(material);
		if (msEnabled)
		{
			if (num == -1)
			{
				msSharedMaterialCache.Add(material);
				renderer.sharedMaterials = msSharedMaterialCache.ToArray();
			}
		}
		else if (num != -1)
		{
			msSharedMaterialCache.RemoveAt(num);
			renderer.sharedMaterials = msSharedMaterialCache.ToArray();
		}
	}

	public static void EnableShadow(bool enable)
	{
		if (msEnabled == enable)
		{
			return;
		}
		msEnabled = enable;
		foreach (ShadowLODComponent msShadow in msShadows)
		{
			msShadow.UpdateShadow();
		}
	}

	public void OnValidate()
	{
		if (renderer == null)
		{
			renderer = GetComponent<Renderer>();
			renderer.GetSharedMaterials(msSharedMaterialCache);
			material = msSharedMaterialCache[msSharedMaterialCache.Count - 1];
		}
	}

	public void ToggleShadow()
	{
		msEnabled = !msEnabled;
		UpdateShadow();
	}
}
