using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace System.Data.SqlClient;

public sealed class SqlConnectionStringBuilder : DbConnectionStringBuilder
{
	private enum Keywords
	{
		DataSource,
		FailoverPartner,
		AttachDBFilename,
		InitialCatalog,
		IntegratedSecurity,
		PersistSecurityInfo,
		UserID,
		Password,
		Enlist,
		Pooling,
		MinPoolSize,
		MaxPoolSize,
		MultipleActiveResultSets,
		Replication,
		ConnectTimeout,
		Encrypt,
		TrustServerCertificate,
		LoadBalanceTimeout,
		PacketSize,
		TypeSystemVersion,
		ApplicationName,
		CurrentLanguage,
		WorkstationID,
		UserInstance,
		TransactionBinding,
		ApplicationIntent,
		MultiSubnetFailover,
		ConnectRetryCount,
		ConnectRetryInterval,
		KeywordsCount
	}

	private sealed class SqlInitialCatalogConverter : StringConverter
	{
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return GetStandardValuesSupportedInternal(context);
		}

		private bool GetStandardValuesSupportedInternal(ITypeDescriptorContext context)
		{
			bool result = false;
			if (context != null && context.Instance is SqlConnectionStringBuilder sqlConnectionStringBuilder && 0 < sqlConnectionStringBuilder.DataSource.Length && (sqlConnectionStringBuilder.IntegratedSecurity || 0 < sqlConnectionStringBuilder.UserID.Length))
			{
				result = true;
			}
			return result;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (GetStandardValuesSupportedInternal(context))
			{
				List<string> list = new List<string>();
				try
				{
					SqlConnectionStringBuilder sqlConnectionStringBuilder = (SqlConnectionStringBuilder)context.Instance;
					using SqlConnection sqlConnection = new SqlConnection();
					sqlConnection.ConnectionString = sqlConnectionStringBuilder.ConnectionString;
					sqlConnection.Open();
					foreach (DataRow row in sqlConnection.GetSchema("DATABASES").Rows)
					{
						string item = (string)row["database_name"];
						list.Add(item);
					}
				}
				catch (SqlException e)
				{
					ADP.TraceExceptionWithoutRethrow(e);
				}
				return new StandardValuesCollection(list);
			}
			return null;
		}
	}

	internal const int KeywordsCount = 29;

	internal const int DeprecatedKeywordsCount = 4;

	private static readonly string[] s_validKeywords = CreateValidKeywords();

	private static readonly Dictionary<string, Keywords> s_keywords = CreateKeywordsDictionary();

	private ApplicationIntent _applicationIntent;

	private string _applicationName = "Core .Net SqlClient Data Provider";

	private string _attachDBFilename = "";

	private string _currentLanguage = "";

	private string _dataSource = "";

	private string _failoverPartner = "";

	private string _initialCatalog = "";

	private string _password = "";

	private string _transactionBinding = "Implicit Unbind";

	private string _typeSystemVersion = "Latest";

	private string _userID = "";

	private string _workstationID = "";

	private int _connectTimeout = 15;

	private int _loadBalanceTimeout;

	private int _maxPoolSize = 100;

	private int _minPoolSize;

	private int _packetSize = 8000;

	private int _connectRetryCount = 1;

	private int _connectRetryInterval = 10;

	private bool _encrypt;

	private bool _trustServerCertificate;

	private bool _enlist = true;

	private bool _integratedSecurity;

	private bool _multipleActiveResultSets;

	private bool _multiSubnetFailover;

	private bool _persistSecurityInfo;

	private bool _pooling = true;

	private bool _replication;

	private bool _userInstance;

	private static readonly string[] s_notSupportedKeywords = new string[5] { "Asynchronous Processing", "Connection Reset", "Context Connection", "Transaction Binding", "async" };

	private static readonly string[] s_notSupportedNetworkLibraryKeywords = new string[3] { "Network Library", "net", "network" };

	public override object this[string keyword]
	{
		get
		{
			Keywords index = GetIndex(keyword);
			return GetAt(index);
		}
		set
		{
			if (value != null)
			{
				switch (GetIndex(keyword))
				{
				case Keywords.ApplicationIntent:
					ApplicationIntent = ConvertToApplicationIntent(keyword, value);
					break;
				case Keywords.ApplicationName:
					ApplicationName = ConvertToString(value);
					break;
				case Keywords.AttachDBFilename:
					AttachDBFilename = ConvertToString(value);
					break;
				case Keywords.CurrentLanguage:
					CurrentLanguage = ConvertToString(value);
					break;
				case Keywords.DataSource:
					DataSource = ConvertToString(value);
					break;
				case Keywords.FailoverPartner:
					FailoverPartner = ConvertToString(value);
					break;
				case Keywords.InitialCatalog:
					InitialCatalog = ConvertToString(value);
					break;
				case Keywords.Password:
					Password = ConvertToString(value);
					break;
				case Keywords.UserID:
					UserID = ConvertToString(value);
					break;
				case Keywords.TransactionBinding:
					TransactionBinding = ConvertToString(value);
					break;
				case Keywords.TypeSystemVersion:
					TypeSystemVersion = ConvertToString(value);
					break;
				case Keywords.WorkstationID:
					WorkstationID = ConvertToString(value);
					break;
				case Keywords.ConnectTimeout:
					ConnectTimeout = ConvertToInt32(value);
					break;
				case Keywords.LoadBalanceTimeout:
					LoadBalanceTimeout = ConvertToInt32(value);
					break;
				case Keywords.MaxPoolSize:
					MaxPoolSize = ConvertToInt32(value);
					break;
				case Keywords.MinPoolSize:
					MinPoolSize = ConvertToInt32(value);
					break;
				case Keywords.PacketSize:
					PacketSize = ConvertToInt32(value);
					break;
				case Keywords.IntegratedSecurity:
					IntegratedSecurity = ConvertToIntegratedSecurity(value);
					break;
				case Keywords.Encrypt:
					Encrypt = ConvertToBoolean(value);
					break;
				case Keywords.TrustServerCertificate:
					TrustServerCertificate = ConvertToBoolean(value);
					break;
				case Keywords.Enlist:
					Enlist = ConvertToBoolean(value);
					break;
				case Keywords.MultipleActiveResultSets:
					MultipleActiveResultSets = ConvertToBoolean(value);
					break;
				case Keywords.MultiSubnetFailover:
					MultiSubnetFailover = ConvertToBoolean(value);
					break;
				case Keywords.PersistSecurityInfo:
					PersistSecurityInfo = ConvertToBoolean(value);
					break;
				case Keywords.Pooling:
					Pooling = ConvertToBoolean(value);
					break;
				case Keywords.Replication:
					Replication = ConvertToBoolean(value);
					break;
				case Keywords.UserInstance:
					UserInstance = ConvertToBoolean(value);
					break;
				case Keywords.ConnectRetryCount:
					ConnectRetryCount = ConvertToInt32(value);
					break;
				case Keywords.ConnectRetryInterval:
					ConnectRetryInterval = ConvertToInt32(value);
					break;
				default:
					throw UnsupportedKeyword(keyword);
				}
			}
			else
			{
				Remove(keyword);
			}
		}
	}

	public ApplicationIntent ApplicationIntent
	{
		get
		{
			return _applicationIntent;
		}
		set
		{
			if (!DbConnectionStringBuilderUtil.IsValidApplicationIntentValue(value))
			{
				throw ADP.InvalidEnumerationValue(typeof(ApplicationIntent), (int)value);
			}
			SetApplicationIntentValue(value);
			_applicationIntent = value;
		}
	}

	public string ApplicationName
	{
		get
		{
			return _applicationName;
		}
		set
		{
			SetValue("Application Name", value);
			_applicationName = value;
		}
	}

	public string AttachDBFilename
	{
		get
		{
			return _attachDBFilename;
		}
		set
		{
			SetValue("AttachDbFilename", value);
			_attachDBFilename = value;
		}
	}

	public int ConnectTimeout
	{
		get
		{
			return _connectTimeout;
		}
		set
		{
			if (value < 0)
			{
				throw ADP.InvalidConnectionOptionValue("Connect Timeout");
			}
			SetValue("Connect Timeout", value);
			_connectTimeout = value;
		}
	}

	public string CurrentLanguage
	{
		get
		{
			return _currentLanguage;
		}
		set
		{
			SetValue("Current Language", value);
			_currentLanguage = value;
		}
	}

	public string DataSource
	{
		get
		{
			return _dataSource;
		}
		set
		{
			SetValue("Data Source", value);
			_dataSource = value;
		}
	}

	public bool Encrypt
	{
		get
		{
			return _encrypt;
		}
		set
		{
			SetValue("Encrypt", value);
			_encrypt = value;
		}
	}

	public bool TrustServerCertificate
	{
		get
		{
			return _trustServerCertificate;
		}
		set
		{
			SetValue("TrustServerCertificate", value);
			_trustServerCertificate = value;
		}
	}

	public bool Enlist
	{
		get
		{
			return _enlist;
		}
		set
		{
			SetValue("Enlist", value);
			_enlist = value;
		}
	}

	public string FailoverPartner
	{
		get
		{
			return _failoverPartner;
		}
		set
		{
			SetValue("Failover Partner", value);
			_failoverPartner = value;
		}
	}

	[TypeConverter(typeof(SqlInitialCatalogConverter))]
	public string InitialCatalog
	{
		get
		{
			return _initialCatalog;
		}
		set
		{
			SetValue("Initial Catalog", value);
			_initialCatalog = value;
		}
	}

	public bool IntegratedSecurity
	{
		get
		{
			return _integratedSecurity;
		}
		set
		{
			SetValue("Integrated Security", value);
			_integratedSecurity = value;
		}
	}

	public int LoadBalanceTimeout
	{
		get
		{
			return _loadBalanceTimeout;
		}
		set
		{
			if (value < 0)
			{
				throw ADP.InvalidConnectionOptionValue("Load Balance Timeout");
			}
			SetValue("Load Balance Timeout", value);
			_loadBalanceTimeout = value;
		}
	}

	public int MaxPoolSize
	{
		get
		{
			return _maxPoolSize;
		}
		set
		{
			if (value < 1)
			{
				throw ADP.InvalidConnectionOptionValue("Max Pool Size");
			}
			SetValue("Max Pool Size", value);
			_maxPoolSize = value;
		}
	}

	public int ConnectRetryCount
	{
		get
		{
			return _connectRetryCount;
		}
		set
		{
			if (value < 0 || value > 255)
			{
				throw ADP.InvalidConnectionOptionValue("ConnectRetryCount");
			}
			SetValue("ConnectRetryCount", value);
			_connectRetryCount = value;
		}
	}

	public int ConnectRetryInterval
	{
		get
		{
			return _connectRetryInterval;
		}
		set
		{
			if (value < 1 || value > 60)
			{
				throw ADP.InvalidConnectionOptionValue("ConnectRetryInterval");
			}
			SetValue("ConnectRetryInterval", value);
			_connectRetryInterval = value;
		}
	}

	public int MinPoolSize
	{
		get
		{
			return _minPoolSize;
		}
		set
		{
			if (value < 0)
			{
				throw ADP.InvalidConnectionOptionValue("Min Pool Size");
			}
			SetValue("Min Pool Size", value);
			_minPoolSize = value;
		}
	}

	public bool MultipleActiveResultSets
	{
		get
		{
			return _multipleActiveResultSets;
		}
		set
		{
			SetValue("MultipleActiveResultSets", value);
			_multipleActiveResultSets = value;
		}
	}

	public bool MultiSubnetFailover
	{
		get
		{
			return _multiSubnetFailover;
		}
		set
		{
			SetValue("MultiSubnetFailover", value);
			_multiSubnetFailover = value;
		}
	}

	public int PacketSize
	{
		get
		{
			return _packetSize;
		}
		set
		{
			if (value < 512 || 32768 < value)
			{
				throw SQL.InvalidPacketSizeValue();
			}
			SetValue("Packet Size", value);
			_packetSize = value;
		}
	}

	public string Password
	{
		get
		{
			return _password;
		}
		set
		{
			SetValue("Password", value);
			_password = value;
		}
	}

	public bool PersistSecurityInfo
	{
		get
		{
			return _persistSecurityInfo;
		}
		set
		{
			SetValue("Persist Security Info", value);
			_persistSecurityInfo = value;
		}
	}

	public bool Pooling
	{
		get
		{
			return _pooling;
		}
		set
		{
			SetValue("Pooling", value);
			_pooling = value;
		}
	}

	public bool Replication
	{
		get
		{
			return _replication;
		}
		set
		{
			SetValue("Replication", value);
			_replication = value;
		}
	}

	public string TransactionBinding
	{
		get
		{
			return _transactionBinding;
		}
		set
		{
			SetValue("Transaction Binding", value);
			_transactionBinding = value;
		}
	}

	public string TypeSystemVersion
	{
		get
		{
			return _typeSystemVersion;
		}
		set
		{
			SetValue("Type System Version", value);
			_typeSystemVersion = value;
		}
	}

	public string UserID
	{
		get
		{
			return _userID;
		}
		set
		{
			SetValue("User ID", value);
			_userID = value;
		}
	}

	public bool UserInstance
	{
		get
		{
			return _userInstance;
		}
		set
		{
			SetValue("User Instance", value);
			_userInstance = value;
		}
	}

	public string WorkstationID
	{
		get
		{
			return _workstationID;
		}
		set
		{
			SetValue("Workstation ID", value);
			_workstationID = value;
		}
	}

	public override ICollection Keys => new ReadOnlyCollection<string>(s_validKeywords);

	public override ICollection Values
	{
		get
		{
			object[] array = new object[s_validKeywords.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = GetAt((Keywords)i);
			}
			return new ReadOnlyCollection<object>(array);
		}
	}

	[Obsolete("This property is ignored beginning in .NET Framework 4.5.For more information about SqlClient support for asynchronous programming, seehttps://docs.microsoft.com/en-us/dotnet/framework/data/adonet/asynchronous-programming")]
	public bool AsynchronousProcessing { get; set; }

	[Obsolete("ConnectionReset has been deprecated.  SqlConnection will ignore the 'connection reset'keyword and always reset the connection")]
	public bool ConnectionReset { get; set; }

	[System.MonoTODO("Not implemented in corefx: https://github.com/dotnet/corefx/issues/22474")]
	public SqlAuthenticationMethod Authentication
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	[System.MonoTODO("Not implemented in corefx: https://github.com/dotnet/corefx/issues/22474")]
	public bool ContextConnection
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	[System.MonoTODO("Not implemented in corefx: https://github.com/dotnet/corefx/issues/22474")]
	public string NetworkLibrary
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	[System.MonoTODO("Not implemented in corefx: https://github.com/dotnet/corefx/issues/22474")]
	public PoolBlockingPeriod PoolBlockingPeriod
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	[System.MonoTODO("Not implemented in corefx: https://github.com/dotnet/corefx/issues/22474")]
	public bool TransparentNetworkIPResolution
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	[System.MonoTODO("Not implemented in corefx: https://github.com/dotnet/corefx/issues/22474")]
	public SqlConnectionColumnEncryptionSetting ColumnEncryptionSetting
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	private static string[] CreateValidKeywords()
	{
		string[] array = new string[29];
		array[25] = "ApplicationIntent";
		array[20] = "Application Name";
		array[2] = "AttachDbFilename";
		array[14] = "Connect Timeout";
		array[21] = "Current Language";
		array[0] = "Data Source";
		array[15] = "Encrypt";
		array[8] = "Enlist";
		array[1] = "Failover Partner";
		array[3] = "Initial Catalog";
		array[4] = "Integrated Security";
		array[17] = "Load Balance Timeout";
		array[11] = "Max Pool Size";
		array[10] = "Min Pool Size";
		array[12] = "MultipleActiveResultSets";
		array[26] = "MultiSubnetFailover";
		array[18] = "Packet Size";
		array[7] = "Password";
		array[5] = "Persist Security Info";
		array[9] = "Pooling";
		array[13] = "Replication";
		array[24] = "Transaction Binding";
		array[16] = "TrustServerCertificate";
		array[19] = "Type System Version";
		array[6] = "User ID";
		array[23] = "User Instance";
		array[22] = "Workstation ID";
		array[27] = "ConnectRetryCount";
		array[28] = "ConnectRetryInterval";
		return array;
	}

	private static Dictionary<string, Keywords> CreateKeywordsDictionary()
	{
		return new Dictionary<string, Keywords>(47, StringComparer.OrdinalIgnoreCase)
		{
			{
				"ApplicationIntent",
				Keywords.ApplicationIntent
			},
			{
				"Application Name",
				Keywords.ApplicationName
			},
			{
				"AttachDbFilename",
				Keywords.AttachDBFilename
			},
			{
				"Connect Timeout",
				Keywords.ConnectTimeout
			},
			{
				"Current Language",
				Keywords.CurrentLanguage
			},
			{
				"Data Source",
				Keywords.DataSource
			},
			{
				"Encrypt",
				Keywords.Encrypt
			},
			{
				"Enlist",
				Keywords.Enlist
			},
			{
				"Failover Partner",
				Keywords.FailoverPartner
			},
			{
				"Initial Catalog",
				Keywords.InitialCatalog
			},
			{
				"Integrated Security",
				Keywords.IntegratedSecurity
			},
			{
				"Load Balance Timeout",
				Keywords.LoadBalanceTimeout
			},
			{
				"MultipleActiveResultSets",
				Keywords.MultipleActiveResultSets
			},
			{
				"Max Pool Size",
				Keywords.MaxPoolSize
			},
			{
				"Min Pool Size",
				Keywords.MinPoolSize
			},
			{
				"MultiSubnetFailover",
				Keywords.MultiSubnetFailover
			},
			{
				"Packet Size",
				Keywords.PacketSize
			},
			{
				"Password",
				Keywords.Password
			},
			{
				"Persist Security Info",
				Keywords.PersistSecurityInfo
			},
			{
				"Pooling",
				Keywords.Pooling
			},
			{
				"Replication",
				Keywords.Replication
			},
			{
				"Transaction Binding",
				Keywords.TransactionBinding
			},
			{
				"TrustServerCertificate",
				Keywords.TrustServerCertificate
			},
			{
				"Type System Version",
				Keywords.TypeSystemVersion
			},
			{
				"User ID",
				Keywords.UserID
			},
			{
				"User Instance",
				Keywords.UserInstance
			},
			{
				"Workstation ID",
				Keywords.WorkstationID
			},
			{
				"ConnectRetryCount",
				Keywords.ConnectRetryCount
			},
			{
				"ConnectRetryInterval",
				Keywords.ConnectRetryInterval
			},
			{
				"app",
				Keywords.ApplicationName
			},
			{
				"extended properties",
				Keywords.AttachDBFilename
			},
			{
				"initial file name",
				Keywords.AttachDBFilename
			},
			{
				"connection timeout",
				Keywords.ConnectTimeout
			},
			{
				"timeout",
				Keywords.ConnectTimeout
			},
			{
				"language",
				Keywords.CurrentLanguage
			},
			{
				"addr",
				Keywords.DataSource
			},
			{
				"address",
				Keywords.DataSource
			},
			{
				"network address",
				Keywords.DataSource
			},
			{
				"server",
				Keywords.DataSource
			},
			{
				"database",
				Keywords.InitialCatalog
			},
			{
				"trusted_connection",
				Keywords.IntegratedSecurity
			},
			{
				"connection lifetime",
				Keywords.LoadBalanceTimeout
			},
			{
				"pwd",
				Keywords.Password
			},
			{
				"persistsecurityinfo",
				Keywords.PersistSecurityInfo
			},
			{
				"uid",
				Keywords.UserID
			},
			{
				"user",
				Keywords.UserID
			},
			{
				"wsid",
				Keywords.WorkstationID
			}
		};
	}

	public SqlConnectionStringBuilder()
		: this(null)
	{
	}

	public SqlConnectionStringBuilder(string connectionString)
	{
		if (!string.IsNullOrEmpty(connectionString))
		{
			base.ConnectionString = connectionString;
		}
	}

	public override void Clear()
	{
		base.Clear();
		for (int i = 0; i < s_validKeywords.Length; i++)
		{
			Reset((Keywords)i);
		}
	}

	public override bool ContainsKey(string keyword)
	{
		ADP.CheckArgumentNull(keyword, "keyword");
		return s_keywords.ContainsKey(keyword);
	}

	private static bool ConvertToBoolean(object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToBoolean(value);
	}

	private static int ConvertToInt32(object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToInt32(value);
	}

	private static bool ConvertToIntegratedSecurity(object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToIntegratedSecurity(value);
	}

	private static string ConvertToString(object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToString(value);
	}

	private static ApplicationIntent ConvertToApplicationIntent(string keyword, object value)
	{
		return DbConnectionStringBuilderUtil.ConvertToApplicationIntent(keyword, value);
	}

	private object GetAt(Keywords index)
	{
		return index switch
		{
			Keywords.ApplicationIntent => ApplicationIntent, 
			Keywords.ApplicationName => ApplicationName, 
			Keywords.AttachDBFilename => AttachDBFilename, 
			Keywords.ConnectTimeout => ConnectTimeout, 
			Keywords.CurrentLanguage => CurrentLanguage, 
			Keywords.DataSource => DataSource, 
			Keywords.Encrypt => Encrypt, 
			Keywords.Enlist => Enlist, 
			Keywords.FailoverPartner => FailoverPartner, 
			Keywords.InitialCatalog => InitialCatalog, 
			Keywords.IntegratedSecurity => IntegratedSecurity, 
			Keywords.LoadBalanceTimeout => LoadBalanceTimeout, 
			Keywords.MultipleActiveResultSets => MultipleActiveResultSets, 
			Keywords.MaxPoolSize => MaxPoolSize, 
			Keywords.MinPoolSize => MinPoolSize, 
			Keywords.MultiSubnetFailover => MultiSubnetFailover, 
			Keywords.PacketSize => PacketSize, 
			Keywords.Password => Password, 
			Keywords.PersistSecurityInfo => PersistSecurityInfo, 
			Keywords.Pooling => Pooling, 
			Keywords.Replication => Replication, 
			Keywords.TransactionBinding => TransactionBinding, 
			Keywords.TrustServerCertificate => TrustServerCertificate, 
			Keywords.TypeSystemVersion => TypeSystemVersion, 
			Keywords.UserID => UserID, 
			Keywords.UserInstance => UserInstance, 
			Keywords.WorkstationID => WorkstationID, 
			Keywords.ConnectRetryCount => ConnectRetryCount, 
			Keywords.ConnectRetryInterval => ConnectRetryInterval, 
			_ => throw UnsupportedKeyword(s_validKeywords[(int)index]), 
		};
	}

	private Keywords GetIndex(string keyword)
	{
		ADP.CheckArgumentNull(keyword, "keyword");
		if (s_keywords.TryGetValue(keyword, out var value))
		{
			return value;
		}
		throw UnsupportedKeyword(keyword);
	}

	public override bool Remove(string keyword)
	{
		ADP.CheckArgumentNull(keyword, "keyword");
		if (s_keywords.TryGetValue(keyword, out var value) && base.Remove(s_validKeywords[(int)value]))
		{
			Reset(value);
			return true;
		}
		return false;
	}

	private void Reset(Keywords index)
	{
		switch (index)
		{
		case Keywords.ApplicationIntent:
			_applicationIntent = ApplicationIntent.ReadWrite;
			break;
		case Keywords.ApplicationName:
			_applicationName = "Core .Net SqlClient Data Provider";
			break;
		case Keywords.AttachDBFilename:
			_attachDBFilename = "";
			break;
		case Keywords.ConnectTimeout:
			_connectTimeout = 15;
			break;
		case Keywords.CurrentLanguage:
			_currentLanguage = "";
			break;
		case Keywords.DataSource:
			_dataSource = "";
			break;
		case Keywords.Encrypt:
			_encrypt = false;
			break;
		case Keywords.Enlist:
			_enlist = true;
			break;
		case Keywords.FailoverPartner:
			_failoverPartner = "";
			break;
		case Keywords.InitialCatalog:
			_initialCatalog = "";
			break;
		case Keywords.IntegratedSecurity:
			_integratedSecurity = false;
			break;
		case Keywords.LoadBalanceTimeout:
			_loadBalanceTimeout = 0;
			break;
		case Keywords.MultipleActiveResultSets:
			_multipleActiveResultSets = false;
			break;
		case Keywords.MaxPoolSize:
			_maxPoolSize = 100;
			break;
		case Keywords.MinPoolSize:
			_minPoolSize = 0;
			break;
		case Keywords.MultiSubnetFailover:
			_multiSubnetFailover = false;
			break;
		case Keywords.PacketSize:
			_packetSize = 8000;
			break;
		case Keywords.Password:
			_password = "";
			break;
		case Keywords.PersistSecurityInfo:
			_persistSecurityInfo = false;
			break;
		case Keywords.Pooling:
			_pooling = true;
			break;
		case Keywords.ConnectRetryCount:
			_connectRetryCount = 1;
			break;
		case Keywords.ConnectRetryInterval:
			_connectRetryInterval = 10;
			break;
		case Keywords.Replication:
			_replication = false;
			break;
		case Keywords.TransactionBinding:
			_transactionBinding = "Implicit Unbind";
			break;
		case Keywords.TrustServerCertificate:
			_trustServerCertificate = false;
			break;
		case Keywords.TypeSystemVersion:
			_typeSystemVersion = "Latest";
			break;
		case Keywords.UserID:
			_userID = "";
			break;
		case Keywords.UserInstance:
			_userInstance = false;
			break;
		case Keywords.WorkstationID:
			_workstationID = "";
			break;
		default:
			throw UnsupportedKeyword(s_validKeywords[(int)index]);
		}
	}

	private void SetValue(string keyword, bool value)
	{
		base[keyword] = value.ToString();
	}

	private void SetValue(string keyword, int value)
	{
		base[keyword] = value.ToString((IFormatProvider)null);
	}

	private void SetValue(string keyword, string value)
	{
		ADP.CheckArgumentNull(value, keyword);
		base[keyword] = value;
	}

	private void SetApplicationIntentValue(ApplicationIntent value)
	{
		base["ApplicationIntent"] = DbConnectionStringBuilderUtil.ApplicationIntentToString(value);
	}

	public override bool ShouldSerialize(string keyword)
	{
		ADP.CheckArgumentNull(keyword, "keyword");
		if (s_keywords.TryGetValue(keyword, out var value))
		{
			return base.ShouldSerialize(s_validKeywords[(int)value]);
		}
		return false;
	}

	public override bool TryGetValue(string keyword, out object value)
	{
		if (s_keywords.TryGetValue(keyword, out var value2))
		{
			value = GetAt(value2);
			return true;
		}
		value = null;
		return false;
	}

	private Exception UnsupportedKeyword(string keyword)
	{
		if (s_notSupportedKeywords.Contains(keyword, StringComparer.OrdinalIgnoreCase))
		{
			return SQL.UnsupportedKeyword(keyword);
		}
		if (s_notSupportedNetworkLibraryKeywords.Contains(keyword, StringComparer.OrdinalIgnoreCase))
		{
			return SQL.NetworkLibraryKeywordNotSupported();
		}
		return ADP.KeywordNotSupported(keyword);
	}
}
