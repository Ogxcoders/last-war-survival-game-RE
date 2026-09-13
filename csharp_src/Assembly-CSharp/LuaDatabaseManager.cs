using System;
using System.Collections.Generic;
using GameFramework;
using XLua;
using XLua.LuaDLL;

public class LuaDatabaseManager
{
	public static void InitDataBase(string dbFileName, Action<bool> callback)
	{
		DatabaseManager.Instance.Release();
		DatabaseManager.Instance.Initialize(dbFileName, delegate(bool s)
		{
			callback(s);
		});
	}

	public static void UninitDatabase()
	{
		DatabaseManager.Instance.Release();
	}

	private static LuaTable DBExecResult2LuaTable(DBExecResult r)
	{
		LuaTable luaTable = GameEntry.Lua.Env.NewTable();
		luaTable.SetInt("error", r.error);
		luaTable.SetInt("errorcode", r.errorcode);
		luaTable.SetString("errormsg", r.errormsg);
		luaTable.SetInt("change_rows", r.change_rows);
		luaTable.SetInt("col_count", r.col_count);
		if (r.cols != null)
		{
			LuaTable luaTable2 = GameEntry.Lua.Env.NewTable();
			for (int i = 0; i < r.cols.Length; i++)
			{
				LuaTable luaTable3 = GameEntry.Lua.Env.NewTable();
				luaTable3.SetString("name", r.cols[i].name);
				luaTable3.SetInt("type", r.cols[i].type);
				luaTable2.SetTable(i + 1, luaTable3);
			}
			luaTable.SetTable("cols", luaTable2);
		}
		else
		{
			Log.Error("no cols ?? error: {0}, sql: {1}", r.errormsg ?? "", r.errorsql ?? "");
		}
		if (r.values != null)
		{
			LuaTable luaTable4 = GameEntry.Lua.Env.NewTable();
			for (int j = 0; j < r.values.Count; j++)
			{
				DBAnyValue[] array = r.values[j];
				LuaTable luaTable5 = GameEntry.Lua.Env.NewTable();
				for (int k = 0; k < array.Length; k++)
				{
					switch (array[k].type)
					{
					case 1:
						luaTable5.SetLong(k + 1, array[k].lv);
						break;
					case 2:
						luaTable5.SetDouble(k + 1, array[k].fv);
						break;
					case 5:
						luaTable5.Set<int, object>(k + 1, null);
						break;
					default:
						luaTable5.SetString(k + 1, array[k].sv);
						break;
					}
				}
				luaTable4.SetTable(j + 1, luaTable5);
			}
			luaTable.SetTable("values", luaTable4);
		}
		else
		{
			Log.Error("no values ????");
		}
		return luaTable;
	}

	private static int ValueTableToRow(List<DBAnyValue> row, string format, int stack)
	{
		IntPtr l = GameEntry.Lua.Env.L;
		int num = ((!string.IsNullOrEmpty(format)) ? format.Length : 0);
		int newTop = Lua.lua_gettop(l);
		int i;
		for (i = 1; i < 100; i++)
		{
			Lua.lua_pushnumber(l, i);
			Lua.lua_rawget(l, stack);
			if (Lua.lua_isnil(l, -1))
			{
				break;
			}
			int num2 = -1;
			if (i < num)
			{
				switch (format[i - 1])
				{
				case 's':
					num2 = 3;
					break;
				case 'd':
					num2 = 2;
					break;
				case 'i':
					num2 = 1;
					break;
				}
			}
			if (num2 == -1)
			{
				num2 = ((Lua.lua_type(l, -1) != LuaTypes.LUA_TNUMBER) ? 3 : (Lua.lua_isinteger(l, -1) ? 1 : 2));
			}
			DBAnyValue item = default(DBAnyValue);
			switch (num2)
			{
			case 1:
				item.lv = Lua.xlua_tointeger(l, -1);
				item.type = 1;
				break;
			case 2:
				item.fv = Lua.lua_tonumber(l, -1);
				item.type = 2;
				break;
			default:
			{
				string text = Lua.lua_tostring(l, -1);
				item.sv = text ?? "";
				item.type = 3;
				break;
			}
			}
			row.Add(item);
			Lua.lua_pop(l, 1);
		}
		int result = i - 1;
		Lua.lua_settop(l, newTop);
		return result;
	}

