namespace UnityEngine.UIElements;

public class UxmlRootElementFactory : UxmlFactory<VisualElement, UxmlRootElementTraits>
{
	public override string uxmlName => "UXML";

	public override string uxmlQualifiedName => uxmlNamespace + "." + uxmlName;

	public override string substituteForTypeName => string.Empty;

	public override string substituteForTypeNamespace => string.Empty;

	public override string substituteForTypeQualifiedName => string.Empty;

	public override VisualElement Create(IUxmlAttributes bag, CreationContext cc)
	{
		return null;
	}
}
