using Unity;

namespace System.Configuration;

public sealed class DefaultValidator : ConfigurationValidatorBase
{
	public DefaultValidator()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public override bool CanValidate(Type type)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(bool);
	}

	public override void Validate(object value)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
