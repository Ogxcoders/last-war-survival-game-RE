using System;
using Box2DSharp.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiniGame.Core;

public class FVector2Converter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		FVector2 fVector = (FVector2)value;
		writer.WriteStartObject();
		writer.WritePropertyName("x");
		writer.WriteValue(fVector.X.RawValue);
		writer.WritePropertyName("y");
		writer.WriteValue(fVector.Y.RawValue);
		writer.WriteEndObject();
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		JObject jObject = JObject.Load(reader);
		FP x = FP.FromRaw((long)jObject["x"]);
		FP y = FP.FromRaw((long)jObject["y"]);
		return new FVector2(x, y);
	}

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(FVector2);
	}
}
