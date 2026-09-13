using System.Threading;

namespace System.Xml.Linq;

public sealed class XNamespace
{
	internal const string xmlPrefixNamespace = "http://www.w3.org/XML/1998/namespace";

	internal const string xmlnsPrefixNamespace = "http://www.w3.org/2000/xmlns/";

	private static XHashtable<WeakReference> s_namespaces;

	private static WeakReference s_refNone;

	private static WeakReference s_refXml;

	private static WeakReference s_refXmlns;

	private string _namespaceName;

	private int _hashCode;

	private XHashtable<XName> _names;

	private const int NamesCapacity = 8;

	private const int NamespacesCapacity = 32;

	public string NamespaceName => _namespaceName;

	public static XNamespace None => EnsureNamespace(ref s_refNone, string.Empty);

	public static XNamespace Xml => EnsureNamespace(ref s_refXml, "http://www.w3.org/XML/1998/namespace");

	public static XNamespace Xmlns => EnsureNamespace(ref s_refXmlns, "http://www.w3.org/2000/xmlns/");

	internal XNamespace(string namespaceName)
	{
		_namespaceName = namespaceName;
		_hashCode = namespaceName.GetHashCode();
		_names = new XHashtable<XName>(ExtractLocalName, 8);
	}

	public XName GetName(string localName)
	{
		if (localName == null)
		{
			throw new ArgumentNullException("localName");
		}
		return GetName(localName, 0, localName.Length);
	}

	public override string ToString()
	{
		return _namespaceName;
	}

	public static XNamespace Get(string namespaceName)
	{
		if (namespaceName == null)
		{
			throw new ArgumentNullException("namespaceName");
		}
		return Get(namespaceName, 0, namespaceName.Length);
	}

	[CLSCompliant(false)]
	public static implicit operator XNamespace(string namespaceName)
	{
		if (namespaceName == null)
		{
			return null;
		}
		return Get(namespaceName);
	}

	public static XName operator +(XNamespace ns, string localName)
	{
		if (ns == null)
		{
			throw new ArgumentNullException("ns");
		}
		return ns.GetName(localName);
	}

	public override bool Equals(object obj)
	{
		return this == obj;
	}

	public override int GetHashCode()
	{
		return _hashCode;
	}

	public static bool operator ==(XNamespace left, XNamespace right)
	{
		return (object)left == right;
	}

	public static bool operator !=(XNamespace left, XNamespace right)
	{
		return (object)left != right;
	}

	internal XName GetName(string localName, int index, int count)
	{
		if (_names.TryGetValue(localName, index, count, out var value))
		{
			return value;
		}
		return _names.Add(new XName(this, localName.Substring(index, count)));
	}

	internal static XNamespace Get(string namespaceName, int index, int count)
	{
		if (count == 0)
		{
			return None;
		}
		if (s_namespaces == null)
		{
			Interlocked.CompareExchange(ref s_namespaces, new XHashtable<WeakReference>(ExtractNamespace, 32), null);
		}
		XNamespace xNamespace;
		do
		{
			if (!s_namespaces.TryGetValue(namespaceName, index, count, out var value))
			{
				if (count == "http://www.w3.org/XML/1998/namespace".Length && string.CompareOrdinal(namespaceName, index, "http://www.w3.org/XML/1998/namespace", 0, count) == 0)
				{
					return Xml;
				}
				if (count == "http://www.w3.org/2000/xmlns/".Length && string.CompareOrdinal(namespaceName, index, "http://www.w3.org/2000/xmlns/", 0, count) == 0)
				{
					return Xmlns;
				}
				value = s_namespaces.Add(new WeakReference(new XNamespace(namespaceName.Substring(index, count))));
			}
			xNamespace = ((value != null) ? ((XNamespace)value.Target) : null);
		}
		while (xNamespace == null);
		return xNamespace;
	}

	private static string ExtractLocalName(XName n)
	{
		return n.LocalName;
	}

	private static string ExtractNamespace(WeakReference r)
	{
		XNamespace xNamespace;
		if (r == null || (xNamespace = (XNamespace)r.Target) == null)
		{
			return null;
		}
		return xNamespace.NamespaceName;
	}

	private static XNamespace EnsureNamespace(ref WeakReference refNmsp, string namespaceName)
	{
		XNamespace xNamespace;
		while (true)
		{
			WeakReference weakReference = refNmsp;
			if (weakReference != null)
			{
				xNamespace = (XNamespace)weakReference.Target;
				if (xNamespace != null)
				{
					break;
				}
			}
			Interlocked.CompareExchange(ref refNmsp, new WeakReference(new XNamespace(namespaceName)), weakReference);
		}
		return xNamespace;
	}
}
