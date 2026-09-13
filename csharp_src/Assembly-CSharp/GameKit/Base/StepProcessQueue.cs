using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameKit.Base;

public class StepProcessQueue<T>
{
	private readonly Queue<T> _queue = new Queue<T>();

	private readonly Stopwatch _timer = new Stopwatch();

	private readonly long _timeLimit;

	private readonly int _minTask;

	private readonly int _maxTask = int.MaxValue;

	private readonly Action<T> _action;

	public int Count => _queue.Count;

	public StepProcessQueue(Action<T> action, long timeLimit, int minTask = 1, int maxTask = 9999)
	{
		_timeLimit = timeLimit;
		_minTask = minTask;
		_maxTask = maxTask;
		_action = action;
	}

	public void Push(T item)
	{
		_queue.Enqueue(item);
	}

	public void Clear()
	{
		_queue.Clear();
	}

	public void UpdateStep()
	{
		_timer.Restart();
		int num = _minTask;
		if (_queue.Count > _maxTask - _minTask)
		{
			num += _maxTask / 10;
		}
		while (_queue.Count > 0)
		{
			num--;
			T obj = _queue.Dequeue();
			_action(obj);
			if (num <= 0 && _timer.ElapsedMilliseconds >= _timeLimit)
			{
				break;
			}
		}
		_timer.Stop();
	}

	public void UpdateAll()
	{
		while (_queue.Count > 0)
		{
			T obj = _queue.Dequeue();
			_action(obj);
		}
	}
}
