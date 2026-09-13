using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using BestHTTP.Logger;
using BestHTTP.PlatformSupport.FileSystem;

namespace BestHTTP.Caching;

public static class HTTPCacheService
{
	private const int LibraryVersion = 2;

	private static bool isSupported;

	private static bool IsSupportCheckDone;

	private static Dictionary<Uri, HTTPCacheFileInfo> library;

	private static Dictionary<ulong, HTTPCacheFileInfo> UsedIndexes;

	private static bool InClearThread;

	private static bool InMaintainenceThread;

	private static ulong NextNameIDX;

	public static bool IsSupported
	{
		get
		{
			if (IsSupportCheckDone)
			{
				return isSupported;
			}
			try
			{
				HTTPManager.IOService.DirectoryExists(HTTPManager.GetRootCacheFolder());
				isSupported = true;
			}
			catch
			{
				isSupported = false;
				HTTPManager.Logger.Warning("HTTPCacheService", "Cache Service Disabled!");
			}
			finally
			{
				IsSupportCheckDone = true;
			}
			return isSupported;
		}
	}

	private static Dictionary<Uri, HTTPCacheFileInfo> Library
	{
		get
		{
			LoadLibrary();
			return library;
		}
	}

	internal static string CacheFolder { get; private set; }

	private static string LibraryPath { get; set; }

	static HTTPCacheService()
	{
		UsedIndexes = new Dictionary<ulong, HTTPCacheFileInfo>();
		NextNameIDX = 1uL;
	}

	internal static void CheckSetup()
	{
		if (!IsSupported)
		{
			return;
		}
		try
		{
			SetupCacheFolder();
			LoadLibrary();
		}
		catch
		{
		}
	}

	internal static void SetupCacheFolder()
	{
		if (!IsSupported)
		{
			return;
		}
		try
		{
			if (string.IsNullOrEmpty(CacheFolder) || string.IsNullOrEmpty(LibraryPath))
			{
				CacheFolder = Path.Combine(HTTPManager.GetRootCacheFolder(), "HTTPCache");
				if (!HTTPManager.IOService.DirectoryExists(CacheFolder))
				{
					HTTPManager.IOService.DirectoryCreate(CacheFolder);
				}
				LibraryPath = Path.Combine(HTTPManager.GetRootCacheFolder(), "Library");
			}
		}
		catch
		{
			isSupported = false;
			HTTPManager.Logger.Warning("HTTPCacheService", "Cache Service Disabled!");
		}
	}

	internal static ulong GetNameIdx()
	{
		lock (Library)
		{
			ulong nextNameIDX = NextNameIDX;
			do
			{
				NextNameIDX = ++NextNameIDX % ulong.MaxValue;
			}
			while (UsedIndexes.ContainsKey(NextNameIDX));
			return nextNameIDX;
		}
	}

	internal static bool HasEntity(Uri uri)
	{
		if (!IsSupported)
		{
			return false;
		}
		lock (Library)
		{
			return Library.ContainsKey(uri);
		}
	}

	public static bool DeleteEntity(Uri uri, bool removeFromLibrary = true)
	{
		if (!IsSupported)
		{
			return false;
		}
		lock (HTTPCacheFileLock.Acquire(uri))
		{
			lock (Library)
			{
				HTTPCacheFileInfo value;
				bool num = Library.TryGetValue(uri, out value);
				if (num)
				{
					value.Delete();
				}
				if (num && removeFromLibrary)
				{
					Library.Remove(uri);
					UsedIndexes.Remove(value.MappedNameIDX);
				}
				return true;
			}
		}
	}

	internal static bool IsCachedEntityExpiresInTheFuture(HTTPRequest request)
	{
		if (!IsSupported)
		{
			return false;
		}
		lock (Library)
		{
			if (Library.TryGetValue(request.CurrentUri, out var value))
			{
				return value.WillExpireInTheFuture();
			}
		}
		return false;
	}

	internal static void SetHeaders(HTTPRequest request)
	{
		if (!IsSupported)
		{
			return;
		}
		request.RemoveHeader("If-None-Match");
		request.RemoveHeader("If-Modified-Since");
		lock (Library)
		{
			if (Library.TryGetValue(request.CurrentUri, out var value))
			{
				value.SetUpRevalidationHeaders(request);
			}
		}
	}

	internal static HTTPCacheFileInfo GetEntity(Uri uri)
	{
		if (!IsSupported)
		{
			return null;
		}
		HTTPCacheFileInfo value = null;
		lock (Library)
		{
			Library.TryGetValue(uri, out value);
			return value;
		}
	}

