#define UNITY_ASSERTIONS
using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;
using UnityEngine.UIElements.StyleSheets.Syntax;

namespace UnityEngine.UIElements;

internal class StyleVariableResolver
{
	public enum Result
	{
		Valid,
		Invalid,
		NotFound
	}

	internal const int kMaxResolves = 100;

	private static StyleSyntaxParser s_SyntaxParser = new StyleSyntaxParser();

	private StylePropertyValueMatcher m_Matcher = new StylePropertyValueMatcher();

	private List<StylePropertyValue> m_ResolvedValues = new List<StylePropertyValue>();

	private Stack<string> m_ResolvedVarStack = new Stack<string>();

	private Expression m_ValidationExpression;

	private StyleProperty m_Property;

	private StyleSheet m_Sheet;

	private StyleValueHandle[] m_Handles;

	public List<StylePropertyValue> resolvedValues => m_ResolvedValues;

	public StyleVariableContext variableContext { get; set; }

	public void Init(StyleProperty property, StyleSheet sheet, StyleValueHandle[] handles)
	{
		m_ResolvedValues.Clear();
		m_Sheet = sheet;
		m_Property = property;
		m_Handles = handles;
	}

	public void AddValue(StyleValueHandle handle)
	{
		m_ResolvedValues.Add(new StylePropertyValue
		{
			sheet = m_Sheet,
			handle = handle
		});
	}

	public Result ResolveVarFunction(ref int index)
	{
		m_ResolvedVarStack.Clear();
		m_ValidationExpression = null;
		if (!m_Property.isCustomProperty)
		{
			if (!StylePropertyCache.TryGetSyntax(m_Property.name, out var syntax))
			{
				Debug.LogAssertion("Unknown style property " + m_Property.name);
				return Result.Invalid;
			}
			m_ValidationExpression = s_SyntaxParser.Parse(syntax);
		}
		ParseVarFunction(m_Sheet, m_Handles, ref index, out var argCount, out var variableName);
		Result result = ResolveVariable(variableName);
		if (result != Result.Valid)
		{
			if (result == Result.NotFound && argCount > 1 && !m_Property.isCustomProperty)
			{
				StyleValueHandle styleValueHandle = m_Handles[++index];
				Debug.Assert(styleValueHandle.valueType == StyleValueType.FunctionSeparator, $"Unexpected value type {styleValueHandle.valueType} in var function");
				if (styleValueHandle.valueType == StyleValueType.FunctionSeparator && index + 1 < m_Handles.Length)
				{
					index++;
					result = ResolveFallback(ref index);
				}
			}
			else
			{
				m_ResolvedValues.Clear();
			}
		}
		return result;
	}

	private Result ResolveVariable(string variableName)
	{
		if (!variableContext.TryFindVariable(variableName, out var v))
		{
			return Result.NotFound;
		}
		if (m_ResolvedVarStack.Contains(v.name))
		{
			return Result.NotFound;
		}
		m_ResolvedVarStack.Push(v.name);
		Result result = Result.Valid;
		for (int i = 0; i < v.handles.Length; i++)
		{
			if (result != Result.Valid)
			{
				break;
			}
			StyleValueHandle handle = v.handles[i];
			if (handle.IsVarFunction())
			{
				ParseVarFunction(v.sheet, v.handles, ref i, out var _, out var variableName2);
				result = ResolveVariable(variableName2);
				continue;
			}
			StylePropertyValue spv = new StylePropertyValue
			{
				sheet = v.sheet,
				handle = handle
			};
			result = ValidateResolve(spv);
		}
		m_ResolvedVarStack.Pop();
		return result;
	}

	private Result ValidateResolve(StylePropertyValue spv)
	{
		if (m_ResolvedValues.Count + 1 > 100)
		{
			return Result.Invalid;
		}
		m_ResolvedValues.Add(spv);
		if (m_Property.isCustomProperty)
		{
			return Result.Valid;
		}
		MatchResult matchResult = m_Matcher.Match(m_ValidationExpression, m_ResolvedValues);
		if (!matchResult.success)
		{
			m_ResolvedValues.RemoveAt(m_ResolvedValues.Count - 1);
		}
		return (!matchResult.success) ? Result.Invalid : Result.Valid;
	}

	private Result ResolveFallback(ref int index)
	{
		Result result = Result.Valid;
		while (index < m_Handles.Length && result == Result.Valid)
		{
			StyleValueHandle handle = m_Handles[index];
			if (handle.IsVarFunction())
			{
				ParseVarFunction(m_Sheet, m_Handles, ref index, out var argCount, out var variableName);
				result = ResolveVariable(variableName);
				if (result == Result.NotFound && argCount > 1)
				{
					handle = m_Handles[++index];
					Debug.Assert(handle.valueType == StyleValueType.FunctionSeparator, $"Unexpected value type {handle.valueType} in var function");
					if (handle.valueType == StyleValueType.FunctionSeparator && index + 1 < m_Handles.Length)
					{
						index++;
						result = ResolveFallback(ref index);
					}
				}
			}
			else
			{
				StylePropertyValue spv = new StylePropertyValue
				{
					sheet = m_Sheet,
					handle = handle
				};
				result = ValidateResolve(spv);
			}
			index++;
		}
		return result;
	}

	private static void ParseVarFunction(StyleSheet sheet, StyleValueHandle[] handles, ref int index, out int argCount, out string variableName)
	{
		argCount = (int)sheet.ReadFloat(handles[++index]);
		variableName = sheet.ReadVariable(handles[++index]);
	}
}
