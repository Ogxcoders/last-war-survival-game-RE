using System.Collections.Generic;
using Sfs2XLw.Bitswarm;
using Sfs2XLw.Requests;

namespace Sfs2XLw.Controllers;

public class SystemController : BaseController
{
	private Dictionary<int, RequestDelegate> requestHandlers;

	public SystemController(ISocketClient socketClient)
		: base(socketClient)
	{
		requestHandlers = new Dictionary<int, RequestDelegate>();
	}

	public override void HandleMessage(IMessage message)
	{
		if (sfs.Debug)
		{
			log.Info(string.Concat("Message: ", (RequestType)message.Id, " ", message));
		}
		if (!requestHandlers.ContainsKey(message.Id))
		{
			log.Warn("Unknown message id: " + message.Id);
		}
		else
		{
			requestHandlers[message.Id](message);
		}
	}
}
