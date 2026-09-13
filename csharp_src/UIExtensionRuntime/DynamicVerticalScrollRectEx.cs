using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[SelectionBase]
public class DynamicVerticalScrollRectEx : ScrollRect
{
	private struct ItemInfo
	{
		public int index;

		public int prefabIdx;

		public float topY;

		public float bottomY;
	}

	private struct ItemRef
	{
		public int dataIndex;

		public int prefabIdx;

		public GameObject objRef;

		public ItemRef(int dataIndex, int prefabIdx, GameObject objRef)
		{
			this.dataIndex = dataIndex;
			this.prefabIdx = prefabIdx;
			this.objRef = objRef;
		}

		public void Destroy()
		{
			if (!(objRef == null))
			{
				UnityEngine.Object.Destroy(objRef);
			}
		}
	}

	public List<GameObject> itemPrefabs;

	public List<float> itemHeights;

	public float padding;

	public float margin;

	private ItemInfo[] _itemInfos;

	private float _offsetLimit;

	private float _offset;

	private Vector2Int _prevIdxSeg = new Vector2Int(-1, -1);

	private Dictionary<int, ItemRef> _items = new Dictionary<int, ItemRef>();

	private Dictionary<int, Queue<ItemRef>> _itemPools = new Dictionary<int, Queue<ItemRef>>();

	private Dictionary<int, float> _overrideItemHeights = new Dictionary<int, float>();

	private int _minOverrideItemHeightDataIdx = -1;

	private bool _updateItemsFlag;

	private HashSet<int> _tempIdxs = new HashSet<int>();

	private Vector2 m_PointerStartLocalPos = Vector2.zero;

	private float _draggingStartOffset;

	private bool m_dragging;

	private float m_PrevOffset;

	public float contentH => base.content.rect.height;

	public float normalizedOffset
	{
		get
		{
			if (_offsetLimit <= 0f)
			{
				return 1f;
			}
			return _offset / _offsetLimit;
		}
		set
		{
			if (_offsetLimit <= 0f)
			{
				_offset = 0f;
			}
			else
			{
				_offset = _offsetLimit * value;
			}
			_updateItemsFlag = true;
		}
	}

	public event Action<GameObject, int> onInstantiateItem;

	public event Action<GameObject, int> onDisplayItem;

	public event Action<GameObject> onClearItem;

	public event Action<float> onOffsetChanged;

	public event Action<GameObject> onArabicMirrorItemInstantiated;

	private new void OnDestroy()
	{
		Clear();
	}

	public void Clear()
	{
		_itemInfos = null;
		_offsetLimit = 0f;
		_offset = 0f;
		_prevIdxSeg = new Vector2Int(-1, -1);
		foreach (KeyValuePair<int, ItemRef> item in _items)
		{
			item.Value.Destroy();
		}
		_items.Clear();
		foreach (KeyValuePair<int, Queue<ItemRef>> itemPool in _itemPools)
		{
			foreach (ItemRef item2 in itemPool.Value)
			{
				item2.Destroy();
			}
		}
		_itemPools.Clear();
		this.onInstantiateItem = null;
		this.onDisplayItem = null;
		this.onClearItem = null;
		this.onOffsetChanged = null;
	}

	public void SetDatas(int[] prefabIdxs)
	{
		__RecycleAllItems();
		_itemInfos = new ItemInfo[prefabIdxs.Length];
		_overrideItemHeights.Clear();
		float num = margin;
		for (int i = 0; i < prefabIdxs.Length; i++)
		{
			ItemInfo itemInfo = _itemInfos[i];
			itemInfo.index = i;
			itemInfo.prefabIdx = prefabIdxs[i];
			itemInfo.topY = num;
			itemInfo.bottomY = num + itemHeights[itemInfo.prefabIdx];
			num = itemInfo.bottomY + padding;
			_itemInfos[i] = itemInfo;
		}
		__UpdateScrollRect(num - padding + margin - contentH);
	}

	public void SetOverrideItemHeight(int dataIdx, float height)
	{
		if (dataIdx >= 0 && dataIdx < _itemInfos.Length && (!_overrideItemHeights.ContainsKey(dataIdx) || _overrideItemHeights[dataIdx] != height) && itemHeights[_itemInfos[dataIdx].prefabIdx] != height)
		{
			_overrideItemHeights[dataIdx] = height;
			if (_minOverrideItemHeightDataIdx < 0)
			{
				_minOverrideItemHeightDataIdx = dataIdx;
			}
			else if (dataIdx < _minOverrideItemHeightDataIdx)
			{
				_minOverrideItemHeightDataIdx = dataIdx;
			}
		}
	}

