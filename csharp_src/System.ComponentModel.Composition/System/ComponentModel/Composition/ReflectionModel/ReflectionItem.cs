namespace System.ComponentModel.Composition.ReflectionModel;

internal abstract class ReflectionItem
{
	public abstract string Name { get; }

	public abstract Type ReturnType { get; }

	public abstract ReflectionItemType ItemType { get; }

	public abstract string GetDisplayName();
}
