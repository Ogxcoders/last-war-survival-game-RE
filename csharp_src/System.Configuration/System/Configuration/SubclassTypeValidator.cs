using Unity;

namespace System.Configuration;

public sealed class SubclassTypeValidator : ConfigurationValidatorBase
{
	public SubclassTypeValidator(Type baseClass)
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
