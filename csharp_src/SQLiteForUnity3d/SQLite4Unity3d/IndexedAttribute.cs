using System;

namespace SQLite4Unity3d;

[AttributeUsage(AttributeTargets.Property)]
public class IndexedAttribute : Attribute
{
	public string Name { get; set; }

	public int Order { get; set; }

	public virtual bool Unique { get; set; }

	public IndexedAttribute()
	{
	}

	public IndexedAttribute(string name, int order)
	{
		Name = name;
		Order = order;
	}
}
