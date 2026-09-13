using System;
using GameFramework;
using Sfs2X.Bitswarm;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Exceptions;
using Sfs2X.Protocol.Serialization;
using Sfs2X.Util;

namespace ProtoBufNet;

public class NetPacket : INetPacket, IDisposable
{
	private const int compressionThreshold = 1024;

	private const int maxMessageSize = 1000000;

	private package_header header;

	private ByteArray buffer;

	private ByteArray _headerBuff;

	private ByteArray _bodyLengthBuff;

	private ByteArray _bodyBuff;

	private NetProfiler _profiler;

	private ushort _sid;

	public int bodyLengthHasRead { get; set; }

	public int bodyHasRead { get; set; }

	public ISFSObject info { get; private set; }

	public string logType { get; set; }

	public long recvTime { get; set; }

	public package_header Header => header;

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

	public NetPacket(NetProfiler profiler, ushort sid, bool read = true)
	{
		if (read)
		{
			_headerBuff = new ByteArray(new byte[1]);
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
			buffer = _headerBuff.Bytes,
			offset = 0,
			length = _headerBuff.Length
		};
	}

	public void bodyLengthBuffInfo(out NetPacketBufferInfo bufferInfo)
	{
		bufferInfo = new NetPacketBufferInfo
		{
			buffer = _bodyLengthBuff.Bytes,
			offset = 0,
			length = _bodyLengthBuff.Length
		};
	}

	public byte[] bodyBuffBytes()
	{
		return _bodyBuff.Bytes;
	}

	public int bodyBuffLength()
	{
		return _bodyBuff.Length;
	}

	public void onBeforeSend(IMessage msg)
	{
		ByteArray byteArray = msg.Content.ToBinary();
		bool compressed = byteArray.Length > 1024;
		if (byteArray.Length > 1000000)
		{
			throw new SFSCodecError("Message size is too big: " + byteArray.Length + ", the server limit is: " + 1000000);
		}
		int num = SFSIOHandler.SHORT_BYTE_SIZE;
		if (byteArray.Length > 65535)
		{
			num = SFSIOHandler.INT_BYTE_SIZE;
		}
		header = new package_header(encrypted: true, compressed, blueBoxed: false, num == SFSIOHandler.INT_BYTE_SIZE, NetPacketConst.useForwardServerId);
		buffer = new ByteArray();
		if (header.Compressed)
		{
			byteArray.Compress();
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

	private byte[] Decrypt(byte[] data)
	{
		byte[] bytes = BitConverter.GetBytes(data.Length);
		int num = data.Length;
		for (int i = 0; i < num; i++)
		{
			data[i] ^= bytes[i % bytes.Length];
		}
		return data;
	}

	public void parseHeader()
	{
		byte b = _headerBuff.ReadByte();
		if (~(b & 0x80) > 0)
		{
			throw new SFSError("Unexpected header byte: " + b + "\n" + DefaultObjectDumpFormatter.HexDump(_headerBuff));
		}
		header = package_header.FromBinary(b);
		if (header.BigSized)
		{
			_bodyLengthBuff = new ByteArray(new byte[4]);
		}
		else
		{
			_bodyLengthBuff = new ByteArray(new byte[2]);
		}
	}

	public void parseBodyLength()
	{
		_ = SFSIOHandler.SHORT_BYTE_SIZE;
		int num = 0;
		num = ((!header.BigSized) ? _bodyLengthBuff.ReadUShort() : _bodyLengthBuff.ReadInt());
		header.ExpectedLength = num;
		_bodyBuff = new ByteArray(new byte[num]);
	}

	public void onPackageReceiveBegin()
	{
		_profiler.OnReceivePacketBegin(this);
	}

	public void onPackageReceiveFinish()
	{
		try
		{
			_profiler.OnReceivePacketFinish(this);
			if (header.Encrypted)
			{
				_profiler.OnReceiveDecryptBegin(this);
				_bodyBuff.Bytes = Decrypt(_bodyBuff.Bytes);
				_profiler.OnReceiveDecryptFinish(this);
			}
			if (header.Compressed)
			{
				_profiler.OnReceiveUncompressBegin(this);
				_bodyBuff.Uncompress();
				_profiler.OnReceiveUncompressFinish(this);
			}
			_profiler.OnReceiveDecodeSFSObjectBegin(this);
			info = SFSObject.NewFromBinaryData(_bodyBuff);
			_profiler.OnReceiveDecodeSFSObjectBegin(this);
		}
		catch (Exception ex)
		{
			Log.Error("parse mesg error {0}", ex.StackTrace);
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
	}
}
