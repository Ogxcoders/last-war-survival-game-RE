using System;
using System.Collections.Generic;
using System.IO;
using BestHTTP.PlatformSupport.FileSystem;

namespace BestHTTP.Cookies;

public static class CookieJar
{
	private const int Version = 1;

	public static TimeSpan AccessThreshold = TimeSpan.FromDays(7.0);

	private static List<Cookie> Cookies = new List<Cookie>();

	private static object Locker = new object();

	private static bool _isSavingSupported;

	private static bool IsSupportCheckDone;

	private static bool Loaded;

	public static bool IsSavingSupported
	{
		get
		{
			if (IsSupportCheckDone)
			{
				return _isSavingSupported;
			}
			try
			{
				HTTPManager.IOService.DirectoryExists(HTTPManager.GetRootCacheFolder());
				_isSavingSupported = true;
			}
			catch
			{
				_isSavingSupported = false;
				HTTPManager.Logger.Warning("CookieJar", "Cookie saving and loading disabled!");
			}
			finally
			{
				IsSupportCheckDone = true;
			}
			return _isSavingSupported;
		}
	}

	private static string CookieFolder { get; set; }

	private static string LibraryPath { get; set; }

	internal static void SetupFolder()
	{
		if (!IsSavingSupported)
		{
			return;
		}
		try
		{
			if (string.IsNullOrEmpty(CookieFolder) || string.IsNullOrEmpty(LibraryPath))
			{
				CookieFolder = Path.Combine(HTTPManager.GetRootCacheFolder(), "Cookies");
				LibraryPath = Path.Combine(CookieFolder, "Library");
			}
		}
		catch
		{
		}
	}

	internal static void Set(HTTPResponse response)
	{
		if (response == null)
		{
			return;
		}
		lock (Locker)
		{
			try
			{
				Maintain();
				List<Cookie> list = new List<Cookie>();
				List<string> headerValues = response.GetHeaderValues("set-cookie");
				if (headerValues == null)
				{
					return;
				}
				foreach (string item in headerValues)
				{
					try
					{
						Cookie cookie = Cookie.Parse(item, response.baseRequest.CurrentUri);
						if (cookie == null)
						{
							continue;
						}
						int idx;
						Cookie cookie2 = Find(cookie, out idx);
						if (!string.IsNullOrEmpty(cookie.Value) && cookie.WillExpireInTheFuture())
						{
							if (cookie2 == null)
							{
								Cookies.Add(cookie);
								list.Add(cookie);
							}
							else
							{
								cookie.Date = cookie2.Date;
								Cookies[idx] = cookie;
								list.Add(cookie);
							}
						}
						else if (idx != -1)
						{
							Cookies.RemoveAt(idx);
						}
					}
					catch
					{
					}
				}
				response.Cookies = list;
			}
			catch
			{
			}
		}
	}

	internal static void Maintain()
	{
		lock (Locker)
		{
			try
			{
				uint num = 0u;
				int num2 = 0;
				while (num2 < Cookies.Count)
				{
					Cookie cookie = Cookies[num2];
					if (!cookie.WillExpireInTheFuture() || cookie.LastAccess + AccessThreshold < DateTime.UtcNow)
					{
						Cookies.RemoveAt(num2);
						continue;
					}
					if (!cookie.IsSession)
					{
						num += cookie.GuessSize();
					}
					num2++;
				}
				if (num > HTTPManager.CookieJarSize)
				{
					Cookies.Sort();
					while (num > HTTPManager.CookieJarSize && Cookies.Count > 0)
					{
						Cookie cookie2 = Cookies[0];
						Cookies.RemoveAt(0);
						num -= cookie2.GuessSize();
					}
				}
			}
			catch
			{
			}
		}
	}

