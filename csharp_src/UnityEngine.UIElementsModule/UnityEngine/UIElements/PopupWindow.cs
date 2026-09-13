using System.Collections.Generic;

namespace UnityEngine.UIElements;

public class PopupWindow : TextElement
{
	public new class UxmlFactory : UxmlFactory<PopupWindow, UxmlTraits>
	{
	}

	public new class UxmlTraits : TextElement.UxmlTraits
	{
		public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
		{
			get
			{
				yield return new UxmlChildElementDescription(typeof(VisualElement));
			}
		}
	}

	private VisualElement m_ContentContainer;

	public new static readonly string ussClassName = "unity-popup-window";

	public static readonly string contentUssClassName = ussClassName + "__content-container";

	public override VisualElement contentContainer => m_ContentContainer;

	public PopupWindow()
	{
		AddToClassList(ussClassName);
		m_ContentContainer = new VisualElement
		{
			name = "unity-content-container"
		};
		m_ContentContainer.AddToClassList(contentUssClassName);
		base.hierarchy.Add(m_ContentContainer);
	}
}
