namespace System.Data.SqlClient;

internal static class TdsEnums
{
	public enum EnvChangeType : byte
	{
		ENVCHANGE_DATABASE = 1,
		ENVCHANGE_LANG = 2,
		ENVCHANGE_CHARSET = 3,
		ENVCHANGE_PACKETSIZE = 4,
		ENVCHANGE_LOCALEID = 5,
		ENVCHANGE_COMPFLAGS = 6,
		ENVCHANGE_COLLATION = 7,
		ENVCHANGE_BEGINTRAN = 8,
		ENVCHANGE_COMMITTRAN = 9,
		ENVCHANGE_ROLLBACKTRAN = 10,
		ENVCHANGE_ENLISTDTC = 11,
		ENVCHANGE_DEFECTDTC = 12,
		ENVCHANGE_LOGSHIPNODE = 13,
		ENVCHANGE_PROMOTETRANSACTION = 15,
		ENVCHANGE_TRANSACTIONMANAGERADDRESS = 16,
		ENVCHANGE_TRANSACTIONENDED = 17,
		ENVCHANGE_SPRESETCONNECTIONACK = 18,
		ENVCHANGE_USERINSTANCE = 19,
		ENVCHANGE_ROUTING = 20
	}

	[Flags]
	public enum FeatureExtension : uint
	{
		None = 0u,
		SessionRecovery = 1u,
		FedAuth = 2u,
		GlobalTransactions = 8u
	}

	public enum FedAuthLibrary : byte
	{
		LiveId = 0,
		SecurityToken = 1,
		ADAL = 2,
		Default = 127
	}

	internal enum TransactionManagerRequestType
	{
		GetDTCAddress = 0,
		Propagate = 1,
		Begin = 5,
		Promote = 6,
		Commit = 7,
		Rollback = 8,
		Save = 9
	}

	internal enum TransactionManagerIsolationLevel
	{
		Unspecified,
		ReadUncommitted,
		ReadCommitted,
		RepeatableRead,
		Serializable,
		Snapshot
	}

	internal enum GenericType
	{
		MultiSet = 131
	}

	public const string SQL_PROVIDER_NAME = "Core .Net SqlClient Data Provider";

	public static readonly decimal SQL_SMALL_MONEY_MIN = new decimal(-214748.3648);

	public static readonly decimal SQL_SMALL_MONEY_MAX = new decimal(214748.3647);

	public const SqlDbType SmallVarBinary = (SqlDbType)24;

	public const string TCP = "tcp";

	public const string NP = "np";

	public const string RPC = "rpc";

	public const string BV = "bv";

	public const string ADSP = "adsp";

	public const string SPX = "spx";

	public const string VIA = "via";

	public const string LPC = "lpc";

	public const string ADMIN = "admin";

	public const string INIT_SSPI_PACKAGE = "InitSSPIPackage";

	public const string INIT_SESSION = "InitSession";

	public const string CONNECTION_GET_SVR_USER = "ConnectionGetSvrUser";

	public const string GEN_CLIENT_CONTEXT = "GenClientContext";

	public const byte SOFTFLUSH = 0;

	public const byte HARDFLUSH = 1;

	public const byte IGNORE = 2;

	public const int HEADER_LEN = 8;

	public const int HEADER_LEN_FIELD_OFFSET = 2;

	public const int YUKON_HEADER_LEN = 12;

	public const int MARS_ID_OFFSET = 8;

	public const int HEADERTYPE_QNOTIFICATION = 1;

	public const int HEADERTYPE_MARS = 2;

	public const int HEADERTYPE_TRACE = 3;

	public const int SUCCEED = 1;

	public const int FAIL = 0;

	public const short TYPE_SIZE_LIMIT = 8000;

	public const int MIN_PACKET_SIZE = 512;

	public const int DEFAULT_LOGIN_PACKET_SIZE = 4096;

	public const int MAX_PRELOGIN_PAYLOAD_LENGTH = 1024;

	public const int MAX_PACKET_SIZE = 32768;

	public const int MAX_SERVER_USER_NAME = 256;

