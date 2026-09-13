using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Microsoft.SqlServer.Server;

internal class SmiOrderProperty : SmiMetaDataProperty
{
	internal struct SmiColumnOrder
	{
		internal int SortOrdinal;

		internal SortOrder Order;
	}

	private IList<SmiColumnOrder> _columns;

	internal SmiColumnOrder this[int ordinal]
	{
		get
		{
			if (_columns.Count <= ordinal)
			{
				return new SmiColumnOrder
				{
					Order = SortOrder.Unspecified,
					SortOrdinal = -1
				};
			}
			return _columns[ordinal];
		}
	}

	internal SmiOrderProperty(IList<SmiColumnOrder> columnOrders)
	{
		_columns = new ReadOnlyCollection<SmiColumnOrder>(columnOrders);
	}

	[Conditional("DEBUG")]
	internal void CheckCount(int countToMatch)
	{
	}
}
