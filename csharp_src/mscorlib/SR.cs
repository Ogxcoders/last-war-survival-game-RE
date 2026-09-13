using System.Globalization;

internal static class SR
{
	public const string RTL = "RTL_False";

	public const string ContinueButtonText = "Continue";

	public const string DebugMessageTruncated = "{0}...\n<truncated>";

	public const string DebugAssertTitleShort = "Assertion Failed";

	public const string DebugAssertTitle = "Assertion Failed: Cancel=Debug, OK=Continue";

	public const string NotSupported = "This operation is not supported.";

	public const string DebugLaunchFailed = "Cannot launch the debugger.  Make sure that a Microsoft (R) .NET Framework debugger is properly installed.";

	public const string DebugLaunchFailedTitle = "Microsoft .NET Framework Debug Launch Failure";

	public const string ObjectDisposed = "Object {0} has been disposed and can no longer be used.";

	public const string ExceptionOccurred = "An exception occurred writing trace output to log file '{0}'. {1}";

	public const string MustAddListener = "Only TraceListeners can be added to a TraceListenerCollection.";

	public const string ToStringNull = "(null)";

	public const string EnumConverterInvalidValue = "The value '{0}' is not a valid value for the enum '{1}'.";

	public const string ConvertFromException = "{0} cannot convert from {1}.";

	public const string ConvertToException = "'{0}' is unable to convert '{1}' to '{2}'.";

	public const string ConvertInvalidPrimitive = "{0} is not a valid value for {1}.";

	public const string ErrorMissingPropertyAccessors = "Accessor methods for the {0} property are missing.";

	public const string ErrorInvalidPropertyType = "Invalid type for the {0} property.";

	public const string ErrorMissingEventAccessors = "Accessor methods for the {0} event are missing.";

	public const string ErrorInvalidEventHandler = "Invalid event handler for the {0} event.";

	public const string ErrorInvalidEventType = "Invalid type for the {0} event.";

	public const string InvalidMemberName = "Invalid member name.";

	public const string ErrorBadExtenderType = "The {0} extender provider is not compatible with the {1} type.";

	public const string NullableConverterBadCtorArg = "The specified type is not a nullable type.";

	public const string TypeDescriptorExpectedElementType = "Expected types in the collection to be of type {0}.";

	public const string TypeDescriptorSameAssociation = "Cannot create an association when the primary and secondary objects are the same.";

	public const string TypeDescriptorAlreadyAssociated = "The primary and secondary objects are already associated with each other.";

	public const string TypeDescriptorProviderError = "The type description provider {0} has returned null from {1} which is illegal.";

	public const string TypeDescriptorUnsupportedRemoteObject = "The object {0} is being remoted by a proxy that does not support interface discovery.  This type of remoted object is not supported.";

	public const string TypeDescriptorArgsCountMismatch = "The number of elements in the Type and Object arrays must match.";

	public const string ErrorCreateSystemEvents = "Failed to create system events window thread.";

	public const string ErrorCreateTimer = "Cannot create timer.";

	public const string ErrorKillTimer = "Cannot end timer.";

	public const string ErrorSystemEventsNotSupported = "System event notifications are not supported under the current context. Server processes, for example, may not support global system event notifications.";

	public const string ErrorGetTempPath = "Cannot get temporary file name";

	public const string CHECKOUTCanceled = "The checkout was canceled by the user.";

	public const string ErrorInvalidServiceInstance = "The service instance must derive from or implement {0}.";

	public const string ErrorServiceExists = "The service {0} already exists in the service container.";

	public const string Argument_InvalidNumberStyles = "An undefined NumberStyles value is being used.";

	public const string Argument_InvalidHexStyle = "With the AllowHexSpecifier bit set in the enum bit field, the only other valid bits that can be combined into the enum value must be a subset of those in HexNumber.";

	public const string Argument_ByteArrayLengthMustBeAMultipleOf4 = "The Byte[] length must be a multiple of 4.";

	public const string Argument_InvalidCharactersInString = "The string contained an invalid character.";

	public const string Argument_ParsedStringWasInvalid = "The parsed string was invalid.";

	public const string Argument_MustBeBigInt = "The parameter must be a BigInteger.";

	public const string Format_InvalidFormatSpecifier = "Format specifier was invalid.";

	public const string Format_TooLarge = "The value is too large to be represented by this format specifier.";

	public const string ArgumentOutOfRange_MustBeLessThanUInt32MaxValue = "The value must be less than UInt32.MaxValue (2^32).";

	public const string ArgumentOutOfRange_MustBeNonNeg = "The number must be greater than or equal to zero.";

	public const string NotSupported_NumberStyle = "The NumberStyle option is not supported.";

	public const string Overflow_BigIntInfinity = "BigInteger cannot represent infinity.";

	public const string Overflow_NotANumber = "The value is not a number.";

	public const string Overflow_ParseBigInteger = "The value could not be parsed.";

	public const string Overflow_Int32 = "Value was either too large or too small for an Int32.";

	public const string Overflow_Int64 = "Value was either too large or too small for an Int64.";

	public const string Overflow_UInt32 = "Value was either too large or too small for a UInt32.";

	public const string Overflow_UInt64 = "Value was either too large or too small for a UInt64.";

	public const string Overflow_Decimal = "Value was either too large or too small for a Decimal.";

	public const string Argument_FrameworkNameTooShort = "FrameworkName cannot have less than two components or more than three components.";

	public const string Argument_FrameworkNameInvalid = "FrameworkName is invalid.";

	public const string Argument_FrameworkNameInvalidVersion = "FrameworkName version component is invalid.";

	public const string Argument_FrameworkNameMissingVersion = "FrameworkName version component is missing.";

	public const string ArgumentNull_Key = "Key cannot be null.";

	public const string Argument_InvalidValue = "Argument {0} should be larger than {1}.";

	public const string Arg_MultiRank = "Multi dimension array is not supported on this operation.";

	public const string Barrier_ctor_ArgumentOutOfRange = "The participantCount argument must be non-negative and less than or equal to 32767.";

	public const string Barrier_AddParticipants_NonPositive_ArgumentOutOfRange = "The participantCount argument must be a positive value.";

	public const string Barrier_AddParticipants_Overflow_ArgumentOutOfRange = "Adding participantCount participants would result in the number of participants exceeding the maximum number allowed.";

	public const string Barrier_InvalidOperation_CalledFromPHA = "This method may not be called from within the postPhaseAction.";

	public const string Barrier_RemoveParticipants_NonPositive_ArgumentOutOfRange = "The participantCount argument must be a positive value.";

	public const string Barrier_RemoveParticipants_ArgumentOutOfRange = "The participantCount argument must be less than or equal the number of participants.";

	public const string Barrier_RemoveParticipants_InvalidOperation = "The participantCount argument is greater than the number of participants that haven't yet arrived at the barrier in this phase.";

	public const string Barrier_SignalAndWait_ArgumentOutOfRange = "The specified timeout must represent a value between -1 and Int32.MaxValue, inclusive.";

	public const string Barrier_SignalAndWait_InvalidOperation_ZeroTotal = "The barrier has no registered participants.";

	public const string Barrier_SignalAndWait_InvalidOperation_ThreadsExceeded = "The number of threads using the barrier exceeded the total number of registered participants.";

	public const string Barrier_Dispose = "The barrier has been disposed.";

	public const string BarrierPostPhaseException = "The postPhaseAction failed with an exception.";

	public const string UriTypeConverter_ConvertFrom_CannotConvert = "{0} cannot convert from {1}.";

	public const string UriTypeConverter_ConvertTo_CannotConvert = "{0} cannot convert {1} to {2}.";

	public const string ISupportInitializeDescr = "Specifies support for transacted initialization.";

	public const string CantModifyListSortDescriptionCollection = "Once a ListSortDescriptionCollection has been created it can't be modified.";

	public const string Argument_NullComment = "The 'Comment' property of the CodeCommentStatement '{0}' cannot be null.";

	public const string InvalidPrimitiveType = "Invalid Primitive Type: {0}. Consider using CodeObjectCreateExpression.";

	public const string Cannot_Specify_Both_Compiler_Path_And_Version = "Cannot specify both the '{0}' and '{1}' CodeDom provider options to choose a compiler. Please remove one of them.";

	public const string CodeGenOutputWriter = "The output writer for code generation and the writer supplied don't match and cannot be used. This is generally caused by a bad implementation of a CodeGenerator derived class.";

	public const string CodeGenReentrance = "This code generation API cannot be called while the generator is being used to generate something else.";

	public const string InvalidLanguageIdentifier = "The identifier:\"{0}\" on the property:\"{1}\" of type:\"{2}\" is not a valid language-independent identifier name. Check to see if CodeGenerator.IsValidLanguageIndependentIdentifier allows the identifier name.";

	public const string InvalidTypeName = "The type name:\"{0}\" on the property:\"{1}\" of type:\"{2}\" is not a valid language-independent type name.";

	public const string Empty_attribute = "The '{0}' attribute cannot be an empty string.";

	public const string Invalid_nonnegative_integer_attribute = "The '{0}' attribute must be a non-negative integer.";

	public const string CodeDomProvider_NotDefined = "There is no CodeDom provider defined for the language.";

	public const string Language_Names_Cannot_Be_Empty = "You need to specify a non-empty String for a language name in the CodeDom configuration section.";

	public const string Extension_Names_Cannot_Be_Empty_Or_Non_Period_Based = "An extension name in the CodeDom configuration section must be a non-empty string which starts with a period.";

	public const string Unable_To_Locate_Type = "The CodeDom provider type \"{0}\" could not be located.";

	public const string NotSupported_CodeDomAPI = "This CodeDomProvider does not support this method.";

	public const string ArityDoesntMatch = "The total arity specified in '{0}' does not match the number of TypeArguments supplied.  There were '{1}' TypeArguments supplied.";

	public const string PartialTrustErrorTextReplacement = "<The original value of this property potentially contains file system information and has been suppressed.>";

	public const string PartialTrustIllegalProvider = "When used in partial trust, langID must be C#, VB, J#, or JScript, and the language provider must be in the global assembly cache.";

	public const string IllegalAssemblyReference = "Assembly references cannot begin with '-', or contain a '/' or '\\'.";

	public const string NullOrEmpty_Value_in_Property = "The '{0}' property cannot contain null or empty strings.";

	public const string AutoGen_Comment_Line1 = "auto-generated>";

	public const string AutoGen_Comment_Line2 = "This code was generated by a tool.";

	public const string AutoGen_Comment_Line3 = "Runtime Version:";

	public const string AutoGen_Comment_Line4 = "Changes to this file may cause incorrect behavior and will be lost if";

	public const string AutoGen_Comment_Line5 = "the code is regenerated.";

	public const string CantContainNullEntries = "Array '{0}' cannot contain null entries.";

	public const string InvalidPathCharsInChecksum = "The CodeChecksumPragma file name '{0}' contains invalid path characters.";

	public const string InvalidRegion = "The region directive '{0}' contains invalid characters.  RegionText cannot contain any new line characters.";

	public const string Provider_does_not_support_options = "This CodeDomProvider type does not have a constructor that takes providerOptions - \"{0}\"";

	public const string MetaExtenderName = "{0} on {1}";

	public const string InvalidEnumArgument = "The value of argument '{0}' ({1}) is invalid for Enum type '{2}'.";

	public const string InvalidArgument = "'{1}' is not a valid value for '{0}'.";

	public const string InvalidNullArgument = "Null is not a valid value for {0}.";

	public const string LicExceptionTypeOnly = "A valid license cannot be granted for the type {0}. Contact the manufacturer of the component for more information.";

	public const string LicExceptionTypeAndInstance = "An instance of type '{1}' was being created, and a valid license could not be granted for the type '{0}'. Please,  contact the manufacturer of the component for more information.";

	public const string LicMgrContextCannotBeChanged = "The CurrentContext property of the LicenseManager is currently locked and cannot be changed.";

	public const string LicMgrAlreadyLocked = "The CurrentContext property of the LicenseManager is already locked by another user.";

	public const string LicMgrDifferentUser = "The CurrentContext property of the LicenseManager can only be unlocked with the same contextUser.";

	public const string InvalidElementType = "Element type {0} is not supported.";

	public const string InvalidIdentifier = "Identifier '{0}' is not valid.";

	public const string ExecFailedToCreate = "Failed to create file {0}.";

	public const string ExecTimeout = "Timed out waiting for a program to execute. The command being executed was {0}.";

	public const string ExecBadreturn = "An invalid return code was encountered waiting for a program to execute. The command being executed was {0}.";

	public const string ExecCantGetRetCode = "Unable to get the return code for a program being executed. The command that was being executed was '{0}'.";

	public const string ExecCantExec = "Cannot execute a program. The command being executed was {0}.";

	public const string ExecCantRevert = "Cannot execute a program. Impersonation failed.";

	public const string CompilerNotFound = "Compiler executable file {0} cannot be found.";

	public const string DuplicateFileName = "The file name '{0}' was already in the collection.";

	public const string CollectionReadOnly = "Collection is read-only.";

	public const string BitVectorFull = "Bit vector is full.";

	public const string ArrayConverterText = "{0} Array";

	public const string CollectionConverterText = "(Collection)";

	public const string MultilineStringConverterText = "(Text)";

	public const string CultureInfoConverterDefaultCultureString = "(Default)";

	public const string CultureInfoConverterInvalidCulture = "The {0} culture cannot be converted to a CultureInfo object on this computer.";

	public const string InvalidPrimitive = "The text {0} is not a valid {1}.";

	public const string TimerInvalidInterval = "'{0}' is not a valid value for 'Interval'. 'Interval' must be greater than {1}.";

	public const string TraceSwitchLevelTooHigh = "Attempted to set {0} to a value that is too high.  Setting level to TraceLevel.Verbose";

	public const string TraceSwitchLevelTooLow = "Attempted to set {0} to a value that is too low.  Setting level to TraceLevel.Off";

	public const string TraceSwitchInvalidLevel = "The Level must be set to a value in the enumeration TraceLevel.";

	public const string TraceListenerIndentSize = "The IndentSize property must be non-negative.";

	public const string TraceListenerFail = "Fail:";

	public const string TraceAsTraceSource = "Trace";

	public const string InvalidLowBoundArgument = "'{1}' is not a valid value for '{0}'. '{0}' must be greater than {2}.";

	public const string DuplicateComponentName = "Duplicate component name '{0}'.  Component names must be unique and case-insensitive.";

	public const string NotImplemented = "{0}: Not implemented";

	public const string OutOfMemory = "Could not allocate needed memory.";

	public const string EOF = "End of data stream encountered.";

	public const string IOError = "Unknown input/output failure.";

	public const string BadChar = "Unexpected Character: '{0}'.";

	public const string toStringNone = "(none)";

	public const string toStringUnknown = "(unknown)";

	public const string InvalidEnum = "{0} is not a valid {1} value.";

	public const string IndexOutOfRange = "Index {0} is out of range.";

	public const string ErrorPropertyAccessorException = "Property accessor '{0}' on object '{1}' threw the following exception:'{2}'";

	public const string InvalidOperation = "Invalid operation.";

	public const string EmptyStack = "Stack has no items in it.";

	public const string PerformanceCounterDesc = "Represents a Windows performance counter component.";

	public const string PCCategoryName = "Category name of the performance counter object.";

	public const string PCCounterName = "Counter name of the performance counter object.";

	public const string PCInstanceName = "Instance name of the performance counter object.";

	public const string PCMachineName = "Specifies the machine from where to read the performance data.";

	public const string PCInstanceLifetime = "Specifies the lifetime of the instance.";

	public const string PropertyCategoryAction = "Action";

	public const string PropertyCategoryAppearance = "Appearance";

	public const string PropertyCategoryAsynchronous = "Asynchronous";

	public const string PropertyCategoryBehavior = "Behavior";

	public const string PropertyCategoryData = "Data";

	public const string PropertyCategoryDDE = "DDE";

	public const string PropertyCategoryDesign = "Design";

	public const string PropertyCategoryDragDrop = "Drag Drop";

	public const string PropertyCategoryFocus = "Focus";

	public const string PropertyCategoryFont = "Font";

	public const string PropertyCategoryFormat = "Format";

	public const string PropertyCategoryKey = "Key";

	public const string PropertyCategoryList = "List";

	public const string PropertyCategoryLayout = "Layout";

	public const string PropertyCategoryDefault = "Misc";

	public const string PropertyCategoryMouse = "Mouse";

	public const string PropertyCategoryPosition = "Position";

	public const string PropertyCategoryText = "Text";

	public const string PropertyCategoryScale = "Scale";

	public const string PropertyCategoryWindowStyle = "Window Style";

	public const string PropertyCategoryConfig = "Configurations";

	public const string ArgumentNull_ArrayWithNullElements = "The array cannot contain null elements.";

	public const string OnlyAllowedOnce = "This operation is only allowed once per object.";

	public const string BeginIndexNotNegative = "Start index cannot be less than 0 or greater than input length.";

	public const string LengthNotNegative = "Length cannot be less than 0 or exceed input length.";

	public const string UnimplementedState = "Unimplemented state.";

	public const string UnexpectedOpcode = "Unexpected opcode in regular expression generation: {0}.";

	public const string NoResultOnFailed = "Result cannot be called on a failed Match.";

	public const string UnterminatedBracket = "Unterminated [] set.";

	public const string TooManyParens = "Too many )'s.";

	public const string NestedQuantify = "Nested quantifier {0}.";

	public const string QuantifyAfterNothing = "Quantifier {x,y} following nothing.";

	public const string InternalError = "Internal error in ScanRegex.";

	public const string IllegalRange = "Illegal {x,y} with x > y.";

	public const string NotEnoughParens = "Not enough )'s.";

	public const string BadClassInCharRange = "Cannot include class \\{0} in character range.";

	public const string ReversedCharRange = "[x-y] range in reverse order.";

	public const string UndefinedReference = "(?({0}) ) reference to undefined group.";

	public const string MalformedReference = "(?({0}) ) malformed.";

	public const string UnrecognizedGrouping = "Unrecognized grouping construct.";

	public const string UnterminatedComment = "Unterminated (?#...) comment.";

	public const string IllegalEndEscape = "Illegal \\ at end of pattern.";

	public const string MalformedNameRef = "Malformed \\k<...> named back reference.";

	public const string UndefinedBackref = "Reference to undefined group number {0}.";

	public const string UndefinedNameRef = "Reference to undefined group name {0}.";

	public const string TooFewHex = "Insufficient hexadecimal digits.";

	public const string MissingControl = "Missing control character.";

	public const string UnrecognizedControl = "Unrecognized control character.";

	public const string UnrecognizedEscape = "Unrecognized escape sequence \\{0}.";

	public const string IllegalCondition = "Illegal conditional (?(...)) expression.";

	public const string TooManyAlternates = "Too many | in (?()|).";

	public const string MakeException = "parsing \"{0}\" - {1}";

	public const string IncompleteSlashP = "Incomplete \\p{X} character escape.";

	public const string MalformedSlashP = "Malformed \\p{X} character escape.";

	public const string InvalidGroupName = "Invalid group name: Group names must begin with a word character.";

	public const string CapnumNotZero = "Capture number cannot be zero.";

	public const string AlternationCantCapture = "Alternation conditions do not capture and cannot be named.";

	public const string AlternationCantHaveComment = "Alternation conditions cannot be comments.";

	public const string CaptureGroupOutOfRange = "Capture group numbers must be less than or equal to Int32.MaxValue.";

	public const string SubtractionMustBeLast = "A subtraction must be the last element in a character class.";

	public const string UnknownProperty = "Unknown property '{0}'.";

	public const string ReplacementError = "Replacement pattern error.";

	public const string CountTooSmall = "Count cannot be less than -1.";

	public const string EnumNotStarted = "Enumeration has either not started or has already finished.";

	public const string Arg_InvalidArrayType = "Target array type is not compatible with the type of items in the collection.";

	public const string RegexMatchTimeoutException_Occurred = "The RegEx engine has timed out while trying to match a pattern to an input string. This can occur for many reasons, including very large inputs or excessive backtracking caused by nested quantifiers, back-references and other factors.";

	public const string IllegalDefaultRegexMatchTimeoutInAppDomain = "AppDomain data '{0}' contains an invalid value or object for specifying a default matching timeout for System.Text.RegularExpressions.Regex.";

	public const string FileObject_AlreadyOpen = "The file is already open.  Call Close before trying to open the FileObject again.";

	public const string FileObject_Closed = "The FileObject is currently closed.  Try opening it.";

	public const string FileObject_NotWhileWriting = "File information cannot be queried while open for writing.";

	public const string FileObject_FileDoesNotExist = "File information cannot be queried if the file does not exist.";

	public const string FileObject_MustBeClosed = "This operation can only be done when the FileObject is closed.";

	public const string FileObject_MustBeFileName = "You must specify a file name, not a relative or absolute path.";

	public const string FileObject_InvalidInternalState = "FileObject's open mode wasn't set to a valid value.  This FileObject is corrupt.";

	public const string FileObject_PathNotSet = "The path has not been set, or is an empty string.  Please ensure you specify some path.";

	public const string FileObject_Reading = "The file is currently open for reading.  Close the file and reopen it before attempting this.";

	public const string FileObject_Writing = "The file is currently open for writing.  Close the file and reopen it before attempting this.";

	public const string FileObject_InvalidEnumeration = "Enumerator is positioned before the first line or after the last line of the file.";

	public const string FileObject_NoReset = "Reset is not supported on a FileLineEnumerator.";

	public const string DirectoryObject_MustBeDirName = "You must specify a directory name, not a relative or absolute path.";

	public const string DirectoryObjectPathDescr = "The fully qualified, or relative path to the directory you wish to read from. E.g., \"c:\\temp\".";

	public const string FileObjectDetectEncodingDescr = "Determines whether the file will be parsed to see if it has a byte order mark indicating its encoding.  If it does, this will be used rather than the current specified encoding.";

	public const string FileObjectEncodingDescr = "The encoding to use when reading the file. UTF-8 is the default.";

	public const string FileObjectPathDescr = "The fully qualified, or relative path to the file you wish to read from. E.g., \"myfile.txt\".";

	public const string Arg_EnumIllegalVal = "Illegal enum value: {0}.";

	public const string Arg_OutOfRange_NeedNonNegNum = "Non-negative number required.";

	public const string Argument_InvalidPermissionState = "Invalid permission state.";

	public const string Argument_InvalidOidValue = "The OID value was invalid.";

	public const string Argument_WrongType = "Operation on type '{0}' attempted with target of incorrect type.";

	public const string Arg_EmptyOrNullString = "String cannot be empty or null.";

	public const string Arg_EmptyOrNullArray = "Array cannot be empty or null.";

	public const string Argument_InvalidClassAttribute = "The value of \"class\" attribute is invalid.";

	public const string Argument_InvalidNameType = "The value of \"nameType\" is invalid.";

	public const string InvalidOperation_DuplicateItemNotAllowed = "Duplicate items are not allowed in the collection.";

	public const string Cryptography_Asn_MismatchedOidInCollection = "The AsnEncodedData object does not have the same OID for the collection.";

	public const string Cryptography_Cms_Envelope_Empty_Content = "Cannot create CMS enveloped for empty content.";

	public const string Cryptography_Cms_Invalid_Recipient_Info_Type = "The recipient info type {0} is not valid.";

	public const string Cryptography_Cms_Invalid_Subject_Identifier_Type = "The subject identifier type {0} is not valid.";

	public const string Cryptography_Cms_Invalid_Subject_Identifier_Type_Value_Mismatch = "The subject identifier type {0} does not match the value data type {1}.";

	public const string Cryptography_Cms_Key_Agree_Date_Not_Available = "The Date property is not available for none KID key agree recipient.";

	public const string Cryptography_Cms_Key_Agree_Other_Key_Attribute_Not_Available = "The OtherKeyAttribute property is not available for none KID key agree recipient.";

	public const string Cryptography_Cms_MessageNotSigned = "The CMS message is not signed.";

	public const string Cryptography_Cms_MessageNotSignedByNoSignature = "The CMS message is not signed by NoSignature.";

	public const string Cryptography_Cms_MessageNotEncrypted = "The CMS message is not encrypted.";

	public const string Cryptography_Cms_Not_Supported = "The Cryptographic Message Standard (CMS) is not supported on this platform.";

	public const string Cryptography_Cms_RecipientCertificateNotFound = "The recipient certificate is not specified.";

	public const string Cryptography_Cms_Sign_Empty_Content = "Cannot create CMS signature for empty content.";

	public const string Cryptography_Cms_Sign_No_Signature_First_Signer = "CmsSigner has to be the first signer with NoSignature.";

	public const string Cryptography_DpApi_InvalidMemoryLength = "The length of the data should be a multiple of 16 bytes.";

	public const string Cryptography_InvalidHandle = "{0} is an invalid handle.";

	public const string Cryptography_InvalidContextHandle = "The chain context handle is invalid.";

	public const string Cryptography_InvalidStoreHandle = "The store handle is invalid.";

	public const string Cryptography_Oid_InvalidValue = "The OID value is invalid.";

	public const string Cryptography_Pkcs9_ExplicitAddNotAllowed = "The PKCS 9 attribute cannot be explicitly added to the collection.";

	public const string Cryptography_Pkcs9_InvalidOid = "The OID does not represent a valid PKCS 9 attribute.";

	public const string Cryptography_Pkcs9_MultipleSigningTimeNotAllowed = "Cannot add multiple PKCS 9 signing time attributes.";

	public const string Cryptography_Pkcs9_AttributeMismatch = "The parameter should be a PKCS 9 attribute.";

	public const string Cryptography_X509_AddFailed = "Adding certificate with index '{0}' failed.";

	public const string Cryptography_X509_BadEncoding = "Input data cannot be coded as a valid certificate.";

	public const string Cryptography_X509_ExportFailed = "The certificate export operation failed.";

	public const string Cryptography_X509_ExtensionMismatch = "The parameter should be an X509Extension.";

	public const string Cryptography_X509_InvalidFindType = "Invalid find type.";

	public const string Cryptography_X509_InvalidFindValue = "Invalid find value.";

	public const string Cryptography_X509_InvalidEncodingFormat = "Invalid encoding format.";

	public const string Cryptography_X509_InvalidContentType = "Invalid content type.";

	public const string Cryptography_X509_KeyMismatch = "The public key of the certificate does not match the value specified.";

	public const string Cryptography_X509_RemoveFailed = "Removing certificate with index '{0}' failed.";

	public const string Cryptography_X509_StoreNotOpen = "The X509 certificate store has not been opened.";

	public const string Environment_NotInteractive = "The current session is not interactive.";

	public const string NotSupported_InvalidKeyImpl = "Only asymmetric keys that implement ICspAsymmetricAlgorithm are supported.";

	public const string NotSupported_KeyAlgorithm = "The certificate key algorithm is not supported.";

	public const string NotSupported_PlatformRequiresNT = "This operation is only supported on Windows 2000, Windows XP, and higher.";

	public const string NotSupported_UnreadableStream = "Stream does not support reading.";

	public const string Security_InvalidValue = "The {0} value was invalid.";

	public const string Unknown_Error = "Unknown error.";

	public const string security_ServiceNameCollection_EmptyServiceName = "A service name must not be null or empty.";

	public const string security_ExtendedProtectionPolicy_UseDifferentConstructorForNever = "To construct a policy with PolicyEnforcement.Never, the single-parameter constructor must be used.";

	public const string security_ExtendedProtectionPolicy_NoEmptyServiceNameCollection = "The ServiceNameCollection must contain at least one service name.";

	public const string security_ExtendedProtection_NoOSSupport = "This operation requires OS support for extended protection.";

	public const string net_nonClsCompliantException = "A non-CLS Compliant Exception (i.e. an object that does not derive from System.Exception) was thrown.";

	public const string net_illegalConfigWith = "The '{0}' attribute cannot appear when '{1}' is present.";

	public const string net_illegalConfigWithout = "The '{0}' attribute can only appear when '{1}' is present.";

	public const string net_resubmitcanceled = "An error occurred on an automatic resubmission of the request.";

	public const string net_redirect_perm = "WebPermission demand failed for redirect URI.";

	public const string net_resubmitprotofailed = "Cannot handle redirect from HTTP/HTTPS protocols to other dissimilar ones.";

	public const string net_invalidversion = "This protocol version is not supported.";

	public const string net_toolong = "The size of {0} is too long. It cannot be longer than {1} characters.";

	public const string net_connclosed = "The underlying connection was closed: {0}.";

	public const string net_mutualauthfailed = "The requirement for mutual authentication was not met by the remote server.";

	public const string net_invasync = "Cannot block a call on this socket while an earlier asynchronous call is in progress.";

	public const string net_inasync = "An asynchronous call is already in progress. It must be completed or canceled before you can call this method.";

	public const string net_mustbeuri = "The {0} parameter must represent a valid Uri (see inner exception).";

	public const string net_format_shexp = "The shell expression '{0}' could not be parsed because it is formatted incorrectly.";

	public const string net_cannot_load_proxy_helper = "Failed to load the proxy script runtime environment from the Microsoft.JScript assembly.";

	public const string net_io_no_0timeouts = "NetworkStream does not support a 0 millisecond timeout, use a value greater than zero for the timeout instead.";

	public const string net_tooManyRedirections = "Too many automatic redirections were attempted.";

	public const string net_authmodulenotregistered = "The supplied authentication module is not registered.";

	public const string net_authschemenotregistered = "There is no registered module for this authentication scheme.";

	public const string net_proxyschemenotsupported = "The ServicePointManager does not support proxies with the {0} scheme.";

	public const string net_maxsrvpoints = "The maximum number of service points was exceeded.";

	public const string net_notconnected = "The operation is not allowed on non-connected sockets.";

	public const string net_notstream = "The operation is not allowed on non-stream oriented sockets.";

	public const string net_nocontentlengthonget = "Content-Length or Chunked Encoding cannot be set for an operation that does not write data.";

	public const string net_contentlengthmissing = "When performing a write operation with AllowWriteStreamBuffering set to false, you must either set ContentLength to a non-negative number or set SendChunked to true.";

	public const string net_nonhttpproxynotallowed = "The URI scheme for the supplied IWebProxy has the illegal value '{0}'. Only 'http' is supported.";

	public const string net_need_writebuffering = "This request requires buffering data to succeed.";

	public const string net_nodefaultcreds = "Default credentials cannot be supplied for the {0} authentication scheme.";

	public const string net_stopped = "Not listening. You must call the Start() method before calling this method.";

	public const string net_udpconnected = "Cannot send packets to an arbitrary host while connected.";

	public const string net_no_concurrent_io_allowed = "The stream does not support concurrent IO read or write operations.";

	public const string net_needmorethreads = "There were not enough free threads in the ThreadPool to complete the operation.";

	public const string net_MethodNotSupportedException = "This method is not supported by this class.";

	public const string net_ProtocolNotSupportedException = "The '{0}' protocol is not supported by this class.";

	public const string net_SelectModeNotSupportedException = "The '{0}' select mode is not supported by this class.";

	public const string net_InvalidSocketHandle = "The socket handle is not valid.";

	public const string net_InvalidAddressFamily = "The AddressFamily {0} is not valid for the {1} end point, use {2} instead.";

	public const string net_InvalidEndPointAddressFamily = "The supplied EndPoint of AddressFamily {0} is not valid for this Socket, use {1} instead.";

	public const string net_InvalidSocketAddressSize = "The supplied {0} is an invalid size for the {1} end point.";

	public const string net_invalidAddressList = "None of the discovered or specified addresses match the socket address family.";

	public const string net_invalidPingBufferSize = "The buffer length must not exceed 65500 bytes.";

	public const string net_cant_perform_during_shutdown = "This operation cannot be performed while the AppDomain is shutting down.";

	public const string net_cant_create_environment = "Unable to create another web proxy script environment at this time.";

	public const string net_protocol_invalid_family = "'{0}' Client can only accept InterNetwork or InterNetworkV6 addresses.";

	public const string net_protocol_invalid_multicast_family = "Multicast family is not the same as the family of the '{0}' Client.";

	public const string net_empty_osinstalltype = "The Registry value '{0}' was either empty or not a string type.";

	public const string net_unknown_osinstalltype = "Unknown Windows installation type '{0}'.";

	public const string net_cant_determine_osinstalltype = "Can't determine OS installation type: Can't read key '{0}'. Exception message: {1}";

	public const string net_osinstalltype = "Current OS installation type is '{0}'.";

	public const string net_entire_body_not_written = "You must write ContentLength bytes to the request stream before calling [Begin]GetResponse.";

	public const string net_must_provide_request_body = "You must provide a request body if you set ContentLength>0 or SendChunked==true.  Do this by calling [Begin]GetRequestStream before [Begin]GetResponse.";

	public const string net_sockets_zerolist = "The parameter {0} must contain one or more elements.";

	public const string net_sockets_blocking = "The operation is not allowed on a non-blocking Socket.";

	public const string net_sockets_useblocking = "Use the Blocking property to change the status of the Socket.";

	public const string net_sockets_select = "The operation is not allowed on objects of type {0}. Use only objects of type {1}.";

	public const string net_sockets_toolarge_select = "The {0} list contains too many items; a maximum of {1} is allowed.";

	public const string net_sockets_empty_select = "All lists are either null or empty.";

	public const string net_sockets_mustbind = "You must call the Bind method before performing this operation.";

	public const string net_sockets_mustlisten = "You must call the Listen method before performing this operation.";

	public const string net_sockets_mustnotlisten = "You may not perform this operation after calling the Listen method.";

	public const string net_sockets_mustnotbebound = "The socket must not be bound or connected.";

	public const string net_sockets_namedmustnotbebound = "{0}: The socket must not be bound or connected.";

	public const string net_sockets_invalid_socketinformation = "The specified value for the socket information is invalid.";

	public const string net_sockets_invalid_ipaddress_length = "The number of specified IP addresses has to be greater than 0.";

	public const string net_sockets_invalid_optionValue = "The specified value is not a valid '{0}'.";

	public const string net_sockets_invalid_optionValue_all = "The specified value is not valid.";

	public const string net_sockets_invalid_dnsendpoint = "The parameter {0} must not be of type DnsEndPoint.";

	public const string net_sockets_disconnectedConnect = "Once the socket has been disconnected, you can only reconnect again asynchronously, and only to a different EndPoint.  BeginConnect must be called on a thread that won't exit until the operation has been completed.";

	public const string net_sockets_disconnectedAccept = "Once the socket has been disconnected, you can only accept again asynchronously.  BeginAccept must be called on a thread that won't exit until the operation has been completed.";

	public const string net_tcplistener_mustbestopped = "The TcpListener must not be listening before performing this operation.";

	public const string net_sockets_no_duplicate_async = "BeginConnect cannot be called while another asynchronous operation is in progress on the same Socket.";

	public const string net_socketopinprogress = "An asynchronous socket operation is already in progress using this SocketAsyncEventArgs instance.";

	public const string net_buffercounttoosmall = "The Buffer space specified by the Count property is insufficient for the AcceptAsync method.";

	public const string net_multibuffernotsupported = "Multiple buffers cannot be used with this method.";

	public const string net_ambiguousbuffers = "Buffer and BufferList properties cannot both be non-null.";

	public const string net_sockets_ipv6only = "This operation is only valid for IPv6 Sockets.";

	public const string net_perfcounter_initialized_success = "System.Net performance counters initialization completed successful.";

	public const string net_perfcounter_initialized_error = "System.Net performance counters initialization completed with errors. See System.Net trace file for more information.";

	public const string net_perfcounter_nocategory = "Performance counter category '{0}' doesn't exist. No System.Net performance counter values available.";

	public const string net_perfcounter_initialization_started = "System.Net performance counter initialization started.";

	public const string net_perfcounter_cant_queue_workitem = "Can't queue counter initialization logic on a thread pool thread. System.Net performance counters will not be available.";

	public const string net_config_proxy = "Error creating the Web Proxy specified in the 'system.net/defaultProxy' configuration section.";

	public const string net_config_proxy_module_not_public = "The specified proxy module type is not public.";

	public const string net_config_authenticationmodules = "Error creating the modules specified in the 'system.net/authenticationModules' configuration section.";

	public const string net_config_webrequestmodules = "Error creating the modules specified in the 'system.net/webRequestModules' configuration section.";

	public const string net_config_requestcaching = "Error creating the Web Request caching policy specified in the 'system.net/requestCaching' configuration section.";

	public const string net_config_section_permission = "Insufficient permissions for setting the configuration section '{0}'.";

	public const string net_config_element_permission = "Insufficient permissions for setting the configuration element '{0}'.";

	public const string net_config_property_permission = "Insufficient permissions for setting the configuration property '{0}'.";

	public const string net_WebResponseParseError_InvalidHeaderName = "Header name is invalid";

	public const string net_WebResponseParseError_InvalidContentLength = "'Content-Length' header value is invalid";

	public const string net_WebResponseParseError_IncompleteHeaderLine = "Invalid header name";

	public const string net_WebResponseParseError_CrLfError = "CR must be followed by LF";

	public const string net_WebResponseParseError_InvalidChunkFormat = "Response chunk format is invalid";

	public const string net_WebResponseParseError_UnexpectedServerResponse = "Unexpected server response received";

	public const string net_webstatus_Success = "Status success";

	public const string net_webstatus_ReceiveFailure = "An unexpected error occurred on a receive";

	public const string net_webstatus_SendFailure = "An unexpected error occurred on a send";

	public const string net_webstatus_PipelineFailure = "A pipeline failure occurred";

	public const string net_webstatus_RequestCanceled = "The request was canceled";

	public const string net_webstatus_ConnectionClosed = "The connection was closed unexpectedly";

	public const string net_webstatus_TrustFailure = "Could not establish trust relationship for the SSL/TLS secure channel";

	public const string net_webstatus_SecureChannelFailure = "Could not create SSL/TLS secure channel";

	public const string net_webstatus_ServerProtocolViolation = "The server committed a protocol violation";

	public const string net_webstatus_KeepAliveFailure = "A connection that was expected to be kept alive was closed by the server";

	public const string net_webstatus_ProxyNameResolutionFailure = "The proxy name could not be resolved";

	public const string net_webstatus_MessageLengthLimitExceeded = "The message length limit was exceeded";

	public const string net_webstatus_CacheEntryNotFound = "The request cache-only policy does not allow a network request and the response is not found in cache";

	public const string net_webstatus_RequestProhibitedByCachePolicy = "The request could not be satisfied using a cache-only policy";

	public const string net_webstatus_RequestProhibitedByProxy = "The IWebProxy object associated with the request did not allow the request to proceed";

	public const string net_httpstatuscode_NoContent = "No Content";

	public const string net_httpstatuscode_NonAuthoritativeInformation = "Non Authoritative Information";

	public const string net_httpstatuscode_ResetContent = "Reset Content";

	public const string net_httpstatuscode_PartialContent = "Partial Content";

