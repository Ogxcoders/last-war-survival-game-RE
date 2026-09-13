using System.Globalization;

namespace System.Runtime.Serialization;

internal static class SR
{
	public const string ArrayExceededSize = "Array length '{0}' provided by the get-only collection of type '{1}' is less than the number of array elements found in the input stream.  Consider increasing the length of the array.";

	public const string ArrayExceededSizeAttribute = "Array length '{0}' provided by Size attribute is not equal to the number of array elements '{1}' from namespace '{2}' found.";

	public const string ArrayTypeIsNotSupported = "An internal error has occurred. '{0}[]' is not supported when generating code for serialization.";

	public const string CannotDeserializeRefAtTopLevel = "Cannot deserialize since root element references unrecognized object with id '{0}'.";

	public const string CannotLoadMemberType = "Cannot load member type '{0}'.";

	public const string CannotSerializeObjectWithCycles = "Object graph for type '{0}' contains cycles and cannot be serialized if references are not tracked. Consider using the DataContractAttribute with the IsReference property set to true.";

	public const string CanOnlyStoreIntoArgOrLocGot0 = "An internal error has occurred. Data can only be stored into ArgBuilder or LocalBuilder. Got: {0}.";

	public const string CharIsInvalidPrimitive = "An internal error has occurred. Char is not a valid schema primitive and should be treated as int in DataContract.";

	public const string CallbackMustReturnVoid = "Serialization Callback '{1}' in type '{0}' must return void.";

	public const string CallbackParameterInvalid = "Serialization Callback '{1}' in type '{0}' must have a single parameter of type '{2}'.";

	public const string CallbacksCannotBeVirtualMethods = "Virtual Method '{0}' of type '{1}' cannot be marked with '{2}' attribute.";

	public const string CollectionMustHaveAddMethod = "Collection type '{0}' does not have a valid Add method.";

	public const string CollectionMustHaveGetEnumeratorMethod = "Collection type '{0}' does not have a valid GetEnumerator method.";

	public const string CollectionMustHaveItemType = "Collection type '{0}' must have a non-null item type.";

	public const string CollectionTypeCannotBeBuiltIn = "{0} is a built-in type and cannot be a collection.";

	public const string CollectionTypeCannotHaveDataContract = "{0} has DataContractAttribute attribute.";

	public const string CollectionTypeDoesNotHaveAddMethod = "{0} does not have a valid Add method with parameter of type '{1}'.";

	public const string CollectionTypeDoesNotHaveDefaultCtor = "{0} does not have a default constructor.";

	public const string CollectionTypeHasMultipleDefinitionsOfInterface = "{0} has multiple definitions of interface '{1}'.";

	public const string CollectionTypeIsNotIEnumerable = "{0} does not implement IEnumerable interface.";

	public const string DataContractCacheOverflow = "An internal error has occurred. DataContract cache overflow.";

	public const string DataContractNamespaceAlreadySet = "ContractNamespaceAttribute attribute maps CLR namespace '{2}' to multiple data contract namespaces '{0}' and '{1}'. You can map a CLR namespace to only one data contract namespace.";

	public const string DataContractNamespaceIsNotValid = "DataContract namespace '{0}' is not a valid URI.";

	public const string DataContractNamespaceReserved = "DataContract namespace '{0}' cannot be specified since it is reserved.";

	public const string DataMemberOnEnumField = "Member '{0}.{1}' has DataMemberAttribute attribute. Use EnumMemberAttribute attribute instead.";

	public const string DcTypeNotFoundOnDeserialize = "Element '{2}:{3}' contains data of the '{0}:{1}' data contract. The deserializer has no knowledge of any type that maps to this contract. Add the type corresponding to '{1}' to the list of known types - for example, by using the KnownTypeAttribute attribute or by adding it to the list of known types passed to DataContractSerializer.";

	public const string DcTypeNotFoundOnSerialize = "Type '{0}' with data contract name '{1}:{2}' is not expected. Add any types not known statically to the list of known types - for example, by using the KnownTypeAttribute attribute or by adding them to the list of known types passed to DataContractSerializer.";

	public const string DcTypeNotResolvedOnDeserialize = "Element '{2}:{3}' contains data from a type that maps to the name '{0}:{1}'. The deserializer has no knowledge of any type that maps to this name. Consider changing the implementation of the ResolveName method on your DataContractResolver to return a non-null value for name '{1}' and namespace '{0}'.";

	public const string DeserializedObjectWithIdNotFound = "Deserialized object with reference id '{0}' not found in stream.";

	public const string DupContractInKnownTypes = "Type '{0}' cannot be added to list of known types since another type '{1}' with the same data contract name '{2}:{3}' is already present.";

	public const string DupKeyValueName = "The collection data contract type '{0}' specifies the same value '{1}' for both the KeyName and the ValueName properties. This is not allowed. Consider changing either the KeyName or the ValueName property.";

	public const string DupEnumMemberValue = "Type '{2}' contains two members '{0}' 'and '{1}' with the same name '{3}'. Multiple members with the same name in one type are not supported. Consider changing one of the member names using EnumMemberAttribute attribute.";

	public const string DupMemberName = "Type '{2}' contains two members '{0}' 'and '{1}' with the same data member name '{3}'. Multiple members with the same name in one type are not supported. Consider changing one of the member names using DataMemberAttribute attribute.";

	public const string DuplicateAttribute = "Invalid Callback. Method '{3}' in type '{2}' has both '{0}' and '{1}'.";

	public const string DuplicateCallback = "Invalid attribute. Both '{0}' and '{1}' in type '{2}' have '{3}'.";

	public const string EncounteredWithNameNamespace = "{0}. Encountered '{1}'  with name '{2}', namespace '{3}'.";

	public const string EnumTypeCannotHaveIsReference = "Enum type '{0}' cannot have the IsReference setting of '{1}'. Either change the setting to '{2}', or remove it completely.";

	public const string ErrorDeserializing = "There was an error deserializing the object {0}. {1}";

	public const string ErrorInLine = "Error in line {0} position {1}.";

	public const string ErrorIsStartObject = "There was an error checking start element of object {0}. {1}";

	public const string ErrorSerializing = "There was an error serializing the object {0}. {1}";

	public const string ErrorTypeInfo = "of type {0}";

	public const string ErrorWriteEndObject = "There was an error writing end element of object {0}. {1}";

	public const string ErrorWriteStartObject = "There was an error writing start element of object {0}. {1}";

	public const string ExceededMaxItemsQuota = "Maximum number of items that can be serialized or deserialized in an object graph is '{0}'.";

	public const string ExpectingElement = "Expecting element '{1}' from namespace '{0}'.";

	public const string ExpectingElementAtDeserialize = "Expecting state '{0}' when ReadObject is called.";

	public const string ExpectingEnd = "Expecting End'{0}'.";

	public const string ExpectingState = "Expecting state '{0}'.";

	public const string GenericNameBraceMismatch = "The data contract name '{0}' for type '{1}' has a curly brace '{{' that is not matched with a closing curly brace. Curly braces have special meaning in data contract names - they are used to customize the naming of data contracts for generic types.";

	public const string GenericParameterNotValid = "In the data contract name for type '{1}', there are curly braces with '{0}' inside, which is an invalid value. Curly braces have special meaning in data contract names - they are used to customize the naming of data contracts for generic types. Based on the number of generic parameters this type has, the contents of the curly braces must either be a number between 0 and '{2}' to insert the name of the generic parameter at that index or the '#' symbol to insert a digest of the generic parameter namespaces.";

	public const string InconsistentIsReference = "The IsReference setting for type '{0}' is '{1}', but the same setting for its parent class '{2}' is '{3}'. Derived types must have the same value for IsReference as the base type. Change the setting on type '{0}' to '{3}', or on type '{2}' to '{1}', or do not set IsReference explicitly.";

	public const string IndexedPropertyCannotBeSerialized = "Property '{1}' in type '{0}' cannot be serialized because serialization of indexed properties is not supported.";

	public const string InterfaceTypeCannotBeCreated = "Interface type '{0}' cannot be created. Consider replacing with a non-interface serializable type.";

