using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Diagnostics;
using System.Runtime.Serialization.Diagnostics;
using System.Security;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization;

public abstract class XmlObjectSerializer
{
	[SecurityCritical]
	private static IFormatterConverter formatterConverter;

	internal virtual Dictionary<XmlQualifiedName, DataContract> KnownDataContracts => null;

	internal static IFormatterConverter FormatterConverter
	{
		[SecuritySafeCritical]
		get
		{
			if (formatterConverter == null)
			{
				formatterConverter = new FormatterConverter();
			}
			return formatterConverter;
		}
	}

	public abstract void WriteStartObject(XmlDictionaryWriter writer, object graph);

	public abstract void WriteObjectContent(XmlDictionaryWriter writer, object graph);

	public abstract void WriteEndObject(XmlDictionaryWriter writer);

	public virtual void WriteObject(Stream stream, object graph)
	{
		CheckNull(stream, "stream");
		XmlDictionaryWriter xmlDictionaryWriter = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8, ownsStream: false);
		WriteObject(xmlDictionaryWriter, graph);
		xmlDictionaryWriter.Flush();
	}

	public virtual void WriteObject(XmlWriter writer, object graph)
	{
		CheckNull(writer, "writer");
		WriteObject(XmlDictionaryWriter.CreateDictionaryWriter(writer), graph);
	}

	public virtual void WriteStartObject(XmlWriter writer, object graph)
	{
		CheckNull(writer, "writer");
		WriteStartObject(XmlDictionaryWriter.CreateDictionaryWriter(writer), graph);
	}

	public virtual void WriteObjectContent(XmlWriter writer, object graph)
	{
		CheckNull(writer, "writer");
		WriteObjectContent(XmlDictionaryWriter.CreateDictionaryWriter(writer), graph);
	}

	public virtual void WriteEndObject(XmlWriter writer)
	{
		CheckNull(writer, "writer");
		WriteEndObject(XmlDictionaryWriter.CreateDictionaryWriter(writer));
	}

	public virtual void WriteObject(XmlDictionaryWriter writer, object graph)
	{
		WriteObjectHandleExceptions(new XmlWriterDelegator(writer), graph);
	}

	internal void WriteObjectHandleExceptions(XmlWriterDelegator writer, object graph)
	{
		WriteObjectHandleExceptions(writer, graph, null);
	}

	internal void WriteObjectHandleExceptions(XmlWriterDelegator writer, object graph, DataContractResolver dataContractResolver)
	{
		try
		{
			CheckNull(writer, "writer");
			if (DiagnosticUtility.ShouldTraceInformation)
			{
				TraceUtility.Trace(TraceEventType.Information, 196609, SR.GetString("WriteObject begins"), new StringTraceRecord("Type", GetTypeInfo(GetSerializeType(graph))));
				InternalWriteObject(writer, graph, dataContractResolver);
				TraceUtility.Trace(TraceEventType.Information, 196610, SR.GetString("WriteObject ends"), new StringTraceRecord("Type", GetTypeInfo(GetSerializeType(graph))));
			}
			else
			{
				InternalWriteObject(writer, graph, dataContractResolver);
			}
		}
		catch (XmlException innerException)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateSerializationException(GetTypeInfoError("There was an error serializing the object {0}. {1}", GetSerializeType(graph), innerException), innerException));
		}
		catch (FormatException innerException2)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateSerializationException(GetTypeInfoError("There was an error serializing the object {0}. {1}", GetSerializeType(graph), innerException2), innerException2));
		}
	}

	internal virtual void InternalWriteObject(XmlWriterDelegator writer, object graph)
	{
		WriteStartObject(writer.Writer, graph);
		WriteObjectContent(writer.Writer, graph);
		WriteEndObject(writer.Writer);
	}

	internal virtual void InternalWriteObject(XmlWriterDelegator writer, object graph, DataContractResolver dataContractResolver)
	{
		InternalWriteObject(writer, graph);
	}

	internal virtual void InternalWriteStartObject(XmlWriterDelegator writer, object graph)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
	}

	internal virtual void InternalWriteObjectContent(XmlWriterDelegator writer, object graph)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
	}

	internal virtual void InternalWriteEndObject(XmlWriterDelegator writer)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
	}

	internal void WriteStartObjectHandleExceptions(XmlWriterDelegator writer, object graph)
	{
		try
		{
			CheckNull(writer, "writer");
			InternalWriteStartObject(writer, graph);
		}
		catch (XmlException innerException)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateSerializationException(GetTypeInfoError("There was an error writing start element of object {0}. {1}", GetSerializeType(graph), innerException), innerException));
		}
		catch (FormatException innerException2)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateSerializationException(GetTypeInfoError("There was an error writing start element of object {0}. {1}", GetSerializeType(graph), innerException2), innerException2));
		}
	}

	internal void WriteObjectContentHandleExceptions(XmlWriterDelegator writer, object graph)
	{
		try
		{
			CheckNull(writer, "writer");
			if (DiagnosticUtility.ShouldTraceInformation)
			{
				TraceUtility.Trace(TraceEventType.Information, 196611, SR.GetString("WriteObjectContent begins"), new StringTraceRecord("Type", GetTypeInfo(GetSerializeType(graph))));
				if (writer.WriteState != WriteState.Element)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateSerializationException(SR.GetString("WriteState '{0}' not valid. Caller must write start element before serializing in contentOnly mode.", writer.WriteState)));
				}
				InternalWriteObjectContent(writer, graph);
				TraceUtility.Trace(TraceEventType.Information, 196612, SR.GetString("WriteObjectContent ends"), new StringTraceRecord("Type", GetTypeInfo(GetSerializeType(graph))));
			}
			else
			{
				if (writer.WriteState != WriteState.Element)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateSerializationException(SR.GetString("WriteState '{0}' not valid. Caller must write start element before serializing in contentOnly mode.", writer.WriteState)));
				}
				InternalWriteObjectContent(writer, graph);
			}
		}
		catch (XmlException innerException)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateSerializationException(GetTypeInfoError("There was an error serializing the object {0}. {1}", GetSerializeType(graph), innerException), innerException));
		}
		catch (FormatException innerException2)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateSerializationException(GetTypeInfoError("There was an error serializing the object {0}. {1}", GetSerializeType(graph), innerException2), innerException2));
		}
	}

	internal void WriteEndObjectHandleExceptions(XmlWriterDelegator writer)
	{
		try
		{
			CheckNull(writer, "writer");
			InternalWriteEndObject(writer);
		}
		catch (XmlException innerException)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateSerializationException(GetTypeInfoError("There was an error writing end element of object {0}. {1}", null, innerException), innerException));
		}
		catch (FormatException innerException2)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateSerializationException(GetTypeInfoError("There was an error writing end element of object {0}. {1}", null, innerException2), innerException2));
		}
	}

	internal void WriteRootElement(XmlWriterDelegator writer, DataContract contract, XmlDictionaryString name, XmlDictionaryString ns, bool needsContractNsAtRoot)
	{
		if (name == null)
		{
			if (contract.HasRoot)
			{
				contract.WriteRootElement(writer, contract.TopLevelElementName, contract.TopLevelElementNamespace);
			}
			return;
		}
		contract.WriteRootElement(writer, name, ns);
		if (needsContractNsAtRoot)
		{
			writer.WriteNamespaceDecl(contract.Namespace);
		}
	}

	internal bool CheckIfNeedsContractNsAtRoot(XmlDictionaryString name, XmlDictionaryString ns, DataContract contract)
	{
		if (name == null)
		{
			return false;
		}
		if (contract.IsBuiltInDataContract || !contract.CanContainReferences || contract.IsISerializable)
		{
			return false;
		}
		string text = XmlDictionaryString.GetString(contract.Namespace);
		if (string.IsNullOrEmpty(text) || text == XmlDictionaryString.GetString(ns))
		{
			return false;
		}
		return true;
	}

	internal static void WriteNull(XmlWriterDelegator writer)
	{
		writer.WriteAttributeBool("i", DictionaryGlobals.XsiNilLocalName, DictionaryGlobals.SchemaInstanceNamespace, value: true);
	}

	internal static bool IsContractDeclared(DataContract contract, DataContract declaredContract)
	{
		if (contract.Name != declaredContract.Name || contract.Namespace != declaredContract.Namespace)
		{
			if (contract.Name.Value == declaredContract.Name.Value)
			{
				return contract.Namespace.Value == declaredContract.Namespace.Value;
			}
			return false;
		}
		return true;
	}

	public virtual object ReadObject(Stream stream)
	{
		CheckNull(stream, "stream");
		return ReadObject(XmlDictionaryReader.CreateTextReader(stream, XmlDictionaryReaderQuotas.Max));
	}

	public virtual object ReadObject(XmlReader reader)
	{
		CheckNull(reader, "reader");
		return ReadObject(XmlDictionaryReader.CreateDictionaryReader(reader));
	}

	public virtual object ReadObject(XmlDictionaryReader reader)
	{
		return ReadObjectHandleExceptions(new XmlReaderDelegator(reader), verifyObjectName: true);
	}

	public virtual object ReadObject(XmlReader reader, bool verifyObjectName)
	{
		CheckNull(reader, "reader");
		return ReadObject(XmlDictionaryReader.CreateDictionaryReader(reader), verifyObjectName);
	}

	public abstract object ReadObject(XmlDictionaryReader reader, bool verifyObjectName);

	public virtual bool IsStartObject(XmlReader reader)
	{
		CheckNull(reader, "reader");
		return IsStartObject(XmlDictionaryReader.CreateDictionaryReader(reader));
	}

	public abstract bool IsStartObject(XmlDictionaryReader reader);

	internal virtual object InternalReadObject(XmlReaderDelegator reader, bool verifyObjectName)
	{
		return ReadObject(reader.UnderlyingReader, verifyObjectName);
	}

	internal virtual object InternalReadObject(XmlReaderDelegator reader, bool verifyObjectName, DataContractResolver dataContractResolver)
	{
		return InternalReadObject(reader, verifyObjectName);
	}

	internal virtual bool InternalIsStartObject(XmlReaderDelegator reader)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException());
	}

	internal object ReadObjectHandleExceptions(XmlReaderDelegator reader, bool verifyObjectName)
	{
		return ReadObjectHandleExceptions(reader, verifyObjectName, null);
	}

	internal object ReadObjectHandleExceptions(XmlReaderDelegator reader, bool verifyObjectName, DataContractResolver dataContractResolver)
	{
		try
		{
			CheckNull(reader, "reader");
			if (DiagnosticUtility.ShouldTraceInformation)
			{
				TraceUtility.Trace(TraceEventType.Information, 196613, SR.GetString("ReadObject begins"), new StringTraceRecord("Type", GetTypeInfo(GetDeserializeType())));
				object result = InternalReadObject(reader, verifyObjectName, dataContractResolver);
				TraceUtility.Trace(TraceEventType.Information, 196614, SR.GetString("ReadObject ends"), new StringTraceRecord("Type", GetTypeInfo(GetDeserializeType())));
				return result;
			}
			return InternalReadObject(reader, verifyObjectName, dataContractResolver);
		}
		catch (XmlException innerException)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateSerializationException(GetTypeInfoError("There was an error deserializing the object {0}. {1}", GetDeserializeType(), innerException), innerException));
		}
		catch (FormatException innerException2)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateSerializationException(GetTypeInfoError("There was an error deserializing the object {0}. {1}", GetDeserializeType(), innerException2), innerException2));
		}
	}

	internal bool IsStartObjectHandleExceptions(XmlReaderDelegator reader)
	{
		try
		{
			CheckNull(reader, "reader");
			return InternalIsStartObject(reader);
		}
		catch (XmlException innerException)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateSerializationException(GetTypeInfoError("There was an error checking start element of object {0}. {1}", GetDeserializeType(), innerException), innerException));
		}
		catch (FormatException innerException2)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(CreateSerializationException(GetTypeInfoError("There was an error checking start element of object {0}. {1}", GetDeserializeType(), innerException2), innerException2));
		}
	}

	internal bool IsRootXmlAny(XmlDictionaryString rootName, DataContract contract)
	{
		if (rootName == null)
		{
			return !contract.HasRoot;
		}
		return false;
	}

	internal bool IsStartElement(XmlReaderDelegator reader)
	{
		if (!reader.MoveToElement())
		{
			return reader.IsStartElement();
		}
		return true;
	}

	internal bool IsRootElement(XmlReaderDelegator reader, DataContract contract, XmlDictionaryString name, XmlDictionaryString ns)
	{
		reader.MoveToElement();
		if (name != null)
		{
			return reader.IsStartElement(name, ns);
		}
		if (!contract.HasRoot)
		{
			return reader.IsStartElement();
		}
		if (reader.IsStartElement(contract.TopLevelElementName, contract.TopLevelElementNamespace))
		{
			return true;
		}
		ClassDataContract classDataContract = contract as ClassDataContract;
		if (classDataContract != null)
		{
			classDataContract = classDataContract.BaseContract;
		}
		while (classDataContract != null)
		{
			if (reader.IsStartElement(classDataContract.TopLevelElementName, classDataContract.TopLevelElementNamespace))
			{
				return true;
			}
			classDataContract = classDataContract.BaseContract;
		}
		if (classDataContract == null)
		{
			DataContract primitiveDataContract = PrimitiveDataContract.GetPrimitiveDataContract(Globals.TypeOfObject);
			if (reader.IsStartElement(primitiveDataContract.TopLevelElementName, primitiveDataContract.TopLevelElementNamespace))
			{
				return true;
			}
		}
		return false;
	}

	internal static void CheckNull(object obj, string name)
	{
		if (obj == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException(name));
		}
	}

	internal static string TryAddLineInfo(XmlReaderDelegator reader, string errorMessage)
	{
		if (reader.HasLineInfo())
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} {1}", SR.GetString("Error in line {0} position {1}.", reader.LineNumber, reader.LinePosition), errorMessage);
		}
		return errorMessage;
	}

	internal static Exception CreateSerializationExceptionWithReaderDetails(string errorMessage, XmlReaderDelegator reader)
	{
		return CreateSerializationException(TryAddLineInfo(reader, SR.GetString("{0}. Encountered '{1}'  with name '{2}', namespace '{3}'.", errorMessage, reader.NodeType, reader.LocalName, reader.NamespaceURI)));
	}

	internal static SerializationException CreateSerializationException(string errorMessage)
	{
		return CreateSerializationException(errorMessage, null);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static SerializationException CreateSerializationException(string errorMessage, Exception innerException)
	{
		return new SerializationException(errorMessage, innerException);
	}

	private static string GetTypeInfo(Type type)
	{
		if (!(type == null))
		{
			return DataContract.GetClrTypeFullName(type);
		}
		return string.Empty;
	}

	private static string GetTypeInfoError(string errorMessage, Type type, Exception innerException)
	{
		string text = ((type == null) ? string.Empty : SR.GetString("of type {0}", DataContract.GetClrTypeFullName(type)));
		string text2 = ((innerException == null) ? string.Empty : innerException.Message);
		return SR.GetString(errorMessage, text, text2);
	}

	internal virtual Type GetSerializeType(object graph)
	{
		return graph?.GetType();
	}

	internal virtual Type GetDeserializeType()
	{
		return null;
	}
}
