namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class ImmutableObjectAttribute : Attribute
{
	public static readonly ImmutableObjectAttribute Yes = new ImmutableObjectAttribute(immutable: true);

	public static readonly ImmutableObjectAttribute No = new ImmutableObjectAttribute(immutable: false);

	public static readonly ImmutableObjectAttribute Default = No;

	public bool Immutable { get; }

	public ImmutableObjectAttribute(bool immutable)
	{
		Immutable = immutable;
	}

	public override bool Equals(object obj)
	{
		if (obj == this)
		{
			return true;
		}
		return (obj as ImmutableObjectAttribute)?.Immutable == Immutable;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool IsDefaultAttribute()
	{
		return Equals(Default);
	}
}
