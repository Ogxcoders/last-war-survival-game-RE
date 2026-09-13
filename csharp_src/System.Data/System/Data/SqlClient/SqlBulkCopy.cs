using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace System.Data.SqlClient;

public sealed class SqlBulkCopy : IDisposable
{
	private enum ValueSourceType
	{
		Unspecified,
		IDataReader,
		DataTable,
		RowArray,
		DbDataReader
	}

	private enum ValueMethod : byte
	{
		GetValue,
		SqlTypeSqlDecimal,
		SqlTypeSqlDouble,
		SqlTypeSqlSingle,
		DataFeedStream,
		DataFeedText,
		DataFeedXml
	}

	private readonly struct SourceColumnMetadata
	{
		public readonly ValueMethod Method;

		public readonly bool IsSqlType;

		public readonly bool IsDataFeed;

		public SourceColumnMetadata(ValueMethod method, bool isSqlType, bool isDataFeed)
		{
			Method = method;
			IsSqlType = isSqlType;
			IsDataFeed = isDataFeed;
		}
	}

	private const int MetaDataResultId = 1;

	private const int CollationResultId = 2;

	private const int CollationId = 3;

	private const int MAX_LENGTH = int.MaxValue;

	private const int DefaultCommandTimeout = 30;

	private bool _enableStreaming;

	private int _batchSize;

	private bool _ownConnection;

	private SqlBulkCopyOptions _copyOptions;

	private int _timeout = 30;

	private string _destinationTableName;

	private int _rowsCopied;

	private int _notifyAfter;

	private int _rowsUntilNotification;

	private bool _insideRowsCopiedEvent;

	private object _rowSource;

	private SqlDataReader _SqlDataReaderRowSource;

	private DbDataReader _DbDataReaderRowSource;

	private DataTable _dataTableSource;

	private SqlBulkCopyColumnMappingCollection _columnMappings;

	private SqlBulkCopyColumnMappingCollection _localColumnMappings;

	private SqlConnection _connection;

	private SqlTransaction _internalTransaction;

	private SqlTransaction _externalTransaction;

	private ValueSourceType _rowSourceType;

	private DataRow _currentRow;

	private int _currentRowLength;

	private DataRowState _rowStateToSkip;

	private IEnumerator _rowEnumerator;

	private TdsParser _parser;

	private TdsParserStateObject _stateObj;

	private List<_ColumnMapping> _sortedColumnMappings;

	private SqlRowsCopiedEventHandler _rowsCopiedEventHandler;

	private int _savedBatchSize;

	private bool _hasMoreRowToCopy;

	private bool _isAsyncBulkCopy;

	private bool _isBulkCopyingInProgress;

	private SqlInternalConnectionTds.SyncAsyncLock _parserLock;

	private SourceColumnMetadata[] _currentRowMetadata;

	public int BatchSize
	{
		get
		{
			return _batchSize;
		}
		set
		{
			if (value >= 0)
			{
				_batchSize = value;
				return;
			}
			throw ADP.ArgumentOutOfRange("BatchSize");
		}
	}

	public int BulkCopyTimeout
	{
		get
		{
			return _timeout;
		}
		set
		{
			if (value < 0)
			{
				throw SQL.BulkLoadInvalidTimeout(value);
			}
			_timeout = value;
		}
	}

	public bool EnableStreaming
	{
		get
		{
			return _enableStreaming;
		}
		set
		{
			_enableStreaming = value;
		}
	}

	public SqlBulkCopyColumnMappingCollection ColumnMappings => _columnMappings;

	public string DestinationTableName
	{
		get
		{
			return _destinationTableName;
		}
		set
		{
			if (value == null)
			{
				throw ADP.ArgumentNull("DestinationTableName");
			}
			if (value.Length == 0)
			{
				throw ADP.ArgumentOutOfRange("DestinationTableName");
			}
			_destinationTableName = value;
		}
	}

	public int NotifyAfter
	{
		get
		{
			return _notifyAfter;
		}
		set
		{
			if (value >= 0)
			{
				_notifyAfter = value;
				return;
			}
			throw ADP.ArgumentOutOfRange("NotifyAfter");
		}
	}

	internal SqlStatistics Statistics
	{
		get
		{
			if (_connection != null && _connection.StatisticsEnabled)
			{
				return _connection.Statistics;
			}
			return null;
		}
	}

	public event SqlRowsCopiedEventHandler SqlRowsCopied
	{
		add
		{
			_rowsCopiedEventHandler = (SqlRowsCopiedEventHandler)Delegate.Combine(_rowsCopiedEventHandler, value);
		}
		remove
		{
			_rowsCopiedEventHandler = (SqlRowsCopiedEventHandler)Delegate.Remove(_rowsCopiedEventHandler, value);
		}
	}

	public SqlBulkCopy(SqlConnection connection)
	{
		if (connection == null)
		{
			throw ADP.ArgumentNull("connection");
		}
		_connection = connection;
		_columnMappings = new SqlBulkCopyColumnMappingCollection();
	}

	public SqlBulkCopy(SqlConnection connection, SqlBulkCopyOptions copyOptions, SqlTransaction externalTransaction)
		: this(connection)
	{
		_copyOptions = copyOptions;
		if (externalTransaction != null && IsCopyOption(SqlBulkCopyOptions.UseInternalTransaction))
		{
			throw SQL.BulkLoadConflictingTransactionOption();
		}
		if (!IsCopyOption(SqlBulkCopyOptions.UseInternalTransaction))
		{
			_externalTransaction = externalTransaction;
		}
	}

	public SqlBulkCopy(string connectionString)
	{
		if (connectionString == null)
		{
			throw ADP.ArgumentNull("connectionString");
		}
		_connection = new SqlConnection(connectionString);
		_columnMappings = new SqlBulkCopyColumnMappingCollection();
		_ownConnection = true;
	}

	public SqlBulkCopy(string connectionString, SqlBulkCopyOptions copyOptions)
		: this(connectionString)
	{
		_copyOptions = copyOptions;
	}

	void IDisposable.Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private bool IsCopyOption(SqlBulkCopyOptions copyOption)
	{
		return (_copyOptions & copyOption) == copyOption;
	}

	private string CreateInitialQuery()
	{
		string[] array;
		try
		{
			array = MultipartIdentifier.ParseMultipartIdentifier(DestinationTableName, "[\"", "]\"", "SqlBulkCopy.WriteToServer failed because the SqlBulkCopy.DestinationTableName is an invalid multipart name", ThrowOnEmptyMultipartName: true);
		}
		catch (Exception inner)
		{
			throw SQL.BulkLoadInvalidDestinationTable(DestinationTableName, inner);
		}
		if (string.IsNullOrEmpty(array[3]))
		{
			throw SQL.BulkLoadInvalidDestinationTable(DestinationTableName, null);
		}
		string text = "select @@trancount; SET FMTONLY ON select * from " + DestinationTableName + " SET FMTONLY OFF ";
		string text2 = ((!_connection.IsKatmaiOrNewer) ? "sp_tablecollations_90" : "sp_tablecollations_100");
		string text3 = array[3];
		bool num = text3.Length > 0 && '#' == text3[0];
		if (!string.IsNullOrEmpty(text3))
		{
			text3 = SqlServerEscapeHelper.EscapeStringAsLiteral(text3);
			text3 = SqlServerEscapeHelper.EscapeIdentifier(text3);
		}
		string text4 = array[2];
		if (!string.IsNullOrEmpty(text4))
		{
			text4 = SqlServerEscapeHelper.EscapeStringAsLiteral(text4);
			text4 = SqlServerEscapeHelper.EscapeIdentifier(text4);
		}
		string text5 = array[1];
		if (num && string.IsNullOrEmpty(text5))
		{
			return text + string.Format(null, "exec tempdb..{0} N'{1}.{2}'", text2, text4, text3);
		}
		if (!string.IsNullOrEmpty(text5))
		{
			text5 = SqlServerEscapeHelper.EscapeIdentifier(text5);
		}
		return text + string.Format(null, "exec {0}..{1} N'{2}.{3}'", text5, text2, text4, text3);
	}

