using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Xml;

namespace System.Runtime.Serialization;

internal sealed class ClassDataContract : DataContract
{
	private class ClassDataContractCriticalHelper : DataContractCriticalHelper
	{
		internal struct Member
		{
			internal DataMember member;

			internal string ns;

			internal int baseTypeIndex;

			internal Member(DataMember member, string ns, int baseTypeIndex)
			{
				this.member = member;
				this.ns = ns;
				this.baseTypeIndex = baseTypeIndex;
			}
		}

		internal class DataMemberConflictComparer : IComparer<Member>
		{
			internal static DataMemberConflictComparer Singleton = new DataMemberConflictComparer();

			public int Compare(Member x, Member y)
			{
				int num = string.CompareOrdinal(x.ns, y.ns);
				if (num != 0)
				{
					return num;
				}
				int num2 = string.CompareOrdinal(x.member.Name, y.member.Name);
				if (num2 != 0)
				{
					return num2;
				}
				return x.baseTypeIndex - y.baseTypeIndex;
			}
		}

		private ClassDataContract baseContract;

		private List<DataMember> members;

		private MethodInfo onSerializing;

		private MethodInfo onSerialized;

		private MethodInfo onDeserializing;

		private MethodInfo onDeserialized;

		private MethodInfo extensionDataSetMethod;

		private Dictionary<XmlQualifiedName, DataContract> knownDataContracts;

		private string serializationExceptionMessage;

		private bool isISerializable;

		private bool isKnownTypeAttributeChecked;

		private bool isMethodChecked;

		private bool hasExtensionData;

		private bool isNonAttributedType;

		private bool hasDataContract;

		private XmlDictionaryString[] childElementNamespaces;

		private XmlFormatClassReaderDelegate xmlFormatReaderDelegate;

		private XmlFormatClassWriterDelegate xmlFormatWriterDelegate;

		public XmlDictionaryString[] ContractNamespaces;

		public XmlDictionaryString[] MemberNames;

		public XmlDictionaryString[] MemberNamespaces;

		private static Type[] serInfoCtorArgs;

		internal ClassDataContract BaseContract
		{
			get
			{
				return baseContract;
			}
			set
			{
				baseContract = value;
				if (baseContract != null && base.IsValueType)
				{
					ThrowInvalidDataContractException(SR.GetString("Data contract '{0}' from namespace '{1}' is a value type and cannot have base contract '{2}' from namespace '{3}'.", base.StableName.Name, base.StableName.Namespace, baseContract.StableName.Name, baseContract.StableName.Namespace));
				}
			}
		}

		internal List<DataMember> Members
		{
			get
			{
				return members;
			}
			set
			{
				members = value;
			}
		}

		internal MethodInfo OnSerializing
		{
			get
			{
				EnsureMethodsImported();
				return onSerializing;
			}
		}

		internal MethodInfo OnSerialized
		{
			get
			{
				EnsureMethodsImported();
				return onSerialized;
			}
		}

		internal MethodInfo OnDeserializing
		{
			get
			{
				EnsureMethodsImported();
				return onDeserializing;
			}
		}

		internal MethodInfo OnDeserialized
		{
			get
			{
				EnsureMethodsImported();
				return onDeserialized;
			}
		}

		internal MethodInfo ExtensionDataSetMethod
		{
			get
			{
				EnsureMethodsImported();
				return extensionDataSetMethod;
			}
		}

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

		internal string SerializationExceptionMessage => serializationExceptionMessage;

		internal string DeserializationExceptionMessage
		{
			get
			{
				if (serializationExceptionMessage == null)
				{
					return null;
				}
				return SR.GetString("Error on deserializing read-only members in the class: {0}", serializationExceptionMessage);
			}
		}

		internal override bool IsISerializable
		{
			get
			{
				return isISerializable;
			}
			set
			{
				isISerializable = value;
			}
		}

		internal bool HasDataContract => hasDataContract;

		internal bool HasExtensionData => hasExtensionData;

		internal bool IsNonAttributedType => isNonAttributedType;

		internal XmlFormatClassWriterDelegate XmlFormatWriterDelegate
		{
			get
			{
				return xmlFormatWriterDelegate;
			}
			set
			{
				xmlFormatWriterDelegate = value;
			}
		}

		internal XmlFormatClassReaderDelegate XmlFormatReaderDelegate
		{
			get
			{
				return xmlFormatReaderDelegate;
			}
			set
			{
				xmlFormatReaderDelegate = value;
			}
		}

		public XmlDictionaryString[] ChildElementNamespaces
		{
			get
			{
				return childElementNamespaces;
			}
			set
			{
				childElementNamespaces = value;
			}
		}

		private static Type[] SerInfoCtorArgs
		{
			get
			{
				if (serInfoCtorArgs == null)
				{
					serInfoCtorArgs = new Type[2]
					{
						typeof(SerializationInfo),
						typeof(StreamingContext)
					};
				}
				return serInfoCtorArgs;
			}
		}

		internal ClassDataContractCriticalHelper()
		{
		}

