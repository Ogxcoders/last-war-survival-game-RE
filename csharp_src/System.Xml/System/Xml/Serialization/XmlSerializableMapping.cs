using System.Reflection;
using System.Xml.Schema;

namespace System.Xml.Serialization;

internal class XmlSerializableMapping : XmlTypeMapping
{
	private XmlSchema _schema;

	private XmlSchemaComplexType _schemaType;

	private XmlQualifiedName _schemaTypeName;

	internal XmlSchema Schema => _schema;

	internal XmlSchemaType SchemaType => _schemaType;

	internal XmlQualifiedName SchemaTypeName => _schemaTypeName;

	internal XmlSerializableMapping(XmlRootAttribute root, string elementName, string ns, TypeData typeData, string xmlType, string xmlTypeNamespace)
		: base(elementName, ns, typeData, xmlType, xmlTypeNamespace)
	{
		XmlSchemaProviderAttribute xmlSchemaProviderAttribute = (XmlSchemaProviderAttribute)Attribute.GetCustomAttribute(typeData.Type, typeof(XmlSchemaProviderAttribute));
		if (xmlSchemaProviderAttribute != null)
		{
			_schemaTypeName = XmlQualifiedName.Empty;
			if (xmlSchemaProviderAttribute.IsAny)
			{
				base.IsAny = true;
				return;
			}
			string methodName = xmlSchemaProviderAttribute.MethodName;
			MethodInfo method = typeData.Type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			if (method == null)
			{
				throw new InvalidOperationException($"Type '{typeData.Type}' must implement public static method '{methodName}'");
			}
			if (!typeof(XmlQualifiedName).IsAssignableFrom(method.ReturnType) && !typeof(XmlSchemaComplexType).IsAssignableFrom(method.ReturnType))
			{
				throw new InvalidOperationException($"Method '{methodName}' indicated by XmlSchemaProviderAttribute must have its return type as XmlQualifiedName");
			}
			XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
			object obj = method.Invoke(null, new object[1] { xmlSchemaSet });
			if (obj == null)
			{
				return;
			}
			if (obj is XmlSchemaComplexType)
			{
				_schemaType = (XmlSchemaComplexType)obj;
				if (!_schemaType.QualifiedName.IsEmpty)
				{
					_schemaTypeName = _schemaType.QualifiedName;
				}
				else
				{
					_schemaTypeName = new XmlQualifiedName(xmlType, xmlTypeNamespace);
				}
			}
			else
			{
				if (!(obj is XmlQualifiedName))
				{
					throw new InvalidOperationException($"Method {typeData.Type.Name}.{methodName}() specified by XmlSchemaProviderAttribute has invalid signature: return type must be compatible with System.Xml.XmlQualifiedName.");
				}
				_schemaTypeName = (XmlQualifiedName)obj;
			}
			UpdateRoot(new XmlQualifiedName((root != null) ? root.ElementName : _schemaTypeName.Name, (root != null) ? root.Namespace : (base.Namespace ?? _schemaTypeName.Namespace)));
			base.XmlTypeNamespace = _schemaTypeName.Namespace;
			base.XmlType = _schemaTypeName.Name;
			if (!_schemaTypeName.IsEmpty && xmlSchemaSet.Count > 0)
			{
				XmlSchema[] array = new XmlSchema[xmlSchemaSet.Count];
				xmlSchemaSet.CopyTo(array, 0);
				_schema = array[0];
			}
		}
		else
		{
			IXmlSerializable xmlSerializable = (IXmlSerializable)Activator.CreateInstance(typeData.Type, nonPublic: true);
			try
			{
				_schema = xmlSerializable.GetSchema();
			}
			catch (Exception)
			{
			}
			if (_schema != null && (_schema.Id == null || _schema.Id.Length == 0))
			{
				throw new InvalidOperationException("Schema Id is missing. The schema returned from " + typeData.Type.FullName + ".GetSchema() must have an Id.");
			}
		}
	}
}
