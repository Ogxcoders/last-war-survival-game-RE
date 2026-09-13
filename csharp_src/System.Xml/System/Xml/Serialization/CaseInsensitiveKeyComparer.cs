using System.Collections;
using System.Globalization;

namespace System.Xml.Serialization;

internal class CaseInsensitiveKeyComparer : CaseInsensitiveComparer, IEqualityComparer
{
	public CaseInsensitiveKeyComparer()
		: base(CultureInfo.CurrentCulture)
	{
	}

	bool IEqualityComparer.Equals(object x, object y)
	{
		return Compare(x, y) == 0;
	}

	int IEqualityComparer.GetHashCode(object obj)
	{
		return ((obj as string) ?? throw new ArgumentException(null, "obj")).ToUpper(CultureInfo.CurrentCulture).GetHashCode();
	}
}
