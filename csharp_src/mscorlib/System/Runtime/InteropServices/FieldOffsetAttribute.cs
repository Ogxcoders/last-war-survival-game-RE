using System.Reflection;
using System.Security;

namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Field, Inherited = false)]
[ComVisible(true)]
public sealed class FieldOffsetAttribute : Attribute
{
	internal int _val;

	public int Value => _val;

	[SecurityCritical]
	internal static Attribute GetCustomAttribute(RuntimeFieldInfo field)
	{
		int fieldOffset;
		if (field.DeclaringType != null && (fieldOffset = field.GetFieldOffset()) >= 0)
		{
			return new FieldOffsetAttribute(fieldOffset);
		}
		return null;
	}

	[SecurityCritical]
	internal static bool IsDefined(RuntimeFieldInfo field)
	{
		return GetCustomAttribute(field) != null;
	}

	public FieldOffsetAttribute(int offset)
	{
		_val = offset;
	}
}
