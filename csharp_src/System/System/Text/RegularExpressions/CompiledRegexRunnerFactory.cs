using System.Reflection.Emit;

namespace System.Text.RegularExpressions;

internal sealed class CompiledRegexRunnerFactory : RegexRunnerFactory
{
	private readonly DynamicMethod _goMethod;

	private readonly DynamicMethod _findFirstCharMethod;

	private readonly DynamicMethod _initTrackCountMethod;

	public CompiledRegexRunnerFactory(DynamicMethod go, DynamicMethod firstChar, DynamicMethod trackCount)
	{
		_goMethod = go;
		_findFirstCharMethod = firstChar;
		_initTrackCountMethod = trackCount;
	}

	protected internal override RegexRunner CreateInstance()
	{
		CompiledRegexRunner compiledRegexRunner = new CompiledRegexRunner();
		compiledRegexRunner.SetDelegates((Action<RegexRunner>)_goMethod.CreateDelegate(typeof(Action<RegexRunner>)), (Func<RegexRunner, bool>)_findFirstCharMethod.CreateDelegate(typeof(Func<RegexRunner, bool>)), (Action<RegexRunner>)_initTrackCountMethod.CreateDelegate(typeof(Action<RegexRunner>)));
		return compiledRegexRunner;
	}
}
