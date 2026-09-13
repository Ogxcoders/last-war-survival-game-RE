using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Diagnostics;
using System.Runtime.Serialization.Diagnostics;
using System.Security;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Runtime.Serialization;

internal class SchemaExporter
{
	private XmlSchemaSet schemas;

	private XmlDocument xmlDoc;

	private DataContractSet dataContractSet;

	[SecurityCritical]
	private static XmlQualifiedName anytypeQualifiedName;

	[SecurityCritical]
	private static XmlQualifiedName stringQualifiedName;

	[SecurityCritical]
	private static XmlQualifiedName defaultEnumBaseTypeName;

	[SecurityCritical]
	private static XmlQualifiedName enumerationValueAnnotationName;

	[SecurityCritical]
	private static XmlQualifiedName surrogateDataAnnotationName;

	[SecurityCritical]
	private static XmlQualifiedName defaultValueAnnotation;

	[SecurityCritical]
	private static XmlQualifiedName actualTypeAnnotationName;

	[SecurityCritical]
	private static XmlQualifiedName isDictionaryAnnotationName;

	[SecurityCritical]
	private static XmlQualifiedName isValueTypeName;

	private XmlSchemaSet Schemas => schemas;

	private XmlDocument XmlDoc
	{
		get
		{
			if (xmlDoc == null)
			{
				xmlDoc = new XmlDocument();
			}
			return xmlDoc;
		}
	}

	internal static XmlSchemaSequence ISerializableSequence => new XmlSchemaSequence
	{
		Items = { (XmlSchemaObject)ISerializableWildcardElement }
	};

	internal static XmlSchemaAny ISerializableWildcardElement => new XmlSchemaAny
	{
		MinOccurs = 0m,
		MaxOccursString = "unbounded",
		Namespace = "##local",
		ProcessContents = XmlSchemaContentProcessing.Skip
	};

	internal static XmlQualifiedName AnytypeQualifiedName
	{
		[SecuritySafeCritical]
		get
		{
			if (anytypeQualifiedName == null)
			{
				anytypeQualifiedName = new XmlQualifiedName("anyType", "http://www.w3.org/2001/XMLSchema");
			}
			return anytypeQualifiedName;
		}
	}

	internal static XmlQualifiedName StringQualifiedName
	{
		[SecuritySafeCritical]
		get
		{
			if (stringQualifiedName == null)
			{
				stringQualifiedName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
			}
			return stringQualifiedName;
		}
	}

	internal static XmlQualifiedName DefaultEnumBaseTypeName
	{
		[SecuritySafeCritical]
		get
		{
			if (defaultEnumBaseTypeName == null)
			{
				defaultEnumBaseTypeName = new XmlQualifiedName("int", "http://www.w3.org/2001/XMLSchema");
			}
			return defaultEnumBaseTypeName;
		}
	}

	internal static XmlQualifiedName EnumerationValueAnnotationName
	{
		[SecuritySafeCritical]
		get
		{
			if (enumerationValueAnnotationName == null)
			{
				enumerationValueAnnotationName = new XmlQualifiedName("EnumerationValue", "http://schemas.microsoft.com/2003/10/Serialization/");
			}
			return enumerationValueAnnotationName;
		}
	}

	internal static XmlQualifiedName SurrogateDataAnnotationName
	{
		[SecuritySafeCritical]
		get
		{
			if (surrogateDataAnnotationName == null)
			{
				surrogateDataAnnotationName = new XmlQualifiedName("Surrogate", "http://schemas.microsoft.com/2003/10/Serialization/");
			}
			return surrogateDataAnnotationName;
		}
	}

	internal static XmlQualifiedName DefaultValueAnnotation
	{
		[SecuritySafeCritical]
		get
		{
			if (defaultValueAnnotation == null)
			{
				defaultValueAnnotation = new XmlQualifiedName("DefaultValue", "http://schemas.microsoft.com/2003/10/Serialization/");
			}
			return defaultValueAnnotation;
		}
	}

	internal static XmlQualifiedName ActualTypeAnnotationName
	{
		[SecuritySafeCritical]
		get
		{
			if (actualTypeAnnotationName == null)
			{
				actualTypeAnnotationName = new XmlQualifiedName("ActualType", "http://schemas.microsoft.com/2003/10/Serialization/");
			}
			return actualTypeAnnotationName;
		}
	}

	internal static XmlQualifiedName IsDictionaryAnnotationName
	{
		[SecuritySafeCritical]
		get
		{
			if (isDictionaryAnnotationName == null)
			{
				isDictionaryAnnotationName = new XmlQualifiedName("IsDictionary", "http://schemas.microsoft.com/2003/10/Serialization/");
			}
			return isDictionaryAnnotationName;
		}
	}

	internal static XmlQualifiedName IsValueTypeName
	{
		[SecuritySafeCritical]
		get
		{
			if (isValueTypeName == null)
			{
				isValueTypeName = new XmlQualifiedName("IsValueType", "http://schemas.microsoft.com/2003/10/Serialization/");
			}
			return isValueTypeName;
		}
	}

	internal static XmlSchemaAttribute ISerializableFactoryTypeAttribute => new XmlSchemaAttribute
	{
		RefName = new XmlQualifiedName("FactoryType", "http://schemas.microsoft.com/2003/10/Serialization/")
	};

	internal static XmlSchemaAttribute RefAttribute => new XmlSchemaAttribute
	{
		RefName = Globals.RefQualifiedName
	};

	internal static XmlSchemaAttribute IdAttribute => new XmlSchemaAttribute
	{
		RefName = Globals.IdQualifiedName
	};

