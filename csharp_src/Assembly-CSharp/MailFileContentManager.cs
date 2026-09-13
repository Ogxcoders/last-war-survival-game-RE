using System;
using System.Collections.Generic;

public class MailFileContentManager
{
	public static void InitFileManager(bool useBinary)
	{
		FileContentHelper.useBinary = useBinary;
		FileContentManager.Instance.Release();
		FileContentManager.Instance.Initialize();
	}

	public static void UninitFileManager()
	{
		FileContentManager.Instance.Release();
	}

	public static bool IsFileExist(string id)
	{
		return FileContentManager.Instance.IsFileExist(id);
	}

	public static void DeleteFile(string id, Action callback)
	{
		FileContentManager.Instance.ExecuteDelete(id, callback);
	}

	public static void CreateFile(string id, string content, Action callback)
	{
		FileContentManager.Instance.ExecuteCreate(id, content, callback);
	}

	public static void ReadFile(string id, Action<string> callback)
	{
		FileContentManager.Instance.ExecuteRead(id, callback);
	}

	public static void DeleteFileList(List<string> ids, Action callback)
	{
		for (int i = 0; i < ids.Count; i++)
		{
			FileContentManager.Instance.ExecuteDelete(ids[i], callback);
		}
	}
}
