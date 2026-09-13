using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageViewComponent : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler
{
	private ScrollRect rect;

	private float targethorizontal;

	private bool isDrag;

	private List<float> posList = new List<float>();

	private int currentPageIndex = -1;

	public Action<int> onPageChanged;

	public RectTransform item;

	public RectTransform Content;

	private bool stopMove = true;

	public float smooting = 4f;

	public float sensitivity;

	private float startTime;

	public bool _forceOneByOne;

	public bool _emptyEnds;

	private float startDragHorizontal;

	public void Refresh(Action<int> onUpdate)
	{
		rect = base.transform.GetComponent<ScrollRect>();
		float num = rect.content.rect.width - GetComponent<RectTransform>().rect.width;
		float num2 = rect.content.rect.width / item.rect.width;
		posList.Clear();
		posList.Add(0f);
		for (int i = 1; (float)i < num2 - 1f; i++)
		{
			posList.Add(GetComponent<RectTransform>().rect.width * (float)i / num);
		}
		onPageChanged = onUpdate;
		posList.Add(1f);
	}

	public void RefreshByCell(Action<int> OnUpdate)
	{
		rect = base.transform.GetComponent<ScrollRect>();
		int num = Mathf.RoundToInt(rect.content.rect.width / item.rect.width);
		float num2 = item.rect.width / 2f;
		float num3 = GetComponent<RectTransform>().rect.width / 2f;
		float num4 = rect.content.rect.width - num3 * 2f;
		float num5 = num2 * 2f / num4;
		float num6 = num2 * 2f / rect.content.rect.width;
		float num7 = (num3 - num2) / rect.content.rect.width;
		int num8 = 0;
		posList.Clear();
		for (int i = 0; i < num; i++)
		{
			float num9 = 0f;
			if ((float)i * num6 < num7)
			{
				num9 = 0f;
				num8 = i;
			}
			else
			{
				num9 = Mathf.Min((float)(num8 + 1) * num6 - num7 + (float)(i - (num8 + 1)) * num5, 1f);
			}
			posList.Add(num9);
		}
		onPageChanged = OnUpdate;
	}

	private void Update()
	{
		if (!isDrag && !stopMove)
		{
			startTime += Time.deltaTime;
			float num = startTime * smooting;
			rect.horizontalNormalizedPosition = Mathf.Lerp(rect.horizontalNormalizedPosition, targethorizontal, num);
			if (num >= 1f)
			{
				stopMove = true;
			}
		}
	}

	private void OnDestroy()
	{
		onPageChanged = null;
	}

	public void pageTo(int index)
	{
		if (index >= 0 && index < posList.Count)
		{
			rect.horizontalNormalizedPosition = posList[index];
			SetPageIndex(index);
		}
	}

	private void SetPageIndex(int index)
	{
		if (currentPageIndex != index)
		{
			currentPageIndex = index;
			if (onPageChanged != null)
			{
				onPageChanged(index);
			}
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		isDrag = true;
		startDragHorizontal = rect.horizontalNormalizedPosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		int num = 0;
		if (_forceOneByOne)
		{
			if (!Mathf.Approximately(rect.horizontalNormalizedPosition, startDragHorizontal))
			{
				bool num2 = rect.horizontalNormalizedPosition > startDragHorizontal;
				int num3 = (_emptyEnds ? 1 : 0);
				if (num2)
				{
					num = currentPageIndex + 1;
					num = math.min(num, posList.Count - 1 - num3);
				}
				else
				{
					num = currentPageIndex - 1;
					num = math.max(num, num3);
				}
			}
			else
			{
				num = currentPageIndex;
			}
		}
		else
		{
			float horizontalNormalizedPosition = rect.horizontalNormalizedPosition;
			horizontalNormalizedPosition += (horizontalNormalizedPosition - startDragHorizontal) * sensitivity;
			horizontalNormalizedPosition = ((horizontalNormalizedPosition < 1f) ? horizontalNormalizedPosition : 1f);
			horizontalNormalizedPosition = ((horizontalNormalizedPosition > 0f) ? horizontalNormalizedPosition : 0f);
			float num4 = Mathf.Abs(posList[num] - horizontalNormalizedPosition);
			for (int i = 1; i < posList.Count; i++)
			{
				float num5 = Mathf.Abs(posList[i] - horizontalNormalizedPosition);
				if (num5 < num4)
				{
					num = i;
					num4 = num5;
				}
			}
		}
		SetPageIndex(num);
		targethorizontal = posList[num];
		isDrag = false;
		startTime = 0f;
		stopMove = false;
	}
}
