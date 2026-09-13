using System.Collections.Generic;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization.Json;

internal class JsonDataContract
{
	internal class JsonDataContractCriticalHelper
	{
		private static object cacheLock = new object();

		private static object createDataContractLock = new object();

		private static JsonDataContract[] dataContractCache = new JsonDataContract[32];

		private static int dataContractID = 0;

		private static TypeHandleRef typeHandleRef = new TypeHandleRef();

		private static Dictionary<TypeHandleRef, IntRef> typeToIDCache = new Dictionary<TypeHandleRef, IntRef>(new TypeHandleRefEqualityComparer());

		private Dictionary<XmlQualifiedName, DataContract> knownDataContracts;

		private DataContract traditionalDataContract;

		private string typeName;

		internal Dictionary<XmlQualifiedName, DataContract> KnownDataContracts => knownDataContracts;

		internal DataContract TraditionalDataContract => traditionalDataContract;

		internal virtual string TypeName => typeName;

		internal JsonDataContractCriticalHelper(DataContract traditionalDataContract)
		{
			this.traditionalDataContract = traditionalDataContract;
			AddCollectionItemContractsToKnownDataContracts();
			typeName = (string.IsNullOrEmpty(traditionalDataContract.Namespace.Value) ? traditionalDataContract.Name.Value : (traditionalDataContract.Name.Value + ":" + XmlObjectSerializerWriteContextComplexJson.TruncateDefaultDataContractNamespace(traditionalDataContract.Namespace.Value)));
		}

		public static JsonDataContract GetJsonDataContract(DataContract traditionalDataContract)
		{
			int id = GetId(traditionalDataContract.UnderlyingType.TypeHandle);
			JsonDataContract jsonDataContract = dataContractCache[id];
			if (jsonDataContract == null)
			{
				jsonDataContract = CreateJsonDataContract(id, traditionalDataContract);
				dataContractCache[id] = jsonDataContract;
			}
			return jsonDataContract;
		}

		internal static int GetId(RuntimeTypeHandle typeHandle)
		{
			lock (cacheLock)
			{
				typeHandleRef.Value = typeHandle;
				if (!typeToIDCache.TryGetValue(typeHandleRef, out var value))
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
					value = new IntRef(num);
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

		private static JsonDataContract CreateJsonDataContract(int id, DataContract traditionalDataContract)
		{
			lock (createDataContractLock)
			{
				JsonDataContract jsonDataContract = dataContractCache[id];
				if (jsonDataContract == null)
				{
					Type type = traditionalDataContract.GetType();
					if (type == typeof(ObjectDataContract))
					{
						jsonDataContract = new JsonObjectDataContract(traditionalDataContract);
					}
					else if (type == typeof(StringDataContract))
					{
						jsonDataContract = new JsonStringDataContract((StringDataContract)traditionalDataContract);
					}
					else if (type == typeof(UriDataContract))
					{
						jsonDataContract = new JsonUriDataContract((UriDataContract)traditionalDataContract);
					}
					else if (type == typeof(QNameDataContract))
					{
						jsonDataContract = new JsonQNameDataContract((QNameDataContract)traditionalDataContract);
					}
					else if (type == typeof(ByteArrayDataContract))
					{
						jsonDataContract = new JsonByteArrayDataContract((ByteArrayDataContract)traditionalDataContract);
					}
					else if (traditionalDataContract.IsPrimitive || traditionalDataContract.UnderlyingType == Globals.TypeOfXmlQualifiedName)
					{
						jsonDataContract = new JsonDataContract(traditionalDataContract);
					}
					else if (type == typeof(ClassDataContract))
					{
						jsonDataContract = new JsonClassDataContract((ClassDataContract)traditionalDataContract);
					}
					else if (type == typeof(EnumDataContract))
					{
						jsonDataContract = new JsonEnumDataContract((EnumDataContract)traditionalDataContract);
					}
					else if (type == typeof(GenericParameterDataContract) || type == typeof(SpecialTypeDataContract))
					{
						jsonDataContract = new JsonDataContract(traditionalDataContract);
					}
					else if (type == typeof(CollectionDataContract))
					{
						jsonDataContract = new JsonCollectionDataContract((CollectionDataContract)traditionalDataContract);
					}
					else
					{
						if (!(type == typeof(XmlDataContract)))
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("traditionalDataContract", SR.GetString("Type '{0}' is not suppotred by DataContractJsonSerializer.", traditionalDataContract.UnderlyingType));
						}
						jsonDataContract = new JsonXmlDataContract((XmlDataContract)traditionalDataContract);
					}
				}
				return jsonDataContract;
			}
		}

		private void AddCollectionItemContractsToKnownDataContracts()
		{
			if (traditionalDataContract.KnownDataContracts == null)
			{
				return;
			}
			foreach (KeyValuePair<XmlQualifiedName, DataContract> knownDataContract in traditionalDataContract.KnownDataContracts)
			{
				if ((object)knownDataContract == null)
				{
					continue;
				}
				CollectionDataContract collectionDataContract = knownDataContract.Value as CollectionDataContract;
				while (collectionDataContract != null)
				{
					DataContract itemContract = collectionDataContract.ItemContract;
					if (knownDataContracts == null)
					{
						knownDataContracts = new Dictionary<XmlQualifiedName, DataContract>();
					}
					if (!knownDataContracts.ContainsKey(itemContract.StableName))
					{
						knownDataContracts.Add(itemContract.StableName, itemContract);
					}
					if (collectionDataContract.ItemType.IsGenericType && collectionDataContract.ItemType.GetGenericTypeDefinition() == typeof(KeyValue<, >))
					{
						DataContract dataContract = DataContract.GetDataContract(Globals.TypeOfKeyValuePair.MakeGenericType(collectionDataContract.ItemType.GetGenericArguments()));
						if (!knownDataContracts.ContainsKey(dataContract.StableName))
						{
							knownDataContracts.Add(dataContract.StableName, dataContract);
						}
					}
					if (!(itemContract is CollectionDataContract))
					{
						break;
					}
					collectionDataContract = itemContract as CollectionDataContract;
				}
			}
		}
	}

