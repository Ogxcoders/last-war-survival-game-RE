using Unity;

namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class LongValidatorAttribute : ConfigurationValidatorAttribute
{
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

	public long MaxValue
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(long);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public long MinValue
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(long);
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
