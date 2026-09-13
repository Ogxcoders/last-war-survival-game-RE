using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BestHTTP.Extensions;
using BestHTTP.WebSocket.Extensions;

namespace BestHTTP.WebSocket.Frames;

public struct WebSocketFrameReader
{
	public byte Header { get; private set; }

	public bool IsFinal { get; private set; }

	public WebSocketFrameTypes Type { get; private set; }

	public bool HasMask { get; private set; }

	public ulong Length { get; private set; }

	public byte[] Data { get; private set; }

	public string DataAsText { get; private set; }

	internal void Read(Stream stream)
	{
		Header = ReadByte(stream);
		IsFinal = (Header & 0x80) != 0;
		Type = (WebSocketFrameTypes)(Header & 0xF);
		byte b = ReadByte(stream);
		HasMask = (b & 0x80) != 0;
		Length = (ulong)(b & 0x7F);
		if (Length == 126)
		{
			byte[] array = VariableSizedBufferPool.Get(2L, canBeLarger: true);
			stream.ReadBuffer(array, 2);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse((Array)array, 0, 2);
			}
			Length = BitConverter.ToUInt16(array, 0);
			VariableSizedBufferPool.Release(array);
		}
		else if (Length == 127)
		{
			byte[] array2 = VariableSizedBufferPool.Get(8L, canBeLarger: true);
			stream.ReadBuffer(array2, 8);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse((Array)array2, 0, 8);
			}
			Length = BitConverter.ToUInt64(array2, 0);
			VariableSizedBufferPool.Release(array2);
		}
		byte[] array3 = null;
		if (HasMask)
		{
			array3 = VariableSizedBufferPool.Get(4L, canBeLarger: true);
			if (stream.Read(array3, 0, 4) < array3.Length)
			{
				throw ExceptionHelper.ServerClosedTCPStream();
			}
		}
		if (Type == WebSocketFrameTypes.Text || Type == WebSocketFrameTypes.Continuation)
		{
			Data = VariableSizedBufferPool.Get((long)Length, canBeLarger: true);
		}
		else if (Length == 0L)
		{
			Data = VariableSizedBufferPool.NoData;
		}
		else
		{
			Data = new byte[Length];
		}
		if (Length == 0L)
		{
			return;
		}
		uint num = 0u;
		do
		{
			int num2 = stream.Read(Data, (int)num, (int)(Length - num));
			if (num2 <= 0)
			{
				throw ExceptionHelper.ServerClosedTCPStream();
			}
			num += (uint)num2;
		}
		while (num < Length);
		if (HasMask)
		{
			for (uint num3 = 0u; num3 < Length; num3++)
			{
				Data[num3] ^= array3[num3 % 4];
			}
			VariableSizedBufferPool.Release(array3);
		}
	}

	private byte ReadByte(Stream stream)
	{
		int num = stream.ReadByte();
		if (num < 0)
		{
			throw ExceptionHelper.ServerClosedTCPStream();
		}
		return (byte)num;
	}

	public void Assemble(List<WebSocketFrameReader> fragments)
	{
		fragments.Add(this);
		ulong num = 0uL;
		for (int i = 0; i < fragments.Count; i++)
		{
			num += fragments[i].Length;
		}
		byte[] array = ((fragments[0].Type == WebSocketFrameTypes.Text) ? VariableSizedBufferPool.Get((long)num, canBeLarger: true) : new byte[num]);
		ulong num2 = 0uL;
		for (int j = 0; j < fragments.Count; j++)
		{
			Array.Copy(fragments[j].Data, 0, array, (int)num2, (int)fragments[j].Length);
			VariableSizedBufferPool.Release(fragments[j].Data);
			num2 += fragments[j].Length;
		}
		Type = fragments[0].Type;
		Header = fragments[0].Header;
		Length = num;
		Data = array;
	}

	public void DecodeWithExtensions(WebSocket webSocket)
	{
		if (webSocket.Extensions != null)
		{
			for (int i = 0; i < webSocket.Extensions.Length; i++)
			{
				IExtension extension = webSocket.Extensions[i];
				if (extension != null)
				{
					byte[] array = extension.Decode(Header, Data, (int)Length);
					if (Data != array)
					{
						VariableSizedBufferPool.Release(Data);
						Data = array;
						Length = (ulong)array.Length;
					}
				}
			}
		}
		if (Type == WebSocketFrameTypes.Text && Data != null)
		{
			DataAsText = Encoding.UTF8.GetString(Data, 0, (int)Length);
			VariableSizedBufferPool.Release(Data);
			Data = null;
		}
	}
}
