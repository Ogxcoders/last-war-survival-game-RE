using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Xml;

namespace System.Runtime.Serialization;

internal sealed class CollectionDataContract : DataContract
{
	private class CollectionDataContractCriticalHelper : DataContractCriticalHelper
	{
		private static Type[] _knownInterfaces;

		private Type itemType;

		private bool isItemTypeNullable;

		private CollectionKind kind;

		private readonly MethodInfo getEnumeratorMethod;

		private readonly MethodInfo addMethod;

		private readonly ConstructorInfo constructor;

		private readonly string serializationExceptionMessage;

		private readonly string deserializationExceptionMessage;

		private DataContract itemContract;

		private DataContract sharedTypeContract;

		private Dictionary<XmlQualifiedName, DataContract> knownDataContracts;

		private bool isKnownTypeAttributeChecked;

		private string itemName;

		private bool itemNameSetExplicit;

		private XmlDictionaryString collectionItemName;

		private string keyName;

		private string valueName;

		private XmlDictionaryString childElementNamespace;

		private string invalidCollectionInSharedContractMessage;

		private XmlFormatCollectionReaderDelegate xmlFormatReaderDelegate;

		private XmlFormatGetOnlyCollectionReaderDelegate xmlFormatGetOnlyCollectionReaderDelegate;

		private XmlFormatCollectionWriterDelegate xmlFormatWriterDelegate;

		private bool isConstructorCheckRequired;

		internal static Type[] KnownInterfaces
		{
			get
			{
				if (_knownInterfaces == null)
				{
					_knownInterfaces = new Type[8]
					{
						Globals.TypeOfIDictionaryGeneric,
						Globals.TypeOfIDictionary,
						Globals.TypeOfIListGeneric,
						Globals.TypeOfICollectionGeneric,
						Globals.TypeOfIList,
						Globals.TypeOfIEnumerableGeneric,
						Globals.TypeOfICollection,
						Globals.TypeOfIEnumerable
					};
				}
				return _knownInterfaces;
			}
		}

		internal CollectionKind Kind => kind;

		internal Type ItemType => itemType;

		internal DataContract ItemContract
		{
			get
			{
				if (itemContract == null && base.UnderlyingType != null)
				{
					if (IsDictionary)
					{
						if (string.CompareOrdinal(KeyName, ValueName) == 0)
						{
							DataContract.ThrowInvalidDataContractException(SR.GetString("The collection data contract type '{0}' specifies the same value '{1}' for both the KeyName and the ValueName properties. This is not allowed. Consider changing either the KeyName or the ValueName property.", DataContract.GetClrTypeFullName(base.UnderlyingType), KeyName), base.UnderlyingType);
						}
						itemContract = ClassDataContract.CreateClassDataContractForKeyValue(ItemType, base.Namespace, new string[2] { KeyName, ValueName });
						DataContract.GetDataContract(ItemType);
					}
					else
					{
						itemContract = DataContract.GetDataContract(ItemType);
					}
				}
				return itemContract;
			}
			set
			{
				itemContract = value;
			}
		}

		internal DataContract SharedTypeContract
		{
			get
			{
				return sharedTypeContract;
			}
			set
			{
				sharedTypeContract = value;
			}
		}

		internal string ItemName
		{
			get
			{
				return itemName;
			}
			set
			{
				itemName = value;
			}
		}

		internal bool IsConstructorCheckRequired
		{
			get
			{
				return isConstructorCheckRequired;
			}
			set
			{
				isConstructorCheckRequired = value;
			}
		}

		public XmlDictionaryString CollectionItemName => collectionItemName;

		internal string KeyName
		{
			get
			{
				return keyName;
			}
			set
			{
				keyName = value;
			}
		}

		internal string ValueName
		{
			get
			{
				return valueName;
			}
			set
			{
				valueName = value;
			}
		}

		internal bool IsDictionary => KeyName != null;

		public string SerializationExceptionMessage => serializationExceptionMessage;

		public string DeserializationExceptionMessage => deserializationExceptionMessage;

		public XmlDictionaryString ChildElementNamespace
		{
			get
			{
				return childElementNamespace;
			}
			set
			{
				childElementNamespace = value;
			}
		}

		internal bool IsItemTypeNullable
		{
			get
			{
				return isItemTypeNullable;
			}
			set
			{
				isItemTypeNullable = value;
			}
		}

		internal MethodInfo GetEnumeratorMethod => getEnumeratorMethod;

		internal MethodInfo AddMethod => addMethod;

		internal ConstructorInfo Constructor => constructor;

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

		internal string InvalidCollectionInSharedContractMessage => invalidCollectionInSharedContractMessage;

		internal bool ItemNameSetExplicit => itemNameSetExplicit;

		internal XmlFormatCollectionWriterDelegate XmlFormatWriterDelegate
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

		internal XmlFormatCollectionReaderDelegate XmlFormatReaderDelegate
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

		internal XmlFormatGetOnlyCollectionReaderDelegate XmlFormatGetOnlyCollectionReaderDelegate
		{
			get
			{
				return xmlFormatGetOnlyCollectionReaderDelegate;
			}
			set
			{
				xmlFormatGetOnlyCollectionReaderDelegate = value;
			}
		}

