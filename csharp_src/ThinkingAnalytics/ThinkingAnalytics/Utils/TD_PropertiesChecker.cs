using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ThinkingAnalytics.Utils;

public class TD_PropertiesChecker
{
	private static readonly Regex keyPattern = new Regex("^[a-zA-Z][a-zA-Z\\d_#]{0,49}$");

	public static bool IsNumeric(object obj)
	{
		if (!(obj is sbyte) && !(obj is byte) && !(obj is short) && !(obj is ushort) && !(obj is int) && !(obj is uint) && !(obj is long) && !(obj is ulong) && !(obj is double) && !(obj is decimal))
		{
			return obj is float;
		}
		return true;
	}

	public static bool IsString(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		return obj is string;
	}

	public static bool IsDictionary(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (obj.GetType().IsGenericType)
		{
			return obj.GetType().GetGenericTypeDefinition() == typeof(Dictionary<, >);
		}
		return false;
	}

	public static bool IsList(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (!obj.GetType().IsGenericType || !(obj.GetType().GetGenericTypeDefinition() == typeof(List<>)))
		{
			return obj is Array;
		}
		return true;
	}

	public static bool CheckProperties<V>(Dictionary<string, V> properties)
	{
		if (properties == null)
		{
			return true;
		}
		foreach (KeyValuePair<string, V> property in properties)
		{
			if (!CheckString(property.Key))
			{
				return false;
			}
			if (!(property.Value is string) && !(property.Value is DateTime) && !(property.Value is bool) && !IsNumeric(property.Value) && !IsList(property.Value) && !IsDictionary(property.Value))
			{
				TD_Log.w("TA.PropertiesChecker - property values must be one of: string, numberic, Boolean, DateTime, Array, Row");
				return false;
			}
			if (IsString(property.Value))
			{
				return CheckProperties(property.Value as string);
			}
			if (IsNumeric(property.Value))
			{
				return CheckProperties(Convert.ToDouble(property.Value));
			}
			if (IsList(property.Value))
			{
				return CheckProperties(property.Value as List<object>);
			}
			if (IsDictionary(property.Value))
			{
				return CheckProperties(property.Value as Dictionary<string, object>);
			}
		}
		return true;
	}

	public static bool CheckProperties(List<object> properties)
	{
		if (properties == null)
		{
			return true;
		}
		foreach (object property in properties)
		{
			if (!(property is string) && !(property is DateTime) && !(property is bool) && !IsNumeric(property) && !IsDictionary(property))
			{
				TD_Log.w("TA.PropertiesChecker - property values in list must be one of: string, numberic, Boolean, DateTime, Row");
				return false;
			}
			if (IsString(property))
			{
				return CheckProperties(property as string);
			}
			if (IsNumeric(property))
			{
				return CheckProperties(Convert.ToDouble(property));
			}
			if (IsDictionary(property))
			{
				return CheckProperties(property as Dictionary<string, object>);
			}
		}
		return true;
	}

	public static bool CheckProperties(List<string> properties)
	{
		if (properties == null)
		{
			return true;
		}
		foreach (string property in properties)
		{
			if (!CheckString(property))
			{
				return false;
			}
		}
		return true;
	}

	public static bool CheckProperties(string properties)
	{
		if (properties != null && Encoding.UTF8.GetBytes(Convert.ToString(properties)).Length > 2048)
		{
			TD_Log.w("TA.PropertiesChecker - the string is too long: " + properties);
			return false;
		}
		return true;
	}

	public static bool CheckProperties(double properties)
	{
		if (properties > 9999999999999.998 || properties < -9999999999999.998)
		{
			TD_Log.w("TA.PropertiesChecker - number value is invalid: " + properties + ", 数据范围是-9E15至9E15，小数点最多保留3位");
			return false;
		}
		return true;
	}

	public static bool CheckString(string eventName)
	{
		if (string.IsNullOrEmpty(eventName))
		{
			TD_Log.w("TA.PropertiesChecker - the string is null");
			return false;
		}
		if (keyPattern.IsMatch(eventName))
		{
			return true;
		}
		TD_Log.w("TA.PropertiesChecker - the string is invalid for TA: " + eventName + ", 事件名和属性名规则: 必须以字母开头，只能包含：数字，字母（忽略大小写）和下划线“_”，长度最大为50个字符。请注意配置时不要带有空格。");
		return false;
	}

	public static void MergeProperties(Dictionary<string, object> source, Dictionary<string, object> dest)
	{
		if (source == null)
		{
			return;
		}
		foreach (KeyValuePair<string, object> item in source)
		{
			if (dest.ContainsKey(item.Key))
			{
				dest[item.Key] = item.Value;
			}
			else
			{
				dest.Add(item.Key, item.Value);
			}
		}
	}
}
