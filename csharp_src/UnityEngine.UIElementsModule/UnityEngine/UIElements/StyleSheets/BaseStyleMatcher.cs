#define UNITY_ASSERTIONS
using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets.Syntax;

namespace UnityEngine.UIElements.StyleSheets;

internal abstract class BaseStyleMatcher
{
	private Stack<int> m_MarkStack = new Stack<int>();

	protected int m_CurrentIndex;

	public abstract int valueCount { get; }

	public abstract bool isVariable { get; }

	public bool hasCurrent => m_CurrentIndex < valueCount;

	public int matchedVariableCount { get; set; }

	protected abstract bool MatchKeyword(string keyword);

	protected abstract bool MatchNumber();

	protected abstract bool MatchInteger();

	protected abstract bool MatchLength();

	protected abstract bool MatchPercentage();

	protected abstract bool MatchColor();

	protected abstract bool MatchResource();

	protected abstract bool MatchUrl();

	protected void Initialize()
	{
		m_CurrentIndex = 0;
		m_MarkStack.Clear();
		matchedVariableCount = 0;
	}

	public void MoveNext()
	{
		if (m_CurrentIndex + 1 <= valueCount)
		{
			m_CurrentIndex++;
		}
	}

	public void SaveMark()
	{
		m_MarkStack.Push(m_CurrentIndex);
	}

	public void RestoreMark()
	{
		m_CurrentIndex = m_MarkStack.Pop();
	}

	public void DropMark()
	{
		m_MarkStack.Pop();
	}

	protected bool Match(Expression exp)
	{
		bool flag = true;
		if (exp.multiplier.type == ExpressionMultiplierType.None)
		{
			return MatchExpression(exp);
		}
		Debug.Assert(exp.multiplier.type != ExpressionMultiplierType.OneOrMoreComma, "'#' multiplier in syntax expression is not supported");
		Debug.Assert(exp.multiplier.type != ExpressionMultiplierType.GroupAtLeastOne, "'!' multiplier in syntax expression is not supported");
		int min = exp.multiplier.min;
		int max = exp.multiplier.max;
		int num = 0;
		int num2 = 0;
		while (flag && hasCurrent && num2 < max)
		{
			flag = MatchExpression(exp);
			if (flag)
			{
				num++;
			}
			num2++;
		}
		return num >= min && num <= max;
	}

	private bool MatchExpression(Expression exp)
	{
		bool flag = false;
		if (exp.type == ExpressionType.Combinator)
		{
			flag = MatchCombinator(exp);
		}
		else
		{
			if (isVariable)
			{
				flag = true;
				matchedVariableCount++;
			}
			else if (exp.type == ExpressionType.Data)
			{
				flag = MatchDataType(exp);
			}
			else if (exp.type == ExpressionType.Keyword)
			{
				flag = MatchKeyword(exp.keyword);
			}
			if (flag)
			{
				MoveNext();
			}
		}
		if (!flag && !hasCurrent && matchedVariableCount > 0)
		{
			flag = true;
		}
		return flag;
	}

	private bool MatchGroup(Expression exp)
	{
		Debug.Assert(exp.subExpressions.Length == 1, "Group has invalid number of sub expressions");
		Expression exp2 = exp.subExpressions[0];
		return Match(exp2);
	}

	private bool MatchCombinator(Expression exp)
	{
		SaveMark();
		bool flag = false;
		switch (exp.combinator)
		{
		case ExpressionCombinator.Or:
			flag = MatchOr(exp);
			break;
		case ExpressionCombinator.OrOr:
			flag = MatchOrOr(exp);
			break;
		case ExpressionCombinator.AndAnd:
			flag = MatchAndAnd(exp);
			break;
		case ExpressionCombinator.Juxtaposition:
			flag = MatchJuxtaposition(exp);
			break;
		case ExpressionCombinator.Group:
			flag = MatchGroup(exp);
			break;
		}
		if (flag)
		{
			DropMark();
		}
		else
		{
			RestoreMark();
		}
		return flag;
	}

	private bool MatchOr(Expression exp)
	{
		bool flag = false;
		int num = 0;
		while (!flag && num < exp.subExpressions.Length)
		{
			flag = Match(exp.subExpressions[num]);
			num++;
		}
		return flag;
	}

	private bool MatchOrOr(Expression exp)
	{
		int num = MatchMany(exp);
		return num > 0;
	}

	private bool MatchAndAnd(Expression exp)
	{
		int num = MatchMany(exp);
		int num2 = exp.subExpressions.Length;
		return num == num2;
	}

	private unsafe int MatchMany(Expression exp)
	{
		int num = 0;
		int num2 = 0;
		int num3 = exp.subExpressions.Length;
		int* ptr = stackalloc int[num3];
		int num4 = 0;
		while (num4 < num3 && num + num2 < num3)
		{
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				if (ptr[i] == num4)
				{
					flag = true;
					break;
				}
			}
			bool flag2 = false;
			if (!flag)
			{
				flag2 = Match(exp.subExpressions[num4]);
			}
			if (flag2)
			{
				if (num2 == matchedVariableCount)
				{
					ptr[num] = num4;
					num++;
				}
				else
				{
					num2 = matchedVariableCount;
				}
				num4 = 0;
			}
			else
			{
				num4++;
			}
		}
		return num + num2;
	}

	private bool MatchJuxtaposition(Expression exp)
	{
		bool flag = true;
		int num = 0;
		while (flag && num < exp.subExpressions.Length)
		{
			flag = Match(exp.subExpressions[num]);
			num++;
		}
		return flag;
	}

	private bool MatchDataType(Expression exp)
	{
		bool result = false;
		if (hasCurrent)
		{
			switch (exp.dataType)
			{
			case DataType.Number:
				result = MatchNumber();
				break;
			case DataType.Integer:
				result = MatchInteger();
				break;
			case DataType.Length:
				result = MatchLength();
				break;
			case DataType.Percentage:
				result = MatchPercentage();
				break;
			case DataType.Color:
				result = MatchColor();
				break;
			case DataType.Resource:
				result = MatchResource();
				break;
			case DataType.Url:
				result = MatchUrl();
				break;
			}
		}
		return result;
	}
}
