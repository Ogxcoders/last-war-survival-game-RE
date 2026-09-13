using GameFramework;
using VEngine;

public class LocaleDownload : Operation
{
	private int _version;

	private string _localeAbb;

	private Download _downloadTxt;

	private Download _downloadBin;

	public LocaleDownload(int version, string localeAbb)
	{
		_version = version;
		_localeAbb = localeAbb;
	}

	public override void Start()
	{
		base.Start();
		DownloadInfo downloadInfo = new DownloadInfo();
		downloadInfo.crc = 0u;
		downloadInfo.savePath = ClientConfig.GetLocaleDownloadDataPath(_version, _localeAbb);
		downloadInfo.size = 0uL;
		downloadInfo.url = ClientConfig.LocaleDownloadURL(_version, _localeAbb);
		DownloadInfo downloadInfo2 = new DownloadInfo();
		downloadInfo2.crc = 0u;
		downloadInfo2.savePath = ClientConfig.GetLocaleBinDownloadDataPath(_version, _localeAbb);
		downloadInfo2.size = 0uL;
		downloadInfo2.url = ClientConfig.LocaleBinDownloadURL(_version, _localeAbb);
		_downloadTxt = Download.DownloadAsync(downloadInfo);
		_downloadBin = Download.DownloadAsync(downloadInfo2);
		Log.Info($"LocaleDownload::Start Download {_version}, {_localeAbb}, {downloadInfo.url}");
		Log.Info($"LocaleDownload::Start Download {_version}, {_localeAbb}, {downloadInfo2.url}");
	}

	protected override void Update()
	{
		if (_downloadTxt == null || _downloadBin == null || !_downloadTxt.isDone || !_downloadBin.isDone)
		{
			return;
		}
		if (string.IsNullOrEmpty(_downloadTxt.error) && string.IsNullOrEmpty(_downloadBin.error))
		{
			Log.Info($"LocaleDownload Download Succeed {_version}, {_localeAbb}");
			if (ApplicationLaunch.kParallelInit)
			{
				ApplicationLaunch.EnqueueTask(new LaunchTask(ELaunchTask.LocaleFileLoadInPersistent, new LocaleFileParallel.LocaleFileLoadInPersistentPayload
				{
					locale_version = _version,
					locale_abb = _localeAbb,
					isReload = true
				}));
			}
			else
			{
				GameEntry.Localization.ReloadDictionaryByUpdate(_version, _localeAbb);
			}
		}
		else
		{
			Log.Error($"LocaleDownload Download Failed Txt: {_version}, {_localeAbb}, {_downloadTxt.error}");
			Log.Error($"LocaleDownload Download Failed Bin: {_version}, {_localeAbb}, {_downloadBin.error}");
		}
		Finish();
		LocalizationFileUpdate.StopDownload();
	}

	public void Stop()
	{
		if (!base.isDone)
		{
			if (_downloadTxt != null && _downloadTxt.isDone)
			{
				_downloadTxt.Cancel();
			}
			if (_downloadBin != null && _downloadBin.isDone)
			{
				_downloadBin.Cancel();
			}
		}
	}
}
