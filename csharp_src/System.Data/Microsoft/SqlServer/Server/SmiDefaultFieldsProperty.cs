using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Microsoft.SqlServer.Server;

internal class SmiDefaultFieldsProperty : SmiMetaDataProperty
{
	private IList<bool> _defaults;

	internal bool this[int ordinal]
	{
		get
		{
			if (_defaults.Count <= ordinal)
			{
				return false;
			}
			return _defaults[ordinal];
		}
	}

	internal SmiDefaultFieldsProperty(IList<bool> defaultFields)
	{
		_defaults = new ReadOnlyCollection<bool>(defaultFields);
	}

	[Conditional("DEBUG")]
	internal void CheckCount(int countToMatch)
	{
	}
}
