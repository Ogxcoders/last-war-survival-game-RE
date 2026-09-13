namespace System.ComponentModel.Composition.ReflectionModel;

internal abstract class ReflectionWritableMember : ReflectionMember
{
	public abstract bool CanWrite { get; }

	public abstract void SetValue(object instance, object value);
}