	public const string InvalidCollectionContractItemName = "Type '{0}' cannot have CollectionDataContractAttribute attribute ItemName set to null or empty string.";

	public const string InvalidCollectionContractKeyName = "Type '{0}' cannot have CollectionDataContractAttribute attribute KeyName set to null or empty string.";

	public const string InvalidCollectionContractKeyNoDictionary = "The collection data contract type '{0}' specifies '{1}' for the KeyName property. This is not allowed since the type is not IDictionary. Remove the setting for the KeyName property.";

	public const string InvalidCollectionContractName = "Type '{0}' cannot have CollectionDataContractAttribute attribute Name set to null or empty string.";

	public const string InvalidCollectionContractNamespace = "Type '{0}' cannot have CollectionDataContractAttribute attribute Namespace set to null.";

	public const string InvalidCollectionContractValueName = "Type '{0}' cannot have CollectionDataContractAttribute attribute ValueName set to null or empty string.";

	public const string InvalidCollectionContractValueNoDictionary = "The collection data contract type '{0}' specifies '{1}' for the ValueName property. This is not allowed since the type is not IDictionary. Remove the setting for the ValueName property.";

	public const string InvalidCollectionDataContract = "Type '{0}' with CollectionDataContractAttribute attribute is an invalid collection type since it";

	public const string InvalidCollectionType = "Type '{0}' is an invalid collection type since it";

	public const string InvalidDataContractName = "Type '{0}' cannot have DataContractAttribute attribute Name set to null or empty string.";

	public const string InvalidDataContractNamespace = "Type '{0}' cannot have DataContractAttribute attribute Namespace set to null.";

	public const string InvalidDataMemberName = "Member '{0}' in type '{1}' cannot have DataMemberAttribute attribute Name set to null or empty string.";

	public const string InvalidEnumMemberValue = "'{0}' in type '{1}' cannot have EnumMemberAttribute attribute Value set to null or empty string.";

	public const string InvalidEnumValueOnRead = "Invalid enum value '{0}' cannot be deserialized into type '{1}'. Ensure that the necessary enum values are present and are marked with EnumMemberAttribute attribute if the type has DataContractAttribute attribute.";

	public const string InvalidEnumValueOnWrite = "Enum value '{0}' is invalid for type '{1}' and cannot be serialized. Ensure that the necessary enum values are present and are marked with EnumMemberAttribute attribute if the type has DataContractAttribute attribute.";

	public const string InvalidGetSchemaMethod = "Type '{0}' cannot have MethodName on XmlSchemaProviderAttribute attribute set to null or empty string.";

	public const string InvalidGlobalDataContractNamespace = "CLR namespace '{0}' cannot have ContractNamespace set to null.";

	public const string InvalidMember = "Member '{0}.{1}' cannot be serialized since it is neither a field nor a property, and therefore cannot be marked with the DataMemberAttribute attribute. Remove the DataMemberAttribute attribute from the '{1}' member.";

	public const string InvalidNonNullReturnValueByIsAny = "Method '{0}.{1}()' returns a non-null value. The return value must be null since IsAny=true.";

	public const string InvalidPrimitiveType = "Type '{0}' is not a valid serializable type.";

	public const string InvalidReturnTypeOnGetSchemaMethod = "Method '{0}.{1}()' returns '{2}'. The return type must be compatible with '{3}'.";

	public const string InvalidSizeDefinition = "Invalid Size '{0}'. Must be non-negative integer.";

	public const string InvalidXmlDataContractName = "XML data contract Name for type '{0}' cannot be set to null or empty string.";

	public const string InvalidXsIdDefinition = "Invalid Id '{0}'. Must not be null or empty.";

	public const string InvalidXsRefDefinition = "Invalid Ref '{0}'. Must not be null or empty.";

	public const string IsAnyCannotBeNull = "A null value cannot be serialized at the top level for IXmlSerializable root type '{0}' since its IsAny setting is 'true'. This type must write all its contents including the root element. Verify that the IXmlSerializable implementation is correct.";

	public const string IsAnyCannotBeSerializedAsDerivedType = "An object of type '{0}' cannot be serialized at the top level for IXmlSerializable root type '{1}' since its IsAny setting is 'true'. This type must write all its contents including the root element. Verify that the IXmlSerializable implementation is correct.";

	public const string IsAnyCannotHaveXmlRoot = "Type '{0}' cannot specify an XmlRootAttribute attribute because its IsAny setting is 'true'. This type must write all its contents including the root element. Verify that the IXmlSerializable implementation is correct.";

	public const string IsNotAssignableFrom = "An internal error has occurred. '{0}' is not assignable from '{1}' - error generating code for serialization.";

	public const string IsRequiredDataMemberOnIsReferenceDataContractType = "'{0}.{1}' has the IsRequired setting of '{2}. However, '{0}' has the IsReference setting of '{2}', because either it is set explicitly, or it is derived from a base class. Set IsRequired on '{0}.{1}' to false, or disable IsReference on '{0}'.";

	public const string IXmlSerializableCannotHaveCollectionDataContract = "Type '{0}' cannot be IXmlSerializable and have CollectionDataContractAttribute attribute.";

	public const string IXmlSerializableCannotHaveDataContract = "Type '{0}' cannot be IXmlSerializable and have DataContractAttribute attribute.";

	public const string IXmlSerializableIllegalOperation = "This method cannot be called from IXmlSerializable implementations.";

	public const string IXmlSerializableMissingEndElements = "IXmlSerializable.WriteXml method of type '{0}' did not close all open tags. Verify that the IXmlSerializable implementation is correct.";

	public const string IXmlSerializableMustHaveDefaultConstructor = "IXmlSerializable Type '{0}' must have default constructor.";

	public const string IXmlSerializableWritePastSubTree = "IXmlSerializable.WriteXml method of type '{0}' attempted to close too many tags.  Verify that the IXmlSerializable implementation is correct.";

	public const string KnownTypeAttributeEmptyString = "Method name specified by KnownTypeAttribute attribute on type '{0}' cannot be the empty string.";

	public const string KnownTypeAttributeUnknownMethod = "KnownTypeAttribute attribute on type '{1}' specifies a method named '{0}' to provide known types. Static method '{0}()' was not found on this type. Ensure that the method exists and is marked as static.";

	public const string KnownTypeAttributeReturnType = "KnownTypeAttribute attribute on type '{0}' specifies a method named '{1}' to provide known types. The return type of this method is invalid because it is not assignable to IEnumerable<Type>. Ensure that the method exists and has a valid signature.";

	public const string KnownTypeAttributeOneScheme = "Type '{0}': If a KnownTypeAttribute attribute specifies a method it must be the only KnownTypeAttribute attribute on that type.";

	public const string KnownTypeAttributeNoType = "KnownTypeAttribute attribute on type '{0}' contains no Type.";

	public const string KnownTypeConfigClosedGenericDeclared = "Declared type '{0}' in config cannot be a closed or partial generic type.";

	public const string KnownTypeAttributeValidMethodTypes = "Method specified by KnownTypeAttribute attribute on type '{0}' does not expose valid types.";

	public const string KnownTypeAttributeNoData = "KnownTypeAttribute attribute on type '{0}' contains no data.";

	public const string KnownTypeAttributeMethodNull = "Method specified by KnownTypeAttribute attribute on type '{0}' returned null.";

	public const string MaxArrayLengthExceeded = "The maximum array length ({0}) has been exceeded while reading XML data for array of type '{1}'.";

	public const string MissingGetSchemaMethod = "Type '{0}' does not have a static method '{1}' that takes a parameter of type 'System.Xml.Schema.XmlSchemaSet' as specified by the XmlSchemaProviderAttribute attribute.";

	public const string MultipleIdDefinition = "Invalid XML encountered. The same Id value '{0}' is defined more than once. Multiple objects cannot be deserialized using the same Id.";

	public const string NoConversionPossibleTo = "An internal error has occurred. No conversion is possible to '{0}' - error generating code for serialization.";

	public const string NoGetMethodForProperty = "No get method for property '{1}' in type '{0}'.";

	public const string NoSetMethodForProperty = "No set method for property '{1}' in type '{0}'.";

