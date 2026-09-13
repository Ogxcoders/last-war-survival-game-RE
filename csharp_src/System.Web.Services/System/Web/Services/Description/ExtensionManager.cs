using System.Collections;
using System.Reflection;
using System.Web.Services.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

internal abstract class ExtensionManager
{
	private static Hashtable extensionsByName;

	private static Hashtable extensionsByType;

	private static ArrayList maps;

	private static ArrayList extensions;

	static ExtensionManager()
	{
		maps = new ArrayList();
		extensions = new ArrayList();
		extensionsByName = new Hashtable();
		extensionsByType = new Hashtable();
		RegisterExtensionType(typeof(HttpAddressBinding));
		RegisterExtensionType(typeof(HttpBinding));
		RegisterExtensionType(typeof(HttpOperationBinding));
		RegisterExtensionType(typeof(HttpUrlEncodedBinding));
		RegisterExtensionType(typeof(HttpUrlReplacementBinding));
		RegisterExtensionType(typeof(MimeContentBinding));
		RegisterExtensionType(typeof(MimeMultipartRelatedBinding));
		RegisterExtensionType(typeof(MimeTextBinding));
		RegisterExtensionType(typeof(MimeXmlBinding));
		RegisterExtensionType(typeof(SoapAddressBinding));
		RegisterExtensionType(typeof(SoapBinding));
		RegisterExtensionType(typeof(SoapBodyBinding));
		RegisterExtensionType(typeof(SoapFaultBinding));
		RegisterExtensionType(typeof(SoapHeaderBinding));
		RegisterExtensionType(typeof(SoapOperationBinding));
		RegisterExtensionType(typeof(Soap12AddressBinding));
		RegisterExtensionType(typeof(Soap12Binding));
		RegisterExtensionType(typeof(Soap12BodyBinding));
		RegisterExtensionType(typeof(Soap12FaultBinding));
		RegisterExtensionType(typeof(Soap12HeaderBinding));
		RegisterExtensionType(typeof(Soap12OperationBinding));
		CreateExtensionSerializers();
	}

	private static void RegisterExtensionType(Type type)
	{
		ExtensionInfo extensionInfo = new ExtensionInfo();
		extensionInfo.Type = type;
		object[] customAttributes = type.GetCustomAttributes(typeof(XmlFormatExtensionPrefixAttribute), inherit: true);
		object[] array = customAttributes;
		for (int i = 0; i < array.Length; i++)
		{
			XmlFormatExtensionPrefixAttribute xmlFormatExtensionPrefixAttribute = (XmlFormatExtensionPrefixAttribute)array[i];
			extensionInfo.NamespaceDeclarations.Add(new XmlQualifiedName(xmlFormatExtensionPrefixAttribute.Prefix, xmlFormatExtensionPrefixAttribute.Namespace));
		}
		customAttributes = type.GetCustomAttributes(typeof(XmlFormatExtensionAttribute), inherit: true);
		if (customAttributes.Length != 0)
		{
			XmlFormatExtensionAttribute xmlFormatExtensionAttribute = (XmlFormatExtensionAttribute)customAttributes[0];
			extensionInfo.ElementName = xmlFormatExtensionAttribute.ElementName;
			if (xmlFormatExtensionAttribute.Namespace != null)
			{
				extensionInfo.Namespace = xmlFormatExtensionAttribute.Namespace;
			}
		}
		XmlRootAttribute xmlRootAttribute = new XmlRootAttribute();
		xmlRootAttribute.ElementName = extensionInfo.ElementName;
		if (extensionInfo.Namespace != null)
		{
			xmlRootAttribute.Namespace = extensionInfo.Namespace;
		}
		XmlTypeMapping value = new XmlReflectionImporter().ImportTypeMapping(type, xmlRootAttribute);
		if (extensionInfo.ElementName == null)
		{
			throw new InvalidOperationException("XmlFormatExtensionAttribute must be applied to type " + type);
		}
		extensionsByName.Add(extensionInfo.Namespace + " " + extensionInfo.ElementName, extensionInfo);
		extensionsByType.Add(type, extensionInfo);
		maps.Add(value);
		extensions.Add(extensionInfo);
	}

	private static void CreateExtensionSerializers()
	{
		XmlSerializer[] array = XmlSerializer.FromMappings((XmlMapping[])maps.ToArray(typeof(XmlMapping)));
		for (int i = 0; i < array.Length; i++)
		{
			((ExtensionInfo)extensions[i]).Serializer = array[i];
		}
		maps = null;
		extensions = null;
	}

	public static ExtensionInfo GetFormatExtensionInfo(string elementName, string namesp)
	{
		return (ExtensionInfo)extensionsByName[namesp + " " + elementName];
	}

	public static ExtensionInfo GetFormatExtensionInfo(Type extType)
	{
		return (ExtensionInfo)extensionsByType[extType];
	}

	public static ICollection GetFormatExtensions()
	{
		return extensionsByName.Values;
	}

	public static ServiceDescriptionFormatExtensionCollection GetExtensionPoint(object ob)
	{
		Type type = ob.GetType();
		object[] customAttributes = type.GetCustomAttributes(typeof(XmlFormatExtensionPointAttribute), inherit: true);
		if (customAttributes.Length == 0)
		{
			return null;
		}
		XmlFormatExtensionPointAttribute xmlFormatExtensionPointAttribute = (XmlFormatExtensionPointAttribute)customAttributes[0];
		PropertyInfo property = type.GetProperty(xmlFormatExtensionPointAttribute.MemberName);
		if (property != null)
		{
			return property.GetValue(ob, null) as ServiceDescriptionFormatExtensionCollection;
		}
		FieldInfo field = type.GetField(xmlFormatExtensionPointAttribute.MemberName);
		if (field != null)
		{
			return field.GetValue(ob) as ServiceDescriptionFormatExtensionCollection;
		}
		throw new InvalidOperationException("XmlFormatExtensionPointAttribute: Member " + xmlFormatExtensionPointAttribute.MemberName + " not found");
	}

	public static ArrayList BuildExtensionImporters()
	{
		return new ArrayList(0);
	}

	public static ArrayList BuildExtensionReflectors()
	{
		return new ArrayList(0);
	}
}
