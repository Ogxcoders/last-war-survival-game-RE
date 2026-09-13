using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicInfinityListRenderer : MonoBehaviour
{
	public bool IsVertical = true;

	public Vector2 CellSize;

	public Vector2 SpacingSize;

	public int ColumnCount;

	public GameObject RenderGO;

	protected int mRendererCount;

	private Vector2 mMaskSize;

	private Rect mRectMask;

	protected ScrollRect mScrollRect;

	protected RectTransform mRectTransformContainer;

	protected List<DynamicInfinityItem> mList_items;

	private Dictionary<int, DynamicRect> mDict_dRect;

	protected IList mDataProviders;

	protected bool mHasInited;

	public Action<int> Callback;

	protected Coroutine m_Coroutine;

	public virtual void InitRendererList(DynamicInfinityItem.OnSelect OnSelect, DynamicInfinityItem.OnUpdateData OnUpdate, Action<int> cb = null)
	{
		if (mHasInited)
		{
			return;
		}
		mRectTransformContainer = base.transform as RectTransform;
		mMaskSize = base.transform.parent.GetComponent<RectTransform>().sizeDelta;
		mScrollRect = base.transform.parent.GetComponent<ScrollRect>();
		if (IsVertical)
		{
			mRendererCount = ColumnCount * (Mathf.CeilToInt(mMaskSize.y / GetBlockSizeY()) + 1);
		}
		else
		{
			mRendererCount = ColumnCount * (Mathf.CeilToInt(mMaskSize.x / GetBlockSizeX()) + 1);
		}
		_UpdateDynmicRects(mRendererCount);
		mList_items = new List<DynamicInfinityItem>();
		Callback = cb;
		for (int i = 0; i < mRendererCount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(RenderGO);
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			gameObject.layer = base.gameObject.layer;
			DynamicInfinityItem component = gameObject.GetComponent<DynamicInfinityItem>();
			if (component == null)
			{
				throw new Exception("Render must extend DynamicInfinityItem");
			}
			mList_items.Add(component);
			mList_items[i].DRect = mDict_dRect[i];
			mList_items[i].OnSelectHandler = OnSelect;
			mList_items[i].OnUpdateDataHandler = OnUpdate;
			gameObject.SetActive(value: false);
			_UpdateChildTransformPos(gameObject, i);
		}
		_SetListRenderSize(mRendererCount);
		mHasInited = true;
	}

	private void _SetListRenderSize(int count)
	{
		if (IsVertical)
		{
			mRectTransformContainer.sizeDelta = new Vector2(mRectTransformContainer.sizeDelta.x, (float)Mathf.CeilToInt((float)count * 1f / (float)ColumnCount) * GetBlockSizeY());
			mRectMask = new Rect(0f, 0f - mMaskSize.y, mMaskSize.x, mMaskSize.y);
		}
		else
		{
			mRectTransformContainer.sizeDelta = new Vector2((float)Mathf.CeilToInt((float)count * 1f / (float)ColumnCount) * GetBlockSizeX(), mRectTransformContainer.sizeDelta.y);
			mRectMask = new Rect(0f, 0f - mMaskSize.x, mMaskSize.x, mMaskSize.y);
		}
	}

	private void _UpdateChildTransformPos(GameObject child, int index)
	{
		int num = index / ColumnCount;
		int num2 = index % ColumnCount;
		Vector2 anchoredPosition = default(Vector2);
		if (IsVertical)
		{
			anchoredPosition.x = (float)num2 * GetBlockSizeX();
			anchoredPosition.y = 0f - CellSize.y - (float)num * GetBlockSizeY();
		}
		else
		{
			anchoredPosition.x = CellSize.x / 2f + (float)num * GetBlockSizeX();
			anchoredPosition.y = (float)(-num2) * GetBlockSizeY();
		}
		((RectTransform)child.transform).anchoredPosition3D = Vector3.zero;
		((RectTransform)child.transform).anchoredPosition = anchoredPosition;
	}

	protected float GetBlockSizeY()
	{
		return CellSize.y + SpacingSize.y;
	}

	protected float GetBlockSizeX()
	{
		return CellSize.x + SpacingSize.x;
	}

	private void _UpdateDynmicRects(int count)
	{
		mDict_dRect = new Dictionary<int, DynamicRect>();
		for (int i = 0; i < count; i++)
		{
			int num = i / ColumnCount;
			int num2 = i % ColumnCount;
			DynamicRect dynamicRect = null;
			dynamicRect = ((!IsVertical) ? new DynamicRect((float)num * GetBlockSizeX() + CellSize.x, (float)(-num2) * GetBlockSizeY(), CellSize.x, CellSize.y, i) : new DynamicRect((float)num2 * GetBlockSizeX(), (float)(-num) * GetBlockSizeY() - CellSize.y, CellSize.x, CellSize.y, i));
			mDict_dRect[i] = dynamicRect;
		}
	}

	public void SetDataProvider(IList datas)
	{
		_UpdateDynmicRects(datas.Count);
		_SetListRenderSize(datas.Count);
		mDataProviders = datas;
		ClearAllListRenderDr();
	}

	private void ClearAllListRenderDr()
	{
		if (mList_items != null)
		{
			int count = mList_items.Count;
			for (int i = 0; i < count; i++)
			{
				mList_items[i].DRect = null;
			}
		}
	}

	public IList GetDataProvider()
	{
		return mDataProviders;
	}

	[ContextMenu("RefreshDataProvider")]
	public void RefreshDataProvider()
	{
		if (mDataProviders == null)
		{
			throw new Exception("dataProviders 为空！请先使用SetDataProvider ");
		}
		_UpdateDynmicRects(mDataProviders.Count);
		_SetListRenderSize(mDataProviders.Count);
		ClearAllListRenderDr();
	}

	public virtual void LocateRenderItemAtTarget(object target, float delay)
	{
		LocateRenderItemAtIndex(mDataProviders.IndexOf(target), delay);
	}

	public virtual void LocateRenderItemAtIndex(int index, float delay)
	{
		if (index < 0 || index > mDataProviders.Count - 1)
		{
			throw new Exception("Locate Index Error " + index);
		}
		index = Math.Min(index, mDataProviders.Count - mRendererCount + 2);
		index = Math.Max(0, index);
		Vector2 anchoredPosition = mRectTransformContainer.anchoredPosition;
		int num = index / ColumnCount;
		m_Coroutine = StartCoroutine(TweenMoveToPos(v2Pos: (!IsVertical) ? new Vector2((float)(-num) * GetBlockSizeX(), anchoredPosition.y) : new Vector2(anchoredPosition.x, (float)num * GetBlockSizeY()), pos: anchoredPosition, delay: delay));
	}

	protected IEnumerator TweenMoveToPos(Vector2 pos, Vector2 v2Pos, float delay)
	{
		bool running = true;
		float passedTime = 0f;
		while (running)
		{
			yield return new WaitForEndOfFrame();
			passedTime += Time.deltaTime;
			_ = Vector2.zero;
			Vector2 anchoredPosition;
			if (passedTime >= delay)
			{
				anchoredPosition = v2Pos;
				running = false;
				StopCoroutine(m_Coroutine);
				m_Coroutine = null;
			}
			else
			{
				anchoredPosition = Vector2.Lerp(pos, v2Pos, passedTime / delay);
			}
			mRectTransformContainer.anchoredPosition = anchoredPosition;
		}
	}

	protected void UpdateRender()
	{
		if (IsVertical)
		{
			mRectMask.y = 0f - mMaskSize.y - mRectTransformContainer.anchoredPosition.y;
		}
		else
		{
			mRectMask.x = mMaskSize.x - mRectTransformContainer.anchoredPosition.x;
		}
		Dictionary<int, DynamicRect> dictionary = new Dictionary<int, DynamicRect>();
		foreach (DynamicRect value in mDict_dRect.Values)
		{
			if (value.Overlaps(mRectMask))
			{
				dictionary.Add(value.Index, value);
			}
		}
		int count = mList_items.Count;
		for (int i = 0; i < count; i++)
		{
			DynamicInfinityItem dynamicInfinityItem = mList_items[i];
			if (dynamicInfinityItem.DRect != null && !dictionary.ContainsKey(dynamicInfinityItem.DRect.Index))
			{
				dynamicInfinityItem.DRect = null;
			}
		}
		foreach (DynamicRect value2 in dictionary.Values)
		{
			if (GetDynmicItem(value2) == null)
			{
				DynamicInfinityItem nullDynmicItem = GetNullDynmicItem();
				nullDynmicItem.DRect = value2;
				_UpdateChildTransformPos(nullDynmicItem.gameObject, value2.Index);
				if (mDataProviders != null && value2.Index < mDataProviders.Count)
				{
					nullDynmicItem.SetData(mDataProviders[value2.Index]);
				}
			}
			else if (Callback != null)
			{
				Callback(value2.Index);
			}
		}
	}

	private DynamicInfinityItem GetNullDynmicItem()
	{
		int count = mList_items.Count;
		for (int i = 0; i < count; i++)
		{
			DynamicInfinityItem dynamicInfinityItem = mList_items[i];
			if (dynamicInfinityItem.DRect == null)
			{
				return dynamicInfinityItem;
			}
		}
		throw new Exception("Error");
	}

	private DynamicInfinityItem GetDynmicItem(DynamicRect rect)
	{
		int count = mList_items.Count;
		for (int i = 0; i < count; i++)
		{
			DynamicInfinityItem dynamicInfinityItem = mList_items[i];
			if (dynamicInfinityItem.DRect != null && rect.Index == dynamicInfinityItem.DRect.Index)
			{
				return dynamicInfinityItem;
			}
		}
		return null;
	}

	private void Update()
	{
		if (mHasInited)
		{
			UpdateRender();
		}
	}
}
