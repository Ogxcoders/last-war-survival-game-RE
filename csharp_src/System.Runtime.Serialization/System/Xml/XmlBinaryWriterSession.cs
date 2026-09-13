using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Xml;

public class XmlBinaryWriterSession
{
	private class PriorityDictionary<K, V> where K : class
	{
		private struct Entry
		{
			public K Key;

			public V Value;

			public int Time;
		}

		private Dictionary<K, V> dictionary;

		private Entry[] list;

		private int listCount;

		private int now;

		private int Now
		{
			get
			{
				if (++now == int.MaxValue)
				{
					DecreaseAll();
				}
				return now;
			}
		}

		public PriorityDictionary()
		{
			list = new Entry[16];
		}

		public void Clear()
		{
			now = 0;
			listCount = 0;
			Array.Clear(list, 0, list.Length);
			if (dictionary != null)
			{
				dictionary.Clear();
			}
		}

		public bool TryGetValue(K key, out V value)
		{
			for (int i = 0; i < listCount; i++)
			{
				if (list[i].Key == key)
				{
					value = list[i].Value;
					list[i].Time = Now;
					return true;
				}
			}
			for (int j = 0; j < listCount; j++)
			{
				if (list[j].Key.Equals(key))
				{
					value = list[j].Value;
					list[j].Time = Now;
					return true;
				}
			}
			if (dictionary == null)
			{
				value = default(V);
				return false;
			}
			if (!dictionary.TryGetValue(key, out value))
			{
				return false;
			}
			int num = 0;
			int time = list[0].Time;
			for (int k = 1; k < listCount; k++)
			{
				if (list[k].Time < time)
				{
					num = k;
					time = list[k].Time;
				}
			}
			list[num].Key = key;
			list[num].Value = value;
			list[num].Time = Now;
			return true;
		}

		public void Add(K key, V value)
		{
			if (listCount < list.Length)
			{
				list[listCount].Key = key;
				list[listCount].Value = value;
				listCount++;
				return;
			}
			if (dictionary == null)
			{
				dictionary = new Dictionary<K, V>();
				for (int i = 0; i < listCount; i++)
				{
					dictionary.Add(list[i].Key, list[i].Value);
				}
			}
			dictionary.Add(key, value);
		}

		private void DecreaseAll()
		{
			for (int i = 0; i < listCount; i++)
			{
				list[i].Time /= 2;
			}
			now /= 2;
		}
	}

	private class IntArray
	{
		private int[] array;

		public int this[int index]
		{
			get
			{
				if (index >= array.Length)
				{
					return 0;
				}
				return array[index];
			}
			set
			{
				if (index >= array.Length)
				{
					int[] destinationArray = new int[Math.Max(index + 1, array.Length * 2)];
					Array.Copy(array, destinationArray, array.Length);
					array = destinationArray;
				}
				array[index] = value;
			}
		}

		public IntArray(int size)
		{
			array = new int[size];
		}
	}

	private PriorityDictionary<string, int> strings;

	private PriorityDictionary<IXmlDictionary, IntArray> maps;

	private int nextKey;

	public XmlBinaryWriterSession()
	{
		nextKey = 0;
		maps = new PriorityDictionary<IXmlDictionary, IntArray>();
		strings = new PriorityDictionary<string, int>();
	}

	public virtual bool TryAdd(XmlDictionaryString value, out int key)
	{
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
		}
		if (maps.TryGetValue(value.Dictionary, out var value2))
		{
			key = value2[value.Key] - 1;
			if (key != -1)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("The specified key already exists in the dictionary.")));
			}
			key = Add(value.Value);
			value2[value.Key] = key + 1;
			return true;
		}
		key = Add(value.Value);
		value2 = AddKeys(value.Dictionary, value.Key + 1);
		value2[value.Key] = key + 1;
		return true;
	}

	private int Add(string s)
	{
		int num = nextKey++;
		strings.Add(s, num);
		return num;
	}

	private IntArray AddKeys(IXmlDictionary dictionary, int minCount)
	{
		IntArray intArray = new IntArray(Math.Max(minCount, 16));
		maps.Add(dictionary, intArray);
		return intArray;
	}

	public void Reset()
	{
		nextKey = 0;
		maps.Clear();
		strings.Clear();
	}

	internal bool TryLookup(XmlDictionaryString s, out int key)
	{
		if (maps.TryGetValue(s.Dictionary, out var value))
		{
			key = value[s.Key] - 1;
			if (key != -1)
			{
				return true;
			}
		}
		if (strings.TryGetValue(s.Value, out key))
		{
			if (value == null)
			{
				value = AddKeys(s.Dictionary, s.Key + 1);
			}
			value[s.Key] = key + 1;
			return true;
		}
		key = -1;
		return false;
	}
}
