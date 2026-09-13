using Unity;

namespace System.Configuration;

public class StringValidator : ConfigurationValidatorBase
{
	public StringValidator(int minLength)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public StringValidator(int minLength, int maxLength)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public StringValidator(int minLength, int maxLength, string invalidCharacters)
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
