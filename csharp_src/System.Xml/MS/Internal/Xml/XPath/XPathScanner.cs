using System;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath;

internal sealed class XPathScanner
{
	public enum LexKind
	{
		Comma = 44,
		Slash = 47,
		At = 64,
		Dot = 46,
		LParens = 40,
		RParens = 41,
		LBracket = 91,
		RBracket = 93,
		Star = 42,
		Plus = 43,
		Minus = 45,
		Eq = 61,
		Lt = 60,
		Gt = 62,
		Bang = 33,
		Dollar = 36,
		Apos = 39,
		Quote = 34,
		Union = 124,
		Ne = 78,
		Le = 76,
		Ge = 71,
		And = 65,
		Or = 79,
		DotDot = 68,
		SlashSlash = 83,
		Name = 110,
		String = 115,
		Number = 100,
		Axe = 97,
		Eof = 69
	}

	private string _xpathExpr;

	private int _xpathExprIndex;

	private LexKind _kind;

	private char _currentChar;

	private string _name;

	private string _prefix;

	private string _stringValue;

	private double _numberValue = double.NaN;

	private bool _canBeFunction;

	private XmlCharType _xmlCharType = XmlCharType.Instance;

	public string SourceText => _xpathExpr;

	private char CurrentChar => _currentChar;

	public LexKind Kind => _kind;

	public string Name => _name;

	public string Prefix => _prefix;

	public string StringValue => _stringValue;

	public double NumberValue => _numberValue;

	public bool CanBeFunction => _canBeFunction;

	public XPathScanner(string xpathExpr)
	{
		if (xpathExpr == null)
		{
			throw XPathException.Create("'{0}' is an invalid expression.", string.Empty);
		}
		_xpathExpr = xpathExpr;
		NextChar();
		NextLex();
	}

	private bool NextChar()
	{
		if (_xpathExprIndex < _xpathExpr.Length)
		{
			_currentChar = _xpathExpr[_xpathExprIndex++];
			return true;
		}
		_currentChar = '\0';
		return false;
	}

	private void SkipSpace()
	{
		while (_xmlCharType.IsWhiteSpace(CurrentChar) && NextChar())
		{
		}
	}

	public bool NextLex()
	{
		SkipSpace();
		switch (CurrentChar)
		{
		case '\0':
			_kind = LexKind.Eof;
			return false;
		case '#':
		case '$':
		case '(':
		case ')':
		case '*':
		case '+':
		case ',':
		case '-':
		case '=':
		case '@':
		case '[':
		case ']':
		case '|':
			_kind = (LexKind)Convert.ToInt32(CurrentChar, CultureInfo.InvariantCulture);
			NextChar();
			break;
		case '<':
			_kind = LexKind.Lt;
			NextChar();
			if (CurrentChar == '=')
			{
				_kind = LexKind.Le;
				NextChar();
			}
			break;
		case '>':
			_kind = LexKind.Gt;
			NextChar();
			if (CurrentChar == '=')
			{
				_kind = LexKind.Ge;
				NextChar();
			}
			break;
		case '!':
			_kind = LexKind.Bang;
			NextChar();
			if (CurrentChar == '=')
			{
				_kind = LexKind.Ne;
				NextChar();
			}
			break;
		case '.':
			_kind = LexKind.Dot;
			NextChar();
			if (CurrentChar == '.')
			{
				_kind = LexKind.DotDot;
				NextChar();
			}
			else if (XmlCharType.IsDigit(CurrentChar))
			{
				_kind = LexKind.Number;
				_numberValue = ScanFraction();
			}
			break;
		case '/':
			_kind = LexKind.Slash;
			NextChar();
			if (CurrentChar == '/')
			{
				_kind = LexKind.SlashSlash;
				NextChar();
			}
			break;
		case '"':
		case '\'':
			_kind = LexKind.String;
			_stringValue = ScanString();
			break;
		default:
			if (XmlCharType.IsDigit(CurrentChar))
			{
				_kind = LexKind.Number;
				_numberValue = ScanNumber();
				break;
			}
			if (_xmlCharType.IsStartNCNameSingleChar(CurrentChar))
			{
				_kind = LexKind.Name;
				_name = ScanName();
				_prefix = string.Empty;
				if (CurrentChar == ':')
				{
					NextChar();
					if (CurrentChar == ':')
					{
						NextChar();
						_kind = LexKind.Axe;
					}
					else
					{
						_prefix = _name;
						if (CurrentChar == '*')
						{
							NextChar();
							_name = "*";
						}
						else
						{
							if (!_xmlCharType.IsStartNCNameSingleChar(CurrentChar))
							{
								throw XPathException.Create("'{0}' has an invalid qualified name.", SourceText);
							}
							_name = ScanName();
						}
					}
				}
				else
				{
					SkipSpace();
					if (CurrentChar == ':')
					{
						NextChar();
						if (CurrentChar != ':')
						{
							throw XPathException.Create("'{0}' has an invalid qualified name.", SourceText);
						}
						NextChar();
						_kind = LexKind.Axe;
					}
				}
				SkipSpace();
				_canBeFunction = CurrentChar == '(';
				break;
			}
			throw XPathException.Create("'{0}' has an invalid token.", SourceText);
		}
		return true;
	}

	private double ScanNumber()
	{
		int startIndex = _xpathExprIndex - 1;
		int num = 0;
		while (XmlCharType.IsDigit(CurrentChar))
		{
			NextChar();
			num++;
		}
		if (CurrentChar == '.')
		{
			NextChar();
			num++;
			while (XmlCharType.IsDigit(CurrentChar))
			{
				NextChar();
				num++;
			}
		}
		return XmlConvert.ToXPathDouble(_xpathExpr.Substring(startIndex, num));
	}

	private double ScanFraction()
	{
		int startIndex = _xpathExprIndex - 2;
		int num = 1;
		while (XmlCharType.IsDigit(CurrentChar))
		{
			NextChar();
			num++;
		}
		return XmlConvert.ToXPathDouble(_xpathExpr.Substring(startIndex, num));
	}

	private string ScanString()
	{
		char currentChar = CurrentChar;
		NextChar();
		int startIndex = _xpathExprIndex - 1;
		int num = 0;
		while (CurrentChar != currentChar)
		{
			if (!NextChar())
			{
				throw XPathException.Create("This is an unclosed string.");
			}
			num++;
		}
		NextChar();
		return _xpathExpr.Substring(startIndex, num);
	}

	private string ScanName()
	{
		int startIndex = _xpathExprIndex - 1;
		int num = 0;
		while (_xmlCharType.IsNCNameSingleChar(CurrentChar))
		{
			NextChar();
			num++;
		}
		return _xpathExpr.Substring(startIndex, num);
	}
}
