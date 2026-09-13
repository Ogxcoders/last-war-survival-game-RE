using System.Text;

namespace System.Collections.Generic;

public static class KeyValuePair
{
	public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
	{
		return new KeyValuePair<TKey, TValue>(key, value);
	}

	internal static string PairToString(object key, object value)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		stringBuilder.Append('[');
		if (key != null)
		{
			stringBuilder.Append(key);
		}
		stringBuilder.Append(", ");
		if (value != null)
		{
			stringBuilder.Append(value);
		}
		stringBuilder.Append(']');
		return StringBuilderCache.GetStringAndRelease(stringBuilder);
	}
}
[Serializable]
public readonly struct KeyValuePair<TKey, TValue>
{
	private readonly TKey key;

	private readonly TValue value;

	public TKey Key => key;

	public TValue Value => value;

	public KeyValuePair(TKey key, TValue value)
	{
		this.key = key;
		this.value = value;
	}

	public override string ToString()
	{
		return KeyValuePair.PairToString(Key, Value);
	}

	public void Deconstruct(out TKey key, out TValue value)
	{
		key = Key;
		value = Value;
	}
}
