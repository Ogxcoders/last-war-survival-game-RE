using Unity;

namespace System.Configuration;

public class LongValidator : ConfigurationValidatorBase
{
	public LongValidator(long minValue, long maxValue)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public LongValidator(long minValue, long maxValue, bool rangeIsExclusive)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public LongValidator(long minValue, long maxValue, bool rangeIsExclusive, long resolution)
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
