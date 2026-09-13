using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Data.SqlClient;

internal static class SqlClientDiagnosticListenerExtensions
{
	public const string DiagnosticListenerName = "SqlClientDiagnosticListener";

	private const string SqlClientPrefix = "System.Data.SqlClient.";

	public const string SqlBeforeExecuteCommand = "System.Data.SqlClient.WriteCommandBefore";

	public const string SqlAfterExecuteCommand = "System.Data.SqlClient.WriteCommandAfter";

	public const string SqlErrorExecuteCommand = "System.Data.SqlClient.WriteCommandError";

	public const string SqlBeforeOpenConnection = "System.Data.SqlClient.WriteConnectionOpenBefore";

	public const string SqlAfterOpenConnection = "System.Data.SqlClient.WriteConnectionOpenAfter";

	public const string SqlErrorOpenConnection = "System.Data.SqlClient.WriteConnectionOpenError";

	public const string SqlBeforeCloseConnection = "System.Data.SqlClient.WriteConnectionCloseBefore";

	public const string SqlAfterCloseConnection = "System.Data.SqlClient.WriteConnectionCloseAfter";

	public const string SqlErrorCloseConnection = "System.Data.SqlClient.WriteConnectionCloseError";

	public const string SqlBeforeCommitTransaction = "System.Data.SqlClient.WriteTransactionCommitBefore";

	public const string SqlAfterCommitTransaction = "System.Data.SqlClient.WriteTransactionCommitAfter";

	public const string SqlErrorCommitTransaction = "System.Data.SqlClient.WriteTransactionCommitError";

	public const string SqlBeforeRollbackTransaction = "System.Data.SqlClient.WriteTransactionRollbackBefore";

	public const string SqlAfterRollbackTransaction = "System.Data.SqlClient.WriteTransactionRollbackAfter";

	public const string SqlErrorRollbackTransaction = "System.Data.SqlClient.WriteTransactionRollbackError";

	public static Guid WriteCommandBefore(this DiagnosticListener @this, SqlCommand sqlCommand, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteCommandBefore"))
		{
			Guid guid = Guid.NewGuid();
			@this.Write("System.Data.SqlClient.WriteCommandBefore", new
			{
				OperationId = guid,
				Operation = operation,
				ConnectionId = sqlCommand.Connection?.ClientConnectionId,
				Command = sqlCommand
			});
			return guid;
		}
		return Guid.Empty;
	}

	public static void WriteCommandAfter(this DiagnosticListener @this, Guid operationId, SqlCommand sqlCommand, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteCommandAfter"))
		{
			@this.Write("System.Data.SqlClient.WriteCommandAfter", new
			{
				OperationId = operationId,
				Operation = operation,
				ConnectionId = sqlCommand.Connection?.ClientConnectionId,
				Command = sqlCommand,
				Statistics = sqlCommand.Statistics?.GetDictionary(),
				Timestamp = Stopwatch.GetTimestamp()
			});
		}
	}

	public static void WriteCommandError(this DiagnosticListener @this, Guid operationId, SqlCommand sqlCommand, Exception ex, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteCommandError"))
		{
			@this.Write("System.Data.SqlClient.WriteCommandError", new
			{
				OperationId = operationId,
				Operation = operation,
				ConnectionId = sqlCommand.Connection?.ClientConnectionId,
				Command = sqlCommand,
				Exception = ex,
				Timestamp = Stopwatch.GetTimestamp()
			});
		}
	}

	public static Guid WriteConnectionOpenBefore(this DiagnosticListener @this, SqlConnection sqlConnection, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteConnectionOpenBefore"))
		{
			Guid guid = Guid.NewGuid();
			@this.Write("System.Data.SqlClient.WriteConnectionOpenBefore", new
			{
				OperationId = guid,
				Operation = operation,
				Connection = sqlConnection,
				Timestamp = Stopwatch.GetTimestamp()
			});
			return guid;
		}
		return Guid.Empty;
	}

	public static void WriteConnectionOpenAfter(this DiagnosticListener @this, Guid operationId, SqlConnection sqlConnection, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteConnectionOpenAfter"))
		{
			@this.Write("System.Data.SqlClient.WriteConnectionOpenAfter", new
			{
				OperationId = operationId,
				Operation = operation,
				ConnectionId = sqlConnection.ClientConnectionId,
				Connection = sqlConnection,
				Statistics = sqlConnection.Statistics?.GetDictionary(),
				Timestamp = Stopwatch.GetTimestamp()
			});
		}
	}

	public static void WriteConnectionOpenError(this DiagnosticListener @this, Guid operationId, SqlConnection sqlConnection, Exception ex, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteConnectionOpenError"))
		{
			@this.Write("System.Data.SqlClient.WriteConnectionOpenError", new
			{
				OperationId = operationId,
				Operation = operation,
				ConnectionId = sqlConnection.ClientConnectionId,
				Connection = sqlConnection,
				Exception = ex,
				Timestamp = Stopwatch.GetTimestamp()
			});
		}
	}

