using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI;

[AddComponentMenu("UI/Selectable", 70)]
[ExecuteAlways]
[SelectionBase]
[DisallowMultipleComponent]
public class Selectable : UIBehaviour, IMoveHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
	public enum Transition
	{
		None,
		ColorTint,
		SpriteSwap,
		Animation
	}

	protected enum SelectionState
	{
		Normal,
		Highlighted,
		Pressed,
		Selected,
		Disabled
	}

	protected static Selectable[] s_Selectables = new Selectable[10];

	protected static int s_SelectableCount = 0;

	private bool m_EnableCalled;

	[FormerlySerializedAs("navigation")]
	[SerializeField]
	private Navigation m_Navigation = Navigation.defaultNavigation;

	[FormerlySerializedAs("transition")]
	[SerializeField]
	private Transition m_Transition = Transition.ColorTint;

	[FormerlySerializedAs("colors")]
	[SerializeField]
	private ColorBlock m_Colors = ColorBlock.defaultColorBlock;

	[FormerlySerializedAs("spriteState")]
	[SerializeField]
	private SpriteState m_SpriteState;

	[FormerlySerializedAs("animationTriggers")]
	[SerializeField]
	private AnimationTriggers m_AnimationTriggers = new AnimationTriggers();

	[Tooltip("Can the Selectable be interacted with?")]
	[SerializeField]
	private bool m_Interactable = true;

	[FormerlySerializedAs("highlightGraphic")]
	[FormerlySerializedAs("m_HighlightGraphic")]
	[SerializeField]
	private Graphic m_TargetGraphic;

	private bool m_GroupsAllowInteraction = true;

	protected int m_CurrentIndex = -1;

	private readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();

	public static Selectable[] allSelectablesArray
	{
		get
		{
			Selectable[] array = new Selectable[s_SelectableCount];
			Array.Copy(s_Selectables, array, s_SelectableCount);
			return array;
		}
	}

	public static int allSelectableCount => s_SelectableCount;

	[Obsolete("Replaced with allSelectablesArray to have better performance when disabling a element", false)]
	public static List<Selectable> allSelectables => new List<Selectable>(allSelectablesArray);

	public Navigation navigation
	{
		get
		{
			return m_Navigation;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_Navigation, value))
			{
				OnSetProperty();
			}
		}
	}

	public Transition transition
	{
		get
		{
			return m_Transition;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_Transition, value))
			{
				OnSetProperty();
			}
		}
	}

	public ColorBlock colors
	{
		get
		{
			return m_Colors;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_Colors, value))
			{
				OnSetProperty();
			}
		}
	}

	public SpriteState spriteState
	{
		get
		{
			return m_SpriteState;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_SpriteState, value))
			{
				OnSetProperty();
			}
		}
	}

	public AnimationTriggers animationTriggers
	{
		get
		{
			return m_AnimationTriggers;
		}
		set
		{
			if (SetPropertyUtility.SetClass(ref m_AnimationTriggers, value))
			{
				OnSetProperty();
			}
		}
	}

	public Graphic targetGraphic
	{
		get
		{
			return m_TargetGraphic;
		}
		set
		{
			if (SetPropertyUtility.SetClass(ref m_TargetGraphic, value))
			{
				OnSetProperty();
			}
		}
	}

	public bool interactable
	{
		get
		{
			return m_Interactable;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_Interactable, value))
			{
				if (!m_Interactable && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == base.gameObject)
				{
					EventSystem.current.SetSelectedGameObject(null);
				}
				OnSetProperty();
			}
		}
	}

	private bool isPointerInside { get; set; }

	private bool isPointerDown { get; set; }

	private bool hasSelection { get; set; }

	public Image image
	{
		get
		{
			return m_TargetGraphic as Image;
		}
		set
		{
			m_TargetGraphic = value;
		}
	}

	public Animator animator => GetComponent<Animator>();

	protected SelectionState currentSelectionState
	{
		get
		{
			if (!IsInteractable())
			{
				return SelectionState.Disabled;
			}
			if (isPointerDown)
			{
				return SelectionState.Pressed;
			}
			if (hasSelection)
			{
				return SelectionState.Selected;
			}
			if (isPointerInside)
			{
				return SelectionState.Highlighted;
			}
			return SelectionState.Normal;
		}
	}

	public static int AllSelectablesNoAlloc(Selectable[] selectables)
	{
		int num = ((selectables.Length < s_SelectableCount) ? selectables.Length : s_SelectableCount);
		Array.Copy(s_Selectables, selectables, num);
		return num;
	}

	protected Selectable()
	{
	}

	protected override void Awake()
	{
		if (m_TargetGraphic == null)
		{
			m_TargetGraphic = GetComponent<Graphic>();
		}
	}

	protected override void OnCanvasGroupChanged()
	{
		bool flag = true;
		Transform parent = base.transform;
		while (parent != null)
		{
			parent.GetComponents(m_CanvasGroupCache);
			bool flag2 = false;
			for (int i = 0; i < m_CanvasGroupCache.Count; i++)
			{
				if (!m_CanvasGroupCache[i].interactable)
				{
					flag = false;
					flag2 = true;
				}
				if (m_CanvasGroupCache[i].ignoreParentGroups)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				break;
			}
			parent = parent.parent;
		}
		if (flag != m_GroupsAllowInteraction)
		{
			m_GroupsAllowInteraction = flag;
			OnSetProperty();
		}
	}

	public virtual bool IsInteractable()
	{
		if (m_GroupsAllowInteraction)
		{
			return m_Interactable;
		}
		return false;
	}

	protected override void OnDidApplyAnimationProperties()
	{
		OnSetProperty();
	}

	protected override void OnEnable()
	{
		if (!m_EnableCalled)
		{
			base.OnEnable();
			if (s_SelectableCount == s_Selectables.Length)
			{
				Selectable[] destinationArray = new Selectable[s_Selectables.Length * 2];
				Array.Copy(s_Selectables, destinationArray, s_Selectables.Length);
				s_Selectables = destinationArray;
			}
			m_CurrentIndex = s_SelectableCount;
			s_Selectables[m_CurrentIndex] = this;
			s_SelectableCount++;
			isPointerDown = false;
			DoStateTransition(currentSelectionState, instant: true);
			m_EnableCalled = true;
		}
	}

	protected override void OnTransformParentChanged()
	{
		base.OnTransformParentChanged();
		OnCanvasGroupChanged();
	}

	private void OnSetProperty()
	{
		DoStateTransition(currentSelectionState, instant: false);
	}

	protected override void OnDisable()
	{
		if (m_EnableCalled)
		{
			s_SelectableCount--;
			s_Selectables[s_SelectableCount].m_CurrentIndex = m_CurrentIndex;
			s_Selectables[m_CurrentIndex] = s_Selectables[s_SelectableCount];
			s_Selectables[s_SelectableCount] = null;
			InstantClearState();
			base.OnDisable();
			m_EnableCalled = false;
		}
	}

	protected virtual void InstantClearState()
	{
		string normalTrigger = m_AnimationTriggers.normalTrigger;
		isPointerInside = false;
		isPointerDown = false;
		hasSelection = false;
		switch (m_Transition)
		{
		case Transition.ColorTint:
			StartColorTween(Color.white, instant: true);
			break;
		case Transition.SpriteSwap:
			DoSpriteSwap(null);
			break;
		case Transition.Animation:
			TriggerAnimation(normalTrigger);
			break;
		}
	}

	protected virtual void DoStateTransition(SelectionState state, bool instant)
	{
		if (base.gameObject.activeInHierarchy)
		{
			Color color;
			Sprite newSprite;
			string triggername;
			switch (state)
			{
			case SelectionState.Normal:
				color = m_Colors.normalColor;
				newSprite = null;
				triggername = m_AnimationTriggers.normalTrigger;
				break;
			case SelectionState.Highlighted:
				color = m_Colors.highlightedColor;
				newSprite = m_SpriteState.highlightedSprite;
				triggername = m_AnimationTriggers.highlightedTrigger;
				break;
			case SelectionState.Pressed:
				color = m_Colors.pressedColor;
				newSprite = m_SpriteState.pressedSprite;
				triggername = m_AnimationTriggers.pressedTrigger;
				break;
			case SelectionState.Selected:
				color = m_Colors.selectedColor;
				newSprite = m_SpriteState.selectedSprite;
				triggername = m_AnimationTriggers.selectedTrigger;
				break;
			case SelectionState.Disabled:
				color = m_Colors.disabledColor;
				newSprite = m_SpriteState.disabledSprite;
				triggername = m_AnimationTriggers.disabledTrigger;
				break;
			default:
				color = Color.black;
				newSprite = null;
				triggername = string.Empty;
				break;
			}
			switch (m_Transition)
			{
			case Transition.ColorTint:
				StartColorTween(color * m_Colors.colorMultiplier, instant);
				break;
			case Transition.SpriteSwap:
				DoSpriteSwap(newSprite);
				break;
			case Transition.Animation:
				TriggerAnimation(triggername);
				break;
			}
		}
	}

	public Selectable FindSelectable(Vector3 dir)
	{
		dir = dir.normalized;
		Vector3 vector = Quaternion.Inverse(base.transform.rotation) * dir;
		Vector3 vector2 = base.transform.TransformPoint(GetPointOnRectEdge(base.transform as RectTransform, vector));
		float num = float.NegativeInfinity;
		Selectable result = null;
		for (int i = 0; i < s_SelectableCount; i++)
		{
			Selectable selectable = s_Selectables[i];
			if (selectable == this || !selectable.IsInteractable() || selectable.navigation.mode == Navigation.Mode.None)
			{
				continue;
			}
			RectTransform rectTransform = selectable.transform as RectTransform;
			Vector3 position = ((rectTransform != null) ? ((Vector3)rectTransform.rect.center) : Vector3.zero);
			Vector3 rhs = selectable.transform.TransformPoint(position) - vector2;
			float num2 = Vector3.Dot(dir, rhs);
			if (!(num2 <= 0f))
			{
				float num3 = num2 / rhs.sqrMagnitude;
				if (num3 > num)
				{
					num = num3;
					result = selectable;
				}
			}
		}
		return result;
	}

	private static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
	{
		if (rect == null)
		{
			return Vector3.zero;
		}
		if (dir != Vector2.zero)
		{
			dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
		}
		dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
		return dir;
	}

	private void Navigate(AxisEventData eventData, Selectable sel)
	{
		if (sel != null && sel.IsActive())
		{
			eventData.selectedObject = sel.gameObject;
		}
	}

	public virtual Selectable FindSelectableOnLeft()
	{
		if (m_Navigation.mode == Navigation.Mode.Explicit)
		{
			return m_Navigation.selectOnLeft;
		}
		if ((m_Navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None)
		{
			return FindSelectable(base.transform.rotation * Vector3.left);
		}
		return null;
	}

	public virtual Selectable FindSelectableOnRight()
	{
		if (m_Navigation.mode == Navigation.Mode.Explicit)
		{
			return m_Navigation.selectOnRight;
		}
		if ((m_Navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None)
		{
			return FindSelectable(base.transform.rotation * Vector3.right);
		}
		return null;
	}

	public virtual Selectable FindSelectableOnUp()
	{
		if (m_Navigation.mode == Navigation.Mode.Explicit)
		{
			return m_Navigation.selectOnUp;
		}
		if ((m_Navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None)
		{
			return FindSelectable(base.transform.rotation * Vector3.up);
		}
		return null;
	}

	public virtual Selectable FindSelectableOnDown()
	{
		if (m_Navigation.mode == Navigation.Mode.Explicit)
		{
			return m_Navigation.selectOnDown;
		}
		if ((m_Navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None)
		{
			return FindSelectable(base.transform.rotation * Vector3.down);
		}
		return null;
	}

	public virtual void OnMove(AxisEventData eventData)
	{
		switch (eventData.moveDir)
		{
		case MoveDirection.Right:
			Navigate(eventData, FindSelectableOnRight());
			break;
		case MoveDirection.Up:
			Navigate(eventData, FindSelectableOnUp());
			break;
		case MoveDirection.Left:
			Navigate(eventData, FindSelectableOnLeft());
			break;
		case MoveDirection.Down:
			Navigate(eventData, FindSelectableOnDown());
			break;
		}
	}

	private void StartColorTween(Color targetColor, bool instant)
	{
		if (!(m_TargetGraphic == null))
		{
			m_TargetGraphic.CrossFadeColor(targetColor, instant ? 0f : m_Colors.fadeDuration, ignoreTimeScale: true, useAlpha: true);
		}
	}

	private void DoSpriteSwap(Sprite newSprite)
	{
		if (!(image == null))
		{
			image.overrideSprite = newSprite;
		}
	}

	private void TriggerAnimation(string triggername)
	{
		if (transition == Transition.Animation && !(animator == null) && animator.isActiveAndEnabled && animator.hasBoundPlayables && !string.IsNullOrEmpty(triggername))
		{
			animator.ResetTrigger(m_AnimationTriggers.normalTrigger);
			animator.ResetTrigger(m_AnimationTriggers.highlightedTrigger);
			animator.ResetTrigger(m_AnimationTriggers.pressedTrigger);
			animator.ResetTrigger(m_AnimationTriggers.selectedTrigger);
			animator.ResetTrigger(m_AnimationTriggers.disabledTrigger);
			animator.SetTrigger(triggername);
		}
	}

	protected bool IsHighlighted()
	{
		if (!IsActive() || !IsInteractable())
		{
			return false;
		}
		if (isPointerInside && !isPointerDown)
		{
			return !hasSelection;
		}
		return false;
	}

	protected bool IsPressed()
	{
		if (!IsActive() || !IsInteractable())
		{
			return false;
		}
		return isPointerDown;
	}

	private void EvaluateAndTransitionToSelectionState()
	{
		if (IsActive() && IsInteractable())
		{
			DoStateTransition(currentSelectionState, instant: false);
		}
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (IsInteractable() && navigation.mode != Navigation.Mode.None && EventSystem.current != null)
			{
				EventSystem.current.SetSelectedGameObject(base.gameObject, eventData);
			}
			isPointerDown = true;
			EvaluateAndTransitionToSelectionState();
		}
	}

	public virtual void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			isPointerDown = false;
			EvaluateAndTransitionToSelectionState();
		}
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		isPointerInside = true;
		EvaluateAndTransitionToSelectionState();
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		isPointerInside = false;
		EvaluateAndTransitionToSelectionState();
	}

	public virtual void OnSelect(BaseEventData eventData)
	{
		hasSelection = true;
		EvaluateAndTransitionToSelectionState();
	}

	public virtual void OnDeselect(BaseEventData eventData)
	{
		hasSelection = false;
		EvaluateAndTransitionToSelectionState();
	}

	public virtual void Select()
	{
		if (!(EventSystem.current == null) && !EventSystem.current.alreadySelecting)
		{
			EventSystem.current.SetSelectedGameObject(base.gameObject);
		}
	}
}
