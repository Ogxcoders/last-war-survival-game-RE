namespace System.Xml.Xsl.XPath;

internal sealed class XPathScanner
{
	private string xpathExpr;

	private int curIndex;

	private char curChar;

	private LexKind kind;

	private string name;

	private string prefix;

	private string stringValue;

	private bool canBeFunction;

	private int lexStart;

	private int prevLexEnd;

	private LexKind prevKind;

	private XPathAxis axis;

	private XmlCharType xmlCharType = XmlCharType.Instance;

	public string Source => xpathExpr;

	public LexKind Kind => kind;

	public int LexStart => lexStart;

	public int LexSize => curIndex - lexStart;

	public int PrevLexEnd => prevLexEnd;

	public string Name => name;

	public string Prefix => prefix;

	public string RawValue
	{
		get
		{
			if (kind == LexKind.Eof)
			{
				return LexKindToString(kind);
			}
			return xpathExpr.Substring(lexStart, curIndex - lexStart);
		}
	}

	public string StringValue => stringValue;

	public bool CanBeFunction => canBeFunction;

	public XPathAxis Axis => axis;

	public XPathScanner(string xpathExpr)
		: this(xpathExpr, 0)
	{
	}

	public XPathScanner(string xpathExpr, int startFrom)
	{
		this.xpathExpr = xpathExpr;
		kind = LexKind.Unknown;
		SetSourceIndex(startFrom);
		NextLex();
	}

	private void SetSourceIndex(int index)
	{
		curIndex = index - 1;
		NextChar();
	}

	private void NextChar()
	{
		curIndex++;
		if (curIndex < xpathExpr.Length)
		{
			curChar = xpathExpr[curIndex];
		}
		else
		{
			curChar = '\0';
		}
	}

	private void SkipSpace()
	{
		while (xmlCharType.IsWhiteSpace(curChar))
		{
			NextChar();
		}
	}

	private static bool IsAsciiDigit(char ch)
	{
		return (uint)(ch - 48) <= 9u;
	}

	public void NextLex()
	{
		prevLexEnd = curIndex;
		prevKind = kind;
		SkipSpace();
		lexStart = curIndex;
		switch (curChar)
		{
		case '\0':
			kind = LexKind.Eof;
			return;
		case '$':
		case '(':
		case ')':
		case ',':
		case '@':
		case '[':
		case ']':
		case '}':
			kind = (LexKind)curChar;
			NextChar();
			return;
		case '.':
			NextChar();
			if (curChar == '.')
			{
				kind = LexKind.DotDot;
				NextChar();
				return;
			}
			if (IsAsciiDigit(curChar))
			{
				SetSourceIndex(lexStart);
				goto case '0';
			}
			kind = LexKind.Dot;
			return;
		case ':':
			NextChar();
			if (curChar == ':')
			{
				kind = LexKind.ColonColon;
				NextChar();
			}
			else
			{
				kind = LexKind.Unknown;
			}
			return;
		case '*':
			kind = LexKind.Star;
			NextChar();
			CheckOperator(star: true);
			return;
		case '/':
			NextChar();
			if (curChar == '/')
			{
				kind = LexKind.SlashSlash;
				NextChar();
			}
			else
			{
				kind = LexKind.Slash;
			}
			return;
		case '|':
			kind = LexKind.Union;
			NextChar();
			return;
		case '+':
			kind = LexKind.Plus;
			NextChar();
			return;
		case '-':
			kind = LexKind.Minus;
			NextChar();
			return;
		case '=':
			kind = LexKind.Eq;
			NextChar();
			return;
		case '!':
			NextChar();
			if (curChar == '=')
			{
				kind = LexKind.Ne;
				NextChar();
			}
			else
			{
				kind = LexKind.Unknown;
			}
			return;
		case '<':
			NextChar();
			if (curChar == '=')
			{
				kind = LexKind.Le;
				NextChar();
			}
			else
			{
				kind = LexKind.Lt;
			}
			return;
		case '>':
			NextChar();
			if (curChar == '=')
			{
				kind = LexKind.Ge;
				NextChar();
			}
			else
			{
				kind = LexKind.Gt;
			}
			return;
		case '"':
		case '\'':
			kind = LexKind.String;
			ScanString();
			return;
		case '0':
		case '1':
		case '2':
		case '3':
		case '4':
		case '5':
		case '6':
		case '7':
		case '8':
		case '9':
			kind = LexKind.Number;
			ScanNumber();
			return;
		}
		if (xmlCharType.IsStartNCNameSingleChar(curChar))
		{
			kind = LexKind.Name;
			name = ScanNCName();
			prefix = string.Empty;
			canBeFunction = false;
			axis = XPathAxis.Unknown;
			bool flag = false;
			int sourceIndex = curIndex;
			if (curChar == ':')
			{
				NextChar();
				if (curChar == ':')
				{
					NextChar();
					flag = true;
					SetSourceIndex(sourceIndex);
				}
				else if (curChar == '*')
				{
					NextChar();
					prefix = name;
					name = "*";
				}
				else if (xmlCharType.IsStartNCNameSingleChar(curChar))
				{
					prefix = name;
					name = ScanNCName();
					sourceIndex = curIndex;
					SkipSpace();
					canBeFunction = curChar == '(';
					SetSourceIndex(sourceIndex);
				}
				else
				{
					SetSourceIndex(sourceIndex);
				}
			}
			else
			{
				SkipSpace();
				if (curChar == ':')
				{
					NextChar();
					if (curChar == ':')
					{
						NextChar();
						flag = true;
					}
					SetSourceIndex(sourceIndex);
				}
				else
				{
					canBeFunction = curChar == '(';
				}
			}
			if (!CheckOperator(star: false) && flag)
			{
				axis = CheckAxis();
			}
		}
		else
		{
			kind = LexKind.Unknown;
			NextChar();
		}
	}

