namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class MergablePropertyAttribute : Attribute
{
	public static readonly MergablePropertyAttribute Yes = new MergablePropertyAttribute(allowMerge: true);

	public static readonly MergablePropertyAttribute No = new MergablePropertyAttribute(allowMerge: false);

	public static readonly MergablePropertyAttribute Default = Yes;

	public bool AllowMerge { get; }

	public MergablePropertyAttribute(bool allowMerge)
	{
		AllowMerge = allowMerge;
	}

	public override bool Equals(object obj)
	{
		if (obj == this)
		{
			return true;
		}
		return (obj as MergablePropertyAttribute)?.AllowMerge == AllowMerge;
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
