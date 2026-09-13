namespace System.Linq;

internal static class Error
{
	internal static Exception ArgumentNotIEnumerableGeneric(string message)
	{
		return new ArgumentException(Strings.ArgumentNotIEnumerableGeneric(message));
	}

	internal static Exception ArgumentNotValid(string message)
	{
		return new ArgumentException(Strings.ArgumentNotValid(message));
	}

	internal static Exception NoMethodOnType(string name, object type)
	{
		return new InvalidOperationException(Strings.NoMethodOnType(name, type));
	}

	internal static Exception NoMethodOnTypeMatchingArguments(string name, object type)
	{
		return new InvalidOperationException(Strings.NoMethodOnTypeMatchingArguments(name, type));
	}

	internal static Exception EnumeratingNullEnumerableExpression()
	{
		return new InvalidOperationException(Strings.EnumeratingNullEnumerableExpression());
	}

	internal static Exception ArgumentNull(string s)
	{
		return new ArgumentNullException(s);
	}

	internal static Exception ArgumentOutOfRange(string s)
	{
		return new ArgumentOutOfRangeException(s);
	}

	internal static Exception MoreThanOneElement()
	{
		return new InvalidOperationException("Sequence contains more than one element");
	}

	internal static Exception MoreThanOneMatch()
	{
		return new InvalidOperationException("Sequence contains more than one matching element");
	}

	internal static Exception NoElements()
	{
		return new InvalidOperationException("Sequence contains no elements");
	}

	internal static Exception NoMatch()
	{
		return new InvalidOperationException("Sequence contains no matching element");
	}

	internal static Exception NotSupported()
	{
		return new NotSupportedException();
	}
}
