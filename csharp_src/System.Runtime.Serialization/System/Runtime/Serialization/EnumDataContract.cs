using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Xml;

namespace System.Runtime.Serialization;

internal sealed class EnumDataContract : DataContract
{
	private class EnumDataContractCriticalHelper : DataContractCriticalHelper
	{
		private static Dictionary<Type, XmlQualifiedName> typeToName;

		private static Dictionary<XmlQualifiedName, Type> nameToType;

		private XmlQualifiedName baseContractName;

		private List<DataMember> members;

		private List<long> values;

		private bool isULong;

		private bool isFlags;

		private bool hasDataContract;

		private XmlDictionaryString[] childElementNames;

		internal XmlQualifiedName BaseContractName
		{
			get
			{
				return baseContractName;
			}
			set
			{
				baseContractName = value;
				Type baseType = GetBaseType(baseContractName);
				if (baseType == null)
				{
					ThrowInvalidDataContractException(SR.GetString("Invalid enum base type is specified for type '{0}' in '{1}' namespace, element name is '{2}' in '{3}' namespace.", value.Name, value.Namespace, base.StableName.Name, base.StableName.Namespace));
				}
				ImportBaseType(baseType);
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

		internal List<long> Values
		{
			get
			{
				return values;
			}
			set
			{
				values = value;
			}
		}

		internal bool IsFlags
		{
			get
			{
				return isFlags;
			}
			set
			{
				isFlags = value;
			}
		}

		internal bool IsULong => isULong;

		internal XmlDictionaryString[] ChildElementNames => childElementNames;

		static EnumDataContractCriticalHelper()
		{
			typeToName = new Dictionary<Type, XmlQualifiedName>();
			nameToType = new Dictionary<XmlQualifiedName, Type>();
			Add(typeof(sbyte), "byte");
			Add(typeof(byte), "unsignedByte");
			Add(typeof(short), "short");
			Add(typeof(ushort), "unsignedShort");
			Add(typeof(int), "int");
			Add(typeof(uint), "unsignedInt");
			Add(typeof(long), "long");
			Add(typeof(ulong), "unsignedLong");
		}

		internal static void Add(Type type, string localName)
		{
			XmlQualifiedName xmlQualifiedName = DataContract.CreateQualifiedName(localName, "http://www.w3.org/2001/XMLSchema");
			typeToName.Add(type, xmlQualifiedName);
			nameToType.Add(xmlQualifiedName, type);
		}

		internal static XmlQualifiedName GetBaseContractName(Type type)
		{
			XmlQualifiedName value = null;
			typeToName.TryGetValue(type, out value);
			return value;
		}

		internal static Type GetBaseType(XmlQualifiedName baseContractName)
		{
			Type value = null;
			nameToType.TryGetValue(baseContractName, out value);
			return value;
		}

		internal EnumDataContractCriticalHelper()
		{
			base.IsValueType = true;
		}

		internal EnumDataContractCriticalHelper(Type type)
			: base(type)
		{
			base.StableName = DataContract.GetStableName(type, out hasDataContract);
			Type type2 = Enum.GetUnderlyingType(type);
			baseContractName = GetBaseContractName(type2);
			ImportBaseType(type2);
			IsFlags = type.IsDefined(Globals.TypeOfFlagsAttribute, inherit: false);
			ImportDataMembers();
			XmlDictionary xmlDictionary = new XmlDictionary(2 + Members.Count);
			base.Name = xmlDictionary.Add(base.StableName.Name);
			base.Namespace = xmlDictionary.Add(base.StableName.Namespace);
			childElementNames = new XmlDictionaryString[Members.Count];
			for (int i = 0; i < Members.Count; i++)
			{
				childElementNames[i] = xmlDictionary.Add(Members[i].Name);
			}
			if (DataContract.TryGetDCAttribute(type, out var dataContractAttribute) && dataContractAttribute.IsReference)
			{
				DataContract.ThrowInvalidDataContractException(SR.GetString("Enum type '{0}' cannot have the IsReference setting of '{1}'. Either change the setting to '{2}', or remove it completely.", DataContract.GetClrTypeFullName(type), dataContractAttribute.IsReference, false), type);
			}
		}

		private void ImportBaseType(Type baseType)
		{
			isULong = baseType == Globals.TypeOfULong;
		}

		private void ImportDataMembers()
		{
			Type type = base.UnderlyingType;
			FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
			Dictionary<string, DataMember> memberNamesTable = new Dictionary<string, DataMember>();
			List<DataMember> list = new List<DataMember>(fields.Length);
			List<long> list2 = new List<long>(fields.Length);
			foreach (FieldInfo fieldInfo in fields)
			{
				bool flag = false;
				if (hasDataContract)
				{
					object[] customAttributes = fieldInfo.GetCustomAttributes(Globals.TypeOfEnumMemberAttribute, inherit: false);
					if (customAttributes != null && customAttributes.Length != 0)
					{
						if (customAttributes.Length > 1)
						{
							ThrowInvalidDataContractException(SR.GetString("Member '{0}.{1}' has more than one EnumMemberAttribute attribute.", DataContract.GetClrTypeFullName(fieldInfo.DeclaringType), fieldInfo.Name));
						}
						EnumMemberAttribute enumMemberAttribute = (EnumMemberAttribute)customAttributes[0];
						DataMember dataMember = new DataMember(fieldInfo);
						if (enumMemberAttribute.IsValueSetExplicitly)
						{
							if (enumMemberAttribute.Value == null || enumMemberAttribute.Value.Length == 0)
							{
								ThrowInvalidDataContractException(SR.GetString("'{0}' in type '{1}' cannot have EnumMemberAttribute attribute Value set to null or empty string.", fieldInfo.Name, DataContract.GetClrTypeFullName(type)));
							}
							dataMember.Name = enumMemberAttribute.Value;
						}
						else
						{
							dataMember.Name = fieldInfo.Name;
						}
						ClassDataContract.CheckAndAddMember(list, dataMember, memberNamesTable);
						flag = true;
					}
					object[] customAttributes2 = fieldInfo.GetCustomAttributes(Globals.TypeOfDataMemberAttribute, inherit: false);
					if (customAttributes2 != null && customAttributes2.Length != 0)
					{
						ThrowInvalidDataContractException(SR.GetString("Member '{0}.{1}' has DataMemberAttribute attribute. Use EnumMemberAttribute attribute instead.", DataContract.GetClrTypeFullName(fieldInfo.DeclaringType), fieldInfo.Name));
					}
				}
				else if (!fieldInfo.IsNotSerialized)
				{
					DataMember dataMember2 = new DataMember(fieldInfo);
					dataMember2.Name = fieldInfo.Name;
					ClassDataContract.CheckAndAddMember(list, dataMember2, memberNamesTable);
					flag = true;
				}
				if (flag)
				{
					object value = fieldInfo.GetValue(null);
					if (isULong)
					{
						list2.Add((long)((IConvertible)value).ToUInt64(null));
					}
					else
					{
						list2.Add(((IConvertible)value).ToInt64(null));
					}
				}
			}
			Thread.MemoryBarrier();
			members = list;
			values = list2;
		}
	}

	[SecurityCritical]
	private EnumDataContractCriticalHelper helper;

	internal XmlQualifiedName BaseContractName
	{
		[SecuritySafeCritical]
		get
		{
			return helper.BaseContractName;
		}
		[SecurityCritical]
		set
		{
			helper.BaseContractName = value;
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

	internal List<long> Values
	{
		[SecuritySafeCritical]
		get
		{
			return helper.Values;
		}
		[SecurityCritical]
		set
		{
			helper.Values = value;
		}
	}

	internal bool IsFlags
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsFlags;
		}
		[SecurityCritical]
		set
		{
			helper.IsFlags = value;
		}
	}

	internal bool IsULong
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsULong;
		}
	}

