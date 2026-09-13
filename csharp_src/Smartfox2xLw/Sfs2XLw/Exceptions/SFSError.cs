using System;

namespace Sfs2XLw.Exceptions;

public class SFSError : Exception
{
	public SFSError(string message)
		: base(message)
	{
	}
}
