using Internal.Cryptography;

namespace System.Security.Cryptography;

public sealed class Oid
{
	private string _value;

	private string _friendlyName;

	private OidGroup _group;

	public string Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
		}
	}

	public string FriendlyName
	{
		get
		{
			if (_friendlyName == null && _value != null)
			{
				_friendlyName = OidLookup.ToFriendlyName(_value, _group, fallBackToAllGroups: true);
			}
			return _friendlyName;
		}
		set
		{
			_friendlyName = value;
			if (_friendlyName != null)
			{
				string text = OidLookup.ToOid(_friendlyName, _group, fallBackToAllGroups: true);
				if (text != null)
				{
					_value = text;
				}
			}
		}
	}

	public Oid()
	{
	}

	public Oid(string oid)
	{
		string text = OidLookup.ToOid(oid, OidGroup.All, fallBackToAllGroups: false);
		if (text == null)
		{
			text = oid;
		}
		Value = text;
		_group = OidGroup.All;
	}

	public Oid(string value, string friendlyName)
	{
		_value = value;
		_friendlyName = friendlyName;
	}

	public Oid(Oid oid)
	{
		if (oid == null)
		{
			throw new ArgumentNullException("oid");
		}
		_value = oid._value;
		_friendlyName = oid._friendlyName;
		_group = oid._group;
	}

	public static Oid FromFriendlyName(string friendlyName, OidGroup group)
	{
		if (friendlyName == null)
		{
			throw new ArgumentNullException("friendlyName");
		}
		return new Oid(OidLookup.ToOid(friendlyName, group, fallBackToAllGroups: false) ?? throw new CryptographicException("No OID value matches this name."), friendlyName, group);
	}

	public static Oid FromOidValue(string oidValue, OidGroup group)
	{
		if (oidValue == null)
		{
			throw new ArgumentNullException("oidValue");
		}
		string text = OidLookup.ToFriendlyName(oidValue, group, fallBackToAllGroups: false);
		if (text == null)
		{
			throw new CryptographicException("The OID value is invalid.");
		}
		return new Oid(oidValue, text, group);
	}

	private Oid(string value, string friendlyName, OidGroup group)
	{
		_value = value;
		_friendlyName = friendlyName;
		_group = group;
	}
}
