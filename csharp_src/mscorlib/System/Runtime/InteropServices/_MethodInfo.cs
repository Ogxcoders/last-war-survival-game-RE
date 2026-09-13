using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices;

[CLSCompliant(false)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("FFCC1B5D-ECB8-38DD-9B01-3DC8ABC2AA5F")]
[TypeLibImportClass(typeof(MethodInfo))]
[ComVisible(true)]
public interface _MethodInfo
{
	MemberTypes MemberType { get; }

	string Name { get; }

	Type DeclaringType { get; }

	Type ReflectedType { get; }

	RuntimeMethodHandle MethodHandle { get; }

	MethodAttributes Attributes { get; }

	CallingConventions CallingConvention { get; }

	bool IsPublic { get; }

	bool IsPrivate { get; }

	bool IsFamily { get; }

	bool IsAssembly { get; }

	bool IsFamilyAndAssembly { get; }

	bool IsFamilyOrAssembly { get; }

	bool IsStatic { get; }

	bool IsFinal { get; }

	bool IsVirtual { get; }

	bool IsHideBySig { get; }

	bool IsAbstract { get; }

	bool IsSpecialName { get; }

	bool IsConstructor { get; }

	Type ReturnType { get; }

	ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

	void GetTypeInfoCount(out uint pcTInfo);

	void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

	void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

	void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

	new string ToString();

	new bool Equals(object other);

	new int GetHashCode();

	new Type GetType();

	object[] GetCustomAttributes(Type attributeType, bool inherit);

	object[] GetCustomAttributes(bool inherit);

	bool IsDefined(Type attributeType, bool inherit);

	ParameterInfo[] GetParameters();

	MethodImplAttributes GetMethodImplementationFlags();

	object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

	object Invoke(object obj, object[] parameters);

	MethodInfo GetBaseDefinition();
}
