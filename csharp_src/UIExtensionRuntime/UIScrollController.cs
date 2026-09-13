using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScrollController : MonoBehaviour
{
	public enum Arrangement
	{
		Horizontal,
		Vertical
	}

	public Arrangement movement;

	[Range(0f, 20f)]
	public float cellPadiding = 2f;

	public float cellWidth = 500f;

	public float cellHeight = 140f;

	[Range(0f, 20f)]
	public int viewCount = 6;

	public GameObject itemPrefab;

	public RectTransform content;

	public int currentPosIndex;

	public float currentPosY;

	public float currentContentPosY;

	public bool isUpwardScroll;

	private Vector3 lastTimePos;

	private int index = -1;

	public List<UIScrollItem> itemList;

	public int dataCount;

	private Queue<UIScrollItem> unUsedQueue;

	public bool test;

	public int testCount;

	public Action<Vector2> OnValueChanged { get; set; }

	public Action<UIScrollItem> OnAddItem { get; set; }

	public Action<UIScrollItem> OnRemoveItem { get; set; }

	public Action<UIScrollItem> OnDelItem { get; set; }

	public Action<List<UIScrollItem>> OnInitItem { get; set; }

	public int DataCount
	{
		get
		{
			return dataCount;
		}
		set
		{
			dataCount = value;
			UpdateTotalWidth();
		}
	}

	private void Start()
	{
		if (test)
		{
			ScrollRect component = base.transform.GetComponent<ScrollRect>();
			Init(testCount, component);
		}
	}

	public void Init(int initCount, ScrollRect rect)
	{
		itemList = new List<UIScrollItem>();
		unUsedQueue = new Queue<UIScrollItem>();
		DataCount = initCount;
		OnValueChange(Vector2.zero);
		if (OnInitItem != null)
		{
			OnInitItem(itemList);
		}
		rect.onValueChanged.AddListener(OnValueChange);
	}

	public void RemoveScript()
	{
		UnityEngine.Object.Destroy(this);
	}

	public void OnValueChange(Vector2 pos)
	{
		int posIndex = GetPosIndex();
		if (lastTimePos.y - pos.y > 0f)
		{
			isUpwardScroll = true;
		}
		else
		{
			isUpwardScroll = false;
		}
		lastTimePos = pos;
		currentPosIndex = posIndex;
		currentPosY = (float)currentPosIndex * (0f - (cellHeight + cellPadiding));
		currentContentPosY = (float)((currentPosIndex != -1) ? currentPosIndex : 0) * (cellHeight + cellPadiding);
		if (index != posIndex)
		{
			if (posIndex < 0)
			{
				index = 0;
			}
			else if (posIndex > dataCount - 1)
			{
				index = dataCount - 1;
			}
			else
			{
				index = posIndex;
			}
			for (int num = itemList.Count; num > 0; num--)
			{
				UIScrollItem uIScrollItem = itemList[num - 1];
				if (uIScrollItem.Index < index || uIScrollItem.Index >= index + viewCount)
				{
					RemoveItem(uIScrollItem);
					if (OnRemoveItem != null)
					{
						OnRemoveItem(uIScrollItem);
					}
				}
			}
			for (int i = index; i < index + viewCount; i++)
			{
				if (i < 0 || i >= dataCount)
				{
					continue;
				}
				bool flag = false;
				foreach (UIScrollItem item in itemList)
				{
					if (item.Index == i)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					CreateItem(i);
				}
			}
		}
		if (OnValueChanged != null)
		{
			OnValueChanged(pos);
		}
	}

	public UIScrollItem AddItem(int index)
	{
		if (index > dataCount)
		{
			Debug.LogError("添加错误:" + index);
			return null;
		}
		UIScrollItem result = AddItemIntoPanel(index);
		DataCount++;
		return result;
	}

	public void DelItem(int index)
	{
		Debug.LogWarning("dataCount == " + dataCount + "       index == " + index);
		if (index < 0 || index > dataCount)
		{
			Debug.LogError("删除错误:" + index);
			return;
		}
		DelItemFromPanel(index);
		DataCount--;
	}

	private UIScrollItem AddItemIntoPanel(int index)
	{
		for (int i = 0; i < itemList.Count; i++)
		{
			UIScrollItem uIScrollItem = itemList[i];
			if (uIScrollItem.Index >= index)
			{
				uIScrollItem.Index++;
			}
		}
		return CreateItem(index);
	}

	private void DelItemFromPanel(int index)
	{
		int num = -1;
		int num2 = int.MaxValue;
		for (int num3 = itemList.Count; num3 > 0; num3--)
		{
			UIScrollItem uIScrollItem = itemList[num3 - 1];
			if (uIScrollItem.Index == index)
			{
				if (OnDelItem != null)
				{
					OnDelItem(uIScrollItem);
				}
				UnityEngine.Object.Destroy(uIScrollItem.gameObject);
				itemList.Remove(uIScrollItem);
			}
			if (uIScrollItem.Index > num)
			{
				num = uIScrollItem.Index;
			}
			if (uIScrollItem.Index < num2)
			{
				num2 = uIScrollItem.Index;
			}
			if (uIScrollItem.Index > index)
			{
				uIScrollItem.Index--;
			}
		}
		if (num < DataCount - 1)
		{
			Debug.LogWarning("CreateItem");
			CreateItem(num);
		}
	}

	private UIScrollItem CreateItem(int index)
	{
		UIScrollItem uIScrollItem;
		if (unUsedQueue.Count > 0)
		{
			uIScrollItem = unUsedQueue.Dequeue();
		}
		else
		{
			GameObject obj = UnityEngine.Object.Instantiate(itemPrefab);
			obj.transform.SetParent(content);
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			uIScrollItem = obj.GetComponent<UIScrollItem>();
		}
		if (!uIScrollItem.gameObject.activeInHierarchy)
		{
			uIScrollItem.gameObject.SetActive(value: true);
		}
		uIScrollItem.Scroller = this;
		uIScrollItem.Index = index;
		itemList.Add(uIScrollItem);
		if (OnAddItem != null)
		{
			OnAddItem(uIScrollItem);
		}
		return uIScrollItem;
	}

	private void RemoveItem(UIScrollItem item)
	{
		if (item != null && itemList.Contains(item))
		{
			itemList.Remove(item);
			unUsedQueue.Enqueue(item);
			item.gameObject.SetActive(value: false);
			item.gameObject.name = "[InPool]";
		}
	}

	private int GetPosIndex()
	{
		return movement switch
		{
			Arrangement.Horizontal => Mathf.FloorToInt(content.anchoredPosition.x / (0f - (cellWidth + cellPadiding))), 
			Arrangement.Vertical => Mathf.FloorToInt(content.anchoredPosition.y / (cellHeight + cellPadiding)), 
			_ => 0, 
		};
	}

	public Vector3 GetPosition(int i)
	{
		return movement switch
		{
			Arrangement.Horizontal => new Vector3((float)i * (cellWidth + cellPadiding), 0f, 0f), 
			Arrangement.Vertical => new Vector3(0f, (float)i * (0f - (cellHeight + cellPadiding)), 0f), 
			_ => Vector3.zero, 
		};
	}

	private void UpdateTotalWidth()
	{
		switch (movement)
		{
		case Arrangement.Horizontal:
			content.sizeDelta = new Vector2(cellWidth * (float)dataCount + cellPadiding * (float)(dataCount - 1), content.sizeDelta.y);
			break;
		case Arrangement.Vertical:
			content.sizeDelta = new Vector2(content.sizeDelta.x, cellHeight * (float)dataCount + cellPadiding * (float)(dataCount - 1));
			break;
		}
	}
}
