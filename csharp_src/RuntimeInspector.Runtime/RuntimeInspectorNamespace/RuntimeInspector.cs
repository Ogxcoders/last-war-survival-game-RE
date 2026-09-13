using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class RuntimeInspector : SkinnedWindow, ITooltipManager
{
	public enum VariableVisibility
	{
		None,
		SerializableOnly,
		All
	}

	public enum HeaderVisibility
	{
		Collapsible,
		AlwaysVisible,
		Hidden
	}

	public delegate object InspectedObjectChangingDelegate(object previousInspectedObject, object newInspectedObject);

	public delegate void ComponentFilterDelegate(GameObject gameObject, List<Component> components);

	private const string POOL_OBJECT_NAME = "RuntimeInspectorPool";

	[SerializeField]
	[FormerlySerializedAs("refreshInterval")]
	private float m_refreshInterval;

	private float nextRefreshTime = -1f;

	[Space]
	[SerializeField]
	private VariableVisibility m_exposeFields = VariableVisibility.SerializableOnly;

	[SerializeField]
	private VariableVisibility m_exposeProperties = VariableVisibility.SerializableOnly;

	[Space]
	[SerializeField]
	private bool m_arrayIndicesStartAtOne;

	[SerializeField]
	private bool m_useTitleCaseNaming;

	[Space]
	[SerializeField]
	private bool m_showAddComponentButton = true;

	[SerializeField]
	private bool m_showRemoveComponentButton = true;

	[Space]
	[SerializeField]
	private bool m_showTooltips;

	[SerializeField]
	private float m_tooltipDelay = 0.5f;

	[Space]
	[SerializeField]
	private int m_nestLimit = 5;

	[SerializeField]
	private HeaderVisibility m_inspectedObjectHeaderVisibility;

	[SerializeField]
	private int poolCapacity = 10;

	private Transform poolParent;

	[SerializeField]
	private RuntimeHierarchy m_connectedHierarchy;

	[SerializeField]
	private RuntimeInspectorSettings[] settings;

	private bool m_isLocked;

	[Header("Internal Variables")]
	[SerializeField]
	private ScrollRect scrollView;

	private RectTransform drawArea;

	[SerializeField]
	private Image background;

	[SerializeField]
	private Image scrollbar;

	private static int aliveInspectors = 0;

	private bool initialized;

	private readonly Dictionary<Type, InspectorField[]> typeToDrawers = new Dictionary<Type, InspectorField[]>(89);

	private readonly Dictionary<Type, InspectorField[]> typeToReferenceDrawers = new Dictionary<Type, InspectorField[]>(89);

	private readonly List<InspectorField> eligibleDrawers = new List<InspectorField>(4);

	private static readonly Dictionary<Type, List<InspectorField>> drawersPool = new Dictionary<Type, List<InspectorField>>();

	private readonly List<VariableSet> hiddenVariables = new List<VariableSet>(32);

	private readonly List<VariableSet> exposedVariables = new List<VariableSet>(32);

	private InspectorField currentDrawer;

	private bool inspectLock;

	private bool isDirty;

	private object m_inspectedObject;

	private Canvas m_canvas;

	private PointerEventData nullPointerEventData;

	public InspectedObjectChangingDelegate OnInspectedObjectChanging;

	private ComponentFilterDelegate m_componentFilter;

	public float RefreshInterval
	{
		get
		{
			return m_refreshInterval;
		}
		set
		{
			m_refreshInterval = value;
		}
	}

	public VariableVisibility ExposeFields
	{
		get
		{
			return m_exposeFields;
		}
		set
		{
			if (m_exposeFields != value)
			{
				m_exposeFields = value;
				isDirty = true;
			}
		}
	}

	public VariableVisibility ExposeProperties
	{
		get
		{
			return m_exposeProperties;
		}
		set
		{
			if (m_exposeProperties != value)
			{
				m_exposeProperties = value;
				isDirty = true;
			}
		}
	}

	public bool ArrayIndicesStartAtOne
	{
		get
		{
			return m_arrayIndicesStartAtOne;
		}
		set
		{
			if (m_arrayIndicesStartAtOne != value)
			{
				m_arrayIndicesStartAtOne = value;
				isDirty = true;
			}
		}
	}

	public bool UseTitleCaseNaming
	{
		get
		{
			return m_useTitleCaseNaming;
		}
		set
		{
			if (m_useTitleCaseNaming != value)
			{
				m_useTitleCaseNaming = value;
				isDirty = true;
			}
		}
	}

	public bool ShowAddComponentButton
	{
		get
		{
			return m_showAddComponentButton;
		}
		set
		{
			if (m_showAddComponentButton != value)
			{
				m_showAddComponentButton = value;
				isDirty = true;
			}
		}
	}

	public bool ShowRemoveComponentButton
	{
		get
		{
			return m_showRemoveComponentButton;
		}
		set
		{
			if (m_showRemoveComponentButton != value)
			{
				m_showRemoveComponentButton = value;
				isDirty = true;
			}
		}
	}

	public bool ShowTooltips => m_showTooltips;

	public float TooltipDelay
	{
		get
		{
			return m_tooltipDelay;
		}
		set
		{
			m_tooltipDelay = value;
		}
	}

	internal TooltipListener TooltipListener { get; private set; }

	public int NestLimit
	{
		get
		{
			return m_nestLimit;
		}
		set
		{
			if (m_nestLimit != value)
			{
				m_nestLimit = value;
				isDirty = true;
			}
		}
	}

	public HeaderVisibility InspectedObjectHeaderVisibility
	{
		get
		{
			return m_inspectedObjectHeaderVisibility;
		}
		set
		{
			if (m_inspectedObjectHeaderVisibility != value)
			{
				m_inspectedObjectHeaderVisibility = value;
				if (currentDrawer != null && currentDrawer is ExpandableInspectorField)
				{
					((ExpandableInspectorField)currentDrawer).HeaderVisibility = m_inspectedObjectHeaderVisibility;
				}
			}
		}
	}

	public RuntimeHierarchy ConnectedHierarchy
	{
		get
		{
			return m_connectedHierarchy;
		}
		set
		{
			m_connectedHierarchy = value;
		}
	}

	public bool IsLocked
	{
		get
		{
			return m_isLocked;
		}
		set
		{
			m_isLocked = value;
		}
	}

	public object InspectedObject => m_inspectedObject;

	public bool IsBound => !m_inspectedObject.IsNull();

	public Canvas Canvas => m_canvas;

	public ComponentFilterDelegate ComponentFilter
	{
		get
		{
			return m_componentFilter;
		}
		set
		{
			m_componentFilter = value;
			Refresh();
		}
	}

	protected override void Awake()
	{
		base.Awake();
		Initialize();
	}

	private void Initialize()
	{
		if (initialized)
		{
			return;
		}
		initialized = true;
		drawArea = scrollView.content;
		m_canvas = GetComponentInParent<Canvas>();
		nullPointerEventData = new PointerEventData(null);
		if (m_showTooltips)
		{
			TooltipListener = base.gameObject.AddComponent<TooltipListener>();
			TooltipListener.Initialize(this);
		}
		GameObject gameObject = GameObject.Find("RuntimeInspectorPool");
		if (gameObject == null)
		{
			gameObject = new GameObject("RuntimeInspectorPool");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
		poolParent = gameObject.transform;
		aliveInspectors++;
		for (int i = 0; i < settings.Length; i++)
		{
			if (!settings[i])
			{
				continue;
			}
			VariableSet[] array = settings[i].HiddenVariables;
			foreach (VariableSet variableSet in array)
			{
				if (variableSet.Init())
				{
					hiddenVariables.Add(variableSet);
				}
			}
			VariableSet[] array2 = settings[i].ExposedVariables;
			foreach (VariableSet variableSet2 in array2)
			{
				if (variableSet2.Init())
				{
					exposedVariables.Add(variableSet2);
				}
			}
		}
		RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Add(drawArea);
		RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Add(poolParent);
	}

	private void OnDestroy()
	{
		if (--aliveInspectors == 0)
		{
			if ((bool)poolParent)
			{
				RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Remove(poolParent);
				UnityEngine.Object.DestroyImmediate(poolParent.gameObject);
			}
			ColorPicker.DestroyInstance();
			ObjectReferencePicker.DestroyInstance();
			drawersPool.Clear();
		}
		RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Remove(drawArea);
	}

	private void OnTransformParentChanged()
	{
		m_canvas = GetComponentInParent<Canvas>();
	}

	protected override void Update()
	{
		base.Update();
		if (IsBound)
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (isDirty)
			{
				object inspectedObject = m_inspectedObject;
				StopInspectInternal();
				InspectInternal(inspectedObject);
				isDirty = false;
				nextRefreshTime = realtimeSinceStartup + m_refreshInterval;
			}
			else if (realtimeSinceStartup > nextRefreshTime)
			{
				nextRefreshTime = realtimeSinceStartup + m_refreshInterval;
				Refresh();
			}
		}
		else if (currentDrawer != null)
		{
			StopInspectInternal();
		}
	}

	public void Refresh()
	{
		if (IsBound)
		{
			if (currentDrawer == null)
			{
				m_inspectedObject = null;
			}
			else
			{
				currentDrawer.Refresh();
			}
		}
	}

	public void RefreshDelayed()
	{
		nextRefreshTime = 0f;
	}

	internal void EnsureScrollViewIsWithinBounds()
	{
		if (scrollView.verticalNormalizedPosition <= Mathf.Epsilon)
		{
			scrollView.verticalNormalizedPosition = 0.0001f;
		}
		scrollView.OnScroll(nullPointerEventData);
	}

	protected override void RefreshSkin()
	{
		background.color = base.Skin.BackgroundColor;
		scrollbar.color = base.Skin.ScrollbarColor;
		if (IsBound && !isDirty)
		{
			currentDrawer.Skin = base.Skin;
		}
	}

	public void Inspect(object obj)
	{
		if (!m_isLocked)
		{
			InspectInternal(obj);
		}
	}

	internal void InspectInternal(object obj)
	{
		if (inspectLock)
		{
			return;
		}
		isDirty = false;
		Initialize();
		if (OnInspectedObjectChanging != null)
		{
			obj = OnInspectedObjectChanging(m_inspectedObject, obj);
		}
		if (m_inspectedObject == obj)
		{
			return;
		}
		StopInspectInternal();
		inspectLock = true;
		try
		{
			m_inspectedObject = obj;
			if (obj.IsNull())
			{
				return;
			}
			if (obj.GetType().IsValueType)
			{
				m_inspectedObject = null;
				Debug.LogError("Can't inspect a value type!");
				return;
			}
			InspectorField inspectorField = CreateDrawerForType(obj.GetType(), drawArea, 0, drawObjectsAsFields: false);
			if (inspectorField != null)
			{
				inspectorField.BindTo(obj.GetType(), string.Empty, () => m_inspectedObject, delegate(object value)
				{
					m_inspectedObject = value;
				});
				inspectorField.NameRaw = obj.GetNameWithType();
				inspectorField.Refresh();
				if (inspectorField is ExpandableInspectorField)
				{
					((ExpandableInspectorField)inspectorField).IsExpanded = true;
				}
				currentDrawer = inspectorField;
				if (currentDrawer is ExpandableInspectorField)
				{
					((ExpandableInspectorField)currentDrawer).HeaderVisibility = m_inspectedObjectHeaderVisibility;
				}
				GameObject gameObject = m_inspectedObject as GameObject;
				if (!gameObject && (bool)(m_inspectedObject as Component))
				{
					gameObject = ((Component)m_inspectedObject).gameObject;
				}
				if ((bool)ConnectedHierarchy && (bool)gameObject && !ConnectedHierarchy.Select(gameObject.transform, RuntimeHierarchy.SelectOptions.FocusOnSelection))
				{
					ConnectedHierarchy.Deselect();
				}
			}
			else
			{
				m_inspectedObject = null;
			}
		}
		finally
		{
			inspectLock = false;
		}
	}

	public void StopInspect()
	{
		if (!m_isLocked)
		{
			StopInspectInternal();
		}
	}

	internal void StopInspectInternal()
	{
		if (!inspectLock)
		{
			if (currentDrawer != null)
			{
				currentDrawer.Unbind();
				currentDrawer = null;
			}
			m_inspectedObject = null;
			scrollView.verticalNormalizedPosition = 1f;
			ColorPicker.Instance.Close();
			ObjectReferencePicker.Instance.Close();
		}
	}

	public InspectorField CreateDrawerForType(Type type, Transform drawerParent, int depth, bool drawObjectsAsFields = true, MemberInfo variable = null)
	{
		InspectorField[] drawersForType = GetDrawersForType(type, drawObjectsAsFields);
		if (drawersForType != null)
		{
			for (int i = 0; i < drawersForType.Length; i++)
			{
				if (drawersForType[i].CanBindTo(type, variable))
				{
					InspectorField inspectorField = InstantiateDrawer(drawersForType[i], drawerParent);
					inspectorField.Inspector = this;
					inspectorField.Skin = base.Skin;
					inspectorField.Depth = depth;
					return inspectorField;
				}
			}
		}
		return null;
	}

	private InspectorField InstantiateDrawer(InspectorField drawer, Transform drawerParent)
	{
		if (drawersPool.TryGetValue(drawer.GetType(), out var value))
		{
			for (int num = value.Count - 1; num >= 0; num--)
			{
				InspectorField inspectorField = value[num];
				value.RemoveAt(num);
				if ((bool)inspectorField)
				{
					inspectorField.transform.SetParent(drawerParent, worldPositionStays: false);
					inspectorField.gameObject.SetActive(value: true);
					return inspectorField;
				}
			}
		}
		InspectorField inspectorField2 = UnityEngine.Object.Instantiate(drawer, drawerParent, worldPositionStays: false);
		inspectorField2.Initialize();
		return inspectorField2;
	}

	private InspectorField[] GetDrawersForType(Type type, bool drawObjectsAsFields)
	{
		bool flag = drawObjectsAsFields && typeof(UnityEngine.Object).IsAssignableFrom(type);
		if ((flag && typeToReferenceDrawers.TryGetValue(type, out var value)) || (!flag && typeToDrawers.TryGetValue(type, out value)))
		{
			return value;
		}
		Dictionary<Type, InspectorField[]> dictionary = (flag ? typeToReferenceDrawers : typeToDrawers);
		eligibleDrawers.Clear();
		for (int num = settings.Length - 1; num >= 0; num--)
		{
			InspectorField[] array = (flag ? settings[num].ReferenceDrawers : settings[num].StandardDrawers);
			for (int num2 = array.Length - 1; num2 >= 0; num2--)
			{
				if (array[num2].SupportsType(type))
				{
					eligibleDrawers.Add(array[num2]);
				}
			}
		}
		return dictionary[type] = ((eligibleDrawers.Count > 0) ? eligibleDrawers.ToArray() : null);
	}

	internal void PoolDrawer(InspectorField drawer)
	{
		if (!drawersPool.TryGetValue(drawer.GetType(), out var value))
		{
			value = new List<InspectorField>(poolCapacity);
			drawersPool[drawer.GetType()] = value;
		}
		if (value.Count < poolCapacity)
		{
			drawer.gameObject.SetActive(value: false);
			drawer.transform.SetParent(poolParent, worldPositionStays: false);
			value.Add(drawer);
		}
		else
		{
			UnityEngine.Object.Destroy(drawer.gameObject);
		}
	}

	internal ExposedVariablesEnumerator GetExposedVariablesForType(Type type)
	{
		MemberInfo[] allVariables = type.GetAllVariables();
		if (allVariables == null)
		{
			return new ExposedVariablesEnumerator(null, null, null, VariableVisibility.None, VariableVisibility.None);
		}
		List<VariableSet> list = null;
		List<VariableSet> list2 = null;
		for (int i = 0; i < hiddenVariables.Count; i++)
		{
			if (hiddenVariables[i].type.IsAssignableFrom(type))
			{
				if (list == null)
				{
					list = new List<VariableSet> { hiddenVariables[i] };
				}
				else
				{
					list.Add(hiddenVariables[i]);
				}
			}
		}
		for (int j = 0; j < exposedVariables.Count; j++)
		{
			if (exposedVariables[j].type.IsAssignableFrom(type))
			{
				if (list2 == null)
				{
					list2 = new List<VariableSet> { exposedVariables[j] };
				}
				else
				{
					list2.Add(exposedVariables[j]);
				}
			}
		}
		return new ExposedVariablesEnumerator(allVariables, list, list2, m_exposeFields, m_exposeProperties);
	}
}
