using System;
using System.Reflection;

namespace SFSLitJsonLw;

internal struct PropertyMetadata
{
	public MemberInfo Info;

	public bool IsField;

	public Type Type;
}
