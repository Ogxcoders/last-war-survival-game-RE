using System.Collections;

namespace System.Xml.Schema;

internal class SymbolsDictionary
{
	private int last;

	private Hashtable names;

	private Hashtable wildcards;

	private ArrayList particles;

	private object particleLast;

	private bool isUpaEnforced = true;

	public int Count => last + 1;

	public int CountOfNames => names.Count;

	public bool IsUpaEnforced
	{
		get
		{
			return isUpaEnforced;
		}
		set
		{
			isUpaEnforced = value;
		}
	}

	public int this[XmlQualifiedName name]
	{
		get
		{
			object obj = names[name];
			if (obj != null)
			{
				return (int)obj;
			}
			if (wildcards != null)
			{
				obj = wildcards[name.Namespace];
				if (obj != null)
				{
					return (int)obj;
				}
			}
			return last;
		}
	}

	public SymbolsDictionary()
	{
		names = new Hashtable();
		particles = new ArrayList();
	}

	public int AddName(XmlQualifiedName name, object particle)
	{
		object obj = names[name];
		if (obj != null)
		{
			int num = (int)obj;
			if (particles[num] != particle)
			{
				isUpaEnforced = false;
			}
			return num;
		}
		names.Add(name, last);
		particles.Add(particle);
		return last++;
	}

	public void AddNamespaceList(NamespaceList list, object particle, bool allowLocal)
	{
		switch (list.Type)
		{
		case NamespaceList.ListType.Any:
			particleLast = particle;
			break;
		case NamespaceList.ListType.Other:
			AddWildcard(list.Excluded, null);
			if (!allowLocal)
			{
				AddWildcard(string.Empty, null);
			}
			break;
		case NamespaceList.ListType.Set:
		{
			foreach (string item in list.Enumerate)
			{
				AddWildcard(item, particle);
			}
			break;
		}
		}
	}

	private void AddWildcard(string wildcard, object particle)
	{
		if (wildcards == null)
		{
			wildcards = new Hashtable();
		}
		object obj = wildcards[wildcard];
		if (obj == null)
		{
			wildcards.Add(wildcard, last);
			particles.Add(particle);
			last++;
		}
		else if (particle != null)
		{
			particles[(int)obj] = particle;
		}
	}

	public ICollection GetNamespaceListSymbols(NamespaceList list)
	{
		ArrayList arrayList = new ArrayList();
		foreach (XmlQualifiedName key in names.Keys)
		{
			if (key != XmlQualifiedName.Empty && list.Allows(key))
			{
				arrayList.Add(names[key]);
			}
		}
		if (wildcards != null)
		{
			foreach (string key2 in wildcards.Keys)
			{
				if (list.Allows(key2))
				{
					arrayList.Add(wildcards[key2]);
				}
			}
		}
		if (list.Type == NamespaceList.ListType.Any || list.Type == NamespaceList.ListType.Other)
		{
			arrayList.Add(last);
		}
		return arrayList;
	}

	public bool Exists(XmlQualifiedName name)
	{
		if (names[name] != null)
		{
			return true;
		}
		return false;
	}

	public object GetParticle(int symbol)
	{
		if (symbol != last)
		{
			return particles[symbol];
		}
		return particleLast;
	}

	public string NameOf(int symbol)
	{
		foreach (DictionaryEntry name in names)
		{
			if ((int)name.Value == symbol)
			{
				return ((XmlQualifiedName)name.Key).ToString();
			}
		}
		if (wildcards != null)
		{
			foreach (DictionaryEntry wildcard in wildcards)
			{
				if ((int)wildcard.Value == symbol)
				{
					return (string)wildcard.Key + ":*";
				}
			}
		}
		return "##other:*";
	}
}
