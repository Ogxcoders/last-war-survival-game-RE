using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary;

[ComVisible(true)]
public sealed class BinaryFormatter : IRemotingFormatter, IFormatter
{
	internal ISurrogateSelector m_surrogates;

	internal StreamingContext m_context;

	internal SerializationBinder m_binder;

	internal FormatterTypeStyle m_typeFormat = FormatterTypeStyle.TypesAlways;

	internal FormatterAssemblyStyle m_assemblyFormat;

	internal TypeFilterLevel m_securityLevel = TypeFilterLevel.Full;

	internal object[] m_crossAppDomainArray;

	private static Dictionary<Type, TypeInformation> typeNameCache = new Dictionary<Type, TypeInformation>();

	public FormatterTypeStyle TypeFormat
	{
		get
		{
			return m_typeFormat;
		}
		set
		{
			m_typeFormat = value;
		}
	}

	public FormatterAssemblyStyle AssemblyFormat
	{
		get
		{
			return m_assemblyFormat;
		}
		set
		{
			m_assemblyFormat = value;
		}
	}

	public TypeFilterLevel FilterLevel
	{
		get
		{
			return m_securityLevel;
		}
		set
		{
			m_securityLevel = value;
		}
	}

	public ISurrogateSelector SurrogateSelector
	{
		get
		{
			return m_surrogates;
		}
		set
		{
			m_surrogates = value;
		}
	}

	public SerializationBinder Binder
	{
		get
		{
			return m_binder;
		}
		set
		{
			m_binder = value;
		}
	}

	public StreamingContext Context
	{
		get
		{
			return m_context;
		}
		set
		{
			m_context = value;
		}
	}

	public BinaryFormatter()
	{
		m_surrogates = null;
		m_context = new StreamingContext(StreamingContextStates.All);
	}

	public BinaryFormatter(ISurrogateSelector selector, StreamingContext context)
	{
		m_surrogates = selector;
		m_context = context;
	}

	public object Deserialize(Stream serializationStream)
	{
		return Deserialize(serializationStream, null);
	}

	[SecurityCritical]
	internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck)
	{
		return Deserialize(serializationStream, handler, fCheck, null);
	}

	[SecuritySafeCritical]
	public object Deserialize(Stream serializationStream, HeaderHandler handler)
	{
		return Deserialize(serializationStream, handler, fCheck: true);
	}

	[SecuritySafeCritical]
	public object DeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage)
	{
		return Deserialize(serializationStream, handler, fCheck: true, methodCallMessage);
	}

	[SecurityCritical]
	[ComVisible(false)]
	public object UnsafeDeserialize(Stream serializationStream, HeaderHandler handler)
	{
		return Deserialize(serializationStream, handler, fCheck: false);
	}

	[SecurityCritical]
	[ComVisible(false)]
	public object UnsafeDeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage)
	{
		return Deserialize(serializationStream, handler, fCheck: false, methodCallMessage);
	}

	[SecurityCritical]
	internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck, IMethodCallMessage methodCallMessage)
	{
		return Deserialize(serializationStream, handler, fCheck, isCrossAppDomain: false, methodCallMessage);
	}

	[SecurityCritical]
	internal object Deserialize(Stream serializationStream, HeaderHandler handler, bool fCheck, bool isCrossAppDomain, IMethodCallMessage methodCallMessage)
	{
		if (serializationStream == null)
		{
			throw new ArgumentNullException("serializationStream", Environment.GetResourceString("Parameter '{0}' cannot be null.", serializationStream));
		}
		if (serializationStream.CanSeek && serializationStream.Length == 0L)
		{
			throw new SerializationException(Environment.GetResourceString("Attempting to deserialize an empty stream."));
		}
		InternalFE internalFE = new InternalFE();
		internalFE.FEtypeFormat = m_typeFormat;
		internalFE.FEserializerTypeEnum = InternalSerializerTypeE.Binary;
		internalFE.FEassemblyFormat = m_assemblyFormat;
		internalFE.FEsecurityLevel = m_securityLevel;
		ObjectReader objectReader = new ObjectReader(serializationStream, m_surrogates, m_context, internalFE, m_binder);
		objectReader.crossAppDomainArray = m_crossAppDomainArray;
		return objectReader.Deserialize(handler, new __BinaryParser(serializationStream, objectReader), fCheck, isCrossAppDomain, methodCallMessage);
	}

	public void Serialize(Stream serializationStream, object graph)
	{
		Serialize(serializationStream, graph, null);
	}

	[SecuritySafeCritical]
	public void Serialize(Stream serializationStream, object graph, Header[] headers)
	{
		Serialize(serializationStream, graph, headers, fCheck: true);
	}

	[SecurityCritical]
	internal void Serialize(Stream serializationStream, object graph, Header[] headers, bool fCheck)
	{
		if (serializationStream == null)
		{
			throw new ArgumentNullException("serializationStream", Environment.GetResourceString("Parameter '{0}' cannot be null.", serializationStream));
		}
		InternalFE internalFE = new InternalFE();
		internalFE.FEtypeFormat = m_typeFormat;
		internalFE.FEserializerTypeEnum = InternalSerializerTypeE.Binary;
		internalFE.FEassemblyFormat = m_assemblyFormat;
		ObjectWriter objectWriter = new ObjectWriter(m_surrogates, m_context, internalFE, m_binder);
		__BinaryWriter serWriter = new __BinaryWriter(serializationStream, objectWriter, m_typeFormat);
		objectWriter.Serialize(graph, headers, serWriter, fCheck);
		m_crossAppDomainArray = objectWriter.crossAppDomainArray;
	}

	internal static TypeInformation GetTypeInformation(Type type)
	{
		lock (typeNameCache)
		{
			TypeInformation value = null;
			if (!typeNameCache.TryGetValue(type, out value))
			{
				bool hasTypeForwardedFrom;
				string clrAssemblyName = FormatterServices.GetClrAssemblyName(type, out hasTypeForwardedFrom);
				value = new TypeInformation(FormatterServices.GetClrTypeFullName(type), clrAssemblyName, hasTypeForwardedFrom);
				typeNameCache.Add(type, value);
			}
			return value;
		}
	}
}
