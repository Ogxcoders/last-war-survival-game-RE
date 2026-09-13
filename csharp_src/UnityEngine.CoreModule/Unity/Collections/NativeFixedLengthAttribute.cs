using System;
using UnityEngine.Scripting;

namespace Unity.Collections;

[RequiredByNativeCode]
[AttributeUsage(AttributeTargets.Field)]
public sealed class NativeFixedLengthAttribute : Attribute
{
	public int FixedLength;

	public NativeFixedLengthAttribute(int fixedLength)
	{
		FixedLength = fixedLength;
	}
}
