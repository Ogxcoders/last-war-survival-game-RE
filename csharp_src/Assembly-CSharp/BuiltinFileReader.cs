using System.IO;
using GameFramework;
using UnityEngine.Networking;
using VEngine;

public class BuiltinFileReader
{
	public static bool ReadyFileFromBuiltIn(string fileName, out string error, out DownloadHandler handler)
	{
		string text = Versions.LocalProtocol + fileName;
		Log.Info("Read File From BuiltIn " + text);
		UnityWebRequest unityWebRequest = UnityWebRequest.Get(text);
		unityWebRequest.SendWebRequest();
		while (!unityWebRequest.isDone)
		{
		}
		error = unityWebRequest.error;
		handler = unityWebRequest.downloadHandler;
		return string.IsNullOrEmpty(error);
	}

	public static bool CopyFileFromBuiltIn(string fileName, string savePath)
	{
		string text = Versions.LocalProtocol + fileName;
		Log.Info("Download File from BuiltIn " + text + ", " + savePath + ".");
		UnityWebRequest unityWebRequest = UnityWebRequest.Get(text);
		unityWebRequest.downloadHandler = new DownloadHandlerFile(savePath);
		unityWebRequest.SendWebRequest();
		while (!unityWebRequest.isDone)
		{
		}
		if (!string.IsNullOrEmpty(unityWebRequest.error))
		{
			File.Delete(savePath);
			Log.Error(unityWebRequest.error);
			return false;
		}
		return true;
	}
}
