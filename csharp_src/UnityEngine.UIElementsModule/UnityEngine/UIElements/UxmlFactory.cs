using System.Collections.Generic;

namespace UnityEngine.UIElements;

public class UxmlFactory<TCreatedType, TTraits> : IUxmlFactory where TCreatedType : VisualElement, new() where TTraits : UxmlTraits, new()
{
	internal TTraits m_Traits;

	public virtual string uxmlName => typeof(TCreatedType).Name;

	public virtual string uxmlNamespace => typeof(TCreatedType).Namespace ?? string.Empty;

	public virtual string uxmlQualifiedName => typeof(TCreatedType).FullName;

	public bool canHaveAnyAttribute => m_Traits.canHaveAnyAttribute;

	public virtual IEnumerable<UxmlAttributeDescription> uxmlAttributesDescription
	{
		get
		{
			foreach (UxmlAttributeDescription item in m_Traits.uxmlAttributesDescription)
			{
				yield return item;
			}
		}
	}

	public virtual IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
	{
		get
		{
			foreach (UxmlChildElementDescription item in m_Traits.uxmlChildElementsDescription)
			{
				yield return item;
			}
		}
	}

	public virtual string substituteForTypeName
	{
		get
		{
			if ((object)typeof(TCreatedType) == typeof(VisualElement))
			{
				return string.Empty;
			}
			return typeof(VisualElement).Name;
		}
	}

	public virtual string substituteForTypeNamespace
	{
		get
		{
			if ((object)typeof(TCreatedType) == typeof(VisualElement))
			{
				return string.Empty;
			}
			return typeof(VisualElement).Namespace ?? string.Empty;
		}
	}

	public virtual string substituteForTypeQualifiedName
	{
		get
		{
			if ((object)typeof(TCreatedType) == typeof(VisualElement))
			{
				return string.Empty;
			}
			return typeof(VisualElement).FullName;
		}
	}

	protected UxmlFactory()
	{
		m_Traits = new TTraits();
	}

	public virtual bool AcceptsAttributeBag(IUxmlAttributes bag, CreationContext cc)
	{
		return true;
	}

	public virtual VisualElement Create(IUxmlAttributes bag, CreationContext cc)
	{
		TCreatedType val = new TCreatedType();
		m_Traits.Init(val, bag, cc);
		return val;
	}
}
public class UxmlFactory<TCreatedType> : UxmlFactory<TCreatedType, VisualElement.UxmlTraits> where TCreatedType : VisualElement, new()
{
}
