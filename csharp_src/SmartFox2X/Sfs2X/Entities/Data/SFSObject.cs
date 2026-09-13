using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Sfs2X.Protocol.Serialization;
using Sfs2X.Util;

namespace Sfs2X.Entities.Data;

public class SFSObject : ISFSObject
{
	private Dictionary<string, SFSDataWrapper> dataHolder;

	private ISFSDataSerializer serializer;

	public Dictionary<string, SFSDataWrapper> DataHolder => dataHolder;

	public static SFSObject NewFromBinaryData(ByteArray ba)
	{
		return NewFromIBinaryData(ba);
	}

	public static SFSObject NewFromIBinaryData(IByteArray ba)
	{
		return DefaultSFSDataSerializer.Instance.IBinary2Object(ba) as SFSObject;
	}

	public static ISFSObject NewFromJsonData(string js)
	{
		return DefaultSFSDataSerializer.Instance.Json2Object(js);
	}

	public static SFSObject NewInstance()
	{
		return new SFSObject();
	}

	public static SFSObject NewInstance(int keyNum)
	{
		return new SFSObject(keyNum);
	}

	public SFSObject()
	{
		dataHolder = new Dictionary<string, SFSDataWrapper>();
		serializer = DefaultSFSDataSerializer.Instance;
	}

	public SFSObject(int keyNum)
	{
		dataHolder = new Dictionary<string, SFSDataWrapper>(keyNum);
		serializer = DefaultSFSDataSerializer.Instance;
	}

