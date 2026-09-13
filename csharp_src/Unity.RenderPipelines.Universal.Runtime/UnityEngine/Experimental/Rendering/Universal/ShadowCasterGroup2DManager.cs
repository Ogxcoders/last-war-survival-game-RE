using System.Collections.Generic;

namespace UnityEngine.Experimental.Rendering.Universal;

internal class ShadowCasterGroup2DManager
{
	private static List<ShadowCasterGroup2D> s_ShadowCasterGroups;

	public static List<ShadowCasterGroup2D> shadowCasterGroups => s_ShadowCasterGroups;

	public static void AddGroup(ShadowCasterGroup2D group)
	{
		if (!(group == null))
		{
			if (s_ShadowCasterGroups == null)
			{
				s_ShadowCasterGroups = new List<ShadowCasterGroup2D>();
			}
			LightUtility.AddShadowCasterGroupToList(group, s_ShadowCasterGroups);
		}
	}

	public static void RemoveGroup(ShadowCasterGroup2D group)
	{
		if (group != null && s_ShadowCasterGroups != null)
		{
			LightUtility.RemoveShadowCasterGroupFromList(group, s_ShadowCasterGroups);
		}
	}
}
