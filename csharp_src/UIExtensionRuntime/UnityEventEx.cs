using System;
using System.Reflection;
using UnityEngine.Events;

public static class UnityEventEx
{
	public static void Clear(this UnityEventBase ev)
	{
		Type typeFromHandle = typeof(UnityEventBase);
		Type type = ev.GetType();
		while (type != typeFromHandle && type != null)
		{
			type = type.BaseType;
		}
		if (!(type == typeFromHandle))
		{
			return;
		}
		FieldInfo field = type.GetField("m_Calls", BindingFlags.Instance | BindingFlags.NonPublic);
		if (!(field != null))
		{
			return;
		}
		object value = field.GetValue(ev);
		MethodInfo method = value.GetType().GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public);
		if (method != null)
		{
			method.Invoke(value, null);
		}
		FieldInfo field2 = value.GetType().GetField("m_ExecutingCalls", BindingFlags.Instance | BindingFlags.NonPublic);
		if (field2 != null)
		{
			object value2 = field2.GetValue(value);
			method = value2.GetType().GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public);
			if (method != null)
			{
				method.Invoke(value2, null);
			}
		}
	}
}
