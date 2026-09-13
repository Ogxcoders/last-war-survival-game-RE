using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Data.SqlClient;

internal sealed class SqlCommandSet
{
	private sealed class LocalCommand
	{
		internal readonly string CommandText;

		internal readonly SqlParameterCollection Parameters;

		internal readonly int ReturnParameterIndex;

		internal readonly CommandType CmdType;

		internal LocalCommand(string commandText, SqlParameterCollection parameters, int returnParameterIndex, CommandType cmdType)
		{
			CommandText = commandText;
			Parameters = parameters;
			ReturnParameterIndex = returnParameterIndex;
			CmdType = cmdType;
		}
	}

	private const string SqlIdentifierPattern = "^@[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\uff3f_@#\\$]*$";

	private static readonly Regex s_sqlIdentifierParser = new Regex("^@[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\uff3f_@#\\$]*$", RegexOptions.ExplicitCapture | RegexOptions.Singleline);

	private List<LocalCommand> _commandList = new List<LocalCommand>();

	private SqlCommand _batchCommand;

	private SqlCommand BatchCommand
	{
		get
		{
			SqlCommand batchCommand = _batchCommand;
			if (batchCommand == null)
			{
				throw ADP.ObjectDisposed(this);
			}
			return batchCommand;
		}
	}

	internal int CommandCount => CommandList.Count;

	private List<LocalCommand> CommandList
	{
		get
		{
			List<LocalCommand> commandList = _commandList;
			if (commandList == null)
			{
				throw ADP.ObjectDisposed(this);
			}
			return commandList;
		}
	}

	internal int CommandTimeout
	{
		set
		{
			BatchCommand.CommandTimeout = value;
		}
	}

	internal SqlConnection Connection
	{
		get
		{
			return BatchCommand.Connection;
		}
		set
		{
			BatchCommand.Connection = value;
		}
	}

	internal SqlTransaction Transaction
	{
		set
		{
			BatchCommand.Transaction = value;
		}
	}

	internal SqlCommandSet()
	{
		_batchCommand = new SqlCommand();
	}

	internal void Append(SqlCommand command)
	{
		ADP.CheckArgumentNull(command, "command");
		string commandText = command.CommandText;
		if (string.IsNullOrEmpty(commandText))
		{
			throw ADP.CommandTextRequired("Append");
		}
		CommandType commandType = command.CommandType;
		switch (commandType)
		{
		case CommandType.TableDirect:
			throw SQL.NotSupportedCommandType(commandType);
		default:
			throw ADP.InvalidCommandType(commandType);
		case CommandType.Text:
		case CommandType.StoredProcedure:
		{
			SqlParameterCollection sqlParameterCollection = null;
			SqlParameterCollection parameters = command.Parameters;
			if (0 < parameters.Count)
			{
				sqlParameterCollection = new SqlParameterCollection();
				for (int i = 0; i < parameters.Count; i++)
				{
					SqlParameter sqlParameter = new SqlParameter();
					parameters[i].CopyTo(sqlParameter);
					sqlParameterCollection.Add(sqlParameter);
					if (!s_sqlIdentifierParser.IsMatch(sqlParameter.ParameterName))
					{
						throw ADP.BadParameterName(sqlParameter.ParameterName);
					}
				}
				foreach (SqlParameter item2 in sqlParameterCollection)
				{
					object value = item2.Value;
					if (value is byte[] array)
					{
						int offset = item2.Offset;
						int size = item2.Size;
						int num = array.Length - offset;
						if (size != 0 && size < num)
						{
							num = size;
						}
						byte[] array2 = new byte[Math.Max(num, 0)];
						Buffer.BlockCopy(array, offset, array2, 0, array2.Length);
						item2.Offset = 0;
						item2.Value = array2;
					}
					else if (value is char[] array3)
					{
						int offset2 = item2.Offset;
						int size2 = item2.Size;
						int num2 = array3.Length - offset2;
						if (size2 != 0 && size2 < num2)
						{
							num2 = size2;
						}
						char[] array4 = new char[Math.Max(num2, 0)];
						Buffer.BlockCopy(array3, offset2, array4, 0, array4.Length * 2);
						item2.Offset = 0;
						item2.Value = array4;
					}
					else if (value is ICloneable cloneable)
					{
						item2.Value = cloneable.Clone();
					}
				}
			}
			int returnParameterIndex = -1;
			if (sqlParameterCollection != null)
			{
				for (int j = 0; j < sqlParameterCollection.Count; j++)
				{
					if (ParameterDirection.ReturnValue == sqlParameterCollection[j].Direction)
					{
						returnParameterIndex = j;
						break;
					}
				}
			}
			LocalCommand item = new LocalCommand(commandText, sqlParameterCollection, returnParameterIndex, command.CommandType);
			CommandList.Add(item);
			break;
		}
		}
	}

	internal static void BuildStoredProcedureName(StringBuilder builder, string part)
	{
		if (part == null || 0 >= part.Length)
		{
			return;
		}
		if ('[' == part[0])
		{
			int num = 0;
			foreach (char c in part)
			{
				if (']' == c)
				{
					num++;
				}
			}
			if (1 == num % 2)
			{
				builder.Append(part);
				return;
			}
		}
		SqlServerEscapeHelper.EscapeIdentifier(builder, part);
	}

	internal void Clear()
	{
		DbCommand batchCommand = BatchCommand;
		if (batchCommand != null)
		{
			batchCommand.Parameters.Clear();
			batchCommand.CommandText = null;
		}
		_commandList?.Clear();
	}

	internal void Dispose()
	{
		SqlCommand batchCommand = _batchCommand;
		_commandList = null;
		_batchCommand = null;
		batchCommand?.Dispose();
	}

	internal int ExecuteNonQuery()
	{
		ValidateCommandBehavior("ExecuteNonQuery", CommandBehavior.Default);
		BatchCommand.BatchRPCMode = true;
		BatchCommand.ClearBatchCommand();
		BatchCommand.Parameters.Clear();
		for (int i = 0; i < _commandList.Count; i++)
		{
			LocalCommand localCommand = _commandList[i];
			BatchCommand.AddBatchCommand(localCommand.CommandText, localCommand.Parameters, localCommand.CmdType);
		}
		return BatchCommand.ExecuteBatchRPCCommand();
	}

	internal SqlParameter GetParameter(int commandIndex, int parameterIndex)
	{
		return CommandList[commandIndex].Parameters[parameterIndex];
	}

	internal bool GetBatchedAffected(int commandIdentifier, out int recordsAffected, out Exception error)
	{
		error = BatchCommand.GetErrors(commandIdentifier);
		int? recordsAffected2 = BatchCommand.GetRecordsAffected(commandIdentifier);
		recordsAffected = recordsAffected2.GetValueOrDefault();
		return recordsAffected2.HasValue;
	}

	internal int GetParameterCount(int commandIndex)
	{
		return CommandList[commandIndex].Parameters.Count;
	}

	private void ValidateCommandBehavior(string method, CommandBehavior behavior)
	{
		if ((behavior & ~(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection)) != CommandBehavior.Default)
		{
			ADP.ValidateCommandBehavior(behavior);
			throw ADP.NotSupportedCommandBehavior(behavior & ~(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection), method);
		}
	}
}
