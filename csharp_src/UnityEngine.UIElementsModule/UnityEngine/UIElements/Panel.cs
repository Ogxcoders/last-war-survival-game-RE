#define UNITY_ASSERTIONS
using System;
using System.Collections.Generic;
using Unity.Profiling;

namespace UnityEngine.UIElements;

internal class Panel : BaseVisualElementPanel
{
	private VisualElement m_RootContainer;

	private VisualTreeUpdater m_VisualTreeUpdater;

	private string m_PanelName;

	private uint m_Version = 0u;

	private uint m_RepaintVersion = 0u;

	internal static Action BeforeUpdaterChange;

	internal static Action AfterUpdaterChange;

	private ProfilerMarker m_MarkerUpdate;

	private ProfilerMarker m_MarkerLayout;

	private ProfilerMarker m_MarkerBindings;

	private ProfilerMarker m_MarkerAnimations;

	private static ProfilerMarker s_MarkerPickAll = new ProfilerMarker("Panel.PickAll");

	private TimerEventScheduler m_Scheduler;

	private Focusable m_SavedFocusedElement;

	private static TimeMsFunction s_TimeSinceStartup;

	private Shader m_StandardShader;

	private bool m_ValidatingLayout = false;

	public override VisualElement visualTree => m_RootContainer;

	public override EventDispatcher dispatcher { get; protected set; }

	public TimerEventScheduler timerEventScheduler => m_Scheduler ?? (m_Scheduler = new TimerEventScheduler());

	internal override IScheduler scheduler => timerEventScheduler;

	public override ScriptableObject ownerObject { get; protected set; }

	public override ContextType contextType { get; protected set; }

	public override SavePersistentViewData saveViewData { get; set; }

	public override GetViewDataDictionary getViewDataDictionary { get; set; }

	public override FocusController focusController { get; set; }

	public override EventInterests IMGUIEventInterests { get; set; }

	internal static LoadResourceFunction loadResourceFunc { private get; set; }

	internal string name
	{
		get
		{
			return m_PanelName;
		}
		set
		{
			m_PanelName = value;
			CreateMarkers();
		}
	}

	internal static TimeMsFunction TimeSinceStartup
	{
		get
		{
			return s_TimeSinceStartup;
		}
		set
		{
			if (value == null)
			{
				value = DefaultTimeSinceStartupMs;
			}
			s_TimeSinceStartup = value;
		}
	}

	public override int IMGUIContainersCount { get; set; }

	public override IMGUIContainer rootIMGUIContainer { get; set; }

	internal override uint version => m_Version;

	internal override uint repaintVersion => m_RepaintVersion;

	internal override Shader standardShader
	{
		get
		{
			return m_StandardShader;
		}
		set
		{
			if (m_StandardShader != value)
			{
				m_StandardShader = value;
				InvokeStandardShaderChanged();
			}
		}
	}

	internal static Object LoadResource(string pathName, Type type, float dpiScaling)
	{
		Object obj = null;
		if (loadResourceFunc != null)
		{
			return loadResourceFunc(pathName, type, dpiScaling);
		}
		return Resources.Load(pathName, type);
	}

	internal void Focus()
	{
		if (m_SavedFocusedElement != null && !(m_SavedFocusedElement is IMGUIContainer))
		{
			m_SavedFocusedElement.Focus();
		}
		m_SavedFocusedElement = null;
	}

	internal void Blur()
	{
		m_SavedFocusedElement = focusController?.GetLeafFocusedElement();
		if (m_SavedFocusedElement != null && !(m_SavedFocusedElement is IMGUIContainer))
		{
			m_SavedFocusedElement.Blur();
		}
	}

	private void CreateMarkers()
	{
		if (!string.IsNullOrEmpty(m_PanelName))
		{
			m_MarkerUpdate = new ProfilerMarker("Panel.Update." + m_PanelName);
			m_MarkerLayout = new ProfilerMarker("Panel.Layout." + m_PanelName);
			m_MarkerBindings = new ProfilerMarker("Panel.Bindings." + m_PanelName);
			m_MarkerAnimations = new ProfilerMarker("Panel.Animations." + m_PanelName);
		}
		else
		{
			m_MarkerUpdate = new ProfilerMarker("Panel.Update");
			m_MarkerLayout = new ProfilerMarker("Panel.Layout");
			m_MarkerBindings = new ProfilerMarker("Panel.Bindings");
			m_MarkerAnimations = new ProfilerMarker("Panel.Animations");
		}
	}

	internal static Panel CreateEditorPanel(ScriptableObject ownerObject)
	{
		return new Panel(ownerObject, ContextType.Editor, new EventDispatcher());
	}

	public Panel(ScriptableObject ownerObject, ContextType contextType, EventDispatcher dispatcher)
	{
		m_VisualTreeUpdater = new VisualTreeUpdater(this);
		this.ownerObject = ownerObject;
		this.contextType = contextType;
		this.dispatcher = dispatcher;
		repaintData = new RepaintData();
		cursorManager = new CursorManager();
		base.contextualMenuManager = null;
		m_RootContainer = new VisualElement
		{
			name = VisualElementUtils.GetUniqueName("unity-panel-container"),
			viewDataKey = "PanelContainer"
		};
		visualTree.SetPanel(this);
		focusController = new FocusController(new VisualElementFocusRing(visualTree));
		CreateMarkers();
		InvokeHierarchyChanged(visualTree, HierarchyChangeType.Add);
	}

