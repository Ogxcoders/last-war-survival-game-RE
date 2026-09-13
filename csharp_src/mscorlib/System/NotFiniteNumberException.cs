using System.Runtime.Serialization;

namespace System;

[Serializable]
public class NotFiniteNumberException : ArithmeticException
{
	private double _offendingNumber;

	public double OffendingNumber => _offendingNumber;

	public NotFiniteNumberException()
		: base("Arg_NotFiniteNumberException = Number encountered was not a finite quantity.")
	{
		_offendingNumber = 0.0;
		base.HResult = -2146233048;
	}

	public NotFiniteNumberException(double offendingNumber)
	{
		_offendingNumber = offendingNumber;
		base.HResult = -2146233048;
	}

	public NotFiniteNumberException(string message)
		: base(message)
	{
		_offendingNumber = 0.0;
		base.HResult = -2146233048;
	}

	public NotFiniteNumberException(string message, double offendingNumber)
		: base(message)
	{
		_offendingNumber = offendingNumber;
		base.HResult = -2146233048;
	}

	public NotFiniteNumberException(string message, Exception innerException)
		: base(message, innerException)
	{
		base.HResult = -2146233048;
	}

	public NotFiniteNumberException(string message, double offendingNumber, Exception innerException)
		: base(message, innerException)
	{
		_offendingNumber = offendingNumber;
		base.HResult = -2146233048;
	}

	protected NotFiniteNumberException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		_offendingNumber = info.GetInt32("OffendingNumber");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue("OffendingNumber", _offendingNumber, typeof(int));
	}
}
