using System;
using System.Collections.Generic;
using Framework.Utils.UnityEx;
using GameFramework;
using GameKit.Base;
using Sfs2X.Entities.Data;
using UnityEngine;
using XLua;

namespace UnityGameFramework.Runtime;

public class EventComponent : IGameController
{
	public class ObjectPool<T>
	{
		private readonly Queue<T> _objects;

		private readonly Func<T> _objectGenerator;

		public ObjectPool(Func<T> objectGenerator)
		{
			_objectGenerator = objectGenerator ?? throw new ArgumentNullException("objectGenerator");
			_objects = new Queue<T>();
		}

		public T Get()
		{
			if (_objects.Count > 0)
			{
				return _objects.Dequeue();
			}
			return _objectGenerator();
		}

		public void Return(T item)
		{
			_objects.Enqueue(item);
		}

		public int Count()
		{
			return _objects.Count;
		}
	}

	private ObjectPool<List<Action<object>>> m_Pool = new ObjectPool<List<Action<object>>>(() => new List<Action<object>>(1));

	private readonly Dictionary<int, List<Action<object>>> m_EventHandlers;

	private readonly EventPoolMode m_EventPoolMode;

	public EventComponent(EventPoolMode mode)
	{
		m_EventHandlers = new Dictionary<int, List<Action<object>>>();
		m_EventPoolMode = mode;
	}

	public void OnUpdate(float elapseSeconds)
	{
	}

	public void Shutdown()
	{
		m_EventHandlers.Clear();
	}

	private bool Check(EventId id, Action<object> handler)
	{
		return false;
	}

	public void Subscribe(EventId eventID, Action<object> handler)
	{
		if (handler == null)
		{
			throw new GameFrameworkException("Event handler is invalid.");
		}
		int key = (int)eventID;
		List<Action<object>> value = null;
		if (!m_EventHandlers.TryGetValue(key, out value) || value == null)
		{
			value = m_Pool.Get();
			value.Clear();
			value.Add(handler);
			m_EventHandlers[key] = value;
		}
		else if ((m_EventPoolMode & EventPoolMode.AllowMultiHandler) == 0)
		{
			Log.Error($"Event '{key.ToString()}' not allow multi handler.");
		}
		else if ((m_EventPoolMode & EventPoolMode.AllowDuplicateHandler) == 0 && Check(eventID, handler))
		{
			Log.Error($"Event '{key.ToString()}' not allow duplicate handler.");
		}
		else
		{
			value.Add(handler);
		}
	}

	public void Unsubscribe(EventId eventId, Action<object> handler)
	{
		if (handler == null)
		{
			throw new GameFrameworkException("Event handler is invalid.");
		}
		if (!m_EventHandlers.TryGetValue((int)eventId, out var value) || value == null)
		{
			return;
		}
		for (int num = value.Count - 1; num >= 0; num--)
		{
			if (value[num] == handler)
			{
				value.RemoveAt(num);
				break;
			}
		}
		if (value.Count == 0)
		{
			if (m_Pool.Count() < 128)
			{
				m_Pool.Return(value);
			}
			m_EventHandlers[(int)eventId] = null;
		}
	}

	public void Fire(EventId eventId, object userData = null)
	{
		bool csharpOnly = userData != null && !(userData is int) && !(userData is long) && !(userData is bool) && !(userData is string) && !(userData is Vector2) && !(userData is Vector3) && !(userData is LuaTable) && !(userData is TouchInfo) && !(userData is SFSObject);
		using (ProfilerRuntime.CreateSample(eventId.ToString()))
		{
			HandleEvent(eventId, userData, csharpOnly);
		}
	}

	private void HandleEvent(EventId eventId, object userData, bool csharpOnly = false)
	{
		int num = (int)eventId;
		if (m_EventHandlers.TryGetValue(num, out var value) && value != null)
		{
			List<Action<object>> list = m_Pool.Get();
			list.Clear();
			for (int i = 0; i < value.Count; i++)
			{
				list.Add(value[i]);
			}
			int count = list.Count;
			for (int j = 0; j < count; j++)
			{
				try
				{
					list[j](userData);
				}
				catch (Exception ex)
				{
					Log.Error("HandleEvent process exception!!! exception:{0}", ex.ToString());
				}
			}
			m_Pool.Return(list);
		}
		if (value == null && (m_EventPoolMode & EventPoolMode.AllowNoHandler) == 0)
		{
			throw new GameFrameworkException($"Event '{eventId.ToString()}' not allow no handler.");
		}
		if (!csharpOnly)
		{
			if (userData is SFSObject)
			{
				GameEntry.Lua.EventManager?.DispatchCSEventSFSObject(num, (userData as SFSObject).ToBinary().Bytes);
			}
			else
			{
				GameEntry.Lua.EventManager?.DispatchCSEvent(num, userData);
			}
		}
	}
}
