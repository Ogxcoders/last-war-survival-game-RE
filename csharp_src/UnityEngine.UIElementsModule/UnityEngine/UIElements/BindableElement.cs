namespace UnityEngine.UIElements;

public class BindableElement : VisualElement, IBindable
{
	public new class UxmlFactory : UxmlFactory<BindableElement, UxmlTraits>
	{
	}

	public new class UxmlTraits : VisualElement.UxmlTraits
	{
		private UxmlStringAttributeDescription m_PropertyPath;

		public UxmlTraits()
		{
			m_PropertyPath = new UxmlStringAttributeDescription
			{
				name = "binding-path"
			};
		}

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			string valueFromBag = m_PropertyPath.GetValueFromBag(bag, cc);
			if (!string.IsNullOrEmpty(valueFromBag) && ve is IBindable bindable)
			{
				bindable.bindingPath = valueFromBag;
			}
		}
	}

	public IBinding binding { get; set; }

	public string bindingPath { get; set; }
}
