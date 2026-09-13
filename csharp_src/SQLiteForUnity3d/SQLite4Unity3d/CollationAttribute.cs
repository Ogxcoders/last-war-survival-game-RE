using System;

namespace SQLite4Unity3d;

[AttributeUsage(AttributeTargets.Property)]
public class CollationAttribute : Attribute
{
	public string Value { get; private set; }

	public CollationAttribute(string collation)
	{
		Value = collation;
	}
}
