using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization;

internal abstract class DataContract
{
	protected class DataContractCriticalHelper
	{
		private static Dictionary<TypeHandleRef, IntRef> typeToIDCache;

		private static DataContract[] dataContractCache;

		private static int dataContractID;

		private static Dictionary<Type, DataContract> typeToBuiltInContract;

		private static Dictionary<XmlQualifiedName, DataContract> nameToBuiltInContract;

		private static Dictionary<string, DataContract> typeNameToBuiltInContract;

		private static Dictionary<string, string> namespaces;

		private static Dictionary<string, XmlDictionaryString> clrTypeStrings;

		private static XmlDictionary clrTypeStringsDictionary;

		private static TypeHandleRef typeHandleRef;

		private static object cacheLock;

		private static object createDataContractLock;

		private static object initBuiltInContractsLock;

		private static object namespacesLock;

		private static object clrTypeStringsLock;

		private readonly Type underlyingType;

		private Type originalUnderlyingType;

		private bool isReference;

		private bool isValueType;

		private XmlQualifiedName stableName;

		private GenericInfo genericInfo;

		private XmlDictionaryString name;

		private XmlDictionaryString ns;

		private Type typeForInitialization;

		private MethodInfo parseMethod;

		private bool parseMethodSet;

		internal Type UnderlyingType => underlyingType;

		internal Type OriginalUnderlyingType
		{
			get
			{
				if (originalUnderlyingType == null)
				{
					originalUnderlyingType = GetDataContractOriginalType(underlyingType);
				}
				return originalUnderlyingType;
			}
		}

		internal virtual bool IsBuiltInDataContract => false;

		internal Type TypeForInitialization => typeForInitialization;

		internal bool IsReference
		{
			get
			{
				return isReference;
			}
			set
			{
				isReference = value;
			}
		}

		internal bool IsValueType
		{
			get
			{
				return isValueType;
			}
			set
			{
				isValueType = value;
			}
		}

		internal XmlQualifiedName StableName
		{
			get
			{
				return stableName;
			}
			set
			{
				stableName = value;
			}
		}

		internal GenericInfo GenericInfo
		{
			get
			{
				return genericInfo;
			}
			set
			{
				genericInfo = value;
			}
		}

		internal virtual Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		internal virtual bool IsISerializable
		{
			get
			{
				return false;
			}
			set
			{
				ThrowInvalidDataContractException(SR.GetString("To set IsISerializable, class data cotnract is required."));
			}
		}

		internal XmlDictionaryString Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public XmlDictionaryString Namespace
		{
			get
			{
				return ns;
			}
			set
			{
				ns = value;
			}
		}

		internal virtual bool HasRoot
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		internal virtual XmlDictionaryString TopLevelElementName
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		internal virtual XmlDictionaryString TopLevelElementNamespace
		{
			get
			{
				return ns;
			}
			set
			{
				ns = value;
			}
		}

		internal virtual bool CanContainReferences => true;

		internal virtual bool IsPrimitive => false;

		internal MethodInfo ParseMethod
		{
			get
			{
				if (!parseMethodSet)
				{
					MethodInfo method = UnderlyingType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[1] { Globals.TypeOfString }, null);
					if (method != null && method.ReturnType == UnderlyingType)
					{
						parseMethod = method;
					}
					parseMethodSet = true;
				}
				return parseMethod;
			}
		}

		static DataContractCriticalHelper()
		{
			typeHandleRef = new TypeHandleRef();
			cacheLock = new object();
			createDataContractLock = new object();
			initBuiltInContractsLock = new object();
			namespacesLock = new object();
			clrTypeStringsLock = new object();
			typeToIDCache = new Dictionary<TypeHandleRef, IntRef>(new TypeHandleRefEqualityComparer());
			dataContractCache = new DataContract[32];
			dataContractID = 0;
		}

		internal static DataContract GetDataContractSkipValidation(int id, RuntimeTypeHandle typeHandle, Type type)
		{
			DataContract dataContract = dataContractCache[id];
			if (dataContract == null)
			{
				return CreateDataContract(id, typeHandle, type);
			}
			return dataContract.GetValidContract();
		}

		internal static DataContract GetGetOnlyCollectionDataContractSkipValidation(int id, RuntimeTypeHandle typeHandle, Type type)
		{
			DataContract dataContract = dataContractCache[id];
			if (dataContract == null)
			{
				dataContract = CreateGetOnlyCollectionDataContract(id, typeHandle, type);
				AssignDataContractToId(dataContract, id);
			}
			return dataContract;
		}

