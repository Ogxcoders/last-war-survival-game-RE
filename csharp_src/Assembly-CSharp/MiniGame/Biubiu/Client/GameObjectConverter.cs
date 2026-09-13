using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class GameObjectConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		GameObject gameObject = (GameObject)value;
		writer.WriteStartObject();
		writer.WritePropertyName("name");
		writer.WriteValue(gameObject.name);
		writer.WriteEndObject();
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		return GameObject.Find((string?)JObject.Load(reader)["name"]);
	}

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(GameObject);
	}
}