		internal ClassDataContractCriticalHelper(Type type)
			: base(type)
		{
			XmlQualifiedName stableNameAndSetHasDataContract = GetStableNameAndSetHasDataContract(type);
			if (type == Globals.TypeOfDBNull)
			{
				base.StableName = stableNameAndSetHasDataContract;
				members = new List<DataMember>();
				XmlDictionary xmlDictionary = new XmlDictionary(2);
				base.Name = xmlDictionary.Add(base.StableName.Name);
				base.Namespace = xmlDictionary.Add(base.StableName.Namespace);
				ContractNamespaces = (MemberNames = (MemberNamespaces = new XmlDictionaryString[0]));
				EnsureMethodsImported();
				return;
			}
			Type type2 = type.BaseType;
			isISerializable = Globals.TypeOfISerializable.IsAssignableFrom(type);
			SetIsNonAttributedType(type);
			if (isISerializable)
			{
				if (HasDataContract)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("ISerializable type '{0}' cannot have DataContract.", DataContract.GetClrTypeFullName(type))));
				}
				if (type2 != null && (!type2.IsSerializable || !Globals.TypeOfISerializable.IsAssignableFrom(type2)))
				{
					type2 = null;
				}
			}
			base.IsValueType = type.IsValueType;
			if (type2 != null && type2 != Globals.TypeOfObject && type2 != Globals.TypeOfValueType && type2 != Globals.TypeOfUri)
			{
				DataContract dataContract = DataContract.GetDataContract(type2);
				if (dataContract is CollectionDataContract)
				{
					BaseContract = ((CollectionDataContract)dataContract).SharedTypeContract as ClassDataContract;
				}
				else
				{
					BaseContract = dataContract as ClassDataContract;
				}
				if (BaseContract != null && BaseContract.IsNonAttributedType && !isNonAttributedType)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot inherit from a type that is not marked with DataContractAttribute or SerializableAttribute.  Consider marking the base type '{1}' with DataContractAttribute or SerializableAttribute, or removing them from the derived type.", DataContract.GetClrTypeFullName(type), DataContract.GetClrTypeFullName(type2))));
				}
			}
			else
			{
				BaseContract = null;
			}
			hasExtensionData = Globals.TypeOfIExtensibleDataObject.IsAssignableFrom(type);
			if (hasExtensionData && !HasDataContract && !IsNonAttributedType)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("On '{0}' type, only DataContract types can have extension data.", DataContract.GetClrTypeFullName(type))));
			}
			if (isISerializable)
			{
				SetDataContractName(stableNameAndSetHasDataContract);
			}
			else
			{
				base.StableName = stableNameAndSetHasDataContract;
				ImportDataMembers();
				XmlDictionary xmlDictionary2 = new XmlDictionary(2 + Members.Count);
				base.Name = xmlDictionary2.Add(base.StableName.Name);
				base.Namespace = xmlDictionary2.Add(base.StableName.Namespace);
				int num = 0;
				int num2 = 0;
				if (BaseContract == null)
				{
					MemberNames = new XmlDictionaryString[Members.Count];
					MemberNamespaces = new XmlDictionaryString[Members.Count];
					ContractNamespaces = new XmlDictionaryString[1];
				}
				else
				{
					if (BaseContract.IsReadOnlyContract)
					{
						serializationExceptionMessage = BaseContract.SerializationExceptionMessage;
					}
					num = BaseContract.MemberNames.Length;
					MemberNames = new XmlDictionaryString[Members.Count + num];
					Array.Copy(BaseContract.MemberNames, MemberNames, num);
					MemberNamespaces = new XmlDictionaryString[Members.Count + num];
					Array.Copy(BaseContract.MemberNamespaces, MemberNamespaces, num);
					num2 = BaseContract.ContractNamespaces.Length;
					ContractNamespaces = new XmlDictionaryString[1 + num2];
					Array.Copy(BaseContract.ContractNamespaces, ContractNamespaces, num2);
				}
				ContractNamespaces[num2] = base.Namespace;
				for (int i = 0; i < Members.Count; i++)
				{
					MemberNames[i + num] = xmlDictionary2.Add(Members[i].Name);
					MemberNamespaces[i + num] = base.Namespace;
				}
			}
			EnsureMethodsImported();
		}

		internal ClassDataContractCriticalHelper(Type type, XmlDictionaryString ns, string[] memberNames)
			: base(type)
		{
			base.StableName = new XmlQualifiedName(GetStableNameAndSetHasDataContract(type).Name, ns.Value);
			ImportDataMembers();
			XmlDictionary xmlDictionary = new XmlDictionary(1 + Members.Count);
			base.Name = xmlDictionary.Add(base.StableName.Name);
			base.Namespace = ns;
			ContractNamespaces = new XmlDictionaryString[1] { base.Namespace };
			MemberNames = new XmlDictionaryString[Members.Count];
			MemberNamespaces = new XmlDictionaryString[Members.Count];
			for (int i = 0; i < Members.Count; i++)
			{
				Members[i].Name = memberNames[i];
				MemberNames[i] = xmlDictionary.Add(Members[i].Name);
				MemberNamespaces[i] = base.Namespace;
			}
			EnsureMethodsImported();
		}

		private void EnsureIsReferenceImported(Type type)
		{
			bool flag = false;
			DataContractAttribute dataContractAttribute;
			bool flag2 = DataContract.TryGetDCAttribute(type, out dataContractAttribute);
			if (BaseContract != null)
			{
				if (flag2 && dataContractAttribute.IsReferenceSetExplicitly)
				{
					bool flag3 = BaseContract.IsReference;
					if ((flag3 && !dataContractAttribute.IsReference) || (!flag3 && dataContractAttribute.IsReference))
					{
						DataContract.ThrowInvalidDataContractException(SR.GetString("The IsReference setting for type '{0}' is '{1}', but the same setting for its parent class '{2}' is '{3}'. Derived types must have the same value for IsReference as the base type. Change the setting on type '{0}' to '{3}', or on type '{2}' to '{1}', or do not set IsReference explicitly.", DataContract.GetClrTypeFullName(type), dataContractAttribute.IsReference, DataContract.GetClrTypeFullName(BaseContract.UnderlyingType), BaseContract.IsReference), type);
					}
					else
					{
						flag = dataContractAttribute.IsReference;
					}
				}
				else
				{
					flag = BaseContract.IsReference;
				}
			}
			else if (flag2 && dataContractAttribute.IsReference)
			{
				flag = dataContractAttribute.IsReference;
			}
			if (flag && type.IsValueType)
			{
				DataContract.ThrowInvalidDataContractException(SR.GetString("Value type '{0}' cannot have the IsReference setting of '{1}'. Either change the setting to '{2}', or remove it completely.", DataContract.GetClrTypeFullName(type), true, false), type);
			}
			else
			{
				base.IsReference = flag;
			}
		}

		private void ImportDataMembers()
		{
			Type type = base.UnderlyingType;
			EnsureIsReferenceImported(type);
			List<DataMember> list = new List<DataMember>();
			Dictionary<string, DataMember> memberNamesTable = new Dictionary<string, DataMember>();
			MemberInfo[] array = ((!isNonAttributedType) ? type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public));
			foreach (MemberInfo memberInfo in array)
			{
				if (HasDataContract)
				{
					object[] customAttributes = memberInfo.GetCustomAttributes(typeof(DataMemberAttribute), inherit: false);
					if (customAttributes == null || customAttributes.Length == 0)
					{
						continue;
					}
					if (customAttributes.Length > 1)
					{
						ThrowInvalidDataContractException(SR.GetString("Member '{0}.{1}' has more than one DataMemberAttribute attribute.", DataContract.GetClrTypeFullName(memberInfo.DeclaringType), memberInfo.Name));
					}
					DataMember dataMember = new DataMember(memberInfo);
					if (memberInfo.MemberType == MemberTypes.Property)
					{
						PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
						MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);
						if (getMethod != null && IsMethodOverriding(getMethod))
						{
							continue;
						}
						MethodInfo setMethod = propertyInfo.GetSetMethod(nonPublic: true);
						if (setMethod != null && IsMethodOverriding(setMethod))
						{
							continue;
						}
						if (getMethod == null)
						{
							ThrowInvalidDataContractException(SR.GetString("No get method for property '{1}' in type '{0}'.", propertyInfo.DeclaringType, propertyInfo.Name));
						}
						if (setMethod == null && !SetIfGetOnlyCollection(dataMember, skipIfReadOnlyContract: false))
						{
							serializationExceptionMessage = SR.GetString("No set method for property '{1}' in type '{0}'.", propertyInfo.DeclaringType, propertyInfo.Name);
						}
						if (getMethod.GetParameters().Length != 0)
						{
							ThrowInvalidDataContractException(SR.GetString("Property '{1}' in type '{0}' cannot be serialized because serialization of indexed properties is not supported.", propertyInfo.DeclaringType, propertyInfo.Name));
						}
					}
					else if (memberInfo.MemberType != MemberTypes.Field)
					{
						ThrowInvalidDataContractException(SR.GetString("Member '{0}.{1}' cannot be serialized since it is neither a field nor a property, and therefore cannot be marked with the DataMemberAttribute attribute. Remove the DataMemberAttribute attribute from the '{1}' member.", DataContract.GetClrTypeFullName(type), memberInfo.Name));
					}
					DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)customAttributes[0];
					if (dataMemberAttribute.IsNameSetExplicitly)
					{
						if (dataMemberAttribute.Name == null || dataMemberAttribute.Name.Length == 0)
						{
							ThrowInvalidDataContractException(SR.GetString("Member '{0}' in type '{1}' cannot have DataMemberAttribute attribute Name set to null or empty string.", memberInfo.Name, DataContract.GetClrTypeFullName(type)));
						}
						dataMember.Name = dataMemberAttribute.Name;
					}
					else
					{
						dataMember.Name = memberInfo.Name;
					}
					dataMember.Name = DataContract.EncodeLocalName(dataMember.Name);
					dataMember.IsNullable = DataContract.IsTypeNullable(dataMember.MemberType);
					dataMember.IsRequired = dataMemberAttribute.IsRequired;
					if (dataMemberAttribute.IsRequired && base.IsReference)
					{
						DataContractCriticalHelper.ThrowInvalidDataContractException(SR.GetString("'{0}.{1}' has the IsRequired setting of '{2}. However, '{0}' has the IsReference setting of '{2}', because either it is set explicitly, or it is derived from a base class. Set IsRequired on '{0}.{1}' to false, or disable IsReference on '{0}'.", DataContract.GetClrTypeFullName(memberInfo.DeclaringType), memberInfo.Name, true), type);
					}
					dataMember.EmitDefaultValue = dataMemberAttribute.EmitDefaultValue;
					dataMember.Order = dataMemberAttribute.Order;
					CheckAndAddMember(list, dataMember, memberNamesTable);
					continue;
				}
				if (isNonAttributedType)
				{
					FieldInfo fieldInfo = memberInfo as FieldInfo;
					PropertyInfo propertyInfo2 = memberInfo as PropertyInfo;
					if ((fieldInfo == null && propertyInfo2 == null) || (fieldInfo != null && fieldInfo.IsInitOnly))
					{
						continue;
					}
					object[] customAttributes2 = memberInfo.GetCustomAttributes(typeof(IgnoreDataMemberAttribute), inherit: false);
					if (customAttributes2 != null && customAttributes2.Length != 0)
					{
						if (customAttributes2.Length <= 1)
						{
							continue;
						}
						ThrowInvalidDataContractException(SR.GetString("Member '{0}.{1}' has more than one IgnoreDataMemberAttribute attribute.", DataContract.GetClrTypeFullName(memberInfo.DeclaringType), memberInfo.Name));
					}
					DataMember dataMember2 = new DataMember(memberInfo);
					if (propertyInfo2 != null)
					{
						MethodInfo getMethod2 = propertyInfo2.GetGetMethod();
						if (getMethod2 == null || IsMethodOverriding(getMethod2) || getMethod2.GetParameters().Length != 0)
						{
							continue;
						}
						MethodInfo setMethod2 = propertyInfo2.GetSetMethod(nonPublic: true);
						if (setMethod2 == null)
						{
							if (!SetIfGetOnlyCollection(dataMember2, skipIfReadOnlyContract: true))
							{
								continue;
							}
						}
						else if (!setMethod2.IsPublic || IsMethodOverriding(setMethod2))
						{
							continue;
						}
						if (hasExtensionData && dataMember2.MemberType == Globals.TypeOfExtensionDataObject && memberInfo.Name == "ExtensionData")
						{
							continue;
						}
					}
					dataMember2.Name = DataContract.EncodeLocalName(memberInfo.Name);
					dataMember2.IsNullable = DataContract.IsTypeNullable(dataMember2.MemberType);
					CheckAndAddMember(list, dataMember2, memberNamesTable);
					continue;
				}
				FieldInfo fieldInfo2 = memberInfo as FieldInfo;
				if (!(fieldInfo2 != null) || fieldInfo2.IsNotSerialized)
				{
					continue;
				}
				DataMember dataMember3 = new DataMember(memberInfo);
				dataMember3.Name = DataContract.EncodeLocalName(memberInfo.Name);
				object[] customAttributes3 = fieldInfo2.GetCustomAttributes(Globals.TypeOfOptionalFieldAttribute, inherit: false);
				if (customAttributes3 == null || customAttributes3.Length == 0)
				{
					if (base.IsReference)
					{
						DataContractCriticalHelper.ThrowInvalidDataContractException(SR.GetString("For type '{0}', non-optional field member '{1}' is on the Serializable type that has IsReference as {2}.", DataContract.GetClrTypeFullName(memberInfo.DeclaringType), memberInfo.Name, true), type);
					}
					dataMember3.IsRequired = true;
				}
				dataMember3.IsNullable = DataContract.IsTypeNullable(dataMember3.MemberType);
				CheckAndAddMember(list, dataMember3, memberNamesTable);
			}
			if (list.Count > 1)
			{
				list.Sort(DataMemberComparer.Singleton);
			}
			SetIfMembersHaveConflict(list);
			Thread.MemoryBarrier();
			members = list;
		}

		private bool SetIfGetOnlyCollection(DataMember memberContract, bool skipIfReadOnlyContract)
		{
			if (CollectionDataContract.IsCollection(memberContract.MemberType, constructorRequired: false, skipIfReadOnlyContract) && !memberContract.MemberType.IsValueType)
			{
				memberContract.IsGetOnlyCollection = true;
				return true;
			}
			return false;
		}

		private void SetIfMembersHaveConflict(List<DataMember> members)
		{
			if (BaseContract == null)
			{
				return;
			}
			int num = 0;
			List<Member> list = new List<Member>();
			foreach (DataMember member in members)
			{
				list.Add(new Member(member, base.StableName.Namespace, num));
			}
			for (ClassDataContract classDataContract = BaseContract; classDataContract != null; classDataContract = classDataContract.BaseContract)
			{
				num++;
				foreach (DataMember member2 in classDataContract.Members)
				{
					list.Add(new Member(member2, classDataContract.StableName.Namespace, num));
				}
			}
			IComparer<Member> singleton = DataMemberConflictComparer.Singleton;
			list.Sort(singleton);
			int num2;
			for (num2 = 0; num2 < list.Count - 1; num2++)
			{
				int num3 = num2;
				int i = num2;
				bool flag = false;
				for (; i < list.Count - 1 && string.CompareOrdinal(list[i].member.Name, list[i + 1].member.Name) == 0 && string.CompareOrdinal(list[i].ns, list[i + 1].ns) == 0; i++)
				{
					list[i].member.ConflictingMember = list[i + 1].member;
					if (!flag)
					{
						flag = list[i + 1].member.HasConflictingNameAndType || list[i].member.MemberType != list[i + 1].member.MemberType;
					}
				}
				if (flag)
				{
					for (int j = num3; j <= i; j++)
					{
						list[j].member.HasConflictingNameAndType = true;
					}
				}
				num2 = i + 1;
			}
		}

		[SecuritySafeCritical]
		private XmlQualifiedName GetStableNameAndSetHasDataContract(Type type)
		{
			return DataContract.GetStableName(type, out hasDataContract);
		}

		private void SetIsNonAttributedType(Type type)
		{
			isNonAttributedType = !type.IsSerializable && !hasDataContract && IsNonAttributedTypeValidForSerialization(type);
		}

		private static bool IsMethodOverriding(MethodInfo method)
		{
			if (method.IsVirtual)
			{
				return (method.Attributes & MethodAttributes.VtableLayoutMask) == 0;
			}
			return false;
		}

		internal void EnsureMethodsImported()
		{
			if (isMethodChecked || !(base.UnderlyingType != null))
			{
				return;
			}
			lock (this)
			{
				if (isMethodChecked)
				{
					return;
				}
				MethodInfo[] methods = base.UnderlyingType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (MethodInfo methodInfo in methods)
				{
					Type prevAttributeType = null;
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (HasExtensionData && IsValidExtensionDataSetMethod(methodInfo, parameters))
					{
						if (methodInfo.Name == "System.Runtime.Serialization.IExtensibleDataObject.set_ExtensionData" || !methodInfo.IsPublic)
						{
							extensionDataSetMethod = XmlFormatGeneratorStatics.ExtensionDataSetExplicitMethodInfo;
						}
						else
						{
							extensionDataSetMethod = methodInfo;
						}
					}
					if (IsValidCallback(methodInfo, parameters, Globals.TypeOfOnSerializingAttribute, onSerializing, ref prevAttributeType))
					{
						onSerializing = methodInfo;
					}
					if (IsValidCallback(methodInfo, parameters, Globals.TypeOfOnSerializedAttribute, onSerialized, ref prevAttributeType))
					{
						onSerialized = methodInfo;
					}
					if (IsValidCallback(methodInfo, parameters, Globals.TypeOfOnDeserializingAttribute, onDeserializing, ref prevAttributeType))
					{
						onDeserializing = methodInfo;
					}
					if (IsValidCallback(methodInfo, parameters, Globals.TypeOfOnDeserializedAttribute, onDeserialized, ref prevAttributeType))
					{
						onDeserialized = methodInfo;
					}
				}
				Thread.MemoryBarrier();
				isMethodChecked = true;
			}
		}

		private bool IsValidExtensionDataSetMethod(MethodInfo method, ParameterInfo[] parameters)
		{
			if (method.Name == "System.Runtime.Serialization.IExtensibleDataObject.set_ExtensionData" || method.Name == "set_ExtensionData")
			{
				if (extensionDataSetMethod != null)
				{
					ThrowInvalidDataContractException(SR.GetString("Duplicate extension data set method was found, for method '{0}', existing method is '{1}', on data contract type '{2}'.", method, extensionDataSetMethod, DataContract.GetClrTypeFullName(method.DeclaringType)));
				}
				if (method.ReturnType != Globals.TypeOfVoid)
				{
					DataContract.ThrowInvalidDataContractException(SR.GetString("For type '{0}' method '{1}', extension data set method must return void.", DataContract.GetClrTypeFullName(method.DeclaringType), method), method.DeclaringType);
				}
				if (parameters == null || parameters.Length != 1 || parameters[0].ParameterType != Globals.TypeOfExtensionDataObject)
				{
					DataContract.ThrowInvalidDataContractException(SR.GetString("For type '{0}' method '{1}', extension data set method has invalid type of parameter '{2}'.", DataContract.GetClrTypeFullName(method.DeclaringType), method, Globals.TypeOfExtensionDataObject), method.DeclaringType);
				}
				return true;
			}
			return false;
		}

		private static bool IsValidCallback(MethodInfo method, ParameterInfo[] parameters, Type attributeType, MethodInfo currentCallback, ref Type prevAttributeType)
		{
			if (method.IsDefined(attributeType, inherit: false))
			{
				if (currentCallback != null)
				{
					DataContract.ThrowInvalidDataContractException(SR.GetString("Invalid attribute. Both '{0}' and '{1}' in type '{2}' have '{3}'.", method, currentCallback, DataContract.GetClrTypeFullName(method.DeclaringType), attributeType), method.DeclaringType);
				}
				else if (prevAttributeType != null)
				{
					DataContract.ThrowInvalidDataContractException(SR.GetString("Invalid Callback. Method '{3}' in type '{2}' has both '{0}' and '{1}'.", prevAttributeType, attributeType, DataContract.GetClrTypeFullName(method.DeclaringType), method), method.DeclaringType);
				}
				else if (method.IsVirtual)
				{
					DataContract.ThrowInvalidDataContractException(SR.GetString("Virtual Method '{0}' of type '{1}' cannot be marked with '{2}' attribute.", method, DataContract.GetClrTypeFullName(method.DeclaringType), attributeType), method.DeclaringType);
				}
				else
				{
					if (method.ReturnType != Globals.TypeOfVoid)
					{
						DataContract.ThrowInvalidDataContractException(SR.GetString("Serialization Callback '{1}' in type '{0}' must return void.", DataContract.GetClrTypeFullName(method.DeclaringType), method), method.DeclaringType);
					}
					if (parameters == null || parameters.Length != 1 || parameters[0].ParameterType != Globals.TypeOfStreamingContext)
					{
						DataContract.ThrowInvalidDataContractException(SR.GetString("Serialization Callback '{1}' in type '{0}' must have a single parameter of type '{2}'.", DataContract.GetClrTypeFullName(method.DeclaringType), method, Globals.TypeOfStreamingContext), method.DeclaringType);
					}
					prevAttributeType = attributeType;
				}
				return true;
			}
			return false;
		}

		internal ConstructorInfo GetISerializableConstructor()
		{
			if (!IsISerializable)
			{
				return null;
			}
			ConstructorInfo constructor = base.UnderlyingType.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, SerInfoCtorArgs, null);
			if (constructor == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Constructor that takes SerializationInfo and StreamingContext is not found for '{0}'.", DataContract.GetClrTypeFullName(base.UnderlyingType))));
			}
			return constructor;
		}

		internal ConstructorInfo GetNonAttributedTypeConstructor()
		{
			if (!IsNonAttributedType)
			{
				return null;
			}
			Type type = base.UnderlyingType;
			if (type.IsValueType)
			{
				return null;
			}
			ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null);
			if (constructor == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("The Type '{0}' must have a parameterless constructor.", DataContract.GetClrTypeFullName(type))));
			}
			return constructor;
		}
	}

	internal class DataMemberComparer : IComparer<DataMember>
	{
		internal static DataMemberComparer Singleton = new DataMemberComparer();

		public int Compare(DataMember x, DataMember y)
		{
			int num = x.Order - y.Order;
			if (num != 0)
			{
				return num;
			}
			return string.CompareOrdinal(x.Name, y.Name);
		}
	}

	public XmlDictionaryString[] ContractNamespaces;

	public XmlDictionaryString[] MemberNames;

	public XmlDictionaryString[] MemberNamespaces;

	private XmlDictionaryString[] childElementNamespaces;

	private ClassDataContractCriticalHelper helper;

	internal ClassDataContract BaseContract
	{
		[SecuritySafeCritical]
		get
		{
			return helper.BaseContract;
		}
		[SecurityCritical]
		set
		{
			helper.BaseContract = value;
		}
	}

	internal List<DataMember> Members
	{
		[SecuritySafeCritical]
		get
		{
			return helper.Members;
		}
		[SecurityCritical]
		set
		{
			helper.Members = value;
		}
	}

	public XmlDictionaryString[] ChildElementNamespaces
	{
		[SecuritySafeCritical]
		get
		{
			if (childElementNamespaces == null)
			{
				lock (this)
				{
					if (childElementNamespaces == null)
					{
						if (helper.ChildElementNamespaces == null)
						{
							XmlDictionaryString[] array = CreateChildElementNamespaces();
							Thread.MemoryBarrier();
							helper.ChildElementNamespaces = array;
						}
						childElementNamespaces = helper.ChildElementNamespaces;
					}
				}
			}
			return childElementNamespaces;
		}
	}

	internal MethodInfo OnSerializing
	{
		[SecuritySafeCritical]
		get
		{
			return helper.OnSerializing;
		}
	}

	internal MethodInfo OnSerialized
	{
		[SecuritySafeCritical]
		get
		{
			return helper.OnSerialized;
		}
	}

	internal MethodInfo OnDeserializing
	{
		[SecuritySafeCritical]
		get
		{
			return helper.OnDeserializing;
		}
	}

	internal MethodInfo OnDeserialized
	{
		[SecuritySafeCritical]
		get
		{
			return helper.OnDeserialized;
		}
	}

	internal MethodInfo ExtensionDataSetMethod
	{
		[SecuritySafeCritical]
		get
		{
			return helper.ExtensionDataSetMethod;
		}
	}

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

	internal override bool IsISerializable
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsISerializable;
		}
		[SecurityCritical]
		set
		{
			helper.IsISerializable = value;
		}
	}

	internal bool IsNonAttributedType
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsNonAttributedType;
		}
	}

	internal bool HasDataContract
	{
		[SecuritySafeCritical]
		get
		{
			return helper.HasDataContract;
		}
	}

	internal bool HasExtensionData
	{
		[SecuritySafeCritical]
		get
		{
			return helper.HasExtensionData;
		}
	}

	internal string SerializationExceptionMessage
	{
		[SecuritySafeCritical]
		get
		{
			return helper.SerializationExceptionMessage;
		}
	}

	internal string DeserializationExceptionMessage
	{
		[SecuritySafeCritical]
		get
		{
			return helper.DeserializationExceptionMessage;
		}
	}

	internal bool IsReadOnlyContract => DeserializationExceptionMessage != null;

	internal XmlFormatClassWriterDelegate XmlFormatWriterDelegate
	{
		[SecuritySafeCritical]
		get
		{
			if (helper.XmlFormatWriterDelegate == null)
			{
				lock (this)
				{
					if (helper.XmlFormatWriterDelegate == null)
					{
						XmlFormatClassWriterDelegate xmlFormatWriterDelegate = new XmlFormatWriterGenerator().GenerateClassWriter(this);
						Thread.MemoryBarrier();
						helper.XmlFormatWriterDelegate = xmlFormatWriterDelegate;
					}
				}
			}
			return helper.XmlFormatWriterDelegate;
		}
	}

	internal XmlFormatClassReaderDelegate XmlFormatReaderDelegate
	{
		[SecuritySafeCritical]
		get
		{
			if (helper.XmlFormatReaderDelegate == null)
			{
				lock (this)
				{
					if (helper.XmlFormatReaderDelegate == null)
					{
						if (IsReadOnlyContract)
						{
							DataContract.ThrowInvalidDataContractException(helper.DeserializationExceptionMessage, null);
						}
						XmlFormatClassReaderDelegate xmlFormatReaderDelegate = new XmlFormatReaderGenerator().GenerateClassReader(this);
						Thread.MemoryBarrier();
						helper.XmlFormatReaderDelegate = xmlFormatReaderDelegate;
					}
				}
			}
			return helper.XmlFormatReaderDelegate;
		}
	}

	[SecuritySafeCritical]
	internal ClassDataContract()
		: base(new ClassDataContractCriticalHelper())
	{
		InitClassDataContract();
	}

	internal ClassDataContract(Type type)
		: base(new ClassDataContractCriticalHelper(type))
	{
		InitClassDataContract();
	}

	private ClassDataContract(Type type, XmlDictionaryString ns, string[] memberNames)
		: base(new ClassDataContractCriticalHelper(type, ns, memberNames))
	{
		InitClassDataContract();
	}

	private void InitClassDataContract()
	{
		helper = base.Helper as ClassDataContractCriticalHelper;
		ContractNamespaces = helper.ContractNamespaces;
		MemberNames = helper.MemberNames;
		MemberNamespaces = helper.MemberNamespaces;
	}

	[SecuritySafeCritical]
	internal ConstructorInfo GetISerializableConstructor()
	{
		return helper.GetISerializableConstructor();
	}

	[SecuritySafeCritical]
	internal ConstructorInfo GetNonAttributedTypeConstructor()
	{
		return helper.GetNonAttributedTypeConstructor();
	}

	internal static ClassDataContract CreateClassDataContractForKeyValue(Type type, XmlDictionaryString ns, string[] memberNames)
	{
		return new ClassDataContract(type, ns, memberNames);
	}

	internal static void CheckAndAddMember(List<DataMember> members, DataMember memberContract, Dictionary<string, DataMember> memberNamesTable)
	{
		if (memberNamesTable.TryGetValue(memberContract.Name, out var value))
		{
			Type declaringType = memberContract.MemberInfo.DeclaringType;
			DataContract.ThrowInvalidDataContractException(SR.GetString(declaringType.IsEnum ? "Type '{2}' contains two members '{0}' 'and '{1}' with the same name '{3}'. Multiple members with the same name in one type are not supported. Consider changing one of the member names using EnumMemberAttribute attribute." : "Type '{2}' contains two members '{0}' 'and '{1}' with the same data member name '{3}'. Multiple members with the same name in one type are not supported. Consider changing one of the member names using DataMemberAttribute attribute.", value.MemberInfo.Name, memberContract.MemberInfo.Name, DataContract.GetClrTypeFullName(declaringType), memberContract.Name), declaringType);
		}
		memberNamesTable.Add(memberContract.Name, memberContract);
		members.Add(memberContract);
	}

	internal static XmlDictionaryString GetChildNamespaceToDeclare(DataContract dataContract, Type childType, XmlDictionary dictionary)
	{
		childType = DataContract.UnwrapNullableType(childType);
		if (!childType.IsEnum && !Globals.TypeOfIXmlSerializable.IsAssignableFrom(childType) && DataContract.GetBuiltInDataContract(childType) == null && childType != Globals.TypeOfDBNull)
		{
			string text = DataContract.GetStableName(childType).Namespace;
			if (text.Length > 0 && text != dataContract.Namespace.Value)
			{
				return dictionary.Add(text);
			}
		}
		return null;
	}

	internal static bool IsNonAttributedTypeValidForSerialization(Type type)
	{
		if (type.IsArray)
		{
			return false;
		}
		if (type.IsEnum)
		{
			return false;
		}
		if (type.IsGenericParameter)
		{
			return false;
		}
		if (Globals.TypeOfIXmlSerializable.IsAssignableFrom(type))
		{
			return false;
		}
		if (type.IsPointer)
		{
			return false;
		}
		if (type.IsDefined(Globals.TypeOfCollectionDataContractAttribute, inherit: false))
		{
			return false;
		}
		Type[] interfaces = type.GetInterfaces();
		for (int i = 0; i < interfaces.Length; i++)
		{
			if (CollectionDataContract.IsCollectionInterface(interfaces[i]))
			{
				return false;
			}
		}
		if (type.IsSerializable)
		{
			return false;
		}
		if (Globals.TypeOfISerializable.IsAssignableFrom(type))
		{
			return false;
		}
		if (type.IsDefined(Globals.TypeOfDataContractAttribute, inherit: false))
		{
			return false;
		}
		if (type == Globals.TypeOfExtensionDataObject)
		{
			return false;
		}
		if (type.IsValueType)
		{
			return type.IsVisible;
		}
		if (type.IsVisible)
		{
			return type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null) != null;
		}
		return false;
	}

	private XmlDictionaryString[] CreateChildElementNamespaces()
	{
		if (Members == null)
		{
			return null;
		}
		XmlDictionaryString[] array = null;
		if (BaseContract != null)
		{
			array = BaseContract.ChildElementNamespaces;
		}
		int num = ((array != null) ? array.Length : 0);
		XmlDictionaryString[] array2 = new XmlDictionaryString[Members.Count + num];
		if (num > 0)
		{
			Array.Copy(array, 0, array2, 0, array.Length);
		}
		XmlDictionary dictionary = new XmlDictionary();
		for (int i = 0; i < Members.Count; i++)
		{
			array2[i + num] = GetChildNamespaceToDeclare(this, Members[i].MemberType, dictionary);
		}
		return array2;
	}

	[SecuritySafeCritical]
	private void EnsureMethodsImported()
	{
		helper.EnsureMethodsImported();
	}

	public override void WriteXmlValue(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context)
	{
		XmlFormatWriterDelegate(xmlWriter, obj, context, this);
	}

	public override object ReadXmlValue(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context)
	{
		xmlReader.Read();
		object result = XmlFormatReaderDelegate(xmlReader, context, MemberNames, MemberNamespaces);
		xmlReader.ReadEndElement();
		return result;
	}

	[SecuritySafeCritical]
	internal override DataContract BindGenericParameters(DataContract[] paramContracts, Dictionary<DataContract, DataContract> boundContracts)
	{
		Type underlyingType = base.UnderlyingType;
		if (!underlyingType.IsGenericType || !underlyingType.ContainsGenericParameters)
		{
			return this;
		}
		lock (this)
		{
			if (boundContracts.TryGetValue(this, out var value))
			{
				return value;
			}
			ClassDataContract classDataContract = new ClassDataContract();
			boundContracts.Add(this, classDataContract);
			XmlQualifiedName stableName;
			object[] array;
			if (underlyingType.IsGenericTypeDefinition)
			{
				stableName = base.StableName;
				array = paramContracts;
			}
			else
			{
				stableName = DataContract.GetStableName(underlyingType.GetGenericTypeDefinition());
				Type[] genericArguments = underlyingType.GetGenericArguments();
				array = new object[genericArguments.Length];
				for (int i = 0; i < genericArguments.Length; i++)
				{
					Type type = genericArguments[i];
					if (type.IsGenericParameter)
					{
						array[i] = paramContracts[type.GenericParameterPosition];
					}
					else
					{
						array[i] = type;
					}
				}
			}
			classDataContract.StableName = DataContract.CreateQualifiedName(DataContract.ExpandGenericParameters(XmlConvert.DecodeName(stableName.Name), new GenericNameProvider(DataContract.GetClrTypeFullName(base.UnderlyingType), array)), stableName.Namespace);
			if (BaseContract != null)
			{
				classDataContract.BaseContract = (ClassDataContract)BaseContract.BindGenericParameters(paramContracts, boundContracts);
			}
			classDataContract.IsISerializable = IsISerializable;
			classDataContract.IsValueType = base.IsValueType;
			classDataContract.IsReference = base.IsReference;
			if (Members != null)
			{
				classDataContract.Members = new List<DataMember>(Members.Count);
				foreach (DataMember member in Members)
				{
					classDataContract.Members.Add(member.BindGenericParameters(paramContracts, boundContracts));
				}
			}
			return classDataContract;
		}
	}

	internal override bool Equals(object other, Dictionary<DataContractPairKey, object> checkedContracts)
	{
		if (IsEqualOrChecked(other, checkedContracts))
		{
			return true;
		}
		if (base.Equals(other, checkedContracts) && other is ClassDataContract classDataContract)
		{
			if (IsISerializable)
			{
				if (!classDataContract.IsISerializable)
				{
					return false;
				}
			}
			else
			{
				if (classDataContract.IsISerializable)
				{
					return false;
				}
				if (Members == null)
				{
					if (classDataContract.Members != null && !IsEveryDataMemberOptional(classDataContract.Members))
					{
						return false;
					}
				}
				else if (classDataContract.Members == null)
				{
					if (!IsEveryDataMemberOptional(Members))
					{
						return false;
					}
				}
				else
				{
					Dictionary<string, DataMember> dictionary = new Dictionary<string, DataMember>(Members.Count);
					List<DataMember> list = new List<DataMember>();
					for (int i = 0; i < Members.Count; i++)
					{
						dictionary.Add(Members[i].Name, Members[i]);
					}
					for (int j = 0; j < classDataContract.Members.Count; j++)
					{
						if (dictionary.TryGetValue(classDataContract.Members[j].Name, out var value))
						{
							if (!value.Equals(classDataContract.Members[j], checkedContracts))
							{
								return false;
							}
							dictionary.Remove(value.Name);
						}
						else
						{
							list.Add(classDataContract.Members[j]);
						}
					}
					if (!IsEveryDataMemberOptional(dictionary.Values))
					{
						return false;
					}
					if (!IsEveryDataMemberOptional(list))
					{
						return false;
					}
				}
			}
			if (BaseContract == null)
			{
				return classDataContract.BaseContract == null;
			}
			if (classDataContract.BaseContract == null)
			{
				return false;
			}
			return BaseContract.Equals(classDataContract.BaseContract, checkedContracts);
		}
		return false;
	}

	private bool IsEveryDataMemberOptional(IEnumerable<DataMember> dataMembers)
	{
		foreach (DataMember dataMember in dataMembers)
		{
			if (dataMember.IsRequired)
			{
				return false;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
