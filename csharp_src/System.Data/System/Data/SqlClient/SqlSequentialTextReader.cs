using System.Data.Common;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.SqlClient;

internal sealed class SqlSequentialTextReader : TextReader
{
	private SqlDataReader _reader;

	private int _columnIndex;

	private Encoding _encoding;

	private Decoder _decoder;

	private byte[] _leftOverBytes;

	private int _peekedChar;

	private Task _currentTask;

	private CancellationTokenSource _disposalTokenSource;

	internal int ColumnIndex => _columnIndex;

	private bool IsClosed => _reader == null;

	private bool HasPeekedChar => _peekedChar >= 0;

	internal SqlSequentialTextReader(SqlDataReader reader, int columnIndex, Encoding encoding)
	{
		_reader = reader;
		_columnIndex = columnIndex;
		_encoding = encoding;
		_decoder = encoding.GetDecoder();
		_leftOverBytes = null;
		_peekedChar = -1;
		_currentTask = null;
		_disposalTokenSource = new CancellationTokenSource();
	}

	public override int Peek()
	{
		if (_currentTask != null)
		{
			throw ADP.AsyncOperationPending();
		}
		if (IsClosed)
		{
			throw ADP.ObjectDisposed(this);
		}
		if (!HasPeekedChar)
		{
			_peekedChar = Read();
		}
		return _peekedChar;
	}

	public override int Read()
	{
		if (_currentTask != null)
		{
			throw ADP.AsyncOperationPending();
		}
		if (IsClosed)
		{
			throw ADP.ObjectDisposed(this);
		}
		int result = -1;
		if (HasPeekedChar)
		{
			result = _peekedChar;
			_peekedChar = -1;
		}
		else
		{
			char[] array = new char[1];
			if (InternalRead(array, 0, 1) == 1)
			{
				result = array[0];
			}
		}
		return result;
	}

	public override int Read(char[] buffer, int index, int count)
	{
		ValidateReadParameters(buffer, index, count);
		if (IsClosed)
		{
			throw ADP.ObjectDisposed(this);
		}
		if (_currentTask != null)
		{
			throw ADP.AsyncOperationPending();
		}
		int num = 0;
		int num2 = count;
		if (num2 > 0 && HasPeekedChar)
		{
			buffer[index + num] = (char)_peekedChar;
			num++;
			num2--;
			_peekedChar = -1;
		}
		return num + InternalRead(buffer, index + num, num2);
	}

