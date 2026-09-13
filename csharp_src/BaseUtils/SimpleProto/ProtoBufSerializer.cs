using System;
using System.Collections;
using System.Reflection;
using GameFramework;

namespace SimpleProto;

internal class ProtoBufSerializer
{
	protected enum WireType
	{
		LengthDelimited,
		VarInt,
		Data32,
		Data64
	}

	protected enum ValueType
	{
		NotSupported,
		Int32,
		Int64,
		Float32,
		Float64,
		String,
		ByteArray,
		List,
		Dictionary,
		Compound
	}

	private static bool USE_VAR_INT32 = true;

	private static bool USE_VAR_INT64 = true;

	public static void to_var_int(long n, OutStream outs)
	{
		ulong num = (ulong)((n << 1) ^ (n >> 63));
		byte b;
		while (true)
		{
			b = (byte)(num & 0x7F);
			num >>= 7;
			if (num == 0L)
			{
				break;
			}
			b |= 0x80;
			outs.Write(b);
		}
		outs.Write(b);
	}

	public static long from_var_int(InStream ins)
	{
		long num = 0L;
		int num2 = 0;
		byte val;
		do
		{
			val = 0;
			ins.Read(ref val);
			long num3 = (long)val & 0x7FL;
			num += num3 << num2;
			num2 += 7;
		}
		while ((val & 0x80) != 0);
		return (num >>> 1) ^ -(num & 1);
	}

	private static ushort make_field_desc(int fsindex, WireType wire_type)
	{
		return (ushort)((uint)(fsindex << 3) | (uint)wire_type);
	}

	private static int parse_field_index(ushort desc)
	{
		return (ushort)(desc >> 3);
	}

	private static WireType parse_wire_type(ushort desc)
	{
		return (WireType)(desc & 7);
	}

	private static void save_field_desc(int fsindex, WireType wire_type, OutStream outs)
	{
		if (fsindex != -1)
		{
			ushort val = make_field_desc(fsindex, wire_type);
			outs.Write(val);
		}
	}

	private static void save_protobuf_lenth_delimited(int fsindex, Buffer buff, OutStream outs)
	{
		save_field_desc(fsindex, WireType.LengthDelimited, outs);
		outs.Write(buff);
	}

	private static void save_protobuf_string(int fsindex, string val, OutStream outs)
	{
		Buffer buffer = new Buffer(1024);
		new OutStream(buffer).Write(val);
		save_protobuf_lenth_delimited(fsindex, buffer, outs);
	}

	private static void save_protobuf_array(int fsindex, byte[] val, OutStream outs)
	{
		Buffer buffer = new Buffer(1024);
		new OutStream(buffer).Write(val);
		save_protobuf_lenth_delimited(fsindex, buffer, outs);
	}

	private static void save_protobuf_int32(int fsindex, int val, OutStream outs, WireType wire_type)
	{
		if (wire_type == WireType.VarInt)
		{
			save_field_desc(fsindex, WireType.VarInt, outs);
			to_var_int(val, outs);
		}
		else
		{
			save_field_desc(fsindex, WireType.Data32, outs);
			outs.Write(val);
		}
	}

	private static void save_protobuf_int64(int fsindex, long val, OutStream outs, WireType wire_type)
	{
		if (wire_type == WireType.VarInt)
		{
			save_field_desc(fsindex, WireType.VarInt, outs);
			to_var_int(val, outs);
		}
		else
		{
			save_field_desc(fsindex, WireType.Data64, outs);
			outs.Write(val);
		}
	}

	private static void save_protobuf_float32(int fsindex, float val, OutStream outs)
	{
		save_field_desc(fsindex, WireType.Data32, outs);
		outs.Write(val);
	}

	private static void save_protobuf_float64(int fsindex, double val, OutStream outs)
	{
		save_field_desc(fsindex, WireType.Data64, outs);
		outs.Write(val);
	}

