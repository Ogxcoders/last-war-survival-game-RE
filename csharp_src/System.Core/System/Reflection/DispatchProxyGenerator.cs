using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace System.Reflection;

internal static class DispatchProxyGenerator
{
	private class PackedArgs
	{
		internal const int DispatchProxyPosition = 0;

		internal const int DeclaringTypePosition = 1;

		internal const int MethodTokenPosition = 2;

		internal const int ArgsPosition = 3;

		internal const int GenericTypesPosition = 4;

		internal const int ReturnValuePosition = 5;

		internal static readonly Type[] PackedTypes = new Type[6]
		{
			typeof(object),
			typeof(Type),
			typeof(int),
			typeof(object[]),
			typeof(Type[]),
			typeof(object)
		};

		private object[] _args;

		internal DispatchProxy DispatchProxy => (DispatchProxy)_args[0];

		internal Type DeclaringType => (Type)_args[1];

		internal int MethodToken => (int)_args[2];

		internal object[] Args => (object[])_args[3];

		internal Type[] GenericTypes => (Type[])_args[4];

		internal object ReturnValue
		{
			set
			{
				_args[5] = value;
			}
		}

		internal PackedArgs()
			: this(new object[PackedTypes.Length])
		{
		}

		internal PackedArgs(object[] args)
		{
			_args = args;
		}
	}

	private class ProxyAssembly
	{
		private AssemblyBuilder _ab;

		private ModuleBuilder _mb;

		private int _typeId;

		private Dictionary<MethodBase, int> _methodToToken = new Dictionary<MethodBase, int>();

		private List<MethodBase> _methodsByToken = new List<MethodBase>();

		private HashSet<string> _ignoresAccessAssemblyNames = new HashSet<string>();

		private ConstructorInfo _ignoresAccessChecksToAttributeConstructor;

		internal ConstructorInfo IgnoresAccessChecksAttributeConstructor
		{
			get
			{
				if (_ignoresAccessChecksToAttributeConstructor == null)
				{
					TypeInfo typeInfo = GenerateTypeInfoOfIgnoresAccessChecksToAttribute();
					_ignoresAccessChecksToAttributeConstructor = typeInfo.DeclaredConstructors.Single();
				}
				return _ignoresAccessChecksToAttributeConstructor;
			}
		}

		public ProxyAssembly()
		{
			_ab = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("ProxyBuilder"), AssemblyBuilderAccess.Run);
			_mb = _ab.DefineDynamicModule("testmod");
		}

		public ProxyBuilder CreateProxy(string name, Type proxyBaseType)
		{
			int num = Interlocked.Increment(ref _typeId);
			TypeBuilder tb = _mb.DefineType(name + "_" + num, TypeAttributes.Public, proxyBaseType);
			return new ProxyBuilder(this, tb, proxyBaseType);
		}

