using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class GetALPointsMessage : BaseMessage
{
	private static GetALPointsMessage _instance;

	public static GetALPointsMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<GetALPointsMessage>());

	public override string GetMsgId()
	{
		return "get.al.points";
	}

	protected override IRequest CSSetData(params object[] args)
	{
		if (args == null || args.Length < 1)
		{
			return null;
		}
		if (int.TryParse(args[0].ToString(), out var result))
		{
			ISFSObject iSFSObject = new SFSObject();
			iSFSObject.PutInt("serverId", result);
			return new ExtensionRequest(GetMsgId(), iSFSObject);
		}
		return null;
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		WorldScene worldScene = SceneManager.World as WorldScene;
		if (!(worldScene == null))
		{
			worldScene.OnHandleALPoints(message);
		}
	}
}
