using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Runtime.Serialization.Formatters;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.Serialization;

internal class XmlObjectSerializerReadContextComplex : XmlObjectSerializerReadContext
{
	private sealed class TopLevelAssemblyTypeResolver
	{
		private Assembly topLevelAssembly;

		public TopLevelAssemblyTypeResolver(Assembly topLevelAssembly)
		{
			this.topLevelAssembly = topLevelAssembly;
		}

		public Type ResolveType(Assembly assembly, string simpleTypeName, bool ignoreCase)
		{
			if (assembly == null)
			{
				assembly = topLevelAssembly;
			}
			return assembly.GetType(simpleTypeName, throwOnError: false, ignoreCase);
		}
	}

	private class XmlObjectDataContractTypeInfo
	{
		private Assembly assembly;

		private Type type;

		public Assembly Assembly => assembly;

		public Type Type => type;

		public XmlObjectDataContractTypeInfo(Assembly assembly, Type type)
		{
			this.assembly = assembly;
			this.type = type;
		}
	}

	private class XmlObjectDataContractTypeKey
	{
		private string assemblyName;

		private string typeName;

		public XmlObjectDataContractTypeKey(string assemblyName, string typeName)
		{
			this.assemblyName = assemblyName;
			this.typeName = typeName;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (!(obj is XmlObjectDataContractTypeKey xmlObjectDataContractTypeKey))
			{
				return false;
			}
			if (assemblyName != xmlObjectDataContractTypeKey.assemblyName)
			{
				return false;
			}
			if (typeName != xmlObjectDataContractTypeKey.typeName)
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			int num = 0;
			if (assemblyName != null)
			{
				num = assemblyName.GetHashCode();
			}
			if (typeName != null)
			{
				num ^= typeName.GetHashCode();
			}
			return num;
		}
	}

	private static Hashtable dataContractTypeCache = new Hashtable();

	private bool preserveObjectReferences;

	protected IDataContractSurrogate dataContractSurrogate;

	private SerializationMode mode;

	private SerializationBinder binder;

	private ISurrogateSelector surrogateSelector;

	private FormatterAssemblyStyle assemblyFormat;

	private Hashtable surrogateDataContracts;

	internal override SerializationMode Mode => mode;

	internal XmlObjectSerializerReadContextComplex(DataContractSerializer serializer, DataContract rootTypeDataContract, DataContractResolver dataContractResolver)
		: base(serializer, rootTypeDataContract, dataContractResolver)
	{
		mode = SerializationMode.SharedContract;
		preserveObjectReferences = serializer.PreserveObjectReferences;
		dataContractSurrogate = serializer.DataContractSurrogate;
	}

	internal XmlObjectSerializerReadContextComplex(NetDataContractSerializer serializer)
		: base(serializer)
	{
		mode = SerializationMode.SharedType;
		preserveObjectReferences = true;
		binder = serializer.Binder;
		surrogateSelector = serializer.SurrogateSelector;
		assemblyFormat = serializer.AssemblyFormat;
	}

	internal XmlObjectSerializerReadContextComplex(XmlObjectSerializer serializer, int maxItemsInObjectGraph, StreamingContext streamingContext, bool ignoreExtensionDataObject)
		: base(serializer, maxItemsInObjectGraph, streamingContext, ignoreExtensionDataObject)
	{
	}

