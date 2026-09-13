using System;

namespace Joker;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ConfigFloatRangeAttribute : ConfigValidatorAttribute
{
	private readonly float _min;

	private readonly float _max;

	public ConfigFloatRangeAttribute(float min, float max)
	{
		_min = min;
		_max = max;
	}

	public override void CheckValid(object value)
	{
		float num = (float)value;
		if (num < _min || num > _max)
		{
			throw new ConfigInvalidException($"{value} must in range [{_min},{_max}]");
		}
	}
}
