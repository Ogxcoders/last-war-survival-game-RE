using System.Data.Common;
using System.Data.Sql;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Data.SqlClient;

public sealed class SqlCommandBuilder : DbCommandBuilder
{
	public override CatalogLocation CatalogLocation
	{
		get
		{
			return CatalogLocation.Start;
		}
		set
		{
			if (CatalogLocation.Start != value)
			{
				throw ADP.SingleValuedProperty("CatalogLocation", "Start");
			}
		}
	}

	public override string CatalogSeparator
	{
		get
		{
			return ".";
		}
		set
		{
			if ("." != value)
			{
				throw ADP.SingleValuedProperty("CatalogSeparator", ".");
			}
		}
	}

	public new SqlDataAdapter DataAdapter
	{
		get
		{
			return (SqlDataAdapter)base.DataAdapter;
		}
		set
		{
			base.DataAdapter = value;
		}
	}

	public override string QuotePrefix
	{
		get
		{
			return base.QuotePrefix;
		}
		set
		{
			if ("[" != value && "\"" != value)
			{
				throw ADP.DoubleValuedProperty("QuotePrefix", "[", "\"");
			}
			base.QuotePrefix = value;
		}
	}

	public override string QuoteSuffix
	{
		get
		{
			return base.QuoteSuffix;
		}
		set
		{
			if ("]" != value && "\"" != value)
			{
				throw ADP.DoubleValuedProperty("QuoteSuffix", "]", "\"");
			}
			base.QuoteSuffix = value;
		}
	}

	public override string SchemaSeparator
	{
		get
		{
			return ".";
		}
		set
		{
			if ("." != value)
			{
				throw ADP.SingleValuedProperty("SchemaSeparator", ".");
			}
		}
	}

	public SqlCommandBuilder()
	{
		GC.SuppressFinalize(this);
		base.QuotePrefix = "[";
		base.QuoteSuffix = "]";
	}

	public SqlCommandBuilder(SqlDataAdapter adapter)
		: this()
	{
		DataAdapter = adapter;
	}

	private void SqlRowUpdatingHandler(object sender, SqlRowUpdatingEventArgs ruevent)
	{
		RowUpdatingHandler(ruevent);
	}

	public new SqlCommand GetInsertCommand()
	{
		return (SqlCommand)base.GetInsertCommand();
	}

	public new SqlCommand GetInsertCommand(bool useColumnsForParameterNames)
	{
		return (SqlCommand)base.GetInsertCommand(useColumnsForParameterNames);
	}

	public new SqlCommand GetUpdateCommand()
	{
		return (SqlCommand)base.GetUpdateCommand();
	}

	public new SqlCommand GetUpdateCommand(bool useColumnsForParameterNames)
	{
		return (SqlCommand)base.GetUpdateCommand(useColumnsForParameterNames);
	}

	public new SqlCommand GetDeleteCommand()
	{
		return (SqlCommand)base.GetDeleteCommand();
	}

	public new SqlCommand GetDeleteCommand(bool useColumnsForParameterNames)
	{
		return (SqlCommand)base.GetDeleteCommand(useColumnsForParameterNames);
	}

	protected override void ApplyParameterInfo(DbParameter parameter, DataRow datarow, StatementType statementType, bool whereClause)
	{
		SqlParameter sqlParameter = (SqlParameter)parameter;
		object obj = datarow[SchemaTableColumn.ProviderType];
		sqlParameter.SqlDbType = (SqlDbType)obj;
		sqlParameter.Offset = 0;
		if (sqlParameter.SqlDbType == SqlDbType.Udt && !sqlParameter.SourceColumnNullMapping)
		{
			sqlParameter.UdtTypeName = datarow["DataTypeName"] as string;
		}
		else
		{
			sqlParameter.UdtTypeName = string.Empty;
		}
		object obj2 = datarow[SchemaTableColumn.NumericPrecision];
		if (DBNull.Value != obj2)
		{
			byte b = (byte)(short)obj2;
			sqlParameter.PrecisionInternal = (byte)((byte.MaxValue != b) ? b : 0);
		}
		obj2 = datarow[SchemaTableColumn.NumericScale];
		if (DBNull.Value != obj2)
		{
			byte b2 = (byte)(short)obj2;
			sqlParameter.ScaleInternal = (byte)((byte.MaxValue != b2) ? b2 : 0);
		}
	}

	protected override string GetParameterName(int parameterOrdinal)
	{
		return "@p" + parameterOrdinal.ToString(CultureInfo.InvariantCulture);
	}

	protected override string GetParameterName(string parameterName)
	{
		return "@" + parameterName;
	}

	protected override string GetParameterPlaceholder(int parameterOrdinal)
	{
		return "@p" + parameterOrdinal.ToString(CultureInfo.InvariantCulture);
	}

	private void ConsistentQuoteDelimiters(string quotePrefix, string quoteSuffix)
	{
		if (("\"" == quotePrefix && "\"" != quoteSuffix) || ("[" == quotePrefix && "]" != quoteSuffix))
		{
			throw ADP.InvalidPrefixSuffix();
		}
	}

	public static void DeriveParameters(SqlCommand command)
	{
		if (command == null)
		{
			throw ADP.ArgumentNull("command");
		}
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			command.DeriveParameters();
		}
		catch (OutOfMemoryException e)
		{
			command?.Connection?.Abort(e);
			throw;
		}
		catch (StackOverflowException e2)
		{
			command?.Connection?.Abort(e2);
			throw;
		}
		catch (ThreadAbortException e3)
		{
			command?.Connection?.Abort(e3);
			throw;
		}
	}

	protected override DataTable GetSchemaTable(DbCommand srcCommand)
	{
		SqlCommand sqlCommand = srcCommand as SqlCommand;
		SqlNotificationRequest notification = sqlCommand.Notification;
		sqlCommand.Notification = null;
		try
		{
			using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo);
			return sqlDataReader.GetSchemaTable();
		}
		finally
		{
			sqlCommand.Notification = notification;
		}
	}

	protected override DbCommand InitializeCommand(DbCommand command)
	{
		return (SqlCommand)base.InitializeCommand(command);
	}

	public override string QuoteIdentifier(string unquotedIdentifier)
	{
		ADP.CheckArgumentNull(unquotedIdentifier, "unquotedIdentifier");
		string quoteSuffix = QuoteSuffix;
		string quotePrefix = QuotePrefix;
		ConsistentQuoteDelimiters(quotePrefix, quoteSuffix);
		return ADP.BuildQuotedString(quotePrefix, quoteSuffix, unquotedIdentifier);
	}

	protected override void SetRowUpdatingHandler(DbDataAdapter adapter)
	{
		if (adapter == base.DataAdapter)
		{
			((SqlDataAdapter)adapter).RowUpdating -= SqlRowUpdatingHandler;
		}
		else
		{
			((SqlDataAdapter)adapter).RowUpdating += SqlRowUpdatingHandler;
		}
	}

	public override string UnquoteIdentifier(string quotedIdentifier)
	{
		ADP.CheckArgumentNull(quotedIdentifier, "quotedIdentifier");
		string quoteSuffix = QuoteSuffix;
		string quotePrefix = QuotePrefix;
		ConsistentQuoteDelimiters(quotePrefix, quoteSuffix);
		ADP.RemoveStringQuotes(quotePrefix, quoteSuffix, quotedIdentifier, out var unquotedString);
		return unquotedString;
	}
}
