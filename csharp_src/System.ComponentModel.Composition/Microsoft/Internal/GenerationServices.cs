using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.Internal;

internal static class GenerationServices
{
	private static readonly MethodInfo _typeGetTypeFromHandleMethod = typeof(Type).GetMethod("GetTypeFromHandle");

	private static readonly Type TypeType = typeof(Type);

	private static readonly Type StringType = typeof(string);

	private static readonly Type CharType = typeof(char);

	private static readonly Type BooleanType = typeof(bool);

	private static readonly Type ByteType = typeof(byte);

	private static readonly Type SByteType = typeof(sbyte);

	private static readonly Type Int16Type = typeof(short);

	private static readonly Type UInt16Type = typeof(ushort);

	private static readonly Type Int32Type = typeof(int);

	private static readonly Type UInt32Type = typeof(uint);

	private static readonly Type Int64Type = typeof(long);

	private static readonly Type UInt64Type = typeof(ulong);

	private static readonly Type DoubleType = typeof(double);

	private static readonly Type SingleType = typeof(float);

	private static readonly Type IEnumerableTypeofT = typeof(IEnumerable<>);

	private static readonly Type IEnumerableType = typeof(IEnumerable);

	private static readonly MethodInfo ExceptionGetData = typeof(Exception).GetProperty("Data").GetGetMethod();

	private static readonly MethodInfo DictionaryAdd = typeof(IDictionary).GetMethod("Add");

	private static readonly ConstructorInfo ObjectCtor = typeof(object).GetConstructor(Type.EmptyTypes);

