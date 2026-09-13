using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements;

public abstract class UxmlAttributeDescription
{
	public enum Use
	{
		None,
		Optional,
		Prohibited,
		Required
	}

	protected const string xmlSchemaNamespace = "http://www.w3.org/2001/XMLSchema";

	private string[] m_ObsoleteNames;

	public string name { get; set; }

	public IEnumerable<string> obsoleteNames
	{
		get
		{
			return m_ObsoleteNames;
		}
		set
		{
			m_ObsoleteNames = value.ToArray();
		}
	}

	public string type { get; protected set; }

	public string typeNamespace { get; protected set; }

	public abstract string defaultValueAsString { get; }

	public Use use { get; set; }

	public UxmlTypeRestriction restriction { get; set; }

	protected UxmlAttributeDescription()
	{
		use = Use.Optional;
		restriction = null;
	}

	internal bool TryGetValueFromBagAsString(IUxmlAttributes bag, CreationContext cc, out string value)
	{
		if (name == null && (m_ObsoleteNames == null || m_ObsoleteNames.Length == 0))
		{
			Debug.LogError("Attribute description has no name.");
			value = null;
			return false;
		}
		bag.TryGetAttributeValue("name", out var value2);
		if (!string.IsNullOrEmpty(value2) && cc.attributeOverrides != null)
		{
			for (int i = 0; i < cc.attributeOverrides.Count; i++)
			{
				if (cc.attributeOverrides[i].m_ElementName != value2)
				{
					continue;
				}
				if (cc.attributeOverrides[i].m_AttributeName != name)
				{
					if (m_ObsoleteNames == null)
					{
						continue;
					}
					bool flag = false;
					for (int j = 0; j < m_ObsoleteNames.Length; j++)
					{
						if (cc.attributeOverrides[i].m_AttributeName == m_ObsoleteNames[j])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						continue;
					}
				}
				value = cc.attributeOverrides[i].m_Value;
				return true;
			}
		}
		if (name == null)
		{
			for (int k = 0; k < m_ObsoleteNames.Length; k++)
			{
				if (bag.TryGetAttributeValue(m_ObsoleteNames[k], out value))
				{
					if (cc.visualTreeAsset != null)
					{
					}
					return true;
				}
			}
			value = null;
			return false;
		}
		if (!bag.TryGetAttributeValue(name, out value))
		{
			if (m_ObsoleteNames != null)
			{
				for (int l = 0; l < m_ObsoleteNames.Length; l++)
				{
					if (bag.TryGetAttributeValue(m_ObsoleteNames[l], out value))
					{
						if (cc.visualTreeAsset != null)
						{
						}
						return true;
					}
				}
			}
			value = null;
			return false;
		}
		return true;
	}

	protected bool TryGetValueFromBag<T>(IUxmlAttributes bag, CreationContext cc, Func<string, T, T> converterFunc, T defaultValue, ref T value)
	{
		if (TryGetValueFromBagAsString(bag, cc, out var value2))
		{
			if (converterFunc != null)
			{
				value = converterFunc(value2, defaultValue);
			}
			else
			{
				value = defaultValue;
			}
			return true;
		}
		return false;
	}

	protected T GetValueFromBag<T>(IUxmlAttributes bag, CreationContext cc, Func<string, T, T> converterFunc, T defaultValue)
	{
		if (converterFunc == null)
		{
			throw new ArgumentNullException("converterFunc");
		}
		if (TryGetValueFromBagAsString(bag, cc, out var value))
		{
			return converterFunc(value, defaultValue);
		}
		return defaultValue;
	}
}
