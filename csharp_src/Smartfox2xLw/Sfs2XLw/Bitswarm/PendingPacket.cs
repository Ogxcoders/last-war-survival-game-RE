using Sfs2XLw.Core;
using Sfs2XLw.Util;

namespace Sfs2XLw.Bitswarm;

public class PendingPacket
{
	private PacketHeader header;

	private ByteArray buffer;

	public PacketHeader Header => header;

	public ByteArray Buffer
	{
		get
		{
			return buffer;
		}
		set
		{
			buffer = value;
		}
	}

	public PendingPacket(PacketHeader header)
	{
		this.header = header;
		buffer = new ByteArray();
		buffer.Compressed = header.Compressed;
	}
}
