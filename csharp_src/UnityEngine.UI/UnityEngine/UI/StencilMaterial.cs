using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.Rendering;

namespace UnityEngine.UI;

public static class StencilMaterial
{
	private class MatEntry
	{
		public Material baseMat;

		public Material customMat;

		public int count;

		public int stencilId;

		public StencilOp operation;

		public CompareFunction compareFunction = CompareFunction.Always;

		public int readMask;

		public int writeMask;

		public bool useAlphaClip;

		public ColorWriteMask colorMask;
	}

	private static List<MatEntry> m_List = new List<MatEntry>();

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use Material.Add instead.", true)]
	public static Material Add(Material baseMat, int stencilID)
	{
		return null;
	}

	public static Material Add(Material baseMat, int stencilID, StencilOp operation, CompareFunction compareFunction, ColorWriteMask colorWriteMask)
	{
		return Add(baseMat, stencilID, operation, compareFunction, colorWriteMask, 255, 255);
	}

	public static Material Add(Material baseMat, int stencilID, StencilOp operation, CompareFunction compareFunction, ColorWriteMask colorWriteMask, int readMask, int writeMask)
	{
		if ((stencilID <= 0 && colorWriteMask == ColorWriteMask.All) || baseMat == null)
		{
			return baseMat;
		}
		if (!baseMat.HasProperty("_Stencil"))
		{
			Debug.LogWarning("Material " + baseMat.name + " doesn't have _Stencil property", baseMat);
			return baseMat;
		}
		if (!baseMat.HasProperty("_StencilOp"))
		{
			Debug.LogWarning("Material " + baseMat.name + " doesn't have _StencilOp property", baseMat);
			return baseMat;
		}
		if (!baseMat.HasProperty("_StencilComp"))
		{
			Debug.LogWarning("Material " + baseMat.name + " doesn't have _StencilComp property", baseMat);
			return baseMat;
		}
		if (!baseMat.HasProperty("_StencilReadMask"))
		{
			Debug.LogWarning("Material " + baseMat.name + " doesn't have _StencilReadMask property", baseMat);
			return baseMat;
		}
		if (!baseMat.HasProperty("_StencilWriteMask"))
		{
			Debug.LogWarning("Material " + baseMat.name + " doesn't have _StencilWriteMask property", baseMat);
			return baseMat;
		}
		if (!baseMat.HasProperty("_ColorMask"))
		{
			Debug.LogWarning("Material " + baseMat.name + " doesn't have _ColorMask property", baseMat);
			return baseMat;
		}
		for (int i = 0; i < m_List.Count; i++)
		{
			MatEntry matEntry = m_List[i];
			if (matEntry.baseMat == baseMat && matEntry.stencilId == stencilID && matEntry.operation == operation && matEntry.compareFunction == compareFunction && matEntry.readMask == readMask && matEntry.writeMask == writeMask && matEntry.colorMask == colorWriteMask)
			{
				matEntry.count++;
				return matEntry.customMat;
			}
		}
		MatEntry matEntry2 = new MatEntry();
		matEntry2.count = 1;
		matEntry2.baseMat = baseMat;
		matEntry2.customMat = new Material(baseMat);
		matEntry2.customMat.hideFlags = HideFlags.HideAndDontSave;
		matEntry2.stencilId = stencilID;
		matEntry2.operation = operation;
		matEntry2.compareFunction = compareFunction;
		matEntry2.readMask = readMask;
		matEntry2.writeMask = writeMask;
		matEntry2.colorMask = colorWriteMask;
		matEntry2.useAlphaClip = operation != StencilOp.Keep && writeMask > 0;
		matEntry2.customMat.name = $"Stencil Id:{stencilID}, Op:{operation}, Comp:{compareFunction}, WriteMask:{writeMask}, ReadMask:{readMask}, ColorMask:{colorWriteMask} AlphaClip:{matEntry2.useAlphaClip} ({baseMat.name})";
		matEntry2.customMat.SetInt("_Stencil", stencilID);
		matEntry2.customMat.SetInt("_StencilOp", (int)operation);
		matEntry2.customMat.SetInt("_StencilComp", (int)compareFunction);
		matEntry2.customMat.SetInt("_StencilReadMask", readMask);
		matEntry2.customMat.SetInt("_StencilWriteMask", writeMask);
		matEntry2.customMat.SetInt("_ColorMask", (int)colorWriteMask);
		matEntry2.customMat.SetInt("_UseUIAlphaClip", matEntry2.useAlphaClip ? 1 : 0);
		if (matEntry2.useAlphaClip)
		{
			matEntry2.customMat.EnableKeyword("UNITY_UI_ALPHACLIP");
		}
		else
		{
			matEntry2.customMat.DisableKeyword("UNITY_UI_ALPHACLIP");
		}
		m_List.Add(matEntry2);
		return matEntry2.customMat;
	}

	public static void Remove(Material customMat)
	{
		if (customMat == null)
		{
			return;
		}
		for (int i = 0; i < m_List.Count; i++)
		{
			MatEntry matEntry = m_List[i];
			if (!(matEntry.customMat != customMat))
			{
				if (--matEntry.count == 0)
				{
					Misc.DestroyImmediate(matEntry.customMat);
					matEntry.baseMat = null;
					m_List.RemoveAt(i);
				}
				break;
			}
		}
	}

	public static void ClearAll()
	{
		for (int i = 0; i < m_List.Count; i++)
		{
			MatEntry matEntry = m_List[i];
			Misc.DestroyImmediate(matEntry.customMat);
			matEntry.baseMat = null;
		}
		m_List.Clear();
	}
}
