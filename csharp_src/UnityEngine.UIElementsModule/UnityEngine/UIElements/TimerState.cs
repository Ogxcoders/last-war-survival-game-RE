using System;

namespace UnityEngine.UIElements;

public struct TimerState : IEquatable<TimerState>
{
	public long start { get; set; }

	public long now { get; set; }

	public long deltaTime => now - start;

	public override bool Equals(object obj)
	{
		return obj is TimerState && Equals((TimerState)obj);
	}

	public bool Equals(TimerState other)
	{
		return start == other.start && now == other.now && deltaTime == other.deltaTime;
	}

	public override int GetHashCode()
	{
		int num = 540054806;
		num = num * -1521134295 + start.GetHashCode();
		num = num * -1521134295 + now.GetHashCode();
		return num * -1521134295 + deltaTime.GetHashCode();
	}

	public static bool operator ==(TimerState state1, TimerState state2)
	{
		return state1.Equals(state2);
	}

	public static bool operator !=(TimerState state1, TimerState state2)
	{
		return !(state1 == state2);
	}
}