	internal static HTTPResponse GetFullResponse(HTTPRequest request)
	{
		if (!IsSupported)
		{
			return null;
		}
		lock (Library)
		{
			if (Library.TryGetValue(request.CurrentUri, out var value))
			{
				return value.ReadResponseTo(request);
			}
		}
		return null;
	}

	internal static bool IsCacheble(Uri uri, HTTPMethods method, HTTPResponse response)
	{
		if (!IsSupported)
		{
			return false;
		}
		if (method != HTTPMethods.Get)
		{
			return false;
		}
		if (response == null)
		{
			return false;
		}
		if (response.StatusCode < 200 || response.StatusCode >= 400)
		{
			return false;
		}
		List<string> headerValues = response.GetHeaderValues("cache-control");
		if (headerValues != null && headerValues.Exists(delegate(string headerValue)
		{
			string text = headerValue.ToLower();
			return text.Contains("no-store") || text.Contains("no-cache");
		}))
		{
			return false;
		}
		List<string> headerValues2 = response.GetHeaderValues("pragma");
		if (headerValues2 != null && headerValues2.Exists(delegate(string headerValue)
		{
			string text = headerValue.ToLower();
			return text.Contains("no-store") || text.Contains("no-cache");
		}))
		{
			return false;
		}
		if (response.GetHeaderValues("content-range") != null)
		{
			return false;
		}
		return true;
	}

	internal static HTTPCacheFileInfo Store(Uri uri, HTTPMethods method, HTTPResponse response)
	{
		if (response == null || response.Data == null || response.Data.Length == 0)
		{
			return null;
		}
		if (!IsSupported)
		{
			return null;
		}
		HTTPCacheFileInfo value = null;
		lock (Library)
		{
			if (!Library.TryGetValue(uri, out value))
			{
				Library.Add(uri, value = new HTTPCacheFileInfo(uri));
				UsedIndexes.Add(value.MappedNameIDX, value);
			}
			try
			{
				value.Store(response);
				if (HTTPManager.Logger.Level == Loglevels.All)
				{
					HTTPManager.Logger.Verbose("HTTPCacheService", $"{uri.ToString()} - Saved to cache");
				}
			}
			catch
			{
				DeleteEntity(uri);
				throw;
			}
		}
		return value;
	}

	internal static Stream PrepareStreamed(Uri uri, HTTPResponse response)
	{
		if (!IsSupported)
		{
			return null;
		}
		lock (Library)
		{
			if (!Library.TryGetValue(uri, out var value))
			{
				Library.Add(uri, value = new HTTPCacheFileInfo(uri));
				UsedIndexes.Add(value.MappedNameIDX, value);
			}
			try
			{
				return value.GetSaveStream(response);
			}
			catch
			{
				DeleteEntity(uri);
				throw;
			}
		}
	}

	public static void BeginClear()
	{
		if (IsSupported && !InClearThread)
		{
			InClearThread = true;
			SetupCacheFolder();
			ThreadPool.QueueUserWorkItem(delegate(object param)
			{
				ClearImpl(param);
			});
		}
	}

	private static void ClearImpl(object param)
	{
		if (!IsSupported)
		{
			return;
		}
		try
		{
			string[] files = HTTPManager.IOService.GetFiles(CacheFolder);
			for (int i = 0; i < files.Length; i++)
			{
				try
				{
					HTTPManager.IOService.FileDelete(files[i]);
				}
				catch
				{
				}
			}
		}
		finally
		{
			UsedIndexes.Clear();
			library.Clear();
			NextNameIDX = 1uL;
			SaveLibrary();
			InClearThread = false;
		}
	}

	public static void BeginMaintainence(HTTPCacheMaintananceParams maintananceParam)
	{
		if (maintananceParam == null)
		{
			throw new ArgumentNullException("maintananceParams == null");
		}
		if (!IsSupported || InMaintainenceThread)
		{
			return;
		}
		InMaintainenceThread = true;
		SetupCacheFolder();
		ThreadPool.QueueUserWorkItem(delegate
		{
			try
			{
				lock (Library)
				{
					DateTime dateTime = DateTime.UtcNow - maintananceParam.DeleteOlder;
					List<HTTPCacheFileInfo> list = new List<HTTPCacheFileInfo>();
					foreach (KeyValuePair<Uri, HTTPCacheFileInfo> item in Library)
					{
						if (item.Value.LastAccess < dateTime && DeleteEntity(item.Key, removeFromLibrary: false))
						{
							list.Add(item.Value);
						}
					}
					for (int i = 0; i < list.Count; i++)
					{
						Library.Remove(list[i].Uri);
						UsedIndexes.Remove(list[i].MappedNameIDX);
					}
					list.Clear();
					ulong num = GetCacheSize();
					if (num > maintananceParam.MaxCacheSize)
					{
						List<HTTPCacheFileInfo> list2 = new List<HTTPCacheFileInfo>(library.Count);
						foreach (KeyValuePair<Uri, HTTPCacheFileInfo> item2 in library)
						{
							list2.Add(item2.Value);
						}
						list2.Sort();
						int num2 = 0;
						while (num >= maintananceParam.MaxCacheSize && num2 < list2.Count)
						{
							try
							{
								HTTPCacheFileInfo hTTPCacheFileInfo = list2[num2];
								ulong num3 = (ulong)hTTPCacheFileInfo.BodyLength;
								DeleteEntity(hTTPCacheFileInfo.Uri);
								num -= num3;
							}
							catch
							{
							}
							finally
							{
								num2++;
							}
						}
					}
				}
			}
			finally
			{
				SaveLibrary();
				InMaintainenceThread = false;
			}
		});
	}

