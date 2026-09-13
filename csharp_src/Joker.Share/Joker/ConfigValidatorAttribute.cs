using System;

namespace Joker;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ConfigValidatorAttribute : Attribute
{
	public virtual void CheckValid(object value)
	{
		throw new NotImplementedException();
	}
}
