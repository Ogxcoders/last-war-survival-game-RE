using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectEx : ScrollRect
{
	private bool routeToParent;

	private void DoForParents<T>(Action<T> action, bool onlyForFirstParent = false) where T : IEventSystemHandler
	{
		Transform parent = base.transform.parent;
		while (parent != null)
		{
			Component[] components = parent.GetComponents<Component>();
			foreach (Component component in components)
			{
				if (component is T)
				{
					action((T)(IEventSystemHandler)component);
				}
			}
			parent = parent.parent;
		}
	}

	public override void OnInitializePotentialDrag(PointerEventData eventData)
	{
		DoForParents(delegate(IInitializePotentialDragHandler parent)
		{
			parent.OnInitializePotentialDrag(eventData);
		}, onlyForFirstParent: true);
		base.OnInitializePotentialDrag(eventData);
	}

	public override void OnDrag(PointerEventData eventData)
	{
		if (routeToParent)
		{
			DoForParents(delegate(IDragHandler parent)
			{
				parent.OnDrag(eventData);
			}, onlyForFirstParent: true);
		}
		else
		{
			base.OnDrag(eventData);
		}
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		if (!base.horizontal && Math.Abs(eventData.delta.x) > Math.Abs(eventData.delta.y))
		{
			routeToParent = true;
		}
		else if (!base.vertical && Math.Abs(eventData.delta.x) < Math.Abs(eventData.delta.y))
		{
			routeToParent = true;
		}
		else
		{
			routeToParent = false;
		}
		if (routeToParent)
		{
			DoForParents(delegate(IBeginDragHandler parent)
			{
				parent.OnBeginDrag(eventData);
			}, onlyForFirstParent: true);
		}
		else
		{
			base.OnBeginDrag(eventData);
		}
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		if (routeToParent)
		{
			DoForParents(delegate(IEndDragHandler parent)
			{
				parent.OnEndDrag(eventData);
			}, onlyForFirstParent: true);
		}
		else
		{
			base.OnEndDrag(eventData);
		}
		routeToParent = false;
	}
}
