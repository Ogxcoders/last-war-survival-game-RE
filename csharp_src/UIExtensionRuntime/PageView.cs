using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageView : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler, IInitializePotentialDragHandler
{
	public Action OnPageMoveBegin;

	public Action<GameObject> OnPageMoveDone;

	public Action<int> OnPageChanged;

	private ScrollRect rect;

	private float targethorizontal;

	private bool isDrag;

	private List<float> posList = new List<float>();

	private List<Vector2> itemPoslist = new List<Vector2>();

	private CircularArry<Vector2> inputList = new CircularArry<Vector2>();

	private bool stopMove = true;

	public float smooting = 4f;

	private float dragSmooting;

	public float dragSmootingMax = 4f;

	public float sensitivity;

	private float startTime;

	private float startDragHorizontal;

	private float dragTime;

	private float screenValue = (float)Screen.width * 0.02f;

	public readonly List<GameObject> PageChilds = new List<GameObject>();

	private int currentPageIndex = -1;

	[SerializeField]
	private RectTransform content;

	private Canvas canvas;

	private float unitWidth;

	private Vector2 centre;

	private float AnglewithUpAxis = 50f;

	private Vector2 initializePos;

	public bool isHorizontal;

	public bool isVertical;

	public int CurrentPageIndex => currentPageIndex;

	public RectTransform Content
	{
		private get
		{
			return content;
		}
		set
		{
			content = value;
		}
	}

	private void Start()
	{
	}

	public PageView SetScrollRect(ScrollRect rect)
	{
		this.rect = rect;
		Content = rect.content;
		return this;
	}

	public PageView SetContentWidth()
	{
		if (Content.GetComponent<ContentSizeFitter>() != null)
		{
			Canvas.ForceUpdateCanvases();
		}
		else if (rect != null && rect.content != null)
		{
			Content = rect.content;
			float num = 0f;
			for (int i = 0; i < Content.childCount; i++)
			{
				RectTransform component = Content.GetChild(i).GetComponent<RectTransform>();
				if (component.gameObject.activeInHierarchy)
				{
					num += LayoutUtility.GetMinWidth(component);
				}
			}
			Content.sizeDelta = new Vector2(num, 0f);
		}
		return this;
	}

	public void SetAllPagePosition()
	{
		isDrag = false;
		posList.Clear();
		PageChilds.Clear();
		itemPoslist.Clear();
		if (rect != null && Content != null && Content.childCount != 0)
		{
			RectTransform component = Content.GetChild(0).gameObject.GetComponent<RectTransform>();
			unitWidth = LayoutUtility.GetMinWidth(component);
			float num = Content.rect.width - unitWidth;
			for (int i = 0; i < Content.transform.childCount; i++)
			{
				posList.Add(unitWidth * (float)i / num);
				Vector2 item = new Vector2((0f - unitWidth) * (float)i, 0f);
				itemPoslist.Add(item);
				SetPageList(Content.transform.GetChild(i).gameObject);
			}
		}
	}

	private void SetPageList(GameObject ChildGo)
	{
		if (ChildGo != null)
		{
			ScrollRect componentInChildren = ChildGo.GetComponentInChildren<ScrollRect>();
			if (componentInChildren != null)
			{
				PageChilds.Add(componentInChildren.gameObject);
			}
		}
	}

	public void SetViewCentre()
	{
		if (rect != null)
		{
			RectTransform component = rect.GetComponent<RectTransform>();
			if (component != null)
			{
				centre = component.anchoredPosition;
			}
		}
	}

	private void Update()
	{
		if (isDrag || stopMove)
		{
			return;
		}
		startTime += Time.deltaTime;
		float t = startTime * Mathf.Max(smooting, dragSmooting);
		if ((double)Mathf.Abs(rect.horizontalNormalizedPosition - targethorizontal) < 0.02)
		{
			rect.horizontalNormalizedPosition = targethorizontal;
			stopMove = true;
			if (OnPageMoveDone != null)
			{
				OnPageMoveDone(PageChilds[currentPageIndex]);
			}
		}
		else
		{
			rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, t);
		}
	}

	public void ClearChild()
	{
		PageChilds.Clear();
	}

	private void OnDestroy()
	{
		ClearChild();
	}

	public void PageTo(int index)
	{
		if (index < 0 || index >= posList.Count)
		{
			return;
		}
		if (rect != null)
		{
			Vector2 vector = itemPoslist[index];
			if (Vector2.Distance(vector, Content.anchoredPosition) > 0.02f)
			{
				Content.anchoredPosition = vector;
			}
		}
		SetPageIndex(index);
	}

	public void AutoPageTo(int index)
	{
		if (!isDrag)
		{
			index = Mathf.Clamp(index, 0, posList.Count);
			targethorizontal = posList[index];
			startTime = 0f;
			SetPageIndex(index);
			stopMove = false;
			if (OnPageMoveBegin != null)
			{
				OnPageMoveBegin();
			}
		}
	}

	private void SetPageIndex(int index)
	{
		if (CurrentPageIndex != index)
		{
			currentPageIndex = index;
			if (OnPageChanged != null)
			{
				OnPageChanged(index);
			}
		}
	}

	public void OnInitializePotentialDrag(PointerEventData eventData)
	{
		initializePos = eventData.position;
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
		OnBeginDrag(eventData.position);
		if (!isVertical)
		{
			dragSmooting = 0f;
			inputList.Clear();
			isDrag = true;
			startDragHorizontal = rect.horizontalNormalizedPosition;
			dragTime = Time.time;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!isDrag)
		{
			return;
		}
		dragTime = Time.time - dragTime;
		if (dragTime < 0.2f)
		{
			Vector2 vector = inputList.Last.Value - inputList.First.Value;
			float num = Mathf.Abs(vector.x);
			if (num > screenValue)
			{
				dragSmooting = num / 10f;
				dragSmooting = Mathf.Clamp(dragSmooting, 0f, dragSmootingMax);
				int num2 = 0;
				if (vector.x > 0f)
				{
					if (currentPageIndex == 0)
					{
						return;
					}
					num2 = currentPageIndex - 1;
				}
				else
				{
					if (currentPageIndex == posList.Count - 1)
					{
						return;
					}
					num2 = currentPageIndex + 1;
				}
				isDrag = false;
				AutoPageTo(num2);
				return;
			}
		}
		float horizontalNormalizedPosition = rect.horizontalNormalizedPosition;
		horizontalNormalizedPosition += (horizontalNormalizedPosition - startDragHorizontal) * sensitivity;
		horizontalNormalizedPosition = ((horizontalNormalizedPosition < 1f) ? horizontalNormalizedPosition : 1f);
		horizontalNormalizedPosition = ((horizontalNormalizedPosition > 0f) ? horizontalNormalizedPosition : 0f);
		int num3 = 0;
		float num4 = Mathf.Abs(posList[num3] - horizontalNormalizedPosition);
		for (int i = 1; i < posList.Count; i++)
		{
			float num5 = Mathf.Abs(posList[i] - horizontalNormalizedPosition);
			if (num5 < num4)
			{
				num3 = i;
				num4 = num5;
			}
		}
		SetPageIndex(num3);
		targethorizontal = posList[num3];
		isDrag = false;
		startTime = 0f;
		stopMove = false;
		if (OnPageMoveBegin != null)
		{
			OnPageMoveBegin();
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (isDrag)
		{
			inputList.Add(eventData.position);
		}
	}
}