	public GameObject FindItemByDataIdx(int dataIdx)
	{
		if (_items.TryGetValue(dataIdx, out var value))
		{
			return value.objRef;
		}
		return null;
	}

	public float GetScrollOffsetOfDataIdx(int dataIdx, float additionOffset)
	{
		if (dataIdx < 0 || dataIdx >= _itemInfos.Length)
		{
			return 0f;
		}
		float num = _itemInfos[dataIdx].topY + additionOffset;
		return num + CalculateOverLimit(num);
	}

	public void SetScrollOffset(float offset)
	{
		_offset = offset;
		_updateItemsFlag = true;
	}

	private void __UpdateScrollRect(float limit)
	{
		_offsetLimit = Mathf.Max(0f, limit);
		_updateItemsFlag = true;
		UpdateScrollbars();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if ((bool)base.verticalScrollbar)
		{
			base.verticalScrollbar.onValueChanged.AddListener(SetNormalizedOffset);
		}
		_updateItemsFlag = true;
	}

	protected override void OnDisable()
	{
		if ((bool)base.verticalScrollbar)
		{
			base.verticalScrollbar.onValueChanged.RemoveListener(SetNormalizedOffset);
		}
		base.OnDisable();
	}

	public override void Rebuild(CanvasUpdate executing)
	{
		if (executing == CanvasUpdate.PostLayout)
		{
			UpdateScrollbars();
			UpdatePrevOffset();
		}
	}

	private int __FindStartIdx()
	{
		int num = 0;
		int num2 = _itemInfos.Length - 1;
		while (num2 >= num)
		{
			int num3 = (num + num2) / 2;
			if (num3 < 0 || num3 >= _itemInfos.Length)
			{
				return -1;
			}
			ItemInfo itemInfo = _itemInfos[num3];
			if (_offset > itemInfo.topY && _offset < itemInfo.bottomY)
			{
				return num3;
			}
			if (_offset <= itemInfo.topY)
			{
				if (num3 == num)
				{
					return num3;
				}
				num2 = num3 - 1;
			}
			else if (_offset >= itemInfo.bottomY)
			{
				if (num3 == num2)
				{
					return num3;
				}
				num = num3 + 1;
			}
		}
		return -1;
	}

	private int __FindEndIdx(ref int startIdx)
	{
		if (startIdx == -1 && _itemInfos.Length != 0 && _itemInfos[0].topY <= _offset + contentH)
		{
			startIdx = 0;
		}
		if (startIdx == -1)
		{
			return -1;
		}
		for (int i = startIdx; i < _itemInfos.Length; i++)
		{
			if (_itemInfos[i].bottomY > _offset + contentH)
			{
				return i;
			}
			if (i == _itemInfos.Length - 1)
			{
				return i;
			}
			if (_itemInfos[i + 1].topY > _offset + contentH)
			{
				return i;
			}
		}
		return -1;
	}

