using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Mono;

namespace System.Reflection;

[Serializable]
public abstract class EventInfo : MemberInfo
{
	private delegate void AddEventAdapter(object _this, Delegate dele);

	private delegate void AddEvent<T, D>(T _this, D dele);

	private delegate void StaticAddEvent<D>(D dele);

	private AddEventAdapter cached_add_event;

	public override MemberTypes MemberType => MemberTypes.Event;

	public abstract EventAttributes Attributes { get; }

	public bool IsSpecialName => (Attributes & EventAttributes.SpecialName) != 0;

	public virtual MethodInfo AddMethod => GetAddMethod(nonPublic: true);

	public virtual MethodInfo RemoveMethod => GetRemoveMethod(nonPublic: true);

	public virtual MethodInfo RaiseMethod => GetRaiseMethod(nonPublic: true);

	public virtual bool IsMulticast
	{
		get
		{
			Type eventHandlerType = EventHandlerType;
			return typeof(MulticastDelegate).IsAssignableFrom(eventHandlerType);
		}
	}

	public virtual Type EventHandlerType
	{
		get
		{
			ParameterInfo[] parametersInternal = GetAddMethod(nonPublic: true).GetParametersInternal();
			Type typeFromHandle = typeof(Delegate);
			for (int i = 0; i < parametersInternal.Length; i++)
			{
				Type parameterType = parametersInternal[i].ParameterType;
				if (parameterType.IsSubclassOf(typeFromHandle))
				{
					return parameterType;
				}
			}
			return null;
		}
	}

	public MethodInfo[] GetOtherMethods()
	{
		return GetOtherMethods(nonPublic: false);
	}

	public virtual MethodInfo[] GetOtherMethods(bool nonPublic)
	{
		throw NotImplemented.ByDesign;
	}

	public MethodInfo GetAddMethod()
	{
		return GetAddMethod(nonPublic: false);
	}

	public MethodInfo GetRemoveMethod()
	{
		return GetRemoveMethod(nonPublic: false);
	}

	public MethodInfo GetRaiseMethod()
	{
		return GetRaiseMethod(nonPublic: false);
	}

	public abstract MethodInfo GetAddMethod(bool nonPublic);

	public abstract MethodInfo GetRemoveMethod(bool nonPublic);

	public abstract MethodInfo GetRaiseMethod(bool nonPublic);

	[DebuggerHidden]
	[DebuggerStepThrough]
	public virtual void RemoveEventHandler(object target, Delegate handler)
	{
		MethodInfo removeMethod = GetRemoveMethod(nonPublic: false);
		if (removeMethod == null)
		{
			throw new InvalidOperationException("Cannot remove the event handler since no public remove method exists for the event.");
		}
		if (removeMethod.GetParametersNoCopy()[0].ParameterType == typeof(EventRegistrationToken))
		{
			throw new InvalidOperationException("Adding or removing event handlers dynamically is not supported on WinRT events.");
		}
		removeMethod.Invoke(target, new object[1] { handler });
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static bool operator ==(EventInfo left, EventInfo right)
	{
		if ((object)left == right)
		{
			return true;
		}
		if ((object)left == null || (object)right == null)
		{
			return false;
		}
		return left.Equals(right);
	}

	public static bool operator !=(EventInfo left, EventInfo right)
	{
		return !(left == right);
	}

	[DebuggerHidden]
	[DebuggerStepThrough]
	public virtual void AddEventHandler(object target, Delegate handler)
	{
		if (cached_add_event == null)
		{
			MethodInfo addMethod = GetAddMethod();
			if (addMethod == null)
			{
				throw new InvalidOperationException("Cannot add the event handler since no public add method exists for the event.");
			}
			if (addMethod.DeclaringType.IsValueType)
			{
				if (target == null && !addMethod.IsStatic)
				{
					throw new TargetException("Cannot add a handler to a non static event with a null target");
				}
				addMethod.Invoke(target, new object[1] { handler });
				return;
			}
			cached_add_event = CreateAddEventDelegate(addMethod);
		}
		cached_add_event(target, handler);
	}

	private static void AddEventFrame<T, D>(AddEvent<T, D> addEvent, object obj, object dele)
	{
		if (obj == null)
		{
			throw new TargetException("Cannot add a handler to a non static event with a null target");
		}
		if (!(obj is T))
		{
			throw new TargetException("Object doesn't match target");
		}
		if (!(dele is D))
		{
			throw new ArgumentException($"Object of type {dele.GetType()} cannot be converted to type {typeof(D)}.");
		}
		addEvent((T)obj, (D)dele);
	}

	private static void StaticAddEventAdapterFrame<D>(StaticAddEvent<D> addEvent, object obj, object dele)
	{
		addEvent((D)dele);
	}

	private static AddEventAdapter CreateAddEventDelegate(MethodInfo method)
	{
		Type[] typeArguments;
		Type typeFromHandle;
		string name;
		if (method.IsStatic)
		{
			typeArguments = new Type[1] { method.GetParametersInternal()[0].ParameterType };
			typeFromHandle = typeof(StaticAddEvent<>);
			name = "StaticAddEventAdapterFrame";
		}
		else
		{
			typeArguments = new Type[2]
			{
				method.DeclaringType,
				method.GetParametersInternal()[0].ParameterType
			};
			typeFromHandle = typeof(AddEvent<, >);
			name = "AddEventFrame";
		}
		object obj = Delegate.CreateDelegate(typeFromHandle.MakeGenericType(typeArguments), method, throwOnBindFailure: false);
		if (obj == null)
		{
			throw new MethodAccessException();
		}
		MethodInfo method2 = typeof(EventInfo).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
		method2 = method2.MakeGenericMethod(typeArguments);
		return (AddEventAdapter)Delegate.CreateDelegate(typeof(AddEventAdapter), obj, method2, throwOnBindFailure: true);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern EventInfo internal_from_handle_type(IntPtr event_handle, IntPtr type_handle);

	internal static EventInfo GetEventFromHandle(RuntimeEventHandle handle, RuntimeTypeHandle reflectedType)
	{
		if (handle.Value == IntPtr.Zero)
		{
			throw new ArgumentException("The handle is invalid.");
		}
		EventInfo eventInfo = internal_from_handle_type(handle.Value, reflectedType.Value);
		if (eventInfo == null)
		{
			throw new ArgumentException("The event handle and the type handle are incompatible.");
		}
		return eventInfo;
	}
}
