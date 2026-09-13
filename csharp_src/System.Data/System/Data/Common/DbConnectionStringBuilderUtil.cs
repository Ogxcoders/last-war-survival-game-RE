using System.Data.SqlClient;
using System.Reflection;

namespace System.Data.Common;

internal static class DbConnectionStringBuilderUtil
{
	private const string ApplicationIntentReadWriteString = "ReadWrite";

	private const string ApplicationIntentReadOnlyString = "ReadOnly";

	internal static bool ConvertToBoolean(object value)
	{
		if (value is string text)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(text, "true") || StringComparer.OrdinalIgnoreCase.Equals(text, "yes"))
			{
				return true;
			}
			if (StringComparer.OrdinalIgnoreCase.Equals(text, "false") || StringComparer.OrdinalIgnoreCase.Equals(text, "no"))
			{
				return false;
			}
			string x = text.Trim();
			if (StringComparer.OrdinalIgnoreCase.Equals(x, "true") || StringComparer.OrdinalIgnoreCase.Equals(x, "yes"))
			{
				return true;
			}
			if (StringComparer.OrdinalIgnoreCase.Equals(x, "false") || StringComparer.OrdinalIgnoreCase.Equals(x, "no"))
			{
				return false;
			}
			return bool.Parse(text);
		}
		try
		{
			return Convert.ToBoolean(value);
		}
		catch (InvalidCastException innerException)
		{
			throw ADP.ConvertFailed(value.GetType(), typeof(bool), innerException);
		}
	}

	internal static bool ConvertToIntegratedSecurity(object value)
	{
		if (value is string text)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(text, "sspi") || StringComparer.OrdinalIgnoreCase.Equals(text, "true") || StringComparer.OrdinalIgnoreCase.Equals(text, "yes"))
			{
				return true;
			}
			if (StringComparer.OrdinalIgnoreCase.Equals(text, "false") || StringComparer.OrdinalIgnoreCase.Equals(text, "no"))
			{
				return false;
			}
			string x = text.Trim();
			if (StringComparer.OrdinalIgnoreCase.Equals(x, "sspi") || StringComparer.OrdinalIgnoreCase.Equals(x, "true") || StringComparer.OrdinalIgnoreCase.Equals(x, "yes"))
			{
				return true;
			}
			if (StringComparer.OrdinalIgnoreCase.Equals(x, "false") || StringComparer.OrdinalIgnoreCase.Equals(x, "no"))
			{
				return false;
			}
			return bool.Parse(text);
		}
		try
		{
			return Convert.ToBoolean(value);
		}
		catch (InvalidCastException innerException)
		{
			throw ADP.ConvertFailed(value.GetType(), typeof(bool), innerException);
		}
	}

	internal static int ConvertToInt32(object value)
	{
		try
		{
			return Convert.ToInt32(value);
		}
		catch (InvalidCastException innerException)
		{
			throw ADP.ConvertFailed(value.GetType(), typeof(int), innerException);
		}
	}

	internal static string ConvertToString(object value)
	{
		try
		{
			return Convert.ToString(value);
		}
		catch (InvalidCastException innerException)
		{
			throw ADP.ConvertFailed(value.GetType(), typeof(string), innerException);
		}
	}

	internal static bool TryConvertToApplicationIntent(string value, out ApplicationIntent result)
	{
		if (StringComparer.OrdinalIgnoreCase.Equals(value, "ReadOnly"))
		{
			result = ApplicationIntent.ReadOnly;
			return true;
		}
		if (StringComparer.OrdinalIgnoreCase.Equals(value, "ReadWrite"))
		{
			result = ApplicationIntent.ReadWrite;
			return true;
		}
		result = ApplicationIntent.ReadWrite;
		return false;
	}

	internal static bool IsValidApplicationIntentValue(ApplicationIntent value)
	{
		if (value != ApplicationIntent.ReadOnly)
		{
			return value == ApplicationIntent.ReadWrite;
		}
		return true;
	}

	internal static string ApplicationIntentToString(ApplicationIntent value)
	{
		if (value == ApplicationIntent.ReadOnly)
		{
			return "ReadOnly";
		}
		return "ReadWrite";
	}

	internal static ApplicationIntent ConvertToApplicationIntent(string keyword, object value)
	{
		if (value is string text)
		{
			if (TryConvertToApplicationIntent(text, out var result))
			{
				return result;
			}
			string value2 = text.Trim();
			if (TryConvertToApplicationIntent(value2, out result))
			{
				return result;
			}
			throw ADP.InvalidConnectionOptionValue(keyword);
		}
		ApplicationIntent applicationIntent;
		if (value is ApplicationIntent)
		{
			applicationIntent = (ApplicationIntent)value;
		}
		else
		{
			if (value.GetType().GetTypeInfo().IsEnum)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(ApplicationIntent), null);
			}
			try
			{
				applicationIntent = (ApplicationIntent)Enum.ToObject(typeof(ApplicationIntent), value);
			}
			catch (ArgumentException innerException)
			{
				throw ADP.ConvertFailed(value.GetType(), typeof(ApplicationIntent), innerException);
			}
		}
		if (IsValidApplicationIntentValue(applicationIntent))
		{
			return applicationIntent;
		}
		throw ADP.InvalidEnumerationValue(typeof(ApplicationIntent), (int)applicationIntent);
	}
}
