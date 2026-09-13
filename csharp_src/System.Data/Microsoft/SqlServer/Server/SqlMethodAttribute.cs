using System;

namespace Microsoft.SqlServer.Server;

[Serializable]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class SqlMethodAttribute : SqlFunctionAttribute
{
	private bool m_fCallOnNullInputs;

	private bool m_fMutator;

	private bool m_fInvokeIfReceiverIsNull;

	public bool OnNullCall
	{
		get
		{
			return m_fCallOnNullInputs;
		}
		set
		{
			m_fCallOnNullInputs = value;
		}
	}

	public bool IsMutator
	{
		get
		{
			return m_fMutator;
		}
		set
		{
			m_fMutator = value;
		}
	}

	public bool InvokeIfReceiverIsNull
	{
		get
		{
			return m_fInvokeIfReceiverIsNull;
		}
		set
		{
			m_fInvokeIfReceiverIsNull = value;
		}
	}

	public SqlMethodAttribute()
	{
		m_fCallOnNullInputs = true;
		m_fMutator = false;
		m_fInvokeIfReceiverIsNull = false;
	}
}