	public const byte MIN_ERROR_CLASS = 11;

	public const byte MAX_USER_CORRECTABLE_ERROR_CLASS = 16;

	public const byte FATAL_ERROR_CLASS = 20;

	public const byte MT_SQL = 1;

	public const byte MT_LOGIN = 2;

	public const byte MT_RPC = 3;

	public const byte MT_TOKENS = 4;

	public const byte MT_BINARY = 5;

	public const byte MT_ATTN = 6;

	public const byte MT_BULK = 7;

	public const byte MT_OPEN = 8;

	public const byte MT_CLOSE = 9;

	public const byte MT_ERROR = 10;

	public const byte MT_ACK = 11;

	public const byte MT_ECHO = 12;

	public const byte MT_LOGOUT = 13;

	public const byte MT_TRANS = 14;

	public const byte MT_OLEDB = 15;

	public const byte MT_LOGIN7 = 16;

	public const byte MT_SSPI = 17;

	public const byte MT_PRELOGIN = 18;

	public const byte ST_EOM = 1;

	public const byte ST_AACK = 2;

	public const byte ST_IGNORE = 2;

	public const byte ST_BATCH = 4;

	public const byte ST_RESET_CONNECTION = 8;

	public const byte ST_RESET_CONNECTION_PRESERVE_TRANSACTION = 16;

	public const byte SQLCOLFMT = 161;

	public const byte SQLPROCID = 124;

	public const byte SQLCOLNAME = 160;

	public const byte SQLTABNAME = 164;

	public const byte SQLCOLINFO = 165;

	public const byte SQLALTNAME = 167;

	public const byte SQLALTFMT = 168;

	public const byte SQLERROR = 170;

	public const byte SQLINFO = 171;

	public const byte SQLRETURNVALUE = 172;

	public const byte SQLRETURNSTATUS = 121;

	public const byte SQLRETURNTOK = 219;

	public const byte SQLALTCONTROL = 175;

	public const byte SQLROW = 209;

	public const byte SQLNBCROW = 210;

	public const byte SQLALTROW = 211;

	public const byte SQLDONE = 253;

	public const byte SQLDONEPROC = 254;

	public const byte SQLDONEINPROC = byte.MaxValue;

	public const byte SQLOFFSET = 120;

	public const byte SQLORDER = 169;

	public const byte SQLDEBUG_CMD = 96;

	public const byte SQLLOGINACK = 173;

	public const byte SQLFEATUREEXTACK = 174;

	public const byte SQLSESSIONSTATE = 228;

	public const byte SQLENVCHANGE = 227;

	public const byte SQLSECLEVEL = 237;

	public const byte SQLROWCRC = 57;

	public const byte SQLCOLMETADATA = 129;

	public const byte SQLALTMETADATA = 136;

	public const byte SQLSSPI = 237;

	public const byte ENV_DATABASE = 1;

	public const byte ENV_LANG = 2;

	public const byte ENV_CHARSET = 3;

	public const byte ENV_PACKETSIZE = 4;

	public const byte ENV_LOCALEID = 5;

	public const byte ENV_COMPFLAGS = 6;

	public const byte ENV_COLLATION = 7;

	public const byte ENV_BEGINTRAN = 8;

	public const byte ENV_COMMITTRAN = 9;

	public const byte ENV_ROLLBACKTRAN = 10;

	public const byte ENV_ENLISTDTC = 11;

	public const byte ENV_DEFECTDTC = 12;

	public const byte ENV_LOGSHIPNODE = 13;

	public const byte ENV_PROMOTETRANSACTION = 15;

	public const byte ENV_TRANSACTIONMANAGERADDRESS = 16;

	public const byte ENV_TRANSACTIONENDED = 17;

	public const byte ENV_SPRESETCONNECTIONACK = 18;

	public const byte ENV_USERINSTANCE = 19;

	public const byte ENV_ROUTING = 20;

	public const int DONE_MORE = 1;

	public const int DONE_ERROR = 2;

	public const int DONE_INXACT = 4;

	public const int DONE_PROC = 8;

	public const int DONE_COUNT = 16;

