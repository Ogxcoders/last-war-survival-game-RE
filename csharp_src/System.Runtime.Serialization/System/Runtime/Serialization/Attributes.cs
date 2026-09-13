using System.Security;
using System.Xml;

namespace System.Runtime.Serialization;

internal class Attributes
{
	[SecurityCritical]
	private static XmlDictionaryString[] serializationLocalNames;

	[SecurityCritical]
	private static XmlDictionaryString[] schemaInstanceLocalNames;

	internal string Id;

	internal string Ref;

	internal string XsiTypeName;

	internal string XsiTypeNamespace;

	internal string XsiTypePrefix;

	internal bool XsiNil;

	internal string ClrAssembly;

	internal string ClrType;

	internal int ArraySZSize;

	internal string FactoryTypeName;

	internal string FactoryTypeNamespace;

	internal string FactoryTypePrefix;

	internal bool UnrecognizedAttributesFound;

	[SecuritySafeCritical]
	static Attributes()
	{
		serializationLocalNames = new XmlDictionaryString[6]
		{
			DictionaryGlobals.IdLocalName,
			DictionaryGlobals.ArraySizeLocalName,
			DictionaryGlobals.RefLocalName,
			DictionaryGlobals.ClrTypeLocalName,
			DictionaryGlobals.ClrAssemblyLocalName,
			DictionaryGlobals.ISerializableFactoryTypeLocalName
		};
		schemaInstanceLocalNames = new XmlDictionaryString[2]
		{
			DictionaryGlobals.XsiNilLocalName,
			DictionaryGlobals.XsiTypeLocalName
		};
	}

	[SecuritySafeCritical]
	internal void Read(XmlReaderDelegator reader)
	{
		Reset();
		while (reader.MoveToNextAttribute())
		{
			switch (reader.IndexOfLocalName(serializationLocalNames, DictionaryGlobals.SerializationNamespace))
			{
			case 0:
				ReadId(reader);
				continue;
			case 1:
				ReadArraySize(reader);
				continue;
			case 2:
				ReadRef(reader);
				continue;
			case 3:
				ClrType = reader.Value;
				continue;
			case 4:
				ClrAssembly = reader.Value;
				continue;
			case 5:
				ReadFactoryType(reader);
				continue;
			}
			switch (reader.IndexOfLocalName(schemaInstanceLocalNames, DictionaryGlobals.SchemaInstanceNamespace))
			{
			case 0:
				ReadXsiNil(reader);
				continue;
			case 1:
				ReadXsiType(reader);
				continue;
			}
			if (!reader.IsNamespaceUri(DictionaryGlobals.XmlnsNamespace))
			{
				UnrecognizedAttributesFound = true;
			}
		}
		reader.MoveToElement();
	}

	internal void Reset()
	{
		Id = Globals.NewObjectId;
		Ref = Globals.NewObjectId;
		XsiTypeName = null;
		XsiTypeNamespace = null;
		XsiTypePrefix = null;
		XsiNil = false;
		ClrAssembly = null;
		ClrType = null;
		ArraySZSize = -1;
		FactoryTypeName = null;
		FactoryTypeNamespace = null;
		FactoryTypePrefix = null;
		UnrecognizedAttributesFound = false;
	}

	private void ReadId(XmlReaderDelegator reader)
	{
		Id = reader.ReadContentAsString();
		if (string.IsNullOrEmpty(Id))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Invalid Id '{0}'. Must not be null or empty.", Id)));
		}
	}

	private void ReadRef(XmlReaderDelegator reader)
	{
		Ref = reader.ReadContentAsString();
		if (string.IsNullOrEmpty(Ref))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Invalid Ref '{0}'. Must not be null or empty.", Ref)));
		}
	}

	private void ReadXsiNil(XmlReaderDelegator reader)
	{
		XsiNil = reader.ReadContentAsBoolean();
	}

	private void ReadArraySize(XmlReaderDelegator reader)
	{
		ArraySZSize = reader.ReadContentAsInt();
		if (ArraySZSize < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Invalid Size '{0}'. Must be non-negative integer.", ArraySZSize)));
		}
	}

	private void ReadXsiType(XmlReaderDelegator reader)
	{
		string value = reader.Value;
		if (value != null && value.Length > 0)
		{
			XmlObjectSerializerReadContext.ParseQualifiedName(value, reader, out XsiTypeName, out XsiTypeNamespace, out XsiTypePrefix);
		}
	}

	private void ReadFactoryType(XmlReaderDelegator reader)
	{
		string value = reader.Value;
		if (value != null && value.Length > 0)
		{
			XmlObjectSerializerReadContext.ParseQualifiedName(value, reader, out FactoryTypeName, out FactoryTypeNamespace, out FactoryTypePrefix);
		}
	}
}
