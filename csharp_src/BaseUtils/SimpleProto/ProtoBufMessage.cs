using System;

namespace SimpleProto;

public abstract class ProtoBufMessage
{
	public static void Encrypt(uint key, byte[] bytes, int length)
	{
		for (int i = 0; i < length; i += 4)
		{
			bytes[i] ^= (byte)key;
			bytes[i + 1] ^= (byte)(key >> 8);
			bytes[i + 2] ^= (byte)(key >> 16);
			bytes[i + 3] ^= (byte)(key >> 24);
			key = (uint)CRC32.GetCRC32(bytes, i, 4);
		}
	}

	public static void Decrypt(uint key, byte[] bytes, int length)
	{
		for (int i = 0; i < length; i += 4)
		{
			int cRC = CRC32.GetCRC32(bytes, i, 4);
			bytes[i] ^= (byte)key;
			bytes[i + 1] ^= (byte)(key >> 8);
			bytes[i + 2] ^= (byte)(key >> 16);
			bytes[i + 3] ^= (byte)(key >> 24);
			key = (uint)cRC;
		}
	}

	public static void ToStream(OutStream outs, ProtoBufMessage msg, uint key)
	{
		ProtoBufSerializer.SaveStream(msg, outs);
	}

	public static void FromStream(InStream ins, ProtoBufMessage msg, uint key)
	{
		ProtoBufSerializer.LoadStream(msg, ins);
	}

	public void ToStream(OutStream outs)
	{
		ToStream(outs, this, 12345678u);
	}

	public byte[] SerializeToBytes()
	{
		OutStream outStream = new OutStream(new Buffer(1024));
		ToStream(outStream);
		byte[] array = new byte[outStream.Offset];
		Array.Copy(outStream.GetBuffer().buffer, array, outStream.Offset);
		return array;
	}

	public void DeSerializeFromBytes(byte[] data)
	{
		InStream ins = new InStream(new Buffer(data));
		FromStream(ins);
	}

	public void FromStream(InStream ins)
	{
		FromStream(ins, this, 12345678u);
	}
}
