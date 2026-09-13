using System.Security;
using System.Threading;
using System.Xml;

namespace System.Runtime.Serialization.Json;

internal class JsonCollectionDataContract : JsonDataContract
{
	private class JsonCollectionDataContractCriticalHelper : JsonDataContractCriticalHelper
	{
		private JsonFormatCollectionReaderDelegate jsonFormatReaderDelegate;

		private JsonFormatGetOnlyCollectionReaderDelegate jsonFormatGetOnlyReaderDelegate;

		private JsonFormatCollectionWriterDelegate jsonFormatWriterDelegate;

		private CollectionDataContract traditionalCollectionDataContract;

		internal JsonFormatCollectionReaderDelegate JsonFormatReaderDelegate
		{
			get
			{
				return jsonFormatReaderDelegate;
			}
			set
			{
				jsonFormatReaderDelegate = value;
			}
		}

		internal JsonFormatGetOnlyCollectionReaderDelegate JsonFormatGetOnlyReaderDelegate
		{
			get
			{
				return jsonFormatGetOnlyReaderDelegate;
			}
			set
			{
				jsonFormatGetOnlyReaderDelegate = value;
			}
		}

		internal JsonFormatCollectionWriterDelegate JsonFormatWriterDelegate
		{
			get
			{
				return jsonFormatWriterDelegate;
			}
			set
			{
				jsonFormatWriterDelegate = value;
			}
		}

		internal CollectionDataContract TraditionalCollectionDataContract => traditionalCollectionDataContract;

		public JsonCollectionDataContractCriticalHelper(CollectionDataContract traditionalDataContract)
			: base(traditionalDataContract)
		{
			traditionalCollectionDataContract = traditionalDataContract;
		}
	}

	[SecurityCritical]
	private JsonCollectionDataContractCriticalHelper helper;

	internal JsonFormatCollectionReaderDelegate JsonFormatReaderDelegate
	{
		[SecuritySafeCritical]
		get
		{
			if (helper.JsonFormatReaderDelegate == null)
			{
				lock (this)
				{
					if (helper.JsonFormatReaderDelegate == null)
					{
						if (TraditionalCollectionDataContract.IsReadOnlyContract)
						{
							DataContract.ThrowInvalidDataContractException(TraditionalCollectionDataContract.DeserializationExceptionMessage, null);
						}
						JsonFormatCollectionReaderDelegate jsonFormatReaderDelegate = new JsonFormatReaderGenerator().GenerateCollectionReader(TraditionalCollectionDataContract);
						Thread.MemoryBarrier();
						helper.JsonFormatReaderDelegate = jsonFormatReaderDelegate;
					}
				}
			}
			return helper.JsonFormatReaderDelegate;
		}
	}

	internal JsonFormatGetOnlyCollectionReaderDelegate JsonFormatGetOnlyReaderDelegate
	{
		[SecuritySafeCritical]
		get
		{
			if (helper.JsonFormatGetOnlyReaderDelegate == null)
			{
				lock (this)
				{
					if (helper.JsonFormatGetOnlyReaderDelegate == null)
					{
						CollectionKind kind = TraditionalCollectionDataContract.Kind;
						if (base.TraditionalDataContract.UnderlyingType.IsInterface && (kind == CollectionKind.Enumerable || kind == CollectionKind.Collection || kind == CollectionKind.GenericEnumerable))
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("On type '{0}', get-only collection must have an Add method.", DataContract.GetClrTypeFullName(base.TraditionalDataContract.UnderlyingType))));
						}
						if (TraditionalCollectionDataContract.IsReadOnlyContract)
						{
							DataContract.ThrowInvalidDataContractException(TraditionalCollectionDataContract.DeserializationExceptionMessage, null);
						}
						JsonFormatGetOnlyCollectionReaderDelegate jsonFormatGetOnlyReaderDelegate = new JsonFormatReaderGenerator().GenerateGetOnlyCollectionReader(TraditionalCollectionDataContract);
						Thread.MemoryBarrier();
						helper.JsonFormatGetOnlyReaderDelegate = jsonFormatGetOnlyReaderDelegate;
					}
				}
			}
			return helper.JsonFormatGetOnlyReaderDelegate;
		}
	}

	internal JsonFormatCollectionWriterDelegate JsonFormatWriterDelegate
	{
		[SecuritySafeCritical]
		get
		{
			if (helper.JsonFormatWriterDelegate == null)
			{
				lock (this)
				{
					if (helper.JsonFormatWriterDelegate == null)
					{
						JsonFormatCollectionWriterDelegate jsonFormatWriterDelegate = new JsonFormatWriterGenerator().GenerateCollectionWriter(TraditionalCollectionDataContract);
						Thread.MemoryBarrier();
						helper.JsonFormatWriterDelegate = jsonFormatWriterDelegate;
					}
				}
			}
			return helper.JsonFormatWriterDelegate;
		}
	}

	private CollectionDataContract TraditionalCollectionDataContract
	{
		[SecuritySafeCritical]
		get
		{
			return helper.TraditionalCollectionDataContract;
		}
	}

	[SecuritySafeCritical]
	public JsonCollectionDataContract(CollectionDataContract traditionalDataContract)
		: base(new JsonCollectionDataContractCriticalHelper(traditionalDataContract))
	{
		helper = base.Helper as JsonCollectionDataContractCriticalHelper;
	}

	public override object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
	{
		jsonReader.Read();
		object result = null;
		if (context.IsGetOnlyCollection)
		{
			context.IsGetOnlyCollection = false;
			JsonFormatGetOnlyReaderDelegate(jsonReader, context, XmlDictionaryString.Empty, JsonGlobals.itemDictionaryString, TraditionalCollectionDataContract);
		}
		else
		{
			result = JsonFormatReaderDelegate(jsonReader, context, XmlDictionaryString.Empty, JsonGlobals.itemDictionaryString, TraditionalCollectionDataContract);
		}
		jsonReader.ReadEndElement();
		return result;
	}

	public override void WriteJsonValueCore(XmlWriterDelegator jsonWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, RuntimeTypeHandle declaredTypeHandle)
	{
		context.IsGetOnlyCollection = false;
		JsonFormatWriterDelegate(jsonWriter, obj, context, TraditionalCollectionDataContract);
	}
}
