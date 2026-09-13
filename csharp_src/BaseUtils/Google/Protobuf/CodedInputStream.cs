using System;
using System.Collections.Generic;
using System.IO;

namespace Google.Protobuf;

public sealed class CodedInputStream : IDisposable
{
	private readonly bool leaveOpen;

	private readonly byte[] buffer;

	private int bufferSize;

	private int bufferSizeAfterLimit;

	private int bufferPos;

	private readonly Stream input;

	private uint lastTag;

	private uint nextTag;

	private bool hasNextTag;

	internal const int DefaultRecursionLimit = 100;

	internal const int DefaultSizeLimit = int.MaxValue;

	internal const int BufferSize = 4096;

	private int totalBytesRetired;

	private int currentLimit = int.MaxValue;

	private int recursionDepth;

	private readonly int recursionLimit;

	private readonly int sizeLimit;

	public long Position
	{
		get
		{
			if (input != null)
			{
				return input.Position - (bufferSize + bufferSizeAfterLimit - bufferPos);
			}
			return bufferPos;
		}
	}

	internal uint LastTag => lastTag;

	public int SizeLimit => sizeLimit;

	public int RecursionLimit => recursionLimit;

	internal bool DiscardUnknownFields { get; set; }

	internal ExtensionRegistry ExtensionRegistry { get; set; }

	internal bool ReachedLimit
	{
		get
		{
			if (currentLimit == int.MaxValue)
			{
				return false;
			}
			return totalBytesRetired + bufferPos >= currentLimit;
		}
	}

	public bool IsAtEnd
	{
		get
		{
			if (bufferPos == bufferSize)
			{
				return !RefillBuffer(mustSucceed: false);
			}
			return false;
		}
	}

	public CodedInputStream(byte[] buffer)
		: this(null, ProtoPreconditions.CheckNotNull(buffer, "buffer"), 0, buffer.Length, leaveOpen: true)
	{
	}

	public CodedInputStream(byte[] buffer, int offset, int length)
		: this(null, ProtoPreconditions.CheckNotNull(buffer, "buffer"), offset, offset + length, leaveOpen: true)
	{
		if (offset < 0 || offset > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("offset", "Offset must be within the buffer");
		}
		if (length < 0 || offset + length > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("length", "Length must be non-negative and within the buffer");
		}
	}

	public CodedInputStream(Stream input)
		: this(input, leaveOpen: false)
	{
	}

	public CodedInputStream(Stream input, bool leaveOpen)
		: this(ProtoPreconditions.CheckNotNull(input, "input"), new byte[4096], 0, 0, leaveOpen)
	{
	}

	internal CodedInputStream(Stream input, byte[] buffer, int bufferPos, int bufferSize, bool leaveOpen)
	{
		this.input = input;
		this.buffer = buffer;
		this.bufferPos = bufferPos;
		this.bufferSize = bufferSize;
		sizeLimit = int.MaxValue;
		recursionLimit = 100;
		this.leaveOpen = leaveOpen;
	}

	internal CodedInputStream(Stream input, byte[] buffer, int bufferPos, int bufferSize, int sizeLimit, int recursionLimit, bool leaveOpen)
		: this(input, buffer, bufferPos, bufferSize, leaveOpen)
	{
		if (sizeLimit <= 0)
		{
			throw new ArgumentOutOfRangeException("sizeLimit", "Size limit must be positive");
		}
		if (recursionLimit <= 0)
		{
			throw new ArgumentOutOfRangeException("recursionLimit!", "Recursion limit must be positive");
		}
		this.sizeLimit = sizeLimit;
		this.recursionLimit = recursionLimit;
	}

	public static CodedInputStream CreateWithLimits(Stream input, int sizeLimit, int recursionLimit)
	{
		return new CodedInputStream(input, new byte[4096], 0, 0, sizeLimit, recursionLimit, leaveOpen: false);
	}

	public void Dispose()
	{
		if (!leaveOpen)
		{
			input.Dispose();
		}
	}

	internal void CheckReadEndOfStreamTag()
	{
		if (lastTag != 0)
		{
			throw InvalidProtocolBufferException.MoreDataAvailable();
		}
	}

