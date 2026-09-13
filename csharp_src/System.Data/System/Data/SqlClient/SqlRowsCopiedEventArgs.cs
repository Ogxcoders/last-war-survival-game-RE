namespace System.Data.SqlClient;

public class SqlRowsCopiedEventArgs : EventArgs
{
	private bool _abort;

	private long _rowsCopied;

	public bool Abort
	{
		get
		{
			return _abort;
		}
		set
		{
			_abort = value;
		}
	}

	public long RowsCopied => _rowsCopied;

	public SqlRowsCopiedEventArgs(long rowsCopied)
	{
		_rowsCopied = rowsCopied;
	}
}
