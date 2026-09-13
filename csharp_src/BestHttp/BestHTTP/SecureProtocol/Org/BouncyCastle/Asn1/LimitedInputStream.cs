using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;

internal abstract class LimitedInputStream : BaseInputStream
{
	protected readonly Stream _in;

	private int _limit;

	internal LimitedInputStream(Stream inStream, int limit)
	{
		_in = inStream;
		_limit = limit;
	}

	internal virtual int GetRemaining()
	{
		return _limit;
	}

	protected virtual void SetParentEofDetect(bool on)
	{
		if (_in is IndefiniteLengthInputStream)
		{
			((IndefiniteLengthInputStream)_in).SetEofOn00(on);
		}
	}
}