	public const string net_httpstatuscode_MultipleChoices = "Multiple Choices Redirect";

	public const string net_httpstatuscode_Ambiguous = "Ambiguous Redirect";

	public const string net_httpstatuscode_MovedPermanently = "Moved Permanently Redirect";

	public const string net_httpstatuscode_Moved = "Moved Redirect";

	public const string net_httpstatuscode_Found = "Found Redirect";

	public const string net_httpstatuscode_Redirect = "Redirect";

	public const string net_httpstatuscode_SeeOther = "See Other";

	public const string net_httpstatuscode_RedirectMethod = "Redirect Method";

	public const string net_httpstatuscode_NotModified = "Not Modified";

	public const string net_httpstatuscode_UseProxy = "Use Proxy Redirect";

	public const string net_httpstatuscode_TemporaryRedirect = "Temporary Redirect";

	public const string net_httpstatuscode_RedirectKeepVerb = "Redirect Keep Verb";

	public const string net_httpstatuscode_BadRequest = "Bad Request";

	public const string net_httpstatuscode_Unauthorized = "Unauthorized";

	public const string net_httpstatuscode_PaymentRequired = "Payment Required";

	public const string net_httpstatuscode_Forbidden = "Forbidden";

	public const string net_httpstatuscode_NotFound = "Not Found";

	public const string net_httpstatuscode_MethodNotAllowed = "Method Not Allowed";

	public const string net_httpstatuscode_NotAcceptable = "Not Acceptable";

	public const string net_httpstatuscode_ProxyAuthenticationRequired = "Proxy Authentication Required";

	public const string net_httpstatuscode_RequestTimeout = "Request Timeout";

	public const string net_httpstatuscode_Conflict = "Conflict";

	public const string net_httpstatuscode_Gone = "Gone";

	public const string net_httpstatuscode_LengthRequired = "Length Required";

	public const string net_httpstatuscode_InternalServerError = "Internal Server Error";

	public const string net_httpstatuscode_NotImplemented = "Not Implemented";

	public const string net_httpstatuscode_BadGateway = "Bad Gateway";

	public const string net_httpstatuscode_ServiceUnavailable = "Server Unavailable";

	public const string net_httpstatuscode_GatewayTimeout = "Gateway Timeout";

	public const string net_httpstatuscode_HttpVersionNotSupported = "Http Version Not Supported";

	public const string net_emptystringset = "This property cannot be set to an empty string.";

	public const string net_emptystringcall = "The parameter '{0}' cannot be an empty string.";

	public const string net_headers_req = "This collection holds response headers and cannot contain the specified request header.";

	public const string net_headers_rsp = "This collection holds request headers and cannot contain the specified response header.";

	public const string net_headers_toolong = "Header values cannot be longer than {0} characters.";

	public const string net_WebHeaderInvalidNonAsciiChars = "Specified value has invalid non-ASCII characters.";

	public const string net_WebHeaderMissingColon = "Specified value does not have a ':' separator.";

	public const string net_headerrestrict = "The '{0}' header must be modified using the appropriate property or method.";

	public const string net_io_completionportwasbound = "The socket has already been bound to an io completion port.";

	public const string net_io_writefailure = "Unable to write data to the transport connection: {0}.";

	public const string net_io_readfailure = "Unable to read data from the transport connection: {0}.";

	public const string net_io_connectionclosed = "The connection was closed";

	public const string net_io_transportfailure = "Unable to create a transport connection.";

	public const string net_io_internal_bind = "Internal Error: A socket handle could not be bound to a completion port.";

	public const string net_io_invalidnestedcall = "The {0} method cannot be called when another {1} operation is pending.";

	public const string net_io_must_be_rw_stream = "The stream has to be read/write.";

	public const string net_io_header_id = "Found a wrong header field {0} read = {1}, expected = {2}.";

	public const string net_io_out_range = "The byte count must not exceed {0} bytes for this stream type.";

	public const string net_io_encrypt = "The encryption operation failed, see inner exception.";

	public const string net_io_decrypt = "The decryption operation failed, see inner exception.";

	public const string net_io_read = "The read operation failed, see inner exception.";

	public const string net_io_write = "The write operation failed, see inner exception.";

	public const string net_io_eof = "Received an unexpected EOF or 0 bytes from the transport stream.";

	public const string net_io_async_result = "The parameter: {0} is not valid. Use the object returned from corresponding Begin async call.";

	public const string net_tls_version = "The SSL version is not supported.";

	public const string net_perm_target = "Cannot cast target permission type.";

	public const string net_perm_both_regex = "Cannot subset Regex. Only support if both patterns are identical.";

	public const string net_perm_none = "There are no permissions to check.";

	public const string net_perm_attrib_count = "The value for '{0}' must be specified.";

	public const string net_perm_invalid_val = "The parameter value '{0}={1}' is invalid.";

	public const string net_perm_attrib_multi = "The permission '{0}={1}' cannot be added. Add a separate Attribute statement.";

	public const string net_perm_epname = "The argument value '{0}' is invalid for creating a SocketPermission object.";

	public const string net_perm_invalid_val_in_element = "The '{0}' element contains one or more invalid values.";

	public const string net_invalid_ip_addr = "IPv4 address 0.0.0.0 and IPv6 address ::0 are unspecified addresses that cannot be used as a target address.";

	public const string dns_bad_ip_address = "An invalid IP address was specified.";

	public const string net_bad_mac_address = "An invalid physical address was specified.";

	public const string net_ping = "An exception occurred during a Ping request.";

	public const string net_bad_ip_address_prefix = "An invalid IP address prefix was specified.";

	public const string net_max_ip_address_list_length_exceeded = "Too many addresses to sort. The maximum number of addresses allowed are {0}.";

	public const string net_ipv4_not_installed = "IPv4 is not installed.";

	public const string net_ipv6_not_installed = "IPv6 is not installed.";

	public const string net_webclient = "An exception occurred during a WebClient request.";

	public const string net_webclient_ContentType = "The Content-Type header cannot be changed from its default value for this request.";

	public const string net_webclient_Multipart = "The Content-Type header cannot be set to a multipart type for this request.";

	public const string net_webclient_no_concurrent_io_allowed = "WebClient does not support concurrent I/O operations.";

	public const string net_webclient_invalid_baseaddress = "The specified value is not a valid base address.";

	public const string net_container_add_cookie = "An error occurred when adding a cookie to the container.";

	public const string net_cookie_invalid = "Invalid contents for cookie = '{0}'.";

	public const string net_cookie_size = "The value size of the cookie is '{0}'. This exceeds the configured maximum size, which is '{1}'.";

	public const string net_cookie_parse_header = "An error occurred when parsing the Cookie header for Uri '{0}'.";

	public const string net_cookie_attribute = "The '{0}'='{1}' part of the cookie is invalid.";

	public const string net_cookie_format = "Cookie format error.";

	public const string net_cookie_capacity_range = "'{0}' has to be greater than '{1}' and less than '{2}'.";

	public const string net_set_token = "Failed to impersonate a thread doing authentication of a Web Request.";

	public const string net_revert_token = "Failed to revert the thread token after authenticating a Web Request.";

	public const string net_ssl_io_async_context = "Async context creation failed.";

	public const string net_ssl_io_encrypt = "The encryption operation failed, see inner exception.";

	public const string net_ssl_io_decrypt = "The decryption operation failed, see inner exception.";

	public const string net_ssl_io_context_expired = "The security context has expired.";

	public const string net_ssl_io_handshake_start = "The handshake failed. The remote side has dropped the stream.";

	public const string net_ssl_io_handshake = "The handshake failed, see inner exception.";

	public const string net_ssl_io_frame = "The handshake failed due to an unexpected packet format.";

	public const string net_ssl_io_corrupted = "The stream is corrupted due to an invalid SSL version number in the SSL protocol header.";

	public const string net_ssl_io_cert_validation = "The remote certificate is invalid according to the validation procedure.";

	public const string net_ssl_io_invalid_end_call = "{0} can only be called once for each asynchronous operation.";

	public const string net_ssl_io_invalid_begin_call = "{0} cannot be called when another {1} operation is pending.";

	public const string net_ssl_io_no_server_cert = "The server mode SSL must use a certificate with the associated private key.";

	public const string net_auth_bad_client_creds = "The server has rejected the client credentials.";

	public const string net_auth_bad_client_creds_or_target_mismatch = "Either the target name is incorrect or the server has rejected the client credentials.";

	public const string net_auth_context_expectation = "A security requirement was not fulfilled during authentication. Required: {0}, negotiated: {1}.";

	public const string net_auth_context_expectation_remote = "A remote side security requirement was not fulfilled during authentication. Try increasing the ProtectionLevel and/or ImpersonationLevel.";

	public const string net_auth_supported_impl_levels = "The supported values are Identification, Impersonation or Delegation.";

	public const string net_auth_no_anonymous_support = "The TokenImpersonationLevel.Anonymous level is not supported for authentication.";

	public const string net_auth_reauth = "This operation is not allowed on a security context that has already been authenticated.";

	public const string net_auth_noauth = "This operation is only allowed using a successfully authenticated context.";

	public const string net_auth_client_server = "Once authentication is attempted as the client or server, additional authentication attempts must use the same client or server role.";

	public const string net_auth_noencryption = "This authenticated context does not support data encryption.";

	public const string net_auth_SSPI = "Authentication failed, see inner exception.";

	public const string net_auth_failure = "Authentication failed, see inner exception.";

	public const string net_auth_eof = "Authentication failed because the remote party has closed the transport stream.";

	public const string net_auth_alert = "Authentication failed on the remote side (the stream might still be available for additional authentication attempts).";

	public const string net_auth_ignored_reauth = "Re-authentication failed because the remote party continued to encrypt more than {0} bytes before answering re-authentication.";

	public const string net_auth_empty_read = "Protocol error: cannot proceed with SSPI handshake because an empty blob was received.";

	public const string net_auth_must_specify_extended_protection_scheme = "An extended protection policy must specify either a custom channel binding or a custom service name collection.";

	public const string net_frame_size = "Received an invalid authentication frame. The message size is limited to {0} bytes, attempted to read {1} bytes.";

	public const string net_frame_read_io = "Received incomplete authentication message. Remote party has probably closed the connection.";

	public const string net_frame_read_size = "Cannot determine the frame size or a corrupted frame was received.";

	public const string net_frame_max_size = "The payload size is limited to {0}, attempted set it to {1}.";

	public const string net_jscript_load = "The proxy JScript file threw an exception while being initialized: {0}.";

	public const string net_proxy_not_gmt = "The specified value is not a valid GMT time.";

	public const string net_proxy_invalid_dayofweek = "The specified value is not a valid day of the week.";

	public const string net_proxy_invalid_url_format = "The system proxy settings contain an invalid proxy server setting: '{0}'.";

	public const string net_param_not_string = "Argument must be a string instead of {0}.";

	public const string net_value_cannot_be_negative = "The specified value cannot be negative.";

	public const string net_invalid_offset = "Value of offset cannot be negative or greater than the length of the buffer.";

	public const string net_offset_plus_count = "Sum of offset and count cannot be greater than the length of the buffer.";

	public const string net_cannot_be_false = "The specified value cannot be false.";

	public const string net_cache_shadowstream_not_writable = "Shadow stream must be writable.";

	public const string net_cache_validator_fail = "The validation method {0}() returned a failure for this request.";

	public const string net_cache_access_denied = "For this RequestCache object, {0} access is denied.";

	public const string net_cache_validator_result = "The validation method {0}() returned the unexpected status: {1}.";

	public const string net_cache_retrieve_failure = "Cache retrieve failed: {0}.";

	public const string net_cache_not_supported_body = "The cached response is not supported for a request with a content body.";

	public const string net_cache_not_supported_command = "The cached response is not supported for a request with the specified request method.";

	public const string net_cache_not_accept_response = "The cache protocol refused the server response. To allow automatic request retrying, set request.AllowAutoRedirect to true.";

	public const string net_cache_method_failed = "The request (Method = {0}) cannot be served from the cache and will fail because of the effective CachePolicy: {1}.";

	public const string net_cache_key_failed = "The request failed because no cache entry (CacheKey = {0}) was found and the effective CachePolicy is {1}.";

	public const string net_cache_no_stream = "The cache protocol returned a cached response but the cache entry is invalid because it has a null stream. (Cache Key = {0}).";

	public const string net_cache_unsupported_partial_stream = "A partial content stream does not support this operation or some method argument is out of range.";

	public const string net_cache_not_configured = "No cache protocol is available for this request.";

	public const string net_cache_non_seekable_stream_not_supported = "The transport stream instance passed in the RangeStream constructor is not seekable and therefore is not supported.";

	public const string net_invalid_cast = "Invalid cast from {0} to {1}.";

	public const string net_collection_readonly = "The collection is read-only.";

	public const string net_not_ipermission = "Specified value does not contain 'IPermission' as its tag.";

	public const string net_no_classname = "Specified value does not contain a 'class' attribute.";

	public const string net_no_typename = "The value class attribute is not valid.";

	public const string net_servicePointAddressNotSupportedInHostMode = "This property is not supported for protocols that do not use URI.";

	public const string net_Websockets_WebSocketBaseFaulted = "An exception caused the WebSocket to enter the Aborted state. Please see the InnerException, if present, for more details.";

	public const string net_WebSockets_Generic = "An internal WebSocket error occurred. Please see the innerException, if present, for more details.";

	public const string net_WebSockets_NotAWebSocket_Generic = "A WebSocket operation was called on a request or response that is not a WebSocket.";

	public const string net_WebSockets_UnsupportedWebSocketVersion_Generic = "Unsupported WebSocket version.";

	public const string net_WebSockets_HeaderError_Generic = "The WebSocket request or response contained unsupported header(s).";

	public const string net_WebSockets_UnsupportedProtocol_Generic = "The WebSocket request or response operation was called with unsupported protocol(s).";

	public const string net_WebSockets_ClientSecWebSocketProtocolsBlank = "The WebSocket client sent a blank '{0}' header; this is not allowed by the WebSocket protocol specification. The client should omit the header if the client is not negotiating any sub-protocols.";

	public const string net_WebSockets_InvalidState_Generic = "The WebSocket instance cannot be used for communication because it has been transitioned into an invalid state.";

	public const string net_WebSockets_InvalidMessageType_Generic = "The received  message type is invalid after calling {0}. {0} should only be used if no more data is expected from the remote endpoint. Use '{1}' instead to keep being able to receive data but close the output channel.";

	public const string net_WebSockets_ConnectionClosedPrematurely_Generic = "The remote party closed the WebSocket connection without completing the close handshake.";

	public const string net_WebSockets_Scheme = "Only Uris starting with 'ws://' or 'wss://' are supported.";

	public const string net_WebSockets_AlreadyStarted = "The WebSocket has already been started.";

	public const string net_WebSockets_Connect101Expected = "The server returned status code '{0}' when status code '101' was expected.";

	public const string net_WebSockets_InvalidResponseHeader = "The '{0}' header value '{1}' is invalid.";

	public const string net_WebSockets_NotConnected = "The WebSocket is not connected.";

	public const string net_WebSockets_InvalidRegistration = "The WebSocket schemes must be registered with the HttpWebRequest class.";

	public const string net_WebSockets_NoDuplicateProtocol = "Duplicate protocols are not allowed: '{0}'.";

	public const string net_log_exception = "Exception in {0}::{1} - {2}.";

	public const string net_log_sspi_enumerating_security_packages = "Enumerating security packages:";

	public const string net_log_sspi_security_package_not_found = "Security package '{0}' was not found.";

	public const string net_log_sspi_security_context_input_buffer = "{0}(In-Buffer length={1}, Out-Buffer length={2}, returned code={3}).";

	public const string net_log_sspi_security_context_input_buffers = "{0}(In-Buffers count={1}, Out-Buffer length={2}, returned code={3}).";

	public const string net_log_sspi_selected_cipher_suite = "{0}(Protocol={1}, Cipher={2} {3} bit strength, Hash={4} {5} bit strength, Key Exchange={6} {7} bit strength).";

	public const string net_log_remote_certificate = "Remote certificate: {0}.";

	public const string net_log_locating_private_key_for_certificate = "Locating the private key for the certificate: {0}.";

	public const string net_log_cert_is_of_type_2 = "Certificate is of type X509Certificate2 and contains the private key.";

	public const string net_log_found_cert_in_store = "Found the certificate in the {0} store.";

	public const string net_log_did_not_find_cert_in_store = "Cannot find the certificate in either the LocalMachine store or the CurrentUser store.";

	public const string net_log_open_store_failed = "Opening Certificate store {0} failed, exception: {1}.";

	public const string net_log_got_certificate_from_delegate = "Got a certificate from the client delegate.";

	public const string net_log_no_delegate_and_have_no_client_cert = "Client delegate did not provide a certificate; and there are not other user-provided certificates. Need to attempt a session restart.";

	public const string net_log_no_delegate_but_have_client_cert = "Client delegate did not provide a certificate; but there are other user-provided certificates\".";

	public const string net_log_attempting_restart_using_cert = "Attempting to restart the session using the user-provided certificate: {0}.";

	public const string net_log_no_issuers_try_all_certs = "We have user-provided certificates. The server has not specified any issuers, so try all the certificates.";

	public const string net_log_server_issuers_look_for_matching_certs = "We have user-provided certificates. The server has specified {0} issuer(s). Looking for certificates that match any of the issuers.";

	public const string net_log_selected_cert = "Selected certificate: {0}.";

	public const string net_log_n_certs_after_filtering = "Left with {0} client certificates to choose from.";

	public const string net_log_finding_matching_certs = "Trying to find a matching certificate in the certificate store.";

	public const string net_log_using_cached_credential = "Using the cached credential handle.";

	public const string net_log_remote_cert_user_declared_valid = "Remote certificate was verified as valid by the user.";

	public const string net_log_remote_cert_user_declared_invalid = "Remote certificate was verified as invalid by the user.";

	public const string net_log_remote_cert_has_no_errors = "Remote certificate has no errors.";

	public const string net_log_remote_cert_has_errors = "Remote certificate has errors:";

	public const string net_log_remote_cert_not_available = "The remote server did not provide a certificate.";

	public const string net_log_remote_cert_name_mismatch = "Certificate name mismatch.";

	public const string net_log_proxy_autodetect_script_location_parse_error = "WebProxy failed to parse the auto-detected location of a proxy script:\"{0}\" into a Uri.";

	public const string net_log_proxy_autodetect_failed = "WebProxy failed to autodetect a Uri for a proxy script.";

	public const string net_log_proxy_script_execution_error = "WebProxy caught an exception while executing the ScriptReturn script: {0}.";

	public const string net_log_proxy_script_download_compile_error = "WebProxy caught an exception while  downloading/compiling the proxy script: {0}.";

	public const string net_log_proxy_system_setting_update = "ScriptEngine was notified of a potential change in the system's proxy settings and will update WebProxy settings.";

	public const string net_log_proxy_update_due_to_ip_config_change = "ScriptEngine was notified of a change in the IP configuration and will update WebProxy settings.";

	public const string net_log_proxy_called_with_null_parameter = "{0} was called with a null '{1}' parameter.";

	public const string net_log_proxy_called_with_invalid_parameter = "{0} was called with an invalid parameter.";

	public const string net_log_proxy_ras_supported = "RAS supported: {0}";

	public const string net_log_proxy_ras_notsupported_exception = "RAS is not supported. Can't create RasHelper instance.";

	public const string net_log_proxy_winhttp_cant_open_session = "Can't open WinHttp session. Error code: {0}.";

	public const string net_log_proxy_winhttp_getproxy_failed = "Can't retrieve proxy settings for Uri '{0}'. Error code: {1}.";

	public const string net_log_proxy_winhttp_timeout_error = "Can't specify proxy discovery timeout. Error code: {0}.";

	public const string net_log_cache_validation_failed_resubmit = "Resubmitting this request because cache cannot validate the response.";

	public const string net_log_cache_refused_server_response = "Caching protocol has refused the server response. To allow automatic request retrying set request.AllowAutoRedirect=true.";

	public const string net_log_cache_ftp_proxy_doesnt_support_partial = "This FTP request is configured to use a proxy through HTTP protocol. Cache revalidation and partially cached responses are not supported.";

	public const string net_log_cache_ftp_method = "FTP request method={0}.";

	public const string net_log_cache_ftp_supports_bin_only = "Caching is not supported for non-binary FTP request mode.";

	public const string net_log_cache_replacing_entry_with_HTTP_200 = "Replacing cache entry metadata with 'HTTP/1.1 200 OK' status line to satisfy HTTP cache protocol logic.";

	public const string net_log_cache_now_time = "[Now Time (UTC)] = {0}.";

	public const string net_log_cache_max_age_absolute = "[MaxAge] Absolute time expiration check (sensitive to clock skew), cache Expires: {0}.";

	public const string net_log_cache_age1 = "[Age1] Now - LastSynchronized = [Age1] Now - LastSynchronized = {0}, Last Synchronized: {1}.";

	public const string net_log_cache_age1_date_header = "[Age1] NowTime-Date Header = {0}, Date Header: {1}.";

	public const string net_log_cache_age1_last_synchronized = "[Age1] Now - LastSynchronized + AgeHeader = {0}, Last Synchronized: {1}.";

	public const string net_log_cache_age1_last_synchronized_age_header = "[Age1] Now - LastSynchronized + AgeHeader = {0}, Last Synchronized: {1}, Age Header: {2}.";

	public const string net_log_cache_age2 = "[Age2] AgeHeader = {0}.";

	public const string net_log_cache_max_age_cache_s_max_age = "[MaxAge] Cache s_MaxAge = {0}.";

	public const string net_log_cache_max_age_expires_date = "[MaxAge] Cache Expires - Date = {0}, Expires: {1}.";

	public const string net_log_cache_max_age_cache_max_age = "[MaxAge] Cache MaxAge = {0}.";

	public const string net_log_cache_no_max_age_use_10_percent = "[MaxAge] Cannot compute Cache MaxAge, use 10% since LastModified: {0}, LastModified: {1}.";

	public const string net_log_cache_no_max_age_use_default = "[MaxAge] Cannot compute Cache MaxAge, using default RequestCacheValidator.UnspecifiedMaxAge: {0}.";

	public const string net_log_cache_validator_invalid_for_policy = "This validator should not be called for policy : {0}.";

	public const string net_log_cache_response_last_modified = "Response LastModified={0},  ContentLength= {1}.";

	public const string net_log_cache_cache_last_modified = "Cache    LastModified={0},  ContentLength= {1}.";

	public const string net_log_cache_partial_and_non_zero_content_offset = "A Cache Entry is partial and the user request has non zero ContentOffset = {0}. A restart from cache is not supported for partial cache entries.";

	public const string net_log_cache_response_valid_based_on_policy = "Response is valid based on Policy = {0}.";

	public const string net_log_cache_null_response_failure = "Response is null so this Request should fail.";

	public const string net_log_cache_ftp_response_status = "FTP Response Status={0}, {1}.";

	public const string net_log_cache_resp_valid_based_on_retry = "Accept this response as valid based on the retry count = {0}.";

	public const string net_log_cache_no_update_based_on_method = "Cache is not updated based on the request Method = {0}.";

	public const string net_log_cache_removed_existing_invalid_entry = "Existing entry is removed because it was found invalid.";

	public const string net_log_cache_not_updated_based_on_policy = "Cache is not updated based on Policy = {0}.";

	public const string net_log_cache_not_updated_because_no_response = "Cache is not updated because there is no response associated with the request.";

	public const string net_log_cache_removed_existing_based_on_method = "Existing cache entry is removed based on the request Method = {0}.";

	public const string net_log_cache_existing_not_removed_because_unexpected_response_status = "Existing cache entry should but cannot be removed due to unexpected response Status = ({0}) {1}.";

	public const string net_log_cache_removed_existing_based_on_policy = "Existing cache entry is removed based on Policy = {0}.";

	public const string net_log_cache_not_updated_based_on_ftp_response_status = "Cache is not updated based on the FTP response status. Expected = {0}, actual = {1}.";

	public const string net_log_cache_update_not_supported_for_ftp_restart = "Cache update is not supported for restarted FTP responses. Restart offset = {0}.";

	public const string net_log_cache_removed_entry_because_ftp_restart_response_changed = "Existing cache entry is removed since a restarted response was changed on the server, cache LastModified date = {0}, new LastModified date = {1}.";

	public const string net_log_cache_last_synchronized = "The cache entry last synchronized time = {0}.";

	public const string net_log_cache_suppress_update_because_synched_last_minute = "Suppressing cache update since the entry was synchronized within the last minute.";

	public const string net_log_cache_updating_last_synchronized = "Updating cache entry last synchronized time = {0}.";

	public const string net_log_cache_cannot_remove = "{0} Cannot Remove (throw): Key = {1}, Error = {2}.";

	public const string net_log_cache_key_status = "{0}, Key = {1}, -> Status = {2}.";

	public const string net_log_cache_key_remove_failed_status = "{0}, Key = {1}, Remove operation failed -> Status = {2}.";

	public const string net_log_cache_usecount_file = "{0}, UseCount = {1}, File = {2}.";

	public const string net_log_cache_stream = "{0}, stream = {1}.";

	public const string net_log_cache_filename = "{0} -> Filename = {1}, Status = {2}.";

	public const string net_log_cache_lookup_failed = "{0}, Lookup operation failed -> {1}.";

	public const string net_log_cache_exception = "{0}, Exception = {1}.";

	public const string net_log_cache_expected_length = "Expected length (0=none)= {0}.";

	public const string net_log_cache_last_modified = "LastModified    (0=none)= {0}.";

	public const string net_log_cache_expires = "Expires         (0=none)= {0}.";

	public const string net_log_cache_max_stale = "MaxStale (sec)          = {0}.";

	public const string net_log_cache_dumping_metadata = "...Dumping Metadata...";

	public const string net_log_cache_create_failed = "Create operation failed -> {0}.";

	public const string net_log_cache_set_expires = "Set Expires               ={0}.";

	public const string net_log_cache_set_last_modified = "Set LastModified          ={0}.";

	public const string net_log_cache_set_last_synchronized = "Set LastSynchronized      ={0}.";

	public const string net_log_cache_enable_max_stale = "Enable MaxStale (sec) ={0}.";

	public const string net_log_cache_disable_max_stale = "Disable MaxStale (set to 0).";

	public const string net_log_cache_set_new_metadata = "Set new Metadata.";

	public const string net_log_cache_dumping = "...Dumping...";

	public const string net_log_cache_key = "{0}, Key = {1}.";

	public const string net_log_cache_no_commit = "{0}, Nothing was written to the stream, do not commit that cache entry.";

	public const string net_log_cache_error_deleting_filename = "{0}, Error deleting a Filename = {1}.";

	public const string net_log_cache_update_failed = "{0}, Key = {1}, Update operation failed -> {2}.";

	public const string net_log_cache_delete_failed = "{0}, Key = {1}, Delete operation failed -> {2}.";

	public const string net_log_cache_commit_failed = "{0}, Key = {1}, Commit operation failed -> {2}.";

	public const string net_log_cache_committed_as_partial = "{0}, Key = {1}, Committed entry as partial, not cached bytes count = {2}.";

	public const string net_log_cache_max_stale_and_update_status = "{0}, MaxStale = {1}, Update Status = {2}.";

	public const string net_log_cache_failing_request_with_exception = "Failing request with the WebExceptionStatus = {0}.";

	public const string net_log_cache_request_method = "Request Method = {0}.";

	public const string net_log_cache_http_status_parse_failure = "Cannot Parse Cache HTTP Status Line: {0}.";

	public const string net_log_cache_http_status_line = "Entry Status Line = HTTP/{0} {1} {2}.";

	public const string net_log_cache_cache_control = "Cache Cache-Control = {0}.";

	public const string net_log_cache_invalid_http_version = "The cached version is invalid, assuming HTTP 1.0.";

	public const string net_log_cache_no_http_response_header = "This Cache Entry does not carry HTTP response headers.";

	public const string net_log_cache_http_header_parse_error = "Cannot parse HTTP headers in entry metadata, offending string: {0}.";

	public const string net_log_cache_metadata_name_value_parse_error = "Cannot parse all strings in system metadata as \"name:value\", offending string: {0}.";

	public const string net_log_cache_content_range_error = "Invalid format of Response Content-Range:{0}.";

	public const string net_log_cache_cache_control_error = "Invalid CacheControl header = {0}.";

	public const string net_log_cache_unexpected_status = "The cache protocol method {0} has returned unexpected status: {1}.";

	public const string net_log_cache_object_and_exception = "{0} exception: {1}.";

	public const string net_log_cache_revalidation_not_needed = "{0}, No cache entry revalidation is needed.";

	public const string net_log_cache_not_updated_based_on_cache_protocol_status = "{0}, Cache is not updated based on the current cache protocol status = {1}.";

	public const string net_log_cache_closing_cache_stream = "{0}: {1} Closing effective cache stream, type = {2}, cache entry key = {3}.";

	public const string net_log_cache_exception_ignored = "{0}: an exception (ignored) on {1} = {2}.";

	public const string net_log_cache_no_cache_entry = "{0} has requested a cache response but the entry does not exist (Stream.Null).";

	public const string net_log_cache_null_cached_stream = "{0} has requested a cache response but the cached stream is null.";

	public const string net_log_cache_requested_combined_but_null_cached_stream = "{0} has requested a combined response but the cached stream is null.";

	public const string net_log_cache_returned_range_cache = "{0} has returned a range cache stream, Offset = {1}, Length = {2}.";

	public const string net_log_cache_entry_not_found_freshness_undefined = "{0}, Cache Entry not found, freshness result = Undefined.";

	public const string net_log_cache_dumping_cache_context = "...Dumping Cache Context...";

	public const string net_log_cache_result = "{0}, result = {1}.";

	public const string net_log_cache_uri_with_query_has_no_expiration = "Request Uri has a Query, and no explicit expiration time is provided.";

	public const string net_log_cache_uri_with_query_and_cached_resp_from_http_10 = "Request Uri has a Query, and cached response is from HTTP 1.0 server.";

	public const string net_log_cache_valid_as_fresh_or_because_policy = "Valid as fresh or because of Cache Policy = {0}.";

	public const string net_log_cache_accept_based_on_retry_count = "Accept this response base on the retry count = {0}.";

	public const string net_log_cache_date_header_older_than_cache_entry = "Response Date header value is older than that of the cache entry.";

	public const string net_log_cache_server_didnt_satisfy_range = "Server did not satisfy the range: {0}.";

	public const string net_log_cache_304_received_on_unconditional_request = "304 response was received on an unconditional request.";

	public const string net_log_cache_304_received_on_unconditional_request_expected_200_206 = "304 response was received on an unconditional request, but expected response code is 200 or 206.";

	public const string net_log_cache_last_modified_header_older_than_cache_entry = "HTTP 1.0 Response Last-Modified header value is older than that of the cache entry.";

	public const string net_log_cache_freshness_outside_policy_limits = "Response freshness is not within the specified policy limits.";

	public const string net_log_cache_need_to_remove_invalid_cache_entry_304 = "Need to remove an invalid cache entry with status code == 304(NotModified).";

	public const string net_log_cache_resp_status = "Response Status = {0}.";

	public const string net_log_cache_resp_304_or_request_head = "Response==304 or Request was HEAD, updating cache entry.";

	public const string net_log_cache_dont_update_cached_headers = "Do not update Cached Headers.";

	public const string net_log_cache_update_cached_headers = "Update Cached Headers.";

	public const string net_log_cache_partial_resp_not_combined_with_existing_entry = "A partial response is not combined with existing cache entry, Cache Stream Size = {0}, response Range Start = {1}.";

	public const string net_log_cache_request_contains_conditional_header = "User Request contains a conditional header.";

	public const string net_log_cache_not_a_get_head_post = "This was Not a GET, HEAD or POST request.";

	public const string net_log_cache_cannot_update_cache_if_304 = "Cannot update cache if Response status == 304 and a cache entry was not found.";

	public const string net_log_cache_cannot_update_cache_with_head_resp = "Cannot update cache with HEAD response if the cache entry does not exist.";

	public const string net_log_cache_http_resp_is_null = "HttpWebResponse is null.";

	public const string net_log_cache_resp_cache_control_is_no_store = "Response Cache-Control = no-store.";

	public const string net_log_cache_resp_cache_control_is_public = "Response Cache-Control = public.";

	public const string net_log_cache_resp_cache_control_is_private = "Response Cache-Control = private, and Cache is public.";

	public const string net_log_cache_resp_cache_control_is_private_plus_headers = "Response Cache-Control = private+Headers, removing those headers.";

	public const string net_log_cache_resp_older_than_cache = "HttpWebResponse date is older than of the cached one.";

	public const string net_log_cache_revalidation_required = "Response revalidation is always required but neither Last-Modified nor ETag header is set on the response.";

	public const string net_log_cache_needs_revalidation = "Response can be cached although it will always require revalidation.";

	public const string net_log_cache_resp_allows_caching = "Response explicitly allows caching = Cache-Control: {0}.";

	public const string net_log_cache_auth_header_and_no_s_max_age = "Request carries Authorization Header and no s-maxage, proxy-revalidate or public directive found.";

	public const string net_log_cache_post_resp_without_cache_control_or_expires = "POST Response without Cache-Control or Expires headers.";

	public const string net_log_cache_valid_based_on_status_code = "Valid based on Status Code: {0}.";

	public const string net_log_cache_resp_no_cache_control = "Response with no CacheControl and Status Code = {0}.";

	public const string net_log_cache_age = "Cache Age = {0}, Cache MaxAge = {1}.";

	public const string net_log_cache_policy_min_fresh = "Client Policy MinFresh = {0}.";

	public const string net_log_cache_policy_max_age = "Client Policy MaxAge = {0}.";

	public const string net_log_cache_policy_cache_sync_date = "Client Policy CacheSyncDate (UTC) = {0}, Cache LastSynchronizedUtc = {1}.";

	public const string net_log_cache_policy_max_stale = "Client Policy MaxStale = {0}.";

	public const string net_log_cache_control_no_cache = "Cached CacheControl = no-cache.";

	public const string net_log_cache_control_no_cache_removing_some_headers = "Cached CacheControl = no-cache, Removing some headers.";

	public const string net_log_cache_control_must_revalidate = "Cached CacheControl = must-revalidate and Cache is not fresh.";

	public const string net_log_cache_cached_auth_header = "The cached entry has Authorization Header and cache is not fresh.";

	public const string net_log_cache_cached_auth_header_no_control_directive = "The cached entry has Authorization Header and no Cache-Control directive present that would allow to use that entry.";

	public const string net_log_cache_after_validation = "After Response Cache Validation.";

	public const string net_log_cache_resp_status_304 = "Response status == 304 but the cache entry does not exist.";

	public const string net_log_cache_head_resp_has_different_content_length = "A response resulted from a HEAD request has different Content-Length header.";

	public const string net_log_cache_head_resp_has_different_content_md5 = "A response resulted from a HEAD request has different Content-MD5 header.";

	public const string net_log_cache_head_resp_has_different_etag = "A response resulted from a HEAD request has different ETag header.";

	public const string net_log_cache_304_head_resp_has_different_last_modified = "A 304 response resulted from a HEAD request has different Last-Modified header.";

	public const string net_log_cache_existing_entry_has_to_be_discarded = "An existing cache entry has to be discarded.";

	public const string net_log_cache_existing_entry_should_be_discarded = "An existing cache entry should be discarded.";

	public const string net_log_cache_206_resp_non_matching_entry = "A 206 Response has been received and either ETag or Last-Modified header value does not match cache entry.";

	public const string net_log_cache_206_resp_starting_position_not_adjusted = "The starting position for 206 Response is not adjusted to the end of cache entry.";

	public const string net_log_cache_combined_resp_requested = "Creation of a combined response has been requested from the cache protocol.";

	public const string net_log_cache_updating_headers_on_304 = "Updating headers on 304 response.";

	public const string net_log_cache_suppressing_headers_update_on_304 = "Suppressing cache headers update on 304, new headers don't add anything.";

	public const string net_log_cache_status_code_not_304_206 = "A Response Status Code is not 304 or 206.";

	public const string net_log_cache_sxx_resp_cache_only = "A 5XX Response and Cache-Only like policy, serving from cache.";

	public const string net_log_cache_sxx_resp_can_be_replaced = "A 5XX Response that can be replaced by existing cache entry.";

	public const string net_log_cache_vary_header_empty = "Cache entry Vary header is empty.";

	public const string net_log_cache_vary_header_contains_asterisks = "Cache entry Vary header contains '*'.";

	public const string net_log_cache_no_headers_in_metadata = "No request headers are found in cached metadata to test based on the cached response Vary header.";

	public const string net_log_cache_vary_header_mismatched_count = "Vary header: Request and cache header fields count does not match, header name = {0}.";

	public const string net_log_cache_vary_header_mismatched_field = "Vary header: A Cache header field mismatch the request one, header name = {0}, cache field = {1}, request field = {2}.";

	public const string net_log_cache_vary_header_match = "All required Request headers match based on cached Vary response header.";

	public const string net_log_cache_range = "Request Range (not in Cache yet) = Range:{0}.";

	public const string net_log_cache_range_invalid_format = "Invalid format of Request Range:{0}.";

	public const string net_log_cache_range_not_in_cache = "Cannot serve from Cache, Range:{0}.";

	public const string net_log_cache_range_in_cache = "Serving Request Range from cache, Range:{0}.";

	public const string net_log_cache_partial_resp = "Serving Partial Response (206) from cache, Content-Range:{0}.";

	public const string net_log_cache_range_request_range = "Range Request (user specified), Range: {0}.";

	public const string net_log_cache_could_be_partial = "Could be a Partial Cached Response, Size = {0}, Response Content Length = {1}.";

	public const string net_log_cache_condition_if_none_match = "Request Condition = If-None-Match:{0}.";

	public const string net_log_cache_condition_if_modified_since = "Request Condition = If-Modified-Since:{0}.";

	public const string net_log_cache_cannot_construct_conditional_request = "A Conditional Request cannot be constructed.";

	public const string net_log_cache_cannot_construct_conditional_range_request = "A Conditional Range request cannot be constructed.";

	public const string net_log_cache_entry_size_too_big = "Cached Entry Size = {0} is too big, cannot do a range request.";

	public const string net_log_cache_condition_if_range = "Request Condition = If-Range:{0}.";

	public const string net_log_cache_conditional_range_not_implemented_on_http_10 = "A Conditional Range request on Http <= 1.0 is not implemented.";

	public const string net_log_cache_saving_request_headers = "Saving Request Headers, Vary: {0}.";

	public const string net_log_cache_only_byte_range_implemented = "Ranges other than bytes are not implemented.";

	public const string net_log_cache_multiple_complex_range_not_implemented = "Multiple/complexe ranges are not implemented.";

	public const string net_log_digest_hash_algorithm_not_supported = "The hash algorithm is not supported by Digest authentication: {0}.";

