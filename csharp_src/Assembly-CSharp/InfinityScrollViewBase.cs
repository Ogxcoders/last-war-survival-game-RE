using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InfinityScrollViewBase : MonoBehaviour
{
	[SerializeField]
	private protected Vector2 cellSize;

	[SerializeField]
	private protected Vector2 spacingSize;

	[SerializeField]
	private GameObject itemTemplate;

	protected RectTransform rectTransform;

	protected ScrollRect scrollRect;

	protected int itemCount;

	protected Vector2 maskSize;

	protected Rect maskRect;

	protected List<InfinityItem> infinityItems;

	protected Dictionary<int, InfinityRect> rectDic;

	protected bool inited;

	public Action<GameObject, int, int> onUpdate;

	public Action<GameObject, int> onDestroy;

	private Dictionary<int, InfinityRect> inOverlaps = new Dictionary<int, InfinityRect>();

	private int lastNumber;

	private int lastKey;

	private bool isMove;

	protected Coroutine _coroutine;

	protected int maxCount = -1;

	public int MaxCount
	{
		set
		{
			if (!inited)
			{
				maxCount = value;
			}
		}
	}

	public virtual void Init(Action<GameObject, int> onInit, Action<GameObject, int, int> onUpdate, Action<GameObject, int> onDestroy)
	{
		rectTransform = base.transform as RectTransform;
		RectTransform component = base.transform.parent.GetComponent<RectTransform>();
		maskSize = new Vector2(component.rect.width, component.rect.height);
		scrollRect = base.transform.parent.GetComponent<ScrollRect>();
		if (scrollRect != null)
		{
			scrollRect.onValueChanged.AddListener(OnScrollChange);
		}
		itemCount = CalculationItemCount();
		infinityItems = new List<InfinityItem>();
		for (int i = 0; i < itemCount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(itemTemplate, base.transform);
			if (MirrorVersionConfig.IsMirrorVersionOpen && itemTemplate?.scene.name == null)
			{
				ArabicMirror.MirrorEntry(isInnerMirror: true, isProcessRootAnchorAndPivot: false, gameObject);
			}
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			gameObject.layer = base.gameObject.layer;
			gameObject.name = Convert.ToString(i + 1);
			InfinityItem item = gameObject.AddComponent<InfinityItem>();
			infinityItems.Add(item);
			onInit(gameObject, i + 1);
			gameObject.transform.localScale = Vector3.zero;
			UpdateItemTransformPos(gameObject, i);
		}
		UpdateDynmicRects(itemCount);
		isMove = false;
		this.onUpdate = onUpdate;
		this.onDestroy = onDestroy;
		inited = true;
	}

	private void OnDestroy()
	{
		if (inited)
		{
			Dispose();
		}
	}

	public virtual void Dispose()
	{
		onUpdate = null;
		onDestroy = null;
		if (scrollRect != null)
		{
			scrollRect.onValueChanged.RemoveAllListeners();
		}
		StopLocateCoroutine();
	}

	public virtual void SetItemCount(int itemCount)
	{
		UpdateDynmicRects(itemCount);
		SetRenderListSize(itemCount);
		ClearAllRender();
		UpdateRender(Vector2.zero);
	}

	public virtual void ForceUpdate()
	{
		UpdateRender(Vector2.zero, isForce: true);
	}

	public virtual void ForceUpdateCell()
	{
		UpdateRender(Vector2.zero, isForce: true);
	}

	public virtual int GetColumnCount()
	{
		return 1;
	}

	public virtual int GetRenderCount()
	{
		return inOverlaps.Count;
	}

	protected float GetItemSizeX()
	{
		return cellSize.x + spacingSize.x;
	}

	protected float GetItemSizeY()
	{
		return cellSize.y + spacingSize.y;
	}

	protected InfinityItem GetInfinityItem(InfinityRect rect)
	{
		int count = infinityItems.Count;
		for (int i = 0; i < count; i++)
		{
			InfinityItem infinityItem = infinityItems[i];
			if (infinityItem.Rect != null && rect.Index == infinityItem.Rect.Index)
			{
				return infinityItem;
			}
		}
		return null;
	}

	public virtual InfinityItem GetInfinityItemByIndex(int index)
	{
		return infinityItems[index];
	}

	protected InfinityItem GetNullInfinityItem()
	{
		int count = infinityItems.Count;
		for (int i = 0; i < count; i++)
		{
			InfinityItem infinityItem = infinityItems[i];
			if (infinityItem.Rect == null)
			{
				return infinityItem;
			}
		}
		return null;
	}

	protected void ClearAllRender()
	{
		if (infinityItems == null)
		{
			return;
		}
		for (int i = 0; i < infinityItems.Count; i++)
		{
			InfinityItem infinityItem = infinityItems[i];
			if (infinityItem.Rect != null)
			{
				int index = infinityItem.Rect.Index;
				if (onDestroy != null)
				{
					onDestroy(infinityItem.gameObject, index);
				}
				infinityItem.Rect = null;
			}
		}
		lastNumber = 0;
	}

	protected abstract void UpdateDynmicRects(int itemCount);

	protected abstract void SetRenderListSize(int itemCount);

	protected abstract void UpdateItemTransformPos(GameObject item, int index);

	protected abstract void UpdateMaskRect();

	protected abstract int CalculationItemCount();

	public abstract void MoveItemByIndex(int index, float delay);

	protected abstract Vector2 GetItemAnchoredPos(int index);

	public virtual void StopLocateCoroutine()
	{
		if (_coroutine != null)
		{
			StopCoroutine(_coroutine);
		}
		_coroutine = null;
	}

	private void OnScrollChange(Vector2 pos)
	{
		if (!isMove)
		{
			UpdateRender(pos);
		}
	}

	protected void UpdateRender(Vector2 pos, bool isForce = false)
	{
		UpdateMaskRect();
		inOverlaps.Clear();
		int num = 0;
		foreach (InfinityRect value in rectDic.Values)
		{
			if (value.Overlaps(maskRect))
			{
				inOverlaps.Add(value.Index, value);
				num += value.Index + 1;
				lastKey = value.Index;
			}
		}
		if (!isForce && lastNumber == num)
		{
			return;
		}
		lastNumber = num;
		int count = infinityItems.Count;
		for (int i = 0; i < count; i++)
		{
			InfinityItem infinityItem = infinityItems[i];
			if (infinityItem.Rect != null && !inOverlaps.ContainsKey(infinityItem.Rect.Index))
			{
				int index = infinityItem.Rect.Index;
				if (onDestroy != null && infinityItem.gameObject != null)
				{
					onDestroy(infinityItem.gameObject, index);
				}
				infinityItem.Rect = null;
			}
		}
		foreach (InfinityRect value2 in inOverlaps.Values)
		{
			InfinityItem infinityItem2 = GetInfinityItem(value2);
			if (infinityItem2 == null)
			{
				infinityItem2 = GetNullInfinityItem();
				infinityItem2.Rect = value2;
				UpdateItemTransformPos(infinityItem2.gameObject, infinityItem2.Rect.Index);
				onUpdate(infinityItem2.gameObject, infinityItem2.Rect.Index, inOverlaps.Count);
			}
			else if (isForce)
			{
				onUpdate(infinityItem2.gameObject, infinityItem2.Rect.Index, inOverlaps.Count);
			}
		}
	}

	protected IEnumerator TweenMoveToPos(Vector2 pos, Vector2 toPos, float delay)
	{
		bool running = true;
		float passedTime = 0f;
		while (running)
		{
			yield return new WaitForEndOfFrame();
			passedTime += Time.deltaTime;
			Vector2 anchoredPosition;
			if (passedTime >= delay)
			{
				anchoredPosition = toPos;
				running = false;
				StopCoroutine(_coroutine);
				_coroutine = null;
			}
			else
			{
				anchoredPosition = Vector2.Lerp(pos, toPos, passedTime / delay);
			}
			rectTransform.anchoredPosition = anchoredPosition;
		}
	}

	protected IEnumerator TweenUpdateToPoss(int index, float delay)
	{
		float passedTime = 0f;
		isMove = true;
		foreach (InfinityRect value in rectDic.Values)
		{
			if (lastKey + 1 == value.Index && !inOverlaps.ContainsKey(value.Index))
			{
				inOverlaps.Add(value.Index, value);
			}
		}
		foreach (InfinityRect value2 in inOverlaps.Values)
		{
			if (value2.Index == lastKey + 1)
			{
				InfinityItem infinityItem = GetInfinityItem(value2);
				if (infinityItem == null)
				{
					infinityItem = GetNullInfinityItem();
					infinityItem.Rect = value2;
					UpdateItemTransformPos(infinityItem.gameObject, infinityItem.Rect.Index);
					onUpdate(infinityItem.gameObject, infinityItem.Rect.Index, inOverlaps.Count);
				}
			}
		}
		bool running = true;
		while (running)
		{
			yield return new WaitForEndOfFrame();
			passedTime += Time.deltaTime;
			if (passedTime >= delay)
			{
				running = false;
				StopCoroutine(_coroutine);
				_coroutine = null;
				GetInfinityItem(inOverlaps[index - 1]).Rect = null;
				GameEntry.Event.Fire(EventId.OnTaskForceRefreshFinish);
				isMove = false;
				continue;
			}
			foreach (InfinityRect value3 in inOverlaps.Values)
			{
				if (index <= value3.Index)
				{
					InfinityItem infinityItem2 = GetInfinityItem(value3);
					if (infinityItem2 == null)
					{
						infinityItem2 = GetNullInfinityItem();
						infinityItem2.Rect = value3;
					}
					Vector2 itemAnchoredPos = GetItemAnchoredPos(value3.Index - 1);
					Vector2 anchoredPosition = Vector2.Lerp(((RectTransform)infinityItem2.gameObject.transform).anchoredPosition, itemAnchoredPos, passedTime / delay);
					((RectTransform)infinityItem2.gameObject.transform).anchoredPosition = anchoredPosition;
				}
			}
		}
	}

	private void Update()
	{
		_ = inited;
	}
}
