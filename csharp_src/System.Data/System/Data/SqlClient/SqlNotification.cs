namespace System.Data.SqlClient;

internal class SqlNotification : MarshalByRefObject
{
	private readonly SqlNotificationInfo _info;

	private readonly SqlNotificationSource _source;

	private readonly SqlNotificationType _type;

	private readonly string _key;

	internal SqlNotificationInfo Info => _info;

	internal string Key => _key;

	internal SqlNotificationSource Source => _source;

	internal SqlNotificationType Type => _type;

	internal SqlNotification(SqlNotificationInfo info, SqlNotificationSource source, SqlNotificationType type, string key)
	{
		_info = info;
		_source = source;
		_type = type;
		_key = key;
	}
}
