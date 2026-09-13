using System.Reflection;

namespace System.Runtime.InteropServices;

public class ComAwareEventInfo : EventInfo
{
	[MonoTODO]
	public override EventAttributes Attributes
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[MonoTODO]
	public override Type DeclaringType
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[MonoTODO]
	public override string Name
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[MonoTODO]
	public override Type ReflectedType
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[MonoTODO]
	public ComAwareEventInfo(Type type, string eventName)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	public override void AddEventHandler(object target, Delegate handler)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	public override void RemoveEventHandler(object target, Delegate handler)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	public override MethodInfo GetAddMethod(bool nonPublic)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	public override MethodInfo GetRaiseMethod(bool nonPublic)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	public override MethodInfo GetRemoveMethod(bool nonPublic)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	public override object[] GetCustomAttributes(bool inherit)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw new NotImplementedException();
	}
}
