using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace System.Text.RegularExpressions;

internal sealed class RegexLWCGCompiler : RegexCompiler
{
	private static int s_regexCount = 0;

	private static Type[] s_paramTypes = new Type[1] { typeof(RegexRunner) };

	public RegexRunnerFactory FactoryInstanceFromCode(RegexCode code, RegexOptions options)
	{
		_code = code;
		_codes = code.Codes;
		_strings = code.Strings;
		_fcPrefix = code.FCPrefix;
		_bmPrefix = code.BMPrefix;
		_anchors = code.Anchors;
		_trackcount = code.TrackCount;
		_options = options;
		string text = Interlocked.Increment(ref s_regexCount).ToString(CultureInfo.InvariantCulture);
		DynamicMethod go = DefineDynamicMethod("Go" + text, null, typeof(CompiledRegexRunner));
		GenerateGo();
		DynamicMethod firstChar = DefineDynamicMethod("FindFirstChar" + text, typeof(bool), typeof(CompiledRegexRunner));
		GenerateFindFirstChar();
		DynamicMethod trackCount = DefineDynamicMethod("InitTrackCount" + text, null, typeof(CompiledRegexRunner));
		GenerateInitTrackCount();
		return new CompiledRegexRunnerFactory(go, firstChar, trackCount);
	}

	public DynamicMethod DefineDynamicMethod(string methname, Type returntype, Type hostType)
	{
		MethodAttributes attributes = MethodAttributes.Public | MethodAttributes.Static;
		CallingConventions callingConvention = CallingConventions.Standard;
		DynamicMethod dynamicMethod = new DynamicMethod(methname, attributes, callingConvention, returntype, s_paramTypes, hostType, skipVisibility: false);
		_ilg = dynamicMethod.GetILGenerator();
		return dynamicMethod;
	}
}
