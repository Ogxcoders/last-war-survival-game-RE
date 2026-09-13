using System;

namespace Sfs2XLw.Exceptions;

public class SFSCodecError : Exception
{
	public SFSCodecError(string message)
		: base(message)
	{
	}
}
