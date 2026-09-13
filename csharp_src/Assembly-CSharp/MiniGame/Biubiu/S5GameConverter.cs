using System;
using Box2DSharp.Foreign;
using Newtonsoft.Json;

namespace MiniGame.Biubiu;

public class S5GameConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		_ = (S5Game)value;
		writer.WriteStartObject();
		writer.WritePropertyName("name");
		writer.WriteValue("S5Game");
		writer.WriteEndObject();
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(S5Game);
	}
}
