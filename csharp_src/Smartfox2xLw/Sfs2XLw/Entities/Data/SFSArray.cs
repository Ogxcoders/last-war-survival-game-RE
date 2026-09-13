using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Sfs2XLw.Exceptions;
using Sfs2XLw.Protocol.Serialization;
using Sfs2XLw.Util;

namespace Sfs2XLw.Entities.Data;

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

	public SFSArray()
	{
		dataHolder = new List<SFSDataWrapper>();
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

	public void AddNull()
	{
		AddObject(null, SFSDataType.NULL);
	}

	public void AddBool(bool val)
	{
		Add(new SFSPrimitiveDataWrapper<bool>(SFSDataType.BOOL, val, -1));
	}

	public void AddByte(byte val)
	{
		Add(new SFSPrimitiveDataWrapper<byte>(SFSDataType.BYTE, val, -1));
	}

	public void AddShort(short val)
	{
		Add(new SFSPrimitiveDataWrapper<short>(SFSDataType.SHORT, val, -1));
	}

	public void AddInt(int val)
	{
		Add(new SFSPrimitiveDataWrapper<int>(SFSDataType.INT, val, -1));
	}

	public void AddLong(long val)
	{
		Add(new SFSPrimitiveDataWrapper<long>(SFSDataType.LONG, val, -1));
	}

	public void AddFloat(float val)
	{
		Add(new SFSPrimitiveDataWrapper<float>(SFSDataType.FLOAT, val, -1));
	}

	public void AddDouble(double val)
	{
		Add(new SFSPrimitiveDataWrapper<double>(SFSDataType.DOUBLE, val, -1));
	}

	public void AddUtfString(string val)
	{
		AddObject(val, SFSDataType.UTF_STRING);
	}

	public void AddText(string val)
	{
		AddObject(val, SFSDataType.TEXT);
	}

	public void AddBoolArray(bool[] val)
	{
		AddObject(val, SFSDataType.BOOL_ARRAY);
	}

	public void AddByteArray(ByteArray val)
	{
		AddObject(val, SFSDataType.BYTE_ARRAY);
	}

	public void AddShortArray(short[] val)
	{
		AddObject(val, SFSDataType.SHORT_ARRAY);
	}

	public void AddIntArray(int[] val)
	{
		AddObject(val, SFSDataType.INT_ARRAY);
	}

	public void AddLongArray(long[] val)
	{
		AddObject(val, SFSDataType.LONG_ARRAY);
	}

	public void AddFloatArray(float[] val)
	{
		AddObject(val, SFSDataType.FLOAT_ARRAY);
	}

	public void AddDoubleArray(double[] val)
	{
		AddObject(val, SFSDataType.DOUBLE_ARRAY);
	}

	public void AddUtfStringArray(string[] val)
	{
		AddObject(val, SFSDataType.UTF_STRING_ARRAY);
	}

	public void AddSFSArray(ISFSArray val)
	{
		AddObject(val, SFSDataType.SFS_ARRAY);
	}

	public void AddSFSObject(ISFSObject val)
	{
		AddObject(val, SFSDataType.SFS_OBJECT);
	}

	public void AddClass(object val)
	{
		AddObject(val, SFSDataType.CLASS);
	}

	public void Add(SFSDataWrapper wrappedObject)
	{
		dataHolder.Add(wrappedObject);
	}

	private void AddObject(object val, SFSDataType tp)
	{
		switch (tp)
		{
		case SFSDataType.BYTE:
			Add(new SFSPrimitiveDataWrapper<byte>(tp, (byte)val, -1));
			break;
		case SFSDataType.BOOL:
			Add(new SFSPrimitiveDataWrapper<bool>(tp, (bool)val, -1));
			break;
		case SFSDataType.INT:
			Add(new SFSPrimitiveDataWrapper<int>(tp, (int)val, -1));
			break;
		case SFSDataType.LONG:
			Add(new SFSPrimitiveDataWrapper<long>(tp, (long)val, -1));
			break;
		case SFSDataType.SHORT:
			Add(new SFSPrimitiveDataWrapper<short>(tp, (short)val, -1));
			break;
		case SFSDataType.FLOAT:
			Add(new SFSPrimitiveDataWrapper<float>(tp, (float)val, -1));
			break;
		case SFSDataType.DOUBLE:
			Add(new SFSPrimitiveDataWrapper<double>(tp, (double)val, -1));
			break;
		case SFSDataType.UTF_STRING:
		case SFSDataType.TEXT:
			Add(new SFSStringDataWrapper(tp, val, -1, -1));
			break;
		default:
			Add(new SFSRefDataWrapper(tp, val, -1));
			break;
		}
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
		return (dataHolder[index] as SFSPrimitiveDataWrapper<bool>).Value;
	}

	public virtual byte GetByte(int index)
	{
		return (dataHolder[index] as SFSPrimitiveDataWrapper<byte>).Value;
	}

	public virtual short GetShort(int index)
	{
		return (dataHolder[index] as SFSPrimitiveDataWrapper<short>).Value;
	}

	public virtual int GetInt(int index)
	{
		return (dataHolder[index] as SFSPrimitiveDataWrapper<int>).Value;
	}

	public virtual long GetLong(int index)
	{
		return (dataHolder[index] as SFSPrimitiveDataWrapper<long>).Value;
	}

	public virtual float GetFloat(int index)
	{
		return (dataHolder[index] as SFSPrimitiveDataWrapper<float>).Value;
	}

	public virtual double GetDouble(int index)
	{
		return (dataHolder[index] as SFSPrimitiveDataWrapper<double>).Value;
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
