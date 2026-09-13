using System;
using Unity.Profiling;
using UnityEngine.Yoga;

namespace UnityEngine.UIElements;

internal class UIRLayoutUpdater : BaseVisualTreeUpdater
{
	private const int kMaxValidateLayoutCount = 5;

	private static readonly string s_Description = "UIR Update Layout";

	private static readonly ProfilerMarker s_ProfilerMarker = new ProfilerMarker(s_Description);

	public override ProfilerMarker profilerMarker => s_ProfilerMarker;

	public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
	{
		if ((versionChangeType & (VersionChangeType.Hierarchy | VersionChangeType.Layout)) != 0)
		{
			if ((versionChangeType & VersionChangeType.Hierarchy) != 0 && base.panel.duringLayoutPhase)
			{
				throw new InvalidOperationException("Hierarchy change detected while computing layout, this is not supported.");
			}
			YogaNode yogaNode = ve.yogaNode;
			if (yogaNode != null && yogaNode.IsMeasureDefined)
			{
				yogaNode.MarkDirty();
			}
		}
	}

	public override void Update()
	{
		int num = 0;
		while (base.visualTree.yogaNode.IsDirty)
		{
			if (num > 0)
			{
				base.panel.ApplyStyles();
			}
			base.panel.duringLayoutPhase = true;
			base.visualTree.yogaNode.CalculateLayout();
			base.panel.duringLayoutPhase = false;
			using (new EventDispatcherGate(base.visualTree.panel.dispatcher))
			{
				UpdateSubTree(base.visualTree, num);
			}
			if (num++ >= 5)
			{
				Debug.LogError("Layout update is struggling to process current layout (consider simplifying to avoid recursive layout): " + base.visualTree);
				break;
			}
		}
	}

	private void UpdateSubTree(VisualElement ve, int currentLayoutPass)
	{
		Rect rect = new Rect(ve.yogaNode.LayoutX, ve.yogaNode.LayoutY, ve.yogaNode.LayoutWidth, ve.yogaNode.LayoutHeight);
		Rect lastLayout = ve.lastLayout;
		bool flag = false;
		VersionChangeType versionChangeType = (VersionChangeType)0;
		if (lastLayout.width != rect.width || lastLayout.height != rect.height)
		{
			versionChangeType |= VersionChangeType.Size | VersionChangeType.Repaint;
			flag = true;
		}
		if (rect.position != lastLayout.position)
		{
			versionChangeType |= VersionChangeType.Transform;
			flag = true;
		}
		if (versionChangeType != 0)
		{
			ve.IncrementVersion(versionChangeType);
		}
		ve.lastLayout = rect;
		bool hasNewLayout = ve.yogaNode.HasNewLayout;
		if (hasNewLayout)
		{
			int childCount = ve.hierarchy.childCount;
			for (int i = 0; i < childCount; i++)
			{
				UpdateSubTree(ve.hierarchy[i], currentLayoutPass);
			}
		}
		if (flag)
		{
			using GeometryChangedEvent geometryChangedEvent = GeometryChangedEvent.GetPooled(lastLayout, rect);
			geometryChangedEvent.layoutPass = currentLayoutPass;
			geometryChangedEvent.target = ve;
			ve.SendEvent(geometryChangedEvent);
		}
		if (hasNewLayout)
		{
			ve.yogaNode.MarkLayoutSeen();
		}
	}
}
