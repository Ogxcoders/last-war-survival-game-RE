using System;
using System.IO;
using BestHTTP.Extensions;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

internal sealed class RecordStream
{
	private class HandshakeHashUpdateStream : BaseOutputStream
	{
		private readonly RecordStream mOuter;

		public HandshakeHashUpdateStream(RecordStream mOuter)
		{
			this.mOuter = mOuter;
		}

		public override void Write(byte[] buf, int off, int len)
		{
			mOuter.mHandshakeHash.BlockUpdate(buf, off, len);
		}
	}

	private class SequenceNumber
	{
		private long value;

		private bool exhausted;

		internal long NextValue(byte alertDescription)
		{
			if (exhausted)
			{
				throw new TlsFatalAlert(alertDescription);
			}
			long result = value;
			if (++value == 0L)
			{
				exhausted = true;
			}
			return result;
		}
	}

	private const int DEFAULT_PLAINTEXT_LIMIT = 16384;

	internal const int TLS_HEADER_SIZE = 5;

	internal const int TLS_HEADER_TYPE_OFFSET = 0;

	internal const int TLS_HEADER_VERSION_OFFSET = 1;

	internal const int TLS_HEADER_LENGTH_OFFSET = 3;

	private TlsProtocol mHandler;

	private Stream mInput;

	private Stream mOutput;

	private TlsCompression mPendingCompression;

	private TlsCompression mReadCompression;

	private TlsCompression mWriteCompression;

	private TlsCipher mPendingCipher;

	private TlsCipher mReadCipher;

	private TlsCipher mWriteCipher;

	private SequenceNumber mReadSeqNo = new SequenceNumber();

	private SequenceNumber mWriteSeqNo = new SequenceNumber();

	private MemoryStream mBuffer = new MemoryStream();

	private TlsHandshakeHash mHandshakeHash;

	private readonly BaseOutputStream mHandshakeHashUpdater;

	private ProtocolVersion mReadVersion;

	private ProtocolVersion mWriteVersion;

	private bool mRestrictReadVersion = true;

	private int mPlaintextLimit;

	private int mCompressedLimit;

	private int mCiphertextLimit;

	internal ProtocolVersion ReadVersion
	{
		get
		{
			return mReadVersion;
		}
		set
		{
			mReadVersion = value;
		}
	}

	internal TlsHandshakeHash HandshakeHash => mHandshakeHash;

	internal Stream HandshakeHashUpdater => mHandshakeHashUpdater;

	internal RecordStream(TlsProtocol handler, Stream input, Stream output)
	{
		mHandler = handler;
		mInput = input;
		mOutput = output;
		mReadCompression = new TlsNullCompression();
		mWriteCompression = mReadCompression;
		mHandshakeHashUpdater = new HandshakeHashUpdateStream(this);
	}

	internal void Init(TlsContext context)
	{
		mReadCipher = new TlsNullCipher(context);
		mWriteCipher = mReadCipher;
		mHandshakeHash = new DeferredHash();
		mHandshakeHash.Init(context);
		SetPlaintextLimit(16384);
	}

	internal int GetPlaintextLimit()
	{
		return mPlaintextLimit;
	}

	internal void SetPlaintextLimit(int plaintextLimit)
	{
		mPlaintextLimit = plaintextLimit;
		mCompressedLimit = mPlaintextLimit + 1024;
		mCiphertextLimit = mCompressedLimit + 1024;
	}

	internal void SetWriteVersion(ProtocolVersion writeVersion)
	{
		mWriteVersion = writeVersion;
	}

	internal void SetRestrictReadVersion(bool enabled)
	{
		mRestrictReadVersion = enabled;
	}

	internal void SetPendingConnectionState(TlsCompression tlsCompression, TlsCipher tlsCipher)
	{
		mPendingCompression = tlsCompression;
		mPendingCipher = tlsCipher;
	}

	internal void SentWriteCipherSpec()
	{
		if (mPendingCompression == null || mPendingCipher == null)
		{
			throw new TlsFatalAlert(40);
		}
		mWriteCompression = mPendingCompression;
		mWriteCipher = mPendingCipher;
		mWriteSeqNo = new SequenceNumber();
	}

	internal void ReceivedReadCipherSpec()
	{
		if (mPendingCompression == null || mPendingCipher == null)
		{
			throw new TlsFatalAlert(40);
		}
		mReadCompression = mPendingCompression;
		mReadCipher = mPendingCipher;
		mReadSeqNo = new SequenceNumber();
	}

	internal void FinaliseHandshake()
	{
		if (mReadCompression != mPendingCompression || mWriteCompression != mPendingCompression || mReadCipher != mPendingCipher || mWriteCipher != mPendingCipher)
		{
			throw new TlsFatalAlert(40);
		}
		mPendingCompression = null;
		mPendingCipher = null;
	}

