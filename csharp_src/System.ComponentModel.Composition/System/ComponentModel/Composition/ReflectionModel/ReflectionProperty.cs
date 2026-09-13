using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class ReflectionProperty : ReflectionWritableMember
{
	private readonly MethodInfo _getMethod;

	private readonly MethodInfo _setMethod;

	public override MemberInfo UnderlyingMember => UnderlyingGetMethod ?? UnderlyingSetMethod;

	public override bool CanRead => UnderlyingGetMethod != null;

	public override bool CanWrite => UnderlyingSetMethod != null;

	public MethodInfo UnderlyingGetMethod => _getMethod;

	public MethodInfo UnderlyingSetMethod => _setMethod;

	public override string Name
	{
		get
		{
			string name = (UnderlyingGetMethod ?? UnderlyingSetMethod).Name;
			Assumes.IsTrue(name.Length > 4);
			return name.Substring(4);
		}
	}

	public override bool RequiresInstance => !(UnderlyingGetMethod ?? UnderlyingSetMethod).IsStatic;

	public override Type ReturnType
	{
		get
		{
			if (UnderlyingGetMethod != null)
			{
				return UnderlyingGetMethod.ReturnType;
			}
			ParameterInfo[] parameters = UnderlyingSetMethod.GetParameters();
			Assumes.IsTrue(parameters.Length != 0);
			return parameters[^1].ParameterType;
		}
	}

	public override ReflectionItemType ItemType => ReflectionItemType.Property;

	public ReflectionProperty(MethodInfo getMethod, MethodInfo setMethod)
	{
		Assumes.IsTrue(getMethod != null || setMethod != null);
		_getMethod = getMethod;
		_setMethod = setMethod;
	}

	public override string GetDisplayName()
	{
		return ReflectionServices.GetDisplayName(base.DeclaringType, Name);
	}

	public override object GetValue(object instance)
	{
		Assumes.NotNull(_getMethod);
		return UnderlyingGetMethod.SafeInvoke(instance);
	}

	public override void SetValue(object instance, object value)
	{
		Assumes.NotNull(_setMethod);
		UnderlyingSetMethod.SafeInvoke(instance, value);
	}
}
