using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIScrollBase<T> where T : class
{
	public Transform content;

	public UIScrollController uiScroController;

	protected Dictionary<UIScrollItem, List<GameObject>> gameObjectsPerLine;

	protected List<T> dataList;

	private ScrollRect scroRect;

	private int numPerLine;

	private int viewCount;

	private float cellPadiding;

	private float cellHeight;

	private float cellWidth;

	public Action<Vector2> onValueChanged;

	public Action<GameObject> addListenerEvent;

	public Action<T, GameObject> freshEvent;

	public int GetCurrentPosIndex => uiScroController.currentPosIndex;

	public GameObject GetFirstGameObjectByIndex()
	{
		GameObject result = null;
		if (content.childCount > 0 && (bool)content.Find("Scroll0/0"))
		{
			result = content.Find("Scroll0/0").gameObject;
		}
		return result;
	}

	public GameObject GetGameObjectByIndex(int idx)
	{
		GameObject result = null;
		if (content.childCount > 0)
		{
			string n = $"Scroll{idx}/{idx}";
			if ((bool)content.Find(n))
			{
				result = content.Find(n).gameObject;
			}
		}
		return result;
	}

	public virtual void InitArguments(List<T> data, ScrollRect sr, Transform content, GameObject itemPrefab, Vector2 rect, int numPerLine, int viewCount = 6, float cellPadiding = 10f)
	{
		gameObjectsPerLine = new Dictionary<UIScrollItem, List<GameObject>>();
		this.content = content;
		dataList = data;
		scroRect = sr;
		if (itemPrefab.GetComponent<UIScrollController>() == null)
		{
			uiScroController = scroRect.gameObject.AddComponent<UIScrollController>();
		}
		uiScroController.content = this.content.GetComponent<RectTransform>();
		uiScroController.movement = UIScrollController.Arrangement.Vertical;
		if (itemPrefab.GetComponent<UIScrollItem>() == null)
		{
			itemPrefab.AddComponent<UIScrollItem>();
		}
		uiScroController.itemPrefab = itemPrefab;
		this.numPerLine = numPerLine;
		this.viewCount = viewCount;
		this.cellPadiding = cellPadiding;
		cellHeight = rect.y;
		cellWidth = rect.x;
	}

	public virtual void SetItemPrefab(GameObject go)
	{
		uiScroController.itemPrefab = go;
	}

	public List<T> GetDataList()
	{
		return dataList;
	}

	public void ReSetDataList(List<T> newList)
	{
		if (newList == null)
		{
			throw new NotImplementedException("传入的数据为null");
		}
		dataList = newList;
		uiScroController.DataCount = dataList.Count;
	}

	public void SetArrangement(int type)
	{
		switch (type)
		{
		case 0:
			uiScroController.movement = UIScrollController.Arrangement.Vertical;
			break;
		case 1:
			uiScroController.movement = UIScrollController.Arrangement.Horizontal;
			break;
		}
	}

	public void Start()
	{
		UIScrollController uIScrollController = uiScroController;
		uIScrollController.OnInitItem = (Action<List<UIScrollItem>>)Delegate.Combine(uIScrollController.OnInitItem, new Action<List<UIScrollItem>>(OnInitItem));
		int initCount = Mathf.CeilToInt((float)dataList.Count * 1f / (float)numPerLine);
		uiScroController.viewCount = viewCount;
		uiScroController.cellPadiding = cellPadiding;
		uiScroController.cellHeight = cellHeight;
		uiScroController.cellWidth = cellWidth;
		uiScroController.Init(initCount, scroRect);
		UIScrollController uIScrollController2 = uiScroController;
		uIScrollController2.OnAddItem = (Action<UIScrollItem>)Delegate.Combine(uIScrollController2.OnAddItem, new Action<UIScrollItem>(OnAddItem));
		UIScrollController uIScrollController3 = uiScroController;
		uIScrollController3.OnRemoveItem = (Action<UIScrollItem>)Delegate.Combine(uIScrollController3.OnRemoveItem, new Action<UIScrollItem>(OnRemoveItem));
		UIScrollController uIScrollController4 = uiScroController;
		uIScrollController4.OnDelItem = (Action<UIScrollItem>)Delegate.Combine(uIScrollController4.OnDelItem, new Action<UIScrollItem>(OnDelItem));
		UIScrollController uIScrollController5 = uiScroController;
		uIScrollController5.OnValueChanged = (Action<Vector2>)Delegate.Combine(uIScrollController5.OnValueChanged, new Action<Vector2>(OnValueChanged));
	}

	public T GetDataByIndex(int index)
	{
		if (index >= 0 && index < dataList.Count)
		{
			return dataList[index];
		}
		throw new NotImplementedException("index out of range --> index == " + index + " ; range" + 0 + "," + dataList.Count);
	}

	public void ResetScrollList(List<T> newList)
	{
		int num = Mathf.FloorToInt(dataList.Count / numPerLine);
		int num2 = ((dataList.Count % numPerLine == 0) ? num : (num + 1));
		int num3 = Mathf.FloorToInt(newList.Count / numPerLine);
		int num4 = ((newList.Count % numPerLine == 0) ? num3 : (num3 + 1));
		if (num2 <= num4)
		{
			return;
		}
		int num5 = num2 - num4;
		if (num4 >= viewCount)
		{
			uiScroController.DataCount -= num5;
			dataList = newList;
			TryLocation(0);
			return;
		}
		int num6 = 0;
		num6 = ((num2 < viewCount) ? (num2 - num4) : (viewCount - num4));
		List<int> list = new List<int>();
		for (int i = 0; i < uiScroController.itemList.Count; i++)
		{
			GameObject gameObject = uiScroController.itemList[i].gameObject;
			int result = 0;
			int.TryParse(gameObject.name.Replace("Scroll", string.Empty), out result);
			list.Add(result);
		}
		list.Sort(delegate(int x, int y)
		{
			if (x > y)
			{
				return 1;
			}
			return (x < y) ? (-1) : 0;
		});
		for (int num7 = 0; num7 < num6; num7++)
		{
			uiScroController.DelItem(list[num7]);
		}
	}

	public void ClearController()
	{
		if (content != null)
		{
			Vector2 anchoredPosition = uiScroController.content.anchoredPosition;
			anchoredPosition.x = Mathf.Abs(uiScroController.GetPosition(0).x);
			anchoredPosition.y = Mathf.Abs(uiScroController.GetPosition(0).y);
			uiScroController.content.anchoredPosition = anchoredPosition;
			scroRect.enabled = true;
			for (int i = 0; i < content.childCount; i++)
			{
				UnityEngine.Object.Destroy(content.GetChild(i).gameObject);
			}
			content.DetachChildren();
		}
		if (uiScroController != null)
		{
			uiScroController.RemoveScript();
			uiScroController = null;
		}
	}

	public void TryRefreshShowUI()
	{
		int currentPosIndex = uiScroController.currentPosIndex;
		TryRefreshUIData(currentPosIndex, content);
	}

	public void InsertByIndex(int idx, T insertItem)
	{
		if (uiScroController.currentPosIndex == -1)
		{
			uiScroController.currentPosIndex = 0;
		}
		int currentPosIndex = uiScroController.currentPosIndex;
		if (dataList.Count % numPerLine == 0)
		{
			dataList.Insert(idx, insertItem);
			if (uiScroController.DataCount < uiScroController.viewCount)
			{
				int dataCount = uiScroController.DataCount;
				UIScrollItem uIScrollItem = uiScroController.AddItem(dataCount);
				if (!gameObjectsPerLine.ContainsKey(uIScrollItem))
				{
					SingeItemMap(dataList.Count - 1, uIScrollItem);
				}
				else
				{
					SingeItemMap(dataList.Count - 1, uIScrollItem, gameObjectsPerLine[uIScrollItem]);
				}
			}
			else
			{
				uiScroController.DataCount++;
			}
			TryRefreshUIData(currentPosIndex, content);
		}
		else
		{
			if (idx < dataList.Count)
			{
				dataList.Insert(idx, insertItem);
			}
			else
			{
				dataList.Add(insertItem);
			}
			TryRefreshUIData(currentPosIndex, content);
		}
	}

	public void AppendInsert(List<T> insertList)
	{
		Debug.LogErrorFormat("AppendInsert GetCurrentPosIndex={0}", GetCurrentPosIndex);
		InsertByIndex(GetCurrentPosIndex, insertList);
	}

	public void InsertByIndex(int startIdx, List<T> insertList)
	{
		int startsWith = 0;
		for (int i = 0; i < insertList.Count; i++)
		{
			T item = insertList[insertList.Count - 1 - i];
			if (uiScroController.currentPosIndex == -1)
			{
				uiScroController.currentPosIndex = 0;
			}
			startsWith = uiScroController.currentPosIndex;
			if (dataList.Count % numPerLine == 0)
			{
				dataList.Insert(startIdx, item);
				if (uiScroController.DataCount < uiScroController.viewCount)
				{
					UIScrollItem uIScrollItem = uiScroController.AddItem(uiScroController.DataCount);
					if (!gameObjectsPerLine.ContainsKey(uIScrollItem))
					{
						SingeItemMap(dataList.Count - 1, uIScrollItem);
					}
					else
					{
						SingeItemMap(dataList.Count - 1, uIScrollItem, gameObjectsPerLine[uIScrollItem]);
					}
				}
				else
				{
					uiScroController.DataCount++;
				}
			}
			else if (startIdx < dataList.Count)
			{
				dataList.Insert(startIdx, item);
			}
			else
			{
				dataList.Add(item);
			}
		}
		TryRefreshUIData(startsWith, content);
	}

	public void RemoveByIdx(int idx)
	{
		uiScroController.currentPosIndex = ((uiScroController.currentPosIndex != -1) ? uiScroController.currentPosIndex : 0);
		int currentPosIndex = uiScroController.currentPosIndex;
		Remove(idx);
		TryRefreshUIData(currentPosIndex, content);
	}

	public void RemoveByIdx(List<int> idxList)
	{
		List<int> list = idxList.OrderByDescending((int e) => e).ToList();
		int startsWith = 0;
		for (int num = 0; num < list.Count; num++)
		{
			uiScroController.currentPosIndex = ((uiScroController.currentPosIndex != -1) ? uiScroController.currentPosIndex : 0);
			startsWith = uiScroController.currentPosIndex;
			Remove(list[num]);
		}
		TryRefreshUIData(startsWith, content);
	}

	private void Remove(int idx)
	{
		dataList.RemoveAt(idx);
		if (dataList.Count % numPerLine == 0)
		{
			if (uiScroController.DataCount <= uiScroController.viewCount)
			{
				int index = uiScroController.DataCount - 1;
				uiScroController.DelItem(index);
			}
			else
			{
				uiScroController.DataCount--;
			}
		}
	}

	public void TryLocation(int idx)
	{
		if (dataList.Count != 0 && idx < dataList.Count)
		{
			int i = ((idx != 0) ? (Mathf.CeilToInt(idx / numPerLine) - 1) : 0);
			Vector2 anchoredPosition = uiScroController.content.anchoredPosition;
			anchoredPosition.x = Mathf.Abs(uiScroController.GetPosition(i).x);
			anchoredPosition.y = Mathf.Abs(uiScroController.GetPosition(i).y);
			uiScroController.content.anchoredPosition = anchoredPosition;
			uiScroController.OnValueChange(anchoredPosition);
			TryRefreshShowUI();
		}
	}

	private void TryRefreshUIData(int startsWith, Transform content)
	{
		for (int i = 0; i < uiScroController.viewCount; i++)
		{
			int num = startsWith + i;
			string n = "Scroll" + num;
			Transform transform = content.Find(n);
			if (!(transform != null))
			{
				continue;
			}
			UIScrollItem component = transform.GetComponent<UIScrollItem>();
			if (gameObjectsPerLine.ContainsKey(component))
			{
				List<GameObject> list = gameObjectsPerLine[component];
				for (int j = 0; j < list.Count; j++)
				{
					GameObject gameObject = list[j];
					int num2 = num * numPerLine + j;
					T val = null;
					gameObject.name = num2.ToString();
					if (num2 < dataList.Count)
					{
						val = dataList[num2];
						if (freshEvent != null)
						{
							freshEvent(val, gameObject);
							gameObject.SetActive(value: true);
						}
					}
					else
					{
						gameObject.SetActive(value: false);
						gameObject.name = "[Hiding]";
					}
				}
			}
			else
			{
				Debug.LogWarning("uiMagicItemModelDic. ContainsKey(scrollItem) ===== " + false);
			}
		}
	}

	private void SingeItemMap(int startIdxPerLine, UIScrollItem scrollItem, List<GameObject> gos = null)
	{
		GameObject gameObject = scrollItem.gameObject;
		int childCount = gameObject.transform.childCount;
		GameObject gameObject2 = null;
		for (int i = 0; i < childCount; i++)
		{
			Transform transform = null;
			if (gos == null)
			{
				transform = gameObject.transform.GetChild(i);
				gameObject2 = transform.gameObject;
				if (addListenerEvent != null)
				{
					addListenerEvent(gameObject2);
				}
			}
			else
			{
				gameObject2 = gos[i];
				transform = gameObject2.transform;
			}
			int num = i + startIdxPerLine;
			transform.name = num.ToString();
			T val = null;
			if (num < dataList.Count)
			{
				val = dataList[num];
				if (freshEvent != null)
				{
					freshEvent(val, transform.gameObject);
					transform.gameObject.SetActive(value: true);
				}
			}
			else if (i == 0)
			{
				gameObject.SetActive(value: false);
			}
			else
			{
				gameObject2.SetActive(value: false);
			}
			EntryContainer(scrollItem, gameObject2);
		}
	}

	private void EntryContainer(UIScrollItem scrollItem, GameObject cellGo)
	{
		if (gameObjectsPerLine.ContainsKey(scrollItem))
		{
			if (!gameObjectsPerLine[scrollItem].Contains(cellGo))
			{
				gameObjectsPerLine[scrollItem].Add(cellGo);
			}
		}
		else
		{
			gameObjectsPerLine[scrollItem] = new List<GameObject>();
			gameObjectsPerLine[scrollItem].Add(cellGo);
		}
		if (!cellGo.activeSelf)
		{
			cellGo.name = "[Hiding]";
		}
		if (!scrollItem.gameObject.activeSelf)
		{
			scrollItem.gameObject.name = "[Hiding]";
		}
	}

	private void OnInitItem(List<UIScrollItem> items)
	{
		int num = 0;
		for (int i = 0; i < items.Count; i++)
		{
			SingeItemMap(num, items[i]);
			num += numPerLine;
		}
	}

	private void OnDelItem(UIScrollItem arg1)
	{
		if (gameObjectsPerLine.ContainsKey(arg1))
		{
			gameObjectsPerLine.Remove(arg1);
		}
	}

	private void OnAddItem(UIScrollItem arg1)
	{
		if (!gameObjectsPerLine.ContainsKey(arg1))
		{
			SingeItemMap(dataList.Count - 1, arg1);
		}
		if (uiScroController.currentPosIndex == -1)
		{
			uiScroController.currentPosIndex = 0;
		}
		int currentPosIndex = uiScroController.currentPosIndex;
		TryRefreshUIData(currentPosIndex, content);
	}

	private void OnRemoveItem(UIScrollItem arg1)
	{
		if (uiScroController.currentPosIndex == -1)
		{
			uiScroController.currentPosIndex = 0;
		}
		int currentPosIndex = uiScroController.currentPosIndex;
		TryRefreshUIData(currentPosIndex, content);
	}

	private void OnValueChanged(Vector2 pos)
	{
		if (onValueChanged != null)
		{
			onValueChanged(pos);
		}
	}
}
