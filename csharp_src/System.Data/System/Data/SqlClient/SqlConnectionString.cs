using System.Collections.Generic;
using System.Data.Common;

namespace System.Data.SqlClient;

internal sealed class SqlConnectionString : DbConnectionOptions
{
	internal static class DEFAULT
	{
		internal const ApplicationIntent ApplicationIntent = ApplicationIntent.ReadWrite;

		internal const string Application_Name = "Core .Net SqlClient Data Provider";

		internal const string AttachDBFilename = "";

		internal const int Connect_Timeout = 15;

		internal const string Current_Language = "";

		internal const string Data_Source = "";

		internal const bool Encrypt = false;

		internal const bool Enlist = true;

		internal const string FailoverPartner = "";

		internal const string Initial_Catalog = "";

		internal const bool Integrated_Security = false;

		internal const int Load_Balance_Timeout = 0;

		internal const bool MARS = false;

		internal const int Max_Pool_Size = 100;

		internal const int Min_Pool_Size = 0;

		internal const bool MultiSubnetFailover = false;

		internal const int Packet_Size = 8000;

		internal const string Password = "";

		internal const bool Persist_Security_Info = false;

		internal const bool Pooling = true;

		internal const bool TrustServerCertificate = false;

		internal const string Type_System_Version = "";

		internal const string User_ID = "";

		internal const bool User_Instance = false;

		internal const bool Replication = false;

		internal const int Connect_Retry_Count = 1;

		internal const int Connect_Retry_Interval = 10;
	}

	internal static class KEY
	{
		internal const string ApplicationIntent = "applicationintent";

		internal const string Application_Name = "application name";

		internal const string AsynchronousProcessing = "asynchronous processing";

		internal const string AttachDBFilename = "attachdbfilename";

		internal const string Connect_Timeout = "connect timeout";

		internal const string Connection_Reset = "connection reset";

		internal const string Context_Connection = "context connection";

		internal const string Current_Language = "current language";

		internal const string Data_Source = "data source";

		internal const string Encrypt = "encrypt";

		internal const string Enlist = "enlist";

		internal const string FailoverPartner = "failover partner";

		internal const string Initial_Catalog = "initial catalog";

		internal const string Integrated_Security = "integrated security";

		internal const string Load_Balance_Timeout = "load balance timeout";

		internal const string MARS = "multipleactiveresultsets";

		internal const string Max_Pool_Size = "max pool size";

		internal const string Min_Pool_Size = "min pool size";

		internal const string MultiSubnetFailover = "multisubnetfailover";

		internal const string Network_Library = "network library";

		internal const string Packet_Size = "packet size";

		internal const string Password = "password";

		internal const string Persist_Security_Info = "persist security info";

		internal const string Pooling = "pooling";

		internal const string TransactionBinding = "transaction binding";

		internal const string TrustServerCertificate = "trustservercertificate";

		internal const string Type_System_Version = "type system version";

		internal const string User_ID = "user id";

		internal const string User_Instance = "user instance";

		internal const string Workstation_Id = "workstation id";

		internal const string Replication = "replication";

		internal const string Connect_Retry_Count = "connectretrycount";

		internal const string Connect_Retry_Interval = "connectretryinterval";
	}

	private static class SYNONYM
	{
		internal const string APP = "app";

		internal const string Async = "async";

		internal const string EXTENDED_PROPERTIES = "extended properties";

		internal const string INITIAL_FILE_NAME = "initial file name";

		internal const string CONNECTION_TIMEOUT = "connection timeout";

		internal const string TIMEOUT = "timeout";

		internal const string LANGUAGE = "language";

		internal const string ADDR = "addr";

		internal const string ADDRESS = "address";

		internal const string SERVER = "server";

		internal const string NETWORK_ADDRESS = "network address";

		internal const string DATABASE = "database";

		internal const string TRUSTED_CONNECTION = "trusted_connection";

		internal const string Connection_Lifetime = "connection lifetime";

		internal const string NET = "net";

		internal const string NETWORK = "network";

		internal const string Pwd = "pwd";

		internal const string PERSISTSECURITYINFO = "persistsecurityinfo";

		internal const string UID = "uid";

		internal const string User = "user";

		internal const string WSID = "wsid";
	}

