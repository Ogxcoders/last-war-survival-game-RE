using System;

namespace UnityEngine.Networking;

public static class UnityWebRequestTexture
{
	public static UnityWebRequest GetTexture(string uri)
	{
		return GetTexture(uri, nonReadable: false);
	}

	public static UnityWebRequest GetTexture(Uri uri)
	{
		return GetTexture(uri, nonReadable: false);
	}

	public static UnityWebRequest GetTexture(string uri, bool nonReadable)
	{
		return new UnityWebRequest(uri, "GET", new DownloadHandlerTexture(!nonReadable), null);
	}

	public static UnityWebRequest GetTexture(Uri uri, bool nonReadable)
	{
		return new UnityWebRequest(uri, "GET", new DownloadHandlerTexture(!nonReadable), null);
	}
}
