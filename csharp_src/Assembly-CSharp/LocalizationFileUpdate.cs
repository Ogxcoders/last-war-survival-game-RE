public class LocalizationFileUpdate
{
	public static LocaleDownload _localeDownload;

	public static LocaleDevDownload _localeDevDownload;

	public static void StartDownloadBG()
	{
		if (!ClientConfig.SKIP_UPDATE_TABLE_AND_LOCALIZATION && ClientConfig.REMOTE_LOCALE_VERSION > ClientConfig.LOCALE_VERSION && ClientConfig.REMOTE_LOCALE_SUPPORT.Contains(ClientConfig.LOCALE_ABB))
		{
			StartDownload(ClientConfig.REMOTE_LOCALE_VERSION, ClientConfig.LOCALE_ABB);
		}
	}

	public static void StartDownload(int version, string localeAbb)
	{
		_localeDownload = new LocaleDownload(version, localeAbb);
		_localeDownload.Start();
	}

	public static void StopDownload()
	{
		_localeDownload?.Stop();
		_localeDownload = null;
		_localeDevDownload?.Stop();
		_localeDevDownload = null;
	}
}
