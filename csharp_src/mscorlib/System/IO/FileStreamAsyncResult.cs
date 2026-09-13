using System.Threading;

namespace System.IO;

internal class FileStreamAsyncResult : IAsyncResult
{
	private object state;

	private bool completed;

	private bool done;

	private Exception exc;

	private ManualResetEvent wh;

	private AsyncCallback cb;

	private bool completedSynch;

	public byte[] Buffer;

	public int Offset;

	public int Count;

	public int OriginalCount;

	public int BytesRead;

	private AsyncCallback realcb;

	public object AsyncState => state;

	public bool CompletedSynchronously => completedSynch;

	public WaitHandle AsyncWaitHandle => wh;

	public bool IsCompleted => completed;

	public Exception Exception => exc;

	public bool Done
	{
		get
		{
			return done;
		}
		set
		{
			done = value;
		}
	}

	public FileStreamAsyncResult(AsyncCallback cb, object state)
	{
		this.state = state;
		realcb = cb;
		if (realcb != null)
		{
			this.cb = CBWrapper;
		}
		wh = new ManualResetEvent(initialState: false);
	}

	private static void CBWrapper(IAsyncResult ares)
	{
		((FileStreamAsyncResult)ares).realcb.BeginInvoke(ares, null, null);
	}

	public void SetComplete(Exception e)
	{
		exc = e;
		completed = true;
		wh.Set();
		if (cb != null)
		{
			cb(this);
		}
	}

	public void SetComplete(Exception e, int nbytes)
	{
		BytesRead = nbytes;
		SetComplete(e);
	}

	public void SetComplete(Exception e, int nbytes, bool synch)
	{
		completedSynch = synch;
		SetComplete(e, nbytes);
	}
}