	public const int DONE_ATTN = 32;

	public const int DONE_INPROC = 64;

	public const int DONE_RPCINBATCH = 128;

	public const int DONE_SRVERROR = 256;

	public const int DONE_FMTSENT = 32768;

	public const byte FEATUREEXT_TERMINATOR = byte.MaxValue;

	public const byte FEATUREEXT_SRECOVERY = 1;

	public const byte FEATUREEXT_GLOBALTRANSACTIONS = 5;

	public const byte FEATUREEXT_FEDAUTH = 2;

	public const byte FEDAUTHLIB_LIVEID = 0;

	public const byte FEDAUTHLIB_SECURITYTOKEN = 1;

	public const byte FEDAUTHLIB_ADAL = 2;

	public const byte FEDAUTHLIB_RESERVED = 127;

	public const byte MAX_LOG_NAME = 30;

	public const byte MAX_PROG_NAME = 10;

	public const byte SEC_COMP_LEN = 8;

	public const byte MAX_PK_LEN = 6;

	public const byte MAX_NIC_SIZE = 6;

	public const byte SQLVARIANT_SIZE = 2;

	public const byte VERSION_SIZE = 4;

	public const int CLIENT_PROG_VER = 100663296;

	public const int YUKON_LOG_REC_FIXED_LEN = 94;

	public const int TEXT_TIME_STAMP_LEN = 8;

	public const int COLLATION_INFO_LEN = 4;

	public const int YUKON_MAJOR = 114;

	public const int KATMAI_MAJOR = 115;

	public const int DENALI_MAJOR = 116;

	public const int YUKON_INCREMENT = 9;

	public const int KATMAI_INCREMENT = 11;

	public const int DENALI_INCREMENT = 0;

	public const int YUKON_RTM_MINOR = 2;

	public const int KATMAI_MINOR = 3;

	public const int DENALI_MINOR = 4;

	public const int ORDER_68000 = 1;

	public const int USE_DB_ON = 1;

	public const int INIT_DB_FATAL = 1;

	public const int SET_LANG_ON = 1;

	public const int INIT_LANG_FATAL = 1;

	public const int ODBC_ON = 1;

	public const int SSPI_ON = 1;

	public const int REPL_ON = 3;

	public const int READONLY_INTENT_ON = 1;

	public const byte SQLLenMask = 48;

	public const byte SQLFixedLen = 48;

	public const byte SQLVarLen = 32;

	public const byte SQLZeroLen = 16;

	public const byte SQLVarCnt = 0;

	public const byte SQLDifferentName = 32;

	public const byte SQLExpression = 4;

	public const byte SQLKey = 8;

	public const byte SQLHidden = 16;

	public const byte Nullable = 1;

	public const byte Identity = 16;

	public const byte Updatability = 11;

	public const byte ClrFixedLen = 1;

	public const byte IsColumnSet = 4;

	public const uint VARLONGNULL = uint.MaxValue;

	public const int VARNULL = 65535;

	public const int MAXSIZE = 8000;

	public const byte FIXEDNULL = 0;

	public const ulong UDTNULL = ulong.MaxValue;

	public const int SQLVOID = 31;

	public const int SQLTEXT = 35;

	public const int SQLVARBINARY = 37;

	public const int SQLINTN = 38;

	public const int SQLVARCHAR = 39;

	public const int SQLBINARY = 45;

	public const int SQLIMAGE = 34;

	public const int SQLCHAR = 47;

	public const int SQLINT1 = 48;

	public const int SQLBIT = 50;

	public const int SQLINT2 = 52;

	public const int SQLINT4 = 56;

	public const int SQLMONEY = 60;

	public const int SQLDATETIME = 61;

	public const int SQLFLT8 = 62;

	public const int SQLFLTN = 109;

	public const int SQLMONEYN = 110;

	public const int SQLDATETIMN = 111;

	public const int SQLFLT4 = 59;

	public const int SQLMONEY4 = 122;

	public const int SQLDATETIM4 = 58;

	public const int SQLDECIMALN = 106;

