using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Leopotam.EcsLite;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MiniGame.Core;

public class GameSerializer : IGameSerializer
{
	private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
	{
		TypeNameHandling = TypeNameHandling.All
	};

	private readonly List<IEcsPoolDelegate> _poolDelegates = new List<IEcsPoolDelegate>();

	private readonly List<JsonConverter> _buildinConverters = new List<JsonConverter>
	{
		new FPConverter(),
		new FVector2Converter(),
		new FVector3Converter(),
		new TypeConverter()
	};

	public GameSerializer(ISerializationBinder jsonBinder, IList<JsonConverter> converters, IList<IEcsPoolDelegate> poolDelegates)
	{
		if (converters == null)
		{
			converters = _buildinConverters;
		}
		else
		{
			foreach (JsonConverter buildinConverter in _buildinConverters)
			{
				converters.Add(buildinConverter);
			}
		}
		_jsonSettings.Converters = converters;
		_jsonSettings.SerializationBinder = jsonBinder;
		_poolDelegates.AddRange(poolDelegates);
	}

	public IEnumerable<IEcsPoolDelegate> GetPoolDelegates()
	{
		return _poolDelegates;
	}

	public string ToJson(object obj, Type type, bool format = false)
	{
		if (format)
		{
			return JsonConvert.SerializeObject(obj, Formatting.Indented, _jsonSettings);
		}
		return JsonConvert.SerializeObject(obj, type, _jsonSettings);
	}

	public object FromJson(string json, Type type)
	{
		return JsonConvert.DeserializeObject(json, type, _jsonSettings);
	}

	public void ToBinary(BinaryWriter writer, object obj, Type type)
	{
		throw new NotImplementedException();
	}

	public object FromBinary(BinaryReader reader, Type type)
	{
		throw new NotImplementedException();
	}

	public string ToJson<T>(T obj, bool format = false)
	{
		return ToJson(obj, typeof(T), format);
	}

	public T FromJson<T>(string json)
	{
		return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
	}

	public void ToBinary<T>(BinaryWriter writer, T obj)
	{
		throw new NotImplementedException();
	}

	public T FromBinary<T>(BinaryReader reader)
	{
		throw new NotImplementedException();
	}

	public string GetMD5(string input)
	{
		using MD5 mD = MD5.Create();
		return BitConverter.ToString(mD.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLower();
	}

	public string GetMD5(byte[] bytes)
	{
		using MD5 mD = MD5.Create();
		return BitConverter.ToString(mD.ComputeHash(bytes)).Replace("-", "").ToLower();
	}
}
