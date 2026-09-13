using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml.XmlConfiguration;

namespace System.Xml.Xsl.XPath;

internal class XPathParser<Node>
{
	private XPathScanner scanner;

	private IXPathBuilder<Node> builder;

	private Stack<int> posInfo = new Stack<int>();

	private const int MaxParseRelativePathDepth = 1024;

	private int parseRelativePath;

	private const int MaxParseSubExprDepth = 1024;

	private int parseSubExprDepth;

	private static int[] XPathOperatorPrecedence = new int[16]
	{
		0, 1, 2, 3, 3, 4, 4, 4, 4, 5,
		5, 6, 6, 6, 7, 8
	};

	public Node Parse(XPathScanner scanner, IXPathBuilder<Node> builder, LexKind endLex)
	{
		Node result = default(Node);
		this.scanner = scanner;
		this.builder = builder;
		posInfo.Clear();
		try
		{
			builder.StartBuild();
			result = ParseExpr();
			scanner.CheckToken(endLex);
		}
		catch (XPathCompileException ex)
		{
			if (ex.queryString == null)
			{
				ex.queryString = scanner.Source;
				PopPosInfo(out ex.startChar, out ex.endChar);
			}
			throw;
		}
		finally
		{
			result = builder.EndBuild(result);
		}
		return result;
	}

	internal static bool IsStep(LexKind lexKind)
	{
		if (lexKind != LexKind.Dot && lexKind != LexKind.DotDot && lexKind != LexKind.At && lexKind != LexKind.Axis && lexKind != LexKind.Star)
		{
			return lexKind == LexKind.Name;
		}
		return true;
	}

	private Node ParseLocationPath()
	{
		if (scanner.Kind == LexKind.Slash)
		{
			scanner.NextLex();
			Node val = builder.Axis(XPathAxis.Root, XPathNodeType.All, null, null);
			if (IsStep(scanner.Kind))
			{
				val = builder.JoinStep(val, ParseRelativeLocationPath());
			}
			return val;
		}
		if (scanner.Kind == LexKind.SlashSlash)
		{
			scanner.NextLex();
			return builder.JoinStep(builder.Axis(XPathAxis.Root, XPathNodeType.All, null, null), builder.JoinStep(builder.Axis(XPathAxis.DescendantOrSelf, XPathNodeType.All, null, null), ParseRelativeLocationPath()));
		}
		return ParseRelativeLocationPath();
	}

	private Node ParseRelativeLocationPath()
	{
		if (++parseRelativePath > 1024 && XsltConfigSection.LimitXPathComplexity)
		{
			throw scanner.CreateException("The stylesheet is too complex.");
		}
		Node val = ParseStep();
		if (scanner.Kind == LexKind.Slash)
		{
			scanner.NextLex();
			val = builder.JoinStep(val, ParseRelativeLocationPath());
		}
		else if (scanner.Kind == LexKind.SlashSlash)
		{
			scanner.NextLex();
			val = builder.JoinStep(val, builder.JoinStep(builder.Axis(XPathAxis.DescendantOrSelf, XPathNodeType.All, null, null), ParseRelativeLocationPath()));
		}
		parseRelativePath--;
		return val;
	}

	private Node ParseStep()
	{
		Node val;
		if (LexKind.Dot == scanner.Kind)
		{
			scanner.NextLex();
			val = builder.Axis(XPathAxis.Self, XPathNodeType.All, null, null);
			if (LexKind.LBracket == scanner.Kind)
			{
				throw scanner.CreateException("Abbreviated step '.' cannot be followed by a predicate. Use the full form 'self::node()[predicate]' instead.");
			}
		}
		else if (LexKind.DotDot == scanner.Kind)
		{
			scanner.NextLex();
			val = builder.Axis(XPathAxis.Parent, XPathNodeType.All, null, null);
			if (LexKind.LBracket == scanner.Kind)
			{
				throw scanner.CreateException("Abbreviated step '..' cannot be followed by a predicate. Use the full form 'parent::node()[predicate]' instead.");
			}
		}
		else
		{
			XPathAxis axis;
			switch (scanner.Kind)
			{
			case LexKind.Axis:
				axis = scanner.Axis;
				scanner.NextLex();
				scanner.NextLex();
				break;
			case LexKind.At:
				axis = XPathAxis.Attribute;
				scanner.NextLex();
				break;
			case LexKind.Name:
			case LexKind.Star:
				axis = XPathAxis.Child;
				break;
			default:
				throw scanner.CreateException("Unexpected token '{0}' in the expression.", scanner.RawValue);
			}
			val = ParseNodeTest(axis);
			while (LexKind.LBracket == scanner.Kind)
			{
				val = builder.Predicate(val, ParsePredicate(), IsReverseAxis(axis));
			}
		}
		return val;
	}