	internal enum TypeSystem
	{
		Latest = 2008,
		SQLServer2000 = 2000,
		SQLServer2005 = 2005,
		SQLServer2008 = 2008,
		SQLServer2012 = 2012
	}

	internal static class TYPESYSTEMVERSION
	{
		internal const string Latest = "Latest";

		internal const string SQL_Server_2000 = "SQL Server 2000";

		internal const string SQL_Server_2005 = "SQL Server 2005";

		internal const string SQL_Server_2008 = "SQL Server 2008";

		internal const string SQL_Server_2012 = "SQL Server 2012";
	}

	internal enum TransactionBindingEnum
	{
		ImplicitUnbind,
		ExplicitUnbind
	}

	internal static class TRANSACTIONBINDING
	{
		internal const string ImplicitUnbind = "Implicit Unbind";

		internal const string ExplicitUnbind = "Explicit Unbind";
	}

	internal const int SynonymCount = 18;

	internal const int DeprecatedSynonymCount = 3;

	private static Dictionary<string, string> s_sqlClientSynonyms;

	private readonly bool _integratedSecurity;

	private readonly bool _encrypt;

	private readonly bool _trustServerCertificate;

	private readonly bool _enlist;

	private readonly bool _mars;

	private readonly bool _persistSecurityInfo;

	private readonly bool _pooling;

	private readonly bool _replication;

	private readonly bool _userInstance;

	private readonly bool _multiSubnetFailover;

	private readonly int _connectTimeout;

	private readonly int _loadBalanceTimeout;

	private readonly int _maxPoolSize;

	private readonly int _minPoolSize;

	private readonly int _packetSize;

	private readonly int _connectRetryCount;

	private readonly int _connectRetryInterval;

	private readonly ApplicationIntent _applicationIntent;

	private readonly string _applicationName;

	private readonly string _attachDBFileName;

	private readonly string _currentLanguage;

	private readonly string _dataSource;

	private readonly string _localDBInstance;

	private readonly string _failoverPartner;

	private readonly string _initialCatalog;

	private readonly string _password;

	private readonly string _userID;

	private readonly string _workstationId;

	private readonly TransactionBindingEnum _transactionBinding;

	private readonly TypeSystem _typeSystemVersion;

	private readonly Version _typeSystemAssemblyVersion;

	private static readonly Version constTypeSystemAsmVersion10 = new Version("10.0.0.0");

	private static readonly Version constTypeSystemAsmVersion11 = new Version("11.0.0.0");

	internal bool IntegratedSecurity => _integratedSecurity;

	internal bool Asynchronous => true;

	internal bool ConnectionReset => true;

	internal bool Encrypt => _encrypt;

	internal bool TrustServerCertificate => _trustServerCertificate;

	internal bool Enlist => _enlist;

	internal bool MARS => _mars;

	internal bool MultiSubnetFailover => _multiSubnetFailover;

	internal bool PersistSecurityInfo => _persistSecurityInfo;

	internal bool Pooling => _pooling;

	internal bool Replication => _replication;

	internal bool UserInstance => _userInstance;

	internal int ConnectTimeout => _connectTimeout;

	internal int LoadBalanceTimeout => _loadBalanceTimeout;

	internal int MaxPoolSize => _maxPoolSize;

	internal int MinPoolSize => _minPoolSize;

	internal int PacketSize => _packetSize;

	internal int ConnectRetryCount => _connectRetryCount;

	internal int ConnectRetryInterval => _connectRetryInterval;

	internal ApplicationIntent ApplicationIntent => _applicationIntent;

	internal string ApplicationName => _applicationName;

	internal string AttachDBFilename => _attachDBFileName;

	internal string CurrentLanguage => _currentLanguage;

	internal string DataSource => _dataSource;

	internal string LocalDBInstance => _localDBInstance;

	internal string FailoverPartner => _failoverPartner;

	internal string InitialCatalog => _initialCatalog;

	internal string Password => _password;

	internal string UserID => _userID;

	internal string WorkstationId => _workstationId;

	internal TypeSystem TypeSystemVersion => _typeSystemVersion;

	internal Version TypeSystemAssemblyVersion => _typeSystemAssemblyVersion;

	internal TransactionBindingEnum TransactionBinding => _transactionBinding;

