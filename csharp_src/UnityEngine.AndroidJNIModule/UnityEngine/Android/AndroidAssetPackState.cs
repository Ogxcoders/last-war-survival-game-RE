namespace UnityEngine.Android;

public class AndroidAssetPackState
{
	public string name { get; }

	public AndroidAssetPackStatus status { get; }

	public AndroidAssetPackError error { get; }

	internal AndroidAssetPackState(string name, AndroidAssetPackStatus status, AndroidAssetPackError error)
	{
		this.name = name;
		this.status = status;
		this.error = error;
	}
}
