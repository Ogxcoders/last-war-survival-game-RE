using System.Collections;
using System.Xml;

namespace System.Runtime.Diagnostics;

internal class DictionaryTraceRecord : TraceRecord
{
	private IDictionary dictionary;

	internal override string EventId => "http://schemas.microsoft.com/2006/08/ServiceModel/DictionaryTraceRecord";

	internal DictionaryTraceRecord(IDictionary dictionary)
	{
		this.dictionary = dictionary;
	}

	internal override void WriteTo(XmlWriter xml)
	{
		if (dictionary == null)
		{
			return;
		}
		foreach (object key in dictionary.Keys)
		{
			object obj = dictionary[key];
			xml.WriteElementString(key.ToString(), (obj == null) ? string.Empty : obj.ToString());
		}
	}
}