	internal SchemaExporter(XmlSchemaSet schemas, DataContractSet dataContractSet)
	{
		this.schemas = schemas;
		this.dataContractSet = dataContractSet;
	}

	internal void Export()
	{
		try
		{
			ExportSerializationSchema();
			foreach (KeyValuePair<XmlQualifiedName, DataContract> item in dataContractSet)
			{
				DataContract value = item.Value;
				if (!dataContractSet.IsContractProcessed(value))
				{
					ExportDataContract(value);
					dataContractSet.SetContractProcessed(value);
				}
			}
		}
		finally
		{
			xmlDoc = null;
			dataContractSet = null;
		}
	}

	private void ExportSerializationSchema()
	{
		if (!Schemas.Contains("http://schemas.microsoft.com/2003/10/Serialization/"))
		{
			XmlSchema xmlSchema = XmlSchema.Read(new XmlTextReader(new StringReader("<?xml version='1.0' encoding='utf-8'?>\n<xs:schema elementFormDefault='qualified' attributeFormDefault='qualified' xmlns:tns='http://schemas.microsoft.com/2003/10/Serialization/' targetNamespace='http://schemas.microsoft.com/2003/10/Serialization/' xmlns:xs='http://www.w3.org/2001/XMLSchema'>\n  <xs:element name='anyType' nillable='true' type='xs:anyType' />\n  <xs:element name='anyURI' nillable='true' type='xs:anyURI' />\n  <xs:element name='base64Binary' nillable='true' type='xs:base64Binary' />\n  <xs:element name='boolean' nillable='true' type='xs:boolean' />\n  <xs:element name='byte' nillable='true' type='xs:byte' />\n  <xs:element name='dateTime' nillable='true' type='xs:dateTime' />\n  <xs:element name='decimal' nillable='true' type='xs:decimal' />\n  <xs:element name='double' nillable='true' type='xs:double' />\n  <xs:element name='float' nillable='true' type='xs:float' />\n  <xs:element name='int' nillable='true' type='xs:int' />\n  <xs:element name='long' nillable='true' type='xs:long' />\n  <xs:element name='QName' nillable='true' type='xs:QName' />\n  <xs:element name='short' nillable='true' type='xs:short' />\n  <xs:element name='string' nillable='true' type='xs:string' />\n  <xs:element name='unsignedByte' nillable='true' type='xs:unsignedByte' />\n  <xs:element name='unsignedInt' nillable='true' type='xs:unsignedInt' />\n  <xs:element name='unsignedLong' nillable='true' type='xs:unsignedLong' />\n  <xs:element name='unsignedShort' nillable='true' type='xs:unsignedShort' />\n  <xs:element name='char' nillable='true' type='tns:char' />\n  <xs:simpleType name='char'>\n    <xs:restriction base='xs:int'/>\n  </xs:simpleType>  \n  <xs:element name='duration' nillable='true' type='tns:duration' />\n  <xs:simpleType name='duration'>\n    <xs:restriction base='xs:duration'>\n      <xs:pattern value='\\-?P(\\d*D)?(T(\\d*H)?(\\d*M)?(\\d*(\\.\\d*)?S)?)?' />\n      <xs:minInclusive value='-P10675199DT2H48M5.4775808S' />\n      <xs:maxInclusive value='P10675199DT2H48M5.4775807S' />\n    </xs:restriction>\n  </xs:simpleType>\n  <xs:element name='guid' nillable='true' type='tns:guid' />\n  <xs:simpleType name='guid'>\n    <xs:restriction base='xs:string'>\n      <xs:pattern value='[\\da-fA-F]{8}-[\\da-fA-F]{4}-[\\da-fA-F]{4}-[\\da-fA-F]{4}-[\\da-fA-F]{12}' />\n    </xs:restriction>\n  </xs:simpleType>\n  <xs:attribute name='FactoryType' type='xs:QName' />\n  <xs:attribute name='Id' type='xs:ID' />\n  <xs:attribute name='Ref' type='xs:IDREF' />\n</xs:schema>\n"))
			{
				DtdProcessing = DtdProcessing.Prohibit
			}, null);
			if (xmlSchema == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Could not read serialization schema for '{0}' namespace.", "http://schemas.microsoft.com/2003/10/Serialization/")));
			}
			Schemas.Add(xmlSchema);
		}
	}

	private void ExportDataContract(DataContract dataContract)
	{
		if (dataContract.IsBuiltInDataContract)
		{
			return;
		}
		if (dataContract is XmlDataContract)
		{
			ExportXmlDataContract((XmlDataContract)dataContract);
			return;
		}
		XmlSchema schema = GetSchema(dataContract.StableName.Namespace);
		if (dataContract is ClassDataContract)
		{
			ClassDataContract classDataContract = (ClassDataContract)dataContract;
			if (classDataContract.IsISerializable)
			{
				ExportISerializableDataContract(classDataContract, schema);
			}
			else
			{
				ExportClassDataContract(classDataContract, schema);
			}
		}
		else if (dataContract is CollectionDataContract)
		{
			ExportCollectionDataContract((CollectionDataContract)dataContract, schema);
		}
		else if (dataContract is EnumDataContract)
		{
			ExportEnumDataContract((EnumDataContract)dataContract, schema);
		}
		ExportTopLevelElement(dataContract, schema);
		Schemas.Reprocess(schema);
	}