	internal SqlConnectionString(string connectionString)
		: base(connectionString, GetParseSynonyms())
	{
		ThrowUnsupportedIfKeywordSet("asynchronous processing");
		ThrowUnsupportedIfKeywordSet("connection reset");
		ThrowUnsupportedIfKeywordSet("context connection");
		if (ContainsKey("network library"))
		{
			throw SQL.NetworkLibraryKeywordNotSupported();
		}
		_integratedSecurity = ConvertValueToIntegratedSecurity();
		_encrypt = ConvertValueToBoolean("encrypt", defaultValue: false);
		_enlist = ConvertValueToBoolean("enlist", defaultValue: true);
		_mars = ConvertValueToBoolean("multipleactiveresultsets", defaultValue: false);
		_persistSecurityInfo = ConvertValueToBoolean("persist security info", defaultValue: false);
		_pooling = ConvertValueToBoolean("pooling", defaultValue: true);
		_replication = ConvertValueToBoolean("replication", defaultValue: false);
		_userInstance = ConvertValueToBoolean("user instance", defaultValue: false);
		_multiSubnetFailover = ConvertValueToBoolean("multisubnetfailover", defaultValue: false);
		_connectTimeout = ConvertValueToInt32("connect timeout", 15);
		_loadBalanceTimeout = ConvertValueToInt32("load balance timeout", 0);
		_maxPoolSize = ConvertValueToInt32("max pool size", 100);
		_minPoolSize = ConvertValueToInt32("min pool size", 0);
		_packetSize = ConvertValueToInt32("packet size", 8000);
		_connectRetryCount = ConvertValueToInt32("connectretrycount", 1);
		_connectRetryInterval = ConvertValueToInt32("connectretryinterval", 10);
		_applicationIntent = ConvertValueToApplicationIntent();
		_applicationName = ConvertValueToString("application name", "Core .Net SqlClient Data Provider");
		_attachDBFileName = ConvertValueToString("attachdbfilename", "");
		_currentLanguage = ConvertValueToString("current language", "");
		_dataSource = ConvertValueToString("data source", "");
		_localDBInstance = LocalDBAPI.GetLocalDbInstanceNameFromServerName(_dataSource);
		_failoverPartner = ConvertValueToString("failover partner", "");
		_initialCatalog = ConvertValueToString("initial catalog", "");
		_password = ConvertValueToString("password", "");
		_trustServerCertificate = ConvertValueToBoolean("trustservercertificate", defaultValue: false);
		string text = ConvertValueToString("type system version", null);
		string text2 = ConvertValueToString("transaction binding", null);
		_userID = ConvertValueToString("user id", "");
		_workstationId = ConvertValueToString("workstation id", null);
		if (_loadBalanceTimeout < 0)
		{
			throw ADP.InvalidConnectionOptionValue("load balance timeout");
		}
		if (_connectTimeout < 0)
		{
			throw ADP.InvalidConnectionOptionValue("connect timeout");
		}
		if (_maxPoolSize < 1)
		{
			throw ADP.InvalidConnectionOptionValue("max pool size");
		}
		if (_minPoolSize < 0)
		{
			throw ADP.InvalidConnectionOptionValue("min pool size");
		}
		if (_maxPoolSize < _minPoolSize)
		{
			throw ADP.InvalidMinMaxPoolSizeValues();
		}
		if (_packetSize < 512 || 32768 < _packetSize)
		{
			throw SQL.InvalidPacketSizeValue();
		}
		ValidateValueLength(_applicationName, 128, "application name");
		ValidateValueLength(_currentLanguage, 128, "current language");
		ValidateValueLength(_dataSource, 128, "data source");
		ValidateValueLength(_failoverPartner, 128, "failover partner");
		ValidateValueLength(_initialCatalog, 128, "initial catalog");
		ValidateValueLength(_password, 128, "password");
		ValidateValueLength(_userID, 128, "user id");
		if (_workstationId != null)
		{
			ValidateValueLength(_workstationId, 128, "workstation id");
		}
		if (!string.Equals("", _failoverPartner, StringComparison.OrdinalIgnoreCase))
		{
			if (_multiSubnetFailover)
			{
				throw SQL.MultiSubnetFailoverWithFailoverPartner(serverProvidedFailoverPartner: false, null);
			}
			if (string.Equals("", _initialCatalog, StringComparison.OrdinalIgnoreCase))
			{
				throw ADP.MissingConnectionOptionValue("failover partner", "initial catalog");
			}
		}
		if (0 <= _attachDBFileName.IndexOf('|'))
		{
			throw ADP.InvalidConnectionOptionValue("attachdbfilename");
		}
		ValidateValueLength(_attachDBFileName, 260, "attachdbfilename");
		_typeSystemAssemblyVersion = constTypeSystemAsmVersion10;
		if (_userInstance && !string.IsNullOrEmpty(_failoverPartner))
		{
			throw SQL.UserInstanceFailoverNotCompatible();
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "Latest";
		}
		if (text.Equals("Latest", StringComparison.OrdinalIgnoreCase))
		{
			_typeSystemVersion = TypeSystem.Latest;
		}
		else if (text.Equals("SQL Server 2000", StringComparison.OrdinalIgnoreCase))
		{
			_typeSystemVersion = TypeSystem.SQLServer2000;
		}
		else if (text.Equals("SQL Server 2005", StringComparison.OrdinalIgnoreCase))
		{
			_typeSystemVersion = TypeSystem.SQLServer2005;
		}
		else if (text.Equals("SQL Server 2008", StringComparison.OrdinalIgnoreCase))
		{
			_typeSystemVersion = TypeSystem.Latest;
		}
		else
		{
			if (!text.Equals("SQL Server 2012", StringComparison.OrdinalIgnoreCase))
			{
				throw ADP.InvalidConnectionOptionValue("type system version");
			}
			_typeSystemVersion = TypeSystem.SQLServer2012;
			_typeSystemAssemblyVersion = constTypeSystemAsmVersion11;
		}
		if (string.IsNullOrEmpty(text2))
		{
			text2 = "Implicit Unbind";
		}
		if (text2.Equals("Implicit Unbind", StringComparison.OrdinalIgnoreCase))
		{
			_transactionBinding = TransactionBindingEnum.ImplicitUnbind;
		}
		else
		{
			if (!text2.Equals("Explicit Unbind", StringComparison.OrdinalIgnoreCase))
			{
				throw ADP.InvalidConnectionOptionValue("transaction binding");
			}
			_transactionBinding = TransactionBindingEnum.ExplicitUnbind;
		}
		if (_applicationIntent == ApplicationIntent.ReadOnly && !string.IsNullOrEmpty(_failoverPartner))
		{
			throw SQL.ROR_FailoverNotSupportedConnString();
		}
		if (_connectRetryCount < 0 || _connectRetryCount > 255)
		{
			throw ADP.InvalidConnectRetryCountValue();
		}
		if (_connectRetryInterval < 1 || _connectRetryInterval > 60)
		{
			throw ADP.InvalidConnectRetryIntervalValue();
		}
	}

