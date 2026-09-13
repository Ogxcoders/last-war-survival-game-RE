using System;

namespace UnityEngine.UIElements;

public struct ManipulatorActivationFilter : IEquatable<ManipulatorActivationFilter>
{
	public MouseButton button { get; set; }

	public EventModifiers modifiers { get; set; }

	public int clickCount { get; set; }

	public override bool Equals(object obj)
	{
		return obj is ManipulatorActivationFilter && Equals((ManipulatorActivationFilter)obj);
	}

	public bool Equals(ManipulatorActivationFilter other)
	{
		return button == other.button && modifiers == other.modifiers && clickCount == other.clickCount;
	}

	public override int GetHashCode()
	{
		int num = 390957112;
		num = num * -1521134295 + button.GetHashCode();
		num = num * -1521134295 + modifiers.GetHashCode();
		return num * -1521134295 + clickCount.GetHashCode();
	}

	public bool Matches(IMouseEvent e)
	{
		if (e == null)
		{
			return false;
		}
		bool flag = clickCount == 0 || e.clickCount >= clickCount;
		return button == (MouseButton)e.button && HasModifiers(e) && flag;
	}

	private bool HasModifiers(IMouseEvent e)
	{
		if (e == null)
		{
			return false;
		}
		if (((modifiers & EventModifiers.Alt) != EventModifiers.None && !e.altKey) || ((modifiers & EventModifiers.Alt) == 0 && e.altKey))
		{
			return false;
		}
		if (((modifiers & EventModifiers.Control) != EventModifiers.None && !e.ctrlKey) || ((modifiers & EventModifiers.Control) == 0 && e.ctrlKey))
		{
			return false;
		}
		if (((modifiers & EventModifiers.Shift) != EventModifiers.None && !e.shiftKey) || ((modifiers & EventModifiers.Shift) == 0 && e.shiftKey))
		{
			return false;
		}
		return ((modifiers & EventModifiers.Command) == 0 || e.commandKey) && ((modifiers & EventModifiers.Command) != EventModifiers.None || !e.commandKey);
	}

	public static bool operator ==(ManipulatorActivationFilter filter1, ManipulatorActivationFilter filter2)
	{
		return filter1.Equals(filter2);
	}

	public static bool operator !=(ManipulatorActivationFilter filter1, ManipulatorActivationFilter filter2)
	{
		return !(filter1 == filter2);
	}
}
