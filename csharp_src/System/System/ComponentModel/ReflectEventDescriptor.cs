using System.Collections;
using System.ComponentModel.Design;
using System.Reflection;

namespace System.ComponentModel;

internal sealed class ReflectEventDescriptor : EventDescriptor
{
	private Type _type;

	private readonly Type _componentClass;

	private MethodInfo _addMethod;

	private MethodInfo _removeMethod;

	private EventInfo _realEvent;

	private bool _filledMethods;

	public override Type ComponentType => _componentClass;

	public override Type EventType
	{
		get
		{
			FillMethods();
			return _type;
		}
	}

	public override bool IsMulticast => typeof(MulticastDelegate).IsAssignableFrom(EventType);

	public ReflectEventDescriptor(Type componentClass, string name, Type type, Attribute[] attributes)
		: base(name, attributes)
	{
		if (componentClass == null)
		{
			throw new ArgumentException(global::SR.Format("Null is not a valid value for {0}.", "componentClass"));
		}
		if (type == null || !typeof(Delegate).IsAssignableFrom(type))
		{
			throw new ArgumentException(global::SR.Format("Invalid type for the {0} event.", name));
		}
		_componentClass = componentClass;
		_type = type;
	}

	public ReflectEventDescriptor(Type componentClass, EventInfo eventInfo)
		: base(eventInfo.Name, Array.Empty<Attribute>())
	{
		if (componentClass == null)
		{
			throw new ArgumentException(global::SR.Format("Null is not a valid value for {0}.", "componentClass"));
		}
		_componentClass = componentClass;
		_realEvent = eventInfo;
	}

	public ReflectEventDescriptor(Type componentType, EventDescriptor oldReflectEventDescriptor, Attribute[] attributes)
		: base(oldReflectEventDescriptor, attributes)
	{
		_componentClass = componentType;
		_type = oldReflectEventDescriptor.EventType;
		if (oldReflectEventDescriptor is ReflectEventDescriptor reflectEventDescriptor)
		{
			_addMethod = reflectEventDescriptor._addMethod;
			_removeMethod = reflectEventDescriptor._removeMethod;
			_filledMethods = true;
		}
	}

	public override void AddEventHandler(object component, Delegate value)
	{
		FillMethods();
		if (component == null)
		{
			return;
		}
		ISite site = MemberDescriptor.GetSite(component);
		IComponentChangeService componentChangeService = null;
		if (site != null)
		{
			componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
		}
		if (componentChangeService != null)
		{
			try
			{
				componentChangeService.OnComponentChanging(component, this);
			}
			catch (CheckoutException ex)
			{
				if (ex == CheckoutException.Canceled)
				{
					return;
				}
				throw ex;
			}
			componentChangeService.OnComponentChanging(component, this);
		}
		bool flag = false;
		if (site != null && site.DesignMode)
		{
			if (EventType != value.GetType())
			{
				throw new ArgumentException(global::SR.Format("Invalid event handler for the {0} event.", Name));
			}
			IDictionaryService dictionaryService = (IDictionaryService)site.GetService(typeof(IDictionaryService));
			if (dictionaryService != null)
			{
				Delegate a = (Delegate)dictionaryService.GetValue(this);
				a = Delegate.Combine(a, value);
				dictionaryService.SetValue(this, a);
				flag = true;
			}
		}
		if (!flag)
		{
			MethodInfo addMethod = _addMethod;
			object[] parameters = new Delegate[1] { value };
			addMethod.Invoke(component, parameters);
		}
		componentChangeService?.OnComponentChanged(component, this, null, value);
	}

	protected override void FillAttributes(IList attributes)
	{
		FillMethods();
		if (_realEvent != null)
		{
			FillEventInfoAttribute(_realEvent, attributes);
		}
		else
		{
			FillSingleMethodAttribute(_removeMethod, attributes);
			FillSingleMethodAttribute(_addMethod, attributes);
		}
		base.FillAttributes(attributes);
	}

