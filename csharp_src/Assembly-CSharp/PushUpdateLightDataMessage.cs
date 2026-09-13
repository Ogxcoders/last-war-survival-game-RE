using Sfs2X.Entities.Data;

public class PushUpdateLightDataMessage : BaseMessage
{
	private static PushUpdateLightDataMessage _instance;

	public static PushUpdateLightDataMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushUpdateLightDataMessage>());

	public override string GetMsgId()
	{
		return "push.update.light.data";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (SceneManager.World != null && SceneManager.IsInWorld())
		{
			SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
			if (curSkinMeta != null && curSkinMeta.IsDarknessMode())
			{
				SceneManager.World.HandleUpdateLightData(message);
			}
		}
	}

	protected override bool showErrorCode()
	{
		return true;
	}
}
