using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security;

namespace System.Runtime.Serialization;

[ComVisible(true)]
public sealed class SerializationInfo
{
	private const int defaultSize = 4;

	private const string s_mscorlibAssemblySimpleName = "mscorlib";

	private const string s_mscorlibFileName = "mscorlib.dll";

	internal string[] m_members;

	internal object[] m_data;

	internal Type[] m_types;

	private Dictionary<string, int> m_nameToIndex;

	internal int m_currMember;

	internal IFormatterConverter m_converter;

	private string m_fullTypeName;

	private string m_assemName;

	private Type objectType;

	private bool isFullTypeNameSetExplicit;

	private bool isAssemblyNameSetExplicit;

	private bool requireSameTokenInPartialTrust;

	public string FullTypeName
	{
		get
		{
			return m_fullTypeName;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			m_fullTypeName = value;
			isFullTypeNameSetExplicit = true;
		}
	}

	public string AssemblyName
	{
		get
		{
			return m_assemName;
		}
		[SecuritySafeCritical]
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (requireSameTokenInPartialTrust)
			{
				DemandForUnsafeAssemblyNameAssignments(m_assemName, value);
			}
			m_assemName = value;
			isAssemblyNameSetExplicit = true;
		}
	}

	public int MemberCount => m_currMember;

	public Type ObjectType => objectType;

	public bool IsFullTypeNameSetExplicit => isFullTypeNameSetExplicit;

	public bool IsAssemblyNameSetExplicit => isAssemblyNameSetExplicit;

	internal string[] MemberNames => m_members;

	internal object[] MemberValues => m_data;

	[CLSCompliant(false)]
	public SerializationInfo(Type type, IFormatterConverter converter)
		: this(type, converter, requireSameTokenInPartialTrust: false)
	{
	}

	[CLSCompliant(false)]
	public SerializationInfo(Type type, IFormatterConverter converter, bool requireSameTokenInPartialTrust)
	{
		if ((object)type == null)
		{
			throw new ArgumentNullException("type");
		}
		if (converter == null)
		{
			throw new ArgumentNullException("converter");
		}
		objectType = type;
		m_fullTypeName = type.FullName;
		m_assemName = type.Module.Assembly.FullName;
		m_members = new string[4];
		m_data = new object[4];
		m_types = new Type[4];
		m_nameToIndex = new Dictionary<string, int>();
		m_converter = converter;
		this.requireSameTokenInPartialTrust = requireSameTokenInPartialTrust;
	}

	[SecuritySafeCritical]
	public void SetType(Type type)
	{
		if ((object)type == null)
		{
			throw new ArgumentNullException("type");
		}
		if (requireSameTokenInPartialTrust)
		{
			DemandForUnsafeAssemblyNameAssignments(ObjectType.Assembly.FullName, type.Assembly.FullName);
		}
		if ((object)objectType != type)
		{
			objectType = type;
			m_fullTypeName = type.FullName;
			m_assemName = type.Module.Assembly.FullName;
			isFullTypeNameSetExplicit = false;
			isAssemblyNameSetExplicit = false;
		}
	}

	private static bool Compare(byte[] a, byte[] b)
	{
		if (a == null || b == null || a.Length == 0 || b.Length == 0 || a.Length != b.Length)
		{
			return false;
		}
		for (int i = 0; i < a.Length; i++)
		{
			if (a[i] != b[i])
			{
				return false;
			}
		}
		return true;
	}

	[SecuritySafeCritical]
	internal static void DemandForUnsafeAssemblyNameAssignments(string originalAssemblyName, string newAssemblyName)
	{
		IsAssemblyNameAssignmentSafe(originalAssemblyName, newAssemblyName);
	}

	internal static bool IsAssemblyNameAssignmentSafe(string originalAssemblyName, string newAssemblyName)
	{
		if (originalAssemblyName == newAssemblyName)
		{
			return true;
		}
		AssemblyName assemblyName = new AssemblyName(originalAssemblyName);
		AssemblyName assemblyName2 = new AssemblyName(newAssemblyName);
		if (string.Equals(assemblyName2.Name, "mscorlib", StringComparison.OrdinalIgnoreCase) || string.Equals(assemblyName2.Name, "mscorlib.dll", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		return Compare(assemblyName.GetPublicKeyToken(), assemblyName2.GetPublicKeyToken());
	}

	public SerializationInfoEnumerator GetEnumerator()
	{
		return new SerializationInfoEnumerator(m_members, m_data, m_types, m_currMember);
	}

	private void ExpandArrays()
	{
		int num = m_currMember * 2;
		if (num < m_currMember && int.MaxValue > m_currMember)
		{
			num = int.MaxValue;
		}
		string[] array = new string[num];
		object[] array2 = new object[num];
		Type[] array3 = new Type[num];
		Array.Copy(m_members, array, m_currMember);
		Array.Copy(m_data, array2, m_currMember);
		Array.Copy(m_types, array3, m_currMember);
		m_members = array;
		m_data = array2;
		m_types = array3;
	}

	public void AddValue(string name, object value, Type type)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if ((object)type == null)
		{
			throw new ArgumentNullException("type");
		}
		AddValueInternal(name, value, type);
	}

	public void AddValue(string name, object value)
	{
		if (value == null)
		{
			AddValue(name, value, typeof(object));
		}
		else
		{
			AddValue(name, value, value.GetType());
		}
	}

	public void AddValue(string name, bool value)
	{
		AddValue(name, value, typeof(bool));
	}

	public void AddValue(string name, char value)
	{
		AddValue(name, value, typeof(char));
	}

	[CLSCompliant(false)]
	public void AddValue(string name, sbyte value)
	{
		AddValue(name, value, typeof(sbyte));
	}

	public void AddValue(string name, byte value)
	{
		AddValue(name, value, typeof(byte));
	}

	public void AddValue(string name, short value)
	{
		AddValue(name, value, typeof(short));
	}

	[CLSCompliant(false)]
	public void AddValue(string name, ushort value)
	{
		AddValue(name, value, typeof(ushort));
	}

	public void AddValue(string name, int value)
	{
		AddValue(name, value, typeof(int));
	}

	[CLSCompliant(false)]
	public void AddValue(string name, uint value)
	{
		AddValue(name, value, typeof(uint));
	}

	public void AddValue(string name, long value)
	{
		AddValue(name, value, typeof(long));
	}

	[CLSCompliant(false)]
	public void AddValue(string name, ulong value)
	{
		AddValue(name, value, typeof(ulong));
	}

	public void AddValue(string name, float value)
	{
		AddValue(name, value, typeof(float));
	}

	public void AddValue(string name, double value)
	{
		AddValue(name, value, typeof(double));
	}

	public void AddValue(string name, decimal value)
	{
		AddValue(name, value, typeof(decimal));
	}

	public void AddValue(string name, DateTime value)
	{
		AddValue(name, value, typeof(DateTime));
	}

	internal void AddValueInternal(string name, object value, Type type)
	{
		if (m_nameToIndex.ContainsKey(name))
		{
			throw new SerializationException(Environment.GetResourceString("Cannot add the same member twice to a SerializationInfo object."));
		}
		m_nameToIndex.Add(name, m_currMember);
		if (m_currMember >= m_members.Length)
		{
			ExpandArrays();
		}
		m_members[m_currMember] = name;
		m_data[m_currMember] = value;
		m_types[m_currMember] = type;
		m_currMember++;
	}

	internal void UpdateValue(string name, object value, Type type)
	{
		int num = FindElement(name);
		if (num < 0)
		{
			AddValueInternal(name, value, type);
			return;
		}
		m_data[num] = value;
		m_types[num] = type;
	}

	private int FindElement(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (m_nameToIndex.TryGetValue(name, out var value))
		{
			return value;
		}
		return -1;
	}

	private object GetElement(string name, out Type foundType)
	{
		int num = FindElement(name);
		if (num == -1)
		{
			throw new SerializationException(Environment.GetResourceString("Member '{0}' was not found.", name));
		}
		foundType = m_types[num];
		return m_data[num];
	}

	[ComVisible(true)]
	private object GetElementNoThrow(string name, out Type foundType)
	{
		int num = FindElement(name);
		if (num == -1)
		{
			foundType = null;
			return null;
		}
		foundType = m_types[num];
		return m_data[num];
	}

	[SecuritySafeCritical]
	public object GetValue(string name, Type type)
	{
		if ((object)type == null)
		{
			throw new ArgumentNullException("type");
		}
		RuntimeType runtimeType = type as RuntimeType;
		if (runtimeType == null)
		{
			throw new ArgumentException(Environment.GetResourceString("Type must be a runtime Type object."));
		}
		Type foundType;
		object element = GetElement(name, out foundType);
		if (RemotingServices.IsTransparentProxy(element))
		{
			if (RemotingServices.ProxyCheckCast(RemotingServices.GetRealProxy(element), runtimeType))
			{
				return element;
			}
		}
		else if ((object)foundType == type || type.IsAssignableFrom(foundType) || element == null)
		{
			return element;
		}
		return m_converter.Convert(element, type);
	}

	[SecuritySafeCritical]
	[ComVisible(true)]
	internal object GetValueNoThrow(string name, Type type)
	{
		Type foundType;
		object elementNoThrow = GetElementNoThrow(name, out foundType);
		if (elementNoThrow == null)
		{
			return null;
		}
		if (RemotingServices.IsTransparentProxy(elementNoThrow))
		{
			if (RemotingServices.ProxyCheckCast(RemotingServices.GetRealProxy(elementNoThrow), (RuntimeType)type))
			{
				return elementNoThrow;
			}
		}
		else if ((object)foundType == type || type.IsAssignableFrom(foundType) || elementNoThrow == null)
		{
			return elementNoThrow;
		}
		return m_converter.Convert(elementNoThrow, type);
	}

	public bool GetBoolean(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(bool))
		{
			return (bool)element;
		}
		return m_converter.ToBoolean(element);
	}

	public char GetChar(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(char))
		{
			return (char)element;
		}
		return m_converter.ToChar(element);
	}

	[CLSCompliant(false)]
	public sbyte GetSByte(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(sbyte))
		{
			return (sbyte)element;
		}
		return m_converter.ToSByte(element);
	}

	public byte GetByte(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(byte))
		{
			return (byte)element;
		}
		return m_converter.ToByte(element);
	}

	public short GetInt16(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(short))
		{
			return (short)element;
		}
		return m_converter.ToInt16(element);
	}

	[CLSCompliant(false)]
	public ushort GetUInt16(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(ushort))
		{
			return (ushort)element;
		}
		return m_converter.ToUInt16(element);
	}

	public int GetInt32(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(int))
		{
			return (int)element;
		}
		return m_converter.ToInt32(element);
	}

	[CLSCompliant(false)]
	public uint GetUInt32(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(uint))
		{
			return (uint)element;
		}
		return m_converter.ToUInt32(element);
	}

	public long GetInt64(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(long))
		{
			return (long)element;
		}
		return m_converter.ToInt64(element);
	}

	[CLSCompliant(false)]
	public ulong GetUInt64(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(ulong))
		{
			return (ulong)element;
		}
		return m_converter.ToUInt64(element);
	}

	public float GetSingle(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(float))
		{
			return (float)element;
		}
		return m_converter.ToSingle(element);
	}

	public double GetDouble(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(double))
		{
			return (double)element;
		}
		return m_converter.ToDouble(element);
	}

	public decimal GetDecimal(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(decimal))
		{
			return (decimal)element;
		}
		return m_converter.ToDecimal(element);
	}

	public DateTime GetDateTime(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(DateTime))
		{
			return (DateTime)element;
		}
		return m_converter.ToDateTime(element);
	}

	public string GetString(string name)
	{
		Type foundType;
		object element = GetElement(name, out foundType);
		if ((object)foundType == typeof(string) || element == null)
		{
			return (string)element;
		}
		return m_converter.ToString(element);
	}
}