	private static void save_type(Type ft, OutStream outs)
	{
		byte val = (byte)get_wire_type(get_value_type(ft));
		outs.Write(val);
	}

	private static WireType read_type(InStream ins)
	{
		byte val = 0;
		ins.Read(ref val);
		return (WireType)val;
	}

	private static void save_protobuf_list(int fsindex, object listobj, OutStream outs)
	{
		Buffer buffer = new Buffer(1024);
		OutStream outStream = new OutStream(buffer);
		IList list = listobj as IList;
		int count = list.Count;
		outStream.Write(count);
		if (count > 0)
		{
			save_type(list[0].GetType(), outStream);
			foreach (object item in list)
			{
				_encode_item_to_stream(-1, item, outStream);
			}
		}
		save_protobuf_lenth_delimited(fsindex, buffer, outs);
	}

	private static void load_protobuf_list(Type item_type, ref object listobj, InStream ins)
	{
		InStream inStream = load_protobuf_lenth_delimited(ins);
		IList list = listobj as IList;
		int val = 0;
		inStream.Read(ref val);
		if (val > 0)
		{
			WireType wire_type = read_type(inStream);
			for (int i = 0; i < val; i++)
			{
				object value = DecodeItemRaw(item_type, wire_type, inStream);
				list.Add(value);
			}
		}
	}

	private static void save_protobuf_dictionary(int fsindex, object dictobj, OutStream outs)
	{
		Buffer buffer = new Buffer(1024);
		OutStream outStream = new OutStream(buffer);
		IDictionary dictionary = dictobj as IDictionary;
		int count = dictionary.Count;
		outStream.Write(count);
		if (count > 0)
		{
			bool flag = true;
			foreach (object key in dictionary.Keys)
			{
				if (flag)
				{
					save_type(key.GetType(), outStream);
					save_type(dictionary[key].GetType(), outStream);
					flag = false;
				}
				_encode_item_to_stream(-1, key, outStream);
				_encode_item_to_stream(-1, dictionary[key], outStream);
			}
		}
		save_protobuf_lenth_delimited(fsindex, buffer, outs);
	}

	private static void load_protobuf_dictionary(Type key_type, Type value_type, ref object dictobj, InStream ins)
	{
		InStream inStream = load_protobuf_lenth_delimited(ins);
		IDictionary dictionary = dictobj as IDictionary;
		int val = 0;
		inStream.Read(ref val);
		if (val > 0)
		{
			WireType wire_type = read_type(inStream);
			WireType wire_type2 = read_type(inStream);
			for (int i = 0; i < val; i++)
			{
				object key = DecodeItemRaw(key_type, wire_type, inStream);
				object value = DecodeItemRaw(value_type, wire_type2, inStream);
				dictionary.Add(key, value);
			}
		}
	}

	private static InStream load_protobuf_lenth_delimited(InStream ins)
	{
		Buffer buffer = new Buffer(1024);
		ins.Read(ref buffer);
		return new InStream(buffer);
	}

	private static object load_protobuf_string(InStream ins, WireType wire_type)
	{
		if (wire_type == WireType.LengthDelimited)
		{
			string val = "";
			load_protobuf_lenth_delimited(ins).Read(ref val);
			return val;
		}
		return null;
	}

	private static object load_protobuf_array(InStream ins, WireType wire_type)
	{
		if (wire_type == WireType.LengthDelimited)
		{
			byte[] buffer = new byte[0];
			load_protobuf_lenth_delimited(ins).Read(ref buffer);
			return buffer;
		}
		return null;
	}

	private static void load_protobuf_compound(object obj, InStream ins)
	{
		InStream inStream = load_protobuf_lenth_delimited(ins);
		while (inStream.BytesLeft() >= 2)
		{
			ushort val = 0;
			inStream.Read(ref val);
			int num = parse_field_index(val);
			WireType wire_type = parse_wire_type(val);
			if (!DecodeItem(obj, wire_type, num, inStream))
			{
				skip_buffer(wire_type, inStream);
				Log.Warning("LoadItemFromStream " + obj.GetType().Name + "[" + num + "] skiped!");
			}
		}
	}

