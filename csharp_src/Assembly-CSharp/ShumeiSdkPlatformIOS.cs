public class ShumeiSdkPlatformIOS : IShumeiSdkPlatform
{
	public void CallCreate()
	{
		if (GameEntry.Sdk != null)
		{
			GameEntry.Sdk.SendDataToNative("LW_ShumeiSdkCreate", string.Empty);
		}
	}

	public string GetDeviceId()
	{
		if (GameEntry.Sdk != null)
		{
			return GameEntry.Sdk.GetDataFromNative("LW_GetShumeiSdkDeviceId", "");
		}
		return string.Empty;
	}
}
