using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class ReflectionType : ReflectionMember
{
	private Type _type;

	public override MemberInfo UnderlyingMember => _type;

	public override bool CanRead => true;

	public override bool RequiresInstance => true;

	public override Type ReturnType => _type;

	public override ReflectionItemType ItemType => ReflectionItemType.Type;

	public ReflectionType(Type type)
	{
		Assumes.NotNull(type);
		_type = type;
	}

	public override object GetValue(object instance)
	{
		return instance;
	}
}