	private XmlDictionaryString[] ChildElementNames
	{
		[SecuritySafeCritical]
		get
		{
			return helper.ChildElementNames;
		}
	}

	internal override bool CanContainReferences => false;

	[SecuritySafeCritical]
	internal EnumDataContract()
		: base(new EnumDataContractCriticalHelper())
	{
		helper = base.Helper as EnumDataContractCriticalHelper;
	}

	[SecuritySafeCritical]
	internal EnumDataContract(Type type)
		: base(new EnumDataContractCriticalHelper(type))
	{
		helper = base.Helper as EnumDataContractCriticalHelper;
	}

	[SecuritySafeCritical]
	internal static XmlQualifiedName GetBaseContractName(Type type)
	{
		return EnumDataContractCriticalHelper.GetBaseContractName(type);
	}

	[SecuritySafeCritical]
	internal static Type GetBaseType(XmlQualifiedName baseContractName)
	{
		return EnumDataContractCriticalHelper.GetBaseType(baseContractName);
	}

	internal void WriteEnumValue(XmlWriterDelegator writer, object value)
	{
		long num = (IsULong ? ((long)((IConvertible)value).ToUInt64(null)) : ((IConvertible)value).ToInt64(null));
		for (int i = 0; i < Values.Count; i++)
		{
			if (num == Values[i])
			{
				writer.WriteString(ChildElementNames[i].Value);
				return;
			}
		}
		if (IsFlags)
		{
			int num2 = -1;
			bool flag = true;
			for (int j = 0; j < Values.Count; j++)
			{
				long num3 = Values[j];
				if (num3 == 0L)
				{
					num2 = j;
					continue;
				}
				if (num == 0L)
				{
					break;
				}
				if ((num3 & num) == num3)
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						writer.WriteString(DictionaryGlobals.Space.Value);
					}
					writer.WriteString(ChildElementNames[j].Value);
					num &= ~num3;
				}
			}
			if (num != 0L)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Enum value '{0}' is invalid for type '{1}' and cannot be serialized. Ensure that the necessary enum values are present and are marked with EnumMemberAttribute attribute if the type has DataContractAttribute attribute.", value, DataContract.GetClrTypeFullName(base.UnderlyingType))));
			}
			if (flag && num2 >= 0)
			{
				writer.WriteString(ChildElementNames[num2].Value);
			}
			return;
		}
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Enum value '{0}' is invalid for type '{1}' and cannot be serialized. Ensure that the necessary enum values are present and are marked with EnumMemberAttribute attribute if the type has DataContractAttribute attribute.", value, DataContract.GetClrTypeFullName(base.UnderlyingType))));
	}

	internal object ReadEnumValue(XmlReaderDelegator reader)
	{
		string text = reader.ReadElementContentAsString();
		long num = 0L;
		int i = 0;
		if (IsFlags)
		{
			for (; i < text.Length && text[i] == ' '; i++)
			{
			}
			int num2 = i;
			int num3 = 0;
			for (; i < text.Length; i++)
			{
				if (text[i] == ' ')
				{
					num3 = i - num2;
					if (num3 > 0)
					{
						num |= ReadEnumValue(text, num2, num3);
					}
					for (i++; i < text.Length && text[i] == ' '; i++)
					{
					}
					num2 = i;
					if (i == text.Length)
					{
						break;
					}
				}
			}
			num3 = i - num2;
			if (num3 > 0)
			{
				num |= ReadEnumValue(text, num2, num3);
			}
		}
		else
		{
			if (text.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Invalid enum value '{0}' cannot be deserialized into type '{1}'. Ensure that the necessary enum values are present and are marked with EnumMemberAttribute attribute if the type has DataContractAttribute attribute.", text, DataContract.GetClrTypeFullName(base.UnderlyingType))));
			}
			num = ReadEnumValue(text, 0, text.Length);
		}
		if (IsULong)
		{
			return Enum.ToObject(base.UnderlyingType, (ulong)num);
		}
		return Enum.ToObject(base.UnderlyingType, num);
	}

	private long ReadEnumValue(string value, int index, int count)
	{
		for (int i = 0; i < Members.Count; i++)
		{
			string text = Members[i].Name;
			if (text.Length == count && string.CompareOrdinal(value, index, text, 0, count) == 0)
			{
				return Values[i];
			}
		}
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Invalid enum value '{0}' cannot be deserialized into type '{1}'. Ensure that the necessary enum values are present and are marked with EnumMemberAttribute attribute if the type has DataContractAttribute attribute.", value.Substring(index, count), DataContract.GetClrTypeFullName(base.UnderlyingType))));
	}

	internal string GetStringFromEnumValue(long value)
	{
		if (IsULong)
		{
			return XmlConvert.ToString((ulong)value);
		}
		return XmlConvert.ToString(value);
	}

	internal long GetEnumValueFromString(string value)
	{
		if (IsULong)
		{
			return (long)XmlConverter.ToUInt64(value);
		}
		return XmlConverter.ToInt64(value);
	}

	internal override bool Equals(object other, Dictionary<DataContractPairKey, object> checkedContracts)
	{
		if (IsEqualOrChecked(other, checkedContracts))
		{
			return true;
		}
		if (base.Equals(other, null) && other is EnumDataContract enumDataContract)
		{
			if (Members.Count != enumDataContract.Members.Count || Values.Count != enumDataContract.Values.Count)
			{
				return false;
			}
			string[] array = new string[Members.Count];
			string[] array2 = new string[Members.Count];
			for (int i = 0; i < Members.Count; i++)
			{
				array[i] = Members[i].Name;
				array2[i] = enumDataContract.Members[i].Name;
			}
			Array.Sort(array);
			Array.Sort(array2);
			for (int j = 0; j < Members.Count; j++)
			{
				if (array[j] != array2[j])
				{
					return false;
				}
			}
			return IsFlags == enumDataContract.IsFlags;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override void WriteXmlValue(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context)
	{
		WriteEnumValue(xmlWriter, obj);
	}

	public override object ReadXmlValue(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context)
	{
		object obj = ReadEnumValue(xmlReader);
		context?.AddNewObject(obj);
		return obj;
	}
}
