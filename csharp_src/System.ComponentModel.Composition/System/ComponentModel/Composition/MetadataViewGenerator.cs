using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using Microsoft.Internal;

namespace System.ComponentModel.Composition;

internal static class MetadataViewGenerator
{
	public const string MetadataViewType = "MetadataViewType";

	public const string MetadataItemKey = "MetadataItemKey";

	public const string MetadataItemTargetType = "MetadataItemTargetType";

	public const string MetadataItemSourceType = "MetadataItemSourceType";

	public const string MetadataItemValue = "MetadataItemValue";

	private static Lock _lock = new Lock();

	private static Dictionary<Type, Type> _proxies = new Dictionary<Type, Type>();

	private static AssemblyName ProxyAssemblyName = new AssemblyName(string.Format(CultureInfo.InvariantCulture, "MetadataViewProxies_{0}", Guid.NewGuid()));

	private static ModuleBuilder transparentProxyModuleBuilder;

	private static Type[] CtorArgumentTypes = new Type[1] { typeof(IDictionary<string, object>) };

	private static MethodInfo _mdvDictionaryTryGet = CtorArgumentTypes[0].GetMethod("TryGetValue");

	private static readonly MethodInfo ObjectGetType = typeof(object).GetMethod("GetType", Type.EmptyTypes);

	private static AssemblyBuilder CreateProxyAssemblyBuilder(ConstructorInfo constructorInfo)
	{
		return AppDomain.CurrentDomain.DefineDynamicAssembly(ProxyAssemblyName, AssemblyBuilderAccess.Run);
	}

	private static ModuleBuilder GetProxyModuleBuilder(bool requiresCritical)
	{
		if (transparentProxyModuleBuilder == null)
		{
			transparentProxyModuleBuilder = CreateProxyAssemblyBuilder(typeof(SecurityTransparentAttribute).GetConstructor(Type.EmptyTypes)).DefineDynamicModule("MetadataViewProxiesModule");
		}
		return transparentProxyModuleBuilder;
	}

	public static Type GenerateView(Type viewType)
	{
		Assumes.NotNull(viewType);
		Assumes.IsTrue(viewType.IsInterface);
		bool flag;
		Type value;
		using (new ReadLock(_lock))
		{
			flag = _proxies.TryGetValue(viewType, out value);
		}
		if (!flag)
		{
			Type type = GenerateInterfaceViewProxyType(viewType);
			Assumes.NotNull(type);
			using (new WriteLock(_lock))
			{
				if (!_proxies.TryGetValue(viewType, out value))
				{
					value = type;
					_proxies.Add(viewType, value);
				}
			}
		}
		return value;
	}

	private static void GenerateLocalAssignmentFromDefaultAttribute(this ILGenerator IL, DefaultValueAttribute[] attrs, LocalBuilder local)
	{
		if (attrs.Length != 0)
		{
			DefaultValueAttribute defaultValueAttribute = attrs[0];
			IL.LoadValue(defaultValueAttribute.Value);
			if (defaultValueAttribute.Value != null && defaultValueAttribute.Value.GetType().IsValueType)
			{
				IL.Emit(OpCodes.Box, defaultValueAttribute.Value.GetType());
			}
			IL.Emit(OpCodes.Stloc, local);
		}
	}

	private static void GenerateFieldAssignmentFromLocalValue(this ILGenerator IL, LocalBuilder local, FieldBuilder field)
	{
		IL.Emit(OpCodes.Ldarg_0);
		IL.Emit(OpCodes.Ldloc, local);
		IL.Emit(field.FieldType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, field.FieldType);
		IL.Emit(OpCodes.Stfld, field);
	}

