using System;
using System.Buffers;
using System.IO;
using GameFramework;
using LZ4;
using Sfs2X.Bitswarm;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Exceptions;
using Sfs2X.Util;
using ZstdNet;

namespace ProtoBufNet;

public class NetPacketNew : INetPacket, IDisposable
{
	private class SharedBufferManager : ReadOnlyByteArray.ISharedBufferManager
	{
		public byte[] Allocate(int size)
		{
			return ArrayPool<byte>.Shared.Rent(size);
		}

		public void Free(byte[] buffer)
		{
			ArrayPool<byte>.Shared.Return(buffer);
		}
	}

	private const int compressionThreshold = 1024;

	private const int maxMessageSize = 1000000;

	private package_header_new header;

	private ByteArray buffer;

	private ReadOnlyByteArray _headerAndBodyLengthBuff;

	private int _bodyLengthBuffOffset;

	private int _bodyLengthBuffLength;

	private byte[] _bodyBuff;

	private int _bodyBuffLen;

	public static bool U = false;

	private static ReadOnlyByteArray.ISharedBufferManager _sharedBufferManager = new SharedBufferManager();

	private Decompressor _ztsdDecompressor;

	private NetProfiler _profiler;

	private ushort _sid;

	public int bodyLengthHasRead { get; set; }

	public int bodyHasRead { get; set; }

	public ISFSObject info { get; private set; }

	public string logType { get; set; }

	public long recvTime { get; set; }

	public package_header_new Header => header;

	public ByteArray Buffer
	{
		get
		{
			return buffer;
		}
		set
		{
			buffer = value;
		}
	}

	public NetPacketNew(Decompressor decompressor, NetProfiler profiler, ushort sid, bool read = true)
	{
		_ztsdDecompressor = decompressor;
		if (read)
		{
			_bodyLengthBuffOffset = 1;
			_headerAndBodyLengthBuff = new ReadOnlyByteArray(_sharedBufferManager.Allocate(16), 16, _sharedBufferManager);
		}
		bodyLengthHasRead = 0;
		bodyHasRead = 0;
		_profiler = profiler;
		_sid = sid;
	}

	public bool encode()
	{
		return false;
	}

	public void headerBuffInfo(out NetPacketBufferInfo bufferInfo)
	{
		bufferInfo = new NetPacketBufferInfo
		{
			buffer = _headerAndBodyLengthBuff.Bytes,
			offset = 0,
			length = 1
		};
	}

	public void bodyLengthBuffInfo(out NetPacketBufferInfo bufferInfo)
	{
		bufferInfo = new NetPacketBufferInfo
		{
			buffer = _headerAndBodyLengthBuff.Bytes,
			offset = _bodyLengthBuffOffset,
			length = _bodyLengthBuffLength
		};
	}

	public byte[] bodyBuffBytes()
	{
		return _bodyBuff;
	}

	public int bodyBuffLength()
	{
		return _bodyBuffLen;
	}

	public void onBeforeSend(IMessage msg)
	{
		ByteArray byteArray = msg.Content.ToBinary();
		bool flag = byteArray.Length > 1024;
		if (byteArray.Length > 1000000)
		{
			throw new SFSCodecError("Message size is too big: " + byteArray.Length + ", the server limit is: " + 1000000);
		}
		int num = SFSIOHandler.SHORT_BYTE_SIZE;
		if (byteArray.Length > 65535)
		{
			num = SFSIOHandler.INT_BYTE_SIZE;
		}
		header = new package_header_new(encrypted: true, flag, flag && U, num == SFSIOHandler.INT_BYTE_SIZE, NetPacketConst.useForwardServerId);
		buffer = new ByteArray();
		int length = byteArray.Length;
		bool flag2 = header.Compressed && U;
		if (header.Compressed)
		{
			if (U)
			{
				using MemoryStream memoryStream = new MemoryStream();
				using LZ4Stream lZ4Stream = new LZ4Stream(memoryStream, LZ4StreamMode.Compress);
				lZ4Stream.Write(byteArray.Bytes, 0, byteArray.Length);
				byteArray.Bytes = memoryStream.ToArray();
				byteArray.Position = 0;
			}
			else
			{
				byteArray.Compress();
			}
		}
		buffer.WriteByte(header.Encode());
		if (header.Forward)
		{
			buffer.WriteUShort(_sid);
		}
		if (header.BigSized)
		{
			buffer.WriteInt(byteArray.Length);
		}
		else
		{
			buffer.WriteUShort(Convert.ToUInt16(byteArray.Length));
		}
		if (flag2)
		{
			buffer.WriteInt(length);
		}
		if (header.Encrypted)
		{
			buffer.WriteBytes(Encrypt(byteArray.Bytes));
		}
		else
		{
			buffer.WriteBytes(byteArray.Bytes);
		}
	}

	private byte[] Encrypt(byte[] data)
	{
		byte[] bytes = BitConverter.GetBytes(data.Length);
		int num = data.Length;
		for (int i = 0; i < num; i++)
		{
			data[i] ^= bytes[i % bytes.Length];
		}
		return data;
	}

