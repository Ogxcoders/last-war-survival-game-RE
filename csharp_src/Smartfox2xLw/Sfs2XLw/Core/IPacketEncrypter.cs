using Sfs2XLw.Util;

namespace Sfs2XLw.Core;

public interface IPacketEncrypter
{
	void Encrypt(ByteArray data);

	void Decrypt(ByteArray data);
}
