using System.Collections;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Security;

namespace System.Runtime.Serialization;

internal static class ObjectCloneHelper
{
	private static readonly IFormatterConverter s_converter = new FormatterConverter();

	private static readonly StreamingContext s_cloneContext = new StreamingContext(StreamingContextStates.CrossAppDomain);

	private static readonly ISerializationSurrogate s_RemotingSurrogate = new RemotingSurrogate();

	private static readonly ISerializationSurrogate s_ObjRefRemotingSurrogate = new ObjRefSurrogate();

	[SecurityCritical]
	internal static object GetObjectData(object serObj, out string typeName, out string assemName, out string[] fieldNames, out object[] fieldValues)
	{
		Type type = null;
		object obj = null;
		type = ((!RemotingServices.IsTransparentProxy(serObj)) ? serObj.GetType() : typeof(MarshalByRefObject));
		SerializationInfo serializationInfo = new SerializationInfo(type, s_converter);
		if (serObj is ObjRef)
		{
			s_ObjRefRemotingSurrogate.GetObjectData(serObj, serializationInfo, s_cloneContext);
		}
		else if (RemotingServices.IsTransparentProxy(serObj) || serObj is MarshalByRefObject)
		{
			if (obj == null)
			{
				s_RemotingSurrogate.GetObjectData(serObj, serializationInfo, s_cloneContext);
			}
		}
		else
		{
			if (!(serObj is ISerializable))
			{
				throw new ArgumentException(Environment.GetResourceString("Serialization error."));
			}
			((ISerializable)serObj).GetObjectData(serializationInfo, s_cloneContext);
		}
		if (obj == null)
		{
			typeName = serializationInfo.FullTypeName;
			assemName = serializationInfo.AssemblyName;
			fieldNames = serializationInfo.MemberNames;
			fieldValues = serializationInfo.MemberValues;
		}
		else
		{
			typeName = null;
			assemName = null;
			fieldNames = null;
			fieldValues = null;
		}
		return obj;
	}

	[SecurityCritical]
	internal static SerializationInfo PrepareConstructorArgs(object serObj, string[] fieldNames, object[] fieldValues, out StreamingContext context)
	{
		SerializationInfo serializationInfo = null;
		if (serObj is ISerializable)
		{
			serializationInfo = new SerializationInfo(serObj.GetType(), s_converter);
			for (int i = 0; i < fieldNames.Length; i++)
			{
				if (fieldNames[i] != null)
				{
					serializationInfo.AddValue(fieldNames[i], fieldValues[i]);
				}
			}
		}
		else
		{
			Hashtable hashtable = new Hashtable();
			int j = 0;
			int num = 0;
			for (; j < fieldNames.Length; j++)
			{
				if (fieldNames[j] != null)
				{
					hashtable[fieldNames[j]] = fieldValues[j];
					num++;
				}
			}
			MemberInfo[] serializableMembers = FormatterServices.GetSerializableMembers(serObj.GetType());
			for (int k = 0; k < serializableMembers.Length; k++)
			{
				string name = serializableMembers[k].Name;
				if (!hashtable.Contains(name))
				{
					object[] customAttributes = serializableMembers[k].GetCustomAttributes(typeof(OptionalFieldAttribute), inherit: false);
					if (customAttributes == null || customAttributes.Length == 0)
					{
						throw new SerializationException(Environment.GetResourceString("Member '{0}' in class '{1}' is not present in the serialized stream and is not marked with {2}.", serializableMembers[k], serObj.GetType(), typeof(OptionalFieldAttribute).FullName));
					}
				}
				else
				{
					object value = hashtable[name];
					FormatterServices.SerializationSetValue(serializableMembers[k], serObj, value);
				}
			}
		}
		context = s_cloneContext;
		return serializationInfo;
	}
}
