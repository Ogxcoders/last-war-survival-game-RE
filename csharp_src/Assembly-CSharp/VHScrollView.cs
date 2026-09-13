using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VHScrollView : ScrollRect
{
	[SerializeField]
	public ScrollRect parentScroll;

	public bool isVertical;

	private bool isSelf;

	public override void OnBeginDrag(PointerEventData eventData)
	{
		Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
		if (isVertical)
		{
			if (Mathf.Abs(deltaPosition.x) < Mathf.Abs(deltaPosition.y))
			{
				isSelf = true;
				base.OnBeginDrag(eventData);
			}
			else
			{
				isSelf = false;
				parentScroll.OnBeginDrag(eventData);
			}
		}
		else if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
		{
			isSelf = true;
			base.OnBeginDrag(eventData);
		}
		else
		{
			isSelf = false;
			parentScroll.OnBeginDrag(eventData);
		}
	}

	public override void OnDrag(PointerEventData eventData)
	{
		if (isSelf)
		{
			base.OnDrag(eventData);
		}
		else
		{
			parentScroll.OnDrag(eventData);
		}
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		if (isSelf)
		{
			base.OnEndDrag(eventData);
		}
		else
		{
			parentScroll.OnEndDrag(eventData);
		}
	}
}
