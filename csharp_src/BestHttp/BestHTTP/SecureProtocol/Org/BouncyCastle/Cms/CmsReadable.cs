using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;

public interface CmsReadable
{
	Stream GetInputStream();
}