	public override Task<int> ReadAsync(char[] buffer, int index, int count)
	{
		ValidateReadParameters(buffer, index, count);
		TaskCompletionSource<int> completion = new TaskCompletionSource<int>();
		if (IsClosed)
		{
			completion.SetException(ADP.ExceptionWithStackTrace(ADP.ObjectDisposed(this)));
		}
		else
		{
			try
			{
				if (Interlocked.CompareExchange(ref _currentTask, completion.Task, null) != null)
				{
					completion.SetException(ADP.ExceptionWithStackTrace(ADP.AsyncOperationPending()));
				}
				else
				{
					bool flag = true;
					int charsRead = 0;
					int adjustedIndex = index;
					int charsNeeded = count;
					if (HasPeekedChar && charsNeeded > 0)
					{
						int peekedChar = _peekedChar;
						if (peekedChar >= 0)
						{
							buffer[adjustedIndex] = (char)peekedChar;
							adjustedIndex++;
							charsRead++;
							charsNeeded--;
							_peekedChar = -1;
						}
					}
					int byteBufferUsed;
					byte[] byteBuffer = PrepareByteBuffer(charsNeeded, out byteBufferUsed);
					if (byteBufferUsed < byteBuffer.Length || byteBuffer.Length == 0)
					{
						SqlDataReader reader = _reader;
						if (reader != null)
						{
							int bytesRead;
							Task<int> bytesAsync = reader.GetBytesAsync(_columnIndex, byteBuffer, byteBufferUsed, byteBuffer.Length - byteBufferUsed, -1, _disposalTokenSource.Token, out bytesRead);
							if (bytesAsync == null)
							{
								byteBufferUsed += bytesRead;
							}
							else
							{
								flag = false;
								bytesAsync.ContinueWith(delegate(Task<int> t)
								{
									_currentTask = null;
									if (t.Status == TaskStatus.RanToCompletion && !IsClosed)
									{
										try
										{
											int result = t.Result;
											byteBufferUsed += result;
											if (byteBufferUsed > 0)
											{
												charsRead += DecodeBytesToChars(byteBuffer, byteBufferUsed, buffer, adjustedIndex, charsNeeded);
											}
											completion.SetResult(charsRead);
											return;
										}
										catch (Exception exception2)
										{
											completion.SetException(exception2);
											return;
										}
									}
									if (IsClosed)
									{
										completion.SetException(ADP.ExceptionWithStackTrace(ADP.ObjectDisposed(this)));
									}
									else if (t.Status == TaskStatus.Faulted)
									{
										if (t.Exception.InnerException is SqlException)
										{
											completion.SetException(ADP.ExceptionWithStackTrace(ADP.ErrorReadingFromStream(t.Exception.InnerException)));
										}
										else
										{
											completion.SetException(t.Exception.InnerException);
										}
									}
									else
									{
										completion.SetCanceled();
									}
								}, TaskScheduler.Default);
							}
							if (flag && byteBufferUsed > 0)
							{
								charsRead += DecodeBytesToChars(byteBuffer, byteBufferUsed, buffer, adjustedIndex, charsNeeded);
							}
						}
						else
						{
							completion.SetException(ADP.ExceptionWithStackTrace(ADP.ObjectDisposed(this)));
						}
					}
					if (flag)
					{
						_currentTask = null;
						if (IsClosed)
						{
							completion.SetCanceled();
						}
						else
						{
							completion.SetResult(charsRead);
						}
					}
				}
			}
			catch (Exception exception)
			{
				completion.TrySetException(exception);
				Interlocked.CompareExchange(ref _currentTask, null, completion.Task);
				throw;
			}
		}
		return completion.Task;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			SetClosed();
		}
		base.Dispose(disposing);
	}

	internal void SetClosed()
	{
		_disposalTokenSource.Cancel();
		_reader = null;
		_peekedChar = -1;
		((IAsyncResult)_currentTask)?.AsyncWaitHandle.WaitOne();
	}

	private int InternalRead(char[] buffer, int index, int count)
	{
		try
		{
			int byteBufferUsed;
			byte[] array = PrepareByteBuffer(count, out byteBufferUsed);
			byteBufferUsed += _reader.GetBytesInternalSequential(_columnIndex, array, byteBufferUsed, array.Length - byteBufferUsed);
			if (byteBufferUsed > 0)
			{
				return DecodeBytesToChars(array, byteBufferUsed, buffer, index, count);
			}
			return 0;
		}
		catch (SqlException internalException)
		{
			throw ADP.ErrorReadingFromStream(internalException);
		}
	}

	private byte[] PrepareByteBuffer(int numberOfChars, out int byteBufferUsed)
	{
		byte[] array;
		if (numberOfChars == 0)
		{
			array = Array.Empty<byte>();
			byteBufferUsed = 0;
		}
		else
		{
			int maxByteCount = _encoding.GetMaxByteCount(numberOfChars);
			if (_leftOverBytes != null)
			{
				if (_leftOverBytes.Length > maxByteCount)
				{
					array = _leftOverBytes;
					byteBufferUsed = array.Length;
				}
				else
				{
					array = new byte[maxByteCount];
					Buffer.BlockCopy(_leftOverBytes, 0, array, 0, _leftOverBytes.Length);
					byteBufferUsed = _leftOverBytes.Length;
				}
			}
			else
			{
				array = new byte[maxByteCount];
				byteBufferUsed = 0;
			}
		}
		return array;
	}

	private int DecodeBytesToChars(byte[] inBuffer, int inBufferCount, char[] outBuffer, int outBufferOffset, int outBufferCount)
	{
		_decoder.Convert(inBuffer, 0, inBufferCount, outBuffer, outBufferOffset, outBufferCount, flush: false, out var bytesUsed, out var charsUsed, out var completed);
		if (!completed && bytesUsed < inBufferCount)
		{
			_leftOverBytes = new byte[inBufferCount - bytesUsed];
			Buffer.BlockCopy(inBuffer, bytesUsed, _leftOverBytes, 0, _leftOverBytes.Length);
		}
		else
		{
			_leftOverBytes = null;
		}
		return charsUsed;
	}

	internal static void ValidateReadParameters(char[] buffer, int index, int count)
	{
		if (buffer == null)
		{
			throw ADP.ArgumentNull("buffer");
		}
		if (index < 0)
		{
			throw ADP.ArgumentOutOfRange("index");
		}
		if (count < 0)
		{
			throw ADP.ArgumentOutOfRange("count");
		}
		try
		{
			if (checked(index + count) > buffer.Length)
			{
				throw ExceptionBuilder.InvalidOffsetLength();
			}
		}
		catch (OverflowException)
		{
			throw ExceptionBuilder.InvalidOffsetLength();
		}
	}
}
