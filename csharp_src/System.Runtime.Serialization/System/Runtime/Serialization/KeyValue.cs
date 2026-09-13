namespace System.Runtime.Serialization;

[DataContract(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
internal struct KeyValue<K, V>
{
	private K key;

	private V value;

	[DataMember(IsRequired = true)]
	public K Key
	{
		get
		{
			return key;
		}
		set
		{
			key = value;
		}
	}

	[DataMember(IsRequired = true)]
	public V Value
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
		}
	}

	internal KeyValue(K key, V value)
	{
		this.key = key;
		this.value = value;
	}
}
