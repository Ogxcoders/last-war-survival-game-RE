#define UNITY_ASSERTIONS
using System.Collections.Generic;
using Unity.Profiling;

namespace UnityEngine.UIElements;

internal class VisualTreeStyleUpdater : BaseVisualTreeUpdater
{
	private HashSet<VisualElement> m_ApplyStyleUpdateList = new HashSet<VisualElement>();

	private bool m_IsApplyingStyles = false;

	private uint m_Version = 0u;

	private uint m_LastVersion = 0u;

	private VisualTreeStyleUpdaterTraversal m_StyleContextHierarchyTraversal = new VisualTreeStyleUpdaterTraversal();

	private static readonly string s_Description = "Update Style";

	private static readonly ProfilerMarker s_ProfilerMarker = new ProfilerMarker(s_Description);

	public override ProfilerMarker profilerMarker => s_ProfilerMarker;

	public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
	{
		if ((versionChangeType & VersionChangeType.StyleSheet) == VersionChangeType.StyleSheet)
		{
			m_Version++;
			if (m_IsApplyingStyles)
			{
				m_ApplyStyleUpdateList.Add(ve);
			}
			else
			{
				m_StyleContextHierarchyTraversal.AddChangedElement(ve);
			}
		}
	}

	public override void Update()
	{
		if (m_Version == m_LastVersion)
		{
			return;
		}
		m_LastVersion = m_Version;
		ApplyStyles();
		m_StyleContextHierarchyTraversal.Clear();
		foreach (VisualElement applyStyleUpdate in m_ApplyStyleUpdateList)
		{
			m_StyleContextHierarchyTraversal.AddChangedElement(applyStyleUpdate);
		}
		m_ApplyStyleUpdateList.Clear();
	}

	private void ApplyStyles()
	{
		Debug.Assert(base.visualTree.panel != null);
		m_IsApplyingStyles = true;
		m_StyleContextHierarchyTraversal.PrepareTraversal(base.panel.scaledPixelsPerPoint);
		m_StyleContextHierarchyTraversal.Traverse(base.visualTree);
		m_IsApplyingStyles = false;
	}
}
