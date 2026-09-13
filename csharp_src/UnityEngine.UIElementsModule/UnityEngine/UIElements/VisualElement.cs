#define UNITY_ASSERTIONS
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UIElements.Experimental;
using UnityEngine.UIElements.StyleSheets;
using UnityEngine.UIElements.UIR;
using UnityEngine.Yoga;

namespace UnityEngine.UIElements;

public class VisualElement : Focusable, ITransform, ITransitionAnimations, IExperimentalFeatures, IVisualElementScheduler, IResolvedStyle
{
	public class UxmlFactory : UxmlFactory<VisualElement, UxmlTraits>
	{
	}

	public class UxmlTraits : UnityEngine.UIElements.UxmlTraits
	{
		private UxmlStringAttributeDescription m_Name = new UxmlStringAttributeDescription
		{
			name = "name"
		};

		private UxmlStringAttributeDescription m_ViewDataKey = new UxmlStringAttributeDescription
		{
			name = "view-data-key"
		};

		protected UxmlEnumAttributeDescription<PickingMode> m_PickingMode = new UxmlEnumAttributeDescription<PickingMode>
		{
			name = "picking-mode",
			obsoleteNames = new string[1] { "pickingMode" }
		};

		private UxmlStringAttributeDescription m_Tooltip = new UxmlStringAttributeDescription
		{
			name = "tooltip"
		};

		private UxmlEnumAttributeDescription<UsageHints> m_UsageHints = new UxmlEnumAttributeDescription<UsageHints>
		{
			name = "usage-hints"
		};

		private UxmlIntAttributeDescription m_TabIndex = new UxmlIntAttributeDescription
		{
			name = "tabindex",
			defaultValue = 0
		};

		private UxmlStringAttributeDescription m_Class = new UxmlStringAttributeDescription
		{
			name = "class"
		};

		private UxmlStringAttributeDescription m_ContentContainer = new UxmlStringAttributeDescription
		{
			name = "content-container",
			obsoleteNames = new string[1] { "contentContainer" }
		};

		private UxmlStringAttributeDescription m_Style = new UxmlStringAttributeDescription
		{
			name = "style"
		};

		protected UxmlIntAttributeDescription focusIndex { get; set; } = new UxmlIntAttributeDescription
		{
			name = null,
			obsoleteNames = new string[2] { "focus-index", "focusIndex" },
			defaultValue = -1
		};

		protected UxmlBoolAttributeDescription focusable { get; set; } = new UxmlBoolAttributeDescription
		{
			name = "focusable",
			defaultValue = false
		};

