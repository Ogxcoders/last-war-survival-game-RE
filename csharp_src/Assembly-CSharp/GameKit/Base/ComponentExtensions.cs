using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameKit.Base;

public static class ComponentExtensions
{
	public static T GetOrAddComponent<T>(this Component comp, bool set_enable = false) where T : Component
	{
		T val = comp.GetComponent<T>();
		if (val == null)
		{
			val = comp.gameObject.AddComponent<T>();
		}
		Behaviour behaviour = val as Behaviour;
		if (set_enable && behaviour != null)
		{
			behaviour.enabled = set_enable;
		}
		return val;
	}

	public static T GetOrAddComponent<T>(this GameObject go, bool set_enable = false) where T : Component
	{
		T val = go.GetComponent<T>();
		if (val == null)
		{
			val = go.AddComponent<T>();
		}
		Behaviour behaviour = val as Behaviour;
		if (set_enable && behaviour != null)
		{
			behaviour.enabled = set_enable;
		}
		return val;
	}

	public static Component GetOrAddComponent(this Component comp, Type type, bool set_enable = false)
	{
		Component component = comp.GetComponent(type);
		if (component == null)
		{
			component = comp.gameObject.AddComponent(type);
		}
		Behaviour behaviour = component as Behaviour;
		if (set_enable && behaviour != null)
		{
			behaviour.enabled = set_enable;
		}
		return component;
	}

	public static Component GetOrAddComponent(this GameObject go, Type type, bool set_enable = false)
	{
		Component component = go.GetComponent(type);
		if (component == null)
		{
			component = go.AddComponent(type);
		}
		Behaviour behaviour = component as Behaviour;
		if (set_enable && behaviour != null)
		{
			behaviour.enabled = set_enable;
		}
		return component;
	}

	public static void ScrollRect_EndDrag(this ScrollRect scrollRect)
	{
		if (!(scrollRect == null))
		{
			PointerEventData pointerEventData = new PointerEventData(null);
			pointerEventData.button = PointerEventData.InputButton.Left;
			scrollRect.OnEndDrag(pointerEventData);
		}
	}

	public static void ScrollView_EndDrag(this ScrollView scrollView)
	{
		if (!(scrollView == null))
		{
			PointerEventData pointerEventData = new PointerEventData(null);
			pointerEventData.button = PointerEventData.InputButton.Left;
			scrollView.OnEndDrag(pointerEventData);
		}
	}
}