	internal override DataContract GetDataContract(int id, RuntimeTypeHandle typeHandle)
	{
		DataContract dataContract = null;
		if (mode == SerializationMode.SharedType && surrogateSelector != null)
		{
			dataContract = NetDataContractSerializer.GetDataContractFromSurrogateSelector(surrogateSelector, GetStreamingContext(), typeHandle, null, ref surrogateDataContracts);
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

	internal override DataContract GetDataContract(RuntimeTypeHandle typeHandle, Type type)
	{
		DataContract dataContract = null;
		if (mode == SerializationMode.SharedType && surrogateSelector != null)
		{
			dataContract = NetDataContractSerializer.GetDataContractFromSurrogateSelector(surrogateSelector, GetStreamingContext(), typeHandle, type, ref surrogateDataContracts);
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

	public override object InternalDeserialize(XmlReaderDelegator xmlReader, int declaredTypeID, RuntimeTypeHandle declaredTypeHandle, string name, string ns)
	{
		if (mode == SerializationMode.SharedContract)
		{
			if (dataContractSurrogate == null)
			{
				return base.InternalDeserialize(xmlReader, declaredTypeID, declaredTypeHandle, name, ns);
			}
			return InternalDeserializeWithSurrogate(xmlReader, Type.GetTypeFromHandle(declaredTypeHandle), null, name, ns);
		}
		return InternalDeserializeInSharedTypeMode(xmlReader, declaredTypeID, Type.GetTypeFromHandle(declaredTypeHandle), name, ns);
	}

	internal override object InternalDeserialize(XmlReaderDelegator xmlReader, Type declaredType, string name, string ns)
	{
		if (mode == SerializationMode.SharedContract)
		{
			if (dataContractSurrogate == null)
			{
				return base.InternalDeserialize(xmlReader, declaredType, name, ns);
			}
			return InternalDeserializeWithSurrogate(xmlReader, declaredType, null, name, ns);
		}
		return InternalDeserializeInSharedTypeMode(xmlReader, -1, declaredType, name, ns);
	}

	internal override object InternalDeserialize(XmlReaderDelegator xmlReader, Type declaredType, DataContract dataContract, string name, string ns)
	{
		if (mode == SerializationMode.SharedContract)
		{
			if (dataContractSurrogate == null)
			{
				return base.InternalDeserialize(xmlReader, declaredType, dataContract, name, ns);
			}
			return InternalDeserializeWithSurrogate(xmlReader, declaredType, dataContract, name, ns);
		}
		return InternalDeserializeInSharedTypeMode(xmlReader, -1, declaredType, name, ns);
	}

	private object InternalDeserializeInSharedTypeMode(XmlReaderDelegator xmlReader, int declaredTypeID, Type declaredType, string name, string ns)
	{
		object retObj = null;
		if (TryHandleNullOrRef(xmlReader, declaredType, name, ns, ref retObj))
		{
			return retObj;
		}
		string clrAssembly = attributes.ClrAssembly;
		string clrType = attributes.ClrType;
		DataContract dataContract;
		if (clrAssembly != null && clrType != null)
		{
			dataContract = ResolveDataContractInSharedTypeMode(clrAssembly, clrType, out var assembly, out var type);
			if (dataContract == null)
			{
				if (assembly == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Assembly '{0}' was not found.", clrAssembly)));
				}
				if (type == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("CLR type '{1}' in assembly '{0}' is not found.", assembly.FullName, clrType)));
				}
			}
			if (declaredType != null && declaredType.IsArray)
			{
				dataContract = ((declaredTypeID < 0) ? GetDataContract(declaredType) : GetDataContract(declaredTypeID, declaredType.TypeHandle));
			}
		}
		else
		{
			if (clrAssembly != null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.TryAddLineInfo(xmlReader, SR.GetString("Attribute was not found for CLR type '{1}' in namespace '{0}'. XML reader node is on {2}, '{4}' node in '{3}' namespace.", "http://schemas.microsoft.com/2003/10/Serialization/", "Type", xmlReader.NodeType, xmlReader.NamespaceURI, xmlReader.LocalName))));
			}
			if (clrType != null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.TryAddLineInfo(xmlReader, SR.GetString("Attribute was not found for CLR type '{1}' in namespace '{0}'. XML reader node is on {2}, '{4}' node in '{3}' namespace.", "http://schemas.microsoft.com/2003/10/Serialization/", "Assembly", xmlReader.NodeType, xmlReader.NamespaceURI, xmlReader.LocalName))));
			}
			if (declaredType == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(XmlObjectSerializer.TryAddLineInfo(xmlReader, SR.GetString("Attribute was not found for CLR type '{1}' in namespace '{0}'. XML reader node is on {2}, '{4}' node in '{3}' namespace.", "http://schemas.microsoft.com/2003/10/Serialization/", "Type", xmlReader.NodeType, xmlReader.NamespaceURI, xmlReader.LocalName))));
			}
			dataContract = ((declaredTypeID < 0) ? GetDataContract(declaredType) : GetDataContract(declaredTypeID, declaredType.TypeHandle));
		}
		return ReadDataContractValue(dataContract, xmlReader);
	}

	private object InternalDeserializeWithSurrogate(XmlReaderDelegator xmlReader, Type declaredType, DataContract surrogateDataContract, string name, string ns)
	{
		if (TD.DCDeserializeWithSurrogateStartIsEnabled())
		{
			TD.DCDeserializeWithSurrogateStart(declaredType.FullName);
		}
		DataContract dataContract = surrogateDataContract ?? GetDataContract(DataContractSurrogateCaller.GetDataContractType(dataContractSurrogate, declaredType));
		if (IsGetOnlyCollection && dataContract.UnderlyingType != declaredType)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Found on type '{0}'.", DataContract.GetClrTypeFullName(declaredType))));
		}
		ReadAttributes(xmlReader);
		string objectId = GetObjectId();
		object obj = InternalDeserialize(xmlReader, name, ns, declaredType, ref dataContract);
		object deserializedObject = DataContractSurrogateCaller.GetDeserializedObject(dataContractSurrogate, obj, dataContract.UnderlyingType, declaredType);
		ReplaceDeserializedObject(objectId, obj, deserializedObject);
		if (TD.DCDeserializeWithSurrogateStopIsEnabled())
		{
			TD.DCDeserializeWithSurrogateStop();
		}
		return deserializedObject;
	}

	private Type ResolveDataContractTypeInSharedTypeMode(string assemblyName, string typeName, out Assembly assembly)
	{
		assembly = null;
		Type type = null;
		if (binder != null)
		{
			type = binder.BindToType(assemblyName, typeName);
		}
		if (type == null)
		{
			XmlObjectDataContractTypeKey key = new XmlObjectDataContractTypeKey(assemblyName, typeName);
			XmlObjectDataContractTypeInfo xmlObjectDataContractTypeInfo = (XmlObjectDataContractTypeInfo)dataContractTypeCache[key];
			if (xmlObjectDataContractTypeInfo == null)
			{
				if (assemblyFormat == FormatterAssemblyStyle.Full)
				{
					if (assemblyName == "0")
					{
						assembly = Globals.TypeOfInt.Assembly;
					}
					else
					{
						assembly = Assembly.Load(assemblyName);
					}
					if (assembly != null)
					{
						type = assembly.GetType(typeName);
					}
				}
				else
				{
					assembly = ResolveSimpleAssemblyName(assemblyName);
					if (assembly != null)
					{
						try
						{
							type = assembly.GetType(typeName);
						}
						catch (TypeLoadException)
						{
						}
						catch (FileNotFoundException)
						{
						}
						catch (FileLoadException)
						{
						}
						catch (BadImageFormatException)
						{
						}
						if (type == null)
						{
							type = Type.GetType(typeName, ResolveSimpleAssemblyName, new TopLevelAssemblyTypeResolver(assembly).ResolveType, throwOnError: false);
						}
					}
				}
				if (type != null)
				{
					CheckTypeForwardedTo(assembly, type.Assembly, type);
					xmlObjectDataContractTypeInfo = new XmlObjectDataContractTypeInfo(assembly, type);
					lock (dataContractTypeCache)
					{
						if (!dataContractTypeCache.ContainsKey(key))
						{
							dataContractTypeCache[key] = xmlObjectDataContractTypeInfo;
						}
					}
				}
			}
			else
			{
				assembly = xmlObjectDataContractTypeInfo.Assembly;
				type = xmlObjectDataContractTypeInfo.Type;
			}
		}
		return type;
	}

	private DataContract ResolveDataContractInSharedTypeMode(string assemblyName, string typeName, out Assembly assembly, out Type type)
	{
		type = ResolveDataContractTypeInSharedTypeMode(assemblyName, typeName, out assembly);
		if (type != null)
		{
			return GetDataContract(type);
		}
		return null;
	}

	protected override DataContract ResolveDataContractFromTypeName()
	{
		if (mode == SerializationMode.SharedContract)
		{
			return base.ResolveDataContractFromTypeName();
		}
		Assembly assembly;
		Type type;
		if (attributes.ClrAssembly != null && attributes.ClrType != null)
		{
			return ResolveDataContractInSharedTypeMode(attributes.ClrAssembly, attributes.ClrType, out assembly, out type);
		}
		return null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SecuritySafeCritical]
	[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
	private bool CheckIfTypeSerializableForSharedTypeMode(Type memberType)
	{
		ISurrogateSelector selector;
		return surrogateSelector.GetSurrogate(memberType, GetStreamingContext(), out selector) != null;
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

	internal override int GetArraySize()
	{
		if (!preserveObjectReferences)
		{
			return -1;
		}
		return attributes.ArraySZSize;
	}

	private static Assembly ResolveSimpleAssemblyName(AssemblyName assemblyName)
	{
		return ResolveSimpleAssemblyName(assemblyName.FullName);
	}

	private static Assembly ResolveSimpleAssemblyName(string assemblyName)
	{
		Assembly assembly;
		if (assemblyName == "0")
		{
			assembly = Globals.TypeOfInt.Assembly;
		}
		else
		{
			assembly = Assembly.LoadWithPartialName(assemblyName);
			if (assembly == null)
			{
				assembly = Assembly.LoadWithPartialName(new AssemblyName(assemblyName)
				{
					Version = null
				}.FullName);
			}
		}
		return assembly;
	}

	[SecuritySafeCritical]
	private static void CheckTypeForwardedTo(Assembly sourceAssembly, Assembly destinationAssembly, Type resolvedType)
	{
		if (sourceAssembly != destinationAssembly && !NetDataContractSerializer.UnsafeTypeForwardingEnabled)
		{
			_ = sourceAssembly.IsFullyTrusted;
		}
	}
}
