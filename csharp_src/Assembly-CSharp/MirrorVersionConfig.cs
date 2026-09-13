public static class MirrorVersionConfig
{
	private static bool _isAutoMirrorVersionOpen;

	public static bool IsMirrorVersionOpen => _isAutoMirrorVersionOpen;

	static MirrorVersionConfig()
	{
		RefreshOpenFlag();
	}

	public static void RefreshOpenFlag()
	{
		_isAutoMirrorVersionOpen = GameEntry.Lua?.CallWithReturn<bool>("CSharpCallLuaInterface.IsArabicAutoMirrorOpen") ?? false;
		if (_isAutoMirrorVersionOpen)
		{
			AutoReverseImageNameList.Init();
		}
	}
}
