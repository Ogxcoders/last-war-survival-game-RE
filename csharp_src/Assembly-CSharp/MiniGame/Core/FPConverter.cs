using System;
using Box2DSharp.Common;
using Newtonsoft.Json;

namespace MiniGame.Core;

public class FPConverter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		writer.WriteValue(((FP)value).RawValue);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		long num = 0L;
		try
		{
			num = (long)reader.Value;
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
		return FP.FromRaw(num);
	}

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(FP);
	}
}