	public const string net_log_digest_qop_not_supported = "The Quality of Protection value is not supported by Digest authentication: {0}.";

	public const string net_log_digest_requires_nonce = "A nonce parameter required for Digest authentication was not found or was preceded by an invalid parameter.";

	public const string net_log_auth_invalid_challenge = "The challenge string is not valid for this authentication module: {0}";

	public const string net_log_unknown = "unknown";

	public const string net_log_operation_returned_something = "{0} returned {1}.";

	public const string net_log_buffered_n_bytes = "Buffered {0} bytes.";

	public const string net_log_method_equal = "Method={0}.";

	public const string net_log_releasing_connection = "Releasing FTP connection#{0}.";

	public const string net_log_unexpected_exception = "Unexpected exception in {0}.";

	public const string net_log_server_response_error_code = "Error code {0} was received from server response.";

	public const string net_log_resubmitting_request = "Resubmitting request.";

	public const string net_log_retrieving_localhost_exception = "An unexpected exception while retrieving the local address list: {0}.";

	public const string net_log_resolved_servicepoint_may_not_be_remote_server = "A resolved ServicePoint host could be wrongly considered as a remote server.";

	public const string net_log_closed_idle = "{0}#{1} - Closed as idle.";

	public const string net_log_received_status_line = "Received status line: Version={0}, StatusCode={1}, StatusDescription={2}.";

	public const string net_log_sending_headers = "Sending headers\r\n{{\r\n{0}}}.";

	public const string net_log_received_headers = "Received headers\r\n{{\r\n{0}}}.";

	public const string net_log_shell_expression_pattern_format_warning = "ShellServices.ShellExpression.Parse() was called with a badly formatted 'pattern':{0}.";

	public const string net_log_exception_in_callback = "Exception in callback: {0}.";

	public const string net_log_sending_command = "Sending command [{0}]";

	public const string net_log_received_response = "Received response [{0}]";

	public const string net_log_socket_connected = "Created connection from {0} to {1}.";

	public const string net_log_socket_accepted = "Accepted connection from {0} to {1}.";

	public const string net_log_socket_not_logged_file = "Not logging data from file: {0}.";

	public const string net_log_socket_connect_dnsendpoint = "Connecting to a DnsEndPoint.";

	public const string MailAddressInvalidFormat = "The specified string is not in the form required for an e-mail address.";

	public const string MailSubjectInvalidFormat = "The specified string is not in the form required for a subject.";

	public const string MailBase64InvalidCharacter = "An invalid character was found in the Base-64 stream.";

	public const string MailCollectionIsReadOnly = "The collection is read-only.";

	public const string MailDateInvalidFormat = "The date is in an invalid format.";

	public const string MailHeaderFieldAlreadyExists = "The specified singleton field already exists in the collection and cannot be added.";

	public const string MailHeaderFieldInvalidCharacter = "An invalid character was found in the mail header: '{0}'.";

	public const string MailHeaderFieldMalformedHeader = "The mail header is malformed.";

	public const string MailHeaderFieldMismatchedName = "The header name does not match this property.";

	public const string MailHeaderIndexOutOfBounds = "The index value is outside the bounds of the array.";

	public const string MailHeaderItemAccessorOnlySingleton = "The Item property can only be used with singleton fields.";

	public const string MailHeaderListHasChanged = "The underlying list has been changed and the enumeration is out of date.";

	public const string MailHeaderResetCalledBeforeEOF = "The stream should have been consumed before resetting.";

	public const string MailHeaderTargetArrayTooSmall = "The target array is too small to contain all the headers.";

	public const string MailHeaderInvalidCID = "The ContentID cannot contain a '<' or '>' character.";

	public const string MailHostNotFound = "The SMTP host was not found.";

	public const string MailReaderGetContentStreamAlreadyCalled = "GetContentStream() can only be called once.";

	public const string MailReaderTruncated = "Premature end of stream.";

	public const string MailWriterIsInContent = "This operation cannot be performed while in content.";

	public const string MailServerDoesNotSupportStartTls = "Server does not support secure connections.";

	public const string MailServerResponse = "The server response was: {0}";

	public const string SSPIAuthenticationOrSPNNull = "AuthenticationType and ServicePrincipalName cannot be specified as null for server's SSPI Negotiation module.";

	public const string SSPIPInvokeError = "{0} failed with error {1}.";

	public const string SmtpAlreadyConnected = "Already connected.";

	public const string SmtpAuthenticationFailed = "Authentication failed.";

	public const string SmtpAuthenticationFailedNoCreds = "Authentication failed due to lack of credentials.";

	public const string SmtpDataStreamOpen = "Data stream is still open.";

	public const string SmtpDefaultMimePreamble = "This is a multi-part MIME message.";

	public const string SmtpDefaultSubject = "@@SOAP Application Message";

	public const string SmtpInvalidResponse = "Smtp server returned an invalid response.";

	public const string SmtpNotConnected = "Not connected.";

	public const string SmtpSystemStatus = "System status, or system help reply.";

	public const string SmtpHelpMessage = "Help message.";

	public const string SmtpServiceReady = "Service ready.";

	public const string SmtpServiceClosingTransmissionChannel = "Service closing transmission channel.";

	public const string SmtpOK = "Completed.";

	public const string SmtpUserNotLocalWillForward = "User not local; will forward to specified path.";

	public const string SmtpStartMailInput = "Start mail input; end with <CRLF>.<CRLF>.";

	public const string SmtpServiceNotAvailable = "Service not available, closing transmission channel.";

	public const string SmtpMailboxBusy = "Mailbox unavailable.";

	public const string SmtpLocalErrorInProcessing = "Error in processing.";

	public const string SmtpInsufficientStorage = "Insufficient system storage.";

	public const string SmtpPermissionDenied = "Client does not have permission to Send As this sender.";

	public const string SmtpCommandUnrecognized = "Syntax error, command unrecognized.";

	public const string SmtpSyntaxError = "Syntax error in parameters or arguments.";

	public const string SmtpCommandNotImplemented = "Command not implemented.";

	public const string SmtpBadCommandSequence = "Bad sequence of commands.";

	public const string SmtpCommandParameterNotImplemented = "Command parameter not implemented.";

	public const string SmtpMailboxUnavailable = "Mailbox unavailable.";

	public const string SmtpUserNotLocalTryAlternatePath = "User not local; please try a different path.";

	public const string SmtpExceededStorageAllocation = "Exceeded storage allocation.";

	public const string SmtpMailboxNameNotAllowed = "Mailbox name not allowed.";

	public const string SmtpTransactionFailed = "Transaction failed.";

	public const string SmtpSendMailFailure = "Failure sending mail.";

	public const string SmtpRecipientFailed = "Unable to send to a recipient.";

	public const string SmtpRecipientRequired = "A recipient must be specified.";

	public const string SmtpFromRequired = "A from address must be specified.";

	public const string SmtpAllRecipientsFailed = "Unable to send to all recipients.";

	public const string SmtpClientNotPermitted = "Client does not have permission to submit mail to this server.";

	public const string SmtpMustIssueStartTlsFirst = "The SMTP server requires a secure connection or the client was not authenticated.";

	public const string SmtpNeedAbsolutePickupDirectory = "Only absolute directories are allowed for pickup directory.";

	public const string SmtpGetIisPickupDirectoryFailed = "Cannot get IIS pickup directory.";

	public const string SmtpPickupDirectoryDoesnotSupportSsl = "SSL must not be enabled for pickup-directory delivery methods.";

	public const string SmtpOperationInProgress = "Previous operation is still in progress.";

	public const string SmtpAuthResponseInvalid = "The server returned an invalid response in the authentication handshake.";

	public const string SmtpEhloResponseInvalid = "The server returned an invalid response to the EHLO command.";

	public const string SmtpNonAsciiUserNotSupported = "The client or server is only configured for E-mail addresses with ASCII local-parts: {0}.";

	public const string SmtpInvalidHostName = "The address has an invalid host name: {0}.";

	public const string MimeTransferEncodingNotSupported = "The MIME transfer encoding '{0}' is not supported.";

	public const string SeekNotSupported = "Seeking is not supported on this stream.";

	public const string WriteNotSupported = "Writing is not supported on this stream.";

	public const string InvalidHexDigit = "Invalid hex digit '{0}'.";

	public const string InvalidSSPIContext = "The SSPI context is not valid.";

	public const string InvalidSSPIContextKey = "A null session key was obtained from SSPI.";

	public const string InvalidSSPINegotiationElement = "Invalid SSPI BinaryNegotiationElement.";

	public const string InvalidHeaderName = "An invalid character was found in header name.";

	public const string InvalidHeaderValue = "An invalid character was found in header value.";

	public const string CannotGetEffectiveTimeOfSSPIContext = "Cannot get the effective time of the SSPI context.";

	public const string CannotGetExpiryTimeOfSSPIContext = "Cannot get the expiry time of the SSPI context.";

	public const string ReadNotSupported = "Reading is not supported on this stream.";

	public const string InvalidAsyncResult = "The AsyncResult is not valid.";

	public const string UnspecifiedHost = "The SMTP host was not specified.";

	public const string InvalidPort = "The specified port is invalid. The port must be greater than 0.";

	public const string SmtpInvalidOperationDuringSend = "This operation cannot be performed while a message is being sent.";

	public const string MimePartCantResetStream = "One of the streams has already been used and can't be reset to the origin.";

	public const string MediaTypeInvalid = "The specified media type is invalid.";

	public const string ContentTypeInvalid = "The specified content type is invalid.";

	public const string ContentDispositionInvalid = "The specified content disposition is invalid.";

	public const string AttributeNotSupported = "'{0}' is not a valid configuration attribute for type '{1}'.";

	public const string Cannot_remove_with_null = "Cannot remove with null name.";

	public const string Config_base_elements_only = "Only elements allowed.";

	public const string Config_base_no_child_nodes = "Child nodes not allowed.";

	public const string Config_base_required_attribute_empty = "Required attribute '{0}' cannot be empty.";

	public const string Config_base_required_attribute_missing = "Required attribute '{0}' not found.";

	public const string Config_base_time_overflow = "The time span for the property '{0}' exceeds the maximum that can be stored in the configuration.";

	public const string Config_base_type_must_be_configurationvalidation = "The ConfigurationValidation attribute must be derived from ConfigurationValidation.";

	public const string Config_base_type_must_be_typeconverter = "The ConfigurationPropertyConverter attribute must be derived from TypeConverter.";

	public const string Config_base_unknown_format = "Unknown";

	public const string Config_base_unrecognized_attribute = "Unrecognized attribute '{0}'. Note that attribute names are case-sensitive.";

	public const string Config_base_unrecognized_element = "Unrecognized element.";

	public const string Config_invalid_boolean_attribute = "The property '{0}' must have value 'true' or 'false'.";

	public const string Config_invalid_integer_attribute = "The '{0}' attribute must be set to an integer value.";

	public const string Config_invalid_positive_integer_attribute = "The '{0}' attribute must be set to a positive integer value.";

	public const string Config_invalid_type_attribute = "The '{0}' attribute must be set to a valid Type name.";

	public const string Config_missing_required_attribute = "The '{0}' attribute must be specified on the '{1}' tag.";

	public const string Config_name_value_file_section_file_invalid_root = "The root element must match the name of the section referencing the file, '{0}'";

	public const string Config_provider_must_implement_type = "Provider must implement the class '{0}'.";

	public const string Config_provider_name_null_or_empty = "Provider name cannot be null or empty.";

	public const string Config_provider_not_found = "The provider was not found in the collection.";

	public const string Config_property_name_cannot_be_empty = "Property '{0}' cannot be empty or null.";

	public const string Config_section_cannot_clear_locked_section = "Cannot clear section handlers.  Section '{0}' is locked.";

	public const string Config_section_record_not_found = "SectionRecord not found.";

	public const string Config_source_cannot_contain_file = "The 'File' property cannot be used with the ConfigSource property.";

	public const string Config_system_already_set = "The configuration system can only be set once.  Configuration system is already set";

	public const string Config_unable_to_read_security_policy = "Unable to read security policy.";

	public const string Config_write_xml_returned_null = "WriteXml returned null.";

	public const string Cannot_clear_sections_within_group = "Server cannot clear configuration sections from within section groups.  <clear/> must be a child of <configSections>.";

	public const string Cannot_exit_up_top_directory = "Cannot use a leading .. to exit above the top directory.";

	public const string Could_not_create_listener = "Couldn't create listener '{0}'.";

	public const string TL_InitializeData_NotSpecified = "initializeData needs to be valid for this TraceListener.";

	public const string Could_not_create_type_instance = "Could not create {0}.";

	public const string Could_not_find_type = "Couldn't find type for class {0}.";

	public const string Could_not_get_constructor = "Couldn't find constructor for class {0}.";

	public const string EmptyTypeName_NotAllowed = "switchType needs to be a valid class name. It can't be empty.";

	public const string Incorrect_base_type = "The specified type, '{0}' is not derived from the appropriate base type, '{1}'.";

	public const string Only_specify_one = "'switchValue' and 'switchName' cannot both be specified on source '{0}'.";

	public const string Provider_Already_Initialized = "This provider instance has already been initialized.";

	public const string Reference_listener_cant_have_properties = "A listener with no type name specified references the sharedListeners section and cannot have any attributes other than 'Name'.  Listener: '{0}'.";

	public const string Reference_to_nonexistent_listener = "Listener '{0}' does not exist in the sharedListeners section.";

	public const string SettingsPropertyNotFound = "The settings property '{0}' was not found.";

	public const string SettingsPropertyReadOnly = "The settings property '{0}' is read-only.";

	public const string SettingsPropertyWrongType = "The settings property '{0}' is of a non-compatible type.";

	public const string Type_isnt_tracelistener = "Could not add trace listener {0} because it is not a subclass of TraceListener.";

	public const string Unable_to_convert_type_from_string = "Could not find a type-converter to convert object if type '{0}' from string.";

	public const string Unable_to_convert_type_to_string = "Could not find a type-converter to convert object if type '{0}' to string.";

	public const string Value_must_be_numeric = "Error in trace switch '{0}': The value of a switch must be integral.";

	public const string Could_not_create_from_default_value = "The property '{0}' could not be created from it's default value. Error message: {1}";

	public const string Could_not_create_from_default_value_2 = "The property '{0}' could not be created from it's default value because the default value is of a different type.";

	public const string InvalidDirName = "The directory name {0} is invalid.";

	public const string FSW_IOError = "Error reading the {0} directory.";

	public const string PatternInvalidChar = "The character '{0}' in the pattern provided is not valid.";

	public const string BufferSizeTooLarge = "The specified buffer size is too large. FileSystemWatcher cannot allocate {0} bytes for the internal buffer.";

	public const string FSW_ChangedFilter = "Flag to indicate which change event to monitor.";

	public const string FSW_Enabled = "Flag to indicate whether this component is active or not.";

	public const string FSW_Filter = "The file pattern filter.";

	public const string FSW_IncludeSubdirectories = "Flag to watch subdirectories.";

	public const string FSW_Path = "The path to the directory to monitor.";

	public const string FSW_SynchronizingObject = "The object used to marshal the event handler calls issued as a result of a Directory change.";

	public const string FSW_Changed = "Occurs when a file and/or directory change matches the filter.";

	public const string FSW_Created = "Occurs when a file and/or directory creation matches the filter.";

	public const string FSW_Deleted = "Occurs when a file and/or directory deletion matches the filter.";

	public const string FSW_Renamed = "Occurs when a file and/or directory rename matches the filter.";

	public const string FSW_BufferOverflow = "Too many changes at once in directory:{0}.";

	public const string FileSystemWatcherDesc = "Monitors file system change notifications and raises events when a directory or file changes.";

	public const string NotSet = "[Not Set]";

	public const string TimerAutoReset = "Indicates whether the timer will be restarted when it is enabled.";

	public const string TimerEnabled = "Indicates whether the timer is enabled to fire events at a defined interval.";

	public const string TimerInterval = "The number of milliseconds between timer events.";

	public const string TimerIntervalElapsed = "Occurs when the Interval has elapsed.";

	public const string TimerSynchronizingObject = "The object used to marshal the event handler calls issued when an interval has elapsed.";

	public const string MismatchedCounterTypes = "Mismatched counter types.";

	public const string NoPropertyForAttribute = "Could not find a property for the attribute '{0}'.";

	public const string InvalidAttributeType = "The value of attribute '{0}' could not be converted to the proper type.";

	public const string Generic_ArgCantBeEmptyString = "'{0}' can not be empty string.";

	public const string BadLogName = "Event log names must consist of printable characters and cannot contain \\, *, ?, or spaces";

	public const string InvalidProperty = "Invalid value {1} for property {0}.";

	public const string CantMonitorEventLog = "Cannot monitor EntryWritten events for this EventLog. This might be because the EventLog is on a remote machine which is not a supported scenario.";

	public const string InitTwice = "Cannot initialize the same object twice.";

	public const string InvalidParameter = "Invalid value '{1}' for parameter '{0}'.";

	public const string MissingParameter = "Must specify value for {0}.";

	public const string ParameterTooLong = "The size of {0} is too big. It cannot be longer than {1} characters.";

	public const string LocalSourceAlreadyExists = "Source {0} already exists on the local computer.";

	public const string SourceAlreadyExists = "Source {0} already exists on the computer '{1}'.";

	public const string LocalLogAlreadyExistsAsSource = "Log {0} has already been registered as a source on the local computer.";

	public const string LogAlreadyExistsAsSource = "Log {0} has already been registered as a source on the computer '{1}'.";

	public const string DuplicateLogName = "Only the first eight characters of a custom log name are significant, and there is already another log on the system using the first eight characters of the name given. Name given: '{0}', name of existing log: '{1}'.";

	public const string RegKeyMissing = "Cannot open registry key {0}\\{1}\\{2} on computer '{3}'.";

	public const string LocalRegKeyMissing = "Cannot open registry key {0}\\{1}\\{2}.";

	public const string RegKeyMissingShort = "Cannot open registry key {0} on computer {1}.";

	public const string InvalidParameterFormat = "Invalid format for argument {0}.";

	public const string NoLogName = "Log to delete was not specified.";

	public const string RegKeyNoAccess = "Cannot open registry key {0} on computer {1}. You might not have access.";

	public const string MissingLog = "Cannot find Log {0} on computer '{1}'.";

	public const string SourceNotRegistered = "The source '{0}' is not registered on machine '{1}', or you do not have write access to the {2} registry key.";

	public const string LocalSourceNotRegistered = "Source {0} is not registered on the local computer.";

	public const string CantRetrieveEntries = "Cannot retrieve all entries.";

	public const string IndexOutOfBounds = "Index {0} is out of bounds.";

	public const string CantReadLogEntryAt = "Cannot read log entry number {0}.  The event log may be corrupt.";

	public const string MissingLogProperty = "Log property value has not been specified.";

	public const string CantOpenLog = "Cannot open log {0} on machine {1}. Windows has not provided an error code.";

	public const string NeedSourceToOpen = "Source property was not set before opening the event log in write mode.";

	public const string NeedSourceToWrite = "Source property was not set before writing to the event log.";

	public const string CantOpenLogAccess = "Cannot open log for source '{0}'. You may not have write access.";

	public const string LogEntryTooLong = "Log entry string is too long. A string written to the event log cannot exceed 32766 characters.";

	public const string TooManyReplacementStrings = "The maximum allowed number of replacement strings is 255.";

	public const string LogSourceMismatch = "The source '{0}' is not registered in log '{1}'. (It is registered in log '{2}'.) \" The Source and Log properties must be matched, or you may set Log to the empty string, and it will automatically be matched to the Source property.";

	public const string NoAccountInfo = "Cannot obtain account information.";

	public const string NoCurrentEntry = "No current EventLog entry available, cursor is located before the first or after the last element of the enumeration.";

	public const string MessageNotFormatted = "The description for Event ID '{0}' in Source '{1}' cannot be found.  The local computer may not have the necessary registry information or message DLL files to display the message, or you may not have permission to access them.  The following information is part of the event:";

	public const string EventID = "Invalid eventID value '{0}'. It must be in the range between '{1}' and '{2}'.";

	public const string LogDoesNotExists = "The event log '{0}' on computer '{1}' does not exist.";

	public const string InvalidCustomerLogName = "The log name: '{0}' is invalid for customer log creation.";

	public const string CannotDeleteEqualSource = "The event log source '{0}' cannot be deleted, because it's equal to the log name.";

	public const string RentionDaysOutOfRange = "'retentionDays' must be between 1 and 365 days.";

	public const string MaximumKilobytesOutOfRange = "MaximumKilobytes must be between 64 KB and 4 GB, and must be in 64K increments.";

	public const string SomeLogsInaccessible = "The source was not found, but some or all event logs could not be searched.  Inaccessible logs: {0}.";

	public const string SomeLogsInaccessibleToCreate = "The source was not found, but some or all event logs could not be searched.  To create the source, you need permission to read all event logs to make sure that the new source name is unique.  Inaccessible logs: {0}.";

	public const string BadConfigSwitchValue = "The config value for Switch '{0}' was invalid.";

	public const string ConfigSectionsUnique = "The '{0}' section can only appear once per config file.";

	public const string ConfigSectionsUniquePerSection = "The '{0}' tag can only appear once per section.";

	public const string SourceListenerDoesntExist = "The listener '{0}' added to source '{1}' must have a listener with the same name defined in the main Trace listeners section.";

	public const string SourceSwitchDoesntExist = "The source '{0}' must have a switch with the same name defined in the Switches section.";

	public const string CategoryHelpCorrupt = "Cannot load Category Help data because an invalid index '{0}' was read from the registry.";

	public const string CounterNameCorrupt = "Cannot load Counter Name data because an invalid index '{0}' was read from the registry.";

	public const string CounterDataCorrupt = "Cannot load Performance Counter data because an unexpected registry key value type was read from '{0}'.";

	public const string ReadOnlyCounter = "Cannot update Performance Counter, this object has been initialized as ReadOnly.";

	public const string ReadOnlyRemoveInstance = "Cannot remove Performance Counter Instance, this object as been initialized as ReadOnly.";

	public const string NotCustomCounter = "The requested Performance Counter is not a custom counter, it has to be initialized as ReadOnly.";

	public const string CategoryNameMissing = "Failed to initialize because CategoryName is missing.";

	public const string CounterNameMissing = "Failed to initialize because CounterName is missing.";

	public const string InstanceNameProhibited = "Counter is single instance, instance name '{0}' is not valid for this counter category.";

	public const string InstanceNameRequired = "Counter is not single instance, an instance name needs to be specified.";

	public const string MissingInstance = "Instance {0} does not exist in category {1}.";

	public const string PerformanceCategoryExists = "Cannot create Performance Category '{0}' because it already exists.";

	public const string InvalidCounterName = "Invalid empty or null string for counter name.";

	public const string DuplicateCounterName = "Cannot create Performance Category with counter name {0} because the name is a duplicate.";

	public const string CantChangeCategoryRegistration = "Cannot create or delete the Performance Category '{0}' because access is denied.";

	public const string CantDeleteCategory = "Cannot delete Performance Category because this category is not registered or is a system category.";

	public const string MissingCategory = "Category does not exist.";

	public const string MissingCategoryDetail = "Category {0} does not exist.";

	public const string CantReadCategory = "Cannot read Category {0}.";

	public const string MissingCounter = "Counter {0} does not exist.";

	public const string CategoryNameNotSet = "Category name property has not been set.";

	public const string CounterExists = "Could not locate Performance Counter with specified category name '{0}', counter name '{1}'.";

	public const string CantReadCategoryIndex = "Could not Read Category Index: {0}.";

	public const string CantReadCounter = "Counter '{0}' does not exist in the specified Category.";

	public const string CantReadInstance = "Instance '{0}' does not exist in the specified Category.";

	public const string RemoteWriting = "Cannot write to a Performance Counter in a remote machine.";

	public const string CounterLayout = "The Counter layout for the Category specified is invalid, a counter of the type:  AverageCount64, AverageTimer32, CounterMultiTimer, CounterMultiTimerInverse, CounterMultiTimer100Ns, CounterMultiTimer100NsInverse, RawFraction, or SampleFraction has to be immediately followed by any of the base counter types: AverageBase, CounterMultiBase, RawBase or SampleBase.";

	public const string PossibleDeadlock = "The operation couldn't be completed, potential internal deadlock.";

	public const string SharedMemoryGhosted = "Cannot access shared memory, AppDomain has been unloaded.";

	public const string HelpNotAvailable = "Help not available.";

	public const string PerfInvalidHelp = "Invalid help string. Its length must be in the range between '{0}' and '{1}'.";

	public const string PerfInvalidCounterName = "Invalid counter name. Its length must be in the range between '{0}' and '{1}'. Double quotes, control characters and leading or trailing spaces are not allowed.";

	public const string PerfInvalidCategoryName = "Invalid category name. Its length must be in the range between '{0}' and '{1}'. Double quotes, control characters and leading or trailing spaces are not allowed.";

	public const string MustAddCounterCreationData = "Only objects of type CounterCreationData can be added to a CounterCreationDataCollection.";

	public const string RemoteCounterAdmin = "Creating or Deleting Performance Counter Categories on remote machines is not supported.";

	public const string NoInstanceInformation = "The {0} category doesn't provide any instance information, no accurate data can be returned.";

	public const string PerfCounterPdhError = "There was an error calculating the PerformanceCounter value (0x{0}).";

	public const string MultiInstanceOnly = "Category '{0}' is marked as multi-instance.  Performance counters in this category can only be created with instance names.";

	public const string SingleInstanceOnly = "Category '{0}' is marked as single-instance.  Performance counters in this category can only be created without instance names.";

	public const string InstanceNameTooLong = "Instance names used for writing to custom counters must be 127 characters or less.";

	public const string CategoryNameTooLong = "Category names must be 1024 characters or less.";

	public const string InstanceLifetimeProcessonReadOnly = "InstanceLifetime is unused by ReadOnly counters.";

	public const string InstanceLifetimeProcessforSingleInstance = "Single instance categories are only valid with the Global lifetime.";

	public const string InstanceAlreadyExists = "Instance '{0}' already exists with a lifetime of Process.  It cannot be recreated or reused until it has been removed or until the process using it has exited.";

	public const string CantSetLifetimeAfterInitialized = "The InstanceLifetime cannot be set after the instance has been initialized.  You must use the default constructor and set the CategoryName, InstanceName, CounterName, InstanceLifetime and ReadOnly properties manually before setting the RawValue.";

	public const string ProcessLifetimeNotValidInGlobal = "PerformanceCounterInstanceLifetime.Process is not valid in the global shared memory.  If your performance counter category was created with an older version of the Framework, it uses the global shared memory.  Either use PerformanceCounterInstanceLifetime.Global, or if applications running on older versions of the Framework do not need to write to your category, delete and recreate it.";

	public const string CantConvertProcessToGlobal = "An instance with a lifetime of Process can only be accessed from a PerformanceCounter with the InstanceLifetime set to PerformanceCounterInstanceLifetime.Process.";

	public const string CantConvertGlobalToProcess = "An instance with a lifetime of Global can only be accessed from a PerformanceCounter with the InstanceLifetime set to PerformanceCounterInstanceLifetime.Global.";

	public const string PCNotSupportedUnderAppContainer = "Writeable performance counters are not allowed when running in AppContainer.";

	public const string PriorityClassNotSupported = "The AboveNormal and BelowNormal priority classes are not available on this platform.";

	public const string WinNTRequired = "Feature requires Windows NT.";

	public const string Win2kRequired = "Feature requires Windows 2000.";

	public const string NoAssociatedProcess = "No process is associated with this object.";

	public const string ProcessIdRequired = "Feature requires a process identifier.";

	public const string NotSupportedRemote = "Feature is not supported for remote machines.";

	public const string NoProcessInfo = "Process has exited, so the requested information is not available.";

	public const string WaitTillExit = "Process must exit before requested information can be determined.";

	public const string NoProcessHandle = "Process was not started by this object, so requested information cannot be determined.";

	public const string MissingProccess = "Process with an Id of {0} is not running.";

	public const string BadMinWorkset = "Minimum working set size is invalid. It must be less than or equal to the maximum working set size.";

	public const string BadMaxWorkset = "Maximum working set size is invalid. It must be greater than or equal to the minimum working set size.";

	public const string WinNTRequiredForRemote = "Operating system does not support accessing processes on remote computers. This feature requires Windows NT or later.";

	public const string ProcessHasExited = "Cannot process request because the process ({0}) has exited.";

	public const string ProcessHasExitedNoId = "Cannot process request because the process has exited.";

	public const string ThreadExited = "The request cannot be processed because the thread ({0}) has exited.";

	public const string Win2000Required = "Feature requires Windows 2000 or later.";

	public const string ProcessNotFound = "Thread {0} found, but no process {1} found.";

	public const string CantGetProcessId = "Cannot retrieve process identifier from the process handle.";

	public const string ProcessDisabled = "Process performance counter is disabled, so the requested operation cannot be performed.";

	public const string WaitReasonUnavailable = "WaitReason is only available if the ThreadState is Wait.";

	public const string NotSupportedRemoteThread = "Feature is not supported for threads on remote computers.";

	public const string UseShellExecuteRequiresSTA = "Current thread is not in Single Thread Apartment (STA) mode. Starting a process with UseShellExecute set to True requires the current thread be in STA mode.  Ensure that your Main function has STAThreadAttribute marked.";

	public const string CantRedirectStreams = "The Process object must have the UseShellExecute property set to false in order to redirect IO streams.";

	public const string CantUseEnvVars = "The Process object must have the UseShellExecute property set to false in order to use environment variables.";

	public const string CantStartAsUser = "The Process object must have the UseShellExecute property set to false in order to start a process as a user.";

	public const string CouldntConnectToRemoteMachine = "Couldn't connect to remote machine.";

	public const string CouldntGetProcessInfos = "Couldn't get process information from performance counter.";

	public const string InputIdleUnkownError = "WaitForInputIdle failed.  This could be because the process does not have a graphical interface.";

	public const string FileNameMissing = "Cannot start process because a file name has not been provided.";

	public const string EnvironmentBlock = "The environment block provided doesn't have the correct format.";

	public const string EnumProcessModuleFailed = "Unable to enumerate the process modules.";

	public const string EnumProcessModuleFailedDueToWow = "A 32 bit processes cannot access modules of a 64 bit process.";

	public const string PendingAsyncOperation = "An async read operation has already been started on the stream.";

	public const string NoAsyncOperation = "No async read operation is in progress on the stream.";

	public const string InvalidApplication = "The specified executable is not a valid application for this OS platform.";

	public const string StandardOutputEncodingNotAllowed = "StandardOutputEncoding is only supported when standard output is redirected.";

	public const string StandardErrorEncodingNotAllowed = "StandardErrorEncoding is only supported when standard error is redirected.";

	public const string CountersOOM = "Custom counters file view is out of memory.";

	public const string MappingCorrupted = "Cannot continue the current operation, the performance counters memory mapping has been corrupted.";

	public const string SetSecurityDescriptorFailed = "Cannot initialize security descriptor initialized.";

	public const string CantCreateFileMapping = "Cannot create file mapping.";

	public const string CantMapFileView = "Cannot map view of file.";

	public const string CantGetMappingSize = "Cannot calculate the size of the file view.";

	public const string CantGetStandardOut = "StandardOut has not been redirected or the process hasn't started yet.";

	public const string CantGetStandardIn = "StandardIn has not been redirected.";

	public const string CantGetStandardError = "StandardError has not been redirected.";

	public const string CantMixSyncAsyncOperation = "Cannot mix synchronous and asynchronous operation on process stream.";

	public const string NoFileMappingSize = "Cannot retrieve file mapping size while initializing configuration settings.";

	public const string EnvironmentBlockTooLong = "The environment block used to start a process cannot be longer than 65535 bytes.  Your environment block is {0} bytes long.  Remove some environment variables and try again.";

	public const string Arg_SecurityException = "The port name cannot start with '\\'.";

	public const string ArgumentNull_Array = "Array cannot be null.";

	public const string ArgumentNull_Buffer = "Buffer cannot be null.";

	public const string IO_UnknownError = "Unknown Error '{0}'.";

	public const string NotSupported_UnwritableStream = "Stream does not support writing.";

	public const string ObjectDisposed_WriterClosed = "Can not write to a closed TextWriter.";

	public const string NotSupportedOS = "GetPortNames is not supported on Win9x platforms.";

	public const string BaudRate = "The baud rate to use on this serial port.";

	public const string DataBits = "The number of data bits per transmitted/received byte.";

	public const string DiscardNull = "Whether to discard null bytes received on the port before adding to serial buffer.";

	public const string DtrEnable = "Whether to enable the Data Terminal Ready (DTR) line during communications.";

	public const string EncodingMonitoringDescription = "The encoding to use when reading and writing strings.";

	public const string Handshake = "The handshaking protocol for flow control in data exchange, which can be None.";

	public const string NewLine = "The string used by ReadLine and WriteLine to denote a new line.";

	public const string Parity = "The scheme for parity checking each received byte and marking each transmitted byte.";

	public const string ParityReplace = "Byte with which to replace bytes received with parity errors.";

	public const string PortName = "The name of the communications port to open.";

	public const string ReadBufferSize = "The size of the read buffer in bytes.  This is the maximum number of read bytes which can be buffered.";

	public const string ReadTimeout = "The read timeout in Milliseconds.";

	public const string ReceivedBytesThreshold = "Number of bytes required to be available before the Read event is fired.";

	public const string RtsEnable = "Whether to enable the Request To Send (RTS) line during communications.";

	public const string SerialPortDesc = "Represents a serial port resource.";

	public const string StopBits = "The number of stop bits per transmitted/received byte.";

	public const string WriteBufferSize = "The size of the write buffer in bytes.  This is the maximum number of bytes which can be queued for write.";

	public const string WriteTimeout = "The write timeout in milliseconds.";

	public const string SerialErrorReceived = "Raised each time when an error is received from the SerialPort.";

	public const string SerialPinChanged = "Raised each time when pin is changed on the SerialPort.";

	public const string SerialDataReceived = "Raised each time when data is received from the SerialPort.";

	public const string CounterType = "The type of this counter.";

	public const string CounterName = "The name of this counter.";

	public const string CounterHelp = "Help information for this counter.";

	public const string EventLogDesc = "Provides interaction with Windows event logs.";

	public const string ErrorDataReceived = "User event handler to call for async IO with StandardError stream.";

	public const string LogEntries = "The contents of the log.";

	public const string LogLog = "Gets or sets the name of the log to read from and write to.";

	public const string LogMachineName = "The name of the machine on which to read or write events.";

	public const string LogMonitoring = "Indicates if the component monitors the event log for changes.";

	public const string LogSynchronizingObject = "The object used to marshal the event handler calls issued as a result of an EventLog change.";

	public const string LogSource = "The application name (source name) to use when writing to the event log.";

	public const string LogEntryWritten = "Raised each time any application writes an entry to the event log.";

	public const string LogEntryMachineName = "The machine on which this event log resides.";

	public const string LogEntryData = "The binary data associated with this entry in the event log.";

	public const string LogEntryIndex = "The sequence of this entry in the event log.";

	public const string LogEntryCategory = "The category for this message.";

	public const string LogEntryCategoryNumber = "An application-specific category number assigned to this entry.";

	public const string LogEntryEventID = "The number identifying the message for this source.";

	public const string LogEntryEntryType = "The type of entry - Information, Warning, etc.";

	public const string LogEntryMessage = "The text of the message for this entry";

	public const string LogEntrySource = "The name of the application that wrote this entry.";

	public const string LogEntryReplacementStrings = "The application-supplied strings used in the message.";

	public const string LogEntryResourceId = "The full number identifying the message in the event message dll.";

	public const string LogEntryTimeGenerated = "The time at which the application logged this entry.";

	public const string LogEntryTimeWritten = "The time at which the system logged this entry to the event log.";

	public const string LogEntryUserName = "The username of the account associated with this entry by the writing application.";

	public const string OutputDataReceived = "User event handler to call for async IO with StandardOutput stream.";

	public const string PC_CounterHelp = "The description message for this counter.";

	public const string PC_CounterType = "The counter type indicates how to interpret the value of the counter, for example an actual count or a rate of change.";

	public const string PC_ReadOnly = "Indicates if the counter is read only.  Remote counters and counters not created using this component are read-only.";

	public const string PC_RawValue = "Directly accesses the raw value of this counter.  The counter must have been created using this component.";

	public const string ProcessAssociated = "Indicates if the process component is associated with a real process.";

	public const string ProcessDesc = "Provides access to local and remote processes, enabling starting and stopping of local processes.";

	public const string ProcessExitCode = "The value returned from the associated process when it terminated.";

	public const string ProcessTerminated = "Indicates if the associated process has been terminated.";

	public const string ProcessExitTime = "The time that the associated process exited.";

	public const string ProcessHandle = "Returns the native handle for this process.   The handle is only available if the process was started using this component.";

	public const string ProcessHandleCount = "The number of native handles associated with this process.";

	public const string ProcessId = "The unique identifier for the process.";

	public const string ProcessMachineName = "The name of the machine the running the process.";

	public const string ProcessMainModule = "The main module for the associated process.";

	public const string ProcessModules = "The modules that have been loaded by the associated process.";

	public const string ProcessSynchronizingObject = "The object used to marshal the event handler calls issued as a result of a Process exit.";

	public const string ProcessSessionId = "The identifier for the session of the process.";

	public const string ProcessThreads = "The threads running in the associated process.";

	public const string ProcessEnableRaisingEvents = "Whether the process component should watch for the associated process to exit, and raise the Exited event.";

	public const string ProcessExited = "If the WatchForExit property is set to true, then this event is raised when the associated process exits.";

	public const string ProcessFileName = "The name of the application, document or URL to start.";

	public const string ProcessWorkingDirectory = "The initial working directory for the process.";

	public const string ProcessBasePriority = "The base priority computed based on the priority class that all threads run relative to.";

	public const string ProcessMainWindowHandle = "The handle of the main window for the process.";

	public const string ProcessMainWindowTitle = "The caption of the main window for the process.";

	public const string ProcessMaxWorkingSet = "The maximum amount of physical memory the process has required since it was started.";

	public const string ProcessMinWorkingSet = "The minimum amount of physical memory the process has required since it was started.";

	public const string ProcessNonpagedSystemMemorySize = "The number of bytes of non pageable system  memory the process is using.";

	public const string ProcessPagedMemorySize = "The current amount of memory that can be paged to disk that the process is using.";

	public const string ProcessPagedSystemMemorySize = "The number of bytes of pageable system memory the process is using.";

	public const string ProcessPeakPagedMemorySize = "The maximum amount of memory that can be paged to disk that the process has used since it was started.";

	public const string ProcessPeakWorkingSet = "The maximum amount of physical memory the process has used since it was started.";

