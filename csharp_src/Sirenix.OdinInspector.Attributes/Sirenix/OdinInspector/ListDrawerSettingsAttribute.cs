using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
[Conditional("UNITY_EDITOR")]
[DontApplyToListElements]
public sealed class ListDrawerSettingsAttribute : Attribute
{
	public bool HideAddButton;

	public bool HideRemoveButton;

	public string ListElementLabelName;

	public string CustomAddFunction;

	public string CustomRemoveIndexFunction;

	public string CustomRemoveElementFunction;

	public string OnBeginListElementGUI;

	public string OnEndListElementGUI;

	public bool AlwaysAddDefaultValue;

	public bool AddCopiesLastElement;

	public string ElementColor;

	private string onTitleBarGUI;

	private int numberOfItemsPerPage;

	private bool paging;

	private bool draggable;

	private bool isReadOnly;

	private bool showItemCount;

	private bool pagingHasValue;

	private bool draggableHasValue;

	private bool isReadOnlyHasValue;

	private bool showItemCountHasValue;

	private bool expanded;

	private bool expandedHasValue;

	private bool numberOfItemsPerPageHasValue;

	private bool showIndexLabels;

	private bool showIndexLabelsHasValue;

	public bool ShowPaging
	{
		get
		{
			return paging;
		}
		set
		{
			paging = value;
			pagingHasValue = true;
		}
	}

	public bool DraggableItems
	{
		get
		{
			return draggable;
		}
		set
		{
			draggable = value;
			draggableHasValue = true;
		}
	}

	public int NumberOfItemsPerPage
	{
		get
		{
			return numberOfItemsPerPage;
		}
		set
		{
			numberOfItemsPerPage = value;
			numberOfItemsPerPageHasValue = true;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return isReadOnly;
		}
		set
		{
			isReadOnly = value;
			isReadOnlyHasValue = true;
		}
	}

	public bool ShowItemCount
	{
		get
		{
			return showItemCount;
		}
		set
		{
			showItemCount = value;
			showItemCountHasValue = true;
		}
	}

	public bool Expanded
	{
		get
		{
			return expanded;
		}
		set
		{
			expanded = value;
			expandedHasValue = true;
		}
	}

	public bool ShowIndexLabels
	{
		get
		{
			return showIndexLabels;
		}
		set
		{
			showIndexLabels = value;
			showIndexLabelsHasValue = true;
		}
	}

	public string OnTitleBarGUI
	{
		get
		{
			return onTitleBarGUI;
		}
		set
		{
			onTitleBarGUI = value;
		}
	}

	public bool PagingHasValue => pagingHasValue;

	public bool ShowItemCountHasValue => showItemCountHasValue;

	public bool NumberOfItemsPerPageHasValue => numberOfItemsPerPageHasValue;

	public bool DraggableHasValue => draggableHasValue;

	public bool IsReadOnlyHasValue => isReadOnlyHasValue;

	public bool ExpandedHasValue => expandedHasValue;

	public bool ShowIndexLabelsHasValue => showIndexLabelsHasValue;
}
