using BestHTTP;
using UnityEngine;

public class BestHTTPAdapter
{
	private static bool _configured;

	public static void Configure()
	{
		if (!_configured)
		{
			_configured = true;
			HTTPManager.RootCacheFolderProvider = () => Application.persistentDataPath;
			HTTPManager.MaxConnectionPerServer = 4;
			HTTPManager.Setup();
		}
	}
}
