using Unity;

namespace System.Configuration;

public class TimeSpanValidator : ConfigurationValidatorBase
{
	public TimeSpanValidator(TimeSpan minValue, TimeSpan maxValue)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public TimeSpanValidator(TimeSpan minValue, TimeSpan maxValue, bool rangeIsExclusive)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public TimeSpanValidator(TimeSpan minValue, TimeSpan maxValue, bool rangeIsExclusive, long resolutionInSeconds)
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
