using BaseUtils;
using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

namespace GameKit.Base;

public class PingPongRequest : BaseRequest
{
	public PingPongRequest()
		: base(RequestType.PingPong)
	{
	}

	public override void Validate(SmartFox sfs)
	{
	}

	public override void Execute(SmartFox sfs)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutLong("clientTime", RealTimer.elapsedMilliseconds);
		sfso.PutSFSObject(LoginRequest.KEY_PARAMS, sFSObject);
	}
}
