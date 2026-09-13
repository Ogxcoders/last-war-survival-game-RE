using System;
using BestHTTP.Extensions;

namespace BestHTTP.WebSocket.Frames;

public struct RawFrameData : IDisposable
{
	public byte[] Data;

	public int Length;

	public RawFrameData(byte[] data, int length)
	{
		Data = data;
		Length = length;
	}

	public void Dispose()
	{
		VariableSizedBufferPool.Release(Data);
		Data = null;
	}
}
