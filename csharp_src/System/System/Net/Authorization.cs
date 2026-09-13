namespace System.Net;

public class Authorization
{
	private string m_Message;

	private bool m_Complete;

	private string[] m_ProtectionRealm;

	private string m_ConnectionGroupId;

	private bool m_MutualAuth;

	internal string ModuleAuthenticationType;

	public string Message => m_Message;

	public string ConnectionGroupId => m_ConnectionGroupId;

	public bool Complete => m_Complete;

	public string[] ProtectionRealm
	{
		get
		{
			return m_ProtectionRealm;
		}
		set
		{
			string[] protectionRealm = ValidationHelper.MakeEmptyArrayNull(value);
			m_ProtectionRealm = protectionRealm;
		}
	}

	public bool MutuallyAuthenticated
	{
		get
		{
			if (Complete)
			{
				return m_MutualAuth;
			}
			return false;
		}
		set
		{
			m_MutualAuth = value;
		}
	}

	public Authorization(string token)
	{
		m_Message = ValidationHelper.MakeStringNull(token);
		m_Complete = true;
	}

	public Authorization(string token, bool finished)
	{
		m_Message = ValidationHelper.MakeStringNull(token);
		m_Complete = finished;
	}

	public Authorization(string token, bool finished, string connectionGroupId)
		: this(token, finished, connectionGroupId, mutualAuth: false)
	{
	}

	internal Authorization(string token, bool finished, string connectionGroupId, bool mutualAuth)
	{
		m_Message = ValidationHelper.MakeStringNull(token);
		m_ConnectionGroupId = ValidationHelper.MakeStringNull(connectionGroupId);
		m_Complete = finished;
		m_MutualAuth = mutualAuth;
	}

	internal void SetComplete(bool complete)
	{
		m_Complete = complete;
	}
}