	public const string ProcessPeakVirtualMemorySize = "The maximum amount of virtual memory the process has allocated since it was started.";

	public const string ProcessPriorityBoostEnabled = "Whether this process would like a priority boost when the user interacts with it.";

	public const string ProcessPriorityClass = "The priority that the threads in the process run relative to.";

	public const string ProcessPrivateMemorySize = "The current amount of memory that the process has allocated that cannot be shared with other processes.";

	public const string ProcessPrivilegedProcessorTime = "The amount of CPU time the process spent inside the operating system core.";

	public const string ProcessProcessName = "The name of the process.";

	public const string ProcessProcessorAffinity = "A bit mask which represents the processors that the threads within the process are allowed to run on.";

	public const string ProcessResponding = "Whether this process is currently responding.";

	public const string ProcessStandardError = "Standard error stream of the process.";

	public const string ProcessStandardInput = "Standard input stream of the process.";

	public const string ProcessStandardOutput = "Standard output stream of the process.";

	public const string ProcessStartInfo = "Specifies information used to start a process.";

	public const string ProcessStartTime = "The time at which the process was started.";

	public const string ProcessTotalProcessorTime = "The amount of CPU time the process has used.";

	public const string ProcessUserProcessorTime = "The amount of CPU time the process spent outside the operating system core.";

	public const string ProcessVirtualMemorySize = "The amount of virtual memory the process has currently allocated.";

	public const string ProcessWorkingSet = "The current amount of physical memory the process is using.";

	public const string ProcModModuleName = "The name of the module.";

	public const string ProcModFileName = "The file name of the module.";

	public const string ProcModBaseAddress = "The memory address that the module loaded at.";

	public const string ProcModModuleMemorySize = "The amount of virtual memory required by the code and data in the module file.";

	public const string ProcModEntryPointAddress = "The memory address of the function that runs when the module is loaded.";

	public const string ProcessVerb = "The verb to apply to the document specified by the FileName property.";

	public const string ProcessArguments = "Command line arguments that will be passed to the application specified by the FileName property.";

	public const string ProcessErrorDialog = "Whether to show an error dialog to the user if there is an error.";

	public const string ProcessWindowStyle = "How the main window should be created when the process starts.";

	public const string ProcessCreateNoWindow = "Whether to start the process without creating a new window to contain it.";

	public const string ProcessEnvironmentVariables = "Set of environment variables that apply to this process and child processes.";

	public const string ProcessRedirectStandardInput = "Whether the process command input is read from the Process instance's StandardInput member.";

	public const string ProcessRedirectStandardOutput = "Whether the process output is written to the Process instance's StandardOutput member.";

	public const string ProcessRedirectStandardError = "Whether the process's error output is written to the Process instance's StandardError member.";

	public const string ProcessUseShellExecute = "Whether to use the operating system shell to start the process.";

	public const string ThreadBasePriority = "The current base priority of the thread.";

	public const string ThreadCurrentPriority = "The current priority level of the thread.";

	public const string ThreadId = "The unique identifier for the thread.";

	public const string ThreadPriorityBoostEnabled = "Whether the thread would like a priority boost when the user interacts with UI associated with the thread.";

	public const string ThreadPriorityLevel = "The priority level of the thread.";

	public const string ThreadPrivilegedProcessorTime = "The amount of CPU time the thread spent inside the operating system core.";

	public const string ThreadStartAddress = "The memory address of the function that was run when the thread started.";

	public const string ThreadStartTime = "The time the thread was started.";

	public const string ThreadThreadState = "The execution state of the thread.";

	public const string ThreadTotalProcessorTime = "The amount of CPU time the thread has consumed since it was started.";

	public const string ThreadUserProcessorTime = "The amount of CPU time the thread spent outside the operating system core.";

	public const string ThreadWaitReason = "The reason the thread is waiting, if it is waiting.";

	public const string VerbEditorDefault = "(Default)";

	public const string AppSettingsReaderNoKey = "The key '{0}' does not exist in the appSettings configuration section.";

	public const string AppSettingsReaderNoParser = "Type '{0}' does not have a Parse method.";

	public const string AppSettingsReaderCantParse = "The value '{0}' was found in the appSettings configuration section for key '{1}', and this value is not a valid {2}.";

	public const string AppSettingsReaderEmptyString = "(empty string)";

	public const string InvalidPermissionState = "Invalid permission state.";

	public const string PermissionNumberOfElements = "The number of elements on the access path must be the same as the number of tag names.";

	public const string PermissionItemExists = "The item provided already exists.";

	public const string PermissionItemDoesntExist = "The requested item doesn't exist.";

	public const string PermissionBadParameterEnum = "Parameter must be of type enum.";

	public const string PermissionInvalidLength = "Length must be greater than {0}.";

	public const string PermissionTypeMismatch = "Type mismatch.";

	public const string Argument_NotAPermissionElement = "'securityElement' was not a permission element.";

	public const string Argument_InvalidXMLBadVersion = "Invalid Xml - can only parse elements of version one.";

	public const string InvalidPermissionLevel = "Invalid permission level.";

	public const string TargetNotWebBrowserPermissionLevel = "Target not WebBrowserPermissionLevel.";

	public const string WebBrowserBadXml = "Bad Xml {0}";

	public const string KeyedCollNeedNonNegativeNum = "Need a non negative number for capacity.";

	public const string KeyedCollDuplicateKey = "Cannot add item since the item with the key already exists in the collection.";

	public const string KeyedCollReferenceKeyNotFound = "The key reference with respect to which the insertion operation was to be performed was not found.";

	public const string KeyedCollKeyNotFound = "Cannot find the key {0} in the collection.";

	public const string KeyedCollInvalidKey = "Keys must be non-null non-empty Strings.";

	public const string KeyedCollCapacityOverflow = "Capacity overflowed and went negative.  Check capacity of the collection.";

	public const string OrderedDictionary_ReadOnly = "The OrderedDictionary is readonly and cannot be modified.";

	public const string OrderedDictionary_SerializationMismatch = "There was an error deserializing the OrderedDictionary.  The ArrayList does not contain DictionaryEntries.";

	public const string Async_ExceptionOccurred = "An exception occurred during the operation, making the result invalid.  Check InnerException for exception details.";

	public const string Async_QueueingFailed = "Queuing WaitCallback failed.";

	public const string Async_OperationCancelled = "Operation has been cancelled.";

	public const string Async_OperationAlreadyCompleted = "This operation has already had OperationCompleted called on it and further calls are illegal.";

	public const string Async_NullDelegate = "A non-null SendOrPostCallback must be supplied.";

	public const string BackgroundWorker_AlreadyRunning = "BackgroundWorker is already running.";

	public const string BackgroundWorker_CancellationNotSupported = "BackgroundWorker does not support cancellation.";

	public const string BackgroundWorker_OperationCompleted = "Operation has already been completed.";

	public const string BackgroundWorker_ProgressNotSupported = "BackgroundWorker does not support progress.";

	public const string BackgroundWorker_WorkerAlreadyRunning = "This BackgroundWorker is currently busy and cannot run multiple tasks concurrently.";

	public const string BackgroundWorker_WorkerDoesntReportProgress = "This BackgroundWorker states that it doesn't report progress. Modify WorkerReportsProgress to state that it does report progress.";

	public const string BackgroundWorker_WorkerDoesntSupportCancellation = "This BackgroundWorker states that it doesn't support cancellation. Modify WorkerSupportsCancellation to state that it does support cancellation.";

	public const string Async_ProgressChangedEventArgs_ProgressPercentage = "Percentage progress made in operation.";

	public const string Async_ProgressChangedEventArgs_UserState = "User-supplied state to identify operation.";

	public const string Async_AsyncEventArgs_Cancelled = "True if operation was cancelled.";

	public const string Async_AsyncEventArgs_Error = "Exception that occurred during operation.  Null if no error.";

	public const string Async_AsyncEventArgs_UserState = "User-supplied state to identify operation.";

	public const string BackgroundWorker_CancellationPending = "Has the user attempted to cancel the operation? To be accessed from DoWork event handler.";

	public const string BackgroundWorker_DoWork = "Event handler to be run on a different thread when the operation begins.";

	public const string BackgroundWorker_IsBusy = "Is the worker still currently working on a background operation?";

	public const string BackgroundWorker_ProgressChanged = "Raised when the worker thread indicates that some progress has been made.";

	public const string BackgroundWorker_RunWorkerCompleted = "Raised when the worker has completed (either through success, failure, or cancellation).";

	public const string BackgroundWorker_WorkerReportsProgress = "Whether the worker will report progress.";

	public const string BackgroundWorker_WorkerSupportsCancellation = "Whether the worker supports cancellation.";

	public const string BackgroundWorker_DoWorkEventArgs_Argument = "Argument passed into the worker handler from BackgroundWorker.RunWorkerAsync.";

	public const string BackgroundWorker_DoWorkEventArgs_Result = "Result from the worker function.";

	public const string BackgroundWorker_Desc = "Executes an operation on a separate thread.";

	public const string InstanceCreationEditorDefaultText = "(New...)";

	public const string PropertyTabAttributeBadPropertyTabScope = "Scope must be PropertyTabScope.Document or PropertyTabScope.Component";

	public const string PropertyTabAttributeTypeLoadException = "Couldn't find type {0}";

	public const string PropertyTabAttributeArrayLengthMismatch = "tabClasses must have the same number of items as tabScopes";

	public const string PropertyTabAttributeParamsBothNull = "An array of tab type names or tab types must be specified";

	public const string InstanceDescriptorCannotBeStatic = "Parameter cannot be static.";

	public const string InstanceDescriptorMustBeStatic = "Parameter must be static.";

	public const string InstanceDescriptorMustBeReadable = "Parameter must be readable.";

	public const string InstanceDescriptorLengthMismatch = "Length mismatch.";

	public const string ToolboxItemAttributeFailedGetType = "Failed to create ToolboxItem of type: {0}";

	public const string PropertyDescriptorCollectionBadValue = "Parameter must be of type PropertyDescriptor.";

	public const string PropertyDescriptorCollectionBadKey = "Parameter must be of type int or string.";

	public const string AspNetHostingPermissionBadXml = "Bad Xml {0}";

	public const string CorruptedGZipHeader = "The magic number in GZip header is not correct. Make sure you are passing in a GZip stream.";

	public const string UnknownCompressionMode = "The compression mode specified in GZip header is unknown.";

	public const string UnknownState = "Decoder is in some unknown state. This might be caused by corrupted data.";

	public const string InvalidHuffmanData = "Failed to construct a huffman tree using the length array. The stream might be corrupted.";

	public const string InvalidCRC = "The CRC in GZip footer does not match the CRC calculated from the decompressed data.";

	public const string InvalidStreamSize = "The stream size in GZip footer does not match the real stream size.";

	public const string UnknownBlockType = "Unknown block type. Stream might be corrupted.";

	public const string InvalidBlockLength = "Block length does not match with its complement.";

	public const string GenericInvalidData = "Found invalid data while decoding.";

	public const string CannotReadFromDeflateStream = "Reading from the compression stream is not supported.";

	public const string CannotWriteToDeflateStream = "Writing to the compression stream is not supported.";

	public const string NotReadableStream = "The base stream is not readable.";

	public const string NotWriteableStream = "The base stream is not writeable.";

	public const string InvalidArgumentOffsetCount = "Offset plus count is larger than the length of target array.";

	public const string InvalidBeginCall = "Only one asynchronous reader is allowed time at one time.";

	public const string InvalidEndCall = "EndRead is only callable when there is one pending asynchronous reader.";

	public const string StreamSizeOverflow = "The gzip stream can't contain more than 4GB data.";

	public const string ZLibErrorDLLLoadError = "The underlying compression routine could not be loaded correctly.";

	public const string ZLibErrorUnexpected = "The underlying compression routine returned an unexpected error code.";

	public const string ZLibErrorInconsistentStream = "The stream state of the underlying compression routine is inconsistent.";

	public const string ZLibErrorSteamFreedPrematurely = "The stream state of the underlying compression routine was freed prematurely.";

	public const string ZLibErrorNotEnoughMemory = "The underlying compression routine could not reserve sufficient memory.";

	public const string ZLibErrorIncorrectInitParameters = "The underlying compression routine received incorrect initialization parameters.";

	public const string ZLibErrorVersionMismatch = "The version of the underlying compression routine does not match expected version.";

	public const string InvalidOperation_HCCountOverflow = "Handle collector count overflows or underflows.";

	public const string Argument_InvalidThreshold = "maximumThreshold cannot be less than initialThreshold.";

	public const string Argument_SemaphoreInitialMaximum = "The initial count for the semaphore must be greater than or equal to zero and less than the maximum count.";

	public const string Argument_WaitHandleNameTooLong = "The name can be no more than 260 characters in length.";

	public const string WaitHandleCannotBeOpenedException_InvalidHandle = "A WaitHandle with system-wide name '{0}' cannot be created. A WaitHandle of a different type might have the same name.";

	public const string ArgumentNotAPermissionElement = "Argument was not a permission Element.";

	public const string ArgumentWrongType = "Argument should be of type {0}.";

	public const string BadXmlVersion = "Xml version was wrong.";

	public const string BinarySerializationNotSupported = "Binary serialization is current not supported by the LocalFileSettingsProvider.";

	public const string BothScopeAttributes = "The setting {0} has both an ApplicationScopedSettingAttribute and a UserScopedSettingAttribute.";

	public const string NoScopeAttributes = "The setting {0} does not have either an ApplicationScopedSettingAttribute or UserScopedSettingAttribute.";

	public const string PositionOutOfRange = "Position cannot be less than zero.";

	public const string ProviderInstantiationFailed = "Failed to instantiate provider: {0}.";

	public const string ProviderTypeLoadFailed = "Failed to load provider type: {0}.";

	public const string SaveAppScopedNotSupported = "Error saving {0} - The LocalFileSettingsProvider does not support saving changes to application-scoped settings.";

	public const string SettingsResetFailed = "Failed to reset settings: unable to access the configuration section.";

	public const string SettingsSaveFailed = "Failed to save settings: {0}";

	public const string SettingsSaveFailedNoSection = "Failed to save settings: unable to access the configuration section.";

	public const string StringDeserializationFailed = "Could not use String deserialization for setting: {0}.";

	public const string StringSerializationFailed = "Could not use String serialization for setting: {0}.";

	public const string UnknownSerializationFormat = "Unknown serialization format specified.";

	public const string UnknownSeekOrigin = "Unknown SeekOrigin specified.";

	public const string UnknownUserLevel = "Unknown ConfigurationUserLevel specified.";

	public const string UserSettingsNotSupported = "The current configuration system does not support user-scoped settings.";

	public const string XmlDeserializationFailed = "Could not use Xml deserialization for setting: {0}.";

	public const string XmlSerializationFailed = "Could not use Xml serialization for setting: {0}.";

	public const string MemberRelationshipService_RelationshipNotSupported = "Relationships between {0}.{1} and {2}.{3} are not supported.";

	public const string MaskedTextProviderPasswordAndPromptCharError = "The PasswordChar and PromptChar values cannot be the same.";

	public const string MaskedTextProviderInvalidCharError = "The specified character value is not allowed for this property.";

	public const string MaskedTextProviderMaskNullOrEmpty = "The Mask value cannot be null or empty.";

	public const string MaskedTextProviderMaskInvalidChar = "The specified mask contains invalid characters.";

	public const string StandardOleMarshalObjectGetMarshalerFailed = "Failed to get marshaler for IID {0}.";

	public const string SoundAPIBadSoundLocation = "Could not determine a universal resource identifier for the sound location.";

	public const string SoundAPIFileDoesNotExist = "Please be sure a sound file exists at the specified location.";

	public const string SoundAPIFormatNotSupported = "Sound API only supports playing PCM wave files.";

	public const string SoundAPIInvalidWaveFile = "The file located at {0} is not a valid wave file.";

	public const string SoundAPIInvalidWaveHeader = "The wave header is corrupt.";

	public const string SoundAPILoadTimedOut = "The request to load the wave file in memory timed out.";

	public const string SoundAPILoadTimeout = "The LoadTimeout property of a SoundPlayer cannot be negative.";

	public const string SoundAPIReadError = "There was an error reading the file located at {0}. Please make sure that a valid wave file exists at the specified location.";

	public const string WrongActionForCtor = "Constructor supports only the '{0}' action.";

	public const string MustBeResetAddOrRemoveActionForCtor = "Constructor only supports either a Reset, Add, or Remove action.";

	public const string ResetActionRequiresNullItem = "Reset action must be initialized with no changed items.";

	public const string ResetActionRequiresIndexMinus1 = "Reset action must be initialized with index -1.";

	public const string IndexCannotBeNegative = "Index cannot be negative.";

	public const string ObservableCollectionReentrancyNotAllowed = "Cannot change ObservableCollection during a CollectionChanged event.";

	public const string Arg_ArgumentOutOfRangeException = "Specified argument was out of the range of valid values.";

	public const string mono_net_io_shutdown = "mono_net_io_shutdown";

	public const string mono_net_io_renegotiate = "mono_net_io_renegotiate";

	public const string net_ssl_io_already_shutdown = "Write operations are not allowed after the channel was shutdown.";

	public const string net_log_set_socketoption_reuseport_default_on = "net_log_set_socketoption_reuseport_default_on";

	public const string net_log_set_socketoption_reuseport_not_supported = "net_log_set_socketoption_reuseport_not_supported";

	public const string net_log_set_socketoption_reuseport = "net_log_set_socketoption_reuseport";

	public const string net_ssl_app_protocols_invalid = "The application protocol list is invalid.";

	public const string net_ssl_app_protocol_invalid = "The application protocol value is invalid.";

	public const string net_conflicting_options = "The '{0}' option was already set in the SslStream constructor.";

	public const string Arg_NonZeroLowerBound = "The lower bound of target array must be zero.";

	public const string Arg_WrongType = "The value '{0}' is not of type '{1}' and cannot be used in this generic collection.";

	public const string Arg_ArrayPlusOffTooSmall = "Destination array is not long enough to copy all the items in the collection. Check array index and length.";

	public const string ArgumentOutOfRange_NeedNonNegNum = "Non-negative number required.";

	public const string ArgumentOutOfRange_SmallCapacity = "capacity was less than the current size.";

	public const string Argument_InvalidOffLen = "Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.";

	public const string Argument_AddingDuplicate = "An item with the same key has already been added. Key: {0}";

	public const string InvalidOperation_ConcurrentOperationsNotSupported = "Operations that change non-concurrent collections must have exclusive access. A concurrent update was performed on this collection and corrupted its state. The collection's state is no longer correct.";

	public const string InvalidOperation_EmptyQueue = "Queue empty.";

	public const string InvalidOperation_EnumOpCantHappen = "Enumeration has either not started or has already finished.";

	public const string InvalidOperation_EnumFailedVersion = "Collection was modified; enumeration operation may not execute.";

	public const string InvalidOperation_EmptyStack = "Stack empty.";

	public const string InvalidOperation_EnumNotStarted = "Enumeration has not started. Call MoveNext.";

	public const string InvalidOperation_EnumEnded = "Enumeration already finished.";

	public const string NotSupported_KeyCollectionSet = "Mutating a key collection derived from a dictionary is not allowed.";

	public const string NotSupported_ValueCollectionSet = "Mutating a value collection derived from a dictionary is not allowed.";

	public const string Arg_ArrayLengthsDiffer = "Array lengths must be the same.";

	public const string Arg_BitArrayTypeUnsupported = "Only supported array types for CopyTo on BitArrays are Boolean[], Int32[] and Byte[].";

	public const string Arg_HSCapacityOverflow = "HashSet capacity is too big.";

	public const string Arg_HTCapacityOverflow = "Hashtable's capacity overflowed and went negative. Check load factor, capacity and the current size of the table.";

	public const string Arg_InsufficientSpace = "Insufficient space in the target location to copy the information.";

	public const string Arg_RankMultiDimNotSupported = "Only single dimensional arrays are supported for the requested action.";

	public const string Argument_ArrayTooLarge = "The input array length must not exceed Int32.MaxValue / {0}. Otherwise BitArray.Length would exceed Int32.MaxValue.";

	public const string Argument_InvalidArrayType = "Target array type is not compatible with the type of items in the collection.";

	public const string ArgumentOutOfRange_BiggerThanCollection = "Must be less than or equal to the size of the collection.";

	public const string ArgumentOutOfRange_Index = "Index was out of range. Must be non-negative and less than the size of the collection.";

	public const string ExternalLinkedListNode = "The LinkedList node does not belong to current LinkedList.";

	public const string LinkedListEmpty = "The LinkedList is empty.";

	public const string LinkedListNodeIsAttached = "The LinkedList node already belongs to a LinkedList.";

	public const string NotSupported_SortedListNestedWrite = "This operation is not supported on SortedList nested types because they require modifying the original SortedList.";

	public const string SortedSet_LowerValueGreaterThanUpperValue = "Must be less than or equal to upperValue.";

	public const string Serialization_InvalidOnDeser = "OnDeserialization method was called while the object was not being deserialized.";

	public const string Serialization_MismatchedCount = "The serialized Count information doesn't match the number of items.";

	public const string Serialization_MissingKeys = "The keys for this dictionary are missing.";

	public const string Serialization_MissingValues = "The values for this dictionary are missing.";

	public const string Arg_KeyNotFoundWithKey = "The given key '{0}' was not present in the dictionary.";

	public const string BlockingCollection_Add_ConcurrentCompleteAdd = "CompleteAdding may not be used concurrently with additions to the collection.";

	public const string BlockingCollection_Add_Failed = "The underlying collection didn't accept the item.";

	public const string BlockingCollection_CantAddAnyWhenCompleted = "At least one of the specified collections is marked as complete with regards to additions.";

	public const string BlockingCollection_CantTakeAnyWhenAllDone = "All collections are marked as complete with regards to additions.";

	public const string BlockingCollection_CantTakeWhenDone = "The collection argument is empty and has been marked as complete with regards to additions.";

	public const string BlockingCollection_Completed = "The collection has been marked as complete with regards to additions.";

	public const string BlockingCollection_CopyTo_IncorrectType = "The array argument is of the incorrect type.";

	public const string BlockingCollection_CopyTo_MultiDim = "The array argument is multidimensional.";

	public const string BlockingCollection_CopyTo_NonNegative = "The index argument must be greater than or equal zero.";

	public const string Collection_CopyTo_TooManyElems = "The number of elements in the collection is greater than the available space from index to the end of the destination array.";

	public const string BlockingCollection_ctor_BoundedCapacityRange = "The boundedCapacity argument must be positive.";

	public const string BlockingCollection_ctor_CountMoreThanCapacity = "The collection argument contains more items than are allowed by the boundedCapacity.";

	public const string BlockingCollection_Disposed = "The collection has been disposed.";

	public const string BlockingCollection_Take_CollectionModified = "The underlying collection was modified from outside of the BlockingCollection<T>.";

	public const string BlockingCollection_TimeoutInvalid = "The specified timeout must represent a value between -1 and {0}, inclusive.";

	public const string BlockingCollection_ValidateCollectionsArray_DispElems = "The collections argument contains at least one disposed element.";

	public const string BlockingCollection_ValidateCollectionsArray_LargeSize = "The collections length is greater than the supported range for 32 bit machine.";

	public const string BlockingCollection_ValidateCollectionsArray_NullElems = "The collections argument contains at least one null element.";

	public const string BlockingCollection_ValidateCollectionsArray_ZeroSize = "The collections argument is a zero-length array.";

	public const string Common_OperationCanceled = "The operation was canceled.";

	public const string ConcurrentBag_Ctor_ArgumentNullException = "The collection argument is null.";

	public const string ConcurrentBag_CopyTo_ArgumentNullException = "The array argument is null.";

	public const string Collection_CopyTo_ArgumentOutOfRangeException = "The index argument must be greater than or equal zero.";

	public const string ConcurrentCollection_SyncRoot_NotSupported = "The SyncRoot property may not be used for the synchronization of concurrent collections.";

	public const string ConcurrentDictionary_ArrayIncorrectType = "The array is multidimensional, or the type parameter for the set cannot be cast automatically to the type of the destination array.";

	public const string ConcurrentDictionary_SourceContainsDuplicateKeys = "The source argument contains duplicate keys.";

	public const string ConcurrentDictionary_ConcurrencyLevelMustBePositive = "The concurrencyLevel argument must be positive.";

	public const string ConcurrentDictionary_CapacityMustNotBeNegative = "The capacity argument must be greater than or equal to zero.";

	public const string ConcurrentDictionary_IndexIsNegative = "The index argument is less than zero.";

	public const string ConcurrentDictionary_ArrayNotLargeEnough = "The index is equal to or greater than the length of the array, or the number of elements in the dictionary is greater than the available space from index to the end of the destination array.";

	public const string ConcurrentDictionary_KeyAlreadyExisted = "The key already existed in the dictionary.";

	public const string ConcurrentDictionary_ItemKeyIsNull = "TKey is a reference type and item.Key is null.";

	public const string ConcurrentDictionary_TypeOfKeyIncorrect = "The key was of an incorrect type for this dictionary.";

	public const string ConcurrentDictionary_TypeOfValueIncorrect = "The value was of an incorrect type for this dictionary.";

	public const string ConcurrentStack_PushPopRange_CountOutOfRange = "The count argument must be greater than or equal to zero.";

	public const string ConcurrentStack_PushPopRange_InvalidCount = "The sum of the startIndex and count arguments must be less than or equal to the collection's Count.";

	public const string ConcurrentStack_PushPopRange_StartOutOfRange = "The startIndex argument must be greater than or equal to zero.";

	public const string Partitioner_DynamicPartitionsNotSupported = "Dynamic partitions are not supported by this partitioner.";

	public const string PartitionerStatic_CanNotCallGetEnumeratorAfterSourceHasBeenDisposed = "Can not call GetEnumerator on partitions after the source enumerable is disposed";

	public const string PartitionerStatic_CurrentCalledBeforeMoveNext = "MoveNext must be called at least once before calling Current.";

	public const string ConcurrentBag_Enumerator_EnumerationNotStartedOrAlreadyFinished = "Enumeration has either not started or has already finished.";

	public const string Argument_AddingDuplicate__ = "Item has already been added. Key in dictionary: '{0}'  Key being added: '{1}'";

	public const string Argument_ImplementIComparable = "At least one object must implement IComparable.";

	public const string Arg_RemoveArgNotFound = "Cannot remove the specified item because it was not found in the specified Collection.";

	public const string ArgumentNull_Dictionary = "Dictionary cannot be null.";

	public const string ArgumentOutOfRange_QueueGrowFactor = "Queue grow factor must be between {0} and {1}.";

	public const string Array = "{0} Array";

	public const string Collection = "(Collection)";

	public const string none = "(none)";

	public const string Null = "(null)";

	public const string Text = "(Text)";

	public const string InvalidColor = "Color '{0}' is not valid.";

	public const string TextParseFailedFormat = "Text \"{0}\" cannot be parsed. The expected text format is \"{1}\".";

	public const string PropertyValueInvalidEntry = "IDictionary parameter contains at least one entry that is not valid. Ensure all values are consistent with the object's properties.";

	public const string ArgumentException_BufferNotFromPool = "The buffer is not associated with this pool and may not be returned to it.";

	public const string Arg_FileIsDirectory_Name = "The target file '{0}' is a directory, not a file.";

	public const string Arg_HandleNotAsync = "Handle does not support asynchronous operations. The parameters to the FileStream constructor may need to be changed to indicate that the handle was opened synchronously (that is, it was not opened for overlapped I/O).";

	public const string Arg_HandleNotSync = "Handle does not support synchronous operations. The parameters to the FileStream constructor may need to be changed to indicate that the handle was opened asynchronously (that is, it was opened explicitly for overlapped I/O).";

	public const string Arg_InvalidFileAttrs = "Invalid File or Directory attributes value.";

	public const string Arg_InvalidHandle = "Invalid handle.";

	public const string Arg_InvalidSearchPattern = "Search pattern cannot contain '..' to move up directories and can be contained only internally in file/directory names, as in 'a..b'.";

	public const string Arg_Path2IsRooted = "Second path fragment must not be a drive or UNC name.";

	public const string Arg_PathIsVolume = "Path must not be a drive.";

	public const string ArgumentNull_FileName = "File name cannot be null.";

	public const string ArgumentNull_Path = "Path cannot be null.";

	public const string ArgumentOutOfRange_Enum = "Enum value was out of legal range.";

	public const string ArgumentOutOfRange_FileLengthTooBig = "Specified file length was too large for the file system.";

	public const string ArgumentOutOfRange_NeedPosNum = "Positive number required.";

	public const string Argument_EmptyFileName = "Empty file name is not legal.";

	public const string Argument_EmptyPath = "Empty path name is not legal.";

	public const string Argument_FileNotResized = "The stream's length cannot be changed.";

	public const string Argument_InvalidAppendMode = "Append access can be requested only in write-only mode.";

	public const string Argument_InvalidFileModeAndAccessCombo = "Combining FileMode: {0} with FileAccess: {1} is invalid.";

	public const string Argument_InvalidPathChars = "Illegal characters in path '{0}'.";

	public const string Argument_InvalidSeekOrigin = "Invalid seek origin.";

	public const string Argument_InvalidSubPath = "The directory specified, '{0}', is not a subdirectory of '{1}'.";

	public const string Argument_PathEmpty = "Path cannot be the empty string or all whitespace.";

	public const string IO_AlreadyExists_Name = "Cannot create '{0}' because a file or directory with the same name already exists.";

	public const string IO_BindHandleFailed = "BindHandle for ThreadPool failed on this handle.";

	public const string IO_CannotCreateDirectory = "The specified directory '{0}' cannot be created.";

	public const string IO_EOF_ReadBeyondEOF = "Unable to read beyond the end of the stream.";

	public const string IO_FileCreateAlreadyExists = "Cannot create a file when that file already exists.";

	public const string IO_FileExists_Name = "The file '{0}' already exists.";

	public const string IO_FileNotFound = "Unable to find the specified file.";

	public const string IO_FileNotFound_FileName = "Could not find file '{0}'.";

	public const string IO_FileStreamHandlePosition = "The OS handle's position is not what FileStream expected. Do not use a handle simultaneously in one FileStream and in Win32 code or another FileStream. This may cause data loss.";

	public const string IO_FileTooLong2GB = "The file is too long. This operation is currently limited to supporting files less than 2 gigabytes in size.";

	public const string IO_FileTooLongOrHandleNotSync = "IO operation will not work. Most likely the file will become too long or the handle was not opened to support synchronous IO operations.";

	public const string IO_PathNotFound_NoPathName = "Could not find a part of the path.";

	public const string IO_PathNotFound_Path = "Could not find a part of the path '{0}'.";

	public const string IO_PathTooLong = "The specified file name or path is too long, or a component of the specified path is too long.";

	public const string IO_SeekAppendOverwrite = "Unable seek backward to overwrite data that previously existed in a file opened in Append mode.";

	public const string IO_SetLengthAppendTruncate = "Unable to truncate data that previously existed in a file opened in Append mode.";

	public const string IO_SharingViolation_File = "The process cannot access the file '{0}' because it is being used by another process.";

	public const string IO_SharingViolation_NoFileName = "The process cannot access the file because it is being used by another process.";

	public const string IO_SourceDestMustBeDifferent = "Source and destination path must be different.";

	public const string IO_SourceDestMustHaveSameRoot = "Source and destination path must have identical roots. Move will not work across volumes.";

	public const string IO_SyncOpOnUIThread = "Synchronous operations should not be performed on the UI thread.  Consider wrapping this method in Task.Run.";

	public const string IO_UnknownFileName = "[Unknown]";

	public const string IndexOutOfRange_IORaceCondition = "Probable I/O race condition detected while copying memory. The I/O package is not thread safe by default. In multithreaded applications, a stream must be accessed in a thread-safe way, such as a thread-safe wrapper returned by TextReader's or TextWriter's Synchronized methods. This also applies to classes like StreamWriter and StreamReader.";

	public const string NotSupported_UnseekableStream = "Stream does not support seeking.";

	public const string ObjectDisposed_FileClosed = "Cannot access a closed file.";

	public const string UnauthorizedAccess_IODenied_NoPathName = "Access to the path is denied.";

	public const string UnauthorizedAccess_IODenied_Path = "Access to the path '{0}' is denied.";

	public const string ObjectDisposed_StreamClosed = "Cannot access a closed Stream.";

	public const string PlatformNotSupported_FileEncryption = "File encryption is not supported on this platform.";

	public const string IO_PathTooLong_Path = "The path '{0}' is too long, or a component of the specified path is too long.";

	public const string InvalidDirName_NotExists = "The directory name '{0}' does not exist.";

	public const string EventStream_FailedToStart = "Failed to start the EventStream";

	public const string IOException_INotifyInstanceSystemLimitExceeded = "The system limit on the number of inotify instances has been reached.";

	public const string IOException_INotifyInstanceUserLimitExceeded_Value = "The configured user limit ({0}) on the number of inotify instances has been reached.";

	public const string IOException_INotifyWatchesUserLimitExceeded_Value = "The configured user limit ({0}) on the number of inotify watches has been reached.";

	public const string IOException_INotifyInstanceUserLimitExceeded = "The configured user limit on the number of inotify instances has been reached.";

	public const string IOException_INotifyWatchesUserLimitExceeded = "The configured user limit on the number of inotify watches has been reached.";

	public const string BaseStream_Invalid_Not_Open = "The BaseStream is only available when the port is open.";

	public const string PortNameEmpty_String = "The PortName cannot be empty.";

	public const string Port_not_open = "The port is closed.";

	public const string Port_already_open = "The port is already open.";

	public const string Cant_be_set_when_open = "'{0}' cannot be set while the port is open.";

	public const string Max_Baud = "The maximum baud rate for the device is {0}.";

	public const string In_Break_State = "The port is in the break state and cannot be written to.";

	public const string Write_timed_out = "The write timed out.";

	public const string CantSetRtsWithHandshaking = "RtsEnable cannot be accessed if Handshake is set to RequestToSend or RequestToSendXOnXOff.";

	public const string NotSupportedEncoding = "SerialPort does not support encoding '{0}'.  The supported encodings include ASCIIEncoding, UTF8Encoding, UnicodeEncoding, UTF32Encoding, and most single or double byte code pages.  For a complete list please see the documentation.";

	public const string Arg_InvalidSerialPort = "The given port name does not start with COM/com or does not resolve to a valid serial port.";

	public const string Arg_InvalidSerialPortExtended = "The given port name is invalid.  It may be a valid port, but not a serial port.";

	public const string ArgumentOutOfRange_Bounds_Lower_Upper = "Argument must be between {0} and {1}.";

	public const string ArgumentOutOfRange_NeedNonNegNumRequired = "Non-negative number required.";

	public const string ArgumentOutOfRange_Timeout = "The timeout must be greater than or equal to -1.";

	public const string ArgumentOutOfRange_WriteTimeout = "The timeout must be either a positive number or -1.";

	public const string IO_OperationAborted = "The I/O operation has been aborted because of either a thread exit or an application request.";

	public const string InvalidNullEmptyArgument = "Argument {0} cannot be null or zero-length.";

	public const string Arg_WrongAsyncResult = "IAsyncResult object did not come from the corresponding async method on this type.";

	public const string InvalidOperation_EndReadCalledMultiple = "EndRead can only be called once for each asynchronous operation.";

	public const string InvalidOperation_EndWriteCalledMultiple = "EndWrite can only be called once for each asynchronous operation.";

	public const string IO_PortNotFound = "The specified port does not exist.";

	public const string IO_PortNotFoundFileName = "The port '{0}' does not exist.";

	public const string PlatformNotSupported_IOPorts = "System.IO.Ports is currently only supported on Windows.";

	public const string PlatformNotSupported_SerialPort_GetPortNames = "Enumeration of serial port names is not supported on the current platform.";

	public const string NotSupported_CannotCallEqualsOnSpan = "Equals() on Span and ReadOnlySpan is not supported. Use operator== instead.";

	public const string NotSupported_CannotCallGetHashCodeOnSpan = "GetHashCode() on Span and ReadOnlySpan is not supported.";

	public const string Argument_InvalidTypeWithPointersNotSupported = "Cannot use type '{0}'. Only value types without pointers or references are supported.";

	public const string Argument_DestinationTooShort = "Destination is too short.";

	public const string MemoryDisposed = "Memory<T> has been disposed.";

	public const string OutstandingReferences = "Release all references before disposing this instance.";

	public const string Argument_BadFormatSpecifier = "Format specifier was invalid.";

	public const string Argument_GWithPrecisionNotSupported = "The 'G' format combined with a precision is not supported.";

	public const string Argument_CannotParsePrecision = "Characters following the format symbol must be a number of {0} or less.";

	public const string Argument_PrecisionTooLarge = "Precision cannot be larger than {0}.";

	public const string Argument_OverlapAlignmentMismatch = "Overlapping spans have mismatching alignment.";

	public const string EndPositionNotReached = "End position was not reached during enumeration.";

	public const string UnexpectedSegmentType = "Unexpected segment type.";

	public const string net_log_listener_delegate_exception = "Sending 500 response, AuthenticationSchemeSelectorDelegate threw an exception: {0}.";

	public const string net_log_listener_unsupported_authentication_scheme = "Received a request with an unsupported authentication scheme, Authorization:{0} SupportedSchemes:{1}.";

	public const string net_log_listener_unmatched_authentication_scheme = "Received a request with an unmatched or no authentication scheme. AuthenticationSchemes:{0}, Authorization:{1}.";

	public const string net_io_invalidasyncresult = "The IAsyncResult object was not returned from the corresponding asynchronous method on this class.";

	public const string net_io_invalidendcall = "{0} can only be called once for each asynchronous operation.";

	public const string net_listener_cannot_set_custom_cbt = "Custom channel bindings are not supported.";

	public const string net_listener_detach_error = "Can't detach Url group from request queue. Status code: {0}.";

	public const string net_listener_scheme = "Only Uri prefixes starting with 'http://' or 'https://' are supported.";

	public const string net_listener_host = "Only Uri prefixes with a valid hostname are supported.";

	public const string net_listener_not_supported = "The request is not supported.";

	public const string net_listener_mustcall = "Please call the {0} method before calling this method.";

	public const string net_listener_slash = "Only Uri prefixes ending in '/' are allowed.";

	public const string net_listener_already = "Failed to listen on prefix '{0}' because it conflicts with an existing registration on the machine.";

	public const string net_log_listener_no_cbt_disabled = "No channel binding check because extended protection is disabled.";

	public const string net_log_listener_no_cbt_http = "No channel binding check for requests without a secure channel.";

	public const string net_log_listener_no_cbt_trustedproxy = "No channel binding check for the trusted proxy scenario.";

	public const string net_log_listener_cbt = "Channel binding check enabled.";

	public const string net_log_listener_no_spn_kerberos = "No explicit service name check because Kerberos authentication already validates the service name.";