		private void Init(CollectionKind kind, Type itemType, CollectionDataContractAttribute collectionContractAttribute)
		{
			this.kind = kind;
			if (itemType != null)
			{
				this.itemType = itemType;
				isItemTypeNullable = DataContract.IsTypeNullable(itemType);
				bool flag = kind == CollectionKind.Dictionary || kind == CollectionKind.GenericDictionary;
				string text = null;
				string text2 = null;
				string text3 = null;
				if (collectionContractAttribute != null)
				{
					if (collectionContractAttribute.IsItemNameSetExplicitly)
					{
						if (collectionContractAttribute.ItemName == null || collectionContractAttribute.ItemName.Length == 0)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have CollectionDataContractAttribute attribute ItemName set to null or empty string.", DataContract.GetClrTypeFullName(base.UnderlyingType))));
						}
						text = DataContract.EncodeLocalName(collectionContractAttribute.ItemName);
						itemNameSetExplicit = true;
					}
					if (collectionContractAttribute.IsKeyNameSetExplicitly)
					{
						if (collectionContractAttribute.KeyName == null || collectionContractAttribute.KeyName.Length == 0)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have CollectionDataContractAttribute attribute KeyName set to null or empty string.", DataContract.GetClrTypeFullName(base.UnderlyingType))));
						}
						if (!flag)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("The collection data contract type '{0}' specifies '{1}' for the KeyName property. This is not allowed since the type is not IDictionary. Remove the setting for the KeyName property.", DataContract.GetClrTypeFullName(base.UnderlyingType), collectionContractAttribute.KeyName)));
						}
						text2 = DataContract.EncodeLocalName(collectionContractAttribute.KeyName);
					}
					if (collectionContractAttribute.IsValueNameSetExplicitly)
					{
						if (collectionContractAttribute.ValueName == null || collectionContractAttribute.ValueName.Length == 0)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have CollectionDataContractAttribute attribute ValueName set to null or empty string.", DataContract.GetClrTypeFullName(base.UnderlyingType))));
						}
						if (!flag)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("The collection data contract type '{0}' specifies '{1}' for the ValueName property. This is not allowed since the type is not IDictionary. Remove the setting for the ValueName property.", DataContract.GetClrTypeFullName(base.UnderlyingType), collectionContractAttribute.ValueName)));
						}
						text3 = DataContract.EncodeLocalName(collectionContractAttribute.ValueName);
					}
				}
				XmlDictionary xmlDictionary = (flag ? new XmlDictionary(5) : new XmlDictionary(3));
				base.Name = xmlDictionary.Add(base.StableName.Name);
				base.Namespace = xmlDictionary.Add(base.StableName.Namespace);
				itemName = text ?? DataContract.GetStableName(DataContract.UnwrapNullableType(itemType)).Name;
				collectionItemName = xmlDictionary.Add(itemName);
				if (flag)
				{
					keyName = text2 ?? "Key";
					valueName = text3 ?? "Value";
				}
			}
			if (collectionContractAttribute != null)
			{
				base.IsReference = collectionContractAttribute.IsReference;
			}
		}

		internal CollectionDataContractCriticalHelper(CollectionKind kind)
		{
			Init(kind, null, null);
		}

		internal CollectionDataContractCriticalHelper(Type type)
			: base(type)
		{
			if (type == Globals.TypeOfArray)
			{
				type = Globals.TypeOfObjectArray;
			}
			if (type.GetArrayRank() > 1)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Multi-dimensional arrays are not supported.")));
			}
			base.StableName = DataContract.GetStableName(type);
			Init(CollectionKind.Array, type.GetElementType(), null);
		}

		internal CollectionDataContractCriticalHelper(Type type, DataContract itemContract)
			: base(type)
		{
			if (type.GetArrayRank() > 1)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Multi-dimensional arrays are not supported.")));
			}
			base.StableName = DataContract.CreateQualifiedName("ArrayOf" + itemContract.StableName.Name, itemContract.StableName.Namespace);
			this.itemContract = itemContract;
			Init(CollectionKind.Array, type.GetElementType(), null);
		}

		internal CollectionDataContractCriticalHelper(Type type, CollectionKind kind, Type itemType, MethodInfo getEnumeratorMethod, string serializationExceptionMessage, string deserializationExceptionMessage)
			: base(type)
		{
			if (getEnumeratorMethod == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Collection type '{0}' does not have a valid GetEnumerator method.", DataContract.GetClrTypeFullName(type))));
			}
			if (itemType == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Collection type '{0}' must have a non-null item type.", DataContract.GetClrTypeFullName(type))));
			}
			base.StableName = DataContract.GetCollectionStableName(type, itemType, out var collectionContractAttribute);
			Init(kind, itemType, collectionContractAttribute);
			this.getEnumeratorMethod = getEnumeratorMethod;
			this.serializationExceptionMessage = serializationExceptionMessage;
			this.deserializationExceptionMessage = deserializationExceptionMessage;
		}

		internal CollectionDataContractCriticalHelper(Type type, CollectionKind kind, Type itemType, MethodInfo getEnumeratorMethod, MethodInfo addMethod, ConstructorInfo constructor)
			: this(type, kind, itemType, getEnumeratorMethod, (string)null, (string)null)
		{
			if (addMethod == null && !type.IsInterface)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Collection type '{0}' does not have a valid Add method.", DataContract.GetClrTypeFullName(type))));
			}
			this.addMethod = addMethod;
			this.constructor = constructor;
		}

		internal CollectionDataContractCriticalHelper(Type type, CollectionKind kind, Type itemType, MethodInfo getEnumeratorMethod, MethodInfo addMethod, ConstructorInfo constructor, bool isConstructorCheckRequired)
			: this(type, kind, itemType, getEnumeratorMethod, addMethod, constructor)
		{
			this.isConstructorCheckRequired = isConstructorCheckRequired;
		}

		internal CollectionDataContractCriticalHelper(Type type, string invalidCollectionInSharedContractMessage)
			: base(type)
		{
			Init(CollectionKind.Collection, null, null);
			this.invalidCollectionInSharedContractMessage = invalidCollectionInSharedContractMessage;
		}
	}

	public class DictionaryEnumerator : IEnumerator<KeyValue<object, object>>, IDisposable, IEnumerator
	{
		private IDictionaryEnumerator enumerator;

		public KeyValue<object, object> Current => new KeyValue<object, object>(enumerator.Key, enumerator.Value);

		object IEnumerator.Current => Current;

		public DictionaryEnumerator(IDictionaryEnumerator enumerator)
		{
			this.enumerator = enumerator;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			return enumerator.MoveNext();
		}

		public void Reset()
		{
			enumerator.Reset();
		}
	}

	public class GenericDictionaryEnumerator<K, V> : IEnumerator<KeyValue<K, V>>, IDisposable, IEnumerator
	{
		private IEnumerator<KeyValuePair<K, V>> enumerator;

		public KeyValue<K, V> Current
		{
			get
			{
				KeyValuePair<K, V> current = enumerator.Current;
				return new KeyValue<K, V>(current.Key, current.Value);
			}
		}

		object IEnumerator.Current => Current;

		public GenericDictionaryEnumerator(IEnumerator<KeyValuePair<K, V>> enumerator)
		{
			this.enumerator = enumerator;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			return enumerator.MoveNext();
		}

		public void Reset()
		{
			enumerator.Reset();
		}
	}

	[SecurityCritical]
	private XmlDictionaryString collectionItemName;

	[SecurityCritical]
	private XmlDictionaryString childElementNamespace;

	[SecurityCritical]
	private DataContract itemContract;

	[SecurityCritical]
	private CollectionDataContractCriticalHelper helper;

	private static Type[] KnownInterfaces
	{
		[SecuritySafeCritical]
		get
		{
			return CollectionDataContractCriticalHelper.KnownInterfaces;
		}
	}

	internal CollectionKind Kind
	{
		[SecuritySafeCritical]
		get
		{
			return helper.Kind;
		}
	}

	internal Type ItemType
	{
		[SecuritySafeCritical]
		get
		{
			return helper.ItemType;
		}
	}

	public DataContract ItemContract
	{
		[SecuritySafeCritical]
		get
		{
			return itemContract ?? helper.ItemContract;
		}
		[SecurityCritical]
		set
		{
			itemContract = value;
			helper.ItemContract = value;
		}
	}

	internal DataContract SharedTypeContract
	{
		[SecuritySafeCritical]
		get
		{
			return helper.SharedTypeContract;
		}
	}

	internal string ItemName
	{
		[SecuritySafeCritical]
		get
		{
			return helper.ItemName;
		}
		[SecurityCritical]
		set
		{
			helper.ItemName = value;
		}
	}

	public XmlDictionaryString CollectionItemName
	{
		[SecuritySafeCritical]
		get
		{
			return collectionItemName;
		}
	}

	internal string KeyName
	{
		[SecuritySafeCritical]
		get
		{
			return helper.KeyName;
		}
		[SecurityCritical]
		set
		{
			helper.KeyName = value;
		}
	}

	internal string ValueName
	{
		[SecuritySafeCritical]
		get
		{
			return helper.ValueName;
		}
		[SecurityCritical]
		set
		{
			helper.ValueName = value;
		}
	}

	internal bool IsDictionary => KeyName != null;

	public XmlDictionaryString ChildElementNamespace
	{
		[SecuritySafeCritical]
		get
		{
			if (childElementNamespace == null)
			{
				lock (this)
				{
					if (childElementNamespace == null)
					{
						if (helper.ChildElementNamespace == null && !IsDictionary)
						{
							XmlDictionaryString childNamespaceToDeclare = ClassDataContract.GetChildNamespaceToDeclare(this, ItemType, new XmlDictionary());
							Thread.MemoryBarrier();
							helper.ChildElementNamespace = childNamespaceToDeclare;
						}
						childElementNamespace = helper.ChildElementNamespace;
					}
				}
			}
			return childElementNamespace;
		}
	}

	internal bool IsItemTypeNullable
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsItemTypeNullable;
		}
		[SecurityCritical]
		set
		{
			helper.IsItemTypeNullable = value;
		}
	}

	internal bool IsConstructorCheckRequired
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsConstructorCheckRequired;
		}
		[SecurityCritical]
		set
		{
			helper.IsConstructorCheckRequired = value;
		}
	}

	internal MethodInfo GetEnumeratorMethod
	{
		[SecuritySafeCritical]
		get
		{
			return helper.GetEnumeratorMethod;
		}
	}

	internal MethodInfo AddMethod
	{
		[SecuritySafeCritical]
		get
		{
			return helper.AddMethod;
		}
	}

	internal ConstructorInfo Constructor
	{
		[SecuritySafeCritical]
		get
		{
			return helper.Constructor;
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

	internal string InvalidCollectionInSharedContractMessage
	{
		[SecuritySafeCritical]
		get
		{
			return helper.InvalidCollectionInSharedContractMessage;
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

	private bool ItemNameSetExplicit
	{
		[SecuritySafeCritical]
		get
		{
			return helper.ItemNameSetExplicit;
		}
	}

	internal XmlFormatCollectionWriterDelegate XmlFormatWriterDelegate
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
						XmlFormatCollectionWriterDelegate xmlFormatWriterDelegate = new XmlFormatWriterGenerator().GenerateCollectionWriter(this);
						Thread.MemoryBarrier();
						helper.XmlFormatWriterDelegate = xmlFormatWriterDelegate;
					}
				}
			}
			return helper.XmlFormatWriterDelegate;
		}
	}

	internal XmlFormatCollectionReaderDelegate XmlFormatReaderDelegate
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
						XmlFormatCollectionReaderDelegate xmlFormatReaderDelegate = new XmlFormatReaderGenerator().GenerateCollectionReader(this);
						Thread.MemoryBarrier();
						helper.XmlFormatReaderDelegate = xmlFormatReaderDelegate;
					}
				}
			}
			return helper.XmlFormatReaderDelegate;
		}
	}

	internal XmlFormatGetOnlyCollectionReaderDelegate XmlFormatGetOnlyCollectionReaderDelegate
	{
		[SecuritySafeCritical]
		get
		{
			if (helper.XmlFormatGetOnlyCollectionReaderDelegate == null)
			{
				lock (this)
				{
					if (helper.XmlFormatGetOnlyCollectionReaderDelegate == null)
					{
						if (base.UnderlyingType.IsInterface && (Kind == CollectionKind.Enumerable || Kind == CollectionKind.Collection || Kind == CollectionKind.GenericEnumerable))
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("On type '{0}', get-only collection must have an Add method.", DataContract.GetClrTypeFullName(base.UnderlyingType))));
						}
						if (IsReadOnlyContract)
						{
							DataContract.ThrowInvalidDataContractException(helper.DeserializationExceptionMessage, null);
						}
						XmlFormatGetOnlyCollectionReaderDelegate xmlFormatGetOnlyCollectionReaderDelegate = new XmlFormatReaderGenerator().GenerateGetOnlyCollectionReader(this);
						Thread.MemoryBarrier();
						helper.XmlFormatGetOnlyCollectionReaderDelegate = xmlFormatGetOnlyCollectionReaderDelegate;
					}
				}
			}
			return helper.XmlFormatGetOnlyCollectionReaderDelegate;
		}
	}

	[SecuritySafeCritical]
	internal CollectionDataContract(CollectionKind kind)
		: base(new CollectionDataContractCriticalHelper(kind))
	{
		InitCollectionDataContract(this);
	}

	[SecuritySafeCritical]
	internal CollectionDataContract(Type type)
		: base(new CollectionDataContractCriticalHelper(type))
	{
		InitCollectionDataContract(this);
	}

	[SecuritySafeCritical]
	internal CollectionDataContract(Type type, DataContract itemContract)
		: base(new CollectionDataContractCriticalHelper(type, itemContract))
	{
		InitCollectionDataContract(this);
	}

	[SecuritySafeCritical]
	private CollectionDataContract(Type type, CollectionKind kind, Type itemType, MethodInfo getEnumeratorMethod, string serializationExceptionMessage, string deserializationExceptionMessage)
		: base(new CollectionDataContractCriticalHelper(type, kind, itemType, getEnumeratorMethod, serializationExceptionMessage, deserializationExceptionMessage))
	{
		InitCollectionDataContract(GetSharedTypeContract(type));
	}

	[SecuritySafeCritical]
	private CollectionDataContract(Type type, CollectionKind kind, Type itemType, MethodInfo getEnumeratorMethod, MethodInfo addMethod, ConstructorInfo constructor)
		: base(new CollectionDataContractCriticalHelper(type, kind, itemType, getEnumeratorMethod, addMethod, constructor))
	{
		InitCollectionDataContract(GetSharedTypeContract(type));
	}

	[SecuritySafeCritical]
	private CollectionDataContract(Type type, CollectionKind kind, Type itemType, MethodInfo getEnumeratorMethod, MethodInfo addMethod, ConstructorInfo constructor, bool isConstructorCheckRequired)
		: base(new CollectionDataContractCriticalHelper(type, kind, itemType, getEnumeratorMethod, addMethod, constructor, isConstructorCheckRequired))
	{
		InitCollectionDataContract(GetSharedTypeContract(type));
	}

	[SecuritySafeCritical]
	private CollectionDataContract(Type type, string invalidCollectionInSharedContractMessage)
		: base(new CollectionDataContractCriticalHelper(type, invalidCollectionInSharedContractMessage))
	{
		InitCollectionDataContract(GetSharedTypeContract(type));
	}

	[SecurityCritical]
	private void InitCollectionDataContract(DataContract sharedTypeContract)
	{
		helper = base.Helper as CollectionDataContractCriticalHelper;
		collectionItemName = helper.CollectionItemName;
		if (helper.Kind == CollectionKind.Dictionary || helper.Kind == CollectionKind.GenericDictionary)
		{
			itemContract = helper.ItemContract;
		}
		helper.SharedTypeContract = sharedTypeContract;
	}

	private void InitSharedTypeContract()
	{
	}

	private DataContract GetSharedTypeContract(Type type)
	{
		if (type.IsDefined(Globals.TypeOfCollectionDataContractAttribute, inherit: false))
		{
			return this;
		}
		if (type.IsSerializable || type.IsDefined(Globals.TypeOfDataContractAttribute, inherit: false))
		{
			return new ClassDataContract(type);
		}
		return null;
	}

	internal static bool IsCollectionInterface(Type type)
	{
		if (type.IsGenericType)
		{
			type = type.GetGenericTypeDefinition();
		}
		return ((ICollection<Type>)KnownInterfaces).Contains(type);
	}

	internal static bool IsCollection(Type type)
	{
		Type itemType;
		return IsCollection(type, out itemType);
	}

	internal static bool IsCollection(Type type, out Type itemType)
	{
		return IsCollectionHelper(type, out itemType, constructorRequired: true);
	}

	internal static bool IsCollection(Type type, bool constructorRequired, bool skipIfReadOnlyContract)
	{
		Type itemType;
		return IsCollectionHelper(type, out itemType, constructorRequired, skipIfReadOnlyContract);
	}

	private static bool IsCollectionHelper(Type type, out Type itemType, bool constructorRequired, bool skipIfReadOnlyContract = false)
	{
		if (type.IsArray && DataContract.GetBuiltInDataContract(type) == null)
		{
			itemType = type.GetElementType();
			return true;
		}
		DataContract dataContract;
		return IsCollectionOrTryCreate(type, tryCreate: false, out dataContract, out itemType, constructorRequired, skipIfReadOnlyContract);
	}

	internal static bool TryCreate(Type type, out DataContract dataContract)
	{
		Type itemType;
		return IsCollectionOrTryCreate(type, tryCreate: true, out dataContract, out itemType, constructorRequired: true);
	}

	internal static bool TryCreateGetOnlyCollectionDataContract(Type type, out DataContract dataContract)
	{
		if (type.IsArray)
		{
			dataContract = new CollectionDataContract(type);
			return true;
		}
		Type itemType;
		return IsCollectionOrTryCreate(type, tryCreate: true, out dataContract, out itemType, constructorRequired: false);
	}

	internal static MethodInfo GetTargetMethodWithName(string name, Type type, Type interfaceType)
	{
		InterfaceMapping interfaceMap = type.GetInterfaceMap(interfaceType);
		for (int i = 0; i < interfaceMap.TargetMethods.Length; i++)
		{
			if (interfaceMap.InterfaceMethods[i].Name == name)
			{
				return interfaceMap.InterfaceMethods[i];
			}
		}
		return null;
	}

	private static bool IsArraySegment(Type t)
	{
		if (t.IsGenericType)
		{
			return t.GetGenericTypeDefinition() == typeof(ArraySegment<>);
		}
		return false;
	}

	private static bool IsCollectionOrTryCreate(Type type, bool tryCreate, out DataContract dataContract, out Type itemType, bool constructorRequired, bool skipIfReadOnlyContract = false)
	{
		dataContract = null;
		itemType = Globals.TypeOfObject;
		if (DataContract.GetBuiltInDataContract(type) != null)
		{
			return HandleIfInvalidCollection(type, tryCreate, hasCollectionDataContract: false, createContractWithException: false, "{0} is a built-in type and cannot be a collection.", null, ref dataContract);
		}
		bool hasCollectionDataContract = IsCollectionDataContract(type);
		bool flag = false;
		string serializationExceptionMessage = null;
		string deserializationExceptionMessage = null;
		Type baseType = type.BaseType;
		bool flag2 = baseType != null && baseType != Globals.TypeOfObject && baseType != Globals.TypeOfValueType && baseType != Globals.TypeOfUri && IsCollection(baseType) && !type.IsSerializable;
		if (type.IsDefined(Globals.TypeOfDataContractAttribute, inherit: false))
		{
			return HandleIfInvalidCollection(type, tryCreate, hasCollectionDataContract, flag2, "{0} has DataContractAttribute attribute.", null, ref dataContract);
		}
		if (Globals.TypeOfIXmlSerializable.IsAssignableFrom(type) || IsArraySegment(type))
		{
			return false;
		}
		if (!Globals.TypeOfIEnumerable.IsAssignableFrom(type))
		{
			return HandleIfInvalidCollection(type, tryCreate, hasCollectionDataContract, flag2, "{0} does not implement IEnumerable interface.", null, ref dataContract);
		}
		MethodInfo method;
		MethodInfo addMethod;
		if (type.IsInterface)
		{
			Type type2 = (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
			Type[] knownInterfaces = KnownInterfaces;
			for (int i = 0; i < knownInterfaces.Length; i++)
			{
				if (!(knownInterfaces[i] == type2))
				{
					continue;
				}
				addMethod = null;
				if (type.IsGenericType)
				{
					Type[] genericArguments = type.GetGenericArguments();
					if (type2 == Globals.TypeOfIDictionaryGeneric)
					{
						itemType = Globals.TypeOfKeyValue.MakeGenericType(genericArguments);
						addMethod = type.GetMethod("Add");
						method = Globals.TypeOfIEnumerableGeneric.MakeGenericType(Globals.TypeOfKeyValuePair.MakeGenericType(genericArguments)).GetMethod("GetEnumerator");
					}
					else
					{
						itemType = genericArguments[0];
						if (type2 == Globals.TypeOfICollectionGeneric || type2 == Globals.TypeOfIListGeneric)
						{
							addMethod = Globals.TypeOfICollectionGeneric.MakeGenericType(itemType).GetMethod("Add");
						}
						method = Globals.TypeOfIEnumerableGeneric.MakeGenericType(itemType).GetMethod("GetEnumerator");
					}
				}
				else
				{
					if (type2 == Globals.TypeOfIDictionary)
					{
						itemType = typeof(KeyValue<object, object>);
						addMethod = type.GetMethod("Add");
					}
					else
					{
						itemType = Globals.TypeOfObject;
						if (type2 == Globals.TypeOfIList)
						{
							addMethod = Globals.TypeOfIList.GetMethod("Add");
						}
					}
					method = Globals.TypeOfIEnumerable.GetMethod("GetEnumerator");
				}
				if (tryCreate)
				{
					dataContract = new CollectionDataContract(type, (CollectionKind)(i + 1), itemType, method, addMethod, null);
				}
				return true;
			}
		}
		ConstructorInfo constructorInfo = null;
		if (!type.IsValueType)
		{
			constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null);
			if (constructorInfo == null && constructorRequired)
			{
				if (type.IsSerializable)
				{
					return HandleIfInvalidCollection(type, tryCreate, hasCollectionDataContract, flag2, "{0} does not have a default constructor.", null, ref dataContract);
				}
				flag = true;
				GetReadOnlyCollectionExceptionMessages(type, hasCollectionDataContract, "{0} does not have a default constructor.", null, out serializationExceptionMessage, out deserializationExceptionMessage);
			}
		}
		Type type3 = null;
		CollectionKind collectionKind = CollectionKind.None;
		bool flag3 = false;
		Type[] interfaces = type.GetInterfaces();
		foreach (Type type4 in interfaces)
		{
			Type type5 = (type4.IsGenericType ? type4.GetGenericTypeDefinition() : type4);
			Type[] knownInterfaces2 = KnownInterfaces;
			for (int k = 0; k < knownInterfaces2.Length; k++)
			{
				if (knownInterfaces2[k] == type5)
				{
					CollectionKind collectionKind2 = (CollectionKind)(k + 1);
					if (collectionKind == CollectionKind.None || (int)collectionKind2 < (int)collectionKind)
					{
						collectionKind = collectionKind2;
						type3 = type4;
						flag3 = false;
					}
					else if ((collectionKind & collectionKind2) == collectionKind2)
					{
						flag3 = true;
					}
					break;
				}
			}
		}
		switch (collectionKind)
		{
		case CollectionKind.None:
			return HandleIfInvalidCollection(type, tryCreate, hasCollectionDataContract, flag2, "{0} does not implement IEnumerable interface.", null, ref dataContract);
		case CollectionKind.GenericEnumerable:
		case CollectionKind.Collection:
		case CollectionKind.Enumerable:
			if (flag3)
			{
				type3 = Globals.TypeOfIEnumerable;
			}
			itemType = (type3.IsGenericType ? type3.GetGenericArguments()[0] : Globals.TypeOfObject);
			GetCollectionMethods(type, type3, new Type[1] { itemType }, addMethodOnInterface: false, out method, out addMethod);
			if (addMethod == null)
			{
				if (type.IsSerializable || skipIfReadOnlyContract)
				{
					return HandleIfInvalidCollection(type, tryCreate, hasCollectionDataContract, flag2 && !skipIfReadOnlyContract, "{0} does not have a valid Add method with parameter of type '{1}'.", DataContract.GetClrTypeFullName(itemType), ref dataContract);
				}
				flag = true;
				GetReadOnlyCollectionExceptionMessages(type, hasCollectionDataContract, "{0} does not have a valid Add method with parameter of type '{1}'.", DataContract.GetClrTypeFullName(itemType), out serializationExceptionMessage, out deserializationExceptionMessage);
			}
			if (tryCreate)
			{
				dataContract = (flag ? new CollectionDataContract(type, collectionKind, itemType, method, serializationExceptionMessage, deserializationExceptionMessage) : new CollectionDataContract(type, collectionKind, itemType, method, addMethod, constructorInfo, !constructorRequired));
			}
			break;
		default:
		{
			if (flag3)
			{
				return HandleIfInvalidCollection(type, tryCreate, hasCollectionDataContract, flag2, "{0} has multiple definitions of interface '{1}'.", KnownInterfaces[(uint)(collectionKind - 1)].Name, ref dataContract);
			}
			Type[] array = null;
			switch (collectionKind)
			{
			case CollectionKind.GenericDictionary:
			{
				array = type3.GetGenericArguments();
				bool flag4 = type3.IsGenericTypeDefinition || (array[0].IsGenericParameter && array[1].IsGenericParameter);
				itemType = (flag4 ? Globals.TypeOfKeyValue : Globals.TypeOfKeyValue.MakeGenericType(array));
				break;
			}
			case CollectionKind.Dictionary:
				array = new Type[2]
				{
					Globals.TypeOfObject,
					Globals.TypeOfObject
				};
				itemType = Globals.TypeOfKeyValue.MakeGenericType(array);
				break;
			case CollectionKind.GenericList:
			case CollectionKind.GenericCollection:
				array = type3.GetGenericArguments();
				itemType = array[0];
				break;
			case CollectionKind.List:
				itemType = Globals.TypeOfObject;
				array = new Type[1] { itemType };
				break;
			}
			if (tryCreate)
			{
				GetCollectionMethods(type, type3, array, addMethodOnInterface: true, out method, out addMethod);
				dataContract = (flag ? new CollectionDataContract(type, collectionKind, itemType, method, serializationExceptionMessage, deserializationExceptionMessage) : new CollectionDataContract(type, collectionKind, itemType, method, addMethod, constructorInfo, !constructorRequired));
			}
			break;
		}
		}
		return !(flag && skipIfReadOnlyContract);
	}

	internal static bool IsCollectionDataContract(Type type)
	{
		return type.IsDefined(Globals.TypeOfCollectionDataContractAttribute, inherit: false);
	}

	private static bool HandleIfInvalidCollection(Type type, bool tryCreate, bool hasCollectionDataContract, bool createContractWithException, string message, string param, ref DataContract dataContract)
	{
		if (hasCollectionDataContract)
		{
			if (tryCreate)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(GetInvalidCollectionMessage(message, SR.GetString("Type '{0}' with CollectionDataContractAttribute attribute is an invalid collection type since it", DataContract.GetClrTypeFullName(type)), param)));
			}
			return true;
		}
		if (createContractWithException)
		{
			if (tryCreate)
			{
				dataContract = new CollectionDataContract(type, GetInvalidCollectionMessage(message, SR.GetString("Type '{0}' is an invalid collection type since it", DataContract.GetClrTypeFullName(type)), param));
			}
			return true;
		}
		return false;
	}

	private static void GetReadOnlyCollectionExceptionMessages(Type type, bool hasCollectionDataContract, string message, string param, out string serializationExceptionMessage, out string deserializationExceptionMessage)
	{
		serializationExceptionMessage = GetInvalidCollectionMessage(message, SR.GetString(hasCollectionDataContract ? "Type '{0}' with CollectionDataContractAttribute attribute is an invalid collection type since it" : "Type '{0}' is an invalid collection type since it", DataContract.GetClrTypeFullName(type)), param);
		deserializationExceptionMessage = GetInvalidCollectionMessage(message, SR.GetString("Error on deserializing read-only collection: {0}", DataContract.GetClrTypeFullName(type)), param);
	}

	private static string GetInvalidCollectionMessage(string message, string nestedMessage, string param)
	{
		if (param != null)
		{
			return SR.GetString(message, nestedMessage, param);
		}
		return SR.GetString(message, nestedMessage);
	}

	private static void FindCollectionMethodsOnInterface(Type type, Type interfaceType, ref MethodInfo addMethod, ref MethodInfo getEnumeratorMethod)
	{
		InterfaceMapping interfaceMap = type.GetInterfaceMap(interfaceType);
		for (int i = 0; i < interfaceMap.TargetMethods.Length; i++)
		{
			if (interfaceMap.InterfaceMethods[i].Name == "Add")
			{
				addMethod = interfaceMap.InterfaceMethods[i];
			}
			else if (interfaceMap.InterfaceMethods[i].Name == "GetEnumerator")
			{
				getEnumeratorMethod = interfaceMap.InterfaceMethods[i];
			}
		}
	}

	private static void GetCollectionMethods(Type type, Type interfaceType, Type[] addMethodTypeArray, bool addMethodOnInterface, out MethodInfo getEnumeratorMethod, out MethodInfo addMethod)
	{
		addMethod = (getEnumeratorMethod = null);
		if (addMethodOnInterface)
		{
			addMethod = type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public, null, addMethodTypeArray, null);
			if (addMethod == null || addMethod.GetParameters()[0].ParameterType != addMethodTypeArray[0])
			{
				FindCollectionMethodsOnInterface(type, interfaceType, ref addMethod, ref getEnumeratorMethod);
				if (addMethod == null)
				{
					Type[] interfaces = interfaceType.GetInterfaces();
					foreach (Type type2 in interfaces)
					{
						if (IsKnownInterface(type2))
						{
							FindCollectionMethodsOnInterface(type, type2, ref addMethod, ref getEnumeratorMethod);
							if (addMethod == null)
							{
								break;
							}
						}
					}
				}
			}
		}
		else
		{
			addMethod = type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, addMethodTypeArray, null);
		}
		if (!(getEnumeratorMethod == null))
		{
			return;
		}
		getEnumeratorMethod = type.GetMethod("GetEnumerator", BindingFlags.Instance | BindingFlags.Public, null, Globals.EmptyTypeArray, null);
		if (getEnumeratorMethod == null || !Globals.TypeOfIEnumerator.IsAssignableFrom(getEnumeratorMethod.ReturnType))
		{
			Type type3 = interfaceType.GetInterface("System.Collections.Generic.IEnumerable*");
			if (type3 == null)
			{
				type3 = Globals.TypeOfIEnumerable;
			}
			getEnumeratorMethod = GetTargetMethodWithName("GetEnumerator", type, type3);
		}
	}

	private static bool IsKnownInterface(Type type)
	{
		Type type2 = (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
		Type[] knownInterfaces = KnownInterfaces;
		foreach (Type type3 in knownInterfaces)
		{
			if (type2 == type3)
			{
				return true;
			}
		}
		return false;
	}

	[SecuritySafeCritical]
	internal override DataContract BindGenericParameters(DataContract[] paramContracts, Dictionary<DataContract, DataContract> boundContracts)
	{
		if (boundContracts.TryGetValue(this, out var value))
		{
			return value;
		}
		CollectionDataContract collectionDataContract = new CollectionDataContract(Kind);
		boundContracts.Add(this, collectionDataContract);
		collectionDataContract.ItemContract = ItemContract.BindGenericParameters(paramContracts, boundContracts);
		collectionDataContract.IsItemTypeNullable = !collectionDataContract.ItemContract.IsValueType;
		collectionDataContract.ItemName = (ItemNameSetExplicit ? ItemName : collectionDataContract.ItemContract.StableName.Name);
		collectionDataContract.KeyName = KeyName;
		collectionDataContract.ValueName = ValueName;
		collectionDataContract.StableName = DataContract.CreateQualifiedName(DataContract.ExpandGenericParameters(XmlConvert.DecodeName(base.StableName.Name), new GenericNameProvider(DataContract.GetClrTypeFullName(base.UnderlyingType), paramContracts)), IsCollectionDataContract(base.UnderlyingType) ? base.StableName.Namespace : DataContract.GetCollectionNamespace(collectionDataContract.ItemContract.StableName.Namespace));
		return collectionDataContract;
	}

	internal override DataContract GetValidContract(SerializationMode mode)
	{
		if (mode == SerializationMode.SharedType)
		{
			if (SharedTypeContract == null)
			{
				DataContract.ThrowTypeNotSerializable(base.UnderlyingType);
			}
			return SharedTypeContract;
		}
		ThrowIfInvalid();
		return this;
	}

	private void ThrowIfInvalid()
	{
		if (InvalidCollectionInSharedContractMessage != null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(InvalidCollectionInSharedContractMessage));
		}
	}

	internal override DataContract GetValidContract()
	{
		if (IsConstructorCheckRequired)
		{
			CheckConstructor();
		}
		return this;
	}

	[SecuritySafeCritical]
	private void CheckConstructor()
	{
		if (Constructor == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("{0} does not have a default constructor.", DataContract.GetClrTypeFullName(base.UnderlyingType))));
		}
		IsConstructorCheckRequired = false;
	}

	internal override bool IsValidContract(SerializationMode mode)
	{
		if (mode == SerializationMode.SharedType)
		{
			return SharedTypeContract != null;
		}
		return InvalidCollectionInSharedContractMessage == null;
	}

	internal override bool Equals(object other, Dictionary<DataContractPairKey, object> checkedContracts)
	{
		if (IsEqualOrChecked(other, checkedContracts))
		{
			return true;
		}
		if (base.Equals(other, checkedContracts) && other is CollectionDataContract collectionDataContract)
		{
			bool flag = ItemContract != null && !ItemContract.IsValueType;
			bool flag2 = collectionDataContract.ItemContract != null && !collectionDataContract.ItemContract.IsValueType;
			if (ItemName == collectionDataContract.ItemName && (IsItemTypeNullable || flag) == (collectionDataContract.IsItemTypeNullable || flag2))
			{
				return ItemContract.Equals(collectionDataContract.ItemContract, checkedContracts);
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override void WriteXmlValue(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context)
	{
		context.IsGetOnlyCollection = false;
		XmlFormatWriterDelegate(xmlWriter, obj, context, this);
	}

	public override object ReadXmlValue(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context)
	{
		xmlReader.Read();
		object result = null;
		if (context.IsGetOnlyCollection)
		{
			context.IsGetOnlyCollection = false;
			XmlFormatGetOnlyCollectionReaderDelegate(xmlReader, context, CollectionItemName, Namespace, this);
		}
		else
		{
			result = XmlFormatReaderDelegate(xmlReader, context, CollectionItemName, Namespace, this);
		}
		xmlReader.ReadEndElement();
		return result;
	}
}