	public uint PeekTag()
	{
		if (hasNextTag)
		{
			return nextTag;
		}
		uint num = lastTag;
		nextTag = ReadTag();
		hasNextTag = true;
		lastTag = num;
		return nextTag;
	}

	public uint ReadTag()
	{
		if (hasNextTag)
		{
			lastTag = nextTag;
			hasNextTag = false;
			return lastTag;
		}
		if (bufferPos + 2 <= bufferSize)
		{
			int num = buffer[bufferPos++];
			if (num < 128)
			{
				lastTag = (uint)num;
			}
			else
			{
				int num2 = num & 0x7F;
				if ((num = buffer[bufferPos++]) < 128)
				{
					num2 |= num << 7;
					lastTag = (uint)num2;
				}
				else
				{
					bufferPos -= 2;
					lastTag = ReadRawVarint32();
				}
			}
		}
		else
		{
			if (IsAtEnd)
			{
				lastTag = 0u;
				return 0u;
			}
			lastTag = ReadRawVarint32();
		}
		if (WireFormat.GetTagFieldNumber(lastTag) == 0)
		{
			throw InvalidProtocolBufferException.InvalidTag();
		}
		if (ReachedLimit)
		{
			return 0u;
		}
		return lastTag;
	}

	public void SkipLastField()
	{
		if (lastTag == 0)
		{
			throw new InvalidOperationException("SkipLastField cannot be called at the end of a stream");
		}
		switch (WireFormat.GetTagWireType(lastTag))
		{
		case WireFormat.WireType.StartGroup:
			SkipGroup(lastTag);
			break;
		case WireFormat.WireType.EndGroup:
			throw new InvalidProtocolBufferException("SkipLastField called on an end-group tag, indicating that the corresponding start-group was missing");
		case WireFormat.WireType.Fixed32:
			ReadFixed32();
			break;
		case WireFormat.WireType.Fixed64:
			ReadFixed64();
			break;
		case WireFormat.WireType.LengthDelimited:
		{
			int size = ReadLength();
			SkipRawBytes(size);
			break;
		}
		case WireFormat.WireType.Varint:
			ReadRawVarint32();
			break;
		}
	}

