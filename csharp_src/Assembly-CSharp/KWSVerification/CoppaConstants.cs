namespace KWSVerification;

public static class CoppaConstants
{
	public const string KWS_CLIENT_ID = "2Y2Q2OGE3OWIzMWViMT";

	public const string AIR_KEY_PREFIX = "lwDid_";

	public const string COUNTRY_US = "US";

	public const int CHILD_AGE_THRESHOLD = 12;

	public const int MIN_AGE = 3;

	public const int MAX_AGE = 61;

	public const int DEFAULT_AGE = 3;

	public const int GRACE_PERIOD_DAYS = 90;

	public const int NEW_USER_DELAY_DAYS = 0;

	public const string PARENT_CONFIRMED_YES = "1";

	public const string PARENT_CONFIRMED_NO = "-1";

	public const string PARENT_CONFIRMED_PENDING = "0";

	public const string EMAIL_REGEX_PATTERN = "^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$";

	public const int EMAIL_RESEND_COOLDOWN_SECONDS = 180;

	public const int EMAIL_SUCCESS_DISPLAY_SECONDS = 4;

	public const int VERIFICATION_MAX_ATTEMPTS = 600;
}