	public static ILGenerator CreateGeneratorForPublicConstructor(this TypeBuilder typeBuilder, Type[] ctrArgumentTypes)
	{
		ILGenerator iLGenerator = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, ctrArgumentTypes).GetILGenerator();
		iLGenerator.Emit(OpCodes.Ldarg_0);
		iLGenerator.Emit(OpCodes.Call, ObjectCtor);
		return iLGenerator;
	}

	public static void LoadValue(this ILGenerator ilGenerator, object value)
	{
		Assumes.NotNull(ilGenerator);
		if (value == null)
		{
			ilGenerator.LoadNull();
			return;
		}
		Type type = value.GetType();
		object obj = value;
		if (type.IsEnum)
		{
			obj = Convert.ChangeType(value, Enum.GetUnderlyingType(type), null);
			type = obj.GetType();
		}
		if (type == StringType)
		{
			ilGenerator.LoadString((string)obj);
			return;
		}
		if (TypeType.IsAssignableFrom(type))
		{
			ilGenerator.LoadTypeOf((Type)obj);
			return;
		}
		if (IEnumerableType.IsAssignableFrom(type))
		{
			ilGenerator.LoadEnumerable((IEnumerable)obj);
			return;
		}
		if (type == CharType || type == BooleanType || type == ByteType || type == SByteType || type == Int16Type || type == UInt16Type || type == Int32Type)
		{
			ilGenerator.LoadInt((int)Convert.ChangeType(obj, typeof(int), CultureInfo.InvariantCulture));
			return;
		}
		if (type == UInt32Type)
		{
			ilGenerator.LoadInt((int)(uint)obj);
			return;
		}
		if (type == Int64Type)
		{
			ilGenerator.LoadLong((long)obj);
			return;
		}
		if (type == UInt64Type)
		{
			ilGenerator.LoadLong((long)(ulong)obj);
			return;
		}
		if (type == SingleType)
		{
			ilGenerator.LoadFloat((float)obj);
			return;
		}
		if (type == DoubleType)
		{
			ilGenerator.LoadDouble((double)obj);
			return;
		}
		throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.InvalidMetadataValue, value.GetType().FullName));
	}

	public static void AddItemToLocalDictionary(this ILGenerator ilGenerator, LocalBuilder dictionary, object key, object value)
	{
		Assumes.NotNull(ilGenerator);
		Assumes.NotNull(dictionary);
		Assumes.NotNull(key);
		Assumes.NotNull(value);
		ilGenerator.Emit(OpCodes.Ldloc, dictionary);
		ilGenerator.LoadValue(key);
		ilGenerator.LoadValue(value);
		ilGenerator.Emit(OpCodes.Callvirt, DictionaryAdd);
	}

	public static void AddLocalToLocalDictionary(this ILGenerator ilGenerator, LocalBuilder dictionary, object key, LocalBuilder value)
	{
		Assumes.NotNull(ilGenerator);
		Assumes.NotNull(dictionary);
		Assumes.NotNull(key);
		Assumes.NotNull(value);
		ilGenerator.Emit(OpCodes.Ldloc, dictionary);
		ilGenerator.LoadValue(key);
		ilGenerator.Emit(OpCodes.Ldloc, value);
		ilGenerator.Emit(OpCodes.Callvirt, DictionaryAdd);
	}

	public static void GetExceptionDataAndStoreInLocal(this ILGenerator ilGenerator, LocalBuilder exception, LocalBuilder dataStore)
	{
		Assumes.NotNull(ilGenerator);
		Assumes.NotNull(exception);
		Assumes.NotNull(dataStore);
		ilGenerator.Emit(OpCodes.Ldloc, exception);
		ilGenerator.Emit(OpCodes.Callvirt, ExceptionGetData);
		ilGenerator.Emit(OpCodes.Stloc, dataStore);
	}

	private static void LoadEnumerable(this ILGenerator ilGenerator, IEnumerable enumerable)
	{
		Assumes.NotNull(ilGenerator);
		Assumes.NotNull(enumerable);
		Type type = null;
		Type targetClosedInterfaceType = null;
		type = ((!ReflectionServices.TryGetGenericInterfaceType(enumerable.GetType(), IEnumerableTypeofT, out targetClosedInterfaceType)) ? typeof(object) : targetClosedInterfaceType.GetGenericArguments()[0]);
		Type localType = type.MakeArrayType();
		LocalBuilder local = ilGenerator.DeclareLocal(localType);
		ilGenerator.LoadInt(enumerable.Cast<object>().Count());
		ilGenerator.Emit(OpCodes.Newarr, type);
		ilGenerator.Emit(OpCodes.Stloc, local);
		int num = 0;
		foreach (object item in enumerable)
		{
			ilGenerator.Emit(OpCodes.Ldloc, local);
			ilGenerator.LoadInt(num);
			ilGenerator.LoadValue(item);
			if (IsBoxingRequiredForValue(item) && !type.IsValueType)
			{
				ilGenerator.Emit(OpCodes.Box, item.GetType());
			}
			ilGenerator.Emit(OpCodes.Stelem, type);
			num++;
		}
		ilGenerator.Emit(OpCodes.Ldloc, local);
	}

	private static bool IsBoxingRequiredForValue(object value)
	{
		return value?.GetType().IsValueType ?? false;
	}

	private static void LoadNull(this ILGenerator ilGenerator)
	{
		ilGenerator.Emit(OpCodes.Ldnull);
	}

	private static void LoadString(this ILGenerator ilGenerator, string s)
	{
		Assumes.NotNull(ilGenerator);
		if (s == null)
		{
			ilGenerator.LoadNull();
		}
		else
		{
			ilGenerator.Emit(OpCodes.Ldstr, s);
		}
	}

	private static void LoadInt(this ILGenerator ilGenerator, int value)
	{
		Assumes.NotNull(ilGenerator);
		ilGenerator.Emit(OpCodes.Ldc_I4, value);
	}

	private static void LoadLong(this ILGenerator ilGenerator, long value)
	{
		Assumes.NotNull(ilGenerator);
		ilGenerator.Emit(OpCodes.Ldc_I8, value);
	}

	private static void LoadFloat(this ILGenerator ilGenerator, float value)
	{
		Assumes.NotNull(ilGenerator);
		ilGenerator.Emit(OpCodes.Ldc_R4, value);
	}

	private static void LoadDouble(this ILGenerator ilGenerator, double value)
	{
		Assumes.NotNull(ilGenerator);
		ilGenerator.Emit(OpCodes.Ldc_R8, value);
	}

	private static void LoadTypeOf(this ILGenerator ilGenerator, Type type)
	{
		Assumes.NotNull(ilGenerator);
		ilGenerator.Emit(OpCodes.Ldtoken, type);
		ilGenerator.EmitCall(OpCodes.Call, _typeGetTypeFromHandleMethod, null);
	}
}
