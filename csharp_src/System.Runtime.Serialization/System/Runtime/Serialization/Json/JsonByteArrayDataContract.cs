namespace System.Runtime.Serialization.Json;

internal class JsonByteArrayDataContract : JsonDataContract
{
	public JsonByteArrayDataContract(ByteArrayDataContract traditionalByteArrayDataContract)
		: base(traditionalByteArrayDataContract)
	{
	}

	public override object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
	{
		if (context == null)
		{
			if (!JsonDataContract.TryReadNullAtTopLevel(jsonReader))
			{
				return jsonReader.ReadElementContentAsBase64();
			}
			return null;
		}
		return JsonDataContract.HandleReadValue(jsonReader.ReadElementContentAsBase64(), context);
	}
}
