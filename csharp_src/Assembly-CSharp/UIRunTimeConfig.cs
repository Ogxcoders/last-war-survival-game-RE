using GameFramework.Localization;

public static class UIRunTimeConfig
{
	private static bool _isArabic;

	private static bool _isInit;

	public static bool IsArabic
	{
		get
		{
			if (!_isInit && GameEntry.Setting != null)
			{
				return GameEntry.Setting.UserLanguage == Language.Arabic;
			}
			return _isArabic;
		}
		set
		{
			_isArabic = value;
			_isInit = true;
		}
	}
}
