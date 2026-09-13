using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;
using System.Security.Permissions;
using System.Xml;

namespace System.Runtime.Serialization;

internal class XmlObjectSerializerWriteContextComplex : XmlObjectSerializerWriteContext
{
	protected IDataContractSurrogate dataContractSurrogate;

	private SerializationMode mode;

	private SerializationBinder binder;

	private ISurrogateSelector surrogateSelector;

	private StreamingContext streamingContext;

	private Hashtable surrogateDataContracts;

	internal override SerializationMode Mode => mode;

	internal XmlObjectSerializerWriteContextComplex(DataContractSerializer serializer, DataContract rootTypeDataContract, DataContractResolver dataContractResolver)
		: base(serializer, rootTypeDataContract, dataContractResolver)
	{
		mode = SerializationMode.SharedContract;
		preserveObjectReferences = serializer.PreserveObjectReferences;
		dataContractSurrogate = serializer.DataContractSurrogate;
	}

	internal XmlObjectSerializerWriteContextComplex(NetDataContractSerializer serializer, Hashtable surrogateDataContracts)
		: base(serializer)
	{
		mode = SerializationMode.SharedType;
		preserveObjectReferences = true;
		streamingContext = serializer.Context;
		binder = serializer.Binder;
		surrogateSelector = serializer.SurrogateSelector;
		this.surrogateDataContracts = surrogateDataContracts;
	}

	internal XmlObjectSerializerWriteContextComplex(XmlObjectSerializer serializer, int maxItemsInObjectGraph, StreamingContext streamingContext, bool ignoreExtensionDataObject)
		: base(serializer, maxItemsInObjectGraph, streamingContext, ignoreExtensionDataObject)
	{
	}

