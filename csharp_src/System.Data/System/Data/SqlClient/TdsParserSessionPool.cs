using System.Collections.Generic;
using System.Data.Common;

namespace System.Data.SqlClient;

internal class TdsParserSessionPool
{
	private const int MaxInactiveCount = 10;

	private readonly TdsParser _parser;

	private readonly List<TdsParserStateObject> _cache;

	private int _cachedCount;

	private TdsParserStateObject[] _freeStateObjects;

	private int _freeStateObjectCount;

	private bool IsDisposed => _freeStateObjects == null;

	internal int ActiveSessionsCount => _cachedCount - _freeStateObjectCount;

	internal TdsParserSessionPool(TdsParser parser)
	{
		_parser = parser;
		_cache = new List<TdsParserStateObject>();
		_freeStateObjects = new TdsParserStateObject[10];
		_freeStateObjectCount = 0;
	}

	internal void Deactivate()
	{
		lock (_cache)
		{
			for (int num = _cache.Count - 1; num >= 0; num--)
			{
				TdsParserStateObject tdsParserStateObject = _cache[num];
				if (tdsParserStateObject != null && tdsParserStateObject.IsOrphaned)
				{
					PutSession(tdsParserStateObject);
				}
			}
		}
	}

	internal void Dispose()
	{
		lock (_cache)
		{
			for (int i = 0; i < _freeStateObjectCount; i++)
			{
				if (_freeStateObjects[i] != null)
				{
					_freeStateObjects[i].Dispose();
				}
			}
			_freeStateObjects = null;
			_freeStateObjectCount = 0;
			for (int j = 0; j < _cache.Count; j++)
			{
				if (_cache[j] != null)
				{
					if (_cache[j].IsOrphaned)
					{
						_cache[j].Dispose();
					}
					else
					{
						_cache[j].DecrementPendingCallbacks(release: false);
					}
				}
			}
			_cache.Clear();
			_cachedCount = 0;
		}
	}

	internal TdsParserStateObject GetSession(object owner)
	{
		lock (_cache)
		{
			if (IsDisposed)
			{
				throw ADP.ClosedConnectionError();
			}
			TdsParserStateObject tdsParserStateObject;
			if (_freeStateObjectCount > 0)
			{
				_freeStateObjectCount--;
				tdsParserStateObject = _freeStateObjects[_freeStateObjectCount];
				_freeStateObjects[_freeStateObjectCount] = null;
			}
			else
			{
				tdsParserStateObject = _parser.CreateSession();
				_cache.Add(tdsParserStateObject);
				_cachedCount = _cache.Count;
			}
			tdsParserStateObject.Activate(owner);
			return tdsParserStateObject;
		}
	}

	internal void PutSession(TdsParserStateObject session)
	{
		bool flag = session.Deactivate();
		lock (_cache)
		{
			if (IsDisposed)
			{
				session.Dispose();
			}
			else if (flag && _freeStateObjectCount < 10)
			{
				_freeStateObjects[_freeStateObjectCount] = session;
				_freeStateObjectCount++;
			}
			else
			{
				_cache.Remove(session);
				_cachedCount = _cache.Count;
				session.Dispose();
			}
			session.RemoveOwner();
		}
	}
}
