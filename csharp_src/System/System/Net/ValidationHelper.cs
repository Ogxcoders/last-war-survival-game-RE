using System.Globalization;

namespace System.Net;

internal static class ValidationHelper
{
	public static string[] EmptyArray = new string[0];

	internal static readonly char[] InvalidMethodChars = new char[4] { ' ', '\r', '\n', '\t' };

	internal static readonly char[] InvalidParamChars = new char[22]
	{
		'(', ')', '<', '>', '@', ',', ';', ':', '\\', '"',
		'\'', '/', '[', ']', '?', '=', '{', '}', ' ', '\t',
		'\r', '\n'
	};

	public static string[] MakeEmptyArrayNull(string[] stringArray)
	{
		if (stringArray == null || stringArray.Length == 0)
		{
			return null;
		}
		return stringArray;
	}

	public static string MakeStringNull(string stringValue)
	{
		if (stringValue == null || stringValue.Length == 0)
		{
			return null;
		}
		return stringValue;
	}

	public static string ExceptionMessage(Exception exception)
	{
		if (exception == null)
		{
			return string.Empty;
		}
		if (exception.InnerException == null)
		{
			return exception.Message;
		}
		return exception.Message + " (" + ExceptionMessage(exception.InnerException) + ")";
	}

	public static string ToString(object objectValue)
	{
		if (objectValue == null)
		{
			return "(null)";
		}
		if (objectValue is string && ((string)objectValue).Length == 0)
		{
			return "(string.empty)";
		}
		if (objectValue is Exception)
		{
			return ExceptionMessage(objectValue as Exception);
		}
		if (objectValue is IntPtr)
		{
			return "0x" + ((IntPtr)objectValue).ToString("x");
		}
		return objectValue.ToString();
	}

	public static string HashString(object objectValue)
	{
		if (objectValue == null)
		{
			return "(null)";
		}
		if (objectValue is string && ((string)objectValue).Length == 0)
		{
			return "(string.empty)";
		}
		return objectValue.GetHashCode().ToString(NumberFormatInfo.InvariantInfo);
	}

	public static bool IsInvalidHttpString(string stringValue)
	{
		return stringValue.IndexOfAny(InvalidParamChars) != -1;
	}

	public static bool IsBlankString(string stringValue)
	{
		if (stringValue != null)
		{
			return stringValue.Length == 0;
		}
		return true;
	}

	public static bool ValidateTcpPort(int port)
	{
		if (port >= 0)
		{
			return port <= 65535;
		}
		return false;
	}

	public static bool ValidateRange(int actual, int fromAllowed, int toAllowed)
	{
		if (actual >= fromAllowed)
		{
			return actual <= toAllowed;
		}
		return false;
	}

	internal static void ValidateSegment(ArraySegment<byte> segment)
	{
		if (segment.Array == null)
		{
			throw new ArgumentNullException("segment");
		}
		if (segment.Offset < 0 || segment.Count < 0 || segment.Count > segment.Array.Length - segment.Offset)
		{
			throw new ArgumentOutOfRangeException("segment");
		}
	}
}