	public static int GetCacheEntityCount()
	{
		if (!IsSupported)
		{
			return 0;
		}
		CheckSetup();
		lock (Library)
		{
			return Library.Count;
		}
	}

	public static ulong GetCacheSize()
	{
		ulong num = 0uL;
		if (!IsSupported)
		{
			return num;
		}
		CheckSetup();
		lock (Library)
		{
			foreach (KeyValuePair<Uri, HTTPCacheFileInfo> item in Library)
			{
				if (item.Value.BodyLength > 0)
				{
					num += (ulong)item.Value.BodyLength;
				}
			}
			return num;
		}
	}

	private static void LoadLibrary()
	{
		if (library != null || !IsSupported)
		{
			return;
		}
		library = new Dictionary<Uri, HTTPCacheFileInfo>(new UriComparer());
		if (!HTTPManager.IOService.FileExists(LibraryPath))
		{
			DeleteUnusedFiles();
			return;
		}
		try
		{
			int num;
			lock (library)
			{
				using Stream input = HTTPManager.IOService.CreateFileStream(LibraryPath, FileStreamModes.Open);
				using BinaryReader binaryReader = new BinaryReader(input);
				num = binaryReader.ReadInt32();
				if (num > 1)
				{
					NextNameIDX = binaryReader.ReadUInt64();
				}
				int num2 = binaryReader.ReadInt32();
				for (int i = 0; i < num2; i++)
				{
					Uri uri = new Uri(binaryReader.ReadString());
					HTTPCacheFileInfo hTTPCacheFileInfo = new HTTPCacheFileInfo(uri, binaryReader, num);
					if (hTTPCacheFileInfo.IsExists())
					{
						library.Add(uri, hTTPCacheFileInfo);
						if (num > 1)
						{
							UsedIndexes.Add(hTTPCacheFileInfo.MappedNameIDX, hTTPCacheFileInfo);
						}
					}
				}
			}
			if (num == 1)
			{
				BeginClear();
			}
			else
			{
				DeleteUnusedFiles();
			}
		}
		catch
		{
		}
	}

	internal static void SaveLibrary()
	{
		if (library == null || !IsSupported)
		{
			return;
		}
		try
		{
			lock (Library)
			{
				using Stream output = HTTPManager.IOService.CreateFileStream(LibraryPath, FileStreamModes.Create);
				using BinaryWriter binaryWriter = new BinaryWriter(output);
				binaryWriter.Write(2);
				binaryWriter.Write(NextNameIDX);
				binaryWriter.Write(Library.Count);
				foreach (KeyValuePair<Uri, HTTPCacheFileInfo> item in Library)
				{
					binaryWriter.Write(item.Key.ToString());
					item.Value.SaveTo(binaryWriter);
				}
			}
		}
		catch
		{
		}
	}

	internal static void SetBodyLength(Uri uri, int bodyLength)
	{
		if (!IsSupported)
		{
			return;
		}
		lock (Library)
		{
			if (Library.TryGetValue(uri, out var value))
			{
				value.BodyLength = bodyLength;
				return;
			}
			Library.Add(uri, value = new HTTPCacheFileInfo(uri, DateTime.UtcNow, bodyLength));
			UsedIndexes.Add(value.MappedNameIDX, value);
		}
	}

	private static void DeleteUnusedFiles()
	{
		if (!IsSupported)
		{
			return;
		}
		CheckSetup();
		string[] files = HTTPManager.IOService.GetFiles(CacheFolder);
		for (int i = 0; i < files.Length; i++)
		{
			try
			{
				string fileName = Path.GetFileName(files[i]);
				ulong result = 0uL;
				bool flag = false;
				if (ulong.TryParse(fileName, NumberStyles.AllowHexSpecifier, null, out result))
				{
					lock (Library)
					{
						flag = !UsedIndexes.ContainsKey(result);
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					HTTPManager.IOService.FileDelete(files[i]);
				}
			}
			catch
			{
			}
		}
	}
}
