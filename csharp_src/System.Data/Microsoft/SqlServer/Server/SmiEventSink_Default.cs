using System.Data.SqlClient;

namespace Microsoft.SqlServer.Server;

internal class SmiEventSink_Default : SmiEventSink
{
	private SqlErrorCollection _errors;

	private SqlErrorCollection _warnings;

	internal bool HasMessages
	{
		get
		{
			if (_errors == null)
			{
				return _warnings != null;
			}
			return true;
		}
	}

	internal virtual string ServerVersion => null;

	protected virtual void DispatchMessages()
	{
		SqlException ex = ProcessMessages(ignoreWarnings: true);
		if (ex != null)
		{
			throw ex;
		}
	}

	protected SqlException ProcessMessages(bool ignoreWarnings)
	{
		SqlException result = null;
		SqlErrorCollection sqlErrorCollection = null;
		if (_errors != null)
		{
			if (_warnings != null)
			{
				foreach (SqlError warning in _warnings)
				{
					_errors.Add(warning);
				}
			}
			sqlErrorCollection = _errors;
			_errors = null;
			_warnings = null;
		}
		else
		{
			if (!ignoreWarnings)
			{
				sqlErrorCollection = _warnings;
			}
			_warnings = null;
		}
		if (sqlErrorCollection != null)
		{
			result = SqlException.CreateException(sqlErrorCollection, ServerVersion);
		}
		return result;
	}

	internal void ProcessMessagesAndThrow()
	{
		if (HasMessages)
		{
			DispatchMessages();
		}
	}

	internal SmiEventSink_Default()
	{
	}
}
