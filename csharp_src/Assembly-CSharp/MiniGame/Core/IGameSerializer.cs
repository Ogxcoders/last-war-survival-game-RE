using System;
using System.Collections.Generic;
using System.IO;
using Leopotam.EcsLite;

namespace MiniGame.Core;

public interface IGameSerializer
{
	IEnumerable<IEcsPoolDelegate> GetPoolDelegates();

	string ToJson(object obj, Type type, bool format = false);

	object FromJson(string json, Type type);

	void ToBinary(BinaryWriter writer, object obj, Type type);

	object FromBinary(BinaryReader reader, Type type);

	string ToJson<T>(T obj, bool format = false);

	T FromJson<T>(string json);

	void ToBinary<T>(BinaryWriter writer, T obj);

	T FromBinary<T>(BinaryReader reader);

	string GetMD5(string data);

	string GetMD5(byte[] bytes);
}
