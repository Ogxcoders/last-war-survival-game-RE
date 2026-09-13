using System.Collections;
using System.Web.Services.Description;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

internal class SoapTypeStubInfo : TypeStubInfo
{
	private Hashtable methods_byaction = new Hashtable();

	internal SoapParameterStyle ParameterStyle;

	internal SoapExtensionRuntimeConfig[][] SoapExtensions;

	internal SoapBindingStyle SoapBindingStyle;

	internal XmlReflectionImporter xmlImporter;

	internal SoapReflectionImporter soapImporter;

	internal SoapServiceRoutingStyle RoutingStyle => base.LogicalType.RoutingStyle;

	public override XmlReflectionImporter XmlImporter => xmlImporter;

	public override SoapReflectionImporter SoapImporter => soapImporter;

	public override string ProtocolName => "Soap";

	public SoapTypeStubInfo(LogicalTypeInfo logicalTypeInfo)
		: base(logicalTypeInfo)
	{
		xmlImporter = new XmlReflectionImporter();
		soapImporter = new SoapReflectionImporter();
		if (typeof(SoapHttpClientProtocol).IsAssignableFrom(base.Type))
		{
			if (base.Bindings.Count == 0 || ((BindingInfo)base.Bindings[0]).WebServiceBindingAttribute == null)
			{
				throw new InvalidOperationException("WebServiceBindingAttribute is required on proxy class '" + base.Type?.ToString() + "'.");
			}
			if (base.Bindings.Count > 1)
			{
				throw new InvalidOperationException("Only one WebServiceBinding attribute may be specified on type '" + base.Type?.ToString() + "'.");
			}
		}
		object[] customAttributes = base.Type.GetCustomAttributes(typeof(SoapDocumentServiceAttribute), inherit: false);
		if (customAttributes.Length == 1)
		{
			SoapDocumentServiceAttribute soapDocumentServiceAttribute = (SoapDocumentServiceAttribute)customAttributes[0];
			ParameterStyle = soapDocumentServiceAttribute.ParameterStyle;
			SoapBindingStyle = SoapBindingStyle.Document;
		}
		else
		{
			customAttributes = base.Type.GetCustomAttributes(typeof(SoapRpcServiceAttribute), inherit: false);
			if (customAttributes.Length == 1)
			{
				ParameterStyle = SoapParameterStyle.Wrapped;
				SoapBindingStyle = SoapBindingStyle.Rpc;
			}
			else
			{
				ParameterStyle = SoapParameterStyle.Wrapped;
				SoapBindingStyle = SoapBindingStyle.Document;
			}
		}
		if (ParameterStyle == SoapParameterStyle.Default)
		{
			ParameterStyle = SoapParameterStyle.Wrapped;
		}
		xmlImporter.IncludeTypes(base.Type);
		soapImporter.IncludeTypes(base.Type);
		SoapExtensions = new SoapExtensionRuntimeConfig[2][];
	}

	protected override MethodStubInfo CreateMethodStubInfo(TypeStubInfo parent, LogicalMethodInfo lmi, bool isClientProxy)
	{
		SoapMethodStubInfo soapMethodStubInfo = null;
		object[] customAttributes = lmi.GetCustomAttributes(typeof(SoapDocumentMethodAttribute));
		if (customAttributes.Length == 0)
		{
			customAttributes = lmi.GetCustomAttributes(typeof(SoapRpcMethodAttribute));
		}
		if (customAttributes.Length == 0 && isClientProxy)
		{
			return null;
		}
		soapMethodStubInfo = ((customAttributes.Length != 0) ? new SoapMethodStubInfo(parent, lmi, customAttributes[0], xmlImporter, soapImporter) : new SoapMethodStubInfo(parent, lmi, null, xmlImporter, soapImporter));
		methods_byaction[soapMethodStubInfo.Action] = soapMethodStubInfo;
		return soapMethodStubInfo;
	}

	public SoapMethodStubInfo GetMethodForSoapAction(string name)
	{
		return (SoapMethodStubInfo)methods_byaction[name.Trim('"', ' ')];
	}
}
