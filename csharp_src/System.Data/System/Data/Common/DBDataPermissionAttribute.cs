using System.ComponentModel;
using System.Security.Permissions;

namespace System.Data.Common;

[Serializable]
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public abstract class DBDataPermissionAttribute : CodeAccessSecurityAttribute
{
	private bool _allowBlankPassword;

	private string _connectionString;

	private string _restrictions;

	private KeyRestrictionBehavior _behavior;

	public bool AllowBlankPassword
	{
		get
		{
			return _allowBlankPassword;
		}
		set
		{
			_allowBlankPassword = value;
		}
	}

	public string ConnectionString
	{
		get
		{
			string connectionString = _connectionString;
			if (connectionString == null)
			{
				return string.Empty;
			}
			return connectionString;
		}
		set
		{
			_connectionString = value;
		}
	}

	public KeyRestrictionBehavior KeyRestrictionBehavior
	{
		get
		{
			return _behavior;
		}
		set
		{
			if ((uint)value <= 1u)
			{
				_behavior = value;
				return;
			}
			throw ADP.InvalidKeyRestrictionBehavior(value);
		}
	}

	public string KeyRestrictions
	{
		get
		{
			string restrictions = _restrictions;
			if (restrictions == null)
			{
				return ADP.StrEmpty;
			}
			return restrictions;
		}
		set
		{
			_restrictions = value;
		}
	}

	protected DBDataPermissionAttribute(SecurityAction action)
		: base(action)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeConnectionString()
	{
		return _connectionString != null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeKeyRestrictions()
	{
		return _restrictions != null;
	}
}
