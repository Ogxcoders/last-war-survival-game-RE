using System.Xml.Schema;

namespace System.Xml.Serialization;

internal class TypeScope
{
	internal static XmlQualifiedName ParseWsdlArrayType(string type, out string dims, XmlSchemaObject parent)
	{
		int num = type.LastIndexOf(':');
		string text = ((num > 0) ? type.Substring(0, num) : "");
		int num2 = type.IndexOf('[', num + 1);
		if (num2 <= num)
		{
			throw new InvalidOperationException(Res.GetString("Invalid wsd:arrayType syntax: '{0}'.", type));
		}
		string name = type.Substring(num + 1, num2 - num - 1);
		dims = type.Substring(num2);
		while (parent != null)
		{
			if (parent.Namespaces != null)
			{
				string text2 = (string)parent.Namespaces.Namespaces[text];
				if (text2 != null)
				{
					text = text2;
					break;
				}
			}
			parent = parent.Parent;
		}
		return new XmlQualifiedName(name, text);
	}
}
