using Unity;

namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class TimeSpanValidatorAttribute : ConfigurationValidatorAttribute
{
	public const string TimeSpanMaxValue = "10675199.02:48:05.4775807";

	public const string TimeSpanMinValue = "-10675199.02:48:05.4775808";

	public bool ExcludeRange
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public TimeSpan MaxValue
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(TimeSpan);
		}
	}

	public string MaxValueString
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

	public TimeSpan MinValue
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(TimeSpan);
		}
	}

	public string MinValueString
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

	public override ConfigurationValidatorBase ValidatorInstance
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}
}
