using System;
using System.Collections.Generic;

public class OSSClientManager
{
	private static OSSClientManager _instance;

	private static string _accessKeyId = "";

	private static string _accessKeySecret = "";

	private static string _endpoint = "";

	private static HashSet<IAsyncResult> _puttingOperations = new HashSet<IAsyncResult>();

	private static HashSet<IAsyncResult> _gettingOperations = new HashSet<IAsyncResult>();

	public static OSSClientManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new OSSClientManager();
			}
			return _instance;
		}
	}

	public void Release()
	{
	}

	public void ListBuckets()
	{
	}

	private static void AsyncGetObjectCallback(IAsyncResult ar)
	{
	}

	private static void AsyncPutObjectCallback(IAsyncResult ar)
	{
	}
}
