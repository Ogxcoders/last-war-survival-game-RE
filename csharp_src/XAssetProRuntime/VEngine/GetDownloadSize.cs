using System.Collections.Generic;

namespace VEngine;

public sealed class GetDownloadSize : Operation
{
	private readonly List<BundleInfo> bundles = new List<BundleInfo>();

	public readonly List<DownloadInfo> result = new List<DownloadInfo>();

	public GetDownloadSizeMode mode;

	public Manifest[] manifests;

	public int count => bundles.Count;

	public int index { get; private set; }

	public ulong totalSize { get; private set; }

	public string[] items { get; set; }

	public string current
	{
		get
		{
			if (index < bundles.Count)
			{
				return bundles[index].name;
			}
			return string.Empty;
		}
	}

	public override void Start()
	{
		base.Start();
		if (Versions.SkipUpdate)
		{
			Finish();
			return;
		}
		index = 0;
		totalSize = 0uL;
		if (bundles.Count > 0)
		{
			bundles.Clear();
		}
		bundles.AddRange((mode == GetDownloadSizeMode.Groups) ? Versions.GetBundlesWithGroups(manifests, items) : Versions.GetBundlesWithAssets(manifests, items));
		if (bundles.Count == 0)
		{
			Finish();
		}
	}

	protected override void Update()
	{
		OperationStatus operationStatus = base.status;
		if (operationStatus != OperationStatus.Processing)
		{
			return;
		}
		while (index < bundles.Count)
		{
			BundleInfo bundleInfo = bundles[index];
			string savePath = Versions.GetDownloadDataPath(bundleInfo.name);
			if (!Versions.IsDownloaded(bundleInfo) && !result.Exists((DownloadInfo downloadInfo) => downloadInfo.savePath == savePath))
			{
				totalSize += bundleInfo.size;
				result.Add(new DownloadInfo
				{
					crc = bundleInfo.crc,
					url = Versions.GetDownloadURL(bundleInfo.name),
					size = bundleInfo.size,
					savePath = savePath
				});
			}
			index++;
			if (index == bundles.Count)
			{
				Finish();
			}
			if (Updater.busy)
			{
				break;
			}
		}
	}
}