	internal void CheckRecordHeader(byte[] recordHeader)
	{
		CheckType(TlsUtilities.ReadUint8(recordHeader, 0), 10);
		if (!mRestrictReadVersion)
		{
			if ((TlsUtilities.ReadVersionRaw(recordHeader, 1) & 0xFFFFFF00u) != 768)
			{
				throw new TlsFatalAlert(47);
			}
		}
		else
		{
			ProtocolVersion protocolVersion = TlsUtilities.ReadVersion(recordHeader, 1);
			if (mReadVersion != null && !protocolVersion.Equals(mReadVersion))
			{
				throw new TlsFatalAlert(47);
			}
		}
		CheckLength(TlsUtilities.ReadUint16(recordHeader, 3), mCiphertextLimit, 22);
	}

	internal bool ReadRecord()
	{
		byte[] array = TlsUtilities.ReadAllOrNothing(5, mInput);
		if (array == null)
		{
			return false;
		}
		byte b = TlsUtilities.ReadUint8(array, 0);
		CheckType(b, 10);
		if (!mRestrictReadVersion)
		{
			if ((TlsUtilities.ReadVersionRaw(array, 1) & 0xFFFFFF00u) != 768)
			{
				throw new TlsFatalAlert(47);
			}
		}
		else
		{
			ProtocolVersion protocolVersion = TlsUtilities.ReadVersion(array, 1);
			if (mReadVersion == null)
			{
				mReadVersion = protocolVersion;
			}
			else if (!protocolVersion.Equals(mReadVersion))
			{
				throw new TlsFatalAlert(47);
			}
		}
		int num = TlsUtilities.ReadUint16(array, 3);
		CheckLength(num, mCiphertextLimit, 22);
		byte[] array2 = DecodeAndVerify(b, mInput, num);
		mHandler.ProcessRecord(b, array2, 0, array2.Length);
		return true;
	}

	internal byte[] DecodeAndVerify(byte type, Stream input, int len)
	{
		byte[] array = TlsUtilities.ReadFully(len, input);
		long seqNo = mReadSeqNo.NextValue(10);
		byte[] array2 = mReadCipher.DecodeCiphertext(seqNo, type, array, 0, array.Length);
		CheckLength(array2.Length, mCompressedLimit, 22);
		Stream stream = mReadCompression.Decompress(mBuffer);
		if (stream != mBuffer)
		{
			stream.Write(array2, 0, array2.Length);
			stream.Flush();
			array2 = GetBufferContents();
		}
		CheckLength(array2.Length, mPlaintextLimit, 30);
		if (array2.Length < 1 && type != 23)
		{
			throw new TlsFatalAlert(47);
		}
		return array2;
	}

	internal void WriteRecord(byte type, byte[] plaintext, int plaintextOffset, int plaintextLength)
	{
		if (mWriteVersion != null)
		{
			CheckType(type, 80);
			CheckLength(plaintextLength, mPlaintextLimit, 80);
			if (plaintextLength < 1 && type != 23)
			{
				throw new TlsFatalAlert(80);
			}
			Stream stream = mWriteCompression.Compress(mBuffer);
			long seqNo = mWriteSeqNo.NextValue(80);
			byte[] array;
			if (stream == mBuffer)
			{
				array = mWriteCipher.EncodePlaintext(seqNo, type, plaintext, plaintextOffset, plaintextLength);
			}
			else
			{
				stream.Write(plaintext, plaintextOffset, plaintextLength);
				stream.Flush();
				byte[] bufferContents = GetBufferContents();
				CheckLength(bufferContents.Length, plaintextLength + 1024, 80);
				array = mWriteCipher.EncodePlaintext(seqNo, type, bufferContents, 0, bufferContents.Length);
			}
			CheckLength(array.Length, mCiphertextLimit, 80);
			int num = array.Length + 5;
			byte[] array2 = VariableSizedBufferPool.Get(num, canBeLarger: true);
			TlsUtilities.WriteUint8(type, array2, 0);
			TlsUtilities.WriteVersion(mWriteVersion, array2, 1);
			TlsUtilities.WriteUint16(array.Length, array2, 3);
			Array.Copy(array, 0, array2, 5, array.Length);
			mOutput.Write(array2, 0, num);
			VariableSizedBufferPool.Release(array2);
			mOutput.Flush();
		}
	}

	internal void NotifyHelloComplete()
	{
		mHandshakeHash = mHandshakeHash.NotifyPrfDetermined();
	}

	internal TlsHandshakeHash PrepareToFinish()
	{
		TlsHandshakeHash result = mHandshakeHash;
		mHandshakeHash = mHandshakeHash.StopTracking();
		return result;
	}

	internal void SafeClose()
	{
		try
		{
			Platform.Dispose(mInput);
		}
		catch (IOException)
		{
		}
		try
		{
			Platform.Dispose(mOutput);
		}
		catch (IOException)
		{
		}
	}

	internal void Flush()
	{
		mOutput.Flush();
	}

	private byte[] GetBufferContents()
	{
		byte[] result = mBuffer.ToArray();
		mBuffer.SetLength(0L);
		return result;
	}

	private static void CheckType(byte type, byte alertDescription)
	{
		if ((uint)(type - 20) > 3u)
		{
			throw new TlsFatalAlert(alertDescription);
		}
	}

	private static void CheckLength(int length, int limit, byte alertDescription)
	{
		if (length > limit)
		{
			throw new TlsFatalAlert(alertDescription);
		}
	}
}
