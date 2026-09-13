namespace UnityEngine.Yoga;

internal struct YogaValue
{
	private float value;

	private YogaUnit unit;

	public YogaUnit Unit => unit;

	public float Value => value;

	public static YogaValue Point(float value)
	{
		return new YogaValue
		{
			value = value,
			unit = ((!YogaConstants.IsUndefined(value)) ? YogaUnit.Point : YogaUnit.Undefined)
		};
	}

	public bool Equals(YogaValue other)
	{
		return Unit == other.Unit && (Value.Equals(other.Value) || Unit == YogaUnit.Undefined);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		return obj is YogaValue && Equals((YogaValue)obj);
	}

	public override int GetHashCode()
	{
		return (Value.GetHashCode() * 397) ^ (int)Unit;
	}

	public static YogaValue Undefined()
	{
		return new YogaValue
		{
			value = float.NaN,
			unit = YogaUnit.Undefined
		};
	}

	public static YogaValue Auto()
	{
		return new YogaValue
		{
			value = float.NaN,
			unit = YogaUnit.Auto
		};
	}

	public static YogaValue Percent(float value)
	{
		return new YogaValue
		{
			value = value,
			unit = ((!YogaConstants.IsUndefined(value)) ? YogaUnit.Percent : YogaUnit.Undefined)
		};
	}

	public static implicit operator YogaValue(float pointValue)
	{
		return Point(pointValue);
	}

	internal static YogaValue MarshalValue(YogaValue value)
	{
		return value;
	}
}
