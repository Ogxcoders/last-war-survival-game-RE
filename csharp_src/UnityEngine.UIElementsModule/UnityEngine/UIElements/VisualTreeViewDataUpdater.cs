using System.Collections.Generic;
using Unity.Profiling;

namespace UnityEngine.UIElements;

internal class VisualTreeViewDataUpdater : BaseVisualTreeUpdater
{
	private HashSet<VisualElement> m_UpdateList = new HashSet<VisualElement>();

	private HashSet<VisualElement> m_ParentList = new HashSet<VisualElement>();

	private const int kMaxValidatePersistentDataCount = 5;

	private uint m_Version = 0u;

	private uint m_LastVersion = 0u;

	private static readonly string s_Description = "Update ViewData";

	private static readonly ProfilerMarker s_ProfilerMarker = new ProfilerMarker(s_Description);

	public override ProfilerMarker profilerMarker => s_ProfilerMarker;

	public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
	{
		if ((versionChangeType & VersionChangeType.ViewData) == VersionChangeType.ViewData)
		{
			m_Version++;
			m_UpdateList.Add(ve);
			PropagateToParents(ve);
		}
	}

	public override void Update()
	{
		if (m_Version == m_LastVersion)
		{
			return;
		}
		int num = 0;
		while (m_LastVersion != m_Version)
		{
			m_LastVersion = m_Version;
			ValidateViewDataOnSubTree(base.visualTree, enablePersistence: true);
			num++;
			if (num > 5)
			{
				Debug.LogError("UIElements: Too many children recursively added that rely on persistent view data: " + base.visualTree);
				break;
			}
		}
		m_UpdateList.Clear();
		m_ParentList.Clear();
	}

	private void ValidateViewDataOnSubTree(VisualElement ve, bool enablePersistence)
	{
		enablePersistence = ve.IsViewDataPersitenceSupportedOnChildren(enablePersistence);
		if (m_UpdateList.Contains(ve))
		{
			m_UpdateList.Remove(ve);
			ve.OnViewDataReady(enablePersistence);
		}
		if (m_ParentList.Contains(ve))
		{
			m_ParentList.Remove(ve);
			int childCount = ve.hierarchy.childCount;
			for (int i = 0; i < childCount; i++)
			{
				ValidateViewDataOnSubTree(ve.hierarchy[i], enablePersistence);
			}
		}
	}

	private void PropagateToParents(VisualElement ve)
	{
		VisualElement parent = ve.hierarchy.parent;
		while (parent != null && m_ParentList.Add(parent))
		{
			parent = parent.hierarchy.parent;
		}
	}
}
