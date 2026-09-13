using System.Collections.ObjectModel;
using System.Globalization;

namespace System.Data.ProviderBase;

internal sealed class FieldNameLookup : BasicFieldNameLookup
{
	private readonly int _defaultLocaleID;

	public FieldNameLookup(string[] fieldNames, int defaultLocaleID)
		: base(fieldNames)
	{
		_defaultLocaleID = defaultLocaleID;
	}

	public FieldNameLookup(ReadOnlyCollection<string> columnNames, int defaultLocaleID)
		: base(columnNames)
	{
		_defaultLocaleID = defaultLocaleID;
	}

	public FieldNameLookup(IDataReader reader, int defaultLocaleID)
		: base(reader)
	{
		_defaultLocaleID = defaultLocaleID;
	}

	protected override CompareInfo GetCompareInfo()
	{
		CompareInfo compareInfo = null;
		if (-1 != _defaultLocaleID)
		{
			compareInfo = CompareInfo.GetCompareInfo(_defaultLocaleID);
		}
		if (compareInfo == null)
		{
			compareInfo = base.GetCompareInfo();
		}
		return compareInfo;
	}
}
