namespace System.Data.SqlClient;

public class SqlNotificationEventArgs : EventArgs
{
	private SqlNotificationType _type;

	private SqlNotificationInfo _info;

	private SqlNotificationSource _source;

	internal static SqlNotificationEventArgs s_notifyError = new SqlNotificationEventArgs(SqlNotificationType.Subscribe, SqlNotificationInfo.Error, SqlNotificationSource.Object);

	public SqlNotificationType Type => _type;

	public SqlNotificationInfo Info => _info;

	public SqlNotificationSource Source => _source;

	public SqlNotificationEventArgs(SqlNotificationType type, SqlNotificationInfo info, SqlNotificationSource source)
	{
		_info = info;
		_source = source;
		_type = type;
	}
}
