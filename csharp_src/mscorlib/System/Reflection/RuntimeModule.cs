using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
[ComVisible(true)]
[ComDefaultInterface(typeof(_Module))]
[ClassInterface(ClassInterfaceType.None)]
internal class RuntimeModule : Module
{
	internal IntPtr _impl;

	internal Assembly assembly;

	internal string fqname;

	internal string name;

	internal string scopename;

	internal bool is_resource;

	internal int token;

	public override Assembly Assembly => assembly;

	public override string Name => name;

	public override string ScopeName => scopename;

	public override int MDStreamVersion
	{
		get
		{
			if (_impl == IntPtr.Zero)
			{
				throw new NotSupportedException();
			}
			return GetMDStreamVersion(_impl);
		}
	}

	public override Guid ModuleVersionId => GetModuleVersionId();

	public override string FullyQualifiedName => fqname;

	public override int MetadataToken => get_MetadataToken(this);

	internal IntPtr MonoModule => _impl;

	public override bool IsResource()
	{
		return is_resource;
	}

	public override Type[] FindTypes(TypeFilter filter, object filterCriteria)
	{
		List<Type> list = new List<Type>();
		Type[] types = GetTypes();
		foreach (Type type in types)
		{
			if (filter(type, filterCriteria))
			{
				list.Add(type);
			}
		}
		return list.ToArray();
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		return MonoCustomAttrs.GetCustomAttributes(this, inherit);
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		return MonoCustomAttrs.GetCustomAttributes(this, attributeType, inherit);
	}

