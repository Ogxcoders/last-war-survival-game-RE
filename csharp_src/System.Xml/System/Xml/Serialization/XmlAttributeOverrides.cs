using System.Collections;
using System.Globalization;
using System.Text;

namespace System.Xml.Serialization;

public class XmlAttributeOverrides
{
	private Hashtable overrides;

	public XmlAttributes this[Type type] => this[type, string.Empty];

	public XmlAttributes this[Type type, string member] => (XmlAttributes)overrides[GetKey(type, member)];

	public XmlAttributeOverrides()
	{
		overrides = new Hashtable();
	}

	public void Add(Type type, XmlAttributes attributes)
	{
		Add(type, string.Empty, attributes);
	}

	public void Add(Type type, string member, XmlAttributes attributes)
	{
		if (overrides[GetKey(type, member)] != null)
		{
			throw new Exception("The attributes for the given type and Member already exist in the collection");
		}
		overrides.Add(GetKey(type, member), attributes);
	}

	private TypeMember GetKey(Type type, string member)
	{
		return new TypeMember(type, member);
	}

	internal void AddKeyHash(StringBuilder sb)
	{
		sb.Append("XAO ");
		foreach (DictionaryEntry @override in overrides)
		{
			XmlAttributes obj = (XmlAttributes)@override.Value;
			IFormattable formattable = @override.Key as IFormattable;
			sb.Append((formattable != null) ? formattable.ToString(null, CultureInfo.InvariantCulture) : @override.Key.ToString()).Append(' ');
			obj.AddKeyHash(sb);
		}
		sb.Append("|");
	}
}
