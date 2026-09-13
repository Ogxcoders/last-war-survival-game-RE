namespace UnityEngine.UIElements;

public class BaseFieldTraits<TValueType, TValueUxmlAttributeType> : BaseField<TValueType>.UxmlTraits where TValueUxmlAttributeType : TypedUxmlAttributeDescription<TValueType>, new()
{
	private TValueUxmlAttributeType m_Value = new TValueUxmlAttributeType
	{
		name = "value"
	};

	public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
	{
		base.Init(ve, bag, cc);
		((INotifyValueChanged<TValueType>)ve).SetValueWithoutNotify(m_Value.GetValueFromBag(bag, cc));
	}
}