	private static object load_protobuf_int32(InStream ins, WireType wire_type)
	{
		switch (wire_type)
		{
		case WireType.VarInt:
			return (int)from_var_int(ins);
		case WireType.Data32:
		{
			int val = 0;
			ins.Read(ref val);
			return val;
		}
		default:
			return null;
		}
	}

	private static object load_protobuf_int64(InStream ins, WireType wire_type)
	{
		long val = 0L;
		switch (wire_type)
		{
		case WireType.VarInt:
			val = from_var_int(ins);
			return val;
		case WireType.Data64:
			ins.Read(ref val);
			return val;
		default:
			return null;
		}
	}

	private static object load_protobuf_float32(InStream ins, WireType wire_type)
	{
		float val = 0f;
		if (wire_type == WireType.Data32)
		{
			ins.Read(ref val);
			return val;
		}
		return null;
	}

	private static object load_protobuf_float64(InStream ins, WireType wire_type)
	{
		double val = 0.0;
		if (wire_type == WireType.Data64)
		{
			ins.Read(ref val);
			return val;
		}
		return null;
	}

	private static void skip_buffer(WireType wire_type, InStream ins)
	{
		int val = 0;
		switch (wire_type)
		{
		case WireType.LengthDelimited:
			ins.Read(ref val);
			ins.Skip(val);
			break;
		case WireType.VarInt:
			from_var_int(ins);
			break;
		case WireType.Data32:
			ins.Skip(4);
			break;
		case WireType.Data64:
			ins.Skip(8);
			break;
		}
	}

	private static void save_protobuf_compound(int fsindex, object obj, OutStream outs)
	{
		Buffer buffer = new Buffer(1024);
		OutStream outs2 = new OutStream(buffer);
		_encode_compound_to_stream(obj, outs2);
		save_protobuf_lenth_delimited(fsindex, buffer, outs);
	}

