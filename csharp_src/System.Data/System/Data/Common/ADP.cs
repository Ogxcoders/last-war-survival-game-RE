using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace System.Data.Common;

internal static class ADP
{
	internal enum InternalErrorCode
	{
		UnpooledObjectHasOwner = 0,
		UnpooledObjectHasWrongOwner = 1,
		PushingObjectSecondTime = 2,
		PooledObjectHasOwner = 3,
		PooledObjectInPoolMoreThanOnce = 4,
		CreateObjectReturnedNull = 5,
		NewObjectCannotBePooled = 6,
		NonPooledObjectUsedMoreThanOnce = 7,
		AttemptingToPoolOnRestrictedToken = 8,
		ConvertSidToStringSidWReturnedNull = 10,
		AttemptingToConstructReferenceCollectionOnStaticObject = 12,
		AttemptingToEnlistTwice = 13,
		CreateReferenceCollectionReturnedNull = 14,
		PooledObjectWithoutPool = 15,
		UnexpectedWaitAnyResult = 16,
		SynchronousConnectReturnedPending = 17,
		CompletedConnectReturnedPending = 18,
		NameValuePairNext = 20,
		InvalidParserState1 = 21,
		InvalidParserState2 = 22,
		InvalidParserState3 = 23,
		InvalidBuffer = 30,
		UnimplementedSMIMethod = 40,
		InvalidSmiCall = 41,
		SqlDependencyObtainProcessDispatcherFailureObjectHandle = 50,
		SqlDependencyProcessDispatcherFailureCreateInstance = 51,
		SqlDependencyProcessDispatcherFailureAppDomain = 52,
		SqlDependencyCommandHashIsNotAssociatedWithNotification = 53,
		UnknownTransactionFailure = 60
	}

	internal enum ConnectionError
	{
		BeginGetConnectionReturnsNull,
		GetConnectionReturnsNull,
		ConnectionOptionsMissing,
		CouldNotSwitchToClosedPreviouslyOpenedState
	}

	private static Task<bool> _trueTask;

	private static Task<bool> _falseTask;

	internal const CompareOptions DefaultCompareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;

	internal const int DefaultConnectionTimeout = 15;

	private static readonly Type s_stackOverflowType = typeof(StackOverflowException);

	private static readonly Type s_outOfMemoryType = typeof(OutOfMemoryException);

	private static readonly Type s_threadAbortType = typeof(ThreadAbortException);

	private static readonly Type s_nullReferenceType = typeof(NullReferenceException);

	private static readonly Type s_accessViolationType = typeof(AccessViolationException);

	private static readonly Type s_securityType = typeof(SecurityException);

	internal const string ConnectionString = "ConnectionString";

	internal const string DataSetColumn = "DataSetColumn";

	internal const string DataSetTable = "DataSetTable";

	internal const string Fill = "Fill";

	internal const string FillSchema = "FillSchema";

	internal const string SourceColumn = "SourceColumn";

	internal const string SourceTable = "SourceTable";

	internal const string Parameter = "Parameter";

	internal const string ParameterName = "ParameterName";

	internal const string ParameterSetPosition = "set_Position";

	internal const int DefaultCommandTimeout = 30;

	internal const float FailoverTimeoutStep = 0.08f;

	internal static readonly string StrEmpty = "";

	internal const int CharSize = 2;

	private static Version s_systemDataVersion;

	internal static readonly string[] AzureSqlServerEndpoints = new string[4]
	{
		global::SR.GetString(".database.windows.net"),
		global::SR.GetString(".database.cloudapi.de"),
		global::SR.GetString(".database.usgovcloudapi.net"),
		global::SR.GetString(".database.chinacloudapi.cn")
	};

	internal const int DecimalMaxPrecision = 29;

	internal const int DecimalMaxPrecision28 = 28;

	internal static readonly IntPtr PtrZero = new IntPtr(0);

	internal static readonly int PtrSize = IntPtr.Size;

	internal const string BeginTransaction = "BeginTransaction";

	internal const string ChangeDatabase = "ChangeDatabase";

	internal const string CommitTransaction = "CommitTransaction";

	internal const string CommandTimeout = "CommandTimeout";

	internal const string DeriveParameters = "DeriveParameters";

	internal const string ExecuteReader = "ExecuteReader";

	internal const string ExecuteNonQuery = "ExecuteNonQuery";

	internal const string ExecuteScalar = "ExecuteScalar";

	internal const string GetSchema = "GetSchema";

	internal const string GetSchemaTable = "GetSchemaTable";

	internal const string Prepare = "Prepare";

	internal const string RollbackTransaction = "RollbackTransaction";

	internal const string QuoteIdentifier = "QuoteIdentifier";

	internal const string UnquoteIdentifier = "UnquoteIdentifier";

	internal static Task<bool> TrueTask => _trueTask ?? (_trueTask = Task.FromResult(result: true));

	internal static Task<bool> FalseTask => _falseTask ?? (_falseTask = Task.FromResult(result: false));

	internal static Timer UnsafeCreateTimer(TimerCallback callback, object state, int dueTime, int period)
	{
		bool flag = false;
		try
		{
			if (!ExecutionContext.IsFlowSuppressed())
			{
				ExecutionContext.SuppressFlow();
				flag = true;
			}
			return new Timer(callback, state, dueTime, period);
		}
		finally
		{
			if (flag)
			{
				ExecutionContext.RestoreFlow();
			}
		}
	}

	private static void TraceException(string trace, Exception e)
	{
		if (e != null)
		{
			DataCommonEventSource.Log.Trace(trace, e);
		}
	}

	internal static void TraceExceptionAsReturnValue(Exception e)
	{
		TraceException("<comm.ADP.TraceException|ERR|THROW> '{0}'", e);
	}

	internal static void TraceExceptionWithoutRethrow(Exception e)
	{
		TraceException("<comm.ADP.TraceException|ERR|CATCH> '%ls'\n", e);
	}