	internal override DataContract GetDataContract(RuntimeTypeHandle typeHandle, Type type)
	{
		DataContract dataContract = null;
		if (mode == SerializationMode.SharedType && surrogateSelector != null)
		{
			dataContract = NetDataContractSerializer.GetDataContractFromSurrogateSelector(surrogateSelector, streamingContext, typeHandle, type, ref surrogateDataContracts);
		}
		if (dataContract != null)
		{
			if (IsGetOnlyCollection && dataContract is SurrogateDataContract)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Found on type '{0}'.", DataContract.GetClrTypeFullName(dataContract.UnderlyingType))));
			}
			return dataContract;
		}
		return base.GetDataContract(typeHandle, type);
	}

	internal override DataContract GetDataContract(int id, RuntimeTypeHandle typeHandle)
	{
		DataContract dataContract = null;
		if (mode == SerializationMode.SharedType && surrogateSelector != null)
		{
			dataContract = NetDataContractSerializer.GetDataContractFromSurrogateSelector(surrogateSelector, streamingContext, typeHandle, null, ref surrogateDataContracts);
		}
		if (dataContract != null)
		{
			if (IsGetOnlyCollection && dataContract is SurrogateDataContract)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Found on type '{0}'.", DataContract.GetClrTypeFullName(dataContract.UnderlyingType))));
			}
			return dataContract;
		}
		return base.GetDataContract(id, typeHandle);
	}

	internal override DataContract GetDataContractSkipValidation(int typeId, RuntimeTypeHandle typeHandle, Type type)
	{
		DataContract dataContract = null;
		if (mode == SerializationMode.SharedType && surrogateSelector != null)
		{
			dataContract = NetDataContractSerializer.GetDataContractFromSurrogateSelector(surrogateSelector, streamingContext, typeHandle, null, ref surrogateDataContracts);
		}
		if (dataContract != null)
		{
			if (IsGetOnlyCollection && dataContract is SurrogateDataContract)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Found on type '{0}'.", DataContract.GetClrTypeFullName(dataContract.UnderlyingType))));
			}
			return dataContract;
		}
		return base.GetDataContractSkipValidation(typeId, typeHandle, type);
	}

	internal override bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, DataContract dataContract)
	{
		if (mode == SerializationMode.SharedType)
		{
			NetDataContractSerializer.WriteClrTypeInfo(xmlWriter, dataContract, binder);
			return true;
		}
		return false;
	}

	internal override bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, Type dataContractType, string clrTypeName, string clrAssemblyName)
	{
		if (mode == SerializationMode.SharedType)
		{
			NetDataContractSerializer.WriteClrTypeInfo(xmlWriter, dataContractType, binder, clrTypeName, clrAssemblyName);
			return true;
		}
		return false;
	}

	internal override bool WriteClrTypeInfo(XmlWriterDelegator xmlWriter, Type dataContractType, SerializationInfo serInfo)
	{
		if (mode == SerializationMode.SharedType)
		{
			NetDataContractSerializer.WriteClrTypeInfo(xmlWriter, dataContractType, binder, serInfo);
			return true;
		}
		return false;
	}

	public override void WriteAnyType(XmlWriterDelegator xmlWriter, object value)
	{
		if (!OnHandleReference(xmlWriter, value, canContainCyclicReference: false))
		{
			xmlWriter.WriteAnyType(value);
		}
	}

	public override void WriteString(XmlWriterDelegator xmlWriter, string value)
	{
		if (!OnHandleReference(xmlWriter, value, canContainCyclicReference: false))
		{
			xmlWriter.WriteString(value);
		}
	}

	public override void WriteString(XmlWriterDelegator xmlWriter, string value, XmlDictionaryString name, XmlDictionaryString ns)
	{
		if (value == null)
		{
			WriteNull(xmlWriter, typeof(string), isMemberTypeSerializable: true, name, ns);
			return;
		}
		xmlWriter.WriteStartElementPrimitive(name, ns);
		if (!OnHandleReference(xmlWriter, value, canContainCyclicReference: false))
		{
			xmlWriter.WriteString(value);
		}
		xmlWriter.WriteEndElementPrimitive();
	}

	public override void WriteBase64(XmlWriterDelegator xmlWriter, byte[] value)
	{
		if (!OnHandleReference(xmlWriter, value, canContainCyclicReference: false))
		{
			xmlWriter.WriteBase64(value);
		}
	}

	public override void WriteBase64(XmlWriterDelegator xmlWriter, byte[] value, XmlDictionaryString name, XmlDictionaryString ns)
	{
		if (value == null)
		{
			WriteNull(xmlWriter, typeof(byte[]), isMemberTypeSerializable: true, name, ns);
			return;
		}
		xmlWriter.WriteStartElementPrimitive(name, ns);
		if (!OnHandleReference(xmlWriter, value, canContainCyclicReference: false))
		{
			xmlWriter.WriteBase64(value);
		}
		xmlWriter.WriteEndElementPrimitive();
	}

	public override void WriteUri(XmlWriterDelegator xmlWriter, Uri value)
	{
		if (!OnHandleReference(xmlWriter, value, canContainCyclicReference: false))
		{
			xmlWriter.WriteUri(value);
		}
	}

	public override void WriteUri(XmlWriterDelegator xmlWriter, Uri value, XmlDictionaryString name, XmlDictionaryString ns)
	{
		if (value == null)
		{
			WriteNull(xmlWriter, typeof(Uri), isMemberTypeSerializable: true, name, ns);
			return;
		}
		xmlWriter.WriteStartElementPrimitive(name, ns);
		if (!OnHandleReference(xmlWriter, value, canContainCyclicReference: false))
		{
			xmlWriter.WriteUri(value);
		}
		xmlWriter.WriteEndElementPrimitive();
	}

	public override void WriteQName(XmlWriterDelegator xmlWriter, XmlQualifiedName value)
	{
		if (!OnHandleReference(xmlWriter, value, canContainCyclicReference: false))
		{
			xmlWriter.WriteQName(value);
		}
	}

	public override void WriteQName(XmlWriterDelegator xmlWriter, XmlQualifiedName value, XmlDictionaryString name, XmlDictionaryString ns)
	{
		if (value == null)
		{
			WriteNull(xmlWriter, typeof(XmlQualifiedName), isMemberTypeSerializable: true, name, ns);
			return;
		}
		if (ns != null && ns.Value != null && ns.Value.Length > 0)
		{
			xmlWriter.WriteStartElement("q", name, ns);
		}
		else
		{
			xmlWriter.WriteStartElement(name, ns);
		}
		if (!OnHandleReference(xmlWriter, value, canContainCyclicReference: false))
		{
			xmlWriter.WriteQName(value);
		}
		xmlWriter.WriteEndElement();
	}

	public override void InternalSerialize(XmlWriterDelegator xmlWriter, object obj, bool isDeclaredType, bool writeXsiType, int declaredTypeID, RuntimeTypeHandle declaredTypeHandle)
	{
		if (dataContractSurrogate == null)
		{
			base.InternalSerialize(xmlWriter, obj, isDeclaredType, writeXsiType, declaredTypeID, declaredTypeHandle);
		}
		else
		{
			InternalSerializeWithSurrogate(xmlWriter, obj, isDeclaredType, writeXsiType, declaredTypeID, declaredTypeHandle);
		}
	}

	internal override bool OnHandleReference(XmlWriterDelegator xmlWriter, object obj, bool canContainCyclicReference)
	{
		if (preserveObjectReferences && !IsGetOnlyCollection)
		{
			bool newId = true;
			int id = base.SerializedObjects.GetId(obj, ref newId);
			if (newId)
			{
				xmlWriter.WriteAttributeInt("z", DictionaryGlobals.IdLocalName, DictionaryGlobals.SerializationNamespace, id);
			}
			else
			{
				xmlWriter.WriteAttributeInt("z", DictionaryGlobals.RefLocalName, DictionaryGlobals.SerializationNamespace, id);
				xmlWriter.WriteAttributeBool("i", DictionaryGlobals.XsiNilLocalName, DictionaryGlobals.SchemaInstanceNamespace, value: true);
			}
			return !newId;
		}
		return base.OnHandleReference(xmlWriter, obj, canContainCyclicReference);
	}

	internal override void OnEndHandleReference(XmlWriterDelegator xmlWriter, object obj, bool canContainCyclicReference)
	{
		if (!preserveObjectReferences || IsGetOnlyCollection)
		{
			base.OnEndHandleReference(xmlWriter, obj, canContainCyclicReference);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SecuritySafeCritical]
	[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
	private bool CheckIfTypeSerializableForSharedTypeMode(Type memberType)
	{
		ISurrogateSelector selector;
		return surrogateSelector.GetSurrogate(memberType, streamingContext, out selector) != null;
	}

	internal override void CheckIfTypeSerializable(Type memberType, bool isMemberTypeSerializable)
	{
		if (mode == SerializationMode.SharedType && surrogateSelector != null && CheckIfTypeSerializableForSharedTypeMode(memberType))
		{
			return;
		}
		if (dataContractSurrogate != null)
		{
			while (memberType.IsArray)
			{
				memberType = memberType.GetElementType();
			}
			memberType = DataContractSurrogateCaller.GetDataContractType(dataContractSurrogate, memberType);
			if (!DataContract.IsTypeSerializable(memberType))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot be serialized. Consider marking it with the DataContractAttribute attribute, and marking all of its members you want serialized with the DataMemberAttribute attribute. Alternatively, you can ensure that the type is public and has a parameterless constructor - all public members of the type will then be serialized, and no attributes will be required.", memberType)));
			}
		}
		else
		{
			base.CheckIfTypeSerializable(memberType, isMemberTypeSerializable);
		}
	}

	internal override Type GetSurrogatedType(Type type)
	{
		if (dataContractSurrogate == null)
		{
			return base.GetSurrogatedType(type);
		}
		type = DataContract.UnwrapNullableType(type);
		Type surrogatedType = DataContractSerializer.GetSurrogatedType(dataContractSurrogate, type);
		if (IsGetOnlyCollection && surrogatedType != type)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Found on type '{0}'.", DataContract.GetClrTypeFullName(type))));
		}
		return surrogatedType;
	}

	private void InternalSerializeWithSurrogate(XmlWriterDelegator xmlWriter, object obj, bool isDeclaredType, bool writeXsiType, int declaredTypeID, RuntimeTypeHandle declaredTypeHandle)
	{
		RuntimeTypeHandle handle = (isDeclaredType ? declaredTypeHandle : Type.GetTypeHandle(obj));
		object obj2 = obj;
		int oldObjId = 0;
		Type objType = Type.GetTypeFromHandle(handle);
		Type surrogatedType = GetSurrogatedType(Type.GetTypeFromHandle(declaredTypeHandle));
		if (TD.DCSerializeWithSurrogateStartIsEnabled())
		{
			TD.DCSerializeWithSurrogateStart(surrogatedType.FullName);
		}
		declaredTypeHandle = surrogatedType.TypeHandle;
		obj = DataContractSerializer.SurrogateToDataContractType(dataContractSurrogate, obj, surrogatedType, ref objType);
		handle = objType.TypeHandle;
		if (obj2 != obj)
		{
			oldObjId = base.SerializedObjects.ReassignId(0, obj2, obj);
		}
		if (writeXsiType)
		{
			surrogatedType = Globals.TypeOfObject;
			SerializeWithXsiType(xmlWriter, obj, handle, objType, -1, surrogatedType.TypeHandle, surrogatedType);
		}
		else if (declaredTypeHandle.Equals(handle))
		{
			DataContract dataContract = GetDataContract(handle, objType);
			SerializeWithoutXsiType(dataContract, xmlWriter, obj, declaredTypeHandle);
		}
		else
		{
			SerializeWithXsiType(xmlWriter, obj, handle, objType, -1, declaredTypeHandle, surrogatedType);
		}
		if (obj2 != obj)
		{
			base.SerializedObjects.ReassignId(oldObjId, obj, obj2);
		}
		if (TD.DCSerializeWithSurrogateStopIsEnabled())
		{
			TD.DCSerializeWithSurrogateStop();
		}
	}

	internal override void WriteArraySize(XmlWriterDelegator xmlWriter, int size)
	{
		if (preserveObjectReferences && size > -1)
		{
			xmlWriter.WriteAttributeInt("z", DictionaryGlobals.ArraySizeLocalName, DictionaryGlobals.SerializationNamespace, size);
		}
	}
}
