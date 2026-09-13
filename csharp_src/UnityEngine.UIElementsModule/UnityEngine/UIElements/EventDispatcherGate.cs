using System;

namespace UnityEngine.UIElements;

public struct EventDispatcherGate : IDisposable, IEquatable<EventDispatcherGate>
{
	private readonly EventDispatcher m_Dispatcher;

	public EventDispatcherGate(EventDispatcher d)
	{
		if (d == null)
		{
			throw new ArgumentNullException("d");
		}
		m_Dispatcher = d;
		m_Dispatcher.CloseGate();
	}

	public void Dispose()
	{
		m_Dispatcher.OpenGate();
	}

	public bool Equals(EventDispatcherGate other)
	{
		return object.Equals(m_Dispatcher, other.m_Dispatcher);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		return obj is EventDispatcherGate && Equals((EventDispatcherGate)obj);
	}

	public override int GetHashCode()
	{
		return (m_Dispatcher != null) ? m_Dispatcher.GetHashCode() : 0;
	}

	public static bool operator ==(EventDispatcherGate left, EventDispatcherGate right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(EventDispatcherGate left, EventDispatcherGate right)
	{
		return !left.Equals(right);
	}
}
