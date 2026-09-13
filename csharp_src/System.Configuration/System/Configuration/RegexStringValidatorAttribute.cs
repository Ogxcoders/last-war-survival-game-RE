using Unity;

namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class RegexStringValidatorAttribute : ConfigurationValidatorAttribute
{
	public string Regex
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
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

	public RegexStringValidatorAttribute(string regex)
	{
	}
}