	private static void GenerateLocalAssignmentFromFlag(this ILGenerator IL, LocalBuilder local, bool flag)
	{
		IL.Emit(flag ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
		IL.Emit(OpCodes.Stloc, local);
	}

	private static Type GenerateInterfaceViewProxyType(Type viewType)
	{
		Type[] interfaces = new Type[1] { viewType };
		TypeBuilder typeBuilder = GetProxyModuleBuilder(requiresCritical: false).DefineType(string.Format(CultureInfo.InvariantCulture, "_proxy_{0}_{1}", viewType.FullName, Guid.NewGuid()), TypeAttributes.Public, typeof(object), interfaces);
		ILGenerator iLGenerator = typeBuilder.CreateGeneratorForPublicConstructor(CtorArgumentTypes);
		LocalBuilder localBuilder = iLGenerator.DeclareLocal(typeof(Exception));
		LocalBuilder localBuilder2 = iLGenerator.DeclareLocal(typeof(IDictionary));
		LocalBuilder localBuilder3 = iLGenerator.DeclareLocal(typeof(Type));
		LocalBuilder localBuilder4 = iLGenerator.DeclareLocal(typeof(object));
		LocalBuilder local = iLGenerator.DeclareLocal(typeof(bool));
		Label label = iLGenerator.BeginExceptionBlock();
		foreach (PropertyInfo allProperty in viewType.GetAllProperties())
		{
			string fieldName = string.Format(CultureInfo.InvariantCulture, "_{0}_{1}", allProperty.Name, Guid.NewGuid());
			string text = string.Format(CultureInfo.InvariantCulture, "{0}", allProperty.Name);
			Type[] parameterTypes = new Type[1] { allProperty.PropertyType };
			Type[] returnTypeOptionalCustomModifiers = null;
			Type[] returnTypeRequiredCustomModifiers = null;
			FieldBuilder field = typeBuilder.DefineField(fieldName, allProperty.PropertyType, FieldAttributes.Private);
			PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(text, PropertyAttributes.None, allProperty.PropertyType, parameterTypes);
			Label label2 = iLGenerator.BeginExceptionBlock();
			DefaultValueAttribute[] attributes = allProperty.GetAttributes<DefaultValueAttribute>(inherit: false);
			if (attributes.Length != 0)
			{
				iLGenerator.BeginExceptionBlock();
			}
			Label label3 = iLGenerator.DefineLabel();
			iLGenerator.GenerateLocalAssignmentFromFlag(local, flag: true);
			iLGenerator.Emit(OpCodes.Ldarg_1);
			iLGenerator.Emit(OpCodes.Ldstr, allProperty.Name);
			iLGenerator.Emit(OpCodes.Ldloca, localBuilder4);
			iLGenerator.Emit(OpCodes.Callvirt, _mdvDictionaryTryGet);
			iLGenerator.Emit(OpCodes.Brtrue, label3);
			iLGenerator.GenerateLocalAssignmentFromFlag(local, flag: false);
			iLGenerator.GenerateLocalAssignmentFromDefaultAttribute(attributes, localBuilder4);
			iLGenerator.MarkLabel(label3);
			iLGenerator.GenerateFieldAssignmentFromLocalValue(localBuilder4, field);
			iLGenerator.Emit(OpCodes.Leave, label2);
			if (attributes.Length != 0)
			{
				iLGenerator.BeginCatchBlock(typeof(InvalidCastException));
				Label label4 = iLGenerator.DefineLabel();
				iLGenerator.Emit(OpCodes.Ldloc, local);
				iLGenerator.Emit(OpCodes.Brtrue, label4);
				iLGenerator.Emit(OpCodes.Rethrow);
				iLGenerator.MarkLabel(label4);
				iLGenerator.GenerateLocalAssignmentFromDefaultAttribute(attributes, localBuilder4);
				iLGenerator.GenerateFieldAssignmentFromLocalValue(localBuilder4, field);
				iLGenerator.EndExceptionBlock();
			}
			iLGenerator.BeginCatchBlock(typeof(NullReferenceException));
			iLGenerator.Emit(OpCodes.Stloc, localBuilder);
			iLGenerator.GetExceptionDataAndStoreInLocal(localBuilder, localBuilder2);
			iLGenerator.AddItemToLocalDictionary(localBuilder2, "MetadataItemKey", text);
			iLGenerator.AddItemToLocalDictionary(localBuilder2, "MetadataItemTargetType", allProperty.PropertyType);
			iLGenerator.Emit(OpCodes.Rethrow);
			iLGenerator.BeginCatchBlock(typeof(InvalidCastException));
			iLGenerator.Emit(OpCodes.Stloc, localBuilder);
			iLGenerator.GetExceptionDataAndStoreInLocal(localBuilder, localBuilder2);
			iLGenerator.AddItemToLocalDictionary(localBuilder2, "MetadataItemKey", text);
			iLGenerator.AddItemToLocalDictionary(localBuilder2, "MetadataItemTargetType", allProperty.PropertyType);
			iLGenerator.Emit(OpCodes.Rethrow);
			iLGenerator.EndExceptionBlock();
			if (allProperty.CanWrite)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Strings.InvalidSetterOnMetadataField, viewType, text));
			}
			if (allProperty.CanRead)
			{
				MethodBuilder methodBuilder = typeBuilder.DefineMethod(string.Format(CultureInfo.InvariantCulture, "get_{0}", text), MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask | MethodAttributes.SpecialName, CallingConventions.HasThis, allProperty.PropertyType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, Type.EmptyTypes, null, null);
				typeBuilder.DefineMethodOverride(methodBuilder, allProperty.GetGetMethod());
				ILGenerator iLGenerator2 = methodBuilder.GetILGenerator();
				iLGenerator2.Emit(OpCodes.Ldarg_0);
				iLGenerator2.Emit(OpCodes.Ldfld, field);
				iLGenerator2.Emit(OpCodes.Ret);
				propertyBuilder.SetGetMethod(methodBuilder);
			}
		}
		iLGenerator.Emit(OpCodes.Leave, label);
		iLGenerator.BeginCatchBlock(typeof(NullReferenceException));
		iLGenerator.Emit(OpCodes.Stloc, localBuilder);
		iLGenerator.GetExceptionDataAndStoreInLocal(localBuilder, localBuilder2);
		iLGenerator.AddItemToLocalDictionary(localBuilder2, "MetadataViewType", viewType);
		iLGenerator.Emit(OpCodes.Rethrow);
		iLGenerator.BeginCatchBlock(typeof(InvalidCastException));
		iLGenerator.Emit(OpCodes.Stloc, localBuilder);
		iLGenerator.GetExceptionDataAndStoreInLocal(localBuilder, localBuilder2);
		iLGenerator.Emit(OpCodes.Ldloc, localBuilder4);
		iLGenerator.Emit(OpCodes.Call, ObjectGetType);
		iLGenerator.Emit(OpCodes.Stloc, localBuilder3);
		iLGenerator.AddItemToLocalDictionary(localBuilder2, "MetadataViewType", viewType);
		iLGenerator.AddLocalToLocalDictionary(localBuilder2, "MetadataItemSourceType", localBuilder3);
		iLGenerator.AddLocalToLocalDictionary(localBuilder2, "MetadataItemValue", localBuilder4);
		iLGenerator.Emit(OpCodes.Rethrow);
		iLGenerator.EndExceptionBlock();
		iLGenerator.Emit(OpCodes.Ret);
		return typeBuilder.CreateType();
	}
}
