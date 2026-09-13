using Unity;

namespace System.Configuration;

public class IntegerValidator : ConfigurationValidatorBase
{
	public IntegerValidator(int minValue, int maxValue)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public IntegerValidator(int minValue, int maxValue, bool rangeIsExclusive)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public IntegerValidator(int minValue, int maxValue, bool rangeIsExclusive, int resolution)
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