	private XmlSchemaElement ExportTopLevelElement(DataContract dataContract, XmlSchema schema)
	{
		if (schema == null || dataContract.StableName.Namespace != dataContract.TopLevelElementNamespace.Value)
		{
			schema = GetSchema(dataContract.TopLevelElementNamespace.Value);
		}
		XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
		xmlSchemaElement.Name = dataContract.TopLevelElementName.Value;
		SetElementType(xmlSchemaElement, dataContract, schema);
		xmlSchemaElement.IsNillable = true;
		schema.Items.Add(xmlSchemaElement);
		return xmlSchemaElement;
	}

	private void ExportClassDataContract(ClassDataContract classDataContract, XmlSchema schema)
	{
		XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
		xmlSchemaComplexType.Name = classDataContract.StableName.Name;
		schema.Items.Add(xmlSchemaComplexType);
		XmlElement xmlElement = null;
		if (classDataContract.UnderlyingType.IsGenericType)
		{
			xmlElement = ExportGenericInfo(classDataContract.UnderlyingType, "GenericType", "http://schemas.microsoft.com/2003/10/Serialization/");
		}
		XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
		for (int i = 0; i < classDataContract.Members.Count; i++)
		{
			DataMember dataMember = classDataContract.Members[i];
			XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
			xmlSchemaElement.Name = dataMember.Name;
			XmlElement xmlElement2 = null;
			DataContract memberTypeDataContract = dataContractSet.GetMemberTypeDataContract(dataMember);
			if (CheckIfMemberHasConflict(dataMember))
			{
				xmlSchemaElement.SchemaTypeName = AnytypeQualifiedName;
				xmlElement2 = ExportActualType(memberTypeDataContract.StableName);
				SchemaHelper.AddSchemaImport(memberTypeDataContract.StableName.Namespace, schema);
			}
			else
			{
				SetElementType(xmlSchemaElement, memberTypeDataContract, schema);
			}
			SchemaHelper.AddElementForm(xmlSchemaElement, schema);
			if (dataMember.IsNullable)
			{
				xmlSchemaElement.IsNillable = true;
			}
			if (!dataMember.IsRequired)
			{
				xmlSchemaElement.MinOccurs = 0m;
			}
			xmlSchemaElement.Annotation = GetSchemaAnnotation(xmlElement2, ExportSurrogateData(dataMember), ExportEmitDefaultValue(dataMember));
			xmlSchemaSequence.Items.Add(xmlSchemaElement);
		}
		XmlElement xmlElement3 = null;
		if (classDataContract.BaseContract != null)
		{
			XmlSchemaComplexContentExtension xmlSchemaComplexContentExtension = CreateTypeContent(xmlSchemaComplexType, classDataContract.BaseContract.StableName, schema);
			xmlSchemaComplexContentExtension.Particle = xmlSchemaSequence;
			if (classDataContract.IsReference && !classDataContract.BaseContract.IsReference)
			{
				AddReferenceAttributes(xmlSchemaComplexContentExtension.Attributes, schema);
			}
		}
		else
		{
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			if (classDataContract.IsValueType)
			{
				xmlElement3 = GetAnnotationMarkup(IsValueTypeName, XmlConvert.ToString(classDataContract.IsValueType), schema);
			}
			if (classDataContract.IsReference)
			{
				AddReferenceAttributes(xmlSchemaComplexType.Attributes, schema);
			}
		}
		xmlSchemaComplexType.Annotation = GetSchemaAnnotation(xmlElement, ExportSurrogateData(classDataContract), xmlElement3);
	}

	private void AddReferenceAttributes(XmlSchemaObjectCollection attributes, XmlSchema schema)
	{
		SchemaHelper.AddSchemaImport("http://schemas.microsoft.com/2003/10/Serialization/", schema);
		schema.Namespaces.Add("ser", "http://schemas.microsoft.com/2003/10/Serialization/");
		attributes.Add(IdAttribute);
		attributes.Add(RefAttribute);
	}

	private void SetElementType(XmlSchemaElement element, DataContract dataContract, XmlSchema schema)
	{
		if (dataContract is XmlDataContract { IsAnonymous: not false } xmlDataContract)
		{
			element.SchemaType = xmlDataContract.XsdType;
			return;
		}
		element.SchemaTypeName = dataContract.StableName;
		if (element.SchemaTypeName.Namespace.Equals("http://schemas.microsoft.com/2003/10/Serialization/"))
		{
			schema.Namespaces.Add("ser", "http://schemas.microsoft.com/2003/10/Serialization/");
		}
		SchemaHelper.AddSchemaImport(dataContract.StableName.Namespace, schema);
	}

	private bool CheckIfMemberHasConflict(DataMember dataMember)
	{
		if (dataMember.HasConflictingNameAndType)
		{
			return true;
		}
		for (DataMember conflictingMember = dataMember.ConflictingMember; conflictingMember != null; conflictingMember = conflictingMember.ConflictingMember)
		{
			if (conflictingMember.HasConflictingNameAndType)
			{
				return true;
			}
		}
		return false;
	}

	private XmlElement ExportEmitDefaultValue(DataMember dataMember)
	{
		if (dataMember.EmitDefaultValue)
		{
			return null;
		}
		XmlElement xmlElement = XmlDoc.CreateElement(DefaultValueAnnotation.Name, DefaultValueAnnotation.Namespace);
		XmlAttribute xmlAttribute = XmlDoc.CreateAttribute("EmitDefaultValue");
		xmlAttribute.Value = "false";
		xmlElement.Attributes.Append(xmlAttribute);
		return xmlElement;
	}

	private XmlElement ExportActualType(XmlQualifiedName typeName)
	{
		return ExportActualType(typeName, XmlDoc);
	}

