using UnityEngine;

namespace Spine.Unity;

public static class ISpineComponentExtensions
{
	public static bool IsNullOrDestroyed(this ISpineComponent component)
	{
		if (component == null)
		{
			return true;
		}
		return (Object)component == null;
	}
}
