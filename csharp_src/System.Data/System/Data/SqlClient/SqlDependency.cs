using System.Collections.Generic;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Data.Sql;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml;

namespace System.Data.SqlClient;

public sealed class SqlDependency
{
	internal class IdentityUserNamePair
	{
		private DbConnectionPoolIdentity _identity;

		private string _userName;

		internal DbConnectionPoolIdentity Identity => _identity;

		internal string UserName => _userName;

		internal IdentityUserNamePair(DbConnectionPoolIdentity identity, string userName)
		{
			_identity = identity;
			_userName = userName;
		}

		public override bool Equals(object value)
		{
			IdentityUserNamePair identityUserNamePair = (IdentityUserNamePair)value;
			bool result = false;
			if (identityUserNamePair == null)
			{
				result = false;
			}
			else if (this == identityUserNamePair)
			{
				result = true;
			}
			else if (_identity != null)
			{
				if (_identity.Equals(identityUserNamePair._identity))
				{
					result = true;
				}
			}
			else if (_userName == identityUserNamePair._userName)
			{
				result = true;
			}
			return result;
		}

		public override int GetHashCode()
		{
			int num = 0;
			if (_identity != null)
			{
				return _identity.GetHashCode();
			}
			return _userName.GetHashCode();
		}
	}

	private class DatabaseServicePair
	{
		private string _database;

		private string _service;

		internal string Database => _database;

		internal string Service => _service;

		internal DatabaseServicePair(string database, string service)
		{
			_database = database;
			_service = service;
		}

		public override bool Equals(object value)
		{
			DatabaseServicePair databaseServicePair = (DatabaseServicePair)value;
			bool result = false;
			if (databaseServicePair == null)
			{
				result = false;
			}
			else if (this == databaseServicePair)
			{
				result = true;
			}
			else if (_database == databaseServicePair._database)
			{
				result = true;
			}
			return result;
		}

		public override int GetHashCode()
		{
			return _database.GetHashCode();
		}
	}

	internal class EventContextPair
	{
		private OnChangeEventHandler _eventHandler;

		private ExecutionContext _context;

		private SqlDependency _dependency;

		private SqlNotificationEventArgs _args;

		private static ContextCallback s_contextCallback = InvokeCallback;

		internal EventContextPair(OnChangeEventHandler eventHandler, SqlDependency dependency)
		{
			_eventHandler = eventHandler;
			_context = ExecutionContext.Capture();
			_dependency = dependency;
		}

		public override bool Equals(object value)
		{
			EventContextPair eventContextPair = (EventContextPair)value;
			bool result = false;
			if (eventContextPair == null)
			{
				result = false;
			}
			else if (this == eventContextPair)
			{
				result = true;
			}
			else if (_eventHandler == eventContextPair._eventHandler)
			{
				result = true;
			}
			return result;
		}

		public override int GetHashCode()
		{
			return _eventHandler.GetHashCode();
		}

		internal void Invoke(SqlNotificationEventArgs args)
		{
			_args = args;
			ExecutionContext.Run(_context, s_contextCallback, this);
		}

		private static void InvokeCallback(object eventContextPair)
		{
			EventContextPair eventContextPair2 = (EventContextPair)eventContextPair;
			eventContextPair2._eventHandler(eventContextPair2._dependency, eventContextPair2._args);
		}
	}

	private readonly string _id = Guid.NewGuid().ToString() + ";" + s_appDomainKey;

	private string _options;

	private int _timeout;

	private bool _dependencyFired;

	private List<EventContextPair> _eventList = new List<EventContextPair>();

	private object _eventHandlerLock = new object();

	private DateTime _expirationTime = DateTime.MaxValue;

	private List<string> _serverList = new List<string>();

	private static object s_startStopLock = new object();

	private static readonly string s_appDomainKey = Guid.NewGuid().ToString();

	private static Dictionary<string, Dictionary<IdentityUserNamePair, List<DatabaseServicePair>>> s_serverUserHash = new Dictionary<string, Dictionary<IdentityUserNamePair, List<DatabaseServicePair>>>(StringComparer.OrdinalIgnoreCase);

	private static SqlDependencyProcessDispatcher s_processDispatcher = null;

	private static readonly string s_assemblyName = typeof(SqlDependencyProcessDispatcher).Assembly.FullName;

	private static readonly string s_typeName = typeof(SqlDependencyProcessDispatcher).FullName;

	public bool HasChanges => _dependencyFired;

	public string Id => _id;

	internal static string AppDomainKey => s_appDomainKey;

	internal DateTime ExpirationTime => _expirationTime;

	internal string Options => _options;

	internal static SqlDependencyProcessDispatcher ProcessDispatcher => s_processDispatcher;