	internal SqlConnectionString(SqlConnectionString connectionOptions, string dataSource, bool userInstance, bool? setEnlistValue)
		: base(connectionOptions)
	{
		_integratedSecurity = connectionOptions._integratedSecurity;
		_encrypt = connectionOptions._encrypt;
		if (setEnlistValue.HasValue)
		{
			_enlist = setEnlistValue.Value;
		}
		else
		{
			_enlist = connectionOptions._enlist;
		}
		_mars = connectionOptions._mars;
		_persistSecurityInfo = connectionOptions._persistSecurityInfo;
		_pooling = connectionOptions._pooling;
		_replication = connectionOptions._replication;
		_userInstance = userInstance;
		_connectTimeout = connectionOptions._connectTimeout;
		_loadBalanceTimeout = connectionOptions._loadBalanceTimeout;
		_maxPoolSize = connectionOptions._maxPoolSize;
		_minPoolSize = connectionOptions._minPoolSize;
		_multiSubnetFailover = connectionOptions._multiSubnetFailover;
		_packetSize = connectionOptions._packetSize;
		_applicationName = connectionOptions._applicationName;
		_attachDBFileName = connectionOptions._attachDBFileName;
		_currentLanguage = connectionOptions._currentLanguage;
		_dataSource = dataSource;
		_localDBInstance = LocalDBAPI.GetLocalDbInstanceNameFromServerName(_dataSource);
		_failoverPartner = connectionOptions._failoverPartner;
		_initialCatalog = connectionOptions._initialCatalog;
		_password = connectionOptions._password;
		_userID = connectionOptions._userID;
		_workstationId = connectionOptions._workstationId;
		_typeSystemVersion = connectionOptions._typeSystemVersion;
		_transactionBinding = connectionOptions._transactionBinding;
		_applicationIntent = connectionOptions._applicationIntent;
		_connectRetryCount = connectionOptions._connectRetryCount;
		_connectRetryInterval = connectionOptions._connectRetryInterval;
		ValidateValueLength(_dataSource, 128, "data source");
	}

