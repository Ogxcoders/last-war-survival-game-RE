namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.All)]
public sealed class AmbientValueAttribute : Attribute
{
	public object Value { get; }

	public AmbientValueAttribute(Type type, string value)
	{
		try
		{
			Value = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);
		}
		catch
		{
		}
	}

	public AmbientValueAttribute(char value)
	{
		Value = value;
	}

	public AmbientValueAttribute(byte value)
	{
		Value = value;
	}

	public AmbientValueAttribute(short value)
	{
		Value = value;
	}

	public AmbientValueAttribute(int value)
	{
		Value = value;
	}

	public AmbientValueAttribute(long value)
	{
		Value = value;
	}

	public AmbientValueAttribute(float value)
	{
		Value = value;
	}

	public AmbientValueAttribute(double value)
	{
		Value = value;
	}

	public AmbientValueAttribute(bool value)
	{
		Value = value;
	}

	public AmbientValueAttribute(string value)
	{
		Value = value;
	}

	public AmbientValueAttribute(object value)
	{
		Value = value;
	}

	public override bool Equals(object obj)
	{
		if (obj == this)
		{
			return true;
		}
		if (obj is AmbientValueAttribute ambientValueAttribute)
		{
			if (Value == null)
			{
				return ambientValueAttribute.Value == null;
			}
			return Value.Equals(ambientValueAttribute.Value);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
