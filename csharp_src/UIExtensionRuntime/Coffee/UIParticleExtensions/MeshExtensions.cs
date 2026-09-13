using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIParticleExtensions;

internal static class MeshExtensions
{
	private static readonly List<Color32> s_Colors = new List<Color32>();

	public static void ModifyColorSpaceToLinear(this Mesh self)
	{
		self.GetColors(s_Colors);
		for (int i = 0; i < s_Colors.Count; i++)
		{
			s_Colors[i] = ((Color)s_Colors[i]).gamma;
		}
		self.SetColors(s_Colors);
		s_Colors.Clear();
	}

	public static void Clear(this CombineInstance[] self)
	{
		for (int i = 0; i < self.Length; i++)
		{
			MeshPool.Return(self[i].mesh);
			self[i].mesh = null;
		}
	}
}
