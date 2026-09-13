using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SFSLitJsonLw;
using Sfs2XLw.Entities.Data;
using Sfs2XLw.Exceptions;
using Sfs2XLw.Util;

namespace Sfs2XLw.Protocol.Serialization;

public class DefaultSFSDataSerializer : ISFSDataSerializer
{
	private static readonly string CLASS_MARKER_KEY = "$C";

	private static readonly string CLASS_FIELDS_KEY = "$F";

	private static readonly string FIELD_NAME_KEY = "N";

	private static readonly string FIELD_VALUE_KEY = "V";

	private static DefaultSFSDataSerializer instance;

	private static Assembly runningAssembly = null;

	public static DefaultSFSDataSerializer Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new DefaultSFSDataSerializer();
			}
			return instance;
		}
	}

	public static Assembly RunningAssembly
	{
		get
		{
			return runningAssembly;
		}
		set
		{
			runningAssembly = value;
		}
	}

	private DefaultSFSDataSerializer()
	{
	}

	public ByteArray Object2Binary(ISFSObject obj)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(Convert.ToByte(18));
		byteArray.WriteShort(Convert.ToInt16(obj.Size()));
		return Obj2bin(obj, byteArray);
	}

	private ByteArray Obj2bin(ISFSObject obj, ByteArray buffer)
	{
		string[] keys = obj.GetKeys();
		foreach (string text in keys)
		{
			SFSDataWrapper data = obj.GetData(text);
			buffer = EncodeSFSObjectKey(buffer, text);
			buffer = EncodeObject(buffer, data.Type, data.Data);
		}
		return buffer;
	}

	public ByteArray Array2Binary(ISFSArray array)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(Convert.ToByte(17));
		byteArray.WriteShort(Convert.ToInt16(array.Size()));
		return Arr2bin(array, byteArray);
	}

	private ByteArray Arr2bin(ISFSArray array, ByteArray buffer)
	{
		for (int i = 0; i < array.Size(); i++)
		{
			SFSDataWrapper wrappedElementAt = array.GetWrappedElementAt(i);
			buffer = EncodeObject(buffer, wrappedElementAt.Type, wrappedElementAt.Data);
		}
		return buffer;
	}

	public ISFSObject Binary2Object(ByteArray data, int offset = 0)
	{
		if (data.Length < 3)
		{
			throw new SFSCodecError("Can't decode an SFSObject. Byte data is insufficient. Size: " + data.Length + " byte(s)");
		}
		data.Position = offset;
		return DecodeSFSObject(data);
	}

	private ISFSObject DecodeSFSObject(ByteArray buffer)
	{
		SFSObject sFSObject = SFSObject.NewInstance();
		byte b = buffer.ReadByte();
		if (b != Convert.ToByte(18))
		{
			throw new SFSCodecError(string.Concat("Invalid SFSDataType. Expected: ", SFSDataType.SFS_OBJECT, ", found: ", b));
		}
		int num = buffer.ReadShort();
		if (num < 0)
		{
			throw new SFSCodecError("Can't decode SFSObject. Size is negative: " + num);
		}
		try
		{
			for (int i = 0; i < num; i++)
			{
				int byteCount;
				string text = buffer.ReadUTF(out byteCount);
				SFSDataWrapper sFSDataWrapper = DecodeObject(buffer, byteCount);
				if (sFSDataWrapper != null)
				{
					sFSObject.Put(text, sFSDataWrapper);
					continue;
				}
				throw new SFSCodecError("Could not decode value for SFSObject with key: " + text);
			}
			return sFSObject;
		}
		catch (SFSCodecError sFSCodecError)
		{
			throw sFSCodecError;
		}
	}

	public ISFSArray Binary2Array(ByteArray data, int offset = 0)
	{
		if (data.Length < 3)
		{
			throw new SFSCodecError("Can't decode an SFSArray. Byte data is insufficient. Size: " + data.Length + " byte(s)");
		}
		data.Position = offset;
		return DecodeSFSArray(data);
	}

	private ISFSArray DecodeSFSArray(ByteArray buffer)
	{
		ISFSArray iSFSArray = SFSArray.NewInstance();
		SFSDataType sFSDataType = (SFSDataType)Convert.ToInt32(buffer.ReadByte());
		if (sFSDataType != SFSDataType.SFS_ARRAY)
		{
			throw new SFSCodecError(string.Concat("Invalid SFSDataType. Expected: ", SFSDataType.SFS_ARRAY, ", found: ", sFSDataType));
		}
		int num = buffer.ReadShort();
		if (num < 0)
		{
			throw new SFSCodecError("Can't decode SFSArray. Size is negative: " + num);
		}
		try
		{
			for (int i = 0; i < num; i++)
			{
				SFSDataWrapper sFSDataWrapper = DecodeObject(buffer, -1);
				if (sFSDataWrapper != null)
				{
					iSFSArray.Add(sFSDataWrapper);
					continue;
				}
				throw new SFSCodecError("Could not decode SFSArray item at index: " + i);
			}
			return iSFSArray;
		}
		catch (SFSCodecError sFSCodecError)
		{
			throw sFSCodecError;
		}
	}

	private SFSDataWrapper DecodeObject(ByteArray buffer, int keyByteCount)
	{
		SFSDataType sFSDataType = (SFSDataType)Convert.ToInt32(buffer.ReadByte());
		switch (sFSDataType)
		{
		case SFSDataType.NULL:
			return BinDecode_NULL(buffer, keyByteCount);
		case SFSDataType.BOOL:
			return BinDecode_BOOL(buffer, keyByteCount);
		case SFSDataType.BOOL_ARRAY:
			return BinDecode_BOOL_ARRAY(buffer, keyByteCount);
		case SFSDataType.BYTE:
			return BinDecode_BYTE(buffer, keyByteCount);
		case SFSDataType.BYTE_ARRAY:
			return BinDecode_BYTE_ARRAY(buffer, keyByteCount);
		case SFSDataType.SHORT:
			return BinDecode_SHORT(buffer, keyByteCount);
		case SFSDataType.SHORT_ARRAY:
			return BinDecode_SHORT_ARRAY(buffer, keyByteCount);
		case SFSDataType.INT:
			return BinDecode_INT(buffer, keyByteCount);
		case SFSDataType.INT_ARRAY:
			return BinDecode_INT_ARRAY(buffer, keyByteCount);
		case SFSDataType.LONG:
			return BinDecode_LONG(buffer, keyByteCount);
		case SFSDataType.LONG_ARRAY:
			return BinDecode_LONG_ARRAY(buffer, keyByteCount);
		case SFSDataType.FLOAT:
			return BinDecode_FLOAT(buffer, keyByteCount);
		case SFSDataType.FLOAT_ARRAY:
			return BinDecode_FLOAT_ARRAY(buffer, keyByteCount);
		case SFSDataType.DOUBLE:
			return BinDecode_DOUBLE(buffer, keyByteCount);
		case SFSDataType.DOUBLE_ARRAY:
			return BinDecode_DOUBLE_ARRAY(buffer, keyByteCount);
		case SFSDataType.UTF_STRING:
			return BinDecode_UTF_STRING(buffer, keyByteCount);
		case SFSDataType.TEXT:
			return BinDecode_TEXT(buffer, keyByteCount);
		case SFSDataType.UTF_STRING_ARRAY:
			return BinDecode_UTF_STRING_ARRAY(buffer, keyByteCount);
		case SFSDataType.SFS_ARRAY:
			buffer.Position--;
			return new SFSRefDataWrapper(17, DecodeSFSArray(buffer), keyByteCount);
		case SFSDataType.SFS_OBJECT:
		{
			buffer.Position--;
			ISFSObject iSFSObject = DecodeSFSObject(buffer);
			byte type = Convert.ToByte(18);
			object data = iSFSObject;
			if (iSFSObject.ContainsKey(CLASS_MARKER_KEY) && iSFSObject.ContainsKey(CLASS_FIELDS_KEY))
			{
				type = Convert.ToByte(19);
				data = Sfs2Cs(iSFSObject);
			}
			return new SFSRefDataWrapper(type, data, keyByteCount);
		}
		default:
			throw new Exception("Unknow SFSDataType ID: " + sFSDataType);
		}
	}

	private ByteArray EncodeObject(ByteArray buffer, int typeId, object data)
	{
		buffer = (SFSDataType)typeId switch
		{
			SFSDataType.NULL => BinEncode_NULL(buffer), 
			SFSDataType.BOOL => BinEncode_BOOL(buffer, (bool)data), 
			SFSDataType.BYTE => BinEncode_BYTE(buffer, (byte)data), 
			SFSDataType.SHORT => BinEncode_SHORT(buffer, (short)data), 
			SFSDataType.INT => BinEncode_INT(buffer, (int)data), 
			SFSDataType.LONG => BinEncode_LONG(buffer, (long)data), 
			SFSDataType.FLOAT => BinEncode_FLOAT(buffer, (float)data), 
			SFSDataType.DOUBLE => BinEncode_DOUBLE(buffer, (double)data), 
			SFSDataType.UTF_STRING => BinEncode_UTF_STRING(buffer, (string)data), 
			SFSDataType.TEXT => BinEncode_TEXT(buffer, (string)data), 
			SFSDataType.BOOL_ARRAY => BinEncode_BOOL_ARRAY(buffer, (bool[])data), 
			SFSDataType.BYTE_ARRAY => BinEncode_BYTE_ARRAY(buffer, (ByteArray)data), 
			SFSDataType.SHORT_ARRAY => BinEncode_SHORT_ARRAY(buffer, (short[])data), 
			SFSDataType.INT_ARRAY => BinEncode_INT_ARRAY(buffer, (int[])data), 
			SFSDataType.LONG_ARRAY => BinEncode_LONG_ARRAY(buffer, (long[])data), 
			SFSDataType.FLOAT_ARRAY => BinEncode_FLOAT_ARRAY(buffer, (float[])data), 
			SFSDataType.DOUBLE_ARRAY => BinEncode_DOUBLE_ARRAY(buffer, (double[])data), 
			SFSDataType.UTF_STRING_ARRAY => BinEncode_UTF_STRING_ARRAY(buffer, (string[])data), 
			SFSDataType.SFS_ARRAY => AddData(buffer, Array2Binary((ISFSArray)data)), 
			SFSDataType.SFS_OBJECT => AddData(buffer, Object2Binary((SFSObject)data)), 
			SFSDataType.CLASS => AddData(buffer, Object2Binary(Cs2Sfs(data))), 
			_ => throw new SFSCodecError("Unrecognized type in SFSObject serialization: " + typeId), 
		};
		return buffer;
	}

	private SFSDataWrapper BinDecode_NULL(ByteArray buffer, int keyByteCount)
	{
		return new SFSRefDataWrapper(SFSDataType.NULL, null, keyByteCount);
	}

	private SFSDataWrapper BinDecode_BOOL(ByteArray buffer, int keyByteCount)
	{
		return new SFSPrimitiveDataWrapper<bool>(SFSDataType.BOOL, buffer.ReadBool(), keyByteCount);
	}

	private SFSDataWrapper BinDecode_BYTE(ByteArray buffer, int keyByteCount)
	{
		return new SFSPrimitiveDataWrapper<byte>(SFSDataType.BYTE, buffer.ReadByte(), keyByteCount);
	}

	private SFSDataWrapper BinDecode_SHORT(ByteArray buffer, int keyByteCount)
	{
		return new SFSPrimitiveDataWrapper<short>(SFSDataType.SHORT, buffer.ReadShort(), keyByteCount);
	}

	private SFSDataWrapper BinDecode_INT(ByteArray buffer, int keyByteCount)
	{
		return new SFSPrimitiveDataWrapper<int>(SFSDataType.INT, buffer.ReadInt(), keyByteCount);
	}

	private SFSDataWrapper BinDecode_LONG(ByteArray buffer, int keyByteCount)
	{
		return new SFSPrimitiveDataWrapper<long>(SFSDataType.LONG, buffer.ReadLong(), keyByteCount);
	}

	private SFSDataWrapper BinDecode_FLOAT(ByteArray buffer, int keyByteCount)
	{
		return new SFSPrimitiveDataWrapper<float>(SFSDataType.FLOAT, buffer.ReadFloat(), keyByteCount);
	}

	private SFSDataWrapper BinDecode_DOUBLE(ByteArray buffer, int keyByteCount)
	{
		return new SFSPrimitiveDataWrapper<double>(SFSDataType.DOUBLE, buffer.ReadDouble(), keyByteCount);
	}

	private SFSDataWrapper BinDecode_UTF_STRING(ByteArray buffer, int keyByteCount)
	{
		int byteCount;
		string data = buffer.ReadUTF(out byteCount);
		return new SFSStringDataWrapper(SFSDataType.UTF_STRING, data, keyByteCount, byteCount);
	}

	private SFSDataWrapper BinDecode_TEXT(ByteArray buffer, int keyByteCount)
	{
		int byteCount;
		string data = buffer.ReadText(out byteCount);
		return new SFSStringDataWrapper(SFSDataType.UTF_STRING, data, keyByteCount, byteCount);
	}

	private SFSDataWrapper BinDecode_BOOL_ARRAY(ByteArray buffer, int keyByteCount)
	{
		int typedArraySize = GetTypedArraySize(buffer);
		bool[] array = new bool[typedArraySize];
		for (int i = 0; i < typedArraySize; i++)
		{
			array[i] = buffer.ReadBool();
		}
		return new SFSRefDataWrapper(SFSDataType.BOOL_ARRAY, array, keyByteCount);
	}

	private SFSDataWrapper BinDecode_BYTE_ARRAY(ByteArray buffer, int keyByteCount)
	{
		int num = buffer.ReadInt();
		if (num < 0)
		{
			throw new SFSCodecError("Array negative size: " + num);
		}
		ByteArray data = new ByteArray(buffer.ReadBytes(num));
		return new SFSRefDataWrapper(SFSDataType.BYTE_ARRAY, data, keyByteCount);
	}

	private SFSDataWrapper BinDecode_SHORT_ARRAY(ByteArray buffer, int keyByteCount)
	{
		int typedArraySize = GetTypedArraySize(buffer);
		short[] array = new short[typedArraySize];
		for (int i = 0; i < typedArraySize; i++)
		{
			array[i] = buffer.ReadShort();
		}
		return new SFSRefDataWrapper(SFSDataType.SHORT_ARRAY, array, keyByteCount);
	}

	private SFSDataWrapper BinDecode_INT_ARRAY(ByteArray buffer, int keyByteCount)
	{
		int typedArraySize = GetTypedArraySize(buffer);
		int[] array = new int[typedArraySize];
		for (int i = 0; i < typedArraySize; i++)
		{
			array[i] = buffer.ReadInt();
		}
		return new SFSRefDataWrapper(SFSDataType.INT_ARRAY, array, keyByteCount);
	}

	private SFSDataWrapper BinDecode_LONG_ARRAY(ByteArray buffer, int keyByteCount)
	{
		int typedArraySize = GetTypedArraySize(buffer);
		long[] array = new long[typedArraySize];
		for (int i = 0; i < typedArraySize; i++)
		{
			array[i] = buffer.ReadLong();
		}
		return new SFSRefDataWrapper(SFSDataType.LONG_ARRAY, array, keyByteCount);
	}

	private SFSDataWrapper BinDecode_FLOAT_ARRAY(ByteArray buffer, int keyByteCount)
	{
		int typedArraySize = GetTypedArraySize(buffer);
		float[] array = new float[typedArraySize];
		for (int i = 0; i < typedArraySize; i++)
		{
			array[i] = buffer.ReadFloat();
		}
		return new SFSRefDataWrapper(SFSDataType.FLOAT_ARRAY, array, keyByteCount);
	}

	private SFSDataWrapper BinDecode_DOUBLE_ARRAY(ByteArray buffer, int keyByteCount)
	{
		int typedArraySize = GetTypedArraySize(buffer);
		double[] array = new double[typedArraySize];
		for (int i = 0; i < typedArraySize; i++)
		{
			array[i] = buffer.ReadDouble();
		}
		return new SFSRefDataWrapper(SFSDataType.DOUBLE_ARRAY, array, keyByteCount);
	}

	private SFSDataWrapper BinDecode_UTF_STRING_ARRAY(ByteArray buffer, int keyByteCount)
	{
		int typedArraySize = GetTypedArraySize(buffer);
		string[] array = new string[typedArraySize];
		for (int i = 0; i < typedArraySize; i++)
		{
			array[i] = buffer.ReadUTF();
		}
		return new SFSRefDataWrapper(SFSDataType.UTF_STRING_ARRAY, array, keyByteCount);
	}

	private int GetTypedArraySize(ByteArray buffer)
	{
		short num = buffer.ReadShort();
		if (num < 0)
		{
			throw new SFSCodecError("Array negative size: " + num);
		}
		return num;
	}

	private ByteArray BinEncode_NULL(ByteArray buffer)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte((byte)0);
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_BOOL(ByteArray buffer, bool val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.BOOL);
		byteArray.WriteBool(val);
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_BYTE(ByteArray buffer, byte val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.BYTE);
		byteArray.WriteByte(val);
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_SHORT(ByteArray buffer, short val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.SHORT);
		byteArray.WriteShort(val);
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_INT(ByteArray buffer, int val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.INT);
		byteArray.WriteInt(val);
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_LONG(ByteArray buffer, long val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.LONG);
		byteArray.WriteLong(val);
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_FLOAT(ByteArray buffer, float val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.FLOAT);
		byteArray.WriteFloat(val);
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_DOUBLE(ByteArray buffer, double val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.DOUBLE);
		byteArray.WriteDouble(val);
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_INT(ByteArray buffer, double val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.DOUBLE);
		byteArray.WriteDouble(val);
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_UTF_STRING(ByteArray buffer, string val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.UTF_STRING);
		byteArray.WriteUTF(val);
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_TEXT(ByteArray buffer, string val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.TEXT);
		byteArray.WriteText(val);
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_BOOL_ARRAY(ByteArray buffer, bool[] val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.BOOL_ARRAY);
		byteArray.WriteShort(Convert.ToInt16(val.Length));
		for (int i = 0; i < val.Length; i++)
		{
			byteArray.WriteBool(val[i]);
		}
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_BYTE_ARRAY(ByteArray buffer, ByteArray val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.BYTE_ARRAY);
		byteArray.WriteInt(val.Length);
		byteArray.WriteBytes(val.Bytes);
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_SHORT_ARRAY(ByteArray buffer, short[] val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.SHORT_ARRAY);
		byteArray.WriteShort(Convert.ToInt16(val.Length));
		for (int i = 0; i < val.Length; i++)
		{
			byteArray.WriteShort(val[i]);
		}
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_INT_ARRAY(ByteArray buffer, int[] val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.INT_ARRAY);
		byteArray.WriteShort(Convert.ToInt16(val.Length));
		for (int i = 0; i < val.Length; i++)
		{
			byteArray.WriteInt(val[i]);
		}
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_LONG_ARRAY(ByteArray buffer, long[] val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.LONG_ARRAY);
		byteArray.WriteShort(Convert.ToInt16(val.Length));
		for (int i = 0; i < val.Length; i++)
		{
			byteArray.WriteLong(val[i]);
		}
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_FLOAT_ARRAY(ByteArray buffer, float[] val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.FLOAT_ARRAY);
		byteArray.WriteShort(Convert.ToInt16(val.Length));
		for (int i = 0; i < val.Length; i++)
		{
			byteArray.WriteFloat(val[i]);
		}
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_DOUBLE_ARRAY(ByteArray buffer, double[] val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.DOUBLE_ARRAY);
		byteArray.WriteShort(Convert.ToInt16(val.Length));
		for (int i = 0; i < val.Length; i++)
		{
			byteArray.WriteDouble(val[i]);
		}
		return AddData(buffer, byteArray);
	}

	private ByteArray BinEncode_UTF_STRING_ARRAY(ByteArray buffer, string[] val)
	{
		ByteArray byteArray = new ByteArray();
		byteArray.WriteByte(SFSDataType.UTF_STRING_ARRAY);
		byteArray.WriteShort(Convert.ToInt16(val.Length));
		for (int i = 0; i < val.Length; i++)
		{
			byteArray.WriteUTF(val[i]);
		}
		return AddData(buffer, byteArray);
	}

	private ByteArray EncodeSFSObjectKey(ByteArray buffer, string val)
	{
		buffer.WriteUTF(val);
		return buffer;
	}

	private ByteArray AddData(ByteArray buffer, ByteArray newData)
	{
		buffer.WriteBytes(newData.Bytes);
		return buffer;
	}

	public string Object2Json(Dictionary<string, object> map)
	{
		return JsonMapper.ToJson(map);
	}

	public void flattenObject(Dictionary<string, object> map, ISFSObject sfsObj)
	{
		string[] keys = sfsObj.GetKeys();
		foreach (string key in keys)
		{
			SFSDataWrapper data = sfsObj.GetData(key);
			if (data.Type == 18)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				map.Add(key, dictionary);
				flattenObject(dictionary, (ISFSObject)data.Data);
			}
			else if (data.Type == 17)
			{
				List<object> list = new List<object>();
				map.Add(key, list);
				flattenArray(list, (ISFSArray)data.Data);
			}
			else
			{
				map.Add(key, data.Data);
			}
		}
	}

	public string Array2Json(List<object> list)
	{
		return JsonMapper.ToJson(list);
	}

	public void flattenArray(List<object> list, ISFSArray sfsArray)
	{
		for (int i = 0; i < sfsArray.Size(); i++)
		{
			SFSDataWrapper wrappedElementAt = sfsArray.GetWrappedElementAt(i);
			if (wrappedElementAt.Type == 18)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				list.Add(dictionary);
				flattenObject(dictionary, (ISFSObject)wrappedElementAt.Data);
			}
			else if (wrappedElementAt.Type == 17)
			{
				List<object> list2 = new List<object>();
				list.Add(list2);
				flattenArray(list2, (ISFSArray)wrappedElementAt.Data);
			}
			else
			{
				list.Add(wrappedElementAt.Data);
			}
		}
	}

	public ISFSObject Json2Object(string jsonStr)
	{
		if (jsonStr.Length < 2)
		{
			throw new InvalidOperationException("Can't decode SFSObject: JSON String is too short. Len: " + jsonStr.Length);
		}
		JsonData jdo = JsonMapper.ToObject(jsonStr);
		return decodeSFSObject(jdo);
	}

	public ISFSArray Json2Array(string jsonStr)
	{
		if (jsonStr.Length < 2)
		{
			throw new InvalidOperationException("Can't decode SFSArray: JSON String is too short. Len: " + jsonStr.Length);
		}
		JsonData jdo = JsonMapper.ToObject(jsonStr);
		return decodeSFSArray(jdo);
	}

	private ISFSObject decodeSFSObject(JsonData jdo)
	{
		ISFSObject iSFSObject = SFSObjectLite.NewInstance();
		foreach (string key in jdo.Keys)
		{
			JsonData jdo2 = jdo[key];
			SFSDataWrapper sFSDataWrapper = decodeJsonObject(jdo2);
			if (sFSDataWrapper != null)
			{
				iSFSObject.Put(key, sFSDataWrapper);
				continue;
			}
			throw new InvalidOperationException("JSON > ISFSObject error: could not decode value for key: " + key);
		}
		return iSFSObject;
	}

	private ISFSArray decodeSFSArray(JsonData jdo)
	{
		ISFSArray iSFSArray = SFSArrayLite.NewInstance();
		for (int i = 0; i < jdo.Count; i++)
		{
			JsonData jsonData = jdo[i];
			SFSDataWrapper sFSDataWrapper = decodeJsonObject(jsonData);
			if (sFSDataWrapper != null)
			{
				iSFSArray.Add(sFSDataWrapper);
				continue;
			}
			throw new InvalidOperationException("JSON > ISFSArray error: could not decode value for object: " + jsonData);
		}
		return iSFSArray;
	}

	private SFSDataWrapper decodeJsonObject(JsonData jdo)
	{
		if (jdo == null)
		{
			return new SFSRefDataWrapper(SFSDataType.NULL, jdo, -1);
		}
		if (jdo.IsInt)
		{
			return new SFSPrimitiveDataWrapper<int>(SFSDataType.INT, (int)jdo, -1);
		}
		if (jdo.IsLong)
		{
			return new SFSPrimitiveDataWrapper<long>(SFSDataType.LONG, (long)jdo, -1);
		}
		if (jdo.IsDouble)
		{
			return new SFSPrimitiveDataWrapper<double>(SFSDataType.DOUBLE, (double)jdo, -1);
		}
		if (jdo.IsBoolean)
		{
			return new SFSPrimitiveDataWrapper<bool>(SFSDataType.BOOL, (bool)jdo, -1);
		}
		if (jdo.IsString)
		{
			return new SFSStringDataWrapper(SFSDataType.UTF_STRING, (string)jdo, -1, -1);
		}
		if (jdo.IsObject)
		{
			if (jdo.Keys.Count == 0)
			{
				return new SFSRefDataWrapper(SFSDataType.NULL, null, -1);
			}
			return new SFSRefDataWrapper(SFSDataType.SFS_OBJECT, decodeSFSObject(jdo), -1);
		}
		if (jdo.IsArray)
		{
			return new SFSRefDataWrapper(SFSDataType.SFS_ARRAY, decodeSFSArray(jdo), -1);
		}
		throw new ArgumentException(string.Format("Unrecognized DataType while converting JsonData object to SFSObject. Object: %s, Type: %s", jdo.ToString(), (jdo == null) ? "null" : jdo.GetType().ToString()));
	}

	public ISFSObject Cs2Sfs(object csObj)
	{
		ISFSObject iSFSObject = SFSObject.NewInstance();
		ConvertCsObj(csObj, iSFSObject);
		return iSFSObject;
	}

	private void ConvertCsObj(object csObj, ISFSObject sfsObj)
	{
		Type type = csObj.GetType();
		string fullName = type.FullName;
		if (!(csObj is SerializableSFSType))
		{
			throw new SFSCodecError(string.Concat("Cannot serialize object: ", csObj, ", type: ", fullName, " -- It doesn't implement the SerializableSFSType interface"));
		}
		ISFSArray iSFSArray = SFSArray.NewInstance();
		sfsObj.PutUtfString(CLASS_MARKER_KEY, fullName);
		sfsObj.PutSFSArray(CLASS_FIELDS_KEY, iSFSArray);
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo fieldInfo in fields)
		{
			string name = fieldInfo.Name;
			object value = fieldInfo.GetValue(csObj);
			ISFSObject iSFSObject = SFSObject.NewInstance();
			SFSDataWrapper sFSDataWrapper = WrapField(value);
			if (sFSDataWrapper != null)
			{
				iSFSObject.PutUtfString(FIELD_NAME_KEY, name);
				iSFSObject.Put(FIELD_VALUE_KEY, sFSDataWrapper);
				iSFSArray.AddSFSObject(iSFSObject);
				continue;
			}
			throw new SFSCodecError(string.Concat("Cannot serialize field of object: ", csObj, ", field: ", name, ", type: ", fieldInfo.GetType().Name, " -- unsupported type!"));
		}
	}

	private SFSDataWrapper WrapField(object val)
	{
		if (val == null)
		{
			return new SFSRefDataWrapper(SFSDataType.NULL, null, -1);
		}
		SFSDataWrapper result = null;
		if (val is bool)
		{
			result = new SFSPrimitiveDataWrapper<bool>(SFSDataType.BOOL, (bool)val, -1);
		}
		else if (val is byte)
		{
			result = new SFSPrimitiveDataWrapper<byte>(SFSDataType.BYTE, (byte)val, -1);
		}
		else if (val is short)
		{
			result = new SFSPrimitiveDataWrapper<short>(SFSDataType.SHORT, (short)val, -1);
		}
		else if (val is int)
		{
			result = new SFSPrimitiveDataWrapper<int>(SFSDataType.INT, (int)val, -1);
		}
		else if (val is long)
		{
			result = new SFSPrimitiveDataWrapper<long>(SFSDataType.LONG, (long)val, -1);
		}
		else if (val is float)
		{
			result = new SFSPrimitiveDataWrapper<float>(SFSDataType.FLOAT, (float)val, -1);
		}
		else if (val is double)
		{
			result = new SFSPrimitiveDataWrapper<double>(SFSDataType.DOUBLE, (double)val, -1);
		}
		else if (val is string)
		{
			result = new SFSStringDataWrapper(SFSDataType.UTF_STRING, val, -1, -1);
		}
		else if (val is ArrayList)
		{
			result = new SFSRefDataWrapper(SFSDataType.SFS_ARRAY, UnrollArray(val as ArrayList), -1);
		}
		else if (val is SerializableSFSType)
		{
			result = new SFSRefDataWrapper(SFSDataType.SFS_OBJECT, Cs2Sfs(val), -1);
		}
		else if (val is Hashtable)
		{
			result = new SFSRefDataWrapper(SFSDataType.SFS_OBJECT, UnrollDictionary(val as Hashtable), -1);
		}
		return result;
	}

	private ISFSArray UnrollArray(ArrayList arr)
	{
		ISFSArray iSFSArray = SFSArray.NewInstance();
		foreach (object item in arr)
		{
			SFSDataWrapper sFSDataWrapper = WrapField(item);
			if (sFSDataWrapper == null)
			{
				throw new SFSCodecError(string.Concat("Cannot serialize field of array: ", item, " -- unsupported type!"));
			}
			iSFSArray.Add(sFSDataWrapper);
		}
		return iSFSArray;
	}

	private ISFSObject UnrollDictionary(Hashtable dict)
	{
		ISFSObject iSFSObject = SFSObject.NewInstance();
		foreach (string key in dict.Keys)
		{
			SFSDataWrapper sFSDataWrapper = WrapField(dict[key]);
			if (sFSDataWrapper == null)
			{
				throw new SFSCodecError(string.Concat("Cannot serialize field of dictionary with key: ", key, ", ", dict[key], " -- unsupported type!"));
			}
			iSFSObject.Put(key, sFSDataWrapper);
		}
		return iSFSObject;
	}

	public object Sfs2Cs(ISFSObject sfsObj)
	{
		if (!sfsObj.ContainsKey(CLASS_MARKER_KEY) || !sfsObj.ContainsKey(CLASS_FIELDS_KEY))
		{
			throw new SFSCodecError("The SFSObject passed does not represent any serialized class.");
		}
		string utfString = sfsObj.GetUtfString(CLASS_MARKER_KEY);
		Type type = null;
		type = (((object)runningAssembly != null) ? runningAssembly.GetType(utfString) : Type.GetType(utfString));
		if ((object)type == null)
		{
			throw new SFSCodecError("Cannot find type: " + utfString);
		}
		object obj = Activator.CreateInstance(type);
		if (!(obj is SerializableSFSType))
		{
			throw new SFSCodecError(string.Concat("Cannot deserialize object: ", obj, ", type: ", utfString, " -- It doesn't implement the SerializableSFSType interface"));
		}
		ConvertSFSObject(sfsObj.GetSFSArray(CLASS_FIELDS_KEY), obj, type);
		return obj;
	}

	private void ConvertSFSObject(ISFSArray fieldList, object csObj, Type objType)
	{
		for (int i = 0; i < fieldList.Size(); i++)
		{
			ISFSObject sFSObject = fieldList.GetSFSObject(i);
			string utfString = sFSObject.GetUtfString(FIELD_NAME_KEY);
			SFSDataWrapper data = sFSObject.GetData(FIELD_VALUE_KEY);
			object value = UnwrapField(data);
			objType.GetField(utfString, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.SetValue(csObj, value);
		}
	}

	private object UnwrapField(SFSDataWrapper wrapper)
	{
		object result = null;
		int type = wrapper.Type;
		if (type <= 8)
		{
			result = wrapper.Data;
		}
		else
		{
			switch (type)
			{
			case 17:
				result = RebuildArray(wrapper.Data as ISFSArray);
				break;
			case 18:
			{
				ISFSObject iSFSObject = wrapper.Data as ISFSObject;
				result = ((!iSFSObject.ContainsKey(CLASS_MARKER_KEY) || !iSFSObject.ContainsKey(CLASS_FIELDS_KEY)) ? RebuildDict(wrapper.Data as ISFSObject) : Sfs2Cs(iSFSObject));
				break;
			}
			case 19:
				result = wrapper.Data;
				break;
			}
		}
		return result;
	}

	private ArrayList RebuildArray(ISFSArray sfsArr)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < sfsArr.Size(); i++)
		{
			arrayList.Add(UnwrapField(sfsArr.GetWrappedElementAt(i)));
		}
		return arrayList;
	}

	private Hashtable RebuildDict(ISFSObject sfsObj)
	{
		Hashtable hashtable = new Hashtable();
		string[] keys = sfsObj.GetKeys();
		foreach (string key in keys)
		{
			hashtable[key] = UnwrapField(sfsObj.GetData(key));
		}
		return hashtable;
	}
}
