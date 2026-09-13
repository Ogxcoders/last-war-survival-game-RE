using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[SelectionBase]
public class DynamicVerticalScrollRect : ScrollRect
{
	private struct ItemRef
	{
		public int dataIndex;

		public GameObject objRef;

		public ItemRef(int dataIndex, GameObject objRef)
		{
			this.dataIndex = dataIndex;
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

	public GameObject itemPrefab;

	public Vector2 itemSize;

	public Vector2 padding;

	public Vector2 margin;

	private int _itemsPerRow;

	private int _rowsPerPage;

	private int _totalRows;

	private float _offsetLimit;

	private float _offset;

	private int _dataCount;

	private Vector2Int _prevIdxSeg = new Vector2Int(-1, -1);

	private Dictionary<int, ItemRef> _items = new Dictionary<int, ItemRef>();

	private Queue<ItemRef> _itemPool = new Queue<ItemRef>();

	private bool _updateItemsFlag;

	private HashSet<int> _tempIdxs = new HashSet<int>();

	private Vector2 m_PointerStartLocalPos = Vector2.zero;

	private float _draggingStartOffset;

	private bool m_dragging;

	private float m_PrevOffset;

	public float contentW => base.content.rect.width - ((base.verticalScrollbar != null && base.verticalScrollbar.gameObject.activeSelf) ? (base.verticalScrollbar.transform as RectTransform).rect.width : 0f);

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

	public event Action<GameObject> onInstantiateItem;

	public event Action<GameObject, int> onDisplayItem;

	public event Action<GameObject> onClearItem;

	public event Action<float> onOffsetChanged;

	private new void OnDestroy()
	{
		Clear();
	}

	public void Clear()
	{
		_itemsPerRow = 0;
		_rowsPerPage = 0;
		_totalRows = 0;
		_offsetLimit = 0f;
		_offset = 0f;
		_dataCount = 0;
		_prevIdxSeg = new Vector2Int(-1, -1);
		foreach (KeyValuePair<int, ItemRef> item in _items)
		{
			item.Value.Destroy();
		}
		_items.Clear();
		foreach (ItemRef item2 in _itemPool)
		{
			item2.Destroy();
		}
		_itemPool.Clear();
		this.onInstantiateItem = null;
		this.onDisplayItem = null;
		this.onClearItem = null;
		this.onOffsetChanged = null;
	}

	public void SetDatas(int dataCount)
	{
		__RecycleAllItems();
		_dataCount = dataCount;
		__UpdateScrollRect();
	}

	public float GetScrollOffsetOfDataIdx(int dataIdx, float additionOffset)
	{
		if (dataIdx < 0 || dataIdx >= _dataCount)
		{
			return 0f;
		}
		float num = (float)(dataIdx / _itemsPerRow) * (itemSize.y + padding.y) + margin.y + additionOffset;
		return num + CalculateOverLimit(num);
	}

	public void SetScrollOffset(float offset)
	{
		_offset = offset;
		_updateItemsFlag = true;
	}

	private void __UpdateScrollRect()
	{
		_itemsPerRow = 1 + (int)((contentW - margin.x * 2f - itemSize.x) / (itemSize.x + padding.x));
		_rowsPerPage = Mathf.CeilToInt(contentH / (itemSize.y + padding.y)) + 1;
		_totalRows = Mathf.CeilToInt((float)_dataCount * 1f / (float)_itemsPerRow);
		_offsetLimit = Mathf.Max(0f, (float)_totalRows * (itemSize.y + padding.y) - padding.y + margin.y * 2f - contentH);
		_updateItemsFlag = true;
		UpdateScrollbars();
	}

	public GameObject FindItemByDataIdx(int dataIdx)
	{
		if (_items.TryGetValue(dataIdx, out var value))
		{
			return value.objRef;
		}
		return null;
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

	private ItemRef __AskItemFromPool()
	{
		if (_itemPool.Count > 0)
		{
			return _itemPool.Dequeue();
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(itemPrefab);
		gameObject.transform.SetParent(base.content);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		(gameObject.transform as RectTransform).pivot = new Vector2(0f, 1f);
		(gameObject.transform as RectTransform).anchorMin = new Vector2(0f, 1f);
		(gameObject.transform as RectTransform).anchorMax = new Vector2(0f, 1f);
		this.onInstantiateItem?.Invoke(gameObject);
		return new ItemRef(-1, gameObject);
	}

	private void __RecycleAllItems()
	{
		foreach (KeyValuePair<int, ItemRef> item in _items)
		{
			ItemRef value = item.Value;
			value.objRef.SetActive(value: false);
			value.dataIndex = -1;
			_itemPool.Enqueue(value);
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
			if (tempIdx >= 0 && tempIdx < _dataCount && _items.ContainsKey(tempIdx))
			{
				ItemRef item = _items[tempIdx];
				item.objRef.SetActive(value: false);
				item.dataIndex = -1;
				_itemPool.Enqueue(item);
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
			if (tempIdx2 >= 0 && tempIdx2 < _dataCount && !_items.ContainsKey(tempIdx2))
			{
				ItemRef value = __AskItemFromPool();
				value.objRef.SetActive(value: true);
				value.dataIndex = tempIdx2;
				_items.Add(tempIdx2, value);
				this.onDisplayItem?.Invoke(value.objRef, tempIdx2);
			}
		}
	}

	private void __PutItemsInRightPlace(int startIdx, int endIdx)
	{
		for (int i = startIdx; i <= endIdx; i++)
		{
			if (i >= 0 && i < _dataCount)
			{
				int num = i / _itemsPerRow;
				int num2 = i % _itemsPerRow;
				ItemRef itemRef = _items[i];
				float x = (float)num2 * itemSize.x + (float)num2 * padding.x + margin.x;
				float num3 = (float)num * itemSize.y + (float)num * padding.y + margin.y;
				(itemRef.objRef.transform as RectTransform).anchoredPosition = new Vector2(x, _offset - num3);
			}
		}
	}

	public void UpdateItems()
	{
		int num = (int)((_offset - margin.y) / (itemSize.y + padding.y)) * _itemsPerRow;
		int num2 = num + _rowsPerPage * _itemsPerRow;
		__PrepareItems(num, num2);
		_prevIdxSeg = new Vector2Int(num, num2);
		__PutItemsInRightPlace(num, num2);
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
		if (!base.content)
		{
			return;
		}
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		float num = CalculateOverLimit(_offset);
		if (!m_dragging && (num != 0f || base.velocity != Vector2.zero))
		{
			float num2 = _offset;
			if (base.movementType == MovementType.Elastic && num != 0f)
			{
				float currentVelocity = base.velocity.y;
				num2 = Mathf.SmoothDamp(_offset, _offset + num, ref currentVelocity, base.elasticity, float.PositiveInfinity, unscaledDeltaTime);
				base.velocity = new Vector2(0f, currentVelocity);
			}
			else if (base.inertia)
			{
				base.velocity *= new Vector2(0f, Mathf.Pow(base.decelerationRate, unscaledDeltaTime));
				if (Mathf.Abs(base.velocity.y) < 1f)
				{
					base.velocity = Vector2.zero;
				}
				num2 += base.velocity.y * unscaledDeltaTime;
			}
			else
			{
				base.velocity = Vector2.zero;
			}
			if (base.velocity != Vector2.zero)
			{
				_offset = num2;
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
