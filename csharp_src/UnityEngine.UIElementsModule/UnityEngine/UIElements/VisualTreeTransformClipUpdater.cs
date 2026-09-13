using Unity.Profiling;

namespace UnityEngine.UIElements;

internal class VisualTreeTransformClipUpdater : BaseVisualTreeUpdater
{
	private uint m_Version = 0u;

	private uint m_LastVersion = 0u;

	private static readonly string s_Description = "Update Transform";

	private static readonly ProfilerMarker s_ProfilerMarker = new ProfilerMarker(s_Description);

	public override ProfilerMarker profilerMarker => s_ProfilerMarker;

	public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
	{
		if ((versionChangeType & (VersionChangeType.Hierarchy | VersionChangeType.Overflow | VersionChangeType.BorderWidth | VersionChangeType.Transform | VersionChangeType.Size)) != 0)
		{
			bool flag = (versionChangeType & VersionChangeType.Transform) != 0;
			bool flag2 = (versionChangeType & (VersionChangeType.Overflow | VersionChangeType.BorderWidth | VersionChangeType.Transform | VersionChangeType.Size)) != 0;
			flag = flag && !ve.isWorldTransformDirty;
			flag2 = flag2 && !ve.isWorldClipDirty;
			if (flag || flag2)
			{
				DirtyHierarchy(ve, flag, flag2);
			}
			DirtyBoundingBoxHierarchy(ve);
			m_Version++;
		}
	}

	private static void DirtyHierarchy(VisualElement ve, bool mustDirtyWorldTransform, bool mustDirtyWorldClip)
	{
		if (mustDirtyWorldTransform)
		{
			ve.isWorldTransformDirty = true;
			ve.isWorldBoundingBoxDirty = true;
		}
		if (mustDirtyWorldClip)
		{
			ve.isWorldClipDirty = true;
		}
		int childCount = ve.hierarchy.childCount;
		for (int i = 0; i < childCount; i++)
		{
			VisualElement visualElement = ve.hierarchy[i];
			if ((mustDirtyWorldTransform && !visualElement.isWorldTransformDirty) || (mustDirtyWorldClip && !visualElement.isWorldClipDirty))
			{
				DirtyHierarchy(visualElement, mustDirtyWorldTransform, mustDirtyWorldClip);
			}
		}
	}

	private static void DirtyBoundingBoxHierarchy(VisualElement ve)
	{
		ve.isBoundingBoxDirty = true;
		ve.isWorldBoundingBoxDirty = true;
		VisualElement parent = ve.hierarchy.parent;
		while (parent != null && !parent.isBoundingBoxDirty)
		{
			parent.isBoundingBoxDirty = true;
			parent.isWorldBoundingBoxDirty = true;
			parent = parent.hierarchy.parent;
		}
	}

	public override void Update()
	{
		if (m_Version != m_LastVersion)
		{
			m_LastVersion = m_Version;
			base.panel.UpdateElementUnderPointers();
			base.panel.visualTree.UpdateBoundingBox();
		}
	}
}
