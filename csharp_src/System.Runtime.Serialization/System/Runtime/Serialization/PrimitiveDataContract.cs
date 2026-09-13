using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization;

internal abstract class PrimitiveDataContract : DataContract
{
	private class PrimitiveDataContractCriticalHelper : DataContractCriticalHelper
	{
		private MethodInfo xmlFormatWriterMethod;

		private MethodInfo xmlFormatContentWriterMethod;

		private MethodInfo xmlFormatReaderMethod;

		internal MethodInfo XmlFormatWriterMethod
		{
			get
			{
				return xmlFormatWriterMethod;
			}
			set
			{
				xmlFormatWriterMethod = value;
			}
		}

		internal MethodInfo XmlFormatContentWriterMethod
		{
			get
			{
				return xmlFormatContentWriterMethod;
			}
			set
			{
				xmlFormatContentWriterMethod = value;
			}
		}

		internal MethodInfo XmlFormatReaderMethod
		{
			get
			{
				return xmlFormatReaderMethod;
			}
			set
			{
				xmlFormatReaderMethod = value;
			}
		}

		internal PrimitiveDataContractCriticalHelper(Type type, XmlDictionaryString name, XmlDictionaryString ns)
			: base(type)
		{
			SetDataContractName(name, ns);
		}
	}

	[SecurityCritical]
	private PrimitiveDataContractCriticalHelper helper;

	internal abstract string WriteMethodName { get; }

	internal abstract string ReadMethodName { get; }

	internal override XmlDictionaryString TopLevelElementNamespace
	{
		get
		{
			return DictionaryGlobals.SerializationNamespace;
		}
		set
		{
		}
	}

	internal override bool CanContainReferences => false;

	internal override bool IsPrimitive => true;

	internal override bool IsBuiltInDataContract => true;

	internal MethodInfo XmlFormatWriterMethod
	{
		[System.Runtime.CompilerServices.PreserveDependency("WriteChar", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteBoolean", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteSignedByte", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteUnsignedByte", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteShort", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteUnsignedShort", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteInt", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteUnsignedInt", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteLong", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteUnsignedLong", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteFloat", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteDouble", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteDecimal", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteDateTime", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteString", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteBase64", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteAnyType", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteTimeSpan", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteGuid", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteUri", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteQName", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteString", "System.Runtime.Serialization.XmlObjectSerializerWriteContext")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteBase64", "System.Runtime.Serialization.XmlObjectSerializerWriteContext")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteAnyType", "System.Runtime.Serialization.XmlObjectSerializerWriteContext")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteUri", "System.Runtime.Serialization.XmlObjectSerializerWriteContext")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteQName", "System.Runtime.Serialization.XmlObjectSerializerWriteContext")]
		[SecuritySafeCritical]
		get
		{
			if (helper.XmlFormatWriterMethod == null)
			{
				if (base.UnderlyingType.IsValueType)
				{
					helper.XmlFormatWriterMethod = typeof(XmlWriterDelegator).GetMethod(WriteMethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[3]
					{
						base.UnderlyingType,
						typeof(XmlDictionaryString),
						typeof(XmlDictionaryString)
					}, null);
				}
				else
				{
					helper.XmlFormatWriterMethod = typeof(XmlObjectSerializerWriteContext).GetMethod(WriteMethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[4]
					{
						typeof(XmlWriterDelegator),
						base.UnderlyingType,
						typeof(XmlDictionaryString),
						typeof(XmlDictionaryString)
					}, null);
				}
			}
			return helper.XmlFormatWriterMethod;
		}
	}

