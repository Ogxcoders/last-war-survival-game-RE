using Protobuf;
using Sfs2X.Entities.Data;
using Sfs2X.Util;

public class PushWorldAreaGreenUpdate : BaseMessage
{
	private static PushWorldAreaGreenUpdate _instance;

	public static PushWorldAreaGreenUpdate Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushWorldAreaGreenUpdate>());

	public override string GetMsgId()
	{
		return "push.update.green.points";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null && SceneManager.World is WorldScene worldScene)
		{
			ByteArray byteArray = message.GetByteArray("greenPoints");
			if (byteArray != null)
			{
				GreenPoints di = GreenPoints.Parser.ParseFrom(byteArray.Bytes);
				worldScene.PointManager.worldGreen.UpdateGreenArea(new WorldAreaGreenInfo(di));
			}
		}
	}
}
