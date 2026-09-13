using System;
using System.Collections.Generic;
using Unity.Profiling;

namespace UnityEngine.UIElements;

internal class VisualTreeBindingsUpdater : BaseVisualTreeHierarchyTrackerUpdater
{
	private static readonly string s_Description = "Update Bindings";

	private static readonly ProfilerMarker s_ProfilerMarker = new ProfilerMarker(s_Description);

	private readonly HashSet<VisualElement> m_ElementsWithBindings = new HashSet<VisualElement>();

	private readonly HashSet<VisualElement> m_ElementsToAdd = new HashSet<VisualElement>();

	private readonly HashSet<VisualElement> m_ElementsToRemove = new HashSet<VisualElement>();

	private const int kMinUpdateDelay = 100;

	private long m_LastUpdateTime = 0L;

	private static ProfilerMarker s_MarkerUpdate = new ProfilerMarker("Bindings.Update");

	private static ProfilerMarker s_MarkerPoll = new ProfilerMarker("Bindings.PollElementsWithBindings");

	private List<IBinding> updatedBindings = new List<IBinding>();

	public override ProfilerMarker profilerMarker => s_ProfilerMarker;

	private IBinding GetUpdaterFromElement(VisualElement ve)
	{
		return (ve as IBindable)?.binding;
	}

	private void StartTracking(VisualElement ve)
	{
		m_ElementsToAdd.Add(ve);
		m_ElementsToRemove.Remove(ve);
	}

	private void StopTracking(VisualElement ve)
	{
		m_ElementsToRemove.Add(ve);
		m_ElementsToAdd.Remove(ve);
	}

	private void StartTrackingRecursive(VisualElement ve)
	{
		IBinding updaterFromElement = GetUpdaterFromElement(ve);
		if (updaterFromElement != null)
		{
			StartTracking(ve);
		}
		int childCount = ve.hierarchy.childCount;
		for (int i = 0; i < childCount; i++)
		{
			VisualElement ve2 = ve.hierarchy[i];
			StartTrackingRecursive(ve2);
		}
	}

	private void StopTrackingRecursive(VisualElement ve)
	{
		StopTracking(ve);
		int childCount = ve.hierarchy.childCount;
		for (int i = 0; i < childCount; i++)
		{
			VisualElement ve2 = ve.hierarchy[i];
			StopTrackingRecursive(ve2);
		}
	}

	public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
	{
		base.OnVersionChanged(ve, versionChangeType);
		if ((versionChangeType & VersionChangeType.Bindings) == VersionChangeType.Bindings)
		{
			if (GetUpdaterFromElement(ve) != null)
			{
				StartTracking(ve);
			}
			else
			{
				StopTracking(ve);
			}
		}
	}

	protected override void OnHierarchyChange(VisualElement ve, HierarchyChangeType type)
	{
		switch (type)
		{
		case HierarchyChangeType.Add:
			StartTrackingRecursive(ve);
			break;
		case HierarchyChangeType.Remove:
			StopTrackingRecursive(ve);
			break;
		}
	}

	private static long CurrentTime()
	{
		return Panel.TimeSinceStartup();
	}

	public void PerformTrackingOperations()
	{
		foreach (VisualElement item in m_ElementsToAdd)
		{
			IBinding updaterFromElement = GetUpdaterFromElement(item);
			if (updaterFromElement != null)
			{
				m_ElementsWithBindings.Add(item);
			}
		}
		m_ElementsToAdd.Clear();
		foreach (VisualElement item2 in m_ElementsToRemove)
		{
			m_ElementsWithBindings.Remove(item2);
		}
		m_ElementsToRemove.Clear();
	}

	public override void Update()
	{
		base.Update();
		PerformTrackingOperations();
		if (m_ElementsWithBindings.Count > 0)
		{
			long num = CurrentTime();
			if (m_LastUpdateTime + 100 < num)
			{
				UpdateBindings();
				m_LastUpdateTime = num;
			}
		}
	}

	private void UpdateBindings()
	{
		foreach (VisualElement elementsWithBinding in m_ElementsWithBindings)
		{
			IBinding updaterFromElement = GetUpdaterFromElement(elementsWithBinding);
			if (updaterFromElement == null || elementsWithBinding.elementPanel != base.panel)
			{
				updaterFromElement?.Release();
				StopTracking(elementsWithBinding);
			}
			else
			{
				updatedBindings.Add(updaterFromElement);
			}
		}
		foreach (IBinding updatedBinding in updatedBindings)
		{
			updatedBinding.PreUpdate();
		}
		foreach (IBinding updatedBinding2 in updatedBindings)
		{
			updatedBinding2.Update();
		}
		updatedBindings.Clear();
	}

	internal void PollElementsWithBindings(Action<VisualElement, IBinding> callback)
	{
		PerformTrackingOperations();
		if (m_ElementsWithBindings.Count <= 0)
		{
			return;
		}
		foreach (VisualElement elementsWithBinding in m_ElementsWithBindings)
		{
			IBinding updaterFromElement = GetUpdaterFromElement(elementsWithBinding);
			if (updaterFromElement == null || elementsWithBinding.elementPanel != base.panel)
			{
				updaterFromElement?.Release();
				StopTracking(elementsWithBinding);
			}
			else
			{
				callback(elementsWithBinding, updaterFromElement);
			}
		}
	}
}
