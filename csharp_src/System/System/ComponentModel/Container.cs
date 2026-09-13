using System.Security.Permissions;

namespace System.ComponentModel;

[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
public class Container : IContainer, IDisposable
{
	private class Site : ISite, IServiceProvider
	{
		private IComponent component;

		private Container container;

		private string name;

		public IComponent Component => component;

		public IContainer Container => container;

		public bool DesignMode => false;

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (value == null || name == null || !value.Equals(name))
				{
					container.ValidateName(component, value);
					name = value;
				}
			}
		}

		internal Site(IComponent component, Container container, string name)
		{
			this.component = component;
			this.container = container;
			this.name = name;
		}

		public object GetService(Type service)
		{
			if (!(service == typeof(ISite)))
			{
				return container.GetService(service);
			}
			return this;
		}
	}

	private ISite[] sites;

	private int siteCount;

	private ComponentCollection components;

	private ContainerFilterService filter;

	private bool checkedFilter;

	private object syncObj = new object();

	public virtual ComponentCollection Components
	{
		get
		{
			lock (syncObj)
			{
				if (components == null)
				{
					IComponent[] array = new IComponent[siteCount];
					for (int i = 0; i < siteCount; i++)
					{
						array[i] = sites[i].Component;
					}
					components = new ComponentCollection(array);
					if (filter == null && checkedFilter)
					{
						checkedFilter = false;
					}
				}
				if (!checkedFilter)
				{
					filter = GetService(typeof(ContainerFilterService)) as ContainerFilterService;
					checkedFilter = true;
				}
				if (filter != null)
				{
					ComponentCollection componentCollection = filter.FilterComponents(components);
					if (componentCollection != null)
					{
						components = componentCollection;
					}
				}
				return components;
			}
		}
	}

	~Container()
	{
		Dispose(disposing: false);
	}

	public virtual void Add(IComponent component)
	{
		Add(component, null);
	}

	public virtual void Add(IComponent component, string name)
	{
		lock (syncObj)
		{
			if (component == null)
			{
				return;
			}
			ISite site = component.Site;
			if (site != null && site.Container == this)
			{
				return;
			}
			if (sites == null)
			{
				sites = new ISite[4];
			}
			else
			{
				ValidateName(component, name);
				if (sites.Length == siteCount)
				{
					ISite[] destinationArray = new ISite[siteCount * 2];
					Array.Copy(sites, 0, destinationArray, 0, siteCount);
					sites = destinationArray;
				}
			}
			site?.Container.Remove(component);
			ISite site2 = CreateSite(component, name);
			sites[siteCount++] = site2;
			component.Site = site2;
			components = null;
		}
	}

	protected virtual ISite CreateSite(IComponent component, string name)
	{
		return new Site(component, this, name);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposing)
		{
			return;
		}
		lock (syncObj)
		{
			while (siteCount > 0)
			{
				ISite obj = sites[--siteCount];
				obj.Component.Site = null;
				obj.Component.Dispose();
			}
			sites = null;
			components = null;
		}
	}

	protected virtual object GetService(Type service)
	{
		if (!(service == typeof(IContainer)))
		{
			return null;
		}
		return this;
	}

	public virtual void Remove(IComponent component)
	{
		Remove(component, preserveSite: false);
	}

	private void Remove(IComponent component, bool preserveSite)
	{
		lock (syncObj)
		{
			if (component == null)
			{
				return;
			}
			ISite site = component.Site;
			if (site == null || site.Container != this)
			{
				return;
			}
			if (!preserveSite)
			{
				component.Site = null;
			}
			for (int i = 0; i < siteCount; i++)
			{
				if (sites[i] == site)
				{
					siteCount--;
					Array.Copy(sites, i + 1, sites, i, siteCount - i);
					sites[siteCount] = null;
					components = null;
					break;
				}
			}
		}
	}

	protected void RemoveWithoutUnsiting(IComponent component)
	{
		Remove(component, preserveSite: true);
	}

	protected virtual void ValidateName(IComponent component, string name)
	{
		if (component == null)
		{
			throw new ArgumentNullException("component");
		}
		if (name == null)
		{
			return;
		}
		for (int i = 0; i < Math.Min(siteCount, sites.Length); i++)
		{
			ISite site = sites[i];
			if (site != null && site.Name != null && string.Equals(site.Name, name, StringComparison.OrdinalIgnoreCase) && site.Component != component && ((InheritanceAttribute)TypeDescriptor.GetAttributes(site.Component)[typeof(InheritanceAttribute)]).InheritanceLevel != InheritanceLevel.InheritedReadOnly)
			{
				throw new ArgumentException(global::SR.GetString("Duplicate component name '{0}'.  Component names must be unique and case-insensitive.", name));
			}
		}
	}
}
