using System.Security;

namespace System.Runtime.Serialization.Json;

internal class JsonEnumDataContract : JsonDataContract
{
	private class JsonEnumDataContractCriticalHelper : JsonDataContractCriticalHelper
	{
		private bool isULong;

		public bool IsULong => isULong;

		public JsonEnumDataContractCriticalHelper(EnumDataContract traditionalEnumDataContract)
			: base(traditionalEnumDataContract)
		{
			isULong = traditionalEnumDataContract.IsULong;
		}
	}

	[SecurityCritical]
	private JsonEnumDataContractCriticalHelper helper;

	public bool IsULong
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsULong;
		}
	}

	[SecuritySafeCritical]
	public JsonEnumDataContract(EnumDataContract traditionalDataContract)
		: base(new JsonEnumDataContractCriticalHelper(traditionalDataContract))
	{
		helper = base.Helper as JsonEnumDataContractCriticalHelper;
	}

	public override object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
	{
		object obj = ((!IsULong) ? Enum.ToObject(base.TraditionalDataContract.UnderlyingType, jsonReader.ReadElementContentAsLong()) : Enum.ToObject(base.TraditionalDataContract.UnderlyingType, jsonReader.ReadElementContentAsUnsignedLong()));
		context?.AddNewObject(obj);
		return obj;
	}

	public override void WriteJsonValueCore(XmlWriterDelegator jsonWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, RuntimeTypeHandle declaredTypeHandle)
	{
		if (IsULong)
		{
			jsonWriter.WriteUnsignedLong(((IConvertible)obj).ToUInt64(null));
		}
		else
		{
			jsonWriter.WriteLong(((IConvertible)obj).ToInt64(null));
		}
	}
}
