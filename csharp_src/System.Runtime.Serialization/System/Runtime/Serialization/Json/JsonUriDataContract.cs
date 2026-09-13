namespace System.Runtime.Serialization.Json;

internal class JsonUriDataContract : JsonDataContract
{
	public JsonUriDataContract(UriDataContract traditionalUriDataContract)
		: base(traditionalUriDataContract)
	{
	}

	public override object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
	{
		if (context == null)
		{
			if (!JsonDataContract.TryReadNullAtTopLevel(jsonReader))
			{
				return jsonReader.ReadElementContentAsUri();
			}
			return null;
		}
		return JsonDataContract.HandleReadValue(jsonReader.ReadElementContentAsUri(), context);
	}
}
