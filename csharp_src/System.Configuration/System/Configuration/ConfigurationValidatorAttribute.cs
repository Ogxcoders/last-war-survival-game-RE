using Unity;

namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public class ConfigurationValidatorAttribute : Attribute
{
	public virtual ConfigurationValidatorBase ValidatorInstance
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public Type ValidatorType
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	protected ConfigurationValidatorAttribute()
	{
	}

	public ConfigurationValidatorAttribute(Type validator)
	{
	}
}