	public const string NullKnownType = "One of the known types provided to the serializer via '{0}' argument was invalid because it was null. All known types specified must be non-null values.";

	public const string NullValueReturnedForGetOnlyCollection = "The get-only collection of type '{0}' returned a null value.  The input stream contains collection items which cannot be added if the instance is null.  Consider initializing the collection either in the constructor of the the object or in the getter.";

	public const string ObjectTableOverflow = "An internal error has occurred. Object table overflow. This could be caused by serializing or deserializing extremely large object graphs.";

	public const string OrderCannotBeNegative = "Property 'Order' in DataMemberAttribute attribute cannot be a negative number.";

	public const string ParameterCountMismatch = "Invalid number of parameters to call method '{0}'. Expected '{1}' parameters, but '{2}' were provided.";

	public const string PartialTrustCollectionContractAddMethodNotPublic = "The collection data contract type '{0}' cannot be deserialized because the method '{1}' is not public. Making the method public will fix this error. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string PartialTrustCollectionContractNoPublicConstructor = "The collection data contract type '{0}' cannot be deserialized because it does not have a public parameterless constructor. Adding a public parameterless constructor will fix this error. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string PartialTrustCollectionContractTypeNotPublic = "The collection data contract type '{0}' cannot be deserialized because it does not have a public parameterless constructor. Adding a public parameterless constructor will fix this error. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string PartialTrustDataContractOnSerializingNotPublic = "The data contract type '{0}' cannot be serialized because the OnSerializing method '{1}' is not public. Making the method public will fix this error. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string PartialTrustDataContractOnSerializedNotPublic = "The data contract type '{0}' cannot be serialized because the OnSerialized method '{1}' is not public. Making the method public will fix this error. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string PartialTrustDataContractOnDeserializingNotPublic = "The data contract type '{0}' cannot be deserialized because the OnDeserializing method '{1}' is not public. Making the method public will fix this error. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string PartialTrustDataContractOnDeserializedNotPublic = "The data contract type '{0}' cannot be deserialized because the OnDeserialized method '{1}' is not public. Making the method public will fix this error. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string PartialTrustDataContractFieldGetNotPublic = "The data contract type '{0}' cannot be serialized because the member '{1}' is not public. Making the member public will fix this error. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string PartialTrustDataContractFieldSetNotPublic = "The data contract type '{0}' cannot be deserialized because the member '{1}' is not public. Making the member public will fix this error. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string PartialTrustDataContractPropertyGetNotPublic = "The data contract type '{0}' cannot be serialized because the property '{1}' does not have a public getter. Adding a public getter will fix this error. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string PartialTrustDataContractPropertySetNotPublic = "The data contract type '{0}' cannot be deserialized because the property '{1}' does not have a public setter. Adding a public setter will fix this error. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string PartialTrustDataContractTypeNotPublic = "The data contract type '{0}' is not serializable because it is not public. Making the type public will fix this error. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string PartialTrustNonAttributedSerializableTypeNoPublicConstructor = "The type '{0}' cannot be deserialized because it does not have a public parameterless constructor. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string PartialTrustIXmlSerializableTypeNotPublic = "The IXmlSerializable type '{0}' is not serializable in partial trust because it is not public. Adding a public parameterless constructor will fix this error. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string PartialTrustIXmlSerialzableNoPublicConstructor = "The IXmlSerializable type '{0}' cannot be deserialized because it does not have a public parameterless constructor. Adding a public parameterless constructor will fix this error. Alternatively, you can make it internal, and use the InternalsVisibleToAttribute attribute on your assembly in order to enable serialization of internal members - see documentation for more details. Be aware that doing so has certain security implications.";

	public const string NonAttributedSerializableTypesMustHaveDefaultConstructor = "The Type '{0}' must have a parameterless constructor.";

	public const string AttributedTypesCannotInheritFromNonAttributedSerializableTypes = "Type '{0}' cannot inherit from a type that is not marked with DataContractAttribute or SerializableAttribute.  Consider marking the base type '{1}' with DataContractAttribute or SerializableAttribute, or removing them from the derived type.";

	public const string GetOnlyCollectionsNotSupported = "Get-only collection properties are not supported.  Consider adding a public setter to property '{0}.{1}' or marking the it with the IgnoreDataMemberAttribute.";

	public const string QuotaMustBePositive = "Quota must be a positive value.";

	public const string QuotaIsReadOnly = "The '{0}' quota is readonly.";

	public const string QuotaCopyReadOnly = "Cannot copy XmlDictionaryReaderQuotas. Target is readonly.";

	public const string RequiredMemberMustBeEmitted = "Member {0} in type {1} cannot be serialized. This exception is usually caused by trying to use a null value where a null value is not allowed. The '{0}' member is set to its default value (usually null or zero). The member's EmitDefault setting is 'false', indicating that the member should not be serialized. However, the member's IsRequired setting is 'true', indicating that it must be serialized. This conflict cannot be resolved.  Consider setting '{0}' to a non-default value. Alternatively, you can change the EmitDefaultValue property on the DataMemberAttribute attribute to true, or changing the IsRequired property to false.";

	public const string ResolveTypeReturnedFalse = "An object of type '{0}' which derives from DataContractResolver returned false from its TryResolveType method when attempting to resolve the name for an object of type '{1}', indicating that the resolution failed. Change the TryResolveType implementation to return true.";

	public const string ResolveTypeReturnedNull = "An object of type '{0}' which derives from DataContractResolver returned a null typeName or typeNamespace but not both from its TryResolveType method when attempting to resolve the name for an object of type '{1}'. Change the TryResolveType implementation to return non-null values, or to return null values for both typeName and typeNamespace in order to serialize as the declared type.";

	public const string SupportForMultidimensionalArraysNotPresent = "Multi-dimensional arrays are not supported.";

	public const string TooManyCollectionContracts = "Type '{0}' has more than one CollectionDataContractAttribute attribute.";

	public const string TooManyDataContracts = "Type '{0}' has more than one DataContractAttribute attribute.";

	public const string TooManyDataMembers = "Member '{0}.{1}' has more than one DataMemberAttribute attribute.";

	public const string TooManyEnumMembers = "Member '{0}.{1}' has more than one EnumMemberAttribute attribute.";

	public const string TooManyIgnoreDataMemberAttributes = "Member '{0}.{1}' has more than one IgnoreDataMemberAttribute attribute.";

	public const string TypeMustBeConcrete = "Error while getting known types for Type '{0}'. The type must not be an open or partial generic class.";

	public const string TypeNotSerializable = "Type '{0}' cannot be serialized. Consider marking it with the DataContractAttribute attribute, and marking all of its members you want serialized with the DataMemberAttribute attribute. Alternatively, you can ensure that the type is public and has a parameterless constructor - all public members of the type will then be serialized, and no attributes will be required.";

	public const string UnexpectedContractType = "An internal error has occurred. Unexpected contract type '{0}' for type '{1}' encountered.";

	public const string UnexpectedElementExpectingElements = "'{0}' '{1}' from namespace '{2}' is not expected. Expecting element '{3}'.";

	public const string UnexpectedEndOfFile = "Unexpected end of file.";

	public const string UnknownConstantType = "Unrecognized constant type '{0}'.";

	public const string UnsupportedIDictionaryAsDataMemberType = "Cannot deserialize one of the DataMember because it is an IDictionary. Use IDictionary<K,V> instead.";

	public const string ValueMustBeNonNegative = "The value of this argument must be non-negative.";

	public const string ValueTypeCannotBeNull = "ValueType '{0}' cannot be null.";

	public const string ValueTypeCannotHaveBaseType = "Data contract '{0}' from namespace '{1}' is a value type and cannot have base contract '{2}' from namespace '{3}'.";

	public const string ValueTypeCannotHaveId = "ValueType '{0}' cannot have id.";

	public const string ValueTypeCannotHaveIsReference = "Value type '{0}' cannot have the IsReference setting of '{1}'. Either change the setting to '{2}', or remove it completely.";

	public const string ValueTypeCannotHaveRef = "ValueType '{0}' cannot have ref to another object.";

	public const string XmlElementAttributes = "Only Element nodes have attributes.";

