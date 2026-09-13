using System.IO;

namespace System.Runtime;

internal class BufferedOutputStream : Stream
{
	private InternalBufferManager bufferManager;

	private byte[][] chunks;

	private int chunkCount;

	private byte[] currentChunk;

	private int currentChunkSize;

	private int maxSize;

	private int maxSizeQuota;

	private int totalSize;

	private bool callerReturnsBuffer;

	private bool bufferReturned;

	private bool initialized;

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => true;

	public override long Length => totalSize;

	public override long Position
	{
		get
		{
			throw Fx.Exception.AsError(new NotSupportedException("Seek Not Supported"));
		}
		set
		{
			throw Fx.Exception.AsError(new NotSupportedException("Seek Not Supported"));
		}
	}

	public BufferedOutputStream()
	{
		chunks = new byte[4][];
	}

	public BufferedOutputStream(int initialSize, int maxSize, InternalBufferManager bufferManager)
		: this()
	{
		Reinitialize(initialSize, maxSize, bufferManager);
	}

	public BufferedOutputStream(int maxSize)
		: this(0, maxSize, InternalBufferManager.Create(0L, int.MaxValue))
	{
	}

	public void Reinitialize(int initialSize, int maxSizeQuota, InternalBufferManager bufferManager)
	{
		Reinitialize(initialSize, maxSizeQuota, maxSizeQuota, bufferManager);
	}

	public void Reinitialize(int initialSize, int maxSizeQuota, int effectiveMaxSize, InternalBufferManager bufferManager)
	{
		this.maxSizeQuota = maxSizeQuota;
		maxSize = effectiveMaxSize;
		this.bufferManager = bufferManager;
		currentChunk = bufferManager.TakeBuffer(initialSize);
		currentChunkSize = 0;
		totalSize = 0;
		chunkCount = 1;
		chunks[0] = currentChunk;
		initialized = true;
	}

	private void AllocNextChunk(int minimumChunkSize)
	{
		int num = ((currentChunk.Length <= 1073741823) ? (currentChunk.Length * 2) : int.MaxValue);
		if (minimumChunkSize > num)
		{
			num = minimumChunkSize;
		}
		byte[] array = bufferManager.TakeBuffer(num);
		if (chunkCount == chunks.Length)
		{
			byte[][] destinationArray = new byte[chunks.Length * 2][];
			Array.Copy(chunks, destinationArray, chunks.Length);
			chunks = destinationArray;
		}
		chunks[chunkCount++] = array;
		currentChunk = array;
		currentChunkSize = 0;
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
	{
		throw Fx.Exception.AsError(new NotSupportedException("Read Not Supported"));
	}

	public override int EndRead(IAsyncResult result)
	{
		throw Fx.Exception.AsError(new NotSupportedException("Read Not Supported"));
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
	{
		Write(buffer, offset, size);
		return new CompletedAsyncResult(callback, state);
	}

	public override void EndWrite(IAsyncResult result)
	{
		CompletedAsyncResult.End(result);
	}

	public void Clear()
	{
		if (!callerReturnsBuffer)
		{
			for (int i = 0; i < chunkCount; i++)
			{
				bufferManager.ReturnBuffer(chunks[i]);
				chunks[i] = null;
			}
		}
		callerReturnsBuffer = false;
		initialized = false;
		bufferReturned = false;
		chunkCount = 0;
		currentChunk = null;
	}

	public override void Close()
	{
	}

	public override void Flush()
	{
	}

	public override int Read(byte[] buffer, int offset, int size)
	{
		throw Fx.Exception.AsError(new NotSupportedException("Read Not Supported"));
	}

	public override int ReadByte()
	{
		throw Fx.Exception.AsError(new NotSupportedException("Read Not Supported"));
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw Fx.Exception.AsError(new NotSupportedException("Seek Not Supported"));
	}

	public override void SetLength(long value)
	{
		throw Fx.Exception.AsError(new NotSupportedException("Seek Not Supported"));
	}

	public MemoryStream ToMemoryStream()
	{
		int bufferSize;
		return new MemoryStream(ToArray(out bufferSize), 0, bufferSize);
	}

	public byte[] ToArray(out int bufferSize)
	{
		byte[] array;
		if (chunkCount == 1)
		{
			array = currentChunk;
			bufferSize = currentChunkSize;
			callerReturnsBuffer = true;
		}
		else
		{
			array = bufferManager.TakeBuffer(totalSize);
			int num = 0;
			int num2 = chunkCount - 1;
			for (int i = 0; i < num2; i++)
			{
				byte[] array2 = chunks[i];
				Buffer.BlockCopy(array2, 0, array, num, array2.Length);
				num += array2.Length;
			}
			Buffer.BlockCopy(currentChunk, 0, array, num, currentChunkSize);
			bufferSize = totalSize;
		}
		bufferReturned = true;
		return array;
	}

	public void Skip(int size)
	{
		WriteCore(null, 0, size);
	}

	public override void Write(byte[] buffer, int offset, int size)
	{
		WriteCore(buffer, offset, size);
	}

	protected virtual Exception CreateQuotaExceededException(int maxSizeQuota)
	{
		return new InvalidOperationException(InternalSR.BufferedOutputStreamQuotaExceeded(maxSizeQuota));
	}

	private void WriteCore(byte[] buffer, int offset, int size)
	{
		if (size < 0)
		{
			throw Fx.Exception.ArgumentOutOfRange("size", size, "Value Must Be Non Negative");
		}
		if (int.MaxValue - size < totalSize)
		{
			throw Fx.Exception.AsError(CreateQuotaExceededException(maxSizeQuota));
		}
		int num = totalSize + size;
		if (num > maxSize)
		{
			throw Fx.Exception.AsError(CreateQuotaExceededException(maxSizeQuota));
		}
		int num2 = currentChunk.Length - currentChunkSize;
		if (size > num2)
		{
			if (num2 > 0)
			{
				if (buffer != null)
				{
					Buffer.BlockCopy(buffer, offset, currentChunk, currentChunkSize, num2);
				}
				currentChunkSize = currentChunk.Length;
				offset += num2;
				size -= num2;
			}
			AllocNextChunk(size);
		}
		if (buffer != null)
		{
			Buffer.BlockCopy(buffer, offset, currentChunk, currentChunkSize, size);
		}
		totalSize = num;
		currentChunkSize += size;
	}

	public override void WriteByte(byte value)
	{
		if (totalSize == maxSize)
		{
			throw Fx.Exception.AsError(CreateQuotaExceededException(maxSize));
		}
		if (currentChunkSize == currentChunk.Length)
		{
			AllocNextChunk(1);
		}
		currentChunk[currentChunkSize++] = value;
	}
}
