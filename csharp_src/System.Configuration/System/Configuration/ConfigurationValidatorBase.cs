using Unity;

namespace System.Configuration;

public abstract class ConfigurationValidatorBase
{
	protected ConfigurationValidatorBase()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public virtual bool CanValidate(Type type)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public abstract void Validate(object value);
}
