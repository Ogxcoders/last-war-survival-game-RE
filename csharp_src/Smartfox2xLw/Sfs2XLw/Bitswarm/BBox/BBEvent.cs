using System.Collections;
using Sfs2XLw.Core;

namespace Sfs2XLw.Bitswarm.BBox;

public class BBEvent : BaseEvent
{
	public static readonly string CONNECT = "bb-connect";

	public static readonly string DISCONNECT = "bb-disconnect";

	public static readonly string DATA = "bb-data";

	public static readonly string IO_ERROR = "bb-ioError";

	public static readonly string SECURITY_ERROR = "bb-securityError";

	public BBEvent(string type)
		: base(type, null)
	{
	}

	public BBEvent(string type, Hashtable arguments)
		: base(type, arguments)
	{
	}
}
