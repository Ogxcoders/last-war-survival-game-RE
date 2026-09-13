using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

public class XmlReturnReader : MimeReturnReader
{
	private XmlSerializer serializer;

	public override object GetInitializer(LogicalMethodInfo methodInfo)
	{
		LogicalTypeInfo logicalTypeInfo = TypeStubManager.GetLogicalTypeInfo(methodInfo.DeclaringType);
		object[] customAttributes = methodInfo.ReturnTypeCustomAttributeProvider.GetCustomAttributes(typeof(XmlRootAttribute), inherit: true);
		XmlRootAttribute root = ((customAttributes.Length != 0) ? (customAttributes[0] as XmlRootAttribute) : null);
		return new XmlSerializer(methodInfo.ReturnType, null, null, root, logicalTypeInfo.GetWebServiceLiteralNamespace(logicalTypeInfo.WebServiceNamespace));
	}

	public override object[] GetInitializers(LogicalMethodInfo[] methodInfos)
	{
		XmlReflectionImporter xmlReflectionImporter = new XmlReflectionImporter();
		XmlMapping[] array = new XmlMapping[methodInfos.Length];
		for (int i = 0; i < array.Length; i++)
		{
			LogicalMethodInfo logicalMethodInfo = methodInfos[i];
			if (logicalMethodInfo.IsVoid)
			{
				array[i] = null;
				continue;
			}
			LogicalTypeInfo logicalTypeInfo = TypeStubManager.GetLogicalTypeInfo(logicalMethodInfo.DeclaringType);
			object[] customAttributes = methodInfos[i].ReturnTypeCustomAttributeProvider.GetCustomAttributes(typeof(XmlRootAttribute), inherit: true);
			XmlRootAttribute root = ((customAttributes.Length != 0) ? (customAttributes[0] as XmlRootAttribute) : null);
			array[i] = xmlReflectionImporter.ImportTypeMapping(methodInfos[i].ReturnType, root, logicalTypeInfo.GetWebServiceLiteralNamespace(logicalTypeInfo.WebServiceNamespace));
		}
		return XmlSerializer.FromMappings(array);
	}

	public override void Initialize(object o)
	{
		serializer = (XmlSerializer)o;
	}

	public override object Read(WebResponse response, Stream responseStream)
	{
		object result = null;
		if (serializer != null)
		{
			if (response.ContentType.IndexOf("text/xml") == -1)
			{
				throw new InvalidOperationException("Result was not XML");
			}
			result = serializer.Deserialize(responseStream);
		}
		responseStream.Close();
		return result;
	}
}