	public const string XmlForObjectCannotHaveContent = "Element {0} from namespace {1} cannot have child contents to be deserialized as an object. Please use XElement to deserialize this pattern of XML.";

	public const string XmlInvalidConversion = "The value '{0}' cannot be parsed as the type '{1}'.";

	public const string XmlInvalidConversionWithoutValue = "The value cannot be parsed as the type '{0}'.";

	public const string XmlStartElementExpected = "Start element expected. Found {0}.";

	public const string XmlWriterMustBeInElement = "WriteState '{0}' not valid. Caller must write start element before serializing in contentOnly mode.";

	public const string OffsetExceedsBufferSize = "The specified offset exceeds the buffer size ({0} bytes).";

	public const string SizeExceedsRemainingBufferSpace = "The specified size exceeds the remaining buffer space ({0} bytes).";

	public const string ValueMustBeInRange = "The value of this argument must fall within the range {0} to {1}.";

	public const string XmlArrayTooSmallOutput = "Array too small.  Must be able to hold at least {0}.";

	public const string XmlInvalidBase64Length = "Base64 sequence length ({0}) not valid. Must be a multiple of 4.";

	public const string XmlInvalidBase64Sequence = "The characters '{0}' at offset {1} are not a valid Base64 sequence.";

	public const string XmlInvalidBinHexLength = "BinHex sequence length ({0}) not valid. Must be a multiple of 2.";

	public const string XmlInvalidBinHexSequence = "The characters '{0}' at offset {1} are not a valid BinHex sequence.";

	public const string XmlInvalidHighSurrogate = "High surrogate char '0x{0}' not valid. High surrogate chars range from 0xD800 to 0xDBFF.";

	public const string XmlInvalidLowSurrogate = "Low surrogate char '0x{0}' not valid. Low surrogate chars range from 0xDC00 to 0xDFFF.";

	public const string XmlInvalidSurrogate = "Surrogate char '0x{0}' not valid. Surrogate chars range from 0x10000 to 0x10FFFF.";

	public const string CombinedPrefixNSLength = "The combined length of the prefix and namespace must not be greater than {0}.";

	public const string InvalidInclusivePrefixListCollection = "The inclusive namespace prefix collection cannot contain null as one of the items.";

	public const string InvalidLocalNameEmpty = "The empty string is not a valid local name.";

	public const string XmlArrayTooSmall = "Array too small.";

	public const string XmlArrayTooSmallInput = "Array too small.  Length of available data must be at least {0}.";

	public const string XmlBadBOM = "Unrecognized Byte Order Mark.";

	public const string XmlBase64DataExpected = "Base64 encoded data expected. Found {0}.";

	public const string XmlCDATAInvalidAtTopLevel = "CData elements not valid at top level of an XML document.";

	public const string XmlCloseCData = "']]>' not valid in text node content.";

	public const string XmlConversionOverflow = "The value '{0}' cannot be represented with the type '{1}'.";

	public const string XmlDeclarationRequired = "An XML declaration with an encoding is required for all non-UTF8 documents.";

	public const string XmlDeclMissingVersion = "Version not found in XML declaration.";

	public const string XmlDeclMissing = "An XML declaration is required for all non-UTF8 documents.";

	public const string XmlDeclNotFirst = "No characters can appear before the XML declaration.";

	public const string XmlDictionaryStringIDRange = "XmlDictionaryString IDs must be in the range from {0} to {1}.";

	public const string XmlDictionaryStringIDUndefinedSession = "XmlDictionaryString ID {0} not defined in the XmlBinaryReaderSession.";

	public const string XmlDictionaryStringIDUndefinedStatic = "XmlDictionaryString ID {0} not defined in the static dictionary.";

	public const string XmlDuplicateAttribute = "Duplicate attribute found. Both '{0}' and '{1}' are from the namespace '{2}'.";

	public const string XmlEmptyNamespaceRequiresNullPrefix = "The empty namespace requires a null or empty prefix.";

	public const string XmlEncodingMismatch = "The encoding in the declaration '{0}' does not match the encoding of the document '{1}'.";

	public const string XmlEncodingNotSupported = "XML encoding not supported.";

	public const string XmlEndElementExpected = "End element '{0}' from namespace '{1}' expected. Found {2}.";

	public const string XmlEndElementNoOpenNodes = "No corresponding start element is open.";

	public const string XmlExpectedEncoding = "The expected encoding '{0}' does not match the actual encoding '{1}'.";

	public const string XmlFoundCData = "cdata '{0}'";

	public const string XmlFoundComment = "comment '{0}'";

	public const string XmlFoundElement = "element '{0}' from namespace '{1}'";

	public const string XmlFoundEndElement = "end element '{0}' from namespace '{1}'";

	public const string XmlFoundEndOfFile = "end of file";

	public const string XmlFoundNodeType = "node {0}";

	public const string XmlFoundText = "text '{0}'";

	public const string XmlFullStartElementExpected = "Non-empty start element expected. Found {0}.";

	public const string XmlFullStartElementLocalNameNsExpected = "Non-empty start element '{0}' from namespace '{1}' expected. Found {2}.";

	public const string XmlFullStartElementNameExpected = "Non-empty start element '{0}' expected. Found {1}.";

	public const string XmlIDDefined = "ID already defined.";

	public const string XmlKeyAlreadyExists = "The specified key already exists in the dictionary.";

	public const string XmlIllegalOutsideRoot = "Text cannot be written outside the root element.";

	public const string XmlInvalidBytes = "Invalid byte encoding.";

	public const string XmlInvalidCharRef = "Character reference not valid.";

	public const string XmlInvalidCommentChars = "XML comments cannot contain '--' or end with '-'.";

	public const string XmlInvalidDeclaration = "XML declaration can only be written at the beginning of the document.";

	public const string XmlInvalidDepth = "Cannot call '{0}' while Depth is '{1}'.";

	public const string XmlInvalidEncoding = "XML encoding must be 'UTF-8'.";

	public const string XmlInvalidFFFE = "Characters with hexadecimal values 0xFFFE and 0xFFFF are not valid.";

	public const string XmlInvalidFormat = "The input source is not correctly formatted.";

	public const string XmlInvalidID = "ID must be >= 0.";

	public const string XmlInvalidOperation = "The reader cannot be advanced.";

	public const string XmlInvalidPrefixState = "A prefix cannot be defined while WriteState is '{0}'.";

	public const string XmlInvalidQualifiedName = "Expected XML qualified name. Found '{0}'.";

	public const string XmlInvalidRootData = "The data at the root level is invalid.";

	public const string XmlInvalidStandalone = "'standalone' value in declaration must be 'yes' or 'no'.";

	public const string XmlInvalidStream = "Stream returned by IStreamProvider cannot be null.";

	public const string XmlInvalidUniqueId = "UniqueId cannot be zero length.";

	public const string XmlInvalidUTF8Bytes = "'{0}' contains invalid UTF8 bytes.";

	public const string XmlInvalidVersion = "XML version must be '1.0'.";

	public const string XmlInvalidWriteState = "'{0}' cannot be called while WriteState is '{1}'.";

	public const string XmlInvalidXmlByte = "The byte 0x{0} is not valid at this location.";

	public const string XmlInvalidXmlSpace = "'{0}' is not a valid xml:space value. Valid values are 'default' and 'preserve'.";

	public const string XmlLineInfo = "Line {0}, position {1}.";

	public const string XmlMalformedDecl = "Malformed XML declaration.";

	public const string XmlMaxArrayLengthExceeded = "The maximum array length quota ({0}) has been exceeded while reading XML data. This quota may be increased by changing the MaxArrayLength property on the XmlDictionaryReaderQuotas object used when creating the XML reader.";

	public const string XmlMaxNameTableCharCountExceeded = "The maximum nametable character count quota ({0}) has been exceeded while reading XML data. The nametable is a data structure used to store strings encountered during XML processing - long XML documents with non-repeating element names, attribute names and attribute values may trigger this quota. This quota may be increased by changing the MaxNameTableCharCount property on the XmlDictionaryReaderQuotas object used when creating the XML reader.";

	public const string XmlMethodNotSupported = "This XmlWriter implementation does not support the '{0}' method.";