	private static XmlElement ExportActualType(XmlQualifiedName typeName, XmlDocument xmlDoc)
	{
		XmlElement xmlElement = xmlDoc.CreateElement(ActualTypeAnnotationName.Name, ActualTypeAnnotationName.Namespace);
		XmlAttribute xmlAttribute = xmlDoc.CreateAttribute("Name");
		xmlAttribute.Value = typeName.Name;
		xmlElement.Attributes.Append(xmlAttribute);
		XmlAttribute xmlAttribute2 = xmlDoc.CreateAttribute("Namespace");
		xmlAttribute2.Value = typeName.Namespace;
		xmlElement.Attributes.Append(xmlAttribute2);
		return xmlElement;
	}

	private XmlElement ExportGenericInfo(Type clrType, string elementName, string elementNs)
	{
		int num = 0;
		Type itemType;
		while (CollectionDataContract.IsCollection(clrType, out itemType) && DataContract.GetBuiltInDataContract(clrType) == null && !CollectionDataContract.IsCollectionDataContract(clrType))
		{
			clrType = itemType;
			num++;
		}
		Type[] array = null;
		IList<int> list = null;
		if (clrType.IsGenericType)
		{
			array = clrType.GetGenericArguments();
			string text;
			if (clrType.DeclaringType == null)
			{
				text = clrType.Name;
			}
			else
			{
				int num2 = ((clrType.Namespace != null) ? clrType.Namespace.Length : 0);
				if (num2 > 0)
				{
					num2++;
				}
				text = DataContract.GetClrTypeFullName(clrType).Substring(num2).Replace('+', '.');
			}
			int num3 = text.IndexOf('[');
			if (num3 >= 0)
			{
				text = text.Substring(0, num3);
			}
			list = DataContract.GetDataContractNameForGenericName(text, null);
			clrType = clrType.GetGenericTypeDefinition();
		}
		XmlQualifiedName xmlQualifiedName = DataContract.GetStableName(clrType);
		if (num > 0)
		{
			string text2 = xmlQualifiedName.Name;
			for (int i = 0; i < num; i++)
			{
				text2 = "ArrayOf" + text2;
			}
			xmlQualifiedName = new XmlQualifiedName(text2, DataContract.GetCollectionNamespace(xmlQualifiedName.Namespace));
		}
		XmlElement xmlElement = XmlDoc.CreateElement(elementName, elementNs);
		XmlAttribute xmlAttribute = XmlDoc.CreateAttribute("Name");
		xmlAttribute.Value = ((array != null) ? XmlConvert.DecodeName(xmlQualifiedName.Name) : xmlQualifiedName.Name);
		xmlElement.Attributes.Append(xmlAttribute);
		XmlAttribute xmlAttribute2 = XmlDoc.CreateAttribute("Namespace");
		xmlAttribute2.Value = xmlQualifiedName.Namespace;
		xmlElement.Attributes.Append(xmlAttribute2);
		if (array != null)
		{
			int num4 = 0;
			int num5 = 0;
			foreach (int item in list)
			{
				int num6 = 0;
				while (num6 < item)
				{
					XmlElement xmlElement2 = ExportGenericInfo(array[num4], "GenericParameter", "http://schemas.microsoft.com/2003/10/Serialization/");
					if (num5 > 0)
					{
						XmlAttribute xmlAttribute3 = XmlDoc.CreateAttribute("NestedLevel");
						xmlAttribute3.Value = num5.ToString(CultureInfo.InvariantCulture);
						xmlElement2.Attributes.Append(xmlAttribute3);
					}
					xmlElement.AppendChild(xmlElement2);
					num6++;
					num4++;
				}
				num5++;
			}
			if (list[num5 - 1] == 0)
			{
				XmlAttribute xmlAttribute4 = XmlDoc.CreateAttribute("NestedLevel");
				xmlAttribute4.Value = list.Count.ToString(CultureInfo.InvariantCulture);
				xmlElement.Attributes.Append(xmlAttribute4);
			}
		}
		return xmlElement;
	}

