using System.Collections;
using System.Runtime.Serialization;

namespace System.Data;

[Serializable]
public class PropertyCollection : Hashtable, ICloneable
{
	public PropertyCollection()
	{
	}

	protected PropertyCollection(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override object Clone()
	{
		PropertyCollection propertyCollection = new PropertyCollection();
		IDictionaryEnumerator dictionaryEnumerator = GetEnumerator();
		try
		{
			while (dictionaryEnumerator.MoveNext())
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)dictionaryEnumerator.Current;
				propertyCollection.Add(dictionaryEntry.Key, dictionaryEntry.Value);
			}
			return propertyCollection;
		}
		finally
		{
			IDisposable disposable = dictionaryEnumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}
}
