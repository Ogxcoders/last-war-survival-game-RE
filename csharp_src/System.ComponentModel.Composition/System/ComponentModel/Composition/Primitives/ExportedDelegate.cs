using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Primitives;

public class ExportedDelegate
{
	private object _instance;

	private MethodInfo _method;

	protected ExportedDelegate()
	{
	}

	public ExportedDelegate(object instance, MethodInfo method)
	{
		Requires.NotNull(method, "method");
		_instance = instance;
		_method = method;
	}

	public virtual Delegate CreateDelegate(Type delegateType)
	{
		Requires.NotNull(delegateType, "delegateType");
		if (delegateType == typeof(Delegate) || delegateType == typeof(MulticastDelegate))
		{
			delegateType = CreateStandardDelegateType();
		}
		return Delegate.CreateDelegate(delegateType, _instance, _method, throwOnBindFailure: false);
	}

	private Type CreateStandardDelegateType()
	{
		ParameterInfo[] parameters = _method.GetParameters();
		Type[] array = new Type[parameters.Length + 1];
		array[parameters.Length] = _method.ReturnType;
		for (int i = 0; i < parameters.Length; i++)
		{
			array[i] = parameters[i].ParameterType;
		}
		return Expression.GetDelegateType(array);
	}
}
