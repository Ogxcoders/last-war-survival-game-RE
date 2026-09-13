using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace System.Data.Common;

[Serializable]
internal sealed class DBConnectionString
{
	private static class KEY
	{
		internal const string Password = "password";

		internal const string PersistSecurityInfo = "persist security info";

		internal const string Pwd = "pwd";
	}

	private readonly string _encryptedUsersConnectionString;

	private readonly Dictionary<string, string> _parsetable;

	private readonly NameValuePair _keychain;

	private readonly bool _hasPassword;

	private readonly string[] _restrictionValues;

	private readonly string _restrictions;

	private readonly KeyRestrictionBehavior _behavior;

	private readonly string _encryptedActualConnectionString;

	internal KeyRestrictionBehavior Behavior => _behavior;

	internal string ConnectionString => _encryptedUsersConnectionString;

	internal bool IsEmpty => _keychain == null;

	internal NameValuePair KeyChain => _keychain;

	internal string Restrictions
	{
		get
		{
			string text = _restrictions;
			if (text == null)
			{
				string[] restrictionValues = _restrictionValues;
				if (restrictionValues != null && restrictionValues.Length != 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < restrictionValues.Length; i++)
					{
						if (!string.IsNullOrEmpty(restrictionValues[i]))
						{
							stringBuilder.Append(restrictionValues[i]);
							stringBuilder.Append("=;");
						}
					}
					text = stringBuilder.ToString();
				}
			}
			if (text == null)
			{
				return "";
			}
			return text;
		}
	}

	internal string this[string keyword] => _parsetable[keyword];

	internal DBConnectionString(string value, string restrictions, KeyRestrictionBehavior behavior, Dictionary<string, string> synonyms, bool useOdbcRules)
		: this(new DbConnectionOptions(value, synonyms, useOdbcRules), restrictions, behavior, synonyms, mustCloneDictionary: false)
	{
	}

	internal DBConnectionString(DbConnectionOptions connectionOptions)
		: this(connectionOptions, null, KeyRestrictionBehavior.AllowOnly, null, mustCloneDictionary: true)
	{
	}

	private DBConnectionString(DbConnectionOptions connectionOptions, string restrictions, KeyRestrictionBehavior behavior, Dictionary<string, string> synonyms, bool mustCloneDictionary)
	{
		if ((uint)behavior <= 1u)
		{
			_behavior = behavior;
			_encryptedUsersConnectionString = connectionOptions.UsersConnectionString(hidePassword: false);
			_hasPassword = connectionOptions._hasPasswordKeyword;
			_parsetable = connectionOptions.Parsetable;
			_keychain = connectionOptions._keyChain;
			if (_hasPassword && !connectionOptions.HasPersistablePassword)
			{
				if (mustCloneDictionary)
				{
					_parsetable = new Dictionary<string, string>(_parsetable);
				}
				if (_parsetable.ContainsKey("password"))
				{
					_parsetable["password"] = "*";
				}
				if (_parsetable.ContainsKey("pwd"))
				{
					_parsetable["pwd"] = "*";
				}
				_keychain = connectionOptions.ReplacePasswordPwd(out _encryptedUsersConnectionString, fakePassword: true);
			}
			if (!string.IsNullOrEmpty(restrictions))
			{
				_restrictionValues = ParseRestrictions(restrictions, synonyms);
				_restrictions = restrictions;
			}
			return;
		}
		throw ADP.InvalidKeyRestrictionBehavior(behavior);
	}

	private DBConnectionString(DBConnectionString connectionString, string[] restrictionValues, KeyRestrictionBehavior behavior)
	{
		_encryptedUsersConnectionString = connectionString._encryptedUsersConnectionString;
		_parsetable = connectionString._parsetable;
		_keychain = connectionString._keychain;
		_hasPassword = connectionString._hasPassword;
		_restrictionValues = restrictionValues;
		_restrictions = null;
		_behavior = behavior;
	}

	internal bool ContainsKey(string keyword)
	{
		return _parsetable.ContainsKey(keyword);
	}

	internal DBConnectionString Intersect(DBConnectionString entry)
	{
		KeyRestrictionBehavior behavior = _behavior;
		string[] restrictionValues = null;
		if (entry == null)
		{
			behavior = KeyRestrictionBehavior.AllowOnly;
		}
		else if (_behavior != entry._behavior)
		{
			behavior = KeyRestrictionBehavior.AllowOnly;
			if (entry._behavior == KeyRestrictionBehavior.AllowOnly)
			{
				if (!ADP.IsEmptyArray(_restrictionValues))
				{
					if (!ADP.IsEmptyArray(entry._restrictionValues))
					{
						restrictionValues = NewRestrictionAllowOnly(entry._restrictionValues, _restrictionValues);
					}
				}
				else
				{
					restrictionValues = entry._restrictionValues;
				}
			}
			else if (!ADP.IsEmptyArray(_restrictionValues))
			{
				restrictionValues = (ADP.IsEmptyArray(entry._restrictionValues) ? _restrictionValues : NewRestrictionAllowOnly(_restrictionValues, entry._restrictionValues));
			}
		}
		else if (KeyRestrictionBehavior.PreventUsage == _behavior)
		{
			restrictionValues = (ADP.IsEmptyArray(_restrictionValues) ? entry._restrictionValues : ((!ADP.IsEmptyArray(entry._restrictionValues)) ? NoDuplicateUnion(_restrictionValues, entry._restrictionValues) : _restrictionValues));
		}
		else if (!ADP.IsEmptyArray(_restrictionValues) && !ADP.IsEmptyArray(entry._restrictionValues))
		{
			restrictionValues = ((_restrictionValues.Length > entry._restrictionValues.Length) ? NewRestrictionIntersect(entry._restrictionValues, _restrictionValues) : NewRestrictionIntersect(_restrictionValues, entry._restrictionValues));
		}
		return new DBConnectionString(this, restrictionValues, behavior);
	}

	[Conditional("DEBUG")]
	private void ValidateCombinedSet(DBConnectionString componentSet, DBConnectionString combinedSet)
	{
		if (componentSet == null || combinedSet._restrictionValues == null || componentSet._restrictionValues == null)
		{
			return;
		}
		if (componentSet._behavior == KeyRestrictionBehavior.AllowOnly)
		{
			if (combinedSet._behavior != KeyRestrictionBehavior.AllowOnly)
			{
				_ = combinedSet._behavior;
				_ = 1;
			}
		}
		else if (componentSet._behavior == KeyRestrictionBehavior.PreventUsage && combinedSet._behavior != KeyRestrictionBehavior.AllowOnly)
		{
			_ = combinedSet._behavior;
			_ = 1;
		}
	}

	private bool IsRestrictedKeyword(string key)
	{
		if (_restrictionValues != null)
		{
			return 0 > Array.BinarySearch(_restrictionValues, key, StringComparer.Ordinal);
		}
		return true;
	}

	internal bool IsSupersetOf(DBConnectionString entry)
	{
		switch (_behavior)
		{
		case KeyRestrictionBehavior.AllowOnly:
		{
			for (NameValuePair nameValuePair = entry.KeyChain; nameValuePair != null; nameValuePair = nameValuePair.Next)
			{
				if (!ContainsKey(nameValuePair.Name) && IsRestrictedKeyword(nameValuePair.Name))
				{
					return false;
				}
			}
			break;
		}
		case KeyRestrictionBehavior.PreventUsage:
		{
			if (_restrictionValues == null)
			{
				break;
			}
			string[] restrictionValues = _restrictionValues;
			foreach (string keyword in restrictionValues)
			{
				if (entry.ContainsKey(keyword))
				{
					return false;
				}
			}
			break;
		}
		default:
			throw ADP.InvalidKeyRestrictionBehavior(_behavior);
		}
		return true;
	}

	private static string[] NewRestrictionAllowOnly(string[] allowonly, string[] preventusage)
	{
		List<string> list = null;
		for (int i = 0; i < allowonly.Length; i++)
		{
			if (0 > Array.BinarySearch(preventusage, allowonly[i], StringComparer.Ordinal))
			{
				if (list == null)
				{
					list = new List<string>();
				}
				list.Add(allowonly[i]);
			}
		}
		string[] result = null;
		if (list != null)
		{
			result = list.ToArray();
		}
		return result;
	}

	private static string[] NewRestrictionIntersect(string[] a, string[] b)
	{
		List<string> list = null;
		for (int i = 0; i < a.Length; i++)
		{
			if (0 <= Array.BinarySearch(b, a[i], StringComparer.Ordinal))
			{
				if (list == null)
				{
					list = new List<string>();
				}
				list.Add(a[i]);
			}
		}
		string[] result = null;
		if (list != null)
		{
			result = list.ToArray();
		}
		return result;
	}

	private static string[] NoDuplicateUnion(string[] a, string[] b)
	{
		List<string> list = new List<string>(a.Length + b.Length);
		for (int i = 0; i < a.Length; i++)
		{
			list.Add(a[i]);
		}
		for (int j = 0; j < b.Length; j++)
		{
			if (0 > Array.BinarySearch(a, b[j], StringComparer.Ordinal))
			{
				list.Add(b[j]);
			}
		}
		string[] array = list.ToArray();
		Array.Sort(array, StringComparer.Ordinal);
		return array;
	}

	private static string[] ParseRestrictions(string restrictions, Dictionary<string, string> synonyms)
	{
		List<string> list = new List<string>();
		StringBuilder buffer = new StringBuilder(restrictions.Length);
		int num = 0;
		int length = restrictions.Length;
		while (num < length)
		{
			int currentPosition = num;
			num = DbConnectionOptions.GetKeyValuePair(restrictions, currentPosition, buffer, useOdbcRules: false, out var keyname, out var _);
			if (!string.IsNullOrEmpty(keyname))
			{
				string text = ((synonyms != null) ? synonyms[keyname] : keyname);
				if (string.IsNullOrEmpty(text))
				{
					throw ADP.KeywordNotSupported(keyname);
				}
				list.Add(text);
			}
		}
		return RemoveDuplicates(list.ToArray());
	}

	internal static string[] RemoveDuplicates(string[] restrictions)
	{
		int num = restrictions.Length;
		if (0 < num)
		{
			Array.Sort(restrictions, StringComparer.Ordinal);
			for (int i = 1; i < restrictions.Length; i++)
			{
				string text = restrictions[i - 1];
				if (text.Length == 0 || text == restrictions[i])
				{
					restrictions[i - 1] = null;
					num--;
				}
			}
			if (restrictions[^1].Length == 0)
			{
				restrictions[^1] = null;
				num--;
			}
			if (num != restrictions.Length)
			{
				string[] array = new string[num];
				num = 0;
				for (int j = 0; j < restrictions.Length; j++)
				{
					if (restrictions[j] != null)
					{
						array[num++] = restrictions[j];
					}
				}
				restrictions = array;
			}
		}
		return restrictions;
	}

	[Conditional("DEBUG")]
	private static void Verify(string[] restrictionValues)
	{
		if (restrictionValues != null)
		{
			for (int i = 1; i < restrictionValues.Length; i++)
			{
			}
		}
	}
}
