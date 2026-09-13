using System.Collections;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

public class ServiceDescriptionImporter
{
	private string protocolName;

	private XmlSchemas schemas;

	private ServiceDescriptionCollection serviceDescriptions;

	private ServiceDescriptionImportStyle style;

	private ArrayList importInfo = new ArrayList();

	public string ProtocolName
	{
		get
		{
			return protocolName;
		}
		set
		{
			protocolName = value;
		}
	}

	public XmlSchemas Schemas => schemas;

	public ServiceDescriptionCollection ServiceDescriptions => serviceDescriptions;

	public ServiceDescriptionImportStyle Style
	{
		get
		{
			return style;
		}
		set
		{
			style = value;
		}
	}

	public ServiceDescriptionImporter()
	{
		protocolName = string.Empty;
		schemas = new XmlSchemas();
		serviceDescriptions = new ServiceDescriptionCollection();
		serviceDescriptions.SetImporter(this);
		style = ServiceDescriptionImportStyle.Client;
	}

	public void AddServiceDescription(ServiceDescription serviceDescription, string appSettingUrlKey, string appSettingBaseUrl)
	{
		if (appSettingUrlKey != null && appSettingUrlKey == string.Empty && style == ServiceDescriptionImportStyle.Server)
		{
			throw new InvalidOperationException("Cannot set appSettingUrlKey if Style is Server");
		}
		OnServiceDescriptionAdded(serviceDescription, appSettingUrlKey, appSettingBaseUrl);
	}

	internal void OnServiceDescriptionAdded(ServiceDescription serviceDescription, string appSettingUrlKey, string appSettingBaseUrl)
	{
		serviceDescriptions.Add(serviceDescription);
		ImportInfo value = new ImportInfo(serviceDescription, appSettingUrlKey, appSettingBaseUrl);
		importInfo.Add(value);
		if (serviceDescription.Types != null)
		{
			schemas.Add(serviceDescription.Types.Schemas);
		}
	}
}
