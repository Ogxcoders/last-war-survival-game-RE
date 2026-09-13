using System.Collections.Generic;
using System.IO;
using VEngine;

public class DownloadUpdateStateBundleChecker : DownloadChecker
{
	public Queue<string> originBundleNames = new Queue<string>();

	protected override void OnCollect()
	{
		int i = 0;
		for (int num = _downloads.Length; i < num; i++)
		{
			foreach (KeyValuePair<string, long> item in _downloads[i])
			{
				string extension = Path.GetExtension(item.Key);
				if ((extension == ".bundle" || extension == ".raw") && (item.Key.StartsWith("gameres") || item.Key.StartsWith("dllres")))
				{
					originBundleNames.Enqueue(item.Key);
				}
			}
		}
	}
}