	internal static ArgumentException Argument(string error)
	{
		ArgumentException ex = new ArgumentException(error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException Argument(string error, Exception inner)
	{
		ArgumentException ex = new ArgumentException(error, inner);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException Argument(string error, string parameter)
	{
		ArgumentException ex = new ArgumentException(error, parameter);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentNullException ArgumentNull(string parameter)
	{
		ArgumentNullException ex = new ArgumentNullException(parameter);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentNullException ArgumentNull(string parameter, string error)
	{
		ArgumentNullException ex = new ArgumentNullException(parameter, error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentOutOfRangeException ArgumentOutOfRange(string parameterName)
	{
		ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException(parameterName);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentOutOfRangeException ArgumentOutOfRange(string message, string parameterName)
	{
		ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException(parameterName, message);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static IndexOutOfRangeException IndexOutOfRange(string error)
	{
		IndexOutOfRangeException ex = new IndexOutOfRangeException(error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static InvalidCastException InvalidCast(string error)
	{
		return InvalidCast(error, null);
	}

	internal static InvalidCastException InvalidCast(string error, Exception inner)
	{
		InvalidCastException ex = new InvalidCastException(error, inner);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static InvalidOperationException InvalidOperation(string error)
	{
		InvalidOperationException ex = new InvalidOperationException(error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static NotSupportedException NotSupported()
	{
		NotSupportedException ex = new NotSupportedException();
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static NotSupportedException NotSupported(string error)
	{
		NotSupportedException ex = new NotSupportedException(error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static bool RemoveStringQuotes(string quotePrefix, string quoteSuffix, string quotedString, out string unquotedString)
	{
		int num = quotePrefix?.Length ?? 0;
		int num2 = quoteSuffix?.Length ?? 0;
		if (num2 + num == 0)
		{
			unquotedString = quotedString;
			return true;
		}
		if (quotedString == null)
		{
			unquotedString = quotedString;
			return false;
		}
		int length = quotedString.Length;
		if (length < num + num2)
		{
			unquotedString = quotedString;
			return false;
		}
		if (num > 0 && !quotedString.StartsWith(quotePrefix, StringComparison.Ordinal))
		{
			unquotedString = quotedString;
			return false;
		}
		if (num2 > 0)
		{
			if (!quotedString.EndsWith(quoteSuffix, StringComparison.Ordinal))
			{
				unquotedString = quotedString;
				return false;
			}
			unquotedString = quotedString.Substring(num, length - (num + num2)).Replace(quoteSuffix + quoteSuffix, quoteSuffix);
		}
		else
		{
			unquotedString = quotedString.Substring(num, length - num);
		}
		return true;
	}

	internal static ArgumentOutOfRangeException NotSupportedEnumerationValue(Type type, string value, string method)
	{
		return ArgumentOutOfRange(global::SR.Format("The {0} enumeration value, {1}, is not supported by the {2} method.", type.Name, value, method), type.Name);
	}

	internal static InvalidOperationException DataAdapter(string error)
	{
		return InvalidOperation(error);
	}

	private static InvalidOperationException Provider(string error)
	{
		return InvalidOperation(error);
	}

	internal static ArgumentException InvalidMultipartName(string property, string value)
	{
		ArgumentException ex = new ArgumentException(global::SR.Format("{0} \"{1}\".", property, value));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException InvalidMultipartNameIncorrectUsageOfQuotes(string property, string value)
	{
		ArgumentException ex = new ArgumentException(global::SR.Format("{0} \"{1}\", incorrect usage of quotes.", property, value));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException InvalidMultipartNameToManyParts(string property, string value, int limit)
	{
		ArgumentException ex = new ArgumentException(global::SR.Format("{0} \"{1}\", the current limit of \"{2}\" is insufficient.", property, value, limit));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static void CheckArgumentNull(object value, string parameterName)
	{
		if (value == null)
		{
			throw ArgumentNull(parameterName);
		}
	}

	internal static bool IsCatchableExceptionType(Exception e)
	{
		Type type = e.GetType();
		if (type != s_stackOverflowType && type != s_outOfMemoryType && type != s_threadAbortType && type != s_nullReferenceType && type != s_accessViolationType)
		{
			return !s_securityType.IsAssignableFrom(type);
		}
		return false;
	}

	internal static bool IsCatchableOrSecurityExceptionType(Exception e)
	{
		Type type = e.GetType();
		if (type != s_stackOverflowType && type != s_outOfMemoryType && type != s_threadAbortType && type != s_nullReferenceType)
		{
			return type != s_accessViolationType;
		}
		return false;
	}

	internal static ArgumentOutOfRangeException InvalidEnumerationValue(Type type, int value)
	{
		return ArgumentOutOfRange(global::SR.Format("The {0} enumeration value, {1}, is invalid.", type.Name, value.ToString(CultureInfo.InvariantCulture)), type.Name);
	}

	internal static ArgumentException ConnectionStringSyntax(int index)
	{
		return Argument(global::SR.Format("Format of the initialization string does not conform to specification starting at index {0}.", index));
	}

	internal static ArgumentException KeywordNotSupported(string keyword)
	{
		return Argument(global::SR.Format("Keyword not supported: '{0}'.", keyword));
	}

	internal static ArgumentException ConvertFailed(Type fromType, Type toType, Exception innerException)
	{
		return Argument(global::SR.Format(" Cannot convert object of type '{0}' to object of type '{1}'.", fromType.FullName, toType.FullName), innerException);
	}

	internal static Exception InvalidConnectionOptionValue(string key)
	{
		return InvalidConnectionOptionValue(key, null);
	}

	internal static Exception InvalidConnectionOptionValue(string key, Exception inner)
	{
		return Argument(global::SR.Format("Invalid value for key '{0}'.", key), inner);
	}

	internal static ArgumentException CollectionRemoveInvalidObject(Type itemType, ICollection collection)
	{
		return Argument(global::SR.Format("Attempted to remove an {0} that is not contained by this {1}.", itemType.Name, collection.GetType().Name));
	}

	internal static ArgumentNullException CollectionNullValue(string parameter, Type collection, Type itemType)
	{
		return ArgumentNull(parameter, global::SR.Format("The {0} only accepts non-null {1} type objects.", collection.Name, itemType.Name));
	}

	internal static IndexOutOfRangeException CollectionIndexInt32(int index, Type collection, int count)
	{
		return IndexOutOfRange(global::SR.Format("Invalid index {0} for this {1} with Count={2}.", index.ToString(CultureInfo.InvariantCulture), collection.Name, count.ToString(CultureInfo.InvariantCulture)));
	}

	internal static IndexOutOfRangeException CollectionIndexString(Type itemType, string propertyName, string propertyValue, Type collection)
	{
		return IndexOutOfRange(global::SR.Format("An {0} with {1} '{2}' is not contained by this {3}.", itemType.Name, propertyName, propertyValue, collection.Name));
	}

	internal static InvalidCastException CollectionInvalidType(Type collection, Type itemType, object invalidValue)
	{
		return InvalidCast(global::SR.Format("The {0} only accepts non-null {1} type objects, not {2} objects.", collection.Name, itemType.Name, invalidValue.GetType().Name));
	}

	private static string ConnectionStateMsg(ConnectionState state)
	{
		switch (state)
		{
		case ConnectionState.Closed:
		case ConnectionState.Connecting | ConnectionState.Broken:
			return "The connection's current state is closed.";
		case ConnectionState.Connecting:
			return "The connection's current state is connecting.";
		case ConnectionState.Open:
			return "The connection's current state is open.";
		case ConnectionState.Open | ConnectionState.Executing:
			return "The connection's current state is executing.";
		case ConnectionState.Open | ConnectionState.Fetching:
			return "The connection's current state is fetching.";
		default:
			return global::SR.Format("The connection's current state: {0}.", state.ToString());
		}
	}

	internal static Exception StreamClosed([CallerMemberName] string method = "")
	{
		return InvalidOperation(global::SR.Format("Invalid attempt to {0} when stream is closed.", method));
	}

	internal static string BuildQuotedString(string quotePrefix, string quoteSuffix, string unQuotedString)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!string.IsNullOrEmpty(quotePrefix))
		{
			stringBuilder.Append(quotePrefix);
		}
		if (!string.IsNullOrEmpty(quoteSuffix))
		{
			stringBuilder.Append(unQuotedString.Replace(quoteSuffix, quoteSuffix + quoteSuffix));
			stringBuilder.Append(quoteSuffix);
		}
		else
		{
			stringBuilder.Append(unQuotedString);
		}
		return stringBuilder.ToString();
	}

	internal static ArgumentException ParametersIsNotParent(Type parameterType, ICollection collection)
	{
		return Argument(global::SR.Format("The {0} is already contained by another {1}.", parameterType.Name, collection.GetType().Name));
	}

	internal static ArgumentException ParametersIsParent(Type parameterType, ICollection collection)
	{
		return Argument(global::SR.Format("The {0} is already contained by another {1}.", parameterType.Name, collection.GetType().Name));
	}

	internal static Exception InternalError(InternalErrorCode internalError)
	{
		return InvalidOperation(global::SR.Format("Internal .Net Framework Data Provider error {0}.", (int)internalError));
	}

	internal static Exception DataReaderClosed([CallerMemberName] string method = "")
	{
		return InvalidOperation(global::SR.Format("Invalid attempt to call {0} when reader is closed.", method));
	}

	internal static ArgumentOutOfRangeException InvalidSourceBufferIndex(int maxLen, long srcOffset, string parameterName)
	{
		return ArgumentOutOfRange(global::SR.Format("Invalid source buffer (size of {0}) offset: {1}", maxLen.ToString(CultureInfo.InvariantCulture), srcOffset.ToString(CultureInfo.InvariantCulture)), parameterName);
	}

	internal static ArgumentOutOfRangeException InvalidDestinationBufferIndex(int maxLen, int dstOffset, string parameterName)
	{
		return ArgumentOutOfRange(global::SR.Format("Invalid destination buffer (size of {0}) offset: {1}", maxLen.ToString(CultureInfo.InvariantCulture), dstOffset.ToString(CultureInfo.InvariantCulture)), parameterName);
	}

	internal static IndexOutOfRangeException InvalidBufferSizeOrIndex(int numBytes, int bufferIndex)
	{
		return IndexOutOfRange(global::SR.Format("Buffer offset '{1}' plus the bytes available '{0}' is greater than the length of the passed in buffer.", numBytes.ToString(CultureInfo.InvariantCulture), bufferIndex.ToString(CultureInfo.InvariantCulture)));
	}

	internal static Exception InvalidDataLength(long length)
	{
		return IndexOutOfRange(global::SR.Format("Data length '{0}' is less than 0.", length.ToString(CultureInfo.InvariantCulture)));
	}

	internal static bool CompareInsensitiveInvariant(string strvalue, string strconst)
	{
		return CultureInfo.InvariantCulture.CompareInfo.Compare(strvalue, strconst, CompareOptions.IgnoreCase) == 0;
	}

	internal static int DstCompare(string strA, string strB)
	{
		return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
	}

	internal static bool IsEmptyArray(string[] array)
	{
		if (array != null)
		{
			return array.Length == 0;
		}
		return true;
	}

	internal static bool IsNull(object value)
	{
		if (value == null || DBNull.Value == value)
		{
			return true;
		}
		if (value is INullable nullable)
		{
			return nullable.IsNull;
		}
		return false;
	}

	internal static Exception InvalidSeekOrigin(string parameterName)
	{
		return ArgumentOutOfRange("Specified SeekOrigin value is invalid.", parameterName);
	}

	internal static void SetCurrentTransaction(Transaction transaction)
	{
		Transaction.Current = transaction;
	}

	internal static Task<T> CreatedTaskWithCancellation<T>()
	{
		return Task.FromCanceled<T>(new CancellationToken(canceled: true));
	}

	internal static void TraceExceptionForCapture(Exception e)
	{
		TraceException("<comm.ADP.TraceException|ERR|CATCH> '{0}'", e);
	}

	internal static DataException Data(string message)
	{
		DataException ex = new DataException(message);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static void CheckArgumentLength(string value, string parameterName)
	{
		CheckArgumentNull(value, parameterName);
		if (value.Length == 0)
		{
			throw Argument(global::SR.Format("Expecting non-empty string for '{0}' parameter.", parameterName));
		}
	}

	internal static void CheckArgumentLength(Array value, string parameterName)
	{
		CheckArgumentNull(value, parameterName);
		if (value.Length == 0)
		{
			throw Argument(global::SR.Format("Expecting non-empty array for '{0}' parameter.", parameterName));
		}
	}

	internal static ArgumentOutOfRangeException InvalidAcceptRejectRule(AcceptRejectRule value)
	{
		return InvalidEnumerationValue(typeof(AcceptRejectRule), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidCatalogLocation(CatalogLocation value)
	{
		return InvalidEnumerationValue(typeof(CatalogLocation), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidConflictOptions(ConflictOption value)
	{
		return InvalidEnumerationValue(typeof(ConflictOption), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidDataRowState(DataRowState value)
	{
		return InvalidEnumerationValue(typeof(DataRowState), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidKeyRestrictionBehavior(KeyRestrictionBehavior value)
	{
		return InvalidEnumerationValue(typeof(KeyRestrictionBehavior), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidLoadOption(LoadOption value)
	{
		return InvalidEnumerationValue(typeof(LoadOption), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidMissingMappingAction(MissingMappingAction value)
	{
		return InvalidEnumerationValue(typeof(MissingMappingAction), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidMissingSchemaAction(MissingSchemaAction value)
	{
		return InvalidEnumerationValue(typeof(MissingSchemaAction), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidRule(Rule value)
	{
		return InvalidEnumerationValue(typeof(Rule), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidSchemaType(SchemaType value)
	{
		return InvalidEnumerationValue(typeof(SchemaType), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidStatementType(StatementType value)
	{
		return InvalidEnumerationValue(typeof(StatementType), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidUpdateStatus(UpdateStatus value)
	{
		return InvalidEnumerationValue(typeof(UpdateStatus), (int)value);
	}

	internal static ArgumentOutOfRangeException NotSupportedStatementType(StatementType value, string method)
	{
		return NotSupportedEnumerationValue(typeof(StatementType), value.ToString(), method);
	}

	internal static ArgumentException InvalidKeyname(string parameterName)
	{
		return Argument("Invalid keyword, contain one or more of 'no characters', 'control characters', 'leading or trailing whitespace' or 'leading semicolons'.", parameterName);
	}

	internal static ArgumentException InvalidValue(string parameterName)
	{
		return Argument("The value contains embedded nulls (\\\\u0000).", parameterName);
	}

	internal static Exception WrongType(Type got, Type expected)
	{
		return Argument(global::SR.Format("Expecting argument of type {1}, but received type {0}.", got.ToString(), expected.ToString()));
	}

	internal static Exception CollectionUniqueValue(Type itemType, string propertyName, string propertyValue)
	{
		return Argument(global::SR.Format("The {0}.{1} is required to be unique, '{2}' already exists in the collection.", itemType.Name, propertyName, propertyValue));
	}

	internal static InvalidOperationException MissingSelectCommand(string method)
	{
		return Provider(global::SR.Format("The SelectCommand property has not been initialized before calling '{0}'.", method));
	}

	private static InvalidOperationException DataMapping(string error)
	{
		return InvalidOperation(error);
	}

	internal static InvalidOperationException ColumnSchemaExpression(string srcColumn, string cacheColumn)
	{
		return DataMapping(global::SR.Format("The column mapping from SourceColumn '{0}' failed because the DataColumn '{1}' is a computed column.", srcColumn, cacheColumn));
	}

	internal static InvalidOperationException ColumnSchemaMismatch(string srcColumn, Type srcType, DataColumn column)
	{
		return DataMapping(global::SR.Format("Inconvertible type mismatch between SourceColumn '{0}' of {1} and the DataColumn '{2}' of {3}.", srcColumn, srcType.Name, column.ColumnName, column.DataType.Name));
	}

	internal static InvalidOperationException ColumnSchemaMissing(string cacheColumn, string tableName, string srcColumn)
	{
		if (string.IsNullOrEmpty(tableName))
		{
			return InvalidOperation(global::SR.Format("Missing the DataColumn '{0}' for the SourceColumn '{2}'.", cacheColumn, tableName, srcColumn));
		}
		return DataMapping(global::SR.Format("Missing the DataColumn '{0}' in the DataTable '{1}' for the SourceColumn '{2}'.", cacheColumn, tableName, srcColumn));
	}

	internal static InvalidOperationException MissingColumnMapping(string srcColumn)
	{
		return DataMapping(global::SR.Format("Missing SourceColumn mapping for '{0}'.", srcColumn));
	}

	internal static InvalidOperationException MissingTableSchema(string cacheTable, string srcTable)
	{
		return DataMapping(global::SR.Format("Missing the '{0}' DataTable for the '{1}' SourceTable.", cacheTable, srcTable));
	}

	internal static InvalidOperationException MissingTableMapping(string srcTable)
	{
		return DataMapping(global::SR.Format("Missing SourceTable mapping: '{0}'", srcTable));
	}

	internal static InvalidOperationException MissingTableMappingDestination(string dstTable)
	{
		return DataMapping(global::SR.Format("Missing TableMapping when TableMapping.DataSetTable='{0}'.", dstTable));
	}

	internal static Exception InvalidSourceColumn(string parameter)
	{
		return Argument("SourceColumn is required to be a non-empty string.", parameter);
	}

	internal static Exception ColumnsAddNullAttempt(string parameter)
	{
		return CollectionNullValue(parameter, typeof(DataColumnMappingCollection), typeof(DataColumnMapping));
	}

	internal static Exception ColumnsDataSetColumn(string cacheColumn)
	{
		return CollectionIndexString(typeof(DataColumnMapping), "DataSetColumn", cacheColumn, typeof(DataColumnMappingCollection));
	}

	internal static Exception ColumnsIndexInt32(int index, IColumnMappingCollection collection)
	{
		return CollectionIndexInt32(index, collection.GetType(), collection.Count);
	}

	internal static Exception ColumnsIndexSource(string srcColumn)
	{
		return CollectionIndexString(typeof(DataColumnMapping), "SourceColumn", srcColumn, typeof(DataColumnMappingCollection));
	}

	internal static Exception ColumnsIsNotParent(ICollection collection)
	{
		return ParametersIsNotParent(typeof(DataColumnMapping), collection);
	}

	internal static Exception ColumnsIsParent(ICollection collection)
	{
		return ParametersIsParent(typeof(DataColumnMapping), collection);
	}

	internal static Exception ColumnsUniqueSourceColumn(string srcColumn)
	{
		return CollectionUniqueValue(typeof(DataColumnMapping), "SourceColumn", srcColumn);
	}

	internal static Exception NotADataColumnMapping(object value)
	{
		return CollectionInvalidType(typeof(DataColumnMappingCollection), typeof(DataColumnMapping), value);
	}

	internal static Exception InvalidSourceTable(string parameter)
	{
		return Argument("SourceTable is required to be a non-empty string", parameter);
	}

	internal static Exception TablesAddNullAttempt(string parameter)
	{
		return CollectionNullValue(parameter, typeof(DataTableMappingCollection), typeof(DataTableMapping));
	}

	internal static Exception TablesDataSetTable(string cacheTable)
	{
		return CollectionIndexString(typeof(DataTableMapping), "DataSetTable", cacheTable, typeof(DataTableMappingCollection));
	}

	internal static Exception TablesIndexInt32(int index, ITableMappingCollection collection)
	{
		return CollectionIndexInt32(index, collection.GetType(), collection.Count);
	}

	internal static Exception TablesIsNotParent(ICollection collection)
	{
		return ParametersIsNotParent(typeof(DataTableMapping), collection);
	}

	internal static Exception TablesIsParent(ICollection collection)
	{
		return ParametersIsParent(typeof(DataTableMapping), collection);
	}

	internal static Exception TablesSourceIndex(string srcTable)
	{
		return CollectionIndexString(typeof(DataTableMapping), "SourceTable", srcTable, typeof(DataTableMappingCollection));
	}

	internal static Exception TablesUniqueSourceTable(string srcTable)
	{
		return CollectionUniqueValue(typeof(DataTableMapping), "SourceTable", srcTable);
	}

	internal static Exception NotADataTableMapping(object value)
	{
		return CollectionInvalidType(typeof(DataTableMappingCollection), typeof(DataTableMapping), value);
	}

	internal static InvalidOperationException UpdateConnectionRequired(StatementType statementType, bool isRowUpdatingCommand)
	{
		string error;
		if (isRowUpdatingCommand)
		{
			error = "Update requires the command clone to have a connection object. The Connection property of the command clone has not been initialized.";
		}
		else
		{
			switch (statementType)
			{
			case StatementType.Insert:
				error = "Update requires the InsertCommand to have a connection object. The Connection property of the InsertCommand has not been initialized.";
				break;
			case StatementType.Update:
				error = "Update requires the UpdateCommand to have a connection object. The Connection property of the UpdateCommand has not been initialized.";
				break;
			case StatementType.Delete:
				error = "Update requires the DeleteCommand to have a connection object. The Connection property of the DeleteCommand has not been initialized.";
				break;
			case StatementType.Batch:
				error = "Update requires a connection object.  The Connection property has not been initialized.";
				goto default;
			default:
				throw InvalidStatementType(statementType);
			}
		}
		return InvalidOperation(error);
	}

	internal static InvalidOperationException ConnectionRequired_Res(string method)
	{
		return InvalidOperation("ADP_ConnectionRequired_" + method);
	}

	internal static InvalidOperationException UpdateOpenConnectionRequired(StatementType statementType, bool isRowUpdatingCommand, ConnectionState state)
	{
		string resourceFormat = (isRowUpdatingCommand ? "Update requires the updating command to have an open connection object. {1}" : (statementType switch
		{
			StatementType.Insert => "Update requires the {0}Command to have an open connection object. {1}", 
			StatementType.Update => "Update requires the {0}Command to have an open connection object. {1}", 
			StatementType.Delete => "Update requires the {0}Command to have an open connection object. {1}", 
			_ => throw InvalidStatementType(statementType), 
		}));
		return InvalidOperation(global::SR.Format(resourceFormat, ConnectionStateMsg(state)));
	}

	internal static ArgumentException UnwantedStatementType(StatementType statementType)
	{
		return Argument(global::SR.Format("The StatementType {0} is not expected here.", statementType.ToString()));
	}

	internal static Exception FillSchemaRequiresSourceTableName(string parameter)
	{
		return Argument("FillSchema: expected a non-empty string for the SourceTable name.", parameter);
	}

	internal static Exception InvalidMaxRecords(string parameter, int max)
	{
		return Argument(global::SR.Format("The MaxRecords value of {0} is invalid; the value must be >= 0.", max.ToString(CultureInfo.InvariantCulture)), parameter);
	}

	internal static Exception InvalidStartRecord(string parameter, int start)
	{
		return Argument(global::SR.Format("The StartRecord value of {0} is invalid; the value must be >= 0.", start.ToString(CultureInfo.InvariantCulture)), parameter);
	}

	internal static Exception FillRequires(string parameter)
	{
		return ArgumentNull(parameter);
	}

	internal static Exception FillRequiresSourceTableName(string parameter)
	{
		return Argument("Fill: expected a non-empty string for the SourceTable name.", parameter);
	}

	internal static Exception FillChapterAutoIncrement()
	{
		return InvalidOperation("Hierarchical chapter columns must map to an AutoIncrement DataColumn.");
	}

	internal static InvalidOperationException MissingDataReaderFieldType(int index)
	{
		return DataAdapter(global::SR.Format("DataReader.GetFieldType({0}) returned null.", index));
	}

	internal static InvalidOperationException OnlyOneTableForStartRecordOrMaxRecords()
	{
		return DataAdapter("Only specify one item in the dataTables array when using non-zero values for startRecords or maxRecords.");
	}

	internal static ArgumentNullException UpdateRequiresNonNullDataSet(string parameter)
	{
		return ArgumentNull(parameter);
	}

	internal static InvalidOperationException UpdateRequiresSourceTable(string defaultSrcTableName)
	{
		return InvalidOperation(global::SR.Format("Update unable to find TableMapping['{0}'] or DataTable '{0}'.", defaultSrcTableName));
	}

	internal static InvalidOperationException UpdateRequiresSourceTableName(string srcTable)
	{
		return InvalidOperation(global::SR.Format("Update: expected a non-empty SourceTable name.", srcTable));
	}

	internal static ArgumentNullException UpdateRequiresDataTable(string parameter)
	{
		return ArgumentNull(parameter);
	}

	internal static Exception UpdateConcurrencyViolation(StatementType statementType, int affected, int expected, DataRow[] dataRows)
	{
		DBConcurrencyException ex = new DBConcurrencyException(global::SR.Format(statementType switch
		{
			StatementType.Update => "Concurrency violation: the UpdateCommand affected {0} of the expected {1} records.", 
			StatementType.Delete => "Concurrency violation: the DeleteCommand affected {0} of the expected {1} records.", 
			StatementType.Batch => "Concurrency violation: the batched command affected {0} of the expected {1} records.", 
			_ => throw InvalidStatementType(statementType), 
		}, affected.ToString(CultureInfo.InvariantCulture), expected.ToString(CultureInfo.InvariantCulture)), null, dataRows);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static InvalidOperationException UpdateRequiresCommand(StatementType statementType, bool isRowUpdatingCommand)
	{
		string error = (isRowUpdatingCommand ? "Update requires the command clone to be valid." : (statementType switch
		{
			StatementType.Select => "Auto SQL generation during Update requires a valid SelectCommand.", 
			StatementType.Insert => "Update requires a valid InsertCommand when passed DataRow collection with new rows.", 
			StatementType.Update => "Update requires a valid UpdateCommand when passed DataRow collection with modified rows.", 
			StatementType.Delete => "Update requires a valid DeleteCommand when passed DataRow collection with deleted rows.", 
			_ => throw InvalidStatementType(statementType), 
		}));
		return InvalidOperation(error);
	}

	internal static ArgumentException UpdateMismatchRowTable(int i)
	{
		return Argument(global::SR.Format("DataRow[{0}] is from a different DataTable than DataRow[0].", i.ToString(CultureInfo.InvariantCulture)));
	}

	internal static DataException RowUpdatedErrors()
	{
		return Data("RowUpdatedEvent: Errors occurred; no additional is information available.");
	}

	internal static DataException RowUpdatingErrors()
	{
		return Data("RowUpdatingEvent: Errors occurred; no additional is information available.");
	}

	internal static InvalidOperationException ResultsNotAllowedDuringBatch()
	{
		return DataAdapter("When batching, the command's UpdatedRowSource property value of UpdateRowSource.FirstReturnedRecord or UpdateRowSource.Both is invalid.");
	}

	internal static InvalidOperationException DynamicSQLJoinUnsupported()
	{
		return InvalidOperation("Dynamic SQL generation is not supported against multiple base tables.");
	}

	internal static InvalidOperationException DynamicSQLNoTableInfo()
	{
		return InvalidOperation("Dynamic SQL generation is not supported against a SelectCommand that does not return any base table information.");
	}

	internal static InvalidOperationException DynamicSQLNoKeyInfoDelete()
	{
		return InvalidOperation("Dynamic SQL generation for the DeleteCommand is not supported against a SelectCommand that does not return any key column information.");
	}

	internal static InvalidOperationException DynamicSQLNoKeyInfoUpdate()
	{
		return InvalidOperation("Dynamic SQL generation for the UpdateCommand is not supported against a SelectCommand that does not return any key column information.");
	}

	internal static InvalidOperationException DynamicSQLNoKeyInfoRowVersionDelete()
	{
		return InvalidOperation("Dynamic SQL generation for the DeleteCommand is not supported against a SelectCommand that does not contain a row version column.");
	}

	internal static InvalidOperationException DynamicSQLNoKeyInfoRowVersionUpdate()
	{
		return InvalidOperation("Dynamic SQL generation for the UpdateCommand is not supported against a SelectCommand that does not contain a row version column.");
	}

	internal static InvalidOperationException DynamicSQLNestedQuote(string name, string quote)
	{
		return InvalidOperation(global::SR.Format("Dynamic SQL generation not supported against table names '{0}' that contain the QuotePrefix or QuoteSuffix character '{1}'.", name, quote));
	}

	internal static InvalidOperationException NoQuoteChange()
	{
		return InvalidOperation("The QuotePrefix and QuoteSuffix properties cannot be changed once an Insert, Update, or Delete command has been generated.");
	}

	internal static InvalidOperationException MissingSourceCommand()
	{
		return InvalidOperation("The DataAdapter.SelectCommand property needs to be initialized.");
	}

	internal static InvalidOperationException MissingSourceCommandConnection()
	{
		return InvalidOperation("The DataAdapter.SelectCommand.Connection property needs to be initialized;");
	}

	internal static DataRow[] SelectAdapterRows(DataTable dataTable, bool sorted)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		DataRowCollection rows = dataTable.Rows;
		IEnumerator enumerator = rows.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				switch (((DataRow)enumerator.Current).RowState)
				{
				case DataRowState.Added:
					num++;
					break;
				case DataRowState.Deleted:
					num2++;
					break;
				case DataRowState.Modified:
					num3++;
					break;
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		DataRow[] array = new DataRow[num + num2 + num3];
		if (sorted)
		{
			num3 = num + num2;
			num2 = num;
			num = 0;
			foreach (DataRow item in rows)
			{
				switch (item.RowState)
				{
				case DataRowState.Added:
					array[num++] = item;
					break;
				case DataRowState.Deleted:
					array[num2++] = item;
					break;
				case DataRowState.Modified:
					array[num3++] = item;
					break;
				}
			}
		}
		else
		{
			int num4 = 0;
			foreach (DataRow item2 in rows)
			{
				if ((item2.RowState & (DataRowState.Added | DataRowState.Deleted | DataRowState.Modified)) != 0)
				{
					array[num4++] = item2;
					if (num4 == array.Length)
					{
						break;
					}
				}
			}
		}
		return array;
	}

	internal static void BuildSchemaTableInfoTableNames(string[] columnNameArray)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(columnNameArray.Length);
		int num = columnNameArray.Length;
		int num2 = columnNameArray.Length - 1;
		while (0 <= num2)
		{
			string text = columnNameArray[num2];
			if (text != null && 0 < text.Length)
			{
				text = text.ToLower(CultureInfo.InvariantCulture);
				if (dictionary.TryGetValue(text, out var value))
				{
					num = Math.Min(num, value);
				}
				dictionary[text] = num2;
			}
			else
			{
				columnNameArray[num2] = string.Empty;
				num = num2;
			}
			num2--;
		}
		int uniqueIndex = 1;
		for (int i = num; i < columnNameArray.Length; i++)
		{
			string text2 = columnNameArray[i];
			if (text2.Length == 0)
			{
				columnNameArray[i] = "Column";
				uniqueIndex = GenerateUniqueName(dictionary, ref columnNameArray[i], i, uniqueIndex);
				continue;
			}
			text2 = text2.ToLower(CultureInfo.InvariantCulture);
			if (i != dictionary[text2])
			{
				GenerateUniqueName(dictionary, ref columnNameArray[i], i, 1);
			}
		}
	}

	private static int GenerateUniqueName(Dictionary<string, int> hash, ref string columnName, int index, int uniqueIndex)
	{
		string text;
		while (true)
		{
			text = columnName + uniqueIndex.ToString(CultureInfo.InvariantCulture);
			string key = text.ToLower(CultureInfo.InvariantCulture);
			if (hash.TryAdd(key, index))
			{
				break;
			}
			uniqueIndex++;
		}
		columnName = text;
		return uniqueIndex;
	}

	internal static int SrcCompare(string strA, string strB)
	{
		if (!(strA == strB))
		{
			return 1;
		}
		return 0;
	}

	internal static Exception ExceptionWithStackTrace(Exception e)
	{
		try
		{
			throw e;
		}
		catch (Exception result)
		{
			return result;
		}
	}

	internal static IndexOutOfRangeException IndexOutOfRange(int value)
	{
		return new IndexOutOfRangeException(value.ToString(CultureInfo.InvariantCulture));
	}

	internal static IndexOutOfRangeException IndexOutOfRange()
	{
		return new IndexOutOfRangeException();
	}

	internal static TimeoutException TimeoutException(string error)
	{
		return new TimeoutException(error);
	}

	internal static InvalidOperationException InvalidOperation(string error, Exception inner)
	{
		return new InvalidOperationException(error, inner);
	}

	internal static OverflowException Overflow(string error)
	{
		return Overflow(error, null);
	}

	internal static OverflowException Overflow(string error, Exception inner)
	{
		return new OverflowException(error, inner);
	}

	internal static TypeLoadException TypeLoad(string error)
	{
		TypeLoadException ex = new TypeLoadException(error);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static PlatformNotSupportedException DbTypeNotSupported(string dbType)
	{
		return new PlatformNotSupportedException(global::SR.GetString("Type {0} is not supported on this platform.", dbType));
	}

	internal static InvalidCastException InvalidCast()
	{
		return new InvalidCastException();
	}

	internal static IOException IO(string error)
	{
		return new IOException(error);
	}

	internal static IOException IO(string error, Exception inner)
	{
		return new IOException(error, inner);
	}

	internal static ObjectDisposedException ObjectDisposed(object instance)
	{
		return new ObjectDisposedException(instance.GetType().Name);
	}

	internal static Exception DataTableDoesNotExist(string collectionName)
	{
		return Argument(global::SR.GetString("The collection '{0}' is missing from the metadata XML.", collectionName));
	}

	internal static InvalidOperationException MethodCalledTwice(string method)
	{
		return new InvalidOperationException(global::SR.GetString("The method '{0}' cannot be called more than once for the same execution.", method));
	}

	internal static ArgumentOutOfRangeException InvalidCommandType(CommandType value)
	{
		return InvalidEnumerationValue(typeof(CommandType), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidIsolationLevel(IsolationLevel value)
	{
		return InvalidEnumerationValue(typeof(IsolationLevel), (int)value);
	}

	internal static ArgumentOutOfRangeException InvalidParameterDirection(ParameterDirection value)
	{
		return InvalidEnumerationValue(typeof(ParameterDirection), (int)value);
	}

	internal static Exception TooManyRestrictions(string collectionName)
	{
		return Argument(global::SR.GetString("More restrictions were provided than the requested schema ('{0}') supports.", collectionName));
	}

	internal static ArgumentOutOfRangeException InvalidUpdateRowSource(UpdateRowSource value)
	{
		return InvalidEnumerationValue(typeof(UpdateRowSource), (int)value);
	}

	internal static ArgumentException InvalidMinMaxPoolSizeValues()
	{
		return Argument(global::SR.GetString("Invalid min or max pool size values, min pool size cannot be greater than the max pool size."));
	}

	internal static InvalidOperationException NoConnectionString()
	{
		return InvalidOperation(global::SR.GetString("The ConnectionString property has not been initialized."));
	}

	internal static Exception MethodNotImplemented([CallerMemberName] string methodName = "")
	{
		return System.NotImplemented.ByDesignWithMessage(methodName);
	}

	internal static Exception QueryFailed(string collectionName, Exception e)
	{
		return InvalidOperation(global::SR.GetString("Unable to build the '{0}' collection because execution of the SQL query failed. See the inner exception for details.", collectionName), e);
	}

	internal static Exception InvalidConnectionOptionValueLength(string key, int limit)
	{
		return Argument(global::SR.GetString("The value's length for key '{0}' exceeds it's limit of '{1}'.", key, limit));
	}

	internal static Exception MissingConnectionOptionValue(string key, string requiredAdditionalKey)
	{
		return Argument(global::SR.GetString("Use of key '{0}' requires the key '{1}' to be present.", key, requiredAdditionalKey));
	}

	internal static Exception PooledOpenTimeout()
	{
		return InvalidOperation(global::SR.GetString("Timeout expired.  The timeout period elapsed prior to obtaining a connection from the pool.  This may have occurred because all pooled connections were in use and max pool size was reached."));
	}

	internal static Exception NonPooledOpenTimeout()
	{
		return TimeoutException(global::SR.GetString("Timeout attempting to open the connection.  The time period elapsed prior to attempting to open the connection has been exceeded.  This may have occurred because of too many simultaneous non-pooled connection attempts."));
	}

	internal static InvalidOperationException TransactionConnectionMismatch()
	{
		return Provider(global::SR.GetString("The transaction is either not associated with the current connection or has been completed."));
	}

	internal static InvalidOperationException TransactionRequired(string method)
	{
		return Provider(global::SR.GetString("{0} requires the command to have a transaction when the connection assigned to the command is in a pending local transaction.  The Transaction property of the command has not been initialized.", method));
	}

	internal static Exception CommandTextRequired(string method)
	{
		return InvalidOperation(global::SR.GetString("{0}: CommandText property has not been initialized", method));
	}

	internal static Exception NoColumns()
	{
		return Argument(global::SR.GetString("The schema table contains no columns."));
	}

	internal static InvalidOperationException ConnectionRequired(string method)
	{
		return InvalidOperation(global::SR.GetString("{0}: Connection property has not been initialized.", method));
	}

	internal static InvalidOperationException OpenConnectionRequired(string method, ConnectionState state)
	{
		return InvalidOperation(global::SR.GetString("{0} requires an open and available Connection. {1}", method, ConnectionStateMsg(state)));
	}

	internal static Exception OpenReaderExists()
	{
		return OpenReaderExists(null);
	}

	internal static Exception OpenReaderExists(Exception e)
	{
		return InvalidOperation(global::SR.GetString("There is already an open DataReader associated with this Command which must be closed first."), e);
	}

	internal static Exception NonSeqByteAccess(long badIndex, long currIndex, string method)
	{
		return InvalidOperation(global::SR.GetString("Invalid {2} attempt at dataIndex '{0}'.  With CommandBehavior.SequentialAccess, you may only read from dataIndex '{1}' or greater.", badIndex.ToString(CultureInfo.InvariantCulture), currIndex.ToString(CultureInfo.InvariantCulture), method));
	}

	internal static Exception InvalidXml()
	{
		return Argument(global::SR.GetString("The metadata XML is invalid."));
	}

	internal static Exception NegativeParameter(string parameterName)
	{
		return InvalidOperation(global::SR.GetString("Invalid value for argument '{0}'. The value must be greater than or equal to 0.", parameterName));
	}

	internal static Exception InvalidXmlMissingColumn(string collectionName, string columnName)
	{
		return Argument(global::SR.GetString("The metadata XML is invalid. The {0} collection must contain a {1} column and it must be a string column.", collectionName, columnName));
	}

	internal static Exception InvalidMetaDataValue()
	{
		return Argument(global::SR.GetString("Invalid value for this metadata."));
	}

	internal static InvalidOperationException NonSequentialColumnAccess(int badCol, int currCol)
	{
		return InvalidOperation(global::SR.GetString("Invalid attempt to read from column ordinal '{0}'.  With CommandBehavior.SequentialAccess, you may only read from column ordinal '{1}' or greater.", badCol.ToString(CultureInfo.InvariantCulture), currCol.ToString(CultureInfo.InvariantCulture)));
	}

	internal static Exception InvalidXmlInvalidValue(string collectionName, string columnName)
	{
		return Argument(global::SR.GetString("The metadata XML is invalid. The {1} column of the {0} collection must contain a non-empty string.", collectionName, columnName));
	}

	internal static Exception CollectionNameIsNotUnique(string collectionName)
	{
		return Argument(global::SR.GetString("There are multiple collections named '{0}'.", collectionName));
	}

	internal static Exception InvalidCommandTimeout(int value, [CallerMemberName] string property = "")
	{
		return Argument(global::SR.GetString("Invalid CommandTimeout value {0}; the value must be >= 0.", value.ToString(CultureInfo.InvariantCulture)), property);
	}

	internal static Exception UninitializedParameterSize(int index, Type dataType)
	{
		return InvalidOperation(global::SR.GetString("{1}[{0}]: the Size property has an invalid size of 0.", index.ToString(CultureInfo.InvariantCulture), dataType.Name));
	}

	internal static Exception UnableToBuildCollection(string collectionName)
	{
		return Argument(global::SR.GetString("Unable to build schema collection '{0}';", collectionName));
	}

	internal static Exception PrepareParameterType(DbCommand cmd)
	{
		return InvalidOperation(global::SR.GetString("{0}.Prepare method requires all parameters to have an explicitly set type.", cmd.GetType().Name));
	}

	internal static Exception UndefinedCollection(string collectionName)
	{
		return Argument(global::SR.GetString("The requested collection ({0}) is not defined.", collectionName));
	}

	internal static Exception UnsupportedVersion(string collectionName)
	{
		return Argument(global::SR.GetString(" requested collection ({0}) is not supported by this version of the provider.", collectionName));
	}

	internal static Exception AmbigousCollectionName(string collectionName)
	{
		return Argument(global::SR.GetString("The collection name '{0}' matches at least two collections with the same name but with different case, but does not match any of them exactly.", collectionName));
	}

	internal static Exception PrepareParameterSize(DbCommand cmd)
	{
		return InvalidOperation(global::SR.GetString("{0}.Prepare method requires all variable length parameters to have an explicitly set non-zero Size.", cmd.GetType().Name));
	}

	internal static Exception PrepareParameterScale(DbCommand cmd, string type)
	{
		return InvalidOperation(global::SR.GetString("{0}.Prepare method requires parameters of type '{1}' have an explicitly set Precision and Scale.", cmd.GetType().Name, type));
	}

	internal static Exception MissingDataSourceInformationColumn()
	{
		return Argument(global::SR.GetString("One of the required DataSourceInformation tables columns is missing."));
	}

	internal static Exception IncorrectNumberOfDataSourceInformationRows()
	{
		return Argument(global::SR.GetString("The DataSourceInformation table must contain exactly one row."));
	}

	internal static Exception MismatchedAsyncResult(string expectedMethod, string gotMethod)
	{
		return InvalidOperation(global::SR.GetString("Mismatched end method call for asyncResult.  Expected call to {0} but {1} was called instead.", expectedMethod, gotMethod));
	}

	internal static Exception ClosedConnectionError()
	{
		return InvalidOperation(global::SR.GetString("Invalid operation. The connection is closed."));
	}

	internal static Exception ConnectionAlreadyOpen(ConnectionState state)
	{
		return InvalidOperation(global::SR.GetString("The connection was not closed. {0}", ConnectionStateMsg(state)));
	}

	internal static Exception TransactionPresent()
	{
		return InvalidOperation(global::SR.GetString("Connection currently has transaction enlisted.  Finish current transaction and retry."));
	}

	internal static Exception LocalTransactionPresent()
	{
		return InvalidOperation(global::SR.GetString("Cannot enlist in the transaction because a local transaction is in progress on the connection.  Finish local transaction and retry."));
	}

	internal static Exception OpenConnectionPropertySet(string property, ConnectionState state)
	{
		return InvalidOperation(global::SR.GetString("Not allowed to change the '{0}' property. {1}", property, ConnectionStateMsg(state)));
	}

	internal static Exception EmptyDatabaseName()
	{
		return Argument(global::SR.GetString("Database cannot be null, the empty string, or string of only whitespace."));
	}

	internal static Exception MissingRestrictionColumn()
	{
		return Argument(global::SR.GetString("One or more of the required columns of the restrictions collection is missing."));
	}

	internal static Exception InternalConnectionError(ConnectionError internalError)
	{
		return InvalidOperation(global::SR.GetString("Internal DbConnection Error: {0}", (int)internalError));
	}

	internal static Exception InvalidConnectRetryCountValue()
	{
		return Argument(global::SR.GetString("Invalid ConnectRetryCount value (should be 0-255)."));
	}

	internal static Exception MissingRestrictionRow()
	{
		return Argument(global::SR.GetString("A restriction exists for which there is no matching row in the restrictions collection."));
	}

	internal static Exception InvalidConnectRetryIntervalValue()
	{
		return Argument(global::SR.GetString("Invalid ConnectRetryInterval value (should be 1-60)."));
	}

	internal static InvalidOperationException AsyncOperationPending()
	{
		return InvalidOperation(global::SR.GetString("Can not start another operation while there is an asynchronous operation pending."));
	}

	internal static IOException ErrorReadingFromStream(Exception internalException)
	{
		return IO(global::SR.GetString("An error occurred while reading."), internalException);
	}

	internal static ArgumentException InvalidDataType(TypeCode typecode)
	{
		return Argument(global::SR.GetString("The parameter data type of {0} is invalid.", typecode.ToString()));
	}

	internal static ArgumentException UnknownDataType(Type dataType)
	{
		return Argument(global::SR.GetString("No mapping exists from object type {0} to a known managed provider native type.", dataType.FullName));
	}

	internal static ArgumentException DbTypeNotSupported(DbType type, Type enumtype)
	{
		return Argument(global::SR.GetString("No mapping exists from DbType {0} to a known {1}.", type.ToString(), enumtype.Name));
	}

	internal static ArgumentException UnknownDataTypeCode(Type dataType, TypeCode typeCode)
	{
		object[] array = new object[2];
		int num = (int)typeCode;
		array[0] = num.ToString(CultureInfo.InvariantCulture);
		array[1] = dataType.FullName;
		return Argument(global::SR.GetString("Unable to handle an unknown TypeCode {0} returned by Type {1}.", array));
	}

	internal static ArgumentException InvalidOffsetValue(int value)
	{
		return Argument(global::SR.GetString("Invalid parameter Offset value '{0}'. The value must be greater than or equal to 0.", value.ToString(CultureInfo.InvariantCulture)));
	}

	internal static ArgumentException InvalidSizeValue(int value)
	{
		return Argument(global::SR.GetString("Invalid parameter Size value '{0}'. The value must be greater than or equal to 0.", value.ToString(CultureInfo.InvariantCulture)));
	}

	internal static ArgumentException ParameterValueOutOfRange(decimal value)
	{
		return Argument(global::SR.GetString("Parameter value '{0}' is out of range.", value.ToString((IFormatProvider)null)));
	}

	internal static ArgumentException ParameterValueOutOfRange(SqlDecimal value)
	{
		return Argument(global::SR.GetString("Parameter value '{0}' is out of range.", value.ToString()));
	}

	internal static ArgumentException VersionDoesNotSupportDataType(string typeName)
	{
		return Argument(global::SR.GetString("The version of SQL Server in use does not support datatype '{0}'.", typeName));
	}

	internal static Exception ParameterConversionFailed(object value, Type destType, Exception inner)
	{
		string message = global::SR.GetString("Failed to convert parameter value from a {0} to a {1}.", value.GetType().Name, destType.Name);
		if (inner is ArgumentException)
		{
			return new ArgumentException(message, inner);
		}
		if (inner is FormatException)
		{
			return new FormatException(message, inner);
		}
		if (inner is InvalidCastException)
		{
			return new InvalidCastException(message, inner);
		}
		if (inner is OverflowException)
		{
			return new OverflowException(message, inner);
		}
		return inner;
	}

	internal static Exception ParametersMappingIndex(int index, DbParameterCollection collection)
	{
		return CollectionIndexInt32(index, collection.GetType(), collection.Count);
	}

	internal static Exception ParametersSourceIndex(string parameterName, DbParameterCollection collection, Type parameterType)
	{
		return CollectionIndexString(parameterType, "ParameterName", parameterName, collection.GetType());
	}

	internal static Exception ParameterNull(string parameter, DbParameterCollection collection, Type parameterType)
	{
		return CollectionNullValue(parameter, collection.GetType(), parameterType);
	}

	internal static Exception UndefinedPopulationMechanism(string populationMechanism)
	{
		throw new NotImplementedException();
	}

	internal static Exception InvalidParameterType(DbParameterCollection collection, Type parameterType, object invalidValue)
	{
		return CollectionInvalidType(collection.GetType(), parameterType, invalidValue);
	}

	internal static Exception ParallelTransactionsNotSupported(DbConnection obj)
	{
		return InvalidOperation(global::SR.GetString("{0} does not support parallel transactions.", obj.GetType().Name));
	}

	internal static Exception TransactionZombied(DbTransaction obj)
	{
		return InvalidOperation(global::SR.GetString("This {0} has completed; it is no longer usable.", obj.GetType().Name));
	}

	internal static Delegate FindBuilder(MulticastDelegate mcd)
	{
		if ((object)mcd != null)
		{
			Delegate[] invocationList = mcd.GetInvocationList();
			foreach (Delegate obj in invocationList)
			{
				if (obj.Target is DbCommandBuilder)
				{
					return obj;
				}
			}
		}
		return null;
	}

	internal static void TimerCurrent(out long ticks)
	{
		ticks = DateTime.UtcNow.ToFileTimeUtc();
	}

	internal static long TimerCurrent()
	{
		return DateTime.UtcNow.ToFileTimeUtc();
	}

	internal static long TimerFromSeconds(int seconds)
	{
		checked
		{
			return unchecked((long)seconds) * 10000000L;
		}
	}

	internal static long TimerFromMilliseconds(long milliseconds)
	{
		return checked(milliseconds * 10000);
	}

	internal static bool TimerHasExpired(long timerExpire)
	{
		return TimerCurrent() > timerExpire;
	}

	internal static long TimerRemaining(long timerExpire)
	{
		long num = TimerCurrent();
		return checked(timerExpire - num);
	}

	internal static long TimerRemainingMilliseconds(long timerExpire)
	{
		return TimerToMilliseconds(TimerRemaining(timerExpire));
	}

	internal static long TimerRemainingSeconds(long timerExpire)
	{
		return TimerToSeconds(TimerRemaining(timerExpire));
	}

	internal static long TimerToMilliseconds(long timerValue)
	{
		return timerValue / 10000;
	}

	private static long TimerToSeconds(long timerValue)
	{
		return timerValue / 10000000;
	}

	internal static string MachineName()
	{
		return Environment.MachineName;
	}

	internal static Transaction GetCurrentTransaction()
	{
		return Transaction.Current;
	}

	internal static bool IsDirection(DbParameter value, ParameterDirection condition)
	{
		return condition == (condition & value.Direction);
	}

	internal static void IsNullOrSqlType(object value, out bool isNull, out bool isSqlType)
	{
		if (value == null || value == DBNull.Value)
		{
			isNull = true;
			isSqlType = false;
		}
		else if (value is INullable nullable)
		{
			isNull = nullable.IsNull;
			isSqlType = value is SqlBinary || value is SqlBoolean || value is SqlByte || value is SqlBytes || value is SqlChars || value is SqlDateTime || value is SqlDecimal || value is SqlDouble || value is SqlGuid || value is SqlInt16 || value is SqlInt32 || value is SqlInt64 || value is SqlMoney || value is SqlSingle || value is SqlString;
		}
		else
		{
			isNull = false;
			isSqlType = false;
		}
	}

	internal static Version GetAssemblyVersion()
	{
		if (s_systemDataVersion == null)
		{
			s_systemDataVersion = new Version("4.0.50524.0");
		}
		return s_systemDataVersion;
	}

	internal static bool IsAzureSqlServerEndpoint(string dataSource)
	{
		int num = dataSource.LastIndexOf(',');
		if (num >= 0)
		{
			dataSource = dataSource.Substring(0, num);
		}
		num = dataSource.LastIndexOf('\\');
		if (num >= 0)
		{
			dataSource = dataSource.Substring(0, num);
		}
		dataSource = dataSource.Trim();
		for (num = 0; num < AzureSqlServerEndpoints.Length; num++)
		{
			if (dataSource.EndsWith(AzureSqlServerEndpoints[num], StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	internal static ArgumentOutOfRangeException InvalidDataRowVersion(DataRowVersion value)
	{
		return InvalidEnumerationValue(typeof(DataRowVersion), (int)value);
	}

	internal static ArgumentException SingleValuedProperty(string propertyName, string value)
	{
		ArgumentException ex = new ArgumentException(global::SR.GetString("The only acceptable value for the property '{0}' is '{1}'.", propertyName, value));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException DoubleValuedProperty(string propertyName, string value1, string value2)
	{
		ArgumentException ex = new ArgumentException(global::SR.GetString("The acceptable values for the property '{0}' are '{1}' or '{2}'.", propertyName, value1, value2));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException InvalidPrefixSuffix()
	{
		ArgumentException ex = new ArgumentException(global::SR.GetString("Specified QuotePrefix and QuoteSuffix values do not match."));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentOutOfRangeException InvalidCommandBehavior(CommandBehavior value)
	{
		return InvalidEnumerationValue(typeof(CommandBehavior), (int)value);
	}

	internal static void ValidateCommandBehavior(CommandBehavior value)
	{
		if (value < CommandBehavior.Default || (CommandBehavior.SingleResult | CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo | CommandBehavior.SingleRow | CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection) < value)
		{
			throw InvalidCommandBehavior(value);
		}
	}

	internal static ArgumentOutOfRangeException NotSupportedCommandBehavior(CommandBehavior value, string method)
	{
		return NotSupportedEnumerationValue(typeof(CommandBehavior), value.ToString(), method);
	}

	internal static ArgumentException BadParameterName(string parameterName)
	{
		ArgumentException ex = new ArgumentException(global::SR.GetString("Specified parameter name '{0}' is not valid.", parameterName));
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static Exception DeriveParametersNotSupported(IDbCommand value)
	{
		return DataAdapter(global::SR.GetString("{0} DeriveParameters only supports CommandType.StoredProcedure, not CommandType. {1}.", value.GetType().Name, value.CommandType.ToString()));
	}

	internal static Exception NoStoredProcedureExists(string sproc)
	{
		return InvalidOperation(global::SR.GetString("The stored procedure '{0}' doesn't exist.", sproc));
	}

	internal static InvalidOperationException TransactionCompletedButNotDisposed()
	{
		return Provider(global::SR.GetString("The transaction associated with the current connection has completed but has not been disposed.  The transaction must be disposed before the connection can be used to execute SQL statements."));
	}

	internal static ArgumentOutOfRangeException InvalidUserDefinedTypeSerializationFormat(Format value)
	{
		return InvalidEnumerationValue(typeof(Format), (int)value);
	}

	internal static ArgumentOutOfRangeException NotSupportedUserDefinedTypeSerializationFormat(Format value, string method)
	{
		return NotSupportedEnumerationValue(typeof(Format), value.ToString(), method);
	}

	internal static ArgumentOutOfRangeException ArgumentOutOfRange(string message, string parameterName, object value)
	{
		ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException(parameterName, value, message);
		TraceExceptionAsReturnValue(ex);
		return ex;
	}

	internal static ArgumentException InvalidArgumentLength(string argumentName, int limit)
	{
		return Argument(global::SR.GetString("The length of argument '{0}' exceeds its limit of '{1}'.", argumentName, limit));
	}

	internal static ArgumentException MustBeReadOnly(string argumentName)
	{
		return Argument(global::SR.GetString("{0} must be marked as read only.", argumentName));
	}

	internal static InvalidOperationException InvalidMixedUsageOfSecureAndClearCredential()
	{
		return InvalidOperation(global::SR.GetString("Cannot use Credential with UserID, UID, Password, or PWD connection string keywords."));
	}

	internal static ArgumentException InvalidMixedArgumentOfSecureAndClearCredential()
	{
		return Argument(global::SR.GetString("Cannot use Credential with UserID, UID, Password, or PWD connection string keywords."));
	}

	internal static InvalidOperationException InvalidMixedUsageOfSecureCredentialAndIntegratedSecurity()
	{
		return InvalidOperation(global::SR.GetString("Cannot use Credential with Integrated Security connection string keyword."));
	}

	internal static ArgumentException InvalidMixedArgumentOfSecureCredentialAndIntegratedSecurity()
	{
		return Argument(global::SR.GetString("Cannot use Credential with Integrated Security connection string keyword."));
	}

	internal static InvalidOperationException InvalidMixedUsageOfAccessTokenAndIntegratedSecurity()
	{
		return InvalidOperation(global::SR.GetString("Cannot set the AccessToken property if the 'Integrated Security' connection string keyword has been set to 'true' or 'SSPI'."));
	}

	internal static InvalidOperationException InvalidMixedUsageOfAccessTokenAndUserIDPassword()
	{
		return InvalidOperation(global::SR.GetString("Cannot set the AccessToken property if 'UserID', 'UID', 'Password', or 'PWD' has been specified in connection string."));
	}

	internal static Exception InvalidMixedUsageOfCredentialAndAccessToken()
	{
		return InvalidOperation(global::SR.GetString("Cannot set the Credential property if the AccessToken property is already set."));
	}

	internal static bool NeedManualEnlistment()
	{
		return false;
	}

	internal static bool IsEmpty(string str)
	{
		return string.IsNullOrEmpty(str);
	}

	internal static Exception DatabaseNameTooLong()
	{
		return Argument(global::SR.GetString("The argument is too long."));
	}

	internal static int StringLength(string inputString)
	{
		return inputString?.Length ?? 0;
	}

	internal static Exception NumericToDecimalOverflow()
	{
		return InvalidCast(global::SR.GetString("The numerical value is too large to fit into a 96 bit decimal."));
	}

	internal static Exception OdbcNoTypesFromProvider()
	{
		return InvalidOperation(global::SR.GetString("The ODBC provider did not return results from SQLGETTYPEINFO."));
	}

	internal static ArgumentException InvalidRestrictionValue(string collectionName, string restrictionName, string restrictionValue)
	{
		return Argument(global::SR.GetString("'{2}' is not a valid value for the '{1}' restriction of the '{0}' schema collection.", collectionName, restrictionName, restrictionValue));
	}

	internal static Exception DataReaderNoData()
	{
		return InvalidOperation(global::SR.GetString("No data exists for the row/column."));
	}

	internal static Exception ConnectionIsDisabled(Exception InnerException)
	{
		return InvalidOperation(global::SR.GetString("The connection has been disabled."), InnerException);
	}

	internal static Exception OffsetOutOfRangeException()
	{
		return InvalidOperation(global::SR.GetString("Offset must refer to a location within the value."));
	}

	internal static InvalidOperationException QuotePrefixNotSet(string method)
	{
		return InvalidOperation(Res.GetString("{0} requires open connection when the quote prefix has not been set.", method));
	}

	internal static string GetFullPath(string filename)
	{
		return Path.GetFullPath(filename);
	}

	internal static InvalidOperationException InvalidDataDirectory()
	{
		return InvalidOperation(global::SR.GetString("The DataDirectory substitute is not a string."));
	}

	internal static void EscapeSpecialCharacters(string unescapedString, StringBuilder escapedString)
	{
		foreach (char value in unescapedString)
		{
			if (".$^{[(|)*+?\\]".IndexOf(value) >= 0)
			{
				escapedString.Append("\\");
			}
			escapedString.Append(value);
		}
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	internal static IntPtr IntPtrOffset(IntPtr pbase, int offset)
	{
		checked
		{
			if (4 == PtrSize)
			{
				return (IntPtr)(pbase.ToInt32() + offset);
			}
			return (IntPtr)(pbase.ToInt64() + offset);
		}
	}

	internal static Exception InvalidXMLBadVersion()
	{
		return Argument(Res.GetString("Invalid Xml; can only parse elements of version one."));
	}

	internal static Exception NotAPermissionElement()
	{
		return Argument(Res.GetString("Given security element is not a permission element."));
	}

	internal static Exception PermissionTypeMismatch()
	{
		return Argument(Res.GetString("Type mismatch."));
	}

	internal static ArgumentOutOfRangeException InvalidPermissionState(PermissionState value)
	{
		return InvalidEnumerationValue(typeof(PermissionState), (int)value);
	}

	internal static ArgumentException ConfigProviderNotFound()
	{
		return Argument(Res.GetString("Unable to find the requested .Net Framework Data Provider.  It may not be installed."));
	}

	internal static InvalidOperationException ConfigProviderInvalid()
	{
		return InvalidOperation(Res.GetString("The requested .Net Framework Data Provider's implementation does not have an Instance field of a System.Data.Common.DbProviderFactory derived type."));
	}

	internal static Exception OleDb()
	{
		return new NotImplementedException("OleDb is not implemented.");
	}
}