	internal int Timeout => _timeout;

	public event OnChangeEventHandler OnChange
	{
		add
		{
			if (value == null)
			{
				return;
			}
			SqlNotificationEventArgs e = null;
			lock (_eventHandlerLock)
			{
				if (_dependencyFired)
				{
					e = new SqlNotificationEventArgs(SqlNotificationType.Subscribe, SqlNotificationInfo.AlreadyChanged, SqlNotificationSource.Client);
				}
				else
				{
					EventContextPair item = new EventContextPair(value, this);
					if (_eventList.Contains(item))
					{
						throw SQL.SqlDependencyEventNoDuplicate();
					}
					_eventList.Add(item);
				}
			}
			if (e != null)
			{
				value(this, e);
			}
		}
		remove
		{
			if (value == null)
			{
				return;
			}
			EventContextPair item = new EventContextPair(value, this);
			lock (_eventHandlerLock)
			{
				int num = _eventList.IndexOf(item);
				if (0 <= num)
				{
					_eventList.RemoveAt(num);
				}
			}
		}
	}

	public SqlDependency()
		: this(null, null, 0)
	{
	}

	public SqlDependency(SqlCommand command)
		: this(command, null, 0)
	{
	}

	public SqlDependency(SqlCommand command, string options, int timeout)
	{
		if (timeout < 0)
		{
			throw SQL.InvalidSqlDependencyTimeout("timeout");
		}
		_timeout = timeout;
		if (options != null)
		{
			_options = options;
		}
		AddCommandInternal(command);
		SqlDependencyPerAppDomainDispatcher.SingletonInstance.AddDependencyEntry(this);
	}

	public void AddCommandDependency(SqlCommand command)
	{
		if (command == null)
		{
			throw ADP.ArgumentNull("command");
		}
		AddCommandInternal(command);
	}

	public static bool Start(string connectionString)
	{
		return Start(connectionString, null, useDefaults: true);
	}

	public static bool Start(string connectionString, string queue)
	{
		return Start(connectionString, queue, useDefaults: false);
	}