	private Task<BulkCopySimpleResultSet> CreateAndExecuteInitialQueryAsync(out BulkCopySimpleResultSet result)
	{
		string text = CreateInitialQuery();
		Task task = _parser.TdsExecuteSQLBatch(text, BulkCopyTimeout, null, _stateObj, !_isAsyncBulkCopy, callerHasConnectionLock: true);
		if (task == null)
		{
			result = new BulkCopySimpleResultSet();
			RunParser(result);
			return null;
		}
		result = null;
		return task.ContinueWith(delegate(Task t)
		{
			if (t.IsFaulted)
			{
				throw t.Exception.InnerException;
			}
			BulkCopySimpleResultSet bulkCopySimpleResultSet = new BulkCopySimpleResultSet();
			RunParserReliably(bulkCopySimpleResultSet);
			return bulkCopySimpleResultSet;
		}, TaskScheduler.Default);
	}

	private string AnalyzeTargetAndCreateUpdateBulkCommand(BulkCopySimpleResultSet internalResults)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (internalResults[2].Count == 0)
		{
			throw SQL.BulkLoadNoCollation();
		}
		stringBuilder.AppendFormat("insert bulk {0} (", DestinationTableName);
		int num = 0;
		int num2 = 0;
		if (_connection.HasLocalTransaction && _externalTransaction == null && _internalTransaction == null && _connection.Parser != null && _connection.Parser.CurrentTransaction != null && _connection.Parser.CurrentTransaction.IsLocal)
		{
			throw SQL.BulkLoadExistingTransaction();
		}
		_SqlMetaDataSet metaData = internalResults[1].MetaData;
		_sortedColumnMappings = new List<_ColumnMapping>(metaData.Length);
		for (int i = 0; i < metaData.Length; i++)
		{
			_SqlMetaData sqlMetaData = metaData[i];
			bool flag = false;
			if (sqlMetaData.type == SqlDbType.Timestamp || (sqlMetaData.isIdentity && !IsCopyOption(SqlBulkCopyOptions.KeepIdentity)))
			{
				metaData[i] = null;
				flag = true;
			}
			int j;
			for (j = 0; j < _localColumnMappings.Count; j++)
			{
				if (_localColumnMappings[j]._destinationColumnOrdinal != sqlMetaData.ordinal && !(UnquotedName(_localColumnMappings[j]._destinationColumnName) == sqlMetaData.column))
				{
					continue;
				}
				if (flag)
				{
					num2++;
					break;
				}
				_sortedColumnMappings.Add(new _ColumnMapping(_localColumnMappings[j]._internalSourceColumnOrdinal, sqlMetaData));
				num++;
				if (num > 1)
				{
					stringBuilder.Append(", ");
				}
				if (sqlMetaData.type == SqlDbType.Variant)
				{
					AppendColumnNameAndTypeName(stringBuilder, sqlMetaData.column, "sql_variant");
				}
				else if (sqlMetaData.type == SqlDbType.Udt)
				{
					AppendColumnNameAndTypeName(stringBuilder, sqlMetaData.column, "varbinary");
				}
				else
				{
					AppendColumnNameAndTypeName(stringBuilder, sqlMetaData.column, sqlMetaData.type.ToString());
				}
				switch (sqlMetaData.metaType.NullableType)
				{
				case 106:
				case 108:
					stringBuilder.AppendFormat(null, "({0},{1})", sqlMetaData.precision, sqlMetaData.scale);
					break;
				case 240:
				{
					if (sqlMetaData.IsLargeUdt)
					{
						stringBuilder.Append("(max)");
						break;
					}
					int length = sqlMetaData.length;
					stringBuilder.AppendFormat(null, "({0})", length);
					break;
				}
				case 41:
				case 42:
				case 43:
					stringBuilder.AppendFormat(null, "({0})", sqlMetaData.scale);
					break;
				default:
					if (!sqlMetaData.metaType.IsFixed && !sqlMetaData.metaType.IsLong)
					{
						int num3 = sqlMetaData.length;
						byte nullableType = sqlMetaData.metaType.NullableType;
						if (nullableType == 99 || nullableType == 231 || nullableType == 239)
						{
							num3 /= 2;
						}
						stringBuilder.AppendFormat(null, "({0})", num3);
					}
					else if (sqlMetaData.metaType.IsPlp && sqlMetaData.metaType.SqlDbType != SqlDbType.Xml)
					{
						stringBuilder.Append("(max)");
					}
					break;
				}
				object obj = internalResults[2][i][3];
				bool flag2;
				switch (sqlMetaData.type)
				{
				case SqlDbType.Char:
				case SqlDbType.NChar:
				case SqlDbType.NText:
				case SqlDbType.NVarChar:
				case SqlDbType.Text:
				case SqlDbType.VarChar:
					flag2 = true;
					break;
				default:
					flag2 = false;
					break;
				}
				if (!(obj != null && flag2))
				{
					break;
				}
				SqlString sqlString = (SqlString)obj;
				if (sqlString.IsNull)
				{
					break;
				}
				stringBuilder.Append(" COLLATE " + sqlString.Value);
				if (_SqlDataReaderRowSource == null || sqlMetaData.collation == null)
				{
					break;
				}
				int internalSourceColumnOrdinal = _localColumnMappings[j]._internalSourceColumnOrdinal;
				int lCID = sqlMetaData.collation.LCID;
				int localeId = _SqlDataReaderRowSource.GetLocaleId(internalSourceColumnOrdinal);
				if (localeId == lCID)
				{
					break;
				}
				throw SQL.BulkLoadLcidMismatch(localeId, _SqlDataReaderRowSource.GetName(internalSourceColumnOrdinal), lCID, sqlMetaData.column);
			}
			if (j == _localColumnMappings.Count)
			{
				metaData[i] = null;
			}
		}
		if (num + num2 != _localColumnMappings.Count)
		{
			throw SQL.BulkLoadNonMatchingColumnMapping();
		}
		stringBuilder.Append(")");
		if ((_copyOptions & (SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.KeepNulls | SqlBulkCopyOptions.FireTriggers)) != SqlBulkCopyOptions.Default)
		{
			bool flag3 = false;
			stringBuilder.Append(" with (");
			if (IsCopyOption(SqlBulkCopyOptions.KeepNulls))
			{
				stringBuilder.Append("KEEP_NULLS");
				flag3 = true;
			}
			if (IsCopyOption(SqlBulkCopyOptions.TableLock))
			{
				stringBuilder.Append((flag3 ? ", " : "") + "TABLOCK");
				flag3 = true;
			}
			if (IsCopyOption(SqlBulkCopyOptions.CheckConstraints))
			{
				stringBuilder.Append((flag3 ? ", " : "") + "CHECK_CONSTRAINTS");
				flag3 = true;
			}
			if (IsCopyOption(SqlBulkCopyOptions.FireTriggers))
			{
				stringBuilder.Append((flag3 ? ", " : "") + "FIRE_TRIGGERS");
				flag3 = true;
			}
			stringBuilder.Append(")");
		}
		return stringBuilder.ToString();
	}

	private Task SubmitUpdateBulkCommand(string TDSCommand)
	{
		Task task = _parser.TdsExecuteSQLBatch(TDSCommand, BulkCopyTimeout, null, _stateObj, !_isAsyncBulkCopy, callerHasConnectionLock: true);
		if (task == null)
		{
			RunParser();
			return null;
		}
		return task.ContinueWith(delegate(Task t)
		{
			if (t.IsFaulted)
			{
				throw t.Exception.InnerException;
			}
			RunParserReliably();
		}, TaskScheduler.Default);
	}

	private void WriteMetaData(BulkCopySimpleResultSet internalResults)
	{
		_stateObj.SetTimeoutSeconds(BulkCopyTimeout);
		_SqlMetaDataSet metaData = internalResults[1].MetaData;
		_stateObj._outputMessageType = 7;
		_parser.WriteBulkCopyMetaData(metaData, _sortedColumnMappings.Count, _stateObj);
	}

	public void Close()
	{
		if (_insideRowsCopiedEvent)
		{
			throw SQL.InvalidOperationInsideEvent();
		}
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!disposing)
		{
			return;
		}
		_columnMappings = null;
		_parser = null;
		try
		{
			if (_internalTransaction != null)
			{
				_internalTransaction.Rollback();
				_internalTransaction.Dispose();
				_internalTransaction = null;
			}
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
		}
		finally
		{
			if (_connection != null)
			{
				if (_ownConnection)
				{
					_connection.Dispose();
				}
				_connection = null;
			}
		}
	}

	private object GetValueFromSourceRow(int destRowIndex, out bool isSqlType, out bool isDataFeed, out bool isNull)
	{
		_SqlMetaData metadata = _sortedColumnMappings[destRowIndex]._metadata;
		int sourceColumnOrdinal = _sortedColumnMappings[destRowIndex]._sourceColumnOrdinal;
		switch (_rowSourceType)
		{
		case ValueSourceType.IDataReader:
		case ValueSourceType.DbDataReader:
		{
			if (_currentRowMetadata[destRowIndex].IsDataFeed)
			{
				if (_DbDataReaderRowSource.IsDBNull(sourceColumnOrdinal))
				{
					isSqlType = false;
					isDataFeed = false;
					isNull = true;
					return DBNull.Value;
				}
				isSqlType = false;
				isDataFeed = true;
				isNull = false;
				switch (_currentRowMetadata[destRowIndex].Method)
				{
				case ValueMethod.DataFeedStream:
					return new StreamDataFeed(_DbDataReaderRowSource.GetStream(sourceColumnOrdinal));
				case ValueMethod.DataFeedText:
					return new TextDataFeed(_DbDataReaderRowSource.GetTextReader(sourceColumnOrdinal));
				case ValueMethod.DataFeedXml:
					return new XmlDataFeed(_SqlDataReaderRowSource.GetXmlReader(sourceColumnOrdinal));
				default:
				{
					isDataFeed = false;
					object value = _DbDataReaderRowSource.GetValue(sourceColumnOrdinal);
					ADP.IsNullOrSqlType(value, out isNull, out isSqlType);
					return value;
				}
				}
			}
			if (_SqlDataReaderRowSource != null)
			{
				if (_currentRowMetadata[destRowIndex].IsSqlType)
				{
					isSqlType = true;
					isDataFeed = false;
					INullable nullable = _currentRowMetadata[destRowIndex].Method switch
					{
						ValueMethod.SqlTypeSqlDecimal => _SqlDataReaderRowSource.GetSqlDecimal(sourceColumnOrdinal), 
						ValueMethod.SqlTypeSqlDouble => new SqlDecimal(_SqlDataReaderRowSource.GetSqlDouble(sourceColumnOrdinal).Value), 
						ValueMethod.SqlTypeSqlSingle => new SqlDecimal(_SqlDataReaderRowSource.GetSqlSingle(sourceColumnOrdinal).Value), 
						_ => (INullable)_SqlDataReaderRowSource.GetSqlValue(sourceColumnOrdinal), 
					};
					isNull = nullable.IsNull;
					return nullable;
				}
				isSqlType = false;
				isDataFeed = false;
				object value2 = _SqlDataReaderRowSource.GetValue(sourceColumnOrdinal);
				isNull = value2 == null || value2 == DBNull.Value;
				if (!isNull && metadata.type == SqlDbType.Udt)
				{
					isNull = (value2 as INullable)?.IsNull ?? false;
				}
				return value2;
			}
			isDataFeed = false;
			IDataReader dataReader = (IDataReader)_rowSource;
			if (_enableStreaming && _SqlDataReaderRowSource == null && dataReader.IsDBNull(sourceColumnOrdinal))
			{
				isSqlType = false;
				isNull = true;
				return DBNull.Value;
			}
			object value3 = dataReader.GetValue(sourceColumnOrdinal);
			ADP.IsNullOrSqlType(value3, out isNull, out isSqlType);
			return value3;
		}
		case ValueSourceType.DataTable:
		case ValueSourceType.RowArray:
		{
			isDataFeed = false;
			object obj = _currentRow[sourceColumnOrdinal];
			ADP.IsNullOrSqlType(obj, out isNull, out isSqlType);
			if (!isNull && _currentRowMetadata[destRowIndex].IsSqlType)
			{
				switch (_currentRowMetadata[destRowIndex].Method)
				{
				case ValueMethod.SqlTypeSqlSingle:
				{
					if (isSqlType)
					{
						return new SqlDecimal(((SqlSingle)obj).Value);
					}
					float num2 = (float)obj;
					if (!float.IsNaN(num2))
					{
						isSqlType = true;
						return new SqlDecimal(num2);
					}
					break;
				}
				case ValueMethod.SqlTypeSqlDouble:
				{
					if (isSqlType)
					{
						return new SqlDecimal(((SqlDouble)obj).Value);
					}
					double num = (double)obj;
					if (!double.IsNaN(num))
					{
						isSqlType = true;
						return new SqlDecimal(num);
					}
					break;
				}
				case ValueMethod.SqlTypeSqlDecimal:
					if (isSqlType)
					{
						return (SqlDecimal)obj;
					}
					isSqlType = true;
					return new SqlDecimal((decimal)obj);
				}
			}
			return obj;
		}
		default:
			throw ADP.NotSupported();
		}
	}

	private Task ReadFromRowSourceAsync(CancellationToken cts)
	{
		if (_isAsyncBulkCopy && _DbDataReaderRowSource != null)
		{
			return _DbDataReaderRowSource.ReadAsync(cts).ContinueWith(delegate(Task<bool> t)
			{
				if (t.Status == TaskStatus.RanToCompletion)
				{
					_hasMoreRowToCopy = t.Result;
				}
				return t;
			}, TaskScheduler.Default).Unwrap();
		}
		_hasMoreRowToCopy = false;
		try
		{
			_hasMoreRowToCopy = ReadFromRowSource();
		}
		catch (Exception exception)
		{
			if (_isAsyncBulkCopy)
			{
				return Task.FromException<bool>(exception);
			}
			throw;
		}
		return null;
	}

	private bool ReadFromRowSource()
	{
		switch (_rowSourceType)
		{
		case ValueSourceType.IDataReader:
		case ValueSourceType.DbDataReader:
			return ((IDataReader)_rowSource).Read();
		case ValueSourceType.DataTable:
		case ValueSourceType.RowArray:
			do
			{
				if (!_rowEnumerator.MoveNext())
				{
					return false;
				}
				_currentRow = (DataRow)_rowEnumerator.Current;
			}
			while ((_currentRow.RowState & _rowStateToSkip) != 0);
			_currentRowLength = _currentRow.ItemArray.Length;
			return true;
		default:
			throw ADP.NotSupported();
		}
	}

	private SourceColumnMetadata GetColumnMetadata(int ordinal)
	{
		int sourceColumnOrdinal = _sortedColumnMappings[ordinal]._sourceColumnOrdinal;
		_SqlMetaData metadata = _sortedColumnMappings[ordinal]._metadata;
		bool isDataFeed;
		bool isSqlType;
		ValueMethod method;
		if ((_SqlDataReaderRowSource != null || _dataTableSource != null) && (metadata.metaType.NullableType == 106 || metadata.metaType.NullableType == 108))
		{
			isDataFeed = false;
			Type type;
			switch (_rowSourceType)
			{
			case ValueSourceType.IDataReader:
			case ValueSourceType.DbDataReader:
				type = _SqlDataReaderRowSource.GetFieldType(sourceColumnOrdinal);
				break;
			case ValueSourceType.DataTable:
			case ValueSourceType.RowArray:
				type = _dataTableSource.Columns[sourceColumnOrdinal].DataType;
				break;
			default:
				type = null;
				break;
			}
			if (typeof(SqlDecimal) == type || typeof(decimal) == type)
			{
				isSqlType = true;
				method = ValueMethod.SqlTypeSqlDecimal;
			}
			else if (typeof(SqlDouble) == type || typeof(double) == type)
			{
				isSqlType = true;
				method = ValueMethod.SqlTypeSqlDouble;
			}
			else if (typeof(SqlSingle) == type || typeof(float) == type)
			{
				isSqlType = true;
				method = ValueMethod.SqlTypeSqlSingle;
			}
			else
			{
				isSqlType = false;
				method = ValueMethod.GetValue;
			}
		}
		else if (_enableStreaming && metadata.length == int.MaxValue)
		{
			isSqlType = false;
			if (_SqlDataReaderRowSource != null)
			{
				MetaType metaType = _SqlDataReaderRowSource.MetaData[sourceColumnOrdinal].metaType;
				if (metadata.type == SqlDbType.VarBinary && metaType.IsBinType && metaType.SqlDbType != SqlDbType.Timestamp && _SqlDataReaderRowSource.IsCommandBehavior(CommandBehavior.SequentialAccess))
				{
					isDataFeed = true;
					method = ValueMethod.DataFeedStream;
				}
				else if ((metadata.type == SqlDbType.VarChar || metadata.type == SqlDbType.NVarChar) && metaType.IsCharType && metaType.SqlDbType != SqlDbType.Xml)
				{
					isDataFeed = true;
					method = ValueMethod.DataFeedText;
				}
				else if (metadata.type == SqlDbType.Xml && metaType.SqlDbType == SqlDbType.Xml)
				{
					isDataFeed = true;
					method = ValueMethod.DataFeedXml;
				}
				else
				{
					isDataFeed = false;
					method = ValueMethod.GetValue;
				}
			}
			else if (_DbDataReaderRowSource != null)
			{
				if (metadata.type == SqlDbType.VarBinary)
				{
					isDataFeed = true;
					method = ValueMethod.DataFeedStream;
				}
				else if (metadata.type == SqlDbType.VarChar || metadata.type == SqlDbType.NVarChar)
				{
					isDataFeed = true;
					method = ValueMethod.DataFeedText;
				}
				else
				{
					isDataFeed = false;
					method = ValueMethod.GetValue;
				}
			}
			else
			{
				isDataFeed = false;
				method = ValueMethod.GetValue;
			}
		}
		else
		{
			isSqlType = false;
			isDataFeed = false;
			method = ValueMethod.GetValue;
		}
		return new SourceColumnMetadata(method, isSqlType, isDataFeed);
	}

	private void CreateOrValidateConnection(string method)
	{
		if (_connection == null)
		{
			throw ADP.ConnectionRequired(method);
		}
		if (_ownConnection && _connection.State != ConnectionState.Open)
		{
			_connection.Open();
		}
		_connection.ValidateConnectionForExecute(method, null);
		if (_externalTransaction != null && _connection != _externalTransaction.Connection)
		{
			throw ADP.TransactionConnectionMismatch();
		}
	}

	private void RunParser(BulkCopySimpleResultSet bulkCopyHandler = null)
	{
		SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
		openTdsConnection.ThreadHasParserLockForClose = true;
		try
		{
			_parser.Run(RunBehavior.UntilDone, null, null, bulkCopyHandler, _stateObj);
		}
		finally
		{
			openTdsConnection.ThreadHasParserLockForClose = false;
		}
	}

	private void RunParserReliably(BulkCopySimpleResultSet bulkCopyHandler = null)
	{
		SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
		openTdsConnection.ThreadHasParserLockForClose = true;
		try
		{
			_parser.Run(RunBehavior.UntilDone, null, null, bulkCopyHandler, _stateObj);
		}
		finally
		{
			openTdsConnection.ThreadHasParserLockForClose = false;
		}
	}

	private void CommitTransaction()
	{
		if (_internalTransaction != null)
		{
			SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
			openTdsConnection.ThreadHasParserLockForClose = true;
			try
			{
				_internalTransaction.Commit();
				_internalTransaction.Dispose();
				_internalTransaction = null;
			}
			finally
			{
				openTdsConnection.ThreadHasParserLockForClose = false;
			}
		}
	}

	private void AbortTransaction()
	{
		if (_internalTransaction == null)
		{
			return;
		}
		if (!_internalTransaction.IsZombied)
		{
			SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
			openTdsConnection.ThreadHasParserLockForClose = true;
			try
			{
				_internalTransaction.Rollback();
			}
			finally
			{
				openTdsConnection.ThreadHasParserLockForClose = false;
			}
		}
		_internalTransaction.Dispose();
		_internalTransaction = null;
	}

	private void AppendColumnNameAndTypeName(StringBuilder query, string columnName, string typeName)
	{
		SqlServerEscapeHelper.EscapeIdentifier(query, columnName);
		query.Append(" ");
		query.Append(typeName);
	}

	private string UnquotedName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		if (name[0] == '[')
		{
			int length = name.Length;
			name = name.Substring(1, length - 2);
		}
		return name;
	}

	private object ValidateBulkCopyVariant(object value)
	{
		switch (MetaType.GetMetaTypeFromValue(value).TDSType)
		{
		case 36:
		case 40:
		case 41:
		case 42:
		case 43:
		case 48:
		case 50:
		case 52:
		case 56:
		case 59:
		case 60:
		case 61:
		case 62:
		case 108:
		case 127:
		case 165:
		case 167:
		case 231:
			if (value is INullable)
			{
				return MetaType.GetComValueFromSqlVariant(value);
			}
			return value;
		default:
			throw SQL.BulkLoadInvalidVariantValue();
		}
	}

	private object ConvertValue(object value, _SqlMetaData metadata, bool isNull, ref bool isSqlType, out bool coercedToDataFeed)
	{
		coercedToDataFeed = false;
		if (isNull)
		{
			if (!metadata.isNullable)
			{
				throw SQL.BulkLoadBulkLoadNotAllowDBNull(metadata.column);
			}
			return value;
		}
		MetaType metaType = metadata.metaType;
		bool typeChanged = false;
		try
		{
			switch (metaType.NullableType)
			{
			case 106:
			case 108:
			{
				MetaType metaTypeFromSqlDbType = MetaType.GetMetaTypeFromSqlDbType(metaType.SqlDbType, isMultiValued: false);
				value = SqlParameter.CoerceValue(value, metaTypeFromSqlDbType, out coercedToDataFeed, out typeChanged, allowStreaming: false);
				SqlDecimal sqlDecimal = ((!isSqlType || typeChanged) ? new SqlDecimal((decimal)value) : ((SqlDecimal)value));
				if (sqlDecimal.Scale != metadata.scale)
				{
					sqlDecimal = TdsParser.AdjustSqlDecimalScale(sqlDecimal, metadata.scale);
				}
				if (sqlDecimal.Precision > metadata.precision)
				{
					try
					{
						sqlDecimal = SqlDecimal.ConvertToPrecScale(sqlDecimal, metadata.precision, sqlDecimal.Scale);
					}
					catch (SqlTruncateException)
					{
						throw SQL.BulkLoadCannotConvertValue(value.GetType(), metaTypeFromSqlDbType, ADP.ParameterValueOutOfRange(sqlDecimal));
					}
				}
				value = sqlDecimal;
				isSqlType = true;
				typeChanged = false;
				break;
			}
			case 34:
			case 35:
			case 36:
			case 38:
			case 40:
			case 41:
			case 42:
			case 43:
			case 50:
			case 58:
			case 59:
			case 61:
			case 62:
			case 104:
			case 109:
			case 110:
			case 111:
			case 165:
			case 167:
			case 173:
			case 175:
			{
				MetaType metaTypeFromSqlDbType = MetaType.GetMetaTypeFromSqlDbType(metaType.SqlDbType, isMultiValued: false);
				value = SqlParameter.CoerceValue(value, metaTypeFromSqlDbType, out coercedToDataFeed, out typeChanged, allowStreaming: false);
				break;
			}
			case 99:
			case 231:
			case 239:
			{
				MetaType metaTypeFromSqlDbType = MetaType.GetMetaTypeFromSqlDbType(metaType.SqlDbType, isMultiValued: false);
				value = SqlParameter.CoerceValue(value, metaTypeFromSqlDbType, out coercedToDataFeed, out typeChanged, allowStreaming: false);
				if (!coercedToDataFeed && ((isSqlType && !typeChanged) ? ((SqlString)value).Value.Length : ((string)value).Length) > metadata.length / 2)
				{
					throw SQL.BulkLoadStringTooLong();
				}
				break;
			}
			case 98:
				value = ValidateBulkCopyVariant(value);
				typeChanged = true;
				break;
			case 240:
				if (!(value is byte[]))
				{
					value = _connection.GetBytes(value);
					typeChanged = true;
				}
				break;
			case 241:
				if (value is XmlReader)
				{
					value = new XmlDataFeed((XmlReader)value);
					typeChanged = true;
					coercedToDataFeed = true;
				}
				break;
			default:
				throw SQL.BulkLoadCannotConvertValue(value.GetType(), metadata.metaType, null);
			}
			if (typeChanged)
			{
				isSqlType = false;
			}
			return value;
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
			throw SQL.BulkLoadCannotConvertValue(value.GetType(), metadata.metaType, e);
		}
	}

	public void WriteToServer(DbDataReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		SqlStatistics statistics = Statistics;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_rowSource = reader;
			_DbDataReaderRowSource = reader;
			_SqlDataReaderRowSource = reader as SqlDataReader;
			_dataTableSource = null;
			_rowSourceType = ValueSourceType.DbDataReader;
			_isAsyncBulkCopy = false;
			WriteRowSourceToServerAsync(reader.FieldCount, CancellationToken.None);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public void WriteToServer(IDataReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		SqlStatistics statistics = Statistics;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_rowSource = reader;
			_SqlDataReaderRowSource = _rowSource as SqlDataReader;
			_DbDataReaderRowSource = _rowSource as DbDataReader;
			_dataTableSource = null;
			_rowSourceType = ValueSourceType.IDataReader;
			_isAsyncBulkCopy = false;
			WriteRowSourceToServerAsync(reader.FieldCount, CancellationToken.None);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public void WriteToServer(DataTable table)
	{
		WriteToServer(table, (DataRowState)0);
	}

	public void WriteToServer(DataTable table, DataRowState rowState)
	{
		if (table == null)
		{
			throw new ArgumentNullException("table");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		SqlStatistics statistics = Statistics;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_rowStateToSkip = ((rowState == (DataRowState)0 || rowState == DataRowState.Deleted) ? DataRowState.Deleted : (~rowState | DataRowState.Deleted));
			_rowSource = table;
			_dataTableSource = table;
			_SqlDataReaderRowSource = null;
			_rowSourceType = ValueSourceType.DataTable;
			_rowEnumerator = table.Rows.GetEnumerator();
			_isAsyncBulkCopy = false;
			WriteRowSourceToServerAsync(table.Columns.Count, CancellationToken.None);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public void WriteToServer(DataRow[] rows)
	{
		SqlStatistics statistics = Statistics;
		if (rows == null)
		{
			throw new ArgumentNullException("rows");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		if (rows.Length == 0)
		{
			return;
		}
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			DataTable table = rows[0].Table;
			_rowStateToSkip = DataRowState.Deleted;
			_rowSource = rows;
			_dataTableSource = table;
			_SqlDataReaderRowSource = null;
			_rowSourceType = ValueSourceType.RowArray;
			_rowEnumerator = rows.GetEnumerator();
			_isAsyncBulkCopy = false;
			WriteRowSourceToServerAsync(table.Columns.Count, CancellationToken.None);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public Task WriteToServerAsync(DataRow[] rows)
	{
		return WriteToServerAsync(rows, CancellationToken.None);
	}

	public Task WriteToServerAsync(DataRow[] rows, CancellationToken cancellationToken)
	{
		Task task = null;
		if (rows == null)
		{
			throw new ArgumentNullException("rows");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		SqlStatistics statistics = Statistics;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			if (rows.Length == 0)
			{
				return cancellationToken.IsCancellationRequested ? Task.FromCanceled(cancellationToken) : Task.CompletedTask;
			}
			DataTable table = rows[0].Table;
			_rowStateToSkip = DataRowState.Deleted;
			_rowSource = rows;
			_dataTableSource = table;
			_SqlDataReaderRowSource = null;
			_rowSourceType = ValueSourceType.RowArray;
			_rowEnumerator = rows.GetEnumerator();
			_isAsyncBulkCopy = true;
			return WriteRowSourceToServerAsync(table.Columns.Count, cancellationToken);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public Task WriteToServerAsync(DbDataReader reader)
	{
		return WriteToServerAsync(reader, CancellationToken.None);
	}

	public Task WriteToServerAsync(DbDataReader reader, CancellationToken cancellationToken)
	{
		Task task = null;
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		SqlStatistics statistics = Statistics;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_rowSource = reader;
			_SqlDataReaderRowSource = reader as SqlDataReader;
			_DbDataReaderRowSource = reader;
			_dataTableSource = null;
			_rowSourceType = ValueSourceType.DbDataReader;
			_isAsyncBulkCopy = true;
			return WriteRowSourceToServerAsync(reader.FieldCount, cancellationToken);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public Task WriteToServerAsync(IDataReader reader)
	{
		return WriteToServerAsync(reader, CancellationToken.None);
	}

	public Task WriteToServerAsync(IDataReader reader, CancellationToken cancellationToken)
	{
		Task task = null;
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		SqlStatistics statistics = Statistics;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_rowSource = reader;
			_SqlDataReaderRowSource = _rowSource as SqlDataReader;
			_DbDataReaderRowSource = _rowSource as DbDataReader;
			_dataTableSource = null;
			_rowSourceType = ValueSourceType.IDataReader;
			_isAsyncBulkCopy = true;
			return WriteRowSourceToServerAsync(reader.FieldCount, cancellationToken);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	public Task WriteToServerAsync(DataTable table)
	{
		return WriteToServerAsync(table, (DataRowState)0, CancellationToken.None);
	}

	public Task WriteToServerAsync(DataTable table, CancellationToken cancellationToken)
	{
		return WriteToServerAsync(table, (DataRowState)0, cancellationToken);
	}

	public Task WriteToServerAsync(DataTable table, DataRowState rowState)
	{
		return WriteToServerAsync(table, rowState, CancellationToken.None);
	}

	public Task WriteToServerAsync(DataTable table, DataRowState rowState, CancellationToken cancellationToken)
	{
		Task task = null;
		if (table == null)
		{
			throw new ArgumentNullException("table");
		}
		if (_isBulkCopyingInProgress)
		{
			throw SQL.BulkLoadPendingOperation();
		}
		SqlStatistics statistics = Statistics;
		try
		{
			statistics = SqlStatistics.StartTimer(Statistics);
			_rowStateToSkip = ((rowState == (DataRowState)0 || rowState == DataRowState.Deleted) ? DataRowState.Deleted : (~rowState | DataRowState.Deleted));
			_rowSource = table;
			_SqlDataReaderRowSource = null;
			_dataTableSource = table;
			_rowSourceType = ValueSourceType.DataTable;
			_rowEnumerator = table.Rows.GetEnumerator();
			_isAsyncBulkCopy = true;
			return WriteRowSourceToServerAsync(table.Columns.Count, cancellationToken);
		}
		finally
		{
			SqlStatistics.StopTimer(statistics);
		}
	}

	private Task WriteRowSourceToServerAsync(int columnCount, CancellationToken ctoken)
	{
		Task currentReconnectionTask = _connection._currentReconnectionTask;
		if (currentReconnectionTask != null && !currentReconnectionTask.IsCompleted)
		{
			if (_isAsyncBulkCopy)
			{
				TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
				currentReconnectionTask.ContinueWith(delegate
				{
					Task task2 = WriteRowSourceToServerAsync(columnCount, ctoken);
					if (task2 == null)
					{
						tcs.SetResult(null);
					}
					else
					{
						AsyncHelper.ContinueTask(task2, tcs, delegate
						{
							tcs.SetResult(null);
						});
					}
				}, ctoken);
				return tcs.Task;
			}
			AsyncHelper.WaitForCompletion(currentReconnectionTask, BulkCopyTimeout, delegate
			{
				throw SQL.CR_ReconnectTimeout();
			}, rethrowExceptions: false);
		}
		bool flag = true;
		_isBulkCopyingInProgress = true;
		CreateOrValidateConnection("WriteToServer");
		SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
		_parserLock = openTdsConnection._parserLock;
		_parserLock.Wait(_isAsyncBulkCopy);
		try
		{
			WriteRowSourceToServerCommon(columnCount);
			Task task = WriteToServerInternalAsync(ctoken);
			if (task != null)
			{
				flag = false;
				return task.ContinueWith(delegate(Task t)
				{
					try
					{
						AbortTransaction();
						return t;
					}
					finally
					{
						_isBulkCopyingInProgress = false;
						if (_parser != null)
						{
							_parser._asyncWrite = false;
						}
						if (_parserLock != null)
						{
							_parserLock.Release();
							_parserLock = null;
						}
					}
				}, TaskScheduler.Default).Unwrap();
			}
			return null;
		}
		catch (OutOfMemoryException e)
		{
			_connection.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			_connection.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			_connection.Abort(e3);
			throw;
		}
		finally
		{
			_columnMappings.ReadOnly = false;
			if (flag)
			{
				try
				{
					AbortTransaction();
				}
				finally
				{
					_isBulkCopyingInProgress = false;
					if (_parser != null)
					{
						_parser._asyncWrite = false;
					}
					if (_parserLock != null)
					{
						_parserLock.Release();
						_parserLock = null;
					}
				}
			}
		}
	}

	private void WriteRowSourceToServerCommon(int columnCount)
	{
		bool flag = false;
		_columnMappings.ReadOnly = true;
		_localColumnMappings = _columnMappings;
		if (_localColumnMappings.Count > 0)
		{
			_localColumnMappings.ValidateCollection();
			foreach (SqlBulkCopyColumnMapping localColumnMapping in _localColumnMappings)
			{
				if (localColumnMapping._internalSourceColumnOrdinal == -1)
				{
					flag = true;
					break;
				}
			}
		}
		else
		{
			_localColumnMappings = new SqlBulkCopyColumnMappingCollection();
			_localColumnMappings.CreateDefaultMapping(columnCount);
		}
		if (!flag)
		{
			return;
		}
		int num = -1;
		flag = false;
		if (_localColumnMappings.Count <= 0)
		{
			return;
		}
		foreach (SqlBulkCopyColumnMapping localColumnMapping2 in _localColumnMappings)
		{
			if (localColumnMapping2._internalSourceColumnOrdinal != -1)
			{
				continue;
			}
			string text = UnquotedName(localColumnMapping2.SourceColumn);
			switch (_rowSourceType)
			{
			case ValueSourceType.DataTable:
				num = ((DataTable)_rowSource).Columns.IndexOf(text);
				break;
			case ValueSourceType.RowArray:
				num = ((DataRow[])_rowSource)[0].Table.Columns.IndexOf(text);
				break;
			case ValueSourceType.IDataReader:
			case ValueSourceType.DbDataReader:
				try
				{
					num = ((IDataReader)_rowSource).GetOrdinal(text);
				}
				catch (IndexOutOfRangeException e)
				{
					throw SQL.BulkLoadNonMatchingColumnName(text, e);
				}
				break;
			}
			if (num == -1)
			{
				throw SQL.BulkLoadNonMatchingColumnName(text);
			}
			localColumnMapping2._internalSourceColumnOrdinal = num;
		}
	}

	internal void OnConnectionClosed()
	{
		_stateObj?.OnConnectionClosed();
	}

	private void OnRowsCopied(SqlRowsCopiedEventArgs value)
	{
		_rowsCopiedEventHandler?.Invoke(this, value);
	}

	private bool FireRowsCopiedEvent(long rowsCopied)
	{
		SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
		bool canBeReleasedFromAnyThread = openTdsConnection._parserLock.CanBeReleasedFromAnyThread;
		openTdsConnection._parserLock.Release();
		SqlRowsCopiedEventArgs e = new SqlRowsCopiedEventArgs(rowsCopied);
		try
		{
			_insideRowsCopiedEvent = true;
			OnRowsCopied(e);
		}
		finally
		{
			_insideRowsCopiedEvent = false;
			openTdsConnection._parserLock.Wait(canBeReleasedFromAnyThread);
		}
		return e.Abort;
	}

	private Task ReadWriteColumnValueAsync(int col)
	{
		bool isSqlType;
		bool isDataFeed;
		bool isNull;
		object obj = GetValueFromSourceRow(col, out isSqlType, out isDataFeed, out isNull);
		_SqlMetaData metadata = _sortedColumnMappings[col]._metadata;
		if (!isDataFeed)
		{
			obj = ConvertValue(obj, metadata, isNull, ref isSqlType, out isDataFeed);
		}
		Task result = null;
		if (metadata.type != SqlDbType.Variant)
		{
			result = _parser.WriteBulkCopyValue(obj, metadata, _stateObj, isSqlType, isDataFeed, isNull);
		}
		else
		{
			SqlBuffer.StorageType storageType = SqlBuffer.StorageType.Empty;
			if (_SqlDataReaderRowSource != null && _connection.IsKatmaiOrNewer)
			{
				storageType = _SqlDataReaderRowSource.GetVariantInternalStorageType(_sortedColumnMappings[col]._sourceColumnOrdinal);
			}
			switch (storageType)
			{
			case SqlBuffer.StorageType.DateTime2:
				_parser.WriteSqlVariantDateTime2((DateTime)obj, _stateObj);
				break;
			case SqlBuffer.StorageType.Date:
				_parser.WriteSqlVariantDate((DateTime)obj, _stateObj);
				break;
			default:
				result = _parser.WriteSqlVariantDataRowValue(obj, _stateObj);
				break;
			}
		}
		return result;
	}

	private void RegisterForConnectionCloseNotification<T>(ref Task<T> outerTask)
	{
		(_connection ?? throw ADP.ClosedConnectionError()).RegisterForConnectionCloseNotification(ref outerTask, this, 3);
	}

	private Task CopyColumnsAsync(int col, TaskCompletionSource<object> source = null)
	{
		Task result = null;
		Task task = null;
		try
		{
			int i;
			for (i = col; i < _sortedColumnMappings.Count; i++)
			{
				task = ReadWriteColumnValueAsync(i);
				if (task != null)
				{
					break;
				}
			}
			if (task != null)
			{
				if (source == null)
				{
					source = new TaskCompletionSource<object>();
					result = source.Task;
				}
				CopyColumnsAsyncSetupContinuation(source, task, i);
				return result;
			}
			source?.SetResult(null);
		}
		catch (Exception exception)
		{
			if (source == null)
			{
				throw;
			}
			source.TrySetException(exception);
		}
		return result;
	}

	private void CopyColumnsAsyncSetupContinuation(TaskCompletionSource<object> source, Task task, int i)
	{
		AsyncHelper.ContinueTask(task, source, delegate
		{
			if (i + 1 < _sortedColumnMappings.Count)
			{
				CopyColumnsAsync(i + 1, source);
			}
			else
			{
				source.SetResult(null);
			}
		}, _connection.GetOpenTdsConnection());
	}

	private void CheckAndRaiseNotification()
	{
		bool flag = false;
		Exception ex = null;
		_rowsCopied++;
		if (_notifyAfter > 0 && _rowsUntilNotification > 0 && --_rowsUntilNotification == 0)
		{
			try
			{
				_stateObj.BcpLock = true;
				flag = FireRowsCopiedEvent(_rowsCopied);
				if (ConnectionState.Open != _connection.State)
				{
					ex = ADP.OpenConnectionRequired("CheckAndRaiseNotification", _connection.State);
				}
			}
			catch (Exception ex2)
			{
				ex = (ADP.IsCatchableExceptionType(ex2) ? OperationAbortedException.Aborted(ex2) : ex2);
			}
			finally
			{
				_stateObj.BcpLock = false;
			}
			if (!flag)
			{
				_rowsUntilNotification = _notifyAfter;
			}
		}
		if (!flag && _rowsUntilNotification > _notifyAfter)
		{
			_rowsUntilNotification = _notifyAfter;
		}
		if (ex == null && flag)
		{
			ex = OperationAbortedException.Aborted(null);
		}
		if (_connection.State != ConnectionState.Open)
		{
			throw ADP.OpenConnectionRequired("WriteToServer", _connection.State);
		}
		if (ex != null)
		{
			_parser._asyncWrite = false;
			_parser.WriteBulkCopyDone(_stateObj);
			RunParser();
			AbortTransaction();
			throw ex;
		}
	}

	private Task CheckForCancellation(CancellationToken cts, TaskCompletionSource<object> tcs)
	{
		if (cts.IsCancellationRequested)
		{
			if (tcs == null)
			{
				tcs = new TaskCompletionSource<object>();
			}
			tcs.SetCanceled();
			return tcs.Task;
		}
		return null;
	}

	private TaskCompletionSource<object> ContinueTaskPend(Task task, TaskCompletionSource<object> source, Func<TaskCompletionSource<object>> action)
	{
		if (task == null)
		{
			return action();
		}
		AsyncHelper.ContinueTask(task, source, delegate
		{
			action();
		});
		return null;
	}

	private Task CopyRowsAsync(int rowsSoFar, int totalRows, CancellationToken cts, TaskCompletionSource<object> source = null)
	{
		Task task = null;
		Task task2 = null;
		try
		{
			int i;
			for (i = rowsSoFar; totalRows <= 0 || i < totalRows; i++)
			{
				if (!_hasMoreRowToCopy)
				{
					break;
				}
				if (_isAsyncBulkCopy)
				{
					task = CheckForCancellation(cts, source);
					if (task != null)
					{
						return task;
					}
				}
				_stateObj.WriteByte(209);
				task2 = CopyColumnsAsync(0);
				if (task2 == null)
				{
					CheckAndRaiseNotification();
					Task task3 = ReadFromRowSourceAsync(cts);
					if (task3 != null)
					{
						if (source == null)
						{
							source = new TaskCompletionSource<object>();
						}
						task = source.Task;
						AsyncHelper.ContinueTask(task3, source, delegate
						{
							CopyRowsAsync(i + 1, totalRows, cts, source);
						}, _connection.GetOpenTdsConnection());
						return task;
					}
					continue;
				}
				source = source ?? new TaskCompletionSource<object>();
				task = source.Task;
				AsyncHelper.ContinueTask(task2, source, delegate
				{
					CheckAndRaiseNotification();
					Task task4 = ReadFromRowSourceAsync(cts);
					if (task4 == null)
					{
						CopyRowsAsync(i + 1, totalRows, cts, source);
					}
					else
					{
						AsyncHelper.ContinueTask(task4, source, delegate
						{
							CopyRowsAsync(i + 1, totalRows, cts, source);
						}, _connection.GetOpenTdsConnection());
					}
				}, _connection.GetOpenTdsConnection());
				return task;
			}
			if (source != null)
			{
				source.TrySetResult(null);
			}
		}
		catch (Exception exception)
		{
			if (source == null)
			{
				throw;
			}
			source.TrySetException(exception);
		}
		return task;
	}

	private Task CopyBatchesAsync(BulkCopySimpleResultSet internalResults, string updateBulkCommandText, CancellationToken cts, TaskCompletionSource<object> source = null)
	{
		try
		{
			while (_hasMoreRowToCopy)
			{
				SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
				if (IsCopyOption(SqlBulkCopyOptions.UseInternalTransaction))
				{
					openTdsConnection.ThreadHasParserLockForClose = true;
					try
					{
						_internalTransaction = _connection.BeginTransaction();
					}
					finally
					{
						openTdsConnection.ThreadHasParserLockForClose = false;
					}
				}
				Task task = SubmitUpdateBulkCommand(updateBulkCommandText);
				if (task == null)
				{
					Task task2 = CopyBatchesAsyncContinued(internalResults, updateBulkCommandText, cts, source);
					if (task2 != null)
					{
						return task2;
					}
					continue;
				}
				if (source == null)
				{
					source = new TaskCompletionSource<object>();
				}
				AsyncHelper.ContinueTask(task, source, delegate
				{
					if (CopyBatchesAsyncContinued(internalResults, updateBulkCommandText, cts, source) == null)
					{
						CopyBatchesAsync(internalResults, updateBulkCommandText, cts, source);
					}
				}, _connection.GetOpenTdsConnection());
				return source.Task;
			}
		}
		catch (Exception exception)
		{
			if (source != null)
			{
				source.TrySetException(exception);
				return source.Task;
			}
			throw;
		}
		if (source != null)
		{
			source.SetResult(null);
			return source.Task;
		}
		return null;
	}

	private Task CopyBatchesAsyncContinued(BulkCopySimpleResultSet internalResults, string updateBulkCommandText, CancellationToken cts, TaskCompletionSource<object> source)
	{
		try
		{
			WriteMetaData(internalResults);
			Task task = CopyRowsAsync(0, _savedBatchSize, cts);
			if (task != null)
			{
				if (source == null)
				{
					source = new TaskCompletionSource<object>();
				}
				AsyncHelper.ContinueTask(task, source, delegate
				{
					if (CopyBatchesAsyncContinuedOnSuccess(internalResults, updateBulkCommandText, cts, source) == null)
					{
						CopyBatchesAsync(internalResults, updateBulkCommandText, cts, source);
					}
				}, _connection.GetOpenTdsConnection(), delegate
				{
					CopyBatchesAsyncContinuedOnError(cleanupParser: false);
				}, delegate
				{
					CopyBatchesAsyncContinuedOnError(cleanupParser: true);
				});
				return source.Task;
			}
			return CopyBatchesAsyncContinuedOnSuccess(internalResults, updateBulkCommandText, cts, source);
		}
		catch (Exception exception)
		{
			if (source != null)
			{
				source.TrySetException(exception);
				return source.Task;
			}
			throw;
		}
	}

	private Task CopyBatchesAsyncContinuedOnSuccess(BulkCopySimpleResultSet internalResults, string updateBulkCommandText, CancellationToken cts, TaskCompletionSource<object> source)
	{
		try
		{
			Task task = _parser.WriteBulkCopyDone(_stateObj);
			if (task == null)
			{
				RunParser();
				CommitTransaction();
				return null;
			}
			if (source == null)
			{
				source = new TaskCompletionSource<object>();
			}
			AsyncHelper.ContinueTask(task, source, delegate
			{
				try
				{
					RunParser();
					CommitTransaction();
				}
				catch (Exception)
				{
					CopyBatchesAsyncContinuedOnError(cleanupParser: false);
					throw;
				}
				CopyBatchesAsync(internalResults, updateBulkCommandText, cts, source);
			}, _connection.GetOpenTdsConnection(), delegate
			{
				CopyBatchesAsyncContinuedOnError(cleanupParser: false);
			});
			return source.Task;
		}
		catch (Exception exception)
		{
			if (source != null)
			{
				source.TrySetException(exception);
				return source.Task;
			}
			throw;
		}
	}

	private void CopyBatchesAsyncContinuedOnError(bool cleanupParser)
	{
		SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
		try
		{
			if (cleanupParser && _parser != null && _stateObj != null)
			{
				_parser._asyncWrite = false;
				_parser.WriteBulkCopyDone(_stateObj);
				RunParser();
			}
			if (_stateObj != null)
			{
				CleanUpStateObjectOnError();
			}
		}
		catch (OutOfMemoryException)
		{
			openTdsConnection.DoomThisConnection();
			throw;
		}
		catch (StackOverflowException)
		{
			openTdsConnection.DoomThisConnection();
			throw;
		}
		catch (ThreadAbortException)
		{
			openTdsConnection.DoomThisConnection();
			throw;
		}
		AbortTransaction();
	}

	private void CleanUpStateObjectOnError()
	{
		if (_stateObj == null)
		{
			return;
		}
		_parser.Connection.ThreadHasParserLockForClose = true;
		try
		{
			_stateObj.ResetBuffer();
			_stateObj._outputPacketNumber = 1;
			if (_parser.State == TdsParserState.OpenNotLoggedIn || _parser.State == TdsParserState.OpenLoggedIn)
			{
				_stateObj.CancelRequest();
			}
			_stateObj._internalTimeout = false;
			_stateObj.CloseSession();
			_stateObj._bulkCopyOpperationInProgress = false;
			_stateObj._bulkCopyWriteTimeout = false;
			_stateObj = null;
		}
		finally
		{
			_parser.Connection.ThreadHasParserLockForClose = false;
		}
	}

	private void WriteToServerInternalRestContinuedAsync(BulkCopySimpleResultSet internalResults, CancellationToken cts, TaskCompletionSource<object> source)
	{
		Task task = null;
		string text = null;
		try
		{
			text = AnalyzeTargetAndCreateUpdateBulkCommand(internalResults);
			if (_sortedColumnMappings.Count != 0)
			{
				_stateObj.SniContext = SniContext.Snix_SendRows;
				_savedBatchSize = _batchSize;
				_rowsUntilNotification = _notifyAfter;
				_rowsCopied = 0;
				_currentRowMetadata = new SourceColumnMetadata[_sortedColumnMappings.Count];
				for (int i = 0; i < _currentRowMetadata.Length; i++)
				{
					_currentRowMetadata[i] = GetColumnMetadata(i);
				}
				task = CopyBatchesAsync(internalResults, text, cts);
			}
			if (task != null)
			{
				if (source == null)
				{
					source = new TaskCompletionSource<object>();
				}
				AsyncHelper.ContinueTask(task, source, delegate
				{
					if (task.IsCanceled)
					{
						_localColumnMappings = null;
						try
						{
							CleanUpStateObjectOnError();
							return;
						}
						finally
						{
							source.SetCanceled();
						}
					}
					if (task.Exception != null)
					{
						source.SetException(task.Exception.InnerException);
						return;
					}
					_localColumnMappings = null;
					try
					{
						CleanUpStateObjectOnError();
					}
					finally
					{
						if (source != null)
						{
							if (cts.IsCancellationRequested)
							{
								source.SetCanceled();
							}
							else
							{
								source.SetResult(null);
							}
						}
					}
				}, _connection.GetOpenTdsConnection());
			}
			else
			{
				_localColumnMappings = null;
				try
				{
					CleanUpStateObjectOnError();
				}
				catch (Exception)
				{
				}
				if (source != null)
				{
					source.SetResult(null);
				}
			}
		}
		catch (Exception exception)
		{
			_localColumnMappings = null;
			try
			{
				CleanUpStateObjectOnError();
			}
			catch (Exception)
			{
			}
			if (source != null)
			{
				source.TrySetException(exception);
				return;
			}
			throw;
		}
	}

	private void WriteToServerInternalRestAsync(CancellationToken cts, TaskCompletionSource<object> source)
	{
		_hasMoreRowToCopy = true;
		Task<BulkCopySimpleResultSet> internalResultsTask = null;
		BulkCopySimpleResultSet result = new BulkCopySimpleResultSet();
		SqlInternalConnectionTds openTdsConnection = _connection.GetOpenTdsConnection();
		try
		{
			_parser = _connection.Parser;
			_parser._asyncWrite = _isAsyncBulkCopy;
			Task task;
			try
			{
				task = _connection.ValidateAndReconnect(delegate
				{
					if (_parserLock != null)
					{
						_parserLock.Release();
						_parserLock = null;
					}
				}, BulkCopyTimeout);
			}
			catch (SqlException inner)
			{
				throw SQL.BulkLoadInvalidDestinationTable(_destinationTableName, inner);
			}
			if (task != null)
			{
				if (_isAsyncBulkCopy)
				{
					CancellationTokenRegistration regReconnectCancel = default(CancellationTokenRegistration);
					TaskCompletionSource<object> cancellableReconnectTS = new TaskCompletionSource<object>();
					if (cts.CanBeCanceled)
					{
						regReconnectCancel = cts.Register(delegate(object s)
						{
							((TaskCompletionSource<object>)s).TrySetCanceled();
						}, cancellableReconnectTS);
					}
					AsyncHelper.ContinueTask(task, cancellableReconnectTS, delegate
					{
						cancellableReconnectTS.SetResult(null);
					});
					AsyncHelper.SetTimeoutException(cancellableReconnectTS, BulkCopyTimeout, () => SQL.BulkLoadInvalidDestinationTable(_destinationTableName, SQL.CR_ReconnectTimeout()), CancellationToken.None);
					AsyncHelper.ContinueTask(cancellableReconnectTS.Task, source, delegate
					{
						regReconnectCancel.Dispose();
						if (_parserLock != null)
						{
							_parserLock.Release();
							_parserLock = null;
						}
						_parserLock = _connection.GetOpenTdsConnection()._parserLock;
						_parserLock.Wait(canReleaseFromAnyThread: true);
						WriteToServerInternalRestAsync(cts, source);
					}, null, delegate
					{
						regReconnectCancel.Dispose();
					}, delegate
					{
						regReconnectCancel.Dispose();
					}, (Exception ex) => SQL.BulkLoadInvalidDestinationTable(_destinationTableName, ex), _connection);
					return;
				}
				try
				{
					AsyncHelper.WaitForCompletion(task, BulkCopyTimeout, delegate
					{
						throw SQL.CR_ReconnectTimeout();
					});
				}
				catch (SqlException inner2)
				{
					throw SQL.BulkLoadInvalidDestinationTable(_destinationTableName, inner2);
				}
				_parserLock = _connection.GetOpenTdsConnection()._parserLock;
				_parserLock.Wait(canReleaseFromAnyThread: false);
				WriteToServerInternalRestAsync(cts, source);
				return;
			}
			if (_isAsyncBulkCopy)
			{
				_connection.AddWeakReference(this, 3);
			}
			openTdsConnection.ThreadHasParserLockForClose = true;
			try
			{
				_stateObj = _parser.GetSession(this);
				_stateObj._bulkCopyOpperationInProgress = true;
				_stateObj.StartSession(this);
			}
			finally
			{
				openTdsConnection.ThreadHasParserLockForClose = false;
			}
			try
			{
				internalResultsTask = CreateAndExecuteInitialQueryAsync(out result);
			}
			catch (SqlException inner3)
			{
				throw SQL.BulkLoadInvalidDestinationTable(_destinationTableName, inner3);
			}
			if (internalResultsTask != null)
			{
				AsyncHelper.ContinueTask(internalResultsTask, source, delegate
				{
					WriteToServerInternalRestContinuedAsync(internalResultsTask.Result, cts, source);
				}, _connection.GetOpenTdsConnection());
			}
			else
			{
				WriteToServerInternalRestContinuedAsync(result, cts, source);
			}
		}
		catch (Exception exception)
		{
			if (source != null)
			{
				source.TrySetException(exception);
				return;
			}
			throw;
		}
	}

	private Task WriteToServerInternalAsync(CancellationToken ctoken)
	{
		TaskCompletionSource<object> source = null;
		Task<object> outerTask = null;
		if (_isAsyncBulkCopy)
		{
			source = new TaskCompletionSource<object>();
			outerTask = source.Task;
			RegisterForConnectionCloseNotification(ref outerTask);
		}
		if (_destinationTableName == null)
		{
			if (source != null)
			{
				source.SetException(SQL.BulkLoadMissingDestinationTable());
				return outerTask;
			}
			throw SQL.BulkLoadMissingDestinationTable();
		}
		try
		{
			Task task = ReadFromRowSourceAsync(ctoken);
			if (task == null)
			{
				if (!_hasMoreRowToCopy)
				{
					if (source != null)
					{
						source.SetResult(null);
					}
					return outerTask;
				}
				WriteToServerInternalRestAsync(ctoken, source);
				return outerTask;
			}
			AsyncHelper.ContinueTask(task, source, delegate
			{
				if (!_hasMoreRowToCopy)
				{
					source.SetResult(null);
				}
				else
				{
					WriteToServerInternalRestAsync(ctoken, source);
				}
			}, _connection.GetOpenTdsConnection());
			return outerTask;
		}
		catch (Exception exception)
		{
			if (source != null)
			{
				source.TrySetException(exception);
				return outerTask;
			}
			throw;
		}
	}
}
