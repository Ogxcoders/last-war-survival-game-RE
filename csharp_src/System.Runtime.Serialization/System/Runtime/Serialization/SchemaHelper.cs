using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace System.Runtime.Serialization;

internal static class SchemaHelper
{
	internal static bool NamespacesEqual(string ns1, string ns2)
	{
		if (ns1 == null || ns1.Length == 0)
		{
			if (ns2 != null)
			{
				return ns2.Length == 0;
			}
			return true;
		}
		return ns1 == ns2;
	}

	internal static XmlSchemaType GetSchemaType(XmlSchemaSet schemas, XmlQualifiedName typeQName, out XmlSchema outSchema)
	{
		outSchema = null;
		ICollection collection = schemas.Schemas();
		string ns = typeQName.Namespace;
		foreach (XmlSchema item in collection)
		{
			if (!NamespacesEqual(ns, item.TargetNamespace))
			{
				continue;
			}
			outSchema = item;
			foreach (XmlSchemaObject item2 in item.Items)
			{
				if (item2 is XmlSchemaType xmlSchemaType && xmlSchemaType.Name == typeQName.Name)
				{
					return xmlSchemaType;
				}
			}
		}
		return null;
	}

	internal static XmlSchemaType GetSchemaType(Dictionary<XmlQualifiedName, SchemaObjectInfo> schemaInfo, XmlQualifiedName typeName)
	{
		if (schemaInfo.TryGetValue(typeName, out var value))
		{
			return value.type;
		}
		return null;
	}

	internal static XmlSchema GetSchemaWithType(Dictionary<XmlQualifiedName, SchemaObjectInfo> schemaInfo, XmlSchemaSet schemas, XmlQualifiedName typeName)
	{
		if (schemaInfo.TryGetValue(typeName, out var value) && value.schema != null)
		{
			return value.schema;
		}
		ICollection collection = schemas.Schemas();
		string ns = typeName.Namespace;
		foreach (XmlSchema item in collection)
		{
			if (NamespacesEqual(ns, item.TargetNamespace))
			{
				return item;
			}
		}
		return null;
	}

	internal static XmlSchemaElement GetSchemaElement(XmlSchemaSet schemas, XmlQualifiedName elementQName, out XmlSchema outSchema)
	{
		outSchema = null;
		ICollection collection = schemas.Schemas();
		string ns = elementQName.Namespace;
		foreach (XmlSchema item in collection)
		{
			if (!NamespacesEqual(ns, item.TargetNamespace))
			{
				continue;
			}
			outSchema = item;
			foreach (XmlSchemaObject item2 in item.Items)
			{
				if (item2 is XmlSchemaElement xmlSchemaElement && xmlSchemaElement.Name == elementQName.Name)
				{
					return xmlSchemaElement;
				}
			}
		}
		return null;
	}

	internal static XmlSchemaElement GetSchemaElement(Dictionary<XmlQualifiedName, SchemaObjectInfo> schemaInfo, XmlQualifiedName elementName)
	{
		if (schemaInfo.TryGetValue(elementName, out var value))
		{
			return value.element;
		}
		return null;
	}

	internal static XmlSchema GetSchema(string ns, XmlSchemaSet schemas)
	{
		if (ns == null)
		{
			ns = string.Empty;
		}
		foreach (XmlSchema item in schemas.Schemas())
		{
			if ((item.TargetNamespace == null && ns.Length == 0) || ns.Equals(item.TargetNamespace))
			{
				return item;
			}
		}
		return CreateSchema(ns, schemas);
	}

	private static XmlSchema CreateSchema(string ns, XmlSchemaSet schemas)
	{
		XmlSchema xmlSchema = new XmlSchema();
		xmlSchema.ElementFormDefault = XmlSchemaForm.Qualified;
		if (ns.Length > 0)
		{
			xmlSchema.TargetNamespace = ns;
			xmlSchema.Namespaces.Add("tns", ns);
		}
		schemas.Add(xmlSchema);
		return xmlSchema;
	}

	internal static void AddElementForm(XmlSchemaElement element, XmlSchema schema)
	{
		if (schema.ElementFormDefault != XmlSchemaForm.Qualified)
		{
			element.Form = XmlSchemaForm.Qualified;
		}
	}

	internal static void AddSchemaImport(string ns, XmlSchema schema)
	{
		if (NamespacesEqual(ns, schema.TargetNamespace) || NamespacesEqual(ns, "http://www.w3.org/2001/XMLSchema") || NamespacesEqual(ns, "http://www.w3.org/2001/XMLSchema-instance"))
		{
			return;
		}
		foreach (XmlSchemaObject include in schema.Includes)
		{
			if (include is XmlSchemaImport && NamespacesEqual(ns, ((XmlSchemaImport)include).Namespace))
			{
				return;
			}
		}
		XmlSchemaImport xmlSchemaImport = new XmlSchemaImport();
		if (ns != null && ns.Length > 0)
		{
			xmlSchemaImport.Namespace = ns;
		}
		schema.Includes.Add(xmlSchemaImport);
	}

	internal static XmlSchema GetSchemaWithGlobalElementDeclaration(XmlSchemaElement element, XmlSchemaSet schemas)
	{
		foreach (XmlSchema item in schemas.Schemas())
		{
			foreach (XmlSchemaObject item2 in item.Items)
			{
				if (item2 is XmlSchemaElement xmlSchemaElement && xmlSchemaElement == element)
				{
					return item;
				}
			}
		}
		return null;
	}

	internal static XmlQualifiedName GetGlobalElementDeclaration(XmlSchemaSet schemas, XmlQualifiedName typeQName, out bool isNullable)
	{
		ICollection collection = schemas.Schemas();
		if (typeQName.Namespace == null)
		{
			_ = string.Empty;
		}
		isNullable = false;
		foreach (XmlSchema item in collection)
		{
			foreach (XmlSchemaObject item2 in item.Items)
			{
				if (item2 is XmlSchemaElement xmlSchemaElement && xmlSchemaElement.SchemaTypeName.Equals(typeQName))
				{
					isNullable = xmlSchemaElement.IsNillable;
					return new XmlQualifiedName(xmlSchemaElement.Name, item.TargetNamespace);
				}
			}
		}
		return null;
	}
}
