using System.Collections.Generic;

namespace System.Runtime.Serialization;

internal class HybridObjectCache
{
	private Dictionary<string, object> objectDictionary;

	private Dictionary<string, object> referencedObjectDictionary;

	internal HybridObjectCache()
	{
	}

	internal void Add(string id, object obj)
	{
		if (objectDictionary == null)
		{
			objectDictionary = new Dictionary<string, object>();
		}
		if (objectDictionary.TryGetValue(id, out var _))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Invalid XML encountered. The same Id value '{0}' is defined more than once. Multiple objects cannot be deserialized using the same Id.", id)));
		}
		objectDictionary.Add(id, obj);
	}

	internal void Remove(string id)
	{
		if (objectDictionary != null)
		{
			objectDictionary.Remove(id);
		}
	}

	internal object GetObject(string id)
	{
		if (referencedObjectDictionary == null)
		{
			referencedObjectDictionary = new Dictionary<string, object>();
			referencedObjectDictionary.Add(id, null);
		}
		else if (!referencedObjectDictionary.ContainsKey(id))
		{
			referencedObjectDictionary.Add(id, null);
		}
		if (objectDictionary != null)
		{
			objectDictionary.TryGetValue(id, out var value);
			return value;
		}
		return null;
	}

	internal bool IsObjectReferenced(string id)
	{
		if (referencedObjectDictionary != null)
		{
			return referencedObjectDictionary.ContainsKey(id);
		}
		return false;
	}
}
