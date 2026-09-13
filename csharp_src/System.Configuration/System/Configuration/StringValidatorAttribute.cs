using Unity;

namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class StringValidatorAttribute : ConfigurationValidatorAttribute
{
	public string InvalidCharacters
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

	public int MaxLength
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

	public int MinLength
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
