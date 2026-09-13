using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Xml;

public class XmlDictionary : IXmlDictionary
{
	private class EmptyDictionary : IXmlDictionary
	{
		public bool TryLookup(string value, out XmlDictionaryString result)
		{
			result = null;
			return false;
		}

		public bool TryLookup(int key, out XmlDictionaryString result)
		{
			result = null;
			return false;
		}

		public bool TryLookup(XmlDictionaryString value, out XmlDictionaryString result)
		{
			result = null;
			return false;
		}
	}

	private static IXmlDictionary empty;

	private Dictionary<string, XmlDictionaryString> lookup;

	private XmlDictionaryString[] strings;

	private int nextId;

	public static IXmlDictionary Empty
	{
		get
		{
			if (empty == null)
			{
				empty = new EmptyDictionary();
			}
			return empty;
		}
	}

	public XmlDictionary()
	{
		lookup = new Dictionary<string, XmlDictionaryString>();
		strings = null;
		nextId = 0;
	}

	public XmlDictionary(int capacity)
	{
		lookup = new Dictionary<string, XmlDictionaryString>(capacity);
		strings = new XmlDictionaryString[capacity];
		nextId = 0;
	}

	public virtual XmlDictionaryString Add(string value)
	{
		if (!lookup.TryGetValue(value, out var value2))
		{
			if (strings == null)
			{
				strings = new XmlDictionaryString[4];
			}
			else if (nextId == strings.Length)
			{
				int num = nextId * 2;
				if (num == 0)
				{
					num = 4;
				}
				Array.Resize(ref strings, num);
			}
			value2 = new XmlDictionaryString(this, value, nextId);
			strings[nextId] = value2;
			lookup.Add(value, value2);
			nextId++;
		}
		return value2;
	}

	public virtual bool TryLookup(string value, out XmlDictionaryString result)
	{
		return lookup.TryGetValue(value, out result);
	}

	public virtual bool TryLookup(int key, out XmlDictionaryString result)
	{
		if (key < 0 || key >= nextId)
		{
			result = null;
			return false;
		}
		result = strings[key];
		return true;
	}

	public virtual bool TryLookup(XmlDictionaryString value, out XmlDictionaryString result)
	{
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
		}
		if (value.Dictionary != this)
		{
			result = null;
			return false;
		}
		result = value;
		return true;
	}
}
