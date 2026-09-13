using Sfs2XLw.Protocol;
using Sfs2XLw.Util;

namespace Sfs2XLw.Bitswarm;

public interface IoHandler
{
	IProtocolCodec Codec { get; }

	void OnDataRead(ByteArray buffer);

	void OnDataRead(string jsonData);

	void OnDataWrite(IMessage message);
}
