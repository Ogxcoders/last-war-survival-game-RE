using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Runtime.InteropServices.WindowsRuntime;

public sealed class EventRegistrationTokenTable<T> where T : class
{
	private Dictionary<EventRegistrationToken, T> m_tokens = new Dictionary<EventRegistrationToken, T>();

	private volatile T m_invokeList;

	public T InvocationList
	{
		get
		{
			return m_invokeList;
		}
		set
		{
			lock (m_tokens)
			{
				m_tokens.Clear();
				m_invokeList = null;
				if (value != null)
				{
					AddEventHandlerNoLock(value);
				}
			}
		}
	}

	public EventRegistrationTokenTable()
	{
		if (!typeof(Delegate).IsAssignableFrom(typeof(T)))
		{
			throw new InvalidOperationException(Environment.GetResourceString("Type '{0}' is not a delegate type.  EventTokenTable may only be used with delegate types.", typeof(T)));
		}
	}

	public EventRegistrationToken AddEventHandler(T handler)
	{
		if (handler == null)
		{
			return new EventRegistrationToken(0uL);
		}
		lock (m_tokens)
		{
			return AddEventHandlerNoLock(handler);
		}
	}

	private EventRegistrationToken AddEventHandlerNoLock(T handler)
	{
		EventRegistrationToken eventRegistrationToken = GetPreferredToken(handler);
		while (m_tokens.ContainsKey(eventRegistrationToken))
		{
			eventRegistrationToken = new EventRegistrationToken(eventRegistrationToken.Value + 1);
		}
		m_tokens[eventRegistrationToken] = handler;
		Delegate a = (Delegate)(object)m_invokeList;
		a = Delegate.Combine(a, (Delegate)(object)handler);
		m_invokeList = (T)(object)a;
		return eventRegistrationToken;
	}

	[FriendAccessAllowed]
	internal T ExtractHandler(EventRegistrationToken token)
	{
		T value = null;
		lock (m_tokens)
		{
			if (m_tokens.TryGetValue(token, out value))
			{
				RemoveEventHandlerNoLock(token);
			}
		}
		return value;
	}

	private static EventRegistrationToken GetPreferredToken(T handler)
	{
		uint num = 0u;
		Delegate[] invocationList = ((Delegate)(object)handler).GetInvocationList();
		num = (uint)((invocationList.Length != 1) ? handler.GetHashCode() : invocationList[0].Method.GetHashCode());
		return new EventRegistrationToken(((ulong)(uint)typeof(T).MetadataToken << 32) | num);
	}

	public void RemoveEventHandler(EventRegistrationToken token)
	{
		if (token.Value == 0L)
		{
			return;
		}
		lock (m_tokens)
		{
			RemoveEventHandlerNoLock(token);
		}
	}

	public void RemoveEventHandler(T handler)
	{
		if (handler == null)
		{
			return;
		}
		lock (m_tokens)
		{
			EventRegistrationToken preferredToken = GetPreferredToken(handler);
			if (m_tokens.TryGetValue(preferredToken, out var value) && value == handler)
			{
				RemoveEventHandlerNoLock(preferredToken);
				return;
			}
			foreach (KeyValuePair<EventRegistrationToken, T> token in m_tokens)
			{
				if (token.Value == (T)handler)
				{
					RemoveEventHandlerNoLock(token.Key);
					break;
				}
			}
		}
	}

	private void RemoveEventHandlerNoLock(EventRegistrationToken token)
	{
		if (m_tokens.TryGetValue(token, out var value))
		{
			m_tokens.Remove(token);
			Delegate source = (Delegate)(object)m_invokeList;
			source = Delegate.Remove(source, (Delegate)(object)value);
			m_invokeList = (T)(object)source;
		}
	}

	public static EventRegistrationTokenTable<T> GetOrCreateEventRegistrationTokenTable(ref EventRegistrationTokenTable<T> refEventTable)
	{
		if (refEventTable == null)
		{
			Interlocked.CompareExchange(ref refEventTable, new EventRegistrationTokenTable<T>(), null);
		}
		return refEventTable;
	}
}
