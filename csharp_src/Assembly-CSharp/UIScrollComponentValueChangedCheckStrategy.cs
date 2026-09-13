using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class UIScrollComponentValueChangedCheckStrategy : NeedHighFPSCheckStrategyBase
{
	public struct ScrollComponentReference
	{
		public WeakReference<IScrollHandler> reference { get; private set; }

		public ScrollComponentReference(IScrollHandler target)
		{
			reference = new WeakReference<IScrollHandler>(target);
		}

		public void OnValueChanged(Vector2 unused)
		{
			if (reference.TryGetTarget(out var target))
			{
				Vector2 vector = Vector2.zero;
				if (target is ScrollRect scrollRect && scrollRect.content != null)
				{
					vector = scrollRect.velocity;
				}
				else if (target is ScrollView scrollView && scrollView.content != null)
				{
					vector = scrollView.velocity;
				}
				if (Mathf.Abs(vector.x) > 1f || Mathf.Abs(vector.y) > 1f)
				{
					OnScrollValueChanged();
				}
			}
		}

		public void AddListener()
		{
			if (reference.TryGetTarget(out var target) && target != null)
			{
				if (target is ScrollRect scrollRect && scrollRect != null)
				{
					scrollRect.onValueChanged.RemoveListener(OnValueChanged);
					scrollRect.onValueChanged.AddListener(OnValueChanged);
				}
				else if (target is ScrollView scrollView && scrollView != null)
				{
					scrollView.onValueChanged.RemoveListener(OnValueChanged);
					scrollView.onValueChanged.AddListener(OnValueChanged);
				}
			}
		}

		public void RemoveListener()
		{
			if (reference.TryGetTarget(out var target) && target != null)
			{
				if (target is ScrollRect scrollRect && scrollRect != null)
				{
					scrollRect.onValueChanged.RemoveListener(OnValueChanged);
				}
				else if (target is ScrollView scrollView && scrollView != null)
				{
					scrollView.onValueChanged.RemoveListener(OnValueChanged);
				}
			}
		}
	}

	private static class ChildrenComponentWalker<T> where T : Behaviour
	{
		private static List<T> s_Components;

		public static void Walk(GameObject go, bool includeInactive, Action<T> callback)
		{
			if (go == null)
			{
				return;
			}
			if (s_Components == null)
			{
				s_Components = new List<T>();
			}
			go.GetComponentsInChildren(includeInactive, s_Components);
			foreach (T s_Component in s_Components)
			{
				callback?.Invoke(s_Component);
			}
			s_Components.Clear();
		}
	}

	public static List<ScrollComponentReference> references { get; private set; } = new List<ScrollComponentReference>();

	public static int lastScrollRectValueChangedFrameCount { get; private set; }

	public override void Check(InputHelper.InputState last, InputHelper.InputState curr)
	{
		pass = Time.frameCount - lastScrollRectValueChangedFrameCount < 4;
		keep = 0f;
	}

	public static void AcquireHighFPSLockerForChildren(GameObject go)
	{
		ChildrenComponentWalker<ScrollRect>.Walk(go, includeInactive: true, AddListener);
		ChildrenComponentWalker<ScrollView>.Walk(go, includeInactive: true, AddListener);
	}

	public static void FreeHighFPSLockerForChildren(GameObject go)
	{
		ChildrenComponentWalker<ScrollRect>.Walk(go, includeInactive: true, RemoveListener);
		ChildrenComponentWalker<ScrollView>.Walk(go, includeInactive: true, RemoveListener);
	}

	public static void OnScrollValueChanged()
	{
		lastScrollRectValueChangedFrameCount = Time.frameCount;
	}

	private static void AddListener(IScrollHandler scroll)
	{
		foreach (ScrollComponentReference reference in references)
		{
			if (reference.reference.TryGetTarget(out var target) && target == scroll)
			{
				return;
			}
		}
		ScrollComponentReference item = new ScrollComponentReference(scroll);
		item.AddListener();
		references.Add(item);
	}

	private static void RemoveListener(IScrollHandler scroll)
	{
		for (int num = references.Count - 1; num >= 0; num--)
		{
			if (references[num].reference.TryGetTarget(out var target) && target == scroll)
			{
				references[num].RemoveListener();
				references.RemoveAt(num);
			}
		}
	}
}
