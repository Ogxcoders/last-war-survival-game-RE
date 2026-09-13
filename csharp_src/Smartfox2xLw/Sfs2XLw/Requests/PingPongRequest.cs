namespace Sfs2XLw.Requests;

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
	}
}
