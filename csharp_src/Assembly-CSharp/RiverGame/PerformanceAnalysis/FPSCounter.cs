using System.Runtime.CompilerServices;

namespace RiverGame.PerformanceAnalysis;

public class FPSCounter : FPSCounterBase
{
	private int _sampleTimes;

	private int _framesInSecond;

	private double _totalTime;

	private float _accFPS;

	private float _curFPS;

	public float currFPS
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _curFPS;
		}
	}

	public float avgFPS
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!((float)_sampleTimes > 0f))
			{
				return 0f;
			}
			return _accFPS / (float)_sampleTimes;
		}
	}

	public override void StartNew()
	{
		_sampleTimes = 0;
		_framesInSecond = 0;
		_totalTime = 0.0;
		_accFPS = 0f;
		_curFPS = 0f;
	}

	protected override void Count()
	{
		_framesInSecond++;
		_totalTime += ForegroundTimer.unscaledDeltaTime;
		if (_totalTime >= 0.5)
		{
			_curFPS = (float)((double)_framesInSecond / _totalTime);
			_totalTime = 0.0;
			_framesInSecond = 0;
			_sampleTimes++;
			_accFPS += _curFPS;
		}
	}
}