	internal void SkipGroup(uint startGroupTag)
	{
		recursionDepth++;
		if (recursionDepth >= recursionLimit)
		{
			throw InvalidProtocolBufferException.RecursionLimitExceeded();
		}
		uint num;
		while (true)
		{
			num = ReadTag();
			if (num == 0)
			{
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			if (WireFormat.GetTagWireType(num) == WireFormat.WireType.EndGroup)
			{
				break;
			}
			SkipLastField();
		}
		int tagFieldNumber = WireFormat.GetTagFieldNumber(startGroupTag);
		int tagFieldNumber2 = WireFormat.GetTagFieldNumber(num);
		if (tagFieldNumber != tagFieldNumber2)
		{
			throw new InvalidProtocolBufferException($"Mismatched end-group tag. Started with field {tagFieldNumber}; ended with field {tagFieldNumber2}");
		}
		recursionDepth--;
	}

	public double ReadDouble()
	{
		if (bufferPos + 8 <= bufferSize)
		{
			if (BitConverter.IsLittleEndian)
			{
				double result = BitConverter.ToDouble(buffer, bufferPos);
				bufferPos += 8;
				return result;
			}
			byte[] value = new byte[8]
			{
				buffer[bufferPos + 7],
				buffer[bufferPos + 6],
				buffer[bufferPos + 5],
				buffer[bufferPos + 4],
				buffer[bufferPos + 3],
				buffer[bufferPos + 2],
				buffer[bufferPos + 1],
				buffer[bufferPos]
			};
			bufferPos += 8;
			return BitConverter.ToDouble(value, 0);
		}
		return BitConverter.Int64BitsToDouble((long)ReadRawLittleEndian64());
	}

	public float ReadFloat()
	{
		if (BitConverter.IsLittleEndian && 4 <= bufferSize - bufferPos)
		{
			float result = BitConverter.ToSingle(buffer, bufferPos);
			bufferPos += 4;
			return result;
		}
		byte[] array = ReadRawBytes(4);
		if (!BitConverter.IsLittleEndian)
		{
			ByteArray.Reverse(array);
		}
		return BitConverter.ToSingle(array, 0);
	}

	public ulong ReadUInt64()
	{
		return ReadRawVarint64();
	}

	public long ReadInt64()
	{
		return (long)ReadRawVarint64();
	}

	public int ReadInt32()
	{
		return (int)ReadRawVarint32();
	}

	public ulong ReadFixed64()
	{
		return ReadRawLittleEndian64();
	}

	public uint ReadFixed32()
	{
		return ReadRawLittleEndian32();
	}

	public bool ReadBool()
	{
		return ReadRawVarint32() != 0;
	}

	public string ReadString()
	{
		int num = ReadLength();
		if (num == 0)
		{
			return "";
		}
		if (num <= bufferSize - bufferPos && num > 0)
		{
			string result = CodedOutputStream.Utf8Encoding.GetString(buffer, bufferPos, num);
			bufferPos += num;
			return result;
		}
		return CodedOutputStream.Utf8Encoding.GetString(ReadRawBytes(num), 0, num);
	}

	public void ReadMessage(IMessage builder)
	{
		int byteLimit = ReadLength();
		if (recursionDepth >= recursionLimit)
		{
			throw InvalidProtocolBufferException.RecursionLimitExceeded();
		}
		int oldLimit = PushLimit(byteLimit);
		recursionDepth++;
		builder.MergeFrom(this);
		CheckReadEndOfStreamTag();
		if (!ReachedLimit)
		{
			throw InvalidProtocolBufferException.TruncatedMessage();
		}
		recursionDepth--;
		PopLimit(oldLimit);
	}

	public void ReadGroup(IMessage builder)
	{
		if (recursionDepth >= recursionLimit)
		{
			throw InvalidProtocolBufferException.RecursionLimitExceeded();
		}
		recursionDepth++;
		builder.MergeFrom(this);
		recursionDepth--;
	}

	public ByteString ReadBytes()
	{
		int num = ReadLength();
		if (num <= bufferSize - bufferPos && num > 0)
		{
			ByteString result = ByteString.CopyFrom(buffer, bufferPos, num);
			bufferPos += num;
			return result;
		}
		return ByteString.AttachBytes(ReadRawBytes(num));
	}

	public uint ReadUInt32()
	{
		return ReadRawVarint32();
	}

	public int ReadEnum()
	{
		return (int)ReadRawVarint32();
	}

	public int ReadSFixed32()
	{
		return (int)ReadRawLittleEndian32();
	}

	public long ReadSFixed64()
	{
		return (long)ReadRawLittleEndian64();
	}

	public int ReadSInt32()
	{
		return DecodeZigZag32(ReadRawVarint32());
	}

	public long ReadSInt64()
	{
		return DecodeZigZag64(ReadRawVarint64());
	}

	public int ReadLength()
	{
		return (int)ReadRawVarint32();
	}

	public bool MaybeConsumeTag(uint tag)
	{
		if (PeekTag() == tag)
		{
			hasNextTag = false;
			return true;
		}
		return false;
	}

	internal static float? ReadFloatWrapperLittleEndian(CodedInputStream input)
	{
		if (input.bufferPos + 6 <= input.bufferSize)
		{
			switch (input.buffer[input.bufferPos])
			{
			case 0:
				input.bufferPos++;
				return 0f;
			case 5:
				if (input.buffer[input.bufferPos + 1] == 13)
				{
					float value = BitConverter.ToSingle(input.buffer, input.bufferPos + 2);
					input.bufferPos += 6;
					return value;
				}
				goto default;
			default:
				return ReadFloatWrapperSlow(input);
			}
		}
		return ReadFloatWrapperSlow(input);
	}

	internal static float? ReadFloatWrapperSlow(CodedInputStream input)
	{
		int num = input.ReadLength();
		if (num == 0)
		{
			return 0f;
		}
		int num2 = input.totalBytesRetired + input.bufferPos + num;
		float value = 0f;
		do
		{
			if (input.ReadTag() == 13)
			{
				value = input.ReadFloat();
			}
			else
			{
				input.SkipLastField();
			}
		}
		while (input.totalBytesRetired + input.bufferPos < num2);
		return value;
	}

	internal static double? ReadDoubleWrapperLittleEndian(CodedInputStream input)
	{
		if (input.bufferPos + 10 <= input.bufferSize)
		{
			switch (input.buffer[input.bufferPos])
			{
			case 0:
				input.bufferPos++;
				return 0.0;
			case 9:
				if (input.buffer[input.bufferPos + 1] == 9)
				{
					double value = BitConverter.ToDouble(input.buffer, input.bufferPos + 2);
					input.bufferPos += 10;
					return value;
				}
				goto default;
			default:
				return ReadDoubleWrapperSlow(input);
			}
		}
		return ReadDoubleWrapperSlow(input);
	}

	internal static double? ReadDoubleWrapperSlow(CodedInputStream input)
	{
		int num = input.ReadLength();
		if (num == 0)
		{
			return 0.0;
		}
		int num2 = input.totalBytesRetired + input.bufferPos + num;
		double value = 0.0;
		do
		{
			if (input.ReadTag() == 9)
			{
				value = input.ReadDouble();
			}
			else
			{
				input.SkipLastField();
			}
		}
		while (input.totalBytesRetired + input.bufferPos < num2);
		return value;
	}

	internal static bool? ReadBoolWrapper(CodedInputStream input)
	{
		return ReadUInt32Wrapper(input) != 0;
	}

	internal static uint? ReadUInt32Wrapper(CodedInputStream input)
	{
		if (input.bufferPos + 7 <= input.bufferSize)
		{
			int num = input.bufferPos;
			int num2 = input.buffer[input.bufferPos++];
			if (num2 == 0)
			{
				return 0u;
			}
			if (num2 >= 128)
			{
				input.bufferPos = num;
				return ReadUInt32WrapperSlow(input);
			}
			int num3 = input.bufferPos + num2;
			if (input.buffer[input.bufferPos++] != 8)
			{
				input.bufferPos = num;
				return ReadUInt32WrapperSlow(input);
			}
			uint value = input.ReadUInt32();
			if (input.bufferPos != num3)
			{
				input.bufferPos = num;
				return ReadUInt32WrapperSlow(input);
			}
			return value;
		}
		return ReadUInt32WrapperSlow(input);
	}

	private static uint? ReadUInt32WrapperSlow(CodedInputStream input)
	{
		int num = input.ReadLength();
		if (num == 0)
		{
			return 0u;
		}
		int num2 = input.totalBytesRetired + input.bufferPos + num;
		uint value = 0u;
		do
		{
			if (input.ReadTag() == 8)
			{
				value = input.ReadUInt32();
			}
			else
			{
				input.SkipLastField();
			}
		}
		while (input.totalBytesRetired + input.bufferPos < num2);
		return value;
	}

	internal static int? ReadInt32Wrapper(CodedInputStream input)
	{
		return (int?)ReadUInt32Wrapper(input);
	}

	internal static ulong? ReadUInt64Wrapper(CodedInputStream input)
	{
		if (input.bufferPos + 12 <= input.bufferSize)
		{
			int num = input.bufferPos;
			int num2 = input.buffer[input.bufferPos++];
			if (num2 == 0)
			{
				return 0uL;
			}
			if (num2 >= 128)
			{
				input.bufferPos = num;
				return ReadUInt64WrapperSlow(input);
			}
			int num3 = input.bufferPos + num2;
			if (input.buffer[input.bufferPos++] != 8)
			{
				input.bufferPos = num;
				return ReadUInt64WrapperSlow(input);
			}
			ulong value = input.ReadUInt64();
			if (input.bufferPos != num3)
			{
				input.bufferPos = num;
				return ReadUInt64WrapperSlow(input);
			}
			return value;
		}
		return ReadUInt64WrapperSlow(input);
	}

	internal static ulong? ReadUInt64WrapperSlow(CodedInputStream input)
	{
		int num = input.ReadLength();
		if (num == 0)
		{
			return 0uL;
		}
		int num2 = input.totalBytesRetired + input.bufferPos + num;
		ulong value = 0uL;
		do
		{
			if (input.ReadTag() == 8)
			{
				value = input.ReadUInt64();
			}
			else
			{
				input.SkipLastField();
			}
		}
		while (input.totalBytesRetired + input.bufferPos < num2);
		return value;
	}

	internal static long? ReadInt64Wrapper(CodedInputStream input)
	{
		return (long?)ReadUInt64Wrapper(input);
	}

	private uint SlowReadRawVarint32()
	{
		int num = ReadRawByte();
		if (num < 128)
		{
			return (uint)num;
		}
		int num2 = num & 0x7F;
		if ((num = ReadRawByte()) < 128)
		{
			num2 |= num << 7;
		}
		else
		{
			num2 |= (num & 0x7F) << 7;
			if ((num = ReadRawByte()) < 128)
			{
				num2 |= num << 14;
			}
			else
			{
				num2 |= (num & 0x7F) << 14;
				if ((num = ReadRawByte()) < 128)
				{
					num2 |= num << 21;
				}
				else
				{
					num2 |= (num & 0x7F) << 21;
					num2 |= (num = ReadRawByte()) << 28;
					if (num >= 128)
					{
						for (int i = 0; i < 5; i++)
						{
							if (ReadRawByte() < 128)
							{
								return (uint)num2;
							}
						}
						throw InvalidProtocolBufferException.MalformedVarint();
					}
				}
			}
		}
		return (uint)num2;
	}

	internal uint ReadRawVarint32()
	{
		if (bufferPos + 5 > bufferSize)
		{
			return SlowReadRawVarint32();
		}
		int num = buffer[bufferPos++];
		if (num < 128)
		{
			return (uint)num;
		}
		int num2 = num & 0x7F;
		if ((num = buffer[bufferPos++]) < 128)
		{
			num2 |= num << 7;
		}
		else
		{
			num2 |= (num & 0x7F) << 7;
			if ((num = buffer[bufferPos++]) < 128)
			{
				num2 |= num << 14;
			}
			else
			{
				num2 |= (num & 0x7F) << 14;
				if ((num = buffer[bufferPos++]) < 128)
				{
					num2 |= num << 21;
				}
				else
				{
					num2 |= (num & 0x7F) << 21;
					num2 |= (num = buffer[bufferPos++]) << 28;
					if (num >= 128)
					{
						for (int i = 0; i < 5; i++)
						{
							if (ReadRawByte() < 128)
							{
								return (uint)num2;
							}
						}
						throw InvalidProtocolBufferException.MalformedVarint();
					}
				}
			}
		}
		return (uint)num2;
	}

	internal static uint ReadRawVarint32(Stream input)
	{
		int num = 0;
		int i;
		for (i = 0; i < 32; i += 7)
		{
			int num2 = input.ReadByte();
			if (num2 == -1)
			{
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			num |= (num2 & 0x7F) << i;
			if ((num2 & 0x80) == 0)
			{
				return (uint)num;
			}
		}
		for (; i < 64; i += 7)
		{
			int num3 = input.ReadByte();
			if (num3 == -1)
			{
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			if ((num3 & 0x80) == 0)
			{
				return (uint)num;
			}
		}
		throw InvalidProtocolBufferException.MalformedVarint();
	}

	internal ulong ReadRawVarint64()
	{
		if (bufferPos + 10 <= bufferSize)
		{
			ulong num = buffer[bufferPos++];
			if (num < 128)
			{
				return num;
			}
			num &= 0x7F;
			int num2 = 7;
			do
			{
				byte b = buffer[bufferPos++];
				num |= (ulong)((long)(b & 0x7F) << num2);
				if (b < 128)
				{
					return num;
				}
				num2 += 7;
			}
			while (num2 < 64);
		}
		else
		{
			int num3 = 0;
			ulong num4 = 0uL;
			do
			{
				byte b2 = ReadRawByte();
				num4 |= (ulong)((long)(b2 & 0x7F) << num3);
				if (b2 < 128)
				{
					return num4;
				}
				num3 += 7;
			}
			while (num3 < 64);
		}
		throw InvalidProtocolBufferException.MalformedVarint();
	}

	internal uint ReadRawLittleEndian32()
	{
		if (bufferPos + 4 <= bufferSize)
		{
			if (BitConverter.IsLittleEndian)
			{
				uint result = BitConverter.ToUInt32(buffer, bufferPos);
				bufferPos += 4;
				return result;
			}
			byte num = buffer[bufferPos];
			uint num2 = buffer[bufferPos + 1];
			uint num3 = buffer[bufferPos + 2];
			uint num4 = buffer[bufferPos + 3];
			bufferPos += 4;
			return num | (num2 << 8) | (num3 << 16) | (num4 << 24);
		}
		byte num5 = ReadRawByte();
		uint num6 = ReadRawByte();
		uint num7 = ReadRawByte();
		uint num8 = ReadRawByte();
		return num5 | (num6 << 8) | (num7 << 16) | (num8 << 24);
	}

	internal ulong ReadRawLittleEndian64()
	{
		if (bufferPos + 8 <= bufferSize)
		{
			if (BitConverter.IsLittleEndian)
			{
				ulong result = BitConverter.ToUInt64(buffer, bufferPos);
				bufferPos += 8;
				return result;
			}
			long num = buffer[bufferPos];
			ulong num2 = buffer[bufferPos + 1];
			ulong num3 = buffer[bufferPos + 2];
			ulong num4 = buffer[bufferPos + 3];
			ulong num5 = buffer[bufferPos + 4];
			ulong num6 = buffer[bufferPos + 5];
			ulong num7 = buffer[bufferPos + 6];
			ulong num8 = buffer[bufferPos + 7];
			bufferPos += 8;
			return (ulong)num | (num2 << 8) | (num3 << 16) | (num4 << 24) | (num5 << 32) | (num6 << 40) | (num7 << 48) | (num8 << 56);
		}
		long num9 = ReadRawByte();
		ulong num10 = ReadRawByte();
		ulong num11 = ReadRawByte();
		ulong num12 = ReadRawByte();
		ulong num13 = ReadRawByte();
		ulong num14 = ReadRawByte();
		ulong num15 = ReadRawByte();
		ulong num16 = ReadRawByte();
		return (ulong)num9 | (num10 << 8) | (num11 << 16) | (num12 << 24) | (num13 << 32) | (num14 << 40) | (num15 << 48) | (num16 << 56);
	}

	internal static int DecodeZigZag32(uint n)
	{
		return (int)((n >> 1) ^ (0 - (n & 1)));
	}

	internal static long DecodeZigZag64(ulong n)
	{
		return (long)((n >> 1) ^ (0L - (n & 1)));
	}

	internal int PushLimit(int byteLimit)
	{
		if (byteLimit < 0)
		{
			throw InvalidProtocolBufferException.NegativeSize();
		}
		byteLimit += totalBytesRetired + bufferPos;
		int num = currentLimit;
		if (byteLimit > num)
		{
			throw InvalidProtocolBufferException.TruncatedMessage();
		}
		currentLimit = byteLimit;
		RecomputeBufferSizeAfterLimit();
		return num;
	}

	private void RecomputeBufferSizeAfterLimit()
	{
		bufferSize += bufferSizeAfterLimit;
		int num = totalBytesRetired + bufferSize;
		if (num > currentLimit)
		{
			bufferSizeAfterLimit = num - currentLimit;
			bufferSize -= bufferSizeAfterLimit;
		}
		else
		{
			bufferSizeAfterLimit = 0;
		}
	}

	internal void PopLimit(int oldLimit)
	{
		currentLimit = oldLimit;
		RecomputeBufferSizeAfterLimit();
	}

	private bool RefillBuffer(bool mustSucceed)
	{
		if (bufferPos < bufferSize)
		{
			throw new InvalidOperationException("RefillBuffer() called when buffer wasn't empty.");
		}
		if (totalBytesRetired + bufferSize == currentLimit)
		{
			if (mustSucceed)
			{
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			return false;
		}
		totalBytesRetired += bufferSize;
		bufferPos = 0;
		bufferSize = ((input != null) ? input.Read(buffer, 0, buffer.Length) : 0);
		if (bufferSize < 0)
		{
			throw new InvalidOperationException("Stream.Read returned a negative count");
		}
		if (bufferSize == 0)
		{
			if (mustSucceed)
			{
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			return false;
		}
		RecomputeBufferSizeAfterLimit();
		int num = totalBytesRetired + bufferSize + bufferSizeAfterLimit;
		if (num < 0 || num > sizeLimit)
		{
			throw InvalidProtocolBufferException.SizeLimitExceeded();
		}
		return true;
	}

	internal byte ReadRawByte()
	{
		if (bufferPos == bufferSize)
		{
			RefillBuffer(mustSucceed: true);
		}
		return buffer[bufferPos++];
	}

	internal byte[] ReadRawBytes(int size)
	{
		if (size < 0)
		{
			throw InvalidProtocolBufferException.NegativeSize();
		}
		if (totalBytesRetired + bufferPos + size > currentLimit)
		{
			SkipRawBytes(currentLimit - totalBytesRetired - bufferPos);
			throw InvalidProtocolBufferException.TruncatedMessage();
		}
		if (size <= bufferSize - bufferPos)
		{
			byte[] array = new byte[size];
			ByteArray.Copy(buffer, bufferPos, array, 0, size);
			bufferPos += size;
			return array;
		}
		if (size < buffer.Length)
		{
			byte[] array2 = new byte[size];
			int num = bufferSize - bufferPos;
			ByteArray.Copy(buffer, bufferPos, array2, 0, num);
			bufferPos = bufferSize;
			RefillBuffer(mustSucceed: true);
			while (size - num > bufferSize)
			{
				Buffer.BlockCopy(buffer, 0, array2, num, bufferSize);
				num += bufferSize;
				bufferPos = bufferSize;
				RefillBuffer(mustSucceed: true);
			}
			ByteArray.Copy(buffer, 0, array2, num, size - num);
			bufferPos = size - num;
			return array2;
		}
		int num2 = bufferPos;
		int num3 = bufferSize;
		totalBytesRetired += bufferSize;
		bufferPos = 0;
		bufferSize = 0;
		int num4 = size - (num3 - num2);
		List<byte[]> list = new List<byte[]>();
		while (num4 > 0)
		{
			byte[] array3 = new byte[Math.Min(num4, buffer.Length)];
			int num5;
			for (int i = 0; i < array3.Length; i += num5)
			{
				num5 = ((input == null) ? (-1) : input.Read(array3, i, array3.Length - i));
				if (num5 <= 0)
				{
					throw InvalidProtocolBufferException.TruncatedMessage();
				}
				totalBytesRetired += num5;
			}
			num4 -= array3.Length;
			list.Add(array3);
		}
		byte[] array4 = new byte[size];
		int num6 = num3 - num2;
		ByteArray.Copy(buffer, num2, array4, 0, num6);
		foreach (byte[] item in list)
		{
			Buffer.BlockCopy(item, 0, array4, num6, item.Length);
			num6 += item.Length;
		}
		return array4;
	}

	private void SkipRawBytes(int size)
	{
		if (size < 0)
		{
			throw InvalidProtocolBufferException.NegativeSize();
		}
		if (totalBytesRetired + bufferPos + size > currentLimit)
		{
			SkipRawBytes(currentLimit - totalBytesRetired - bufferPos);
			throw InvalidProtocolBufferException.TruncatedMessage();
		}
		if (size <= bufferSize - bufferPos)
		{
			bufferPos += size;
			return;
		}
		int num = bufferSize - bufferPos;
		totalBytesRetired += bufferSize;
		bufferPos = 0;
		bufferSize = 0;
		if (num < size)
		{
			if (input == null)
			{
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			SkipImpl(size - num);
			totalBytesRetired += size - num;
		}
	}

	private void SkipImpl(int amountToSkip)
	{
		if (input.CanSeek)
		{
			long position = input.Position;
			input.Position += amountToSkip;
			if (input.Position != position + amountToSkip)
			{
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			return;
		}
		byte[] array = new byte[Math.Min(1024, amountToSkip)];
		while (amountToSkip > 0)
		{
			int num = input.Read(array, 0, Math.Min(array.Length, amountToSkip));
			if (num <= 0)
			{
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			amountToSkip -= num;
		}
	}
}
