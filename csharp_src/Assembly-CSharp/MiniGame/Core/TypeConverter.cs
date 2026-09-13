using System;
using Newtonsoft.Json;

namespace MiniGame.Core;

public class TypeConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		serializer.SerializationBinder.BindToName((Type)value, out string assemblyName, out string typeName);
		writer.WriteValue(typeName + ", " + assemblyName);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		string[] array = ((string)reader.Value).Split(new char[1] { ',' });
		return serializer.SerializationBinder.BindToType((array.Length > 1) ? array[1] : string.Empty, array[0]);
	}

	public override bool CanConvert(Type objectType)
	{
		return typeof(Type).IsAssignableFrom(objectType);
	}
}
