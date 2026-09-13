namespace UnityEngine.UIElements.StyleSheets;

internal abstract class HierarchyTraversal
{
	public virtual void Traverse(VisualElement element)
	{
		TraverseRecursive(element, 0);
	}

	public abstract void TraverseRecursive(VisualElement element, int depth);

	protected void Recurse(VisualElement element, int depth)
	{
		int num = 0;
		while (num < element.hierarchy.childCount)
		{
			VisualElement visualElement = element.hierarchy[num];
			TraverseRecursive(visualElement, depth + 1);
			if (visualElement.hierarchy.parent == element)
			{
				num++;
			}
		}
	}
}
