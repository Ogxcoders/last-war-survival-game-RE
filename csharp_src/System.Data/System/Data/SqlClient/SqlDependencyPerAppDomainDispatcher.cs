using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Threading;

namespace System.Data.SqlClient;

internal class SqlDependencyPerAppDomainDispatcher : MarshalByRefObject
{
	private sealed class DependencyList : List<SqlDependency>
	{
		public readonly string CommandHash;

		internal DependencyList(string commandHash)
		{
			CommandHash = commandHash;
		}
	}

	internal static readonly SqlDependencyPerAppDomainDispatcher SingletonInstance = new SqlDependencyPerAppDomainDispatcher();

	internal object _instanceLock = new object();

	private Dictionary<string, SqlDependency> _dependencyIdToDependencyHash;

	private Dictionary<string, DependencyList> _notificationIdToDependenciesHash;

	private Dictionary<string, string> _commandHashToNotificationId;

	private bool _sqlDependencyTimeOutTimerStarted;

	private DateTime _nextTimeout;

	private Timer _timeoutTimer;

	private SqlDependencyPerAppDomainDispatcher()
	{
		_dependencyIdToDependencyHash = new Dictionary<string, SqlDependency>();
		_notificationIdToDependenciesHash = new Dictionary<string, DependencyList>();
		_commandHashToNotificationId = new Dictionary<string, string>();
		_timeoutTimer = ADP.UnsafeCreateTimer(TimeoutTimerCallback, null, -1, -1);
		SubscribeToAppDomainUnload();
	}

	public override object InitializeLifetimeService()
	{
		return null;
	}

	internal void AddDependencyEntry(SqlDependency dep)
	{
		lock (_instanceLock)
		{
			_dependencyIdToDependencyHash.Add(dep.Id, dep);
		}
	}

