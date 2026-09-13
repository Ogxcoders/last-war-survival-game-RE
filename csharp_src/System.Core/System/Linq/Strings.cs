namespace System.Linq;

internal static class Strings
{
	internal static string ArgumentNotIEnumerableGeneric(string message)
	{
		return SR.Format("{0} is not IEnumerable<>", message);
	}

	internal static string ArgumentNotValid(string message)
	{
		return SR.Format("Argument {0} is not valid", message);
	}

	internal static string NoMethodOnType(string name, object type)
	{
		return SR.Format("There is no method '{0}' on type '{1}'", name, type);
	}

	internal static string NoMethodOnTypeMatchingArguments(string name, object type)
	{
		return SR.Format("There is no method '{0}' on type '{1}' that matches the specified arguments", name, type);
	}

	internal static string EnumeratingNullEnumerableExpression()
	{
		return "Cannot enumerate a query created from a null IEnumerable<>";
	}
}