		internal static DataContract GetDataContractForInitialization(int id)
		{
			return dataContractCache[id] ?? throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("An internal error has occurred. DataContract cache overflow.")));
		}

		internal static int GetIdForInitialization(ClassDataContract classContract)
		{
			int id = DataContract.GetId(classContract.TypeForInitialization.TypeHandle);
			if (id < dataContractCache.Length && ContractMatches(classContract, dataContractCache[id]))
			{
				return id;
			}
			int num = dataContractID;
			for (int i = 0; i < num; i++)
			{
				if (ContractMatches(classContract, dataContractCache[i]))
				{
					return i;
				}
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("An internal error has occurred. DataContract cache overflow.")));
		}

		private static bool ContractMatches(DataContract contract, DataContract cachedContract)
		{
			if (cachedContract != null)
			{
				return cachedContract.UnderlyingType == contract.UnderlyingType;
			}
			return false;
		}

		internal static int GetId(RuntimeTypeHandle typeHandle)
		{
			lock (cacheLock)
			{
				typeHandle = GetDataContractAdapterTypeHandle(typeHandle);
				typeHandleRef.Value = typeHandle;
				if (!typeToIDCache.TryGetValue(typeHandleRef, out var value))
				{
					value = GetNextId();
					try
					{
						typeToIDCache.Add(new TypeHandleRef(typeHandle), value);
					}
					catch (Exception ex)
					{
						if (Fx.IsFatal(ex))
						{
							throw;
						}
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperFatal(ex.Message, ex);
					}
				}
				return value.Value;
			}
		}

		private static IntRef GetNextId()
		{
			int num = dataContractID++;
			if (num >= dataContractCache.Length)
			{
				int num2 = ((num < 1073741823) ? (num * 2) : int.MaxValue);
				if (num2 <= num)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("An internal error has occurred. DataContract cache overflow.")));
				}
				Array.Resize(ref dataContractCache, num2);
			}
			return new IntRef(num);
		}

		private static DataContract CreateDataContract(int id, RuntimeTypeHandle typeHandle, Type type)
		{
			DataContract dataContract = dataContractCache[id];
			if (dataContract == null)
			{
				lock (createDataContractLock)
				{
					dataContract = dataContractCache[id];
					if (dataContract == null)
					{
						if (type == null)
						{
							type = Type.GetTypeFromHandle(typeHandle);
						}
						type = UnwrapNullableType(type);
						type = GetDataContractAdapterType(type);
						dataContract = GetBuiltInDataContract(type);
						if (dataContract == null)
						{
							if (type.IsArray)
							{
								dataContract = new CollectionDataContract(type);
							}
							else if (type.IsEnum)
							{
								dataContract = new EnumDataContract(type);
							}
							else if (type.IsGenericParameter)
							{
								dataContract = new GenericParameterDataContract(type);
							}
							else if (Globals.TypeOfIXmlSerializable.IsAssignableFrom(type))
							{
								dataContract = new XmlDataContract(type);
							}
							else
							{
								if (type.IsPointer)
								{
									type = Globals.TypeOfReflectionPointer;
								}
								if (!CollectionDataContract.TryCreate(type, out dataContract))
								{
									if (type.IsSerializable || type.IsDefined(Globals.TypeOfDataContractAttribute, inherit: false) || ClassDataContract.IsNonAttributedTypeValidForSerialization(type))
									{
										dataContract = new ClassDataContract(type);
									}
									else
									{
										ThrowInvalidDataContractException(SR.GetString("Type '{0}' cannot be serialized. Consider marking it with the DataContractAttribute attribute, and marking all of its members you want serialized with the DataMemberAttribute attribute. Alternatively, you can ensure that the type is public and has a parameterless constructor - all public members of the type will then be serialized, and no attributes will be required.", type), type);
									}
								}
							}
						}
						AssignDataContractToId(dataContract, id);
					}
				}
			}
			return dataContract;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void AssignDataContractToId(DataContract dataContract, int id)
		{
			lock (cacheLock)
			{
				dataContractCache[id] = dataContract;
			}
		}

		private static DataContract CreateGetOnlyCollectionDataContract(int id, RuntimeTypeHandle typeHandle, Type type)
		{
			DataContract dataContract = null;
			lock (createDataContractLock)
			{
				dataContract = dataContractCache[id];
				if (dataContract == null)
				{
					if (type == null)
					{
						type = Type.GetTypeFromHandle(typeHandle);
					}
					type = UnwrapNullableType(type);
					type = GetDataContractAdapterType(type);
					if (!CollectionDataContract.TryCreateGetOnlyCollectionDataContract(type, out dataContract))
					{
						ThrowInvalidDataContractException(SR.GetString("Type '{0}' cannot be serialized. Consider marking it with the DataContractAttribute attribute, and marking all of its members you want serialized with the DataMemberAttribute attribute. Alternatively, you can ensure that the type is public and has a parameterless constructor - all public members of the type will then be serialized, and no attributes will be required.", type), type);
					}
				}
			}
			return dataContract;
		}

		internal static Type GetDataContractAdapterType(Type type)
		{
			if (type == Globals.TypeOfDateTimeOffset)
			{
				return Globals.TypeOfDateTimeOffsetAdapter;
			}
			return type;
		}

		internal static Type GetDataContractOriginalType(Type type)
		{
			if (type == Globals.TypeOfDateTimeOffsetAdapter)
			{
				return Globals.TypeOfDateTimeOffset;
			}
			return type;
		}

		private static RuntimeTypeHandle GetDataContractAdapterTypeHandle(RuntimeTypeHandle typeHandle)
		{
			if (Globals.TypeOfDateTimeOffset.TypeHandle.Equals(typeHandle))
			{
				return Globals.TypeOfDateTimeOffsetAdapter.TypeHandle;
			}
			return typeHandle;
		}

		public static DataContract GetBuiltInDataContract(Type type)
		{
			if (type.IsInterface && !CollectionDataContract.IsCollectionInterface(type))
			{
				type = Globals.TypeOfObject;
			}
			lock (initBuiltInContractsLock)
			{
				if (typeToBuiltInContract == null)
				{
					typeToBuiltInContract = new Dictionary<Type, DataContract>();
				}
				DataContract value = null;
				if (!typeToBuiltInContract.TryGetValue(type, out value))
				{
					TryCreateBuiltInDataContract(type, out value);
					typeToBuiltInContract.Add(type, value);
				}
				return value;
			}
		}

		public static DataContract GetBuiltInDataContract(string name, string ns)
		{
			lock (initBuiltInContractsLock)
			{
				if (nameToBuiltInContract == null)
				{
					nameToBuiltInContract = new Dictionary<XmlQualifiedName, DataContract>();
				}
				DataContract value = null;
				XmlQualifiedName key = new XmlQualifiedName(name, ns);
				if (!nameToBuiltInContract.TryGetValue(key, out value) && TryCreateBuiltInDataContract(name, ns, out value))
				{
					nameToBuiltInContract.Add(key, value);
				}
				return value;
			}
		}

		public static DataContract GetBuiltInDataContract(string typeName)
		{
			if (!typeName.StartsWith("System.", StringComparison.Ordinal))
			{
				return null;
			}
			lock (initBuiltInContractsLock)
			{
				if (typeNameToBuiltInContract == null)
				{
					typeNameToBuiltInContract = new Dictionary<string, DataContract>();
				}
				DataContract value = null;
				if (!typeNameToBuiltInContract.TryGetValue(typeName, out value))
				{
					Type type = null;
					switch (typeName.Substring(7))
					{
					case "Char":
						type = typeof(char);
						break;
					case "Boolean":
						type = typeof(bool);
						break;
					case "SByte":
						type = typeof(sbyte);
						break;
					case "Byte":
						type = typeof(byte);
						break;
					case "Int16":
						type = typeof(short);
						break;
					case "UInt16":
						type = typeof(ushort);
						break;
					case "Int32":
						type = typeof(int);
						break;
					case "UInt32":
						type = typeof(uint);
						break;
					case "Int64":
						type = typeof(long);
						break;
					case "UInt64":
						type = typeof(ulong);
						break;
					case "Single":
						type = typeof(float);
						break;
					case "Double":
						type = typeof(double);
						break;
					case "Decimal":
						type = typeof(decimal);
						break;
					case "DateTime":
						type = typeof(DateTime);
						break;
					case "String":
						type = typeof(string);
						break;
					case "Byte[]":
						type = typeof(byte[]);
						break;
					case "Object":
						type = typeof(object);
						break;
					case "TimeSpan":
						type = typeof(TimeSpan);
						break;
					case "Guid":
						type = typeof(Guid);
						break;
					case "Uri":
						type = typeof(Uri);
						break;
					case "Xml.XmlQualifiedName":
						type = typeof(XmlQualifiedName);
						break;
					case "Enum":
						type = typeof(Enum);
						break;
					case "ValueType":
						type = typeof(ValueType);
						break;
					case "Array":
						type = typeof(Array);
						break;
					case "Xml.XmlElement":
						type = typeof(XmlElement);
						break;
					case "Xml.XmlNode[]":
						type = typeof(XmlNode[]);
						break;
					}
					if (type != null)
					{
						TryCreateBuiltInDataContract(type, out value);
					}
					typeNameToBuiltInContract.Add(typeName, value);
				}
				return value;
			}
		}

		public static bool TryCreateBuiltInDataContract(Type type, out DataContract dataContract)
		{
			if (type.IsEnum)
			{
				dataContract = null;
				return false;
			}
			dataContract = null;
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
				dataContract = new BooleanDataContract();
				break;
			case TypeCode.Byte:
				dataContract = new UnsignedByteDataContract();
				break;
			case TypeCode.Char:
				dataContract = new CharDataContract();
				break;
			case TypeCode.DateTime:
				dataContract = new DateTimeDataContract();
				break;
			case TypeCode.Decimal:
				dataContract = new DecimalDataContract();
				break;
			case TypeCode.Double:
				dataContract = new DoubleDataContract();
				break;
			case TypeCode.Int16:
				dataContract = new ShortDataContract();
				break;
			case TypeCode.Int32:
				dataContract = new IntDataContract();
				break;
			case TypeCode.Int64:
				dataContract = new LongDataContract();
				break;
			case TypeCode.SByte:
				dataContract = new SignedByteDataContract();
				break;
			case TypeCode.Single:
				dataContract = new FloatDataContract();
				break;
			case TypeCode.String:
				dataContract = new StringDataContract();
				break;
			case TypeCode.UInt16:
				dataContract = new UnsignedShortDataContract();
				break;
			case TypeCode.UInt32:
				dataContract = new UnsignedIntDataContract();
				break;
			case TypeCode.UInt64:
				dataContract = new UnsignedLongDataContract();
				break;
			default:
				if (type == typeof(byte[]))
				{
					dataContract = new ByteArrayDataContract();
				}
				else if (type == typeof(object))
				{
					dataContract = new ObjectDataContract();
				}
				else if (type == typeof(Uri))
				{
					dataContract = new UriDataContract();
				}
				else if (type == typeof(XmlQualifiedName))
				{
					dataContract = new QNameDataContract();
				}
				else if (type == typeof(TimeSpan))
				{
					dataContract = new TimeSpanDataContract();
				}
				else if (type == typeof(Guid))
				{
					dataContract = new GuidDataContract();
				}
				else if (type == typeof(Enum) || type == typeof(ValueType))
				{
					dataContract = new SpecialTypeDataContract(type, DictionaryGlobals.ObjectLocalName, DictionaryGlobals.SchemaNamespace);
				}
				else if (type == typeof(Array))
				{
					dataContract = new CollectionDataContract(type);
				}
				else if (type == typeof(XmlElement) || type == typeof(XmlNode[]))
				{
					dataContract = new XmlDataContract(type);
				}
				break;
			}
			return dataContract != null;
		}

		public static bool TryCreateBuiltInDataContract(string name, string ns, out DataContract dataContract)
		{
			dataContract = null;
			if (ns == DictionaryGlobals.SchemaNamespace.Value)
			{
				if (DictionaryGlobals.BooleanLocalName.Value == name)
				{
					dataContract = new BooleanDataContract();
				}
				else if (DictionaryGlobals.SignedByteLocalName.Value == name)
				{
					dataContract = new SignedByteDataContract();
				}
				else if (DictionaryGlobals.UnsignedByteLocalName.Value == name)
				{
					dataContract = new UnsignedByteDataContract();
				}
				else if (DictionaryGlobals.ShortLocalName.Value == name)
				{
					dataContract = new ShortDataContract();
				}
				else if (DictionaryGlobals.UnsignedShortLocalName.Value == name)
				{
					dataContract = new UnsignedShortDataContract();
				}
				else if (DictionaryGlobals.IntLocalName.Value == name)
				{
					dataContract = new IntDataContract();
				}
				else if (DictionaryGlobals.UnsignedIntLocalName.Value == name)
				{
					dataContract = new UnsignedIntDataContract();
				}
				else if (DictionaryGlobals.LongLocalName.Value == name)
				{
					dataContract = new LongDataContract();
				}
				else if (DictionaryGlobals.integerLocalName.Value == name)
				{
					dataContract = new IntegerDataContract();
				}
				else if (DictionaryGlobals.positiveIntegerLocalName.Value == name)
				{
					dataContract = new PositiveIntegerDataContract();
				}
				else if (DictionaryGlobals.negativeIntegerLocalName.Value == name)
				{
					dataContract = new NegativeIntegerDataContract();
				}
				else if (DictionaryGlobals.nonPositiveIntegerLocalName.Value == name)
				{
					dataContract = new NonPositiveIntegerDataContract();
				}
				else if (DictionaryGlobals.nonNegativeIntegerLocalName.Value == name)
				{
					dataContract = new NonNegativeIntegerDataContract();
				}
				else if (DictionaryGlobals.UnsignedLongLocalName.Value == name)
				{
					dataContract = new UnsignedLongDataContract();
				}
				else if (DictionaryGlobals.FloatLocalName.Value == name)
				{
					dataContract = new FloatDataContract();
				}
				else if (DictionaryGlobals.DoubleLocalName.Value == name)
				{
					dataContract = new DoubleDataContract();
				}
				else if (DictionaryGlobals.DecimalLocalName.Value == name)
				{
					dataContract = new DecimalDataContract();
				}
				else if (DictionaryGlobals.DateTimeLocalName.Value == name)
				{
					dataContract = new DateTimeDataContract();
				}
				else if (DictionaryGlobals.StringLocalName.Value == name)
				{
					dataContract = new StringDataContract();
				}
				else if (DictionaryGlobals.timeLocalName.Value == name)
				{
					dataContract = new TimeDataContract();
				}
				else if (DictionaryGlobals.dateLocalName.Value == name)
				{
					dataContract = new DateDataContract();
				}
				else if (DictionaryGlobals.hexBinaryLocalName.Value == name)
				{
					dataContract = new HexBinaryDataContract();
				}
				else if (DictionaryGlobals.gYearMonthLocalName.Value == name)
				{
					dataContract = new GYearMonthDataContract();
				}
				else if (DictionaryGlobals.gYearLocalName.Value == name)
				{
					dataContract = new GYearDataContract();
				}
				else if (DictionaryGlobals.gMonthDayLocalName.Value == name)
				{
					dataContract = new GMonthDayDataContract();
				}
				else if (DictionaryGlobals.gDayLocalName.Value == name)
				{
					dataContract = new GDayDataContract();
				}
				else if (DictionaryGlobals.gMonthLocalName.Value == name)
				{
					dataContract = new GMonthDataContract();
				}
				else if (DictionaryGlobals.normalizedStringLocalName.Value == name)
				{
					dataContract = new NormalizedStringDataContract();
				}
				else if (DictionaryGlobals.tokenLocalName.Value == name)
				{
					dataContract = new TokenDataContract();
				}
				else if (DictionaryGlobals.languageLocalName.Value == name)
				{
					dataContract = new LanguageDataContract();
				}
				else if (DictionaryGlobals.NameLocalName.Value == name)
				{
					dataContract = new NameDataContract();
				}
				else if (DictionaryGlobals.NCNameLocalName.Value == name)
				{
					dataContract = new NCNameDataContract();
				}
				else if (DictionaryGlobals.XSDIDLocalName.Value == name)
				{
					dataContract = new IDDataContract();
				}
				else if (DictionaryGlobals.IDREFLocalName.Value == name)
				{
					dataContract = new IDREFDataContract();
				}
				else if (DictionaryGlobals.IDREFSLocalName.Value == name)
				{
					dataContract = new IDREFSDataContract();
				}
				else if (DictionaryGlobals.ENTITYLocalName.Value == name)
				{
					dataContract = new ENTITYDataContract();
				}
				else if (DictionaryGlobals.ENTITIESLocalName.Value == name)
				{
					dataContract = new ENTITIESDataContract();
				}
				else if (DictionaryGlobals.NMTOKENLocalName.Value == name)
				{
					dataContract = new NMTOKENDataContract();
				}
				else if (DictionaryGlobals.NMTOKENSLocalName.Value == name)
				{
					dataContract = new NMTOKENDataContract();
				}
				else if (DictionaryGlobals.ByteArrayLocalName.Value == name)
				{
					dataContract = new ByteArrayDataContract();
				}
				else if (DictionaryGlobals.ObjectLocalName.Value == name)
				{
					dataContract = new ObjectDataContract();
				}
				else if (DictionaryGlobals.TimeSpanLocalName.Value == name)
				{
					dataContract = new XsDurationDataContract();
				}
				else if (DictionaryGlobals.UriLocalName.Value == name)
				{
					dataContract = new UriDataContract();
				}
				else if (DictionaryGlobals.QNameLocalName.Value == name)
				{
					dataContract = new QNameDataContract();
				}
			}
			else if (ns == DictionaryGlobals.SerializationNamespace.Value)
			{
				if (DictionaryGlobals.TimeSpanLocalName.Value == name)
				{
					dataContract = new TimeSpanDataContract();
				}
				else if (DictionaryGlobals.GuidLocalName.Value == name)
				{
					dataContract = new GuidDataContract();
				}
				else if (DictionaryGlobals.CharLocalName.Value == name)
				{
					dataContract = new CharDataContract();
				}
				else if ("ArrayOfanyType" == name)
				{
					dataContract = new CollectionDataContract(typeof(Array));
				}
			}
			else if (ns == DictionaryGlobals.AsmxTypesNamespace.Value)
			{
				if (DictionaryGlobals.CharLocalName.Value == name)
				{
					dataContract = new AsmxCharDataContract();
				}
				else if (DictionaryGlobals.GuidLocalName.Value == name)
				{
					dataContract = new AsmxGuidDataContract();
				}
			}
			else if (ns == "http://schemas.datacontract.org/2004/07/System.Xml")
			{
				if (name == "XmlElement")
				{
					dataContract = new XmlDataContract(typeof(XmlElement));
				}
				else if (name == "ArrayOfXmlNode")
				{
					dataContract = new XmlDataContract(typeof(XmlNode[]));
				}
			}
			return dataContract != null;
		}

		internal static string GetNamespace(string key)
		{
			lock (namespacesLock)
			{
				if (namespaces == null)
				{
					namespaces = new Dictionary<string, string>();
				}
				if (namespaces.TryGetValue(key, out var value))
				{
					return value;
				}
				try
				{
					namespaces.Add(key, key);
				}
				catch (Exception ex)
				{
					if (Fx.IsFatal(ex))
					{
						throw;
					}
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperFatal(ex.Message, ex);
				}
				return key;
			}
		}

		internal static XmlDictionaryString GetClrTypeString(string key)
		{
			lock (clrTypeStringsLock)
			{
				if (clrTypeStrings == null)
				{
					clrTypeStringsDictionary = new XmlDictionary();
					clrTypeStrings = new Dictionary<string, XmlDictionaryString>();
					try
					{
						clrTypeStrings.Add(Globals.TypeOfInt.Assembly.FullName, clrTypeStringsDictionary.Add("0"));
					}
					catch (Exception ex)
					{
						if (Fx.IsFatal(ex))
						{
							throw;
						}
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperFatal(ex.Message, ex);
					}
				}
				if (clrTypeStrings.TryGetValue(key, out var value))
				{
					return value;
				}
				value = clrTypeStringsDictionary.Add(key);
				try
				{
					clrTypeStrings.Add(key, value);
				}
				catch (Exception ex2)
				{
					if (Fx.IsFatal(ex2))
					{
						throw;
					}
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperFatal(ex2.Message, ex2);
				}
				return value;
			}
		}

		internal static void ThrowInvalidDataContractException(string message, Type type)
		{
			if (type != null)
			{
				lock (cacheLock)
				{
					typeHandleRef.Value = GetDataContractAdapterTypeHandle(type.TypeHandle);
					try
					{
						typeToIDCache.Remove(typeHandleRef);
					}
					catch (Exception ex)
					{
						if (Fx.IsFatal(ex))
						{
							throw;
						}
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperFatal(ex.Message, ex);
					}
				}
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(message));
		}

		internal DataContractCriticalHelper()
		{
		}

		internal DataContractCriticalHelper(Type type)
		{
			underlyingType = type;
			SetTypeForInitialization(type);
			isValueType = type.IsValueType;
		}

		[SecuritySafeCritical]
		private void SetTypeForInitialization(Type classType)
		{
			if (classType.IsSerializable || classType.IsDefined(Globals.TypeOfDataContractAttribute, inherit: false))
			{
				typeForInitialization = classType;
			}
		}

		internal virtual void WriteRootElement(XmlWriterDelegator writer, XmlDictionaryString name, XmlDictionaryString ns)
		{
			if (ns == DictionaryGlobals.SerializationNamespace && !IsPrimitive)
			{
				writer.WriteStartElement("z", name, ns);
			}
			else
			{
				writer.WriteStartElement(name, ns);
			}
		}

		internal void SetDataContractName(XmlQualifiedName stableName)
		{
			XmlDictionary xmlDictionary = new XmlDictionary(2);
			Name = xmlDictionary.Add(stableName.Name);
			Namespace = xmlDictionary.Add(stableName.Namespace);
			StableName = stableName;
		}

		internal void SetDataContractName(XmlDictionaryString name, XmlDictionaryString ns)
		{
			Name = name;
			Namespace = ns;
			StableName = CreateQualifiedName(name.Value, ns.Value);
		}

		internal void ThrowInvalidDataContractException(string message)
		{
			ThrowInvalidDataContractException(message, UnderlyingType);
		}
	}

	[SecurityCritical]
	private XmlDictionaryString name;

	[SecurityCritical]
	private XmlDictionaryString ns;

	[SecurityCritical]
	private DataContractCriticalHelper helper;

	protected DataContractCriticalHelper Helper
	{
		[SecurityCritical]
		get
		{
			return helper;
		}
	}

	internal Type UnderlyingType
	{
		[SecuritySafeCritical]
		get
		{
			return helper.UnderlyingType;
		}
	}

	internal Type OriginalUnderlyingType
	{
		[SecuritySafeCritical]
		get
		{
			return helper.OriginalUnderlyingType;
		}
	}

	internal virtual bool IsBuiltInDataContract
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsBuiltInDataContract;
		}
	}

	internal Type TypeForInitialization
	{
		[SecuritySafeCritical]
		get
		{
			return helper.TypeForInitialization;
		}
	}

	internal bool IsValueType
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsValueType;
		}
		[SecurityCritical]
		set
		{
			helper.IsValueType = value;
		}
	}

	internal bool IsReference
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsReference;
		}
		[SecurityCritical]
		set
		{
			helper.IsReference = value;
		}
	}

	internal XmlQualifiedName StableName
	{
		[SecuritySafeCritical]
		get
		{
			return helper.StableName;
		}
		[SecurityCritical]
		set
		{
			helper.StableName = value;
		}
	}

	internal GenericInfo GenericInfo
	{
		[SecuritySafeCritical]
		get
		{
			return helper.GenericInfo;
		}
		[SecurityCritical]
		set
		{
			helper.GenericInfo = value;
		}
	}

	internal virtual Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
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

	internal virtual bool IsISerializable
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

	internal XmlDictionaryString Name
	{
		[SecuritySafeCritical]
		get
		{
			return name;
		}
	}

	public virtual XmlDictionaryString Namespace
	{
		[SecuritySafeCritical]
		get
		{
			return ns;
		}
	}

	internal virtual bool HasRoot
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	internal virtual XmlDictionaryString TopLevelElementName
	{
		[SecuritySafeCritical]
		get
		{
			return helper.TopLevelElementName;
		}
		[SecurityCritical]
		set
		{
			helper.TopLevelElementName = value;
		}
	}

	internal virtual XmlDictionaryString TopLevelElementNamespace
	{
		[SecuritySafeCritical]
		get
		{
			return helper.TopLevelElementNamespace;
		}
		[SecurityCritical]
		set
		{
			helper.TopLevelElementNamespace = value;
		}
	}

	internal virtual bool CanContainReferences => true;

	internal virtual bool IsPrimitive => false;

	internal MethodInfo ParseMethod
	{
		[SecuritySafeCritical]
		get
		{
			return helper.ParseMethod;
		}
	}

	[SecuritySafeCritical]
	protected DataContract(DataContractCriticalHelper helper)
	{
		this.helper = helper;
		name = helper.Name;
		ns = helper.Namespace;
	}

	internal static DataContract GetDataContract(Type type)
	{
		return GetDataContract(type.TypeHandle, type, SerializationMode.SharedContract);
	}

	internal static DataContract GetDataContract(RuntimeTypeHandle typeHandle, Type type, SerializationMode mode)
	{
		return GetDataContract(GetId(typeHandle), typeHandle, mode);
	}

	internal static DataContract GetDataContract(int id, RuntimeTypeHandle typeHandle, SerializationMode mode)
	{
		return GetDataContractSkipValidation(id, typeHandle, null).GetValidContract(mode);
	}

	[SecuritySafeCritical]
	internal static DataContract GetDataContractSkipValidation(int id, RuntimeTypeHandle typeHandle, Type type)
	{
		return DataContractCriticalHelper.GetDataContractSkipValidation(id, typeHandle, type);
	}

	internal static DataContract GetGetOnlyCollectionDataContract(int id, RuntimeTypeHandle typeHandle, Type type, SerializationMode mode)
	{
		DataContract getOnlyCollectionDataContractSkipValidation = GetGetOnlyCollectionDataContractSkipValidation(id, typeHandle, type);
		getOnlyCollectionDataContractSkipValidation = getOnlyCollectionDataContractSkipValidation.GetValidContract(mode);
		if (getOnlyCollectionDataContractSkipValidation is ClassDataContract)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("For '{0}' type, class data contract was returned for get-only collection.", GetClrTypeFullName(getOnlyCollectionDataContractSkipValidation.UnderlyingType))));
		}
		return getOnlyCollectionDataContractSkipValidation;
	}

	[SecuritySafeCritical]
	internal static DataContract GetGetOnlyCollectionDataContractSkipValidation(int id, RuntimeTypeHandle typeHandle, Type type)
	{
		return DataContractCriticalHelper.GetGetOnlyCollectionDataContractSkipValidation(id, typeHandle, type);
	}

	[SecuritySafeCritical]
	internal static DataContract GetDataContractForInitialization(int id)
	{
		return DataContractCriticalHelper.GetDataContractForInitialization(id);
	}

	[SecuritySafeCritical]
	internal static int GetIdForInitialization(ClassDataContract classContract)
	{
		return DataContractCriticalHelper.GetIdForInitialization(classContract);
	}

	[SecuritySafeCritical]
	internal static int GetId(RuntimeTypeHandle typeHandle)
	{
		return DataContractCriticalHelper.GetId(typeHandle);
	}

	[SecuritySafeCritical]
	public static DataContract GetBuiltInDataContract(Type type)
	{
		return DataContractCriticalHelper.GetBuiltInDataContract(type);
	}

	[SecuritySafeCritical]
	public static DataContract GetBuiltInDataContract(string name, string ns)
	{
		return DataContractCriticalHelper.GetBuiltInDataContract(name, ns);
	}

	[SecuritySafeCritical]
	public static DataContract GetBuiltInDataContract(string typeName)
	{
		return DataContractCriticalHelper.GetBuiltInDataContract(typeName);
	}

	[SecuritySafeCritical]
	internal static string GetNamespace(string key)
	{
		return DataContractCriticalHelper.GetNamespace(key);
	}

	[SecuritySafeCritical]
	internal static XmlDictionaryString GetClrTypeString(string key)
	{
		return DataContractCriticalHelper.GetClrTypeString(key);
	}

	[SecuritySafeCritical]
	internal static void ThrowInvalidDataContractException(string message, Type type)
	{
		DataContractCriticalHelper.ThrowInvalidDataContractException(message, type);
	}

	public virtual void WriteXmlValue(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("An internal error has occurred. Unexpected contract type '{0}' for type '{1}' encountered.", GetClrTypeFullName(GetType()), GetClrTypeFullName(UnderlyingType))));
	}

	public virtual object ReadXmlValue(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context)
	{
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("An internal error has occurred. Unexpected contract type '{0}' for type '{1}' encountered.", GetClrTypeFullName(GetType()), GetClrTypeFullName(UnderlyingType))));
	}

	internal virtual void WriteRootElement(XmlWriterDelegator writer, XmlDictionaryString name, XmlDictionaryString ns)
	{
		if (ns == DictionaryGlobals.SerializationNamespace && !IsPrimitive)
		{
			writer.WriteStartElement("z", name, ns);
		}
		else
		{
			writer.WriteStartElement(name, ns);
		}
	}

	internal virtual DataContract BindGenericParameters(DataContract[] paramContracts, Dictionary<DataContract, DataContract> boundContracts)
	{
		return this;
	}

	internal virtual DataContract GetValidContract(SerializationMode mode)
	{
		return this;
	}

	internal virtual DataContract GetValidContract()
	{
		return this;
	}

	internal virtual bool IsValidContract(SerializationMode mode)
	{
		return true;
	}

	internal static bool IsTypeSerializable(Type type)
	{
		return IsTypeSerializable(type, new Dictionary<Type, object>());
	}

	private static bool IsTypeSerializable(Type type, Dictionary<Type, object> previousCollectionTypes)
	{
		if (type.IsSerializable || type.IsDefined(Globals.TypeOfDataContractAttribute, inherit: false) || type.IsInterface || type.IsPointer || Globals.TypeOfIXmlSerializable.IsAssignableFrom(type))
		{
			return true;
		}
		if (CollectionDataContract.IsCollection(type, out var itemType))
		{
			ValidatePreviousCollectionTypes(type, itemType, previousCollectionTypes);
			if (IsTypeSerializable(itemType, previousCollectionTypes))
			{
				return true;
			}
		}
		if (GetBuiltInDataContract(type) == null)
		{
			return ClassDataContract.IsNonAttributedTypeValidForSerialization(type);
		}
		return true;
	}

	private static void ValidatePreviousCollectionTypes(Type collectionType, Type itemType, Dictionary<Type, object> previousCollectionTypes)
	{
		previousCollectionTypes.Add(collectionType, collectionType);
		while (itemType.IsArray)
		{
			itemType = itemType.GetElementType();
		}
		List<Type> list = new List<Type>();
		Queue<Type> queue = new Queue<Type>();
		queue.Enqueue(itemType);
		list.Add(itemType);
		while (queue.Count > 0)
		{
			itemType = queue.Dequeue();
			if (previousCollectionTypes.ContainsKey(itemType))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' involves recursive collection.", GetClrTypeFullName(itemType))));
			}
			if (!itemType.IsGenericType)
			{
				continue;
			}
			Type[] genericArguments = itemType.GetGenericArguments();
			foreach (Type item in genericArguments)
			{
				if (!list.Contains(item))
				{
					queue.Enqueue(item);
					list.Add(item);
				}
			}
		}
	}

	internal static Type UnwrapRedundantNullableType(Type type)
	{
		Type result = type;
		while (type.IsGenericType && type.GetGenericTypeDefinition() == Globals.TypeOfNullable)
		{
			result = type;
			type = type.GetGenericArguments()[0];
		}
		return result;
	}

	internal static Type UnwrapNullableType(Type type)
	{
		while (type.IsGenericType && type.GetGenericTypeDefinition() == Globals.TypeOfNullable)
		{
			type = type.GetGenericArguments()[0];
		}
		return type;
	}

	private static bool IsAlpha(char ch)
	{
		if (ch < 'A' || ch > 'Z')
		{
			if (ch >= 'a')
			{
				return ch <= 'z';
			}
			return false;
		}
		return true;
	}

	private static bool IsDigit(char ch)
	{
		if (ch >= '0')
		{
			return ch <= '9';
		}
		return false;
	}

	private static bool IsAsciiLocalName(string localName)
	{
		if (localName.Length == 0)
		{
			return false;
		}
		if (!IsAlpha(localName[0]))
		{
			return false;
		}
		for (int i = 1; i < localName.Length; i++)
		{
			char ch = localName[i];
			if (!IsAlpha(ch) && !IsDigit(ch))
			{
				return false;
			}
		}
		return true;
	}

	internal static string EncodeLocalName(string localName)
	{
		if (IsAsciiLocalName(localName))
		{
			return localName;
		}
		if (IsValidNCName(localName))
		{
			return localName;
		}
		return XmlConvert.EncodeLocalName(localName);
	}

	internal static bool IsValidNCName(string name)
	{
		try
		{
			XmlConvert.VerifyNCName(name);
			return true;
		}
		catch (XmlException)
		{
			return false;
		}
	}

	internal static XmlQualifiedName GetStableName(Type type)
	{
		bool hasDataContract;
		return GetStableName(type, out hasDataContract);
	}

	internal static XmlQualifiedName GetStableName(Type type, out bool hasDataContract)
	{
		return GetStableName(type, new Dictionary<Type, object>(), out hasDataContract);
	}

	private static XmlQualifiedName GetStableName(Type type, Dictionary<Type, object> previousCollectionTypes, out bool hasDataContract)
	{
		type = UnwrapRedundantNullableType(type);
		DataContractAttribute dataContractAttribute;
		if (TryGetBuiltInXmlAndArrayTypeStableName(type, previousCollectionTypes, out var stableName))
		{
			hasDataContract = false;
		}
		else if (TryGetDCAttribute(type, out dataContractAttribute))
		{
			stableName = GetDCTypeStableName(type, dataContractAttribute);
			hasDataContract = true;
		}
		else
		{
			stableName = GetNonDCTypeStableName(type, previousCollectionTypes);
			hasDataContract = false;
		}
		return stableName;
	}

	private static XmlQualifiedName GetDCTypeStableName(Type type, DataContractAttribute dataContractAttribute)
	{
		string text = null;
		string text2 = null;
		if (dataContractAttribute.IsNameSetExplicitly)
		{
			text = dataContractAttribute.Name;
			if (text == null || text.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have DataContractAttribute attribute Name set to null or empty string.", GetClrTypeFullName(type))));
			}
			if (type.IsGenericType && !type.IsGenericTypeDefinition)
			{
				text = ExpandGenericParameters(text, type);
			}
			text = EncodeLocalName(text);
		}
		else
		{
			text = GetDefaultStableLocalName(type);
		}
		if (dataContractAttribute.IsNamespaceSetExplicitly)
		{
			text2 = dataContractAttribute.Namespace;
			if (text2 == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have DataContractAttribute attribute Namespace set to null.", GetClrTypeFullName(type))));
			}
			CheckExplicitDataContractNamespaceUri(text2, type);
		}
		else
		{
			text2 = GetDefaultDataContractNamespace(type);
		}
		return CreateQualifiedName(text, text2);
	}

	private static XmlQualifiedName GetNonDCTypeStableName(Type type, Dictionary<Type, object> previousCollectionTypes)
	{
		string text = null;
		if (CollectionDataContract.IsCollection(type, out var itemType))
		{
			ValidatePreviousCollectionTypes(type, itemType, previousCollectionTypes);
			CollectionDataContractAttribute collectionContractAttribute;
			return GetCollectionStableName(type, itemType, previousCollectionTypes, out collectionContractAttribute);
		}
		string defaultStableLocalName = GetDefaultStableLocalName(type);
		text = ((!ClassDataContract.IsNonAttributedTypeValidForSerialization(type)) ? GetDefaultStableNamespace(type) : GetDefaultDataContractNamespace(type));
		return CreateQualifiedName(defaultStableLocalName, text);
	}

	private static bool TryGetBuiltInXmlAndArrayTypeStableName(Type type, Dictionary<Type, object> previousCollectionTypes, out XmlQualifiedName stableName)
	{
		stableName = null;
		DataContract builtInDataContract = GetBuiltInDataContract(type);
		if (builtInDataContract != null)
		{
			stableName = builtInDataContract.StableName;
		}
		else if (Globals.TypeOfIXmlSerializable.IsAssignableFrom(type))
		{
			SchemaExporter.GetXmlTypeInfo(type, out var stableName2, out var _, out var _);
			stableName = stableName2;
		}
		else if (type.IsArray)
		{
			Type elementType = type.GetElementType();
			ValidatePreviousCollectionTypes(type, elementType, previousCollectionTypes);
			stableName = GetCollectionStableName(type, elementType, previousCollectionTypes, out var _);
		}
		return stableName != null;
	}

	[SecuritySafeCritical]
	internal static bool TryGetDCAttribute(Type type, out DataContractAttribute dataContractAttribute)
	{
		dataContractAttribute = null;
		object[] customAttributes = type.GetCustomAttributes(Globals.TypeOfDataContractAttribute, inherit: false);
		if (customAttributes != null && customAttributes.Length != 0)
		{
			dataContractAttribute = (DataContractAttribute)customAttributes[0];
		}
		return dataContractAttribute != null;
	}

	internal static XmlQualifiedName GetCollectionStableName(Type type, Type itemType, out CollectionDataContractAttribute collectionContractAttribute)
	{
		return GetCollectionStableName(type, itemType, new Dictionary<Type, object>(), out collectionContractAttribute);
	}

	private static XmlQualifiedName GetCollectionStableName(Type type, Type itemType, Dictionary<Type, object> previousCollectionTypes, out CollectionDataContractAttribute collectionContractAttribute)
	{
		object[] customAttributes = type.GetCustomAttributes(Globals.TypeOfCollectionDataContractAttribute, inherit: false);
		string text;
		string text2;
		if (customAttributes != null && customAttributes.Length != 0)
		{
			collectionContractAttribute = (CollectionDataContractAttribute)customAttributes[0];
			if (collectionContractAttribute.IsNameSetExplicitly)
			{
				text = collectionContractAttribute.Name;
				if (text == null || text.Length == 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have CollectionDataContractAttribute attribute Name set to null or empty string.", GetClrTypeFullName(type))));
				}
				if (type.IsGenericType && !type.IsGenericTypeDefinition)
				{
					text = ExpandGenericParameters(text, type);
				}
				text = EncodeLocalName(text);
			}
			else
			{
				text = GetDefaultStableLocalName(type);
			}
			if (collectionContractAttribute.IsNamespaceSetExplicitly)
			{
				text2 = collectionContractAttribute.Namespace;
				if (text2 == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have CollectionDataContractAttribute attribute Namespace set to null.", GetClrTypeFullName(type))));
				}
				CheckExplicitDataContractNamespaceUri(text2, type);
			}
			else
			{
				text2 = GetDefaultDataContractNamespace(type);
			}
		}
		else
		{
			collectionContractAttribute = null;
			string text3 = "ArrayOf" + GetArrayPrefix(ref itemType);
			bool hasDataContract;
			XmlQualifiedName stableName = GetStableName(itemType, previousCollectionTypes, out hasDataContract);
			text = text3 + stableName.Name;
			text2 = GetCollectionNamespace(stableName.Namespace);
		}
		return CreateQualifiedName(text, text2);
	}

	private static string GetArrayPrefix(ref Type itemType)
	{
		string text = string.Empty;
		while (itemType.IsArray && GetBuiltInDataContract(itemType) == null)
		{
			text += "ArrayOf";
			itemType = itemType.GetElementType();
		}
		return text;
	}

	internal XmlQualifiedName GetArrayTypeName(bool isNullable)
	{
		XmlQualifiedName xmlQualifiedName;
		if (IsValueType && isNullable)
		{
			GenericInfo genericInfo = new GenericInfo(GetStableName(Globals.TypeOfNullable), Globals.TypeOfNullable.FullName);
			genericInfo.Add(new GenericInfo(StableName, null));
			genericInfo.AddToLevel(0, 1);
			xmlQualifiedName = genericInfo.GetExpandedStableName();
		}
		else
		{
			xmlQualifiedName = StableName;
		}
		string collectionNamespace = GetCollectionNamespace(xmlQualifiedName.Namespace);
		return new XmlQualifiedName("ArrayOf" + xmlQualifiedName.Name, collectionNamespace);
	}

	internal static string GetCollectionNamespace(string elementNs)
	{
		if (!IsBuiltInNamespace(elementNs))
		{
			return elementNs;
		}
		return "http://schemas.microsoft.com/2003/10/Serialization/Arrays";
	}

	internal static XmlQualifiedName GetDefaultStableName(Type type)
	{
		return CreateQualifiedName(GetDefaultStableLocalName(type), GetDefaultStableNamespace(type));
	}

	private static string GetDefaultStableLocalName(Type type)
	{
		if (type.IsGenericParameter)
		{
			return "{" + type.GenericParameterPosition + "}";
		}
		string text = null;
		if (type.IsArray)
		{
			text = GetArrayPrefix(ref type);
		}
		string text2;
		if (type.DeclaringType == null)
		{
			text2 = type.Name;
		}
		else
		{
			int num = ((type.Namespace != null) ? type.Namespace.Length : 0);
			if (num > 0)
			{
				num++;
			}
			text2 = GetClrTypeFullName(type).Substring(num).Replace('+', '.');
		}
		if (text != null)
		{
			text2 = text + text2;
		}
		if (type.IsGenericType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			bool flag = true;
			int num2 = text2.IndexOf('[');
			if (num2 >= 0)
			{
				text2 = text2.Substring(0, num2);
			}
			IList<int> dataContractNameForGenericName = GetDataContractNameForGenericName(text2, stringBuilder);
			bool isGenericTypeDefinition = type.IsGenericTypeDefinition;
			Type[] genericArguments = type.GetGenericArguments();
			for (int i = 0; i < genericArguments.Length; i++)
			{
				Type type2 = genericArguments[i];
				if (isGenericTypeDefinition)
				{
					stringBuilder.Append("{").Append(i).Append("}");
					continue;
				}
				XmlQualifiedName stableName = GetStableName(type2);
				stringBuilder.Append(stableName.Name);
				stringBuilder2.Append(" ").Append(stableName.Namespace);
				if (flag)
				{
					flag = IsBuiltInNamespace(stableName.Namespace);
				}
			}
			if (isGenericTypeDefinition)
			{
				stringBuilder.Append("{#}");
			}
			else if (dataContractNameForGenericName.Count > 1 || !flag)
			{
				foreach (int item in dataContractNameForGenericName)
				{
					stringBuilder2.Insert(0, item).Insert(0, " ");
				}
				stringBuilder.Append(GetNamespacesDigest(stringBuilder2.ToString()));
			}
			text2 = stringBuilder.ToString();
		}
		return EncodeLocalName(text2);
	}

	private static string GetDefaultDataContractNamespace(Type type)
	{
		string text = type.Namespace;
		if (text == null)
		{
			text = string.Empty;
		}
		string text2 = GetGlobalDataContractNamespace(text, type.Module);
		if (text2 == null)
		{
			text2 = GetGlobalDataContractNamespace(text, type.Assembly);
		}
		if (text2 == null)
		{
			text2 = GetDefaultStableNamespace(type);
		}
		else
		{
			CheckExplicitDataContractNamespaceUri(text2, type);
		}
		return text2;
	}

	internal static IList<int> GetDataContractNameForGenericName(string typeName, StringBuilder localName)
	{
		List<int> list = new List<int>();
		int num = 0;
		while (true)
		{
			int num2 = typeName.IndexOf('`', num);
			if (num2 < 0)
			{
				localName?.Append(typeName.Substring(num));
				list.Add(0);
				break;
			}
			localName?.Append(typeName.Substring(num, num2 - num));
			while ((num = typeName.IndexOf('.', num + 1, num2 - num - 1)) >= 0)
			{
				list.Add(0);
			}
			num = typeName.IndexOf('.', num2);
			if (num < 0)
			{
				list.Add(int.Parse(typeName.Substring(num2 + 1), CultureInfo.InvariantCulture));
				break;
			}
			list.Add(int.Parse(typeName.Substring(num2 + 1, num - num2 - 1), CultureInfo.InvariantCulture));
		}
		localName?.Append("Of");
		return list;
	}

	internal static bool IsBuiltInNamespace(string ns)
	{
		if (!(ns == "http://www.w3.org/2001/XMLSchema"))
		{
			return ns == "http://schemas.microsoft.com/2003/10/Serialization/";
		}
		return true;
	}

	internal static string GetDefaultStableNamespace(Type type)
	{
		if (type.IsGenericParameter)
		{
			return "{ns}";
		}
		return GetDefaultStableNamespace(type.Namespace);
	}

	internal static XmlQualifiedName CreateQualifiedName(string localName, string ns)
	{
		return new XmlQualifiedName(localName, GetNamespace(ns));
	}

	internal static string GetDefaultStableNamespace(string clrNs)
	{
		if (clrNs == null)
		{
			clrNs = string.Empty;
		}
		return new Uri(Globals.DataContractXsdBaseNamespaceUri, clrNs).AbsoluteUri;
	}

	private static void CheckExplicitDataContractNamespaceUri(string dataContractNs, Type type)
	{
		if (dataContractNs.Length > 0)
		{
			string text = dataContractNs.Trim();
			if (text.Length == 0 || text.IndexOf("##", StringComparison.Ordinal) != -1)
			{
				ThrowInvalidDataContractException(SR.GetString("DataContract namespace '{0}' is not a valid URI.", dataContractNs), type);
			}
			dataContractNs = text;
		}
		if (Uri.TryCreate(dataContractNs, UriKind.RelativeOrAbsolute, out var result))
		{
			if (result.ToString() == "http://schemas.microsoft.com/2003/10/Serialization/")
			{
				ThrowInvalidDataContractException(SR.GetString("DataContract namespace '{0}' cannot be specified since it is reserved.", "http://schemas.microsoft.com/2003/10/Serialization/"), type);
			}
		}
		else
		{
			ThrowInvalidDataContractException(SR.GetString("DataContract namespace '{0}' is not a valid URI.", dataContractNs), type);
		}
	}

	internal static string GetClrTypeFullName(Type type)
	{
		if (type.IsGenericTypeDefinition || !type.ContainsGenericParameters)
		{
			return type.FullName;
		}
		return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", type.Namespace, type.Name);
	}

	internal static string GetClrAssemblyName(Type type, out bool hasTypeForwardedFrom)
	{
		hasTypeForwardedFrom = false;
		object[] customAttributes = type.GetCustomAttributes(typeof(TypeForwardedFromAttribute), inherit: false);
		if (customAttributes != null && customAttributes.Length != 0)
		{
			TypeForwardedFromAttribute obj = (TypeForwardedFromAttribute)customAttributes[0];
			hasTypeForwardedFrom = true;
			return obj.AssemblyFullName;
		}
		return type.Assembly.FullName;
	}

	internal static string GetClrTypeFullNameUsingTypeForwardedFromAttribute(Type type)
	{
		if (type.IsArray)
		{
			return GetClrTypeFullNameForArray(type);
		}
		return GetClrTypeFullNameForNonArrayTypes(type);
	}

	private static string GetClrTypeFullNameForArray(Type type)
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", GetClrTypeFullNameUsingTypeForwardedFromAttribute(type.GetElementType()), "[", "]");
	}

	private static string GetClrTypeFullNameForNonArrayTypes(Type type)
	{
		if (!type.IsGenericType)
		{
			return GetClrTypeFullName(type);
		}
		Type[] genericArguments = type.GetGenericArguments();
		StringBuilder stringBuilder = new StringBuilder(type.GetGenericTypeDefinition().FullName).Append("[");
		Type[] array = genericArguments;
		foreach (Type type2 in array)
		{
			stringBuilder.Append("[").Append(GetClrTypeFullNameUsingTypeForwardedFromAttribute(type2)).Append(",");
			stringBuilder.Append(" ").Append(GetClrAssemblyName(type2, out var _));
			stringBuilder.Append("]").Append(",");
		}
		return stringBuilder.Remove(stringBuilder.Length - 1, 1).Append("]").ToString();
	}

	internal static void GetClrNameAndNamespace(string fullTypeName, out string localName, out string ns)
	{
		int num = fullTypeName.LastIndexOf('.');
		if (num < 0)
		{
			ns = string.Empty;
			localName = fullTypeName.Replace('+', '.');
		}
		else
		{
			ns = fullTypeName.Substring(0, num);
			localName = fullTypeName.Substring(num + 1).Replace('+', '.');
		}
		int num2 = localName.IndexOf('[');
		if (num2 >= 0)
		{
			localName = localName.Substring(0, num2);
		}
	}

	internal static void GetDefaultStableName(string fullTypeName, out string localName, out string ns)
	{
		GetDefaultStableName(new CodeTypeReference(fullTypeName), out localName, out ns);
	}

	private static void GetDefaultStableName(CodeTypeReference typeReference, out string localName, out string ns)
	{
		string baseType = typeReference.BaseType;
		DataContract builtInDataContract = GetBuiltInDataContract(baseType);
		if (builtInDataContract != null)
		{
			localName = builtInDataContract.StableName.Name;
			ns = builtInDataContract.StableName.Namespace;
			return;
		}
		GetClrNameAndNamespace(baseType, out localName, out ns);
		if (typeReference.TypeArguments.Count > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			bool flag = true;
			IList<int> dataContractNameForGenericName = GetDataContractNameForGenericName(localName, stringBuilder);
			foreach (CodeTypeReference typeArgument in typeReference.TypeArguments)
			{
				GetDefaultStableName(typeArgument, out var localName2, out var value);
				stringBuilder.Append(localName2);
				stringBuilder2.Append(" ").Append(value);
				if (flag)
				{
					flag = IsBuiltInNamespace(value);
				}
			}
			if (dataContractNameForGenericName.Count > 1 || !flag)
			{
				foreach (int item in dataContractNameForGenericName)
				{
					stringBuilder2.Insert(0, item).Insert(0, " ");
				}
				stringBuilder.Append(GetNamespacesDigest(stringBuilder2.ToString()));
			}
			localName = stringBuilder.ToString();
		}
		localName = EncodeLocalName(localName);
		ns = GetDefaultStableNamespace(ns);
	}

	internal static string GetDataContractNamespaceFromUri(string uriString)
	{
		if (!uriString.StartsWith("http://schemas.datacontract.org/2004/07/", StringComparison.Ordinal))
		{
			return uriString;
		}
		return uriString.Substring("http://schemas.datacontract.org/2004/07/".Length);
	}

	private static string GetGlobalDataContractNamespace(string clrNs, ICustomAttributeProvider customAttribuetProvider)
	{
		object[] customAttributes = customAttribuetProvider.GetCustomAttributes(typeof(ContractNamespaceAttribute), inherit: false);
		string text = null;
		for (int i = 0; i < customAttributes.Length; i++)
		{
			ContractNamespaceAttribute contractNamespaceAttribute = (ContractNamespaceAttribute)customAttributes[i];
			string text2 = contractNamespaceAttribute.ClrNamespace;
			if (text2 == null)
			{
				text2 = string.Empty;
			}
			if (text2 == clrNs)
			{
				if (contractNamespaceAttribute.ContractNamespace == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("CLR namespace '{0}' cannot have ContractNamespace set to null.", clrNs)));
				}
				if (text != null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("ContractNamespaceAttribute attribute maps CLR namespace '{2}' to multiple data contract namespaces '{0}' and '{1}'. You can map a CLR namespace to only one data contract namespace.", text, contractNamespaceAttribute.ContractNamespace, clrNs)));
				}
				text = contractNamespaceAttribute.ContractNamespace;
			}
		}
		return text;
	}

	private static string GetNamespacesDigest(string namespaces)
	{
		byte[] inArray = HashHelper.ComputeHash(Encoding.UTF8.GetBytes(namespaces));
		char[] array = new char[24];
		int num = Convert.ToBase64CharArray(inArray, 0, 6, array, 0);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < num; i++)
		{
			char c = array[i];
			switch (c)
			{
			case '/':
				stringBuilder.Append("_S");
				break;
			case '+':
				stringBuilder.Append("_P");
				break;
			default:
				stringBuilder.Append(c);
				break;
			case '=':
				break;
			}
		}
		return stringBuilder.ToString();
	}

	private static string ExpandGenericParameters(string format, Type type)
	{
		GenericNameProvider genericNameProvider = new GenericNameProvider(type);
		return ExpandGenericParameters(format, genericNameProvider);
	}

	internal static string ExpandGenericParameters(string format, IGenericNameProvider genericNameProvider)
	{
		string text = null;
		StringBuilder stringBuilder = new StringBuilder();
		IList<int> nestedParameterCounts = genericNameProvider.GetNestedParameterCounts();
		for (int i = 0; i < format.Length; i++)
		{
			char c = format[i];
			if (c == '{')
			{
				i++;
				int num = i;
				for (; i < format.Length && format[i] != '}'; i++)
				{
				}
				if (i == format.Length)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("The data contract name '{0}' for type '{1}' has a curly brace '{{' that is not matched with a closing curly brace. Curly braces have special meaning in data contract names - they are used to customize the naming of data contracts for generic types.", format, genericNameProvider.GetGenericTypeName())));
				}
				if (format[num] == '#' && i == num + 1)
				{
					if (nestedParameterCounts.Count <= 1 && genericNameProvider.ParametersFromBuiltInNamespaces)
					{
						continue;
					}
					if (text == null)
					{
						StringBuilder stringBuilder2 = new StringBuilder(genericNameProvider.GetNamespaces());
						foreach (int item in nestedParameterCounts)
						{
							stringBuilder2.Insert(0, item).Insert(0, " ");
						}
						text = GetNamespacesDigest(stringBuilder2.ToString());
					}
					stringBuilder.Append(text);
				}
				else
				{
					if (!int.TryParse(format.Substring(num, i - num), out var result) || result < 0 || result >= genericNameProvider.GetParameterCount())
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("In the data contract name for type '{1}', there are curly braces with '{0}' inside, which is an invalid value. Curly braces have special meaning in data contract names - they are used to customize the naming of data contracts for generic types. Based on the number of generic parameters this type has, the contents of the curly braces must either be a number between 0 and '{2}' to insert the name of the generic parameter at that index or the '#' symbol to insert a digest of the generic parameter namespaces.", format.Substring(num, i - num), genericNameProvider.GetGenericTypeName(), genericNameProvider.GetParameterCount() - 1)));
					}
					stringBuilder.Append(genericNameProvider.GetParameterName(result));
				}
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	internal static bool IsTypeNullable(Type type)
	{
		if (type.IsValueType)
		{
			if (type.IsGenericType)
			{
				return type.GetGenericTypeDefinition() == Globals.TypeOfNullable;
			}
			return false;
		}
		return true;
	}

	public static void ThrowTypeNotSerializable(Type type)
	{
		ThrowInvalidDataContractException(SR.GetString("Type '{0}' cannot be serialized. Consider marking it with the DataContractAttribute attribute, and marking all of its members you want serialized with the DataMemberAttribute attribute. Alternatively, you can ensure that the type is public and has a parameterless constructor - all public members of the type will then be serialized, and no attributes will be required.", type), type);
	}

	internal static Dictionary<XmlQualifiedName, DataContract> ImportKnownTypeAttributes(Type type)
	{
		Dictionary<XmlQualifiedName, DataContract> knownDataContracts = null;
		Dictionary<Type, Type> typesChecked = new Dictionary<Type, Type>();
		ImportKnownTypeAttributes(type, typesChecked, ref knownDataContracts);
		return knownDataContracts;
	}

	private static void ImportKnownTypeAttributes(Type type, Dictionary<Type, Type> typesChecked, ref Dictionary<XmlQualifiedName, DataContract> knownDataContracts)
	{
		if (TD.ImportKnownTypesStartIsEnabled())
		{
			TD.ImportKnownTypesStart();
		}
		while (type != null && IsTypeSerializable(type))
		{
			if (typesChecked.ContainsKey(type))
			{
				return;
			}
			typesChecked.Add(type, type);
			object[] customAttributes = type.GetCustomAttributes(Globals.TypeOfKnownTypeAttribute, inherit: false);
			if (customAttributes != null)
			{
				bool flag = false;
				bool flag2 = false;
				for (int i = 0; i < customAttributes.Length; i++)
				{
					KnownTypeAttribute knownTypeAttribute = (KnownTypeAttribute)customAttributes[i];
					if (knownTypeAttribute.Type != null)
					{
						if (flag)
						{
							ThrowInvalidDataContractException(SR.GetString("Type '{0}': If a KnownTypeAttribute attribute specifies a method it must be the only KnownTypeAttribute attribute on that type.", GetClrTypeFullName(type)), type);
						}
						CheckAndAdd(knownTypeAttribute.Type, typesChecked, ref knownDataContracts);
						flag2 = true;
						continue;
					}
					if (flag || flag2)
					{
						ThrowInvalidDataContractException(SR.GetString("Type '{0}': If a KnownTypeAttribute attribute specifies a method it must be the only KnownTypeAttribute attribute on that type.", GetClrTypeFullName(type)), type);
					}
					string methodName = knownTypeAttribute.MethodName;
					if (methodName == null)
					{
						ThrowInvalidDataContractException(SR.GetString("KnownTypeAttribute attribute on type '{0}' contains no data.", GetClrTypeFullName(type)), type);
					}
					if (methodName.Length == 0)
					{
						ThrowInvalidDataContractException(SR.GetString("Method name specified by KnownTypeAttribute attribute on type '{0}' cannot be the empty string.", GetClrTypeFullName(type)), type);
					}
					MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null);
					if (method == null)
					{
						ThrowInvalidDataContractException(SR.GetString("KnownTypeAttribute attribute on type '{1}' specifies a method named '{0}' to provide known types. Static method '{0}()' was not found on this type. Ensure that the method exists and is marked as static.", methodName, GetClrTypeFullName(type)), type);
					}
					if (!Globals.TypeOfTypeEnumerable.IsAssignableFrom(method.ReturnType))
					{
						ThrowInvalidDataContractException(SR.GetString("KnownTypeAttribute attribute on type '{0}' specifies a method named '{1}' to provide known types. The return type of this method is invalid because it is not assignable to IEnumerable<Type>. Ensure that the method exists and has a valid signature.", GetClrTypeFullName(type), methodName), type);
					}
					object obj = method.Invoke(null, Globals.EmptyObjectArray);
					if (obj == null)
					{
						ThrowInvalidDataContractException(SR.GetString("Method specified by KnownTypeAttribute attribute on type '{0}' returned null.", GetClrTypeFullName(type)), type);
					}
					foreach (Type item in (IEnumerable<Type>)obj)
					{
						if (item == null)
						{
							ThrowInvalidDataContractException(SR.GetString("Method specified by KnownTypeAttribute attribute on type '{0}' does not expose valid types.", GetClrTypeFullName(type)), type);
						}
						CheckAndAdd(item, typesChecked, ref knownDataContracts);
					}
					flag = true;
				}
			}
			LoadKnownTypesFromConfig(type, typesChecked, ref knownDataContracts);
			type = type.BaseType;
		}
		if (TD.ImportKnownTypesStopIsEnabled())
		{
			TD.ImportKnownTypesStop();
		}
	}

	[SecuritySafeCritical]
	private static void LoadKnownTypesFromConfig(Type type, Dictionary<Type, Type> typesChecked, ref Dictionary<XmlQualifiedName, DataContract> knownDataContracts)
	{
	}

	private static void CheckRootTypeInConfigIsGeneric(Type type, ref Type rootType, ref Type[] genArgs)
	{
		if (rootType.IsGenericType)
		{
			if (!rootType.ContainsGenericParameters)
			{
				genArgs = rootType.GetGenericArguments();
				rootType = rootType.GetGenericTypeDefinition();
			}
			else
			{
				ThrowInvalidDataContractException(SR.GetString("Error while getting known types for Type '{0}'. The type must not be an open or partial generic class.", type), type);
			}
		}
	}

	private static bool IsElemTypeNullOrNotEqualToRootType(string elemTypeName, Type rootType)
	{
		Type type = Type.GetType(elemTypeName, throwOnError: false);
		if (type == null || !rootType.Equals(type))
		{
			return true;
		}
		return false;
	}

	private static bool IsCollectionElementTypeEqualToRootType(string collectionElementTypeName, Type rootType)
	{
		if (collectionElementTypeName.StartsWith(GetClrTypeFullName(rootType), StringComparison.Ordinal))
		{
			Type type = Type.GetType(collectionElementTypeName, throwOnError: false);
			if (type != null)
			{
				if (type.IsGenericType && !IsOpenGenericType(type))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Declared type '{0}' in config cannot be a closed or partial generic type.", collectionElementTypeName)));
				}
				if (rootType.Equals(type))
				{
					return true;
				}
			}
		}
		return false;
	}

	[SecurityCritical]
	[SecurityTreatAsSafe]
	internal static void CheckAndAdd(Type type, Dictionary<Type, Type> typesChecked, ref Dictionary<XmlQualifiedName, DataContract> nameToDataContractTable)
	{
		type = UnwrapNullableType(type);
		DataContract dataContract = GetDataContract(type);
		DataContract value;
		if (nameToDataContractTable == null)
		{
			nameToDataContractTable = new Dictionary<XmlQualifiedName, DataContract>();
		}
		else if (nameToDataContractTable.TryGetValue(dataContract.StableName, out value))
		{
			if (value.UnderlyingType != DataContractCriticalHelper.GetDataContractAdapterType(type))
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Type '{0}' cannot be added to list of known types since another type '{1}' with the same data contract name '{2}:{3}' is already present.", type, value.UnderlyingType, dataContract.StableName.Namespace, dataContract.StableName.Name)));
			}
			return;
		}
		nameToDataContractTable.Add(dataContract.StableName, dataContract);
		ImportKnownTypeAttributes(type, typesChecked, ref nameToDataContractTable);
	}

	private static bool IsOpenGenericType(Type t)
	{
		Type[] genericArguments = t.GetGenericArguments();
		for (int i = 0; i < genericArguments.Length; i++)
		{
			if (!genericArguments[i].IsGenericParameter)
			{
				return false;
			}
		}
		return true;
	}

	public sealed override bool Equals(object other)
	{
		if (this == other)
		{
			return true;
		}
		return Equals(other, new Dictionary<DataContractPairKey, object>());
	}

	internal virtual bool Equals(object other, Dictionary<DataContractPairKey, object> checkedContracts)
	{
		if (other is DataContract dataContract)
		{
			if (StableName.Name == dataContract.StableName.Name && StableName.Namespace == dataContract.StableName.Namespace)
			{
				return IsReference == dataContract.IsReference;
			}
			return false;
		}
		return false;
	}

	internal bool IsEqualOrChecked(object other, Dictionary<DataContractPairKey, object> checkedContracts)
	{
		if (this == other)
		{
			return true;
		}
		if (checkedContracts != null)
		{
			DataContractPairKey key = new DataContractPairKey(this, other);
			if (checkedContracts.ContainsKey(key))
			{
				return true;
			}
			checkedContracts.Add(key, null);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	internal void ThrowInvalidDataContractException(string message)
	{
		ThrowInvalidDataContractException(message, UnderlyingType);
	}

	internal static bool IsTypeVisible(Type t)
	{
		return true;
	}
}
