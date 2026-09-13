using System.Collections;
using System.Reflection;

namespace System.Diagnostics;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
public sealed class SwitchAttribute : Attribute
{
	private Type type;

	private string name;

	private string description;

	public string SwitchName
	{
		get
		{
			return name;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length == 0)
			{
				throw new ArgumentException(global::SR.GetString("Argument {0} cannot be null or zero-length.", "value"), "value");
			}
			name = value;
		}
	}

	public Type SwitchType
	{
		get
		{
			return type;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			type = value;
		}
	}

	public string SwitchDescription
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}

	public SwitchAttribute(string switchName, Type switchType)
	{
		SwitchName = switchName;
		SwitchType = switchType;
	}

	public static SwitchAttribute[] GetAll(Assembly assembly)
	{
		if (assembly == null)
		{
			throw new ArgumentNullException("assembly");
		}
		ArrayList arrayList = new ArrayList();
		object[] customAttributes = assembly.GetCustomAttributes(typeof(SwitchAttribute), inherit: false);
		arrayList.AddRange(customAttributes);
		Type[] types = assembly.GetTypes();
		for (int i = 0; i < types.Length; i++)
		{
			GetAllRecursive(types[i], arrayList);
		}
		SwitchAttribute[] array = new SwitchAttribute[arrayList.Count];
		arrayList.CopyTo(array, 0);
		return array;
	}

	private static void GetAllRecursive(Type type, ArrayList switchAttribs)
	{
		GetAllRecursive((MemberInfo)type, switchAttribs);
		MemberInfo[] members = type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		for (int i = 0; i < members.Length; i++)
		{
			if (!(members[i] is Type))
			{
				GetAllRecursive(members[i], switchAttribs);
			}
		}
	}

	private static void GetAllRecursive(MemberInfo member, ArrayList switchAttribs)
	{
		object[] customAttributes = member.GetCustomAttributes(typeof(SwitchAttribute), inherit: false);
		switchAttribs.AddRange(customAttributes);
	}
}