	[SecurityCritical]
	private JsonDataContractCriticalHelper helper;

	internal virtual string TypeName => null;

	protected JsonDataContractCriticalHelper Helper
	{
		[SecurityCritical]
		get
		{
			return helper;
		}
	}

	protected DataContract TraditionalDataContract
	{
		[SecuritySafeCritical]
		get
		{
			return helper.TraditionalDataContract;
		}
	}

	private Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
	{
		[SecuritySafeCritical]
		get
		{
			return helper.KnownDataContracts;
		}
	}

	[SecuritySafeCritical]
	protected JsonDataContract(DataContract traditionalDataContract)
	{
		helper = new JsonDataContractCriticalHelper(traditionalDataContract);
	}

	[SecuritySafeCritical]
	protected JsonDataContract(JsonDataContractCriticalHelper helper)
	{
		this.helper = helper;
	}

	[SecuritySafeCritical]
	public static JsonDataContract GetJsonDataContract(DataContract traditionalDataContract)
	{
		return JsonDataContractCriticalHelper.GetJsonDataContract(traditionalDataContract);
	}

	public object ReadJsonValue(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
	{
		PushKnownDataContracts(context);
		object result = ReadJsonValueCore(jsonReader, context);
		PopKnownDataContracts(context);
		return result;
	}

	public virtual object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
	{
		return TraditionalDataContract.ReadXmlValue(jsonReader, context);
	}

	public void WriteJsonValue(XmlWriterDelegator jsonWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, RuntimeTypeHandle declaredTypeHandle)
	{
		PushKnownDataContracts(context);
		WriteJsonValueCore(jsonWriter, obj, context, declaredTypeHandle);
		PopKnownDataContracts(context);
	}

	public virtual void WriteJsonValueCore(XmlWriterDelegator jsonWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, RuntimeTypeHandle declaredTypeHandle)
	{
		TraditionalDataContract.WriteXmlValue(jsonWriter, obj, context);
	}

	protected static object HandleReadValue(object obj, XmlObjectSerializerReadContext context)
	{
		context.AddNewObject(obj);
		return obj;
	}

	protected static bool TryReadNullAtTopLevel(XmlReaderDelegator reader)
	{
		if (reader.MoveToAttribute("type") && reader.Value == "null")
		{
			reader.Skip();
			reader.MoveToElement();
			return true;
		}
		reader.MoveToElement();
		return false;
	}

	protected void PopKnownDataContracts(XmlObjectSerializerContext context)
	{
		if (KnownDataContracts != null)
		{
			context.scopedKnownTypes.Pop();
		}
	}

	protected void PushKnownDataContracts(XmlObjectSerializerContext context)
	{
		if (KnownDataContracts != null)
		{
			context.scopedKnownTypes.Push(KnownDataContracts);
		}
	}
}