	private ItemRef __AskItemFromPool(int prefabIdx)
	{
		if (!_itemPools.TryGetValue(prefabIdx, out var value))
		{
			value = new Queue<ItemRef>();
			_itemPools.Add(prefabIdx, value);
		}
		if (value.Count > 0)
		{
			return value.Dequeue();
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(itemPrefabs[prefabIdx]);
		this.onArabicMirrorItemInstantiated?.Invoke(gameObject);
		gameObject.transform.SetParent(base.content);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		(gameObject.transform as RectTransform).pivot = new Vector2(0.5f, 1f);
		(gameObject.transform as RectTransform).anchorMin = new Vector2(0.5f, 1f);
		(gameObject.transform as RectTransform).anchorMax = new Vector2(0.5f, 1f);
		this.onInstantiateItem?.Invoke(gameObject, prefabIdx);
		return new ItemRef(-1, prefabIdx, gameObject);
	}

	private void __RecycleAllItems()
	{
		foreach (KeyValuePair<int, ItemRef> item in _items)
		{
			ItemRef value = item.Value;
			value.objRef.SetActive(value: false);
			value.dataIndex = -1;
			_itemPools[value.prefabIdx].Enqueue(value);
			this.onClearItem?.Invoke(value.objRef);
		}
		_items.Clear();
		_prevIdxSeg = new Vector2Int(-1, -1);
	}

	private void __PrepareItems(int startIdx, int endIdx)
	{
		_tempIdxs.Clear();
		int num = _prevIdxSeg.y;
		while (num > endIdx && num >= _prevIdxSeg.x)
		{
			_tempIdxs.Add(num);
			num--;
		}
		for (int num2 = Mathf.Min(startIdx - 1, _prevIdxSeg.y); num2 >= _prevIdxSeg.x; num2--)
		{
			_tempIdxs.Add(num2);
		}
		foreach (int tempIdx in _tempIdxs)
		{
			if (tempIdx >= 0 && tempIdx < _itemInfos.Length && _items.ContainsKey(tempIdx))
			{
				ItemRef item = _items[tempIdx];
				item.objRef.SetActive(value: false);
				item.dataIndex = -1;
				_itemPools[item.prefabIdx].Enqueue(item);
				_items.Remove(tempIdx);
				this.onClearItem?.Invoke(item.objRef);
			}
		}
		_tempIdxs.Clear();
		for (int i = startIdx; i < _prevIdxSeg.x && i <= endIdx; i++)
		{
			_tempIdxs.Add(i);
		}
		for (int j = Mathf.Max(startIdx, _prevIdxSeg.y + 1); j <= endIdx; j++)
		{
			_tempIdxs.Add(j);
		}
		foreach (int tempIdx2 in _tempIdxs)
		{
			if (tempIdx2 >= 0 && tempIdx2 < _itemInfos.Length && !_items.ContainsKey(tempIdx2))
			{
				ItemRef value = __AskItemFromPool(_itemInfos[tempIdx2].prefabIdx);
				value.objRef.SetActive(value: true);
				value.dataIndex = tempIdx2;
				_items.Add(tempIdx2, value);
				this.onDisplayItem?.Invoke(value.objRef, tempIdx2);
			}
		}
	}

	private void __RefreshAllItemsInView(int startIdx, int endIdx)
	{
		for (int i = startIdx; i <= endIdx; i++)
		{
			if (i >= 0 && i < _itemInfos.Length)
			{
				ItemRef itemRef = _items[i];
				this.onDisplayItem?.Invoke(itemRef.objRef, i);
			}
		}
	}

	private void __PutItemsInRightPlace(int startIdx, int endIdx)
	{
		for (int i = startIdx; i <= endIdx; i++)
		{
			if (i >= 0 && i < _itemInfos.Length)
			{
				ItemInfo itemInfo = _itemInfos[i];
				(_items[i].objRef.transform as RectTransform).anchoredPosition = new Vector2(0f, _offset - itemInfo.topY);
			}
		}
	}

	public void UpdateItems()
	{
		if (_itemInfos != null)
		{
			int startIdx = __FindStartIdx();
			int num = __FindEndIdx(ref startIdx);
			__PrepareItems(startIdx, num);
			_prevIdxSeg = new Vector2Int(startIdx, num);
			__PutItemsInRightPlace(startIdx, num);
			__RefreshAllItemsInView(startIdx, num);
		}
	}

	public override void OnScroll(PointerEventData data)
	{
		if (IsActive())
		{
			Vector2 scrollDelta = data.scrollDelta;
			scrollDelta.y *= -1f;
			if (Mathf.Abs(scrollDelta.x) > Mathf.Abs(scrollDelta.y))
			{
				scrollDelta.y = scrollDelta.x;
			}
			scrollDelta.x = 0f;
			_offset += scrollDelta.y * base.scrollSensitivity;
			if (base.movementType == MovementType.Clamped)
			{
				_offset += CalculateOverLimit(_offset);
			}
			_updateItemsFlag = true;
		}
	}

	private float CalculateOverLimit(float offset)
	{
		if (base.movementType == MovementType.Unrestricted)
		{
			return 0f;
		}
		float num = 0f;
		float offsetLimit = _offsetLimit;
		if (offset < num)
		{
			return num - offset;
		}
		if (offset > offsetLimit)
		{
			return offsetLimit - offset;
		}
		return 0f;
	}

	public override void OnInitializePotentialDrag(PointerEventData eventData)
	{
		base.velocity = Vector2.zero;
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && IsActive())
		{
			m_PointerStartLocalPos = Vector2.zero;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(base.viewRect, eventData.position, eventData.pressEventCamera, out m_PointerStartLocalPos);
			_draggingStartOffset = _offset;
			m_dragging = true;
		}
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			m_dragging = false;
		}
	}

	public override void OnDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && IsActive() && RectTransformUtility.ScreenPointToLocalPointInRectangle(base.viewRect, eventData.position, eventData.pressEventCamera, out var localPoint))
		{
			Vector2 vector = localPoint - m_PointerStartLocalPos;
			_offset = _draggingStartOffset + vector.y;
			float overStretching = CalculateOverLimit(_offset);
			if (base.movementType == MovementType.Elastic)
			{
				_offset += RubberDelta(overStretching, contentH);
			}
			else if (base.movementType == MovementType.Clamped)
			{
				_offset += CalculateOverLimit(_offset);
			}
			_updateItemsFlag = true;
		}
	}

	private static float RubberDelta(float overStretching, float viewSize)
	{
		return (1f - 1f / (Mathf.Abs(overStretching) * 0.55f / viewSize + 1f)) * viewSize * Mathf.Sign(overStretching);
	}

	protected override void LateUpdate()
	{
		base.onValueChanged.Invoke(Vector2.zero);
	}

	protected void Update()
	{
		if (_minOverrideItemHeightDataIdx >= 0)
		{
			int i = _minOverrideItemHeightDataIdx;
			float num = ((i == 0) ? margin : (_itemInfos[i - 1].bottomY + padding));
			for (; i < _itemInfos.Length; i++)
			{
				ItemInfo itemInfo = _itemInfos[i];
				itemInfo.topY = num;
				itemInfo.bottomY = num + (_overrideItemHeights.ContainsKey(i) ? _overrideItemHeights[i] : itemHeights[itemInfo.prefabIdx]);
				num = itemInfo.bottomY + padding;
				_itemInfos[i] = itemInfo;
			}
			__UpdateScrollRect(num - padding + margin - contentH);
			_minOverrideItemHeightDataIdx = -1;
			_updateItemsFlag = true;
		}
		if (!base.content)
		{
			return;
		}
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		float num2 = CalculateOverLimit(_offset);
		if (!m_dragging && (num2 != 0f || base.velocity != Vector2.zero))
		{
			float num3 = _offset;
			if (base.movementType == MovementType.Elastic && num2 != 0f)
			{
				float currentVelocity = base.velocity.y;
				num3 = Mathf.SmoothDamp(_offset, _offset + num2, ref currentVelocity, base.elasticity, float.PositiveInfinity, unscaledDeltaTime);
				base.velocity = new Vector2(0f, currentVelocity);
			}
			else if (base.inertia)
			{
				base.velocity *= new Vector2(0f, Mathf.Pow(base.decelerationRate, unscaledDeltaTime));
				if (Mathf.Abs(base.velocity.y) < 1f)
				{
					base.velocity = Vector2.zero;
				}
				num3 += base.velocity.y * unscaledDeltaTime;
			}
			else
			{
				base.velocity = Vector2.zero;
			}
			if (base.velocity != Vector2.zero)
			{
				_offset = num3;
				if (base.movementType == MovementType.Clamped)
				{
					_offset += CalculateOverLimit(_offset);
				}
				_updateItemsFlag = true;
			}
		}
		if (m_dragging && base.inertia)
		{
			base.velocity = Vector3.Lerp(b: new Vector3(0f, (_offset - m_PrevOffset) / unscaledDeltaTime, 0f), a: base.velocity, t: unscaledDeltaTime * 10f);
		}
		if (_offset != m_PrevOffset)
		{
			UpdateScrollbars();
			if (this.onOffsetChanged != null)
			{
				this.onOffsetChanged(normalizedOffset);
			}
			UpdatePrevOffset();
		}
		if (_updateItemsFlag)
		{
			UpdateItems();
			_updateItemsFlag = false;
		}
	}

	private void SetNormalizedOffset(float value)
	{
		normalizedOffset = 1f - value;
	}

	private void UpdatePrevOffset()
	{
		if (base.content == null)
		{
			m_PrevOffset = 0f;
		}
		else
		{
			m_PrevOffset = _offset;
		}
	}

	private void UpdateScrollbars()
	{
		if ((bool)base.verticalScrollbar && base.verticalScrollbar.gameObject.activeSelf)
		{
			if (_offsetLimit > 0f)
			{
				base.verticalScrollbar.size = contentH / (contentH + _offsetLimit);
			}
			else
			{
				base.verticalScrollbar.size = 1f;
			}
			base.verticalScrollbar.value = 1f - normalizedOffset;
		}
	}
}
