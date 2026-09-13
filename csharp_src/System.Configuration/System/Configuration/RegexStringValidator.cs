using Unity;

namespace System.Configuration;

public class RegexStringValidator : ConfigurationValidatorBase
{
	public RegexStringValidator(string regex)
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
