using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal abstract class ReflectionMember : ReflectionItem
{
	public abstract bool CanRead { get; }

	public Type DeclaringType => UnderlyingMember.DeclaringType;

	public override string Name => UnderlyingMember.Name;

	public abstract bool RequiresInstance { get; }

	public abstract MemberInfo UnderlyingMember { get; }

	public override string GetDisplayName()
	{
		return UnderlyingMember.GetDisplayName();
	}

	public abstract object GetValue(object instance);
}
