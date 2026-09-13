using Sfs2X.Entities.Data;

public class InitErrorMessage : BaseMessage
{
	private static InitErrorMessage _instance;

	public static InitErrorMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<InitErrorMessage>());

	public override string GetMsgId()
	{
		return "init.error";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		ApplicationLaunch.Instance.Loading.OnInitError();
	}
}