	internal MethodInfo XmlFormatContentWriterMethod
	{
		[System.Runtime.CompilerServices.PreserveDependency("WriteChar", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteBoolean", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteSignedByte", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteUnsignedByte", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteShort", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteUnsignedShort", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteInt", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteUnsignedInt", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteLong", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteUnsignedLong", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteFloat", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteDouble", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteDecimal", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteDateTime", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteString", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteBase64", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteAnyType", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteTimeSpan", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteGuid", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteUri", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteQName", "System.Runtime.Serialization.XmlWriterDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteString", "System.Runtime.Serialization.XmlObjectSerializerWriteContext")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteBase64", "System.Runtime.Serialization.XmlObjectSerializerWriteContext")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteAnyType", "System.Runtime.Serialization.XmlObjectSerializerWriteContext")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteUri", "System.Runtime.Serialization.XmlObjectSerializerWriteContext")]
		[System.Runtime.CompilerServices.PreserveDependency("WriteQName", "System.Runtime.Serialization.XmlObjectSerializerWriteContext")]
		[SecuritySafeCritical]
		get
		{
			if (helper.XmlFormatContentWriterMethod == null)
			{
				if (base.UnderlyingType.IsValueType)
				{
					helper.XmlFormatContentWriterMethod = typeof(XmlWriterDelegator).GetMethod(WriteMethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[1] { base.UnderlyingType }, null);
				}
				else
				{
					helper.XmlFormatContentWriterMethod = typeof(XmlObjectSerializerWriteContext).GetMethod(WriteMethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[2]
					{
						typeof(XmlWriterDelegator),
						base.UnderlyingType
					}, null);
				}
			}
			return helper.XmlFormatContentWriterMethod;
		}
	}

	internal MethodInfo XmlFormatReaderMethod
	{
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsChar", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsBoolean", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsSignedByte", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsUnsignedByte", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsShort", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsUnsignedShort", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsInt", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsUnsignedInt", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsLong", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsUnsignedLong", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsFloat", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsDouble", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsDecimal", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsDateTime", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsString", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsBase64", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsAnyType", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsTimeSpan", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsGuid", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsUri", "System.Runtime.Serialization.XmlReaderDelegator")]
		[System.Runtime.CompilerServices.PreserveDependency("ReadElementContentAsQName", "System.Runtime.Serialization.XmlReaderDelegator")]
		[SecuritySafeCritical]
		get
		{
			if (helper.XmlFormatReaderMethod == null)
			{
				helper.XmlFormatReaderMethod = typeof(XmlReaderDelegator).GetMethod(ReadMethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			return helper.XmlFormatReaderMethod;
		}
	}

	[SecuritySafeCritical]
	protected PrimitiveDataContract(Type type, XmlDictionaryString name, XmlDictionaryString ns)
		: base(new PrimitiveDataContractCriticalHelper(type, name, ns))
	{
		helper = base.Helper as PrimitiveDataContractCriticalHelper;
	}

	internal static PrimitiveDataContract GetPrimitiveDataContract(Type type)
	{
		return DataContract.GetBuiltInDataContract(type) as PrimitiveDataContract;
	}

	internal static PrimitiveDataContract GetPrimitiveDataContract(string name, string ns)
	{
		return DataContract.GetBuiltInDataContract(name, ns) as PrimitiveDataContract;
	}

	public override void WriteXmlValue(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context)
	{
		xmlWriter.WriteAnyType(obj);
	}

	protected object HandleReadValue(object obj, XmlObjectSerializerReadContext context)
	{
		context.AddNewObject(obj);
		return obj;
	}

	protected bool TryReadNullAtTopLevel(XmlReaderDelegator reader)
	{
		Attributes attributes = new Attributes();
		attributes.Read(reader);
		if (attributes.Ref != Globals.NewObjectId)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Cannot deserialize since root element references unrecognized object with id '{0}'.", attributes.Ref)));
		}
		if (attributes.XsiNil)
		{
			reader.Skip();
			return true;
		}
		return false;
	}

	internal override bool Equals(object other, Dictionary<DataContractPairKey, object> checkedContracts)
	{
		if (other is PrimitiveDataContract)
		{
			Type type = GetType();
			Type type2 = other.GetType();
			if (!type.Equals(type2) && !type.IsSubclassOf(type2))
			{
				return type2.IsSubclassOf(type);
			}
			return true;
		}
		return false;
	}
}
