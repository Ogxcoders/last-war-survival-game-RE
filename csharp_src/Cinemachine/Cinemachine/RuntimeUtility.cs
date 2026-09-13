using UnityEngine;

namespace Cinemachine;

[DocumentationSorting(DocumentationSortingAttribute.Level.Undoc)]
public static class RuntimeUtility
{
	public static void DestroyObject(Object obj)
	{
		if (obj != null)
		{
			Object.Destroy(obj);
		}
	}
}
