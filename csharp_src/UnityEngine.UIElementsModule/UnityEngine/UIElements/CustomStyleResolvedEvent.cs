namespace UnityEngine.UIElements;

public class CustomStyleResolvedEvent : EventBase<CustomStyleResolvedEvent>
{
	public ICustomStyle customStyle => (base.target as VisualElement)?.customStyle;
}
