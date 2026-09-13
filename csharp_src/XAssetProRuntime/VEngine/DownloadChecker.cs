using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace VEngine;

public class DownloadChecker : IDownloadChecker
{
	protected string[] _downloadPaths;

	protected Dictionary<string, long>[] _downloads;

	public void Collect(string downloadPath)
	{
		Collect(new string[1] { downloadPath });
	}

	public void Collect(string[] downloadPaths)
	{
		int num = downloadPaths.Length;
		_downloadPaths = downloadPaths;
		_downloads = new Dictionary<string, long>[num];
		for (int i = 0; i < num; i++)
		{
			_downloads[i] = new Dictionary<string, long>(4096);
			foreach (string item in Directory.EnumerateFiles(_downloadPaths[i], "*"))
			{
				string fileName = Path.GetFileName(item);
				_downloads[i].Add(fileName, -1L);
			}
		}
		OnCollect();
	}

	protected virtual void OnCollect()
	{
	}

	public void Log(string tag)
	{
		string text = Application.persistentDataPath + "/DC_Logs";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string[] files = Directory.GetFiles(text, "*.log");
		foreach (string fileName in files)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(fileName);
				if (fileInfo.LastWriteTime < DateTime.Now.AddDays(-7.0))
				{
					fileInfo.Delete();
				}
			}
			catch (Exception)
			{
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		int j = 0;
		for (int num = _downloadPaths.Length; j < num; j++)
		{
			stringBuilder.AppendLine($"{j}, {_downloadPaths[j]}");
			foreach (KeyValuePair<string, long> item in _downloads[j])
			{
				stringBuilder.AppendLine(item.Key);
			}
		}
		string text2 = DateTime.Now.ToString("yyyyMMdd_HHmmss");
		File.WriteAllText(text + "/" + tag + "_" + text2 + ".log", stringBuilder.ToString());
	}

	public bool IsDownloaded(BundleInfo bundle)
	{
		if (Versions.SkipUpdate || bundle.inPlayerAssets)
		{
			return true;
		}
		if (Versions.BundleFastValidation)
		{
			int i = 0;
			for (int num = _downloadPaths.Length; i < num; i++)
			{
				Dictionary<string, long> dictionary = _downloads[i];
				if (dictionary.ContainsKey(bundle.name))
				{
					return true;
				}
				if (dictionary.ContainsKey(bundle.alias))
				{
					return true;
				}
			}
			return false;
		}
		long value = 0L;
		int j = 0;
		for (int num2 = _downloadPaths.Length; j < num2; j++)
		{
			Dictionary<string, long> dictionary2 = _downloads[j];
			if (dictionary2.TryGetValue(bundle.name, out value))
			{
				if (value < 0)
				{
					string text = Path.Combine(_downloadPaths[j], bundle.name);
					FileInfo fileInfo = new FileInfo(text);
					value = (dictionary2[text] = (fileInfo.Exists ? fileInfo.Length : 0));
				}
				return value == (long)bundle.size;
			}
			if (dictionary2.TryGetValue(bundle.alias, out value))
			{
				if (value < 0)
				{
					string text2 = Path.Combine(_downloadPaths[j], bundle.alias);
					FileInfo fileInfo2 = new FileInfo(text2);
					value = (dictionary2[text2] = (fileInfo2.Exists ? fileInfo2.Length : 0));
				}
				return value == (long)bundle.size;
			}
		}
		return false;
	}
}
