using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Runtime.Serialization;

internal sealed class XmlDataContract : DataContract
{
	private class XmlDataContractCriticalHelper : DataContractCriticalHelper
	{
		private Dictionary<XmlQualifiedName, DataContract> knownDataContracts;

		private bool isKnownTypeAttributeChecked;

		private XmlDictionaryString topLevelElementName;

		private XmlDictionaryString topLevelElementNamespace;

		private bool isTopLevelElementNullable;

		private bool isTypeDefinedOnImport;

		private XmlSchemaType xsdType;

		private bool hasRoot;

		private CreateXmlSerializableDelegate createXmlSerializable;

		internal override Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
		{
			get
			{
				if (!isKnownTypeAttributeChecked && base.UnderlyingType != null)
				{
					lock (this)
					{
						if (!isKnownTypeAttributeChecked)
						{
							knownDataContracts = DataContract.ImportKnownTypeAttributes(base.UnderlyingType);
							Thread.MemoryBarrier();
							isKnownTypeAttributeChecked = true;
						}
					}
				}
				return knownDataContracts;
			}
			set
			{
				knownDataContracts = value;
			}
		}

		internal XmlSchemaType XsdType
		{
			get
			{
				return xsdType;
			}
			set
			{
				xsdType = value;
			}
		}

		internal bool IsAnonymous => xsdType != null;

		internal override bool HasRoot
		{
			get
			{
				return hasRoot;
			}
			set
			{
				hasRoot = value;
			}
		}

		internal override XmlDictionaryString TopLevelElementName
		{
			get
			{
				return topLevelElementName;
			}
			set
			{
				topLevelElementName = value;
			}
		}

		internal override XmlDictionaryString TopLevelElementNamespace
		{
			get
			{
				return topLevelElementNamespace;
			}
			set
			{
				topLevelElementNamespace = value;
			}
		}

		internal bool IsTopLevelElementNullable
		{
			get
			{
				return isTopLevelElementNullable;
			}
			set
			{
				isTopLevelElementNullable = value;
			}
		}

		internal bool IsTypeDefinedOnImport
		{
			get
			{
				return isTypeDefinedOnImport;
			}
			set
			{
				isTypeDefinedOnImport = value;
			}
		}

		internal CreateXmlSerializableDelegate CreateXmlSerializableDelegate
		{
			get
			{
				return createXmlSerializable;
			}
			set
			{
				createXmlSerializable = value;
			}
		}

		internal XmlDataContractCriticalHelper()
		{
		}

