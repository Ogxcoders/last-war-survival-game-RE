using System;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Server;

internal sealed class BinaryOrderedUdtNormalizer : Normalizer
{
	internal readonly FieldInfoEx[] FieldsToNormalize;

	private int _size;

	private byte[] _padBuffer;

	internal readonly object NullInstance;

	private bool _isTopLevelUdt;

	internal bool IsNullable => NullInstance != null;

	internal override int Size
	{
		get
		{
			if (_size != 0)
			{
				return _size;
			}
			if (IsNullable && !_isTopLevelUdt)
			{
				_size = 1;
			}
			FieldInfoEx[] fieldsToNormalize = FieldsToNormalize;
			foreach (FieldInfoEx fieldInfoEx in fieldsToNormalize)
			{
				_size += fieldInfoEx.Normalizer.Size;
			}
			return _size;
		}
	}

	private FieldInfo[] GetFields(Type t)
	{
		return t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
	}

	internal BinaryOrderedUdtNormalizer(Type t, bool isTopLevelUdt)
	{
		_skipNormalize = false;
		if (_skipNormalize)
		{
			_isTopLevelUdt = true;
		}
		_isTopLevelUdt = true;
		FieldInfo[] fields = GetFields(t);
		FieldsToNormalize = new FieldInfoEx[fields.Length];
		int num = 0;
		FieldInfo[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			int offset = Marshal.OffsetOf(fieldInfo.DeclaringType, fieldInfo.Name).ToInt32();
			FieldsToNormalize[num++] = new FieldInfoEx(fieldInfo, offset, Normalizer.GetNormalizer(fieldInfo.FieldType));
		}
		Array.Sort(FieldsToNormalize);
		if (_isTopLevelUdt || !typeof(INullable).IsAssignableFrom(t))
		{
			return;
		}
		PropertyInfo property = t.GetProperty("Null", BindingFlags.Static | BindingFlags.Public);
		if (property == null || property.PropertyType != t)
		{
			FieldInfo field = t.GetField("Null", BindingFlags.Static | BindingFlags.Public);
			if (field == null || field.FieldType != t)
			{
				throw new Exception("could not find Null field/property in nullable type " + t);
			}
			NullInstance = field.GetValue(null);
		}
		else
		{
			NullInstance = property.GetValue(null, null);
		}
		_padBuffer = new byte[Size - 1];
	}

	internal void NormalizeTopObject(object udt, Stream s)
	{
		Normalize(null, udt, s);
	}

	internal object DeNormalizeTopObject(Type t, Stream s)
	{
		return DeNormalizeInternal(t, s);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private object DeNormalizeInternal(Type t, Stream s)
	{
		object obj = null;
		if (!_isTopLevelUdt && typeof(INullable).IsAssignableFrom(t) && (byte)s.ReadByte() == 0)
		{
			obj = NullInstance;
			s.Read(_padBuffer, 0, _padBuffer.Length);
			return obj;
		}
		if (obj == null)
		{
			obj = Activator.CreateInstance(t);
		}
		FieldInfoEx[] fieldsToNormalize = FieldsToNormalize;
		foreach (FieldInfoEx fieldInfoEx in fieldsToNormalize)
		{
			fieldInfoEx.Normalizer.DeNormalize(fieldInfoEx.FieldInfo, obj, s);
		}
		return obj;
	}

	internal override void Normalize(FieldInfo fi, object obj, Stream s)
	{
		object obj2 = ((!(fi == null)) ? GetValue(fi, obj) : obj);
		if (obj2 is INullable nullable && !_isTopLevelUdt)
		{
			if (nullable.IsNull)
			{
				s.WriteByte(0);
				s.Write(_padBuffer, 0, _padBuffer.Length);
				return;
			}
			s.WriteByte(1);
		}
		FieldInfoEx[] fieldsToNormalize = FieldsToNormalize;
		foreach (FieldInfoEx fieldInfoEx in fieldsToNormalize)
		{
			fieldInfoEx.Normalizer.Normalize(fieldInfoEx.FieldInfo, obj2, s);
		}
	}

	internal override void DeNormalize(FieldInfo fi, object recvr, Stream s)
	{
		SetValue(fi, recvr, DeNormalizeInternal(fi.FieldType, s));
	}
}