		public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
		{
			get
			{
				yield return new UxmlChildElementDescription(typeof(VisualElement));
			}
		}

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			if (ve == null)
			{
				throw new ArgumentNullException("ve");
			}
			ve.name = m_Name.GetValueFromBag(bag, cc);
			ve.viewDataKey = m_ViewDataKey.GetValueFromBag(bag, cc);
			ve.pickingMode = m_PickingMode.GetValueFromBag(bag, cc);
			if (ve.panel == null)
			{
				ve.usageHints = m_UsageHints.GetValueFromBag(bag, cc);
			}
			int value = 0;
			if (focusIndex.TryGetValueFromBag(bag, cc, ref value))
			{
				ve.tabIndex = ((value >= 0) ? value : 0);
				ve.focusable = value >= 0;
			}
			if (m_TabIndex.TryGetValueFromBag(bag, cc, ref value))
			{
				ve.tabIndex = value;
			}
			bool value2 = false;
			if (focusable.TryGetValueFromBag(bag, cc, ref value2))
			{
				ve.focusable = value2;
			}
			ve.tooltip = m_Tooltip.GetValueFromBag(bag, cc);
		}
	}

	public enum MeasureMode
	{
		Undefined,
		Exactly,
		AtMost
	}

	public struct Hierarchy
	{
		private readonly VisualElement m_Owner;

		public VisualElement parent => m_Owner.m_PhysicalParent;

		public int childCount => m_Owner.m_Children.Count;

		public VisualElement this[int key] => m_Owner.m_Children[key];

		internal Hierarchy(VisualElement element)
		{
			m_Owner = element;
		}

		public void Add(VisualElement child)
		{
			if (child == null)
			{
				throw new ArgumentException("Cannot add null child");
			}
			Insert(childCount, child);
		}

		public void Insert(int index, VisualElement child)
		{
			if (child == null)
			{
				throw new ArgumentException("Cannot insert null child");
			}
			if (index > childCount)
			{
				throw new ArgumentOutOfRangeException("Index out of range: " + index);
			}
			if (child == m_Owner)
			{
				throw new ArgumentException("Cannot insert element as its own child");
			}
			child.RemoveFromHierarchy();
			if (m_Owner.m_Children == s_EmptyList)
			{
				m_Owner.m_Children = VisualElementListPool.Get();
			}
			if (m_Owner.yogaNode.IsMeasureDefined)
			{
				m_Owner.RemoveMeasureFunction();
			}
			PutChildAtIndex(child, index);
			int num = child.imguiContainerDescendantCount + (child.isIMGUIContainer ? 1 : 0);
			if (num > 0)
			{
				m_Owner.ChangeIMGUIContainerCount(num);
			}
			child.hierarchy.SetParent(m_Owner);
			child.PropagateEnabledToChildren(m_Owner.enabledInHierarchy);
			child.InvokeHierarchyChanged(HierarchyChangeType.Add);
			child.IncrementVersion(VersionChangeType.Hierarchy);
			m_Owner.IncrementVersion(VersionChangeType.Hierarchy);
		}

		public void Remove(VisualElement child)
		{
			if (child == null)
			{
				throw new ArgumentException("Cannot remove null child");
			}
			if (child.hierarchy.parent != m_Owner)
			{
				throw new ArgumentException("This visualElement is not my child");
			}
			int index = m_Owner.m_Children.IndexOf(child);
			RemoveAt(index);
		}

		public void RemoveAt(int index)
		{
			if (index < 0 || index >= childCount)
			{
				throw new ArgumentOutOfRangeException("Index out of range: " + index);
			}
			VisualElement visualElement = m_Owner.m_Children[index];
			visualElement.InvokeHierarchyChanged(HierarchyChangeType.Remove);
			RemoveChildAtIndex(index);
			int num = visualElement.imguiContainerDescendantCount + (visualElement.isIMGUIContainer ? 1 : 0);
			if (num > 0)
			{
				m_Owner.ChangeIMGUIContainerCount(-num);
			}
			visualElement.hierarchy.SetParent(null);
			if (childCount == 0)
			{
				ReleaseChildList();
				m_Owner.AssignMeasureFunction();
			}
			m_Owner.elementPanel?.OnVersionChanged(visualElement, VersionChangeType.Hierarchy);
			m_Owner.IncrementVersion(VersionChangeType.Hierarchy);
		}

		public void Clear()
		{
			if (childCount <= 0)
			{
				return;
			}
			List<VisualElement> list = VisualElementListPool.Copy(m_Owner.m_Children);
			ReleaseChildList();
			m_Owner.yogaNode.Clear();
			m_Owner.AssignMeasureFunction();
			foreach (VisualElement item in list)
			{
				item.InvokeHierarchyChanged(HierarchyChangeType.Remove);
				item.hierarchy.SetParent(null);
				item.m_LogicalParent = null;
				m_Owner.elementPanel?.OnVersionChanged(item, VersionChangeType.Hierarchy);
			}
			if (m_Owner.imguiContainerDescendantCount > 0)
			{
				int num = m_Owner.imguiContainerDescendantCount;
				if (m_Owner.isIMGUIContainer)
				{
					num--;
				}
				m_Owner.ChangeIMGUIContainerCount(-num);
			}
			VisualElementListPool.Release(list);
			m_Owner.IncrementVersion(VersionChangeType.Hierarchy);
		}

		internal void BringToFront(VisualElement child)
		{
			if (childCount > 1)
			{
				int num = m_Owner.m_Children.IndexOf(child);
				if (num >= 0 && num < childCount - 1)
				{
					MoveChildElement(child, num, childCount);
				}
			}
		}

		internal void SendToBack(VisualElement child)
		{
			if (childCount > 1)
			{
				int num = m_Owner.m_Children.IndexOf(child);
				if (num > 0)
				{
					MoveChildElement(child, num, 0);
				}
			}
		}

		internal void PlaceBehind(VisualElement child, VisualElement over)
		{
			if (childCount <= 0)
			{
				return;
			}
			int num = m_Owner.m_Children.IndexOf(child);
			if (num >= 0)
			{
				int num2 = m_Owner.m_Children.IndexOf(over);
				if (num2 > 0 && num < num2)
				{
					num2--;
				}
				MoveChildElement(child, num, num2);
			}
		}

		internal void PlaceInFront(VisualElement child, VisualElement under)
		{
			if (childCount <= 0)
			{
				return;
			}
			int num = m_Owner.m_Children.IndexOf(child);
			if (num >= 0)
			{
				int num2 = m_Owner.m_Children.IndexOf(under);
				if (num > num2)
				{
					num2++;
				}
				MoveChildElement(child, num, num2);
			}
		}

		private void MoveChildElement(VisualElement child, int currentIndex, int nextIndex)
		{
			child.InvokeHierarchyChanged(HierarchyChangeType.Remove);
			RemoveChildAtIndex(currentIndex);
			PutChildAtIndex(child, nextIndex);
			child.InvokeHierarchyChanged(HierarchyChangeType.Add);
			m_Owner.IncrementVersion(VersionChangeType.Hierarchy);
		}

		public int IndexOf(VisualElement element)
		{
			return m_Owner.m_Children.IndexOf(element);
		}

		public VisualElement ElementAt(int index)
		{
			return this[index];
		}

		public IEnumerable<VisualElement> Children()
		{
			return m_Owner.m_Children;
		}

		private void SetParent(VisualElement value)
		{
			m_Owner.m_PhysicalParent = value;
			m_Owner.m_LogicalParent = value;
			if (value != null)
			{
				m_Owner.SetPanel(m_Owner.m_PhysicalParent.elementPanel);
			}
			else
			{
				m_Owner.SetPanel(null);
			}
		}

		public void Sort(Comparison<VisualElement> comp)
		{
			if (childCount > 0)
			{
				m_Owner.m_Children.Sort(comp);
				m_Owner.yogaNode.Clear();
				for (int i = 0; i < m_Owner.m_Children.Count; i++)
				{
					m_Owner.yogaNode.Insert(i, m_Owner.m_Children[i].yogaNode);
				}
				m_Owner.InvokeHierarchyChanged(HierarchyChangeType.Move);
				m_Owner.IncrementVersion(VersionChangeType.Hierarchy);
			}
		}

		private void PutChildAtIndex(VisualElement child, int index)
		{
			if (index >= childCount)
			{
				m_Owner.m_Children.Add(child);
				m_Owner.yogaNode.Insert(m_Owner.yogaNode.Count, child.yogaNode);
			}
			else
			{
				m_Owner.m_Children.Insert(index, child);
				m_Owner.yogaNode.Insert(index, child.yogaNode);
			}
		}

		private void RemoveChildAtIndex(int index)
		{
			m_Owner.m_Children.RemoveAt(index);
			m_Owner.yogaNode.RemoveAt(index);
		}

		private void ReleaseChildList()
		{
			if (m_Owner.m_Children != s_EmptyList)
			{
				List<VisualElement> children = m_Owner.m_Children;
				m_Owner.m_Children = s_EmptyList;
				VisualElementListPool.Release(children);
			}
		}

		public bool Equals(Hierarchy other)
		{
			return other == this;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			return obj is Hierarchy && Equals((Hierarchy)obj);
		}

		public override int GetHashCode()
		{
			return (m_Owner != null) ? m_Owner.GetHashCode() : 0;
		}

		public static bool operator ==(Hierarchy x, Hierarchy y)
		{
			return x.m_Owner == y.m_Owner;
		}

		public static bool operator !=(Hierarchy x, Hierarchy y)
		{
			return !(x == y);
		}
	}

	private abstract class BaseVisualElementScheduledItem : ScheduledItem, IVisualElementScheduledItem, IVisualElementPanelActivatable
	{
		public bool isScheduled = false;

		private VisualElementPanelActivator m_Activator;

		public VisualElement element { get; private set; }

		public bool isActive => m_Activator.isActive;

		protected BaseVisualElementScheduledItem(VisualElement handler)
		{
			element = handler;
			m_Activator = new VisualElementPanelActivator(this);
		}

		public IVisualElementScheduledItem StartingIn(long delayMs)
		{
			base.delayMs = delayMs;
			return this;
		}

		public IVisualElementScheduledItem Until(Func<bool> stopCondition)
		{
			if (stopCondition == null)
			{
				stopCondition = ScheduledItem.ForeverCondition;
			}
			timerUpdateStopCondition = stopCondition;
			return this;
		}

		public IVisualElementScheduledItem ForDuration(long durationMs)
		{
			SetDuration(durationMs);
			return this;
		}

		public IVisualElementScheduledItem Every(long intervalMs)
		{
			base.intervalMs = intervalMs;
			if (timerUpdateStopCondition == ScheduledItem.OnceCondition)
			{
				timerUpdateStopCondition = ScheduledItem.ForeverCondition;
			}
			return this;
		}

		internal override void OnItemUnscheduled()
		{
			base.OnItemUnscheduled();
			isScheduled = false;
			if (!m_Activator.isDetaching)
			{
				m_Activator.SetActive(action: false);
			}
		}

		public void Resume()
		{
			m_Activator.SetActive(action: true);
		}

		public void Pause()
		{
			m_Activator.SetActive(action: false);
		}

		public void ExecuteLater(long delayMs)
		{
			if (!isScheduled)
			{
				Resume();
			}
			ResetStartTime();
			StartingIn(delayMs);
		}

		public void OnPanelActivate()
		{
			if (!isScheduled)
			{
				isScheduled = true;
				ResetStartTime();
				element.elementPanel.scheduler.Schedule(this);
			}
		}

		public void OnPanelDeactivate()
		{
			if (isScheduled)
			{
				isScheduled = false;
				element.elementPanel.scheduler.Unschedule(this);
			}
		}

		public bool CanBeActivated()
		{
			return element != null && element.elementPanel != null && element.elementPanel.scheduler != null;
		}
	}

	private abstract class VisualElementScheduledItem<ActionType> : BaseVisualElementScheduledItem
	{
		public ActionType updateEvent;

		public VisualElementScheduledItem(VisualElement handler, ActionType upEvent)
			: base(handler)
		{
			updateEvent = upEvent;
		}

		public static bool Matches(ScheduledItem item, ActionType updateEvent)
		{
			if (item is VisualElementScheduledItem<ActionType> visualElementScheduledItem)
			{
				return EqualityComparer<ActionType>.Default.Equals(visualElementScheduledItem.updateEvent, updateEvent);
			}
			return false;
		}
	}

	private class TimerStateScheduledItem : VisualElementScheduledItem<Action<TimerState>>
	{
		public TimerStateScheduledItem(VisualElement handler, Action<TimerState> updateEvent)
			: base(handler, updateEvent)
		{
		}

		public override void PerformTimerUpdate(TimerState state)
		{
			if (isScheduled)
			{
				updateEvent(state);
			}
		}
	}

	private class SimpleScheduledItem : VisualElementScheduledItem<Action>
	{
		public SimpleScheduledItem(VisualElement handler, Action updateEvent)
			: base(handler, updateEvent)
		{
		}

		public override void PerformTimerUpdate(TimerState state)
		{
			if (isScheduled)
			{
				updateEvent();
			}
		}
	}

	private static uint s_NextId;

	private static List<string> s_EmptyClassList = new List<string>(0);

	internal static readonly PropertyName userDataPropertyKey = new PropertyName("--unity-user-data");

	public static readonly string disabledUssClassName = "unity-disabled";

	private string m_Name;

	private List<string> m_ClassList;

	private string m_TypeName;

	private string m_FullTypeName;

	private List<KeyValuePair<PropertyName, object>> m_PropertyBag;

	private string m_ViewDataKey;

	private RenderHints m_RenderHints;

	internal Rect lastLayout;

	internal RenderChainVEData renderChainData;

	private Vector3 m_Position = Vector3.zero;

	private Quaternion m_Rotation = Quaternion.identity;

	private Vector3 m_Scale = Vector3.one;

	private Rect m_Layout;

	internal bool isBoundingBoxDirty = true;

	private Rect m_BoundingBox;

	internal bool isWorldBoundingBoxDirty = true;

	private Rect m_WorldBoundingBox;

	private Matrix4x4 m_WorldTransformCache = Matrix4x4.identity;

	private Matrix4x4 m_WorldTransformInverseCache = Matrix4x4.identity;

	private Rect m_WorldClip = Rect.zero;

	private Rect m_WorldClipMinusGroup = Rect.zero;

	private static readonly Rect s_InfiniteRect = new Rect(-10000f, -10000f, 40000f, 40000f);

	internal PseudoStates triggerPseudoMask;

	internal PseudoStates dependencyPseudoMask;

	private PseudoStates m_PseudoStates;

	internal VisualElementStylesData m_SharedStyle = VisualElementStylesData.none;

	internal VisualElementStylesData m_Style = VisualElementStylesData.none;

	internal StyleVariableContext variableContext = StyleVariableContext.none;

	internal InheritedStylesData propagatedStyle = InheritedStylesData.none;

	private InheritedStylesData m_InheritedStylesData = InheritedStylesData.none;

	internal readonly uint controlid;

	internal int imguiContainerDescendantCount = 0;

	private bool m_RequireMeasureFunction = false;

	private List<IValueAnimationUpdate> m_RunningAnimations;

	private VisualElement m_PhysicalParent;

	private VisualElement m_LogicalParent;

	private static readonly List<VisualElement> s_EmptyList = new List<VisualElement>();

	private List<VisualElement> m_Children;

	private InlineStyleAccess m_InlineStyleAccess;

	internal List<StyleSheet> styleSheetList;

	internal bool isCompositeRoot { get; set; }

	public string viewDataKey
	{
		get
		{
			return m_ViewDataKey;
		}
		set
		{
			if (m_ViewDataKey != value)
			{
				m_ViewDataKey = value;
				if (!string.IsNullOrEmpty(value))
				{
					IncrementVersion(VersionChangeType.ViewData);
				}
			}
		}
	}

	internal bool enableViewDataPersistence { get; private set; }

	public object userData
	{
		get
		{
			return GetPropertyInternal(userDataPropertyKey);
		}
		set
		{
			SetPropertyInternal(userDataPropertyKey, value);
		}
	}

	public override bool canGrabFocus
	{
		get
		{
			bool flag = false;
			for (VisualElement visualElement = hierarchy.parent; visualElement != null; visualElement = visualElement.parent)
			{
				if (visualElement.isCompositeRoot)
				{
					flag |= !visualElement.canGrabFocus;
					break;
				}
			}
			return !flag && visible && resolvedStyle.display != DisplayStyle.None && enabledInHierarchy && base.canGrabFocus;
		}
	}

	public override FocusController focusController => panel?.focusController;

	public UsageHints usageHints
	{
		get
		{
			return (UsageHints)((((m_RenderHints & RenderHints.GroupTransform) != RenderHints.None) ? 2 : 0) | (((m_RenderHints & RenderHints.BoneTransform) != RenderHints.None) ? 1 : 0));
		}
		set
		{
			if (panel != null)
			{
				throw new InvalidOperationException("usageHints cannot be changed once the VisualElement is part of an active visual tree");
			}
			if ((value & UsageHints.GroupTransform) != UsageHints.None)
			{
				m_RenderHints |= RenderHints.GroupTransform;
			}
			else
			{
				m_RenderHints &= ~RenderHints.GroupTransform;
			}
			if ((value & UsageHints.DynamicTransform) != UsageHints.None)
			{
				m_RenderHints |= RenderHints.BoneTransform;
			}
			else
			{
				m_RenderHints &= ~RenderHints.BoneTransform;
			}
		}
	}

	internal RenderHints renderHints
	{
		get
		{
			return m_RenderHints;
		}
		set
		{
			if (panel != null)
			{
				throw new InvalidOperationException("renderHints cannot be changed once the VisualElement is part of an active visual tree");
			}
			m_RenderHints = value;
		}
	}

	public ITransform transform => this;

	Vector3 ITransform.position
	{
		get
		{
			return m_Position;
		}
		set
		{
			if (!(m_Position == value))
			{
				m_Position = value;
				IncrementVersion(VersionChangeType.Transform);
			}
		}
	}

	Quaternion ITransform.rotation
	{
		get
		{
			return m_Rotation;
		}
		set
		{
			if (!(m_Rotation == value))
			{
				m_Rotation = value;
				IncrementVersion(VersionChangeType.Transform);
			}
		}
	}

	Vector3 ITransform.scale
	{
		get
		{
			return m_Scale;
		}
		set
		{
			if (!(m_Scale == value))
			{
				m_Scale = value;
				IncrementVersion(VersionChangeType.Layout | VersionChangeType.Transform);
			}
		}
	}

	Matrix4x4 ITransform.matrix => Matrix4x4.TRS(m_Position, m_Rotation, m_Scale);

	internal bool isLayoutManual { get; private set; }

	internal float scaledPixelsPerPoint => (panel == null) ? GUIUtility.pixelsPerPoint : (panel as BaseVisualElementPanel).scaledPixelsPerPoint;

	public Rect layout
	{
		get
		{
			Rect result = m_Layout;
			if (yogaNode != null && !isLayoutManual)
			{
				result.x = yogaNode.LayoutX;
				result.y = yogaNode.LayoutY;
				result.width = yogaNode.LayoutWidth;
				result.height = yogaNode.LayoutHeight;
			}
			return result;
		}
		internal set
		{
			if (yogaNode == null)
			{
				yogaNode = new YogaNode();
			}
			if (!isLayoutManual || !(m_Layout == value))
			{
				Rect rect = layout;
				VersionChangeType versionChangeType = (VersionChangeType)0;
				if (!Mathf.Approximately(rect.x, value.x) || !Mathf.Approximately(rect.y, value.y))
				{
					versionChangeType |= VersionChangeType.Transform;
				}
				if (!Mathf.Approximately(rect.width, value.width) || !Mathf.Approximately(rect.height, value.height))
				{
					versionChangeType |= VersionChangeType.Size;
				}
				m_Layout = value;
				isLayoutManual = true;
				IStyle style = this.style;
				style.position = Position.Absolute;
				style.marginLeft = 0f;
				style.marginRight = 0f;
				style.marginBottom = 0f;
				style.marginTop = 0f;
				style.left = value.x;
				style.top = value.y;
				style.right = float.NaN;
				style.bottom = float.NaN;
				style.width = value.width;
				style.height = value.height;
				if (versionChangeType != 0)
				{
					IncrementVersion(versionChangeType);
				}
			}
		}
	}

	public Rect contentRect
	{
		get
		{
			Spacing spacing = new Spacing(resolvedStyle.paddingLeft, resolvedStyle.paddingTop, resolvedStyle.paddingRight, resolvedStyle.paddingBottom);
			return paddingRect - spacing;
		}
	}

	protected Rect paddingRect
	{
		get
		{
			Spacing spacing = new Spacing(resolvedStyle.borderLeftWidth, resolvedStyle.borderTopWidth, resolvedStyle.borderRightWidth, resolvedStyle.borderBottomWidth);
			return rect - spacing;
		}
	}

	internal Rect boundingBox
	{
		get
		{
			if (isBoundingBoxDirty)
			{
				UpdateBoundingBox();
				isBoundingBoxDirty = false;
			}
			return m_BoundingBox;
		}
	}

	internal Rect worldBoundingBox
	{
		get
		{
			if (isWorldBoundingBoxDirty || isBoundingBoxDirty)
			{
				UpdateWorldBoundingBox();
				isWorldBoundingBoxDirty = false;
			}
			return m_WorldBoundingBox;
		}
	}

	public Rect worldBound
	{
		get
		{
			Matrix4x4 lhc = worldTransform;
			return TransformAlignedRect(lhc, rect);
		}
	}

	public Rect localBound
	{
		get
		{
			Matrix4x4 matrix = transform.matrix;
			Rect rect = layout;
			return TransformAlignedRect(matrix, rect);
		}
	}

	internal Rect rect => new Rect(0f, 0f, layout.width, layout.height);

	internal bool isWorldTransformDirty { get; set; } = true;

	internal bool isWorldTransformInverseDirty { get; set; } = true;

	public Matrix4x4 worldTransform
	{
		get
		{
			if (isWorldTransformDirty)
			{
				UpdateWorldTransform();
			}
			return m_WorldTransformCache;
		}
	}

	internal Matrix4x4 worldTransformInverse
	{
		get
		{
			if (isWorldTransformDirty || isWorldTransformInverseDirty)
			{
				m_WorldTransformInverseCache = worldTransform.inverse;
				isWorldTransformInverseDirty = false;
			}
			return m_WorldTransformInverseCache;
		}
	}

	internal bool isWorldClipDirty { get; set; } = true;

	internal Rect worldClip
	{
		get
		{
			if (isWorldClipDirty)
			{
				UpdateWorldClip();
				isWorldClipDirty = false;
			}
			return m_WorldClip;
		}
	}

	internal Rect worldClipMinusGroup
	{
		get
		{
			if (isWorldClipDirty)
			{
				UpdateWorldClip();
				isWorldClipDirty = false;
			}
			return m_WorldClipMinusGroup;
		}
	}

	internal PseudoStates pseudoStates
	{
		get
		{
			return m_PseudoStates;
		}
		set
		{
			if (m_PseudoStates != value)
			{
				m_PseudoStates = value;
				if ((triggerPseudoMask & m_PseudoStates) != 0 || (dependencyPseudoMask & ~m_PseudoStates) != 0)
				{
					IncrementVersion(VersionChangeType.StyleSheet);
				}
			}
		}
	}

	public PickingMode pickingMode { get; set; }

	public string name
	{
		get
		{
			return m_Name;
		}
		set
		{
			if (!(m_Name == value))
			{
				m_Name = value;
				IncrementVersion(VersionChangeType.StyleSheet);
			}
		}
	}

	internal List<string> classList => m_ClassList;

	internal string fullTypeName
	{
		get
		{
			if (string.IsNullOrEmpty(m_FullTypeName))
			{
				m_FullTypeName = GetType().FullName;
			}
			return m_FullTypeName;
		}
	}

	internal string typeName
	{
		get
		{
			if (string.IsNullOrEmpty(m_TypeName))
			{
				Type type = GetType();
				bool isGenericType = type.IsGenericType;
				m_TypeName = type.Name;
				if (isGenericType)
				{
					int num = m_TypeName.IndexOf('`');
					if (num >= 0)
					{
						m_TypeName = m_TypeName.Remove(num);
					}
				}
			}
			return m_TypeName;
		}
	}

	internal YogaNode yogaNode { get; private set; }

	internal VisualElementStylesData sharedStyle => m_SharedStyle;

	internal VisualElementStylesData specifiedStyle => m_Style;

	internal InheritedStylesData inheritedStyle
	{
		get
		{
			return m_InheritedStylesData;
		}
		set
		{
			if (m_InheritedStylesData != value)
			{
				m_InheritedStylesData = value;
				IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.Repaint);
			}
		}
	}

	internal bool hasInlineStyle => m_Style != m_SharedStyle;

	internal ComputedStyle computedStyle { get; private set; }

	internal float opacity
	{
		get
		{
			return resolvedStyle.opacity;
		}
		set
		{
			style.opacity = value;
		}
	}

	public bool enabledInHierarchy => (pseudoStates & PseudoStates.Disabled) != PseudoStates.Disabled;

	public bool enabledSelf { get; private set; }

	public bool visible
	{
		get
		{
			return resolvedStyle.visibility == Visibility.Visible;
		}
		set
		{
			style.visibility = ((!value) ? Visibility.Hidden : Visibility.Visible);
		}
	}

	public Action<MeshGenerationContext> generateVisualContent { get; set; }

	internal bool requireMeasureFunction
	{
		get
		{
			return m_RequireMeasureFunction;
		}
		set
		{
			m_RequireMeasureFunction = value;
			if (m_RequireMeasureFunction && !yogaNode.IsMeasureDefined)
			{
				AssignMeasureFunction();
			}
			else if (!m_RequireMeasureFunction && yogaNode.IsMeasureDefined)
			{
				RemoveMeasureFunction();
			}
		}
	}

	public IExperimentalFeatures experimental => this;

	ITransitionAnimations IExperimentalFeatures.animation => this;

	public Hierarchy hierarchy { get; private set; }

	[Obsolete("VisualElement.cacheAsBitmap is deprecated and has no effect")]
	public bool cacheAsBitmap { get; set; }

	public VisualElement parent => m_LogicalParent;

	internal BaseVisualElementPanel elementPanel { get; private set; }

	public IPanel panel => elementPanel;

	public virtual VisualElement contentContainer => this;

	public VisualElement this[int key]
	{
		get
		{
			if (contentContainer == this)
			{
				return hierarchy[key];
			}
			return contentContainer?[key];
		}
	}

	public int childCount
	{
		get
		{
			if (contentContainer == this)
			{
				return hierarchy.childCount;
			}
			return contentContainer?.childCount ?? 0;
		}
	}

	public IVisualElementScheduler schedule => this;

	public IStyle style
	{
		get
		{
			if (m_InlineStyleAccess == null)
			{
				m_InlineStyleAccess = new InlineStyleAccess(this);
			}
			return m_InlineStyleAccess;
		}
	}

	public ICustomStyle customStyle => specifiedStyle;

	public IResolvedStyle resolvedStyle => this;

	float IResolvedStyle.width => yogaNode.LayoutWidth;

	float IResolvedStyle.height => yogaNode.LayoutHeight;

	StyleFloat IResolvedStyle.maxWidth => ResolveLengthValue(computedStyle.maxWidth, isRow: true);

	StyleFloat IResolvedStyle.maxHeight => ResolveLengthValue(computedStyle.maxHeight, isRow: false);

	StyleFloat IResolvedStyle.minWidth => ResolveLengthValue(computedStyle.minWidth, isRow: true);

	StyleFloat IResolvedStyle.minHeight => ResolveLengthValue(computedStyle.minHeight, isRow: false);

	StyleFloat IResolvedStyle.flexBasis => new StyleFloat(yogaNode.ComputedFlexBasis);

	float IResolvedStyle.flexGrow => computedStyle.flexGrow.value;

	float IResolvedStyle.flexShrink => computedStyle.flexShrink.value;

	FlexDirection IResolvedStyle.flexDirection => computedStyle.flexDirection.value;

	Wrap IResolvedStyle.flexWrap => computedStyle.flexWrap.value;

	float IResolvedStyle.left => yogaNode.LayoutX;

	float IResolvedStyle.top => yogaNode.LayoutY;

	float IResolvedStyle.right => yogaNode.LayoutRight;

	float IResolvedStyle.bottom => yogaNode.LayoutBottom;

	float IResolvedStyle.marginLeft => yogaNode.LayoutMarginLeft;

	float IResolvedStyle.marginTop => yogaNode.LayoutMarginTop;

	float IResolvedStyle.marginRight => yogaNode.LayoutMarginRight;

	float IResolvedStyle.marginBottom => yogaNode.LayoutMarginBottom;

	float IResolvedStyle.paddingLeft => yogaNode.LayoutPaddingLeft;

	float IResolvedStyle.paddingTop => yogaNode.LayoutPaddingTop;

	float IResolvedStyle.paddingRight => yogaNode.LayoutPaddingRight;

	float IResolvedStyle.paddingBottom => yogaNode.LayoutPaddingBottom;

	Position IResolvedStyle.position => computedStyle.position.value;

	Align IResolvedStyle.alignSelf => computedStyle.alignSelf.value;

	TextAnchor IResolvedStyle.unityTextAlign => computedStyle.unityTextAlign.value;

	FontStyle IResolvedStyle.unityFontStyleAndWeight => computedStyle.unityFontStyleAndWeight.value;

	float IResolvedStyle.fontSize => computedStyle.fontSize.value.value;

	WhiteSpace IResolvedStyle.whiteSpace => computedStyle.whiteSpace.value;

	Color IResolvedStyle.color => computedStyle.color.value;

	Color IResolvedStyle.backgroundColor => computedStyle.backgroundColor.value;

	Color IResolvedStyle.borderColor => computedStyle.borderLeftColor.value;

	Font IResolvedStyle.unityFont => computedStyle.unityFont.value;

	ScaleMode IResolvedStyle.unityBackgroundScaleMode => computedStyle.unityBackgroundScaleMode.value;

	Color IResolvedStyle.unityBackgroundImageTintColor => computedStyle.unityBackgroundImageTintColor.value;

	Align IResolvedStyle.alignItems => computedStyle.alignItems.value;

	Align IResolvedStyle.alignContent => computedStyle.alignContent.value;

	Justify IResolvedStyle.justifyContent => computedStyle.justifyContent.value;

	Color IResolvedStyle.borderLeftColor => computedStyle.borderLeftColor.value;

	Color IResolvedStyle.borderRightColor => computedStyle.borderRightColor.value;

	Color IResolvedStyle.borderTopColor => computedStyle.borderTopColor.value;

	Color IResolvedStyle.borderBottomColor => computedStyle.borderBottomColor.value;

	float IResolvedStyle.borderLeftWidth => computedStyle.borderLeftWidth.value;

	float IResolvedStyle.borderRightWidth => computedStyle.borderRightWidth.value;

	float IResolvedStyle.borderTopWidth => computedStyle.borderTopWidth.value;

	float IResolvedStyle.borderBottomWidth => computedStyle.borderBottomWidth.value;

	float IResolvedStyle.borderTopLeftRadius => computedStyle.borderTopLeftRadius.value.value;

	float IResolvedStyle.borderTopRightRadius => computedStyle.borderTopRightRadius.value.value;

	float IResolvedStyle.borderBottomLeftRadius => computedStyle.borderBottomLeftRadius.value.value;

	float IResolvedStyle.borderBottomRightRadius => computedStyle.borderBottomRightRadius.value.value;

	int IResolvedStyle.unitySliceLeft => computedStyle.unitySliceLeft.value;

	int IResolvedStyle.unitySliceTop => computedStyle.unitySliceTop.value;

	int IResolvedStyle.unitySliceRight => computedStyle.unitySliceRight.value;

	int IResolvedStyle.unitySliceBottom => computedStyle.unitySliceBottom.value;

	float IResolvedStyle.opacity => computedStyle.opacity.value;

	Visibility IResolvedStyle.visibility => computedStyle.visibility.value;

	DisplayStyle IResolvedStyle.display => computedStyle.display.value;

	public VisualElementStyleSheetSet styleSheets => new VisualElementStyleSheetSet(this);

	public string tooltip
	{
		get
		{
			TryGetUserArgs<TooltipEvent, string>(OnTooltip, TrickleDown.NoTrickleDown, out var text);
			return text ?? string.Empty;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				UnregisterCallback<TooltipEvent, string>(OnTooltip);
			}
			else
			{
				RegisterCallback<TooltipEvent, string>(OnTooltip, value);
			}
		}
	}

	internal Vector3 ComputeGlobalScale()
	{
		Vector3 scale = m_Scale;
		for (VisualElement visualElement = hierarchy.parent; visualElement != null; visualElement = visualElement.hierarchy.parent)
		{
			scale.Scale(visualElement.m_Scale);
		}
		return scale;
	}

	internal static Rect TransformAlignedRect(Matrix4x4 lhc, Rect rect)
	{
		Vector2 vector = MultiplyMatrix44Point2(lhc, rect.min);
		Vector2 vector2 = MultiplyMatrix44Point2(lhc, rect.max);
		return Rect.MinMaxRect(Math.Min(vector.x, vector2.x), Math.Min(vector.y, vector2.y), Math.Max(vector.x, vector2.x), Math.Max(vector.y, vector2.y));
	}

	internal static Vector2 MultiplyMatrix44Point2(Matrix4x4 lhs, Vector2 point)
	{
		Vector2 result = default(Vector2);
		result.x = lhs.m00 * point.x + lhs.m01 * point.y + lhs.m03;
		result.y = lhs.m10 * point.x + lhs.m11 * point.y + lhs.m13;
		return result;
	}

	internal void UpdateBoundingBox()
	{
		if (float.IsNaN(this.rect.x) || float.IsNaN(this.rect.y) || float.IsNaN(this.rect.width) || float.IsNaN(this.rect.height))
		{
			m_BoundingBox = Rect.zero;
		}
		else
		{
			m_BoundingBox = this.rect;
			int count = m_Children.Count;
			for (int i = 0; i < count; i++)
			{
				Rect rect = m_Children[i].boundingBox;
				rect = m_Children[i].ChangeCoordinatesTo(this, rect);
				m_BoundingBox.xMin = Math.Min(m_BoundingBox.xMin, rect.xMin);
				m_BoundingBox.xMax = Math.Max(m_BoundingBox.xMax, rect.xMax);
				m_BoundingBox.yMin = Math.Min(m_BoundingBox.yMin, rect.yMin);
				m_BoundingBox.yMax = Math.Max(m_BoundingBox.yMax, rect.yMax);
			}
		}
		isWorldBoundingBoxDirty = true;
	}

	internal void UpdateWorldBoundingBox()
	{
		m_WorldBoundingBox = TransformAlignedRect(worldTransform, boundingBox);
	}

	private void UpdateWorldTransform()
	{
		if (elementPanel != null && !elementPanel.duringLayoutPhase)
		{
			isWorldTransformDirty = false;
		}
		Matrix4x4 matrix4x = Matrix4x4.Translate(new Vector3(layout.x, layout.y, 0f));
		if (hierarchy.parent != null)
		{
			m_WorldTransformCache = hierarchy.parent.worldTransform * matrix4x * transform.matrix;
		}
		else
		{
			m_WorldTransformCache = matrix4x * transform.matrix;
		}
		isWorldTransformInverseDirty = true;
		isWorldBoundingBoxDirty = true;
	}

	private void UpdateWorldClip()
	{
		if (hierarchy.parent != null)
		{
			m_WorldClip = hierarchy.parent.worldClip;
			if (hierarchy.parent != renderChainData.groupTransformAncestor)
			{
				m_WorldClipMinusGroup = hierarchy.parent.worldClipMinusGroup;
			}
			else
			{
				IPanel obj = panel;
				m_WorldClipMinusGroup = ((obj != null && obj.contextType == ContextType.Player) ? s_InfiniteRect : GUIClip.topmostRect);
			}
			if (ShouldClip())
			{
				Rect rect = SubstractBorderPadding(worldBound);
				float num = Mathf.Max(rect.xMin, m_WorldClip.xMin);
				float num2 = Mathf.Min(rect.xMax, m_WorldClip.xMax);
				float num3 = Mathf.Max(rect.yMin, m_WorldClip.yMin);
				float num4 = Mathf.Min(rect.yMax, m_WorldClip.yMax);
				float width = Mathf.Max(num2 - num, 0f);
				float height = Mathf.Max(num4 - num3, 0f);
				m_WorldClip = new Rect(num, num3, width, height);
				num = Mathf.Max(rect.xMin, m_WorldClipMinusGroup.xMin);
				num2 = Mathf.Min(rect.xMax, m_WorldClipMinusGroup.xMax);
				num3 = Mathf.Max(rect.yMin, m_WorldClipMinusGroup.yMin);
				num4 = Mathf.Min(rect.yMax, m_WorldClipMinusGroup.yMax);
				width = Mathf.Max(num2 - num, 0f);
				height = Mathf.Max(num4 - num3, 0f);
				m_WorldClipMinusGroup = new Rect(num, num3, width, height);
			}
		}
		else
		{
			m_WorldClipMinusGroup = (m_WorldClip = ((panel != null) ? panel.visualTree.rect : s_InfiniteRect));
		}
	}

	private Rect SubstractBorderPadding(Rect worldRect)
	{
		float m = worldTransform.m00;
		float m2 = worldTransform.m11;
		worldRect.x += resolvedStyle.borderLeftWidth * m;
		worldRect.y += resolvedStyle.borderTopWidth * m2;
		worldRect.width -= (resolvedStyle.borderLeftWidth + resolvedStyle.borderRightWidth) * m;
		worldRect.height -= (resolvedStyle.borderTopWidth + resolvedStyle.borderBottomWidth) * m2;
		if (computedStyle.unityOverflowClipBox == OverflowClipBox.ContentBox)
		{
			worldRect.x += resolvedStyle.paddingLeft * m;
			worldRect.y += resolvedStyle.paddingTop * m2;
			worldRect.width -= (resolvedStyle.paddingLeft + resolvedStyle.paddingRight) * m;
			worldRect.height -= (resolvedStyle.paddingTop + resolvedStyle.paddingBottom) * m2;
		}
		return worldRect;
	}

	internal static Rect ComputeAAAlignedBound(Rect position, Matrix4x4 mat)
	{
		Rect rect = position;
		Vector3 vector = mat.MultiplyPoint3x4(new Vector3(rect.x, rect.y, 0f));
		Vector3 vector2 = mat.MultiplyPoint3x4(new Vector3(rect.x + rect.width, rect.y, 0f));
		Vector3 vector3 = mat.MultiplyPoint3x4(new Vector3(rect.x, rect.y + rect.height, 0f));
		Vector3 vector4 = mat.MultiplyPoint3x4(new Vector3(rect.x + rect.width, rect.y + rect.height, 0f));
		return Rect.MinMaxRect(Mathf.Min(vector.x, Mathf.Min(vector2.x, Mathf.Min(vector3.x, vector4.x))), Mathf.Min(vector.y, Mathf.Min(vector2.y, Mathf.Min(vector3.y, vector4.y))), Mathf.Max(vector.x, Mathf.Max(vector2.x, Mathf.Max(vector3.x, vector4.x))), Mathf.Max(vector.y, Mathf.Max(vector2.y, Mathf.Max(vector3.y, vector4.y))));
	}

	private void ChangeIMGUIContainerCount(int delta)
	{
		for (VisualElement visualElement = this; visualElement != null; visualElement = visualElement.hierarchy.parent)
		{
			visualElement.imguiContainerDescendantCount += delta;
		}
	}

	public VisualElement()
	{
		m_Children = s_EmptyList;
		controlid = ++s_NextId;
		hierarchy = new Hierarchy(this);
		computedStyle = new ComputedStyle(this);
		m_ClassList = s_EmptyClassList;
		m_FullTypeName = string.Empty;
		m_TypeName = string.Empty;
		SetEnabled(value: true);
		base.focusable = false;
		name = string.Empty;
		yogaNode = new YogaNode();
		renderHints = RenderHints.None;
	}

	protected override void ExecuteDefaultAction(EventBase evt)
	{
		base.ExecuteDefaultAction(evt);
		if (evt != null)
		{
			if (evt.eventTypeId == EventBase<MouseOverEvent>.TypeId() || evt.eventTypeId == EventBase<MouseOutEvent>.TypeId())
			{
				UpdateCursorStyle(evt.eventTypeId);
			}
			else if (evt.eventTypeId == EventBase<MouseEnterEvent>.TypeId())
			{
				pseudoStates |= PseudoStates.Hover;
			}
			else if (evt.eventTypeId == EventBase<MouseLeaveEvent>.TypeId())
			{
				pseudoStates &= ~PseudoStates.Hover;
			}
			else if (evt.eventTypeId == EventBase<BlurEvent>.TypeId())
			{
				pseudoStates &= ~PseudoStates.Focus;
			}
			else if (evt.eventTypeId == EventBase<FocusEvent>.TypeId())
			{
				pseudoStates |= PseudoStates.Focus;
			}
		}
	}

	public sealed override void Focus()
	{
		if (!canGrabFocus && hierarchy.parent != null)
		{
			hierarchy.parent.Focus();
		}
		else
		{
			base.Focus();
		}
	}

	internal void SetPanel(BaseVisualElementPanel p)
	{
		if (panel == p)
		{
			return;
		}
		List<VisualElement> list = VisualElementListPool.Get();
		try
		{
			list.Add(this);
			GatherAllChildren(list);
			EventDispatcherGate? eventDispatcherGate = null;
			if (p?.dispatcher != null)
			{
				eventDispatcherGate = new EventDispatcherGate(p.dispatcher);
			}
			EventDispatcherGate? eventDispatcherGate2 = null;
			if (panel?.dispatcher != null && panel.dispatcher != p?.dispatcher)
			{
				eventDispatcherGate2 = new EventDispatcherGate(panel.dispatcher);
			}
			using (eventDispatcherGate)
			{
				using (eventDispatcherGate2)
				{
					foreach (VisualElement item in list)
					{
						item.ChangePanel(p);
					}
				}
			}
		}
		finally
		{
			VisualElementListPool.Release(list);
		}
	}

	private void ChangePanel(BaseVisualElementPanel p)
	{
		if (panel == p)
		{
			return;
		}
		if (panel != null)
		{
			using (DetachFromPanelEvent detachFromPanelEvent = PanelChangedEventBase<DetachFromPanelEvent>.GetPooled(panel, p))
			{
				detachFromPanelEvent.target = this;
				elementPanel.SendEvent(detachFromPanelEvent, DispatchMode.Immediate);
			}
			UnregisterRunningAnimations();
		}
		IPanel originPanel = panel;
		elementPanel = p;
		if (panel != null)
		{
			RegisterRunningAnimations();
			using AttachToPanelEvent attachToPanelEvent = PanelChangedEventBase<AttachToPanelEvent>.GetPooled(originPanel, p);
			attachToPanelEvent.target = this;
			elementPanel.SendEvent(attachToPanelEvent);
		}
		IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Transform);
		if (!string.IsNullOrEmpty(viewDataKey))
		{
			IncrementVersion(VersionChangeType.ViewData);
		}
	}

	public sealed override void SendEvent(EventBase e)
	{
		elementPanel?.SendEvent(e);
	}

	internal void IncrementVersion(VersionChangeType changeType)
	{
		elementPanel?.OnVersionChanged(this, changeType);
	}

	internal void InvokeHierarchyChanged(HierarchyChangeType changeType)
	{
		elementPanel?.InvokeHierarchyChanged(this, changeType);
	}

	protected internal bool SetEnabledFromHierarchy(bool state)
	{
		if (state == ((pseudoStates & PseudoStates.Disabled) != PseudoStates.Disabled))
		{
			return false;
		}
		if (state && enabledSelf && (parent == null || parent.enabledInHierarchy))
		{
			pseudoStates &= ~PseudoStates.Disabled;
		}
		else
		{
			pseudoStates |= PseudoStates.Disabled;
		}
		return true;
	}

	public void SetEnabled(bool value)
	{
		if (enabledSelf != value)
		{
			enabledSelf = value;
			if (value)
			{
				RemoveFromClassList(disabledUssClassName);
			}
			else
			{
				AddToClassList(disabledUssClassName);
			}
			PropagateEnabledToChildren(value);
		}
	}

	private void PropagateEnabledToChildren(bool value)
	{
		if (SetEnabledFromHierarchy(value))
		{
			int count = m_Children.Count;
			for (int i = 0; i < count; i++)
			{
				m_Children[i].PropagateEnabledToChildren(value);
			}
		}
	}

	public void MarkDirtyRepaint()
	{
		IncrementVersion(VersionChangeType.Repaint);
	}

	internal void InvokeGenerateVisualContent(MeshGenerationContext mgc)
	{
		if (generateVisualContent != null)
		{
			try
			{
				generateVisualContent(mgc);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
	}

	internal void GetFullHierarchicalViewDataKey(StringBuilder key)
	{
		if (parent != null)
		{
			parent.GetFullHierarchicalViewDataKey(key);
		}
		if (!string.IsNullOrEmpty(viewDataKey))
		{
			key.Append("__");
			key.Append(viewDataKey);
		}
	}

	internal string GetFullHierarchicalViewDataKey()
	{
		StringBuilder stringBuilder = new StringBuilder();
		GetFullHierarchicalViewDataKey(stringBuilder);
		return stringBuilder.ToString();
	}

	internal T GetOrCreateViewData<T>(object existing, string key) where T : class, new()
	{
		Debug.Assert(elementPanel != null, "VisualElement.elementPanel is null! Cannot load persistent data.");
		ISerializableJsonDictionary serializableJsonDictionary = ((elementPanel == null || elementPanel.getViewDataDictionary == null) ? null : elementPanel.getViewDataDictionary());
		if (serializableJsonDictionary == null || string.IsNullOrEmpty(viewDataKey) || !enableViewDataPersistence)
		{
			if (existing != null)
			{
				return existing as T;
			}
			return new T();
		}
		string key2 = key + "__" + typeof(T).ToString();
		if (!serializableJsonDictionary.ContainsKey(key2))
		{
			serializableJsonDictionary.Set(key2, new T());
		}
		return serializableJsonDictionary.Get<T>(key2);
	}

	internal T GetOrCreateViewData<T>(ScriptableObject existing, string key) where T : ScriptableObject
	{
		Debug.Assert(elementPanel != null, "VisualElement.elementPanel is null! Cannot load view data.");
		ISerializableJsonDictionary serializableJsonDictionary = ((elementPanel == null || elementPanel.getViewDataDictionary == null) ? null : elementPanel.getViewDataDictionary());
		if (serializableJsonDictionary == null || string.IsNullOrEmpty(viewDataKey) || !enableViewDataPersistence)
		{
			if (existing != null)
			{
				return existing as T;
			}
			return ScriptableObject.CreateInstance<T>();
		}
		string key2 = key + "__" + typeof(T).ToString();
		if (!serializableJsonDictionary.ContainsKey(key2))
		{
			serializableJsonDictionary.Set(key2, ScriptableObject.CreateInstance<T>());
		}
		return serializableJsonDictionary.GetScriptable<T>(key2);
	}

	internal void OverwriteFromViewData(object obj, string key)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		Debug.Assert(elementPanel != null, "VisualElement.elementPanel is null! Cannot load view data.");
		ISerializableJsonDictionary serializableJsonDictionary = ((elementPanel == null || elementPanel.getViewDataDictionary == null) ? null : elementPanel.getViewDataDictionary());
		if (serializableJsonDictionary != null && !string.IsNullOrEmpty(viewDataKey) && enableViewDataPersistence)
		{
			string key2 = key + "__" + obj.GetType();
			if (!serializableJsonDictionary.ContainsKey(key2))
			{
				serializableJsonDictionary.Set(key2, obj);
			}
			else
			{
				serializableJsonDictionary.Overwrite(obj, key2);
			}
		}
	}

	internal void SaveViewData()
	{
		if (elementPanel != null && elementPanel.saveViewData != null && !string.IsNullOrEmpty(viewDataKey) && enableViewDataPersistence)
		{
			elementPanel.saveViewData();
		}
	}

	internal bool IsViewDataPersitenceSupportedOnChildren(bool existingState)
	{
		bool result = existingState;
		if (string.IsNullOrEmpty(viewDataKey) && this != contentContainer)
		{
			result = false;
		}
		if (parent != null && this == parent.contentContainer)
		{
			result = true;
		}
		return result;
	}

	internal void OnViewDataReady(bool enablePersistence)
	{
		enableViewDataPersistence = enablePersistence;
		OnViewDataReady();
	}

	internal virtual void OnViewDataReady()
	{
	}

	public virtual bool ContainsPoint(Vector2 localPoint)
	{
		return rect.Contains(localPoint);
	}

	public virtual bool Overlaps(Rect rectangle)
	{
		return rect.Overlaps(rectangle, allowInverse: true);
	}

	private void AssignMeasureFunction()
	{
		yogaNode.SetMeasureFunction((YogaNode node, float f, YogaMeasureMode mode, float f1, YogaMeasureMode heightMode) => Measure(node, f, mode, f1, heightMode));
	}

	private void RemoveMeasureFunction()
	{
		yogaNode.SetMeasureFunction(null);
	}

	protected internal virtual Vector2 DoMeasure(float desiredWidth, MeasureMode widthMode, float desiredHeight, MeasureMode heightMode)
	{
		return new Vector2(float.NaN, float.NaN);
	}

	internal YogaSize Measure(YogaNode node, float width, YogaMeasureMode widthMode, float height, YogaMeasureMode heightMode)
	{
		Debug.Assert(node == yogaNode, "YogaNode instance mismatch");
		Vector2 vector = DoMeasure(width, (MeasureMode)widthMode, height, (MeasureMode)heightMode);
		return MeasureOutput.Make(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
	}

	internal void SetSize(Vector2 size)
	{
		Rect rect = layout;
		rect.width = size.x;
		rect.height = size.y;
		layout = rect;
	}

	private void FinalizeLayout()
	{
		if (hasInlineStyle)
		{
			specifiedStyle.SyncWithLayout(yogaNode);
		}
		else
		{
			yogaNode.CopyStyle(specifiedStyle.yogaNode);
		}
	}

	internal void SetInlineStyles(VisualElementStylesData inlineStyleData)
	{
		Debug.Assert(!inlineStyleData.isShared);
		inlineStyleData.Apply(m_Style, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity);
		m_Style = inlineStyleData;
	}

	internal void SetSharedStyles(VisualElementStylesData sharedStyle)
	{
		Debug.Assert(sharedStyle.isShared);
		if (sharedStyle != m_SharedStyle)
		{
			StyleInt overflow = m_Style.overflow;
			StyleLength borderBottomLeftRadius = m_Style.borderBottomLeftRadius;
			StyleLength borderBottomRightRadius = m_Style.borderBottomRightRadius;
			StyleLength borderTopLeftRadius = m_Style.borderTopLeftRadius;
			StyleLength borderTopRightRadius = m_Style.borderTopRightRadius;
			StyleFloat borderLeftWidth = m_Style.borderLeftWidth;
			StyleFloat borderTopWidth = m_Style.borderTopWidth;
			StyleFloat borderRightWidth = m_Style.borderRightWidth;
			StyleFloat borderBottomWidth = m_Style.borderBottomWidth;
			StyleFloat styleFloat = m_Style.opacity;
			if (hasInlineStyle)
			{
				m_Style.Apply(sharedStyle, StylePropertyApplyMode.CopyIfNotInline);
			}
			else
			{
				m_Style = sharedStyle;
			}
			m_SharedStyle = sharedStyle;
			FinalizeLayout();
			VersionChangeType versionChangeType = VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.Repaint;
			if (m_Style.overflow != overflow)
			{
				versionChangeType |= VersionChangeType.Overflow;
			}
			if (borderBottomLeftRadius != m_Style.borderBottomLeftRadius || borderBottomRightRadius != m_Style.borderBottomRightRadius || borderTopLeftRadius != m_Style.borderTopLeftRadius || borderTopRightRadius != m_Style.borderTopRightRadius)
			{
				versionChangeType |= VersionChangeType.BorderRadius;
			}
			if (borderLeftWidth != m_Style.borderLeftWidth || borderTopWidth != m_Style.borderTopWidth || borderRightWidth != m_Style.borderRightWidth || borderBottomWidth != m_Style.borderBottomWidth)
			{
				versionChangeType |= VersionChangeType.BorderWidth;
			}
			if (m_Style.opacity != styleFloat)
			{
				versionChangeType |= VersionChangeType.Opacity;
			}
			IncrementVersion(versionChangeType);
		}
	}

	internal void ResetPositionProperties()
	{
		if (hasInlineStyle)
		{
			style.position = StyleKeyword.Null;
			style.marginLeft = StyleKeyword.Null;
			style.marginRight = StyleKeyword.Null;
			style.marginBottom = StyleKeyword.Null;
			style.marginTop = StyleKeyword.Null;
			style.left = StyleKeyword.Null;
			style.top = StyleKeyword.Null;
			style.right = StyleKeyword.Null;
			style.bottom = StyleKeyword.Null;
			style.width = StyleKeyword.Null;
			style.height = StyleKeyword.Null;
			FinalizeLayout();
			IncrementVersion(VersionChangeType.Layout);
		}
	}

	public override string ToString()
	{
		return string.Concat(GetType().Name, " ", name, " ", layout, " world rect: ", worldBound);
	}

	public IEnumerable<string> GetClasses()
	{
		return m_ClassList;
	}

	public void ClearClassList()
	{
		if (m_ClassList.Count > 0)
		{
			m_ClassList = s_EmptyClassList;
			IncrementVersion(VersionChangeType.StyleSheet);
		}
	}

	public void AddToClassList(string className)
	{
		if (m_ClassList == s_EmptyClassList)
		{
			m_ClassList = new List<string> { className };
		}
		else
		{
			if (m_ClassList.Contains(className))
			{
				return;
			}
			if (m_ClassList.Capacity == m_ClassList.Count)
			{
				m_ClassList.Capacity++;
			}
			m_ClassList.Add(className);
		}
		IncrementVersion(VersionChangeType.StyleSheet);
	}

	public void RemoveFromClassList(string className)
	{
		if (m_ClassList.Remove(className))
		{
			IncrementVersion(VersionChangeType.StyleSheet);
		}
	}

	public void ToggleInClassList(string className)
	{
		if (ClassListContains(className))
		{
			RemoveFromClassList(className);
		}
		else
		{
			AddToClassList(className);
		}
	}

	public void EnableInClassList(string className, bool enable)
	{
		if (enable)
		{
			AddToClassList(className);
		}
		else
		{
			RemoveFromClassList(className);
		}
	}

	public bool ClassListContains(string cls)
	{
		for (int i = 0; i < m_ClassList.Count; i++)
		{
			if (m_ClassList[i] == cls)
			{
				return true;
			}
		}
		return false;
	}

	public object FindAncestorUserData()
	{
		for (VisualElement visualElement = parent; visualElement != null; visualElement = visualElement.parent)
		{
			if (visualElement.userData != null)
			{
				return visualElement.userData;
			}
		}
		return null;
	}

	internal object GetProperty(PropertyName key)
	{
		CheckUserKeyArgument(key);
		return GetPropertyInternal(key);
	}

	internal void SetProperty(PropertyName key, object value)
	{
		CheckUserKeyArgument(key);
		SetPropertyInternal(key, value);
	}

	private object GetPropertyInternal(PropertyName key)
	{
		if (m_PropertyBag != null)
		{
			for (int i = 0; i < m_PropertyBag.Count; i++)
			{
				if (m_PropertyBag[i].Key == key)
				{
					return m_PropertyBag[i].Value;
				}
			}
		}
		return null;
	}

	private static void CheckUserKeyArgument(PropertyName key)
	{
		if (PropertyName.IsNullOrEmpty(key))
		{
			throw new ArgumentNullException("key");
		}
		if (key == userDataPropertyKey)
		{
			throw new InvalidOperationException($"The {userDataPropertyKey} key is reserved by the system");
		}
	}

	private void SetPropertyInternal(PropertyName key, object value)
	{
		KeyValuePair<PropertyName, object> keyValuePair = new KeyValuePair<PropertyName, object>(key, value);
		if (m_PropertyBag == null)
		{
			m_PropertyBag = new List<KeyValuePair<PropertyName, object>>(1);
			m_PropertyBag.Add(keyValuePair);
			return;
		}
		for (int i = 0; i < m_PropertyBag.Count; i++)
		{
			if (m_PropertyBag[i].Key == key)
			{
				m_PropertyBag[i] = keyValuePair;
				return;
			}
		}
		if (m_PropertyBag.Capacity == m_PropertyBag.Count)
		{
			m_PropertyBag.Capacity++;
		}
		m_PropertyBag.Add(keyValuePair);
	}

	private void UpdateCursorStyle(long eventType)
	{
		if (elementPanel != null)
		{
			if (eventType == EventBase<MouseOverEvent>.TypeId() && elementPanel.GetTopElementUnderPointer(PointerId.mousePointerId) == this)
			{
				elementPanel.cursorManager.SetCursor(computedStyle.cursor.value);
			}
			else if (eventType == EventBase<MouseOutEvent>.TypeId())
			{
				elementPanel.cursorManager.ResetCursor();
			}
		}
	}

	private VisualElementAnimationSystem GetAnimationSystem()
	{
		if (elementPanel != null)
		{
			return elementPanel.GetUpdater(VisualTreeUpdatePhase.Animation) as VisualElementAnimationSystem;
		}
		return null;
	}

	internal void RegisterAnimation(IValueAnimationUpdate anim)
	{
		if (m_RunningAnimations == null)
		{
			m_RunningAnimations = new List<IValueAnimationUpdate>();
		}
		m_RunningAnimations.Add(anim);
		GetAnimationSystem()?.RegisterAnimation(anim);
	}

	internal void UnregisterAnimation(IValueAnimationUpdate anim)
	{
		if (m_RunningAnimations != null)
		{
			m_RunningAnimations.Remove(anim);
		}
		GetAnimationSystem()?.UnregisterAnimation(anim);
	}

	private void UnregisterRunningAnimations()
	{
		if (m_RunningAnimations != null && m_RunningAnimations.Count > 0)
		{
			GetAnimationSystem()?.UnregisterAnimations(m_RunningAnimations);
		}
	}

	private void RegisterRunningAnimations()
	{
		if (m_RunningAnimations != null && m_RunningAnimations.Count > 0)
		{
			GetAnimationSystem()?.RegisterAnimations(m_RunningAnimations);
		}
	}

	ValueAnimation<float> ITransitionAnimations.Start(float from, float to, int durationMs, Action<VisualElement, float> onValueChanged)
	{
		return experimental.animation.Start((VisualElement e) => from, to, durationMs, onValueChanged);
	}

	ValueAnimation<Rect> ITransitionAnimations.Start(Rect from, Rect to, int durationMs, Action<VisualElement, Rect> onValueChanged)
	{
		return experimental.animation.Start((VisualElement e) => from, to, durationMs, onValueChanged);
	}

	ValueAnimation<Color> ITransitionAnimations.Start(Color from, Color to, int durationMs, Action<VisualElement, Color> onValueChanged)
	{
		return experimental.animation.Start((VisualElement e) => from, to, durationMs, onValueChanged);
	}

	ValueAnimation<Vector3> ITransitionAnimations.Start(Vector3 from, Vector3 to, int durationMs, Action<VisualElement, Vector3> onValueChanged)
	{
		return experimental.animation.Start((VisualElement e) => from, to, durationMs, onValueChanged);
	}

	ValueAnimation<Vector2> ITransitionAnimations.Start(Vector2 from, Vector2 to, int durationMs, Action<VisualElement, Vector2> onValueChanged)
	{
		return experimental.animation.Start((VisualElement e) => from, to, durationMs, onValueChanged);
	}

	ValueAnimation<Quaternion> ITransitionAnimations.Start(Quaternion from, Quaternion to, int durationMs, Action<VisualElement, Quaternion> onValueChanged)
	{
		return experimental.animation.Start((VisualElement e) => from, to, durationMs, onValueChanged);
	}

	ValueAnimation<StyleValues> ITransitionAnimations.Start(StyleValues from, StyleValues to, int durationMs)
	{
		if (from.m_StyleValues == null)
		{
			from.Values();
		}
		if (to.m_StyleValues == null)
		{
			to.Values();
		}
		return Start((VisualElement e) => from, to, durationMs);
	}

	ValueAnimation<float> ITransitionAnimations.Start(Func<VisualElement, float> fromValueGetter, float to, int durationMs, Action<VisualElement, float> onValueChanged)
	{
		return StartAnimation(ValueAnimation<float>.Create(this, Lerp.Interpolate), fromValueGetter, to, durationMs, onValueChanged);
	}

	ValueAnimation<Rect> ITransitionAnimations.Start(Func<VisualElement, Rect> fromValueGetter, Rect to, int durationMs, Action<VisualElement, Rect> onValueChanged)
	{
		return StartAnimation(ValueAnimation<Rect>.Create(this, Lerp.Interpolate), fromValueGetter, to, durationMs, onValueChanged);
	}

	ValueAnimation<Color> ITransitionAnimations.Start(Func<VisualElement, Color> fromValueGetter, Color to, int durationMs, Action<VisualElement, Color> onValueChanged)
	{
		return StartAnimation(ValueAnimation<Color>.Create(this, Lerp.Interpolate), fromValueGetter, to, durationMs, onValueChanged);
	}

	ValueAnimation<Vector3> ITransitionAnimations.Start(Func<VisualElement, Vector3> fromValueGetter, Vector3 to, int durationMs, Action<VisualElement, Vector3> onValueChanged)
	{
		return StartAnimation(ValueAnimation<Vector3>.Create(this, Lerp.Interpolate), fromValueGetter, to, durationMs, onValueChanged);
	}

	ValueAnimation<Vector2> ITransitionAnimations.Start(Func<VisualElement, Vector2> fromValueGetter, Vector2 to, int durationMs, Action<VisualElement, Vector2> onValueChanged)
	{
		return StartAnimation(ValueAnimation<Vector2>.Create(this, Lerp.Interpolate), fromValueGetter, to, durationMs, onValueChanged);
	}

	ValueAnimation<Quaternion> ITransitionAnimations.Start(Func<VisualElement, Quaternion> fromValueGetter, Quaternion to, int durationMs, Action<VisualElement, Quaternion> onValueChanged)
	{
		return StartAnimation(ValueAnimation<Quaternion>.Create(this, Lerp.Interpolate), fromValueGetter, to, durationMs, onValueChanged);
	}

	private static ValueAnimation<T> StartAnimation<T>(ValueAnimation<T> anim, Func<VisualElement, T> fromValueGetter, T to, int durationMs, Action<VisualElement, T> onValueChanged)
	{
		anim.initialValue = fromValueGetter;
		anim.to = to;
		anim.durationMs = durationMs;
		anim.valueUpdated = onValueChanged;
		anim.Start();
		return anim;
	}

	private static void AssignStyleValues(VisualElement ve, StyleValues src)
	{
		IStyle style = ve.style;
		if (src.m_StyleValues == null)
		{
			return;
		}
		foreach (StyleValue value in src.m_StyleValues.m_Values)
		{
			switch (value.id)
			{
			case StylePropertyID.MarginLeft:
				style.marginLeft = value.number;
				break;
			case StylePropertyID.MarginTop:
				style.marginTop = value.number;
				break;
			case StylePropertyID.MarginRight:
				style.marginRight = value.number;
				break;
			case StylePropertyID.MarginBottom:
				style.marginBottom = value.number;
				break;
			case StylePropertyID.PaddingLeft:
				style.paddingLeft = value.number;
				break;
			case StylePropertyID.PaddingTop:
				style.paddingTop = value.number;
				break;
			case StylePropertyID.PaddingRight:
				style.paddingRight = value.number;
				break;
			case StylePropertyID.PaddingBottom:
				style.paddingBottom = value.number;
				break;
			case StylePropertyID.PositionLeft:
				style.left = value.number;
				break;
			case StylePropertyID.PositionTop:
				style.top = value.number;
				break;
			case StylePropertyID.PositionRight:
				style.right = value.number;
				break;
			case StylePropertyID.PositionBottom:
				style.bottom = value.number;
				break;
			case StylePropertyID.Width:
				style.width = value.number;
				break;
			case StylePropertyID.Height:
				style.height = value.number;
				break;
			case StylePropertyID.FlexGrow:
				style.flexGrow = value.number;
				break;
			case StylePropertyID.FlexShrink:
				style.flexShrink = value.number;
				break;
			case StylePropertyID.BorderLeftWidth:
				style.borderLeftWidth = value.number;
				break;
			case StylePropertyID.BorderTopWidth:
				style.borderTopWidth = value.number;
				break;
			case StylePropertyID.BorderRightWidth:
				style.borderRightWidth = value.number;
				break;
			case StylePropertyID.BorderBottomWidth:
				style.borderBottomWidth = value.number;
				break;
			case StylePropertyID.BorderTopLeftRadius:
				style.borderTopLeftRadius = value.number;
				break;
			case StylePropertyID.BorderTopRightRadius:
				style.borderTopRightRadius = value.number;
				break;
			case StylePropertyID.BorderBottomRightRadius:
				style.borderBottomRightRadius = value.number;
				break;
			case StylePropertyID.BorderBottomLeftRadius:
				style.borderBottomLeftRadius = value.number;
				break;
			case StylePropertyID.FontSize:
				style.fontSize = value.number;
				break;
			case StylePropertyID.Color:
				style.color = value.color;
				break;
			case StylePropertyID.BackgroundColor:
				style.backgroundColor = value.color;
				break;
			case StylePropertyID.BorderColor:
				style.borderLeftColor = value.color;
				style.borderTopColor = value.color;
				style.borderRightColor = value.color;
				style.borderBottomColor = value.color;
				break;
			case StylePropertyID.BackgroundImageTintColor:
				style.unityBackgroundImageTintColor = value.color;
				break;
			case StylePropertyID.Opacity:
				style.opacity = value.number;
				break;
			}
		}
	}

	private StyleValues ReadCurrentValues(VisualElement ve, StyleValues targetValuesToRead)
	{
		StyleValues result = default(StyleValues);
		IResolvedStyle resolvedStyle = ve.resolvedStyle;
		if (targetValuesToRead.m_StyleValues != null)
		{
			using List<StyleValue>.Enumerator enumerator = targetValuesToRead.m_StyleValues.m_Values.GetEnumerator();
			while (enumerator.MoveNext())
			{
				switch (enumerator.Current.id)
				{
				case StylePropertyID.MarginLeft:
					result.marginLeft = resolvedStyle.marginLeft;
					break;
				case StylePropertyID.MarginTop:
					result.marginTop = resolvedStyle.marginTop;
					break;
				case StylePropertyID.MarginRight:
					result.marginRight = resolvedStyle.marginRight;
					break;
				case StylePropertyID.MarginBottom:
					result.marginBottom = resolvedStyle.marginBottom;
					break;
				case StylePropertyID.PaddingLeft:
					result.paddingLeft = resolvedStyle.paddingLeft;
					break;
				case StylePropertyID.PaddingTop:
					result.paddingTop = resolvedStyle.paddingTop;
					break;
				case StylePropertyID.PaddingRight:
					result.paddingRight = resolvedStyle.paddingRight;
					break;
				case StylePropertyID.PaddingBottom:
					result.paddingBottom = resolvedStyle.paddingBottom;
					break;
				case StylePropertyID.PositionLeft:
					result.left = resolvedStyle.left;
					break;
				case StylePropertyID.PositionTop:
					result.top = resolvedStyle.top;
					break;
				case StylePropertyID.PositionRight:
					result.right = resolvedStyle.right;
					break;
				case StylePropertyID.PositionBottom:
					result.bottom = resolvedStyle.bottom;
					break;
				case StylePropertyID.Width:
					result.width = resolvedStyle.width;
					break;
				case StylePropertyID.Height:
					result.height = resolvedStyle.height;
					break;
				case StylePropertyID.FlexGrow:
					result.flexGrow = resolvedStyle.flexGrow;
					break;
				case StylePropertyID.FlexShrink:
					result.flexShrink = resolvedStyle.flexShrink;
					break;
				case StylePropertyID.BorderLeftWidth:
					result.borderLeftWidth = resolvedStyle.borderLeftWidth;
					break;
				case StylePropertyID.BorderTopWidth:
					result.borderTopWidth = resolvedStyle.borderTopWidth;
					break;
				case StylePropertyID.BorderRightWidth:
					result.borderRightWidth = resolvedStyle.borderRightWidth;
					break;
				case StylePropertyID.BorderBottomWidth:
					result.borderBottomWidth = resolvedStyle.borderBottomWidth;
					break;
				case StylePropertyID.BorderTopLeftRadius:
					result.borderTopLeftRadius = resolvedStyle.borderTopLeftRadius;
					break;
				case StylePropertyID.BorderTopRightRadius:
					result.borderTopRightRadius = resolvedStyle.borderTopRightRadius;
					break;
				case StylePropertyID.BorderBottomRightRadius:
					result.borderBottomRightRadius = resolvedStyle.borderBottomRightRadius;
					break;
				case StylePropertyID.BorderBottomLeftRadius:
					result.borderBottomLeftRadius = resolvedStyle.borderBottomLeftRadius;
					break;
				case StylePropertyID.Color:
					result.color = resolvedStyle.color;
					break;
				case StylePropertyID.BackgroundColor:
					result.backgroundColor = resolvedStyle.backgroundColor;
					break;
				case StylePropertyID.BorderColor:
					result.borderColor = resolvedStyle.borderLeftColor;
					break;
				case StylePropertyID.BackgroundImageTintColor:
					result.unityBackgroundImageTintColor = resolvedStyle.unityBackgroundImageTintColor;
					break;
				case StylePropertyID.Opacity:
					result.opacity = resolvedStyle.opacity;
					break;
				}
			}
		}
		return result;
	}

	ValueAnimation<StyleValues> ITransitionAnimations.Start(StyleValues to, int durationMs)
	{
		if (to.m_StyleValues == null)
		{
			to.Values();
		}
		return Start((VisualElement e) => ReadCurrentValues(e, to), to, durationMs);
	}

	private ValueAnimation<StyleValues> Start(Func<VisualElement, StyleValues> fromValueGetter, StyleValues to, int durationMs)
	{
		return StartAnimation(ValueAnimation<StyleValues>.Create(this, Lerp.Interpolate), fromValueGetter, to, durationMs, AssignStyleValues);
	}

	ValueAnimation<Rect> ITransitionAnimations.Layout(Rect to, int durationMs)
	{
		return experimental.animation.Start((VisualElement e) => new Rect(e.resolvedStyle.left, e.resolvedStyle.top, e.resolvedStyle.width, e.resolvedStyle.height), to, durationMs, delegate(VisualElement e, Rect c)
		{
			e.style.left = c.x;
			e.style.top = c.y;
			e.style.width = c.width;
			e.style.height = c.height;
		});
	}

	ValueAnimation<Vector2> ITransitionAnimations.TopLeft(Vector2 to, int durationMs)
	{
		return experimental.animation.Start((VisualElement e) => new Vector2(e.resolvedStyle.left, e.resolvedStyle.top), to, durationMs, delegate(VisualElement e, Vector2 c)
		{
			e.style.left = c.x;
			e.style.top = c.y;
		});
	}

	ValueAnimation<Vector2> ITransitionAnimations.Size(Vector2 to, int durationMs)
	{
		return experimental.animation.Start((VisualElement e) => e.layout.size, to, durationMs, delegate(VisualElement e, Vector2 c)
		{
			e.style.width = c.x;
			e.style.height = c.y;
		});
	}

	ValueAnimation<float> ITransitionAnimations.Scale(float to, int durationMs)
	{
		return experimental.animation.Start((VisualElement e) => e.transform.scale.x, to, durationMs, delegate(VisualElement e, float c)
		{
			e.transform.scale = new Vector3(c, c, c);
		});
	}

	ValueAnimation<Vector3> ITransitionAnimations.Position(Vector3 to, int durationMs)
	{
		return experimental.animation.Start((VisualElement e) => e.transform.position, to, durationMs, delegate(VisualElement e, Vector3 c)
		{
			e.transform.position = c;
		});
	}

	ValueAnimation<Quaternion> ITransitionAnimations.Rotation(Quaternion to, int durationMs)
	{
		return experimental.animation.Start((VisualElement e) => e.transform.rotation, to, durationMs, delegate(VisualElement e, Quaternion c)
		{
			e.transform.rotation = c;
		});
	}

	internal bool ShouldClip()
	{
		return computedStyle.overflow.value != Overflow.Visible;
	}

	public void Add(VisualElement child)
	{
		if (child != null)
		{
			if (contentContainer == this)
			{
				hierarchy.Add(child);
			}
			else
			{
				contentContainer?.Add(child);
			}
			child.m_LogicalParent = this;
		}
	}

	public void Insert(int index, VisualElement element)
	{
		if (element != null)
		{
			if (contentContainer == this)
			{
				hierarchy.Insert(index, element);
			}
			else
			{
				contentContainer?.Insert(index, element);
			}
			element.m_LogicalParent = this;
		}
	}

	public void Remove(VisualElement element)
	{
		if (contentContainer == this)
		{
			hierarchy.Remove(element);
		}
		else
		{
			contentContainer?.Remove(element);
		}
	}

	public void RemoveAt(int index)
	{
		if (contentContainer == this)
		{
			hierarchy.RemoveAt(index);
		}
		else
		{
			contentContainer?.RemoveAt(index);
		}
	}

	public void Clear()
	{
		if (contentContainer == this)
		{
			hierarchy.Clear();
		}
		else
		{
			contentContainer?.Clear();
		}
	}

	public VisualElement ElementAt(int index)
	{
		return this[index];
	}

	public int IndexOf(VisualElement element)
	{
		if (contentContainer == this)
		{
			return hierarchy.IndexOf(element);
		}
		return contentContainer?.IndexOf(element) ?? (-1);
	}

	public IEnumerable<VisualElement> Children()
	{
		if (contentContainer == this)
		{
			return hierarchy.Children();
		}
		return contentContainer?.Children();
	}

	public void Sort(Comparison<VisualElement> comp)
	{
		if (contentContainer == this)
		{
			hierarchy.Sort(comp);
		}
		else
		{
			contentContainer?.Sort(comp);
		}
	}

	public void BringToFront()
	{
		if (hierarchy.parent != null)
		{
			hierarchy.parent.hierarchy.BringToFront(this);
		}
	}

	public void SendToBack()
	{
		if (hierarchy.parent != null)
		{
			hierarchy.parent.hierarchy.SendToBack(this);
		}
	}

	public void PlaceBehind(VisualElement sibling)
	{
		if (sibling == null)
		{
			throw new ArgumentNullException("sibling");
		}
		if (hierarchy.parent == null || sibling.hierarchy.parent != hierarchy.parent)
		{
			throw new ArgumentException("VisualElements are not siblings");
		}
		hierarchy.parent.hierarchy.PlaceBehind(this, sibling);
	}

	public void PlaceInFront(VisualElement sibling)
	{
		if (sibling == null)
		{
			throw new ArgumentNullException("sibling");
		}
		if (hierarchy.parent == null || sibling.hierarchy.parent != hierarchy.parent)
		{
			throw new ArgumentException("VisualElements are not siblings");
		}
		hierarchy.parent.hierarchy.PlaceInFront(this, sibling);
	}

	public void RemoveFromHierarchy()
	{
		if (hierarchy.parent != null)
		{
			hierarchy.parent.hierarchy.Remove(this);
		}
	}

	public T GetFirstOfType<T>() where T : class
	{
		if (this is T result)
		{
			return result;
		}
		return GetFirstAncestorOfType<T>();
	}

	public T GetFirstAncestorOfType<T>() where T : class
	{
		for (VisualElement visualElement = hierarchy.parent; visualElement != null; visualElement = visualElement.hierarchy.parent)
		{
			if (visualElement is T result)
			{
				return result;
			}
		}
		return null;
	}

	public bool Contains(VisualElement child)
	{
		while (child != null)
		{
			if (child.hierarchy.parent == this)
			{
				return true;
			}
			child = child.hierarchy.parent;
		}
		return false;
	}

	private void GatherAllChildren(List<VisualElement> elements)
	{
		if (m_Children.Count > 0)
		{
			int i = elements.Count;
			elements.AddRange(m_Children);
			for (; i < elements.Count; i++)
			{
				VisualElement visualElement = elements[i];
				elements.AddRange(visualElement.m_Children);
			}
		}
	}

	public VisualElement FindCommonAncestor(VisualElement other)
	{
		if (other == null)
		{
			throw new ArgumentNullException("other");
		}
		if (panel != other.panel)
		{
			return null;
		}
		VisualElement visualElement = this;
		int num = 0;
		while (visualElement != null)
		{
			num++;
			visualElement = visualElement.hierarchy.parent;
		}
		VisualElement visualElement2 = other;
		int num2 = 0;
		while (visualElement2 != null)
		{
			num2++;
			visualElement2 = visualElement2.hierarchy.parent;
		}
		visualElement = this;
		visualElement2 = other;
		while (num > num2)
		{
			num--;
			visualElement = visualElement.hierarchy.parent;
		}
		while (num2 > num)
		{
			num2--;
			visualElement2 = visualElement2.hierarchy.parent;
		}
		while (visualElement != visualElement2)
		{
			visualElement = visualElement.hierarchy.parent;
			visualElement2 = visualElement2.hierarchy.parent;
		}
		return visualElement;
	}

	internal VisualElement GetRoot()
	{
		if (panel != null)
		{
			return panel.visualTree;
		}
		VisualElement visualElement = this;
		while (visualElement.m_PhysicalParent != null)
		{
			visualElement = visualElement.m_PhysicalParent;
		}
		return visualElement;
	}

	internal VisualElement GetNextElementDepthFirst()
	{
		if (m_Children.Count > 0)
		{
			return m_Children[0];
		}
		VisualElement physicalParent = m_PhysicalParent;
		VisualElement visualElement = this;
		while (physicalParent != null)
		{
			int i;
			for (i = 0; i < physicalParent.m_Children.Count && physicalParent.m_Children[i] != visualElement; i++)
			{
			}
			if (i < physicalParent.m_Children.Count - 1)
			{
				return physicalParent.m_Children[i + 1];
			}
			visualElement = physicalParent;
			physicalParent = physicalParent.m_PhysicalParent;
		}
		return null;
	}

	internal VisualElement GetPreviousElementDepthFirst()
	{
		if (m_PhysicalParent != null)
		{
			int i;
			for (i = 0; i < m_PhysicalParent.m_Children.Count && m_PhysicalParent.m_Children[i] != this; i++)
			{
			}
			if (i > 0)
			{
				VisualElement visualElement = m_PhysicalParent.m_Children[i - 1];
				while (visualElement.m_Children.Count > 0)
				{
					visualElement = visualElement.m_Children[visualElement.m_Children.Count - 1];
				}
				return visualElement;
			}
			return m_PhysicalParent;
		}
		return null;
	}

	internal VisualElement RetargetElement(VisualElement retargetAgainst)
	{
		if (retargetAgainst == null)
		{
			return this;
		}
		VisualElement visualElement = retargetAgainst.m_PhysicalParent ?? retargetAgainst;
		while (visualElement.m_PhysicalParent != null && !visualElement.isCompositeRoot)
		{
			visualElement = visualElement.m_PhysicalParent;
		}
		VisualElement result = this;
		VisualElement physicalParent = m_PhysicalParent;
		while (physicalParent != null)
		{
			physicalParent = physicalParent.m_PhysicalParent;
			if (physicalParent == visualElement)
			{
				return result;
			}
			if (physicalParent != null && physicalParent.isCompositeRoot)
			{
				result = physicalParent;
			}
		}
		return this;
	}

	IVisualElementScheduledItem IVisualElementScheduler.Execute(Action<TimerState> timerUpdateEvent)
	{
		TimerStateScheduledItem timerStateScheduledItem = new TimerStateScheduledItem(this, timerUpdateEvent)
		{
			timerUpdateStopCondition = ScheduledItem.OnceCondition
		};
		timerStateScheduledItem.Resume();
		return timerStateScheduledItem;
	}

	IVisualElementScheduledItem IVisualElementScheduler.Execute(Action updateEvent)
	{
		SimpleScheduledItem simpleScheduledItem = new SimpleScheduledItem(this, updateEvent)
		{
			timerUpdateStopCondition = ScheduledItem.OnceCondition
		};
		simpleScheduledItem.Resume();
		return simpleScheduledItem;
	}

	internal void AddStyleSheetPath(string sheetPath)
	{
		StyleSheet styleSheet = Panel.LoadResource(sheetPath, typeof(StyleSheet), scaledPixelsPerPoint) as StyleSheet;
		if (styleSheet == null)
		{
			Debug.LogWarning($"Style sheet not found for path \"{sheetPath}\"");
		}
		else
		{
			styleSheets.Add(styleSheet);
		}
	}

	internal bool HasStyleSheetPath(string sheetPath)
	{
		StyleSheet styleSheet = Panel.LoadResource(sheetPath, typeof(StyleSheet), scaledPixelsPerPoint) as StyleSheet;
		if (styleSheet == null)
		{
			Debug.LogWarning($"Style sheet not found for path \"{sheetPath}\"");
			return false;
		}
		return styleSheets.Contains(styleSheet);
	}

	internal void RemoveStyleSheetPath(string sheetPath)
	{
		StyleSheet styleSheet = Panel.LoadResource(sheetPath, typeof(StyleSheet), scaledPixelsPerPoint) as StyleSheet;
		if (styleSheet == null)
		{
			Debug.LogWarning($"Style sheet not found for path \"{sheetPath}\"");
		}
		else
		{
			styleSheets.Remove(styleSheet);
		}
	}

	private StyleFloat ResolveLengthValue(StyleLength styleLength, bool isRow)
	{
		if (styleLength.keyword != StyleKeyword.Undefined)
		{
			return styleLength.ToStyleFloat();
		}
		Length value = styleLength.value;
		if (value.unit != LengthUnit.Percent)
		{
			return styleLength.ToStyleFloat();
		}
		VisualElement visualElement = hierarchy.parent;
		if (visualElement == null)
		{
			return 0f;
		}
		float num = (isRow ? visualElement.resolvedStyle.width : visualElement.resolvedStyle.height);
		return value.value * num / 100f;
	}

	private static void OnTooltip(TooltipEvent e, string tooltip)
	{
		if (e.currentTarget is VisualElement visualElement)
		{
			e.rect = visualElement.worldBound;
		}
		e.tooltip = tooltip;
		e.StopImmediatePropagation();
	}
}
