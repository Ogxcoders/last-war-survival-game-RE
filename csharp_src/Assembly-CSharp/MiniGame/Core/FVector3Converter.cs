using System;
using Box2DSharp.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiniGame.Core;

public class FVector3Converter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		FVector3 fVector = (FVector3)value;
		writer.WriteStartObject();
		writer.WritePropertyName("x");
		writer.WriteValue(fVector.X.RawValue);
		writer.WritePropertyName("y");
		writer.WriteValue(fVector.Y.RawValue);
		writer.WritePropertyName("z");
		writer.WriteValue(fVector.Z.RawValue);
		writer.WriteEndObject();
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		JObject jObject = JObject.Load(reader);
		FP x = FP.FromRaw((long)jObject["x"]);
		FP y = FP.FromRaw((long)jObject["y"]);
		FP z = FP.FromRaw((long)jObject["z"]);
		return new FVector3(x, y, z);
	}

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(FVector3);
	}
}
