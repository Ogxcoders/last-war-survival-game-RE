using System.Collections;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
internal class EventOnTypeBuilderInst : EventInfo
{
	private TypeBuilderInstantiation instantiation;

	private EventBuilder event_builder;

	private EventInfo event_info;

	public override EventAttributes Attributes
	{
		get
		{
			if (event_builder == null)
			{
				return event_info.Attributes;
			}
			return event_builder.attrs;
		}
	}

	public override Type DeclaringType => instantiation;

	public override string Name
	{
		get
		{
			if (event_builder == null)
			{
				return event_info.Name;
			}
			return event_builder.name;
		}
	}

	public override Type ReflectedType => instantiation;

	internal EventOnTypeBuilderInst(TypeBuilderInstantiation instantiation, EventBuilder evt)
	{
		this.instantiation = instantiation;
		event_builder = evt;
	}

	internal EventOnTypeBuilderInst(TypeBuilderInstantiation instantiation, EventInfo evt)
	{
		this.instantiation = instantiation;
		event_info = evt;
	}

	public override MethodInfo GetAddMethod(bool nonPublic)
	{
		MethodInfo methodInfo = ((event_builder != null) ? event_builder.add_method : event_info.GetAddMethod(nonPublic));
		if (methodInfo == null || (!nonPublic && !methodInfo.IsPublic))
		{
			return null;
		}
		return TypeBuilder.GetMethod(instantiation, methodInfo);
	}

	public override MethodInfo GetRaiseMethod(bool nonPublic)
	{
		MethodInfo methodInfo = ((event_builder != null) ? event_builder.raise_method : event_info.GetRaiseMethod(nonPublic));
		if (methodInfo == null || (!nonPublic && !methodInfo.IsPublic))
		{
			return null;
		}
		return TypeBuilder.GetMethod(instantiation, methodInfo);
	}

	public override MethodInfo GetRemoveMethod(bool nonPublic)
	{
		MethodInfo methodInfo = ((event_builder != null) ? event_builder.remove_method : event_info.GetRemoveMethod(nonPublic));
		if (methodInfo == null || (!nonPublic && !methodInfo.IsPublic))
		{
			return null;
		}
		return TypeBuilder.GetMethod(instantiation, methodInfo);
	}

	public override MethodInfo[] GetOtherMethods(bool nonPublic)
	{
		MethodInfo[] array;
		MethodInfo[] other_methods;
		if (event_builder == null)
		{
			array = event_info.GetOtherMethods(nonPublic);
		}
		else
		{
			other_methods = event_builder.other_methods;
			array = other_methods;
		}
		MethodInfo[] array2 = array;
		if (array2 == null)
		{
			return new MethodInfo[0];
		}
		ArrayList arrayList = new ArrayList();
		other_methods = array2;
		foreach (MethodInfo methodInfo in other_methods)
		{
			if (nonPublic || methodInfo.IsPublic)
			{
				arrayList.Add(TypeBuilder.GetMethod(instantiation, methodInfo));
			}
		}
		MethodInfo[] array3 = new MethodInfo[arrayList.Count];
		arrayList.CopyTo(array3, 0);
		return array3;
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw new NotSupportedException();
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		throw new NotSupportedException();
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw new NotSupportedException();
	}
}
