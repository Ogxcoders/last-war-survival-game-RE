using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

[AddComponentMenu("UI/ScrollView", 51)]
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class ScrollView : UIBehaviour, IInitializePotentialDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler, ICanvasElement, ILayoutElement, ILayoutGroup, ILayoutController, IPointerDownHandler
{
	private class ItemObject
	{
		public bool used;

		public GameObject gameObject;
	}

	public enum MovementType
	{
		Unrestricted,
		Elastic,
		Clamped
	}

	public enum ScrollbarVisibility
	{
		Permanent,
		AutoHide,
		AutoHideAndExpandViewport
	}

	public enum ScrollViewLayoutType
	{
		Horizontal,
		Vertical
	}

	[Serializable]
	public class ScrollRectEvent : UnityEvent<Vector2>
	{
	}

	public delegate void MoveItemDelegate(GameObject itemObj, int index);

	public delegate void PointerEventDelegate(PointerEventData pointerEventData);

	private static GameObject objectPoolRoot;

	[SerializeField]
	private GameObject itemTemplate;

	private List<ItemObject> itemObjectPool = new List<ItemObject>();

	private List<GameObject> unUseObjectList;

	[Tooltip("Total count, negative means INFINITE mode")]
	public int totalCount;

	protected float threshold;

	[Tooltip("Reverse direction for dragging")]
	public bool reverseDirection;

	[HideInInspector]
	public bool arabicMirrorDirection;

	[Tooltip("Rubber scale for outside")]
	public float rubberScale = 1f;

	protected int itemTypeStart;

	protected int itemTypeEnd;

	private Vector2 extraFillSize = Vector2.zero;

	protected int directionSign;

	private float m_ContentSpacing = -1f;

	protected GridLayoutGroup m_GridLayout;

	public int m_ContentConstraintCount;

	[SerializeField]
	private ScrollViewLayoutType layoutType;

	[SerializeField]
	private RectTransform m_Content;

	[SerializeField]
	private MovementType m_MovementType = MovementType.Elastic;

	[SerializeField]
	private float m_Elasticity = 0.1f;

	[SerializeField]
	private bool m_Inertia = true;

	[SerializeField]
	private float m_DecelerationRate = 0.135f;

	[SerializeField]
	private float m_ScrollSensitivity = 1f;

	[SerializeField]
	private RectTransform m_Viewport;

	[SerializeField]
	private Scrollbar m_HorizontalScrollbar;

	[SerializeField]
	private Scrollbar m_VerticalScrollbar;

	[SerializeField]
	private ScrollbarVisibility m_HorizontalScrollbarVisibility;

	[SerializeField]
	private ScrollbarVisibility m_VerticalScrollbarVisibility;

	[SerializeField]
	private float m_HorizontalScrollbarSpacing;

	[SerializeField]
	private float m_VerticalScrollbarSpacing;

	[SerializeField]
	private ScrollRectEvent m_OnValueChanged = new ScrollRectEvent();

	private Vector2 m_PointerStartLocalCursor = Vector2.zero;

	private Vector2 m_ContentStartPosition = Vector2.zero;

	private RectTransform m_ViewRect;

	private Bounds m_ContentBounds;

	private Bounds m_ViewBounds;

	private Vector2 m_Velocity;

	private bool m_Dragging;

	private Vector2 m_PrevPosition = Vector2.zero;

	private Bounds m_PrevContentBounds;

	private Bounds m_PrevViewBounds;

	[NonSerialized]
	private bool m_HasRebuiltLayout;

	private bool m_HSliderExpand;

	private bool m_VSliderExpand;

	private float m_HSliderHeight;

	private float m_VSliderWidth;

	private bool useFixedItemSize;

	private Vector2 fixedItemSize = Vector2.zero;

	[NonSerialized]
	private RectTransform m_Rect;

	private RectTransform m_HorizontalScrollbarRect;

	private RectTransform m_VerticalScrollbarRect;

	private DrivenRectTransformTracker m_Tracker;

	private string _name;

	private readonly Vector3[] m_Corners = new Vector3[4];

	public MoveItemDelegate onItemMoveIn;

	public MoveItemDelegate onItemMoveOut;

	public PointerEventDelegate onBeginDragCallBack;

	public PointerEventDelegate onEndDragCallBack;

	public PointerEventDelegate onDragCallBack;

	public PointerEventDelegate onScrollToCellEndCallBack;

	private static Transform ObjectPoolRoot
	{
		get
		{
			if (objectPoolRoot == null)
			{
				objectPoolRoot = new GameObject("ScrollViewObjectPoolRoot");
				UnityEngine.Object.DontDestroyOnLoad(objectPoolRoot);
			}
			return objectPoolRoot.transform;
		}
	}

	protected float contentSpacing
	{
		get
		{
			if (m_ContentSpacing >= 0f)
			{
				return m_ContentSpacing;
			}
			m_ContentSpacing = 0f;
			if (content != null)
			{
				HorizontalOrVerticalLayoutGroup component = content.GetComponent<HorizontalOrVerticalLayoutGroup>();
				if (component != null)
				{
					m_ContentSpacing = component.spacing;
				}
				m_GridLayout = content.GetComponent<GridLayoutGroup>();
				if (m_GridLayout != null)
				{
					m_ContentSpacing = Mathf.Abs(GetDimension(m_GridLayout.spacing));
				}
			}
			return m_ContentSpacing;
		}
	}

	protected int contentConstraintCount
	{
		get
		{
			if (m_ContentConstraintCount > 0)
			{
				return m_ContentConstraintCount;
			}
			m_ContentConstraintCount = 1;
			if (content != null)
			{
				GridLayoutGroup component = content.GetComponent<GridLayoutGroup>();
				if (component != null)
				{
					if (component.constraint == GridLayoutGroup.Constraint.Flexible)
					{
						Debug.LogWarning("[LoopScrollRect] Flexible not supported yet");
					}
					m_ContentConstraintCount = component.constraintCount;
				}
			}
			return m_ContentConstraintCount;
		}
	}

	private int StartLine => Mathf.CeilToInt((float)itemTypeStart / (float)contentConstraintCount);

	private int CurrentLines => Mathf.CeilToInt((float)(itemTypeEnd - itemTypeStart) / (float)contentConstraintCount);

	private int TotalLines => Mathf.CeilToInt((float)totalCount / (float)contentConstraintCount);

	public RectTransform content
	{
		get
		{
			return m_Content;
		}
		set
		{
			m_Content = value;
		}
	}

	public MovementType movementType
	{
		get
		{
			return m_MovementType;
		}
		set
		{
			m_MovementType = value;
		}
	}

	public float elasticity
	{
		get
		{
			return m_Elasticity;
		}
		set
		{
			m_Elasticity = value;
		}
	}

	public bool inertia
	{
		get
		{
			return m_Inertia;
		}
		set
		{
			m_Inertia = value;
		}
	}

	public float decelerationRate
	{
		get
		{
			return m_DecelerationRate;
		}
		set
		{
			m_DecelerationRate = value;
		}
	}

	public float scrollSensitivity
	{
		get
		{
			return m_ScrollSensitivity;
		}
		set
		{
			m_ScrollSensitivity = value;
		}
	}

	public RectTransform viewport
	{
		get
		{
			return m_Viewport;
		}
		set
		{
			m_Viewport = value;
			SetDirtyCaching();
		}
	}

	public Scrollbar horizontalScrollbar
	{
		get
		{
			return m_HorizontalScrollbar;
		}
		set
		{
			if ((bool)m_HorizontalScrollbar)
			{
				m_HorizontalScrollbar.onValueChanged.RemoveListener(SetHorizontalNormalizedPosition);
			}
			m_HorizontalScrollbar = value;
			if ((bool)m_HorizontalScrollbar)
			{
				m_HorizontalScrollbar.onValueChanged.AddListener(SetHorizontalNormalizedPosition);
			}
			SetDirtyCaching();
		}
	}

	public Scrollbar verticalScrollbar
	{
		get
		{
			return m_VerticalScrollbar;
		}
		set
		{
			if ((bool)m_VerticalScrollbar)
			{
				m_VerticalScrollbar.onValueChanged.RemoveListener(SetVerticalNormalizedPosition);
			}
			m_VerticalScrollbar = value;
			if ((bool)m_VerticalScrollbar)
			{
				m_VerticalScrollbar.onValueChanged.AddListener(SetVerticalNormalizedPosition);
			}
			SetDirtyCaching();
		}
	}

	public ScrollbarVisibility horizontalScrollbarVisibility
	{
		get
		{
			return m_HorizontalScrollbarVisibility;
		}
		set
		{
			m_HorizontalScrollbarVisibility = value;
			SetDirtyCaching();
		}
	}

	public ScrollbarVisibility verticalScrollbarVisibility
	{
		get
		{
			return m_VerticalScrollbarVisibility;
		}
		set
		{
			m_VerticalScrollbarVisibility = value;
			SetDirtyCaching();
		}
	}

	public float horizontalScrollbarSpacing
	{
		get
		{
			return m_HorizontalScrollbarSpacing;
		}
		set
		{
			m_HorizontalScrollbarSpacing = value;
			SetDirty();
		}
	}

	public float verticalScrollbarSpacing
	{
		get
		{
			return m_VerticalScrollbarSpacing;
		}
		set
		{
			m_VerticalScrollbarSpacing = value;
			SetDirty();
		}
	}

	public ScrollRectEvent onValueChanged
	{
		get
		{
			return m_OnValueChanged;
		}
		set
		{
			m_OnValueChanged = value;
		}
	}

	protected RectTransform viewRect
	{
		get
		{
			if (m_ViewRect == null)
			{
				m_ViewRect = m_Viewport;
			}
			if (m_ViewRect == null)
			{
				m_ViewRect = (RectTransform)base.transform;
			}
			return m_ViewRect;
		}
	}

	public Vector2 velocity
	{
		get
		{
			return m_Velocity;
		}
		set
		{
			m_Velocity = value;
		}
	}

	public Vector2 FixedItemSize
	{
		set
		{
			useFixedItemSize = true;
			fixedItemSize = value;
		}
	}

	private RectTransform rectTransform
	{
		get
		{
			if (m_Rect == null)
			{
				m_Rect = GetComponent<RectTransform>();
			}
			return m_Rect;
		}
	}

	public Vector2 normalizedPosition
	{
		get
		{
			return new Vector2(horizontalNormalizedPosition, verticalNormalizedPosition);
		}
		set
		{
			SetNormalizedPosition(value.x, 0);
			SetNormalizedPosition(value.y, 1);
		}
	}

	public float horizontalNormalizedPosition
	{
		get
		{
			UpdateBounds();
			if (totalCount > 0 && itemTypeEnd > itemTypeStart)
			{
				float num = m_ContentBounds.size.x / (float)CurrentLines;
				float num2 = num * (float)TotalLines;
				float num3 = m_ContentBounds.min.x - num * (float)StartLine;
				if (num2 <= m_ViewBounds.size.x)
				{
					return (m_ViewBounds.min.x > num3) ? 1 : 0;
				}
				return (m_ViewBounds.min.x - num3) / (num2 - m_ViewBounds.size.x);
			}
			return 0.5f;
		}
		set
		{
			SetNormalizedPosition(value, 0);
		}
	}

	public float verticalNormalizedPosition
	{
		get
		{
			UpdateBounds();
			if (totalCount > 0 && itemTypeEnd > itemTypeStart)
			{
				float num = m_ContentBounds.size.y / (float)CurrentLines;
				float num2 = num * (float)TotalLines;
				float num3 = m_ContentBounds.max.y + num * (float)StartLine;
				if (num2 <= m_ViewBounds.size.y)
				{
					return (num3 > m_ViewBounds.max.y) ? 1 : 0;
				}
				return (num3 - m_ViewBounds.max.y) / (num2 - m_ViewBounds.size.y);
			}
			return 0.5f;
		}
		set
		{
			SetNormalizedPosition(value, 1);
		}
	}

	private bool hScrollingNeeded
	{
		get
		{
			if (Application.isPlaying)
			{
				return m_ContentBounds.size.x > m_ViewBounds.size.x + 0.01f;
			}
			return true;
		}
	}

	private bool vScrollingNeeded
	{
		get
		{
			if (Application.isPlaying)
			{
				return m_ContentBounds.size.y > m_ViewBounds.size.y + 0.01f;
			}
			return true;
		}
	}

	public virtual float minWidth => -1f;

	public virtual float preferredWidth => -1f;

	public virtual float flexibleWidth { get; private set; }

	public virtual float minHeight => -1f;

	public virtual float preferredHeight => -1f;

	public virtual float flexibleHeight => -1f;

	public virtual int layoutPriority => -1;

	protected float GetSize(RectTransform item)
	{
		if (layoutType == ScrollViewLayoutType.Horizontal)
		{
			float num = contentSpacing;
			if (useFixedItemSize)
			{
				return num + fixedItemSize.x;
			}
			if (m_GridLayout != null)
			{
				return num + m_GridLayout.cellSize.x;
			}
			return num + LayoutUtility.GetPreferredWidth(item);
		}
		float num2 = contentSpacing;
		if (useFixedItemSize)
		{
			return num2 + fixedItemSize.y;
		}
		if (m_GridLayout != null)
		{
			return num2 + m_GridLayout.cellSize.y;
		}
		return num2 + LayoutUtility.GetPreferredHeight(item);
	}

	protected float GetDimension(Vector2 vector)
	{
		if (layoutType != ScrollViewLayoutType.Horizontal)
		{
			return vector.y;
		}
		return 0f - vector.x;
	}

	protected Vector2 GetVector(float value)
	{
		if (layoutType != ScrollViewLayoutType.Horizontal)
		{
			return new Vector2(0f, value);
		}
		return new Vector2(0f - value, 0f);
	}

	protected virtual bool UpdateItems(Bounds viewBounds, Bounds contentBounds)
	{
		bool result = false;
		if (layoutType == ScrollViewLayoutType.Horizontal)
		{
			if (!MirrorVersionConfig.IsMirrorVersionOpen && viewBounds.max.x + extraFillSize.x > contentBounds.max.x)
			{
				float num = NewItemAtEnd();
				float num2 = num;
				while (num > 0f && viewBounds.max.x + extraFillSize.x > contentBounds.max.x + num2)
				{
					num = NewItemAtEnd();
					num2 += num;
				}
				if (num2 > 0f)
				{
					result = true;
				}
			}
			if (MirrorVersionConfig.IsMirrorVersionOpen && viewBounds.min.x - extraFillSize.x < contentBounds.min.x)
			{
				float num3 = NewItemAtEnd();
				float num4 = num3;
				while (num3 > 0f && viewBounds.min.x - extraFillSize.x < contentBounds.min.x - num4)
				{
					num3 = NewItemAtEnd();
					num4 += num3;
				}
				if (num4 > 0f)
				{
					result = true;
				}
			}
			if (!MirrorVersionConfig.IsMirrorVersionOpen && viewBounds.min.x - extraFillSize.x < contentBounds.min.x)
			{
				float num5 = NewItemAtStart();
				float num6 = num5;
				while (num5 > 0f && viewBounds.min.x - extraFillSize.x < contentBounds.min.x - num6)
				{
					num5 = NewItemAtStart();
					num6 += num5;
				}
				if (num6 > 0f)
				{
					result = true;
				}
			}
			if (MirrorVersionConfig.IsMirrorVersionOpen && viewBounds.max.x + extraFillSize.x > contentBounds.max.x)
			{
				float num7 = NewItemAtStart();
				float num8 = num7;
				while (num7 > 0f && viewBounds.max.x + extraFillSize.x > contentBounds.max.x + num8)
				{
					num7 = NewItemAtStart();
					num8 += num7;
				}
				if (num8 > 0f)
				{
					result = true;
				}
			}
			if (!MirrorVersionConfig.IsMirrorVersionOpen && viewBounds.max.x + extraFillSize.x < contentBounds.max.x - threshold)
			{
				float num9 = DeleteItemAtEnd();
				float num10 = num9;
				while (num9 > 0f && viewBounds.max.x + extraFillSize.x < contentBounds.max.x - threshold - num10)
				{
					num9 = DeleteItemAtEnd();
					num10 += num9;
				}
				if (num10 > 0f)
				{
					result = true;
				}
			}
			if (MirrorVersionConfig.IsMirrorVersionOpen && viewBounds.min.x - extraFillSize.x > contentBounds.min.x + threshold)
			{
				float num11 = DeleteItemAtEnd();
				float num12 = num11;
				while (num11 > 0f && viewBounds.min.x - extraFillSize.x > contentBounds.min.x + threshold + num12)
				{
					num11 = DeleteItemAtEnd();
					num12 += num11;
				}
				if (num12 > 0f)
				{
					result = true;
				}
			}
			if (!MirrorVersionConfig.IsMirrorVersionOpen && viewBounds.min.x - extraFillSize.x > contentBounds.min.x + threshold)
			{
				float num13 = DeleteItemAtStart();
				float num14 = num13;
				while (num13 > 0f && viewBounds.min.x - extraFillSize.x > contentBounds.min.x + threshold + num14)
				{
					num13 = DeleteItemAtStart();
					num14 += num13;
				}
				if (num14 > 0f)
				{
					result = true;
				}
			}
			if (MirrorVersionConfig.IsMirrorVersionOpen && viewBounds.max.x + extraFillSize.x < contentBounds.max.x - threshold)
			{
				float num15 = DeleteItemAtStart();
				float num16 = num15;
				while (num15 > 0f && viewBounds.max.x + extraFillSize.x < contentBounds.max.x - threshold - num16)
				{
					num15 = DeleteItemAtStart();
					num16 += num15;
				}
				if (num16 > 0f)
				{
					result = true;
				}
			}
		}
		else
		{
			if (viewBounds.min.y - extraFillSize.y < contentBounds.min.y)
			{
				float num17 = NewItemAtEnd();
				float num18 = num17;
				while (num17 > 0f && viewBounds.min.y - extraFillSize.y < contentBounds.min.y - num18)
				{
					num17 = NewItemAtEnd();
					num18 += num17;
				}
				if (num18 > 0f)
				{
					result = true;
				}
			}
			if (viewBounds.max.y + extraFillSize.y > contentBounds.max.y)
			{
				float num19 = NewItemAtStart();
				float num20 = num19;
				while (num19 > 0f && viewBounds.max.y + extraFillSize.y > contentBounds.max.y + num20)
				{
					num19 = NewItemAtStart();
					num20 += num19;
				}
				if (num20 > 0f)
				{
					result = true;
				}
			}
			if (viewBounds.min.y - extraFillSize.y > contentBounds.min.y + threshold)
			{
				float num21 = DeleteItemAtEnd();
				float num22 = num21;
				while (num21 > 0f && viewBounds.min.y - extraFillSize.y > contentBounds.min.y + threshold + num22)
				{
					num21 = DeleteItemAtEnd();
					num22 += num21;
				}
				if (num22 > 0f)
				{
					result = true;
				}
			}
			if (viewBounds.max.y + extraFillSize.y < contentBounds.max.y - threshold)
			{
				float num23 = DeleteItemAtStart();
				float num24 = num23;
				while (num23 > 0f && viewBounds.max.y + extraFillSize.y < contentBounds.max.y - threshold - num24)
				{
					num23 = DeleteItemAtStart();
					num24 += num23;
				}
				if (num24 > 0f)
				{
					result = true;
				}
			}
		}
		return result;
	}

	private new void Awake()
	{
		base.Awake();
		flexibleWidth = -1f;
		directionSign = ((layoutType == ScrollViewLayoutType.Horizontal) ? 1 : (-1));
		GridLayoutGroup component = content.GetComponent<GridLayoutGroup>();
		if (component != null && ((layoutType == ScrollViewLayoutType.Horizontal && component.constraint != GridLayoutGroup.Constraint.FixedRowCount) || (layoutType == ScrollViewLayoutType.Vertical && component.constraint != GridLayoutGroup.Constraint.FixedColumnCount)))
		{
			Debug.LogError("[LoopHorizontalScrollRect] unsupported GridLayoutGroup constraint");
		}
	}

	private new void OnDestroy()
	{
		foreach (ItemObject item in itemObjectPool)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		if (unUseObjectList == null)
		{
			return;
		}
		foreach (GameObject unUseObject in unUseObjectList)
		{
			if (unUseObject != null)
			{
				UnityEngine.Object.Destroy(unUseObject);
			}
		}
	}

	public void ClearCells()
	{
		if (Application.isPlaying)
		{
			itemTypeStart = 0;
			itemTypeEnd = 0;
			totalCount = 0;
			for (int num = content.childCount - 1; num >= 0; num--)
			{
				ReturnItemObject(content.GetChild(num).gameObject);
			}
		}
	}

	public void ScrollToCell(int index, float speed)
	{
		if (totalCount >= 0 && (index < 0 || index >= totalCount))
		{
			Debug.LogWarningFormat("invalid index {0}", index);
		}
		else if (speed <= 0f)
		{
			Debug.LogWarningFormat("invalid speed {0}", speed);
		}
		else
		{
			StopAllCoroutines();
			StartCoroutine(ScrollToCellCoroutine(index, speed));
		}
	}

	public void StopScrollToCell(object obj)
	{
		StopAllCoroutines();
	}

	private IEnumerator ScrollToCellCoroutine(int index, float speed)
	{
		bool needMoving = true;
		while (needMoving)
		{
			yield return null;
			if (m_Dragging)
			{
				continue;
			}
			float num;
			if (index < itemTypeStart)
			{
				num = (0f - Time.deltaTime) * speed;
			}
			else if (index >= itemTypeEnd)
			{
				num = Time.deltaTime * speed;
			}
			else
			{
				m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
				Bounds bounds4Item = GetBounds4Item(index);
				float num2 = 0f;
				if (directionSign == -1)
				{
					num2 = (reverseDirection ? (m_ViewBounds.min.y - bounds4Item.min.y) : (m_ViewBounds.max.y - bounds4Item.max.y));
				}
				else if (directionSign == 1)
				{
					num2 = (reverseDirection ? (bounds4Item.max.x - m_ViewBounds.max.x) : (bounds4Item.min.x - m_ViewBounds.min.x));
				}
				if (totalCount >= 0)
				{
					if (num2 > 0f && itemTypeEnd == totalCount && !reverseDirection)
					{
						bounds4Item = GetBounds4Item(totalCount - 1);
						if ((directionSign == -1 && bounds4Item.min.y > m_ViewBounds.min.y) || (directionSign == 1 && bounds4Item.max.x < m_ViewBounds.max.x))
						{
							break;
						}
					}
					else if (num2 < 0f && itemTypeStart == 0 && reverseDirection)
					{
						bounds4Item = GetBounds4Item(0);
						if ((directionSign == -1 && bounds4Item.max.y < m_ViewBounds.max.y) || (directionSign == 1 && bounds4Item.min.x > m_ViewBounds.min.x))
						{
							break;
						}
					}
				}
				float num3 = Time.deltaTime * speed;
				if (Mathf.Abs(num2) < num3)
				{
					needMoving = false;
					num = num2;
				}
				else
				{
					num = Mathf.Sign(num2) * num3;
				}
			}
			if (num != 0f)
			{
				int num4 = ((!MirrorVersionConfig.IsMirrorVersionOpen || layoutType != ScrollViewLayoutType.Horizontal) ? 1 : (-1));
				Vector2 vector = GetVector(num * (float)num4);
				content.anchoredPosition += vector;
				m_PrevPosition += vector;
				m_ContentStartPosition += vector;
			}
		}
		StopMovement();
		UpdatePrevData();
		onScrollToCellEndCallBack?.Invoke(null);
	}

	public void RefreshCells()
	{
		if (!Application.isPlaying || !base.isActiveAndEnabled)
		{
			return;
		}
		itemTypeEnd = itemTypeStart;
		for (int i = 0; i < content.childCount; i++)
		{
			Transform child = content.GetChild(i);
			if (itemTypeEnd < totalCount)
			{
				OnItemMoveOut(child.gameObject, itemTypeEnd);
				OnItemMoveIn(child.gameObject, itemTypeEnd);
				itemTypeEnd++;
			}
			else
			{
				OnItemMoveOut(child.gameObject, itemTypeStart + i);
				ReturnItemObject(child.gameObject);
				i--;
			}
		}
	}

	public void RefillCellsFromEnd(int offset = 0)
	{
		if (!Application.isPlaying || itemTemplate == null)
		{
			return;
		}
		StopMovement();
		for (int num = m_Content.childCount - 1; num >= 0; num--)
		{
			GameObject gameObject = m_Content.GetChild(num).gameObject;
			OnItemMoveOut(gameObject, --itemTypeEnd);
			ReturnItemObject(gameObject);
		}
		itemTypeEnd = (reverseDirection ? offset : (totalCount - offset));
		itemTypeStart = itemTypeEnd;
		if (totalCount >= 0 && itemTypeStart % contentConstraintCount != 0)
		{
			Debug.LogWarning("Grid will become strange since we can't fill items in the last line");
		}
		float num2 = 0f;
		float num3 = 0f;
		float num4;
		for (num2 = ((directionSign != -1) ? viewRect.rect.size.x : viewRect.rect.size.y); num2 > num3; num3 += num4)
		{
			num4 = (reverseDirection ? NewItemAtEnd() : NewItemAtStart());
			if (num4 <= 0f)
			{
				break;
			}
		}
		Vector2 anchoredPosition = m_Content.anchoredPosition;
		float num5 = Mathf.Max(0f, num3 - num2);
		if (reverseDirection)
		{
			num5 = 0f - num5;
		}
		if (directionSign == -1)
		{
			anchoredPosition.y = num5;
		}
		else if (directionSign == 1)
		{
			anchoredPosition.x = 0f - num5;
		}
		m_Content.anchoredPosition = anchoredPosition;
	}

	public void SetExtraFillSize(float extraSizeX, float extraSizeY)
	{
		extraFillSize = new Vector2(extraSizeX, extraSizeY);
	}

	public void RefillCells(int offset = 0, bool fillViewRect = false)
	{
		if (!Application.isPlaying || itemTemplate == null)
		{
			return;
		}
		if (totalCount == 0)
		{
			offset = 0;
		}
		else if (offset < 0 || offset >= totalCount)
		{
			if (string.IsNullOrEmpty(_name))
			{
				_name = base.transform.GetFullPath();
			}
			Debug.LogErrorFormat("offset {0} out of range: [{1}, {2}]. GameObject:{3}", offset, 0, totalCount - 1, _name);
			offset = 0;
		}
		StopMovement();
		for (int num = m_Content.childCount - 1; num >= 0; num--)
		{
			GameObject gameObject = m_Content.GetChild(num).gameObject;
			OnItemMoveOut(gameObject, --itemTypeEnd);
			ReturnItemObject(gameObject);
		}
		itemTypeStart = (reverseDirection ? (totalCount - offset) : offset);
		itemTypeEnd = itemTypeStart;
		if (totalCount >= 0 && itemTypeStart % contentConstraintCount != 0)
		{
			Debug.LogWarning("Grid will become strange since we can't fill items in the first line");
		}
		float num2 = 0f;
		float num3 = 0f;
		num2 = ((directionSign != -1) ? (viewRect.rect.size.x + extraFillSize.x) : (viewRect.rect.size.y + extraFillSize.y));
		float num4 = 0f;
		float num5;
		for (; num2 > num3; num3 += num5)
		{
			num5 = (reverseDirection ? NewItemAtStart() : NewItemAtEnd());
			if (num5 <= 0f)
			{
				break;
			}
			num4 = num5;
		}
		float num6 = 0f;
		if (extraFillSize.x > 0f || extraFillSize.y > 0f)
		{
			float num7;
			for (num2 = ((directionSign != -1) ? (num2 + extraFillSize.x) : (num2 + extraFillSize.y)); num2 > num3; num3 += num7)
			{
				num7 = (reverseDirection ? NewItemAtEnd() : NewItemAtStart());
				if (num7 <= 0f)
				{
					break;
				}
				num4 = num7;
				num6 -= num7;
			}
		}
		if (fillViewRect && num4 > 0f && num3 < num2)
		{
			int num8 = (int)((num2 - num3) / num4);
			int num9 = offset - num8;
			if (num9 < 0)
			{
				num9 = 0;
			}
			if (num9 != offset)
			{
				RefillCells(num9);
			}
		}
		Vector2 anchoredPosition = m_Content.anchoredPosition;
		if (directionSign == -1)
		{
			anchoredPosition.y = num6;
		}
		else if (directionSign == 1)
		{
			anchoredPosition.x = num6;
		}
		m_Content.anchoredPosition = anchoredPosition;
	}

	protected float NewItemAtStart()
	{
		if (totalCount >= 0 && itemTypeStart - contentConstraintCount < 0)
		{
			return 0f;
		}
		float num = 0f;
		for (int i = 0; i < contentConstraintCount; i++)
		{
			itemTypeStart--;
			RectTransform rectTransform = InstantiateNextItem(itemTypeStart);
			rectTransform.SetAsFirstSibling();
			num = Mathf.Max(GetSize(rectTransform), num);
			OnItemMoveIn(rectTransform.gameObject, itemTypeStart);
		}
		threshold = Mathf.Max(threshold, num * 1.5f);
		if (!reverseDirection)
		{
			int num2 = ((!MirrorVersionConfig.IsMirrorVersionOpen || layoutType != ScrollViewLayoutType.Horizontal) ? 1 : (-1));
			Vector2 vector = GetVector(num);
			content.anchoredPosition += vector * num2;
			m_PrevPosition += vector * num2;
			m_ContentStartPosition += vector * num2;
		}
		return num;
	}

	protected float DeleteItemAtStart()
	{
		if (((m_Dragging || m_Velocity != Vector2.zero) && totalCount >= 0 && itemTypeEnd >= totalCount - 1) || content.childCount == 0)
		{
			return 0f;
		}
		float num = 0f;
		for (int i = 0; i < contentConstraintCount; i++)
		{
			RectTransform rectTransform = content.GetChild(0) as RectTransform;
			num = Mathf.Max(GetSize(rectTransform), num);
			OnItemMoveOut(rectTransform.gameObject, itemTypeStart);
			ReturnItemObject(rectTransform.gameObject);
			itemTypeStart++;
			if (content.childCount == 0)
			{
				break;
			}
		}
		if (!reverseDirection)
		{
			int num2 = ((!MirrorVersionConfig.IsMirrorVersionOpen || layoutType != ScrollViewLayoutType.Horizontal) ? 1 : (-1));
			Vector2 vector = GetVector(num);
			content.anchoredPosition -= vector * num2;
			m_PrevPosition -= vector * num2;
			m_ContentStartPosition -= vector * num2;
		}
		return num;
	}

	protected float NewItemAtEnd()
	{
		if (totalCount >= 0 && itemTypeEnd >= totalCount)
		{
			return 0f;
		}
		float num = 0f;
		int num2 = contentConstraintCount - content.childCount % contentConstraintCount;
		for (int i = 0; i < num2; i++)
		{
			RectTransform rectTransform = InstantiateNextItem(itemTypeEnd);
			num = Mathf.Max(GetSize(rectTransform), num);
			OnItemMoveIn(rectTransform.gameObject, itemTypeEnd);
			itemTypeEnd++;
			if (totalCount >= 0 && itemTypeEnd >= totalCount)
			{
				break;
			}
		}
		threshold = Mathf.Max(threshold, num * 1.5f);
		if (reverseDirection)
		{
			Vector2 vector = GetVector(num);
			content.anchoredPosition -= vector;
			m_PrevPosition -= vector;
			m_ContentStartPosition -= vector;
		}
		return num;
	}

	protected float DeleteItemAtEnd()
	{
		if (((m_Dragging || m_Velocity != Vector2.zero) && totalCount >= 0 && itemTypeStart < contentConstraintCount) || content.childCount == 0)
		{
			return 0f;
		}
		float num = 0f;
		for (int i = 0; i < contentConstraintCount; i++)
		{
			RectTransform rectTransform = content.GetChild(content.childCount - 1) as RectTransform;
			num = Mathf.Max(GetSize(rectTransform), num);
			OnItemMoveOut(rectTransform.gameObject, --itemTypeEnd);
			ReturnItemObject(rectTransform.gameObject);
			if (itemTypeEnd % contentConstraintCount == 0 || content.childCount == 0)
			{
				break;
			}
		}
		if (reverseDirection)
		{
			Vector2 vector = GetVector(num);
			content.anchoredPosition += vector;
			m_PrevPosition += vector;
			m_ContentStartPosition += vector;
		}
		return num;
	}

	private RectTransform InstantiateNextItem(int itemIdx)
	{
		RectTransform obj = GetItemObject().transform as RectTransform;
		obj.transform.SetParent(content, worldPositionStays: false);
		obj.gameObject.SetActive(value: true);
		return obj;
	}

	public virtual void Rebuild(CanvasUpdate executing)
	{
		if (executing == CanvasUpdate.Prelayout)
		{
			UpdateCachedData();
		}
		if (executing == CanvasUpdate.PostLayout)
		{
			UpdateBounds();
			UpdateScrollbars(Vector2.zero);
			UpdatePrevData();
			m_HasRebuiltLayout = true;
		}
	}

	public virtual void LayoutComplete()
	{
	}

	public virtual void GraphicUpdateComplete()
	{
	}

	private void UpdateCachedData()
	{
		Transform transform = base.transform;
		m_HorizontalScrollbarRect = ((m_HorizontalScrollbar == null) ? null : (m_HorizontalScrollbar.transform as RectTransform));
		m_VerticalScrollbarRect = ((m_VerticalScrollbar == null) ? null : (m_VerticalScrollbar.transform as RectTransform));
		bool num = viewRect.parent == transform;
		bool flag = !m_HorizontalScrollbarRect || m_HorizontalScrollbarRect.parent == transform;
		bool flag2 = !m_VerticalScrollbarRect || m_VerticalScrollbarRect.parent == transform;
		bool flag3 = num && flag && flag2;
		m_HSliderExpand = flag3 && (bool)m_HorizontalScrollbarRect && horizontalScrollbarVisibility == ScrollbarVisibility.AutoHideAndExpandViewport;
		m_VSliderExpand = flag3 && (bool)m_VerticalScrollbarRect && verticalScrollbarVisibility == ScrollbarVisibility.AutoHideAndExpandViewport;
		m_HSliderHeight = ((m_HorizontalScrollbarRect == null) ? 0f : m_HorizontalScrollbarRect.rect.height);
		m_VSliderWidth = ((m_VerticalScrollbarRect == null) ? 0f : m_VerticalScrollbarRect.rect.width);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		GameEntry.Event.Subscribe(EventId.StopSvAutoToCell, StopScrollToCell);
		if ((bool)m_HorizontalScrollbar)
		{
			m_HorizontalScrollbar.onValueChanged.AddListener(SetHorizontalNormalizedPosition);
		}
		if ((bool)m_VerticalScrollbar)
		{
			m_VerticalScrollbar.onValueChanged.AddListener(SetVerticalNormalizedPosition);
		}
		CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
	}

	protected override void OnDisable()
	{
		CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
		GameEntry.Event.Unsubscribe(EventId.StopSvAutoToCell, StopScrollToCell);
		if ((bool)m_HorizontalScrollbar)
		{
			m_HorizontalScrollbar.onValueChanged.RemoveListener(SetHorizontalNormalizedPosition);
		}
		if ((bool)m_VerticalScrollbar)
		{
			m_VerticalScrollbar.onValueChanged.RemoveListener(SetVerticalNormalizedPosition);
		}
		m_HasRebuiltLayout = false;
		m_Tracker.Clear();
		m_Velocity = Vector2.zero;
		LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		base.OnDisable();
	}

	public override bool IsActive()
	{
		if (base.IsActive())
		{
			return m_Content != null;
		}
		return false;
	}

	private void EnsureLayoutHasRebuilt()
	{
		if (!m_HasRebuiltLayout && !CanvasUpdateRegistry.IsRebuildingLayout())
		{
			Canvas.ForceUpdateCanvases();
		}
	}

	public virtual void StopMovement()
	{
		m_Velocity = Vector2.zero;
	}

	public virtual void OnScroll(PointerEventData data)
	{
		if (!IsActive())
		{
			return;
		}
		EnsureLayoutHasRebuilt();
		UpdateBounds();
		Vector2 scrollDelta = data.scrollDelta;
		scrollDelta.y *= -1f;
		if (layoutType == ScrollViewLayoutType.Vertical)
		{
			if (Mathf.Abs(scrollDelta.x) > Mathf.Abs(scrollDelta.y))
			{
				scrollDelta.y = scrollDelta.x;
			}
			scrollDelta.x = 0f;
		}
		if (layoutType == ScrollViewLayoutType.Horizontal)
		{
			if (Mathf.Abs(scrollDelta.y) > Mathf.Abs(scrollDelta.x))
			{
				scrollDelta.x = scrollDelta.y;
			}
			scrollDelta.y = 0f;
		}
		Vector2 anchoredPosition = m_Content.anchoredPosition;
		anchoredPosition += scrollDelta * m_ScrollSensitivity;
		if (m_MovementType == MovementType.Clamped)
		{
			anchoredPosition += CalculateOffset(anchoredPosition - m_Content.anchoredPosition);
		}
		SetContentAnchoredPosition(anchoredPosition);
		UpdateBounds();
	}

	public virtual void OnInitializePotentialDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			m_Velocity = Vector2.zero;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		StopAllCoroutines();
	}

	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && IsActive())
		{
			StopScrollToCell(null);
			UpdateBounds();
			m_PointerStartLocalCursor = Vector2.zero;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out m_PointerStartLocalCursor);
			m_ContentStartPosition = m_Content.anchoredPosition;
			m_Dragging = true;
			onBeginDragCallBack?.Invoke(eventData);
		}
	}

	public virtual void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			m_Dragging = false;
			onEndDragCallBack?.Invoke(eventData);
		}
	}

	public virtual void OnDrag(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left || !IsActive() || !RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out var localPoint))
		{
			return;
		}
		UpdateBounds();
		Vector2 vector = localPoint - m_PointerStartLocalCursor;
		Vector2 vector2 = m_ContentStartPosition + vector;
		Vector2 vector3 = CalculateOffset(vector2 - m_Content.anchoredPosition);
		vector2 += vector3;
		if (m_MovementType == MovementType.Elastic)
		{
			if (vector3.x != 0f)
			{
				vector2.x -= RubberDelta(vector3.x, m_ViewBounds.size.x) * rubberScale;
			}
			if (vector3.y != 0f)
			{
				vector2.y -= RubberDelta(vector3.y, m_ViewBounds.size.y) * rubberScale;
			}
		}
		SetContentAnchoredPosition(vector2);
		onDragCallBack?.Invoke(eventData);
	}

	protected virtual void SetContentAnchoredPosition(Vector2 position)
	{
		if (layoutType == ScrollViewLayoutType.Vertical)
		{
			position.x = m_Content.anchoredPosition.x;
		}
		if (layoutType == ScrollViewLayoutType.Horizontal)
		{
			position.y = m_Content.anchoredPosition.y;
		}
		if (position != m_Content.anchoredPosition)
		{
			m_Content.anchoredPosition = position;
			UpdateBounds(updateItems: true);
		}
	}

	protected virtual void LateUpdate()
	{
		if (!m_Content)
		{
			return;
		}
		EnsureLayoutHasRebuilt();
		UpdateScrollbarVisibility();
		UpdateBounds(updateItems: false, checkBounds: true);
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		Vector2 vector = CalculateOffset(Vector2.zero);
		if (!m_Dragging && (vector != Vector2.zero || m_Velocity != Vector2.zero))
		{
			Vector2 anchoredPosition = m_Content.anchoredPosition;
			for (int i = 0; i < 2; i++)
			{
				if (m_MovementType == MovementType.Elastic && vector[i] != 0f)
				{
					float currentVelocity = m_Velocity[i];
					anchoredPosition[i] = Mathf.SmoothDamp(m_Content.anchoredPosition[i], m_Content.anchoredPosition[i] + vector[i], ref currentVelocity, m_Elasticity, float.PositiveInfinity, unscaledDeltaTime);
					m_Velocity[i] = currentVelocity;
				}
				else if (m_Inertia)
				{
					m_Velocity[i] *= Mathf.Pow(m_DecelerationRate, unscaledDeltaTime);
					if (Mathf.Abs(m_Velocity[i]) < 1f)
					{
						m_Velocity[i] = 0f;
					}
					anchoredPosition[i] += m_Velocity[i] * unscaledDeltaTime;
				}
				else
				{
					m_Velocity[i] = 0f;
				}
			}
			if (m_Velocity != Vector2.zero)
			{
				if (m_MovementType == MovementType.Clamped)
				{
					vector = CalculateOffset(anchoredPosition - m_Content.anchoredPosition);
					anchoredPosition += vector;
				}
				SetContentAnchoredPosition(anchoredPosition);
			}
		}
		if (m_Dragging && m_Inertia)
		{
			Vector3 b = (m_Content.anchoredPosition - m_PrevPosition) / unscaledDeltaTime;
			m_Velocity = Vector3.Lerp(m_Velocity, b, unscaledDeltaTime * 10f);
		}
		if (m_ViewBounds != m_PrevViewBounds || m_ContentBounds != m_PrevContentBounds || m_Content.anchoredPosition != m_PrevPosition)
		{
			UpdateScrollbars(vector);
			m_OnValueChanged.Invoke(normalizedPosition);
			UpdatePrevData();
		}
	}

	private void UpdatePrevData()
	{
		if (m_Content == null)
		{
			m_PrevPosition = Vector2.zero;
		}
		else
		{
			m_PrevPosition = m_Content.anchoredPosition;
		}
		m_PrevViewBounds = m_ViewBounds;
		m_PrevContentBounds = m_ContentBounds;
	}

	private void UpdateScrollbars(Vector2 offset)
	{
		if ((bool)m_HorizontalScrollbar)
		{
			if (m_ContentBounds.size.x > 0f && totalCount > 0)
			{
				m_HorizontalScrollbar.size = Mathf.Clamp01((m_ViewBounds.size.x - Mathf.Abs(offset.x)) / m_ContentBounds.size.x * (float)CurrentLines / (float)TotalLines);
			}
			else
			{
				m_HorizontalScrollbar.size = 1f;
			}
			m_HorizontalScrollbar.value = horizontalNormalizedPosition;
		}
		if ((bool)m_VerticalScrollbar)
		{
			if (m_ContentBounds.size.y > 0f && totalCount > 0)
			{
				m_VerticalScrollbar.size = Mathf.Clamp01((m_ViewBounds.size.y - Mathf.Abs(offset.y)) / m_ContentBounds.size.y * (float)CurrentLines / (float)TotalLines);
			}
			else
			{
				m_VerticalScrollbar.size = 1f;
			}
			m_VerticalScrollbar.value = verticalNormalizedPosition;
		}
	}

	private void SetHorizontalNormalizedPosition(float value)
	{
		SetNormalizedPosition(value, 0);
	}

	private void SetVerticalNormalizedPosition(float value)
	{
		SetNormalizedPosition(value, 1);
	}

	private void SetNormalizedPosition(float value, int axis)
	{
		if (totalCount > 0 && itemTypeEnd > itemTypeStart)
		{
			EnsureLayoutHasRebuilt();
			UpdateBounds();
			Vector3 localPosition = m_Content.localPosition;
			float num = localPosition[axis];
			switch (axis)
			{
			case 0:
			{
				float num5 = m_ContentBounds.size.x / (float)CurrentLines;
				float num6 = num5 * (float)TotalLines;
				float num7 = m_ContentBounds.min.x - num5 * (float)StartLine;
				num += m_ViewBounds.min.x - value * (num6 - m_ViewBounds.size[axis]) - num7;
				break;
			}
			case 1:
			{
				float num2 = m_ContentBounds.size.y / (float)CurrentLines;
				float num3 = num2 * (float)TotalLines;
				float num4 = m_ContentBounds.max.y + num2 * (float)StartLine;
				num -= num4 - value * (num3 - m_ViewBounds.size.y) - m_ViewBounds.max.y;
				break;
			}
			}
			if (Mathf.Abs(localPosition[axis] - num) > 0.01f)
			{
				localPosition[axis] = num;
				m_Content.localPosition = localPosition;
				m_Velocity[axis] = 0f;
				UpdateBounds(updateItems: true);
			}
		}
	}

	private static float RubberDelta(float overStretching, float viewSize)
	{
		return (1f - 1f / (Mathf.Abs(overStretching) * 0.55f / viewSize + 1f)) * viewSize * Mathf.Sign(overStretching);
	}

	protected override void OnRectTransformDimensionsChange()
	{
		SetDirty();
	}

	public virtual void CalculateLayoutInputHorizontal()
	{
	}

	public virtual void CalculateLayoutInputVertical()
	{
	}

	public virtual void SetLayoutHorizontal()
	{
		m_Tracker.Clear();
		if (m_HSliderExpand || m_VSliderExpand)
		{
			m_Tracker.Add(this, viewRect, DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.SizeDelta);
			viewRect.anchorMin = Vector2.zero;
			viewRect.anchorMax = Vector2.one;
			viewRect.sizeDelta = Vector2.zero;
			viewRect.anchoredPosition = Vector2.zero;
			LayoutRebuilder.ForceRebuildLayoutImmediate(content);
			m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
			m_ContentBounds = GetBounds();
		}
		if (m_VSliderExpand && vScrollingNeeded)
		{
			viewRect.sizeDelta = new Vector2(0f - (m_VSliderWidth + m_VerticalScrollbarSpacing), viewRect.sizeDelta.y);
			LayoutRebuilder.ForceRebuildLayoutImmediate(content);
			m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
			m_ContentBounds = GetBounds();
		}
		if (m_HSliderExpand && hScrollingNeeded)
		{
			viewRect.sizeDelta = new Vector2(viewRect.sizeDelta.x, 0f - (m_HSliderHeight + m_HorizontalScrollbarSpacing));
			m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
			m_ContentBounds = GetBounds();
		}
		if (m_VSliderExpand && vScrollingNeeded && viewRect.sizeDelta.x == 0f && viewRect.sizeDelta.y < 0f)
		{
			viewRect.sizeDelta = new Vector2(0f - (m_VSliderWidth + m_VerticalScrollbarSpacing), viewRect.sizeDelta.y);
		}
	}

	public virtual void SetLayoutVertical()
	{
		UpdateScrollbarLayout();
		m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
		m_ContentBounds = GetBounds();
	}

	private void UpdateScrollbarVisibility()
	{
		if ((bool)m_VerticalScrollbar && m_VerticalScrollbarVisibility != ScrollbarVisibility.Permanent && m_VerticalScrollbar.gameObject.activeSelf != vScrollingNeeded)
		{
			m_VerticalScrollbar.gameObject.SetActive(vScrollingNeeded);
		}
		if ((bool)m_HorizontalScrollbar && m_HorizontalScrollbarVisibility != ScrollbarVisibility.Permanent && m_HorizontalScrollbar.gameObject.activeSelf != hScrollingNeeded)
		{
			m_HorizontalScrollbar.gameObject.SetActive(hScrollingNeeded);
		}
	}

	private void UpdateScrollbarLayout()
	{
		if (m_VSliderExpand && (bool)m_HorizontalScrollbar)
		{
			m_Tracker.Add(this, m_HorizontalScrollbarRect, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.SizeDeltaX);
			m_HorizontalScrollbarRect.anchorMin = new Vector2(0f, m_HorizontalScrollbarRect.anchorMin.y);
			m_HorizontalScrollbarRect.anchorMax = new Vector2(1f, m_HorizontalScrollbarRect.anchorMax.y);
			m_HorizontalScrollbarRect.anchoredPosition = new Vector2(0f, m_HorizontalScrollbarRect.anchoredPosition.y);
			if (vScrollingNeeded)
			{
				m_HorizontalScrollbarRect.sizeDelta = new Vector2(0f - (m_VSliderWidth + m_VerticalScrollbarSpacing), m_HorizontalScrollbarRect.sizeDelta.y);
			}
			else
			{
				m_HorizontalScrollbarRect.sizeDelta = new Vector2(0f, m_HorizontalScrollbarRect.sizeDelta.y);
			}
		}
		if (m_HSliderExpand && (bool)m_VerticalScrollbar)
		{
			m_Tracker.Add(this, m_VerticalScrollbarRect, DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaY);
			m_VerticalScrollbarRect.anchorMin = new Vector2(m_VerticalScrollbarRect.anchorMin.x, 0f);
			m_VerticalScrollbarRect.anchorMax = new Vector2(m_VerticalScrollbarRect.anchorMax.x, 1f);
			m_VerticalScrollbarRect.anchoredPosition = new Vector2(m_VerticalScrollbarRect.anchoredPosition.x, 0f);
			if (hScrollingNeeded)
			{
				m_VerticalScrollbarRect.sizeDelta = new Vector2(m_VerticalScrollbarRect.sizeDelta.x, 0f - (m_HSliderHeight + m_HorizontalScrollbarSpacing));
			}
			else
			{
				m_VerticalScrollbarRect.sizeDelta = new Vector2(m_VerticalScrollbarRect.sizeDelta.x, 0f);
			}
		}
	}

	private void UpdateBounds(bool updateItems = false, bool checkBounds = false)
	{
		m_ViewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
		Bounds contentBounds = m_ContentBounds;
		m_ContentBounds = GetBounds();
		_ = contentBounds != m_ContentBounds;
		if (!(m_Content == null))
		{
			if (Application.isPlaying && updateItems && UpdateItems(m_ViewBounds, m_ContentBounds))
			{
				Canvas.ForceUpdateCanvases();
				m_ContentBounds = GetBounds();
			}
			Vector3 size = m_ContentBounds.size;
			Vector3 center = m_ContentBounds.center;
			Vector3 vector = m_ViewBounds.size - size;
			if (vector.x > 0f)
			{
				center.x -= vector.x * (m_Content.pivot.x - 0.5f);
				size.x = m_ViewBounds.size.x;
			}
			if (vector.y > 0f)
			{
				center.y -= vector.y * (m_Content.pivot.y - 0.5f);
				size.y = m_ViewBounds.size.y;
			}
			m_ContentBounds.size = size;
			m_ContentBounds.center = center;
		}
	}

	private Bounds GetBounds()
	{
		if (m_Content == null)
		{
			return default(Bounds);
		}
		Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		Matrix4x4 worldToLocalMatrix = viewRect.worldToLocalMatrix;
		m_Content.GetWorldCorners(m_Corners);
		for (int i = 0; i < 4; i++)
		{
			Vector3 lhs = worldToLocalMatrix.MultiplyPoint3x4(m_Corners[i]);
			vector = Vector3.Min(lhs, vector);
			vector2 = Vector3.Max(lhs, vector2);
		}
		Bounds result = new Bounds(vector, Vector3.zero);
		result.Encapsulate(vector2);
		return result;
	}

	private Bounds GetBounds4Item(int index)
	{
		if (m_Content == null)
		{
			return default(Bounds);
		}
		Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		Matrix4x4 worldToLocalMatrix = viewRect.worldToLocalMatrix;
		int num = index - itemTypeStart;
		if (num < 0 || num >= m_Content.childCount)
		{
			return default(Bounds);
		}
		RectTransform rectTransform = m_Content.GetChild(num) as RectTransform;
		if (rectTransform == null)
		{
			return default(Bounds);
		}
		rectTransform.GetWorldCorners(m_Corners);
		for (int i = 0; i < 4; i++)
		{
			Vector3 lhs = worldToLocalMatrix.MultiplyPoint3x4(m_Corners[i]);
			vector = Vector3.Min(lhs, vector);
			vector2 = Vector3.Max(lhs, vector2);
		}
		Bounds result = new Bounds(vector, Vector3.zero);
		result.Encapsulate(vector2);
		return result;
	}

	private Vector2 CalculateOffset(Vector2 delta)
	{
		Vector2 zero = Vector2.zero;
		if (m_MovementType == MovementType.Unrestricted)
		{
			return zero;
		}
		if (m_MovementType == MovementType.Clamped)
		{
			if (totalCount < 0)
			{
				return zero;
			}
			if (GetDimension(delta) < 0f && itemTypeStart > 0)
			{
				return zero;
			}
			if (GetDimension(delta) > 0f && itemTypeEnd < totalCount)
			{
				return zero;
			}
		}
		Vector2 vector = m_ContentBounds.min;
		Vector2 vector2 = m_ContentBounds.max;
		if (layoutType == ScrollViewLayoutType.Horizontal)
		{
			vector.x += delta.x;
			vector2.x += delta.x;
			if (vector.x > m_ViewBounds.min.x)
			{
				zero.x = m_ViewBounds.min.x - vector.x;
			}
			else if (vector2.x < m_ViewBounds.max.x)
			{
				zero.x = m_ViewBounds.max.x - vector2.x;
			}
		}
		else if (layoutType == ScrollViewLayoutType.Vertical)
		{
			vector.y += delta.y;
			vector2.y += delta.y;
			if (vector2.y < m_ViewBounds.max.y)
			{
				zero.y = m_ViewBounds.max.y - vector2.y;
			}
			else if (vector.y > m_ViewBounds.min.y)
			{
				zero.y = m_ViewBounds.min.y - vector.y;
			}
		}
		return zero;
	}

	protected void SetDirty()
	{
		if (IsActive())
		{
			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}
	}

	protected void SetDirtyCaching()
	{
		if (IsActive())
		{
			CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}
	}

	private GameObject GetItemObject()
	{
		ItemObject itemObject = itemObjectPool.Find((ItemObject i) => !i.used);
		if (itemObject != null)
		{
			itemObject.used = true;
			return itemObject.gameObject;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(itemTemplate);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.anchoredPosition3D = Vector3.zero;
		component.localScale = Vector3.one;
		itemObject = new ItemObject();
		itemObject.gameObject = gameObject;
		itemObject.used = true;
		if (MirrorVersionConfig.IsMirrorVersionOpen && itemTemplate?.scene.name == null)
		{
			ArabicMirror.MirrorEntry(isInnerMirror: true, isProcessRootAnchorAndPivot: true, gameObject);
		}
		itemObjectPool.Add(itemObject);
		return gameObject;
	}

	private void ReturnItemObject(GameObject go)
	{
		go.SetActive(value: false);
		go.transform.SetParent(ObjectPoolRoot, worldPositionStays: false);
		ItemObject itemObject = itemObjectPool.Find((ItemObject i) => i.gameObject == go);
		if (itemObject != null)
		{
			itemObject.used = false;
		}
		else if (unUseObjectList == null)
		{
			unUseObjectList = new List<GameObject>();
			unUseObjectList.Add(go);
		}
		else if (!unUseObjectList.Exists((GameObject t) => t == go))
		{
			unUseObjectList.Add(go);
		}
	}

	private void OnItemMoveIn(GameObject itemObj, int index)
	{
		onItemMoveIn?.Invoke(itemObj, index);
	}

	private void OnItemMoveOut(GameObject itemObj, int index)
	{
		onItemMoveOut?.Invoke(itemObj, index);
	}

	[SpecialName]
	Transform ICanvasElement.get_transform()
	{
		return base.transform;
	}
}