	private byte[] Dencrypt(byte[] data, int len, int offset = 0)
	{
		byte[] bytes = BitConverter.GetBytes(len - offset);
		for (int i = offset; i < len; i++)
		{
			data[i] ^= bytes[i % bytes.Length];
		}
		return data;
	}

	public void parseHeader()
	{
		_headerAndBodyLengthBuff.Position = 0;
		byte b = _headerAndBodyLengthBuff.ReadByte();
		if (~(b & 0x80) > 0)
		{
			throw new SFSError(("Unexpected header byte: " + b + "\n") ?? "");
		}
		header = package_header_new.FromBinary(b);
		bool flag = header.Compressed && header.UseLZ4;
		if (header.BigSized)
		{
			_bodyLengthBuffLength = 4 + (flag ? 4 : 0);
		}
		else
		{
			_bodyLengthBuffLength = 2 + (flag ? 4 : 0);
		}
	}

	public void parseBodyLength()
	{
		int num = 0;
		_headerAndBodyLengthBuff.Position = _bodyLengthBuffOffset;
		num = ((!header.BigSized) ? _headerAndBodyLengthBuff.ReadUShort() : _headerAndBodyLengthBuff.ReadInt());
		header.ExpectedLength = num;
		if (header.Compressed && header.UseLZ4)
		{
			header.UnCompressLength = _headerAndBodyLengthBuff.ReadInt();
		}
		else
		{
			header.UnCompressLength = 0;
		}
		_sharedBufferManager.Free(_headerAndBodyLengthBuff.Bytes);
		_headerAndBodyLengthBuff.Dispose();
		_headerAndBodyLengthBuff = null;
		_bodyBuffLen = num;
		_bodyBuff = _sharedBufferManager.Allocate(num);
	}

	public void onPackageReceiveBegin()
	{
		_profiler.OnReceivePacketBegin(this);
	}

	public void onPackageReceiveFinish()
	{
		IByteArray byteArray = null;
		try
		{
			_profiler.OnReceivePacketFinish(this);
			if (header.Encrypted)
			{
				_profiler.OnReceiveDecryptBegin(this);
				_bodyBuff = Dencrypt(_bodyBuff, _bodyBuffLen);
				_profiler.OnReceiveDecryptFinish(this);
			}
			if (header.Compressed)
			{
				_profiler.OnReceiveUncompressBegin(this);
				if (header.UseLZ4)
				{
					ArraySegment<byte> src = new ArraySegment<byte>(_bodyBuff, 0, _bodyBuffLen);
					byte[] array = ArrayPool<byte>.Shared.Rent(header.UnCompressLength);
					_ztsdDecompressor.Unwrap(src, array, 0);
					_sharedBufferManager.Free(_bodyBuff);
					_bodyBuff = array;
					_bodyBuffLen = header.UnCompressLength;
					byteArray = new ReadOnlyByteArray(_bodyBuff, _bodyBuffLen, _sharedBufferManager);
				}
				else
				{
					MemoryStream memoryStream = new MemoryStream();
					ReadOnlyByteArray.ZlibCompressTo(memoryStream, _bodyBuff, 0, _bodyBuffLen);
					_sharedBufferManager.Free(_bodyBuff);
					_bodyBuff = null;
					_bodyBuffLen = 0;
					byte[] array2 = memoryStream.ToArray();
					byteArray = new ReadOnlyByteArray(array2, array2.Length, _sharedBufferManager);
				}
				_profiler.OnReceiveUncompressFinish(this);
			}
			else
			{
				byteArray = new ReadOnlyByteArray(_bodyBuff, _bodyBuffLen, _sharedBufferManager);
			}
			_profiler.OnReceiveDecodeSFSObjectBegin(this);
			info = SFSObject.NewFromIBinaryData(byteArray);
			_profiler.OnReceiveDecodeSFSObjectFinish(this);
		}
		catch (Exception ex)
		{
			Log.Error("parse mesg error {0}", ex.StackTrace);
		}
		finally
		{
			if (_bodyBuff != null)
			{
				_sharedBufferManager.Free(_bodyBuff);
				_bodyBuff = null;
				_bodyBuffLen = 0;
			}
			(byteArray as IDisposable)?.Dispose();
		}
	}

	public void onPackageSendBegin()
	{
		_profiler.OnSendPacketBegin(this);
	}

	public void onPackageSendFinish()
	{
		_profiler.OnSendPacketFinish(this);
	}

	public void Dispose()
	{
		if (_headerAndBodyLengthBuff != null)
		{
			_sharedBufferManager.Free(_headerAndBodyLengthBuff.Bytes);
			_headerAndBodyLengthBuff.Dispose();
			_headerAndBodyLengthBuff = null;
		}
		if (_bodyBuff != null)
		{
			_sharedBufferManager.Free(_bodyBuff);
			_bodyBuff = null;
			_bodyBuffLen = 0;
		}
	}
}
