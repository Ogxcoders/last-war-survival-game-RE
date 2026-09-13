using System.Collections;

namespace System.ComponentModel;

[MergableProperty(false)]
public interface IListSource
{
	bool ContainsListCollection { get; }

	IList GetList();
}
