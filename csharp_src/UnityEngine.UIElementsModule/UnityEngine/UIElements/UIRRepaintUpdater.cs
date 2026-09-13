using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine.UIElements.UIR;

namespace UnityEngine.UIElements;

internal class UIRRepaintUpdater : BaseVisualTreeUpdater
{
	internal RenderChain renderChain;

	private static ProfilerMarker s_MarkerDrawChain;

	private static readonly string s_Description;

	private static readonly ProfilerMarker s_ProfilerMarker;

	public override ProfilerMarker profilerMarker => s_ProfilerMarker;

	protected bool disposed { get; private set; }

	public event Action<UIRenderDevice> BeforeDrawChain
	{
		add
		{
			if (renderChain != null)
			{
				renderChain.BeforeDrawChain += value;
			}
		}
		remove
		{
			if (renderChain != null)
			{
				renderChain.BeforeDrawChain -= value;
			}
		}
	}

	public UIRRepaintUpdater()
	{
		base.panelChanged += OnPanelChanged;
	}

	public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
	{
		if (renderChain != null)
		{
			bool flag = (versionChangeType & VersionChangeType.Transform) != 0;
			bool flag2 = (versionChangeType & VersionChangeType.Size) != 0;
			bool flag3 = (versionChangeType & VersionChangeType.Overflow) != 0;
			bool flag4 = (versionChangeType & VersionChangeType.BorderRadius) != 0;
			bool flag5 = (versionChangeType & VersionChangeType.BorderWidth) != 0;
			if (flag || flag2 || flag5)
			{
				renderChain.UIEOnTransformOrSizeChanged(ve, flag, flag2 || flag5);
			}
			if (flag3 || flag4)
			{
				renderChain.UIEOnClippingChanged(ve, hierarchical: false);
			}
			if ((versionChangeType & VersionChangeType.Opacity) != 0)
			{
				renderChain.UIEOnOpacityChanged(ve);
			}
			if ((versionChangeType & VersionChangeType.Repaint) != 0)
			{
				renderChain.UIEOnVisualsChanged(ve, hierarchical: false);
			}
		}
	}

	public override void Update()
	{
		if (renderChain?.device != null)
		{
			DrawChain(base.panel.GetViewport(), base.panel.GetProjection());
		}
	}

	internal RenderChain DebugGetRenderChain()
	{
		return renderChain;
	}

	protected virtual RenderChain CreateRenderChain()
	{
		return new RenderChain(base.panel, base.panel.standardShader);
	}

	protected virtual void DrawChain(Rect viewport, Matrix4x4 projection)
	{
		using (s_MarkerDrawChain.Auto())
		{
			renderChain.Render(viewport, projection, base.panel.clearFlags);
		}
	}

	static UIRRepaintUpdater()
	{
		s_MarkerDrawChain = new ProfilerMarker("DrawChain");
		s_Description = "UIRepaint";
		s_ProfilerMarker = new ProfilerMarker(s_Description);
		Utility.GraphicsResourcesRecreate += OnGraphicsResourcesRecreate;
	}

	private static void OnGraphicsResourcesRecreate(bool recreate)
	{
		if (!recreate)
		{
			UIRenderDevice.PrepareForGfxDeviceRecreate();
		}
		Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
		while (panelsIterator.MoveNext())
		{
			RenderChain renderChain = (panelsIterator.Current.Value.GetUpdater(VisualTreeUpdatePhase.Repaint) as UIRRepaintUpdater)?.renderChain;
			if (recreate)
			{
				renderChain?.AfterRenderDeviceRelease();
			}
			else
			{
				renderChain?.BeforeRenderDeviceRelease();
			}
		}
		if (!recreate)
		{
			UIRenderDevice.FlushAllPendingDeviceDisposes();
		}
		else
		{
			UIRenderDevice.WrapUpGfxDeviceRecreate();
		}
	}

	private void OnPanelChanged(BaseVisualElementPanel obj)
	{
		DisposeRenderChain();
		if (base.panel != null)
		{
			renderChain = CreateRenderChain();
			if (base.panel.visualTree != null)
			{
				renderChain.UIEOnChildAdded(base.panel.visualTree.hierarchy.parent, base.panel.visualTree, (base.panel.visualTree.hierarchy.parent != null) ? base.panel.visualTree.hierarchy.parent.IndexOf(base.panel.visualTree) : 0);
				renderChain.UIEOnVisualsChanged(base.panel.visualTree, hierarchical: true);
			}
			base.panel.standardShaderChanged += OnPanelStandardShaderChanged;
			base.panel.hierarchyChanged += OnPanelHierarchyChanged;
		}
	}

	private void OnPanelHierarchyChanged(VisualElement ve, HierarchyChangeType changeType)
	{
		if (renderChain != null && ve.panel != null)
		{
			switch (changeType)
			{
			case HierarchyChangeType.Add:
				renderChain.UIEOnChildAdded(ve.hierarchy.parent, ve, (ve.hierarchy.parent != null) ? ve.hierarchy.parent.IndexOf(ve) : 0);
				break;
			case HierarchyChangeType.Remove:
				renderChain.UIEOnChildRemoving(ve);
				break;
			case HierarchyChangeType.Move:
				renderChain.UIEOnChildrenReordered(ve);
				break;
			}
		}
	}

	private void OnPanelStandardShaderChanged()
	{
		if (renderChain != null)
		{
			renderChain.UIEOnStandardShaderChanged(base.panel.standardShader);
		}
	}

	private void ResetAllElementsDataRecursive(VisualElement ve)
	{
		ve.renderChainData = default(RenderChainVEData);
		int num = ve.hierarchy.childCount - 1;
		while (num >= 0)
		{
			ResetAllElementsDataRecursive(ve.hierarchy[num--]);
		}
	}

	private void DisposeRenderChain()
	{
		if (renderChain != null)
		{
			IPanel panel = renderChain.panel;
			renderChain.Dispose();
			renderChain = null;
			if (panel != null)
			{
				base.panel.hierarchyChanged -= OnPanelHierarchyChanged;
				base.panel.standardShaderChanged -= OnPanelStandardShaderChanged;
				ResetAllElementsDataRecursive(panel.visualTree);
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				DisposeRenderChain();
			}
			disposed = true;
		}
	}
}
