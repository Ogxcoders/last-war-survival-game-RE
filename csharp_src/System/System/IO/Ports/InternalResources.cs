namespace System.IO.Ports;

internal static class InternalResources
{
	internal static void EndOfFile()
	{
		throw new EndOfStreamException(global::SR.GetString("Unable to read beyond the end of the stream."));
	}

	internal static string GetMessage(int errorCode)
	{
		return global::SR.GetString("Unknown Error '{0}'.", errorCode);
	}

	internal static void FileNotOpen()
	{
		throw new ObjectDisposedException(null, global::SR.GetString("The port is closed."));
	}

	internal static void WrongAsyncResult()
	{
		throw new ArgumentException(global::SR.GetString("IAsyncResult object did not come from the corresponding async method on this type."));
	}

	internal static void EndReadCalledTwice()
	{
		throw new ArgumentException(global::SR.GetString("EndRead can only be called once for each asynchronous operation."));
	}

	internal static void EndWriteCalledTwice()
	{
		throw new ArgumentException(global::SR.GetString("EndWrite can only be called once for each asynchronous operation."));
	}

	internal static void WinIOError(int errorCode, string str)
	{
		switch (errorCode)
		{
		case 2:
		case 3:
			if (str.Length == 0)
			{
				throw new IOException(global::SR.GetString("The specified port does not exist."));
			}
			throw new IOException(global::SR.GetString("The port '{0}' does not exist.", str));
		case 5:
			if (str.Length == 0)
			{
				throw new UnauthorizedAccessException(global::SR.GetString("Access to the path is denied."));
			}
			throw new UnauthorizedAccessException(global::SR.GetString("Access to the path '{0}' is denied.", str));
		case 206:
			throw new PathTooLongException(global::SR.GetString("The specified file name or path is too long, or a component of the specified path is too long."));
		case 32:
			if (str.Length == 0)
			{
				throw new IOException(global::SR.GetString("The process cannot access the file because it is being used by another process."));
			}
			throw new IOException(global::SR.GetString("The process cannot access the file '{0}' because it is being used by another process.", str));
		default:
			throw new IOException(GetMessage(errorCode), MakeHRFromErrorCode(errorCode));
		}
	}

	internal static int MakeHRFromErrorCode(int errorCode)
	{
		return -2147024896 | errorCode;
	}
}
