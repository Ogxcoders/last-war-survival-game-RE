using AIHelp;
using Sfs2X.Entities.Data;

public class PushAIHelpConversationStatus : BaseMessage
{
	private static PushAIHelpConversationStatus _instance;

	public static PushAIHelpConversationStatus Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushAIHelpConversationStatus>());

	public override string GetMsgId()
	{
		return "push.aiHelp.status";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
		if (message.ContainsKey("status"))
		{
			AIHelpProxy.CacheAIHelpHaveConversation(message.TryGetInt("status"));
			AIHelpProxy.ResetIsUsingZendesk(AIHelpProxy.CheckZendeskSwitch());
		}
	}
}
