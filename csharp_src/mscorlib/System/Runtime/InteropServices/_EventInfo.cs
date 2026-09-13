using System.Reflection;

namespace System.Runtime.InteropServices;

[CLSCompliant(false)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("9DE59C64-D889-35A1-B897-587D74469E5B")]
[TypeLibImportClass(typeof(EventInfo))]
[ComVisible(true)]
public interface _EventInfo
{
	EventAttributes Attributes { get; }

	Type DeclaringType { get; }

	Type EventHandlerType { get; }

	bool IsMulticast { get; }

	bool IsSpecialName { get; }

	MemberTypes MemberType { get; }

	string Name { get; }

	Type ReflectedType { get; }

	void AddEventHandler(object target, Delegate handler);

	new bool Equals(object other);

	MethodInfo GetAddMethod();

	MethodInfo GetAddMethod(bool nonPublic);

	object[] GetCustomAttributes(bool inherit);

	object[] GetCustomAttributes(Type attributeType, bool inherit);

	new int GetHashCode();

	void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

	void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

	void GetTypeInfoCount(out uint pcTInfo);

	void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

	MethodInfo GetRaiseMethod();

	MethodInfo GetRaiseMethod(bool nonPublic);

	MethodInfo GetRemoveMethod();

	MethodInfo GetRemoveMethod(bool nonPublic);

	new Type GetType();

	bool IsDefined(Type attributeType, bool inherit);

	void RemoveEventHandler(object target, Delegate handler);

	new string ToString();
}
