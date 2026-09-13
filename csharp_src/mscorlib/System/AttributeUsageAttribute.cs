using System.Runtime.InteropServices;

namespace System;

[Serializable]
[AttributeUsage(AttributeTargets.Class, Inherited = true)]
[ComVisible(true)]
public sealed class AttributeUsageAttribute : Attribute
{
	internal AttributeTargets m_attributeTarget = AttributeTargets.All;

	internal bool m_allowMultiple;

	internal bool m_inherited = true;

	internal static AttributeUsageAttribute Default = new AttributeUsageAttribute(AttributeTargets.All);

	public AttributeTargets ValidOn => m_attributeTarget;

	public bool AllowMultiple
	{
		get
		{
			return m_allowMultiple;
		}
		set
		{
			m_allowMultiple = value;
		}
	}

	public bool Inherited
	{
		get
		{
			return m_inherited;
		}
		set
		{
			m_inherited = value;
		}
	}

	public AttributeUsageAttribute(AttributeTargets validOn)
	{
		m_attributeTarget = validOn;
	}

	internal AttributeUsageAttribute(AttributeTargets validOn, bool allowMultiple, bool inherited)
	{
		m_attributeTarget = validOn;
		m_allowMultiple = allowMultiple;
		m_inherited = inherited;
	}
}
