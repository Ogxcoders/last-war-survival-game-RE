namespace System.Web.Services.Description;

internal class ImportInfo
{
	private string _appSettingUrlKey;

	private string _appSettingBaseUrl;

	private ServiceDescription _serviceDescription;

	public WebReference _reference;

	public WebReference Reference => _reference;

	public ServiceDescription ServiceDescription => _serviceDescription;

	public string AppSettingUrlKey
	{
		get
		{
			if (_reference != null)
			{
				return _reference.AppSettingUrlKey;
			}
			return _appSettingUrlKey;
		}
		set
		{
			_appSettingUrlKey = value;
		}
	}

	public string AppSettingBaseUrl
	{
		get
		{
			if (_reference != null)
			{
				return _reference.AppSettingBaseUrl;
			}
			return _appSettingBaseUrl;
		}
		set
		{
			_appSettingBaseUrl = value;
		}
	}

	public ImportInfo(ServiceDescription serviceDescription, string appSettingUrlKey, string appSettingBaseUrl)
	{
		_serviceDescription = serviceDescription;
		_appSettingUrlKey = appSettingUrlKey;
		_appSettingBaseUrl = appSettingBaseUrl;
	}

	public ImportInfo(ServiceDescription serviceDescription, WebReference reference)
	{
		_reference = reference;
		_serviceDescription = serviceDescription;
	}
}
