using System.Collections;

namespace System.ComponentModel;

public interface IBindingListView : IBindingList, IList, ICollection, IEnumerable
{
	string Filter { get; set; }

	ListSortDescriptionCollection SortDescriptions { get; }

	bool SupportsAdvancedSorting { get; }

	bool SupportsFiltering { get; }

	void ApplySort(ListSortDescriptionCollection sorts);

	void RemoveFilter();
}
