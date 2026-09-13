namespace System.Data.SqlClient;

public enum SqlNotificationSource
{
	Data = 0,
	Timeout = 1,
	Object = 2,
	Database = 3,
	System = 4,
	Statement = 5,
	Environment = 6,
	Execution = 7,
	Owner = 8,
	Unknown = -1,
	Client = -2
}
