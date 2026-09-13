using System.Data.Common;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.SqlClient;

internal sealed class SqlSequentialStream : Stream
{
	private SqlDataReader _reader;

	private int _columnIndex;

	private Task _currentTask;

	private int _readTimeout;

	private CancellationTokenSource _disposalTokenSource;

	public override bool CanRead
	{
		get
		{
			if (_reader != null)
			{
				return !_reader.IsClosed;
			}
			return false;
		}
	}

	public override bool CanSeek => false;

	public override bool CanTimeout => true;

	public override bool CanWrite => false;

	public override long Length
	{
		get
		{
			throw ADP.NotSupported();
		}
	}

	public override long Position
	{
		get
		{
			throw ADP.NotSupported();
		}
		set
		{
			throw ADP.NotSupported();
		}
	}

	public override int ReadTimeout
	{
		get
		{
			return _readTimeout;
		}
		set
		{
			if (value > 0 || value == -1)
			{
				_readTimeout = value;
				return;
			}
			throw ADP.ArgumentOutOfRange("value");
		}
	}

	internal int ColumnIndex => _columnIndex;

	internal SqlSequentialStream(SqlDataReader reader, int columnIndex)
	{
		_reader = reader;
		_columnIndex = columnIndex;
		_currentTask = null;
		_disposalTokenSource = new CancellationTokenSource();
		if (reader.Command != null && reader.Command.CommandTimeout != 0)
		{
			_readTimeout = (int)Math.Min((long)reader.Command.CommandTimeout * 1000L, 2147483647L);
		}
		else
		{
			_readTimeout = -1;
		}
	}

	public override void Flush()
	{
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		ValidateReadParameters(buffer, offset, count);
		if (!CanRead)
		{
			throw ADP.ObjectDisposed(this);
		}
		if (_currentTask != null)
		{
			throw ADP.AsyncOperationPending();
		}
		try
		{
			return _reader.GetBytesInternalSequential(_columnIndex, buffer, offset, count, _readTimeout);
		}
		catch (SqlException internalException)
		{
			throw ADP.ErrorReadingFromStream(internalException);
		}
	}

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		ValidateReadParameters(buffer, offset, count);
		TaskCompletionSource<int> completion = new TaskCompletionSource<int>();
		if (!CanRead)
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
					CancellationTokenSource combinedTokenSource;
					if (!cancellationToken.CanBeCanceled)
					{
						combinedTokenSource = _disposalTokenSource;
					}
					else
					{
						combinedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _disposalTokenSource.Token);
					}
					int bytesRead = 0;
					Task<int> task = null;
					SqlDataReader reader = _reader;
					if (reader != null && !cancellationToken.IsCancellationRequested && !_disposalTokenSource.Token.IsCancellationRequested)
					{
						task = reader.GetBytesAsync(_columnIndex, buffer, offset, count, _readTimeout, combinedTokenSource.Token, out bytesRead);
					}
					if (task == null)
					{
						_currentTask = null;
						if (cancellationToken.IsCancellationRequested)
						{
							completion.SetCanceled();
						}
						else if (!CanRead)
						{
							completion.SetException(ADP.ExceptionWithStackTrace(ADP.ObjectDisposed(this)));
						}
						else
						{
							completion.SetResult(bytesRead);
						}
						if (combinedTokenSource != _disposalTokenSource)
						{
							combinedTokenSource.Dispose();
						}
					}
					else
					{
						task.ContinueWith(delegate(Task<int> t)
						{
							_currentTask = null;
							if (t.Status == TaskStatus.RanToCompletion && CanRead)
							{
								completion.SetResult(t.Result);
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
							else if (!CanRead)
							{
								completion.SetException(ADP.ExceptionWithStackTrace(ADP.ObjectDisposed(this)));
							}
							else
							{
								completion.SetCanceled();
							}
							if (combinedTokenSource != _disposalTokenSource)
							{
								combinedTokenSource.Dispose();
							}
						}, TaskScheduler.Default);
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

	public override IAsyncResult BeginRead(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
	{
		return System.Threading.Tasks.TaskToApm.Begin(ReadAsync(array, offset, count, CancellationToken.None), asyncCallback, asyncState);
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		return System.Threading.Tasks.TaskToApm.End<int>(asyncResult);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw ADP.NotSupported();
	}

	public override void SetLength(long value)
	{
		throw ADP.NotSupported();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw ADP.NotSupported();
	}

	internal void SetClosed()
	{
		_disposalTokenSource.Cancel();
		_reader = null;
		((IAsyncResult)_currentTask)?.AsyncWaitHandle.WaitOne();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			SetClosed();
		}
		base.Dispose(disposing);
	}

	internal static void ValidateReadParameters(byte[] buffer, int offset, int count)
	{
		if (buffer == null)
		{
			throw ADP.ArgumentNull("buffer");
		}
		if (offset < 0)
		{
			throw ADP.ArgumentOutOfRange("offset");
		}
		if (count < 0)
		{
			throw ADP.ArgumentOutOfRange("count");
		}
		try
		{
			if (checked(offset + count) > buffer.Length)
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
