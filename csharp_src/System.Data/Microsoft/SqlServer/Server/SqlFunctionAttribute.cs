using System;

namespace Microsoft.SqlServer.Server;

[Serializable]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class SqlFunctionAttribute : Attribute
{
	private bool m_fDeterministic;

	private DataAccessKind m_eDataAccess;

	private SystemDataAccessKind m_eSystemDataAccess;

	private bool m_fPrecise;

	private string m_fName;

	private string m_fTableDefinition;

	private string m_FillRowMethodName;

	public bool IsDeterministic
	{
		get
		{
			return m_fDeterministic;
		}
		set
		{
			m_fDeterministic = value;
		}
	}

	public DataAccessKind DataAccess
	{
		get
		{
			return m_eDataAccess;
		}
		set
		{
			m_eDataAccess = value;
		}
	}

	public SystemDataAccessKind SystemDataAccess
	{
		get
		{
			return m_eSystemDataAccess;
		}
		set
		{
			m_eSystemDataAccess = value;
		}
	}

	public bool IsPrecise
	{
		get
		{
			return m_fPrecise;
		}
		set
		{
			m_fPrecise = value;
		}
	}

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

	public string TableDefinition
	{
		get
		{
			return m_fTableDefinition;
		}
		set
		{
			m_fTableDefinition = value;
		}
	}

	public string FillRowMethodName
	{
		get
		{
			return m_FillRowMethodName;
		}
		set
		{
			m_FillRowMethodName = value;
		}
	}

	public SqlFunctionAttribute()
	{
		m_fDeterministic = false;
		m_eDataAccess = DataAccessKind.None;
		m_eSystemDataAccess = SystemDataAccessKind.None;
		m_fPrecise = false;
		m_fName = null;
		m_fTableDefinition = null;
		m_FillRowMethodName = null;
	}
}
