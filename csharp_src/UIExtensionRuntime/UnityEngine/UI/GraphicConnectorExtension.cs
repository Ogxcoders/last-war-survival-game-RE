namespace UnityEngine.UI;

internal static class GraphicConnectorExtension
{
	public static void SetVerticesDirtyEx(this Graphic graphic)
	{
		GraphicConnector.FindConnector(graphic).SetVerticesDirty(graphic);
	}

	public static void SetMaterialDirtyEx(this Graphic graphic)
	{
		GraphicConnector.FindConnector(graphic).SetMaterialDirty(graphic);
	}

	public static T GetComponentInParentEx<T>(this Component component, bool includeInactive = false) where T : MonoBehaviour
	{
		if (!component)
		{
			return null;
		}
		Transform transform = component.transform;
		while ((bool)transform)
		{
			T component2 = transform.GetComponent<T>();
			if ((bool)component2 && (includeInactive || component2.isActiveAndEnabled))
			{
				return component2;
			}
			transform = transform.parent;
		}
		return null;
	}
}
