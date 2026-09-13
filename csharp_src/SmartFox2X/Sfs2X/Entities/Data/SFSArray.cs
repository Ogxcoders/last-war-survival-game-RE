using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Sfs2X.Exceptions;
using Sfs2X.Protocol.Serialization;
using Sfs2X.Util;

namespace Sfs2X.Entities.Data;

public class SFSArray : ISFSArray, ICollection, IEnumerable
{
	private ISFSDataSerializer serializer;

	private List<SFSDataWrapper> dataHolder;

	bool ICollection.IsSynchronized => false;

	object ICollection.SyncRoot => this;

	int ICollection.Count => dataHolder.Count;

	public static SFSArray NewFromBinaryData(ByteArray ba)
	{
		return DefaultSFSDataSerializer.Instance.Binary2Array(ba) as SFSArray;
	}

	public static ISFSArray NewFromJsonData(string js)
	{
		return DefaultSFSDataSerializer.Instance.Json2Array(js);
	}

	public static SFSArray NewInstance()
	{
		return new SFSArray();
	}

	public static SFSArray NewInstance(int capacity)
	{
		return new SFSArray(capacity);
	}

	public SFSArray()
	{
		dataHolder = new List<SFSDataWrapper>();
		serializer = DefaultSFSDataSerializer.Instance;
	}

	public SFSArray(int capacity)
	{
		dataHolder = new List<SFSDataWrapper>(capacity);
		serializer = DefaultSFSDataSerializer.Instance;
	}

	public bool Contains(object obj)
	{
		if (obj is ISFSArray || obj is ISFSObject)
		{
			throw new SFSError("ISFSArray and ISFSObject are not supported by this method.");
		}
		for (int i = 0; i < Size(); i++)
		{
			if (object.Equals(GetElementAt(i), obj))
			{
				return true;
			}
		}
		return false;
	}

	public SFSDataWrapper GetWrappedElementAt(int index)
	{
		return dataHolder[index];
	}

	public object GetElementAt(int index)
	{
		object result = null;
		if (dataHolder[index] != null)
		{
			result = dataHolder[index].Data;
		}
		return result;
	}

	public object RemoveElementAt(int index)
	{
		if (index >= dataHolder.Count)
		{
			return null;
		}
		SFSDataWrapper sFSDataWrapper = dataHolder[index];
		dataHolder.RemoveAt(index);
		return sFSDataWrapper.Data;
	}

	public int Size()
	{
		return dataHolder.Count;
	}

	public ByteArray ToBinary()
	{
		return serializer.Array2Binary(this);
	}

	public string ToJson()
	{
		return serializer.Array2Json(flatten());
	}

	private List<object> flatten()
	{
		List<object> list = new List<object>();
		DefaultSFSDataSerializer.Instance.flattenArray(list, this);
		return list;
	}

	public string GetDump()
	{
		return GetDump(format: true);
	}

	public string GetDump(bool format)
	{
		if (!format)
		{
			return Dump();
		}
		return DefaultObjectDumpFormatter.PrettyPrintDump(Dump());
	}

	private string Dump()
	{
		StringBuilder stringBuilder = new StringBuilder(Convert.ToString(DefaultObjectDumpFormatter.TOKEN_INDENT_OPEN));
		for (int i = 0; i < dataHolder.Count; i++)
		{
			SFSDataWrapper sFSDataWrapper = dataHolder[i];
			int type = sFSDataWrapper.Type;
			object obj;
			switch (type)
			{
			case 18:
				obj = (sFSDataWrapper.Data as SFSObject).GetDump(format: false);
				break;
			case 17:
				obj = (sFSDataWrapper.Data as SFSArray).GetDump(format: false);
				break;
			case 0:
				obj = "NULL";
				break;
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
			case 16:
				obj = string.Concat("[", sFSDataWrapper.Data, "]");
				break;
			default:
				obj = sFSDataWrapper.Data.ToString();
				break;
			}
			string value = (string)obj;
			SFSDataType sFSDataType = (SFSDataType)type;
			stringBuilder.Append("(" + sFSDataType.ToString().ToLower() + ") ");
			stringBuilder.Append(value);
			stringBuilder.Append(Convert.ToString(DefaultObjectDumpFormatter.TOKEN_DIVIDER));
		}
		string text = stringBuilder.ToString();
		if (Size() > 0)
		{
			text = text.Substring(0, text.Length - 1);
		}
		return text + Convert.ToString(DefaultObjectDumpFormatter.TOKEN_INDENT_CLOSE);
	}

