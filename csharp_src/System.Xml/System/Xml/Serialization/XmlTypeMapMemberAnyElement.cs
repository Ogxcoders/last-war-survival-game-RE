namespace System.Xml.Serialization;

internal class XmlTypeMapMemberAnyElement : XmlTypeMapMemberExpandable
{
	public bool IsDefaultAny
	{
		get
		{
			foreach (XmlTypeMapElementInfo item in base.ElementInfo)
			{
				if (item.IsUnnamedAnyElement)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool CanBeText
	{
		get
		{
			if (base.ElementInfo.Count > 0)
			{
				return ((XmlTypeMapElementInfo)base.ElementInfo[0]).IsTextElement;
			}
			return false;
		}
	}

	public bool IsElementDefined(string name, string ns)
	{
		foreach (XmlTypeMapElementInfo item in base.ElementInfo)
		{
			if (item.IsUnnamedAnyElement)
			{
				return true;
			}
			if (item.ElementName == name && item.Namespace == ns)
			{
				return true;
			}
		}
		return false;
	}
}