	public const string XmlMissingLowSurrogate = "The surrogate pair is invalid. Missing a low surrogate character.";

	public const string XmlMultipleRootElements = "There are multiple root elements.";

	public const string XmlNamespaceNotFound = "The namespace '{0}' is not defined.";

	public const string XmlNestedArraysNotSupported = "Nested arrays are not supported.";

	public const string XmlNoRootElement = "The document does not have a root element.";

	public const string XmlOnlyOneRoot = "Only one root element is permitted per document.";

	public const string XmlOnlyWhitespace = "Only white space characters can be written with this method.";

	public const string XmlOnlySingleValue = "Only a single typed value may be written inside an attribute or content.";

	public const string XmlPrefixBoundToNamespace = "The prefix '{0}' is bound to the namespace '{1}' and cannot be changed to '{2}'.";

	public const string XmlProcessingInstructionNotSupported = "Processing instructions (other than the XML declaration) and DTDs are not supported.";

	public const string XmlReservedPrefix = "Prefixes beginning with \"xml\" (regardless of casing) are reserved for use by XML.";

	public const string XmlSpaceBetweenAttributes = "Whitespace must appear between attributes.";

	public const string XmlSpecificBindingNamespace = "The namespace '{1}' can only be bound to the prefix '{0}'.";

	public const string XmlSpecificBindingPrefix = "The prefix '{0}' can only be bound to the namespace '{1}'.";

	public const string XmlStartElementLocalNameNsExpected = "Start element '{0}' from namespace '{1}' expected. Found {2}.";

	public const string XmlStartElementNameExpected = "Start element '{0}' expected. Found {1}.";

	public const string XmlTagMismatch = "Start element '{0}' does not match end element '{1}'.";

	public const string XmlTokenExpected = "The token '{0}' was expected but found '{1}'.";

	public const string XmlUndefinedPrefix = "The prefix '{0}' is not defined.";

	public const string XmlUnexpectedEndElement = "No matching start tag for end element.";

	public const string XmlUnexpectedEndOfFile = "Unexpected end of file. Following elements are not closed: {0}.";

	public const string XmlWriterClosed = "The XmlWriter is closed.";

	public const string Xml_InvalidNmToken = "Invalid NmToken value '{0}'.";

	public const string AbstractElementNotSupported = "Abstract element '{0}' is not supported.";

	public const string AbstractTypeNotSupported = "Abstract type is not supported";

	public const string AmbiguousReferencedCollectionTypes1 = "Ambiguous collection types were referenced: {0}";

	public const string AmbiguousReferencedCollectionTypes3 = "In '{0}' element in '{1}' namespace, ambiguous collection types were referenced: {2}";

	public const string AmbiguousReferencedTypes1 = "Ambiguous types were referenced: {0}";

	public const string AmbiguousReferencedTypes3 = "In '{0}' element in '{1}' namespace, ambiguous types were referenced: {2}";

	public const string AnnotationAttributeNotFound = "Annotation attribute was not found: default value annotation is '{0}', type is '{1}' in '{2}' namespace, emit default value is {3}.";

	public const string AnonymousTypeNotSupported = "Anonymous type is not supported. Type is '{0}' in '{1}' namespace.";

	public const string AnyAttributeNotSupported = "XML Schema 'any' attribute is not supported";

	public const string ArrayItemFormMustBe = "For array item, element 'form' must be {0}.";

	public const string ArraySizeAttributeIncorrect = "Array size attribute is incorrect; must be between {0} and {1}.";

	public const string ArrayTypeCannotBeImported = "Array type cannot be imported for '{0}' in '{1}' namespace: {2}.";

	public const string AssemblyNotFound = "Assembly '{0}' was not found.";

	public const string AttributeNotFound = "Attribute was not found for CLR type '{1}' in namespace '{0}'. XML reader node is on {2}, '{4}' node in '{3}' namespace.";

	public const string BaseTypeNotISerializable = "Base type '{0}' in '{1}' namespace is not ISerializable.";

	public const string CannotComputeUniqueName = "Cannot compute unique name for '{0}'.";

	public const string CannotDeriveFromSealedReferenceType = "Cannod drive from sealed reference type '{2}', for '{0}' element in '{1}' namespace.";

	public const string CannotDeserializeForwardedType = "Cannot deserialize forwarded type '{0}'.";

	public const string CannotExportNullAssembly = "Cannot export null assembly.";

	public const string CannotExportNullKnownType = "Cannot export null known type.";

	public const string CannotExportNullType = "Cannot export null type.";

	public const string CannotHaveDuplicateAttributeNames = "Cannot have duplicate attribute names '{0}'.";

	public const string CannotHaveDuplicateElementNames = "Cannot have duplicate element names '{0}'.";

	public const string CannotImportInvalidSchemas = "Cannot import invalid schemas.";

	public const string CannotImportNullDataContractName = "Cannot import data contract with null name.";

	public const string CannotImportNullSchema = "Cannot import from schema list that contains null.";

	public const string CannotSetMembersForReferencedType = "Cannot set members for already referenced type. Base type is '{0}'.";

	public const string CannotSetNamespaceForReferencedType = "Cannot set namespace for already referenced type. Base type is '{0}'.";

	public const string CannotUseGenericTypeAsBase = "For '{0}' in '{1}' namespace, generic type cannot be referenced as the base type.";

	public const string ChangingFullTypeNameNotSupported = "Changing full type name is not supported. Serialization type name: '{0}', data contract type name: '{1}'.";

	public const string CircularTypeReference = "Circular type reference was found for '{0}' in '{1}' namespace.";

	public const string ClassDataContractReturnedForGetOnlyCollection = "For '{0}' type, class data contract was returned for get-only collection.";

	public const string CLRNamespaceMappedMultipleTimes = "CLR namespace is mapped multiple times. Current data contract namespace is '{0}', found '{1}' for CLR namespace '{2}'.";

	public const string ClrTypeNotFound = "CLR type '{1}' in assembly '{0}' is not found.";

	public const string CollectionAssignedToIncompatibleInterface = "Collection of type '{0}' is assigned to an incompatible interface '{1}'";

	public const string ComplexTypeRestrictionNotSupported = "XML schema complexType restriction is not supported.";

	public const string ConfigDataContractSerializerSectionLoadError = "Failed to load configuration section for dataContractSerializer.";

	public const string ConfigIndexOutOfRange = "For type '{0}', configuration index is out of range.";

	public const string ConfigMustOnlyAddParamsWithType = "Configuration parameter element must only add params with type.";

	public const string ConfigMustOnlySetTypeOrIndex = "Configuration parameter element can set only one of either type or index.";

	public const string ConfigMustSetTypeOrIndex = "Configuration parameter element must set either type or index.";

	public const string CouldNotReadSerializationSchema = "Could not read serialization schema for '{0}' namespace.";

	public const string DefaultOnElementNotSupported = "On element '{0}', default value is not supported.";

	public const string DerivedTypeNotISerializable = "On type '{0}' in '{1}' namespace, derived type is not ISerializable.";

	public const string DupContractInDataContractSet = "Duplicate contract in data contract set was found, for '{0}' in '{1}' namespace.";

	public const string DuplicateExtensionDataSetMethod = "Duplicate extension data set method was found, for method '{0}', existing method is '{1}', on data contract type '{2}'.";

	public const string DupTypeContractInDataContractSet = "Duplicate type contract in data contract set. Type name '{0}', for data contract '{1}' in '{2}' namespace.";

	public const string ElementMaxOccursMustBe = "On element '{0}', schema element maxOccurs must be 1.";

	public const string ElementMinOccursMustBe = "On element '{0}', schema element minOccurs must be less or equal to 1.";

	public const string ElementRefOnLocalElementNotSupported = "For local element, ref is not supported. The referenced name is '{0}' in '{1}' namespace.";

	public const string EnumEnumerationFacetsMustHaveValue = "Schema enumeration facet must have values.";

	public const string EnumListInAnonymousTypeNotSupported = "Enum list in anonymous type is not supported.";

	public const string EnumListMustContainAnonymousType = "Enum list must contain an anonymous type.";

