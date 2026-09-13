using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiScrollEvent : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler, IPointerClickHandler
{
	public PageView PageView;

	private ScrollRect scrollRect;

	[SerializeField]
	private GameObject rootScroller;

	[SerializeField]
	private ScrollRect rootScrollRect;

	public MultiType RootScrollType;

	public List<GameObject> ChildScrolls;

	public bool isHorizontal;

	public bool isVertical;

	public readonly List<KeyValuePair<GameObject, IPointerClickHandler>> pointerClickHandlers = new List<KeyValuePair<GameObject, IPointerClickHandler>>();

	[SerializeField]
	private GameObject ScrollBaffle;

	private ScrollRect currentChildScroll;

	private GameObject currentChildScrollGo;

	private Vector2 navigation;

	private Vector2 initializePos;

	[SerializeField]
	private float AnglewithUpAxis = 50f;

	private bool dragging;

	private readonly List<GameObject> ClickGameObjectList = new List<GameObject>();

	public Action OnCloseBaffle;

	public bool CanAutoClickDown;

	public GameObject RootScroller
	{
		get
		{
			return rootScroller;
		}
		set
		{
			rootScroller = value;
			if (rootScroller != null)
			{
				rootScrollRect = rootScroller.GetComponent<ScrollRect>();
			}
		}
	}

	private void ClearClickList()
	{
		ClickGameObjectList.Clear();
	}

	public void AddClickList(GameObject go)
	{
		if (go != null && !ClickGameObjectList.Contains(go))
		{
			ClickGameObjectList.Add(go);
		}
	}

	public void Awake()
	{
		base.gameObject.name = "MultiScrollEvent";
	}

	public MultiScrollEvent SetRootScroller(GameObject gameObject)
	{
		RootScroller = gameObject;
		if (gameObject != null)
		{
			ScrollRect component = gameObject.GetComponent<ScrollRect>();
			if (component != null)
			{
				rootScrollRect = component;
				rootScrollRect.enabled = false;
				rootScrollRect.movementType = ScrollRect.MovementType.Clamped;
			}
		}
		if (PageView != null)
		{
			PageView.SetScrollRect(rootScrollRect).SetContentWidth().SetAllPagePosition();
			PageView.PageTo(0);
			PageView pageView = PageView;
			pageView.OnPageMoveDone = (Action<GameObject>)Delegate.Combine(pageView.OnPageMoveDone, new Action<GameObject>(SetCurrentChildScroll));
		}
		rootScrollRect.enabled = true;
		rootScrollRect.movementType = ScrollRect.MovementType.Elastic;
		return this;
	}

	public MultiScrollEvent InitChildScrill()
	{
		ChildScrolls.Clear();
		if (rootScrollRect != null)
		{
			Transform content = rootScrollRect.content;
			if (content != null)
			{
				for (int i = 0; i < content.childCount; i++)
				{
					GameObject gameObject = content.GetChild(i).gameObject;
					if (gameObject != null)
					{
						ScrollRect componentInChildren = gameObject.GetComponentInChildren<ScrollRect>();
						if (componentInChildren != null && componentInChildren.gameObject.activeInHierarchy)
						{
							ChildScrolls.Add(componentInChildren.gameObject);
						}
					}
				}
			}
		}
		currentChildScrollGo = ChildScrolls[0];
		return this;
	}

	public void Hide()
	{
		if (PageView != null)
		{
			if (PageView.OnPageMoveDone != null)
			{
				PageView pageView = PageView;
				pageView.OnPageMoveDone = (Action<GameObject>)Delegate.Remove(pageView.OnPageMoveDone, new Action<GameObject>(SetCurrentChildScroll));
			}
			PageView.ClearChild();
		}
		if (ChildScrolls != null)
		{
			ChildScrolls.Clear();
		}
	}

	private void OnDestroy()
	{
		if (ChildScrolls != null)
		{
			ChildScrolls.Clear();
		}
	}

	private void ClearCurrentChildScroll()
	{
		currentChildScroll = null;
		currentChildScrollGo = null;
	}

	public void SetCurrentChildScroll(GameObject go)
	{
		if (go != null)
		{
			CheckScrollerInCollection(go);
		}
		else
		{
			currentChildScroll = null;
		}
	}

	private bool CheckScrollerInCollection(GameObject go)
	{
		if (go == null)
		{
			return false;
		}
		for (int i = 0; i < ChildScrolls.Count; i++)
		{
			if (ChildScrolls[i] == go)
			{
				currentChildScrollGo = go;
				return true;
			}
		}
		return false;
	}

	public void SetCurrentChildScroll(ScrollRect scroll)
	{
		CheckScrollerInCollection(scroll);
	}

	private bool CheckScrollerInCollection(ScrollRect scroll)
	{
		if (scroll == null)
		{
			return false;
		}
		for (int i = 0; i < ChildScrolls.Count; i++)
		{
			if (ChildScrolls[i] == scroll.gameObject)
			{
				currentChildScroll = scroll;
				return true;
			}
		}
		return false;
	}

	private Vector2 OnBeginDrag(Vector2 vectors)
	{
		Vector2 normalized = (vectors - initializePos).normalized;
		normalized.x = Mathf.Abs(normalized.x);
		normalized.y = Mathf.Abs(normalized.y);
		if (Vector2.Angle(Vector2.up, normalized) > AnglewithUpAxis)
		{
			isHorizontal = true;
			isVertical = false;
		}
		else
		{
			isHorizontal = false;
			isVertical = true;
		}
		return normalized;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		dragging = true;
		navigation = OnBeginDrag(eventData.position);
		if (RootScrollType == MultiType.Horizontal)
		{
			if (isHorizontal)
			{
				ExecuteEvents.Execute(rootScroller, eventData, ExecuteEvents.beginDragHandler);
			}
			if (isVertical && currentChildScrollGo != null)
			{
				ExecuteEvents.Execute(currentChildScrollGo, eventData, ExecuteEvents.beginDragHandler);
			}
		}
		else if (isVertical)
		{
			ExecuteEvents.Execute(rootScroller, eventData, ExecuteEvents.beginDragHandler);
		}
		else if (currentChildScrollGo != null)
		{
			ExecuteEvents.Execute(currentChildScrollGo, eventData, ExecuteEvents.beginDragHandler);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (RootScrollType == MultiType.Horizontal)
		{
			if (isHorizontal)
			{
				ExecuteEvents.Execute(rootScroller, eventData, ExecuteEvents.dragHandler);
			}
			if (isVertical && currentChildScrollGo != null)
			{
				ExecuteEvents.Execute(currentChildScrollGo, eventData, ExecuteEvents.dragHandler);
			}
		}
		else if (isVertical)
		{
			ExecuteEvents.Execute(rootScroller, eventData, ExecuteEvents.dragHandler);
		}
		else if (currentChildScrollGo != null)
		{
			ExecuteEvents.Execute(currentChildScrollGo, eventData, ExecuteEvents.dragHandler);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (RootScrollType == MultiType.Horizontal)
		{
			if (isHorizontal)
			{
				ExecuteEvents.Execute(rootScroller, eventData, ExecuteEvents.endDragHandler);
			}
			if (isVertical && currentChildScrollGo != null)
			{
				ExecuteEvents.Execute(currentChildScrollGo, eventData, ExecuteEvents.endDragHandler);
			}
		}
		else if (isVertical)
		{
			ExecuteEvents.Execute(rootScroller, eventData, ExecuteEvents.endDragHandler);
		}
		else if (currentChildScrollGo != null)
		{
			ExecuteEvents.Execute(currentChildScrollGo, eventData, ExecuteEvents.endDragHandler);
		}
		dragging = false;
	}

	public void OnInitializePotentialDrag(PointerEventData eventData)
	{
		initializePos = eventData.position;
		isVertical = false;
		isHorizontal = false;
	}

	public void OnScroll(PointerEventData eventData)
	{
		if (rootScroller != null)
		{
			ExecuteEvents.Execute(rootScroller, eventData, ExecuteEvents.scrollHandler);
		}
		for (int i = 0; i < ChildScrolls.Count; i++)
		{
			if (!(ChildScrolls[i] == null))
			{
				ExecuteEvents.Execute(ChildScrolls[i], eventData, ExecuteEvents.scrollHandler);
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (dragging)
		{
			return;
		}
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, list);
		GameObject gameObject = eventData.pointerCurrentRaycast.gameObject;
		for (int i = 0; i < list.Count; i++)
		{
			if (CanClick(list[i].gameObject) && gameObject != list[i].gameObject)
			{
				ExecuteEvents.Execute(list[i].gameObject, eventData, ExecuteEvents.pointerClickHandler);
			}
		}
	}

	private bool CanClick(GameObject go)
	{
		return ClickGameObjectList.Contains(go);
	}

	public void OnClick(GameObject Go)
	{
	}

	public void OnRootvalueChange(Vector2 vector)
	{
	}

	private void OnEnable()
	{
		ClearClickList();
	}

	private void OnDisable()
	{
		ClearClickList();
	}

	public void BaffleMultiScroll(bool _enable)
	{
		if (ScrollBaffle != null)
		{
			ScrollBaffle.SetActive(_enable);
		}
	}

	public void OnClickBaffleUI()
	{
		if (ScrollBaffle != null && ScrollBaffle.activeSelf)
		{
			ScrollBaffle.SetActive(value: false);
			OnCloseBaffle?.Invoke();
			OnCloseBaffle = null;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!CanAutoClickDown)
		{
			return;
		}
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventData, list);
		_ = eventData.pointerCurrentRaycast.gameObject;
		for (int i = 0; i < list.Count; i++)
		{
			if (ChildScrolls.Contains(list[i].gameObject))
			{
				currentChildScrollGo = list[i].gameObject;
				break;
			}
		}
	}
}
