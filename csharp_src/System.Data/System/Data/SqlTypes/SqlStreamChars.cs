using System.IO;

namespace System.Data.SqlTypes;

internal abstract class SqlStreamChars : INullable, IDisposable
{
	public abstract bool IsNull { get; }

	public abstract long Length { get; }

	public abstract long Position { get; set; }

	public abstract int Read(char[] buffer, int offset, int count);

	public abstract void Write(char[] buffer, int offset, int count);

	public abstract long Seek(long offset, SeekOrigin origin);

	public abstract void SetLength(long value);

	void IDisposable.Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
	}
}