	private void FillEventInfoAttribute(EventInfo realEventInfo, IList attributes)
	{
		string text = realEventInfo.Name;
		BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
		Type type = realEventInfo.ReflectedType;
		int num = 0;
		while (type != typeof(object))
		{
			num++;
			type = type.BaseType;
		}
		if (num <= 0)
		{
			return;
		}
		type = realEventInfo.ReflectedType;
		Attribute[][] array = new Attribute[num][];
		while (type != typeof(object))
		{
			MemberInfo memberInfo = type.GetEvent(text, bindingAttr);
			if (memberInfo != null)
			{
				array[--num] = ReflectTypeDescriptionProvider.ReflectGetAttributes(memberInfo);
			}
			type = type.BaseType;
		}
		Attribute[][] array2 = array;
		foreach (Attribute[] array3 in array2)
		{
			if (array3 != null)
			{
				Attribute[] array4 = array3;
				foreach (Attribute value in array4)
				{
					attributes.Add(value);
				}
			}
		}
	}

	private void FillMethods()
	{
		if (_filledMethods)
		{
			return;
		}
		if (_realEvent != null)
		{
			_addMethod = _realEvent.GetAddMethod();
			_removeMethod = _realEvent.GetRemoveMethod();
			EventInfo eventInfo = null;
			if (_addMethod == null || _removeMethod == null)
			{
				Type baseType = _componentClass.BaseType;
				while (baseType != null && baseType != typeof(object))
				{
					BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
					EventInfo eventInfo2 = baseType.GetEvent(_realEvent.Name, bindingAttr);
					if (eventInfo2.GetAddMethod() != null)
					{
						eventInfo = eventInfo2;
						break;
					}
				}
			}
			if (eventInfo != null)
			{
				_addMethod = eventInfo.GetAddMethod();
				_removeMethod = eventInfo.GetRemoveMethod();
				_type = eventInfo.EventHandlerType;
			}
			else
			{
				_type = _realEvent.EventHandlerType;
			}
		}
		else
		{
			_realEvent = _componentClass.GetEvent(Name);
			if (_realEvent != null)
			{
				FillMethods();
				return;
			}
			Type[] args = new Type[1] { _type };
			_addMethod = MemberDescriptor.FindMethod(_componentClass, "AddOn" + Name, args, typeof(void));
			_removeMethod = MemberDescriptor.FindMethod(_componentClass, "RemoveOn" + Name, args, typeof(void));
			if (_addMethod == null || _removeMethod == null)
			{
				throw new ArgumentException(global::SR.Format("Accessor methods for the {0} event are missing.", Name));
			}
		}
		_filledMethods = true;
	}

	private void FillSingleMethodAttribute(MethodInfo realMethodInfo, IList attributes)
	{
		string text = realMethodInfo.Name;
		BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
		Type type = realMethodInfo.ReflectedType;
		int num = 0;
		while (type != null && type != typeof(object))
		{
			num++;
			type = type.BaseType;
		}
		if (num <= 0)
		{
			return;
		}
		type = realMethodInfo.ReflectedType;
		Attribute[][] array = new Attribute[num][];
		while (type != null && type != typeof(object))
		{
			MemberInfo method = type.GetMethod(text, bindingAttr);
			if (method != null)
			{
				array[--num] = ReflectTypeDescriptionProvider.ReflectGetAttributes(method);
			}
			type = type.BaseType;
		}
		Attribute[][] array2 = array;
		foreach (Attribute[] array3 in array2)
		{
			if (array3 != null)
			{
				Attribute[] array4 = array3;
				foreach (Attribute value in array4)
				{
					attributes.Add(value);
				}
			}
		}
	}

	public override void RemoveEventHandler(object component, Delegate value)
	{
		FillMethods();
		if (component == null)
		{
			return;
		}
		ISite site = MemberDescriptor.GetSite(component);
		IComponentChangeService componentChangeService = null;
		if (site != null)
		{
			componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
		}
		if (componentChangeService != null)
		{
			try
			{
				componentChangeService.OnComponentChanging(component, this);
			}
			catch (CheckoutException ex)
			{
				if (ex == CheckoutException.Canceled)
				{
					return;
				}
				throw ex;
			}
			componentChangeService.OnComponentChanging(component, this);
		}
		bool flag = false;
		if (site != null && site.DesignMode)
		{
			IDictionaryService dictionaryService = (IDictionaryService)site.GetService(typeof(IDictionaryService));
			if (dictionaryService != null)
			{
				Delegate source = (Delegate)dictionaryService.GetValue(this);
				source = Delegate.Remove(source, value);
				dictionaryService.SetValue(this, source);
				flag = true;
			}
		}
		if (!flag)
		{
			MethodInfo removeMethod = _removeMethod;
			object[] parameters = new Delegate[1] { value };
			removeMethod.Invoke(component, parameters);
		}
		componentChangeService?.OnComponentChanged(component, this, null, value);
	}
}
