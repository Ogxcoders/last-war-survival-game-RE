using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ColoredObjectFieldAttribute : Attribute
{
	public Color BackgroundColor { get; private set; }

	public ColoredObjectFieldAttribute(float r, float g, float b, float a = 0.5f)
	{
		BackgroundColor = new Color(r, g, b, a);
	}

	public ColoredObjectFieldAttribute()
	{
		BackgroundColor = new Color(0f, 1f, 0f, 0.3f);
	}
}