	public const string net_log_listener_no_spn_disabled = "No service name check because extended protection is disabled.";

	public const string net_log_listener_no_spn_cbt = "No service name check because the channel binding was already checked.";

	public const string net_log_listener_no_spn_whensupported = "No service name check because the client did not provide a service name and the server was configured for PolicyEnforcement.WhenSupported.";

	public const string net_log_listener_no_spn_loopback = "No service name check because the authentication was from a client on the local machine.";

	public const string net_log_listener_spn = "Client provided service name '{0}'.";

	public const string net_log_listener_spn_passed = "Service name check succeeded.";

	public const string net_log_listener_spn_failed = "Service name check failed.";

	public const string net_log_listener_spn_failed_always = "Service name check failed because the client did not provide a service name and the server was configured for PolicyEnforcement.Always.";

	public const string net_log_listener_spn_failed_empty = "No acceptable service names were configured!";

	public const string net_log_listener_spn_failed_dump = "Dumping acceptable service names:";

	public const string net_log_listener_spn_add = "Adding default service name '{0}' from prefix '{1}'.";

	public const string net_log_listener_spn_not_add = "No default service name added for prefix '{0}'.";

	public const string net_log_listener_spn_remove = "Removing default service name '{0}' from prefix '{1}'.";

	public const string net_log_listener_spn_not_remove = "No default service name removed for prefix '{0}'.";

	public const string net_listener_no_spns = "No service names could be determined from the registered prefixes. Either add prefixes from which default service names can be derived or specify an ExtendedProtectionPolicy object which contains an explicit list of service names.";

	public const string net_ssp_dont_support_cbt = "The Security Service Providers don't support extended protection. Please install the latest Security Service Providers update.";

	public const string net_PropertyNotImplementedException = "This property is not implemented by this class.";

	public const string net_array_too_small = "The target array is too small.";

	public const string net_listener_mustcompletecall = "The in-progress method {0} must be completed first.";

	public const string net_listener_invalid_cbt_type = "Querying the {0} Channel Binding is not supported.";

	public const string net_listener_callinprogress = "Cannot re-call {0} while a previous call is still in progress.";

	public const string net_log_listener_cant_create_uri = "Can't create Uri from string '{0}://{1}{2}{3}'.";

	public const string net_log_listener_cant_convert_raw_path = "Can't convert Uri path '{0}' using encoding '{1}'.";

	public const string net_log_listener_cant_convert_percent_value = "Can't convert percent encoded value '{0}'.";

	public const string net_log_listener_cant_convert_to_utf8 = "Can't convert string '{0}' into UTF-8 bytes: {1}";

	public const string net_log_listener_cant_convert_bytes = "Can't convert bytes '{0}' into UTF-16 characters: {1}";

	public const string net_invalidstatus = "The status code must be exactly three digits.";

	public const string net_WebHeaderInvalidControlChars = "Specified value has invalid Control characters.";

	public const string net_rspsubmitted = "This operation cannot be performed after the response has been submitted.";

	public const string net_nochunkuploadonhttp10 = "Chunked encoding upload is not supported on the HTTP/1.0 protocol.";

	public const string net_cookie_exists = "Cookie already exists.";

	public const string net_clsmall = "The Content-Length value must be greater than or equal to zero.";

	public const string net_wrongversion = "Only HTTP/1.0 and HTTP/1.1 version requests are currently supported.";

	public const string net_noseek = "This stream does not support seek operations.";

	public const string net_writeonlystream = "The stream does not support reading.";

	public const string net_entitytoobig = "Bytes to be written to the stream exceed the Content-Length bytes size specified.";

	public const string net_io_notenoughbyteswritten = "Cannot close stream until all bytes are written.";

	public const string net_listener_close_urlgroup_error = "Can't close Url group. Status code: {0}.";

	public const string net_WebSockets_NativeSendResponseHeaders = "An error occurred when sending the WebSocket HTTP upgrade response during the {0} operation. The HRESULT returned is '{1}'";

	public const string net_WebSockets_ClientAcceptingNoProtocols = "The WebSocket client did not request any protocols, but server attempted to accept '{0}' protocol(s). ";

	public const string net_WebSockets_AcceptUnsupportedProtocol = "The WebSocket client request requested '{0}' protocol(s), but server is only accepting '{1}' protocol(s).";

	public const string net_WebSockets_AcceptNotAWebSocket = "The {0} operation was called on an incoming request that did not specify a '{1}: {2}' header or the {2} header not contain '{3}'. {2} specified by the client was '{4}'.";

	public const string net_WebSockets_AcceptHeaderNotFound = "The {0} operation was called on an incoming WebSocket request without required '{1}' header. ";

	public const string net_WebSockets_AcceptUnsupportedWebSocketVersion = "The {0} operation was called on an incoming request with WebSocket version '{1}', expected '{2}'. ";

	public const string net_WebSockets_InvalidEmptySubProtocol = "Empty string is not a valid subprotocol value. Please use \\\"null\\\" to specify no value.";

	public const string net_WebSockets_InvalidCharInProtocolString = "The WebSocket protocol '{0}' is invalid because it contains the invalid character '{1}'.";

	public const string net_WebSockets_ReasonNotNull = "The close status description '{0}' is invalid. When using close status code '{1}' the description must be null.";

	public const string net_WebSockets_InvalidCloseStatusCode = "The close status code '{0}' is reserved for system use only and cannot be specified when calling this method.";

	public const string net_WebSockets_InvalidCloseStatusDescription = "The close status description '{0}' is too long. The UTF8-representation of the status description must not be longer than {1} bytes.";

	public const string net_WebSockets_ArgumentOutOfRange_TooSmall = "The argument must be a value greater than {0}.";

	public const string net_WebSockets_ArgumentOutOfRange_TooBig = "The value of the '{0}' parameter ({1}) must be less than or equal to {2}.";

	public const string net_WebSockets_UnsupportedPlatform = "The WebSocket protocol is not supported on this platform.";

	public const string net_readonlystream = "The stream does not support writing.";

	public const string net_WebSockets_InvalidState_ClosedOrAborted = "The '{0}' instance cannot be used for communication because it has been transitioned into the '{1}' state.";

	public const string net_WebSockets_ReceiveAsyncDisallowedAfterCloseAsync = "The WebSocket is in an invalid state for this operation. The '{0}' method has already been called before on this instance. Use '{1}' instead to keep being able to receive data but close the output channel.";

	public const string net_Websockets_AlreadyOneOutstandingOperation = "There is already one outstanding '{0}' call for this WebSocket instance. ReceiveAsync and SendAsync can be called simultaneously, but at most one outstanding operation for each of them is allowed at the same time.";

	public const string net_WebSockets_InvalidMessageType = "The received message type '{2}' is invalid after calling {0}. {0} should only be used if no more data is expected from the remote endpoint. Use '{1}' instead to keep being able to receive data but close the output channel.";

	public const string net_WebSockets_InvalidBufferType = "The buffer type '{0}' is invalid. Valid buffer types are: '{1}', '{2}', '{3}', '{4}', '{5}'.";

	public const string net_WebSockets_ArgumentOutOfRange_InternalBuffer = "The byte array must have a length of at least '{0}' bytes.  ";

	public const string net_WebSockets_Argument_InvalidMessageType = "The message type '{0}' is not allowed for the '{1}' operation. Valid message types are: '{2}, {3}'. To close the WebSocket, use the '{4}' operation instead. ";

	public const string net_securitypackagesupport = "The requested security package is not supported.";

	public const string net_log_operation_failed_with_error = "{0} failed with error {1}.";

	public const string net_MethodNotImplementedException = "This method is not implemented by this class.";

	public const string event_OperationReturnedSomething = "{0} returned {1}.";

	public const string net_invalid_enum = "The specified value is not valid in the '{0}' enumeration.";

	public const string net_auth_message_not_encrypted = "Protocol error: A received message contains a valid signature but it was not encrypted as required by the effective Protection Level.";

	public const string SSPIInvalidHandleType = "'{0}' is not a supported handle type.";

	public const string net_io_operation_aborted = "I/O operation aborted: '{0}'.";

	public const string net_invalid_path = "Invalid path.";

	public const string net_listener_auth_errors = "Authentication errors.";

	public const string net_listener_close = "Listener closed.";

	public const string net_invalid_port = "Invalid port in prefix.";

	public const string net_WebSockets_InvalidState = "The WebSocket is in an invalid state ('{0}') for this operation. Valid states are: '{1}'";

	public const string net_unknown_prefix = "The URI prefix is not recognized.";

	public const string net_reqsubmitted = "This operation cannot be performed after the request has been submitted.";

	public const string net_io_timeout_use_ge_zero = "Timeout can be only be set to 'System.Threading.Timeout.Infinite' or a value >= 0.";

	public const string net_writestarted = "This property cannot be set after writing has started.";

	public const string net_badmethod = "Cannot set null or blank methods on request.";

	public const string net_servererror = "The remote server returned an error: ({0}) {1}.";

	public const string net_reqaborted = "The request was aborted: The request was canceled.";

	public const string net_OperationNotSupportedException = "This operation is not supported.";

	public const string net_nouploadonget = "Cannot send a content-body with this verb-type.";

	public const string net_repcall = "Cannot re-call BeginGetRequestStream/BeginGetResponse while a previous call is still in progress.";

	public const string net_securityprotocolnotsupported = "The requested security protocol is not supported.";

	public const string net_requestaborted = "The request was aborted: {0}.";

	public const string net_webstatus_Timeout = "The operation has timed out.";

	public const string net_baddate = "The value of the date string in the header is invalid.";

	public const string net_connarg = "Keep-Alive and Close may not be set using this property.";

	public const string net_fromto = "The From parameter cannot be less than To.";

	public const string net_needchunked = "TransferEncoding requires the SendChunked property to be set to true.";

	public const string net_no100 = "100-Continue may not be set using this property.";

	public const string net_nochunked = "Chunked encoding must be set via the SendChunked property.";

	public const string net_nottoken = "The supplied string is not a valid HTTP token.";

	public const string net_rangetoosmall = "The From or To parameter cannot be less than 0.";

	public const string net_rangetype = "A different range specifier has already been added to this request.";

	public const string net_toosmall = "The specified value must be greater than 0.";

	public const string net_WebHeaderInvalidCRLFChars = "Specified value has invalid CRLF characters.";

	public const string net_WebHeaderInvalidHeaderChars = "Specified value has invalid HTTP Header characters.";

	public const string net_timeout = "The operation has timed out.";

	public const string net_completed_result = "This operation cannot be performed on a completed asynchronous result object.";

	public const string net_PropertyNotSupportedException = "This property is not supported by this class.";

	public const string net_InvalidStatusCode = "The server returned a status code outside the valid range of 100-599.";

	public const string net_io_timeout_use_gt_zero = "Timeout can be only be set to 'System.Threading.Timeout.Infinite' or a value > 0.";

	public const string net_ftp_servererror = "The remote server returned an error: {0}.";

	public const string net_ftp_active_address_different = "The data connection was made from an address that is different than the address to which the FTP connection was made.";

	public const string net_ftp_invalid_method_name = "FTP Method names cannot be null or empty.";

	public const string net_ftp_invalid_renameto = "The RenameTo filename cannot be null or empty.";

	public const string net_ftp_invalid_response_filename = "The server returned the filename ({0}) which is not valid.";

	public const string net_ftp_invalid_status_response = "The status response ({0}) is not expected in response to '{1}' command.";

	public const string net_ftp_invalid_uri = "The requested URI is invalid for this FTP command.";

	public const string net_ftp_no_defaultcreds = "Default credentials are not supported on an FTP request.";

	public const string net_ftp_response_invalid_format = "The response string '{0}' has invalid format.";

	public const string net_ftp_server_failed_passive = "The server failed the passive mode request with status response ({0}).";

	public const string net_ftp_unsupported_method = "This method is not supported.";

	public const string net_ftp_protocolerror = "The underlying connection was closed: The server committed a protocol violation";

	public const string net_ftp_receivefailure = "The underlying connection was closed: An unexpected error occurred on a receive";

	public const string net_webstatus_NameResolutionFailure = "The remote name could not be resolved";

	public const string net_webstatus_ConnectFailure = "Unable to connect to the remote server";

	public const string net_ftpstatuscode_ServiceNotAvailable = "Service not available, closing control connection.";

	public const string net_ftpstatuscode_CantOpenData = "Can't open data connection";

	public const string net_ftpstatuscode_ConnectionClosed = "Connection closed; transfer aborted";

	public const string net_ftpstatuscode_ActionNotTakenFileUnavailableOrBusy = "File unavailable (e.g., file busy)";

	public const string net_ftpstatuscode_ActionAbortedLocalProcessingError = "Local error in processing";

	public const string net_ftpstatuscode_ActionNotTakenInsufficientSpace = "Insufficient storage space in system";

	public const string net_ftpstatuscode_CommandSyntaxError = "Syntax error, command unrecognized";

	public const string net_ftpstatuscode_ArgumentSyntaxError = "Syntax error in parameters or arguments";

	public const string net_ftpstatuscode_CommandNotImplemented = "Command not implemented";

	public const string net_ftpstatuscode_BadCommandSequence = "Bad sequence of commands";

	public const string net_ftpstatuscode_NotLoggedIn = "Not logged in";

	public const string net_ftpstatuscode_AccountNeeded = "Need account for storing files";

	public const string net_ftpstatuscode_ActionNotTakenFileUnavailable = "File unavailable (e.g., file not found, no access)";

	public const string net_ftpstatuscode_ActionAbortedUnknownPageType = "Page type unknown";

	public const string net_ftpstatuscode_FileActionAborted = "Exceeded storage allocation (for current directory or data set)";

	public const string net_ftpstatuscode_ActionNotTakenFilenameNotAllowed = "File name not allowed";

	public const string net_invalid_host = "The specified value is not a valid Host header string.";

	public const string net_sockets_connect_multiconnect_notsupported = "Sockets on this platform are invalid for use after a failed connection attempt.";

	public const string net_sockets_dualmode_receivefrom_notsupported = "This platform does not support packet information for dual-mode sockets.  If packet information is not required, use Socket.Receive.  If packet information is required set Socket.DualMode to false.";

	public const string net_sockets_accept_receive_notsupported = "This platform does not support receiving data with Socket.AcceptAsync.  Instead, make a separate call to Socket.ReceiveAsync.";

	public const string net_sockets_duplicateandclose_notsupported = "This platform does not support Socket.DuplicateAndClose.  Instead, create a new socket.";

	public const string net_sockets_transmitfileoptions_notsupported = "This platform does not support TransmitFileOptions other than TransmitFileOptions.UseDefaultWorkerThread.";

	public const string ArgumentOutOfRange_PathLengthInvalid = "The path '{0}' is of an invalid length for use with domain sockets on this platform.  The length must be between 1 and {1} characters, inclusive.";

	public const string net_io_readwritefailure = "Unable to transfer data on the transport connection: {0}.";

	public const string PlatformNotSupported_AcceptSocket = "Accepting into an existing Socket is not supported on this platform.";

	public const string PlatformNotSupported_IOControl = "Socket.IOControl handles Windows-specific control codes and is not supported on this platform.";

	public const string PlatformNotSupported_IPProtectionLevel = "IP protection level cannot be controlled on this platform.";

	public const string InvalidOperation_BufferNotExplicitArray = "This operation may only be performed when the buffer was set using the SetBuffer overload that accepts an array.";

	public const string InvalidOperation_IncorrectToken = "The result of the operation was already consumed and may not be used again.";

	public const string InvalidOperation_MultipleContinuations = "Another continuation was already registered.";

	public const string net_http_httpmethod_format_error = "The format of the HTTP method is invalid.";

	public const string net_http_httpmethod_notsupported_error = "The HTTP method '{0}' is not supported on this platform.";

	public const string net_http_reasonphrase_format_error = "The reason phrase must not contain new-line characters.";

	public const string net_http_copyto_array_too_small = "The number of elements is greater than the available space from arrayIndex to the end of the destination array.";

	public const string net_http_headers_not_found = "The given header was not found.";

	public const string net_http_headers_single_value_header = "Cannot add value because header '{0}' does not support multiple values.";

	public const string net_http_headers_invalid_header_name = "The header name format is invalid.";

	public const string net_http_headers_invalid_value = "The format of value '{0}' is invalid.";

	public const string net_http_headers_not_allowed_header_name = "Misused header name. Make sure request headers are used with HttpRequestMessage, response headers with HttpResponseMessage, and content headers with HttpContent objects.";

	public const string net_http_headers_invalid_host_header = "The specified value is not a valid 'Host' header string.";

	public const string net_http_headers_invalid_from_header = "The specified value is not a valid 'From' header string.";

	public const string net_http_headers_invalid_etag_name = "The specified value is not a valid quoted string.";

	public const string net_http_headers_invalid_range = "Invalid range. At least one of the two parameters must not be null.";

	public const string net_http_headers_no_newlines = "New-line characters in header values must be followed by a white-space character.";

	public const string net_http_content_buffersize_exceeded = "Cannot write more bytes to the buffer than the configured maximum buffer size: {0}.";

	public const string net_http_content_no_task_returned = "The async operation did not return a System.Threading.Tasks.Task object.";

	public const string net_http_content_stream_already_read = "The stream was already consumed. It cannot be read again.";

	public const string net_http_content_readonly_stream = "The stream does not support writing.";

	public const string net_http_content_invalid_charset = "The character set provided in ContentType is invalid. Cannot read content as string using an invalid character set.";

	public const string net_http_content_stream_copy_error = "Error while copying content to a stream.";

	public const string net_http_argument_empty_string = "The value cannot be null or empty.";

	public const string net_http_client_request_already_sent = "The request message was already sent. Cannot send the same request message multiple times.";

	public const string net_http_operation_started = "This instance has already started one or more requests. Properties can only be modified before sending the first request.";

	public const string net_http_client_execution_error = "An error occurred while sending the request.";

	public const string net_http_client_absolute_baseaddress_required = "The base address must be an absolute URI.";

	public const string net_http_client_invalid_requesturi = "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.";

	public const string net_http_client_http_baseaddress_required = "Only 'http' and 'https' schemes are allowed.";

	public const string net_http_parser_invalid_base64_string = "Value '{0}' is not a valid Base64 string. Error: {1}";

	public const string net_http_handler_noresponse = "Handler did not return a response message.";

	public const string net_http_handler_norequest = "A request message must be provided. It cannot be null.";

	public const string net_http_message_not_success_statuscode = "Response status code does not indicate success: {0} ({1}).";

	public const string net_http_content_field_too_long = "The field cannot be longer than {0} characters.";

	public const string net_http_log_headers_no_newlines = "Value for header '{0}' contains invalid new-line characters. Value: '{1}'.";

	public const string net_http_log_headers_invalid_quality = "The 'q' value is invalid: '{0}'.";

	public const string net_http_log_headers_wrong_email_format = "Value '{0}' is not a valid email address. Error: {1}";

	public const string net_http_handler_not_assigned = "The inner handler has not been assigned.";

	public const string net_http_invalid_enable_first = "The {0} property must be set to '{1}' to use this property.";

	public const string net_http_content_buffersize_limit = "Buffering more than {0} bytes is not supported.";

	public const string net_http_value_not_supported = "The value '{0}' is not supported for property '{1}'.";

	public const string net_http_io_read = "The read operation failed, see inner exception.";

	public const string net_http_io_read_incomplete = "Unable to read data from the transport connection. The connection was closed before all data could be read. Expected {0} bytes, read {1} bytes.";

	public const string net_http_io_write = "The write operation failed, see inner exception.";

	public const string net_http_chunked_not_allowed_with_empty_content = "'Transfer-Encoding: chunked' header can not be used when content object is not specified.";

	public const string net_http_invalid_cookiecontainer = "When using CookieUsePolicy.UseSpecifiedCookieContainer, the CookieContainer property must not be null.";

	public const string net_http_invalid_proxyusepolicy = "When using a non-null Proxy, the WindowsProxyUsePolicy property must be set to WindowsProxyUsePolicy.UseCustomProxy.";

	public const string net_http_invalid_proxy = "When using WindowsProxyUsePolicy.UseCustomProxy, the Proxy property must not be null.";

	public const string net_http_value_must_be_greater_than = "The specified value must be greater than {0}.";

	public const string net_http_unix_invalid_credential = "The libcurl library in use ({0}) does not support different credentials for different authentication schemes.";

	public const string net_http_unix_https_support_unavailable_libcurl = "The libcurl library in use ({0}) does not support HTTPS.";

	public const string net_http_content_no_concurrent_reads = "The stream does not support concurrent read operations.";

	public const string net_http_username_empty_string = "The username for a credential object cannot be null or empty.";

	public const string net_http_no_concurrent_io_allowed = "The stream does not support concurrent I/O read or write operations.";

	public const string net_http_invalid_response = "The server returned an invalid or unrecognized response.";

	public const string net_http_unix_handler_disposed = "The handler was disposed of while active operations were in progress.";

	public const string net_http_buffer_insufficient_length = "The buffer was not long enough.";

	public const string net_http_response_headers_exceeded_length = "The HTTP response headers length exceeded the set limit of {0} bytes.";

	public const string ArgumentOutOfRange_NeedNonNegativeNum = "Non-negative number required.";

	public const string net_http_libcurl_callback_notsupported_sslbackend = "The handler does not support custom handling of certificates with this combination of libcurl ({0}) and its SSL backend (\"{1}\"). An SSL backend based on \"{2}\" is required. Consider using System.Net.Http.SocketsHttpHandler.";

	public const string net_http_libcurl_callback_notsupported_os = "The handler does not support custom handling of certificates on this operating system. Consider using System.Net.Http.SocketsHttpHandler.";

	public const string net_http_libcurl_clientcerts_notsupported_sslbackend = "The handler does not support client authentication certificates with this combination of libcurl ({0}) and its SSL backend (\"{1}\"). An SSL backend based on \"{2}\" is required. Consider using System.Net.Http.SocketsHttpHandler.";

	public const string net_http_libcurl_clientcerts_notsupported_os = "The handler does not support client authentication certificates on this operating system. Consider using System.Net.Http.SocketsHttpHandler.";

	public const string net_http_libcurl_revocation_notsupported_sslbackend = "The handler does not support changing revocation settings with this combination of libcurl ({0}) and its SSL backend (\"{1}\"). An SSL backend based on \"{2}\" is required. Consider using System.Net.Http.SocketsHttpHandler.";

	public const string net_http_feature_requires_Windows10Version1607 = "Using this feature requires Windows 10 Version 1607.";

	public const string net_http_feature_UWPClientCertSupportRequiresCertInPersonalCertificateStore = "Client certificate was not found in the personal (\\\"MY\\\") certificate store. In UWP, client certificates are only supported if they have been added to that certificate store.";

	public const string net_http_invalid_proxy_scheme = "Only the 'http' scheme is allowed for proxies.";

	public const string net_http_request_invalid_char_encoding = "Request headers must contain only ASCII characters.";

	public const string net_http_ssl_connection_failed = "The SSL connection could not be established, see inner exception.";

	public const string net_http_unsupported_chunking = "HTTP 1.0 does not support chunking.";

	public const string net_http_unsupported_version = "Request HttpVersion 0.X is not supported.  Use 1.0 or above.";

	public const string IO_SeekBeforeBegin = "An attempt was made to move the position before the beginning of the stream.";

	public const string net_http_request_no_host = "CONNECT request must contain Host header.";

	public const string net_http_winhttp_error = "Error {0} calling {1}, '{2}'.";

	public const string net_http_authconnectionfailure = "Authentication failed because the connection could not be reused.";

	public const string net_nego_server_not_supported = "Server implementation is not supported";

	public const string net_nego_protection_level_not_supported = "Requested protection level is not supported with the gssapi implementation currently installed.";

	public const string net_context_buffer_too_small = "Insufficient buffer space. Required: {0} Actual: {1}.";

	public const string net_gssapi_operation_failed_detailed = "GSSAPI operation failed with error - {0} ({1}).";

	public const string net_gssapi_operation_failed = "GSSAPI operation failed with status: {0} (Minor status: {1}).";

	public const string net_nego_channel_binding_not_supported = "No support for channel binding on operating systems other than Windows.";

	public const string net_ntlm_not_possible_default_cred = "NTLM authentication is not possible with default credentials on this platform.";

	public const string net_nego_not_supported_empty_target_with_defaultcreds = "Target name should be non empty if default credentials are passed.";

	public const string Arg_ElementsInSourceIsGreaterThanDestination = "Number of elements in source vector is greater than the destination array";

	public const string Arg_NullArgumentNullRef = "The method was called with a null array argument.";

	public const string Arg_TypeNotSupported = "Specified type is not supported";

	public const string Arg_InsufficientNumberOfElements = "At least {0} element(s) are expected in the parameter \"{1}\".";

	public const string NoMetadataTokenAvailable = "There is no metadata token available for the given member.";

	public const string PlatformNotSupported_ReflectionTypeExtensions = "System.Reflection.TypeExtensions is not supported on NET Standard 1.3 or 1.5.";

	public const string Argument_EmptyApplicationName = "ApplicationId cannot have an empty string for the name.";

	public const string ArgumentOutOfRange_GenericPositive = "Value must be positive.";

	public const string ArgumentOutOfRange_MustBePositive = "'{0}' must be greater than zero.";

	public const string InvalidOperation_ComputerName = "Computer name could not be obtained.";

	public const string InvalidOperation_GetVersion = "OSVersion's call to GetVersionEx failed";

	public const string PersistedFiles_NoHomeDirectory = "The home directory of the current user could not be determined.";

	public const string Argument_BadResourceScopeTypeBits = "Unknown value for the ResourceScope: {0}  Too many resource type bits may be set.";

	public const string Argument_BadResourceScopeVisibilityBits = "Unknown value for the ResourceScope: {0}  Too many resource visibility bits may be set.";

	public const string ArgumentNull_TypeRequiredByResourceScope = "The type parameter cannot be null when scoping the resource's visibility to Private or Assembly.";

	public const string Argument_ResourceScopeWrongDirection = "Resource type in the ResourceScope enum is going from a more restrictive resource type to a more general one.  From: \"{0}\"  To: \"{1}\"";

	public const string AppDomain_Name = "Name:";

	public const string AppDomain_NoContextPolicies = "There are no context policies.";

	public const string Arg_MustBeTrue = "Argument must be true.";

	public const string Arg_CannotUnloadAppDomainException = "Attempt to unload the AppDomain failed.";

	public const string Arg_AppDomainUnloadedException = "Attempted to access an unloaded AppDomain.";

	public const string ZeroLengthString = "String cannot have zero length.";

	public const string EntryPointNotFound = "Entry point not found in assembly";

	public const string Arg_ContextMarshalException = "Attempted to marshal an object across a context boundary.";

	public const string AppDomain_Policy_PrincipalTwice = "Default principal object cannot be set twice.";

	public const string ArgumentNull_Collection = "Collection cannot be null.";

	public const string ArgumentOutOfRange_ArrayListInsert = "Insertion index was out of range. Must be non-negative and less than or equal to size.";

	public const string ArgumentOutOfRange_Count = "Count must be positive and count must refer to a location within the string/array/collection.";

	public const string ArgumentOutOfRange_HashtableLoadFactor = "Load factor needs to be between 0.1 and 1.0.";

	public const string ArgumentOutOfRange_MustBeNonNegNum = "'{0}' must be non-negative.";

	public const string Arg_CannotMixComparisonInfrastructure = "The usage of IKeyComparer and IHashCodeProvider/IComparer interfaces cannot be mixed; use one or the other.";

	public const string InvalidOperation_HashInsertFailed = "Hashtable insert failed. Load factor too high. The most common cause is multiple threads writing to the Hashtable simultaneously.";

	public const string InvalidOperation_UnderlyingArrayListChanged = "This range in the underlying list is invalid. A possible cause is that elements were removed.";

	public const string NotSupported_FixedSizeCollection = "Collection was of a fixed size.";

	public const string NotSupported_RangeCollection = "The specified operation is not supported on Ranges.";

	public const string NotSupported_ReadOnlyCollection = "Collection is read-only.";

	public const string Serialization_KeyValueDifferentSizes = "The keys and values arrays have different sizes.";

	public const string Serialization_NullKey = "One of the serialized keys is null.";

	public const string NotSupported_CannotWriteToBufferedStreamIfReadBufferCannotBeFlushed = "Cannot write to a BufferedStream while the read buffer is not empty if the underlying stream is not seekable. Ensure that the stream underlying this BufferedStream can seek or avoid interleaving read and write operations on this BufferedStream.";

	public const string Argument_StreamNotReadable = "Stream was not readable.";

	public const string Argument_StreamNotWritable = "Stream was not writable.";

	public const string ObjectDisposed_ReaderClosed = "Cannot read from a closed TextReader.";

	public const string ArgumentNull_Child = "Cannot have a null child.";

	public const string Argument_AttributeNamesMustBeUnique = "Attribute names must be unique.";

	public const string Argument_InvalidElementName = "Invalid element name '{0}'.";

	public const string Argument_InvalidElementTag = "Invalid element tag '{0}'.";

	public const string Argument_InvalidElementText = "Invalid element text '{0}'.";

	public const string Argument_InvalidElementValue = "Invalid element value '{0}'.";

	public const string Argument_InvalidFlag = "Value of flags is invalid.";

	public const string PlatformNotSupported_AppDomains = "Secondary AppDomains are not supported on this platform.";

	public const string PlatformNotSupported_CAS = "Code Access Security is not supported on this platform.";

	public const string PlatformNotSupported_AppDomain_ResMon = "AppDomain resource monitoring is not supported on this platform.";

	public const string Argument_EmptyValue = "Value cannot be empty.";

	public const string PlatformNotSupported_RuntimeInformation = "RuntimeInformation is not supported for Portable Class Libraries.";

	public const string Overflow_Negative_Unsigned = "Negative values do not have an unsigned representation.";

	public const string Cryptography_Oid_InvalidName = "No OID value matches this name.";

	public const string Cryptography_SSE_InvalidDataSize = "NoLength of the data to encrypt is invalid.";

	public const string Cryptography_Der_Invalid_Encoding = "ASN1 corrupted data.";

	public const string ObjectDisposed_Generic = "Cannot access a disposed object.";

	public const string Cryptography_Asn_EnumeratedValueRequiresNonFlagsEnum = "ASN.1 Enumerated values only apply to enum types without the [Flags] attribute.";

	public const string Cryptography_Asn_NamedBitListRequiresFlagsEnum = "Named bit list operations require an enum with the [Flags] attribute.";

	public const string Cryptography_Asn_NamedBitListValueTooBig = "The encoded named bit list value is larger than the value size of the '{0}' enum.";

	public const string Cryptography_Asn_UniversalValueIsFixed = "Tags with TagClass Universal must have the appropriate TagValue value for the data type being read or written.";

	public const string Cryptography_Asn_UnusedBitCountRange = "Unused bit count must be between 0 and 7, inclusive.";

	public const string Cryptography_AsnSerializer_AmbiguousFieldType = "Field '{0}' of type '{1}' has ambiguous type '{2}', an attribute derived from AsnTypeAttribute is required.";

	public const string Cryptography_AsnSerializer_Choice_AllowNullNonNullable = "[Choice].AllowNull=true is not valid because type '{0}' cannot have a null value.";

	public const string Cryptography_AsnSerializer_Choice_ConflictingTagMapping = "The tag ({0} {1}) for field '{2}' on type '{3}' already is associated in this context with field '{4}' on type '{5}'.";

	public const string Cryptography_AsnSerializer_Choice_DefaultValueDisallowed = "Field '{0}' on [Choice] type '{1}' has a default value, which is not permitted.";

	public const string Cryptography_AsnSerializer_Choice_NoChoiceWasMade = "An instance of [Choice] type '{0}' has no non-null fields.";

	public const string Cryptography_AsnSerializer_Choice_NonNullableField = "Field '{0}' on [Choice] type '{1}' can not be assigned a null value.";

	public const string Cryptography_AsnSerializer_Choice_TooManyValues = "Fields '{0}' and '{1}' on type '{2}' are both non-null when only one value is permitted.";

	public const string Cryptography_AsnSerializer_Choice_TypeCycle = "Field '{0}' on [Choice] type '{1}' has introduced a type chain cycle.";

	public const string Cryptography_AsnSerializer_MultipleAsnTypeAttributes = "Field '{0}' on type '{1}' has multiple attributes deriving from '{2}' when at most one is permitted.";

	public const string Cryptography_AsnSerializer_NoJaggedArrays = "Type '{0}' cannot be serialized or deserialized because it is an array of arrays.";

	public const string Cryptography_AsnSerializer_NoMultiDimensionalArrays = "Type '{0}' cannot be serialized or deserialized because it is a multi-dimensional array.";

	public const string Cryptography_AsnSerializer_NoOpenTypes = "Type '{0}' cannot be serialized or deserialized because it is not sealed or has unbound generic parameters.";

	public const string Cryptography_AsnSerializer_Optional_NonNullableField = "Field '{0}' on type '{1}' is declared [OptionalValue], but it can not be assigned a null value.";

	public const string Cryptography_AsnSerializer_PopulateFriendlyNameOnString = "Field '{0}' on type '{1}' has [ObjectIdentifier].PopulateFriendlyName set to true, which is not applicable to a string.  Change the field to '{2}' or set PopulateFriendlyName to false.";

	public const string Cryptography_AsnSerializer_SetValueException = "Unable to set field {0} on type {1}.";

	public const string Cryptography_AsnSerializer_SpecificTagChoice = "Field '{0}' on type '{1}' has specified an implicit tag value via [ExpectedTag] for [Choice] type '{2}'. ExplicitTag must be true, or the [ExpectedTag] attribute removed.";

	public const string Cryptography_AsnSerializer_UnexpectedTypeForAttribute = "Field '{0}' of type '{1}' has an effective type of '{2}' when one of ({3}) was expected.";

	public const string Cryptography_AsnSerializer_UtcTimeTwoDigitYearMaxTooSmall = "Field '{0}' on type '{1}' has a [UtcTime] TwoDigitYearMax value ({2}) smaller than the minimum (99).";

	public const string Cryptography_AsnSerializer_UnhandledType = "Could not determine how to serialize or deserialize type '{0}'.";

	public const string Cryptography_AsnWriter_EncodeUnbalancedStack = "Encode cannot be called while a Sequence or SetOf is still open.";

	public const string Cryptography_AsnWriter_PopWrongTag = "Cannot pop the requested tag as it is not currently in progress.";

	public const string Cryptography_BadHashValue = "The hash value is not correct.";

	public const string Cryptography_BadSignature = "Invalid signature.";

	public const string Cryptography_Cms_CannotDetermineSignatureAlgorithm = "Could not determine signature algorithm for the signer certificate.";

	public const string Cryptography_Cms_IncompleteCertChain = "The certificate chain is incomplete, the self-signed root authority could not be determined.";

	public const string Cryptography_Cms_Invalid_Originator_Identifier_Choice = "Invalid originator identifier choice {0} found in decoded CMS.";

	public const string Cryptography_Cms_InvalidMessageType = "Invalid cryptographic message type.";

	public const string Cryptography_Cms_InvalidSignerHashForSignatureAlg = "SignerInfo digest algorithm '{0}' is not valid for signature algorithm '{1}'.";

	public const string Cryptography_Cms_MissingAuthenticatedAttribute = "The cryptographic message does not contain an expected authenticated attribute.";

	public const string Cryptography_Cms_NoCounterCounterSigner = "Only one level of counter-signatures are supported on this platform.";

	public const string Cryptography_Cms_NoRecipients = "The recipients collection is empty. You must specify at least one recipient. This platform does not implement the certificate picker UI.";

	public const string Cryptography_Cms_NoSignerCert = "No signer certificate was provided. This platform does not implement the certificate picker UI.";

	public const string Cryptography_Cms_NoSignerAtIndex = "The signed cryptographic message does not have a signer for the specified signer index.";

	public const string Cryptography_Cms_RecipientNotFound = "The enveloped-data message does not contain the specified recipient.";

	public const string Cryptography_Cms_RecipientType_NotSupported = "The recipient type '{0}' is not supported for encryption or decryption on this platform.";

	public const string Cryptography_Cms_SignerNotFound = "Cannot find the original signer.";

	public const string Cryptography_Cms_Signing_RequiresPrivateKey = "A certificate with a private key is required.";

	public const string Cryptography_Cms_TrustFailure = "Certificate trust could not be established. The first reported error is: {0}";

	public const string Cryptography_Cms_UnknownAlgorithm = "Unknown algorithm '{0}'.";

	public const string Cryptography_Cms_UnknownKeySpec = "Unable to determine the type of key handle from this keyspec {0}.";

	public const string Cryptography_Cms_WrongKeyUsage = "The certificate is not valid for the requested usage.";

	public const string Cryptography_Pkcs_InvalidSignatureParameters = "Invalid signature paramters.";

	public const string Cryptography_Pkcs_PssParametersMissing = "PSS parameters were not present.";

	public const string Cryptography_Pkcs_PssParametersHashMismatch = "This platform requires that the PSS hash algorithm ({0}) match the data digest algorithm ({1}).";

	public const string Cryptography_Pkcs_PssParametersMgfHashMismatch = "This platform does not support the MGF hash algorithm ({0}) being different from the signature hash algorithm ({1}).";

	public const string Cryptography_Pkcs_PssParametersMgfNotSupported = "Mask generation function '{0}' is not supported by this platform.";

	public const string Cryptography_Pkcs_PssParametersSaltMismatch = "PSS salt size {0} is not supported by this platform with hash algorithm {1}.";

	public const string Cryptography_TimestampReq_BadNonce = "The response from the timestamping server did not match the request nonce.";

	public const string Cryptography_TimestampReq_BadResponse = "The response from the timestamping server was not understood.";

	public const string Cryptography_TimestampReq_Failure = "The timestamping server did not grant the request. The request status is '{0}' with failure info '{1}'.";

	public const string Cryptography_TimestampReq_NoCertFound = "The timestamping request required the TSA certificate in the response, but it was not found.";

	public const string Cryptography_TimestampReq_UnexpectedCertFound = "The timestamping request required the TSA certificate not be included in the response, but certificates were present.";

	public const string InvalidOperation_WrongOidInAsnCollection = "AsnEncodedData element in the collection has wrong Oid value: expected = '{0}', actual = '{1}'.";

	public const string PlatformNotSupported_CryptographyPkcs = "System.Security.Cryptography.Pkcs is only supported on Windows platforms.";

	public const string Cryptography_Invalid_IA5String = "The string contains a character not in the 7 bit ASCII character set.";

	public const string Cryptography_UnknownHashAlgorithm = "'{0}' is not a known hash algorithm.";

	public const string Cryptography_WriteEncodedValue_OneValueAtATime = "The input to WriteEncodedValue must represent a single encoded value with no trailing data.";

