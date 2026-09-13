using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices;

[Serializable]
[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
[ComVisible(true)]
public sealed class MethodImplAttribute : Attribute
{
	internal MethodImplOptions _val;

	public MethodCodeType MethodCodeType;

	public MethodImplOptions Value => _val;

	internal MethodImplAttribute(MethodImplAttributes methodImplAttributes)
	{
		MethodImplOptions methodImplOptions = MethodImplOptions.Unmanaged | MethodImplOptions.ForwardRef | MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall | MethodImplOptions.Synchronized | MethodImplOptions.NoInlining | MethodImplOptions.AggressiveInlining | MethodImplOptions.NoOptimization;
		_val = (MethodImplOptions)((int)methodImplAttributes & (int)methodImplOptions);
	}

	public MethodImplAttribute(MethodImplOptions methodImplOptions)
	{
		_val = methodImplOptions;
	}

	public MethodImplAttribute(short value)
	{
		_val = (MethodImplOptions)value;
	}

	public MethodImplAttribute()
	{
	}
}