	private static void _encode_compound_to_stream(object obj, OutStream outs)
	{
		FieldInfo[] fields = obj.GetType().GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			object[] customAttributes = fieldInfo.GetCustomAttributes(inherit: false);
			for (int j = 0; j < customAttributes.Length; j++)
			{
				int index = ((ProtoBufAttribute)customAttributes[j]).Index;
				object value = fieldInfo.GetValue(obj);
				if (value != null)
				{
					_encode_item_to_stream(index, value, outs);
				}
			}
		}
	}

	private static ValueType get_value_type(Type ft)
	{
		if (ft == typeof(int) || ft == typeof(uint) || ft == typeof(byte) || ft == typeof(sbyte) || ft == typeof(short) || ft == typeof(ushort) || ft == typeof(bool))
		{
			return ValueType.Int32;
		}
		if (ft == typeof(char))
		{
			throw new NotImplementedException("char not supported");
		}
		if (ft == typeof(long) || ft == typeof(ulong))
		{
			return ValueType.Int64;
		}
		if (ft == typeof(float))
		{
			return ValueType.Float32;
		}
		if (ft == typeof(double))
		{
			return ValueType.Float64;
		}
		if (ft == typeof(string))
		{
			return ValueType.String;
		}
		if (ft == typeof(byte[]))
		{
			return ValueType.ByteArray;
		}
		if (ft.Name == "List`1")
		{
			return ValueType.List;
		}
		if (ft.Name == "LinkedList`1")
		{
			throw new NotImplementedException("LinkedList not supported");
		}
		if (ft.Name == "Dictionary`2")
		{
			return ValueType.Dictionary;
		}
		return ValueType.Compound;
	}

	private static WireType get_wire_type(ValueType vt)
	{
		switch (vt)
		{
		case ValueType.Int32:
		case ValueType.Float32:
			if (USE_VAR_INT32)
			{
				return WireType.VarInt;
			}
			return WireType.Data32;
		case ValueType.Int64:
		case ValueType.Float64:
			if (USE_VAR_INT64)
			{
				return WireType.VarInt;
			}
			return WireType.Data64;
		default:
			return WireType.LengthDelimited;
		}
	}

	private static void _encode_item_to_stream(int index, object val, OutStream outs)
	{
		ValueType valueType = get_value_type(val.GetType());
		WireType wire_type = get_wire_type(valueType);
		switch (valueType)
		{
		case ValueType.Int32:
		{
			int val2 = Convert.ToInt32(val);
			save_protobuf_int32(index, val2, outs, wire_type);
			break;
		}
		case ValueType.Int64:
			save_protobuf_int64(index, (long)val, outs, wire_type);
			break;
		case ValueType.Float32:
			save_protobuf_float32(index, (float)val, outs);
			break;
		case ValueType.Float64:
			save_protobuf_float64(index, (double)val, outs);
			break;
		case ValueType.String:
			save_protobuf_string(index, (string)val, outs);
			break;
		case ValueType.ByteArray:
			save_protobuf_array(index, (byte[])val, outs);
			break;
		case ValueType.List:
			save_protobuf_list(index, val, outs);
			break;
		case ValueType.Dictionary:
			save_protobuf_dictionary(index, val, outs);
			break;
		default:
			save_protobuf_compound(index, val, outs);
			break;
		}
	}

	private static object DecodeItemRaw(Type ft, WireType wire_type, InStream ins)
	{
		ValueType valueType = get_value_type(ft);
		object dictobj = null;
		switch (valueType)
		{
		case ValueType.Int32:
			dictobj = load_protobuf_int32(ins, wire_type);
			break;
		case ValueType.Int64:
			dictobj = load_protobuf_int64(ins, wire_type);
			break;
		case ValueType.Float32:
			dictobj = load_protobuf_float32(ins, wire_type);
			break;
		case ValueType.Float64:
			dictobj = load_protobuf_float64(ins, wire_type);
			break;
		case ValueType.String:
			dictobj = load_protobuf_string(ins, wire_type);
			break;
		case ValueType.ByteArray:
			dictobj = load_protobuf_array(ins, wire_type);
			break;
		case ValueType.List:
			dictobj = Activator.CreateInstance(ft);
			load_protobuf_list(ft.GetGenericArguments()[0], ref dictobj, ins);
			break;
		case ValueType.Dictionary:
		{
			dictobj = Activator.CreateInstance(ft);
			Type[] genericArguments = ft.GetGenericArguments();
			load_protobuf_dictionary(genericArguments[0], genericArguments[1], ref dictobj, ins);
			break;
		}
		case ValueType.Compound:
			dictobj = Activator.CreateInstance(ft);
			load_protobuf_compound(dictobj, ins);
			break;
		}
		if (dictobj != null)
		{
			return Convert.ChangeType(dictobj, ft);
		}
		Log.Warning(string.Concat("type does not match ", ft, " vs ", wire_type));
		return null;
	}

	private static bool DecodeItem(object obj, WireType wire_type, int fsindex, InStream ins)
	{
		FieldInfo[] fields = obj.GetType().GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			object[] customAttributes = fieldInfo.GetCustomAttributes(inherit: false);
			for (int j = 0; j < customAttributes.Length; j++)
			{
				ProtoBufAttribute protoBufAttribute = (ProtoBufAttribute)customAttributes[j];
				if (fsindex == (ushort)protoBufAttribute.Index)
				{
					object obj2 = DecodeItemRaw(fieldInfo.FieldType, wire_type, ins);
					if (obj2 != null)
					{
						fieldInfo.SetValue(obj, obj2);
						return true;
					}
				}
			}
		}
		return false;
	}

	public static void SaveStream(object obj, OutStream outs)
	{
		save_protobuf_compound(-1, obj, outs);
	}

	public static void LoadStream(object obj, InStream ins)
	{
		load_protobuf_compound(obj, ins);
	}
}
