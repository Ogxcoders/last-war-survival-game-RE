using System.Collections.Generic;

namespace UnityEngine.UIElements;

public class UxmlRootElementTraits : UxmlTraits
{
	public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription => new UxmlChildElementDescription[1]
	{
		new UxmlChildElementDescription(typeof(VisualElement))
	};

	public UxmlRootElementTraits()
	{
		base.canHaveAnyAttribute = false;
	}
}
