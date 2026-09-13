using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Threading;
using System.Xml;

internal class SqlDependencyProcessDispatcher : MarshalByRefObject
{
	private class SqlConnectionContainer
	{
		private SqlConnection _con;

		private SqlCommand _com;

		private SqlParameter _conversationGuidParam;

		private SqlParameter _timeoutParam;

		private SqlConnectionContainerHashHelper _hashHelper;

		private string _queue;

		private string _receiveQuery;

		private string _beginConversationQuery;

		private string _endConversationQuery;

		private string _concatQuery;

		private readonly int _defaultWaitforTimeout = 60000;

		private string _escapedQueueName;

		private string _sprocName;

		private string _dialogHandle;

		private string _cachedServer;

		private string _cachedDatabase;

		private volatile bool _errorState;

		private volatile bool _stop;

		private volatile bool _stopped;

		private volatile bool _serviceQueueCreated;

		private int _startCount;

		private Timer _retryTimer;

		private Dictionary<string, int> _appDomainKeyHash;

		internal string Database
		{
			get
			{
				if (_cachedDatabase == null)
				{
					_cachedDatabase = _con.Database;
				}
				return _cachedDatabase;
			}
		}

		internal SqlConnectionContainerHashHelper HashHelper => _hashHelper;

		internal bool InErrorState => _errorState;

		internal string Queue => _queue;

		internal string Server => _cachedServer;

		internal SqlConnectionContainer(SqlConnectionContainerHashHelper hashHelper, string appDomainKey, bool useDefaults)
		{
			bool flag = false;
			try
			{
				_hashHelper = hashHelper;
				string text = null;
				if (useDefaults)
				{
					text = Guid.NewGuid().ToString();
					_queue = "SqlQueryNotificationService-" + text;
					_hashHelper.ConnectionStringBuilder.ApplicationName = _queue;
				}
				else
				{
					_queue = _hashHelper.Queue;
				}
				_con = new SqlConnection(_hashHelper.ConnectionStringBuilder.ConnectionString);
				_ = (SqlConnectionString)_con.ConnectionOptions;
				_con.Open();
				_cachedServer = _con.DataSource;
				_escapedQueueName = SqlConnection.FixupDatabaseTransactionName(_queue);
				_appDomainKeyHash = new Dictionary<string, int>();
				_com = new SqlCommand
				{
					Connection = _con,
					CommandText = "select is_broker_enabled from sys.databases where database_id=db_id()"
				};
				if (!(bool)_com.ExecuteScalar())
				{
					throw SQL.SqlDependencyDatabaseBrokerDisabled();
				}
				_conversationGuidParam = new SqlParameter("@p1", SqlDbType.UniqueIdentifier);
				_timeoutParam = new SqlParameter("@p2", SqlDbType.Int)
				{
					Value = 0
				};
				_com.Parameters.Add(_timeoutParam);
				flag = true;
				_receiveQuery = "WAITFOR(RECEIVE TOP (1) message_type_name, conversation_handle, cast(message_body AS XML) as message_body from " + _escapedQueueName + "), TIMEOUT @p2;";
				if (useDefaults)
				{
					_sprocName = SqlConnection.FixupDatabaseTransactionName("SqlQueryNotificationStoredProcedure-" + text);
					CreateQueueAndService(restart: false);
				}
				else
				{
					_com.CommandText = _receiveQuery;
					_endConversationQuery = "END CONVERSATION @p1; ";
					_concatQuery = _endConversationQuery + _receiveQuery;
				}
				IncrementStartCount(appDomainKey, out var _);
				SynchronouslyQueryServiceBrokerQueue();
				_timeoutParam.Value = _defaultWaitforTimeout;
				AsynchronouslyQueryServiceBrokerQueue();
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableExceptionType(e))
				{
					throw;
				}
				ADP.TraceExceptionWithoutRethrow(e);
				if (flag)
				{
					TearDownAndDispose();
				}
				else
				{
					if (_com != null)
					{
						_com.Dispose();
						_com = null;
					}
					if (_con != null)
					{
						_con.Dispose();
						_con = null;
					}
				}
				throw;
			}
		}

		internal bool AppDomainUnload(string appDomainKey)
		{
			lock (_appDomainKeyHash)
			{
				if (_appDomainKeyHash.ContainsKey(appDomainKey))
				{
					int num = _appDomainKeyHash[appDomainKey];
					bool appDomainStop = false;
					while (num > 0)
					{
						Stop(appDomainKey, out appDomainStop);
						num--;
					}
				}
			}
			return _stopped;
		}