	public static Guid WriteConnectionCloseBefore(this DiagnosticListener @this, SqlConnection sqlConnection, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteConnectionCloseBefore"))
		{
			Guid guid = Guid.NewGuid();
			@this.Write("System.Data.SqlClient.WriteConnectionCloseBefore", new
			{
				OperationId = guid,
				Operation = operation,
				ConnectionId = sqlConnection.ClientConnectionId,
				Connection = sqlConnection,
				Statistics = sqlConnection.Statistics?.GetDictionary(),
				Timestamp = Stopwatch.GetTimestamp()
			});
			return guid;
		}
		return Guid.Empty;
	}

	public static void WriteConnectionCloseAfter(this DiagnosticListener @this, Guid operationId, Guid clientConnectionId, SqlConnection sqlConnection, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteConnectionCloseAfter"))
		{
			@this.Write("System.Data.SqlClient.WriteConnectionCloseAfter", new
			{
				OperationId = operationId,
				Operation = operation,
				ConnectionId = clientConnectionId,
				Connection = sqlConnection,
				Statistics = sqlConnection.Statistics?.GetDictionary(),
				Timestamp = Stopwatch.GetTimestamp()
			});
		}
	}

	public static void WriteConnectionCloseError(this DiagnosticListener @this, Guid operationId, Guid clientConnectionId, SqlConnection sqlConnection, Exception ex, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteConnectionCloseError"))
		{
			@this.Write("System.Data.SqlClient.WriteConnectionCloseError", new
			{
				OperationId = operationId,
				Operation = operation,
				ConnectionId = clientConnectionId,
				Connection = sqlConnection,
				Statistics = sqlConnection.Statistics?.GetDictionary(),
				Exception = ex,
				Timestamp = Stopwatch.GetTimestamp()
			});
		}
	}

	public static Guid WriteTransactionCommitBefore(this DiagnosticListener @this, IsolationLevel isolationLevel, SqlConnection connection, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteTransactionCommitBefore"))
		{
			Guid guid = Guid.NewGuid();
			@this.Write("System.Data.SqlClient.WriteTransactionCommitBefore", new
			{
				OperationId = guid,
				Operation = operation,
				IsolationLevel = isolationLevel,
				Connection = connection,
				Timestamp = Stopwatch.GetTimestamp()
			});
			return guid;
		}
		return Guid.Empty;
	}

	public static void WriteTransactionCommitAfter(this DiagnosticListener @this, Guid operationId, IsolationLevel isolationLevel, SqlConnection connection, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteTransactionCommitAfter"))
		{
			@this.Write("System.Data.SqlClient.WriteTransactionCommitAfter", new
			{
				OperationId = operationId,
				Operation = operation,
				IsolationLevel = isolationLevel,
				Connection = connection,
				Timestamp = Stopwatch.GetTimestamp()
			});
		}
	}

	public static void WriteTransactionCommitError(this DiagnosticListener @this, Guid operationId, IsolationLevel isolationLevel, SqlConnection connection, Exception ex, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteTransactionCommitError"))
		{
			@this.Write("System.Data.SqlClient.WriteTransactionCommitError", new
			{
				OperationId = operationId,
				Operation = operation,
				IsolationLevel = isolationLevel,
				Connection = connection,
				Exception = ex,
				Timestamp = Stopwatch.GetTimestamp()
			});
		}
	}

	public static Guid WriteTransactionRollbackBefore(this DiagnosticListener @this, IsolationLevel isolationLevel, SqlConnection connection, string transactionName, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteTransactionRollbackBefore"))
		{
			Guid guid = Guid.NewGuid();
			@this.Write("System.Data.SqlClient.WriteTransactionRollbackBefore", new
			{
				OperationId = guid,
				Operation = operation,
				IsolationLevel = isolationLevel,
				Connection = connection,
				TransactionName = transactionName,
				Timestamp = Stopwatch.GetTimestamp()
			});
			return guid;
		}
		return Guid.Empty;
	}

	public static void WriteTransactionRollbackAfter(this DiagnosticListener @this, Guid operationId, IsolationLevel isolationLevel, SqlConnection connection, string transactionName, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteTransactionRollbackAfter"))
		{
			@this.Write("System.Data.SqlClient.WriteTransactionRollbackAfter", new
			{
				OperationId = operationId,
				Operation = operation,
				IsolationLevel = isolationLevel,
				Connection = connection,
				TransactionName = transactionName,
				Timestamp = Stopwatch.GetTimestamp()
			});
		}
	}

	public static void WriteTransactionRollbackError(this DiagnosticListener @this, Guid operationId, IsolationLevel isolationLevel, SqlConnection connection, string transactionName, Exception ex, [CallerMemberName] string operation = "")
	{
		if (@this.IsEnabled("System.Data.SqlClient.WriteTransactionRollbackError"))
		{
			@this.Write("System.Data.SqlClient.WriteTransactionRollbackError", new
			{
				OperationId = operationId,
				Operation = operation,
				IsolationLevel = isolationLevel,
				Connection = connection,
				TransactionName = transactionName,
				Exception = ex,
				Timestamp = Stopwatch.GetTimestamp()
			});
		}
	}
}
