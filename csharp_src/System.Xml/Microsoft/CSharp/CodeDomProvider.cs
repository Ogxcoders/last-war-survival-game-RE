namespace Microsoft.CSharp;

internal class CodeDomProvider
{
	public string CreateEscapedIdentifier(string name)
	{
		if (IsKeyword(name) || IsPrefixTwoUnderscore(name))
		{
			return "@" + name;
		}
		return name;
	}

	private static bool IsKeyword(string value)
	{
		return false;
	}

	private static bool IsPrefixTwoUnderscore(string value)
	{
		if (value.Length < 3)
		{
			return false;
		}
		if (value[0] == '_' && value[1] == '_')
		{
			return value[2] != '_';
		}
		return false;
	}
}
