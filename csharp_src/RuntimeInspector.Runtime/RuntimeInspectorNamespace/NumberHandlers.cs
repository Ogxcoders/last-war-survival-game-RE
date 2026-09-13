using System;
using System.Collections.Generic;
using System.Globalization;

namespace RuntimeInspectorNamespace;

public class NumberHandlers
{
	private class IntHandler : INumberHandler
	{
		public float MinValue => -2.1474836E+09f;

		public float MaxValue => 2.1474836E+09f;

		public bool TryParse(string input, out object value)
		{
			int result2;
			bool result = int.TryParse(input, NumberStyles.Integer, RuntimeInspectorUtils.numberFormat, out result2);
			value = result2;
			return result;
		}

		public bool ValuesAreEqual(object value1, object value2)
		{
			return (int)value1 == (int)value2;
		}

		public object ConvertFromFloat(float value)
		{
			return (int)value;
		}

		public float ConvertToFloat(object value)
		{
			return (int)value;
		}

		public string ToString(object value)
		{
			return ((int)value).ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private class UIntHandler : INumberHandler
	{
		public float MinValue => 0f;

		public float MaxValue => 4.2949673E+09f;

		public bool TryParse(string input, out object value)
		{
			uint result2;
			bool result = uint.TryParse(input, NumberStyles.Integer, RuntimeInspectorUtils.numberFormat, out result2);
			value = result2;
			return result;
		}

		public bool ValuesAreEqual(object value1, object value2)
		{
			return (uint)value1 == (uint)value2;
		}

		public object ConvertFromFloat(float value)
		{
			return (uint)value;
		}

		public float ConvertToFloat(object value)
		{
			return (uint)value;
		}

		public string ToString(object value)
		{
			return ((uint)value).ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private class LongHandler : INumberHandler
	{
		public float MinValue => -9.223372E+18f;

		public float MaxValue => 9.223372E+18f;

		public bool TryParse(string input, out object value)
		{
			long result2;
			bool result = long.TryParse(input, NumberStyles.Integer, RuntimeInspectorUtils.numberFormat, out result2);
			value = result2;
			return result;
		}

		public bool ValuesAreEqual(object value1, object value2)
		{
			return (long)value1 == (long)value2;
		}

		public object ConvertFromFloat(float value)
		{
			return (long)value;
		}

		public float ConvertToFloat(object value)
		{
			return (long)value;
		}

		public string ToString(object value)
		{
			return ((long)value).ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private class ULongHandler : INumberHandler
	{
		public float MinValue => 0f;

		public float MaxValue => 1.8446744E+19f;

		public bool TryParse(string input, out object value)
		{
			ulong result2;
			bool result = ulong.TryParse(input, NumberStyles.Integer, RuntimeInspectorUtils.numberFormat, out result2);
			value = result2;
			return result;
		}

		public bool ValuesAreEqual(object value1, object value2)
		{
			return (ulong)value1 == (ulong)value2;
		}

		public object ConvertFromFloat(float value)
		{
			return (ulong)value;
		}

		public float ConvertToFloat(object value)
		{
			return (ulong)value;
		}

		public string ToString(object value)
		{
			return ((ulong)value).ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private class ByteHandler : INumberHandler
	{
		public float MinValue => 0f;

		public float MaxValue => 255f;

		public bool TryParse(string input, out object value)
		{
			byte result2;
			bool result = byte.TryParse(input, NumberStyles.Integer, RuntimeInspectorUtils.numberFormat, out result2);
			value = result2;
			return result;
		}

		public bool ValuesAreEqual(object value1, object value2)
		{
			return (byte)value1 == (byte)value2;
		}

		public object ConvertFromFloat(float value)
		{
			return (byte)value;
		}

		public float ConvertToFloat(object value)
		{
			return (int)(byte)value;
		}

		public string ToString(object value)
		{
			return ((byte)value).ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private class SByteHandler : INumberHandler
	{
		public float MinValue => -128f;

		public float MaxValue => 127f;

		public bool TryParse(string input, out object value)
		{
			sbyte result2;
			bool result = sbyte.TryParse(input, NumberStyles.Integer, RuntimeInspectorUtils.numberFormat, out result2);
			value = result2;
			return result;
		}

		public bool ValuesAreEqual(object value1, object value2)
		{
			return (sbyte)value1 == (sbyte)value2;
		}

		public object ConvertFromFloat(float value)
		{
			return (sbyte)value;
		}

		public float ConvertToFloat(object value)
		{
			return (sbyte)value;
		}

		public string ToString(object value)
		{
			return ((sbyte)value).ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private class ShortHandler : INumberHandler
	{
		public float MinValue => -32768f;

		public float MaxValue => 32767f;

		public bool TryParse(string input, out object value)
		{
			short result2;
			bool result = short.TryParse(input, NumberStyles.Integer, RuntimeInspectorUtils.numberFormat, out result2);
			value = result2;
			return result;
		}

		public bool ValuesAreEqual(object value1, object value2)
		{
			return (short)value1 == (short)value2;
		}

		public object ConvertFromFloat(float value)
		{
			return (short)value;
		}

		public float ConvertToFloat(object value)
		{
			return (short)value;
		}

		public string ToString(object value)
		{
			return ((short)value).ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private class UShortHandler : INumberHandler
	{
		public float MinValue => 0f;

		public float MaxValue => 65535f;

		public bool TryParse(string input, out object value)
		{
			ushort result2;
			bool result = ushort.TryParse(input, NumberStyles.Integer, RuntimeInspectorUtils.numberFormat, out result2);
			value = result2;
			return result;
		}

		public bool ValuesAreEqual(object value1, object value2)
		{
			return (ushort)value1 == (ushort)value2;
		}

		public object ConvertFromFloat(float value)
		{
			return (ushort)value;
		}

		public float ConvertToFloat(object value)
		{
			return (int)(ushort)value;
		}

		public string ToString(object value)
		{
			return ((ushort)value).ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private class CharHandler : INumberHandler
	{
		public float MinValue => 0f;

		public float MaxValue => 65535f;

		public bool TryParse(string input, out object value)
		{
			char result2;
			bool result = char.TryParse(input, out result2);
			value = result2;
			return result;
		}

		public bool ValuesAreEqual(object value1, object value2)
		{
			return (char)value1 == (char)value2;
		}

		public object ConvertFromFloat(float value)
		{
			return (char)value;
		}

		public float ConvertToFloat(object value)
		{
			return (int)(char)value;
		}

		public string ToString(object value)
		{
			return ((char)value).ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private class FloatHandler : INumberHandler
	{
		public float MinValue => float.MinValue;

		public float MaxValue => float.MaxValue;

		public bool TryParse(string input, out object value)
		{
			float result2;
			bool result = float.TryParse(input, NumberStyles.Float, RuntimeInspectorUtils.numberFormat, out result2);
			value = result2;
			return result;
		}

		public bool ValuesAreEqual(object value1, object value2)
		{
			return (float)value1 == (float)value2;
		}

		public object ConvertFromFloat(float value)
		{
			return value;
		}

		public float ConvertToFloat(object value)
		{
			return (float)value;
		}

		public string ToString(object value)
		{
			return ((float)value).ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private class DoubleHandler : INumberHandler
	{
		public float MinValue => float.MinValue;

		public float MaxValue => float.MaxValue;

		public bool TryParse(string input, out object value)
		{
			double result2;
			bool result = double.TryParse(input, NumberStyles.Float, RuntimeInspectorUtils.numberFormat, out result2);
			value = result2;
			return result;
		}

		public bool ValuesAreEqual(object value1, object value2)
		{
			return (double)value1 == (double)value2;
		}

		public object ConvertFromFloat(float value)
		{
			return (double)value;
		}

		public float ConvertToFloat(object value)
		{
			return (float)(double)value;
		}

		public string ToString(object value)
		{
			return ((double)value).ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private class DecimalHandler : INumberHandler
	{
		public float MinValue => float.MinValue;

		public float MaxValue => float.MaxValue;

		public bool TryParse(string input, out object value)
		{
			decimal result2;
			bool result = decimal.TryParse(input, NumberStyles.Float, RuntimeInspectorUtils.numberFormat, out result2);
			value = result2;
			return result;
		}

		public bool ValuesAreEqual(object value1, object value2)
		{
			return (decimal)value1 == (decimal)value2;
		}

		public object ConvertFromFloat(float value)
		{
			return (decimal)value;
		}

		public float ConvertToFloat(object value)
		{
			return (float)(decimal)value;
		}

		public string ToString(object value)
		{
			return ((decimal)value).ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private static readonly Dictionary<Type, INumberHandler> handlers = new Dictionary<Type, INumberHandler>(16);

	public static INumberHandler Get(Type type)
	{
		if (!handlers.TryGetValue(type, out var value))
		{
			value = ((type == typeof(int)) ? new IntHandler() : ((type == typeof(float)) ? new FloatHandler() : ((type == typeof(long)) ? new LongHandler() : ((type == typeof(double)) ? new DoubleHandler() : ((type == typeof(byte)) ? new ByteHandler() : ((type == typeof(char)) ? new CharHandler() : ((type == typeof(short)) ? new ShortHandler() : ((type == typeof(uint)) ? new UIntHandler() : ((type == typeof(ulong)) ? new ULongHandler() : ((type == typeof(sbyte)) ? new SByteHandler() : ((type == typeof(ushort)) ? ((INumberHandler)new UShortHandler()) : ((INumberHandler)((!(type == typeof(decimal))) ? null : new DecimalHandler())))))))))))));
			handlers[type] = value;
		}
		return value;
	}
}
