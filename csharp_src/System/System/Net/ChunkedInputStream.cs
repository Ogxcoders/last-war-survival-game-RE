using System.IO;
using System.Runtime.InteropServices;

namespace System.Net;

internal class ChunkedInputStream : RequestStream
{
	private class ReadBufferState
	{
		public byte[] Buffer;

		public int Offset;

		public int Count;

		public int InitialCount;

		public HttpStreamAsyncResult Ares;

		public ReadBufferState(byte[] buffer, int offset, int count, HttpStreamAsyncResult ares)
		{
			Buffer = buffer;
			Offset = offset;
			Count = count;
			InitialCount = count;
			Ares = ares;
		}
	}

	private bool disposed;

	private MonoChunkParser decoder;

	private HttpListenerContext context;

	private bool no_more_data;

	public MonoChunkParser Decoder
	{
		get
		{
			return decoder;
		}
		set
		{
			decoder = value;
		}
	}

	public ChunkedInputStream(HttpListenerContext context, Stream stream, byte[] buffer, int offset, int length)
		: base(stream, buffer, offset, length)
	{
		this.context = context;
		WebHeaderCollection headers = (WebHeaderCollection)context.Request.Headers;
		decoder = new MonoChunkParser(headers);
	}

	public override int Read([In][Out] byte[] buffer, int offset, int count)
	{
		IAsyncResult asyncResult = BeginRead(buffer, offset, count, null, null);
		return EndRead(asyncResult);
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		int num = buffer.Length;
		if (offset < 0 || offset > num)
		{
			throw new ArgumentOutOfRangeException("offset exceeds the size of buffer");
		}
		if (count < 0 || offset > num - count)
		{
			throw new ArgumentOutOfRangeException("offset+size exceeds the size of buffer");
		}
		HttpStreamAsyncResult httpStreamAsyncResult = new HttpStreamAsyncResult();
		httpStreamAsyncResult.Callback = cback;
		httpStreamAsyncResult.State = state;
		if (no_more_data)
		{
			httpStreamAsyncResult.Complete();
			return httpStreamAsyncResult;
		}
		int num2 = decoder.Read(buffer, offset, count);
		offset += num2;
		count -= num2;
		if (count == 0)
		{
			httpStreamAsyncResult.Count = num2;
			httpStreamAsyncResult.Complete();
			return httpStreamAsyncResult;
		}
		if (!decoder.WantMore)
		{
			no_more_data = num2 == 0;
			httpStreamAsyncResult.Count = num2;
			httpStreamAsyncResult.Complete();
			return httpStreamAsyncResult;
		}
		httpStreamAsyncResult.Buffer = new byte[8192];
		httpStreamAsyncResult.Offset = 0;
		httpStreamAsyncResult.Count = 8192;
		ReadBufferState readBufferState = new ReadBufferState(buffer, offset, count, httpStreamAsyncResult);
		readBufferState.InitialCount += num2;
		base.BeginRead(httpStreamAsyncResult.Buffer, httpStreamAsyncResult.Offset, httpStreamAsyncResult.Count, (AsyncCallback)OnRead, (object)readBufferState);
		return httpStreamAsyncResult;
	}

	private void OnRead(IAsyncResult base_ares)
	{
		ReadBufferState readBufferState = (ReadBufferState)base_ares.AsyncState;
		HttpStreamAsyncResult ares = readBufferState.Ares;
		try
		{
			int size = base.EndRead(base_ares);
			decoder.Write(ares.Buffer, ares.Offset, size);
			size = decoder.Read(readBufferState.Buffer, readBufferState.Offset, readBufferState.Count);
			readBufferState.Offset += size;
			readBufferState.Count -= size;
			if (readBufferState.Count == 0 || !decoder.WantMore || size == 0)
			{
				no_more_data = !decoder.WantMore && size == 0;
				ares.Count = readBufferState.InitialCount - readBufferState.Count;
				ares.Complete();
			}
			else
			{
				ares.Offset = 0;
				ares.Count = Math.Min(8192, decoder.ChunkLeft + 6);
				base.BeginRead(ares.Buffer, ares.Offset, ares.Count, (AsyncCallback)OnRead, (object)readBufferState);
			}
		}
		catch (Exception ex)
		{
			context.Connection.SendError(ex.Message, 400);
			ares.Complete(ex);
		}
	}

	public override int EndRead(IAsyncResult ares)
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		HttpStreamAsyncResult httpStreamAsyncResult = ares as HttpStreamAsyncResult;
		if (ares == null)
		{
			throw new ArgumentException("Invalid IAsyncResult", "ares");
		}
		if (!ares.IsCompleted)
		{
			ares.AsyncWaitHandle.WaitOne();
		}
		if (httpStreamAsyncResult.Error != null)
		{
			throw new HttpListenerException(400, "I/O operation aborted: " + httpStreamAsyncResult.Error.Message);
		}
		return httpStreamAsyncResult.Count;
	}

	public override void Close()
	{
		if (!disposed)
		{
			disposed = true;
			base.Close();
		}
	}
}
