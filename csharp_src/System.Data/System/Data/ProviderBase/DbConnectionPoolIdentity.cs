namespace System.Data.ProviderBase;

[Serializable]
internal sealed class DbConnectionPoolIdentity
{
	public static readonly DbConnectionPoolIdentity NoIdentity = new DbConnectionPoolIdentity(string.Empty, isRestricted: false, isNetwork: true);

	private readonly string _sidString;

	private readonly bool _isRestricted;

	private readonly bool _isNetwork;

	private readonly int _hashCode;

	internal bool IsRestricted => _isRestricted;

	internal static DbConnectionPoolIdentity GetCurrent()
	{
		return GetCurrentManaged();
	}

	private DbConnectionPoolIdentity(string sidString, bool isRestricted, bool isNetwork)
	{
		_sidString = sidString;
		_isRestricted = isRestricted;
		_isNetwork = isNetwork;
		_hashCode = sidString?.GetHashCode() ?? 0;
	}

	public override bool Equals(object value)
	{
		bool flag = this == NoIdentity || this == value;
		if (!flag && value != null)
		{
			DbConnectionPoolIdentity dbConnectionPoolIdentity = (DbConnectionPoolIdentity)value;
			flag = _sidString == dbConnectionPoolIdentity._sidString && _isRestricted == dbConnectionPoolIdentity._isRestricted && _isNetwork == dbConnectionPoolIdentity._isNetwork;
		}
		return flag;
	}

	public override int GetHashCode()
	{
		return _hashCode;
	}

	internal static DbConnectionPoolIdentity GetCurrentManaged()
	{
		string sidString = ((!string.IsNullOrWhiteSpace(Environment.UserDomainName)) ? (Environment.UserDomainName + "\\") : "") + Environment.UserName;
		bool isNetwork = false;
		bool isRestricted = false;
		return new DbConnectionPoolIdentity(sidString, isRestricted, isNetwork);
	}
}
