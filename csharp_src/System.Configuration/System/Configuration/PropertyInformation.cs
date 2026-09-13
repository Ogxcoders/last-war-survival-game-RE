using System.ComponentModel;
using Unity;

namespace System.Configuration;

public sealed class PropertyInformation
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

	public bool IsKey
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public bool IsLocked
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public bool IsModified
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

	public int LineNumber
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(int);
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

	public string Source
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

	public object Value
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

	public PropertyValueOrigin ValueOrigin
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(PropertyValueOrigin);
		}
	}

	internal PropertyInformation()
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
