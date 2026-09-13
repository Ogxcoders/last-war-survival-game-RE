using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.Serialization;

internal sealed class SurrogateDataContract : DataContract
{
	private class SurrogateDataContractCriticalHelper : DataContractCriticalHelper
	{
		private ISerializationSurrogate serializationSurrogate;

		internal ISerializationSurrogate SerializationSurrogate => serializationSurrogate;

		internal SurrogateDataContractCriticalHelper(Type type, ISerializationSurrogate serializationSurrogate)
			: base(type)
		{
			this.serializationSurrogate = serializationSurrogate;
			DataContract.GetDefaultStableName(DataContract.GetClrTypeFullName(type), out var localName, out var text);
			SetDataContractName(DataContract.CreateQualifiedName(localName, text));
		}
	}

	[SecurityCritical]
	private SurrogateDataContractCriticalHelper helper;

	internal ISerializationSurrogate SerializationSurrogate
	{
		[SecuritySafeCritical]
		get
		{
			return helper.SerializationSurrogate;
		}
	}

	[SecuritySafeCritical]
	internal SurrogateDataContract(Type type, ISerializationSurrogate serializationSurrogate)
		: base(new SurrogateDataContractCriticalHelper(type, serializationSurrogate))
	{
		helper = base.Helper as SurrogateDataContractCriticalHelper;
	}

	public override void WriteXmlValue(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context)
	{
		SerializationInfo serInfo = new SerializationInfo(base.UnderlyingType, XmlObjectSerializer.FormatterConverter, !context.UnsafeTypeForwardingEnabled);
		SerializationSurrogateGetObjectData(obj, serInfo, context.GetStreamingContext());
		context.WriteSerializationInfo(xmlWriter, base.UnderlyingType, serInfo);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SecuritySafeCritical]
	[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
	private object SerializationSurrogateSetObjectData(object obj, SerializationInfo serInfo, StreamingContext context)
	{
		return SerializationSurrogate.SetObjectData(obj, serInfo, context, null);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SecuritySafeCritical]
	[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
	internal static object GetRealObject(IObjectReference obj, StreamingContext context)
	{
		return obj.GetRealObject(context);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SecuritySafeCritical]
	[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
	private object GetUninitializedObject(Type objType)
	{
		return FormatterServices.GetUninitializedObject(objType);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SecuritySafeCritical]
	[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
	private void SerializationSurrogateGetObjectData(object obj, SerializationInfo serInfo, StreamingContext context)
	{
		SerializationSurrogate.GetObjectData(obj, serInfo, context);
	}

	public override object ReadXmlValue(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context)
	{
		xmlReader.Read();
		Type underlyingType = base.UnderlyingType;
		object obj = (underlyingType.IsArray ? Array.CreateInstance(underlyingType.GetElementType(), 0) : GetUninitializedObject(underlyingType));
		context.AddNewObject(obj);
		string objectId = context.GetObjectId();
		SerializationInfo serInfo = context.ReadSerializationInfo(xmlReader, underlyingType);
		object obj2 = SerializationSurrogateSetObjectData(obj, serInfo, context.GetStreamingContext());
		if (obj2 == null)
		{
			obj2 = obj;
		}
		if (obj2 is IDeserializationCallback)
		{
			((IDeserializationCallback)obj2).OnDeserialization(null);
		}
		if (obj2 is IObjectReference)
		{
			obj2 = GetRealObject((IObjectReference)obj2, context.GetStreamingContext());
		}
		context.ReplaceDeserializedObject(objectId, obj, obj2);
		xmlReader.ReadEndElement();
		return obj2;
	}
}
