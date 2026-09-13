using System.Runtime.Serialization;

namespace System.Drawing.Printing;

public class InvalidPrinterException : SystemException
{
	public InvalidPrinterException(PrinterSettings settings)
	{
		throw new PlatformNotSupportedException();
	}

	protected InvalidPrinterException(SerializationInfo info, StreamingContext context)
	{
		throw new PlatformNotSupportedException();
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		throw new PlatformNotSupportedException();
	}
}
