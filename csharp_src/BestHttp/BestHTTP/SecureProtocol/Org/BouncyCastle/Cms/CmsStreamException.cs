using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;

[Serializable]
public class CmsStreamException : IOException
{
	public CmsStreamException()
	{
	}

	public CmsStreamException(string name)
		: base(name)
	{
	}

	public CmsStreamException(string name, Exception e)
		: base(name, e)
	{
	}
}