	public string GetHexDump()
	{
		return DefaultObjectDumpFormatter.HexDump(ToBinary());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddNull()
	{
		dataHolder.Add(new SFSRefDataWrapper(SFSDataType.NULL, null));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddBool(bool val)
	{
		dataHolder.Add(new SFSPrimitiveDataWrapper<bool>(SFSDataType.BOOL, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddByte(byte val)
	{
		dataHolder.Add(new SFSPrimitiveDataWrapper<byte>(SFSDataType.BYTE, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddShort(short val)
	{
		dataHolder.Add(new SFSPrimitiveDataWrapper<short>(SFSDataType.SHORT, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddInt(int val)
	{
		dataHolder.Add(new SFSPrimitiveDataWrapper<int>(SFSDataType.INT, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddLong(long val)
	{
		dataHolder.Add(new SFSPrimitiveDataWrapper<long>(SFSDataType.LONG, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddFloat(float val)
	{
		dataHolder.Add(new SFSPrimitiveDataWrapper<float>(SFSDataType.FLOAT, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddDouble(double val)
	{
		dataHolder.Add(new SFSPrimitiveDataWrapper<double>(SFSDataType.DOUBLE, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddUtfString(string val)
	{
		dataHolder.Add(new SFSRefDataWrapper(SFSDataType.UTF_STRING, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddText(string val)
	{
		dataHolder.Add(new SFSRefDataWrapper(SFSDataType.TEXT, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddBoolArray(bool[] val)
	{
		dataHolder.Add(new SFSRefDataWrapper(SFSDataType.BOOL_ARRAY, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddByteArray(ByteArray val)
	{
		dataHolder.Add(new SFSRefDataWrapper(SFSDataType.BYTE_ARRAY, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddShortArray(short[] val)
	{
		dataHolder.Add(new SFSRefDataWrapper(SFSDataType.SHORT_ARRAY, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddIntArray(int[] val)
	{
		dataHolder.Add(new SFSRefDataWrapper(SFSDataType.INT_ARRAY, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddLongArray(long[] val)
	{
		dataHolder.Add(new SFSRefDataWrapper(SFSDataType.LONG_ARRAY, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddFloatArray(float[] val)
	{
		dataHolder.Add(new SFSRefDataWrapper(SFSDataType.FLOAT_ARRAY, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddDoubleArray(double[] val)
	{
		dataHolder.Add(new SFSRefDataWrapper(SFSDataType.DOUBLE_ARRAY, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddUtfStringArray(string[] val)
	{
		dataHolder.Add(new SFSRefDataWrapper(SFSDataType.UTF_STRING_ARRAY, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddSFSArray(ISFSArray val)
	{
		dataHolder.Add(new SFSRefDataWrapper(SFSDataType.SFS_ARRAY, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddSFSObject(ISFSObject val)
	{
		dataHolder.Add(new SFSRefDataWrapper(SFSDataType.SFS_OBJECT, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddClass(object val)
	{
		dataHolder.Add(new SFSRefDataWrapper(SFSDataType.CLASS, val));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Add(SFSDataWrapper wrappedObject)
	{
		dataHolder.Add(wrappedObject);
	}

	private T GetValue<T>(int index)
	{
		return (T)dataHolder[index].Data;
	}

	public bool IsNull(int index)
	{
		return dataHolder[index].Type == 0;
	}

	public virtual bool GetBool(int index)
	{
		return GetValue<bool>(index);
	}

	public virtual byte GetByte(int index)
	{
		return GetValue<byte>(index);
	}

	public virtual short GetShort(int index)
	{
		return GetValue<short>(index);
	}

	public virtual int GetInt(int index)
	{
		return GetValue<int>(index);
	}

	public virtual long GetLong(int index)
	{
		return GetValue<long>(index);
	}

	public virtual float GetFloat(int index)
	{
		return GetValue<float>(index);
	}

	public virtual double GetDouble(int index)
	{
		return GetValue<double>(index);
	}

	public string GetUtfString(int index)
	{
		return GetValue<string>(index);
	}

	public string GetText(int index)
	{
		return GetValue<string>(index);
	}

	private ICollection GetArray(int index)
	{
		return GetValue<ICollection>(index);
	}

	public virtual bool[] GetBoolArray(int index)
	{
		return (bool[])GetArray(index);
	}

	public virtual ByteArray GetByteArray(int index)
	{
		return GetValue<ByteArray>(index);
	}

	public virtual short[] GetShortArray(int index)
	{
		return (short[])GetArray(index);
	}

	public virtual int[] GetIntArray(int index)
	{
		return (int[])GetArray(index);
	}

	public virtual long[] GetLongArray(int index)
	{
		return (long[])GetArray(index);
	}

	public virtual float[] GetFloatArray(int index)
	{
		return (float[])GetArray(index);
	}

	public virtual double[] GetDoubleArray(int index)
	{
		return (double[])GetArray(index);
	}

	public virtual string[] GetUtfStringArray(int index)
	{
		return (string[])GetArray(index);
	}

	public virtual ISFSArray GetSFSArray(int index)
	{
		return GetValue<ISFSArray>(index);
	}

	public virtual object GetClass(int index)
	{
		return dataHolder[index]?.Data;
	}

	public virtual ISFSObject GetSFSObject(int index)
	{
		return GetValue<ISFSObject>(index);
	}

	void ICollection.CopyTo(Array toArray, int index)
	{
		foreach (SFSDataWrapper item in dataHolder)
		{
			toArray.SetValue(item, index);
			index++;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new SFSArrayEnumerator(dataHolder);
	}
}
