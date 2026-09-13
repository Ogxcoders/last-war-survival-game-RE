using System.ComponentModel;
using Unity;

namespace System.Configuration;

public sealed class ConfigurationProperty
{
	public TypeConverter Converter
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public object DefaultValue
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public string Description
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public bool IsAssemblyStringTransformationRequired
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public bool IsDefaultCollection
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public bool IsKey
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public bool IsRequired
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public bool IsTypeStringTransformationRequired
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public bool IsVersionCheckRequired
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
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

	public Type Type
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ConfigurationValidatorBase Validator
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ConfigurationProperty(string name, Type type)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationProperty(string name, Type type, object defaultValue)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationProperty(string name, Type type, object defaultValue, TypeConverter typeConverter, ConfigurationValidatorBase validator, ConfigurationPropertyOptions options)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationProperty(string name, Type type, object defaultValue, TypeConverter typeConverter, ConfigurationValidatorBase validator, ConfigurationPropertyOptions options, string description)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationProperty(string name, Type type, object defaultValue, ConfigurationPropertyOptions options)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