	internal string AddCommandEntry(string commandHash, SqlDependency dep)
	{
		string value = string.Empty;
		lock (_instanceLock)
		{
			if (_dependencyIdToDependencyHash.ContainsKey(dep.Id))
			{
				if (_commandHashToNotificationId.TryGetValue(commandHash, out value))
				{
					DependencyList value2 = null;
					if (!_notificationIdToDependenciesHash.TryGetValue(value, out value2))
					{
						throw ADP.InternalError(ADP.InternalErrorCode.SqlDependencyCommandHashIsNotAssociatedWithNotification);
					}
					if (!value2.Contains(dep))
					{
						value2.Add(dep);
					}
				}
				else
				{
					value = string.Format(CultureInfo.InvariantCulture, "{0};{1}", SqlDependency.AppDomainKey, Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture));
					DependencyList dependencyList = new DependencyList(commandHash);
					dependencyList.Add(dep);
					_commandHashToNotificationId.Add(commandHash, value);
					_notificationIdToDependenciesHash.Add(value, dependencyList);
				}
			}
		}
		return value;
	}

	internal void InvalidateCommandID(SqlNotification sqlNotification)
	{
		List<SqlDependency> list = null;
		lock (_instanceLock)
		{
			list = LookupCommandEntryWithRemove(sqlNotification.Key);
			if (list != null)
			{
				foreach (SqlDependency item in list)
				{
					LookupDependencyEntryWithRemove(item.Id);
					RemoveDependencyFromCommandToDependenciesHash(item);
				}
			}
		}
		if (list == null)
		{
			return;
		}
		foreach (SqlDependency item2 in list)
		{
			try
			{
				item2.Invalidate(sqlNotification.Type, sqlNotification.Info, sqlNotification.Source);
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableExceptionType(e))
				{
					throw;
				}
				ADP.TraceExceptionWithoutRethrow(e);
			}
		}
	}

	internal void InvalidateServer(string server, SqlNotification sqlNotification)
	{
		List<SqlDependency> list = new List<SqlDependency>();
		lock (_instanceLock)
		{
			foreach (KeyValuePair<string, SqlDependency> item in _dependencyIdToDependencyHash)
			{
				SqlDependency value = item.Value;
				if (value.ContainsServer(server))
				{
					list.Add(value);
				}
			}
			foreach (SqlDependency item2 in list)
			{
				LookupDependencyEntryWithRemove(item2.Id);
				RemoveDependencyFromCommandToDependenciesHash(item2);
			}
		}
		foreach (SqlDependency item3 in list)
		{
			try
			{
				item3.Invalidate(sqlNotification.Type, sqlNotification.Info, sqlNotification.Source);
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableExceptionType(e))
				{
					throw;
				}
				ADP.TraceExceptionWithoutRethrow(e);
			}
		}
	}

	internal SqlDependency LookupDependencyEntry(string id)
	{
		if (id == null)
		{
			throw ADP.ArgumentNull("id");
		}
		if (string.IsNullOrEmpty(id))
		{
			throw SQL.SqlDependencyIdMismatch();
		}
		SqlDependency result = null;
		lock (_instanceLock)
		{
			if (_dependencyIdToDependencyHash.ContainsKey(id))
			{
				result = _dependencyIdToDependencyHash[id];
			}
		}
		return result;
	}

	private void LookupDependencyEntryWithRemove(string id)
	{
		lock (_instanceLock)
		{
			if (_dependencyIdToDependencyHash.ContainsKey(id))
			{
				_dependencyIdToDependencyHash.Remove(id);
				if (_dependencyIdToDependencyHash.Count == 0)
				{
					_timeoutTimer.Change(-1, -1);
					_sqlDependencyTimeOutTimerStarted = false;
				}
			}
		}
	}

	private List<SqlDependency> LookupCommandEntryWithRemove(string notificationId)
	{
		DependencyList value = null;
		lock (_instanceLock)
		{
			if (_notificationIdToDependenciesHash.TryGetValue(notificationId, out value))
			{
				_notificationIdToDependenciesHash.Remove(notificationId);
				_commandHashToNotificationId.Remove(value.CommandHash);
			}
		}
		return value;
	}

	private void RemoveDependencyFromCommandToDependenciesHash(SqlDependency dependency)
	{
		lock (_instanceLock)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			foreach (KeyValuePair<string, DependencyList> item in _notificationIdToDependenciesHash)
			{
				DependencyList value = item.Value;
				if (value.Remove(dependency) && value.Count == 0)
				{
					list.Add(item.Key);
					list2.Add(item.Value.CommandHash);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				_notificationIdToDependenciesHash.Remove(list[i]);
				_commandHashToNotificationId.Remove(list2[i]);
			}
		}
	}

	internal void StartTimer(SqlDependency dep)
	{
		lock (_instanceLock)
		{
			if (!_sqlDependencyTimeOutTimerStarted)
			{
				_timeoutTimer.Change(15000, 15000);
				_nextTimeout = dep.ExpirationTime;
				_sqlDependencyTimeOutTimerStarted = true;
			}
			else if (_nextTimeout > dep.ExpirationTime)
			{
				_nextTimeout = dep.ExpirationTime;
			}
		}
	}

	private static void TimeoutTimerCallback(object state)
	{
		SqlDependency[] array;
		lock (SingletonInstance._instanceLock)
		{
			if (SingletonInstance._dependencyIdToDependencyHash.Count == 0 || SingletonInstance._nextTimeout > DateTime.UtcNow)
			{
				return;
			}
			array = new SqlDependency[SingletonInstance._dependencyIdToDependencyHash.Count];
			SingletonInstance._dependencyIdToDependencyHash.Values.CopyTo(array, 0);
		}
		DateTime utcNow = DateTime.UtcNow;
		DateTime dateTime = DateTime.MaxValue;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].ExpirationTime <= utcNow)
			{
				try
				{
					array[i].Invalidate(SqlNotificationType.Change, SqlNotificationInfo.Error, SqlNotificationSource.Timeout);
				}
				catch (Exception e)
				{
					if (!ADP.IsCatchableExceptionType(e))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(e);
				}
			}
			else
			{
				if (array[i].ExpirationTime < dateTime)
				{
					dateTime = array[i].ExpirationTime;
				}
				array[i] = null;
			}
		}
		lock (SingletonInstance._instanceLock)
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] != null)
				{
					SingletonInstance._dependencyIdToDependencyHash.Remove(array[j].Id);
				}
			}
			if (dateTime < SingletonInstance._nextTimeout)
			{
				SingletonInstance._nextTimeout = dateTime;
			}
		}
	}

	private void SubscribeToAppDomainUnload()
	{
	}
}
