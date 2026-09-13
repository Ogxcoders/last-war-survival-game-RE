using System.Runtime.Serialization;
using System.Text;

namespace System.Xml;

public class XmlDictionaryString
{
	private class EmptyStringDictionary : IXmlDictionary
	{
		private XmlDictionaryString empty;

		public XmlDictionaryString EmptyString => empty;

		public EmptyStringDictionary()
		{
			empty = new XmlDictionaryString(this, string.Empty, 0);
		}

		public bool TryLookup(string value, out XmlDictionaryString result)
		{
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
			}
			if (value.Length == 0)
			{
				result = empty;
				return true;
			}
			result = null;
			return false;
		}

		public bool TryLookup(int key, out XmlDictionaryString result)
		{
			if (key == 0)
			{
				result = empty;
				return true;
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
	}

	internal const int MinKey = 0;

	internal const int MaxKey = 536870911;

	private IXmlDictionary dictionary;

	private string value;

	private int key;

	private byte[] buffer;

	private static EmptyStringDictionary emptyStringDictionary = new EmptyStringDictionary();

	public static XmlDictionaryString Empty => emptyStringDictionary.EmptyString;

	public IXmlDictionary Dictionary => dictionary;

	public int Key => key;

	public string Value => value;

	public XmlDictionaryString(IXmlDictionary dictionary, string value, int key)
	{
		if (dictionary == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("dictionary"));
		}
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("value"));
		}
		if (key < 0 || key > 536870911)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("key", SR.GetString("The value of this argument must fall within the range {0} to {1}.", 0, 536870911)));
		}
		this.dictionary = dictionary;
		this.value = value;
		this.key = key;
	}

	internal static string GetString(XmlDictionaryString s)
	{
		return s?.Value;
	}

	internal byte[] ToUTF8()
	{
		if (buffer == null)
		{
			buffer = Encoding.UTF8.GetBytes(value);
		}
		return buffer;
	}

	public override string ToString()
	{
		return value;
	}
}
