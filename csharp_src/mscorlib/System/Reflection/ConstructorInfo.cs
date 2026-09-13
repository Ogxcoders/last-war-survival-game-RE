using System.Diagnostics;
using System.Globalization;

namespace System.Reflection;

[Serializable]
public abstract class ConstructorInfo : MethodBase
{
	public static readonly string ConstructorName = ".ctor";

	public static readonly string TypeConstructorName = ".cctor";

	public override MemberTypes MemberType => MemberTypes.Constructor;

	[DebuggerHidden]
	[DebuggerStepThrough]
	public object Invoke(object[] parameters)
	{
		return Invoke(BindingFlags.CreateInstance, null, parameters, null);
	}

	public abstract object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static bool operator ==(ConstructorInfo left, ConstructorInfo right)
	{
		if ((object)left == right)
		{
			return true;
		}
		if ((object)left == null || (object)right == null)
		{
			return false;
		}
		return left.Equals(right);
	}

	public static bool operator !=(ConstructorInfo left, ConstructorInfo right)
	{
		return !(left == right);
	}
}