	private static int ValueTableToRows(List<List<DBAnyValue>> rows, string format, int stack)
	{
		IntPtr l = GameEntry.Lua.Env.L;
		if (!string.IsNullOrEmpty(format))
		{
			_ = format.Length;
		}
		int num = Lua.lua_gettop(l);
		int num2 = 0;
		if (!Lua.lua_istable(l, stack))
		{
			return 0;
		}
		bool flag = false;
		Lua.lua_pushnumber(l, 1.0);
		Lua.lua_rawget(l, stack);
		num = Lua.lua_gettop(l);
		if (Lua.lua_istable(l, -1))
		{
			flag = true;
		}
		Lua.lua_pop(l, 1);
		num = Lua.lua_gettop(l);
		if (flag)
		{
			int i;
			for (i = 1; i < 1001; i++)
			{
				num = Lua.lua_gettop(l);
				Lua.lua_pushnumber(l, i);
				Lua.lua_rawget(l, stack);
				if (Lua.lua_isnil(l, -1))
				{
					break;
				}
				num = Lua.lua_gettop(l);
				List<DBAnyValue> list = new List<DBAnyValue>();
				ValueTableToRow(list, format, num);
				rows.Add(list);
				Lua.lua_pop(l, 1);
			}
			num2 = i - 1;
		}
		else
		{
			List<DBAnyValue> list2 = new List<DBAnyValue>(4);
			num2 = ValueTableToRow(list2, format, stack);
			rows.Add(list2);
		}
		Lua.lua_settop(l, num);
		return num2;
	}

	public static void ExecuteMultiSQL(List<string> cmdStr, Action<LuaTable> callback = null)
	{
		DatabaseManager.Instance.ExecuteMulti(cmdStr, delegate(List<DBExecResult> ret)
		{
			if (ret == null)
			{
				callback(null);
			}
			else
			{
				LuaTable luaTable = GameEntry.Lua.Env.NewTable();
				for (int i = 0; i < ret.Count; i++)
				{
					LuaTable value = DBExecResult2LuaTable(ret[i]);
					luaTable.Set(i + 1, value);
				}
				callback(luaTable);
				luaTable.Dispose();
			}
		});
	}

	public static void ExecuteSTMT()
	{
		if (GameEntry.Lua == null)
		{
			return;
		}
		IntPtr l = GameEntry.Lua.Env.L;
		if ((int)l == 0)
		{
			return;
		}
		int newTop = Lua.lua_gettop(l);
		ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(l);
		int num = Lua.lua_gettop(l);
		if (num != 4)
		{
			return;
		}
		string sqlstmt = Lua.lua_tostring(l, 1);
		string format = Lua.lua_tostring(l, 3);
		Action<LuaTable> _callback = null;
		if (num >= 4)
		{
			_callback = objectTranslator.GetDelegate<Action<LuaTable>>(l, 4);
		}
		List<List<DBAnyValue>> list = null;
		if (Lua.lua_istable(l, 2))
		{
			list = new List<List<DBAnyValue>>();
			ValueTableToRows(list, format, 2);
		}
		Lua.lua_settop(l, newTop);
		DatabaseManager.Instance.ExecuteSTMT(sqlstmt, list, delegate(DBExecResult ret)
		{
			if (_callback != null)
			{
				if (ret == null)
				{
					_callback(null);
				}
				else
				{
					LuaTable luaTable = DBExecResult2LuaTable(ret);
					_callback(luaTable);
					luaTable.Dispose();
				}
			}
		});
	}

	public static void ExecuteUrgentSQL(string cmdStr, Action<LuaTable> callback = null)
	{
		DatabaseManager.Instance.Execute3(cmdStr, delegate(DBExecResult ret)
		{
			if (ret == null)
			{
				callback(null);
			}
			else
			{
				LuaTable luaTable = DBExecResult2LuaTable(ret);
				callback(luaTable);
				luaTable.Dispose();
			}
		});
	}

	public static void ExecuteSQL(string cmdStr, Action<LuaTable> callback = null)
	{
		DatabaseManager.Instance.Execute2(cmdStr, delegate(DBExecResult ret)
		{
			if (ret == null)
			{
				callback(null);
			}
			else
			{
				LuaTable luaTable = DBExecResult2LuaTable(ret);
				callback(luaTable);
				luaTable.Dispose();
			}
		});
	}
}