	private string Dump()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Convert.ToString(DefaultObjectDumpFormatter.TOKEN_INDENT_OPEN));
		foreach (KeyValuePair<string, SFSDataWrapper> item in dataHolder)
		{
			SFSDataWrapper value = item.Value;
			string key = item.Key;
			int type = value.Type;
			SFSDataType sFSDataType = (SFSDataType)type;
			stringBuilder.Append("(" + sFSDataType.ToString().ToLower() + ")");
			stringBuilder.Append(" " + key + ": ");
			switch (type)
			{
			case 18:
				stringBuilder.Append((value.Data as SFSObject).GetDump(format: false));
				break;
			case 17:
				stringBuilder.Append((value.Data as SFSArray).GetDump(format: false));
				break;
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
			case 16:
				stringBuilder.Append(string.Concat("[", value.Data, "]"));
				break;
			default:
				stringBuilder.Append(value.Data);
				break;
			}
			stringBuilder.Append(DefaultObjectDumpFormatter.TOKEN_DIVIDER);
		}
		string text = stringBuilder.ToString();
		if (Size() > 0)
		{
			text = text.Substring(0, text.Length - 1);
		}
		string text2 = text;
		char tOKEN_INDENT_CLOSE = DefaultObjectDumpFormatter.TOKEN_INDENT_CLOSE;
		return text2 + tOKEN_INDENT_CLOSE;
	}

	private T GetValue<T>(string key)
	{
		if (!dataHolder.TryGetValue(key, out var value))
		{
			return default(T);
		}
		return (T)value.Data;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public SFSDataWrapper GetData(string key)
	{
		return dataHolder[key];
	}

	public bool IsNull(string key)
	{
		if (!ContainsKey(key))
		{
			return true;
		}
		SFSDataWrapper sFSDataWrapper = dataHolder[key];
		if (sFSDataWrapper.Type != 0)
		{
			return sFSDataWrapper.Data == null;
		}
		return true;
	}

	public virtual bool GetBool(string key)
	{
		if (!dataHolder.TryGetValue(key, out var value))
		{
			return false;
		}
		return (value as SFSPrimitiveDataWrapper<bool>).Value;
	}

	public virtual byte GetByte(string key)
	{
		if (!dataHolder.TryGetValue(key, out var value))
		{
			return 0;
		}
		return (value as SFSPrimitiveDataWrapper<byte>).Value;
	}

	public virtual short GetShort(string key)
	{
		if (!dataHolder.TryGetValue(key, out var value))
		{
			return 0;
		}
		return (value as SFSPrimitiveDataWrapper<short>).Value;
	}

	public virtual int GetInt(string key)
	{
		if (!dataHolder.TryGetValue(key, out var value))
		{
			return 0;
		}
		return (value as SFSPrimitiveDataWrapper<int>).Value;
	}

	public virtual long GetLong(string key)
	{
		if (!dataHolder.TryGetValue(key, out var value))
		{
			return 0L;
		}
		return (value as SFSPrimitiveDataWrapper<long>).Value;
	}

	public virtual float GetFloat(string key)
	{
		if (!dataHolder.TryGetValue(key, out var value))
		{
			return 0f;
		}
		return (value as SFSPrimitiveDataWrapper<float>).Value;
	}

	public virtual double GetDouble(string key)
	{
		if (!dataHolder.TryGetValue(key, out var value))
		{
			return 0.0;
		}
		return (value as SFSPrimitiveDataWrapper<double>).Value;
	}

	public virtual string GetUtfString(string key)
	{
		return GetValue<string>(key);
	}

	public virtual string GetText(string key)
	{
		return GetValue<string>(key);
	}

	private ICollection GetArray(string key)
	{
		return GetValue<ICollection>(key);
	}

	public virtual bool[] GetBoolArray(string key)
	{
		return (bool[])GetArray(key);
	}

	public virtual ByteArray GetByteArray(string key)
	{
		return GetValue<ByteArray>(key);
	}

	public virtual short[] GetShortArray(string key)
	{
		return (short[])GetArray(key);
	}

	public virtual int[] GetIntArray(string key)
	{
		return (int[])GetArray(key);
	}

	public virtual long[] GetLongArray(string key)
	{
		return (long[])GetArray(key);
	}

	public virtual float[] GetFloatArray(string key)
	{
		return (float[])GetArray(key);
	}

	public virtual double[] GetDoubleArray(string key)
	{
		return (double[])GetArray(key);
	}

	public virtual string[] GetUtfStringArray(string key)
	{
		return (string[])GetArray(key);
	}

	public virtual ISFSArray GetSFSArray(string key)
	{
		return GetValue<ISFSArray>(key);
	}

	public virtual ISFSObject GetSFSObject(string key)
	{
		return GetValue<ISFSObject>(key);
	}

	public virtual object GetClass(string key)
	{
		if (!ContainsKey(key))
		{
			return null;
		}
		return dataHolder[key]?.Data;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutNull(string key)
	{
		dataHolder[key] = new SFSRefDataWrapper(SFSDataType.NULL, null);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutBool(string key, bool val)
	{
		dataHolder[key] = new SFSPrimitiveDataWrapper<bool>(SFSDataType.BOOL, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutByte(string key, byte val)
	{
		dataHolder[key] = new SFSPrimitiveDataWrapper<byte>(SFSDataType.BYTE, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutShort(string key, short val)
	{
		dataHolder[key] = new SFSPrimitiveDataWrapper<short>(SFSDataType.SHORT, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutInt(string key, int val)
	{
		dataHolder[key] = new SFSPrimitiveDataWrapper<int>(SFSDataType.INT, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutLong(string key, long val)
	{
		dataHolder[key] = new SFSPrimitiveDataWrapper<long>(SFSDataType.LONG, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutFloat(string key, float val)
	{
		dataHolder[key] = new SFSPrimitiveDataWrapper<float>(SFSDataType.FLOAT, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutDouble(string key, double val)
	{
		dataHolder[key] = new SFSPrimitiveDataWrapper<double>(SFSDataType.DOUBLE, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutUtfString(string key, string val)
	{
		dataHolder[key] = new SFSRefDataWrapper(SFSDataType.UTF_STRING, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutText(string key, string val)
	{
		dataHolder[key] = new SFSRefDataWrapper(SFSDataType.TEXT, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutBoolArray(string key, bool[] val)
	{
		dataHolder[key] = new SFSRefDataWrapper(SFSDataType.BOOL_ARRAY, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutByteArray(string key, ByteArray val)
	{
		dataHolder[key] = new SFSRefDataWrapper(SFSDataType.BYTE_ARRAY, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutShortArray(string key, short[] val)
	{
		dataHolder[key] = new SFSRefDataWrapper(SFSDataType.SHORT_ARRAY, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutIntArray(string key, int[] val)
	{
		dataHolder[key] = new SFSRefDataWrapper(SFSDataType.INT_ARRAY, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutLongArray(string key, long[] val)
	{
		dataHolder[key] = new SFSRefDataWrapper(SFSDataType.LONG_ARRAY, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutFloatArray(string key, float[] val)
	{
		dataHolder[key] = new SFSRefDataWrapper(SFSDataType.FLOAT_ARRAY, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutDoubleArray(string key, double[] val)
	{
		dataHolder[key] = new SFSRefDataWrapper(SFSDataType.DOUBLE_ARRAY, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutUtfStringArray(string key, string[] val)
	{
		dataHolder[key] = new SFSRefDataWrapper(SFSDataType.UTF_STRING_ARRAY, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutSFSArray(string key, ISFSArray val)
	{
		dataHolder[key] = new SFSRefDataWrapper(SFSDataType.SFS_ARRAY, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void PutSFSObject(string key, ISFSObject val)
	{
		dataHolder[key] = new SFSRefDataWrapper(SFSDataType.SFS_OBJECT, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual void PutClass(string key, object val)
	{
		dataHolder[key] = new SFSRefDataWrapper(SFSDataType.CLASS, val);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Put(string key, SFSDataWrapper val)
	{
		dataHolder[key] = val;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool ContainsKey(string key)
	{
		return dataHolder.ContainsKey(key);
	}

	public string GetDump(bool format)
	{
		if (!format)
		{
			return Dump();
		}
		return DefaultObjectDumpFormatter.PrettyPrintDump(Dump());
	}

	public string GetDump()
	{
		return GetDump(format: true);
	}

	public string GetHexDump()
	{
		return DefaultObjectDumpFormatter.HexDump(ToBinary());
	}

	public string[] GetKeys()
	{
		string[] array = new string[dataHolder.Keys.Count];
		dataHolder.Keys.CopyTo(array, 0);
		return array;
	}

	public void RemoveElement(string key)
	{
		dataHolder.Remove(key);
	}

	public int Size()
	{
		return dataHolder.Count;
	}

	public ByteArray ToBinary()
	{
		return serializer.Object2Binary(this);
	}

	public string ToJson()
	{
		return serializer.Object2Json(flatten());
	}

	private Dictionary<string, object> flatten()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		DefaultSFSDataSerializer.Instance.flattenObject(dictionary, this);
		return dictionary;
	}
}
