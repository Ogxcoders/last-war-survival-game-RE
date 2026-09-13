using System;

namespace BestHTTP.SignalRCore;

public interface IEncoder
{
	string Name { get; }

	string EncodeAsText<T>(T value);

	T DecodeAs<T>(string text);

	byte[] EncodeAsBinary<T>(T value);

	T DecodeAs<T>(byte[] data);

	object ConvertTo(Type toType, object obj);
}
