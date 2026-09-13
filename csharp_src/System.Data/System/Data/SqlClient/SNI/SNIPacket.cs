using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.SqlClient.SNI;

internal class SNIPacket : IDisposable, IEquatable<SNIPacket>
{
	private byte[] _data;

	private int _length;

	private int _capacity;

	private int _offset;

	private string _description;

	private SNIAsyncCallback _completionCallback;

	private ArrayPool<byte> _arrayPool = ArrayPool<byte>.Shared;

	private bool _isBufferFromArrayPool;

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value;
		}
	}

	public int DataLeft => _length - _offset;

	public int Length => _length;

	public bool IsInvalid => _data == null;

	public SNIPacket()
	{
	}

	public SNIPacket(int capacity)
	{
		Allocate(capacity);
	}

	public void Dispose()
	{
		Release();
	}

	public void SetCompletionCallback(SNIAsyncCallback completionCallback)
	{
		_completionCallback = completionCallback;
	}

	public void InvokeCompletionCallback(uint sniErrorCode)
	{
		_completionCallback(this, sniErrorCode);
	}

	public void Allocate(int capacity)
	{
		if (_data != null && _data.Length < capacity)
		{
			if (_isBufferFromArrayPool)
			{
				_arrayPool.Return(_data);
			}
			_data = null;
		}
		if (_data == null)
		{
			_data = _arrayPool.Rent(capacity);
			_isBufferFromArrayPool = true;
		}
		_capacity = capacity;
		_length = 0;
		_offset = 0;
	}

	public SNIPacket Clone()
	{
		SNIPacket sNIPacket = new SNIPacket(_capacity);
		Buffer.BlockCopy(_data, 0, sNIPacket._data, 0, _capacity);
		sNIPacket._length = _length;
		sNIPacket._description = _description;
		sNIPacket._completionCallback = _completionCallback;
		return sNIPacket;
	}

	public void GetData(byte[] buffer, ref int dataSize)
	{
		Buffer.BlockCopy(_data, 0, buffer, 0, _length);
		dataSize = _length;
	}

	public void SetData(byte[] data, int length)
	{
		_data = data;
		_length = length;
		_capacity = data.Length;
		_offset = 0;
		_isBufferFromArrayPool = false;
	}

	public int TakeData(SNIPacket packet, int size)
	{
		int num = TakeData(packet._data, packet._length, size);
		packet._length += num;
		return num;
	}

	public void AppendData(byte[] data, int size)
	{
		Buffer.BlockCopy(data, 0, _data, _length, size);
		_length += size;
	}

	public void AppendPacket(SNIPacket packet)
	{
		Buffer.BlockCopy(packet._data, 0, _data, _length, packet._length);
		_length += packet._length;
	}

	public int TakeData(byte[] buffer, int dataOffset, int size)
	{
		if (_offset >= _length)
		{
			return 0;
		}
		if (_offset + size > _length)
		{
			size = _length - _offset;
		}
		Buffer.BlockCopy(_data, _offset, buffer, dataOffset, size);
		_offset += size;
		return size;
	}

	public void Release()
	{
		if (_data != null)
		{
			if (_isBufferFromArrayPool)
			{
				_arrayPool.Return(_data);
			}
			_data = null;
			_capacity = 0;
		}
		Reset();
	}

	public void Reset()
	{
		_length = 0;
		_offset = 0;
		_description = null;
		_completionCallback = null;
	}

	public void ReadFromStreamAsync(Stream stream, SNIAsyncCallback callback)
	{
		bool error = false;
		stream.ReadAsync(_data, 0, _capacity, CancellationToken.None).ContinueWith(delegate(Task<int> t)
		{
			Exception ex = t.Exception?.InnerException;
			if (ex != null)
			{
				SNILoadHandle.SingletonInstance.LastError = new SNIError(SNIProviders.TCP_PROV, 35u, ex);
				error = true;
			}
			else
			{
				_length = t.Result;
				if (_length == 0)
				{
					SNILoadHandle.SingletonInstance.LastError = new SNIError(SNIProviders.TCP_PROV, 0u, 2u, string.Empty);
					error = true;
				}
			}
			if (error)
			{
				Release();
			}
			callback(this, error ? 1u : 0u);
		}, CancellationToken.None, TaskContinuationOptions.DenyChildAttach, TaskScheduler.Default);
	}

	public void ReadFromStream(Stream stream)
	{
		_length = stream.Read(_data, 0, _capacity);
	}

	public void WriteToStream(Stream stream)
	{
		stream.Write(_data, 0, _length);
	}

	public async void WriteToStreamAsync(Stream stream, SNIAsyncCallback callback, SNIProviders provider, bool disposeAfterWriteAsync = false)
	{
		uint status = 0u;
		try
		{
			await stream.WriteAsync(_data, 0, _length, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception sniException)
		{
			SNILoadHandle.SingletonInstance.LastError = new SNIError(provider, 35u, sniException);
			status = 1u;
		}
		callback(this, status);
		if (disposeAfterWriteAsync)
		{
			Dispose();
		}
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj is SNIPacket packet)
		{
			return Equals(packet);
		}
		return false;
	}

	public bool Equals(SNIPacket packet)
	{
		if (packet != null)
		{
			return packet == this;
		}
		return false;
	}
}
