using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityEngine.UIElements;

public abstract class UxmlTraits
{
	public bool canHaveAnyAttribute { get; protected set; }

	public virtual IEnumerable<UxmlAttributeDescription> uxmlAttributesDescription
	{
		get
		{
			foreach (UxmlAttributeDescription item in GetAllAttributeDescriptionForType(GetType()))
			{
				yield return item;
			}
		}
	}

	public virtual IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
	{
		get
		{
			yield break;
		}
	}

	protected UxmlTraits()
	{
		canHaveAnyAttribute = true;
	}

	public virtual void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
	{
	}

	private IEnumerable<UxmlAttributeDescription> GetAllAttributeDescriptionForType(Type t)
	{
		Type baseType = t.BaseType;
		if ((object)baseType != null)
		{
			foreach (UxmlAttributeDescription item in GetAllAttributeDescriptionForType(baseType))
			{
				yield return item;
			}
		}
		foreach (FieldInfo fieldInfo in from f in t.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			where typeof(UxmlAttributeDescription).IsAssignableFrom(f.FieldType)
			select f)
		{
			yield return (UxmlAttributeDescription)fieldInfo.GetValue(this);
		}
	}
}
