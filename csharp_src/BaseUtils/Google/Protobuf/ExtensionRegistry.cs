using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Google.Protobuf;

public sealed class ExtensionRegistry : ICollection<Extension>, IEnumerable<Extension>, IEnumerable, IDeepCloneable<ExtensionRegistry>
{
	private IDictionary<ObjectIntPair<Type>, Extension> extensions;

	public int Count => extensions.Count;

	bool ICollection<Extension>.IsReadOnly => false;

	public ExtensionRegistry()
	{
		extensions = new Dictionary<ObjectIntPair<Type>, Extension>();
	}

	private ExtensionRegistry(IDictionary<ObjectIntPair<Type>, Extension> collection)
	{
		extensions = collection.ToDictionary((KeyValuePair<ObjectIntPair<Type>, Extension> k) => k.Key, (KeyValuePair<ObjectIntPair<Type>, Extension> v) => v.Value);
	}

	internal bool ContainsInputField(CodedInputStream stream, Type target, out Extension extension)
	{
		return extensions.TryGetValue(new ObjectIntPair<Type>(target, WireFormat.GetTagFieldNumber(stream.LastTag)), out extension);
	}

	public void Add(Extension extension)
	{
		ProtoPreconditions.CheckNotNull(extension, "extension");
		extensions.Add(new ObjectIntPair<Type>(extension.TargetType, extension.FieldNumber), extension);
	}

	public void AddRange(IEnumerable<Extension> extensions)
	{
		ProtoPreconditions.CheckNotNull(extensions, "extensions");
		foreach (Extension extension in extensions)
		{
			Add(extension);
		}
	}

	public void Clear()
	{
		extensions.Clear();
	}

	public bool Contains(Extension item)
	{
		ProtoPreconditions.CheckNotNull(item, "item");
		return extensions.ContainsKey(new ObjectIntPair<Type>(item.TargetType, item.FieldNumber));
	}

	void ICollection<Extension>.CopyTo(Extension[] array, int arrayIndex)
	{
		ProtoPreconditions.CheckNotNull(array, "array");
		if (arrayIndex < 0 || arrayIndex >= array.Length)
		{
			throw new ArgumentOutOfRangeException("arrayIndex");
		}
		if (array.Length - arrayIndex < Count)
		{
			throw new ArgumentException("The provided array is shorter than the number of elements in the registry");
		}
		foreach (Extension extension in array)
		{
			extensions.Add(new ObjectIntPair<Type>(extension.TargetType, extension.FieldNumber), extension);
		}
	}

	public IEnumerator<Extension> GetEnumerator()
	{
		return extensions.Values.GetEnumerator();
	}

	public bool Remove(Extension item)
	{
		ProtoPreconditions.CheckNotNull(item, "item");
		return extensions.Remove(new ObjectIntPair<Type>(item.TargetType, item.FieldNumber));
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public ExtensionRegistry Clone()
	{
		return new ExtensionRegistry(extensions);
	}
}
