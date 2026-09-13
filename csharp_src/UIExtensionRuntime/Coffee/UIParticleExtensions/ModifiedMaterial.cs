using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIParticleExtensions;

internal class ModifiedMaterial
{
	private class MatEntry
	{
		public Material baseMat;

		public Material customMat;

		public int count;

		public Texture texture;

		public int id;
	}

	private static readonly List<MatEntry> s_Entries = new List<MatEntry>();

	private static int? disableFogID;

	public static void DisableFog(Material baseMat)
	{
		if (!disableFogID.HasValue)
		{
			disableFogID = Shader.PropertyToID("_DisableFog");
		}
		if (baseMat.HasProperty(disableFogID.Value))
		{
			baseMat.SetFloat(disableFogID.Value, 1f);
		}
	}

	public static Material Add(Material baseMat, Texture texture, int id)
	{
		MatEntry matEntry;
		for (int i = 0; i < s_Entries.Count; i++)
		{
			matEntry = s_Entries[i];
			if (!(matEntry.baseMat != baseMat) && !(matEntry.texture != texture) && matEntry.id == id)
			{
				matEntry.count++;
				return matEntry.customMat;
			}
		}
		matEntry = new MatEntry();
		matEntry.count = 1;
		matEntry.baseMat = baseMat;
		matEntry.texture = texture;
		matEntry.id = id;
		matEntry.customMat = new Material(baseMat);
		matEntry.customMat.hideFlags = HideFlags.HideAndDontSave;
		if ((bool)texture)
		{
			matEntry.customMat.mainTexture = texture;
		}
		s_Entries.Add(matEntry);
		return matEntry.customMat;
	}

	public static void Remove(Material customMat)
	{
		if (!customMat)
		{
			return;
		}
		for (int i = 0; i < s_Entries.Count; i++)
		{
			MatEntry matEntry = s_Entries[i];
			if (!(matEntry.customMat != customMat))
			{
				if (--matEntry.count == 0)
				{
					DestroyImmediate(matEntry.customMat);
					matEntry.baseMat = null;
					matEntry.texture = null;
					s_Entries.RemoveAt(i);
				}
				break;
			}
		}
	}

	private static void DestroyImmediate(Object obj)
	{
		if ((bool)obj)
		{
			if (Application.isEditor)
			{
				Object.DestroyImmediate(obj);
			}
			else
			{
				Object.Destroy(obj);
			}
		}
	}
}
