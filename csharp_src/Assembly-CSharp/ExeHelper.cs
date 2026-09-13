using System;
using System.Collections.Generic;
using GameFramework;
using SQLite4Unity3d;

internal class ExeHelper
{
	private static IntPtr NegativePointer = new IntPtr(-1);

	public static DBExecResult ExecSql(SQLiteConnection dbConnection, string sql)
	{
		DBExecResult dBExecResult = new DBExecResult();
		dBExecResult.values = new List<DBAnyValue[]>(20);
		dBExecResult.error = -1;
		dBExecResult.change_rows = -1;
		dBExecResult.last_insertid = -1L;
		SQLite3.Result result = SQLite3.Result.Error;
		IntPtr handle = dbConnection.Handle;
		lock (dbConnection.SyncObject)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = SQLite3.Prepare2(handle, sql);
				int num = (dBExecResult.col_count = SQLite3.ColumnCount(intPtr));
				dBExecResult.cols = new DBColumn[num];
				for (int i = 0; i < num; i++)
				{
					dBExecResult.cols[i].name = SQLite3.ColumnName16(intPtr, i);
				}
				while (SQLite3.Result.Row == (result = SQLite3.Step(intPtr)))
				{
					DBAnyValue[] array = new DBAnyValue[num];
					for (int j = 0; j < num; j++)
					{
						SQLite3.ColType colType = SQLite3.ColumnType(intPtr, j);
						array[j].type = (int)SQLite3.ColumnType(intPtr, j);
						switch (colType)
						{
						case SQLite3.ColType.Null:
							array[j].is_null = true;
							break;
						case SQLite3.ColType.Integer:
							array[j].lv = SQLite3.ColumnInt64(intPtr, j);
							break;
						case SQLite3.ColType.Float:
							array[j].fv = SQLite3.ColumnDouble(intPtr, j);
							break;
						default:
							array[j].sv = SQLite3.ColumnString(intPtr, j);
							break;
						}
					}
					dBExecResult.values.Add(array);
				}
				if (result == SQLite3.Result.Busy)
				{
					Log.Error(" == RUN SQL BUSY : {1}", sql);
				}
				int change_rows = SQLite3.Changes(handle);
				dBExecResult.error = 0;
				dBExecResult.errorcode = 0;
				dBExecResult.change_rows = change_rows;
				dBExecResult.last_insertid = SQLite3.LastInsertRowid(handle);
			}
			catch (Exception arg)
			{
				Log.Error($"ExeHelper::ExecSql {arg}, {sql}");
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SQLite3.Finalize(intPtr);
				}
			}
		}
		switch (result)
		{
		case SQLite3.Result.Error:
		{
			string errmsg2 = SQLite3.GetErrmsg(handle);
			dBExecResult.errormsg = errmsg2;
			dBExecResult.errorsql = sql;
			GetErrorCode(dBExecResult, result);
			break;
		}
		case SQLite3.Result.Constraint:
			dBExecResult.errorsql = sql;
			if (SQLite3.ExtendedErrCode(handle) == SQLite3.ExtendedResult.ConstraintNotNull)
			{
				dBExecResult.errormsg = SQLite3.GetErrmsg(handle);
			}
			else
			{
				dBExecResult.errormsg = "SQLite3.Result.Constraint";
			}
			GetErrorCode(dBExecResult, result);
			break;
		default:
		{
			string errmsg = SQLite3.GetErrmsg(handle);
			dBExecResult.errormsg = errmsg;
			dBExecResult.errorsql = sql;
			dBExecResult.errorcode = (int)result;
			break;
		}
		case SQLite3.Result.Done:
			break;
		}
		return dBExecResult;
	}

	public static DBExecResult ExecStmt(SQLiteConnection dbConnection, string sql, List<List<DBAnyValue>> values)
	{
		DBExecResult dBExecResult = new DBExecResult();
		dBExecResult.values = new List<DBAnyValue[]>(20);
		dBExecResult.error = -1;
		dBExecResult.change_rows = -1;
		dBExecResult.last_insertid = -1L;
		SQLite3.Result result = SQLite3.Result.Error;
		IntPtr handle = dbConnection.Handle;
		lock (dbConnection.SyncObject)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				intPtr = SQLite3.Prepare2(handle, sql);
				if (values != null)
				{
					for (int i = 0; i < values.Count; i++)
					{
						List<DBAnyValue> list = values[i];
						for (int j = 0; j < list.Count; j++)
						{
							DBAnyValue dBAnyValue = list[j];
							int index = j + 1;
							switch ((SQLite3.ColType)dBAnyValue.type)
							{
							case SQLite3.ColType.Integer:
								SQLite3.BindInt64(intPtr, index, dBAnyValue.lv);
								break;
							case SQLite3.ColType.Float:
								SQLite3.BindDouble(intPtr, index, dBAnyValue.fv);
								break;
							case SQLite3.ColType.Text:
								SQLite3.BindText(intPtr, index, dBAnyValue.sv, -1, NegativePointer);
								break;
							default:
								SQLite3.BindNull(intPtr, index);
								break;
							}
						}
					}
					while (SQLite3.Result.Row == (result = SQLite3.Step(intPtr)))
					{
						if (dBExecResult.col_count <= 0)
						{
							int num = (dBExecResult.col_count = SQLite3.ColumnCount(intPtr));
							dBExecResult.cols = new DBColumn[num];
							for (int k = 0; k < num; k++)
							{
								dBExecResult.cols[k].name = SQLite3.ColumnName16(intPtr, k);
							}
						}
						DBAnyValue[] array = new DBAnyValue[dBExecResult.col_count];
						for (int l = 0; l < dBExecResult.col_count; l++)
						{
							SQLite3.ColType colType = SQLite3.ColumnType(intPtr, l);
							array[l].type = (int)SQLite3.ColumnType(intPtr, l);
							switch (colType)
							{
							case SQLite3.ColType.Null:
								array[l].is_null = true;
								break;
							case SQLite3.ColType.Integer:
								array[l].lv = SQLite3.ColumnInt64(intPtr, l);
								break;
							case SQLite3.ColType.Float:
								array[l].fv = SQLite3.ColumnDouble(intPtr, l);
								break;
							default:
								array[l].sv = SQLite3.ColumnString(intPtr, l);
								break;
							}
						}
						dBExecResult.values.Add(array);
					}
					if (result == SQLite3.Result.Busy)
					{
						Log.Error(" == RUN SQL BUSY : {1}", sql);
					}
					int change_rows = SQLite3.Changes(handle);
					dBExecResult.error = 0;
					dBExecResult.change_rows = change_rows;
					dBExecResult.last_insertid = SQLite3.LastInsertRowid(handle);
				}
			}
			catch (Exception arg)
			{
				Log.Error($"ExeHelper::ExecStmt {arg}, {sql}");
			}
			finally
			{
				switch (result)
				{
				case SQLite3.Result.Error:
				{
					string errmsg2 = SQLite3.GetErrmsg(handle);
					dBExecResult.errormsg = errmsg2;
					dBExecResult.errorsql = sql;
					GetErrorCode(dBExecResult, result);
					break;
				}
				case SQLite3.Result.Constraint:
					dBExecResult.errorsql = sql;
					if (SQLite3.ExtendedErrCode(handle) == SQLite3.ExtendedResult.ConstraintNotNull)
					{
						dBExecResult.errormsg = SQLite3.GetErrmsg(handle);
					}
					else
					{
						dBExecResult.errormsg = "SQLite3.Result.Constraint";
					}
					GetErrorCode(dBExecResult, result);
					break;
				default:
				{
					string errmsg = SQLite3.GetErrmsg(handle);
					dBExecResult.errormsg = errmsg;
					dBExecResult.errorsql = sql;
					dBExecResult.errorcode = (int)result;
					break;
				}
				case SQLite3.Result.Done:
					break;
				}
				if (intPtr != IntPtr.Zero)
				{
					SQLite3.Reset(intPtr);
					SQLite3.Finalize(intPtr);
				}
			}
		}
		return dBExecResult;
	}

	private static void GetErrorCode(DBExecResult ret, SQLite3.Result r)
	{
		ret.errorcode = (int)r;
		if (ret.errormsg == "out of memory")
		{
			ret.errorcode = 7;
		}
		else if (ret.errormsg == "database disk image is malformed")
		{
			ret.errorcode = 11;
		}
		else if (ret.errormsg == "disk I/O error")
		{
			ret.errorcode = 10;
		}
	}
}
