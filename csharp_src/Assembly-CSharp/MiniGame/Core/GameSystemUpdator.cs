using System;
using Box2DSharp.Common;

namespace MiniGame.Core;

public class GameSystemUpdator
{
	public delegate void UpdateCallback();

	private readonly FP _timeStep;

	private FP _timeRunning;

	private FP _timeStepLeft;

	private readonly UpdateCallback _callback;

	public int TickCount { get; private set; }

	public GameSystemUpdator(FP timeStep, UpdateCallback callback)
	{
		if (timeStep <= FP.Zero)
		{
			throw new ArgumentException("TimeStep needs to be greater than 0");
		}
		_callback = callback ?? throw new ArgumentException("GameSystemUpdator needs a valid callback");
		_timeStep = timeStep;
	}

	public void Update(FP dt)
	{
		_timeRunning += dt;
		_timeStepLeft += dt;
		while (_timeStepLeft >= _timeStep)
		{
			_timeStepLeft -= _timeStep;
			TickCount++;
			_callback();
		}
	}
}
