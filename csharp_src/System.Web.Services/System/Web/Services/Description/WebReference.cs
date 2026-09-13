using System.Collections.Specialized;
using System.Web.Services.Discovery;

namespace System.Web.Services.Description;

public sealed class WebReference
{
	private DiscoveryClientDocumentCollection _documents;

	private string _protocolName;

	private string _appSettingUrlKey;

	private string _appSettingBaseUrl;

	private StringCollection _validationWarnings;

	public string AppSettingBaseUrl => _appSettingBaseUrl;

	public string AppSettingUrlKey => _appSettingUrlKey;

	public DiscoveryClientDocumentCollection Documents => _documents;

	public string ProtocolName
	{
		get
		{
			return _protocolName;
		}
		set
		{
			_protocolName = value;
		}
	}

	public StringCollection ValidationWarnings
	{
		get
		{
			if (_validationWarnings == null)
			{
				_validationWarnings = new StringCollection();
			}
			return _validationWarnings;
		}
	}

	internal void SetValidationWarnings(StringCollection col)
	{
		_validationWarnings = col;
	}
}
