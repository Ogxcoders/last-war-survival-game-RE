using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollPage : MonoBehaviour
{
	protected int count;

	protected GameObject adItemPrefab;

	protected Transform adItemParent;

	protected GameObject adPageItemPrefab;

	protected Transform adPageItemParent;

	protected bool isDraged;

	protected GameObject scroll;

	protected RectTransform gridLayoutGroupRect;

	protected GridLayoutGroup cellSize;

	protected List<GameObject> pageItems = new List<GameObject>();

	protected List<GameObject> adItems = new List<GameObject>();

	public float speed = 3000f;

	public float autoSpeed = 1000f;

	public float scrollSpeed = 3000f;

	protected float lerp;

	protected Vector2 beginPos;

	public int curIdx;

	public int preIdx;

	public Vector2 targetPos;

	public bool change;

	public float dis;

	public bool idxLoop;

	public float scorllDis = 10f;

	public int Count => count;

	public Action<GameObject> OnCreateItemEvent { get; set; }

	public Action<GameObject> OnRemoveItemEvent { get; set; }

	public Action<int, int, bool> OnChangeEvent { get; set; }

	public Action<int> OnScrollEndEvent { get; set; }

	public virtual void Initialize(int count, GameObject scroll, GridLayoutGroup gridLayoutGroup, GameObject adItem)
	{
		this.count = count;
		this.scroll = scroll;
		adItemPrefab = adItem;
		adItemParent = gridLayoutGroup.transform;
		gridLayoutGroupRect = gridLayoutGroup.GetComponent<RectTransform>();
		cellSize = gridLayoutGroup;
		if (count > 0)
		{
			SpawnPage();
		}
		UIEventListener.Get(scroll).onBeginDrag = OnBeginDrag;
		UIEventListener.Get(scroll).onEndDrag = OnEndDrag;
		targetPos = gridLayoutGroupRect.anchoredPosition;
	}

	public virtual void Initialize(int count, GameObject scroll, GridLayoutGroup adGridLayoutGroup, GameObject adItem, GridLayoutGroup pageGridLayoutGroup, GameObject pageItem)
	{
		this.count = count;
		this.scroll = scroll;
		adItemPrefab = adItem;
		adItemParent = adGridLayoutGroup.transform;
		gridLayoutGroupRect = adGridLayoutGroup.GetComponent<RectTransform>();
		cellSize = adGridLayoutGroup;
		adPageItemPrefab = pageItem;
		adPageItemParent = pageGridLayoutGroup.transform;
		if (count > 0)
		{
			SpawnPage();
		}
		UIEventListener.Get(scroll).onBeginDrag = OnBeginDrag;
		UIEventListener.Get(scroll).onEndDrag = OnEndDrag;
		targetPos = gridLayoutGroupRect.anchoredPosition;
	}

	public virtual void SetDragEventObj(GameObject go)
	{
		UIEventListener.Get(go).onBeginDrag = OnBeginDrag;
		UIEventListener.Get(go).onEndDrag = OnEndDrag;
	}

	public virtual void RemoveAll()
	{
		if (count > 0)
		{
			for (int num = count - 1; num >= 0; num--)
			{
				RemovePageByIdx(num, countAutoSubtract: false);
			}
			count = 0;
		}
	}

	public virtual int RemovePageByIdx(int idx, bool countAutoSubtract = true)
	{
		if (idx < count && count > 0)
		{
			if (OnRemoveItemEvent != null)
			{
				OnRemoveItemEvent(adItems[idx]);
			}
			OnRemoveItem(adItems[idx]);
			adItems[idx].gameObject.Destroy();
			adItems.RemoveAt(idx);
			if (idx >= count - 1)
			{
				_ = 1;
			}
			else
				_ = idx <= 0;
			if (pageItems.Count > 0)
			{
				pageItems[idx].gameObject.Destroy();
				pageItems.RemoveAt(idx);
			}
			if (!countAutoSubtract)
			{
				return curIdx;
			}
			if (pageItems.Count > 0)
			{
				for (int i = 0; i < pageItems.Count; i++)
				{
					GameObject pageItem = pageItems[i];
					pageItem.name = i.ToString();
					Transform transform = pageItem.transform.Find("Off");
					if (!transform)
					{
						continue;
					}
					Button component = transform.GetComponent<Button>();
					if ((bool)component)
					{
						component.onClick.RemoveAllListeners();
						component.onClick.AddListener(delegate
						{
							ChangePage(pageItem.name.ToInt());
						});
					}
				}
			}
			if (curIdx > 0)
			{
				curIdx--;
				if (OnChangeEvent != null)
				{
					OnChangeEvent(preIdx, curIdx, arg3: false);
				}
				OnChange(preIdx, curIdx);
			}
			preIdx = curIdx;
			if (pageItems.Count > 0)
			{
				SetPageState(pageItems[curIdx], state: true);
			}
			SetTarget();
			if (countAutoSubtract)
			{
				count--;
			}
		}
		return curIdx;
	}

	public virtual void OnChange(int preIdx, int curIdx)
	{
	}

	public virtual void SetTarget()
	{
		float x = (float)curIdx * (0f - cellSize.cellSize.x);
		targetPos.x = x;
		gridLayoutGroupRect.anchoredPosition = targetPos;
		lerp = 0f;
	}

	public virtual void OnCreateItem(GameObject obj)
	{
	}

	public virtual void OnRemoveItem(GameObject obj)
	{
	}

	public virtual void SpawnOnePageItem()
	{
		GameObject adPageItem = adPageItemPrefab.Instantiate();
		adPageItem.transform.SetParent(adPageItemParent);
		adPageItem.name = count.ToString();
		adPageItem.gameObject.SetActive(value: true);
		adPageItem.transform.localScale = Vector3.one;
		Transform transform = adPageItem.transform.Find("Off");
		if ((bool)transform)
		{
			Button component = transform.GetComponent<Button>();
			if ((bool)component)
			{
				component.onClick.RemoveAllListeners();
				component.onClick.AddListener(delegate
				{
					speed = autoSpeed;
					ChangePage(adPageItem.name.ToInt());
				});
			}
		}
		pageItems.Add(adPageItem);
	}

	public virtual GameObject SpawnOnePage()
	{
		GameObject gameObject = adItemPrefab.Instantiate();
		gameObject.transform.SetParent(adItemParent);
		if (OnCreateItemEvent != null)
		{
			OnCreateItemEvent(gameObject);
		}
		gameObject.transform.localScale = Vector3.one;
		OnCreateItem(gameObject);
		gameObject.gameObject.SetActive(value: true);
		adItems.Add(gameObject);
		if (adPageItemPrefab != null)
		{
			SpawnOnePageItem();
		}
		if (count == 0)
		{
			SetPageState(pageItems[0], state: true);
		}
		count++;
		return gameObject;
	}

	public int GetNextIndex()
	{
		int num = 0;
		if (curIdx == count - 1)
		{
			return 0;
		}
		return curIdx + 1;
	}

	public virtual void SpawnPage()
	{
		int num = count;
		count = 0;
		for (int i = 0; i < num; i++)
		{
			SpawnOnePage();
		}
	}

	public virtual void ChangePage(int idx)
	{
		if (!change)
		{
			preIdx = curIdx;
			curIdx = idx;
			IdxLogic();
			if (OnChangeEvent != null)
			{
				OnChangeEvent(preIdx, curIdx, arg3: false);
			}
			OnChange(preIdx, curIdx);
			if (pageItems.Count > 0)
			{
				SetPageState(pageItems[preIdx], state: false);
				SetPageState(pageItems[curIdx], state: true);
			}
		}
	}

	public virtual void SetPageState(GameObject go, bool state)
	{
		go.transform.Find("Off/On").gameObject.SetActive(state);
	}

	public virtual void OnBeginDrag(GameObject arg1, Vector2 arg2, Vector2 arg3)
	{
		beginPos = Input.mousePosition;
	}

	public virtual void OnEndDrag(GameObject arg1, Vector2 arg2, Vector2 arg3)
	{
		if (change)
		{
			return;
		}
		preIdx = curIdx;
		Vector2 vector = (Vector2)Input.mousePosition - beginPos;
		float num = Mathf.Abs(vector.x);
		Vector3 vector2 = Vector3.Normalize(vector);
		if (vector2.x > 0f && num > scorllDis)
		{
			curIdx--;
		}
		else
		{
			if (!(vector2.x < 0f) || !(num > scorllDis))
			{
				return;
			}
			curIdx++;
		}
		IdxLogic();
		if (OnChangeEvent != null)
		{
			OnChangeEvent(preIdx, curIdx, arg3: true);
		}
		speed = scrollSpeed;
		isDraged = true;
		OnChange(preIdx, curIdx);
	}

	public virtual void CheckLoop(int idx)
	{
		if (gridLayoutGroupRect.childCount >= 2)
		{
			Vector2 anchoredPosition = gridLayoutGroupRect.anchoredPosition;
			anchoredPosition.x = (float)idx * (0f - cellSize.cellSize.x);
			gridLayoutGroupRect.anchoredPosition = anchoredPosition;
		}
	}

	public virtual void IdxLogic()
	{
		if (curIdx < 0)
		{
			if (idxLoop)
			{
				curIdx = gridLayoutGroupRect.childCount - 1;
				CheckLoop(curIdx - 1);
			}
			else
			{
				curIdx = 0;
			}
		}
		if (curIdx >= gridLayoutGroupRect.childCount)
		{
			if (idxLoop)
			{
				curIdx = 0;
				CheckLoop(curIdx + 1);
			}
			else
			{
				curIdx = gridLayoutGroupRect.childCount - 1;
			}
		}
		if (preIdx != curIdx)
		{
			change = true;
		}
		float x = (float)curIdx * (0f - cellSize.cellSize.x);
		targetPos.x = x;
		dis = Vector3.Distance(gridLayoutGroupRect.anchoredPosition, targetPos);
	}

	public virtual void OnScrollEnd(int curIdx)
	{
	}

	public virtual void TryChangeToTarget()
	{
		if (change)
		{
			EndLogic();
		}
	}

	private void EndLogic()
	{
		lerp = 0f;
		change = false;
		if (OnScrollEndEvent != null)
		{
			OnScrollEndEvent(curIdx);
		}
		OnScrollEnd(curIdx);
		if (pageItems.Count > 0)
		{
			SetPageState(pageItems[preIdx], state: false);
			SetPageState(pageItems[curIdx], state: true);
		}
		gridLayoutGroupRect.anchoredPosition = targetPos;
	}

	public virtual void Update()
	{
		if (change)
		{
			lerp += speed * Time.deltaTime / dis;
			gridLayoutGroupRect.anchoredPosition = Vector3.Lerp(gridLayoutGroupRect.anchoredPosition, targetPos, lerp);
			if (lerp >= 1f)
			{
				EndLogic();
			}
		}
	}
}
