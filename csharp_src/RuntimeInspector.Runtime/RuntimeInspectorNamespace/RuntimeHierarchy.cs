using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class RuntimeHierarchy : SkinnedWindow, IListViewAdapter, ITooltipManager
{
	[Flags]
	public enum SelectOptions
	{
		None = 0,
		Additive = 1,
		FocusOnSelection = 2,
		ForceRevealSelection = 4
	}

	public enum LongPressAction
	{
		None,
		CreateDraggedReferenceItem,
		ShowMultiSelectionToggles,
		ShowMultiSelectionTogglesThenCreateDraggedReferenceItem
	}

	public delegate void SelectionChangedDelegate(ReadOnlyCollection<Transform> selection);

	public delegate void DoubleClickDelegate(HierarchyData clickedItem);

	public delegate bool GameObjectFilterDelegate(Transform transform);

	[SerializeField]
	private float m_refreshInterval;

	[SerializeField]
	private float m_objectNamesRefreshInterval = 10f;

	[SerializeField]
	private float m_searchRefreshInterval = 5f;

	private float nextHierarchyRefreshTime = -1f;

	private float nextObjectNamesRefreshTime = -1f;

	private float nextSearchRefreshTime = -1f;

	[Space]
	[SerializeField]
	private bool m_allowMultiSelection = true;

	private bool m_multiSelectionToggleSelectionMode;

	private bool justActivatedMultiSelectionToggleSelectionMode;

	[Space]
	[SerializeField]
	private bool m_exposeUnityScenes = true;

	[SerializeField]
	[FormerlySerializedAs("exposedScenes")]
	private string[] exposedUnityScenesSubset;

	[SerializeField]
	private bool m_exposeDontDestroyOnLoadScene = true;

	[SerializeField]
	private string[] pseudoScenesOrder;

	[Space]
	[SerializeField]
	private LongPressAction m_pointerLongPressAction = LongPressAction.CreateDraggedReferenceItem;

	[SerializeField]
	[FormerlySerializedAs("m_draggedReferenceHoldTime")]
	private float m_pointerLongPressDuration = 0.4f;

	[SerializeField]
	private float m_doubleClickThreshold = 0.5f;

	[Space]
	[SerializeField]
	private bool m_canReorganizeItems;

	[SerializeField]
	private bool m_canDropDraggedParentOnChild;

	[SerializeField]
	private bool m_canDropDraggedObjectsToPseudoScenes;

	[Space]
	[SerializeField]
	private bool m_showTooltips;

	[SerializeField]
	private float m_tooltipDelay = 0.5f;

	[Space]
	[SerializeField]
	private bool m_showHorizontalScrollbar;

	private bool m_isInSearchMode;

	[SerializeField]
	private RuntimeInspector m_connectedInspector;

	private bool m_isLocked;

	[Header("Internal Variables")]
	[SerializeField]
	private ScrollRect scrollView;

	[SerializeField]
	private RectTransform drawArea;

	[SerializeField]
	private RecycledListView listView;

	[SerializeField]
	private Image background;

	[SerializeField]
	private Image verticalScrollbar;

	[SerializeField]
	private Image horizontalScrollbar;

	[SerializeField]
	private InputField searchInputField;

	[SerializeField]
	private Image searchIcon;

	[SerializeField]
	private Image searchInputFieldBackground;

	[SerializeField]
	private LayoutElement searchBarLayoutElement;

	[SerializeField]
	private Button deselectAllButton;

	[SerializeField]
	private LayoutElement deselectAllLayoutElement;

	[SerializeField]
	private Text deselectAllLabel;

	[SerializeField]
	private Image selectedPathBackground;

	[SerializeField]
	private Text selectedPathText;

	[SerializeField]
	private HierarchyDragDropListener dragDropListener;

	[SerializeField]
	private HierarchyField drawerPrefab;

	[SerializeField]
	private Sprite m_sceneDrawerBackground;

	[SerializeField]
	private Sprite m_transformDrawerBackground;

	private static int aliveHierarchies;

	private bool initialized;

	private readonly List<HierarchyField> drawers = new List<HierarchyField>(32);

	private readonly List<HierarchyDataRoot> sceneData = new List<HierarchyDataRoot>(8);

	private readonly List<HierarchyDataRoot> searchSceneData = new List<HierarchyDataRoot>(8);

	private readonly Dictionary<string, HierarchyDataRootPseudoScene> pseudoSceneDataLookup = new Dictionary<string, HierarchyDataRootPseudoScene>();

	private readonly List<Transform> m_currentSelection = new List<Transform>(16);

	private readonly HashSet<int> currentSelectionSet = new HashSet<int>();

	private readonly HashSet<int> newSelectionSet = new HashSet<int>();

	private Transform multiSelectionPivotTransform;

	private HierarchyDataRoot multiSelectionPivotSceneData;

	private readonly List<int> multiSelectionPivotSiblingIndexTraversalList = new List<int>(8);

	private readonly Transform[] singleTransformSelection = new Transform[1];

	private int totalItemCount;

	private bool selectLock;

	private bool isListViewDirty = true;

	private bool shouldRecalculateContentWidth;

	private float lastClickTime;

	private HierarchyField lastClickedDrawer;

	private HierarchyField currentlyPressedDrawer;

	private float pressedDrawerDraggedReferenceCreateTime;

	private PointerEventData pressedDrawerActivePointer;

	private Canvas m_canvas;

	private float m_autoScrollSpeed;

	private PointerEventData nullPointerEventData;

	public SelectionChangedDelegate OnSelectionChanged;

	public DoubleClickDelegate OnItemDoubleClicked;

	private GameObjectFilterDelegate m_gameObjectDelegate;

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

	public float ObjectNamesRefreshInterval
	{
		get
		{
			return m_objectNamesRefreshInterval;
		}
		set
		{
			m_objectNamesRefreshInterval = value;
		}
	}

	public float SearchRefreshInterval
	{
		get
		{
			return m_searchRefreshInterval;
		}
		set
		{
			m_searchRefreshInterval = value;
		}
	}

	public bool AllowMultiSelection
	{
		get
		{
			return m_allowMultiSelection;
		}
		set
		{
			if (m_allowMultiSelection == value)
			{
				return;
			}
			m_allowMultiSelection = value;
			if (value)
			{
				return;
			}
			MultiSelectionToggleSelectionMode = false;
			if (m_currentSelection.Count <= 1)
			{
				return;
			}
			for (int num = m_currentSelection.Count - 1; num >= 0; num--)
			{
				if ((bool)m_currentSelection[num])
				{
					singleTransformSelection[0] = m_currentSelection[num];
					SelectInternal(singleTransformSelection);
					return;
				}
			}
			DeselectInternal(null);
		}
	}

	public bool MultiSelectionToggleSelectionMode
	{
		get
		{
			return m_multiSelectionToggleSelectionMode;
		}
		set
		{
			if (!m_allowMultiSelection)
			{
				value = false;
			}
			if (m_multiSelectionToggleSelectionMode == value)
			{
				return;
			}
			m_multiSelectionToggleSelectionMode = value;
			shouldRecalculateContentWidth = true;
			for (int num = drawers.Count - 1; num >= 0; num--)
			{
				if (drawers[num].gameObject.activeSelf)
				{
					drawers[num].MultiSelectionToggleVisible = value;
				}
			}
			deselectAllButton.gameObject.SetActive(value);
			if (!value)
			{
				EnsureScrollViewIsWithinBounds();
			}
		}
	}

	public bool ExposeUnityScenes
	{
		get
		{
			return m_exposeUnityScenes;
		}
		set
		{
			if (m_exposeUnityScenes == value)
			{
				return;
			}
			m_exposeUnityScenes = value;
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				if (value)
				{
					OnSceneLoaded(SceneManager.GetSceneAt(i), LoadSceneMode.Single);
				}
				else
				{
					OnSceneUnloaded(SceneManager.GetSceneAt(i));
				}
			}
		}
	}

	public bool ExposeDontDestroyOnLoadScene
	{
		get
		{
			return m_exposeDontDestroyOnLoadScene;
		}
		set
		{
			if (m_exposeDontDestroyOnLoadScene != value)
			{
				m_exposeDontDestroyOnLoadScene = value;
				if (value)
				{
					OnSceneLoaded(GetDontDestroyOnLoadScene(), LoadSceneMode.Single);
				}
				else
				{
					OnSceneUnloaded(GetDontDestroyOnLoadScene());
				}
			}
		}
	}

	public LongPressAction PointerLongPressAction
	{
		get
		{
			return m_pointerLongPressAction;
		}
		set
		{
			m_pointerLongPressAction = value;
		}
	}

	public float PointerLongPressDuration
	{
		get
		{
			return m_pointerLongPressDuration;
		}
		set
		{
			m_pointerLongPressDuration = value;
		}
	}

	public float DoubleClickThreshold
	{
		get
		{
			return m_doubleClickThreshold;
		}
		set
		{
			m_doubleClickThreshold = value;
		}
	}

	public bool CanReorganizeItems
	{
		get
		{
			return m_canReorganizeItems;
		}
		set
		{
			m_canReorganizeItems = value;
		}
	}

	public bool CanDropDraggedParentOnChild
	{
		get
		{
			return m_canDropDraggedParentOnChild;
		}
		set
		{
			m_canDropDraggedParentOnChild = value;
		}
	}

	public bool CanDropDraggedObjectsToPseudoScenes
	{
		get
		{
			return m_canDropDraggedObjectsToPseudoScenes;
		}
		set
		{
			m_canDropDraggedObjectsToPseudoScenes = value;
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

	public bool ShowHorizontalScrollbar
	{
		get
		{
			return m_showHorizontalScrollbar;
		}
		set
		{
			if (m_showHorizontalScrollbar == value)
			{
				return;
			}
			m_showHorizontalScrollbar = value;
			if (!value)
			{
				scrollView.content.sizeDelta = new Vector2(0f, scrollView.content.sizeDelta.y);
				scrollView.horizontalNormalizedPosition = 0f;
			}
			else
			{
				for (int num = drawers.Count - 1; num >= 0; num--)
				{
					if (drawers[num].gameObject.activeSelf)
					{
						drawers[num].RefreshName();
					}
				}
				shouldRecalculateContentWidth = true;
			}
			scrollView.horizontal = value;
		}
	}

	public string SearchTerm
	{
		get
		{
			return searchInputField.text;
		}
		set
		{
			searchInputField.text = value;
		}
	}

	public bool IsInSearchMode => m_isInSearchMode;

	public RuntimeInspector ConnectedInspector
	{
		get
		{
			return m_connectedInspector;
		}
		set
		{
			if (!(m_connectedInspector != value))
			{
				return;
			}
			m_connectedInspector = value;
			for (int num = m_currentSelection.Count - 1; num >= 0; num--)
			{
				if ((bool)m_currentSelection[num])
				{
					m_connectedInspector.Inspect(m_currentSelection[num].gameObject);
					break;
				}
			}
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

	internal Sprite SceneDrawerBackground => m_sceneDrawerBackground;

	internal Sprite TransformDrawerBackground => m_transformDrawerBackground;

	public ReadOnlyCollection<Transform> CurrentSelection => m_currentSelection.AsReadOnly();

	internal int ItemCount => totalItemCount;

	public Canvas Canvas => m_canvas;

	internal float AutoScrollSpeed
	{
		set
		{
			m_autoScrollSpeed = value;
		}
	}

	public GameObjectFilterDelegate GameObjectFilter
	{
		get
		{
			return m_gameObjectDelegate;
		}
		set
		{
			m_gameObjectDelegate = value;
			for (int i = 0; i < sceneData.Count; i++)
			{
				if (sceneData[i].IsExpanded)
				{
					sceneData[i].IsExpanded = false;
					sceneData[i].IsExpanded = true;
				}
			}
			if (!m_isInSearchMode)
			{
				return;
			}
			for (int j = 0; j < searchSceneData.Count; j++)
			{
				if (searchSceneData[j].IsExpanded)
				{
					searchSceneData[j].IsExpanded = false;
					searchSceneData[j].IsExpanded = true;
				}
			}
		}
	}

	int IListViewAdapter.Count => totalItemCount;

	float IListViewAdapter.ItemHeight => base.Skin.LineHeight;

	protected override void Awake()
	{
		base.Awake();
		Initialize();
	}

	private void Initialize()
	{
		if (!initialized)
		{
			initialized = true;
			listView.SetAdapter(this);
			aliveHierarchies++;
			m_canvas = GetComponentInParent<Canvas>();
			nullPointerEventData = new PointerEventData(null);
			searchInputField.onValueChanged.AddListener(OnSearchTermChanged);
			deselectAllButton.onClick.AddListener(delegate
			{
				DeselectInternal(null);
				MultiSelectionToggleSelectionMode = false;
			});
			m_showHorizontalScrollbar = !m_showHorizontalScrollbar;
			ShowHorizontalScrollbar = !m_showHorizontalScrollbar;
			if (m_showTooltips)
			{
				TooltipListener = base.gameObject.AddComponent<TooltipListener>();
				TooltipListener.Initialize(this);
			}
			RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Add(drawArea);
		}
	}

	private void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;
		if (ExposeUnityScenes)
		{
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				OnSceneLoaded(SceneManager.GetSceneAt(i), LoadSceneMode.Single);
			}
		}
		if (ExposeDontDestroyOnLoadScene)
		{
			OnSceneLoaded(GetDontDestroyOnLoadScene(), LoadSceneMode.Single);
		}
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
		if (--aliveHierarchies == 0)
		{
			HierarchyData.ClearPool();
		}
		RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Remove(drawArea);
	}

	private void OnRectTransformDimensionsChange()
	{
		shouldRecalculateContentWidth = true;
	}

	private void OnTransformParentChanged()
	{
		m_canvas = GetComponentInParent<Canvas>();
	}

	protected override void Update()
	{
		base.Update();
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (realtimeSinceStartup > nextHierarchyRefreshTime)
		{
			Refresh();
		}
		if (m_isInSearchMode && realtimeSinceStartup > nextSearchRefreshTime)
		{
			RefreshSearchResults();
		}
		if (isListViewDirty)
		{
			RefreshListView();
		}
		if (realtimeSinceStartup > nextObjectNamesRefreshTime)
		{
			nextObjectNamesRefreshTime = realtimeSinceStartup + m_objectNamesRefreshInterval;
			for (int num = sceneData.Count - 1; num >= 0; num--)
			{
				sceneData[num].ResetCachedNames();
			}
			for (int num2 = searchSceneData.Count - 1; num2 >= 0; num2--)
			{
				searchSceneData[num2].ResetCachedNames();
			}
			for (int num3 = drawers.Count - 1; num3 >= 0; num3--)
			{
				if (drawers[num3].gameObject.activeSelf)
				{
					drawers[num3].RefreshName();
				}
			}
			shouldRecalculateContentWidth = true;
		}
		if (m_showHorizontalScrollbar && shouldRecalculateContentWidth)
		{
			shouldRecalculateContentWidth = false;
			float num4 = 0f;
			for (int num5 = drawers.Count - 1; num5 >= 0; num5--)
			{
				if (drawers[num5].gameObject.activeSelf)
				{
					float preferredWidth = drawers[num5].PreferredWidth;
					if (preferredWidth > num4)
					{
						num4 = preferredWidth;
					}
				}
			}
			if (m_multiSelectionToggleSelectionMode && drawers.Count > 0)
			{
				num4 += (float)base.Skin.LineHeight;
			}
			float num6 = listView.ViewportWidth + scrollView.verticalScrollbarSpacing;
			if (num4 > num6)
			{
				scrollView.content.sizeDelta = new Vector2(num4 - num6, scrollView.content.sizeDelta.y);
			}
			else
			{
				scrollView.content.sizeDelta = new Vector2(0f, scrollView.content.sizeDelta.y);
			}
			EnsureScrollViewIsWithinBounds();
		}
		if (m_pointerLongPressAction != LongPressAction.None && (bool)currentlyPressedDrawer && realtimeSinceStartup > pressedDrawerDraggedReferenceCreateTime)
		{
			if (currentlyPressedDrawer.gameObject.activeSelf && (bool)currentlyPressedDrawer.Data.BoundTransform)
			{
				if (m_pointerLongPressAction == LongPressAction.CreateDraggedReferenceItem || (m_pointerLongPressAction == LongPressAction.ShowMultiSelectionTogglesThenCreateDraggedReferenceItem && (!m_allowMultiSelection || m_multiSelectionToggleSelectionMode)))
				{
					Transform[] array = (currentlyPressedDrawer.IsSelected ? m_currentSelection.ToArray() : new Transform[1] { currentlyPressedDrawer.Data.BoundTransform });
					if (array.Length > 1)
					{
						int num7 = Array.IndexOf(array, currentlyPressedDrawer.Data.BoundTransform);
						if (num7 > 0)
						{
							for (int num8 = num7; num8 > 0; num8--)
							{
								array[num8] = array[num8 - 1];
							}
							array[0] = currentlyPressedDrawer.Data.BoundTransform;
						}
					}
					UnityEngine.Object[] references = array;
					if ((bool)RuntimeInspectorUtils.CreateDraggedReferenceItem(references, pressedDrawerActivePointer, base.Skin, m_canvas))
					{
						((IPointerEnterHandler)dragDropListener).OnPointerEnter(pressedDrawerActivePointer);
					}
				}
				else if (m_allowMultiSelection && !m_multiSelectionToggleSelectionMode)
				{
					if (currentSelectionSet.Add(currentlyPressedDrawer.Data.BoundTransform.GetHashCode()))
					{
						m_currentSelection.Add(currentlyPressedDrawer.Data.BoundTransform);
						currentlyPressedDrawer.IsSelected = true;
						OnCurrentSelectionChanged();
					}
					MultiSelectionToggleSelectionMode = true;
					justActivatedMultiSelectionToggleSelectionMode = true;
					if ((bool)TooltipListener)
					{
						TooltipListener.OnDrawerHovered(null, null, isHovering: false);
					}
				}
			}
			currentlyPressedDrawer = null;
			pressedDrawerActivePointer = null;
		}
		if (m_autoScrollSpeed != 0f)
		{
			scrollView.verticalNormalizedPosition = Mathf.Clamp01(scrollView.verticalNormalizedPosition + m_autoScrollSpeed * Time.unscaledDeltaTime / (float)totalItemCount);
		}
	}

	public void Refresh()
	{
		nextHierarchyRefreshTime = Time.realtimeSinceStartup + m_refreshInterval;
		bool flag = false;
		for (int i = 0; i < sceneData.Count; i++)
		{
			flag |= sceneData[i].Refresh();
		}
		if (flag)
		{
			isListViewDirty = true;
			return;
		}
		for (int num = drawers.Count - 1; num >= 0; num--)
		{
			if (drawers[num].gameObject.activeSelf)
			{
				drawers[num].Refresh();
			}
		}
	}

	private void RefreshListView()
	{
		isListViewDirty = false;
		totalItemCount = 0;
		if (!m_isInSearchMode)
		{
			for (int num = sceneData.Count - 1; num >= 0; num--)
			{
				totalItemCount += sceneData[num].Height;
			}
		}
		else
		{
			for (int num2 = searchSceneData.Count - 1; num2 >= 0; num2--)
			{
				totalItemCount += searchSceneData[num2].Height;
			}
		}
		listView.UpdateList(resetContentPosition: false);
		EnsureScrollViewIsWithinBounds();
	}

	internal void SetListViewDirty()
	{
		isListViewDirty = true;
	}

	public void RefreshSearchResults()
	{
		if (!m_isInSearchMode)
		{
			return;
		}
		nextSearchRefreshTime = Time.realtimeSinceStartup + m_searchRefreshInterval;
		for (int i = 0; i < searchSceneData.Count; i++)
		{
			HierarchyDataRootSearch hierarchyDataRootSearch = (HierarchyDataRootSearch)searchSceneData[i];
			bool canExpand = hierarchyDataRootSearch.CanExpand;
			hierarchyDataRootSearch.Refresh();
			if (hierarchyDataRootSearch.CanExpand && !canExpand)
			{
				hierarchyDataRootSearch.IsExpanded = true;
			}
			isListViewDirty = true;
		}
	}

	public void RefreshNameOf(Transform target)
	{
		if (!target)
		{
			return;
		}
		Scene scene = target.gameObject.scene;
		for (int num = sceneData.Count - 1; num >= 0; num--)
		{
			HierarchyDataRoot hierarchyDataRoot = sceneData[num];
			if (hierarchyDataRoot is HierarchyDataRootPseudoScene || ((HierarchyDataRootScene)hierarchyDataRoot).Scene == scene)
			{
				sceneData[num].RefreshNameOf(target);
			}
		}
		if (m_isInSearchMode)
		{
			RefreshSearchResults();
			for (int num2 = searchSceneData.Count - 1; num2 >= 0; num2--)
			{
				searchSceneData[num2].RefreshNameOf(target);
			}
		}
		for (int num3 = drawers.Count - 1; num3 >= 0; num3--)
		{
			if (drawers[num3].gameObject.activeSelf && drawers[num3].Data.BoundTransform == target)
			{
				drawers[num3].RefreshName();
			}
		}
		shouldRecalculateContentWidth = true;
	}

	protected override void RefreshSkin()
	{
		background.color = base.Skin.BackgroundColor;
		verticalScrollbar.color = base.Skin.ScrollbarColor;
		horizontalScrollbar.color = base.Skin.ScrollbarColor;
		searchInputField.textComponent.SetSkinInputFieldText(base.Skin);
		searchInputFieldBackground.color = base.Skin.InputFieldNormalBackgroundColor.Tint(0.08f);
		searchIcon.color = base.Skin.ButtonTextColor;
		searchBarLayoutElement.SetHeight(base.Skin.LineHeight);
		deselectAllLayoutElement.SetHeight(base.Skin.LineHeight);
		deselectAllButton.targetGraphic.color = base.Skin.InputFieldInvalidBackgroundColor;
		deselectAllLabel.SetSkinInputFieldText(base.Skin);
		selectedPathBackground.color = base.Skin.BackgroundColor.Tint(0.1f);
		selectedPathText.SetSkinButtonText(base.Skin);
		Text text = searchInputField.placeholder as Text;
		if (text != null)
		{
			float a = text.color.a;
			text.SetSkinInputFieldText(base.Skin);
			Color color = text.color;
			color.a = a;
			text.color = color;
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(drawArea);
		listView.ResetList();
	}

	private void EnsureScrollViewIsWithinBounds()
	{
		if (scrollView.verticalNormalizedPosition <= Mathf.Epsilon)
		{
			scrollView.verticalNormalizedPosition = 0.0001f;
		}
		scrollView.OnScroll(nullPointerEventData);
	}

	void IListViewAdapter.SetItemContent(RecycledListItem item)
	{
		if (isListViewDirty)
		{
			RefreshListView();
		}
		HierarchyField hierarchyField = (HierarchyField)item;
		HierarchyData dataAt = GetDataAt(hierarchyField.Position);
		if (dataAt != null)
		{
			hierarchyField.Skin = base.Skin;
			hierarchyField.SetContent(dataAt);
			hierarchyField.IsSelected = (bool)dataAt.BoundTransform && currentSelectionSet.Contains(dataAt.BoundTransform.GetHashCode());
			hierarchyField.MultiSelectionToggleVisible = m_multiSelectionToggleSelectionMode;
			hierarchyField.Refresh();
			shouldRecalculateContentWidth = true;
		}
	}

	void IListViewAdapter.OnItemClicked(RecycledListItem item)
	{
		HierarchyField hierarchyField = (HierarchyField)item;
		if (OnItemDoubleClicked != null && hierarchyField == lastClickedDrawer && Time.realtimeSinceStartup - lastClickTime <= m_doubleClickThreshold)
		{
			lastClickTime = 0f;
			OnItemDoubleClicked(lastClickedDrawer.Data);
			return;
		}
		lastClickTime = Time.realtimeSinceStartup;
		lastClickedDrawer = hierarchyField;
		bool flag = false;
		Transform boundTransform = hierarchyField.Data.BoundTransform;
		int item2 = (boundTransform ? boundTransform.GetHashCode() : (-1));
		multiSelectionPivotTransform = boundTransform;
		multiSelectionPivotSceneData = hierarchyField.Data.Root;
		hierarchyField.Data.GetSiblingIndexTraversalList(multiSelectionPivotSiblingIndexTraversalList);
		if (m_allowMultiSelection && m_multiSelectionToggleSelectionMode)
		{
			if ((bool)boundTransform)
			{
				if (currentSelectionSet.Add(item2))
				{
					m_currentSelection.Add(boundTransform);
				}
				else
				{
					m_currentSelection.Remove(boundTransform);
					currentSelectionSet.Remove(item2);
					if (m_currentSelection.Count == 0)
					{
						MultiSelectionToggleSelectionMode = false;
					}
				}
				flag = true;
			}
		}
		else if ((bool)boundTransform)
		{
			if (m_currentSelection.Count != 1 || m_currentSelection[0] != boundTransform)
			{
				m_currentSelection.Clear();
				currentSelectionSet.Clear();
				m_currentSelection.Add(boundTransform);
				currentSelectionSet.Add(item2);
				flag = true;
			}
		}
		else if (m_currentSelection.Count > 0)
		{
			m_currentSelection.Clear();
			currentSelectionSet.Clear();
			flag = true;
		}
		if (flag)
		{
			for (int num = drawers.Count - 1; num >= 0; num--)
			{
				if (drawers[num].gameObject.activeSelf)
				{
					Transform boundTransform2 = drawers[num].Data.BoundTransform;
					if ((bool)boundTransform2)
					{
						if (drawers[num].IsSelected != currentSelectionSet.Contains(boundTransform2.GetHashCode()))
						{
							drawers[num].IsSelected = !drawers[num].IsSelected;
						}
					}
					else if (drawers[num].IsSelected)
					{
						drawers[num].IsSelected = false;
					}
				}
			}
			OnCurrentSelectionChanged();
		}
		if (!m_isInSearchMode)
		{
			return;
		}
		bool flag2 = false;
		for (int num2 = m_currentSelection.Count - 1; num2 >= 0; num2--)
		{
			Transform transform = m_currentSelection[num2];
			if ((bool)transform)
			{
				StringBuilder stringBuilder = RuntimeInspectorUtils.stringBuilder;
				stringBuilder.Length = 0;
				stringBuilder.AppendLine("Path:");
				while ((bool)transform)
				{
					stringBuilder.Append("  ").AppendLine(transform.name);
					transform = transform.parent;
				}
				selectedPathText.text = stringBuilder.Append("  ").Append(hierarchyField.Data.Root.Name).ToString();
				flag2 = true;
				break;
			}
		}
		if (selectedPathBackground.gameObject.activeSelf != flag2)
		{
			selectedPathBackground.gameObject.SetActive(flag2);
		}
	}

	private bool FindMultiSelectionPivotAbsoluteIndex(out int pivotAbsoluteIndex)
	{
		pivotAbsoluteIndex = 0;
		if (multiSelectionPivotSceneData == null)
		{
			return false;
		}
		bool flag = false;
		List<HierarchyDataRoot> list = (m_isInSearchMode ? searchSceneData : sceneData);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != multiSelectionPivotSceneData)
			{
				pivotAbsoluteIndex += list[i].Height;
				continue;
			}
			flag = true;
			break;
		}
		if (!flag)
		{
			return false;
		}
		if (multiSelectionPivotSiblingIndexTraversalList.Count == 0)
		{
			return true;
		}
		if (!multiSelectionPivotTransform)
		{
			return false;
		}
		HierarchyData hierarchyData = multiSelectionPivotSceneData.TraverseSiblingIndexList(multiSelectionPivotSiblingIndexTraversalList);
		if (hierarchyData != null && hierarchyData.BoundTransform == multiSelectionPivotTransform)
		{
			pivotAbsoluteIndex += hierarchyData.AbsoluteIndex;
			return true;
		}
		hierarchyData = multiSelectionPivotSceneData.FindTransformInVisibleChildren(multiSelectionPivotTransform, (multiSelectionPivotSceneData is HierarchyDataRootPseudoScene) ? multiSelectionPivotSiblingIndexTraversalList.Count : (-1));
		if (hierarchyData != null)
		{
			pivotAbsoluteIndex += hierarchyData.AbsoluteIndex;
			return true;
		}
		if (multiSelectionPivotSceneData is HierarchyDataRootPseudoScene)
		{
			hierarchyData = multiSelectionPivotSceneData.FindTransformInVisibleChildren(multiSelectionPivotTransform);
			if (hierarchyData != null)
			{
				pivotAbsoluteIndex += hierarchyData.AbsoluteIndex;
				return true;
			}
		}
		return false;
	}

	internal HierarchyData GetDataAt(int index)
	{
		List<HierarchyDataRoot> list = ((!m_isInSearchMode) ? sceneData : searchSceneData);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Depth < 0)
			{
				continue;
			}
			if (index < list[i].Height)
			{
				if (index <= 0)
				{
					return list[i];
				}
				return list[i].FindDataAtIndex(index - 1);
			}
			index -= list[i].Height;
		}
		return null;
	}

	public void OnDrawerPointerEvent(HierarchyField drawer, PointerEventData eventData, bool isPointerDown)
	{
		if (!isPointerDown)
		{
			currentlyPressedDrawer = null;
			pressedDrawerActivePointer = null;
			if (justActivatedMultiSelectionToggleSelectionMode)
			{
				justActivatedMultiSelectionToggleSelectionMode = false;
				eventData.eligibleForClick = false;
			}
		}
		else if (m_pointerLongPressAction != LongPressAction.None)
		{
			currentlyPressedDrawer = drawer;
			pressedDrawerActivePointer = eventData;
			pressedDrawerDraggedReferenceCreateTime = Time.realtimeSinceStartup + m_pointerLongPressDuration;
		}
	}

	public bool Select(Transform selection, SelectOptions selectOptions = SelectOptions.None)
	{
		singleTransformSelection[0] = selection;
		return Select(singleTransformSelection, selectOptions);
	}

	public bool Select(IList<Transform> selection, SelectOptions selectOptions = SelectOptions.None)
	{
		if (!m_isLocked)
		{
			return SelectInternal(selection, selectOptions);
		}
		return false;
	}

	internal bool SelectInternal(IList<Transform> selection, SelectOptions selectOptions = SelectOptions.None)
	{
		if (selectLock)
		{
			return false;
		}
		if (selection.IsEmpty())
		{
			DeselectInternal(null);
			return true;
		}
		Initialize();
		bool flag = (selectOptions & SelectOptions.Additive) == SelectOptions.Additive;
		if (!m_allowMultiSelection)
		{
			flag = false;
			if (selection.Count > 1)
			{
				for (int num = selection.Count - 1; num >= 0; num--)
				{
					if (CanSelectTransform(selection[num]))
					{
						singleTransformSelection[0] = selection[num];
						selection = singleTransformSelection;
						break;
					}
				}
			}
		}
		bool flag2 = false;
		if (flag)
		{
			for (int i = 0; i < selection.Count; i++)
			{
				Transform transform = selection[i];
				if (CanSelectTransform(transform) && currentSelectionSet.Add(transform.GetHashCode()))
				{
					m_currentSelection.Add(transform);
					flag2 = true;
				}
			}
		}
		else
		{
			newSelectionSet.Clear();
			for (int j = 0; j < selection.Count; j++)
			{
				Transform transform2 = selection[j];
				if (CanSelectTransform(transform2))
				{
					int hashCode = transform2.GetHashCode();
					newSelectionSet.Add(hashCode);
					if (currentSelectionSet.Add(hashCode))
					{
						m_currentSelection.Add(transform2);
						flag2 = true;
					}
				}
			}
			for (int num2 = m_currentSelection.Count - 1; num2 >= 0; num2--)
			{
				Transform transform3 = m_currentSelection[num2];
				if ((bool)transform3)
				{
					int hashCode2 = transform3.GetHashCode();
					if (!newSelectionSet.Contains(hashCode2))
					{
						m_currentSelection.RemoveAt(num2);
						currentSelectionSet.Remove(hashCode2);
						flag2 = true;
					}
				}
			}
		}
		if (!flag2 && (selectOptions & SelectOptions.ForceRevealSelection) != SelectOptions.ForceRevealSelection)
		{
			return true;
		}
		if (flag2)
		{
			OnCurrentSelectionChanged();
		}
		Refresh();
		RefreshSearchResults();
		HierarchyDataTransform hierarchyDataTransform = null;
		int num3 = 0;
		List<HierarchyDataRoot> list = (m_isInSearchMode ? searchSceneData : sceneData);
		for (int k = 0; k < m_currentSelection.Count; k++)
		{
			Transform transform4 = m_currentSelection[k];
			if (!transform4)
			{
				continue;
			}
			Scene scene = transform4.gameObject.scene;
			for (int l = 0; l < list.Count; l++)
			{
				HierarchyDataRoot hierarchyDataRoot = list[l];
				if (m_isInSearchMode || hierarchyDataRoot is HierarchyDataRootPseudoScene || ((HierarchyDataRootScene)hierarchyDataRoot).Scene == scene)
				{
					HierarchyDataTransform hierarchyDataTransform2 = hierarchyDataRoot.FindTransform(transform4);
					if (hierarchyDataTransform2 != null)
					{
						hierarchyDataTransform = hierarchyDataTransform2;
						num3 = l;
					}
				}
			}
		}
		RefreshListView();
		if (hierarchyDataTransform != null)
		{
			if ((selectOptions & SelectOptions.FocusOnSelection) == SelectOptions.FocusOnSelection)
			{
				int num4 = hierarchyDataTransform.AbsoluteIndex;
				for (int m = 0; m < num3; m++)
				{
					num4 += list[m].Height;
				}
				LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)base.transform);
				float height = drawArea.rect.height;
				float height2 = ((RectTransform)drawArea.parent).rect.height;
				if (height > height2)
				{
					float num5 = (float)num4 / (float)totalItemCount * height + (float)base.Skin.LineHeight * 0.5f;
					scrollView.verticalNormalizedPosition = 1f - Mathf.Clamp01((num5 - height2 * 0.5f) / (height - height2));
				}
			}
			return true;
		}
		return false;
	}

	public void Deselect()
	{
		Deselect((IList<Transform>)null);
	}

	public void Deselect(Transform deselection)
	{
		singleTransformSelection[0] = deselection;
		Deselect(singleTransformSelection);
	}

	public void Deselect(IList<Transform> deselection)
	{
		if (!m_isLocked)
		{
			DeselectInternal(deselection);
		}
	}

	internal void DeselectInternal(IList<Transform> deselection)
	{
		if (selectLock || m_currentSelection.Count == 0)
		{
			return;
		}
		Initialize();
		bool flag = false;
		if (deselection == null)
		{
			m_currentSelection.Clear();
			currentSelectionSet.Clear();
			flag = true;
		}
		else
		{
			for (int num = deselection.Count - 1; num >= 0; num--)
			{
				Transform transform = deselection[num];
				if ((bool)transform && currentSelectionSet.Remove(transform.GetHashCode()))
				{
					m_currentSelection.Remove(transform);
					flag = true;
				}
			}
		}
		if (!flag)
		{
			return;
		}
		for (int num2 = drawers.Count - 1; num2 >= 0; num2--)
		{
			if (drawers[num2].gameObject.activeSelf && drawers[num2].IsSelected)
			{
				drawers[num2].IsSelected = false;
			}
		}
		OnCurrentSelectionChanged();
	}

	public bool IsSelected(Transform transform)
	{
		if ((bool)transform)
		{
			return currentSelectionSet.Contains(transform.GetHashCode());
		}
		return false;
	}

	private bool CanSelectTransform(Transform transform)
	{
		if (!transform)
		{
			return false;
		}
		if (RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Contains(transform) || (m_gameObjectDelegate != null && !m_gameObjectDelegate(transform)))
		{
			return false;
		}
		Transform transform2 = null;
		for (int i = 0; i < sceneData.Count; i++)
		{
			Transform nearestRootOf = sceneData[i].GetNearestRootOf(transform);
			if ((bool)nearestRootOf && (!transform2 || nearestRootOf.IsChildOf(transform2)))
			{
				transform2 = nearestRootOf;
			}
		}
		if (!transform2)
		{
			return false;
		}
		if (transform2 != transform)
		{
			Transform parent = transform.parent;
			while ((bool)parent && parent != transform2)
			{
				if (RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Contains(parent) || (m_gameObjectDelegate != null && !m_gameObjectDelegate(parent)))
				{
					return false;
				}
				parent = parent.parent;
			}
		}
		return true;
	}

	private void OnCurrentSelectionChanged()
	{
		selectLock = true;
		try
		{
			if ((bool)m_connectedInspector)
			{
				for (int num = m_currentSelection.Count - 1; num >= 0; num--)
				{
					if ((bool)m_currentSelection[num])
					{
						m_connectedInspector.Inspect(m_currentSelection[num].gameObject);
						break;
					}
				}
			}
			if (OnSelectionChanged != null)
			{
				OnSelectionChanged(m_currentSelection.AsReadOnly());
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		finally
		{
			selectLock = false;
		}
	}

	private void OnSearchTermChanged(string search)
	{
		if (search != null)
		{
			search = search.Trim();
		}
		if (string.IsNullOrEmpty(search))
		{
			if (m_isInSearchMode)
			{
				for (int i = 0; i < searchSceneData.Count; i++)
				{
					searchSceneData[i].IsExpanded = false;
				}
				scrollView.verticalNormalizedPosition = 1f;
				selectedPathBackground.gameObject.SetActive(value: false);
				isListViewDirty = true;
				m_isInSearchMode = false;
				if (m_currentSelection.Count > 0)
				{
					SelectInternal(m_currentSelection, SelectOptions.FocusOnSelection | SelectOptions.ForceRevealSelection);
				}
			}
		}
		else if (!m_isInSearchMode)
		{
			scrollView.verticalNormalizedPosition = 1f;
			nextSearchRefreshTime = Time.realtimeSinceStartup + m_searchRefreshInterval;
			isListViewDirty = true;
			m_isInSearchMode = true;
			RefreshSearchResults();
			for (int j = 0; j < searchSceneData.Count; j++)
			{
				searchSceneData[j].IsExpanded = true;
			}
		}
		else
		{
			RefreshSearchResults();
		}
	}

	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		if (!ExposeUnityScenes || (arg0.buildIndex >= 0 && exposedUnityScenesSubset != null && exposedUnityScenesSubset.Length != 0 && Array.IndexOf(exposedUnityScenesSubset, arg0.name) == -1) || !arg0.IsValid())
		{
			return;
		}
		for (int i = 0; i < sceneData.Count; i++)
		{
			if (sceneData[i] is HierarchyDataRootScene && ((HierarchyDataRootScene)sceneData[i]).Scene == arg0)
			{
				return;
			}
		}
		HierarchyDataRootScene hierarchyDataRootScene = new HierarchyDataRootScene(this, arg0);
		hierarchyDataRootScene.Refresh();
		int index = sceneData.Count - pseudoSceneDataLookup.Count;
		sceneData.Insert(index, hierarchyDataRootScene);
		searchSceneData.Insert(index, new HierarchyDataRootSearch(this, hierarchyDataRootScene));
		isListViewDirty = true;
	}

	private void OnSceneUnloaded(Scene arg0)
	{
		for (int i = 0; i < sceneData.Count; i++)
		{
			if (sceneData[i] is HierarchyDataRootScene && ((HierarchyDataRootScene)sceneData[i]).Scene == arg0)
			{
				sceneData[i].IsExpanded = false;
				sceneData.RemoveAt(i);
				searchSceneData[i].IsExpanded = false;
				searchSceneData.RemoveAt(i);
				isListViewDirty = true;
				break;
			}
		}
	}

	private Scene GetDontDestroyOnLoadScene()
	{
		GameObject gameObject = null;
		try
		{
			gameObject = new GameObject();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Scene scene = gameObject.scene;
			UnityEngine.Object.DestroyImmediate(gameObject);
			gameObject = null;
			return scene;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			return default(Scene);
		}
		finally
		{
			if (gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
		}
	}

	public void AddToPseudoScene(string scene, Transform transform)
	{
		GetPseudoScene(scene, createIfNotExists: true).AddChild(transform);
	}

	public void AddToPseudoScene(string scene, IEnumerable<Transform> transforms)
	{
		HierarchyDataRootPseudoScene pseudoScene = GetPseudoScene(scene, createIfNotExists: true);
		foreach (Transform transform in transforms)
		{
			pseudoScene.AddChild(transform);
		}
	}

	public void RemoveFromPseudoScene(string scene, Transform transform, bool deleteSceneIfEmpty)
	{
		HierarchyDataRootPseudoScene pseudoScene = GetPseudoScene(scene, createIfNotExists: false);
		if (pseudoScene != null)
		{
			pseudoScene.RemoveChild(transform);
			if (deleteSceneIfEmpty && pseudoScene.ChildCount == 0)
			{
				DeletePseudoScene(scene);
			}
		}
	}

	public void RemoveFromPseudoScene(string scene, IEnumerable<Transform> transforms, bool deleteSceneIfEmpty)
	{
		HierarchyDataRootPseudoScene pseudoScene = GetPseudoScene(scene, createIfNotExists: false);
		if (pseudoScene == null)
		{
			return;
		}
		foreach (Transform transform in transforms)
		{
			pseudoScene.RemoveChild(transform);
		}
		if (deleteSceneIfEmpty && pseudoScene.ChildCount == 0)
		{
			DeletePseudoScene(scene);
		}
	}

	private HierarchyDataRootPseudoScene GetPseudoScene(string scene, bool createIfNotExists)
	{
		if (pseudoSceneDataLookup.TryGetValue(scene, out var value))
		{
			return value;
		}
		if (createIfNotExists)
		{
			return CreatePseudoSceneInternal(scene);
		}
		return null;
	}

	public void CreatePseudoScene(string scene)
	{
		if (!pseudoSceneDataLookup.ContainsKey(scene))
		{
			CreatePseudoSceneInternal(scene);
		}
	}

	private HierarchyDataRootPseudoScene CreatePseudoSceneInternal(string scene)
	{
		int num = 0;
		if (pseudoScenesOrder.Length != 0)
		{
			for (int i = 0; i < pseudoScenesOrder.Length && !(pseudoScenesOrder[i] == scene); i++)
			{
				if (pseudoSceneDataLookup.ContainsKey(pseudoScenesOrder[i]))
				{
					num++;
				}
			}
		}
		else
		{
			num = pseudoSceneDataLookup.Count;
		}
		HierarchyDataRootPseudoScene hierarchyDataRootPseudoScene = new HierarchyDataRootPseudoScene(this, scene);
		num += sceneData.Count - pseudoSceneDataLookup.Count;
		sceneData.Insert(num, hierarchyDataRootPseudoScene);
		searchSceneData.Insert(num, new HierarchyDataRootSearch(this, hierarchyDataRootPseudoScene));
		pseudoSceneDataLookup[scene] = hierarchyDataRootPseudoScene;
		isListViewDirty = true;
		return hierarchyDataRootPseudoScene;
	}

	public void DeleteAllPseudoScenes()
	{
		for (int num = sceneData.Count - 1; num >= 0; num--)
		{
			if (sceneData[num] is HierarchyDataRootPseudoScene)
			{
				sceneData[num].IsExpanded = false;
				sceneData.RemoveAt(num);
				searchSceneData[num].IsExpanded = false;
				searchSceneData.RemoveAt(num);
			}
		}
		pseudoSceneDataLookup.Clear();
		isListViewDirty = true;
	}

	public void DeletePseudoScene(string scene)
	{
		for (int i = 0; i < sceneData.Count; i++)
		{
			if (sceneData[i] is HierarchyDataRootPseudoScene hierarchyDataRootPseudoScene && hierarchyDataRootPseudoScene.Name == scene)
			{
				pseudoSceneDataLookup.Remove(hierarchyDataRootPseudoScene.Name);
				sceneData[i].IsExpanded = false;
				sceneData.RemoveAt(i);
				searchSceneData[i].IsExpanded = false;
				searchSceneData.RemoveAt(i);
				isListViewDirty = true;
				break;
			}
		}
	}

	RecycledListItem IListViewAdapter.CreateItem(Transform parent)
	{
		HierarchyField hierarchyField = UnityEngine.Object.Instantiate(drawerPrefab, parent, worldPositionStays: false);
		hierarchyField.Initialize(this);
		hierarchyField.Skin = base.Skin;
		drawers.Add(hierarchyField);
		return hierarchyField;
	}
}
