using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class ReflectionMethod : ReflectionMember
{
	private readonly MethodInfo _method;

	public MethodInfo UnderlyingMethod => _method;

	public override MemberInfo UnderlyingMember => UnderlyingMethod;

	public override bool CanRead => true;

	public override bool RequiresInstance => !UnderlyingMethod.IsStatic;

	public override Type ReturnType => UnderlyingMethod.ReturnType;

	public override ReflectionItemType ItemType => ReflectionItemType.Method;

	public ReflectionMethod(MethodInfo method)
	{
		Assumes.NotNull(method);
		_method = method;
	}

	public override object GetValue(object instance)
	{
		return SafeCreateExportedDelegate(instance, _method);
	}

	private static ExportedDelegate SafeCreateExportedDelegate(object instance, MethodInfo method)
	{
		ReflectionInvoke.DemandMemberAccessIfNeeded(method);
		return new ExportedDelegate(instance, method);
	}
}