		internal XmlDataContractCriticalHelper(Type type)
			: base(type)
		{
			if (type.IsDefined(Globals.TypeOfDataContractAttribute, inherit: false))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot be IXmlSerializable and have DataContractAttribute attribute.", DataContract.GetClrTypeFullName(type))));
			}
			if (type.IsDefined(Globals.TypeOfCollectionDataContractAttribute, inherit: false))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot be IXmlSerializable and have CollectionDataContractAttribute attribute.", DataContract.GetClrTypeFullName(type))));
			}
			SchemaExporter.GetXmlTypeInfo(type, out var xmlQualifiedName, out var xmlSchemaType, out var flag);
			base.StableName = xmlQualifiedName;
			XsdType = xmlSchemaType;
			HasRoot = flag;
			XmlDictionary xmlDictionary = new XmlDictionary();
			base.Name = xmlDictionary.Add(base.StableName.Name);
			base.Namespace = xmlDictionary.Add(base.StableName.Namespace);
			object[] array = ((base.UnderlyingType == null) ? null : base.UnderlyingType.GetCustomAttributes(Globals.TypeOfXmlRootAttribute, inherit: false));
			if (array == null || array.Length == 0)
			{
				if (flag)
				{
					topLevelElementName = base.Name;
					topLevelElementNamespace = ((base.StableName.Namespace == "http://www.w3.org/2001/XMLSchema") ? DictionaryGlobals.EmptyString : base.Namespace);
					isTopLevelElementNullable = true;
				}
				return;
			}
			if (flag)
			{
				XmlRootAttribute xmlRootAttribute = (XmlRootAttribute)array[0];
				isTopLevelElementNullable = xmlRootAttribute.IsNullable;
				string elementName = xmlRootAttribute.ElementName;
				topLevelElementName = ((elementName == null || elementName.Length == 0) ? base.Name : xmlDictionary.Add(DataContract.EncodeLocalName(elementName)));
				string text = xmlRootAttribute.Namespace;
				topLevelElementNamespace = ((text == null || text.Length == 0) ? DictionaryGlobals.EmptyString : xmlDictionary.Add(text));
				return;
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot specify an XmlRootAttribute attribute because its IsAny setting is 'true'. This type must write all its contents including the root element. Verify that the IXmlSerializable implementation is correct.", DataContract.GetClrTypeFullName(base.UnderlyingType))));
		}
	}

	[SecurityCritical]
	private XmlDataContractCriticalHelper helper;

	internal override Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
	{
		[SecuritySafeCritical]
		get
		{
			return helper.KnownDataContracts;
		}
		[SecurityCritical]
		set
		{
			helper.KnownDataContracts = value;
		}
	}

	internal XmlSchemaType XsdType
	{
		[SecuritySafeCritical]
		get
		{
			return helper.XsdType;
		}
		[SecurityCritical]
		set
		{
			helper.XsdType = value;
		}
	}

	internal bool IsAnonymous
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsAnonymous;
		}
	}

	internal override bool HasRoot
	{
		[SecuritySafeCritical]
		get
		{
			return helper.HasRoot;
		}
		[SecurityCritical]
		set
		{
			helper.HasRoot = value;
		}
	}

	internal override XmlDictionaryString TopLevelElementName
	{
		[SecuritySafeCritical]
		get
		{
			return helper.TopLevelElementName;
		}
		[SecurityCritical]
		set
		{
			helper.TopLevelElementName = value;
		}
	}

	internal override XmlDictionaryString TopLevelElementNamespace
	{
		[SecuritySafeCritical]
		get
		{
			return helper.TopLevelElementNamespace;
		}
		[SecurityCritical]
		set
		{
			helper.TopLevelElementNamespace = value;
		}
	}

	internal bool IsTopLevelElementNullable
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsTopLevelElementNullable;
		}
		[SecurityCritical]
		set
		{
			helper.IsTopLevelElementNullable = value;
		}
	}

	internal bool IsTypeDefinedOnImport
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsTypeDefinedOnImport;
		}
		[SecurityCritical]
		set
		{
			helper.IsTypeDefinedOnImport = value;
		}
	}

	internal CreateXmlSerializableDelegate CreateXmlSerializableDelegate
	{
		[SecuritySafeCritical]
		get
		{
			if (helper.CreateXmlSerializableDelegate == null)
			{
				lock (this)
				{
					if (helper.CreateXmlSerializableDelegate == null)
					{
						CreateXmlSerializableDelegate createXmlSerializableDelegate = GenerateCreateXmlSerializableDelegate();
						Thread.MemoryBarrier();
						helper.CreateXmlSerializableDelegate = createXmlSerializableDelegate;
					}
				}
			}
			return helper.CreateXmlSerializableDelegate;
		}
	}

	internal override bool CanContainReferences => false;

	internal override bool IsBuiltInDataContract
	{
		get
		{
			if (!(base.UnderlyingType == Globals.TypeOfXmlElement))
			{
				return base.UnderlyingType == Globals.TypeOfXmlNodeArray;
			}
			return true;
		}
	}

	[SecuritySafeCritical]
	internal XmlDataContract()
		: base(new XmlDataContractCriticalHelper())
	{
		helper = base.Helper as XmlDataContractCriticalHelper;
	}

	[SecuritySafeCritical]
	internal XmlDataContract(Type type)
		: base(new XmlDataContractCriticalHelper(type))
	{
		helper = base.Helper as XmlDataContractCriticalHelper;
	}

	private ConstructorInfo GetConstructor()
	{
		Type underlyingType = base.UnderlyingType;
		if (underlyingType.IsValueType)
		{
			return null;
		}
		ConstructorInfo constructor = underlyingType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null);
		if (constructor == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("IXmlSerializable Type '{0}' must have default constructor.", DataContract.GetClrTypeFullName(underlyingType))));
		}
		return constructor;
	}

	[SecurityCritical]
	internal void SetTopLevelElementName(XmlQualifiedName elementName)
	{
		if (elementName != null)
		{
			XmlDictionary xmlDictionary = new XmlDictionary();
			TopLevelElementName = xmlDictionary.Add(elementName.Name);
			TopLevelElementNamespace = xmlDictionary.Add(elementName.Namespace);
		}
	}

	internal override bool Equals(object other, Dictionary<DataContractPairKey, object> checkedContracts)
	{
		if (IsEqualOrChecked(other, checkedContracts))
		{
			return true;
		}
		if (other is XmlDataContract xmlDataContract)
		{
			if (HasRoot != xmlDataContract.HasRoot)
			{
				return false;
			}
			if (IsAnonymous)
			{
				return xmlDataContract.IsAnonymous;
			}
			if (base.StableName.Name == xmlDataContract.StableName.Name)
			{
				return base.StableName.Namespace == xmlDataContract.StableName.Namespace;
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override void WriteXmlValue(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context)
	{
		if (context == null)
		{
			XmlObjectSerializerWriteContext.WriteRootIXmlSerializable(xmlWriter, obj);
		}
		else
		{
			context.WriteIXmlSerializable(xmlWriter, obj);
		}
	}

	public override object ReadXmlValue(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context)
	{
		object obj;
		if (context == null)
		{
			obj = XmlObjectSerializerReadContext.ReadRootIXmlSerializable(xmlReader, this, isMemberType: true);
		}
		else
		{
			obj = context.ReadIXmlSerializable(xmlReader, this, isMemberType: true);
			context.AddNewObject(obj);
		}
		xmlReader.ReadEndElement();
		return obj;
	}

	internal CreateXmlSerializableDelegate GenerateCreateXmlSerializableDelegate()
	{
		return () => new XmlDataContractInterpreter(this).CreateXmlSerializable();
	}
}
