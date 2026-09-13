using System.Diagnostics;

namespace System.Data.SqlClient;

internal class SqlConnectionTimeoutPhaseDuration
{
	private Stopwatch _swDuration = new Stopwatch();

	internal void StartCapture()
	{
		_swDuration.Start();
	}

	internal void StopCapture()
	{
		if (_swDuration.IsRunning)
		{
			_swDuration.Stop();
		}
	}

	internal long GetMilliSecondDuration()
	{
		return _swDuration.ElapsedMilliseconds;
	}
}
