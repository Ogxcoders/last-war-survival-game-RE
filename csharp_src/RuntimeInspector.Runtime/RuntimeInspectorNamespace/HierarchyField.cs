using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class HierarchyField : RecycledListItem, ITooltipContent
{
	private enum ExpandedState
	{
		Collapsed,
		Expanded,
		ArrowHidden
	}

	private const float INACTIVE_ITEM_TEXT_ALPHA = 0.57f;

	private const float TEXT_X_OFFSET = 35f;

	[SerializeField]
	private RectTransform contentTransform;

	[SerializeField]
	private Text nameText;

	[SerializeField]
	private PointerEventListener clickListener;

	[SerializeField]
	private PointerEventListener expandToggle;

	[SerializeField]
	private Image expandArrow;

	[SerializeField]
	private Toggle multiSelectionToggle;

	[SerializeField]
	private Image multiSelectionToggleBackground;

	private RectTransform rectTransform;

	private Image background;

	private int m_skinVersion;

	private UISkin m_skin;

	private bool m_isSelected;

	private bool m_isActive;

	private ExpandedState m_isExpanded;

	public UISkin Skin
	{
		get
		{
			return m_skin;
		}
		set
		{
			if (m_skin != value || m_skinVersion != m_skin.Version)
			{
				m_skin = value;
				m_skinVersion = m_skin.Version;
				rectTransform.sizeDelta = new Vector2(0f, Skin.LineHeight);
				nameText.SetSkinText(Skin);
				expandArrow.color = Skin.ExpandArrowColor;
				nameText.rectTransform.anchoredPosition = new Vector2(Skin.ExpandArrowSpacing + (float)Skin.LineHeight * 0.75f, 0f);
				((RectTransform)expandToggle.transform).sizeDelta = new Vector2(Skin.LineHeight, Skin.LineHeight);
				((RectTransform)multiSelectionToggle.transform).sizeDelta = new Vector2((float)Skin.LineHeight * 0.8f, (float)Skin.LineHeight * 0.8f);
				multiSelectionToggle.graphic.color = Skin.ToggleCheckmarkColor;
				multiSelectionToggleBackground.color = Skin.InputFieldNormalBackgroundColor;
			}
		}
	}

	public bool IsSelected
	{
		get
		{
			return m_isSelected;
		}
		set
		{
			m_isSelected = value;
			Color color;
			if (m_isSelected)
			{
				background.color = Skin.SelectedItemBackgroundColor;
				color = Skin.SelectedItemTextColor;
			}
			else
			{
				background.color = ((Data.Depth == 0) ? Skin.BackgroundColor.Tint(0.075f) : Color.clear);
				color = Skin.TextColor;
			}
			color.a = (m_isActive ? 1f : 0.57f);
			nameText.color = color;
			multiSelectionToggle.isOn = m_isSelected;
		}
	}

	private bool IsActive
	{
		get
		{
			return m_isActive;
		}
		set
		{
			if (m_isActive != value)
			{
				m_isActive = value;
				Color color = nameText.color;
				color.a = (m_isActive ? 1f : 0.57f);
				nameText.color = color;
			}
		}
	}

	public bool MultiSelectionToggleVisible
	{
		get
		{
			return multiSelectionToggle.gameObject.activeSelf;
		}
		set
		{
			if (Data == null || Data.Depth <= 0)
			{
				value = false;
			}
			if (multiSelectionToggle.gameObject.activeSelf != value)
			{
				multiSelectionToggle.gameObject.SetActive(value);
				contentTransform.anchoredPosition = new Vector2((float)(Skin.IndentAmount * Data.Depth) + (value ? ((float)Skin.LineHeight * 0.8f) : 0f), 0f);
			}
		}
	}

	private ExpandedState IsExpanded
	{
		get
		{
			return m_isExpanded;
		}
		set
		{
			if (m_isExpanded != value)
			{
				m_isExpanded = value;
				if (m_isExpanded == ExpandedState.ArrowHidden)
				{
					expandToggle.gameObject.SetActive(value: false);
					return;
				}
				expandToggle.gameObject.SetActive(value: true);
				expandArrow.rectTransform.localEulerAngles = ((m_isExpanded == ExpandedState.Expanded) ? new Vector3(0f, 0f, -90f) : Vector3.zero);
			}
		}
	}

	bool ITooltipContent.IsActive
	{
		get
		{
			if ((bool)this)
			{
				return base.gameObject.activeSelf;
			}
			return false;
		}
	}

	string ITooltipContent.TooltipText => Data.Name;

	public float PreferredWidth { get; private set; }

	public RuntimeHierarchy Hierarchy { get; private set; }

	public HierarchyData Data { get; private set; }

	public void Initialize(RuntimeHierarchy hierarchy)
	{
		Hierarchy = hierarchy;
		rectTransform = (RectTransform)base.transform;
		background = clickListener.GetComponent<Image>();
		if (hierarchy.ShowTooltips)
		{
			clickListener.gameObject.AddComponent<TooltipArea>().Initialize(hierarchy.TooltipListener, this);
		}
		expandToggle.PointerClick += delegate
		{
			ToggleExpandedState();
		};
		clickListener.PointerClick += delegate
		{
			OnClick();
		};
		clickListener.PointerDown += OnPointerDown;
		clickListener.PointerUp += OnPointerUp;
	}

	public void SetContent(HierarchyData data)
	{
		Data = data;
		contentTransform.anchoredPosition = new Vector2((float)(Skin.IndentAmount * data.Depth) + (MultiSelectionToggleVisible ? ((float)Skin.LineHeight * 0.8f) : 0f), 0f);
		background.sprite = ((data.Depth == 0) ? Hierarchy.SceneDrawerBackground : Hierarchy.TransformDrawerBackground);
		RefreshName();
	}

	private void ToggleExpandedState()
	{
		Data.IsExpanded = !Data.IsExpanded;
	}

	public void Refresh()
	{
		IsActive = Data.IsActive;
		IsExpanded = ((!Data.CanExpand) ? ExpandedState.ArrowHidden : (Data.IsExpanded ? ExpandedState.Expanded : ExpandedState.Collapsed));
	}

	public void RefreshName()
	{
		nameText.text = Data.Name;
		if (Hierarchy.ShowHorizontalScrollbar)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(nameText.rectTransform);
			PreferredWidth = (float)(Data.Depth * m_skin.IndentAmount) + 35f + nameText.rectTransform.sizeDelta.x;
		}
	}

	private void OnPointerDown(PointerEventData eventData)
	{
		Hierarchy.OnDrawerPointerEvent(this, eventData, isPointerDown: true);
	}

	private void OnPointerUp(PointerEventData eventData)
	{
		Hierarchy.OnDrawerPointerEvent(this, eventData, isPointerDown: false);
	}
}