		private TypeInfo GenerateTypeInfoOfIgnoresAccessChecksToAttribute()
		{
			TypeBuilder typeBuilder = _mb.DefineType("System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute", TypeAttributes.Public, typeof(Attribute));
			FieldBuilder fieldBuilder = typeBuilder.DefineField("assemblyName", typeof(string), FieldAttributes.Private);
			ILGenerator iLGenerator = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[1] { fieldBuilder.FieldType }).GetILGenerator();
			iLGenerator.Emit(OpCodes.Ldarg_0);
			iLGenerator.Emit(OpCodes.Ldarg, 1);
			iLGenerator.Emit(OpCodes.Stfld, fieldBuilder);
			iLGenerator.Emit(OpCodes.Ret);
			typeBuilder.DefineProperty("AssemblyName", PropertyAttributes.None, CallingConventions.HasThis, typeof(string), null);
			ILGenerator iLGenerator2 = typeBuilder.DefineMethod("get_AssemblyName", MethodAttributes.Public, CallingConventions.HasThis, typeof(string), null).GetILGenerator();
			iLGenerator2.Emit(OpCodes.Ldarg_0);
			iLGenerator2.Emit(OpCodes.Ldfld, fieldBuilder);
			iLGenerator2.Emit(OpCodes.Ret);
			TypeInfo typeInfo = typeof(AttributeUsageAttribute).GetTypeInfo();
			ConstructorInfo con = typeInfo.DeclaredConstructors.Single((ConstructorInfo c) => c.GetParameters().Count() == 1 && c.GetParameters()[0].ParameterType == typeof(AttributeTargets));
			PropertyInfo propertyInfo = typeInfo.DeclaredProperties.Single((PropertyInfo f) => string.Equals(f.Name, "AllowMultiple"));
			CustomAttributeBuilder customAttribute = new CustomAttributeBuilder(con, new object[1] { AttributeTargets.Assembly }, new PropertyInfo[1] { propertyInfo }, new object[1] { true });
			typeBuilder.SetCustomAttribute(customAttribute);
			return typeBuilder.CreateTypeInfo();
		}

		internal void GenerateInstanceOfIgnoresAccessChecksToAttribute(string assemblyName)
		{
			CustomAttributeBuilder customAttribute = new CustomAttributeBuilder(IgnoresAccessChecksAttributeConstructor, new object[1] { assemblyName });
			_ab.SetCustomAttribute(customAttribute);
		}

		internal void EnsureTypeIsVisible(Type type)
		{
			TypeInfo typeInfo = type.GetTypeInfo();
			if (!typeInfo.IsVisible)
			{
				string name = typeInfo.Assembly.GetName().Name;
				if (!_ignoresAccessAssemblyNames.Contains(name))
				{
					GenerateInstanceOfIgnoresAccessChecksToAttribute(name);
					_ignoresAccessAssemblyNames.Add(name);
				}
			}
		}

		internal void GetTokenForMethod(MethodBase method, out Type type, out int token)
		{
			type = method.DeclaringType;
			token = 0;
			if (!_methodToToken.TryGetValue(method, out token))
			{
				_methodsByToken.Add(method);
				token = _methodsByToken.Count - 1;
				_methodToToken[method] = token;
			}
		}

		internal MethodBase ResolveMethodToken(Type type, int token)
		{
			return _methodsByToken[token];
		}
	}

	private class ProxyBuilder
	{
		private class ParametersArray
		{
			private ILGenerator _il;

			private Type[] _paramTypes;

			internal ParametersArray(ILGenerator il, Type[] paramTypes)
			{
				_il = il;
				_paramTypes = paramTypes;
			}

			internal void Get(int i)
			{
				_il.Emit(OpCodes.Ldarg, i + 1);
			}

			internal void BeginSet(int i)
			{
				_il.Emit(OpCodes.Ldarg, i + 1);
			}

			internal void EndSet(int i, Type stackType)
			{
				Type elementType = _paramTypes[i].GetElementType();
				Convert(_il, stackType, elementType, isAddress: false);
				Stind(_il, elementType);
			}
		}

		private class GenericArray<T>
		{
			private ILGenerator _il;

			private LocalBuilder _lb;

			internal GenericArray(ILGenerator il, int len)
			{
				_il = il;
				_lb = il.DeclareLocal(typeof(T[]));
				il.Emit(OpCodes.Ldc_I4, len);
				il.Emit(OpCodes.Newarr, typeof(T));
				il.Emit(OpCodes.Stloc, _lb);
			}

			internal void Load()
			{
				_il.Emit(OpCodes.Ldloc, _lb);
			}

			internal void Get(int i)
			{
				_il.Emit(OpCodes.Ldloc, _lb);
				_il.Emit(OpCodes.Ldc_I4, i);
				_il.Emit(OpCodes.Ldelem_Ref);
			}

			internal void BeginSet(int i)
			{
				_il.Emit(OpCodes.Ldloc, _lb);
				_il.Emit(OpCodes.Ldc_I4, i);
			}

			internal void EndSet(Type stackType)
			{
				Convert(_il, stackType, typeof(T), isAddress: false);
				_il.Emit(OpCodes.Stelem_Ref);
			}
		}

		private sealed class PropertyAccessorInfo
		{
			public MethodInfo InterfaceGetMethod { get; }

			public MethodInfo InterfaceSetMethod { get; }

			public MethodBuilder GetMethodBuilder { get; set; }

			public MethodBuilder SetMethodBuilder { get; set; }

			public PropertyAccessorInfo(MethodInfo interfaceGetMethod, MethodInfo interfaceSetMethod)
			{
				InterfaceGetMethod = interfaceGetMethod;
				InterfaceSetMethod = interfaceSetMethod;
			}
		}

		private sealed class EventAccessorInfo
		{
			public MethodInfo InterfaceAddMethod { get; }

			public MethodInfo InterfaceRemoveMethod { get; }

			public MethodInfo InterfaceRaiseMethod { get; }

			public MethodBuilder AddMethodBuilder { get; set; }

			public MethodBuilder RemoveMethodBuilder { get; set; }

			public MethodBuilder RaiseMethodBuilder { get; set; }

			public EventAccessorInfo(MethodInfo interfaceAddMethod, MethodInfo interfaceRemoveMethod, MethodInfo interfaceRaiseMethod)
			{
				InterfaceAddMethod = interfaceAddMethod;
				InterfaceRemoveMethod = interfaceRemoveMethod;
				InterfaceRaiseMethod = interfaceRaiseMethod;
			}
		}

		private sealed class MethodInfoEqualityComparer : EqualityComparer<MethodInfo>
		{
			public static readonly MethodInfoEqualityComparer Instance = new MethodInfoEqualityComparer();

			private MethodInfoEqualityComparer()
			{
			}

			public sealed override bool Equals(MethodInfo left, MethodInfo right)
			{
				if ((object)left == right)
				{
					return true;
				}
				if (left == null)
				{
					return right == null;
				}
				if (right == null)
				{
					return false;
				}
				if (!object.Equals(left.DeclaringType, right.DeclaringType))
				{
					return false;
				}
				if (!object.Equals(left.ReturnType, right.ReturnType))
				{
					return false;
				}
				if (left.CallingConvention != right.CallingConvention)
				{
					return false;
				}
				if (left.IsStatic != right.IsStatic)
				{
					return false;
				}
				if (left.Name != right.Name)
				{
					return false;
				}
				Type[] genericArguments = left.GetGenericArguments();
				Type[] genericArguments2 = right.GetGenericArguments();
				if (genericArguments.Length != genericArguments2.Length)
				{
					return false;
				}
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (!object.Equals(genericArguments[i], genericArguments2[i]))
					{
						return false;
					}
				}
				ParameterInfo[] parameters = left.GetParameters();
				ParameterInfo[] parameters2 = right.GetParameters();
				if (parameters.Length != parameters2.Length)
				{
					return false;
				}
				for (int j = 0; j < parameters.Length; j++)
				{
					if (!object.Equals(parameters[j].ParameterType, parameters2[j].ParameterType))
					{
						return false;
					}
				}
				return true;
			}

			public sealed override int GetHashCode(MethodInfo obj)
			{
				if (obj == null)
				{
					return 0;
				}
				int hashCode = obj.DeclaringType.GetHashCode();
				hashCode ^= obj.Name.GetHashCode();
				ParameterInfo[] parameters = obj.GetParameters();
				foreach (ParameterInfo parameterInfo in parameters)
				{
					hashCode ^= parameterInfo.ParameterType.GetHashCode();
				}
				return hashCode;
			}
		}

		private static readonly MethodInfo s_delegateInvoke = typeof(Action<object[]>).GetTypeInfo().GetDeclaredMethod("Invoke");

		private ProxyAssembly _assembly;

		private TypeBuilder _tb;

		private Type _proxyBaseType;

		private List<FieldBuilder> _fields;

		private static OpCode[] s_convOpCodes = new OpCode[19]
		{
			OpCodes.Nop,
			OpCodes.Nop,
			OpCodes.Nop,
			OpCodes.Conv_I1,
			OpCodes.Conv_I2,
			OpCodes.Conv_I1,
			OpCodes.Conv_U1,
			OpCodes.Conv_I2,
			OpCodes.Conv_U2,
			OpCodes.Conv_I4,
			OpCodes.Conv_U4,
			OpCodes.Conv_I8,
			OpCodes.Conv_U8,
			OpCodes.Conv_R4,
			OpCodes.Conv_R8,
			OpCodes.Nop,
			OpCodes.Nop,
			OpCodes.Nop,
			OpCodes.Nop
		};

		private static OpCode[] s_ldindOpCodes = new OpCode[19]
		{
			OpCodes.Nop,
			OpCodes.Nop,
			OpCodes.Nop,
			OpCodes.Ldind_I1,
			OpCodes.Ldind_I2,
			OpCodes.Ldind_I1,
			OpCodes.Ldind_U1,
			OpCodes.Ldind_I2,
			OpCodes.Ldind_U2,
			OpCodes.Ldind_I4,
			OpCodes.Ldind_U4,
			OpCodes.Ldind_I8,
			OpCodes.Ldind_I8,
			OpCodes.Ldind_R4,
			OpCodes.Ldind_R8,
			OpCodes.Nop,
			OpCodes.Nop,
			OpCodes.Nop,
			OpCodes.Ldind_Ref
		};

		private static OpCode[] s_stindOpCodes = new OpCode[19]
		{
			OpCodes.Nop,
			OpCodes.Nop,
			OpCodes.Nop,
			OpCodes.Stind_I1,
			OpCodes.Stind_I2,
			OpCodes.Stind_I1,
			OpCodes.Stind_I1,
			OpCodes.Stind_I2,
			OpCodes.Stind_I2,
			OpCodes.Stind_I4,
			OpCodes.Stind_I4,
			OpCodes.Stind_I8,
			OpCodes.Stind_I8,
			OpCodes.Stind_R4,
			OpCodes.Stind_R8,
			OpCodes.Nop,
			OpCodes.Nop,
			OpCodes.Nop,
			OpCodes.Stind_Ref
		};

		internal ProxyBuilder(ProxyAssembly assembly, TypeBuilder tb, Type proxyBaseType)
		{
			_assembly = assembly;
			_tb = tb;
			_proxyBaseType = proxyBaseType;
			_fields = new List<FieldBuilder>();
			_fields.Add(tb.DefineField("invoke", typeof(Action<object[]>), FieldAttributes.Private));
		}

		private void Complete()
		{
			Type[] array = new Type[_fields.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = _fields[i].FieldType;
			}
			ILGenerator iLGenerator = _tb.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, array).GetILGenerator();
			ConstructorInfo con = _proxyBaseType.GetTypeInfo().DeclaredConstructors.SingleOrDefault((ConstructorInfo c) => c.IsPublic && c.GetParameters().Length == 0);
			iLGenerator.Emit(OpCodes.Ldarg_0);
			iLGenerator.Emit(OpCodes.Call, con);
			for (int num = 0; num < array.Length; num++)
			{
				iLGenerator.Emit(OpCodes.Ldarg_0);
				iLGenerator.Emit(OpCodes.Ldarg, num + 1);
				iLGenerator.Emit(OpCodes.Stfld, _fields[num]);
			}
			iLGenerator.Emit(OpCodes.Ret);
		}

		internal Type CreateType()
		{
			Complete();
			return _tb.CreateTypeInfo().AsType();
		}

		internal void AddInterfaceImpl(Type iface)
		{
			_assembly.EnsureTypeIsVisible(iface);
			_tb.AddInterfaceImplementation(iface);
			Dictionary<MethodInfo, PropertyAccessorInfo> dictionary = new Dictionary<MethodInfo, PropertyAccessorInfo>(MethodInfoEqualityComparer.Instance);
			foreach (PropertyInfo runtimeProperty in iface.GetRuntimeProperties())
			{
				PropertyAccessorInfo value = new PropertyAccessorInfo(runtimeProperty.GetMethod, runtimeProperty.SetMethod);
				if (runtimeProperty.GetMethod != null)
				{
					dictionary[runtimeProperty.GetMethod] = value;
				}
				if (runtimeProperty.SetMethod != null)
				{
					dictionary[runtimeProperty.SetMethod] = value;
				}
			}
			Dictionary<MethodInfo, EventAccessorInfo> dictionary2 = new Dictionary<MethodInfo, EventAccessorInfo>(MethodInfoEqualityComparer.Instance);
			foreach (EventInfo runtimeEvent in iface.GetRuntimeEvents())
			{
				EventAccessorInfo value2 = new EventAccessorInfo(runtimeEvent.AddMethod, runtimeEvent.RemoveMethod, runtimeEvent.RaiseMethod);
				if (runtimeEvent.AddMethod != null)
				{
					dictionary2[runtimeEvent.AddMethod] = value2;
				}
				if (runtimeEvent.RemoveMethod != null)
				{
					dictionary2[runtimeEvent.RemoveMethod] = value2;
				}
				if (runtimeEvent.RaiseMethod != null)
				{
					dictionary2[runtimeEvent.RaiseMethod] = value2;
				}
			}
			foreach (MethodInfo runtimeMethod in iface.GetRuntimeMethods())
			{
				MethodBuilder methodBuilder = AddMethodImpl(runtimeMethod);
				if (dictionary.TryGetValue(runtimeMethod, out var value3))
				{
					if (MethodInfoEqualityComparer.Instance.Equals(value3.InterfaceGetMethod, runtimeMethod))
					{
						value3.GetMethodBuilder = methodBuilder;
					}
					else
					{
						value3.SetMethodBuilder = methodBuilder;
					}
				}
				if (dictionary2.TryGetValue(runtimeMethod, out var value4))
				{
					if (MethodInfoEqualityComparer.Instance.Equals(value4.InterfaceAddMethod, runtimeMethod))
					{
						value4.AddMethodBuilder = methodBuilder;
					}
					else if (MethodInfoEqualityComparer.Instance.Equals(value4.InterfaceRemoveMethod, runtimeMethod))
					{
						value4.RemoveMethodBuilder = methodBuilder;
					}
					else
					{
						value4.RaiseMethodBuilder = methodBuilder;
					}
				}
			}
			foreach (PropertyInfo runtimeProperty2 in iface.GetRuntimeProperties())
			{
				PropertyAccessorInfo propertyAccessorInfo = dictionary[runtimeProperty2.GetMethod ?? runtimeProperty2.SetMethod];
				PropertyBuilder propertyBuilder = _tb.DefineProperty(runtimeProperty2.Name, runtimeProperty2.Attributes, runtimeProperty2.PropertyType, (from p in runtimeProperty2.GetIndexParameters()
					select p.ParameterType).ToArray());
				if (propertyAccessorInfo.GetMethodBuilder != null)
				{
					propertyBuilder.SetGetMethod(propertyAccessorInfo.GetMethodBuilder);
				}
				if (propertyAccessorInfo.SetMethodBuilder != null)
				{
					propertyBuilder.SetSetMethod(propertyAccessorInfo.SetMethodBuilder);
				}
			}
			foreach (EventInfo runtimeEvent2 in iface.GetRuntimeEvents())
			{
				EventAccessorInfo eventAccessorInfo = dictionary2[runtimeEvent2.AddMethod ?? runtimeEvent2.RemoveMethod];
				EventBuilder eventBuilder = _tb.DefineEvent(runtimeEvent2.Name, runtimeEvent2.Attributes, runtimeEvent2.EventHandlerType);
				if (eventAccessorInfo.AddMethodBuilder != null)
				{
					eventBuilder.SetAddOnMethod(eventAccessorInfo.AddMethodBuilder);
				}
				if (eventAccessorInfo.RemoveMethodBuilder != null)
				{
					eventBuilder.SetRemoveOnMethod(eventAccessorInfo.RemoveMethodBuilder);
				}
				if (eventAccessorInfo.RaiseMethodBuilder != null)
				{
					eventBuilder.SetRaiseMethod(eventAccessorInfo.RaiseMethodBuilder);
				}
			}
		}

		private MethodBuilder AddMethodImpl(MethodInfo mi)
		{
			ParameterInfo[] parameters = mi.GetParameters();
			Type[] array = ParamTypes(parameters, noByRef: false);
			MethodBuilder methodBuilder = _tb.DefineMethod(mi.Name, MethodAttributes.Public | MethodAttributes.Virtual, mi.ReturnType, array);
			if (mi.ContainsGenericParameters)
			{
				Type[] genericArguments = mi.GetGenericArguments();
				string[] array2 = new string[genericArguments.Length];
				for (int i = 0; i < genericArguments.Length; i++)
				{
					array2[i] = genericArguments[i].Name;
				}
				GenericTypeParameterBuilder[] array3 = methodBuilder.DefineGenericParameters(array2);
				for (int j = 0; j < array3.Length; j++)
				{
					array3[j].SetGenericParameterAttributes(genericArguments[j].GetTypeInfo().GenericParameterAttributes);
				}
			}
			ILGenerator iLGenerator = methodBuilder.GetILGenerator();
			ParametersArray parametersArray = new ParametersArray(iLGenerator, array);
			iLGenerator.Emit(OpCodes.Nop);
			GenericArray<object> genericArray = new GenericArray<object>(iLGenerator, ParamTypes(parameters, noByRef: true).Length);
			for (int k = 0; k < parameters.Length; k++)
			{
				if (!parameters[k].IsOut)
				{
					genericArray.BeginSet(k);
					parametersArray.Get(k);
					genericArray.EndSet(parameters[k].ParameterType);
				}
			}
			GenericArray<object> genericArray2 = new GenericArray<object>(iLGenerator, PackedArgs.PackedTypes.Length);
			genericArray2.BeginSet(0);
			iLGenerator.Emit(OpCodes.Ldarg_0);
			genericArray2.EndSet(typeof(DispatchProxy));
			MethodInfo runtimeMethod = typeof(Type).GetRuntimeMethod("GetTypeFromHandle", new Type[1] { typeof(RuntimeTypeHandle) });
			_assembly.GetTokenForMethod(mi, out var type, out var token);
			genericArray2.BeginSet(1);
			iLGenerator.Emit(OpCodes.Ldtoken, type);
			iLGenerator.Emit(OpCodes.Call, runtimeMethod);
			genericArray2.EndSet(typeof(object));
			genericArray2.BeginSet(2);
			iLGenerator.Emit(OpCodes.Ldc_I4, token);
			genericArray2.EndSet(typeof(int));
			genericArray2.BeginSet(3);
			genericArray.Load();
			genericArray2.EndSet(typeof(object[]));
			if (mi.ContainsGenericParameters)
			{
				genericArray2.BeginSet(4);
				Type[] genericArguments2 = mi.GetGenericArguments();
				GenericArray<Type> genericArray3 = new GenericArray<Type>(iLGenerator, genericArguments2.Length);
				for (int l = 0; l < genericArguments2.Length; l++)
				{
					genericArray3.BeginSet(l);
					iLGenerator.Emit(OpCodes.Ldtoken, genericArguments2[l]);
					iLGenerator.Emit(OpCodes.Call, runtimeMethod);
					genericArray3.EndSet(typeof(Type));
				}
				genericArray3.Load();
				genericArray2.EndSet(typeof(Type[]));
			}
			iLGenerator.Emit(OpCodes.Ldarg_0);
			iLGenerator.Emit(OpCodes.Ldfld, _fields[0]);
			genericArray2.Load();
			iLGenerator.Emit(OpCodes.Call, s_delegateInvoke);
			for (int m = 0; m < parameters.Length; m++)
			{
				if (parameters[m].ParameterType.IsByRef)
				{
					parametersArray.BeginSet(m);
					genericArray.Get(m);
					parametersArray.EndSet(m, typeof(object));
				}
			}
			if (mi.ReturnType != typeof(void))
			{
				genericArray2.Get(5);
				Convert(iLGenerator, typeof(object), mi.ReturnType, isAddress: false);
			}
			iLGenerator.Emit(OpCodes.Ret);
			_tb.DefineMethodOverride(methodBuilder, mi);
			return methodBuilder;
		}

		private static Type[] ParamTypes(ParameterInfo[] parms, bool noByRef)
		{
			Type[] array = new Type[parms.Length];
			for (int i = 0; i < parms.Length; i++)
			{
				array[i] = parms[i].ParameterType;
				if (noByRef && array[i].IsByRef)
				{
					array[i] = array[i].GetElementType();
				}
			}
			return array;
		}

		private static int GetTypeCode(Type type)
		{
			if (type == null)
			{
				return 0;
			}
			if (type == typeof(bool))
			{
				return 3;
			}
			if (type == typeof(char))
			{
				return 4;
			}
			if (type == typeof(sbyte))
			{
				return 5;
			}
			if (type == typeof(byte))
			{
				return 6;
			}
			if (type == typeof(short))
			{
				return 7;
			}
			if (type == typeof(ushort))
			{
				return 8;
			}
			if (type == typeof(int))
			{
				return 9;
			}
			if (type == typeof(uint))
			{
				return 10;
			}
			if (type == typeof(long))
			{
				return 11;
			}
			if (type == typeof(ulong))
			{
				return 12;
			}
			if (type == typeof(float))
			{
				return 13;
			}
			if (type == typeof(double))
			{
				return 14;
			}
			if (type == typeof(decimal))
			{
				return 15;
			}
			if (type == typeof(DateTime))
			{
				return 16;
			}
			if (type == typeof(string))
			{
				return 18;
			}
			if (type.GetTypeInfo().IsEnum)
			{
				return GetTypeCode(Enum.GetUnderlyingType(type));
			}
			return 1;
		}

		private static void Convert(ILGenerator il, Type source, Type target, bool isAddress)
		{
			if (target == source)
			{
				return;
			}
			TypeInfo typeInfo = source.GetTypeInfo();
			TypeInfo typeInfo2 = target.GetTypeInfo();
			if (source.IsByRef)
			{
				Type elementType = source.GetElementType();
				Ldind(il, elementType);
				Convert(il, elementType, target, isAddress);
			}
			else if (typeInfo2.IsValueType)
			{
				if (typeInfo.IsValueType)
				{
					OpCode opcode = s_convOpCodes[GetTypeCode(target)];
					il.Emit(opcode);
					return;
				}
				il.Emit(OpCodes.Unbox, target);
				if (!isAddress)
				{
					Ldind(il, target);
				}
			}
			else if (typeInfo2.IsAssignableFrom(typeInfo))
			{
				if (typeInfo.IsValueType || source.IsGenericParameter)
				{
					if (isAddress)
					{
						Ldind(il, source);
					}
					il.Emit(OpCodes.Box, source);
				}
			}
			else if (target.IsGenericParameter)
			{
				il.Emit(OpCodes.Unbox_Any, target);
			}
			else
			{
				il.Emit(OpCodes.Castclass, target);
			}
		}

		private static void Ldind(ILGenerator il, Type type)
		{
			OpCode opcode = s_ldindOpCodes[GetTypeCode(type)];
			if (!opcode.Equals(OpCodes.Nop))
			{
				il.Emit(opcode);
			}
			else
			{
				il.Emit(OpCodes.Ldobj, type);
			}
		}

		private static void Stind(ILGenerator il, Type type)
		{
			OpCode opcode = s_stindOpCodes[GetTypeCode(type)];
			if (!opcode.Equals(OpCodes.Nop))
			{
				il.Emit(opcode);
			}
			else
			{
				il.Emit(OpCodes.Stobj, type);
			}
		}
	}

	private const int InvokeActionFieldAndCtorParameterIndex = 0;

	private static readonly Dictionary<Type, Dictionary<Type, Type>> s_baseTypeAndInterfaceToGeneratedProxyType = new Dictionary<Type, Dictionary<Type, Type>>();

	private static readonly ProxyAssembly s_proxyAssembly = new ProxyAssembly();

	private static readonly MethodInfo s_dispatchProxyInvokeMethod = typeof(DispatchProxy).GetTypeInfo().GetDeclaredMethod("Invoke");

	internal static object CreateProxyInstance(Type baseType, Type interfaceType)
	{
		return Activator.CreateInstance(GetProxyType(baseType, interfaceType), new Action<object[]>(Invoke));
	}

	private static Type GetProxyType(Type baseType, Type interfaceType)
	{
		lock (s_baseTypeAndInterfaceToGeneratedProxyType)
		{
			Dictionary<Type, Type> value = null;
			if (!s_baseTypeAndInterfaceToGeneratedProxyType.TryGetValue(baseType, out value))
			{
				value = new Dictionary<Type, Type>();
				s_baseTypeAndInterfaceToGeneratedProxyType[baseType] = value;
			}
			Type value2 = null;
			if (!value.TryGetValue(interfaceType, out value2))
			{
				value2 = (value[interfaceType] = GenerateProxyType(baseType, interfaceType));
			}
			return value2;
		}
	}

	private static Type GenerateProxyType(Type baseType, Type interfaceType)
	{
		TypeInfo typeInfo = baseType.GetTypeInfo();
		if (!interfaceType.GetTypeInfo().IsInterface)
		{
			throw new ArgumentException(SR.Format("The type '{0}' must be an interface, not a class.", interfaceType.FullName), "T");
		}
		if (typeInfo.IsSealed)
		{
			throw new ArgumentException(SR.Format("The base type '{0}' cannot be sealed.", typeInfo.FullName), "TProxy");
		}
		if (typeInfo.IsAbstract)
		{
			throw new ArgumentException(SR.Format("The base type '{0}' cannot be abstract.", baseType.FullName), "TProxy");
		}
		if (!typeInfo.DeclaredConstructors.Any((ConstructorInfo c) => c.IsPublic && c.GetParameters().Length == 0))
		{
			throw new ArgumentException(SR.Format("The base type '{0}' must have a public parameterless constructor.", baseType.FullName), "TProxy");
		}
		ProxyBuilder proxyBuilder = s_proxyAssembly.CreateProxy("generatedProxy", baseType);
		foreach (Type implementedInterface in interfaceType.GetTypeInfo().ImplementedInterfaces)
		{
			proxyBuilder.AddInterfaceImpl(implementedInterface);
		}
		proxyBuilder.AddInterfaceImpl(interfaceType);
		return proxyBuilder.CreateType();
	}

	private static void Invoke(object[] args)
	{
		PackedArgs packedArgs = new PackedArgs(args);
		MethodBase methodBase = s_proxyAssembly.ResolveMethodToken(packedArgs.DeclaringType, packedArgs.MethodToken);
		if (methodBase.IsGenericMethodDefinition)
		{
			methodBase = ((MethodInfo)methodBase).MakeGenericMethod(packedArgs.GenericTypes);
		}
		try
		{
			object returnValue = s_dispatchProxyInvokeMethod.Invoke(packedArgs.DispatchProxy, new object[2] { methodBase, packedArgs.Args });
			packedArgs.ReturnValue = returnValue;
		}
		catch (TargetInvocationException ex)
		{
			ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
		}
	}
}
