using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Microsoft.SqlServer.Server;

internal class SmiUniqueKeyProperty : SmiMetaDataProperty
{
	private IList<bool> _columns;

	internal bool this[int ordinal]
	{
		get
		{
			if (_columns.Count <= ordinal)
			{
				return false;
			}
			return _columns[ordinal];
		}
	}

	internal SmiUniqueKeyProperty(IList<bool> columnIsKey)
	{
		_columns = new ReadOnlyCollection<bool>(columnIsKey);
	}

	[Conditional("DEBUG")]
	internal void CheckCount(int countToMatch)
	{
	}
}
