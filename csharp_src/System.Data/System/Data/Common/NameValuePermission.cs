using System.Collections;

namespace System.Data.Common;

[Serializable]
internal sealed class NameValuePermission : IComparable
{
	private string _value;

	private DBConnectionString _entry;

	private NameValuePermission[] _tree;

	internal static readonly NameValuePermission Default;

	internal NameValuePermission()
	{
	}

	private NameValuePermission(string keyword)
	{
		_value = keyword;
	}

	private NameValuePermission(string value, DBConnectionString entry)
	{
		_value = value;
		_entry = entry;
	}

	private NameValuePermission(NameValuePermission permit)
	{
		_value = permit._value;
		_entry = permit._entry;
		_tree = permit._tree;
		if (_tree == null)
		{
			return;
		}
		NameValuePermission[] array = _tree.Clone() as NameValuePermission[];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				array[i] = array[i].CopyNameValue();
			}
		}
		_tree = array;
	}

	int IComparable.CompareTo(object a)
	{
		return StringComparer.Ordinal.Compare(_value, ((NameValuePermission)a)._value);
	}

	internal static void AddEntry(NameValuePermission kvtree, ArrayList entries, DBConnectionString entry)
	{
		if (entry.KeyChain != null)
		{
			for (NameValuePair nameValuePair = entry.KeyChain; nameValuePair != null; nameValuePair = nameValuePair.Next)
			{
				NameValuePermission nameValuePermission = kvtree.CheckKeyForValue(nameValuePair.Name);
				if (nameValuePermission == null)
				{
					nameValuePermission = new NameValuePermission(nameValuePair.Name);
					kvtree.Add(nameValuePermission);
				}
				kvtree = nameValuePermission;
				nameValuePermission = kvtree.CheckKeyForValue(nameValuePair.Value);
				if (nameValuePermission == null)
				{
					DBConnectionString dBConnectionString = ((nameValuePair.Next != null) ? null : entry);
					nameValuePermission = new NameValuePermission(nameValuePair.Value, dBConnectionString);
					kvtree.Add(nameValuePermission);
					if (dBConnectionString != null)
					{
						entries.Add(dBConnectionString);
					}
				}
				else if (nameValuePair.Next == null)
				{
					if (nameValuePermission._entry != null)
					{
						entries.Remove(nameValuePermission._entry);
						nameValuePermission._entry = nameValuePermission._entry.Intersect(entry);
					}
					else
					{
						nameValuePermission._entry = entry;
					}
					entries.Add(nameValuePermission._entry);
				}
				kvtree = nameValuePermission;
			}
		}
		else
		{
			DBConnectionString entry2 = kvtree._entry;
			if (entry2 != null)
			{
				entries.Remove(entry2);
				kvtree._entry = entry2.Intersect(entry);
			}
			else
			{
				kvtree._entry = entry;
			}
			entries.Add(kvtree._entry);
		}
	}

	internal void Intersect(ArrayList entries, NameValuePermission target)
	{
		if (target == null)
		{
			_tree = null;
			_entry = null;
			return;
		}
		if (_entry != null)
		{
			entries.Remove(_entry);
			_entry = _entry.Intersect(target._entry);
			entries.Add(_entry);
		}
		else if (target._entry != null)
		{
			_entry = target._entry.Intersect(null);
			entries.Add(_entry);
		}
		if (_tree == null)
		{
			return;
		}
		int num = _tree.Length;
		for (int i = 0; i < _tree.Length; i++)
		{
			NameValuePermission nameValuePermission = target.CheckKeyForValue(_tree[i]._value);
			if (nameValuePermission != null)
			{
				_tree[i].Intersect(entries, nameValuePermission);
				continue;
			}
			_tree[i] = null;
			num--;
		}
		if (num == 0)
		{
			_tree = null;
		}
		else
		{
			if (num >= _tree.Length)
			{
				return;
			}
			NameValuePermission[] array = new NameValuePermission[num];
			int j = 0;
			int num2 = 0;
			for (; j < _tree.Length; j++)
			{
				if (_tree[j] != null)
				{
					array[num2++] = _tree[j];
				}
			}
			_tree = array;
		}
	}

	private void Add(NameValuePermission permit)
	{
		NameValuePermission[] tree = _tree;
		int num = ((tree != null) ? tree.Length : 0);
		NameValuePermission[] array = new NameValuePermission[1 + num];
		for (int i = 0; i < array.Length - 1; i++)
		{
			array[i] = tree[i];
		}
		array[num] = permit;
		Array.Sort(array);
		_tree = array;
	}

	internal bool CheckValueForKeyPermit(DBConnectionString parsetable)
	{
		if (parsetable == null)
		{
			return false;
		}
		bool flag = false;
		NameValuePermission[] tree = _tree;
		if (tree != null)
		{
			flag = parsetable.IsEmpty;
			if (!flag)
			{
				foreach (NameValuePermission nameValuePermission in tree)
				{
					if (nameValuePermission == null)
					{
						continue;
					}
					string value = nameValuePermission._value;
					if (parsetable.ContainsKey(value))
					{
						string keyInQuestion = parsetable[value];
						NameValuePermission nameValuePermission2 = nameValuePermission.CheckKeyForValue(keyInQuestion);
						if (nameValuePermission2 == null)
						{
							return false;
						}
						if (!nameValuePermission2.CheckValueForKeyPermit(parsetable))
						{
							return false;
						}
						flag = true;
					}
				}
			}
		}
		DBConnectionString entry = _entry;
		if (entry != null)
		{
			flag = entry.IsSupersetOf(parsetable);
		}
		return flag;
	}

	private NameValuePermission CheckKeyForValue(string keyInQuestion)
	{
		NameValuePermission[] tree = _tree;
		if (tree != null)
		{
			foreach (NameValuePermission nameValuePermission in tree)
			{
				if (string.Equals(keyInQuestion, nameValuePermission._value, StringComparison.OrdinalIgnoreCase))
				{
					return nameValuePermission;
				}
			}
		}
		return null;
	}

	internal NameValuePermission CopyNameValue()
	{
		return new NameValuePermission(this);
	}
}
