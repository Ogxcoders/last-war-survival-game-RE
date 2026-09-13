using System.Reflection;
using System.Security;

namespace System.Xml;

[Serializable]
public class XmlQualifiedName
{
	private delegate int HashCodeOfStringDelegate(string s, int sLen, long additionalEntropy);

	private static HashCodeOfStringDelegate hashCodeDelegate = null;

	private string name;

	private string ns;

	[NonSerialized]
	private int hash;

	public static readonly XmlQualifiedName Empty = new XmlQualifiedName(string.Empty);

	public string Namespace => ns;

	public string Name => name;

	public bool IsEmpty
	{
		get
		{
			if (Name.Length == 0)
			{
				return Namespace.Length == 0;
			}
			return false;
		}
	}

	public XmlQualifiedName()
		: this(string.Empty, string.Empty)
	{
	}

	public XmlQualifiedName(string name)
		: this(name, string.Empty)
	{
	}

	public XmlQualifiedName(string name, string ns)
	{
		this.ns = ((ns == null) ? string.Empty : ns);
		this.name = ((name == null) ? string.Empty : name);
	}

	public override int GetHashCode()
	{
		if (hash == 0)
		{
			if (hashCodeDelegate == null)
			{
				hashCodeDelegate = GetHashCodeDelegate();
			}
			hash = hashCodeDelegate(Name, Name.Length, 0L);
		}
		return hash;
	}

	public override string ToString()
	{
		if (Namespace.Length != 0)
		{
			return Namespace + ":" + Name;
		}
		return Name;
	}

	public override bool Equals(object other)
	{
		if (this == other)
		{
			return true;
		}
		XmlQualifiedName xmlQualifiedName = other as XmlQualifiedName;
		if (xmlQualifiedName != null)
		{
			if (Name == xmlQualifiedName.Name)
			{
				return Namespace == xmlQualifiedName.Namespace;
			}
			return false;
		}
		return false;
	}

	public static bool operator ==(XmlQualifiedName a, XmlQualifiedName b)
	{
		if ((object)a == b)
		{
			return true;
		}
		if ((object)a == null || (object)b == null)
		{
			return false;
		}
		if (a.Name == b.Name)
		{
			return a.Namespace == b.Namespace;
		}
		return false;
	}

	public static bool operator !=(XmlQualifiedName a, XmlQualifiedName b)
	{
		return !(a == b);
	}

	public static string ToString(string name, string ns)
	{
		if (ns != null && ns.Length != 0)
		{
			return ns + ":" + name;
		}
		return name;
	}

	[SecuritySafeCritical]
	private static HashCodeOfStringDelegate GetHashCodeDelegate()
	{
		if (!IsRandomizedHashingDisabled())
		{
			MethodInfo method = typeof(string).GetMethod("InternalMarvin32HashString", BindingFlags.Static | BindingFlags.NonPublic);
			if (method != null)
			{
				return (HashCodeOfStringDelegate)Delegate.CreateDelegate(typeof(HashCodeOfStringDelegate), method);
			}
		}
		return GetHashCodeOfString;
	}

	private static bool IsRandomizedHashingDisabled()
	{
		return false;
	}

	private static int GetHashCodeOfString(string s, int length, long additionalEntropy)
	{
		return s.GetHashCode();
	}

	internal void Init(string name, string ns)
	{
		this.name = name;
		this.ns = ns;
		hash = 0;
	}

	internal void SetNamespace(string ns)
	{
		this.ns = ns;
	}

	internal void Verify()
	{
		XmlConvert.VerifyNCName(name);
		if (ns.Length != 0)
		{
			XmlConvert.ToUri(ns);
		}
	}

	internal void Atomize(XmlNameTable nameTable)
	{
		name = nameTable.Add(name);
		ns = nameTable.Add(ns);
	}

	internal static XmlQualifiedName Parse(string s, IXmlNamespaceResolver nsmgr, out string prefix)
	{
		ValidateNames.ParseQNameThrow(s, out prefix, out var localName);
		string text = nsmgr.LookupNamespace(prefix);
		if (text == null)
		{
			if (prefix.Length != 0)
			{
				throw new XmlException("'{0}' is an undeclared prefix.", prefix);
			}
			text = string.Empty;
		}
		return new XmlQualifiedName(localName, text);
	}

	internal XmlQualifiedName Clone()
	{
		return (XmlQualifiedName)MemberwiseClone();
	}

	internal static int Compare(XmlQualifiedName a, XmlQualifiedName b)
	{
		if (null == a)
		{
			if (!(null == b))
			{
				return -1;
			}
			return 0;
		}
		if (null == b)
		{
			return 1;
		}
		int num = string.CompareOrdinal(a.Namespace, b.Namespace);
		if (num == 0)
		{
			num = string.CompareOrdinal(a.Name, b.Name);
		}
		return num;
	}
}