	protected override void Dispose(bool disposing)
	{
		if (!base.disposed)
		{
			if (disposing)
			{
				m_VisualTreeUpdater.Dispose();
			}
			base.Dispose(disposing);
		}
	}

	public static long TimeSinceStartupMs()
	{
		return (s_TimeSinceStartup == null) ? DefaultTimeSinceStartupMs() : s_TimeSinceStartup();
	}

	internal static long DefaultTimeSinceStartupMs()
	{
		return (long)(Time.realtimeSinceStartup * 1000f);
	}

	internal static VisualElement PickAllWithoutValidatingLayout(VisualElement root, Vector2 point)
	{
		return PickAll(root, point);
	}

	private static VisualElement PickAll(VisualElement root, Vector2 point, List<VisualElement> picked = null)
	{
		return PerformPick(root, point, picked);
	}

	private static VisualElement PerformPick(VisualElement root, Vector2 point, List<VisualElement> picked = null)
	{
		if (root.resolvedStyle.display == DisplayStyle.None)
		{
			return null;
		}
		if (root.pickingMode == PickingMode.Ignore && root.hierarchy.childCount == 0)
		{
			return null;
		}
		if (!root.worldBoundingBox.Contains(point))
		{
			return null;
		}
		Vector2 localPoint = root.WorldToLocal(point);
		bool flag = root.ContainsPoint(localPoint);
		if (!flag && root.ShouldClip())
		{
			return null;
		}
		VisualElement visualElement = null;
		int childCount = root.hierarchy.childCount;
		for (int num = childCount - 1; num >= 0; num--)
		{
			VisualElement root2 = root.hierarchy[num];
			VisualElement visualElement2 = PerformPick(root2, point, picked);
			if (visualElement == null && visualElement2 != null && visualElement2.visible)
			{
				visualElement = visualElement2;
			}
		}
		if (picked != null && root.enabledInHierarchy && root.visible && root.pickingMode == PickingMode.Position && flag)
		{
			picked.Add(root);
		}
		if (visualElement != null)
		{
			return visualElement;
		}
		switch (root.pickingMode)
		{
		case PickingMode.Position:
			if (flag && root.enabledInHierarchy && root.visible)
			{
				return root;
			}
			break;
		}
		return null;
	}

	public override VisualElement PickAll(Vector2 point, List<VisualElement> picked)
	{
		ValidateLayout();
		picked?.Clear();
		return PickAll(visualTree, point, picked);
	}

	public override VisualElement Pick(Vector2 point)
	{
		ValidateLayout();
		Vector2 pickPosition;
		bool isTemporary;
		VisualElement topElementUnderPointer = m_TopElementUnderPointers.GetTopElementUnderPointer(PointerId.mousePointerId, out pickPosition, out isTemporary);
		if (!isTemporary && (pickPosition - point).sqrMagnitude < 0.25f)
		{
			return topElementUnderPointer;
		}
		return PickAll(visualTree, point);
	}

	public override void ValidateLayout()
	{
		if (!m_ValidatingLayout)
		{
			m_ValidatingLayout = true;
			m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Styles);
			m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Layout);
			m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.TransformClip);
			m_ValidatingLayout = false;
		}
	}

	public override void UpdateAnimations()
	{
		m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Animation);
	}

	public override void UpdateBindings()
	{
		m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Bindings);
	}

	public override void ApplyStyles()
	{
		m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Styles);
	}

	private void UpdateForRepaint()
	{
		m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.ViewData);
		m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Styles);
		m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Layout);
		m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.TransformClip);
		m_VisualTreeUpdater.UpdateVisualTreePhase(VisualTreeUpdatePhase.Repaint);
	}

	public override void Repaint(Event e)
	{
		if (contextType == ContextType.Editor)
		{
			Debug.Assert(GUIClip.Internal_GetCount() == 0, "UIElement is not compatible with IMGUI GUIClips, only GUIClip.ParentClipScope");
		}
		m_RepaintVersion = version;
		if (contextType == ContextType.Editor)
		{
			base.pixelsPerPoint = GUIUtility.pixelsPerPoint;
		}
		repaintData.repaintEvent = e;
		using (m_MarkerUpdate.Auto())
		{
			UpdateForRepaint();
		}
	}

	internal override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
	{
		m_Version++;
		m_VisualTreeUpdater.OnVersionChanged(ve, versionChangeType);
	}

	internal override void SetUpdater(IVisualTreeUpdater updater, VisualTreeUpdatePhase phase)
	{
		m_VisualTreeUpdater.SetUpdater(updater, phase);
	}

	internal override IVisualTreeUpdater GetUpdater(VisualTreeUpdatePhase phase)
	{
		return m_VisualTreeUpdater.GetUpdater(phase);
	}
}
