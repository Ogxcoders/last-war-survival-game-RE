public class AmericaSubLoadingComponent : BaseSubLoadingComponent
{
	public override void SetVersionText()
	{
		string text = $"{GameEntry.Sdk.Version}[{GameEntry.Sdk.VersionCode}]";
		string resVersion = GameEntry.Resource.GetResVersion();
		versionText.text = GameEntry.Localization.GetString("100050") + " " + text + "   " + GameEntry.Localization.GetString("100051") + " " + resVersion;
	}
}
