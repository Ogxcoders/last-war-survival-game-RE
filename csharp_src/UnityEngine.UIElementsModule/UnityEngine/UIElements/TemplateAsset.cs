#define UNITY_ASSERTIONS
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace UnityEngine.UIElements;

[Serializable]
internal class TemplateAsset : VisualElementAsset
{
	[Serializable]
	public struct AttributeOverride
	{
		public string m_ElementName;

		public string m_AttributeName;

		public string m_Value;
	}

	[SerializeField]
	private string m_TemplateAlias;

	[SerializeField]
	private List<AttributeOverride> m_AttributeOverrides;

	[SerializeField]
	private List<VisualTreeAsset.SlotUsageEntry> m_SlotUsages;

	public string templateAlias
	{
		get
		{
			return m_TemplateAlias;
		}
		set
		{
			m_TemplateAlias = value;
		}
	}

	public List<AttributeOverride> attributeOverrides
	{
		get
		{
			return (m_AttributeOverrides == null) ? (m_AttributeOverrides = new List<AttributeOverride>()) : m_AttributeOverrides;
		}
		set
		{
			m_AttributeOverrides = value;
		}
	}

	internal List<VisualTreeAsset.SlotUsageEntry> slotUsages
	{
		get
		{
			return m_SlotUsages;
		}
		set
		{
			m_SlotUsages = value;
		}
	}

	public TemplateAsset(string templateAlias)
		: base(typeof(TemplateContainer).FullName)
	{
		Assert.IsFalse(string.IsNullOrEmpty(templateAlias), "Template alias must not be null or empty");
		m_TemplateAlias = templateAlias;
	}

	public void AddSlotUsage(string slotName, int resId)
	{
		if (m_SlotUsages == null)
		{
			m_SlotUsages = new List<VisualTreeAsset.SlotUsageEntry>();
		}
		m_SlotUsages.Add(new VisualTreeAsset.SlotUsageEntry(slotName, resId));
	}
}
