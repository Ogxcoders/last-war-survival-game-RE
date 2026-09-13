using System.Collections.Generic;

namespace UnityEngine.UIElements;

public class TemplateContainer : BindableElement
{
	public new class UxmlFactory : UxmlFactory<TemplateContainer, UxmlTraits>
	{
	}

	public new class UxmlTraits : BindableElement.UxmlTraits
	{
		private UxmlStringAttributeDescription m_Template = new UxmlStringAttributeDescription
		{
			name = "template",
			use = UxmlAttributeDescription.Use.Required
		};

		public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
		{
			get
			{
				yield break;
			}
		}

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			TemplateContainer templateContainer = (TemplateContainer)ve;
			templateContainer.templateId = m_Template.GetValueFromBag(bag, cc);
			VisualTreeAsset visualTreeAsset = cc.visualTreeAsset.ResolveTemplate(templateContainer.templateId);
			if (visualTreeAsset == null)
			{
				templateContainer.Add(new Label($"Unknown Element: '{templateContainer.templateId}'"));
			}
			else
			{
				List<TemplateAsset.AttributeOverride> list = (bag as TemplateAsset)?.attributeOverrides;
				List<TemplateAsset.AttributeOverride> attributeOverrides = cc.attributeOverrides;
				List<TemplateAsset.AttributeOverride> list2 = null;
				if (list != null || attributeOverrides != null)
				{
					list2 = new List<TemplateAsset.AttributeOverride>();
					if (attributeOverrides != null)
					{
						list2.AddRange(attributeOverrides);
					}
					if (list != null)
					{
						list2.AddRange(list);
					}
				}
				visualTreeAsset.CloneTree(templateContainer, cc.slotInsertionPoints, list2);
			}
			if (visualTreeAsset == null)
			{
				Debug.LogErrorFormat("Could not resolve template with name '{0}'", templateContainer.templateId);
			}
		}
	}

	private VisualElement m_ContentContainer;

	public string templateId { get; private set; }

	public override VisualElement contentContainer => m_ContentContainer;

	public TemplateContainer()
		: this(null)
	{
	}

	public TemplateContainer(string templateId)
	{
		this.templateId = templateId;
		m_ContentContainer = this;
	}

	internal void SetContentContainer(VisualElement content)
	{
		m_ContentContainer = content;
	}
}
