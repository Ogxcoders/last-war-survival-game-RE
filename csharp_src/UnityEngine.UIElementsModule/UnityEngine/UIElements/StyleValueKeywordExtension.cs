using System;

namespace UnityEngine.UIElements;

internal static class StyleValueKeywordExtension
{
	public static string ToUssString(this StyleValueKeyword svk)
	{
		return svk switch
		{
			StyleValueKeyword.Inherit => "inherit", 
			StyleValueKeyword.Initial => "initial", 
			StyleValueKeyword.Auto => "auto", 
			StyleValueKeyword.Unset => "unset", 
			StyleValueKeyword.True => "true", 
			StyleValueKeyword.False => "false", 
			StyleValueKeyword.None => "none", 
			_ => throw new ArgumentOutOfRangeException("svk", svk, "Unknown StyleValueKeyword"), 
		};
	}
}