	public const string Arg_CryptographyException = "Error occurred during a cryptographic operation.";

	public const string Cryptography_CryptoStream_FlushFinalBlockTwice = "FlushFinalBlock() method was called twice on a CryptoStream. It can only be called once.";

	public const string Cryptography_DefaultAlgorithm_NotSupported = "This platform does not allow the automatic selection of an algorithm.";

	public const string Cryptography_HashNotYetFinalized = "Hash must be finalized before the hash value is retrieved.";

	public const string Cryptography_InvalidFeedbackSize = "Specified feedback size is not valid for this algorithm.";

	public const string Cryptography_InvalidBlockSize = "Specified block size is not valid for this algorithm.";

	public const string Cryptography_InvalidCipherMode = "Specified cipher mode is not valid for this algorithm.";

	public const string Cryptography_InvalidIVSize = "Specified initialization vector (IV) does not match the block size for this algorithm.";

	public const string Cryptography_InvalidKeySize = "Specified key is not a valid size for this algorithm.";

	public const string Cryptography_InvalidPaddingMode = "Specified padding mode is not valid for this algorithm.";

	public const string HashNameMultipleSetNotSupported = "Setting the hashname after it's already been set is not supported on this platform.";

	public const string CryptoConfigNotSupported = "Accessing a hash algorithm by manipulating the HashName property is not supported on this platform. Instead, you must instantiate one of the supplied subtypes (such as HMACSHA1.)";

	public const string InvalidOperation_IncorrectImplementation = "The algorithm's implementation is incorrect.";

	public const string Cryptography_DpApi_ProfileMayNotBeLoaded = "The data protection operation was unsuccessful. This may have been caused by not having the user profile loaded for the current thread's user context, which may be the case when the thread is impersonating.";

	public const string PlatformNotSupported_CryptographyProtectedData = "Windows Data Protection API (DPAPI) is not supported on this platform.";

	public const string Cryptography_Partial_Chain = "A certificate chain could not be built to a trusted root authority.";

	public const string Cryptography_Xml_BadWrappedKeySize = "Bad wrapped key size.";

	public const string Cryptography_Xml_CipherValueElementRequired = "A Cipher Data element should have either a CipherValue or a CipherReference element.";

	public const string Cryptography_Xml_CreateHashAlgorithmFailed = "Could not create hash algorithm object.";

	public const string Cryptography_Xml_CreateTransformFailed = "Could not create the XML transformation identified by the URI {0}.";

	public const string Cryptography_Xml_CreatedKeyFailed = "Failed to create signing key.";

	public const string Cryptography_Xml_DigestMethodRequired = "A DigestMethod must be specified on a Reference prior to generating XML.";

	public const string Cryptography_Xml_DigestValueRequired = "A Reference must contain a DigestValue.";

	public const string Cryptography_Xml_EnvelopedSignatureRequiresContext = "An XmlDocument context is required for enveloped transforms.";

	public const string Cryptography_Xml_InvalidElement = "Malformed element {0}.";

	public const string Cryptography_Xml_InvalidEncryptionProperty = "Malformed encryption property element.";

	public const string Cryptography_Xml_InvalidKeySize = "The key size should be a non negative integer.";

	public const string Cryptography_Xml_InvalidReference = "Malformed reference element.";

	public const string Cryptography_Xml_InvalidSignatureLength = "The length of the signature with a MAC should be less than the hash output length.";

	public const string Cryptography_Xml_InvalidSignatureLength2 = "The length in bits of the signature with a MAC should be a multiple of 8.";

	public const string Cryptography_Xml_InvalidX509IssuerSerialNumber = "X509 issuer serial number is invalid.";

	public const string Cryptography_Xml_KeyInfoRequired = "A KeyInfo element is required to check the signature.";

	public const string Cryptography_Xml_KW_BadKeySize = "The length of the encrypted data in Key Wrap is either 32, 40 or 48 bytes.";

	public const string Cryptography_Xml_LoadKeyFailed = "Signing key is not loaded.";

	public const string Cryptography_Xml_MissingAlgorithm = "Symmetric algorithm is not specified.";

	public const string Cryptography_Xml_MissingCipherData = "Cipher data is not specified.";

	public const string Cryptography_Xml_MissingDecryptionKey = "Unable to retrieve the decryption key.";

	public const string Cryptography_Xml_MissingEncryptionKey = "Unable to retrieve the encryption key.";

	public const string Cryptography_Xml_NotSupportedCryptographicTransform = "The specified cryptographic transform is not supported.";

	public const string Cryptography_Xml_ReferenceElementRequired = "At least one Reference element is required.";

	public const string Cryptography_Xml_ReferenceTypeRequired = "The Reference type must be set in an EncryptedReference object.";

	public const string Cryptography_Xml_SelfReferenceRequiresContext = "An XmlDocument context is required to resolve the Reference Uri {0}.";

	public const string Cryptography_Xml_SignatureDescriptionNotCreated = "SignatureDescription could not be created for the signature algorithm supplied.";

	public const string Cryptography_Xml_SignatureMethodKeyMismatch = "The key does not fit the SignatureMethod.";

	public const string Cryptography_Xml_SignatureMethodRequired = "A signature method is required.";

	public const string Cryptography_Xml_SignatureValueRequired = "Signature requires a SignatureValue.";

	public const string Cryptography_Xml_SignedInfoRequired = "Signature requires a SignedInfo.";

	public const string Cryptography_Xml_TransformIncorrectInputType = "The input type was invalid for this transform.";

	public const string Cryptography_Xml_IncorrectObjectType = "Type of input object is invalid.";

	public const string Cryptography_Xml_UnknownTransform = "Unknown transform has been encountered.";

	public const string Cryptography_Xml_UriNotResolved = "Unable to resolve Uri {0}.";

	public const string Cryptography_Xml_UriNotSupported = " The specified Uri is not supported.";

	public const string Cryptography_Xml_UriRequired = "A Uri attribute is required for a CipherReference element.";

	public const string Cryptography_Xml_XrmlMissingContext = "Null Context property encountered.";

	public const string Cryptography_Xml_XrmlMissingIRelDecryptor = "IRelDecryptor is required.";

	public const string Cryptography_Xml_XrmlMissingIssuer = "Issuer node is required.";

	public const string Cryptography_Xml_XrmlMissingLicence = "License node is required.";

	public const string Cryptography_Xml_XrmlUnableToDecryptGrant = "Unable to decrypt grant content.";

	public const string Log_ActualHashValue = "Actual hash value: {0}";

	public const string Log_BeginCanonicalization = "Beginning canonicalization using \"{0}\" ({1}).";

	public const string Log_BeginSignatureComputation = "Beginning signature computation.";

	public const string Log_BeginSignatureVerification = "Beginning signature verification.";

	public const string Log_BuildX509Chain = "Building and verifying the X509 chain for certificate {0}.";

	public const string Log_CanonicalizationSettings = "Canonicalization transform is using resolver {0} and base URI \"{1}\".";

	public const string Log_CanonicalizedOutput = "Output of canonicalization transform: {0}";

	public const string Log_CertificateChain = "Certificate chain:";

	public const string Log_CheckSignatureFormat = "Checking signature format using format validator \"[{0}] {1}.{2}\".";

	public const string Log_CheckSignedInfo = "Checking signature on SignedInfo with id \"{0}\".";

	public const string Log_FormatValidationSuccessful = "Signature format validation was successful.";

	public const string Log_FormatValidationNotSuccessful = "Signature format validation failed.";

	public const string Log_KeyUsages = "Found key usages \"{0}\" in extension {1} on certificate {2}.";

	public const string Log_NoNamespacesPropagated = "No namespaces are being propagated.";

	public const string Log_PropagatingNamespace = "Propagating namespace {0}=\"{1}\".";

	public const string Log_RawSignatureValue = "Raw signature: {0}";

	public const string Log_ReferenceHash = "Reference {0} hashed with \"{1}\" ({2}) has hash value {3}, expected hash value {4}.";

	public const string Log_RevocationMode = "Revocation mode for chain building: {0}.";

	public const string Log_RevocationFlag = "Revocation flag for chain building: {0}.";

	public const string Log_SigningAsymmetric = "Calculating signature with key {0} using signature description {1}, hash algorithm {2}, and asymmetric signature formatter {3}.";

	public const string Log_SigningHmac = "Calculating signature using keyed hash algorithm {0}.";

	public const string Log_SigningReference = "Hashing reference {0}, Uri \"{1}\", Id \"{2}\", Type \"{3}\" with hash algorithm \"{4}\" ({5}).";

	public const string Log_TransformedReferenceContents = "Transformed reference contents: {0}";

	public const string Log_UnsafeCanonicalizationMethod = "Canonicalization method \"{0}\" is not on the safe list. Safe canonicalization methods are: {1}.";

	public const string Log_UrlTimeout = "URL retrieval timeout for chain building: {0}.";

	public const string Log_VerificationFailed = "Verification failed checking {0}.";

	public const string Log_VerificationFailed_References = "references";

	public const string Log_VerificationFailed_SignedInfo = "SignedInfo";

	public const string Log_VerificationFailed_X509Chain = "X509 chain verification";

	public const string Log_VerificationFailed_X509KeyUsage = "X509 key usage verification";

	public const string Log_VerificationFlag = "Verification flags for chain building: {0}.";

	public const string Log_VerificationTime = "Verification time for chain building: {0}.";

	public const string Log_VerificationWithKeySuccessful = "Verification with key {0} was successful.";

	public const string Log_VerificationWithKeyNotSuccessful = "Verification with key {0} was not successful.";

	public const string Log_VerifyReference = "Processing reference {0}, Uri \"{1}\", Id \"{2}\", Type \"{3}\".";

	public const string Log_VerifySignedInfoAsymmetric = "Verifying SignedInfo using key {0}, signature description {1}, hash algorithm {2}, and asymmetric signature deformatter {3}.";

	public const string Log_VerifySignedInfoHmac = "Verifying SignedInfo using keyed hash algorithm {0}.";

	public const string Log_X509ChainError = "Error building X509 chain: {0}: {1}.";

	public const string Log_XmlContext = "Using context: {0}";

	public const string Log_SignedXmlRecursionLimit = "Signed xml recursion limit hit while trying to decrypt the key. Reference {0} hashed with \"{1}\" and ({2}).";

	public const string Log_UnsafeTransformMethod = "Transform method \"{0}\" is not on the safe list. Safe transform methods are: {1}.";

	public const string Arg_InvalidType = "Invalid type.";

	public const string Chain_NoPolicyMatch = "The certificate has invalid policy.";

	public const string Cryptography_BadHashSize_ForAlgorithm = "The provided value of {0} bytes does not match the expected size of {1} bytes for the algorithm ({2}).";

	public const string Cryptography_Cert_AlreadyHasPrivateKey = "The certificate already has an associated private key.";

	public const string Cryptography_CertReq_AlgorithmMustMatch = "The issuer certificate public key algorithm ({0}) does not match the value for this certificate request ({1}), use the X509SignatureGenerator overload.";

	public const string Cryptography_CertReq_BasicConstraintsRequired = "The issuer certificate does not have a Basic Constraints extension.";

	public const string Cryptography_CertReq_DatesReversed = "The provided notBefore value is later than the notAfter value.";

	public const string Cryptography_CertReq_DateTooOld = "The value predates 1950 and has no defined encoding.";

	public const string Cryptography_CertReq_DuplicateExtension = "An X509Extension with OID '{0}' has already been specified.";

	public const string Cryptography_CertReq_IssuerBasicConstraintsInvalid = "The issuer certificate does not have an appropriate value for the Basic Constraints extension.";

	public const string Cryptography_CertReq_IssuerKeyUsageInvalid = "The issuer certificate's Key Usage extension is present but does not contain the KeyCertSign flag.";

	public const string Cryptography_CertReq_IssuerRequiresPrivateKey = "The provided issuer certificate does not have an associated private key.";

	public const string Cryptography_CertReq_NotAfterNotNested = "The requested notAfter value ({0}) is later than issuerCertificate.NotAfter ({1}).";

	public const string Cryptography_CertReq_NotBeforeNotNested = "The requested notBefore value ({0}) is earlier than issuerCertificate.NotBefore ({1}).";

	public const string Cryptography_CertReq_NoKeyProvided = "This method cannot be used since no signing key was provided via a constructor, use an overload accepting an X509SignatureGenerator instead.";

	public const string Cryptography_CertReq_RSAPaddingRequired = "The issuer certificate uses an RSA key but no RSASignaturePadding was provided to a constructor. If one cannot be provided, use the X509SignatureGenerator overload.";

	public const string Cryptography_CSP_NoPrivateKey = "Object contains only the public half of a key pair. A private key must also be provided.";

	public const string Cryptography_CurveNotSupported = "The specified curve '{0}' or its parameters are not valid for this platform.";

	public const string Cryptography_ECC_NamedCurvesOnly = "Only named curves are supported on this platform.";

	public const string Cryptography_Encryption_MessageTooLong = "The message exceeds the maximum allowable length for the chosen options ({0}).";

	public const string Cryptography_HashAlgorithmNameNullOrEmpty = "The hash algorithm name cannot be null or empty.";

	public const string Cryptography_InvalidOID = "Object identifier (OID) is unknown.";

	public const string Cryptography_InvalidPublicKey_Object = "The provided PublicKey object is invalid, valid Oid and EncodedKeyValue property values are required.";

	public const string Cryptography_InvalidRsaParameters = "The specified RSA parameters are not valid; both Exponent and Modulus are required fields.";

	public const string Cryptography_KeyTooSmall = "The key is too small for the requested operation.";

	public const string Cryptography_OAEP_Decryption_Failed = "Error occurred while decoding OAEP padding.";

	public const string Cryptography_OpenInvalidHandle = "Cannot open an invalid handle.";

	public const string Cryptography_PrivateKey_DoesNotMatch = "The provided key does not match the public key for this certificate.";

	public const string Cryptography_PrivateKey_WrongAlgorithm = "The provided key does not match the public key algorithm for this certificate.";

	public const string Cryptography_RSA_DecryptWrongSize = "The length of the data to decrypt is not valid for the size of this key.";

	public const string Cryptography_SignHash_WrongSize = "The provided hash value is not the expected size for the specified hash algorithm.";

	public const string Cryptography_Unix_X509_DisallowedStoreNotEmpty = "The Disallowed store is not supported on this platform, but already has data. All files under '{0}' must be removed.";

	public const string Cryptography_Unix_X509_MachineStoresReadOnly = "Unix LocalMachine X509Stores are read-only for all users.";

	public const string Cryptography_Unix_X509_MachineStoresRootOnly = "Unix LocalMachine X509Store is limited to the Root and CertificateAuthority stores.";

	public const string Cryptography_Unix_X509_NoDisallowedStore = "The Disallowed store is not supported on this platform.";

	public const string Cryptography_Unix_X509_PropertyNotSettable = "The {0} value cannot be set on Unix.";

	public const string Cryptography_UnknownKeyAlgorithm = "'{0}' is not a known key algorithm.";

	public const string Cryptography_Unix_X509_SerializedExport = "X509ContentType.SerializedCert and X509ContentType.SerializedStore are not supported on Unix.";

	public const string Cryptography_Unmapped_System_Typed_Error = "The system cryptographic library returned error '{0}' of type '{1}'";

	public const string Cryptography_X509_InvalidFlagCombination = "The flags '{0}' may not be specified together.";

	public const string Cryptography_X509_PKCS7_NoSigner = "Cannot find the original signer.";

	public const string Cryptography_X509_StoreAddFailure = "The X509 certificate could not be added to the store.";

	public const string Cryptography_X509_StoreNoFileAvailable = "The X509 certificate could not be added to the store because all candidate file names were in use.";

	public const string Cryptography_X509_StoreNotFound = "The specified X509 certificate store does not exist.";

	public const string Cryptography_X509_StoreReadOnly = "The X509 certificate store is read-only.";

	public const string Cryptography_X509_StoreCannotCreate = "The platform does not have a definition for an X509 certificate store named '{0}' with a StoreLocation of '{1}', and does not support creating it.";

	public const string NotSupported_ECDsa_Csp = "CryptoApi ECDsa keys are not supported.";

	public const string NotSupported_Export_MultiplePrivateCerts = "Only one certificate with a private key can be exported in a single PFX.";

	public const string NotSupported_LegacyBasicConstraints = "The X509 Basic Constraints extension with OID 2.5.29.10 is not supported.";

	public const string NotSupported_ImmutableX509Certificate = "X509Certificate is immutable on this platform. Use the equivalent constructor instead.";

	public const string Security_AccessDenied = "Access is denied.";

	public const string Cryptography_FileStatusError = "Unable to get file status.";

	public const string Cryptography_InvalidDirectoryPermissions = "Invalid directory permissions. The directory '{0}' must be readable, writable and executable by the owner.";

	public const string Cryptography_OwnerNotCurrentUser = "The owner of '{0}' is not the current user.";

	public const string Cryptography_InvalidFilePermissions = "Invalid file permissions. The file '{0}' must readable and writable by the current owner and by no one else, and the permissions could not be changed to meet that criteria.";

	public const string Cryptography_Invalid_X500Name = "The string contains an invalid X500 name attribute key, oid, value or delimiter.";

	public const string Cryptography_X509_NoEphemeralPfx = "This platform does not support loading with EphemeralKeySet. Remove the flag to allow keys to be temporarily created on disk.";

	public const string Cryptography_X509Store_WouldModifyUserTrust = "Removing the requested certificate would modify user trust settings, and has been denied.";

	public const string Cryptography_X509Store_WouldModifyAdminTrust = "Removing the requested certificate would modify admin trust settings, and has been denied.";

	public const string Cryptography_DSA_KeyGenNotSupported = "DSA keys can be imported, but new key generation is not supported on this platform.";

	public const string Cryptography_InvalidDsaParameters_MissingFields = "The specified DSA parameters are not valid; P, Q, G and Y are all required.";

	public const string Cryptography_InvalidDsaParameters_MismatchedPGY = "The specified DSA parameters are not valid; P, G and Y must be the same length (the key size).";

	public const string Cryptography_InvalidDsaParameters_MismatchedQX = "The specified DSA parameters are not valid; Q and X (if present) must be the same length.";

	public const string Cryptography_InvalidDsaParameters_MismatchedPJ = "The specified DSA parameters are not valid; J (if present) must be shorter than P.";

	public const string Cryptography_InvalidDsaParameters_SeedRestriction_ShortKey = "The specified DSA parameters are not valid; Seed, if present, must be 20 bytes long for keys shorter than 1024 bits.";

	public const string Cryptography_InvalidDsaParameters_QRestriction_ShortKey = "The specified DSA parameters are not valid; Q must be 20 bytes long for keys shorter than 1024 bits.";

	public const string Cryptography_InvalidDsaParameters_QRestriction_LargeKey = "The specified DSA parameters are not valid; Q's length must be one of 20, 32 or 64 bytes.";

	public const string InvalidEmptyArgument = "Argument {0} cannot be zero-length.";

	public const string PlatformNotSupported_CompileToAssembly = "This platform does not support writing compiled regular expressions to an assembly.";

	public const string Parallel_Invoke_ActionNull = "One of the actions was null.";

	public const string Parallel_ForEach_OrderedPartitionerKeysNotNormalized = "This method requires the use of an OrderedPartitioner with the KeysNormalized property set to true.";

	public const string Parallel_ForEach_PartitionerNotDynamic = "The Partitioner used here must support dynamic partitioning.";

	public const string Parallel_ForEach_PartitionerReturnedNull = "The Partitioner used here returned a null partitioner source.";

	public const string Parallel_ForEach_NullEnumerator = "The Partitioner source returned a null enumerator.";

	public const string ParallelState_Break_InvalidOperationException_BreakAfterStop = "Break was called after Stop was called.";

	public const string ParallelState_Stop_InvalidOperationException_StopAfterBreak = "Stop was called after Break was called.";

	public const string ParallelState_NotSupportedException_UnsupportedMethod = "This method is not supported.";

	public const string ArgumentOutOfRange_InvalidThreshold = "The specified threshold for creating dictionary is out of range.";

	public const string Argument_ItemNotExist = "The specified item does not exist in this KeyedCollection.";

	public const string AmbiguousImplementationException_NullMessage = "Ambiguous implementation found.";

	public const string Arg_AccessException = "Cannot access member.";

	public const string Arg_AccessViolationException = "Attempted to read or write protected memory. This is often an indication that other memory is corrupt.";

	public const string Arg_ApplicationException = "Error in the application.";

	public const string Arg_ArgumentException = "Value does not fall within the expected range.";

	public const string Arg_ArithmeticException = "Overflow or underflow in the arithmetic operation.";

	public const string Arg_ArrayTypeMismatchException = "Attempted to access an element as a type incompatible with the array.";

	public const string Arg_ArrayZeroError = "Array must not be of length zero.";

	public const string Arg_BadImageFormatException = "Format of the executable (.exe) or library (.dll) is invalid.";

	public const string Arg_BogusIComparer = "Unable to sort because the IComparer.Compare() method returns inconsistent results. Either a value does not compare equal to itself, or one value repeatedly compared to another value yields different results. IComparer: '{0}'.";

	public const string Arg_CannotBeNaN = "TimeSpan does not accept floating point Not-a-Number values.";

	public const string Arg_CannotHaveNegativeValue = "String cannot contain a minus sign if the base is not 10.";

	public const string Arg_CopyNonBlittableArray = "Arrays must contain only blittable data in order to be copied to unmanaged memory.";

	public const string Arg_CopyOutOfRange = "Requested range extends past the end of the array.";

	public const string Arg_DataMisalignedException = "A datatype misalignment was detected in a load or store instruction.";

	public const string Arg_DateTimeRange = "Combination of arguments to the DateTime constructor is out of the legal range.";

	public const string Arg_DirectoryNotFoundException = "Attempted to access a path that is not on the disk.";

	public const string Arg_DecBitCtor = "Decimal byte array constructor requires an array of length four containing valid decimal bytes.";

	public const string Arg_DivideByZero = "Attempted to divide by zero.";

	public const string Arg_DlgtNullInst = "Delegate to an instance method cannot have null 'this'.";

	public const string Arg_DlgtTypeMis = "Delegates must be of the same type.";

	public const string Arg_DuplicateWaitObjectException = "Duplicate objects in argument.";

	public const string Arg_EHClauseNotFilter = "This ExceptionHandlingClause is not a filter.";

	public const string Arg_EnumAndObjectMustBeSameType = "Object must be the same type as the enum. The type passed in was '{0}'; the enum type was '{1}'.";

	public const string Arg_EntryPointNotFoundException = "Entry point was not found.";

	public const string Arg_EntryPointNotFoundExceptionParameterized = "Unable to find an entry point named '{0}' in DLL '{1}'.";

	public const string Arg_ExecutionEngineException = "Internal error in the runtime.";

	public const string Arg_ExternalException = "External component has thrown an exception.";

	public const string Arg_FieldAccessException = "Attempted to access a field that is not accessible by the caller.";

	public const string Arg_FormatException = "One of the identified items was in an invalid format.";

	public const string Arg_GuidArrayCtor = "Byte array for GUID must be exactly {0} bytes long.";

	public const string Arg_HexStyleNotSupported = "The number style AllowHexSpecifier is not supported on floating point data types.";

	public const string Arg_IndexOutOfRangeException = "Index was outside the bounds of the array.";

	public const string Arg_InsufficientExecutionStackException = "Insufficient stack to continue executing the program safely. This can happen from having too many functions on the call stack or function on the stack using too much stack space.";

	public const string Arg_InvalidBase = "Invalid Base.";

	public const string Arg_InvalidCastException = "Specified cast is not valid.";

	public const string Arg_InvalidHexStyle = "With the AllowHexSpecifier bit set in the enum bit field, the only other valid bits that can be combined into the enum value must be a subset of those in HexNumber.";

	public const string Arg_InvalidOperationException = "Operation is not valid due to the current state of the object.";

	public const string Arg_OleAutDateInvalid = " Not a legal OleAut date.";

	public const string Arg_OleAutDateScale = "OleAut date did not convert to a DateTime correctly.";

	public const string Arg_InvalidRuntimeTypeHandle = "Invalid RuntimeTypeHandle.";

	public const string Arg_IOException = "I/O error occurred.";

	public const string Arg_KeyNotFound = "The given key was not present in the dictionary.";

	public const string Arg_LongerThanSrcString = "Source string was not long enough. Check sourceIndex and count.";

	public const string Arg_LowerBoundsMustMatch = "The arrays' lower bounds must be identical.";

	public const string Arg_MissingFieldException = "Attempted to access a non-existing field.";

	public const string Arg_MethodAccessException = "Attempt to access the method failed.";

	public const string Arg_MissingMemberException = "Attempted to access a missing member.";

	public const string Arg_MissingMethodException = "Attempted to access a missing method.";

	public const string Arg_MulticastNotSupportedException = "Attempted to add multiple callbacks to a delegate that does not support multicast.";

	public const string Arg_MustBeBoolean = "Object must be of type Boolean.";

	public const string Arg_MustBeByte = "Object must be of type Byte.";

	public const string Arg_MustBeChar = "Object must be of type Char.";

	public const string Arg_MustBeDateTime = "Object must be of type DateTime.";

	public const string Arg_MustBeDateTimeOffset = "Object must be of type DateTimeOffset.";

	public const string Arg_MustBeDecimal = "Object must be of type Decimal.";

	public const string Arg_MustBeDouble = "Object must be of type Double.";

	public const string Arg_MustBeEnum = "Type provided must be an Enum.";

	public const string Arg_MustBeGuid = "Object must be of type GUID.";

	public const string Arg_MustBeInt16 = "Object must be of type Int16.";

	public const string Arg_MustBeInt32 = "Object must be of type Int32.";

	public const string Arg_MustBeInt64 = "Object must be of type Int64.";

	public const string Arg_MustBePrimArray = "Object must be an array of primitives.";

	public const string Arg_MustBeSByte = "Object must be of type SByte.";

	public const string Arg_MustBeSingle = "Object must be of type Single.";

	public const string Arg_MustBeStatic = "Method must be a static method.";

	public const string Arg_MustBeString = "Object must be of type String.";

	public const string Arg_MustBeStringPtrNotAtom = "The pointer passed in as a String must not be in the bottom 64K of the process's address space.";

	public const string Arg_MustBeTimeSpan = "Object must be of type TimeSpan.";

	public const string Arg_MustBeUInt16 = "Object must be of type UInt16.";

	public const string Arg_MustBeUInt32 = "Object must be of type UInt32.";

	public const string Arg_MustBeUInt64 = "Object must be of type UInt64.";

	public const string Arg_MustBeVersion = "Object must be of type Version.";

	public const string Arg_NeedAtLeast1Rank = "Must provide at least one rank.";

	public const string Arg_Need2DArray = "Array was not a two-dimensional array.";

	public const string Arg_Need3DArray = "Array was not a three-dimensional array.";

	public const string Arg_NegativeArgCount = "Argument count must not be negative.";

	public const string Arg_NotFiniteNumberException = "Arg_NotFiniteNumberException = Number encountered was not a finite quantity.";

	public const string Arg_NotGenericParameter = "Method may only be called on a Type for which Type.IsGenericParameter is true.";

	public const string Arg_NotImplementedException = "The method or operation is not implemented.";

	public const string Arg_NotSupportedException = "Specified method is not supported.";

	public const string Arg_NotSupportedNonZeroLowerBound = "Arrays with non-zero lower bounds are not supported.";

	public const string Arg_NullReferenceException = "Object reference not set to an instance of an object.";

	public const string Arg_ObjObjEx = "Object of type '{0}' cannot be converted to type '{1}'.";

	public const string Arg_OverflowException = "Arithmetic operation resulted in an overflow.";

	public const string Arg_OutOfMemoryException = "Insufficient memory to continue the execution of the program.";

	public const string Arg_PlatformNotSupported = "Operation is not supported on this platform.";

	public const string Arg_ParamName_Name = "Parameter name: {0}";

	public const string Arg_PathEmpty = "The path is empty.";

	public const string Arg_PathIllegalUNC_Path = "The UNC path '{0}' should be of the form \\\\\\\\server\\\\share.";

	public const string Arg_RankException = "Attempted to operate on an array with the incorrect number of dimensions.";

	public const string Arg_RankIndices = "Indices length does not match the array rank.";

	public const string Arg_RanksAndBounds = "Number of lengths and lowerBounds must match.";

	public const string Arg_RegGetOverflowBug = "RegistryKey.GetValue does not allow a String that has a length greater than Int32.MaxValue.";

	public const string Arg_RegKeyNotFound = "The specified registry key does not exist.";

	public const string Arg_RegInvalidKeyName = "Registry key name must start with a valid base key name.";

	public const string Arg_StackOverflowException = "Operation caused a stack overflow.";

	public const string Arg_SynchronizationLockException = "Object synchronization method was called from an unsynchronized block of code.";

	public const string Arg_SystemException = "System error.";

	public const string Arg_TargetInvocationException = "Exception has been thrown by the target of an invocation.";

	public const string Arg_TargetParameterCountException = "Number of parameters specified does not match the expected number.";

	public const string Arg_DefaultValueMissingException = "Missing parameter does not have a default value.";

	public const string Arg_ThreadStartException = "Thread failed to start.";

	public const string Arg_ThreadStateException = "Thread was in an invalid state for the operation being executed.";

	public const string Arg_TimeoutException = "The operation has timed out.";

	public const string Arg_TypeAccessException = "Attempt to access the type failed.";

	public const string Arg_TypeLoadException = "Failure has occurred while loading a type.";

	public const string Arg_UnauthorizedAccessException = "Attempted to perform an unauthorized operation.";

	public const string Arg_VersionString = "Version string portion was too short or too long.";

	public const string Argument_AbsolutePathRequired = "Absolute path information is required.";

	public const string Argument_AdjustmentRulesNoNulls = "The AdjustmentRule array cannot contain null elements.";

	public const string Argument_AdjustmentRulesOutOfOrder = "The elements of the AdjustmentRule array must be in chronological order and must not overlap.";

	public const string Argument_CodepageNotSupported = "{0} is not a supported code page.";

	public const string Argument_CompareOptionOrdinal = "CompareOption.Ordinal cannot be used with other options.";

	public const string Argument_ConflictingDateTimeRoundtripStyles = "The DateTimeStyles value RoundtripKind cannot be used with the values AssumeLocal, AssumeUniversal or AdjustToUniversal.";

	public const string Argument_ConflictingDateTimeStyles = "The DateTimeStyles values AssumeLocal and AssumeUniversal cannot be used together.";

	public const string Argument_ConversionOverflow = "Conversion buffer overflow.";

	public const string Argument_ConvertMismatch = "The conversion could not be completed because the supplied DateTime did not have the Kind property set correctly.  For example, when the Kind property is DateTimeKind.Local, the source time zone must be TimeZoneInfo.Local.";

	public const string Argument_CultureInvalidIdentifier = "{0} is an invalid culture identifier.";

	public const string Argument_CultureIetfNotSupported = "Culture IETF Name {0} is not a recognized IETF name.";

	public const string Argument_CultureIsNeutral = "Culture ID {0} (0x{0:X4}) is a neutral culture; a region cannot be created from it.";

	public const string Argument_CultureNotSupported = "Culture is not supported.";

	public const string Argument_CustomCultureCannotBePassedByNumber = "Customized cultures cannot be passed by LCID, only by name.";

	public const string Argument_DateTimeBadBinaryData = "The binary data must result in a DateTime with ticks between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.";

	public const string Argument_DateTimeHasTicks = "The supplied DateTime must have the Year, Month, and Day properties set to 1.  The time cannot be specified more precisely than whole milliseconds.";

	public const string Argument_DateTimeHasTimeOfDay = "The supplied DateTime includes a TimeOfDay setting.   This is not supported.";

	public const string Argument_DateTimeIsInvalid = "The supplied DateTime represents an invalid time.  For example, when the clock is adjusted forward, any time in the period that is skipped is invalid.";

	public const string Argument_DateTimeIsNotAmbiguous = "The supplied DateTime is not in an ambiguous time range.";

	public const string Argument_DateTimeKindMustBeUnspecified = "The supplied DateTime must have the Kind property set to DateTimeKind.Unspecified.";

	public const string Argument_DateTimeKindMustBeUnspecifiedOrUtc = "The supplied DateTime must have the Kind property set to DateTimeKind.Unspecified or DateTimeKind.Utc.";

	public const string Argument_DateTimeOffsetInvalidDateTimeStyles = "The DateTimeStyles value 'NoCurrentDateDefault' is not allowed when parsing DateTimeOffset.";

	public const string Argument_DateTimeOffsetIsNotAmbiguous = "The supplied DateTimeOffset is not in an ambiguous time range.";

	public const string Argument_EmptyDecString = "Decimal separator cannot be the empty string.";

	public const string Argument_EmptyName = "Empty name is not legal.";

	public const string Argument_EmptyWaithandleArray = "Waithandle array may not be empty.";

	public const string Argument_EncoderFallbackNotEmpty = "Must complete Convert() operation or call Encoder.Reset() before calling GetBytes() or GetByteCount(). Encoder '{0}' fallback '{1}'.";

	public const string Argument_EncodingConversionOverflowBytes = "The output byte buffer is too small to contain the encoded data, encoding '{0}' fallback '{1}'.";

	public const string Argument_EncodingConversionOverflowChars = "The output char buffer is too small to contain the decoded characters, encoding '{0}' fallback '{1}'.";

	public const string Argument_EncodingNotSupported = "'{0}' is not a supported encoding name. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method.";

	public const string Argument_EnumTypeDoesNotMatch = "The argument type, '{0}', is not the same as the enum type '{1}'.";

	public const string Argument_FallbackBufferNotEmpty = "Cannot change fallback when buffer is not empty. Previous Convert() call left data in the fallback buffer.";

	public const string Argument_InvalidArgumentForComparison = "Type of argument is not compatible with the generic comparer.";

	public const string Argument_InvalidArrayLength = "Length of the array must be {0}.";

	public const string Argument_InvalidCalendar = "Not a valid calendar for the given culture.";

	public const string Argument_InvalidCharSequence = "Invalid Unicode code point found at index {0}.";

	public const string Argument_InvalidCharSequenceNoIndex = "String contains invalid Unicode code points.";

	public const string Argument_InvalidCodePageBytesIndex = "Unable to translate bytes {0} at index {1} from specified code page to Unicode.";

	public const string Argument_InvalidCodePageConversionIndex = "Unable to translate Unicode character \\\\u{0:X4} at index {1} to specified code page.";

	public const string Argument_InvalidCultureName = "Culture name '{0}' is not supported.";

	public const string Argument_InvalidDateTimeKind = "Invalid DateTimeKind value.";

	public const string Argument_InvalidDateTimeStyles = "An undefined DateTimeStyles value is being used.";

	public const string Argument_InvalidDigitSubstitution = "The DigitSubstitution property must be of a valid member of the DigitShapes enumeration. Valid entries include Context, NativeNational or None.";

	public const string Argument_InvalidEnumValue = "The value '{0}' is not valid for this usage of the type {1}.";

	public const string Argument_InvalidGroupSize = "Every element in the value array should be between one and nine, except for the last element, which can be zero.";

	public const string Argument_InvalidHighSurrogate = "Found a high surrogate char without a following low surrogate at index: {0}. The input may not be in this encoding, or may not contain valid Unicode (UTF-16) characters.";

	public const string Argument_InvalidId = "The specified ID parameter '{0}' is not supported.";

	public const string Argument_InvalidLowSurrogate = "Found a low surrogate char without a preceding high surrogate at index: {0}. The input may not be in this encoding, or may not contain valid Unicode (UTF-16) characters.";

	public const string Argument_InvalidNativeDigitCount = "The NativeDigits array must contain exactly ten members.";

	public const string Argument_InvalidNativeDigitValue = "Each member of the NativeDigits array must be a single text element (one or more UTF16 code points) with a Unicode Nd (Number, Decimal Digit) property indicating it is a digit.";

	public const string Argument_InvalidNeutralRegionName = "The region name {0} should not correspond to neutral culture; a specific culture name is required.";

	public const string Argument_InvalidNormalizationForm = "Invalid normalization form.";

	public const string Argument_InvalidREG_TZI_FORMAT = "The REG_TZI_FORMAT structure is corrupt.";

	public const string Argument_InvalidResourceCultureName = "The given culture name '{0}' cannot be used to locate a resource file. Resource filenames must consist of only letters, numbers, hyphens or underscores.";

	public const string Argument_InvalidSerializedString = "The specified serialized string '{0}' is not supported.";

	public const string Argument_InvalidTimeSpanStyles = "An undefined TimeSpanStyles value is being used.";

	public const string Argument_MustBeFalse = "Argument must be initialized to false";

	public const string Argument_MustBeRuntimeType = "Type must be a runtime Type object.";

	public const string Argument_NoEra = "No Era was supplied.";

	public const string Argument_NoRegionInvariantCulture = "There is no region associated with the Invariant Culture (Culture ID: 0x7F).";

	public const string Argument_NotIsomorphic = "Object contains non-primitive or non-blittable data.";

	public const string Argument_OffsetLocalMismatch = "The UTC Offset of the local dateTime parameter does not match the offset argument.";

	public const string Argument_OffsetPrecision = "Offset must be specified in whole minutes.";

	public const string Argument_OffsetOutOfRange = "Offset must be within plus or minus 14 hours.";

	public const string Argument_OffsetUtcMismatch = "The UTC Offset for Utc DateTime instances must be 0.";

	public const string Argument_OneOfCulturesNotSupported = "Culture name {0} or {1} is not supported.";

	public const string Argument_OnlyMscorlib = "Only mscorlib's assembly is valid.";

	public const string Argument_OutOfOrderDateTimes = "The DateStart property must come before the DateEnd property.";

	public const string ArgumentOutOfRange_HugeArrayNotSupported = "Arrays larger than 2GB are not supported.";

	public const string ArgumentOutOfRange_Length = "The specified length exceeds maximum capacity of SecureString.";

	public const string ArgumentOutOfRange_LengthTooLarge = "The specified length exceeds the maximum value of {0}.";

	public const string ArgumentOutOfRange_NeedValidId = "The ID parameter must be in the range {0} through {1}.";

	public const string Argument_InvalidTypeName = "The name of the type is invalid.";

	public const string Argument_PathFormatNotSupported_Path = "The format of the path '{0}' is not supported.";

	public const string Argument_RecursiveFallback = "Recursive fallback not allowed for character \\\\u{0:X4}.";

	public const string Argument_RecursiveFallbackBytes = "Recursive fallback not allowed for bytes {0}.";

	public const string Argument_ResultCalendarRange = "The result is out of the supported range for this calendar. The result should be between {0} (Gregorian date) and {1} (Gregorian date), inclusive.";

	public const string Argument_TimeSpanHasSeconds = "The TimeSpan parameter cannot be specified more precisely than whole minutes.";