	public const int SQLNUMERICN = 108;

	public const int SQLUNIQUEID = 36;

	public const int SQLBIGCHAR = 175;

	public const int SQLBIGVARCHAR = 167;

	public const int SQLBIGBINARY = 173;

	public const int SQLBIGVARBINARY = 165;

	public const int SQLBITN = 104;

	public const int SQLNCHAR = 239;

	public const int SQLNVARCHAR = 231;

	public const int SQLNTEXT = 99;

	public const int SQLUDT = 240;

	public const int AOPCNTB = 9;

	public const int AOPSTDEV = 48;

	public const int AOPSTDEVP = 49;

	public const int AOPVAR = 50;

	public const int AOPVARP = 51;

	public const int AOPCNT = 75;

	public const int AOPSUM = 77;

	public const int AOPAVG = 79;

	public const int AOPMIN = 81;

	public const int AOPMAX = 82;

	public const int AOPANY = 83;

	public const int AOPNOOP = 86;

	public const int SQLTIMESTAMP = 80;

	public const int MAX_NUMERIC_LEN = 17;

	public const int DEFAULT_NUMERIC_PRECISION = 29;

	public const int SPHINX_DEFAULT_NUMERIC_PRECISION = 28;

	public const int MAX_NUMERIC_PRECISION = 38;

	public const byte UNKNOWN_PRECISION_SCALE = byte.MaxValue;

	public const int SQLINT8 = 127;

	public const int SQLVARIANT = 98;

	public const int SQLXMLTYPE = 241;

	public const int XMLUNICODEBOM = 65279;

	public static readonly byte[] XMLUNICODEBOMBYTES = new byte[2] { 255, 254 };

	public const int SQLTABLE = 243;

	public const int SQLDATE = 40;

	public const int SQLTIME = 41;

	public const int SQLDATETIME2 = 42;

	public const int SQLDATETIMEOFFSET = 43;

	public const int DEFAULT_VARTIME_SCALE = 7;

	public const ulong SQL_PLP_NULL = ulong.MaxValue;

	public const ulong SQL_PLP_UNKNOWNLEN = 18446744073709551614uL;

	public const int SQL_PLP_CHUNK_TERMINATOR = 0;

	public const ushort SQL_USHORTVARMAXLEN = ushort.MaxValue;

	public const byte TVP_ROWCOUNT_ESTIMATE = 18;

	public const byte TVP_ROW_TOKEN = 1;

	public const byte TVP_END_TOKEN = 0;

	public const ushort TVP_NOMETADATA_TOKEN = ushort.MaxValue;

	public const byte TVP_ORDER_UNIQUE_TOKEN = 16;

	public const int TVP_DEFAULT_COLUMN = 512;

	public const byte TVP_ORDERASC_FLAG = 1;

	public const byte TVP_ORDERDESC_FLAG = 2;

	public const byte TVP_UNIQUE_FLAG = 4;

	public const string SP_EXECUTESQL = "sp_executesql";

	public const string SP_PREPEXEC = "sp_prepexec";

	public const string SP_PREPARE = "sp_prepare";

	public const string SP_EXECUTE = "sp_execute";

	public const string SP_UNPREPARE = "sp_unprepare";

	public const string SP_PARAMS = "sp_procedure_params_rowset";

	public const string SP_PARAMS_MANAGED = "sp_procedure_params_managed";

	public const string SP_PARAMS_MGD10 = "sp_procedure_params_100_managed";

	public const ushort RPC_PROCID_CURSOR = 1;

	public const ushort RPC_PROCID_CURSOROPEN = 2;

	public const ushort RPC_PROCID_CURSORPREPARE = 3;

	public const ushort RPC_PROCID_CURSOREXECUTE = 4;

	public const ushort RPC_PROCID_CURSORPREPEXEC = 5;

	public const ushort RPC_PROCID_CURSORUNPREPARE = 6;

	public const ushort RPC_PROCID_CURSORFETCH = 7;

	public const ushort RPC_PROCID_CURSOROPTION = 8;

	public const ushort RPC_PROCID_CURSORCLOSE = 9;

