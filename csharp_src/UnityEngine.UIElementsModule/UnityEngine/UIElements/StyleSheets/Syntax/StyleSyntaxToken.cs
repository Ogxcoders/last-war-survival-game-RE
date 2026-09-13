namespace UnityEngine.UIElements.StyleSheets.Syntax;

internal struct StyleSyntaxToken
{
	public StyleSyntaxTokenType type;

	public string text;

	public int number;

	public StyleSyntaxToken(StyleSyntaxTokenType t)
	{
		type = t;
		text = null;
		number = 0;
	}

	public StyleSyntaxToken(StyleSyntaxTokenType type, string text)
	{
		this.type = type;
		this.text = text;
		number = 0;
	}

	public StyleSyntaxToken(StyleSyntaxTokenType type, int number)
	{
		this.type = type;
		text = null;
		this.number = number;
	}
}
