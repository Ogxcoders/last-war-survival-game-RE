using System;
using System.Collections.Generic;
using SQLite4Unity3d;

internal class MultiExecuteTask : DatabaseActionTask
{
	private Action<List<DBExecResult>> callback;

	private List<string> sqls;

	private List<DBExecResult> sql_results;

	public MultiExecuteTask(SQLiteConnection dbConnection, List<string> cmd, Action<List<DBExecResult>> callback = null)
		: base(dbConnection)
	{
		sqls = cmd;
		this.callback = callback;
	}

	public override void Process()
	{
		sql_results = new List<DBExecResult>(sqls.Count);
		for (int i = 0; i < sqls.Count; i++)
		{
			DBExecResult item = ExeHelper.ExecSql(dbConnection, sqls[i]);
			sql_results.Add(item);
		}
		base.Process();
	}

	protected internal override void CallBack()
	{
		callback?.Invoke(sql_results);
	}
}
