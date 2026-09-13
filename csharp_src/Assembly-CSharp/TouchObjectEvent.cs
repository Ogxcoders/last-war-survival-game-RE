using System.Collections.Generic;
using UnityEngine;

internal static class TouchObjectEvent
{
	public delegate bool EventFunction<T1>(T1 handler);

	public static readonly EventFunction<ITouchObjectClickHandler> s_ClickHandler = Execute;

	public static readonly EventFunction<ITouchObjectDoubleClickHandler> s_DoubleClickHandler = Execute;

	public static readonly EventFunction<ITouchObjectBeginLongTabHandler> s_BeginLongTabHandler = Execute;

	public static readonly EventFunction<ITouchObjectEndLongTabHandler> s_EndLongTabHandler = Execute;

	public static readonly EventFunction<ITouchObjectPointerDownHandler> s_PointerDownHandler = Execute;

	public static readonly EventFunction<ITouchObjectPointerUpHandler> s_PointerUpHandler = Execute;

	public static bool Execute(ITouchObjectClickHandler handler)
	{
		return handler.OnClick();
	}

	public static bool Execute(ITouchObjectDoubleClickHandler handler)
	{
		return handler.OnDoubleClick();
	}

	public static bool Execute(ITouchObjectBeginLongTabHandler handler)
	{
		return handler.OnBeginLongTap();
	}

	public static bool Execute(ITouchObjectEndLongTabHandler handler)
	{
		return handler.OnEndLongTap();
	}

	public static bool Execute(ITouchObjectPointerDownHandler handler)
	{
		return handler.OnPointerDown();
	}

	public static bool Execute(ITouchObjectPointerUpHandler handler)
	{
		return handler.OnPointerUp();
	}

	public static bool Execute<T>(ITouchObject obj, EventFunction<T> functor) where T : ITouchObject
	{
		if (obj is T handler)
		{
			return functor(handler);
		}
		return false;
	}

	public static ITouchObject GetFirstEventObject<T>(List<ITouchObject> objs)
	{
		foreach (ITouchObject obj in objs)
		{
			if (obj is T)
			{
				return obj;
			}
		}
		return null;
	}

	public static bool ExecuteClick(ITouchObject obj)
	{
		return Execute(obj, s_ClickHandler);
	}

	public static bool ExecuteDoubleClick(ITouchObject obj)
	{
		return Execute(obj, s_DoubleClickHandler);
	}

	public static bool ExecuteBeginDrag(ITouchObject obj, Vector3 dragStartPos)
	{
		if (obj is ITouchObjectBeginDragHandler touchObjectBeginDragHandler)
		{
			return touchObjectBeginDragHandler.OnBeginDrag(dragStartPos);
		}
		return false;
	}

	public static bool ExecuteDrag(ITouchObject obj, Vector3 dragStartPos, Vector3 dragCurrPos)
	{
		if (obj is ITouchObjectDragHandler touchObjectDragHandler)
		{
			return touchObjectDragHandler.OnDrag(dragStartPos, dragCurrPos);
		}
		return false;
	}

	public static bool ExecuteEndDrag(ITouchObject obj, Vector3 dragStopPos)
	{
		if (obj is ITouchObjectEndDragHandler touchObjectEndDragHandler)
		{
			return touchObjectEndDragHandler.OnEndDrag(dragStopPos);
		}
		return false;
	}

	public static bool ExecuteBeginLongTab(ITouchObject obj)
	{
		return Execute(obj, s_BeginLongTabHandler);
	}

	public static bool ExecuteEndLongTab(ITouchObject obj)
	{
		return Execute(obj, s_EndLongTabHandler);
	}

	public static bool ExecutePointerDown(ITouchObject obj)
	{
		return Execute(obj, s_PointerDownHandler);
	}

	public static bool ExecutePointerUp(ITouchObject obj)
	{
		return Execute(obj, s_PointerUpHandler);
	}
}