		private void AsynchronouslyQueryServiceBrokerQueue()
		{
			AsyncCallback callback = AsyncResultCallback;
			_com.BeginExecuteReader(CommandBehavior.Default, callback, null);
		}

		private void AsyncResultCallback(IAsyncResult asyncResult)
		{
			try
			{
				using (SqlDataReader reader = _com.EndExecuteReader(asyncResult))
				{
					ProcessNotificationResults(reader);
				}
				if (!_stop)
				{
					AsynchronouslyQueryServiceBrokerQueue();
				}
				else
				{
					TearDownAndDispose();
				}
			}
			catch (Exception e)
			{
				if (!ADP.IsCatchableExceptionType(e))
				{
					_errorState = true;
					throw;
				}
				if (!_stop)
				{
					ADP.TraceExceptionWithoutRethrow(e);
				}
				if (_stop)
				{
					TearDownAndDispose();
					return;
				}
				_errorState = true;
				Restart(null);
			}
		}

		private void CreateQueueAndService(bool restart)
		{
			SqlCommand sqlCommand = new SqlCommand
			{
				Connection = _con
			};
			SqlTransaction sqlTransaction = null;
			try
			{
				sqlTransaction = (sqlCommand.Transaction = _con.BeginTransaction());
				string text = SqlServerEscapeHelper.MakeStringLiteral(_queue);
				sqlCommand.CommandText = "CREATE PROCEDURE " + _sprocName + " AS BEGIN BEGIN TRANSACTION; RECEIVE TOP(0) conversation_handle FROM " + _escapedQueueName + "; IF (SELECT COUNT(*) FROM " + _escapedQueueName + " WHERE message_type_name = 'http://schemas.microsoft.com/SQL/ServiceBroker/DialogTimer') > 0 BEGIN if ((SELECT COUNT(*) FROM sys.services WHERE name = " + text + ") > 0)   DROP SERVICE " + _escapedQueueName + "; if (OBJECT_ID(" + text + ", 'SQ') IS NOT NULL)   DROP QUEUE " + _escapedQueueName + "; DROP PROCEDURE " + _sprocName + "; END COMMIT TRANSACTION; END";
				if (!restart)
				{
					sqlCommand.ExecuteNonQuery();
				}
				else
				{
					try
					{
						sqlCommand.ExecuteNonQuery();
					}
					catch (Exception e)
					{
						if (!ADP.IsCatchableExceptionType(e))
						{
							throw;
						}
						ADP.TraceExceptionWithoutRethrow(e);
						try
						{
							if (sqlTransaction != null)
							{
								sqlTransaction.Rollback();
								sqlTransaction = null;
							}
						}
						catch (Exception e2)
						{
							if (!ADP.IsCatchableExceptionType(e2))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(e2);
						}
					}
					if (sqlTransaction == null)
					{
						sqlTransaction = (sqlCommand.Transaction = _con.BeginTransaction());
					}
				}
				sqlCommand.CommandText = "IF OBJECT_ID(" + text + ", 'SQ') IS NULL BEGIN CREATE QUEUE " + _escapedQueueName + " WITH ACTIVATION (PROCEDURE_NAME=" + _sprocName + ", MAX_QUEUE_READERS=1, EXECUTE AS OWNER); END; IF (SELECT COUNT(*) FROM sys.services WHERE NAME=" + text + ") = 0 BEGIN CREATE SERVICE " + _escapedQueueName + " ON QUEUE " + _escapedQueueName + " ([http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]); IF (SELECT COUNT(*) FROM sys.database_principals WHERE name='sql_dependency_subscriber' AND type='R') <> 0 BEGIN GRANT SEND ON SERVICE::" + _escapedQueueName + " TO sql_dependency_subscriber; END;  END; BEGIN DIALOG @dialog_handle FROM SERVICE " + _escapedQueueName + " TO SERVICE " + text;
				SqlParameter sqlParameter = new SqlParameter
				{
					ParameterName = "@dialog_handle",
					DbType = DbType.Guid,
					Direction = ParameterDirection.Output
				};
				sqlCommand.Parameters.Add(sqlParameter);
				sqlCommand.ExecuteNonQuery();
				_dialogHandle = ((Guid)sqlParameter.Value/*cast due to .constrained prefix*/).ToString();
				_beginConversationQuery = "BEGIN CONVERSATION TIMER ('" + _dialogHandle + "') TIMEOUT = 120; " + _receiveQuery;
				_com.CommandText = _beginConversationQuery;
				_endConversationQuery = "END CONVERSATION @p1; ";
				_concatQuery = _endConversationQuery + _com.CommandText;
				sqlTransaction.Commit();
				sqlTransaction = null;
				_serviceQueueCreated = true;
			}
			finally
			{
				if (sqlTransaction != null)
				{
					try
					{
						sqlTransaction.Rollback();
						sqlTransaction = null;
					}
					catch (Exception e3)
					{
						if (!ADP.IsCatchableExceptionType(e3))
						{
							throw;
						}
						ADP.TraceExceptionWithoutRethrow(e3);
					}
				}
			}
		}

