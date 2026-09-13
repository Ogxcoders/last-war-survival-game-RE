namespace System.Drawing.Imaging;

public sealed class EncoderParameter : IDisposable
{
	public Encoder Encoder
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
		set
		{
			throw new PlatformNotSupportedException();
		}
	}

	public int NumberOfValues
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public EncoderParameterValueType Type
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public EncoderParameterValueType ValueType
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	public EncoderParameter(Encoder encoder, byte value)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, byte value, bool undefined)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, byte[] value)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, byte[] value, bool undefined)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, short value)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, short[] value)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, int numberValues, EncoderParameterValueType type, IntPtr value)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, int numerator, int denominator)
	{
		throw new PlatformNotSupportedException();
	}

	[Obsolete("This constructor has been deprecated. Use EncoderParameter(Encoder encoder, int numberValues, EncoderParameterValueType type, IntPtr value) instead.  https://go.microsoft.com/fwlink/?linkid=14202")]
	public EncoderParameter(Encoder encoder, int NumberOfValues, int Type, int Value)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, int numerator1, int demoninator1, int numerator2, int demoninator2)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, int[] numerator, int[] denominator)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, int[] numerator1, int[] denominator1, int[] numerator2, int[] denominator2)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, long value)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, long rangebegin, long rangeend)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, long[] value)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, long[] rangebegin, long[] rangeend)
	{
		throw new PlatformNotSupportedException();
	}

	public EncoderParameter(Encoder encoder, string value)
	{
		throw new PlatformNotSupportedException();
	}

	public void Dispose()
	{
		throw new PlatformNotSupportedException();
	}

	~EncoderParameter()
	{
		throw new PlatformNotSupportedException();
	}
}
