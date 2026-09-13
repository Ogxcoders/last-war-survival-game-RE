using Unity;

namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class IntegerValidatorAttribute : ConfigurationValidatorAttribute
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

	public int MaxValue
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(int);
		}
		set
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}

	public int MinValue
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(int);
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