	public const ushort RPC_PROCID_EXECUTESQL = 10;

	public const ushort RPC_PROCID_PREPARE = 11;

	public const ushort RPC_PROCID_EXECUTE = 12;

	public const ushort RPC_PROCID_PREPEXEC = 13;

	public const ushort RPC_PROCID_PREPEXECRPC = 14;

	public const ushort RPC_PROCID_UNPREPARE = 15;

	public const string TRANS_BEGIN = "BEGIN TRANSACTION";

	public const string TRANS_COMMIT = "COMMIT TRANSACTION";

	public const string TRANS_ROLLBACK = "ROLLBACK TRANSACTION";

	public const string TRANS_IF_ROLLBACK = "IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION";

	public const string TRANS_SAVE = "SAVE TRANSACTION";

	public const string TRANS_READ_COMMITTED = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED";

	public const string TRANS_READ_UNCOMMITTED = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";

	public const string TRANS_REPEATABLE_READ = "SET TRANSACTION ISOLATION LEVEL REPEATABLE READ";

	public const string TRANS_SERIALIZABLE = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE";

	public const string TRANS_SNAPSHOT = "SET TRANSACTION ISOLATION LEVEL SNAPSHOT";

	public const byte SHILOH_RPCBATCHFLAG = 128;

	public const byte YUKON_RPCBATCHFLAG = byte.MaxValue;

	public const byte RPC_RECOMPILE = 1;

	public const byte RPC_NOMETADATA = 2;

	public const byte RPC_PARAM_BYREF = 1;

	public const byte RPC_PARAM_DEFAULT = 2;

	public const byte RPC_PARAM_IS_LOB_COOKIE = 8;

	public const string PARAM_OUTPUT = "output";

	public const int MAX_PARAMETER_NAME_LENGTH = 128;

	public const string FMTONLY_ON = " SET FMTONLY ON;";

	public const string FMTONLY_OFF = " SET FMTONLY OFF;";

	public const string BROWSE_ON = " SET NO_BROWSETABLE ON;";

	public const string BROWSE_OFF = " SET NO_BROWSETABLE OFF;";

	public const string TABLE = "Table";

	public const int EXEC_THRESHOLD = 3;

	public const short TIMEOUT_EXPIRED = -2;

	public const short ENCRYPTION_NOT_SUPPORTED = 20;

	public const int LOGON_FAILED = 18456;

	public const int PASSWORD_EXPIRED = 18488;

	public const int IMPERSONATION_FAILED = 1346;

	public const int P_TOKENTOOLONG = 103;

	public const uint SNI_UNINITIALIZED = uint.MaxValue;

	public const uint SNI_SUCCESS = 0u;

	public const uint SNI_ERROR = 1u;

	public const uint SNI_WAIT_TIMEOUT = 258u;

	public const uint SNI_SUCCESS_IO_PENDING = 997u;

	public const short SNI_WSAECONNRESET = 10054;

	public const uint SNI_QUEUE_FULL = 1048576u;

	public const uint SNI_SSL_VALIDATE_CERTIFICATE = 1u;

	public const uint SNI_SSL_USE_SCHANNEL_CACHE = 2u;

	public const uint SNI_SSL_IGNORE_CHANNEL_BINDINGS = 16u;

	public const string DEFAULT_ENGLISH_CODE_PAGE_STRING = "iso_1";

	public const short DEFAULT_ENGLISH_CODE_PAGE_VALUE = 1252;

	public const short CHARSET_CODE_PAGE_OFFSET = 2;

	internal const int MAX_SERVERNAME = 255;

	internal const ushort SELECT = 193;

	internal const ushort INSERT = 195;

	internal const ushort DELETE = 196;

	internal const ushort UPDATE = 197;

	internal const ushort ABORT = 210;

	internal const ushort BEGINXACT = 212;

	internal const ushort ENDXACT = 213;

	internal const ushort BULKINSERT = 240;

	internal const ushort OPENCURSOR = 32;

	internal const ushort MERGE = 279;

	internal const ushort MAXLEN_HOSTNAME = 128;

