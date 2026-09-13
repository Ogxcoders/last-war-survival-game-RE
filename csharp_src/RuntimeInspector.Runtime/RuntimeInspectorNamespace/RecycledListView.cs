using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

[RequireComponent(typeof(ScrollRect))]
public class RecycledListView : MonoBehaviour
{
	[SerializeField]
	private RectTransform viewportTransform;

	[SerializeField]
	private RectTransform contentTransform;

	private float itemHeight;

	private float _1OverItemHeight;

	private float m_viewportWidth;

	private float m_viewportHeight;

	private readonly Dictionary<int, RecycledListItem> items = new Dictionary<int, RecycledListItem>();

	private readonly Stack<RecycledListItem> pooledItems = new Stack<RecycledListItem>();

	private IListViewAdapter adapter;

	private bool isDirty;

	private int currentTopIndex = -1;

	private int currentBottomIndex = -1;

	public float ViewportWidth => m_viewportWidth;

	public float ViewportHeight => m_viewportHeight;

	private void Start()
	{
		GetComponent<ScrollRect>().onValueChanged.AddListener(delegate
		{
			UpdateItemsInTheList();
		});
	}

	private void Update()
	{
		if (isDirty)
		{
			Vector2 size = viewportTransform.rect.size;
			m_viewportWidth = size.x;
			m_viewportHeight = size.y;
			isDirty = false;
			UpdateItemsInTheList();
		}
	}

	public void SetAdapter(IListViewAdapter adapter)
	{
		this.adapter = adapter;
		itemHeight = adapter.ItemHeight;
		_1OverItemHeight = 1f / itemHeight;
	}

	public void UpdateList(bool resetContentPosition = true)
	{
		if (resetContentPosition)
		{
			contentTransform.anchoredPosition = Vector2.zero;
		}
		float y = Mathf.Max(1f, (float)adapter.Count * itemHeight);
		contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, y);
		Vector2 size = viewportTransform.rect.size;
		m_viewportWidth = size.x;
		m_viewportHeight = size.y;
		UpdateItemsInTheList(updateAllVisibleItems: true);
	}

	public void ResetList()
	{
		itemHeight = adapter.ItemHeight;
		_1OverItemHeight = 1f / itemHeight;
		if (currentTopIndex > -1 && currentBottomIndex > -1)
		{
			if (currentBottomIndex > adapter.Count - 1)
			{
				currentBottomIndex = adapter.Count - 1;
			}
			DestroyItemsBetweenIndices(currentTopIndex, currentBottomIndex);
			currentTopIndex = -1;
			currentBottomIndex = -1;
		}
		UpdateList();
	}

	private void OnRectTransformDimensionsChange()
	{
		isDirty = true;
	}

	private void UpdateItemsInTheList(bool updateAllVisibleItems = false)
	{
		if (adapter == null)
		{
			return;
		}
		if (adapter.Count > 0)
		{
			float num = contentTransform.anchoredPosition.y - 1f;
			int num2 = (int)(num * _1OverItemHeight);
			int num3 = (int)((num + m_viewportHeight + 2f) * _1OverItemHeight);
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num3 > adapter.Count - 1)
			{
				num3 = adapter.Count - 1;
			}
			if (currentTopIndex == -1)
			{
				updateAllVisibleItems = true;
				currentTopIndex = num2;
				currentBottomIndex = num3;
				CreateItemsBetweenIndices(num2, num3);
			}
			else
			{
				if (num3 < currentTopIndex || num2 > currentBottomIndex)
				{
					updateAllVisibleItems = true;
					DestroyItemsBetweenIndices(currentTopIndex, currentBottomIndex);
					CreateItemsBetweenIndices(num2, num3);
				}
				else
				{
					if (num2 > currentTopIndex)
					{
						DestroyItemsBetweenIndices(currentTopIndex, num2 - 1);
					}
					if (num3 < currentBottomIndex)
					{
						DestroyItemsBetweenIndices(num3 + 1, currentBottomIndex);
					}
					if (num2 < currentTopIndex)
					{
						CreateItemsBetweenIndices(num2, currentTopIndex - 1);
						if (!updateAllVisibleItems)
						{
							UpdateItemContentsBetweenIndices(num2, currentTopIndex - 1);
						}
					}
					if (num3 > currentBottomIndex)
					{
						CreateItemsBetweenIndices(currentBottomIndex + 1, num3);
						if (!updateAllVisibleItems)
						{
							UpdateItemContentsBetweenIndices(currentBottomIndex + 1, num3);
						}
					}
				}
				currentTopIndex = num2;
				currentBottomIndex = num3;
			}
			if (updateAllVisibleItems)
			{
				UpdateItemContentsBetweenIndices(currentTopIndex, currentBottomIndex);
			}
		}
		else if (currentTopIndex != -1)
		{
			DestroyItemsBetweenIndices(currentTopIndex, currentBottomIndex);
			currentTopIndex = -1;
		}
	}

	private void CreateItemsBetweenIndices(int topIndex, int bottomIndex)
	{
		for (int i = topIndex; i <= bottomIndex; i++)
		{
			CreateItemAtIndex(i);
		}
	}

	private void CreateItemAtIndex(int index)
	{
		RecycledListItem recycledListItem;
		if (pooledItems.Count > 0)
		{
			recycledListItem = pooledItems.Pop();
			recycledListItem.gameObject.SetActive(value: true);
		}
		else
		{
			recycledListItem = adapter.CreateItem(contentTransform);
			recycledListItem.SetAdapter(adapter);
		}
		((RectTransform)recycledListItem.transform).anchoredPosition = new Vector2(0f, (float)(-index) * itemHeight);
		items[index] = recycledListItem;
	}

	private void DestroyItemsBetweenIndices(int topIndex, int bottomIndex)
	{
		for (int i = topIndex; i <= bottomIndex; i++)
		{
			RecycledListItem recycledListItem = items[i];
			recycledListItem.gameObject.SetActive(value: false);
			pooledItems.Push(recycledListItem);
		}
	}

	private void UpdateItemContentsBetweenIndices(int topIndex, int bottomIndex)
	{
		for (int i = topIndex; i <= bottomIndex; i++)
		{
			RecycledListItem recycledListItem = items[i];
			recycledListItem.Position = i;
			adapter.SetItemContent(recycledListItem);
		}
	}
}
