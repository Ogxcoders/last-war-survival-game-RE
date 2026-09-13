using System.Collections.Generic;
using VEngine;

internal class DownloadUpdateBkground
{
	public List<Manifest> manifests;

	private DownloadVersions _downloadVersions;

	public void Start(List<DownloadInfo> downloadInfos)
	{
		_downloadVersions = Versions.DownloadAsync(downloadInfos.ToArray());
	}

	public void Update()
	{
		if (_downloadVersions == null || !_downloadVersions.isDone)
		{
			return;
		}
		if (_downloadVersions.isError)
		{
			List<DownloadInfo> downloadInfos = new List<DownloadInfo>();
			if (GameEntry.Resource.GetDownloadSize(manifests, downloadInfos) != 0)
			{
				Start(downloadInfos);
			}
			else
			{
				_downloadVersions = null;
			}
		}
		else
		{
			_downloadVersions = null;
		}
	}
}