		internal void IncrementStartCount(string appDomainKey, out bool appDomainStart)
		{
			appDomainStart = false;
			Interlocked.Increment(ref _startCount);
			lock (_appDomainKeyHash)
			{
				if (_appDomainKeyHash.ContainsKey(appDomainKey))
				{
					_appDomainKeyHash[appDomainKey] += 1;
					return;
				}
				_appDomainKeyHash[appDomainKey] = 1;
				appDomainStart = true;
			}
		}

		private void ProcessNotificationResults(SqlDataReader reader)
		{
			Guid guid = Guid.Empty;
			try
			{
				if (_stop)
				{
					return;
				}
				while (reader.Read())
				{
					string strA = reader.GetString(0);
					guid = reader.GetGuid(1);
					if (string.Compare(strA, "http://schemas.microsoft.com/SQL/Notifications/QueryNotification", StringComparison.OrdinalIgnoreCase) == 0)
					{
						SqlXml sqlXml = reader.GetSqlXml(2);
						if (sqlXml == null)
						{
							continue;
						}
						SqlNotification sqlNotification = SqlNotificationParser.ProcessMessage(sqlXml);
						if (sqlNotification == null)
						{
							continue;
						}
						string key = sqlNotification.Key;
						int num = key.IndexOf(';');
						if (num < 0)
						{
							continue;
						}
						string key2 = key.Substring(0, num);
						SqlDependencyPerAppDomainDispatcher sqlDependencyPerAppDomainDispatcher;
						lock (s_staticInstance._sqlDependencyPerAppDomainDispatchers)
						{
							sqlDependencyPerAppDomainDispatcher = s_staticInstance._sqlDependencyPerAppDomainDispatchers[key2];
						}
						if (sqlDependencyPerAppDomainDispatcher == null)
						{
							continue;
						}
						try
						{
							sqlDependencyPerAppDomainDispatcher.InvalidateCommandID(sqlNotification);
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
						guid = Guid.Empty;
					}
				}
			}
			finally
			{
				if (guid == Guid.Empty)
				{
					_com.CommandText = _beginConversationQuery ?? _receiveQuery;
					if (_com.Parameters.Count > 1)
					{
						_com.Parameters.Remove(_conversationGuidParam);
					}
				}
				else
				{
					_com.CommandText = _concatQuery;
					_conversationGuidParam.Value = guid;
					if (_com.Parameters.Count == 1)
					{
						_com.Parameters.Add(_conversationGuidParam);
					}
				}
			}
		}

		private void Restart(object unused)
		{
			try
			{
				lock (this)
				{
					if (!_stop)
					{
						try
						{
							_con.Close();
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
				lock (this)
				{
					if (!_stop)
					{
						_con.Open();
					}
				}
				lock (this)
				{
					if (!_stop && _serviceQueueCreated)
					{
						bool flag = false;
						try
						{
							CreateQueueAndService(restart: true);
						}
						catch (Exception e2)
						{
							if (!ADP.IsCatchableExceptionType(e2))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(e2);
							flag = true;
						}
						if (flag)
						{
							s_staticInstance.Invalidate(Server, new SqlNotification(SqlNotificationInfo.Error, SqlNotificationSource.Client, SqlNotificationType.Change, null));
						}
					}
				}
				lock (this)
				{
					if (!_stop)
					{
						_timeoutParam.Value = 0;
						SynchronouslyQueryServiceBrokerQueue();
						_timeoutParam.Value = _defaultWaitforTimeout;
						AsynchronouslyQueryServiceBrokerQueue();
						_errorState = false;
						Timer retryTimer = _retryTimer;
						if (retryTimer != null)
						{
							_retryTimer = null;
							retryTimer.Dispose();
						}
					}
				}
				if (_stop)
				{
					TearDownAndDispose();
				}
			}
			catch (Exception e3)
			{
				if (!ADP.IsCatchableExceptionType(e3))
				{
					throw;
				}
				ADP.TraceExceptionWithoutRethrow(e3);
				try
				{
					s_staticInstance.Invalidate(Server, new SqlNotification(SqlNotificationInfo.Error, SqlNotificationSource.Client, SqlNotificationType.Change, null));
				}
				catch (Exception e4)
				{
					if (!ADP.IsCatchableExceptionType(e4))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(e4);
				}
				try
				{
					_con.Close();
				}
				catch (Exception e5)
				{
					if (!ADP.IsCatchableExceptionType(e5))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(e5);
				}
				if (!_stop)
				{
					_retryTimer = new Timer(Restart, null, _defaultWaitforTimeout, -1);
				}
			}
		}

		internal bool Stop(string appDomainKey, out bool appDomainStop)
		{
			appDomainStop = false;
			if (appDomainKey != null)
			{
				lock (_appDomainKeyHash)
				{
					if (_appDomainKeyHash.ContainsKey(appDomainKey))
					{
						int num = _appDomainKeyHash[appDomainKey];
						if (num > 0)
						{
							_appDomainKeyHash[appDomainKey] = num - 1;
						}
						if (1 == num)
						{
							_appDomainKeyHash.Remove(appDomainKey);
							appDomainStop = true;
						}
					}
				}
			}
			if (Interlocked.Decrement(ref _startCount) == 0)
			{
				lock (this)
				{
					try
					{
						_com.Cancel();
					}
					catch (Exception e)
					{
						if (!ADP.IsCatchableExceptionType(e))
						{
							throw;
						}
						ADP.TraceExceptionWithoutRethrow(e);
					}
					_stop = true;
				}
				Stopwatch stopwatch = Stopwatch.StartNew();
				while (true)
				{
					lock (this)
					{
						if (!_stopped)
						{
							if (!_errorState && stopwatch.Elapsed.Seconds < 30)
							{
								goto IL_0127;
							}
							Timer retryTimer = _retryTimer;
							_retryTimer = null;
							retryTimer?.Dispose();
							TearDownAndDispose();
						}
					}
					break;
					IL_0127:
					Thread.Sleep(1);
				}
			}
			return _stopped;
		}

		private void SynchronouslyQueryServiceBrokerQueue()
		{
			using SqlDataReader reader = _com.ExecuteReader();
			ProcessNotificationResults(reader);
		}

		private void TearDownAndDispose()
		{
			lock (this)
			{
				try
				{
					if (_con.State == ConnectionState.Closed || ConnectionState.Broken == _con.State)
					{
						return;
					}
					if (_com.Parameters.Count > 1)
					{
						try
						{
							_com.CommandText = _endConversationQuery;
							_com.Parameters.Remove(_timeoutParam);
							_com.ExecuteNonQuery();
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
					if (!_serviceQueueCreated || _errorState)
					{
						return;
					}
					_com.CommandText = "BEGIN TRANSACTION; DROP SERVICE " + _escapedQueueName + "; DROP QUEUE " + _escapedQueueName + "; DROP PROCEDURE " + _sprocName + "; COMMIT TRANSACTION;";
					try
					{
						_com.ExecuteNonQuery();
					}
					catch (Exception e2)
					{
						if (!ADP.IsCatchableExceptionType(e2))
						{
							throw;
						}
						ADP.TraceExceptionWithoutRethrow(e2);
					}
				}
				finally
				{
					_stopped = true;
					_con.Dispose();
				}
			}
		}
	}

	private class SqlNotificationParser
	{
		[Flags]
		private enum MessageAttributes
		{
			None = 0,
			Type = 1,
			Source = 2,
			Info = 4,
			All = 7
		}

		private const string RootNode = "QueryNotification";

		private const string MessageNode = "Message";

		private const string InfoAttribute = "info";

		private const string SourceAttribute = "source";

		private const string TypeAttribute = "type";

		internal static SqlNotification ProcessMessage(SqlXml xmlMessage)
		{
			using XmlReader xmlReader = xmlMessage.CreateReader();
			_ = string.Empty;
			MessageAttributes messageAttributes = MessageAttributes.None;
			SqlNotificationType type = SqlNotificationType.Unknown;
			SqlNotificationInfo info = SqlNotificationInfo.Unknown;
			SqlNotificationSource source = SqlNotificationSource.Unknown;
			string key = string.Empty;
			xmlReader.Read();
			if (XmlNodeType.Element == xmlReader.NodeType && "QueryNotification" == xmlReader.LocalName && 3 <= xmlReader.AttributeCount)
			{
				while (MessageAttributes.All != messageAttributes && xmlReader.MoveToNextAttribute())
				{
					try
					{
						switch (xmlReader.LocalName)
						{
						case "type":
							try
							{
								SqlNotificationType sqlNotificationType = (SqlNotificationType)Enum.Parse(typeof(SqlNotificationType), xmlReader.Value, ignoreCase: true);
								if (Enum.IsDefined(typeof(SqlNotificationType), sqlNotificationType))
								{
									type = sqlNotificationType;
								}
							}
							catch (Exception e2)
							{
								if (!ADP.IsCatchableExceptionType(e2))
								{
									throw;
								}
								ADP.TraceExceptionWithoutRethrow(e2);
							}
							messageAttributes |= MessageAttributes.Type;
							break;
						case "source":
							try
							{
								SqlNotificationSource sqlNotificationSource = (SqlNotificationSource)Enum.Parse(typeof(SqlNotificationSource), xmlReader.Value, ignoreCase: true);
								if (Enum.IsDefined(typeof(SqlNotificationSource), sqlNotificationSource))
								{
									source = sqlNotificationSource;
								}
							}
							catch (Exception e3)
							{
								if (!ADP.IsCatchableExceptionType(e3))
								{
									throw;
								}
								ADP.TraceExceptionWithoutRethrow(e3);
							}
							messageAttributes |= MessageAttributes.Source;
							break;
						case "info":
							try
							{
								string value = xmlReader.Value;
								switch (value)
								{
								case "set options":
									info = SqlNotificationInfo.Options;
									break;
								case "previous invalid":
									info = SqlNotificationInfo.PreviousFire;
									break;
								case "query template limit":
									info = SqlNotificationInfo.TemplateLimit;
									break;
								default:
								{
									SqlNotificationInfo sqlNotificationInfo = (SqlNotificationInfo)Enum.Parse(typeof(SqlNotificationInfo), value, ignoreCase: true);
									if (Enum.IsDefined(typeof(SqlNotificationInfo), sqlNotificationInfo))
									{
										info = sqlNotificationInfo;
									}
									break;
								}
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
							messageAttributes |= MessageAttributes.Info;
							break;
						}
					}
					catch (ArgumentException e4)
					{
						ADP.TraceExceptionWithoutRethrow(e4);
						return null;
					}
				}
				if (MessageAttributes.All != messageAttributes)
				{
					return null;
				}
				if (!xmlReader.Read())
				{
					return null;
				}
				if (XmlNodeType.Element != xmlReader.NodeType || string.Compare(xmlReader.LocalName, "Message", StringComparison.OrdinalIgnoreCase) != 0)
				{
					return null;
				}
				if (!xmlReader.Read())
				{
					return null;
				}
				if (xmlReader.NodeType != XmlNodeType.Text)
				{
					return null;
				}
				using (XmlTextReader xmlTextReader = new XmlTextReader(xmlReader.Value, XmlNodeType.Element, null))
				{
					if (!xmlTextReader.Read())
					{
						return null;
					}
					if (xmlTextReader.NodeType != XmlNodeType.Text)
					{
						return null;
					}
					key = xmlTextReader.Value;
					xmlTextReader.Close();
				}
				return new SqlNotification(info, source, type, key);
			}
			return null;
		}
	}

	private class SqlConnectionContainerHashHelper
	{
		private DbConnectionPoolIdentity _identity;

		private string _connectionString;

		private string _queue;

		private SqlConnectionStringBuilder _connectionStringBuilder;

		internal SqlConnectionStringBuilder ConnectionStringBuilder => _connectionStringBuilder;

		internal DbConnectionPoolIdentity Identity => _identity;

		internal string Queue => _queue;

		internal SqlConnectionContainerHashHelper(DbConnectionPoolIdentity identity, string connectionString, string queue, SqlConnectionStringBuilder connectionStringBuilder)
		{
			_identity = identity;
			_connectionString = connectionString;
			_queue = queue;
			_connectionStringBuilder = connectionStringBuilder;
		}

		public override bool Equals(object value)
		{
			SqlConnectionContainerHashHelper sqlConnectionContainerHashHelper = (SqlConnectionContainerHashHelper)value;
			bool flag = false;
			if (sqlConnectionContainerHashHelper == null)
			{
				return false;
			}
			if (this == sqlConnectionContainerHashHelper)
			{
				return true;
			}
			if ((_identity != null && sqlConnectionContainerHashHelper._identity == null) || (_identity == null && sqlConnectionContainerHashHelper._identity != null))
			{
				return false;
			}
			if (_identity == null && sqlConnectionContainerHashHelper._identity == null)
			{
				if (sqlConnectionContainerHashHelper._connectionString == _connectionString && string.Equals(sqlConnectionContainerHashHelper._queue, _queue, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				return false;
			}
			if (sqlConnectionContainerHashHelper._identity.Equals(_identity) && sqlConnectionContainerHashHelper._connectionString == _connectionString && string.Equals(sqlConnectionContainerHashHelper._queue, _queue, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = 0;
			if (_identity != null)
			{
				num = _identity.GetHashCode();
			}
			if (_queue != null)
			{
				return _connectionString.GetHashCode() + _queue.GetHashCode() + num;
			}
			return _connectionString.GetHashCode() + num;
		}
	}

	private static SqlDependencyProcessDispatcher s_staticInstance = new SqlDependencyProcessDispatcher(null);

	private Dictionary<SqlConnectionContainerHashHelper, SqlConnectionContainer> _connectionContainers;

	private Dictionary<string, SqlDependencyPerAppDomainDispatcher> _sqlDependencyPerAppDomainDispatchers;

	internal static SqlDependencyProcessDispatcher SingletonProcessDispatcher => s_staticInstance;

	private SqlDependencyProcessDispatcher(object dummyVariable)
	{
		_connectionContainers = new Dictionary<SqlConnectionContainerHashHelper, SqlConnectionContainer>();
		_sqlDependencyPerAppDomainDispatchers = new Dictionary<string, SqlDependencyPerAppDomainDispatcher>();
	}

	public SqlDependencyProcessDispatcher()
	{
	}

	private static SqlConnectionContainerHashHelper GetHashHelper(string connectionString, out SqlConnectionStringBuilder connectionStringBuilder, out DbConnectionPoolIdentity identity, out string user, string queue)
	{
		connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
		{
			Pooling = false,
			Enlist = false,
			ConnectRetryCount = 0
		};
		if (queue != null)
		{
			connectionStringBuilder.ApplicationName = queue;
		}
		if (connectionStringBuilder.IntegratedSecurity)
		{
			identity = DbConnectionPoolIdentity.GetCurrent();
			user = null;
		}
		else
		{
			identity = null;
			user = connectionStringBuilder.UserID;
		}
		return new SqlConnectionContainerHashHelper(identity, connectionStringBuilder.ConnectionString, queue, connectionStringBuilder);
	}

	public override object InitializeLifetimeService()
	{
		return null;
	}

	private void Invalidate(string server, SqlNotification sqlNotification)
	{
		lock (_sqlDependencyPerAppDomainDispatchers)
		{
			foreach (KeyValuePair<string, SqlDependencyPerAppDomainDispatcher> sqlDependencyPerAppDomainDispatcher in _sqlDependencyPerAppDomainDispatchers)
			{
				SqlDependencyPerAppDomainDispatcher value = sqlDependencyPerAppDomainDispatcher.Value;
				try
				{
					value.InvalidateServer(server, sqlNotification);
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
	}

	internal void QueueAppDomainUnloading(string appDomainKey)
	{
		ThreadPool.QueueUserWorkItem(AppDomainUnloading, appDomainKey);
	}

	private void AppDomainUnloading(object state)
	{
		string text = (string)state;
		lock (_connectionContainers)
		{
			List<SqlConnectionContainerHashHelper> list = new List<SqlConnectionContainerHashHelper>();
			foreach (KeyValuePair<SqlConnectionContainerHashHelper, SqlConnectionContainer> connectionContainer in _connectionContainers)
			{
				SqlConnectionContainer value = connectionContainer.Value;
				if (value.AppDomainUnload(text))
				{
					list.Add(value.HashHelper);
				}
			}
			foreach (SqlConnectionContainerHashHelper item in list)
			{
				_connectionContainers.Remove(item);
			}
		}
		lock (_sqlDependencyPerAppDomainDispatchers)
		{
			_sqlDependencyPerAppDomainDispatchers.Remove(text);
		}
	}

	internal bool StartWithDefault(string connectionString, out string server, out DbConnectionPoolIdentity identity, out string user, out string database, ref string service, string appDomainKey, SqlDependencyPerAppDomainDispatcher dispatcher, out bool errorOccurred, out bool appDomainStart)
	{
		return Start(connectionString, out server, out identity, out user, out database, ref service, appDomainKey, dispatcher, out errorOccurred, out appDomainStart, useDefaults: true);
	}

	internal bool Start(string connectionString, string queue, string appDomainKey, SqlDependencyPerAppDomainDispatcher dispatcher)
	{
		string server;
		DbConnectionPoolIdentity identity;
		bool errorOccurred;
		return Start(connectionString, out server, out identity, out server, out server, ref queue, appDomainKey, dispatcher, out errorOccurred, out errorOccurred, useDefaults: false);
	}

	private bool Start(string connectionString, out string server, out DbConnectionPoolIdentity identity, out string user, out string database, ref string queueService, string appDomainKey, SqlDependencyPerAppDomainDispatcher dispatcher, out bool errorOccurred, out bool appDomainStart, bool useDefaults)
	{
		server = null;
		identity = null;
		user = null;
		database = null;
		errorOccurred = false;
		appDomainStart = false;
		lock (_sqlDependencyPerAppDomainDispatchers)
		{
			if (!_sqlDependencyPerAppDomainDispatchers.ContainsKey(appDomainKey))
			{
				_sqlDependencyPerAppDomainDispatchers[appDomainKey] = dispatcher;
			}
		}
		SqlConnectionStringBuilder connectionStringBuilder;
		SqlConnectionContainerHashHelper hashHelper = GetHashHelper(connectionString, out connectionStringBuilder, out identity, out user, queueService);
		bool result = false;
		SqlConnectionContainer sqlConnectionContainer = null;
		lock (_connectionContainers)
		{
			if (!_connectionContainers.ContainsKey(hashHelper))
			{
				sqlConnectionContainer = new SqlConnectionContainer(hashHelper, appDomainKey, useDefaults);
				_connectionContainers.Add(hashHelper, sqlConnectionContainer);
				result = true;
				appDomainStart = true;
			}
			else
			{
				sqlConnectionContainer = _connectionContainers[hashHelper];
				if (sqlConnectionContainer.InErrorState)
				{
					errorOccurred = true;
				}
				else
				{
					sqlConnectionContainer.IncrementStartCount(appDomainKey, out appDomainStart);
				}
			}
		}
		if (useDefaults && !errorOccurred)
		{
			server = sqlConnectionContainer.Server;
			database = sqlConnectionContainer.Database;
			queueService = sqlConnectionContainer.Queue;
		}
		return result;
	}

	internal bool Stop(string connectionString, out string server, out DbConnectionPoolIdentity identity, out string user, out string database, ref string queueService, string appDomainKey, out bool appDomainStop)
	{
		server = null;
		identity = null;
		user = null;
		database = null;
		appDomainStop = false;
		SqlConnectionStringBuilder connectionStringBuilder;
		SqlConnectionContainerHashHelper hashHelper = GetHashHelper(connectionString, out connectionStringBuilder, out identity, out user, queueService);
		bool result = false;
		lock (_connectionContainers)
		{
			if (_connectionContainers.ContainsKey(hashHelper))
			{
				SqlConnectionContainer sqlConnectionContainer = _connectionContainers[hashHelper];
				server = sqlConnectionContainer.Server;
				database = sqlConnectionContainer.Database;
				queueService = sqlConnectionContainer.Queue;
				if (sqlConnectionContainer.Stop(appDomainKey, out appDomainStop))
				{
					result = true;
					_connectionContainers.Remove(hashHelper);
				}
			}
		}
		return result;
	}
}
