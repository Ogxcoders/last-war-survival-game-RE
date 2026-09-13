using System;

namespace UnityEngine.UIElements;

internal static class StyleValueFunctionExtension
{
	public const string k_Var = "var";

	public const string k_Env = "env";

	public const string k_LinearGradient = "linear-gradient";

	public static StyleValueFunction FromUssString(string ussValue)
	{
		ussValue = ussValue.ToLower();
		return ussValue switch
		{
			"var" => StyleValueFunction.Var, 
			"env" => StyleValueFunction.Env, 
			"linear-gradient" => StyleValueFunction.LinearGradient, 
			_ => throw new ArgumentOutOfRangeException("ussValue", ussValue, "Unknown function name"), 
		};
	}

	public static string ToUssString(this StyleValueFunction svf)
	{
		return svf switch
		{
			StyleValueFunction.Var => "var", 
			StyleValueFunction.Env => "env", 
			StyleValueFunction.LinearGradient => "linear-gradient", 
			_ => throw new ArgumentOutOfRangeException("svf", svf, "Unknown StyleValueFunction"), 
		};
	}
}
