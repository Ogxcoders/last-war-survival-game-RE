using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

namespace GameKit.Base;

public class UserDisconnectRequest : BaseRequest
{
	public UserDisconnectRequest()
		: base(RequestType.ManualDisconnection)
	{
	}

	public override void Validate(SmartFox sfs)
	{
	}

	public override void Execute(SmartFox sfs)
	{
		SFSObject val = new SFSObject();
		sfso.PutSFSObject(LoginRequest.KEY_PARAMS, val);
	}
}
