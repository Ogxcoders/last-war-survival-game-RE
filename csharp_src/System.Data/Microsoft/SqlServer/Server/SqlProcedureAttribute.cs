using System;

namespace Microsoft.SqlServer.Server;

[Serializable]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class SqlProcedureAttribute : Attribute
{
	private string m_fName;

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

	public SqlProcedureAttribute()
	{
		m_fName = null;
	}
}
