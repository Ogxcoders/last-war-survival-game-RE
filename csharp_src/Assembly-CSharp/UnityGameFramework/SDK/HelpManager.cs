using SFSLitJson;

namespace UnityGameFramework.SDK;

public class HelpManager
{
	private static HelpManager _instance;

	public static HelpManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new HelpManager();
			}
			return _instance;
		}
	}

	public static void Purge()
	{
		_instance = null;
	}

	public void init()
	{
	}

	public void setUserData()
	{
	}

	public void showHelpShiftFAQ()
	{
	}

	public void showHelpShiftFAQ(string itemId)
	{
	}

	public void showSingleFAQ(string itemId)
	{
	}

	public void showQACommunity()
	{
	}

	public void showBlog()
	{
	}

	public void showTranslateView()
	{
	}

	public void goToHelpShift()
	{
	}

	public void showFAQ(string publishId)
	{
	}

	public void onShowGuest(string tagKey)
	{
	}

	public void showConversation(string tag)
	{
	}

	private JsonData GetTags(string tag)
	{
		return new JsonData();
	}

	private JsonData GetMetaData(string tag)
	{
		return new JsonData();
	}

	private JsonData GetCustomData(string tag)
	{
		return new JsonData();
	}
}