	internal static Dictionary<string, string> GetParseSynonyms()
	{
		Dictionary<string, string> dictionary = s_sqlClientSynonyms;
		if (dictionary == null)
		{
			dictionary = (s_sqlClientSynonyms = new Dictionary<string, string>(54)
			{
				{ "applicationintent", "applicationintent" },
				{ "application name", "application name" },
				{ "asynchronous processing", "asynchronous processing" },
				{ "attachdbfilename", "attachdbfilename" },
				{ "connect timeout", "connect timeout" },
				{ "connection reset", "connection reset" },
				{ "context connection", "context connection" },
				{ "current language", "current language" },
				{ "data source", "data source" },
				{ "encrypt", "encrypt" },
				{ "enlist", "enlist" },
				{ "failover partner", "failover partner" },
				{ "initial catalog", "initial catalog" },
				{ "integrated security", "integrated security" },
				{ "load balance timeout", "load balance timeout" },
				{ "multipleactiveresultsets", "multipleactiveresultsets" },
				{ "max pool size", "max pool size" },
				{ "min pool size", "min pool size" },
				{ "multisubnetfailover", "multisubnetfailover" },
				{ "network library", "network library" },
				{ "packet size", "packet size" },
				{ "password", "password" },
				{ "persist security info", "persist security info" },
				{ "pooling", "pooling" },
				{ "replication", "replication" },
				{ "trustservercertificate", "trustservercertificate" },
				{ "transaction binding", "transaction binding" },
				{ "type system version", "type system version" },
				{ "user id", "user id" },
				{ "user instance", "user instance" },
				{ "workstation id", "workstation id" },
				{ "connectretrycount", "connectretrycount" },
				{ "connectretryinterval", "connectretryinterval" },
				{ "app", "application name" },
				{ "async", "asynchronous processing" },
				{ "extended properties", "attachdbfilename" },
				{ "initial file name", "attachdbfilename" },
				{ "connection timeout", "connect timeout" },
				{ "timeout", "connect timeout" },
				{ "language", "current language" },
				{ "addr", "data source" },
				{ "address", "data source" },
				{ "network address", "data source" },
				{ "server", "data source" },
				{ "database", "initial catalog" },
				{ "trusted_connection", "integrated security" },
				{ "connection lifetime", "load balance timeout" },
				{ "net", "network library" },
				{ "network", "network library" },
				{ "pwd", "password" },
				{ "persistsecurityinfo", "persist security info" },
				{ "uid", "user id" },
				{ "user", "user id" },
				{ "wsid", "workstation id" }
			});
		}
		return dictionary;
	}

	internal string ObtainWorkstationId()
	{
		string text = WorkstationId;
		if (text == null)
		{
			text = ADP.MachineName();
			ValidateValueLength(text, 128, "workstation id");
		}
		return text;
	}

	private void ValidateValueLength(string value, int limit, string key)
	{
		if (limit < value.Length)
		{
			throw ADP.InvalidConnectionOptionValueLength(key, limit);
		}
	}

	internal ApplicationIntent ConvertValueToApplicationIntent()
	{
		if (!TryGetParsetableValue("applicationintent", out var value))
		{
			return ApplicationIntent.ReadWrite;
		}
		try
		{
			return DbConnectionStringBuilderUtil.ConvertToApplicationIntent("applicationintent", value);
		}
		catch (FormatException inner)
		{
			throw ADP.InvalidConnectionOptionValue("applicationintent", inner);
		}
		catch (OverflowException inner2)
		{
			throw ADP.InvalidConnectionOptionValue("applicationintent", inner2);
		}
	}

	internal void ThrowUnsupportedIfKeywordSet(string keyword)
	{
		if (ContainsKey(keyword))
		{
			throw SQL.UnsupportedKeyword(keyword);
		}
	}
}