	private static bool IsReverseAxis(XPathAxis axis)
	{
		if (axis != XPathAxis.Ancestor && axis != XPathAxis.Preceding && axis != XPathAxis.AncestorOrSelf)
		{
			return axis == XPathAxis.PrecedingSibling;
		}
		return true;
	}

	private Node ParseNodeTest(XPathAxis axis)
	{
		int lexStart = scanner.LexStart;
		InternalParseNodeTest(scanner, axis, out var nodeType, out var nodePrefix, out var nodeName);
		PushPosInfo(lexStart, scanner.PrevLexEnd);
		Node result = builder.Axis(axis, nodeType, nodePrefix, nodeName);
		PopPosInfo();
		return result;
	}

	private static bool IsNodeType(XPathScanner scanner)
	{
		if (scanner.Prefix.Length == 0)
		{
			if (!(scanner.Name == "node") && !(scanner.Name == "text") && !(scanner.Name == "processing-instruction"))
			{
				return scanner.Name == "comment";
			}
			return true;
		}
		return false;
	}

	private static XPathNodeType PrincipalNodeType(XPathAxis axis)
	{
		return axis switch
		{
			XPathAxis.Namespace => XPathNodeType.Namespace, 
			XPathAxis.Attribute => XPathNodeType.Attribute, 
			_ => XPathNodeType.Element, 
		};
	}

	internal static void InternalParseNodeTest(XPathScanner scanner, XPathAxis axis, out XPathNodeType nodeType, out string nodePrefix, out string nodeName)
	{
		switch (scanner.Kind)
		{
		case LexKind.Name:
			if (scanner.CanBeFunction && IsNodeType(scanner))
			{
				nodePrefix = null;
				nodeName = null;
				switch (scanner.Name)
				{
				case "comment":
					nodeType = XPathNodeType.Comment;
					break;
				case "text":
					nodeType = XPathNodeType.Text;
					break;
				case "node":
					nodeType = XPathNodeType.All;
					break;
				default:
					nodeType = XPathNodeType.ProcessingInstruction;
					break;
				}
				scanner.NextLex();
				scanner.PassToken(LexKind.LParens);
				if (nodeType == XPathNodeType.ProcessingInstruction && scanner.Kind != LexKind.RParens)
				{
					scanner.CheckToken(LexKind.String);
					nodePrefix = string.Empty;
					nodeName = scanner.StringValue;
					scanner.NextLex();
				}
				scanner.PassToken(LexKind.RParens);
			}
			else
			{
				nodePrefix = scanner.Prefix;
				nodeName = scanner.Name;
				nodeType = PrincipalNodeType(axis);
				scanner.NextLex();
				if (nodeName == "*")
				{
					nodeName = null;
				}
			}
			break;
		case LexKind.Star:
			nodePrefix = null;
			nodeName = null;
			nodeType = PrincipalNodeType(axis);
			scanner.NextLex();
			break;
		default:
			throw scanner.CreateException("Expected a node test, found '{0}'.", scanner.RawValue);
		}
	}

	private Node ParsePredicate()
	{
		scanner.PassToken(LexKind.LBracket);
		Node result = ParseExpr();
		scanner.PassToken(LexKind.RBracket);
		return result;
	}

	private Node ParseExpr()
	{
		return ParseSubExpr(0);
	}

	private Node ParseSubExpr(int callerPrec)
	{
		if (++parseSubExprDepth > 1024 && XsltConfigSection.LimitXPathComplexity)
		{
			throw scanner.CreateException("The stylesheet is too complex.");
		}
		Node val;
		if (scanner.Kind == LexKind.Minus)
		{
			XPathOperator xPathOperator = XPathOperator.UnaryMinus;
			int callerPrec2 = XPathOperatorPrecedence[(int)xPathOperator];
			scanner.NextLex();
			val = builder.Operator(xPathOperator, ParseSubExpr(callerPrec2), default(Node));
		}
		else
		{
			val = ParseUnionExpr();
		}
		while (true)
		{
			XPathOperator xPathOperator = (XPathOperator)((scanner.Kind <= LexKind.Union) ? scanner.Kind : LexKind.Unknown);
			int num = XPathOperatorPrecedence[(int)xPathOperator];
			if (num <= callerPrec)
			{
				break;
			}
			scanner.NextLex();
			val = builder.Operator(xPathOperator, val, ParseSubExpr(num));
		}
		parseSubExprDepth--;
		return val;
	}

