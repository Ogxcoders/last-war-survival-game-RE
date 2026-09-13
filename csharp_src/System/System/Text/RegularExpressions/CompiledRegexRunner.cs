namespace System.Text.RegularExpressions;

internal sealed class CompiledRegexRunner : RegexRunner
{
	private Action<RegexRunner> _goMethod;

	private Func<RegexRunner, bool> _findFirstCharMethod;

	private Action<RegexRunner> _initTrackCountMethod;

	public void SetDelegates(Action<RegexRunner> go, Func<RegexRunner, bool> firstChar, Action<RegexRunner> trackCount)
	{
		_goMethod = go;
		_findFirstCharMethod = firstChar;
		_initTrackCountMethod = trackCount;
	}

	protected override void Go()
	{
		_goMethod(this);
	}

	protected override bool FindFirstChar()
	{
		return _findFirstCharMethod(this);
	}

	protected override void InitTrackCount()
	{
		_initTrackCountMethod(this);
	}
}
