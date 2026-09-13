using System.Data.Common;

namespace System.Data.Sql;

public sealed class SqlNotificationRequest
{
	private string _userData;

	private string _options;

	private int _timeout;

	public string Options
	{
		get
		{
			return _options;
		}
		set
		{
			if (value != null && 65535 < value.Length)
			{
				throw ADP.ArgumentOutOfRange(string.Empty, "Options");
			}
			_options = value;
		}
	}

	public int Timeout
	{
		get
		{
			return _timeout;
		}
		set
		{
			if (0 > value)
			{
				throw ADP.ArgumentOutOfRange(string.Empty, "Timeout");
			}
			_timeout = value;
		}
	}

	public string UserData
	{
		get
		{
			return _userData;
		}
		set
		{
			if (value != null && 65535 < value.Length)
			{
				throw ADP.ArgumentOutOfRange(string.Empty, "UserData");
			}
			_userData = value;
		}
	}

	public SqlNotificationRequest()
		: this(null, null, 0)
	{
	}

	public SqlNotificationRequest(string userData, string options, int timeout)
	{
		UserData = userData;
		Timeout = timeout;
		Options = options;
	}
}
