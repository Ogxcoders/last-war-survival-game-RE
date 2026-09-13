using System.Reflection;

namespace System;

[Serializable]
internal class __Filters
{
	internal static readonly __Filters Instance = new __Filters();

	internal virtual bool FilterAttribute(MemberInfo m, object filterCriteria)
	{
		if (filterCriteria == null)
		{
			throw new InvalidFilterCriteriaException(Environment.GetResourceString("An Int32 must be provided for the filter criteria."));
		}
		switch (m.MemberType)
		{
		case MemberTypes.Constructor:
		case MemberTypes.Method:
		{
			MethodAttributes methodAttributes = MethodAttributes.PrivateScope;
			try
			{
				methodAttributes = (MethodAttributes)(int)filterCriteria;
			}
			catch
			{
				throw new InvalidFilterCriteriaException(Environment.GetResourceString("An Int32 must be provided for the filter criteria."));
			}
			MethodAttributes methodAttributes2 = ((m.MemberType != MemberTypes.Method) ? ((ConstructorInfo)m).Attributes : ((MethodInfo)m).Attributes);
			if ((methodAttributes & MethodAttributes.MemberAccessMask) != MethodAttributes.PrivateScope && (methodAttributes2 & MethodAttributes.MemberAccessMask) != (methodAttributes & MethodAttributes.MemberAccessMask))
			{
				return false;
			}
			if ((methodAttributes & MethodAttributes.Static) != MethodAttributes.PrivateScope && (methodAttributes2 & MethodAttributes.Static) == 0)
			{
				return false;
			}
			if ((methodAttributes & MethodAttributes.Final) != MethodAttributes.PrivateScope && (methodAttributes2 & MethodAttributes.Final) == 0)
			{
				return false;
			}
			if ((methodAttributes & MethodAttributes.Virtual) != MethodAttributes.PrivateScope && (methodAttributes2 & MethodAttributes.Virtual) == 0)
			{
				return false;
			}
			if ((methodAttributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope && (methodAttributes2 & MethodAttributes.Abstract) == 0)
			{
				return false;
			}
			if ((methodAttributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope && (methodAttributes2 & MethodAttributes.SpecialName) == 0)
			{
				return false;
			}
			return true;
		}
		case MemberTypes.Field:
		{
			FieldAttributes fieldAttributes = FieldAttributes.PrivateScope;
			try
			{
				fieldAttributes = (FieldAttributes)(int)filterCriteria;
			}
			catch
			{
				throw new InvalidFilterCriteriaException(Environment.GetResourceString("An Int32 must be provided for the filter criteria."));
			}
			FieldAttributes attributes = ((FieldInfo)m).Attributes;
			if ((fieldAttributes & FieldAttributes.FieldAccessMask) != FieldAttributes.PrivateScope && (attributes & FieldAttributes.FieldAccessMask) != (fieldAttributes & FieldAttributes.FieldAccessMask))
			{
				return false;
			}
			if ((fieldAttributes & FieldAttributes.Static) != FieldAttributes.PrivateScope && (attributes & FieldAttributes.Static) == 0)
			{
				return false;
			}
			if ((fieldAttributes & FieldAttributes.InitOnly) != FieldAttributes.PrivateScope && (attributes & FieldAttributes.InitOnly) == 0)
			{
				return false;
			}
			if ((fieldAttributes & FieldAttributes.Literal) != FieldAttributes.PrivateScope && (attributes & FieldAttributes.Literal) == 0)
			{
				return false;
			}
			if ((fieldAttributes & FieldAttributes.NotSerialized) != FieldAttributes.PrivateScope && (attributes & FieldAttributes.NotSerialized) == 0)
			{
				return false;
			}
			if ((fieldAttributes & FieldAttributes.PinvokeImpl) != FieldAttributes.PrivateScope && (attributes & FieldAttributes.PinvokeImpl) == 0)
			{
				return false;
			}
			return true;
		}
		default:
			return false;
		}
	}

	internal virtual bool FilterName(MemberInfo m, object filterCriteria)
	{
		if (filterCriteria == null || !(filterCriteria is string))
		{
			throw new InvalidFilterCriteriaException(Environment.GetResourceString("A String must be provided for the filter criteria."));
		}
		string text = (string)filterCriteria;
		text = text.Trim();
		string text2 = m.Name;
		if (m.MemberType == MemberTypes.NestedType)
		{
			text2 = text2.Substring(text2.LastIndexOf('+') + 1);
		}
		if (text.Length > 0 && text[text.Length - 1] == '*')
		{
			text = text.Substring(0, text.Length - 1);
			return text2.StartsWith(text, StringComparison.Ordinal);
		}
		return text2.Equals(text);
	}

	internal virtual bool FilterIgnoreCase(MemberInfo m, object filterCriteria)
	{
		if (filterCriteria == null || !(filterCriteria is string))
		{
			throw new InvalidFilterCriteriaException(Environment.GetResourceString("A String must be provided for the filter criteria."));
		}
		string text = (string)filterCriteria;
		text = text.Trim();
		string text2 = m.Name;
		if (m.MemberType == MemberTypes.NestedType)
		{
			text2 = text2.Substring(text2.LastIndexOf('+') + 1);
		}
		if (text.Length > 0 && text[text.Length - 1] == '*')
		{
			text = text.Substring(0, text.Length - 1);
			return string.Compare(text2, 0, text, 0, text.Length, StringComparison.OrdinalIgnoreCase) == 0;
		}
		return string.Compare(text, text2, StringComparison.OrdinalIgnoreCase) == 0;
	}
}
