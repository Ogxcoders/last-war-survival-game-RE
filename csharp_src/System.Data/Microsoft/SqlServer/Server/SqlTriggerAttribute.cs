using System;

namespace Microsoft.SqlServer.Server;

[Serializable]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class SqlTriggerAttribute : Attribute
{
	private string m_fName;

	private string m_fTarget;

	private string m_fEvent;

	public string Name
	{
		get
		{
			return m_fName;
		}
		set
		{
			m_fName = value;
		}
	}

	public string Target
	{
		get
		{
			return m_fTarget;
		}
		set
		{
			m_fTarget = value;
		}
	}

	public string Event
	{
		get
		{
			return m_fEvent;
		}
		set
		{
			m_fEvent = value;
		}
	}

	public SqlTriggerAttribute()
	{
		m_fName = null;
		m_fTarget = null;
		m_fEvent = null;
	}
}
