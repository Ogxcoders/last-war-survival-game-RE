using System;

namespace VEngine;

public interface ISerializable
{
	void Deserialize(string line);

	void Deserialize(ReadOnlySpan<char> line);

	string Serialize();
}
