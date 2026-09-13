using Unity;

namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ConfigurationPropertyAttribute : Attribute
{
	public object DefaultValue
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public bool IsDefaultCollection
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public bool IsKey
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public bool IsRequired
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public string Name
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ConfigurationPropertyOptions Options
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(ConfigurationPropertyOptions);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public ConfigurationPropertyAttribute(string name)
	{
	}
}