	public const string Argument_TimeZoneNotFound = "The time zone ID '{0}' was not found on the local computer.";

	public const string Argument_TimeZoneInfoBadTZif = "The tzfile does not begin with the magic characters 'TZif'.  Please verify that the file is not corrupt.";

	public const string Argument_TimeZoneInfoInvalidTZif = "The TZif data structure is corrupt.";

	public const string Argument_ToExclusiveLessThanFromExclusive = "fromInclusive must be less than or equal to toExclusive.";

	public const string Argument_TransitionTimesAreIdentical = "The DaylightTransitionStart property must not equal the DaylightTransitionEnd property.";

	public const string Argument_UTCOutOfRange = "The UTC time represented when the offset is applied must be between year 0 and 10,000.";

	public const string ArgumentException_OtherNotArrayOfCorrectLength = "Object is not a array with the same number of elements as the array to compare it to.";

	public const string ArgumentException_TupleIncorrectType = "Argument must be of type {0}.";

	public const string ArgumentException_TupleLastArgumentNotATuple = "The last element of an eight element tuple must be a Tuple.";

	public const string ArgumentException_ValueTupleIncorrectType = "Argument must be of type {0}.";

	public const string ArgumentException_ValueTupleLastArgumentNotAValueTuple = "The last element of an eight element ValueTuple must be a ValueTuple.";

	public const string ArgumentNull_ArrayElement = "At least one element in the specified array was null.";

	public const string ArgumentNull_ArrayValue = "Found a null value within an array.";

	public const string ArgumentNull_Generic = "Value cannot be null.";

	public const string ArgumentNull_Obj = "Object cannot be null.";

	public const string ArgumentNull_String = "String reference not set to an instance of a String.";

	public const string ArgumentNull_Type = "Type cannot be null.";

	public const string ArgumentNull_Waithandles = "The waitHandles parameter cannot be null.";

	public const string ArgumentOutOfRange_AddValue = "Value to add was out of range.";

	public const string ArgumentOutOfRange_ActualValue = "Actual value was {0}.";

	public const string ArgumentOutOfRange_BadYearMonthDay = "Year, Month, and Day parameters describe an un-representable DateTime.";

	public const string ArgumentOutOfRange_BadHourMinuteSecond = "Hour, Minute, and Second parameters describe an un-representable DateTime.";

	public const string ArgumentOutOfRange_CalendarRange = "Specified time is not supported in this calendar. It should be between {0} (Gregorian date) and {1} (Gregorian date), inclusive.";

	public const string ArgumentOutOfRange_Capacity = "Capacity exceeds maximum capacity.";

	public const string ArgumentOutOfRange_DateArithmetic = "The added or subtracted value results in an un-representable DateTime.";

	public const string ArgumentOutOfRange_DateTimeBadMonths = "Months value must be between +/-120000.";

	public const string ArgumentOutOfRange_DateTimeBadTicks = "Ticks must be between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.";

	public const string ArgumentOutOfRange_DateTimeBadYears = "Years value must be between +/-10000.";

	public const string ArgumentOutOfRange_Day = "Day must be between 1 and {0} for month {1}.";

	public const string ArgumentOutOfRange_DayOfWeek = "The DayOfWeek enumeration must be in the range 0 through 6.";

	public const string ArgumentOutOfRange_DayParam = "The Day parameter must be in the range 1 through 31.";

	public const string ArgumentOutOfRange_DecimalRound = "Decimal can only round to between 0 and 28 digits of precision.";

	public const string ArgumentOutOfRange_DecimalScale = "Decimal's scale value must be between 0 and 28, inclusive.";

	public const string ArgumentOutOfRange_EndIndexStartIndex = "endIndex cannot be greater than startIndex.";

	public const string ArgumentOutOfRange_Era = "Time value was out of era range.";

	public const string ArgumentOutOfRange_FileTimeInvalid = "Not a valid Win32 FileTime.";

	public const string ArgumentOutOfRange_GetByteCountOverflow = "Too many characters. The resulting number of bytes is larger than what can be returned as an int.";

	public const string ArgumentOutOfRange_GetCharCountOverflow = "Too many bytes. The resulting number of chars is larger than what can be returned as an int.";

	public const string ArgumentOutOfRange_IndexCount = "Index and count must refer to a location within the string.";

	public const string ArgumentOutOfRange_IndexCountBuffer = "Index and count must refer to a location within the buffer.";

	public const string ArgumentOutOfRange_IndexLength = "Index and length must refer to a location within the string.";

	public const string ArgumentOutOfRange_IndexString = "Index was out of range. Must be non-negative and less than the length of the string.";

	public const string ArgumentOutOfRange_InvalidEraValue = "Era value was not valid.";

	public const string ArgumentOutOfRange_InvalidHighSurrogate = "A valid high surrogate character is between 0xd800 and 0xdbff, inclusive.";

	public const string ArgumentOutOfRange_InvalidLowSurrogate = "A valid low surrogate character is between 0xdc00 and 0xdfff, inclusive.";

	public const string ArgumentOutOfRange_InvalidUTF32 = "A valid UTF32 value is between 0x000000 and 0x10ffff, inclusive, and should not include surrogate codepoint values (0x00d800 ~ 0x00dfff).";

	public const string ArgumentOutOfRange_LengthGreaterThanCapacity = "The length cannot be greater than the capacity.";

	public const string ArgumentOutOfRange_ListInsert = "Index must be within the bounds of the List.";

	public const string ArgumentOutOfRange_ListItem = "Index was out of range. Must be non-negative and less than the size of the list.";

	public const string ArgumentOutOfRange_ListRemoveAt = "Index was out of range. Must be non-negative and less than the size of the list.";

	public const string ArgumentOutOfRange_Month = "Month must be between one and twelve.";

	public const string ArgumentOutOfRange_MonthParam = "The Month parameter must be in the range 1 through 12.";

	public const string ArgumentOutOfRange_MustBeNonNegInt32 = "Value must be non-negative and less than or equal to Int32.MaxValue.";

	public const string ArgumentOutOfRange_NeedNonNegOrNegative1 = "Number must be either non-negative and less than or equal to Int32.MaxValue or -1.";

	public const string ArgumentOutOfRange_NegativeCapacity = "Capacity must be positive.";

	public const string ArgumentOutOfRange_NegativeCount = "Count cannot be less than zero.";

	public const string ArgumentOutOfRange_NegativeLength = "Length cannot be less than zero.";

	public const string ArgumentOutOfRange_NoGCLohSizeGreaterTotalSize = "lohSize can't be greater than totalSize";

	public const string ArgumentOutOfRange_OffsetLength = "Offset and length must refer to a position in the string.";

	public const string ArgumentOutOfRange_OffsetOut = "Either offset did not refer to a position in the string, or there is an insufficient length of destination character array.";

	public const string ArgumentOutOfRange_PartialWCHAR = "Pointer startIndex and length do not refer to a valid string.";

	public const string ArgumentOutOfRange_Range = "Valid values are between {0} and {1}, inclusive.";

	public const string ArgumentOutOfRange_RoundingDigits = "Rounding digits must be between 0 and 15, inclusive.";

	public const string ArgumentOutOfRange_SmallMaxCapacity = "MaxCapacity must be one or greater.";

	public const string ArgumentOutOfRange_StartIndex = "StartIndex cannot be less than zero.";

	public const string ArgumentOutOfRange_StartIndexLargerThanLength = "startIndex cannot be larger than length of string.";

	public const string ArgumentOutOfRange_StartIndexLessThanLength = "startIndex must be less than length of string.";

	public const string ArgumentOutOfRange_UtcOffset = "The TimeSpan parameter must be within plus or minus 14.0 hours.";

	public const string ArgumentOutOfRange_UtcOffsetAndDaylightDelta = "The sum of the BaseUtcOffset and DaylightDelta properties must within plus or minus 14.0 hours.";

	public const string ArgumentOutOfRange_Version = "Version's parameters must be greater than or equal to zero.";

	public const string ArgumentOutOfRange_Week = "The Week parameter must be in the range 1 through 5.";

	public const string ArgumentOutOfRange_Year = "Year must be between 1 and 9999.";

	public const string Arithmetic_NaN = "Function does not accept floating point Not-a-Number values.";

	public const string ArrayTypeMismatch_CantAssignType = "Source array type cannot be assigned to destination array type.";

	public const string BadImageFormatException_CouldNotLoadFileOrAssembly = "Could not load file or assembly '{0}'. An attempt was made to load a program with an incorrect format.";

	public const string CollectionCorrupted = "A prior operation on this collection was interrupted by an exception. Collection's state is no longer trusted.";

	public const string Exception_EndOfInnerExceptionStack = "--- End of inner exception stack trace ---";

	public const string Exception_WasThrown = "Exception of type '{0}' was thrown.";

	public const string Format_BadBase64Char = "The input is not a valid Base-64 string as it contains a non-base 64 character, more than two padding characters, or an illegal character among the padding characters.";

	public const string Format_BadBase64CharArrayLength = "Invalid length for a Base-64 char array or string.";

	public const string Format_BadBoolean = "String was not recognized as a valid Boolean.";

	public const string Format_BadFormatSpecifier = "Format specifier '{0}' was invalid.";

	public const string Format_NoFormatSpecifier = "No format specifiers were provided.";

	public const string Format_BadQuote = "Cannot find a matching quote character for the character '{0}'.";

	public const string Format_EmptyInputString = "Input string was either empty or contained only whitespace.";

	public const string Format_GuidHexPrefix = "Expected hex 0x in '{0}'.";

	public const string Format_GuidInvLen = "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).";

	public const string Format_GuidInvalidChar = "Guid string should only contain hexadecimal characters.";

	public const string Format_GuidBrace = "Expected {0xdddddddd, etc}.";

	public const string Format_GuidComma = "Could not find a comma, or the length between the previous token and the comma was zero (i.e., '0x,'etc.).";

	public const string Format_GuidBraceAfterLastNumber = "Could not find a brace, or the length between the previous token and the brace was zero (i.e., '0x,'etc.).";

	public const string Format_GuidDashes = "Dashes are in the wrong position for GUID parsing.";

	public const string Format_GuidEndBrace = "Could not find the ending brace.";

	public const string Format_ExtraJunkAtEnd = "Additional non-parsable characters are at the end of the string.";

	public const string Format_GuidUnrecognized = "Unrecognized Guid format.";

	public const string Format_IndexOutOfRange = "Index (zero based) must be greater than or equal to zero and less than the size of the argument list.";

	public const string Format_InvalidGuidFormatSpecification = "Format String can be only 'D', 'd', 'N', 'n', 'P', 'p', 'B', 'b', 'X' or 'x'.";

	public const string Format_InvalidString = "Input string was not in a correct format.";

	public const string Format_NeedSingleChar = "String must be exactly one character long.";

	public const string Format_NoParsibleDigits = "Could not find any recognizable digits.";

	public const string Format_BadTimeSpan = "String was not recognized as a valid TimeSpan.";

	public const string InsufficientMemory_MemFailPoint = "Insufficient available memory to meet the expected demands of an operation at this time.  Please try again later.";

	public const string InsufficientMemory_MemFailPoint_TooBig = "Insufficient memory to meet the expected demands of an operation, and this system is likely to never satisfy this request.  If this is a 32 bit system, consider booting in 3 GB mode.";

	public const string InsufficientMemory_MemFailPoint_VAFrag = "Insufficient available memory to meet the expected demands of an operation at this time, possibly due to virtual address space fragmentation.  Please try again later.";

	public const string InvalidCast_CannotCastNullToValueType = "Null object cannot be converted to a value type.";

	public const string InvalidCast_DownCastArrayElement = "At least one element in the source array could not be cast down to the destination array type.";

	public const string InvalidCast_FromTo = "Invalid cast from '{0}' to '{1}'.";

	public const string InvalidCast_IConvertible = "Object must implement IConvertible.";

	public const string InvalidCast_StoreArrayElement = "Object cannot be stored in an array of this type.";

	public const string InvalidOperation_Calling = "WinRT Interop has already been initialized and cannot be initialized again.";

	public const string InvalidOperation_DateTimeParsing = "Internal Error in DateTime and Calendar operations.";

	public const string InvalidOperation_HandleIsNotInitialized = "Handle is not initialized.";

	public const string InvalidOperation_IComparerFailed = "Failed to compare two elements in the array.";

	public const string InvalidOperation_NoValue = "Nullable object must have a value.";

	public const string InvalidOperation_NullArray = "The underlying array is null.";

	public const string InvalidOperation_Overlapped_Pack = "Cannot pack a packed Overlapped again.";

	public const string InvalidOperation_ReadOnly = "Instance is read-only.";

	public const string InvalidOperation_ThreadWrongThreadStart = "The thread was created with a ThreadStart delegate that does not accept a parameter.";

	public const string InvalidOperation_UnknownEnumType = "Unknown enum type.";

	public const string InvalidOperation_WriteOnce = "This property has already been set and cannot be modified.";

	public const string InvalidOperation_ArrayCreateInstance_NotARuntimeType = "Array.CreateInstance() can only accept Type objects created by the runtime.";

	public const string InvalidOperation_TooEarly = "Internal Error: This operation cannot be invoked in an eager class constructor.";

	public const string InvalidOperation_NullContext = "Cannot call Set on a null context";

	public const string InvalidOperation_CannotUseAFCOtherThread = "AsyncFlowControl object must be used on the thread where it was created.";

	public const string InvalidOperation_CannotRestoreUnsupressedFlow = "Cannot restore context flow when it is not suppressed.";

	public const string InvalidOperation_CannotSupressFlowMultipleTimes = "Context flow is already suppressed.";

	public const string InvalidOperation_CannotUseAFCMultiple = "AsyncFlowControl object can be used only once to call Undo().";

	public const string InvalidOperation_AsyncFlowCtrlCtxMismatch = "AsyncFlowControl objects can be used to restore flow only on a Context that had its flow suppressed.";

	public const string InvalidOperation_AsyncIOInProgress = "The stream is currently in use by a previous operation on the stream.";

	public const string InvalidProgram_Default = "Common Language Runtime detected an invalid program.";

	public const string InvalidProgram_Specific = "Common Language Runtime detected an invalid program. The body of method '{0}' is invalid.";

	public const string InvalidProgram_Vararg = "Method '{0}' has a variable argument list. Variable argument lists are not supported in .NET Core.";

	public const string InvalidProgram_CallVirtFinalize = "Object.Finalize() can not be called directly. It is only callable by the runtime.";

	public const string InvalidProgram_NativeCallable = "NativeCallable method cannot be called from managed code.";

	public const string InvalidTimeZone_InvalidRegistryData = "The time zone ID '{0}' was found on the local computer, but the registry information was corrupt.";

	public const string InvalidTimeZone_InvalidFileData = "The time zone ID '{0}' was found on the local computer, but the file at '{1}' was corrupt.";

	public const string InvalidTimeZone_InvalidJulianDay = "Invalid Julian day in POSIX strings.";

	public const string InvalidTimeZone_NJulianDayNotSupported = "Julian n day in POSIX strings is not supported.";

	public const string InvalidTimeZone_NoTTInfoStructures = "There are no ttinfo structures in the tzfile.  At least one ttinfo structure is required in order to construct a TimeZoneInfo object.";

	public const string InvalidTimeZone_UnparseablePosixMDateString = "'{0}' is not a valid POSIX-TZ-environment-variable MDate rule.  A valid rule has the format 'Mm.w.d'.";

	public const string IO_DriveNotFound_Drive = "Could not find the drive '{0}'. The drive might not be ready or might not be mapped.";

	public const string IO_FileName_Name = "File name: '{0}'";

	public const string IO_FileLoad = "Could not load the specified file.";

	public const string IO_FileLoad_FileName = "Could not load the file '{0}'.";

	public const string Lazy_CreateValue_NoParameterlessCtorForT = "The lazily-initialized type does not have a public, parameterless constructor.";

	public const string Lazy_ctor_ModeInvalid = "The mode argument specifies an invalid value.";

	public const string Lazy_StaticInit_InvalidOperation = "ValueFactory returned null.";

	public const string Lazy_ToString_ValueNotCreated = "Value is not created.";

	public const string Lazy_Value_RecursiveCallsToValue = "ValueFactory attempted to access the Value property of this instance.";

	public const string MissingConstructor_Name = "Constructor on type '{0}' not found.";

	public const string MustUseCCRewrite = "An assembly (probably '{1}') must be rewritten using the code contracts binary rewriter (CCRewrite) because it is calling Contract.{0} and the CONTRACTS_FULL symbol is defined.  Remove any explicit definitions of the CONTRACTS_FULL symbol from your project and rebuild.  CCRewrite can be downloaded from http://go.microsoft.com/fwlink/?LinkID=169180. \\r\\nAfter the rewriter is installed, it can be enabled in Visual Studio from the project's Properties page on the Code Contracts pane.  Ensure that 'Perform Runtime Contract Checking' is enabled, which will define CONTRACTS_FULL.";

	public const string NotSupported_MaxWaitHandles = "The number of WaitHandles must be less than or equal to 64.";

	public const string NotSupported_NoCodepageData = "No data is available for encoding {0}. For information on defining a custom encoding, see the documentation for the Encoding.RegisterProvider method.";

	public const string NotSupported_StringComparison = "The string comparison type passed in is currently not supported.";

	public const string NotSupported_VoidArray = "Arrays of System.Void are not supported.";

	public const string NotSupported_ByRefLike = "Cannot create boxed ByRef-like values.";

	public const string NotSupported_Type = "Type is not supported.";

	public const string NotSupported_WaitAllSTAThread = "WaitAll for multiple handles on a STA thread is not supported.";

	public const string ObjectDisposed_ObjectName_Name = "Object name: '{0}'.";

	public const string Overflow_Byte = "Value was either too large or too small for an unsigned byte.";

	public const string Overflow_Char = "Value was either too large or too small for a character.";

	public const string Overflow_Double = "Value was either too large or too small for a Double.";

	public const string Overflow_TimeSpanElementTooLarge = "The TimeSpan could not be parsed because at least one of the numeric components is out of range or contains too many digits.";

	public const string Overflow_Duration = "The duration cannot be returned for TimeSpan.MinValue because the absolute value of TimeSpan.MinValue exceeds the value of TimeSpan.MaxValue.";

	public const string Overflow_Int16 = "Value was either too large or too small for an Int16.";

	public const string Overflow_NegateTwosCompNum = "Negating the minimum value of a twos complement number is invalid.";

	public const string Overflow_NegativeUnsigned = "The string was being parsed as an unsigned number and could not have a negative sign.";

	public const string Overflow_SByte = "Value was either too large or too small for a signed byte.";

	public const string Overflow_Single = "Value was either too large or too small for a Single.";

	public const string Overflow_TimeSpanTooLong = "TimeSpan overflowed because the duration is too long.";

	public const string Overflow_UInt16 = "Value was either too large or too small for a UInt16.";

	public const string Rank_MultiDimNotSupported = "Only single dimension arrays are supported here.";

	public const string RuntimeWrappedException = "An object that does not derive from System.Exception has been wrapped in a RuntimeWrappedException.";

	public const string SpinWait_SpinUntil_ArgumentNull = "The condition argument is null.";

	public const string Serialization_CorruptField = "The value of the field '{0}' is invalid.  The serialized data is corrupt.";

	public const string Serialization_InvalidData = "An error occurred while deserializing the object.  The serialized data is corrupt.";

	public const string Serialization_InvalidEscapeSequence = "The serialized data contained an invalid escape sequence '\\\\{0}'.";

	public const string Serialization_InvalidType = "Only system-provided types can be passed to the GetUninitializedObject method. '{0}' is not a valid instance of a type.";

	public const string SpinWait_SpinUntil_TimeoutWrong = "The timeout must represent a value between -1 and Int32.MaxValue, inclusive.";

	public const string Threading_AbandonedMutexException = "The wait completed due to an abandoned mutex.";

	public const string Threading_SemaphoreFullException = "Adding the specified count to the semaphore would cause it to exceed its maximum count.";

	public const string Threading_ThreadInterrupted = "Thread was interrupted from a waiting state.";

	public const string Threading_WaitHandleCannotBeOpenedException = "No handle of the given name exists.";

	public const string Threading_WaitHandleCannotBeOpenedException_InvalidHandle = "A WaitHandle with system-wide name '{0}' cannot be created. A WaitHandle of a different type might have the same name.";

	public const string TimeZoneNotFound_MissingData = "The time zone ID '{0}' was not found on the local computer.";

	public const string TypeInitialization_Default = "Type constructor threw an exception.";

	public const string TypeInitialization_Type = "The type initializer for '{0}' threw an exception.";

	public const string TypeInitialization_Type_NoTypeAvailable = "A type initializer threw an exception. To determine which type, inspect the InnerException's StackTrace property.";

	public const string Verification_Exception = "Operation could destabilize the runtime.";

	public const string Arg_EnumFormatUnderlyingTypeAndObjectMustBeSameType = "Enum underlying type and the object must be same type or object. Type passed in was '{0}'; the enum underlying type was '{1}'.";

	public const string Format_InvalidEnumFormatSpecification = "Format String can be only 'G', 'g', 'X', 'x', 'F', 'f', 'D' or 'd'.";

	public const string Arg_MustBeEnumBaseTypeOrEnum = "The value passed in must be an enum base or an underlying type for an enum, such as an Int32.";

	public const string Arg_EnumUnderlyingTypeAndObjectMustBeSameType = "Enum underlying type and the object must be same type or object must be a String. Type passed in was '{0}'; the enum underlying type was '{1}'.";

	public const string Arg_MustBeType = "Type must be a type provided by the runtime.";

	public const string Arg_MustContainEnumInfo = "Must specify valid information for parsing in the string.";

	public const string Arg_EnumValueNotFound = "Requested value '{0}' was not found.";

	public const string Argument_StringZeroLength = "String cannot be of zero length.";

	public const string Argument_StringFirstCharIsZero = "The first char in the string is the null character.";

	public const string Argument_LongEnvVarValue = "Environment variable name or value is too long.";

	public const string Argument_IllegalEnvVarName = "Environment variable name cannot contain equal character.";

	public const string AssumptionFailed = "Assumption failed.";

	public const string AssumptionFailed_Cnd = "Assumption failed: {0}";

	public const string AssertionFailed = "Assertion failed.";

	public const string AssertionFailed_Cnd = "Assertion failed: {0}";

	public const string PreconditionFailed = "Precondition failed.";

	public const string PreconditionFailed_Cnd = "Precondition failed: {0}";

	public const string PostconditionFailed = "Postcondition failed.";

	public const string PostconditionFailed_Cnd = "Postcondition failed: {0}";

	public const string PostconditionOnExceptionFailed = "Postcondition failed after throwing an exception.";

	public const string PostconditionOnExceptionFailed_Cnd = "Postcondition failed after throwing an exception: {0}";

	public const string InvariantFailed = "Invariant failed.";

	public const string InvariantFailed_Cnd = "Invariant failed: {0}";

	public const string MissingEncodingNameResource = "Could not find a resource entry for the encoding codepage '{0} - {1}'";

	public const string Globalization_cp_1200 = "Unicode";

	public const string Globalization_cp_1201 = "Unicode (Big-Endian)";

	public const string Globalization_cp_12000 = "Unicode (UTF-32)";

	public const string Globalization_cp_12001 = "Unicode (UTF-32 Big-Endian)";

	public const string Globalization_cp_20127 = "US-ASCII";

	public const string Globalization_cp_28591 = "Western European (ISO)";

	public const string Globalization_cp_65000 = "Unicode (UTF-7)";

	public const string Globalization_cp_65001 = "Unicode (UTF-8)";

	public const string DebugAssertBanner = "---- DEBUG ASSERTION FAILED ----";

	public const string DebugAssertLongMessage = "---- Assert Long Message ----";

	public const string DebugAssertShortMessage = "---- Assert Short Message ----";

	public const string InvalidCast_Empty = "Object cannot be cast to Empty.";

	public const string Arg_UnknownTypeCode = "Unknown TypeCode value.";

	public const string Format_BadDatePattern = "Could not determine the order of year, month, and date from '{0}'.";

	public const string Format_BadDateTime = "String '{0}' was not recognized as a valid DateTime.";

	public const string Format_BadDateTimeCalendar = "The DateTime represented by the string '{0}' is not supported in calendar '{1}'.";

	public const string Format_BadDayOfWeek = "String '{0}' was not recognized as a valid DateTime because the day of week was incorrect.";

	public const string Format_DateOutOfRange = "The DateTime represented by the string '{0}' is out of range.";

	public const string Format_MissingIncompleteDate = "There must be at least a partial date with a year present in the input string '{0}'.";

	public const string Format_OffsetOutOfRange = "The time zone offset of string '{0}' must be within plus or minus 14 hours.";

	public const string Format_RepeatDateTimePattern = "DateTime pattern '{0}' appears more than once with different values.";

	public const string Format_UnknownDateTimeWord = "The string '{0}' was not recognized as a valid DateTime. There is an unknown word starting at index '{1}'.";

	public const string Format_UTCOutOfRange = "The UTC representation of the date '{0}' falls outside the year range 1-9999.";

	public const string RFLCT_Ambiguous = "Ambiguous match found.";

	public const string AggregateException_ctor_DefaultMessage = "One or more errors occurred.";

	public const string AggregateException_ctor_InnerExceptionNull = "An element of innerExceptions was null.";

	public const string AggregateException_DeserializationFailure = "The serialization stream contains no inner exceptions.";

	public const string AggregateException_InnerException = "(Inner Exception #{0}) ";

	public const string ArgumentOutOfRange_TimeoutTooLarge = "Time-out interval must be less than 2^32-2.";

	public const string ArgumentOutOfRange_PeriodTooLarge = "Period must be less than 2^32-2.";

	public const string TaskScheduler_FromCurrentSynchronizationContext_NoCurrent = "The current SynchronizationContext may not be used as a TaskScheduler.";

	public const string TaskScheduler_ExecuteTask_WrongTaskScheduler = "ExecuteTask may not be called for a task which was previously queued to a different TaskScheduler.";

	public const string TaskScheduler_InconsistentStateAfterTryExecuteTaskInline = "The TryExecuteTaskInline call to the underlying scheduler succeeded, but the task body was not invoked.";

	public const string TaskSchedulerException_ctor_DefaultMessage = "An exception was thrown by a TaskScheduler.";

	public const string Task_MultiTaskContinuation_FireOptions = "It is invalid to exclude specific continuation kinds for continuations off of multiple tasks.";

	public const string Task_ContinueWith_ESandLR = "The specified TaskContinuationOptions combined LongRunning and ExecuteSynchronously.  Synchronous continuations should not be long running.";

	public const string Task_MultiTaskContinuation_EmptyTaskList = "The tasks argument contains no tasks.";

	public const string Task_MultiTaskContinuation_NullTask = "The tasks argument included a null value.";

	public const string Task_FromAsync_PreferFairness = "It is invalid to specify TaskCreationOptions.PreferFairness in calls to FromAsync.";

	public const string Task_FromAsync_LongRunning = "It is invalid to specify TaskCreationOptions.LongRunning in calls to FromAsync.";

	public const string AsyncMethodBuilder_InstanceNotInitialized = "The builder was not properly initialized.";

	public const string TaskT_TransitionToFinal_AlreadyCompleted = "An attempt was made to transition a task to a final state when it had already completed.";

	public const string TaskT_DebuggerNoResult = "{Not yet computed}";

	public const string OperationCanceled = "The operation was canceled.";

	public const string CancellationToken_CreateLinkedToken_TokensIsEmpty = "No tokens were supplied.";

	public const string CancellationTokenSource_Disposed = "The CancellationTokenSource has been disposed.";

	public const string CancellationToken_SourceDisposed = "The CancellationTokenSource associated with this CancellationToken has been disposed.";

	public const string TaskExceptionHolder_UnknownExceptionType = "(Internal)Expected an Exception or an IEnumerable<Exception>";

	public const string TaskExceptionHolder_UnhandledException = "A Task's exception(s) were not observed either by Waiting on the Task or accessing its Exception property. As a result, the unobserved exception was rethrown by the finalizer thread.";

	public const string Task_Delay_InvalidMillisecondsDelay = "The value needs to be either -1 (signifying an infinite timeout), 0 or a positive integer.";

	public const string Task_Delay_InvalidDelay = "The value needs to translate in milliseconds to -1 (signifying an infinite timeout), 0 or a positive integer less than or equal to Int32.MaxValue.";

	public const string Task_Dispose_NotCompleted = "A task may only be disposed if it is in a completion state (RanToCompletion, Faulted or Canceled).";

	public const string Task_WaitMulti_NullTask = "The tasks array included at least one null element.";

	public const string Task_ContinueWith_NotOnAnything = "The specified TaskContinuationOptions excluded all continuation kinds.";

	public const string Task_RunSynchronously_AlreadyStarted = "RunSynchronously may not be called on a task that was already started.";

	public const string Task_ThrowIfDisposed = "The task has been disposed.";

	public const string Task_RunSynchronously_TaskCompleted = "RunSynchronously may not be called on a task that has already completed.";

	public const string Task_RunSynchronously_Promise = "RunSynchronously may not be called on a task not bound to a delegate, such as the task returned from an asynchronous method.";

	public const string Task_RunSynchronously_Continuation = "RunSynchronously may not be called on a continuation task.";

	public const string Task_Start_AlreadyStarted = "Start may not be called on a task that was already started.";

	public const string Task_Start_ContinuationTask = "Start may not be called on a continuation task.";

	public const string Task_Start_Promise = "Start may not be called on a promise-style task.";

	public const string Task_Start_TaskCompleted = "Start may not be called on a task that has completed.";

	public const string TaskCanceledException_ctor_DefaultMessage = "A task was canceled.";

	public const string TaskCompletionSourceT_TrySetException_NoExceptions = "The exceptions collection was empty.";

	public const string TaskCompletionSourceT_TrySetException_NullException = "The exceptions collection included at least one null element.";

	public const string Argument_MinMaxValue = "'{0}' cannot be greater than {1}.";

	public const string ExecutionContext_ExceptionInAsyncLocalNotification = "An exception was not handled in an AsyncLocal<T> notification callback.";

	public const string InvalidOperation_WrongAsyncResultOrEndCalledMultiple = "Either the IAsyncResult object did not come from the corresponding async method on this type, or the End method was called multiple times with the same IAsyncResult.";

	public const string SpinLock_IsHeldByCurrentThread = "Thread tracking is disabled.";

	public const string SpinLock_TryEnter_LockRecursionException = "The calling thread already holds the lock.";

	public const string SpinLock_Exit_SynchronizationLockException = "The calling thread does not hold the lock.";

	public const string SpinLock_TryReliableEnter_ArgumentException = "The tookLock argument must be set to false before calling this method.";

	public const string SpinLock_TryEnter_ArgumentOutOfRange = "The timeout must be a value between -1 and Int32.MaxValue, inclusive.";

	public const string ManualResetEventSlim_Disposed = "The event has been disposed.";

	public const string ManualResetEventSlim_ctor_SpinCountOutOfRange = "The spinCount argument must be in the range 0 to {0}, inclusive.";

	public const string ManualResetEventSlim_ctor_TooManyWaiters = "There are too many threads currently waiting on the event. A maximum of {0} waiting threads are supported.";

	public const string InvalidOperation_SendNotSupportedOnWindowsRTSynchronizationContext = "Send is not supported in the Windows Runtime SynchronizationContext";

	public const string SemaphoreSlim_Disposed = "The semaphore has been disposed.";

	public const string SemaphoreSlim_Release_CountWrong = "The releaseCount argument must be greater than zero.";

	public const string SemaphoreSlim_Wait_TimeoutWrong = "The timeout must represent a value between -1 and Int32.MaxValue, inclusive.";

	public const string SemaphoreSlim_ctor_MaxCountWrong = "The maximumCount argument must be a positive number. If a maximum is not required, use the constructor without a maxCount parameter.";

	public const string SemaphoreSlim_ctor_InitialCountWrong = "The initialCount argument must be non-negative and less than or equal to the maximumCount.";

	public const string ThreadLocal_ValuesNotAvailable = "The ThreadLocal object is not tracking values. To use the Values property, use a ThreadLocal constructor that accepts the trackAllValues parameter and set the parameter to true.";

	public const string ThreadLocal_Value_RecursiveCallsToValue = "ValueFactory attempted to access the Value property of this instance.";

	public const string ThreadLocal_Disposed = "The ThreadLocal object has been disposed.";

	public const string LockRecursionException_WriteAfterReadNotAllowed = "Write lock may not be acquired with read lock held. This pattern is prone to deadlocks. Please ensure that read locks are released before taking a write lock. If an upgrade is necessary, use an upgrade lock in place of the read lock.";

	public const string LockRecursionException_RecursiveWriteNotAllowed = "Recursive write lock acquisitions not allowed in this mode.";

	public const string LockRecursionException_ReadAfterWriteNotAllowed = "A read lock may not be acquired with the write lock held in this mode.";

	public const string LockRecursionException_RecursiveUpgradeNotAllowed = "Recursive upgradeable lock acquisitions not allowed in this mode.";

	public const string LockRecursionException_RecursiveReadNotAllowed = "Recursive read lock acquisitions not allowed in this mode.";

	public const string SynchronizationLockException_IncorrectDispose = "The lock is being disposed while still being used. It either is being held by a thread and/or has active waiters waiting to acquire the lock.";

	public const string SynchronizationLockException_MisMatchedWrite = "The write lock is being released without being held.";

	public const string LockRecursionException_UpgradeAfterReadNotAllowed = "Upgradeable lock may not be acquired with read lock held.";

	public const string LockRecursionException_UpgradeAfterWriteNotAllowed = "Upgradeable lock may not be acquired with write lock held in this mode. Acquiring Upgradeable lock gives the ability to read along with an option to upgrade to a writer.";

	public const string SynchronizationLockException_MisMatchedUpgrade = "The upgradeable lock is being released without being held.";

	public const string SynchronizationLockException_MisMatchedRead = "The read lock is being released without being held.";

	public const string InvalidOperation_TimeoutsNotSupported = "Timeouts are not supported on this stream.";

	public const string NotSupported_SubclassOverride = "Derived classes must provide an implementation.";

	public const string InvalidOperation_NoPublicRemoveMethod = "Cannot remove the event handler since no public remove method exists for the event.";

	public const string InvalidOperation_NoPublicAddMethod = "Cannot add the event handler since no public add method exists for the event.";

	public const string SerializationException = "Serialization error.";

	public const string Serialization_NotFound = "Member '{0}' was not found.";

	public const string Serialization_OptionalFieldVersionValue = "Version value must be positive.";

	public const string Serialization_SameNameTwice = "Cannot add the same member twice to a SerializationInfo object.";

	public const string NotSupported_AbstractNonCLS = "This non-CLS method is not implemented.";

	public const string NotSupported_NoTypeInfo = "Cannot resolve {0} to a TypeInfo object.";

	public const string Arg_CustomAttributeFormatException = "Binary format of the specified custom attribute was invalid.";

	public const string Argument_InvalidMemberForNamedArgument = "The member must be either a field or a property.";

	public const string Arg_InvalidFilterCriteriaException = "Specified filter criteria was invalid.";

	public const string Arg_ParmArraySize = "Must specify one or more parameters.";

	public const string Arg_MustBePointer = "Type must be a Pointer.";

	public const string Argument_InvalidEnum = "The Enum type should contain one and only one instance field.";

	public const string Argument_MustHaveAttributeBaseClass = "Type passed in must be derived from System.Attribute or System.Attribute itself.";

	public const string InvalidFilterCriteriaException_CritString = "A String must be provided for the filter criteria.";

	public const string InvalidFilterCriteriaException_CritInt = "An Int32 must be provided for the filter criteria.";

	public const string InvalidOperation_NotSupportedOnWinRTEvent = "Adding or removing event handlers dynamically is not supported on WinRT events.";

	public const string PlatformNotSupported_ReflectionOnly = "ReflectionOnly loading is not supported on this platform.";

	public const string PlatformNotSupported_OSXFileLocking = "Locking/unlocking file regions is not supported on this platform. Use FileShare on the entire file instead.";

	public const string PlatformNotSupported_ReflectionEmit = "Dynamic code generation is not supported on this platform.";

	public const string MissingMember_Name = "Member '{0}' not found.";

	public const string MissingMethod_Name = "Method '{0}' not found.";

	public const string MissingField_Name = "Field '{0}' not found.";

	public const string Format_StringZeroLength = "String cannot have zero length.";

	public const string Security_CannotReadFileData = "The time zone ID '{0}' was found on the local computer, but the application does not have permission to read the file.";

	public const string Security_CannotReadRegistryData = "The time zone ID '{0}' was found on the local computer, but the application does not have permission to read the registry information.";

	public const string Security_InvalidAssemblyPublicKey = "Invalid assembly public key.";

	public const string Security_RegistryPermission = "Requested registry access is not allowed.";

	public const string ClassLoad_General = "Could not load type '{0}' from assembly '{1}'.";

	public const string ClassLoad_RankTooLarge = "'{0}' from assembly '{1}' has too many dimensions.";

	public const string ClassLoad_ExplicitGeneric = "Could not load type '{0}' from assembly '{1}' because generic types cannot have explicit layout.";

	public const string ClassLoad_BadFormat = "Could not load type '{0}' from assembly '{1}' because the format is invalid.";

	public const string ClassLoad_ValueClassTooLarge = "Array of type '{0}' from assembly '{1}' cannot be created because base value type is too large.";

	public const string ClassLoad_ExplicitLayout = "Could not load type '{0}' from assembly '{1}' because it contains an object field at offset '{2}' that is incorrectly aligned or overlapped by a non-object field.";

	public const string EE_MissingMethod = "Method not found: '{0}'.";

	public const string EE_MissingField = "Field not found: '{0}'.";

	public const string UnauthorizedAccess_RegistryKeyGeneric_Key = "Access to the registry key '{0}' is denied.";

	public const string UnknownError_Num = "Unknown error '{0}'.";

	public const string Argument_NeedStructWithNoRefs = "The specified Type must be a struct containing no references.";

	public const string ArgumentOutOfRange_AddressSpace = "The number of bytes cannot exceed the virtual address space on a 32 bit machine.";

	public const string ArgumentOutOfRange_UIntPtrMax = "The length of the buffer must be less than the maximum UIntPtr value for your platform.";

	public const string Arg_BufferTooSmall = "Not enough space available in the buffer.";

	public const string InvalidOperation_MustCallInitialize = "You must call Initialize on this object instance before using it.";

	public const string Argument_InvalidSafeBufferOffLen = "Offset and length were greater than the size of the SafeBuffer.";

	public const string Argument_NotEnoughBytesToRead = "There are not enough bytes remaining in the accessor to read at this position.";

	public const string Argument_NotEnoughBytesToWrite = "There are not enough bytes remaining in the accessor to write at this position.";

	public const string Argument_OffsetAndCapacityOutOfBounds = "Offset and capacity were greater than the size of the view.";

