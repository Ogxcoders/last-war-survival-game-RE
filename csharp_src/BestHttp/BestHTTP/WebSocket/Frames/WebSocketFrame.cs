using System;
using BestHTTP.Extensions;
using BestHTTP.WebSocket.Extensions;

namespace BestHTTP.WebSocket.Frames;

public sealed class WebSocketFrame
{
	public WebSocketFrameTypes Type { get; private set; }

	public bool IsFinal { get; private set; }

	public byte Header { get; private set; }

	public byte[] Data { get; private set; }

	public int DataLength { get; private set; }

	public bool UseExtensions { get; private set; }

	public WebSocketFrame(WebSocket webSocket, WebSocketFrameTypes type, byte[] data)
		: this(webSocket, type, data, useExtensions: true)
	{
	}

	public WebSocketFrame(WebSocket webSocket, WebSocketFrameTypes type, byte[] data, bool useExtensions)
		: this(webSocket, type, data, 0uL, (ulong)((data != null) ? data.Length : 0), isFinal: true, useExtensions)
	{
	}

	public WebSocketFrame(WebSocket webSocket, WebSocketFrameTypes type, byte[] data, bool isFinal, bool useExtensions)
		: this(webSocket, type, data, 0uL, (ulong)((data != null) ? data.Length : 0), isFinal, useExtensions)
	{
	}

	public WebSocketFrame(WebSocket webSocket, WebSocketFrameTypes type, byte[] data, ulong pos, ulong length, bool isFinal, bool useExtensions)
	{
		Type = type;
		IsFinal = isFinal;
		UseExtensions = useExtensions;
		DataLength = (int)length;
		if (data != null)
		{
			Data = VariableSizedBufferPool.Get(DataLength, canBeLarger: true);
			Array.Copy(data, (int)pos, Data, 0, DataLength);
		}
		else
		{
			data = VariableSizedBufferPool.NoData;
		}
		Header = (byte)((uint)(byte)(IsFinal ? 128 : 0) | (uint)Type);
		if (!UseExtensions || webSocket == null || webSocket.Extensions == null)
		{
			return;
		}
		for (int i = 0; i < webSocket.Extensions.Length; i++)
		{
			IExtension extension = webSocket.Extensions[i];
			if (extension != null)
			{
				Header |= extension.GetFrameHeader(this, Header);
				byte[] array = extension.Encode(this);
				if (array != Data)
				{
					VariableSizedBufferPool.Release(Data);
					Data = array;
					DataLength = array.Length;
				}
			}
		}
	}

	public RawFrameData Get()
	{
		if (Data == null)
		{
			Data = VariableSizedBufferPool.NoData;
		}
		using BufferPoolMemoryStream bufferPoolMemoryStream = new BufferPoolMemoryStream(DataLength + 9);
		bufferPoolMemoryStream.WriteByte(Header);
		if (DataLength < 126)
		{
			bufferPoolMemoryStream.WriteByte((byte)(0x80 | (byte)DataLength));
		}
		else if (DataLength < 65535)
		{
			bufferPoolMemoryStream.WriteByte(254);
			byte[] bytes = BitConverter.GetBytes((ushort)DataLength);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse((Array)bytes, 0, bytes.Length);
			}
			bufferPoolMemoryStream.Write(bytes, 0, bytes.Length);
		}
		else
		{
			bufferPoolMemoryStream.WriteByte(byte.MaxValue);
			byte[] bytes2 = BitConverter.GetBytes((ulong)DataLength);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse((Array)bytes2, 0, bytes2.Length);
			}
			bufferPoolMemoryStream.Write(bytes2, 0, bytes2.Length);
		}
		byte[] bytes3 = BitConverter.GetBytes(GetHashCode());
		bufferPoolMemoryStream.Write(bytes3, 0, bytes3.Length);
		for (int i = 0; i < DataLength; i++)
		{
			bufferPoolMemoryStream.WriteByte((byte)(Data[i] ^ bytes3[i % 4]));
		}
		return new RawFrameData(bufferPoolMemoryStream.ToArray(canBeLarger: true), (int)bufferPoolMemoryStream.Length);
	}

	public WebSocketFrame[] Fragment(ushort maxFragmentSize)
	{
		if (Data == null)
		{
			return null;
		}
		if (Type != WebSocketFrameTypes.Binary && Type != WebSocketFrameTypes.Text)
		{
			return null;
		}
		if (DataLength <= maxFragmentSize)
		{
			return null;
		}
		IsFinal = false;
		Header &= 127;
		int num = DataLength / maxFragmentSize + ((DataLength % maxFragmentSize == 0) ? (-1) : 0);
		WebSocketFrame[] array = new WebSocketFrame[num];
		ulong num3;
		for (ulong num2 = maxFragmentSize; num2 < (ulong)DataLength; num2 += num3)
		{
			num3 = Math.Min(maxFragmentSize, (ulong)DataLength - num2);
			array[^(num--)] = new WebSocketFrame(null, WebSocketFrameTypes.Continuation, Data, num2, num3, num2 + num3 >= (ulong)DataLength, useExtensions: false);
		}
		DataLength = maxFragmentSize;
		return array;
	}
}
