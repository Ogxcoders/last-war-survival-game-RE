namespace System.Runtime.Serialization.Json;

internal class JsonQNameDataContract : JsonDataContract
{
	public JsonQNameDataContract(QNameDataContract traditionalQNameDataContract)
		: base(traditionalQNameDataContract)
	{
	}

	public override object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
	{
		if (context == null)
		{
			if (!JsonDataContract.TryReadNullAtTopLevel(jsonReader))
			{
				return jsonReader.ReadElementContentAsQName();
			}
			return null;
		}
		return JsonDataContract.HandleReadValue(jsonReader.ReadElementContentAsQName(), context);
	}
}