	public const string ArgumentOutOfRange_UnmanagedMemStreamLength = "UnmanagedMemoryStream length must be non-negative and less than 2^63 - 1 - baseAddress.";

	public const string Argument_UnmanagedMemAccessorWrapAround = "The UnmanagedMemoryAccessor capacity and offset would wrap around the high end of the address space.";

	public const string ArgumentOutOfRange_StreamLength = "Stream length must be non-negative and less than 2^31 - 1 - origin.";

	public const string ArgumentOutOfRange_UnmanagedMemStreamWrapAround = "The UnmanagedMemoryStream capacity would wrap around the high end of the address space.";

	public const string InvalidOperation_CalledTwice = "The method cannot be called twice on the same instance.";

	public const string IO_FixedCapacity = "Unable to expand length of this stream beyond its capacity.";

	public const string IO_StreamTooLong = "Stream was too long.";

	public const string Arg_BadDecimal = "Read an invalid decimal value from the buffer.";

	public const string NotSupported_Reading = "Accessor does not support reading.";

	public const string NotSupported_UmsSafeBuffer = "This operation is not supported for an UnmanagedMemoryStream created from a SafeBuffer.";

	public const string NotSupported_Writing = "Accessor does not support writing.";

	public const string IndexOutOfRange_UMSPosition = "Unmanaged memory stream position was beyond the capacity of the stream.";

	public const string ObjectDisposed_ViewAccessorClosed = "Cannot access a closed accessor.";

	public const string ArgumentOutOfRange_PositionLessThanCapacityRequired = "The position may not be greater or equal to the capacity of the accessor.";

	public const string Arg_EndOfStreamException = "Attempted to read past the end of the stream.";

	public const string Argument_InvalidHandle = "'handle' has been disposed or is an invalid handle.";

	public const string Argument_AlreadyBoundOrSyncHandle = "'handle' has already been bound to the thread pool, or was not opened for asynchronous I/O.";

	public const string Argument_PreAllocatedAlreadyAllocated = "'preAllocated' is already in use.";

	public const string Argument_NativeOverlappedAlreadyFree = "'overlapped' has already been freed.";

	public const string Argument_NativeOverlappedWrongBoundHandle = "'overlapped' was not allocated by this ThreadPoolBoundHandle instance.";

	public const string NotSupported_FileStreamOnNonFiles = "FileStream was asked to open a device that was not a file. For support for devices like 'com1:' or 'lpt1:', call CreateFile, then use the FileStream constructors that take an OS handle as an IntPtr.";

	public const string Arg_ResourceFileUnsupportedVersion = "The ResourceReader class does not know how to read this version of .resources files.";

	public const string Resources_StreamNotValid = "Stream is not a valid resource file.";

	public const string BadImageFormat_ResourcesHeaderCorrupted = "Corrupt .resources file. Unable to read resources from this file because of invalid header information. Try regenerating the .resources file.";

	public const string BadImageFormat_NegativeStringLength = "Corrupt .resources file. String length must be non-negative.";

	public const string BadImageFormat_ResourcesNameInvalidOffset = "Corrupt .resources file. The Invalid offset into name section is .";

	public const string BadImageFormat_TypeMismatch = "Corrupt .resources file.  The specified type doesn't match the available data in the stream.";

	public const string BadImageFormat_ResourceNameCorrupted_NameIndex = "Corrupt .resources file. The resource name for name index that extends past the end of the stream is ";

	public const string BadImageFormat_ResourcesDataInvalidOffset = "Corrupt .resources file. Invalid offset  into data section is ";

	public const string Format_Bad7BitInt32 = "Too many bytes in what should have been a 7 bit encoded Int32.";

	public const string BadImageFormat_InvalidType = "Corrupt .resources file.  The specified type doesn't exist.";

	public const string ResourceReaderIsClosed = "ResourceReader is closed.";

	public const string Arg_MissingManifestResourceException = "Unable to find manifest resource.";

	public const string UnauthorizedAccess_MemStreamBuffer = "MemoryStream's internal buffer cannot be accessed.";

	public const string NotSupported_MemStreamNotExpandable = "Memory stream is not expandable.";

	public const string ArgumentNull_Stream = "Stream cannot be null.";

	public const string IO_InvalidStringLen_Len = "BinaryReader encountered an invalid string length of {0} characters.";

	public const string ArgumentOutOfRange_BinaryReaderFillBuffer = "The number of bytes requested does not fit into BinaryReader's internal buffer.";

	public const string Serialization_InsufficientDeserializationState = "Insufficient state to deserialize the object. Missing field '{0}'.";

	public const string NotSupported_UnitySerHolder = "The UnitySerializationHolder object is designed to transmit information about other types and is not serializable itself.";

	public const string Serialization_UnableToFindModule = "The given module {0} cannot be found within the assembly {1}.";

	public const string Argument_InvalidUnity = "Invalid Unity type.";

	public const string InvalidOperation_InvalidHandle = "The handle is invalid.";

	public const string PlatformNotSupported_NamedSynchronizationPrimitives = "The named version of this synchronization primitive is not supported on this platform.";

	public const string Overflow_MutexReacquireCount = "The current thread attempted to reacquire a mutex that has reached its maximum acquire count.";

	public const string Serialization_InsufficientState = "Insufficient state to return the real object.";

	public const string Serialization_UnknownMember = "Cannot get the member '{0}'.";

	public const string Serialization_NullSignature = "The method signature cannot be null.";

	public const string Serialization_MemberTypeNotRecognized = "Unknown member type.";

	public const string Serialization_BadParameterInfo = "Non existent ParameterInfo. Position bigger than member's parameters length.";

	public const string Serialization_NoParameterInfo = "Serialized member does not have a ParameterInfo.";

	public const string ArgumentNull_Assembly = "Assembly cannot be null.";

	public const string Arg_InvalidNeutralResourcesLanguage_Asm_Culture = "The NeutralResourcesLanguageAttribute on the assembly \"{0}\" specifies an invalid culture name: \"{1}\".";

	public const string Arg_InvalidNeutralResourcesLanguage_FallbackLoc = "The NeutralResourcesLanguageAttribute specifies an invalid or unrecognized ultimate resource fallback location: \"{0}\".";

	public const string Arg_InvalidSatelliteContract_Asm_Ver = "Satellite contract version attribute on the assembly '{0}' specifies an invalid version: {1}.";

	public const string Arg_ResMgrNotResSet = "Type parameter must refer to a subclass of ResourceSet.";

	public const string BadImageFormat_ResourceNameCorrupted = "Corrupt .resources file. A resource name extends past the end of the stream.";

	public const string BadImageFormat_ResourcesNameTooLong = "Corrupt .resources file. Resource name extends past the end of the file.";

	public const string InvalidOperation_ResMgrBadResSet_Type = "'{0}': ResourceSet derived classes must provide a constructor that takes a String file name and a constructor that takes a Stream.";

	public const string InvalidOperation_ResourceNotStream_Name = "Resource '{0}' was not a Stream - call GetObject instead.";

	public const string MissingManifestResource_MultipleBlobs = "A case-insensitive lookup for resource file \"{0}\" in assembly \"{1}\" found multiple entries. Remove the duplicates or specify the exact case.";

	public const string MissingManifestResource_NoNeutralAsm = "Could not find any resources appropriate for the specified culture or the neutral culture.  Make sure \"{0}\" was correctly embedded or linked into assembly \"{1}\" at compile time, or that all the satellite assemblies required are loadable and fully signed.";

	public const string MissingManifestResource_NoNeutralDisk = "Could not find any resources appropriate for the specified culture (or the neutral culture) on disk.";

	public const string MissingManifestResource_NoPRIresources = "Unable to open Package Resource Index.";

	public const string MissingManifestResource_ResWFileNotLoaded = "Unable to load resources for resource file \"{0}\" in package \"{1}\".";

	public const string MissingSatelliteAssembly_Culture_Name = "The satellite assembly named \"{1}\" for fallback culture \"{0}\" either could not be found or could not be loaded. This is generally a setup problem. Please consider reinstalling or repairing the application.";

	public const string MissingSatelliteAssembly_Default = "Resource lookup fell back to the ultimate fallback resources in a satellite assembly, but that satellite either was not found or could not be loaded. Please consider reinstalling or repairing the application.";

	public const string NotSupported_ObsoleteResourcesFile = "Found an obsolete .resources file in assembly '{0}'. Rebuild that .resources file then rebuild that assembly.";

	public const string NotSupported_ResourceObjectSerialization = "Cannot read resources that depend on serialization.";

	public const string ObjectDisposed_ResourceSet = "Cannot access a closed resource set.";

	public const string Arg_ResourceNameNotExist = "The specified resource name \"{0}\" does not exist in the resource file.";

	public const string BadImageFormat_ResourceDataLengthInvalid = "Corrupt .resources file.  The specified data length '{0}' is not a valid position in the stream.";

	public const string BadImageFormat_ResourcesIndexTooLong = "Corrupt .resources file. String for name index '{0}' extends past the end of the file.";

	public const string InvalidOperation_ResourceNotString_Name = "Resource '{0}' was not a String - call GetObject instead.";

	public const string InvalidOperation_ResourceNotString_Type = "Resource was of type '{0}' instead of String - call GetObject instead.";

	public const string NotSupported_WrongResourceReader_Type = "This .resources file should not be read with this reader. The resource reader type is \"{0}\".";

	public const string Arg_MustBeDelegate = "Type must derive from Delegate.";

	public const string NotSupported_GlobalMethodSerialization = "Serialization of global methods (including implicit serialization via the use of asynchronous delegates) is not supported.";

	public const string NotSupported_DelegateSerHolderSerial = "DelegateSerializationHolder objects are designed to represent a delegate during serialization and are not serializable themselves.";

	public const string DelegateSer_InsufficientMetadata = "The delegate cannot be serialized properly due to missing metadata for the target method.";

	public const string Argument_NoUninitializedStrings = "Uninitialized Strings cannot be created.";

	public const string ArgumentOutOfRangeException_NoGCRegionSizeTooLarge = "totalSize is too large. For more information about setting the maximum size, see \\\"Latency Modes\\\" in http://go.microsoft.com/fwlink/?LinkId=522706.";

	public const string InvalidOperationException_AlreadyInNoGCRegion = "The NoGCRegion mode was already in progress.";

	public const string InvalidOperationException_NoGCRegionAllocationExceeded = "Allocated memory exceeds specified memory for NoGCRegion mode.";

	public const string InvalidOperationException_NoGCRegionInduced = "Garbage collection was induced in NoGCRegion mode.";

	public const string InvalidOperationException_NoGCRegionNotInProgress = "NoGCRegion mode must be set.";

	public const string InvalidOperationException_SetLatencyModeNoGC = "The NoGCRegion mode is in progress. End it and then set a different mode.";

	public const string InvalidOperation_NotWithConcurrentGC = "This API is not available when the concurrent GC is enabled.";

	public const string ThreadState_AlreadyStarted = "Thread is running or terminated; it cannot restart.";

	public const string ThreadState_Dead_Priority = "Thread is dead; priority cannot be accessed.";

	public const string ThreadState_Dead_State = "Thread is dead; state cannot be accessed.";

	public const string ThreadState_NotStarted = "Thread has not been started.";

	public const string ThreadState_SetPriorityFailed = "Unable to set thread priority.";

	public const string Serialization_InvalidFieldState = "Object fields may not be properly initialized.";

	public const string Acc_CreateAbst = "Cannot create an abstract class.";

	public const string Acc_CreateGeneric = "Cannot create a type for which Type.ContainsGenericParameters is true.";

	public const string NotSupported_ManagedActivation = "Cannot create uninitialized instances of types requiring managed activation.";

	public const string PlatformNotSupported_ResourceManager_ResWFileUnsupportedMethod = "ResourceManager method '{0}' is not supported when reading from .resw resource files.";

	public const string PlatformNotSupported_ResourceManager_ResWFileUnsupportedProperty = "ResourceManager property '{0}' is not supported when reading from .resw resource files.";

	public const string Serialization_NonSerType = "Type '{0}' in Assembly '{1}' is not marked as serializable.";

	public const string InvalidCast_DBNull = "Object cannot be cast to DBNull.";

	public const string NotSupported_NYI = "This feature is not currently implemented.";

	public const string Delegate_GarbageCollected = "The corresponding delegate has been garbage collected. Please make sure the delegate is still referenced by managed code when you are using the marshalled native function pointer.";

	public const string Arg_AmbiguousMatchException = "Ambiguous match found.";

	public const string NotSupported_ChangeType = "ChangeType operation is not supported.";

	public const string Arg_EmptyArray = "Array may not be empty.";

	public const string MissingMember = "Member not found.";

	public const string MissingField = "Field not found.";

	public const string InvalidCast_FromDBNull = "Object cannot be cast from DBNull to other types.";

	public const string NotSupported_DBNullSerial = "Only one DBNull instance may exist, and calls to DBNull deserialization methods are not allowed.";

	public const string Serialization_StringBuilderCapacity = "The serialized Capacity property of StringBuilder must be positive, less than or equal to MaxCapacity and greater than or equal to the String length.";

	public const string Serialization_StringBuilderMaxCapacity = "The serialized MaxCapacity property of StringBuilder must be positive and greater than or equal to the String length.";

	public const string PlatformNotSupported_Remoting = "Remoting is not supported on this platform.";

	public const string PlatformNotSupported_StrongNameSigning = "Strong-name signing is not supported on this platform.";

	public const string Serialization_MissingDateTimeData = "Invalid serialized DateTime data. Unable to find 'ticks' or 'dateData'.";

	public const string Serialization_DateTimeTicksOutOfRange = "Invalid serialized DateTime data. Ticks must be between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.";

	public const string FeatureRemoved_Message = "Code to support feature '{0}' was removed during publishing. If this is in error, update the project configuration to not disable feature '{0}'.";

	public const string Arg_InvalidANSIString = "The ANSI string passed in could not be converted from the default ANSI code page to Unicode.";

	public const string PlatformNotSupported_ArgIterator = "ArgIterator is not supported on this platform.";

	public const string Arg_TypeUnloadedException = "Type had been unloaded.";

	public const string Overflow_Currency = "Value was either too large or too small for a Currency.";

	public const string PlatformNotSupported_SecureBinarySerialization = "Secure binary serialization is not supported on this platform.";

	public const string Serialization_InvalidPtrValue = "An IntPtr or UIntPtr with an eight byte value cannot be deserialized on a machine with a four byte word size.";

	public const string EventSource_AbstractMustNotDeclareEventMethods = "Abstract event source must not declare event methods ({0} with ID {1}).";

	public const string EventSource_AbstractMustNotDeclareKTOC = "Abstract event source must not declare {0} nested type.";

	public const string EventSource_AddScalarOutOfRange = "Getting out of bounds during scalar addition.";

	public const string EventSource_BadHexDigit = "Bad Hexidecimal digit \"{0}\".";

	public const string EventSource_ChannelTypeDoesNotMatchEventChannelValue = "Channel {0} does not match event channel value {1}.";

	public const string EventSource_DataDescriptorsOutOfRange = "Data descriptors are out of range.";

	public const string EventSource_DuplicateStringKey = "Multiple definitions for string \"{0}\".";

	public const string EventSource_EnumKindMismatch = "The type of {0} is not expected in {1}.";

	public const string EventSource_EvenHexDigits = "Must have an even number of Hexidecimal digits.";

	public const string EventSource_EventChannelOutOfRange = "Channel {0} has a value of {1} which is outside the legal range (16-254).";

	public const string EventSource_EventIdReused = "Event {0} has ID {1} which is already in use.";

	public const string EventSource_EventMustHaveTaskIfNonDefaultOpcode = "Event {0} (with ID {1}) has a non-default opcode but not a task.";

	public const string EventSource_EventMustNotBeExplicitImplementation = "Event method {0} (with ID {1}) is an explicit interface method implementation. Re-write method as implicit implementation.";

	public const string EventSource_EventNameDoesNotEqualTaskPlusOpcode = "Event {0} (with ID {1}) has a name that is not the concatenation of its task name and opcode.";

	public const string EventSource_EventNameReused = "Event name {0} used more than once.  If you wish to overload a method, the overloaded method should have a NonEvent attribute.";

	public const string EventSource_EventParametersMismatch = "Event {0} was called with {1} argument(s), but it is defined with {2} parameter(s).";

	public const string EventSource_EventSourceGuidInUse = "An instance of EventSource with Guid {0} already exists.";

	public const string EventSource_EventTooBig = "The payload for a single event is too large.";

	public const string EventSource_EventWithAdminChannelMustHaveMessage = "Event {0} specifies an Admin channel {1}. It must specify a Message property.";

	public const string EventSource_IllegalKeywordsValue = "Keyword {0} has a value of {1} which is outside the legal range (0-0x0000080000000000).";

	public const string EventSource_IllegalOpcodeValue = "Opcode {0} has a value of {1} which is outside the legal range (11-238).";

	public const string EventSource_IllegalTaskValue = "Task {0} has a value of {1} which is outside the legal range (1-65535).";

	public const string EventSource_IllegalValue = "Illegal value \"{0}\" (prefix strings with @ to indicate a literal string).";

	public const string EventSource_IncorrentlyAuthoredTypeInfo = "Incorrectly-authored TypeInfo - a type should be serialized as one field or as one group";

	public const string EventSource_InvalidCommand = "Invalid command value.";

	public const string EventSource_InvalidEventFormat = "Can't specify both etw event format flags.";

	public const string EventSource_KeywordCollision = "Keywords {0} and {1} are defined with the same value ({2}).";

	public const string EventSource_KeywordNeedPowerOfTwo = "Value {0} for keyword {1} needs to be a power of 2.";

	public const string EventSource_ListenerCreatedInsideCallback = "Creating an EventListener inside a EventListener callback.";

	public const string EventSource_ListenerNotFound = "Listener not found.";

	public const string EventSource_ListenerWriteFailure = "An error occurred when writing to a listener.";

	public const string EventSource_MaxChannelExceeded = "Attempt to define more than the maximum limit of 8 channels for a provider.";

	public const string EventSource_MismatchIdToWriteEvent = "Event {0} was assigned event ID {1} but {2} was passed to WriteEvent.";

	public const string EventSource_NeedGuid = "The Guid of an EventSource must be non zero.";

	public const string EventSource_NeedName = "The name of an EventSource must not be null.";

	public const string EventSource_NeedPositiveId = "Event IDs must be positive integers.";

	public const string EventSource_NoFreeBuffers = "No Free Buffers available from the operating system (e.g. event rate too fast).";

	public const string EventSource_NonCompliantTypeError = "The API supports only anonymous types or types decorated with the EventDataAttribute. Non-compliant type: {0} dataType.";

	public const string EventSource_NoRelatedActivityId = "EventSource expects the first parameter of the Event method to be of type Guid and to be named \"relatedActivityId\" when calling WriteEventWithRelatedActivityId.";

	public const string EventSource_NotSupportedArrayOfBinary = "Arrays of Binary are not supported.";

	public const string EventSource_NotSupportedArrayOfNil = "Arrays of Nil are not supported.";

	public const string EventSource_NotSupportedArrayOfNullTerminatedString = "Arrays of null-terminated string are not supported.";

	public const string EventSource_NotSupportedCustomSerializedData = "Enumerables of custom-serialized data are not supported";

	public const string EventSource_NotSupportedNestedArraysEnums = "Nested arrays/enumerables are not supported.";

	public const string EventSource_NullInput = "Null passed as a event argument.";

	public const string EventSource_OpcodeCollision = "Opcodes {0} and {1} are defined with the same value ({2}).";

	public const string EventSource_PinArrayOutOfRange = "Pins are out of range.";

	public const string EventSource_RecursiveTypeDefinition = "Recursive type definition is not supported.";

	public const string EventSource_SessionIdError = "Bit position in AllKeywords ({0}) must equal the command argument named \"EtwSessionKeyword\" ({1}).";

	public const string EventSource_StopsFollowStarts = "An event with stop suffix must follow a corresponding event with a start suffix.";

	public const string EventSource_TaskCollision = "Tasks {0} and {1} are defined with the same value ({2}).";

	public const string EventSource_TaskOpcodePairReused = "Event {0} (with ID {1}) has the same task/opcode pair as event {2} (with ID {3}).";

	public const string EventSource_TooManyArgs = "Too many arguments.";

	public const string EventSource_TooManyFields = "Too many fields in structure.";

	public const string EventSource_ToString = "EventSource({0}, {1})";

	public const string EventSource_TraitEven = "There must be an even number of trait strings (they are key-value pairs).";

	public const string EventSource_TypeMustBeSealedOrAbstract = "Event source types must be sealed or abstract.";

	public const string EventSource_TypeMustDeriveFromEventSource = "Event source types must derive from EventSource.";

	public const string EventSource_UndefinedChannel = "Use of undefined channel value {0} for event {1}.";

	public const string EventSource_UndefinedKeyword = "Use of undefined keyword value {0} for event {1}.";

	public const string EventSource_UndefinedOpcode = "Use of undefined opcode value {0} for event {1}.";

	public const string EventSource_UnknownEtwTrait = "Unknown ETW trait \"{0}\".";

	public const string EventSource_UnsupportedEventTypeInManifest = "Unsupported type {0} in event source.";

	public const string EventSource_UnsupportedMessageProperty = "Event {0} specifies an illegal or unsupported formatting message (\"{1}\").";

	public const string EventSource_VarArgsParameterMismatch = "The parameters to the Event method do not match the parameters to the WriteEvent method. This may cause the event to be displayed incorrectly.";

	public const string Arg_SurrogatesNotAllowedAsSingleChar = "Unicode surrogate characters must be written out as pairs together in the same call, not individually. Consider passing in a character array instead.";

	public const string CustomAttributeFormat_InvalidFieldFail = "'{0}' field specified was not found.";

	public const string CustomAttributeFormat_InvalidPropertyFail = "'{0}' property specified was not found.";

	public const string ArrayTypeMismatch_ConstrainedCopy = "Array.ConstrainedCopy will only work on array types that are provably compatible, without any form of boxing, unboxing, widening, or casting of each array element.  Change the array types (i.e., copy a Derived[] to a Base[]), or use a mitigation strategy in the CER for Array.Copy's less powerful reliability contract, such as cloning the array or throwing away the potentially corrupt destination array.";

	public const string Arg_DllNotFoundException = "Dll was not found.";

	public const string Arg_DllNotFoundExceptionParameterized = "Unable to load DLL '{0}': The specified module could not be found.";

	public const string Arg_DriveNotFoundException = "Attempted to access a drive that is not available.";

	public const string WrongSizeArrayInNStruct = "Type could not be marshaled because the length of an embedded array instance does not match the declared length in the layout.";

	public const string Arg_InteropMarshalUnmappableChar = "Cannot marshal: Encountered unmappable character.";

	public const string Arg_MarshalDirectiveException = "Marshaling directives are invalid.";

	public const string Arg_RegSubKeyValueAbsent = "No value exists with that name.";

	public const string Arg_RegValStrLenBug = "Registry value names should not be greater than 16,383 characters.";

	public const string Serialization_DelegatesNotSupported = "Serializing delegates is not supported on this platform.";

	public const string Arg_OpenType = "Cannot create an instance of {0} as it is an open type.";

	public const string Arg_PlatformNotSupported_AssemblyName_GetAssemblyName = "AssemblyName.GetAssemblyName() is not supported on this platform.";

	public const string NotSupported_OpenType = "Cannot create arrays of open type.";

	public const string NotSupported_ByRefLikeArray = "Cannot create arrays of ByRef-like values.";

	public const string StackTrace_AtWord = "   at ";

	public const string StackTrace_EndStackTraceFromPreviousThrow = "--- End of stack trace from previous location where exception was thrown ---";

	public const string InvalidAssemblyName = "The given assembly name or codebase was invalid";

	public const string Argument_HasToBeArrayClass = "Must be an array type.";

	public const string Argument_IdnBadBidi = "Left to right characters may not be mixed with right to left characters in IDN labels.";

	public const string Argument_IdnBadLabelSize = "IDN labels must be between 1 and 63 characters long.";

	public const string Argument_IdnBadNameSize = "IDN names must be between 1 and {0} characters long.";

	public const string Argument_IdnBadPunycode = "Invalid IDN encoded string.";

	public const string Argument_IdnBadStd3 = "Label contains character '{0}' not allowed with UseStd3AsciiRules";

	public const string Argument_IdnIllegalName = "Decoded string is not a valid IDN name.";

	public const string InvalidOperation_NotGenericType = "This operation is only valid on generic types.";

	public const string NotSupported_SignatureType = "This method is not supported on signature types.";

	public const string Memory_OutstandingReferences = "Release all references before disposing this instance.";

	public const string HashCode_HashCodeNotSupported = "HashCode is a mutable struct and should not be compared with other HashCodes. Use ToHashCode to retrieve the computed hash code.";

	public const string HashCode_EqualityNotSupported = "HashCode is a mutable struct and should not be compared with other HashCodes.";

	public const string IO_InvalidReadLength = "The read operation returned an invalid length.";

	public const string Arg_BasePathNotFullyQualified = "Basepath argument is not fully qualified.";

	public const string NullReference_InvokeNullRefReturned = "The target method returned a null reference.";

	public const string Thread_Operation_RequiresCurrentThread = "This operation must be performed on the same thread as that represented by the Thread instance.";

	public const string InvalidOperation_WrongAsyncResultOrEndReadCalledMultiple = "Either the IAsyncResult object did not come from the corresponding async method on this type, or EndRead was called multiple times with the same IAsyncResult.";

	public const string InvalidOperation_WrongAsyncResultOrEndWriteCalledMultiple = "Either the IAsyncResult object did not come from the corresponding async method on this type, or EndWrite was called multiple times with the same IAsyncResult.";

	public const string ArgumentOutOfRange_Week_ISO = "The week parameter must be in the range 1 through 53.";

	public const string net_uri_BadAuthority = "Invalid URI: The Authority/Host could not be parsed.";

	public const string net_uri_BadAuthorityTerminator = "Invalid URI: The Authority/Host cannot end with a backslash character ('\\\\').";

	public const string net_uri_BadFormat = "Invalid URI: The format of the URI could not be determined.";

	public const string net_uri_NeedFreshParser = "The URI parser instance passed into 'uriParser' parameter is already registered with the scheme name '{0}'.";

	public const string net_uri_AlreadyRegistered = "A URI scheme name '{0}' already has a registered custom parser.";

	public const string net_uri_BadHostName = "Invalid URI: The hostname could not be parsed.";

	public const string net_uri_BadPort = "Invalid URI: Invalid port specified.";

	public const string net_uri_BadScheme = "Invalid URI: The URI scheme is not valid.";

	public const string net_uri_BadString = "Invalid URI: There is an invalid sequence in the string.";

	public const string net_uri_BadUserPassword = "Invalid URI: The username:password construct is badly formed.";

	public const string net_uri_CannotCreateRelative = "A relative URI cannot be created because the 'uriString' parameter represents an absolute URI.";

	public const string net_uri_SchemeLimit = "Invalid URI: The Uri scheme is too long.";

	public const string net_uri_EmptyUri = "Invalid URI: The URI is empty.";

	public const string net_uri_InvalidUriKind = "The value '{0}' passed for the UriKind parameter is invalid.";

	public const string net_uri_MustRootedPath = "Invalid URI: A Dos path must be rooted, for example, 'c:\\\\'.";

	public const string net_uri_NotAbsolute = "This operation is not supported for a relative URI.";

	public const string net_uri_PortOutOfRange = "A derived type '{0}' has reported an invalid value for the Uri port '{1}'.";

	public const string net_uri_SizeLimit = "Invalid URI: The Uri string is too long.";

	public const string net_uri_UserDrivenParsing = "A derived type '{0}' is responsible for parsing this Uri instance. The base implementation must not be used.";

	public const string net_uri_NotJustSerialization = "UriComponents.SerializationInfoString must not be combined with other UriComponents.";

	public const string net_uri_BadUnicodeHostForIdn = "An invalid Unicode character by IDN standards was specified in the host.";

	public const string Argument_ExtraNotValid = "Extra portion of URI not valid.";

	public const string Argument_InvalidUriSubcomponent = "The subcomponent, {0}, of this uri is not valid.";

	public const string AccessControl_InvalidHandle = "The supplied handle is invalid. This can happen when trying to set an ACL on an anonymous kernel object.";

	public const string Arg_RegSubKeyAbsent = "Cannot delete a subkey tree because the subkey does not exist.";

	public const string Arg_RegKeyDelHive = "Cannot delete a registry hive's subtree.";

	public const string Arg_RegKeyNoRemoteConnect = "No remote connection to '{0}' while trying to read the registry.";

	public const string Arg_RegKeyOutOfRange = "Registry HKEY was out of the legal range.";

	public const string Arg_RegKeyStrLenBug = "Registry key names should not be greater than 255 characters.";

	public const string Arg_RegBadKeyKind = "The specified RegistryValueKind is an invalid value.";

	public const string Arg_RegSetMismatchedKind = "The type of the value object did not match the specified RegistryValueKind or the object could not be properly converted.";

	public const string Arg_RegSetBadArrType = "RegistryKey.SetValue does not support arrays of type '{0}'. Only Byte[] and String[] are supported.";

	public const string Arg_RegSetStrArrNull = "RegistryKey.SetValue does not allow a String[] that contains a null String reference.";

	public const string Arg_DllInitFailure = "One machine may not have remote administration enabled, or both machines may not be running the remote registry service.";

	public const string Argument_InvalidRegistryOptionsCheck = "The specified RegistryOptions value is invalid.";

	public const string Argument_InvalidRegistryViewCheck = "The specified RegistryView value is invalid.";

	public const string Argument_InvalidRegistryKeyPermissionCheck = "The specified RegistryKeyPermissionCheck value is invalid.";

	public const string InvalidOperation_RegRemoveSubKey = "Registry key has subkeys and recursive removes are not supported by this method.";

	public const string ObjectDisposed_RegKeyClosed = "Cannot access a closed registry key.";

	public const string PlatformNotSupported_Registry = "Registry is not supported on this platform.";

	public const string UnauthorizedAccess_RegistryNoWrite = "Cannot write to the registry key.";

	public const string Cryptography_ArgECDHKeySizeMismatch = "The keys from both parties must be the same size to generate a secret agreement.";

	public const string Cryptography_ArgECDHRequiresECDHKey = "Keys used with the ECDiffieHellmanCng algorithm must have an algorithm group of ECDiffieHellman.";

	public const string Cryptography_TlsRequiresLabelAndSeed = "The TLS key derivation function requires both the label and seed properties to be set.";

	public const string Cryptography_TlsRequires64ByteSeed = "The TLS key derivation function requires a seed value of exactly 64 bytes.";

	public const string Cryptography_Config_EncodedOIDError = "Encoded OID length is too large (greater than 0x7f bytes).";

	public const string Cryptography_ECXmlSerializationFormatRequired = "XML serialization of an elliptic curve key requires using an overload which specifies the XML format to be used.";

	public const string Cryptography_InvalidCurveOid = "The specified Oid is not valid. The Oid.FriendlyName or Oid.Value property must be set.";

	public const string Cryptography_InvalidCurveKeyParameters = "The specified key parameters are not valid. Q.X and Q.Y are required fields. Q.X, Q.Y must be the same length. If D is specified it must be the same length as Q.X and Q.Y for named curves or the same length as Order for explicit curves.";

	public const string Cryptography_InvalidECCharacteristic2Curve = "The specified Characteristic2 curve parameters are not valid. Polynomial, A, B, G.X, G.Y, and Order are required. A, B, G.X, G.Y must be the same length, and the same length as Q.X, Q.Y and D if those are specified. Seed, Cofactor and Hash are optional. Other parameters are not allowed.";

	public const string Cryptography_InvalidECPrimeCurve = "The specified prime curve parameters are not valid. Prime, A, B, G.X, G.Y and Order are required and must be the same length, and the same length as Q.X, Q.Y and D if those are specified. Seed, Cofactor and Hash are optional. Other parameters are not allowed.";

	public const string Cryptography_InvalidECNamedCurve = "The specified named curve parameters are not valid. Only the Oid parameter must be set.";

	public const string Cryptography_InvalidKey_SemiWeak = "Specified key is a known semi-weak key for '{0}' and cannot be used.";

	public const string Cryptography_InvalidKey_Weak = "Specified key is a known weak key for '{0}' and cannot be used.";

	public const string Cryptography_InvalidOperation = "This operation is not supported for this class.";

	public const string Cryptography_InvalidPadding = "Padding is invalid and cannot be removed.";

	public const string Cryptography_MissingIV = "The cipher mode specified requires that an initialization vector (IV) be used.";

	public const string Cryptography_MissingKey = "No asymmetric key object has been associated with this formatter object.";

	public const string Cryptography_MissingOID = "Required object identifier (OID) cannot be found.";

	public const string Cryptography_MustTransformWholeBlock = "TransformBlock may only process bytes in block sized increments.";

	public const string Cryptography_NotValidPrivateKey = "Key is not a valid private key.";

	public const string Cryptography_NotValidPublicOrPrivateKey = "Key is not a valid public or private key.";

	public const string Cryptography_PartialBlock = "The input data is not a complete block.";

	public const string Cryptography_PasswordDerivedBytes_FewBytesSalt = "Salt is not at least eight bytes.";

	public const string Cryptography_RC2_EKS40 = "EffectiveKeySize value must be at least 40 bits.";

	public const string Cryptography_RC2_EKSKS = "KeySize value must be at least as large as the EffectiveKeySize value.";

	public const string Cryptography_RC2_EKSKS2 = "EffectiveKeySize must be the same as KeySize in this implementation.";

	public const string Cryptography_Rijndael_BlockSize = "BlockSize must be 128 in this implementation.";

	public const string Cryptography_TransformBeyondEndOfBuffer = "Attempt to transform beyond end of buffer.";

	public const string Cryptography_CipherModeNotSupported = "The specified CipherMode '{0}' is not supported.";

	public const string Cryptography_UnknownPaddingMode = "Unknown padding mode used.";

	public const string Cryptography_UnexpectedTransformTruncation = "CNG provider unexpectedly terminated encryption or decryption prematurely.";

	public const string Cryptography_UnsupportedPaddingMode = "The specified PaddingMode is not supported.";

	public const string NotSupported_Method = "Method not supported.";

	public const string Cryptography_AlgorithmTypesMustBeVisible = "Algorithms added to CryptoConfig must be accessable from outside their assembly.";

	public const string Cryptography_AddNullOrEmptyName = "CryptoConfig cannot add a mapping for a null or empty name.";

	public const string ArgumentOutOfRange_ConsoleKey = "Console key values must be between 0 and 255 inclusive.";

	public const string Arg_InvalidComObjectException = "Attempt has been made to use a COM object that does not have a backing class factory.";

	public const string Arg_MustBeNullTerminatedString = "The string must be null-terminated.";

	public const string Arg_InvalidOleVariantTypeException = "Specified OLE variant was invalid.";

	public const string Arg_SafeArrayRankMismatchException = "Specified array was not of the expected rank.";

	public const string Arg_SafeArrayTypeMismatchException = "Specified array was not of the expected type.";

	public const string TypeNotDelegate = "'Type '{0}' is not a delegate type.  EventTokenTable may only be used with delegate types.'";

	public const string InvalidOperationException_ActorGraphCircular = "An Actor must not create a circular reference between itself (or one of its child Actors) and one of its parents.";

	public const string InvalidOperation_ClaimCannotBeRemoved = "The Claim '{0}' was not able to be removed.  It is either not part of this Identity or it is a Claim that is owned by the Principal that contains this Identity. For example, the Principal will own the Claim when creating a GenericPrincipal with roles. The roles will be exposed through the Identity that is passed in the constructor, but not actually owned by the Identity.  Similar logic exists for a RolePrincipal.";

	public const string PlatformNotSupported_Serialization = "This instance contains state that cannot be serialized and deserialized on this platform.";

	public const string PrivilegeNotHeld_Default = "The process does not possess some privilege required for this operation.";

	public const string PrivilegeNotHeld_Named = "The process does not possess the '{0}' privilege which is required for this operation.";

	public const string CountdownEvent_Decrement_BelowZero = "Invalid attempt made to decrement the event's count below zero.";

	public const string CountdownEvent_Increment_AlreadyZero = "The event is already signaled and cannot be incremented.";

	public const string CountdownEvent_Increment_AlreadyMax = "The increment operation would cause the CurrentCount to overflow.";

	public const string ArrayWithOffsetOverflow = "ArrayWithOffset: offset exceeds array size.";

	public const string Arg_NotIsomorphic = "Object contains non-primitive or non-blittable data.";

	public const string StructArrayTooLarge = "Array size exceeds addressing limitations.";

	public const string IO_DriveNotFound = "Could not find the drive. The drive might not be ready or might not be mapped.";

	public const string Argument_MustSupplyParent = "When supplying the ID of a containing object, the FieldInfo that identifies the current field within that object must also be supplied.";

	public const string Argument_MemberAndArray = "Cannot supply both a MemberInfo and an Array to indicate the parent of a value type.";

	public const string Argument_MustSupplyContainer = "When supplying a FieldInfo for fixing up a nested type, a valid ID for that containing object must also be supplied.";

	public const string Serialization_NoID = "Object has never been assigned an objectID";

	public const string Arg_SwitchExpressionException = "Non-exhaustive switch expression failed to match its input.";

	public const string SwitchExpressionException_UnmatchedValue = "Unmatched value was {0}.";

	public const string Argument_InvalidRandomRange = "Range of random number does not contain at least one possibility.";

	public const string BufferWriterAdvancedTooFar = "Cannot advance past the end of the buffer, which has a size of {0}.";

	public const string net_gssapi_operation_failed_detailed_majoronly = "GSSAPI operation failed with error - {0}.";

	public const string net_gssapi_operation_failed_majoronly = "SSAPI operation failed with status: {0}.";

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

	internal static string Format(string resourceFormat, params object[] args)
	{
		if (args != null)
		{
			return string.Format(CultureInfo.InvariantCulture, resourceFormat, args);
		}
		return resourceFormat;
	}

	internal static string Format(string resourceFormat, object p1)
	{
		return string.Format(CultureInfo.InvariantCulture, resourceFormat, p1);
	}

	internal static string Format(string resourceFormat, object p1, object p2)
	{
		return string.Format(CultureInfo.InvariantCulture, resourceFormat, p1, p2);
	}

	internal static string Format(CultureInfo ci, string resourceFormat, object p1, object p2)
	{
		return string.Format(ci, resourceFormat, p1, p2);
	}

	internal static string Format(string resourceFormat, object p1, object p2, object p3)
	{
		return string.Format(CultureInfo.InvariantCulture, resourceFormat, p1, p2, p3);
	}

	internal static string GetResourceString(string str)
	{
		return str;
	}

	public static object GetObject(string name)
	{
		return name;
	}
}
