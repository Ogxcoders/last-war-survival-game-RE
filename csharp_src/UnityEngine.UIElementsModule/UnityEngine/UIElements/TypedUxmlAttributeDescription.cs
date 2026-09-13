namespace UnityEngine.UIElements;

public abstract class TypedUxmlAttributeDescription<T> : UxmlAttributeDescription
{
	public T defaultValue { get; set; }

	public override string defaultValueAsString => defaultValue.ToString();

	public abstract T GetValueFromBag(IUxmlAttributes bag, CreationContext cc);
}
