using GameFramework;
using Sfs2X.Entities.Data;

namespace GameKit.Base;

public class CrossNetProxy : NetRawProxy
{
	public CrossNetProxy(string name, string h, int p, int connectionType, INetManager net, ushort sid)
		: base(name, h, p, connectionType, net, sid)
	{
	}

	public override void OnExtensionResponse(string cmd, SFSObject so)
	{
		SyncPingPong();
		parent.OnExtensionResponse(cmd, so);
		if (!_isCurProxy)
		{
			Log.Info("NetRawProxy::OnExtensionResponse::" + cmd + " " + base.proxyName);
		}
	}
}
