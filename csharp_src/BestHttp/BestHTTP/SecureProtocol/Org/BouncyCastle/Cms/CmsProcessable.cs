using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;

public interface CmsProcessable
{
	void Write(Stream outStream);

	[Obsolete]
	object GetContent();
}
