using System.Reflection;

namespace System.Xml.Serialization;

internal class XmlTypeMapMember
{
	private string _name;

	private int _index;

	private int _globalIndex = -1;

	private int _specifiedGlobalIndex = -1;

	private TypeData _typeData;

	private MemberInfo _member;

	private MemberInfo _specifiedMember;

	private MethodInfo _shouldSerialize;

	private object _defaultValue = DBNull.Value;

	private string documentation;

	private int _flags;

	private const int OPTIONAL = 1;

	private const int RETURN_VALUE = 2;

	private const int IGNORE = 4;

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public object DefaultValue
	{
		get
		{
			return _defaultValue;
		}
		set
		{
			_defaultValue = value;
		}
	}

	public string Documentation
	{
		get
		{
			return documentation;
		}
		set
		{
			documentation = value;
		}
	}

	public TypeData TypeData
	{
		get
		{
			return _typeData;
		}
		set
		{
			_typeData = value;
		}
	}

	public int Index
	{
		get
		{
			return _index;
		}
		set
		{
			_index = value;
		}
	}

	public int GlobalIndex
	{
		get
		{
			return _globalIndex;
		}
		set
		{
			_globalIndex = value;
		}
	}

	public int SpecifiedGlobalIndex => _specifiedGlobalIndex;

	public bool IsOptionalValueType
	{
		get
		{
			return (_flags & 1) != 0;
		}
		set
		{
			_flags = (value ? (_flags | 1) : (_flags & -2));
		}
	}

	public bool IsReturnValue
	{
		get
		{
			return (_flags & 2) != 0;
		}
		set
		{
			_flags = (value ? (_flags | 2) : (_flags & -3));
		}
	}

	public bool Ignore
	{
		get
		{
			return (_flags & 4) != 0;
		}
		set
		{
			_flags = (value ? (_flags | 4) : (_flags & -5));
		}
	}

	public bool HasSpecified => _specifiedMember != null;

	public bool HasShouldSerialize => _shouldSerialize != null;

	public virtual bool RequiresNullable => false;

	public bool IsReadOnly(Type type)
	{
		if (_member == null)
		{
			InitMember(type);
		}
		if (_member is PropertyInfo)
		{
			return !((PropertyInfo)_member).CanWrite;
		}
		return false;
	}

	public static object GetValue(object ob, string name)
	{
		MemberInfo[] member = ob.GetType().GetMember(name, BindingFlags.Instance | BindingFlags.Public);
		if (member[0] is PropertyInfo)
		{
			return ((PropertyInfo)member[0]).GetValue(ob, null);
		}
		return ((FieldInfo)member[0]).GetValue(ob);
	}

	public object GetValue(object ob)
	{
		if (_member == null)
		{
			InitMember(ob.GetType());
		}
		if (_member is PropertyInfo)
		{
			return ((PropertyInfo)_member).GetValue(ob, null);
		}
		return ((FieldInfo)_member).GetValue(ob);
	}

	public void SetValue(object ob, object value)
	{
		if (_member == null)
		{
			InitMember(ob.GetType());
		}
		_typeData.ConvertForAssignment(ref value);
		if (_member is PropertyInfo)
		{
			((PropertyInfo)_member).SetValue(ob, value, null);
		}
		else
		{
			((FieldInfo)_member).SetValue(ob, value);
		}
	}

	public static void SetValue(object ob, string name, object value)
	{
		MemberInfo[] member = ob.GetType().GetMember(name, BindingFlags.Instance | BindingFlags.Public);
		if (member[0] is PropertyInfo)
		{
			((PropertyInfo)member[0]).SetValue(ob, value, null);
		}
		else
		{
			((FieldInfo)member[0]).SetValue(ob, value);
		}
	}

	private void InitMember(Type type)
	{
		MemberInfo[] member = type.GetMember(_name, BindingFlags.Instance | BindingFlags.Public);
		_member = member[0];
		member = type.GetMember(_name + "Specified", BindingFlags.Instance | BindingFlags.Public);
		if (member.Length != 0)
		{
			_specifiedMember = member[0];
		}
		if (_specifiedMember is PropertyInfo && !((PropertyInfo)_specifiedMember).CanRead)
		{
			_specifiedMember = null;
		}
		MethodInfo method = type.GetMethod("ShouldSerialize" + _name, BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null);
		if (method != null && method.ReturnType == typeof(bool) && !method.IsGenericMethod)
		{
			_shouldSerialize = method;
		}
	}

	public void CheckOptionalValueType(Type type)
	{
		if (_member == null)
		{
			InitMember(type);
		}
		IsOptionalValueType = _specifiedMember != null || _shouldSerialize != null;
	}

	public void CheckOptionalValueType(XmlReflectionMember[] members)
	{
		for (int i = 0; i < members.Length; i++)
		{
			XmlReflectionMember xmlReflectionMember = members[i];
			if (xmlReflectionMember.MemberName == Name + "Specified" && xmlReflectionMember.MemberType == typeof(bool) && xmlReflectionMember.XmlAttributes.XmlIgnore)
			{
				IsOptionalValueType = true;
				_specifiedGlobalIndex = i;
				break;
			}
		}
	}

	public bool GetValueSpecified(object ob)
	{
		if (_specifiedGlobalIndex != -1)
		{
			object[] array = (object[])ob;
			if (_specifiedGlobalIndex < array.Length)
			{
				return (bool)array[_specifiedGlobalIndex];
			}
			return false;
		}
		bool flag = true;
		if (_specifiedMember != null)
		{
			flag = ((!(_specifiedMember is PropertyInfo)) ? ((bool)((FieldInfo)_specifiedMember).GetValue(ob)) : ((bool)((PropertyInfo)_specifiedMember).GetValue(ob, null)));
		}
		if (_shouldSerialize != null)
		{
			flag = flag && (bool)_shouldSerialize.Invoke(ob, new object[0]);
		}
		return flag;
	}

	public bool IsValueSpecifiedSettable()
	{
		if (_specifiedMember is PropertyInfo)
		{
			return ((PropertyInfo)_specifiedMember).CanWrite;
		}
		if (_specifiedMember is FieldInfo)
		{
			return !((FieldInfo)_specifiedMember).IsInitOnly;
		}
		return false;
	}

	public void SetValueSpecified(object ob, bool value)
	{
		if (_specifiedGlobalIndex != -1)
		{
			((object[])ob)[_specifiedGlobalIndex] = value;
		}
		else if (_specifiedMember is PropertyInfo)
		{
			if (((PropertyInfo)_specifiedMember).CanWrite)
			{
				((PropertyInfo)_specifiedMember).SetValue(ob, value, null);
			}
		}
		else if (_specifiedMember is FieldInfo)
		{
			((FieldInfo)_specifiedMember).SetValue(ob, value);
		}
	}
}