	internal const ushort MAXLEN_USERNAME = 128;

	internal const ushort MAXLEN_PASSWORD = 128;

	internal const ushort MAXLEN_APPNAME = 128;

	internal const ushort MAXLEN_SERVERNAME = 128;

	internal const ushort MAXLEN_CLIENTINTERFACE = 128;

	internal const ushort MAXLEN_LANGUAGE = 128;

	internal const ushort MAXLEN_DATABASE = 128;

	internal const ushort MAXLEN_ATTACHDBFILE = 260;

	internal const ushort MAXLEN_NEWPASSWORD = 128;

	public static readonly ushort[] CODE_PAGE_FROM_SORT_ID = new ushort[256]
	{
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		437, 437, 437, 437, 437, 0, 0, 0, 0, 0,
		850, 850, 850, 850, 850, 0, 0, 0, 0, 850,
		1252, 1252, 1252, 1252, 1252, 850, 850, 850, 850, 850,
		850, 850, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 1252, 1252, 1252, 1252, 1252, 0, 0, 0, 0,
		1250, 1250, 1250, 1250, 1250, 1250, 1250, 1250, 1250, 1250,
		1250, 1250, 1250, 1250, 1250, 1250, 1250, 1250, 1250, 0,
		0, 0, 0, 0, 1251, 1251, 1251, 1251, 1251, 0,
		0, 0, 1253, 1253, 1253, 0, 0, 0, 0, 0,
		1253, 1253, 1253, 0, 1253, 0, 0, 0, 1254, 1254,
		1254, 0, 0, 0, 0, 0, 1255, 1255, 1255, 0,
		0, 0, 0, 0, 1256, 1256, 1256, 0, 0, 0,
		0, 0, 1257, 1257, 1257, 1257, 1257, 1257, 1257, 1257,
		1257, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 1252, 1252, 1252, 1252, 0, 0, 0,
		0, 0, 932, 932, 949, 949, 950, 950, 936, 936,
		932, 949, 950, 936, 874, 874, 874, 0, 0, 0,
		1252, 1252, 1252, 1252, 1252, 1252, 1252, 1252, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
		0, 0, 0, 0, 0, 0
	};

	internal static readonly long[] TICKS_FROM_SCALE = new long[8] { 10000000L, 1000000L, 100000L, 10000L, 1000L, 100L, 10L, 1L };

	internal const int WHIDBEY_DATE_LENGTH = 10;

	internal static readonly int[] WHIDBEY_TIME_LENGTH = new int[8] { 8, 10, 11, 12, 13, 14, 15, 16 };

	internal static readonly int[] WHIDBEY_DATETIME2_LENGTH = new int[8] { 19, 21, 22, 23, 24, 25, 26, 27 };

	internal static readonly int[] WHIDBEY_DATETIMEOFFSET_LENGTH = new int[8] { 26, 28, 29, 30, 31, 32, 33, 34 };

	internal static string GetSniContextEnumName(SniContext sniContext)
	{
		return sniContext switch
		{
			SniContext.Undefined => "Undefined", 
			SniContext.Snix_Connect => "Snix_Connect", 
			SniContext.Snix_PreLoginBeforeSuccessfulWrite => "Snix_PreLoginBeforeSuccessfulWrite", 
			SniContext.Snix_PreLogin => "Snix_PreLogin", 
			SniContext.Snix_LoginSspi => "Snix_LoginSspi", 
			SniContext.Snix_ProcessSspi => "Snix_ProcessSspi", 
			SniContext.Snix_Login => "Snix_Login", 
			SniContext.Snix_EnableMars => "Snix_EnableMars", 
			SniContext.Snix_AutoEnlist => "Snix_AutoEnlist", 
			SniContext.Snix_GetMarsSession => "Snix_GetMarsSession", 
			SniContext.Snix_Execute => "Snix_Execute", 
			SniContext.Snix_Read => "Snix_Read", 
			SniContext.Snix_Close => "Snix_Close", 
			SniContext.Snix_SendRows => "Snix_SendRows", 
			_ => null, 
		};
	}
}
