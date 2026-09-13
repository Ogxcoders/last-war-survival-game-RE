using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class ObjectReferenceField : InspectorField, IDropHandler, IEventSystemHandler
{
	[SerializeField]
	private RectTransform referencePickerArea;

	[SerializeField]
	private PointerEventListener input;

	[SerializeField]
	private PointerEventListener inspectReferenceButton;

	private Image inspectReferenceImage;

	[SerializeField]
	protected Image background;

	[SerializeField]
	protected Text referenceNameText;

	public override void Initialize()
	{
		base.Initialize();
		input.PointerClick += ShowReferencePicker;
		if (inspectReferenceButton != null)
		{
			inspectReferenceButton.PointerClick += InspectReference;
			inspectReferenceImage = inspectReferenceButton.GetComponent<Image>();
		}
	}

	public override bool SupportsType(Type type)
	{
		return typeof(UnityEngine.Object).IsAssignableFrom(type);
	}

	private void ShowReferencePicker(PointerEventData eventData)
	{
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(base.BoundVariableType);
		ObjectReferencePicker.Instance.Skin = base.Inspector.Skin;
		ObjectReferencePicker instance = ObjectReferencePicker.Instance;
		ObjectReferencePicker.ReferenceCallback onReferenceChanged = delegate(object reference)
		{
			OnReferenceChanged((UnityEngine.Object)reference);
		};
		ObjectReferencePicker.NameGetter referenceNameGetter = (object reference) => (!(UnityEngine.Object)reference) ? "None" : ((UnityEngine.Object)reference).name;
		ObjectReferencePicker.NameGetter referenceDisplayNameGetter = (object reference) => reference.GetNameWithType();
		object[] references = array;
		instance.Show(onReferenceChanged, null, referenceNameGetter, referenceDisplayNameGetter, references, (UnityEngine.Object)base.Value, includeNullReference: true, "Select " + base.BoundVariableType.Name, base.Inspector.Canvas);
	}

	private void InspectReference(PointerEventData eventData)
	{
		if (base.Value != null && !base.Value.Equals(null))
		{
			if (base.Value is Component)
			{
				base.Inspector.InspectInternal(((Component)base.Value).gameObject);
			}
			else
			{
				base.Inspector.InspectInternal(base.Value);
			}
		}
	}

	protected override void OnBound(MemberInfo variable)
	{
		base.OnBound(variable);
		OnReferenceChanged((UnityEngine.Object)base.Value);
	}

	protected virtual void OnReferenceChanged(UnityEngine.Object reference)
	{
		if ((UnityEngine.Object)base.Value != reference)
		{
			base.Value = reference;
		}
		if (referenceNameText != null)
		{
			referenceNameText.text = reference.GetNameWithType(base.BoundVariableType);
		}
		if (inspectReferenceButton != null)
		{
			inspectReferenceButton.gameObject.SetActive(base.Value != null && !base.Value.Equals(null));
		}
		base.Inspector.RefreshDelayed();
	}

	public void OnDrop(PointerEventData eventData)
	{
		UnityEngine.Object obj = (UnityEngine.Object)RuntimeInspectorUtils.GetAssignableObjectFromDraggedReferenceItem(eventData, base.BoundVariableType);
		if ((bool)obj)
		{
			OnReferenceChanged(obj);
		}
	}

	protected override void OnSkinChanged()
	{
		base.OnSkinChanged();
		background.color = base.Skin.InputFieldNormalBackgroundColor.Tint(0.075f);
		referenceNameText.SetSkinInputFieldText(base.Skin);
		referenceNameText.resizeTextMinSize = Mathf.Max(2, base.Skin.FontSize - 2);
		referenceNameText.resizeTextMaxSize = base.Skin.FontSize;
		if ((bool)inspectReferenceImage)
		{
			inspectReferenceImage.color = base.Skin.TextColor.Tint(0.1f);
			inspectReferenceImage.GetComponent<LayoutElement>().SetWidth(Mathf.Max(base.Skin.LineHeight - 8, 6));
		}
		if ((bool)referencePickerArea)
		{
			Vector2 anchorMin = new Vector2(base.Skin.LabelWidthPercentage, 0f);
			variableNameMask.rectTransform.anchorMin = anchorMin;
			referencePickerArea.anchorMin = anchorMin;
		}
	}

	public override void Refresh()
	{
		object value = base.Value;
		base.Refresh();
		if (value != base.Value)
		{
			OnReferenceChanged((UnityEngine.Object)base.Value);
		}
	}
}
