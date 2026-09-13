using System;

namespace Joker;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ConfigIntRangeAttribute : ConfigValidatorAttribute
{
	private readonly int _min;

	private readonly int _max;

	public ConfigIntRangeAttribute(int min, int max)
	{
		_min = min;
		_max = max;
	}

	public override void CheckValid(object value)
	{
		int num = (int)value;
		if (num < _min || num > _max)
		{
			throw new ConfigInvalidException($"{value} must in range [{_min},{_max}]");
		}
	}
}
