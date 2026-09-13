using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Xml;

public class XmlBinaryReaderSession : IXmlDictionary
{
	private const int MaxArrayEntries = 2048;

	private XmlDictionaryString[] strings;

	private Dictionary<int, XmlDictionaryString> stringDict;

	public XmlDictionaryString Add(int id, string value)
	{
		if (id < 0)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException(SR.GetString("ID must be >= 0.")));
		}
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
		}
		if (TryLookup(id, out var result))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("ID already defined.")));
		}
		result = new XmlDictionaryString(this, value, id);
		if (id >= 2048)
		{
			if (stringDict == null)
			{
				stringDict = new Dictionary<int, XmlDictionaryString>();
			}
			stringDict.Add(id, result);
		}
		else
		{
			if (strings == null)
			{
				strings = new XmlDictionaryString[Math.Max(id + 1, 16)];
			}
			else if (id >= strings.Length)
			{
				XmlDictionaryString[] destinationArray = new XmlDictionaryString[Math.Min(Math.Max(id + 1, strings.Length * 2), 2048)];
				Array.Copy(strings, destinationArray, strings.Length);
				strings = destinationArray;
			}
			strings[id] = result;
		}
		return result;
	}

	public bool TryLookup(int key, out XmlDictionaryString result)
	{
		if (strings != null && key >= 0 && key < strings.Length)
		{
			result = strings[key];
			return result != null;
		}
		if (key >= 2048 && stringDict != null)
		{
			return stringDict.TryGetValue(key, out result);
		}
		result = null;
		return false;
	}

	public bool TryLookup(string value, out XmlDictionaryString result)
	{
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
		}
		if (strings != null)
		{
			for (int i = 0; i < strings.Length; i++)
			{
				XmlDictionaryString xmlDictionaryString = strings[i];
				if (xmlDictionaryString != null && xmlDictionaryString.Value == value)
				{
					result = xmlDictionaryString;
					return true;
				}
			}
		}
		if (stringDict != null)
		{
			foreach (XmlDictionaryString value2 in stringDict.Values)
			{
				if (value2.Value == value)
				{
					result = value2;
					return true;
				}
			}
		}
		result = null;
		return false;
	}

	public bool TryLookup(XmlDictionaryString value, out XmlDictionaryString result)
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

	public void Clear()
	{
		if (strings != null)
		{
			Array.Clear(strings, 0, strings.Length);
		}
		if (stringDict != null)
		{
			stringDict.Clear();
		}
	}
}
