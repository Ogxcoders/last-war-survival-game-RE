using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public class PreviewFieldAttribute : Attribute
{
	private ObjectFieldAlignment alignment;

	private bool alignmentHasValue;

	public float Height;

	public ObjectFieldAlignment Alignment
	{
		get
		{
			return alignment;
		}
		set
		{
			alignment = value;
			alignmentHasValue = true;
		}
	}

	public bool AlignmentHasValue => alignmentHasValue;

	public PreviewFieldAttribute()
	{
		Height = 0f;
	}

	public PreviewFieldAttribute(float height)
	{
		Height = height;
	}

	public PreviewFieldAttribute(float height, ObjectFieldAlignment alignment)
	{
		Height = height;
		Alignment = alignment;
	}

	public PreviewFieldAttribute(ObjectFieldAlignment alignment)
	{
		Alignment = alignment;
	}
}