	private XmlElement ExportSurrogateData(object key)
	{
		object surrogateData = dataContractSet.GetSurrogateData(key);
		if (surrogateData == null)
		{
			return null;
		}
		StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.OmitXmlDeclaration = true;
		XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);
		Collection<Type> collection = new Collection<Type>();
		DataContractSurrogateCaller.GetKnownCustomDataTypes(dataContractSet.DataContractSurrogate, collection);
		new DataContractSerializer(Globals.TypeOfObject, SurrogateDataAnnotationName.Name, SurrogateDataAnnotationName.Namespace, collection, int.MaxValue, ignoreExtensionDataObject: false, preserveObjectReferences: true, null).WriteObject(xmlWriter, surrogateData);
		xmlWriter.Flush();
		return (XmlElement)XmlDoc.ReadNode(XmlReader.Create(new StringReader(stringWriter.ToString())));
	}

	private void ExportCollectionDataContract(CollectionDataContract collectionDataContract, XmlSchema schema)
	{
		XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
		xmlSchemaComplexType.Name = collectionDataContract.StableName.Name;
		schema.Items.Add(xmlSchemaComplexType);
		XmlElement xmlElement = null;
		XmlElement xmlElement2 = null;
		if (collectionDataContract.UnderlyingType.IsGenericType && CollectionDataContract.IsCollectionDataContract(collectionDataContract.UnderlyingType))
		{
			xmlElement = ExportGenericInfo(collectionDataContract.UnderlyingType, "GenericType", "http://schemas.microsoft.com/2003/10/Serialization/");
		}
		if (collectionDataContract.IsDictionary)
		{
			xmlElement2 = ExportIsDictionary();
		}
		xmlSchemaComplexType.Annotation = GetSchemaAnnotation(xmlElement2, xmlElement, ExportSurrogateData(collectionDataContract));
		XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
		XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
		xmlSchemaElement.Name = collectionDataContract.ItemName;
		xmlSchemaElement.MinOccurs = 0m;
		xmlSchemaElement.MaxOccursString = "unbounded";
		if (collectionDataContract.IsDictionary)
		{
			ClassDataContract obj = collectionDataContract.ItemContract as ClassDataContract;
			XmlSchemaComplexType xmlSchemaComplexType2 = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence2 = new XmlSchemaSequence();
			foreach (DataMember member in obj.Members)
			{
				XmlSchemaElement xmlSchemaElement2 = new XmlSchemaElement();
				xmlSchemaElement2.Name = member.Name;
				SetElementType(xmlSchemaElement2, dataContractSet.GetMemberTypeDataContract(member), schema);
				SchemaHelper.AddElementForm(xmlSchemaElement2, schema);
				if (member.IsNullable)
				{
					xmlSchemaElement2.IsNillable = true;
				}
				xmlSchemaElement2.Annotation = GetSchemaAnnotation(ExportSurrogateData(member));
				xmlSchemaSequence2.Items.Add(xmlSchemaElement2);
			}
			xmlSchemaComplexType2.Particle = xmlSchemaSequence2;
			xmlSchemaElement.SchemaType = xmlSchemaComplexType2;
		}
		else
		{
			if (collectionDataContract.IsItemTypeNullable)
			{
				xmlSchemaElement.IsNillable = true;
			}
			DataContract itemTypeDataContract = dataContractSet.GetItemTypeDataContract(collectionDataContract);
			SetElementType(xmlSchemaElement, itemTypeDataContract, schema);
		}
		SchemaHelper.AddElementForm(xmlSchemaElement, schema);
		xmlSchemaSequence.Items.Add(xmlSchemaElement);
		xmlSchemaComplexType.Particle = xmlSchemaSequence;
		if (collectionDataContract.IsReference)
		{
			AddReferenceAttributes(xmlSchemaComplexType.Attributes, schema);
		}
	}

	private XmlElement ExportIsDictionary()
	{
		XmlElement xmlElement = XmlDoc.CreateElement(IsDictionaryAnnotationName.Name, IsDictionaryAnnotationName.Namespace);
		xmlElement.InnerText = "true";
		return xmlElement;
	}

	private void ExportEnumDataContract(EnumDataContract enumDataContract, XmlSchema schema)
	{
		XmlSchemaSimpleType xmlSchemaSimpleType = new XmlSchemaSimpleType();
		xmlSchemaSimpleType.Name = enumDataContract.StableName.Name;
		XmlElement xmlElement = ((enumDataContract.BaseContractName == DefaultEnumBaseTypeName) ? null : ExportActualType(enumDataContract.BaseContractName));
		xmlSchemaSimpleType.Annotation = GetSchemaAnnotation(xmlElement, ExportSurrogateData(enumDataContract));
		schema.Items.Add(xmlSchemaSimpleType);
		XmlSchemaSimpleTypeRestriction xmlSchemaSimpleTypeRestriction = new XmlSchemaSimpleTypeRestriction();
		xmlSchemaSimpleTypeRestriction.BaseTypeName = StringQualifiedName;
		SchemaHelper.AddSchemaImport(enumDataContract.BaseContractName.Namespace, schema);
		if (enumDataContract.Values != null)
		{
			for (int i = 0; i < enumDataContract.Values.Count; i++)
			{
				XmlSchemaEnumerationFacet xmlSchemaEnumerationFacet = new XmlSchemaEnumerationFacet();
				xmlSchemaEnumerationFacet.Value = enumDataContract.Members[i].Name;
				if (enumDataContract.Values[i] != GetDefaultEnumValue(enumDataContract.IsFlags, i))
				{
					xmlSchemaEnumerationFacet.Annotation = GetSchemaAnnotation(EnumerationValueAnnotationName, enumDataContract.GetStringFromEnumValue(enumDataContract.Values[i]), schema);
				}
				xmlSchemaSimpleTypeRestriction.Facets.Add(xmlSchemaEnumerationFacet);
			}
		}
		if (enumDataContract.IsFlags)
		{
			XmlSchemaSimpleTypeList xmlSchemaSimpleTypeList = new XmlSchemaSimpleTypeList();
			XmlSchemaSimpleType xmlSchemaSimpleType2 = new XmlSchemaSimpleType();
			xmlSchemaSimpleType2.Content = xmlSchemaSimpleTypeRestriction;
			xmlSchemaSimpleTypeList.ItemType = xmlSchemaSimpleType2;
			xmlSchemaSimpleType.Content = xmlSchemaSimpleTypeList;
		}
		else
		{
			xmlSchemaSimpleType.Content = xmlSchemaSimpleTypeRestriction;
		}
	}

	internal static long GetDefaultEnumValue(bool isFlags, int index)
	{
		if (!isFlags)
		{
			return index;
		}
		return (long)Math.Pow(2.0, index);
	}

	private void ExportISerializableDataContract(ClassDataContract dataContract, XmlSchema schema)
	{
		XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
		xmlSchemaComplexType.Name = dataContract.StableName.Name;
		schema.Items.Add(xmlSchemaComplexType);
		XmlElement xmlElement = null;
		if (dataContract.UnderlyingType.IsGenericType)
		{
			xmlElement = ExportGenericInfo(dataContract.UnderlyingType, "GenericType", "http://schemas.microsoft.com/2003/10/Serialization/");
		}
		XmlElement xmlElement2 = null;
		if (dataContract.BaseContract != null)
		{
			CreateTypeContent(xmlSchemaComplexType, dataContract.BaseContract.StableName, schema);
		}
		else
		{
			schema.Namespaces.Add("ser", "http://schemas.microsoft.com/2003/10/Serialization/");
			xmlSchemaComplexType.Particle = ISerializableSequence;
			XmlSchemaAttribute iSerializableFactoryTypeAttribute = ISerializableFactoryTypeAttribute;
			xmlSchemaComplexType.Attributes.Add(iSerializableFactoryTypeAttribute);
			SchemaHelper.AddSchemaImport(ISerializableFactoryTypeAttribute.RefName.Namespace, schema);
			if (dataContract.IsValueType)
			{
				xmlElement2 = GetAnnotationMarkup(IsValueTypeName, XmlConvert.ToString(dataContract.IsValueType), schema);
			}
		}
		xmlSchemaComplexType.Annotation = GetSchemaAnnotation(xmlElement, ExportSurrogateData(dataContract), xmlElement2);
	}

	private XmlSchemaComplexContentExtension CreateTypeContent(XmlSchemaComplexType type, XmlQualifiedName baseTypeName, XmlSchema schema)
	{
		SchemaHelper.AddSchemaImport(baseTypeName.Namespace, schema);
		XmlSchemaComplexContentExtension xmlSchemaComplexContentExtension = new XmlSchemaComplexContentExtension();
		xmlSchemaComplexContentExtension.BaseTypeName = baseTypeName;
		type.ContentModel = new XmlSchemaComplexContent();
		type.ContentModel.Content = xmlSchemaComplexContentExtension;
		return xmlSchemaComplexContentExtension;
	}

	private void ExportXmlDataContract(XmlDataContract dataContract)
	{
		Type underlyingType = dataContract.UnderlyingType;
		if (!IsSpecialXmlType(underlyingType, out var typeName, out var xsdType, out var hasRoot) && !InvokeSchemaProviderMethod(underlyingType, schemas, out typeName, out xsdType, out hasRoot))
		{
			InvokeGetSchemaMethod(underlyingType, schemas, typeName);
		}
		if (hasRoot)
		{
			typeName.Equals(dataContract.StableName);
			if (SchemaHelper.GetSchemaElement(Schemas, new XmlQualifiedName(dataContract.TopLevelElementName.Value, dataContract.TopLevelElementNamespace.Value), out var outSchema) == null)
			{
				ExportTopLevelElement(dataContract, outSchema).IsNillable = dataContract.IsTopLevelElementNullable;
				ReprocessAll(schemas);
			}
			XmlSchemaType xmlSchemaType = xsdType;
			xsdType = SchemaHelper.GetSchemaType(schemas, typeName, out outSchema);
			if (xmlSchemaType == null && xsdType == null && typeName.Namespace != "http://www.w3.org/2001/XMLSchema")
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Schema type '{0}' is missing and required for '{1}' type.", typeName, DataContract.GetClrTypeFullName(underlyingType))));
			}
			if (xsdType != null)
			{
				xsdType.Annotation = GetSchemaAnnotation(ExportSurrogateData(dataContract), dataContract.IsValueType ? GetAnnotationMarkup(IsValueTypeName, XmlConvert.ToString(dataContract.IsValueType), outSchema) : null);
			}
			else if (DiagnosticUtility.ShouldTraceVerbose)
			{
				TraceUtility.Trace(TraceEventType.Verbose, 196622, SR.GetString("XSD export annotation failed"), new StringTraceRecord("Type", typeName.Namespace + ":" + typeName.Name));
			}
		}
	}

	private static void ReprocessAll(XmlSchemaSet schemas)
	{
		Hashtable hashtable = new Hashtable();
		Hashtable hashtable2 = new Hashtable();
		XmlSchema[] array = new XmlSchema[schemas.Count];
		schemas.CopyTo(array, 0);
		foreach (XmlSchema xmlSchema in array)
		{
			XmlSchemaObject[] array2 = new XmlSchemaObject[xmlSchema.Items.Count];
			xmlSchema.Items.CopyTo(array2, 0);
			foreach (XmlSchemaObject xmlSchemaObject in array2)
			{
				Hashtable hashtable3;
				XmlQualifiedName xmlQualifiedName;
				if (xmlSchemaObject is XmlSchemaElement)
				{
					hashtable3 = hashtable;
					xmlQualifiedName = new XmlQualifiedName(((XmlSchemaElement)xmlSchemaObject).Name, xmlSchema.TargetNamespace);
				}
				else
				{
					if (!(xmlSchemaObject is XmlSchemaType))
					{
						continue;
					}
					hashtable3 = hashtable2;
					xmlQualifiedName = new XmlQualifiedName(((XmlSchemaType)xmlSchemaObject).Name, xmlSchema.TargetNamespace);
				}
				if (hashtable3[xmlQualifiedName] != null)
				{
					if (DiagnosticUtility.ShouldTraceWarning)
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>(2)
						{
							{
								"ItemType",
								xmlSchemaObject.ToString()
							},
							{
								"Name",
								xmlQualifiedName.Namespace + ":" + xmlQualifiedName.Name
							}
						};
						TraceUtility.Trace(TraceEventType.Warning, 196624, SR.GetString("XSD export duplicate items"), new DictionaryTraceRecord(dictionary));
					}
					xmlSchema.Items.Remove(xmlSchemaObject);
				}
				else
				{
					hashtable3.Add(xmlQualifiedName, xmlSchemaObject);
				}
			}
			schemas.Reprocess(xmlSchema);
		}
	}

	internal static void GetXmlTypeInfo(Type type, out XmlQualifiedName stableName, out XmlSchemaType xsdType, out bool hasRoot)
	{
		if (!IsSpecialXmlType(type, out stableName, out xsdType, out hasRoot))
		{
			XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
			xmlSchemaSet.XmlResolver = null;
			InvokeSchemaProviderMethod(type, xmlSchemaSet, out stableName, out xsdType, out hasRoot);
			if (stableName.Name == null || stableName.Name.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("XML data contract Name for type '{0}' cannot be set to null or empty string.", DataContract.GetClrTypeFullName(type))));
			}
		}
	}

	private static bool InvokeSchemaProviderMethod(Type clrType, XmlSchemaSet schemas, out XmlQualifiedName stableName, out XmlSchemaType xsdType, out bool hasRoot)
	{
		xsdType = null;
		hasRoot = true;
		object[] customAttributes = clrType.GetCustomAttributes(Globals.TypeOfXmlSchemaProviderAttribute, inherit: false);
		if (customAttributes == null || customAttributes.Length == 0)
		{
			stableName = DataContract.GetDefaultStableName(clrType);
			return false;
		}
		XmlSchemaProviderAttribute xmlSchemaProviderAttribute = (XmlSchemaProviderAttribute)customAttributes[0];
		if (xmlSchemaProviderAttribute.IsAny)
		{
			xsdType = CreateAnyElementType();
			hasRoot = false;
		}
		string methodName = xmlSchemaProviderAttribute.MethodName;
		if (methodName == null || methodName.Length == 0)
		{
			if (!xmlSchemaProviderAttribute.IsAny)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have MethodName on XmlSchemaProviderAttribute attribute set to null or empty string.", DataContract.GetClrTypeFullName(clrType))));
			}
			stableName = DataContract.GetDefaultStableName(clrType);
		}
		else
		{
			MethodInfo method = clrType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[1] { typeof(XmlSchemaSet) }, null);
			if (method == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' does not have a static method '{1}' that takes a parameter of type 'System.Xml.Schema.XmlSchemaSet' as specified by the XmlSchemaProviderAttribute attribute.", DataContract.GetClrTypeFullName(clrType), methodName)));
			}
			if (!Globals.TypeOfXmlQualifiedName.IsAssignableFrom(method.ReturnType) && !Globals.TypeOfXmlSchemaType.IsAssignableFrom(method.ReturnType))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Method '{0}.{1}()' returns '{2}'. The return type must be compatible with '{3}'.", DataContract.GetClrTypeFullName(clrType), methodName, DataContract.GetClrTypeFullName(method.ReturnType), DataContract.GetClrTypeFullName(Globals.TypeOfXmlQualifiedName), typeof(XmlSchemaType))));
			}
			object obj = method.Invoke(null, new object[1] { schemas });
			if (xmlSchemaProviderAttribute.IsAny)
			{
				if (obj != null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Method '{0}.{1}()' returns a non-null value. The return value must be null since IsAny=true.", DataContract.GetClrTypeFullName(clrType), methodName)));
				}
				stableName = DataContract.GetDefaultStableName(clrType);
			}
			else if (obj == null)
			{
				xsdType = CreateAnyElementType();
				hasRoot = false;
				stableName = DataContract.GetDefaultStableName(clrType);
			}
			else if (obj is XmlSchemaType { Name: var localName } xmlSchemaType)
			{
				string ns = null;
				if (localName == null || localName.Length == 0)
				{
					DataContract.GetDefaultStableName(DataContract.GetClrTypeFullName(clrType), out localName, out ns);
					stableName = new XmlQualifiedName(localName, ns);
					xmlSchemaType.Annotation = GetSchemaAnnotation(ExportActualType(stableName, new XmlDocument()));
					xsdType = xmlSchemaType;
				}
				else
				{
					foreach (XmlSchema item in schemas.Schemas())
					{
						foreach (XmlSchemaObject item2 in item.Items)
						{
							if (item2 == xmlSchemaType)
							{
								ns = item.TargetNamespace;
								if (ns == null)
								{
									ns = string.Empty;
								}
								break;
							}
						}
						if (ns != null)
						{
							break;
						}
					}
					if (ns == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Schema type '{0}' is missing and required for '{1}' type.", localName, DataContract.GetClrTypeFullName(clrType))));
					}
					stableName = new XmlQualifiedName(localName, ns);
				}
			}
			else
			{
				stableName = (XmlQualifiedName)obj;
			}
		}
		return true;
	}

	private static void InvokeGetSchemaMethod(Type clrType, XmlSchemaSet schemas, XmlQualifiedName stableName)
	{
		XmlSchema schema = ((IXmlSerializable)Activator.CreateInstance(clrType)).GetSchema();
		if (schema == null)
		{
			AddDefaultDatasetType(schemas, stableName.Name, stableName.Namespace);
			return;
		}
		if (schema.Id == null || schema.Id.Length == 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("On type '{0}', the return value from GetSchema method was invalid.", DataContract.GetClrTypeFullName(clrType))));
		}
		AddDefaultTypedDatasetType(schemas, schema, stableName.Name, stableName.Namespace);
	}

	internal static void AddDefaultXmlType(XmlSchemaSet schemas, string localName, string ns)
	{
		XmlSchemaComplexType xmlSchemaComplexType = CreateAnyType();
		xmlSchemaComplexType.Name = localName;
		XmlSchema schema = SchemaHelper.GetSchema(ns, schemas);
		schema.Items.Add(xmlSchemaComplexType);
		schemas.Reprocess(schema);
	}

	private static XmlSchemaComplexType CreateAnyType()
	{
		XmlSchemaComplexType obj = new XmlSchemaComplexType
		{
			IsMixed = true,
			Particle = new XmlSchemaSequence()
		};
		XmlSchemaAny item = new XmlSchemaAny
		{
			MinOccurs = 0m,
			MaxOccurs = decimal.MaxValue,
			ProcessContents = XmlSchemaContentProcessing.Lax
		};
		((XmlSchemaSequence)obj.Particle).Items.Add(item);
		obj.AnyAttribute = new XmlSchemaAnyAttribute();
		return obj;
	}

	private static XmlSchemaComplexType CreateAnyElementType()
	{
		XmlSchemaComplexType obj = new XmlSchemaComplexType
		{
			IsMixed = false,
			Particle = new XmlSchemaSequence()
		};
		XmlSchemaAny item = new XmlSchemaAny
		{
			MinOccurs = 0m,
			ProcessContents = XmlSchemaContentProcessing.Lax
		};
		((XmlSchemaSequence)obj.Particle).Items.Add(item);
		return obj;
	}

	internal static bool IsSpecialXmlType(Type type, out XmlQualifiedName typeName, out XmlSchemaType xsdType, out bool hasRoot)
	{
		xsdType = null;
		hasRoot = true;
		if (type == Globals.TypeOfXmlElement || type == Globals.TypeOfXmlNodeArray)
		{
			string text = null;
			if (type == Globals.TypeOfXmlElement)
			{
				xsdType = CreateAnyElementType();
				text = "XmlElement";
				hasRoot = false;
			}
			else
			{
				xsdType = CreateAnyType();
				text = "ArrayOfXmlNode";
				hasRoot = true;
			}
			typeName = new XmlQualifiedName(text, DataContract.GetDefaultStableNamespace(type));
			return true;
		}
		typeName = null;
		return false;
	}

	private static void AddDefaultDatasetType(XmlSchemaSet schemas, string localName, string ns)
	{
		XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
		xmlSchemaComplexType.Name = localName;
		xmlSchemaComplexType.Particle = new XmlSchemaSequence();
		XmlSchemaElement xmlSchemaElement = new XmlSchemaElement();
		xmlSchemaElement.RefName = new XmlQualifiedName("schema", "http://www.w3.org/2001/XMLSchema");
		((XmlSchemaSequence)xmlSchemaComplexType.Particle).Items.Add(xmlSchemaElement);
		XmlSchemaAny item = new XmlSchemaAny();
		((XmlSchemaSequence)xmlSchemaComplexType.Particle).Items.Add(item);
		XmlSchema schema = SchemaHelper.GetSchema(ns, schemas);
		schema.Items.Add(xmlSchemaComplexType);
		schemas.Reprocess(schema);
	}

	private static void AddDefaultTypedDatasetType(XmlSchemaSet schemas, XmlSchema datasetSchema, string localName, string ns)
	{
		XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
		xmlSchemaComplexType.Name = localName;
		xmlSchemaComplexType.Particle = new XmlSchemaSequence();
		XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
		xmlSchemaAny.Namespace = ((datasetSchema.TargetNamespace == null) ? string.Empty : datasetSchema.TargetNamespace);
		((XmlSchemaSequence)xmlSchemaComplexType.Particle).Items.Add(xmlSchemaAny);
		schemas.Add(datasetSchema);
		XmlSchema schema = SchemaHelper.GetSchema(ns, schemas);
		schema.Items.Add(xmlSchemaComplexType);
		schemas.Reprocess(datasetSchema);
		schemas.Reprocess(schema);
	}

	private XmlSchemaAnnotation GetSchemaAnnotation(XmlQualifiedName annotationQualifiedName, string innerText, XmlSchema schema)
	{
		XmlSchemaAnnotation xmlSchemaAnnotation = new XmlSchemaAnnotation();
		XmlSchemaAppInfo xmlSchemaAppInfo = new XmlSchemaAppInfo();
		XmlElement annotationMarkup = GetAnnotationMarkup(annotationQualifiedName, innerText, schema);
		xmlSchemaAppInfo.Markup = new XmlNode[1] { annotationMarkup };
		xmlSchemaAnnotation.Items.Add(xmlSchemaAppInfo);
		return xmlSchemaAnnotation;
	}

	private static XmlSchemaAnnotation GetSchemaAnnotation(params XmlNode[] nodes)
	{
		if (nodes == null || nodes.Length == 0)
		{
			return null;
		}
		bool flag = false;
		for (int i = 0; i < nodes.Length; i++)
		{
			if (nodes[i] != null)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return null;
		}
		XmlSchemaAnnotation xmlSchemaAnnotation = new XmlSchemaAnnotation();
		XmlSchemaAppInfo xmlSchemaAppInfo = new XmlSchemaAppInfo();
		xmlSchemaAnnotation.Items.Add(xmlSchemaAppInfo);
		xmlSchemaAppInfo.Markup = nodes;
		return xmlSchemaAnnotation;
	}

	private XmlElement GetAnnotationMarkup(XmlQualifiedName annotationQualifiedName, string innerText, XmlSchema schema)
	{
		XmlElement xmlElement = XmlDoc.CreateElement(annotationQualifiedName.Name, annotationQualifiedName.Namespace);
		SchemaHelper.AddSchemaImport(annotationQualifiedName.Namespace, schema);
		xmlElement.InnerText = innerText;
		return xmlElement;
	}

	private XmlSchema GetSchema(string ns)
	{
		return SchemaHelper.GetSchema(ns, Schemas);
	}
}
