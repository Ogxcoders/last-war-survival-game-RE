using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using GameFramework;
using GameKit.Base;
using SQLite4Unity3d;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{
	private static DatabaseManager _instance;

	private static readonly object _lock = new object();

	private Action<bool> OnInitCallback;

	private QueuedThread thread;

	private SQLiteConnection dbConnection;

	private Queue<DatabaseActionTask> taskQueue = new Queue<DatabaseActionTask>();

	private Queue<DatabaseActionTask> urgentTaskQueue = new Queue<DatabaseActionTask>();

	private bool isInited;

	public static DatabaseManager Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = UnityEngine.Object.FindObjectOfType<DatabaseManager>();
						if (UnityEngine.Object.FindObjectsOfType<DatabaseManager>().Length > 1)
						{
							return _instance;
						}
						if (_instance == null)
						{
							GameObject obj = new GameObject("(Singleton) " + typeof(DatabaseManager));
							_instance = obj.AddComponent<DatabaseManager>();
							UnityEngine.Object.DontDestroyOnLoad(obj);
						}
					}
				}
			}
			return _instance;
		}
	}

	public void Initialize(string databaseFile, Action<bool> callback)
	{
		OnInitCallback = callback;
		string dstPath = Path.Combine(Application.persistentDataPath, databaseFile);
		if (!File.Exists(dstPath))
		{
			string uri = Path.Combine(Application.streamingAssetsPath, databaseFile);
			SingletonBehaviour<WebRequestManager>.Instance.Get(uri, delegate(UnityWebRequest request, bool hasErr, object userdata)
			{
				if (!hasErr)
				{
					if (request.isDone)
					{
						File.WriteAllBytes(dstPath, request.downloadHandler.data);
						InitDatabase(dstPath);
					}
				}
				else
				{
					Log.Error(request.error);
				}
			});
		}
		else
		{
			InitDatabase(dstPath);
		}
	}

	public void Release()
	{
		try
		{
			taskQueue.Clear();
			urgentTaskQueue.Clear();
			if (thread != null)
			{
				thread.Stop();
				thread = null;
			}
			if (dbConnection != null)
			{
				dbConnection.Close();
				dbConnection = null;
			}
			OnInitCallback = null;
		}
		catch (Exception ex)
		{
			Log.Error("dbmanager release error", ex.Message);
		}
	}

	private void InitDatabase(string path)
	{
		Log.Info("InitDatabase begin: {0}", path);
		dbConnection = new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
		DBFileValidation(path);
		if (dbConnection != null)
		{
			thread = new QueuedThread("DatabaseThread");
			thread.Start();
			isInited = true;
			Log.Info("InitDatabase succeed: {0}", path);
		}
		else
		{
			Log.Error("InitDatabase failed: {0}", path);
		}
		OnInitCallback?.Invoke(isInited);
	}

	private void DBFileValidation(string path)
	{
		if (File.Exists(path))
		{
			bool flag = true;
			try
			{
				DBExecResult dBExecResult = ExeHelper.ExecSql(dbConnection, "SELECT name FROM sqlite_master WHERE type='table';");
				if (dBExecResult.error == 0 && dBExecResult.col_count != 0)
				{
					int i = 0;
					for (int count = dBExecResult.values.Count; i < count; i++)
					{
						string sv = dBExecResult.values[i][0].sv;
						try
						{
							DBExecResult dBExecResult2 = ExeHelper.ExecSql(dbConnection, "SELECT COUNT(*) FROM " + sv + ";");
							if (dBExecResult2.error != 0 || dBExecResult2.col_count == 0)
							{
								Log.Error("InitDatabase test connection. table: " + sv + ", rowCount: " + dBExecResult2.errormsg);
							}
							DBExecResult dBExecResult3 = ExeHelper.ExecSql(dbConnection, "PRAGMA table_info(" + sv + ");");
							StringBuilder stringBuilder = new StringBuilder();
							int j = 0;
							for (int count2 = dBExecResult3.values.Count; j < count2; j++)
							{
								stringBuilder.Append($"{j}: {dBExecResult3.values[j][1].sv}({dBExecResult3.values[j][2].sv}), ");
							}
						}
						catch (Exception arg)
						{
							Log.Error($"InitDatabase test connection. query table cols: {arg}");
						}
					}
				}
				else
				{
					Log.Error("InitDatabase test connection. query table: " + dBExecResult.errormsg);
					flag = false;
				}
			}
			catch (Exception arg2)
			{
				Log.Error($"InitDatabase test connection. {arg2}");
				flag = false;
			}
			if (!flag)
			{
				Log.Info("InitDatabase begin recreate db file.");
				dbConnection.Close();
				File.Delete(path);
				string path2 = path + "-journal";
				if (File.Exists(path2))
				{
					File.Delete(path2);
				}
				Log.Info("InitDatabase create db file.");
				dbConnection = new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
			}
		}
		else
		{
			Log.Error("InitDatabase can`t find file " + path);
		}
	}

	private void Update()
	{
		UpdateTask();
	}

	public void UpdateTask()
	{
		if (thread == null)
		{
			return;
		}
		int num = 5;
		while (num > 0 && urgentTaskQueue.Count > 0)
		{
			if (urgentTaskQueue.Peek().Processed)
			{
				try
				{
					urgentTaskQueue.Dequeue().CallBack();
				}
				catch (Exception ex)
				{
					Log.Error(ex.Message);
				}
			}
			num--;
		}
		while (num > 0 && taskQueue.Count > 0)
		{
			if (taskQueue.Peek().Processed)
			{
				try
				{
					taskQueue.Dequeue().CallBack();
				}
				catch (Exception ex2)
				{
					Log.Error(ex2.Message);
				}
			}
			num--;
		}
	}

	public void Execute2(string cmdStr, Action<DBExecResult> callback = null)
	{
		if (!isInited)
		{
			callback?.Invoke(null);
			return;
		}
		ExecuteTask2 executeTask = new ExecuteTask2(dbConnection, cmdStr, callback);
		thread.AddTask(executeTask);
		taskQueue.Enqueue(executeTask);
	}

	public void Execute3(string cmdStr, Action<DBExecResult> callback = null)
	{
		if (!isInited)
		{
			callback?.Invoke(null);
			return;
		}
		ExecuteTask2 executeTask = new ExecuteTask2(dbConnection, cmdStr, callback);
		thread.AddUrgentTask(executeTask);
		urgentTaskQueue.Enqueue(executeTask);
	}

	public void ExecuteSTMT(string sqlstmt, List<List<DBAnyValue>> values, Action<DBExecResult> callback = null)
	{
		if (!isInited)
		{
			callback?.Invoke(null);
			return;
		}
		ExecuteStmtTask executeStmtTask = new ExecuteStmtTask(dbConnection, sqlstmt, values, callback);
		thread.AddTask(executeStmtTask);
		taskQueue.Enqueue(executeStmtTask);
	}

	public void ExecuteMulti(List<string> cmdStr, Action<List<DBExecResult>> callback = null)
	{
		if (!isInited)
		{
			callback?.Invoke(null);
			return;
		}
		MultiExecuteTask multiExecuteTask = new MultiExecuteTask(dbConnection, cmdStr, callback);
		thread.AddTask(multiExecuteTask);
		taskQueue.Enqueue(multiExecuteTask);
	}

	public static string GetPrimaryKeyValue<T>(T obj, SQLiteConnection dbConnection)
	{
		TableMapping mapping = dbConnection.GetMapping<T>();
		PropertyInfo property = obj.GetType().GetProperty(mapping.PK.PropertyName);
		if (property == null)
		{
			return string.Empty;
		}
		return property.GetValue(obj) as string;
	}
}
