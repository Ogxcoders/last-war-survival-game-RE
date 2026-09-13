using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements.StyleSheets.Syntax;

internal class StyleSyntaxParser
{
	private List<Expression> m_ProcessExpressionList = new List<Expression>();

	private Stack<Expression> m_ExpressionStack = new Stack<Expression>();

	private Stack<ExpressionCombinator> m_CombinatorStack = new Stack<ExpressionCombinator>();

	private Dictionary<string, Expression> m_ParsedExpressionCache = new Dictionary<string, Expression>();

	public Expression Parse(string syntax)
	{
		if (string.IsNullOrEmpty(syntax))
		{
			return null;
		}
		Expression value = null;
		if (!m_ParsedExpressionCache.TryGetValue(syntax, out value))
		{
			StyleSyntaxTokenizer styleSyntaxTokenizer = new StyleSyntaxTokenizer();
			styleSyntaxTokenizer.Tokenize(syntax);
			try
			{
				value = ParseExpression(styleSyntaxTokenizer);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			m_ParsedExpressionCache[syntax] = value;
		}
		return value;
	}

	private Expression ParseExpression(StyleSyntaxTokenizer tokenizer)
	{
		StyleSyntaxToken current = tokenizer.current;
		while (!IsExpressionEnd(current))
		{
			Expression expression = null;
			if (current.type == StyleSyntaxTokenType.String || current.type == StyleSyntaxTokenType.LessThan)
			{
				expression = ParseTerm(tokenizer);
			}
			else
			{
				if (current.type != StyleSyntaxTokenType.OpenBracket)
				{
					throw new Exception($"Unexpected token '{current.type}' in expression");
				}
				expression = ParseGroup(tokenizer);
			}
			m_ExpressionStack.Push(expression);
			ExpressionCombinator expressionCombinator = ParseCombinatorType(tokenizer);
			if (expressionCombinator != ExpressionCombinator.None)
			{
				if (m_CombinatorStack.Count > 0)
				{
					ExpressionCombinator expressionCombinator2 = m_CombinatorStack.Peek();
					int num = (int)expressionCombinator2;
					int num2 = (int)expressionCombinator;
					while (num > num2 && expressionCombinator2 != ExpressionCombinator.Group)
					{
						ProcessCombinatorStack();
						expressionCombinator2 = ((m_CombinatorStack.Count > 0) ? m_CombinatorStack.Peek() : ExpressionCombinator.None);
						num = (int)expressionCombinator2;
					}
				}
				m_CombinatorStack.Push(expressionCombinator);
			}
			current = tokenizer.current;
		}
		while (m_CombinatorStack.Count > 0)
		{
			ExpressionCombinator expressionCombinator3 = m_CombinatorStack.Peek();
			if (expressionCombinator3 == ExpressionCombinator.Group)
			{
				m_CombinatorStack.Pop();
				break;
			}
			ProcessCombinatorStack();
		}
		return m_ExpressionStack.Pop();
	}

	private void ProcessCombinatorStack()
	{
		ExpressionCombinator expressionCombinator = m_CombinatorStack.Pop();
		Expression item = m_ExpressionStack.Pop();
		Expression item2 = m_ExpressionStack.Pop();
		m_ProcessExpressionList.Clear();
		m_ProcessExpressionList.Add(item2);
		m_ProcessExpressionList.Add(item);
		while (m_CombinatorStack.Count > 0 && expressionCombinator == m_CombinatorStack.Peek())
		{
			Expression item3 = m_ExpressionStack.Pop();
			m_ProcessExpressionList.Insert(0, item3);
			m_CombinatorStack.Pop();
		}
		Expression expression = new Expression(ExpressionType.Combinator);
		expression.combinator = expressionCombinator;
		expression.subExpressions = m_ProcessExpressionList.ToArray();
		m_ExpressionStack.Push(expression);
	}

	private Expression ParseTerm(StyleSyntaxTokenizer tokenizer)
	{
		Expression expression = null;
		StyleSyntaxToken current = tokenizer.current;
		if (current.type == StyleSyntaxTokenType.LessThan)
		{
			expression = ParseDataType(tokenizer);
		}
		else
		{
			if (current.type != StyleSyntaxTokenType.String)
			{
				throw new Exception($"Unexpected token '{current.type}' in expression. Expected term token");
			}
			expression = new Expression(ExpressionType.Keyword);
			expression.keyword = current.text.ToLower();
			tokenizer.MoveNext();
		}
		ParseMultiplier(tokenizer, ref expression.multiplier);
		return expression;
	}

	private ExpressionCombinator ParseCombinatorType(StyleSyntaxTokenizer tokenizer)
	{
		ExpressionCombinator expressionCombinator = ExpressionCombinator.None;
		StyleSyntaxToken token = tokenizer.current;
		while (!IsExpressionEnd(token) && expressionCombinator == ExpressionCombinator.None)
		{
			StyleSyntaxToken token2 = tokenizer.PeekNext();
			switch (token.type)
			{
			case StyleSyntaxTokenType.Space:
				if (!IsCombinator(token2) && token2.type != StyleSyntaxTokenType.CloseBracket)
				{
					expressionCombinator = ExpressionCombinator.Juxtaposition;
				}
				break;
			case StyleSyntaxTokenType.SingleBar:
				expressionCombinator = ExpressionCombinator.Or;
				break;
			case StyleSyntaxTokenType.DoubleBar:
				expressionCombinator = ExpressionCombinator.OrOr;
				break;
			case StyleSyntaxTokenType.DoubleAmpersand:
				expressionCombinator = ExpressionCombinator.AndAnd;
				break;
			default:
				throw new Exception($"Unexpected token '{token.type}' in expression. Expected combinator token");
			}
			token = tokenizer.MoveNext();
		}
		EatSpace(tokenizer);
		return expressionCombinator;
	}

	private Expression ParseGroup(StyleSyntaxTokenizer tokenizer)
	{
		StyleSyntaxToken current = tokenizer.current;
		if (current.type != StyleSyntaxTokenType.OpenBracket)
		{
			throw new Exception($"Unexpected token '{current.type}' in group expression. Expected '[' token");
		}
		m_CombinatorStack.Push(ExpressionCombinator.Group);
		tokenizer.MoveNext();
		EatSpace(tokenizer);
		Expression expression = ParseExpression(tokenizer);
		current = tokenizer.current;
		if (current.type != StyleSyntaxTokenType.CloseBracket)
		{
			throw new Exception($"Unexpected token '{current.type}' in group expression. Expected ']' token");
		}
		tokenizer.MoveNext();
		Expression expression2 = new Expression(ExpressionType.Combinator);
		expression2.combinator = ExpressionCombinator.Group;
		expression2.subExpressions = new Expression[1] { expression };
		ParseMultiplier(tokenizer, ref expression2.multiplier);
		return expression2;
	}

	private Expression ParseDataType(StyleSyntaxTokenizer tokenizer)
	{
		Expression expression = null;
		StyleSyntaxToken current = tokenizer.current;
		if (current.type != StyleSyntaxTokenType.LessThan)
		{
			throw new Exception($"Unexpected token '{current.type}' in data type expression. Expected '<' token");
		}
		current = tokenizer.MoveNext();
		switch (current.type)
		{
		case StyleSyntaxTokenType.String:
		{
			DataType dataType = DataType.None;
			try
			{
				object obj = Enum.Parse(typeof(DataType), current.text, ignoreCase: true);
				if (obj != null)
				{
					dataType = (DataType)obj;
				}
			}
			catch (Exception)
			{
				throw new Exception("Unknown data type '" + current.text + "'");
			}
			expression = new Expression(ExpressionType.Data);
			expression.dataType = dataType;
			tokenizer.MoveNext();
			break;
		}
		case StyleSyntaxTokenType.SingleQuote:
			expression = ParseProperty(tokenizer);
			break;
		default:
			throw new Exception($"Unexpected token '{current.type}' in data type expression");
		}
		current = tokenizer.current;
		if (current.type != StyleSyntaxTokenType.GreaterThan)
		{
			throw new Exception($"Unexpected token '{current.type}' in data type expression. Expected '>' token");
		}
		tokenizer.MoveNext();
		return expression;
	}

	private Expression ParseProperty(StyleSyntaxTokenizer tokenizer)
	{
		Expression value = null;
		StyleSyntaxToken current = tokenizer.current;
		if (current.type != StyleSyntaxTokenType.SingleQuote)
		{
			throw new Exception($"Unexpected token '{current.type}' in property expression. Expected ''' token");
		}
		current = tokenizer.MoveNext();
		if (current.type != StyleSyntaxTokenType.String)
		{
			throw new Exception($"Unexpected token '{current.type}' in property expression. Expected 'string' token");
		}
		string text = current.text;
		if (!StylePropertyCache.TryGetSyntax(text, out var syntax))
		{
			throw new Exception("Unknown property '" + text + "' <''> expression.");
		}
		if (!m_ParsedExpressionCache.TryGetValue(syntax, out value))
		{
			m_CombinatorStack.Push(ExpressionCombinator.Group);
			value = Parse(syntax);
		}
		current = tokenizer.MoveNext();
		if (current.type != StyleSyntaxTokenType.SingleQuote)
		{
			throw new Exception($"Unexpected token '{current.type}' in property expression. Expected ''' token");
		}
		current = tokenizer.MoveNext();
		if (current.type != StyleSyntaxTokenType.GreaterThan)
		{
			throw new Exception($"Unexpected token '{current.type}' in property expression. Expected '>' token");
		}
		Expression expression = new Expression(ExpressionType.Combinator);
		expression.combinator = ExpressionCombinator.Group;
		expression.subExpressions = new Expression[1] { value };
		return expression;
	}

	private void ParseMultiplier(StyleSyntaxTokenizer tokenizer, ref ExpressionMultiplier multiplier)
	{
		StyleSyntaxToken current = tokenizer.current;
		if (IsMultiplier(current))
		{
			switch (current.type)
			{
			case StyleSyntaxTokenType.Asterisk:
				multiplier.type = ExpressionMultiplierType.ZeroOrMore;
				break;
			case StyleSyntaxTokenType.Plus:
				multiplier.type = ExpressionMultiplierType.OneOrMore;
				break;
			case StyleSyntaxTokenType.QuestionMark:
				multiplier.type = ExpressionMultiplierType.ZeroOrOne;
				break;
			case StyleSyntaxTokenType.HashMark:
				multiplier.type = ExpressionMultiplierType.OneOrMoreComma;
				break;
			case StyleSyntaxTokenType.ExclamationPoint:
				multiplier.type = ExpressionMultiplierType.GroupAtLeastOne;
				break;
			case StyleSyntaxTokenType.OpenBrace:
				multiplier.type = ExpressionMultiplierType.Ranges;
				break;
			default:
				throw new Exception($"Unexpected token '{current.type}' in expression. Expected multiplier token");
			}
			current = tokenizer.MoveNext();
		}
		if (multiplier.type == ExpressionMultiplierType.Ranges)
		{
			ParseRanges(tokenizer, out multiplier.min, out multiplier.max);
		}
	}

	private void ParseRanges(StyleSyntaxTokenizer tokenizer, out int min, out int max)
	{
		min = -1;
		max = -1;
		StyleSyntaxToken styleSyntaxToken = tokenizer.current;
		bool flag = false;
		while (styleSyntaxToken.type != StyleSyntaxTokenType.CloseBrace)
		{
			switch (styleSyntaxToken.type)
			{
			case StyleSyntaxTokenType.Number:
				if (!flag)
				{
					min = styleSyntaxToken.number;
				}
				else
				{
					max = styleSyntaxToken.number;
				}
				break;
			case StyleSyntaxTokenType.Comma:
				flag = true;
				break;
			default:
				throw new Exception($"Unexpected token '{styleSyntaxToken.type}' in expression. Expected ranges token");
			}
			styleSyntaxToken = tokenizer.MoveNext();
		}
		tokenizer.MoveNext();
	}

	private static void EatSpace(StyleSyntaxTokenizer tokenizer)
	{
		if (tokenizer.current.type == StyleSyntaxTokenType.Space)
		{
			tokenizer.MoveNext();
		}
	}

	private static bool IsExpressionEnd(StyleSyntaxToken token)
	{
		StyleSyntaxTokenType type = token.type;
		if (type == StyleSyntaxTokenType.CloseBracket || type == StyleSyntaxTokenType.End)
		{
			return true;
		}
		return false;
	}

	private static bool IsCombinator(StyleSyntaxToken token)
	{
		StyleSyntaxTokenType type = token.type;
		if ((uint)(type - 3) <= 3u)
		{
			return true;
		}
		return false;
	}

	private static bool IsMultiplier(StyleSyntaxToken token)
	{
		StyleSyntaxTokenType type = token.type;
		if ((uint)(type - 9) <= 4u || type == StyleSyntaxTokenType.OpenBrace)
		{
			return true;
		}
		return false;
	}
}
