using GameKit.Base;
using Sfs2X.Core;
using Sfs2X.Entities.Data;

public interface INetManager
{
	bool OnConnection(INetProxy proxy, BaseEvent e);

	void OnConnectionLost(string reason, INetProxy proxy);

	void OnLogout(BaseEvent e);

	void OnLogin(BaseEvent e);

	void OnLoginError(BaseEvent e);

	void OnExtensionResponse(string cmd, SFSObject so);

	bool IsMainLine();
}