	public override FieldInfo GetField(string name, BindingFlags bindingAttr)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (IsResource())
		{
			return null;
		}
		Type globalType = GetGlobalType(_impl);
		if (!(globalType != null))
		{
			return null;
		}
		return globalType.GetField(name, bindingAttr);
	}

	public override FieldInfo[] GetFields(BindingFlags bindingFlags)
	{
		if (IsResource())
		{
			return new FieldInfo[0];
		}
		Type globalType = GetGlobalType(_impl);
		if (!(globalType != null))
		{
			return new FieldInfo[0];
		}
		return globalType.GetFields(bindingFlags);
	}

	protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		if (IsResource())
		{
			return null;
		}
		Type globalType = GetGlobalType(_impl);
		if (globalType == null)
		{
			return null;
		}
		if (types == null)
		{
			return globalType.GetMethod(name);
		}
		return globalType.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
	}

	public override MethodInfo[] GetMethods(BindingFlags bindingFlags)
	{
		if (IsResource())
		{
			return new MethodInfo[0];
		}
		Type globalType = GetGlobalType(_impl);
		if (!(globalType != null))
		{
			return new MethodInfo[0];
		}
		return globalType.GetMethods(bindingFlags);
	}

	internal override ModuleHandle GetModuleHandleImpl()
	{
		return new ModuleHandle(_impl);
	}

	public override void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
	{
		GetPEKind(_impl, out peKind, out machine);
	}

	public override Type GetType(string className, bool throwOnError, bool ignoreCase)
	{
		if (className == null)
		{
			throw new ArgumentNullException("className");
		}
		if (className == string.Empty)
		{
			throw new ArgumentException("Type name can't be empty");
		}
		return assembly.InternalGetType(this, className, throwOnError, ignoreCase);
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		return MonoCustomAttrs.IsDefined(this, attributeType, inherit);
	}

	public override FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		return ResolveField(this, _impl, metadataToken, genericTypeArguments, genericMethodArguments);
	}

	internal static FieldInfo ResolveField(Module module, IntPtr monoModule, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		ResolveTokenError error;
		IntPtr intPtr = ResolveFieldToken(monoModule, metadataToken, ptrs_from_types(genericTypeArguments), ptrs_from_types(genericMethodArguments), out error);
		if (intPtr == IntPtr.Zero)
		{
			throw resolve_token_exception(module.Name, metadataToken, error, "Field");
		}
		return FieldInfo.GetFieldFromHandle(new RuntimeFieldHandle(intPtr));
	}

	public override MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		return ResolveMember(this, _impl, metadataToken, genericTypeArguments, genericMethodArguments);
	}

	internal static MemberInfo ResolveMember(Module module, IntPtr monoModule, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		ResolveTokenError error;
		MemberInfo memberInfo = ResolveMemberToken(monoModule, metadataToken, ptrs_from_types(genericTypeArguments), ptrs_from_types(genericMethodArguments), out error);
		if (memberInfo == null)
		{
			throw resolve_token_exception(module.Name, metadataToken, error, "MemberInfo");
		}
		return memberInfo;
	}

	public override MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		return ResolveMethod(this, _impl, metadataToken, genericTypeArguments, genericMethodArguments);
	}

	internal static MethodBase ResolveMethod(Module module, IntPtr monoModule, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		ResolveTokenError error;
		IntPtr intPtr = ResolveMethodToken(monoModule, metadataToken, ptrs_from_types(genericTypeArguments), ptrs_from_types(genericMethodArguments), out error);
		if (intPtr == IntPtr.Zero)
		{
			throw resolve_token_exception(module.Name, metadataToken, error, "MethodBase");
		}
		return RuntimeMethodInfo.GetMethodFromHandleNoGenericCheck(new RuntimeMethodHandle(intPtr));
	}

	public override string ResolveString(int metadataToken)
	{
		return ResolveString(this, _impl, metadataToken);
	}

	internal static string ResolveString(Module module, IntPtr monoModule, int metadataToken)
	{
		ResolveTokenError error;
		string text = ResolveStringToken(monoModule, metadataToken, out error);
		if (text == null)
		{
			throw resolve_token_exception(module.Name, metadataToken, error, "string");
		}
		return text;
	}

	public override Type ResolveType(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		return ResolveType(this, _impl, metadataToken, genericTypeArguments, genericMethodArguments);
	}

	internal static Type ResolveType(Module module, IntPtr monoModule, int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		ResolveTokenError error;
		IntPtr intPtr = ResolveTypeToken(monoModule, metadataToken, ptrs_from_types(genericTypeArguments), ptrs_from_types(genericMethodArguments), out error);
		if (intPtr == IntPtr.Zero)
		{
			throw resolve_token_exception(module.Name, metadataToken, error, "Type");
		}
		return Type.GetTypeFromHandle(new RuntimeTypeHandle(intPtr));
	}

	public override byte[] ResolveSignature(int metadataToken)
	{
		return ResolveSignature(this, _impl, metadataToken);
	}

	internal static byte[] ResolveSignature(Module module, IntPtr monoModule, int metadataToken)
	{
		ResolveTokenError error;
		byte[] array = ResolveSignature(monoModule, metadataToken, out error);
		if (array == null)
		{
			throw resolve_token_exception(module.Name, metadataToken, error, "signature");
		}
		return array;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		UnitySerializationHolder.GetUnitySerializationInfo(info, 5, ScopeName, GetRuntimeAssembly());
	}

	public override Type[] GetTypes()
	{
		return InternalGetTypes(_impl);
	}

	public override IList<CustomAttributeData> GetCustomAttributesData()
	{
		return CustomAttributeData.GetCustomAttributes(this);
	}

	internal RuntimeAssembly GetRuntimeAssembly()
	{
		return (RuntimeAssembly)assembly;
	}

	internal override Guid GetModuleVersionId()
	{
		byte[] array = new byte[16];
		GetGuidInternal(_impl, array);
		return new Guid(array);
	}

	internal static Exception resolve_token_exception(string name, int metadataToken, ResolveTokenError error, string tokenType)
	{
		if (error == ResolveTokenError.OutOfRange)
		{
			return new ArgumentOutOfRangeException("metadataToken", $"Token 0x{metadataToken:x} is not valid in the scope of module {name}");
		}
		return new ArgumentException($"Token 0x{metadataToken:x} is not a valid {tokenType} token in the scope of module {name}", "metadataToken");
	}

	internal static IntPtr[] ptrs_from_types(Type[] types)
	{
		if (types == null)
		{
			return null;
		}
		IntPtr[] array = new IntPtr[types.Length];
		for (int i = 0; i < types.Length; i++)
		{
			if (types[i] == null)
			{
				throw new ArgumentException();
			}
			array[i] = types[i].TypeHandle.Value;
		}
		return array;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern int get_MetadataToken(Module module);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern int GetMDStreamVersion(IntPtr module);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern Type[] InternalGetTypes(IntPtr module);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern IntPtr GetHINSTANCE(IntPtr module);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetGuidInternal(IntPtr module, byte[] guid);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern Type GetGlobalType(IntPtr module);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern IntPtr ResolveTypeToken(IntPtr module, int token, IntPtr[] type_args, IntPtr[] method_args, out ResolveTokenError error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern IntPtr ResolveMethodToken(IntPtr module, int token, IntPtr[] type_args, IntPtr[] method_args, out ResolveTokenError error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern IntPtr ResolveFieldToken(IntPtr module, int token, IntPtr[] type_args, IntPtr[] method_args, out ResolveTokenError error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern string ResolveStringToken(IntPtr module, int token, out ResolveTokenError error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern MemberInfo ResolveMemberToken(IntPtr module, int token, IntPtr[] type_args, IntPtr[] method_args, out ResolveTokenError error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern byte[] ResolveSignature(IntPtr module, int metadataToken, out ResolveTokenError error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void GetPEKind(IntPtr module, out PortableExecutableKinds peKind, out ImageFileMachine machine);
}