	private Node ParseUnionExpr()
	{
		int lexStart = scanner.LexStart;
		Node val = ParsePathExpr();
		if (scanner.Kind == LexKind.Union)
		{
			PushPosInfo(lexStart, scanner.PrevLexEnd);
			val = builder.Operator(XPathOperator.Union, default(Node), val);
			PopPosInfo();
			while (scanner.Kind == LexKind.Union)
			{
				scanner.NextLex();
				lexStart = scanner.LexStart;
				Node right = ParsePathExpr();
				PushPosInfo(lexStart, scanner.PrevLexEnd);
				val = builder.Operator(XPathOperator.Union, val, right);
				PopPosInfo();
			}
		}
		return val;
	}

	private Node ParsePathExpr()
	{
		if (IsPrimaryExpr())
		{
			int lexStart = scanner.LexStart;
			Node val = ParseFilterExpr();
			int prevLexEnd = scanner.PrevLexEnd;
			if (scanner.Kind == LexKind.Slash)
			{
				scanner.NextLex();
				PushPosInfo(lexStart, prevLexEnd);
				val = builder.JoinStep(val, ParseRelativeLocationPath());
				PopPosInfo();
			}
			else if (scanner.Kind == LexKind.SlashSlash)
			{
				scanner.NextLex();
				PushPosInfo(lexStart, prevLexEnd);
				val = builder.JoinStep(val, builder.JoinStep(builder.Axis(XPathAxis.DescendantOrSelf, XPathNodeType.All, null, null), ParseRelativeLocationPath()));
				PopPosInfo();
			}
			return val;
		}
		return ParseLocationPath();
	}

	private Node ParseFilterExpr()
	{
		int lexStart = scanner.LexStart;
		Node val = ParsePrimaryExpr();
		int prevLexEnd = scanner.PrevLexEnd;
		while (scanner.Kind == LexKind.LBracket)
		{
			PushPosInfo(lexStart, prevLexEnd);
			val = builder.Predicate(val, ParsePredicate(), reverseStep: false);
			PopPosInfo();
		}
		return val;
	}

	private bool IsPrimaryExpr()
	{
		if (scanner.Kind != LexKind.String && scanner.Kind != LexKind.Number && scanner.Kind != LexKind.Dollar && scanner.Kind != LexKind.LParens)
		{
			if (scanner.Kind == LexKind.Name && scanner.CanBeFunction)
			{
				return !IsNodeType(scanner);
			}
			return false;
		}
		return true;
	}

	private Node ParsePrimaryExpr()
	{
		Node result;
		switch (scanner.Kind)
		{
		case LexKind.String:
			result = builder.String(scanner.StringValue);
			scanner.NextLex();
			break;
		case LexKind.Number:
			result = builder.Number(XPathConvert.StringToDouble(scanner.RawValue));
			scanner.NextLex();
			break;
		case LexKind.Dollar:
		{
			int lexStart = scanner.LexStart;
			scanner.NextLex();
			scanner.CheckToken(LexKind.Name);
			PushPosInfo(lexStart, scanner.LexStart + scanner.LexSize);
			result = builder.Variable(scanner.Prefix, scanner.Name);
			PopPosInfo();
			scanner.NextLex();
			break;
		}
		case LexKind.LParens:
			scanner.NextLex();
			result = ParseExpr();
			scanner.PassToken(LexKind.RParens);
			break;
		default:
			result = ParseFunctionCall();
			break;
		}
		return result;
	}

	private Node ParseFunctionCall()
	{
		List<Node> list = new List<Node>();
		string name = scanner.Name;
		string prefix = scanner.Prefix;
		int lexStart = scanner.LexStart;
		scanner.PassToken(LexKind.Name);
		scanner.PassToken(LexKind.LParens);
		if (scanner.Kind != LexKind.RParens)
		{
			while (true)
			{
				list.Add(ParseExpr());
				if (scanner.Kind != LexKind.Comma)
				{
					break;
				}
				scanner.NextLex();
			}
			scanner.CheckToken(LexKind.RParens);
		}
		scanner.NextLex();
		PushPosInfo(lexStart, scanner.PrevLexEnd);
		Node result = builder.Function(prefix, name, list);
		PopPosInfo();
		return result;
	}

	private void PushPosInfo(int startChar, int endChar)
	{
		posInfo.Push(startChar);
		posInfo.Push(endChar);
	}

	private void PopPosInfo()
	{
		posInfo.Pop();
		posInfo.Pop();
	}

	private void PopPosInfo(out int startChar, out int endChar)
	{
		endChar = posInfo.Pop();
		startChar = posInfo.Pop();
	}
}