	public const string EnumOnlyEnumerationFacetsSupported = "For schema facets, only enumeration is supported.";

	public const string EnumRestrictionInvalid = "For simpleType restriction, only enum is supported and this type could not be convert to enum.";

	public const string EnumTypeCannotBeImported = "For '{0}' in '{1}' namespace, enum type cannot be imported: {2}";

	public const string EnumTypeNotSupportedByDataContractJsonSerializer = "Enum type is not supported by DataContractJsonSerializer. The underlying type is '{0}'.";

	public const string EnumUnionInAnonymousTypeNotSupported = "Enum union in anonymous type is not supported.";

	public const string ExtensionDataSetMustReturnVoid = "For type '{0}' method '{1}', extension data set method must return void.";

	public const string ExtensionDataSetParameterInvalid = "For type '{0}' method '{1}', extension data set method has invalid type of parameter '{2}'.";

	public const string FactoryObjectContainsSelfReference = "Factory object contains a reference to self. Old object is '{0}', new object is '{1}'.";

	public const string FactoryTypeNotISerializable = "For data contract '{1}', factory type '{0}' is not ISerializable.";

	public const string FixedOnElementNotSupported = "On schema element '{0}', fixed value is not supported.";

	public const string FlushBufferAlreadyInUse = "Flush buffer is already in use.";

	public const string FormMustBeQualified = "On schema element '{0}', form must be qualified.";

	public const string GenericAnnotationAttributeNotFound = "On type '{0}' Generic annotation attribute '{1}' was not found.";

	public const string GenericAnnotationForNestedLevelMustBeIncreasing = "On type '{2}', generic annotation for nested level must be increasing. Argument element is '{0}' in '{1}' namespace.";

	public const string GenericAnnotationHasInvalidAttributeValue = "On type '{2}', generic annotation has invalid attribute value '{3}'. Argument element is '{0}' in '{1}' namespace. Nested level attribute attribute name is '{4}'. Type is '{5}'.";

	public const string GenericAnnotationHasInvalidElement = "On type '{2}', generic annotation has invalid element. Argument element is '{0}' in '{1}' namespace.";

	public const string GenericTypeNameMismatch = "Generic type name mismatch. Expected '{0}' in '{1}' namespace, got '{2}' in '{3}' namespace instead.";

	public const string GenericTypeNotExportable = "Generic type '{0}' is not exportable.";

	public const string GetOnlyCollectionMustHaveAddMethod = "On type '{0}', get-only collection must have an Add method.";

	public const string GetRealObjectReturnedNull = "On the surrogate data contract for '{0}', GetRealObject method returned null.";

	public const string InvalidAnnotationExpectingText = "For annotation element '{0}' in namespace '{1}', expected text but got element '{2}' in '{3}' namespace.";

	public const string InvalidAssemblyFormat = "'{0}': invalid assembly format.";

	public const string InvalidCharacterEncountered = "Encountered an invalid character '{0}'.";

	public const string InvalidClassDerivation = "Invalid class derivation from '{0}' in '{1}' namespace.";

	public const string InvalidClrNameGeneratedForISerializable = "Invalid CLR name '{2}' is generated for ISerializable type '{0}' in '{1}' namespace.";

	public const string InvalidClrNamespaceGeneratedForISerializable = "Invalid CLR namespace '{3}' is generated for ISerializable type '{0}' in '{1}' namespace. Data contract namespace from the URI would be generated as '{2}'.";

	public const string InvalidDataNode = "Invalid data node for '{0}' type.";

	public const string InvalidEmitDefaultAnnotation = "Invalid EmilDefault annotation for '{0}' in type '{1}' in '{2}' namespace.";

	public const string InvalidEnumBaseType = "Invalid enum base type is specified for type '{0}' in '{1}' namespace, element name is '{2}' in '{3}' namespace.";

	public const string InvalidISerializableDerivation = "Invalid ISerializable derivation from '{0}' in '{1}' namespace.";

	public const string InvalidKeyValueType = "'{0}' is an invalid key value type.";

	public const string InvalidKeyValueTypeNamespace = "'{0}' in '{1}' namespace is an invalid key value type.";

	public const string InvalidReturnSchemaOnGetSchemaMethod = "On type '{0}', the return value from GetSchema method was invalid.";

	public const string InvalidStateInExtensionDataReader = "Invalid state in extension data reader.";

	public const string InvalidXmlDeserializingExtensionData = "Invalid XML while deserializing extension data.";

	public const string IsAnyNotSupportedByNetDataContractSerializer = "For type '{0}', IsAny is not supported by NetDataContractSerializer.";

	public const string IsDictionaryFormattedIncorrectly = "IsDictionary formatted value '{0}' is incorrect: {1}";

	public const string ISerializableAssemblyNameSetToZero = "ISerializable AssemblyName is set to \"0\" for type '{0}'.";

	public const string ISerializableCannotHaveDataContract = "ISerializable type '{0}' cannot have DataContract.";

	public const string ISerializableContainsMoreThanOneItems = "ISerializable cannot contain more than one item.";

	public const string ISerializableDerivedContainsOneOrMoreItems = "Type derived from ISerializable cannot contain more than one item.";

	public const string ISerializableDoesNotContainAny = "ISerializable does not contain any element.";

	public const string ISerializableMustRefFactoryTypeAttribute = "ISerializable must have ref attribute that points to its factory type.";

	public const string ISerializableTypeCannotBeImported = "ISerializable type '{0}' in '{1}' namespace cannot be imported: {2}";

	public const string ISerializableWildcardMaxOccursMustBe = "ISerializable wildcard maxOccurs must be '{0}'.";

	public const string ISerializableWildcardMinOccursMustBe = "ISerializable wildcard maxOccurs must be '{0}'.";

	public const string ISerializableWildcardNamespaceInvalid = "ISerializable wildcard namespace is invalid: '{0}'.";

	public const string ISerializableWildcardProcessContentsInvalid = "ISerializable wildcard processContents is invalid: '{0}'.";

	public const string IsReferenceGetOnlyCollectionsNotSupported = "On type '{1}', attribute '{0}' points to get-only collection, which is not supported.";

	public const string IsValueTypeFormattedIncorrectly = "IsValueType is formatted incorrectly as '{0}': {1}";

	public const string JsonAttributeAlreadyWritten = "JSON attribute '{0}' is already written.";

	public const string JsonAttributeMustHaveElement = "JSON attribute must have an owner element.";

	public const string JsonCannotWriteStandaloneTextAfterQuotedText = "JSON writer cannot write standalone text after quoted text.";

	public const string JsonCannotWriteTextAfterNonTextAttribute = "JSON writer cannot write text after non-text attribute. Data type is '{0}'.";

	public const string JsonDateTimeOutOfRange = "JSON DateTime is out of range.";

	public const string JsonDuplicateMemberInInput = "Duplicate member '{0}' is found in JSON input.";

	public const string JsonDuplicateMemberNames = "Duplicate member, including '{1}', is found in JSON input, in type '{0}'.";

	public const string JsonEncodingNotSupported = "JSON Encoding is not supported.";

	public const string JsonEncounteredUnexpectedCharacter = "Encountered an unexpected character '{0}' in JSON.";

	public const string JsonEndElementNoOpenNodes = "Encountered an end element while there was no open element in JSON writer.";

	public const string JsonExpectedEncoding = "Expected encoding '{0}', got '{1}' instead.";

	public const string JsonInvalidBytes = "Invalid bytes in JSON.";

	public const string JsonInvalidDataTypeSpecifiedForServerType = "The specified data type is invalid for server type. Type: '{0}', specified data type: '{1}', server type: '{2}', object '{3}'.";

	public const string JsonInvalidDateTimeString = "Invalid JSON dateTime string is specified: original value '{0}', start guide writer: {1}, end guard writer: {2}.";

	public const string JsonInvalidFFFE = "FFFE in JSON is invalid.";

	public const string JsonInvalidItemNameForArrayElement = "Invalid JSON item name '{0}' for array element (item element is '{1}' in JSON).";

	public const string JsonInvalidLocalNameEmpty = "Empty string is invalid as a local name.";

	public const string JsonInvalidMethodBetweenStartEndAttribute = "Invalid method call state between start and end attribute.";

