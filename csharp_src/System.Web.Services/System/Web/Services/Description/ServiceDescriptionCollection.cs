using System.Xml;

namespace System.Web.Services.Description;

public sealed class ServiceDescriptionCollection : ServiceDescriptionBaseCollection
{
	private ServiceDescriptionImporter importer;

	public ServiceDescription this[int index]
	{
		get
		{
			if (index < 0 || index > base.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (ServiceDescription)base.List[index];
		}
		set
		{
			base.List[index] = value;
		}
	}

	public ServiceDescription this[string ns] => (ServiceDescription)Table[ns];

	public ServiceDescriptionCollection()
		: base(null)
	{
	}

	internal void SetImporter(ServiceDescriptionImporter i)
	{
		importer = i;
	}

	public int Add(ServiceDescription serviceDescription)
	{
		Insert(base.Count, serviceDescription);
		return base.Count - 1;
	}

	public bool Contains(ServiceDescription serviceDescription)
	{
		return base.List.Contains(serviceDescription);
	}

	public void CopyTo(ServiceDescription[] array, int index)
	{
		base.List.CopyTo(array, index);
	}

	public Binding GetBinding(XmlQualifiedName name)
	{
		foreach (ServiceDescription item in base.List)
		{
			if (!(item.TargetNamespace == name.Namespace))
			{
				continue;
			}
			foreach (Binding binding in item.Bindings)
			{
				if (binding.Name == name.Name)
				{
					return binding;
				}
			}
		}
		throw new InvalidOperationException("Binding '" + name?.ToString() + "' not found");
	}

	protected override string GetKey(object value)
	{
		return ((ServiceDescription)value).TargetNamespace;
	}

	public Message GetMessage(XmlQualifiedName name)
	{
		foreach (ServiceDescription item in base.List)
		{
			if (!(item.TargetNamespace == name.Namespace))
			{
				continue;
			}
			foreach (Message message in item.Messages)
			{
				if (message.Name == name.Name)
				{
					return message;
				}
			}
		}
		throw new InvalidOperationException("Message '" + name?.ToString() + "' not found");
	}

	public PortType GetPortType(XmlQualifiedName name)
	{
		foreach (ServiceDescription item in base.List)
		{
			if (!(item.TargetNamespace == name.Namespace))
			{
				continue;
			}
			foreach (PortType portType in item.PortTypes)
			{
				if (portType.Name == name.Name)
				{
					return portType;
				}
			}
		}
		throw new InvalidOperationException("Port type '" + name?.ToString() + "' not found");
	}

	public Service GetService(XmlQualifiedName name)
	{
		foreach (ServiceDescription item in base.List)
		{
			if (!(item.TargetNamespace == name.Namespace))
			{
				continue;
			}
			foreach (Service service in item.Services)
			{
				if (service.Name == name.Name)
				{
					return service;
				}
			}
		}
		throw new InvalidOperationException("Service '" + name?.ToString() + "' not found");
	}

	public int IndexOf(ServiceDescription serviceDescription)
	{
		return base.List.IndexOf(serviceDescription);
	}

	public void Insert(int index, ServiceDescription serviceDescription)
	{
		base.List.Insert(index, serviceDescription);
		OnInsertComplete(index, serviceDescription);
	}

	public void Remove(ServiceDescription serviceDescription)
	{
		base.List.Remove(serviceDescription);
	}

	[System.MonoTODO]
	protected override void OnInsertComplete(int index, object item)
	{
		base.OnInsertComplete(index, item);
	}

	[System.MonoTODO]
	protected override void SetParent(object value, object parent)
	{
	}
}
