#define UNITY_ASSERTIONS
using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements;

internal abstract class BaseVisualElementPanel : IPanel, IDisposable
{
	private float m_Scale = 1f;

	private float m_PixelsPerPoint = 1f;

	internal ElementUnderPointer m_TopElementUnderPointers = new ElementUnderPointer();

	public abstract EventInterests IMGUIEventInterests { get; set; }

	public abstract ScriptableObject ownerObject { get; protected set; }

	public abstract SavePersistentViewData saveViewData { get; set; }

	public abstract GetViewDataDictionary getViewDataDictionary { get; set; }

	public abstract int IMGUIContainersCount { get; set; }

	public abstract IMGUIContainer rootIMGUIContainer { get; set; }

	public abstract FocusController focusController { get; set; }

	internal float scale
	{
		get
		{
			return m_Scale;
		}
		set
		{
			if (!Mathf.Approximately(m_Scale, value))
			{
				m_Scale = value;
				visualTree.IncrementVersion(VersionChangeType.StyleSheet);
			}
		}
	}

	internal float pixelsPerPoint
	{
		get
		{
			return m_PixelsPerPoint;
		}
		set
		{
			if (!Mathf.Approximately(m_PixelsPerPoint, value))
			{
				m_PixelsPerPoint = value;
				visualTree.IncrementVersion(VersionChangeType.StyleSheet);
			}
		}
	}

	public float scaledPixelsPerPoint => m_PixelsPerPoint * m_Scale;

	internal PanelClearFlags clearFlags { get; set; } = PanelClearFlags.All;

	internal bool duringLayoutPhase { get; set; }

	internal bool isDirty => version != repaintVersion;

	internal abstract uint version { get; }

	internal abstract uint repaintVersion { get; }

	internal virtual RepaintData repaintData { get; set; }

	internal virtual ICursorManager cursorManager { get; set; }

	public ContextualMenuManager contextualMenuManager { get; internal set; }

	public abstract VisualElement visualTree { get; }

	public abstract EventDispatcher dispatcher { get; protected set; }

	internal abstract IScheduler scheduler { get; }

	public abstract ContextType contextType { get; protected set; }

	internal bool disposed { get; private set; }

	internal abstract Shader standardShader { get; set; }

	internal event Action standardShaderChanged;

	internal event HierarchyEvent hierarchyChanged;

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing && ownerObject != null)
			{
				UIElementsUtility.RemoveCachedPanel(ownerObject.GetInstanceID());
			}
			disposed = true;
		}
	}

	public abstract void Repaint(Event e);

	public abstract void ValidateLayout();

	public abstract void UpdateAnimations();

	public abstract void UpdateBindings();

	public abstract void ApplyStyles();

	internal abstract void OnVersionChanged(VisualElement ele, VersionChangeType changeTypeFlag);

	internal abstract void SetUpdater(IVisualTreeUpdater updater, VisualTreeUpdatePhase phase);

	internal Matrix4x4 GetProjection()
	{
		Rect layout = visualTree.layout;
		return ProjectionUtils.Ortho(layout.xMin, layout.xMax, layout.yMax, layout.yMin, -1f, 1f);
	}

	internal Rect GetViewport()
	{
		return visualTree.layout;
	}

	internal void SendEvent(EventBase e, DispatchMode dispatchMode = DispatchMode.Default)
	{
		Debug.Assert(dispatcher != null);
		dispatcher?.Dispatch(e, this, dispatchMode);
	}

	public abstract VisualElement Pick(Vector2 point);

	public abstract VisualElement PickAll(Vector2 point, List<VisualElement> picked);

	internal abstract IVisualTreeUpdater GetUpdater(VisualTreeUpdatePhase phase);

	internal VisualElement GetTopElementUnderPointer(int pointerId)
	{
		return m_TopElementUnderPointers.GetTopElementUnderPointer(pointerId);
	}

	private void SetElementUnderPointer(VisualElement newElementUnderPointer, int pointerId, Vector2 pointerPos)
	{
		m_TopElementUnderPointers.SetElementUnderPointer(newElementUnderPointer, pointerId, pointerPos);
	}

	internal void SetElementUnderPointer(VisualElement newElementUnderPointer, EventBase triggerEvent)
	{
		m_TopElementUnderPointers.SetElementUnderPointer(newElementUnderPointer, triggerEvent);
	}

	internal void ClearCachedElementUnderPointer(EventBase triggerEvent)
	{
		m_TopElementUnderPointers.SetTemporaryElementUnderPointer(null, triggerEvent);
	}

	internal void CommitElementUnderPointers()
	{
		m_TopElementUnderPointers.CommitElementUnderPointers(dispatcher);
	}

	protected void InvokeStandardShaderChanged()
	{
		if (this.standardShaderChanged != null)
		{
			this.standardShaderChanged();
		}
	}

	internal void InvokeHierarchyChanged(VisualElement ve, HierarchyChangeType changeType)
	{
		if (this.hierarchyChanged != null)
		{
			this.hierarchyChanged(ve, changeType);
		}
	}

	internal void UpdateElementUnderPointers()
	{
		foreach (int hoveringPointer in PointerId.hoveringPointers)
		{
			if (PointerDeviceState.GetPanel(hoveringPointer) != this)
			{
				SetElementUnderPointer(null, hoveringPointer, new Vector2(float.MinValue, float.MinValue));
				continue;
			}
			Vector2 pointerPosition = PointerDeviceState.GetPointerPosition(hoveringPointer);
			VisualElement newElementUnderPointer = PickAll(pointerPosition, null);
			SetElementUnderPointer(newElementUnderPointer, hoveringPointer, pointerPosition);
		}
		CommitElementUnderPointers();
	}

	public void Update()
	{
		scheduler.UpdateScheduledEvents();
		ValidateLayout();
		UpdateBindings();
	}
}
