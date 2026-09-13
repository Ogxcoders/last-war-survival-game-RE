using Sfs2XLw.Bitswarm;
using Sfs2XLw.Entities.Data;
using Sfs2XLw.Util;

namespace Sfs2XLw.Protocol;

public interface IProtocolCodec
{
	IoHandler IOHandler { get; set; }

	void OnPacketRead(ISFSObject packet);

	void OnPacketRead(ByteArray packet);

	void OnPacketWrite(IMessage message);
}