	public const string JsonInvalidRootElementName = "Invalid root element name '{0}' (root element is '{1}' in JSON).";

	public const string JsonInvalidStartElementCall = "Invalid call to JSON WriteStartElement method.";

	public const string JsonInvalidWriteState = "Invalid write state {1} for '{0}' method.";

	public const string JsonMethodNotSupported = "Method {0} is not supported in JSON.";

	public const string JsonMultipleRootElementsNotAllowedOnWriter = "Multiple root element is not allowed on JSON writer.";

	public const string JsonMustSpecifyDataType = "On JSON writer data type '{0}' must be specified. Object string is '{1}', server type string is '{2}'.";

	public const string JsonMustUseWriteStringForWritingAttributeValues = "On JSON writer WriteString must be used for writing attribute values.";

	public const string JsonNamespaceMustBeEmpty = "JSON namespace is specified as '{0}' but it must be empty.";

	public const string JsonNestedArraysNotSupported = "Nested array is not supported in JSON: '{0}'";

	public const string JsonNodeTypeArrayOrObjectNotSpecified = "Either Object or Array of JSON node type must be specified.";

	public const string JsonNoMatchingStartAttribute = "WriteEndAttribute was called while there is no open attribute.";

	public const string JsonOffsetExceedsBufferSize = "On JSON writer, offset exceeded buffer size {0}.";

	public const string JsonOneRequiredMemberNotFound = "Required member {1} in type '{0}' is not found.";

	public const string JsonOnlyWhitespace = "Only whitespace characters are allowed for {1} method. The specified value is '{0}'";

	public const string JsonOpenAttributeMustBeClosedFirst = "JSON attribute must be closed first before calling {0} method.";

	public const string JsonPrefixMustBeNullOrEmpty = "JSON prefix must be null or empty. '{0}' is specified instead.";

	public const string JsonRequiredMembersNotFound = "Required members {0} in type '{1}' are not found.";

	public const string JsonServerTypeSpecifiedForInvalidDataType = "Server type is specified for invalid data type in JSON. Server type: '{0}', type: '{1}', dataType: '{2}', object: '{3}'.";

	public const string JsonSizeExceedsRemainingBufferSpace = "JSON size exceeded remaining buffer space, by {0} byte(s).";

	public const string JsonTypeNotSupportedByDataContractJsonSerializer = "Type '{0}' is not suppotred by DataContractJsonSerializer.";

	public const string JsonUnexpectedAttributeLocalName = "Unexpected attribute local name '{0}'.";

	public const string JsonUnexpectedAttributeValue = "Unexpected attribute value '{0}'.";

	public const string JsonUnexpectedEndOfFile = "Unexpected end of file in JSON.";

	public const string JsonUnsupportedForIsReference = "Unsupported value for IsReference for type '{0}', IsReference value is {1}.";

	public const string JsonWriteArrayNotSupported = "JSON WriteArray is not supported.";

	public const string JsonWriterClosed = "JSON writer is already closed.";

	public const string JsonXmlInvalidDeclaration = "Attempt to write invalid XML declration.";

	public const string JsonXmlProcessingInstructionNotSupported = "processing instruction is not supported in JSON writer.";

	public const string KeyTypeCannotBeParsedInSimpleDictionary = "Key type '{1}' for collection type '{0}' cannot be parsed in simple dictionary.";

	public const string KnownTypeConfigGenericParamMismatch = "Generic parameter count do not match between known type and configuration. Type is '{0}', known type has {1} parameters, configuration has {2} parameters.";

	public const string KnownTypeConfigIndexOutOfBounds = "For known type configuration, index is out of bound. Root type: '{0}' has {1} type arguments, and index was {2}.";

	public const string KnownTypeConfigIndexOutOfBoundsZero = "For known type configuration, index is out of bound. Root type: '{0}' has {1} type arguments, and index was {2}.";

	public const string KnownTypeConfigObject = "Known type configuration specifies System.Object.";

	public const string MaxMimePartsExceeded = "MIME parts number exceeded the maximum settings. Must be less than {0}. Specified as '{1}'.";

	public const string MimeContentTypeHeaderInvalid = "MIME content type header is invalid.";

	public const string MimeHeaderInvalidCharacter = "MIME header has an invalid character ('{0}', {1} in hexadecimal value).";

	public const string MimeMessageGetContentStreamCalledAlready = "On MimeMessage, GetContentStream method is already called.";

	public const string MimeReaderHeaderAlreadyExists = "MIME header '{0}' already exists.";

	public const string MimeReaderMalformedHeader = "Malformed MIME header.";

	public const string MimeReaderResetCalledBeforeEOF = "On MimeReader, Reset method is called before EOF.";

	public const string MimeReaderTruncated = "MIME parts are truncated.";

	public const string MimeVersionHeaderInvalid = "MIME version header is invalid.";

	public const string MimeWriterInvalidStateForClose = "MIME writer is at invalid state for closing.";

	public const string MimeWriterInvalidStateForContent = "MIME writer is at invalid state for content.";

	public const string MimeWriterInvalidStateForHeader = "MIME writer is at invalid state for header.";

	public const string MimeWriterInvalidStateForStartPart = "MIME writer is at invalid state for starting a part.";

	public const string MimeWriterInvalidStateForStartPreface = "MIME writer is at invalid state for starting preface.";

	public const string MissingSchemaType = "Schema type '{0}' is missing and required for '{1}' type.";

	public const string MixedContentNotSupported = "Mixed content is not supported.";

	public const string MtomBoundaryInvalid = "MIME boundary is invalid: '{0}'.";

	public const string MtomBufferQuotaExceeded = "MTOM buffer quota exceeded. The maximum size is {0}.";

	public const string MtomContentTransferEncodingNotPresent = "MTOM content transfer encoding is not present. ContentTransferEncoding header is '{0}'.";

	public const string MtomContentTransferEncodingNotSupported = "MTOM content transfer encoding value is not supported. Raw value is '{0}', '{1}' in 7bit encoding, '{2}' in 8bit encoding, and '{3}' in binary.";

	public const string MtomContentTypeInvalid = "MTOM content type is invalid.";

	public const string MtomDataMustNotContainXopInclude = "MTOM data must not contain xop:Include element. '{0}' element in '{1}' namespace.";

	public const string MtomExceededMaxSizeInBytes = "MTOM exceeded max size in bytes. The maximum size is {0}.";

	public const string MtomInvalidCIDUri = "Invalid MTOM CID URI: '{0}'.";

	public const string MtomInvalidEmptyURI = "empty URI is invalid for MTOM MIME part.";

	public const string MtomInvalidStartUri = "Invalid MTOM start URI: '{0}'.";

	public const string MtomInvalidTransferEncodingForMimePart = "Invalid transfer encoding for MIME part: '{0}', in binary: '{1}'.";

	public const string MtomMessageContentTypeNotFound = "MTOM message content type was not found.";

	public const string MtomMessageInvalidContent = "MTOM message content is invalid.";

	public const string MtomMessageInvalidContentInMimePart = "MTOM message content in MIME part is invalid.";

	public const string MtomMessageInvalidMimeVersion = "MTOM message has invalid MIME version. Expected '{1}', got '{0}' instead.";

	public const string MtomMessageNotApplicationXopXml = "MTOM msssage type is not '{0}'.";

	public const string MtomMessageNotMultipart = "MTOM message is not multipart: media type should be '{0}', media subtype should be '{1}'.";

	public const string MtomMessageRequiredParamNotSpecified = "Required MTOM parameter '{0}' is not specified.";

	public const string MtomMimePartReferencedMoreThanOnce = "Specified MIME part '{0}' is referenced more than once.";

	public const string MtomPartNotFound = "MTOM part with URI '{0}' is not found.";

	public const string MtomRootContentTypeNotFound = "MTOM root content type is not found.";

	public const string MtomRootNotApplicationXopXml = "MTOM root should have media type '{0}' and subtype '{1}'.";

	public const string MtomRootPartNotFound = "MTOM root part is not found.";

	public const string MtomRootRequiredParamNotSpecified = "Required MTOM root parameter '{0}' is not specified.";

