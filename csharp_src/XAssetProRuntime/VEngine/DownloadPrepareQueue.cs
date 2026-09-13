using System.Collections.Generic;

namespace VEngine;

public class DownloadPrepareQueue
{
	public uint MaxDownloads = 10u;

	public List<Download> Prepared = new List<Download>();

	public DownloadPrepareQueue(uint max)
	{
		MaxDownloads = max;
	}
}