	internal static void Persist()
	{
		if (!IsSavingSupported)
		{
			return;
		}
		lock (Locker)
		{
			if (!Loaded)
			{
				return;
			}
			try
			{
				Maintain();
				if (!HTTPManager.IOService.DirectoryExists(CookieFolder))
				{
					HTTPManager.IOService.DirectoryCreate(CookieFolder);
				}
				using Stream output = HTTPManager.IOService.CreateFileStream(LibraryPath, FileStreamModes.Create);
				using BinaryWriter binaryWriter = new BinaryWriter(output);
				binaryWriter.Write(1);
				int num = 0;
				foreach (Cookie cookie in Cookies)
				{
					if (!cookie.IsSession)
					{
						num++;
					}
				}
				binaryWriter.Write(num);
				foreach (Cookie cookie2 in Cookies)
				{
					if (!cookie2.IsSession)
					{
						cookie2.SaveTo(binaryWriter);
					}
				}
			}
			catch
			{
			}
		}
	}

	internal static void Load()
	{
		if (!IsSavingSupported)
		{
			return;
		}
		lock (Locker)
		{
			if (Loaded)
			{
				return;
			}
			SetupFolder();
			try
			{
				Cookies.Clear();
				if (!HTTPManager.IOService.DirectoryExists(CookieFolder))
				{
					HTTPManager.IOService.DirectoryCreate(CookieFolder);
				}
				if (!HTTPManager.IOService.FileExists(LibraryPath))
				{
					return;
				}
				using Stream input = HTTPManager.IOService.CreateFileStream(LibraryPath, FileStreamModes.Open);
				using BinaryReader binaryReader = new BinaryReader(input);
				binaryReader.ReadInt32();
				int num = binaryReader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					Cookie cookie = new Cookie();
					cookie.LoadFrom(binaryReader);
					if (cookie.WillExpireInTheFuture())
					{
						Cookies.Add(cookie);
					}
				}
			}
			catch
			{
				Cookies.Clear();
			}
			finally
			{
				Loaded = true;
			}
		}
	}

	public static List<Cookie> Get(Uri uri)
	{
		lock (Locker)
		{
			Load();
			List<Cookie> list = null;
			for (int i = 0; i < Cookies.Count; i++)
			{
				Cookie cookie = Cookies[i];
				if (cookie.WillExpireInTheFuture() && uri.Host.IndexOf(cookie.Domain) != -1 && uri.AbsolutePath.StartsWith(cookie.Path))
				{
					if (list == null)
					{
						list = new List<Cookie>();
					}
					list.Add(cookie);
				}
			}
			return list;
		}
	}

	public static void Set(Uri uri, Cookie cookie)
	{
		Set(cookie);
	}

	public static void Set(Cookie cookie)
	{
		lock (Locker)
		{
			Load();
			Find(cookie, out var idx);
			if (idx >= 0)
			{
				Cookies[idx] = cookie;
			}
			else
			{
				Cookies.Add(cookie);
			}
		}
	}

	public static List<Cookie> GetAll()
	{
		lock (Locker)
		{
			Load();
			return Cookies;
		}
	}

	public static void Clear()
	{
		lock (Locker)
		{
			Load();
			Cookies.Clear();
		}
	}

	public static void Clear(TimeSpan olderThan)
	{
		lock (Locker)
		{
			Load();
			int num = 0;
			while (num < Cookies.Count)
			{
				Cookie cookie = Cookies[num];
				if (!cookie.WillExpireInTheFuture() || cookie.Date + olderThan < DateTime.UtcNow)
				{
					Cookies.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
		}
	}

	public static void Clear(string domain)
	{
		lock (Locker)
		{
			Load();
			int num = 0;
			while (num < Cookies.Count)
			{
				Cookie cookie = Cookies[num];
				if (!cookie.WillExpireInTheFuture() || cookie.Domain.IndexOf(domain) != -1)
				{
					Cookies.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
		}
	}

	public static void Remove(Uri uri, string name)
	{
		lock (Locker)
		{
			Load();
			int num = 0;
			while (num < Cookies.Count)
			{
				Cookie cookie = Cookies[num];
				if (cookie.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && uri.Host.IndexOf(cookie.Domain) != -1)
				{
					Cookies.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
		}
	}

	private static Cookie Find(Cookie cookie, out int idx)
	{
		for (int i = 0; i < Cookies.Count; i++)
		{
			Cookie cookie2 = Cookies[i];
			if (cookie2.Equals(cookie))
			{
				idx = i;
				return cookie2;
			}
		}
		idx = -1;
		return null;
	}
}
