namespace AppsFlyerSDK;

public interface IAppsFlyerUserInvite
{
	void onInviteLinkGenerated(string link);

	void onInviteLinkGeneratedFailure(string error);

	void onOpenStoreLinkGenerated(string link);
}
