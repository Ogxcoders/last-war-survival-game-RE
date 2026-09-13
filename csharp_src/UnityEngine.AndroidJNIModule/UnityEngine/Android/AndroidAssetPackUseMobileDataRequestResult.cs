namespace UnityEngine.Android;

public class AndroidAssetPackUseMobileDataRequestResult
{
	public bool allowed { get; }

	internal AndroidAssetPackUseMobileDataRequestResult(bool allowed)
	{
		this.allowed = allowed;
	}
}
