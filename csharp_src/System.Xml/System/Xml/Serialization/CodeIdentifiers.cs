using System.Collections;
using System.Globalization;

namespace System.Xml.Serialization;

public class CodeIdentifiers
{
	private Hashtable identifiers;

	private Hashtable reservedIdentifiers;

	private ArrayList list;

	private bool camelCase;

	public bool UseCamelCasing
	{
		get
		{
			return camelCase;
		}
		set
		{
			camelCase = value;
		}
	}

	public CodeIdentifiers()
		: this(caseSensitive: true)
	{
	}

	public CodeIdentifiers(bool caseSensitive)
	{
		if (caseSensitive)
		{
			identifiers = new Hashtable();
			reservedIdentifiers = new Hashtable();
		}
		else
		{
			IEqualityComparer equalityComparer = new CaseInsensitiveKeyComparer();
			identifiers = new Hashtable(equalityComparer);
			reservedIdentifiers = new Hashtable(equalityComparer);
		}
		list = new ArrayList();
	}

	public void Clear()
	{
		identifiers.Clear();
		list.Clear();
	}

	public string MakeRightCase(string identifier)
	{
		if (camelCase)
		{
			return CodeIdentifier.MakeCamel(identifier);
		}
		return CodeIdentifier.MakePascal(identifier);
	}

	public string MakeUnique(string identifier)
	{
		if (IsInUse(identifier))
		{
			int num = 1;
			string text;
			while (true)
			{
				text = identifier + num.ToString(CultureInfo.InvariantCulture);
				if (!IsInUse(text))
				{
					break;
				}
				num++;
			}
			identifier = text;
		}
		if (identifier.Length > 511)
		{
			return MakeUnique("Item");
		}
		return identifier;
	}

	public void AddReserved(string identifier)
	{
		reservedIdentifiers.Add(identifier, identifier);
	}

	public void RemoveReserved(string identifier)
	{
		reservedIdentifiers.Remove(identifier);
	}

	public string AddUnique(string identifier, object value)
	{
		identifier = MakeUnique(identifier);
		Add(identifier, value);
		return identifier;
	}

	public bool IsInUse(string identifier)
	{
		if (!identifiers.Contains(identifier))
		{
			return reservedIdentifiers.Contains(identifier);
		}
		return true;
	}

	public void Add(string identifier, object value)
	{
		identifiers.Add(identifier, value);
		list.Add(value);
	}

	public void Remove(string identifier)
	{
		list.Remove(identifiers[identifier]);
		identifiers.Remove(identifier);
	}

	public object ToArray(Type type)
	{
		Array array = Array.CreateInstance(type, list.Count);
		list.CopyTo(array, 0);
		return array;
	}

	internal CodeIdentifiers Clone()
	{
		return new CodeIdentifiers
		{
			identifiers = (Hashtable)identifiers.Clone(),
			reservedIdentifiers = (Hashtable)reservedIdentifiers.Clone(),
			list = (ArrayList)list.Clone(),
			camelCase = camelCase
		};
	}
}
