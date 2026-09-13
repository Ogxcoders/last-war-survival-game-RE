namespace System.ComponentModel;

public abstract class InstanceCreationEditor
{
	public virtual string Text => "(New...)";

	public abstract object CreateInstance(ITypeDescriptorContext context, Type instanceType);
}
