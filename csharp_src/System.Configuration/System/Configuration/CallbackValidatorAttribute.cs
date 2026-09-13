using Unity;

namespace System.Configuration;

[AttributeUsage(AttributeTargets.Property)]
public sealed class CallbackValidatorAttribute : ConfigurationValidatorAttribute
{
	public string CallbackMethodName
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

	public Type Type
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
