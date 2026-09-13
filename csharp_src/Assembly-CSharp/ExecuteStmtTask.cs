using System;
using System.Collections.Generic;
using SQLite4Unity3d;

internal class ExecuteStmtTask : DatabaseActionTask
{
	private Action<DBExecResult> callback;

	private string sql;

	private DBExecResult sql_result;

	private List<List<DBAnyValue>> values;

	public ExecuteStmtTask(SQLiteConnection dbConnection, string cmd, List<List<DBAnyValue>> values, Action<DBExecResult> callback = null)
		: base(dbConnection)
	{
		sql = cmd;
		this.values = values;
		this.callback = callback;
	}

	public override void Process()
	{
		sql_result = ExeHelper.ExecStmt(dbConnection, sql, values);
		base.Process();
	}

	protected internal override void CallBack()
	{
		callback?.Invoke(sql_result);
	}
}
