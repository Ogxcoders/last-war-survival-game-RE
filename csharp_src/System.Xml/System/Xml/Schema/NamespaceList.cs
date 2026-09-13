using System.Collections;
using System.Text;

namespace System.Xml.Schema;

internal class NamespaceList
{
	public enum ListType
	{
		Any,
		Other,
		Set
	}

	private ListType type;

	private Hashtable set;

	private string targetNamespace;

	public ListType Type => type;

	public string Excluded => targetNamespace;

	public ICollection Enumerate
	{
		get
		{
			ListType listType = type;
			if ((uint)listType > 1u && listType == ListType.Set)
			{
				return set.Keys;
			}
			throw new InvalidOperationException();
		}
	}

	public NamespaceList()
	{
	}

	public NamespaceList(string namespaces, string targetNamespace)
	{
		this.targetNamespace = targetNamespace;
		namespaces = namespaces.Trim();
		if (namespaces == "##any" || namespaces.Length == 0)
		{
			type = ListType.Any;
			return;
		}
		if (namespaces == "##other")
		{
			type = ListType.Other;
			return;
		}
		type = ListType.Set;
		set = new Hashtable();
		string[] array = XmlConvert.SplitString(namespaces);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == "##local")
			{
				set[string.Empty] = string.Empty;
				continue;
			}
			if (array[i] == "##targetNamespace")
			{
				set[targetNamespace] = targetNamespace;
				continue;
			}
			XmlConvert.ToUri(array[i]);
			set[array[i]] = array[i];
		}
	}

	public NamespaceList Clone()
	{
		NamespaceList namespaceList = (NamespaceList)MemberwiseClone();
		if (type == ListType.Set)
		{
			namespaceList.set = (Hashtable)set.Clone();
		}
		return namespaceList;
	}

	public virtual bool Allows(string ns)
	{
		switch (type)
		{
		case ListType.Any:
			return true;
		case ListType.Other:
			if (ns != targetNamespace)
			{
				return ns.Length != 0;
			}
			return false;
		case ListType.Set:
			return set[ns] != null;
		default:
			return false;
		}
	}

	public bool Allows(XmlQualifiedName qname)
	{
		return Allows(qname.Namespace);
	}

	public override string ToString()
	{
		switch (type)
		{
		case ListType.Any:
			return "##any";
		case ListType.Other:
			return "##other";
		case ListType.Set:
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string key in set.Keys)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(" ");
				}
				if (key == targetNamespace)
				{
					stringBuilder.Append("##targetNamespace");
				}
				else if (key.Length == 0)
				{
					stringBuilder.Append("##local");
				}
				else
				{
					stringBuilder.Append(key);
				}
			}
			return stringBuilder.ToString();
		}
		default:
			return string.Empty;
		}
	}

	public static bool IsSubset(NamespaceList sub, NamespaceList super)
	{
		if (super.type == ListType.Any)
		{
			return true;
		}
		if (sub.type == ListType.Other && super.type == ListType.Other)
		{
			return super.targetNamespace == sub.targetNamespace;
		}
		if (sub.type == ListType.Set)
		{
			if (super.type == ListType.Other)
			{
				return !sub.set.Contains(super.targetNamespace);
			}
			foreach (string key in sub.set.Keys)
			{
				if (!super.set.Contains(key))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	public static NamespaceList Union(NamespaceList o1, NamespaceList o2, bool v1Compat)
	{
		NamespaceList namespaceList = null;
		if (o1.type == ListType.Any)
		{
			namespaceList = new NamespaceList();
		}
		else if (o2.type == ListType.Any)
		{
			namespaceList = new NamespaceList();
		}
		else if (o1.type == ListType.Set && o2.type == ListType.Set)
		{
			namespaceList = o1.Clone();
			foreach (string key in o2.set.Keys)
			{
				namespaceList.set[key] = key;
			}
		}
		else if (o1.type == ListType.Other && o2.type == ListType.Other)
		{
			namespaceList = ((!(o1.targetNamespace == o2.targetNamespace)) ? new NamespaceList("##other", string.Empty) : o1.Clone());
		}
		else if (o1.type == ListType.Set && o2.type == ListType.Other)
		{
			namespaceList = (v1Compat ? ((!o1.set.Contains(o2.targetNamespace)) ? o2.Clone() : new NamespaceList()) : ((o2.targetNamespace != string.Empty) ? o1.CompareSetToOther(o2) : ((!o1.set.Contains(string.Empty)) ? new NamespaceList("##other", string.Empty) : new NamespaceList())));
		}
		else if (o2.type == ListType.Set && o1.type == ListType.Other)
		{
			namespaceList = (v1Compat ? ((!o2.set.Contains(o2.targetNamespace)) ? o1.Clone() : new NamespaceList()) : ((o1.targetNamespace != string.Empty) ? o2.CompareSetToOther(o1) : ((!o2.set.Contains(string.Empty)) ? new NamespaceList("##other", string.Empty) : new NamespaceList())));
		}
		return namespaceList;
	}

	private NamespaceList CompareSetToOther(NamespaceList other)
	{
		NamespaceList namespaceList = null;
		if (set.Contains(other.targetNamespace))
		{
			if (set.Contains(string.Empty))
			{
				return new NamespaceList();
			}
			return new NamespaceList("##other", string.Empty);
		}
		if (set.Contains(string.Empty))
		{
			return null;
		}
		return other.Clone();
	}

	public static NamespaceList Intersection(NamespaceList o1, NamespaceList o2, bool v1Compat)
	{
		NamespaceList namespaceList = null;
		if (o1.type == ListType.Any)
		{
			namespaceList = o2.Clone();
		}
		else if (o2.type == ListType.Any)
		{
			namespaceList = o1.Clone();
		}
		else if (o1.type == ListType.Set && o2.type == ListType.Other)
		{
			namespaceList = o1.Clone();
			namespaceList.RemoveNamespace(o2.targetNamespace);
			if (!v1Compat)
			{
				namespaceList.RemoveNamespace(string.Empty);
			}
		}
		else if (o1.type == ListType.Other && o2.type == ListType.Set)
		{
			namespaceList = o2.Clone();
			namespaceList.RemoveNamespace(o1.targetNamespace);
			if (!v1Compat)
			{
				namespaceList.RemoveNamespace(string.Empty);
			}
		}
		else if (o1.type == ListType.Set && o2.type == ListType.Set)
		{
			namespaceList = o1.Clone();
			namespaceList = new NamespaceList();
			namespaceList.type = ListType.Set;
			namespaceList.set = new Hashtable();
			foreach (string key in o1.set.Keys)
			{
				if (o2.set.Contains(key))
				{
					namespaceList.set.Add(key, key);
				}
			}
		}
		else if (o1.type == ListType.Other && o2.type == ListType.Other)
		{
			if (o1.targetNamespace == o2.targetNamespace)
			{
				return o1.Clone();
			}
			if (!v1Compat)
			{
				if (o1.targetNamespace == string.Empty)
				{
					namespaceList = o2.Clone();
				}
				else if (o2.targetNamespace == string.Empty)
				{
					namespaceList = o1.Clone();
				}
			}
		}
		return namespaceList;
	}

	private void RemoveNamespace(string tns)
	{
		if (set[tns] != null)
		{
			set.Remove(tns);
		}
	}

	public bool IsEmpty()
	{
		if (type == ListType.Set)
		{
			if (set != null)
			{
				return set.Count == 0;
			}
			return true;
		}
		return false;
	}
}