	private bool CheckOperator(bool star)
	{
		LexKind lexKind;
		if (star)
		{
			lexKind = LexKind.Multiply;
		}
		else
		{
			if (prefix.Length != 0 || name.Length > 3)
			{
				return false;
			}
			switch (name)
			{
			case "or":
				lexKind = LexKind.Or;
				break;
			case "and":
				lexKind = LexKind.And;
				break;
			case "div":
				lexKind = LexKind.Divide;
				break;
			case "mod":
				lexKind = LexKind.Modulo;
				break;
			default:
				return false;
			}
		}
		if (prevKind <= LexKind.Union)
		{
			return false;
		}
		switch (prevKind)
		{
		case LexKind.ColonColon:
		case LexKind.SlashSlash:
		case LexKind.Dollar:
		case LexKind.LParens:
		case LexKind.Comma:
		case LexKind.Slash:
		case LexKind.At:
		case LexKind.LBracket:
			return false;
		default:
			kind = lexKind;
			return true;
		}
	}

	private XPathAxis CheckAxis()
	{
		kind = LexKind.Axis;
		switch (name)
		{
		case "ancestor":
			return XPathAxis.Ancestor;
		case "ancestor-or-self":
			return XPathAxis.AncestorOrSelf;
		case "attribute":
			return XPathAxis.Attribute;
		case "child":
			return XPathAxis.Child;
		case "descendant":
			return XPathAxis.Descendant;
		case "descendant-or-self":
			return XPathAxis.DescendantOrSelf;
		case "following":
			return XPathAxis.Following;
		case "following-sibling":
			return XPathAxis.FollowingSibling;
		case "namespace":
			return XPathAxis.Namespace;
		case "parent":
			return XPathAxis.Parent;
		case "preceding":
			return XPathAxis.Preceding;
		case "preceding-sibling":
			return XPathAxis.PrecedingSibling;
		case "self":
			return XPathAxis.Self;
		default:
			kind = LexKind.Name;
			return XPathAxis.Unknown;
		}
	}

	private void ScanNumber()
	{
		while (IsAsciiDigit(curChar))
		{
			NextChar();
		}
		if (curChar == '.')
		{
			NextChar();
			while (IsAsciiDigit(curChar))
			{
				NextChar();
			}
		}
		if ((curChar & -33) == 69)
		{
			NextChar();
			if (curChar == '+' || curChar == '-')
			{
				NextChar();
			}
			while (IsAsciiDigit(curChar))
			{
				NextChar();
			}
			throw CreateException("Scientific notation is not allowed.");
		}
	}

	private void ScanString()
	{
		int num = curIndex + 1;
		int num2 = xpathExpr.IndexOf(curChar, num);
		if (num2 < 0)
		{
			SetSourceIndex(xpathExpr.Length);
			throw CreateException("String literal was not closed.");
		}
		stringValue = xpathExpr.Substring(num, num2 - num);
		SetSourceIndex(num2 + 1);
	}

	private string ScanNCName()
	{
		int num = curIndex;
		while (xmlCharType.IsNCNameSingleChar(curChar))
		{
			NextChar();
		}
		return xpathExpr.Substring(num, curIndex - num);
	}

	public void PassToken(LexKind t)
	{
		CheckToken(t);
		NextLex();
	}

	public void CheckToken(LexKind t)
	{
		if (kind != t)
		{
			if (t == LexKind.Eof)
			{
				throw CreateException("Expected end of the expression, found '{0}'.", RawValue);
			}
			throw CreateException("Expected token '{0}', found '{1}'.", LexKindToString(t), RawValue);
		}
	}

	private string LexKindToString(LexKind t)
	{
		if (LexKind.Eof < t)
		{
			return new string((char)t, 1);
		}
		return t switch
		{
			LexKind.Name => "<name>", 
			LexKind.String => "<string literal>", 
			LexKind.Eof => "<eof>", 
			_ => string.Empty, 
		};
	}

	public XPathCompileException CreateException(string resId, params string[] args)
	{
		return new XPathCompileException(xpathExpr, lexStart, curIndex, resId, args);
	}
}