	internal static bool Start(string connectionString, string queue, bool useDefaults)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			if (connectionString == null)
			{
				throw ADP.ArgumentNull("connectionString");
			}
			throw ADP.Argument("connectionString");
		}
		if (!useDefaults && string.IsNullOrEmpty(queue))
		{
			useDefaults = true;
			queue = null;
		}
		bool errorOccurred = false;
		bool result = false;
		lock (s_startStopLock)
		{
			try
			{
				if (s_processDispatcher == null)
				{
					s_processDispatcher = SqlDependencyProcessDispatcher.SingletonProcessDispatcher;
				}
				if (useDefaults)
				{
					string server = null;
					DbConnectionPoolIdentity identity = null;
					string user = null;
					string database = null;
					string service = null;
					bool appDomainStart = false;
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						result = s_processDispatcher.StartWithDefault(connectionString, out server, out identity, out user, out database, ref service, s_appDomainKey, SqlDependencyPerAppDomainDispatcher.SingletonInstance, out errorOccurred, out appDomainStart);
					}
					finally
					{
						if (appDomainStart && !errorOccurred)
						{
							IdentityUserNamePair identityUser = new IdentityUserNamePair(identity, user);
							DatabaseServicePair databaseService = new DatabaseServicePair(database, service);
							if (!AddToServerUserHash(server, identityUser, databaseService))
							{
								try
								{
									Stop(connectionString, queue, useDefaults, startFailed: true);
								}
								catch (Exception e)
								{
									if (!ADP.IsCatchableExceptionType(e))
									{
										throw;
									}
									ADP.TraceExceptionWithoutRethrow(e);
								}
								throw SQL.SqlDependencyDuplicateStart();
							}
						}
					}
				}
				else
				{
					result = s_processDispatcher.Start(connectionString, queue, s_appDomainKey, SqlDependencyPerAppDomainDispatcher.SingletonInstance);
				}
			}
			catch (Exception e2)
			{
				if (!ADP.IsCatchableExceptionType(e2))
				{
					throw;
				}
				ADP.TraceExceptionWithoutRethrow(e2);
				throw;
			}
		}
		return result;
	}

	public static bool Stop(string connectionString)
	{
		return Stop(connectionString, null, useDefaults: true, startFailed: false);
	}

	public static bool Stop(string connectionString, string queue)
	{
		return Stop(connectionString, queue, useDefaults: false, startFailed: false);
	}

	internal static bool Stop(string connectionString, string queue, bool useDefaults, bool startFailed)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			if (connectionString == null)
			{
				throw ADP.ArgumentNull("connectionString");
			}
			throw ADP.Argument("connectionString");
		}
		if (!useDefaults && string.IsNullOrEmpty(queue))
		{
			useDefaults = true;
			queue = null;
		}
		bool result = false;
		lock (s_startStopLock)
		{
			if (s_processDispatcher != null)
			{
				try
				{
					string server = null;
					DbConnectionPoolIdentity identity = null;
					string user = null;
					string database = null;
					string queueService = null;
					if (useDefaults)
					{
						bool appDomainStop = false;
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
							result = s_processDispatcher.Stop(connectionString, out server, out identity, out user, out database, ref queueService, s_appDomainKey, out appDomainStop);
						}
						finally
						{
							if (appDomainStop && !startFailed)
							{
								IdentityUserNamePair identityUser = new IdentityUserNamePair(identity, user);
								DatabaseServicePair databaseService = new DatabaseServicePair(database, queueService);
								RemoveFromServerUserHash(server, identityUser, databaseService);
							}
						}
					}
					else
					{
						result = s_processDispatcher.Stop(connectionString, out server, out identity, out user, out database, ref queue, s_appDomainKey, out var _);
					}
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
		return result;
	}

	private static bool AddToServerUserHash(string server, IdentityUserNamePair identityUser, DatabaseServicePair databaseService)
	{
		bool result = false;
		lock (s_serverUserHash)
		{
			Dictionary<IdentityUserNamePair, List<DatabaseServicePair>> dictionary;
			if (!s_serverUserHash.ContainsKey(server))
			{
				dictionary = new Dictionary<IdentityUserNamePair, List<DatabaseServicePair>>();
				s_serverUserHash.Add(server, dictionary);
			}
			else
			{
				dictionary = s_serverUserHash[server];
			}
			List<DatabaseServicePair> list;
			if (!dictionary.ContainsKey(identityUser))
			{
				list = new List<DatabaseServicePair>();
				dictionary.Add(identityUser, list);
			}
			else
			{
				list = dictionary[identityUser];
			}
			if (!list.Contains(databaseService))
			{
				list.Add(databaseService);
				result = true;
			}
		}
		return result;
	}

	private static void RemoveFromServerUserHash(string server, IdentityUserNamePair identityUser, DatabaseServicePair databaseService)
	{
		lock (s_serverUserHash)
		{
			if (!s_serverUserHash.ContainsKey(server))
			{
				return;
			}
			Dictionary<IdentityUserNamePair, List<DatabaseServicePair>> dictionary = s_serverUserHash[server];
			if (!dictionary.ContainsKey(identityUser))
			{
				return;
			}
			List<DatabaseServicePair> list = dictionary[identityUser];
			int num = list.IndexOf(databaseService);
			if (num < 0)
			{
				return;
			}
			list.RemoveAt(num);
			if (list.Count == 0)
			{
				dictionary.Remove(identityUser);
				if (dictionary.Count == 0)
				{
					s_serverUserHash.Remove(server);
				}
			}
		}
	}

	internal static string GetDefaultComposedOptions(string server, string failoverServer, IdentityUserNamePair identityUser, string database)
	{
		lock (s_serverUserHash)
		{
			if (!s_serverUserHash.ContainsKey(server))
			{
				if (s_serverUserHash.Count == 0)
				{
					throw SQL.SqlDepDefaultOptionsButNoStart();
				}
				if (string.IsNullOrEmpty(failoverServer) || !s_serverUserHash.ContainsKey(failoverServer))
				{
					throw SQL.SqlDependencyNoMatchingServerStart();
				}
				server = failoverServer;
			}
			Dictionary<IdentityUserNamePair, List<DatabaseServicePair>> dictionary = s_serverUserHash[server];
			List<DatabaseServicePair> list = null;
			if (!dictionary.ContainsKey(identityUser))
			{
				if (dictionary.Count > 1)
				{
					throw SQL.SqlDependencyNoMatchingServerStart();
				}
				using Dictionary<IdentityUserNamePair, List<DatabaseServicePair>>.Enumerator enumerator = dictionary.GetEnumerator();
				if (enumerator.MoveNext())
				{
					list = enumerator.Current.Value;
				}
			}
			else
			{
				list = dictionary[identityUser];
			}
			DatabaseServicePair item = new DatabaseServicePair(database, null);
			DatabaseServicePair databaseServicePair = null;
			int num = list.IndexOf(item);
			if (num != -1)
			{
				databaseServicePair = list[num];
			}
			if (databaseServicePair != null)
			{
				database = FixupServiceOrDatabaseName(databaseServicePair.Database);
				string text = FixupServiceOrDatabaseName(databaseServicePair.Service);
				return "Service=" + text + ";Local Database=" + database;
			}
			if (list.Count == 1)
			{
				object[] array = list.ToArray();
				databaseServicePair = (DatabaseServicePair)array[0];
				string text2 = FixupServiceOrDatabaseName(databaseServicePair.Database);
				string text3 = FixupServiceOrDatabaseName(databaseServicePair.Service);
				return "Service=" + text3 + ";Local Database=" + text2;
			}
			throw SQL.SqlDependencyNoMatchingServerDatabaseStart();
		}
	}

	internal void AddToServerList(string server)
	{
		lock (_serverList)
		{
			int num = _serverList.BinarySearch(server, StringComparer.OrdinalIgnoreCase);
			if (0 > num)
			{
				num = ~num;
				_serverList.Insert(num, server);
			}
		}
	}

	internal bool ContainsServer(string server)
	{
		lock (_serverList)
		{
			return _serverList.Contains(server);
		}
	}

	internal string ComputeHashAndAddToDispatcher(SqlCommand command)
	{
		string commandHash = ComputeCommandHash(command.Connection.ConnectionString, command);
		return SqlDependencyPerAppDomainDispatcher.SingletonInstance.AddCommandEntry(commandHash, this);
	}

	internal void Invalidate(SqlNotificationType type, SqlNotificationInfo info, SqlNotificationSource source)
	{
		List<EventContextPair> list = null;
		lock (_eventHandlerLock)
		{
			if (_dependencyFired && SqlNotificationInfo.AlreadyChanged != info && SqlNotificationSource.Client != source)
			{
				if (!(ExpirationTime >= DateTime.UtcNow))
				{
				}
			}
			else
			{
				_dependencyFired = true;
				list = _eventList;
				_eventList = new List<EventContextPair>();
			}
		}
		if (list == null)
		{
			return;
		}
		foreach (EventContextPair item in list)
		{
			item.Invoke(new SqlNotificationEventArgs(type, info, source));
		}
	}

	internal void StartTimer(SqlNotificationRequest notificationRequest)
	{
		if (_expirationTime == DateTime.MaxValue)
		{
			int num = 432000;
			if (_timeout != 0)
			{
				num = _timeout;
			}
			if (notificationRequest != null && notificationRequest.Timeout < num && notificationRequest.Timeout != 0)
			{
				num = notificationRequest.Timeout;
			}
			_expirationTime = DateTime.UtcNow.AddSeconds(num);
			SqlDependencyPerAppDomainDispatcher.SingletonInstance.StartTimer(this);
		}
	}

	private void AddCommandInternal(SqlCommand cmd)
	{
		if (cmd == null)
		{
			return;
		}
		_ = cmd.Connection;
		if (cmd.Notification != null)
		{
			if (cmd._sqlDep == null || cmd._sqlDep != this)
			{
				throw SQL.SqlCommandHasExistingSqlNotificationRequest();
			}
			return;
		}
		bool flag = false;
		lock (_eventHandlerLock)
		{
			if (!_dependencyFired)
			{
				cmd.Notification = new SqlNotificationRequest
				{
					Timeout = _timeout
				};
				if (_options != null)
				{
					cmd.Notification.Options = _options;
				}
				cmd._sqlDep = this;
			}
			else if (_eventList.Count == 0)
			{
				flag = true;
			}
		}
		if (flag)
		{
			Invalidate(SqlNotificationType.Subscribe, SqlNotificationInfo.AlreadyChanged, SqlNotificationSource.Client);
		}
	}

	private string ComputeCommandHash(string connectionString, SqlCommand command)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("{0};{1}", connectionString, command.CommandText);
		for (int i = 0; i < command.Parameters.Count; i++)
		{
			object value = command.Parameters[i].Value;
			if (value == null || value == DBNull.Value)
			{
				stringBuilder.Append("; NULL");
				continue;
			}
			Type type = value.GetType();
			if (type == typeof(byte[]))
			{
				stringBuilder.Append(";");
				byte[] array = (byte[])value;
				for (int j = 0; j < array.Length; j++)
				{
					stringBuilder.Append(array[j].ToString("x2", CultureInfo.InvariantCulture));
				}
			}
			else if (type == typeof(char[]))
			{
				stringBuilder.Append((char[])value);
			}
			else if (type == typeof(XmlReader))
			{
				stringBuilder.Append(";");
				stringBuilder.Append(Guid.NewGuid().ToString());
			}
			else
			{
				stringBuilder.Append(";");
				stringBuilder.Append(value.ToString());
			}
		}
		return stringBuilder.ToString();
	}

	internal static string FixupServiceOrDatabaseName(string name)
	{
		if (!string.IsNullOrEmpty(name))
		{
			return "\"" + name.Replace("\"", "\"\"") + "\"";
		}
		return name;
	}
}
