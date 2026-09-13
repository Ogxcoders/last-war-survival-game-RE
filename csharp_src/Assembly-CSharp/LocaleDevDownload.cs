using GameFramework;
using VEngine;

public class LocaleDevDownload : Operation
{
	private string _localeAbb;

	private Download _downloadBin;

	public LocaleDevDownload(string localeAbb)
	{
		_localeAbb = localeAbb;
	}

	public override void Start()
	{
		base.Start();
		DownloadInfo downloadInfo = new DownloadInfo();
		downloadInfo.crc = 0u;
		downloadInfo.savePath = ClientConfig.GetDevLocaleBinDownloadDataPath(_localeAbb);
		downloadInfo.size = 0uL;
		downloadInfo.url = "http://lw-local-s188.rivergame.net/hotupdate//locale_debug/" + _localeAbb + ".bin";
		_downloadBin = Download.DownloadAsync(downloadInfo);
		Log.Info("LocaleDevDownload::Start Download " + _localeAbb + ", " + downloadInfo.url);
	}

	protected override void Update()
	{
		if (_downloadBin != null && _downloadBin.isDone)
		{
			if (string.IsNullOrEmpty(_downloadBin.error))
			{
				Log.Info("LocaleDevDownload Download Succeed " + _localeAbb);
				GameEntry.Localization.ReloadDictionaryByUpdateDevLocale(_localeAbb);
			}
			else
			{
				Log.Error("LocaleDevDownload Download Failed Bin: " + _localeAbb + ", " + _downloadBin.error);
			}
			Finish();
			LocalizationFileUpdate.StopDownload();
		}
	}

	public void Stop()
	{
		if (!base.isDone && _downloadBin != null && _downloadBin.isDone)
		{
			_downloadBin.Cancel();
		}
	}
}
