using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements;

public class ListView : BindableElement
{
	public new class UxmlFactory : UxmlFactory<ListView, UxmlTraits>
	{
	}

	public new class UxmlTraits : BindableElement.UxmlTraits
	{
		private UxmlIntAttributeDescription m_ItemHeight = new UxmlIntAttributeDescription
		{
			name = "item-height",
			obsoleteNames = new string[1] { "itemHeight" },
			defaultValue = s_DefaultItemHeight
		};

		public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
		{
			get
			{
				yield break;
			}
		}

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			int value = 0;
			if (m_ItemHeight.TryGetValueFromBag(bag, cc, ref value))
			{
				((ListView)ve).itemHeight = value;
			}
		}
	}

	private class RecycledItem
	{
		public int index;

		public int id;

		public VisualElement element { get; private set; }

		public RecycledItem(VisualElement element)
		{
			this.element = element;
			index = (id = -1);
			element.AddToClassList(itemUssClassName);
		}

		public void DetachElement()
		{
			element.RemoveFromClassList(itemUssClassName);
			element = null;
		}

		public void SetSelected(bool selected)
		{
			if (element != null)
			{
				if (selected)
				{
					element.AddToClassList(itemSelectedVariantUssClassName);
					element.pseudoStates |= PseudoStates.Checked;
				}
				else
				{
					element.RemoveFromClassList(itemSelectedVariantUssClassName);
					element.pseudoStates &= ~PseudoStates.Checked;
				}
			}
		}
	}

	private IList m_ItemsSource;

	private Func<VisualElement> m_MakeItem;

	private Action<VisualElement, int> m_BindItem;

	private Func<int, int> m_GetItemId;

	[SerializeField]
	internal int m_ItemHeight = s_DefaultItemHeight;

	[SerializeField]
	internal bool m_ItemHeightIsInline;

	[SerializeField]
	private float m_ScrollOffset;

	[SerializeField]
	private List<int> m_SelectedIds = new List<int>();

	private List<int> m_SelectedIndices = new List<int>();

	private List<object> m_SelectedItems = new List<object>();

	private int m_RangeSelectionOrigin = -1;

	internal static readonly int s_DefaultItemHeight = 30;

	internal static CustomStyleProperty<int> s_ItemHeightProperty = new CustomStyleProperty<int>("--unity-item-height");

	private int m_FirstVisibleIndex;

	private float m_LastHeight;

	private List<RecycledItem> m_Pool = new List<RecycledItem>();

	private ScrollView m_ScrollView;

	private List<RecycledItem> m_ScrollInsertionList = new List<RecycledItem>();

	private const int k_ExtraVisibleItems = 2;

	private int m_VisibleItemCount;

	public static readonly string ussClassName = "unity-list-view";

	public static readonly string itemUssClassName = ussClassName + "__item";

	public static readonly string itemSelectedVariantUssClassName = itemUssClassName + "--selected";

	public IList itemsSource
	{
		get
		{
			return m_ItemsSource;
		}
		set
		{
			m_ItemsSource = value;
			Refresh();
		}
	}

	public Func<VisualElement> makeItem
	{
		get
		{
			return m_MakeItem;
		}
		set
		{
			if (m_MakeItem != value)
			{
				m_MakeItem = value;
				Refresh();
			}
		}
	}

	public Action<VisualElement, int> bindItem
	{
		get
		{
			return m_BindItem;
		}
		set
		{
			m_BindItem = value;
			Refresh();
		}
	}

	internal Func<int, int> getItemId
	{
		get
		{
			return m_GetItemId;
		}
		set
		{
			m_GetItemId = value;
			Refresh();
		}
	}

	public int itemHeight
	{
		get
		{
			return m_ItemHeight;
		}
		set
		{
			m_ItemHeightIsInline = true;
			m_ItemHeight = value;
			Refresh();
		}
	}

	internal List<int> currentSelectionIds => m_SelectedIds;

	public int selectedIndex
	{
		get
		{
			return (m_SelectedIndices.Count == 0) ? (-1) : m_SelectedIndices.First();
		}
		set
		{
			SetSelection(value);
		}
	}

	public object selectedItem => (m_SelectedItems.Count == 0) ? null : m_SelectedItems.First();

	public override VisualElement contentContainer => m_ScrollView.contentContainer;

	public SelectionType selectionType { get; set; }

	public event Action<object> onItemChosen;

	public event Action<List<object>> onSelectionChanged;

	public ListView()
	{
		AddToClassList(ussClassName);
		selectionType = SelectionType.Single;
		m_ScrollOffset = 0f;
		m_ScrollView = new ScrollView();
		m_ScrollView.viewDataKey = "list-view__scroll-view";
		m_ScrollView.StretchToParentSize();
		m_ScrollView.verticalScroller.valueChanged += OnScroll;
		base.hierarchy.Add(m_ScrollView);
		RegisterCallback<GeometryChangedEvent>(OnSizeChanged);
		RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);
		m_ScrollView.contentContainer.RegisterCallback<MouseDownEvent>(OnClick);
		m_ScrollView.contentContainer.RegisterCallback<KeyDownEvent>(OnKeyDown);
		m_ScrollView.contentContainer.focusable = true;
		m_ScrollView.contentContainer.usageHints &= ~UsageHints.GroupTransform;
		base.focusable = true;
		base.isCompositeRoot = true;
		base.delegatesFocus = true;
	}

	public ListView(IList itemsSource, int itemHeight, Func<VisualElement> makeItem, Action<VisualElement, int> bindItem)
		: this()
	{
		m_ItemsSource = itemsSource;
		m_ItemHeight = itemHeight;
		m_MakeItem = makeItem;
		m_BindItem = bindItem;
	}

	public void OnKeyDown(KeyDownEvent evt)
	{
		if (evt == null || !HasValidDataAndBindings())
		{
			return;
		}
		bool flag = true;
		bool flag2 = true;
		switch (evt.keyCode)
		{
		case KeyCode.UpArrow:
			if (selectedIndex > 0)
			{
				selectedIndex--;
			}
			break;
		case KeyCode.DownArrow:
			if (selectedIndex + 1 < itemsSource.Count)
			{
				selectedIndex++;
			}
			break;
		case KeyCode.Home:
			selectedIndex = 0;
			break;
		case KeyCode.End:
			selectedIndex = itemsSource.Count - 1;
			break;
		case KeyCode.Return:
			if (this.onItemChosen != null && selectedIndex >= 0 && selectedIndex < m_ItemsSource.Count)
			{
				this.onItemChosen(m_ItemsSource[selectedIndex]);
			}
			break;
		case KeyCode.PageDown:
			selectedIndex = Math.Min(itemsSource.Count - 1, selectedIndex + (int)(m_LastHeight / (float)itemHeight));
			break;
		case KeyCode.PageUp:
			selectedIndex = Math.Max(0, selectedIndex - (int)(m_LastHeight / (float)itemHeight));
			break;
		case KeyCode.A:
			if (evt.actionKey)
			{
				SelectAll();
				flag2 = false;
			}
			break;
		case KeyCode.Escape:
			ClearSelection();
			flag2 = false;
			break;
		default:
			flag = false;
			flag2 = false;
			break;
		}
		if (flag)
		{
			evt.StopPropagation();
		}
		if (flag2)
		{
			ScrollToItem(selectedIndex);
		}
	}

	public void ScrollToItem(int index)
	{
		if (!HasValidDataAndBindings())
		{
			throw new InvalidOperationException("Can't scroll without valid source, bind method, or factory method.");
		}
		if (m_VisibleItemCount == 0 || index < -1)
		{
			return;
		}
		if (index == -1)
		{
			int num = (int)(m_LastHeight / (float)itemHeight);
			if (itemsSource.Count < num)
			{
				m_ScrollView.scrollOffset = new Vector2(0f, 0f);
			}
			else
			{
				m_ScrollView.scrollOffset = new Vector2(0f, itemsSource.Count * itemHeight);
			}
			return;
		}
		if (m_FirstVisibleIndex > index)
		{
			m_ScrollView.scrollOffset = Vector2.up * itemHeight * index;
			return;
		}
		int num2 = (int)(m_LastHeight / (float)itemHeight);
		if (index >= m_FirstVisibleIndex + num2)
		{
			bool flag = (int)m_LastHeight % itemHeight != 0;
			int num3 = index - num2;
			if (flag)
			{
				num3++;
			}
			m_ScrollView.scrollOffset = Vector2.up * itemHeight * num3;
		}
	}

	private void OnClick(MouseDownEvent evt)
	{
		if (!HasValidDataAndBindings() || evt.button != 0)
		{
			return;
		}
		int num = (int)(evt.localMousePosition.y / (float)itemHeight);
		if (num > m_ItemsSource.Count - 1)
		{
			return;
		}
		object obj = m_ItemsSource[num];
		int idFromIndex = GetIdFromIndex(num);
		switch (evt.clickCount)
		{
		case 1:
			if (selectionType == SelectionType.None)
			{
				break;
			}
			if (selectionType == SelectionType.Multiple && evt.actionKey)
			{
				m_RangeSelectionOrigin = num;
				if (m_SelectedIds.Contains(idFromIndex))
				{
					RemoveFromSelection(num);
				}
				else
				{
					AddToSelection(num);
				}
			}
			else if (selectionType == SelectionType.Multiple && evt.shiftKey)
			{
				if (m_RangeSelectionOrigin == -1)
				{
					m_RangeSelectionOrigin = num;
					SetSelection(num);
					break;
				}
				foreach (RecycledItem item in m_Pool)
				{
					item.SetSelected(selected: false);
				}
				m_SelectedIds.Clear();
				m_SelectedIndices.Clear();
				m_SelectedItems.Clear();
				if (num < m_RangeSelectionOrigin)
				{
					for (int i = num; i <= m_RangeSelectionOrigin; i++)
					{
						AddToSelection(i);
					}
				}
				else
				{
					for (int j = m_RangeSelectionOrigin; j <= num; j++)
					{
						AddToSelection(j);
					}
				}
			}
			else
			{
				m_RangeSelectionOrigin = num;
				SetSelection(num);
			}
			break;
		case 2:
			if (this.onItemChosen != null)
			{
				this.onItemChosen(obj);
			}
			break;
		}
	}

	internal void SelectAll()
	{
		if (!HasValidDataAndBindings() || selectionType != SelectionType.Multiple)
		{
			return;
		}
		for (int i = 0; i < itemsSource.Count; i++)
		{
			int idFromIndex = GetIdFromIndex(i);
			object item = m_ItemsSource[i];
			foreach (RecycledItem item2 in m_Pool)
			{
				if (item2.id == idFromIndex)
				{
					item2.SetSelected(selected: true);
				}
			}
			if (!m_SelectedIds.Contains(idFromIndex))
			{
				m_SelectedIds.Add(idFromIndex);
				m_SelectedIndices.Add(i);
				m_SelectedItems.Add(item);
			}
		}
		NotifyOfSelectionChange();
		SaveViewData();
	}

	private int GetIdFromIndex(int index)
	{
		if (m_GetItemId == null)
		{
			return index;
		}
		return m_GetItemId(index);
	}

	protected void AddToSelection(int index)
	{
		if (!HasValidDataAndBindings())
		{
			return;
		}
		int idFromIndex = GetIdFromIndex(index);
		object item = m_ItemsSource[index];
		foreach (RecycledItem item2 in m_Pool)
		{
			if (item2.id == idFromIndex)
			{
				item2.SetSelected(selected: true);
			}
		}
		if (!m_SelectedIds.Contains(idFromIndex))
		{
			m_SelectedIds.Add(idFromIndex);
			m_SelectedIndices.Add(index);
			m_SelectedItems.Add(item);
		}
		NotifyOfSelectionChange();
		SaveViewData();
	}

	protected void RemoveFromSelection(int index)
	{
		if (!HasValidDataAndBindings())
		{
			return;
		}
		int idFromIndex = GetIdFromIndex(index);
		object item = m_ItemsSource[index];
		foreach (RecycledItem item2 in m_Pool)
		{
			if (item2.id == idFromIndex)
			{
				item2.SetSelected(selected: false);
			}
		}
		if (m_SelectedIds.Contains(idFromIndex))
		{
			m_SelectedIds.Remove(idFromIndex);
			m_SelectedIndices.Remove(index);
			m_SelectedItems.Remove(item);
		}
		NotifyOfSelectionChange();
		SaveViewData();
	}

	protected void SetSelection(int index)
	{
		if (!HasValidDataAndBindings())
		{
			return;
		}
		if (index < 0)
		{
			ClearSelection();
			return;
		}
		int idFromIndex = GetIdFromIndex(index);
		object item = m_ItemsSource[index];
		foreach (RecycledItem item2 in m_Pool)
		{
			item2.SetSelected(item2.id == idFromIndex);
		}
		m_SelectedIds.Clear();
		m_SelectedIndices.Clear();
		m_SelectedItems.Clear();
		m_SelectedIds.Add(idFromIndex);
		m_SelectedIndices.Add(index);
		m_SelectedItems.Add(item);
		NotifyOfSelectionChange();
		SaveViewData();
	}

	private void NotifyOfSelectionChange()
	{
		if (HasValidDataAndBindings() && this.onSelectionChanged != null)
		{
			this.onSelectionChanged(m_SelectedItems);
		}
	}

	protected void ClearSelection()
	{
		if (!HasValidDataAndBindings())
		{
			return;
		}
		foreach (RecycledItem item in m_Pool)
		{
			item.SetSelected(selected: false);
		}
		m_SelectedIds.Clear();
		m_SelectedIndices.Clear();
		m_SelectedItems.Clear();
		NotifyOfSelectionChange();
	}

	public void ScrollTo(VisualElement visualElement)
	{
		m_ScrollView.ScrollTo(visualElement);
	}

	internal override void OnViewDataReady()
	{
		base.OnViewDataReady();
		string fullHierarchicalViewDataKey = GetFullHierarchicalViewDataKey();
		OverwriteFromViewData(this, fullHierarchicalViewDataKey);
	}

	private void OnScroll(float offset)
	{
		if (!HasValidDataAndBindings())
		{
			return;
		}
		m_ScrollOffset = offset;
		int num = (int)(offset / (float)itemHeight);
		m_ScrollView.contentContainer.style.height = itemsSource.Count * itemHeight;
		if (num == m_FirstVisibleIndex)
		{
			return;
		}
		m_FirstVisibleIndex = num;
		if (m_Pool.Count <= 0)
		{
			return;
		}
		if (m_FirstVisibleIndex < m_Pool[0].index)
		{
			int num2 = m_Pool[0].index - m_FirstVisibleIndex;
			List<RecycledItem> scrollInsertionList = m_ScrollInsertionList;
			for (int i = 0; i < num2; i++)
			{
				if (m_Pool.Count <= 0)
				{
					break;
				}
				RecycledItem recycledItem = m_Pool[m_Pool.Count - 1];
				scrollInsertionList.Add(recycledItem);
				m_Pool.RemoveAt(m_Pool.Count - 1);
				recycledItem.element.SendToBack();
			}
			m_ScrollInsertionList = m_Pool;
			m_Pool = scrollInsertionList;
			m_Pool.AddRange(m_ScrollInsertionList);
			m_ScrollInsertionList.Clear();
		}
		else if (m_FirstVisibleIndex < m_Pool[m_Pool.Count - 1].index)
		{
			List<RecycledItem> scrollInsertionList2 = m_ScrollInsertionList;
			int num3 = 0;
			while (m_FirstVisibleIndex > m_Pool[num3].index)
			{
				RecycledItem recycledItem2 = m_Pool[num3];
				scrollInsertionList2.Add(recycledItem2);
				num3++;
				recycledItem2.element.BringToFront();
			}
			m_Pool.RemoveRange(0, num3);
			m_Pool.AddRange(scrollInsertionList2);
			scrollInsertionList2.Clear();
		}
		for (int j = 0; j < m_Pool.Count && j + m_FirstVisibleIndex < itemsSource.Count; j++)
		{
			Setup(m_Pool[j], j + m_FirstVisibleIndex);
		}
	}

	private bool HasValidDataAndBindings()
	{
		return itemsSource != null && makeItem != null && bindItem != null;
	}

	public void Refresh()
	{
		foreach (RecycledItem item in m_Pool)
		{
			item.DetachElement();
		}
		m_Pool.Clear();
		m_ScrollView.Clear();
		m_SelectedIndices.Clear();
		m_SelectedItems.Clear();
		m_VisibleItemCount = 0;
		if (HasValidDataAndBindings())
		{
			m_LastHeight = m_ScrollView.layout.height;
			if (!float.IsNaN(m_LastHeight))
			{
				ResizeHeight(m_LastHeight);
			}
		}
	}

	private void ResizeHeight(float height)
	{
		int num = itemsSource.Count * itemHeight;
		m_ScrollView.contentContainer.style.height = num;
		float b = Mathf.Max(0f, (float)num - m_ScrollView.contentViewport.layout.height);
		m_ScrollView.verticalScroller.highValue = Mathf.Min(Mathf.Max(m_ScrollOffset, m_ScrollView.verticalScroller.highValue), b);
		m_ScrollView.verticalScroller.value = Mathf.Min(m_ScrollOffset, m_ScrollView.verticalScroller.highValue);
		int num2 = Math.Min((int)(height / (float)itemHeight) + 2, itemsSource.Count);
		if (m_VisibleItemCount != num2)
		{
			if (m_VisibleItemCount > num2)
			{
				int num3 = m_VisibleItemCount - num2;
				for (int i = 0; i < num3; i++)
				{
					int index = m_Pool.Count - 1;
					RecycledItem recycledItem = m_Pool[index];
					recycledItem.element.RemoveFromHierarchy();
					recycledItem.DetachElement();
					m_Pool.RemoveAt(index);
				}
			}
			else
			{
				int num4 = num2 - m_VisibleItemCount;
				for (int j = 0; j < num4; j++)
				{
					int num5 = j + m_FirstVisibleIndex + m_VisibleItemCount;
					VisualElement visualElement = makeItem();
					RecycledItem recycledItem2 = new RecycledItem(visualElement);
					m_Pool.Add(recycledItem2);
					visualElement.AddToClassList("unity-listview-item");
					visualElement.style.marginTop = 0f;
					visualElement.style.marginBottom = 0f;
					visualElement.style.position = Position.Absolute;
					visualElement.style.left = 0f;
					visualElement.style.right = 0f;
					visualElement.style.height = itemHeight;
					if (num5 < itemsSource.Count)
					{
						Setup(recycledItem2, num5);
					}
					else
					{
						visualElement.style.visibility = Visibility.Hidden;
					}
					Add(visualElement);
				}
			}
			m_VisibleItemCount = num2;
		}
		m_LastHeight = height;
		if (m_SelectedIds.Count <= 0)
		{
			return;
		}
		for (int k = 0; k < m_ItemsSource.Count; k++)
		{
			if (m_SelectedIds.Contains(GetIdFromIndex(k)))
			{
				m_SelectedIndices.Add(k);
				m_SelectedItems.Add(m_ItemsSource[k]);
			}
		}
	}

	private void Setup(RecycledItem recycledItem, int newIndex)
	{
		int idFromIndex = GetIdFromIndex(newIndex);
		recycledItem.element.style.visibility = Visibility.Visible;
		if (recycledItem.index != newIndex)
		{
			recycledItem.index = newIndex;
			recycledItem.id = idFromIndex;
			recycledItem.element.style.top = recycledItem.index * itemHeight;
			recycledItem.element.style.bottom = (itemsSource.Count - recycledItem.index - 1) * itemHeight;
			bindItem(recycledItem.element, recycledItem.index);
			recycledItem.SetSelected(m_SelectedIds.Contains(idFromIndex));
		}
	}

	private void OnSizeChanged(GeometryChangedEvent evt)
	{
		if (HasValidDataAndBindings() && evt.newRect.height != evt.oldRect.height)
		{
			ResizeHeight(evt.newRect.height);
		}
	}

	private void OnCustomStyleResolved(CustomStyleResolvedEvent e)
	{
		int value = 0;
		if (!m_ItemHeightIsInline && e.customStyle.TryGetValue(s_ItemHeightProperty, out value))
		{
			itemHeight = value;
		}
	}
}