	public const string MtomRootUnexpectedCharset = "Unexpected charset on MTOM root. Expected '{1}', got '{0}' instead.";

	public const string MtomRootUnexpectedType = "Unexpected type on MTOM root. Expected '{1}', got '{0}' instead.";

	public const string MtomXopIncludeHrefNotSpecified = "xop Include element did not specify '{0}' attribute.";

	public const string MtomXopIncludeInvalidXopAttributes = "xop Include element has invalid attribute: '{0}' in '{1}' namespace.";

	public const string MtomXopIncludeInvalidXopElement = "xop Include element has invalid element: '{0}' in '{1}' namespace.";

	public const string MustContainOnlyLocalElements = "Only local elements can be imported.";

	public const string NoAsyncWritePending = "No async write operation is pending.";

	public const string NonOptionalFieldMemberOnIsReferenceSerializableType = "For type '{0}', non-optional field member '{1}' is on the Serializable type that has IsReference as {2}.";

	public const string OnlyDataContractTypesCanHaveExtensionData = "On '{0}' type, only DataContract types can have extension data.";

	public const string PartialTrustISerializableNoPublicConstructor = "Partial trust access required for the constructor on the ISerializable type '{0}'";

	public const string QueryGeneratorPathToMemberNotFound = "The path to member was not found for XPath query generator.";

	public const string ReadNotSupportedOnStream = "Read operation is not supported on the Stream.";

	public const string ReadOnlyClassDeserialization = "Error on deserializing read-only members in the class: {0}";

	public const string ReadOnlyCollectionDeserialization = "Error on deserializing read-only collection: {0}";

	public const string RecursiveCollectionType = "Type '{0}' involves recursive collection.";

	public const string RedefineNotSupported = "XML Schema 'redefine' is not supported.";

	public const string ReferencedBaseTypeDoesNotExist = "Referenced base type does not exist. Data contract name: '{0}' in '{1}' namespace, expected type: '{2}' in '{3}' namespace. Collection can be '{4}' or '{5}'.";

	public const string ReferencedCollectionTypesCannotContainNull = "Referenced collection types cannot contain null.";

	public const string ReferencedTypeDoesNotMatch = "Referenced type '{0}' does not match the expected type '{1}' in '{2}' namespace.";

	public const string ReferencedTypeMatchingMessage = "Reference type matches.";

	public const string ReferencedTypeNotMatchingMessage = "Reference type does not match.";

	public const string ReferencedTypesCannotContainNull = "Referenced types cannot contain null.";

	public const string RequiresClassDataContractToSetIsISerializable = "To set IsISerializable, class data cotnract is required.";

	public const string RootParticleMustBeSequence = "Root particle must be sequence to be imported.";

	public const string RootSequenceMaxOccursMustBe = "On root sequence, maxOccurs must be 1.";

	public const string RootSequenceMustBeRequired = "Root sequence must have an item and minOccurs must be 1.";

	public const string SeekNotSupportedOnStream = "Seek operation is not supported on this Stream.";

	public const string SerializationInfo_ConstructorNotFound = "Constructor that takes SerializationInfo and StreamingContext is not found for '{0}'.";

	public const string SimpleContentNotSupported = "Simple content is not supported.";

	public const string SimpleTypeRestrictionDoesNotSpecifyBase = "This simpleType restriction does not specify the base type.";

	public const string SimpleTypeUnionNotSupported = "simpleType union is not supported.";

	public const string SpecifiedTypeNotFoundInSchema = "Specified type '{0}' in '{1}' namespace is not found in the schemas.";

	public const string SubstitutionGroupOnElementNotSupported = "substitutionGroups on elements are not supported.";

	public const string SurrogatesWithGetOnlyCollectionsNotSupported = "Surrogates with get-only collections are not supported. Type '{1}' contains '{2}' which is of '{0}' type.";

	public const string SurrogatesWithGetOnlyCollectionsNotSupportedSerDeser = "Surrogates with get-only collections are not supported. Found on type '{0}'.";

	public const string TopLevelElementRepresentsDifferentType = "Top-level element represents a different type. Expected '{0}' type in '{1}' namespace.";

	public const string TraceCodeElementIgnored = "Element ignored";

	public const string TraceCodeFactoryTypeNotFound = "Factory type not found";

	public const string TraceCodeObjectWithLargeDepth = "Object with large depth";

	public const string TraceCodeReadObjectBegin = "ReadObject begins";

	public const string TraceCodeReadObjectEnd = "ReadObject ends";

	public const string TraceCodeWriteObjectBegin = "WriteObject begins";

	public const string TraceCodeWriteObjectContentBegin = "WriteObjectContent begins";

	public const string TraceCodeWriteObjectContentEnd = "WriteObjectContent ends";

	public const string TraceCodeWriteObjectEnd = "WriteObject ends";

	public const string TraceCodeXsdExportAnnotationFailed = "XSD export annotation failed";

	public const string TraceCodeXsdExportBegin = "XSD export begins";

	public const string TraceCodeXsdExportDupItems = "XSD export duplicate items";

	public const string TraceCodeXsdExportEnd = "XSD export ends";

	public const string TraceCodeXsdExportError = "XSD export error";

	public const string TraceCodeXsdImportAnnotationFailed = "XSD import annotation failed";

	public const string TraceCodeXsdImportBegin = "XSD import begins";

	public const string TraceCodeXsdImportEnd = "XSD import ends";

	public const string TraceCodeXsdImportError = "XSD import error";

	public const string TypeCannotBeForwardedFrom = "Type '{0}' in assembly '{1}' cannot be forwarded from assembly '{2}'.";

	public const string TypeCannotBeImported = "Type '{0}' in '{1}' namespace cannot be imported: {2}";

	public const string TypeCannotBeImportedHowToFix = "Type cannot be imported: {0}";

	public const string TypeHasNotBeenImported = "Type '{0}' in '{1}' namespace has not been imported.";

	public const string TypeMustBeIXmlSerializable = "Type '{0}' must be IXmlSerializable. Contract type: '{1}', contract name: '{2}' in '{3}' namespace.";

	public const string TypeShouldNotContainAttributes = "Type should not contain attributes. Serialization namespace: '{0}'.";

	public const string UnknownXmlType = "Unknown XML type: '{0}'.";

	public const string WriteBufferOverflow = "Write buffer overflow.";

	public const string WriteNotSupportedOnStream = "Write operation is not supported on this '{0}' Stream.";

	public const string XmlCanonicalizationNotStarted = "XML canonicalization was not started.";

	public const string XmlCanonicalizationStarted = "XML canonicalization started";

	public const string XmlMaxArrayLengthOrMaxItemsQuotaExceeded = "XML max array length or max items quota exceeded. It must be less than {0}.";

	public const string XmlMaxBytesPerReadExceeded = "XML max bytes per read exceeded. It must be less than {0}.";

	public const string XmlMaxDepthExceeded = "XML max depth exceeded. It must be less than {0}.";

	public const string XmlMaxStringContentLengthExceeded = "XML max string content length exceeded. It must be less than {0}.";

	public const string XmlObjectAssignedToIncompatibleInterface = "Object of type '{0}' is assigned to an incompatible interface '{1}'.";

	public const string PlatformNotSupported_SchemaImporter = "The implementation of the function requires System.Runtime.Serialization.SchemaImporter which is not supported on this platform.";

	public const string PlatformNotSupported_IDataContractSurrogate = "The implementation of the function requires System.Runtime.Serialization.IDataContractSurrogate which is not supported on this platform.";

	internal static string GetString(string name, params object[] args)
	{
		return GetString(CultureInfo.InvariantCulture, name, args);
	}

	internal static string GetString(CultureInfo culture, string name, params object[] args)
	{
		return string.Format(culture, name, args);
	}

	internal static string GetString(string name)
	{
		return name;
	}

	internal static string GetString(CultureInfo culture, string name)
	{
		return name;
	}

	internal static string Format(string resourceFormats)
	{
		return resourceFormats;
	}

	internal static string Format(string resourceFormat, object p1)
	{
		return string.Format(CultureInfo.InvariantCulture, resourceFormat, p1);
	}
}
