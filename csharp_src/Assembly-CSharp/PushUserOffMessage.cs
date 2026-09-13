using Sfs2X.Entities.Data;
using XLua;

[Hotfix(HotfixFlag.Stateless)]
public class PushUserOffMessage : BaseMessage
{
	private static PushUserOffMessage _instance;

	public static PushUserOffMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushUserOffMessage>());

	public override string GetMsgId()
	{
		return "push.user.off";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		GameEntry.GlobalData.pushOffWithQuitGame = true;
		string key = "E100083";
		if (message.ContainsKey("errorCode"))
		{
			key = message.GetUtfString("errorCode");
		}
		GameEntry.Event.Fire(EventId.PushUserOff);
		UIUtils.ShowMessages(GameEntry.Localization.GetString(key), 1, "110006", "110106", null, delegate
		{
			ApplicationLaunch.Instance.Quit();
		});
	}
}
