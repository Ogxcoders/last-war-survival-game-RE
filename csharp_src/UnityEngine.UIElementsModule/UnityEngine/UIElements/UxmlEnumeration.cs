using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements;

public class UxmlEnumeration : UxmlTypeRestriction
{
	private List<string> m_Values = new List<string>();

	public IEnumerable<string> values
	{
		get
		{
			return m_Values;
		}
		set
		{
			m_Values = value.ToList();
		}
	}

	public override bool Equals(UxmlTypeRestriction other)
	{
		if (!(other is UxmlEnumeration uxmlEnumeration))
		{
			return false;
		}
		return values.All(uxmlEnumeration.values.Contains<string>) && values.Count() == uxmlEnumeration.values.Count();
	}
}
