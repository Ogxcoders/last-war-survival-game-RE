using System.Collections.Generic;

namespace System.Runtime.Serialization;

internal struct ObjectReferenceStack
{
	private const int MaximumArraySize = 16;

	private const int InitialArraySize = 4;

	private int count;

	private object[] objectArray;

	private bool[] isReferenceArray;

	private Dictionary<object, object> objectDictionary;

	internal int Count => count;

	internal void Push(object obj)
	{
		if (objectArray == null)
		{
			objectArray = new object[4];
			objectArray[count++] = obj;
			return;
		}
		if (count < 16)
		{
			if (count == objectArray.Length)
			{
				Array.Resize(ref objectArray, objectArray.Length * 2);
			}
			objectArray[count++] = obj;
			return;
		}
		if (objectDictionary == null)
		{
			objectDictionary = new Dictionary<object, object>();
		}
		objectDictionary.Add(obj, null);
		count++;
	}

	internal void EnsureSetAsIsReference(object obj)
	{
		if (count == 0)
		{
			return;
		}
		if (count > 16)
		{
			_ = objectDictionary;
			objectDictionary.Remove(obj);
		}
		else if (objectArray != null && objectArray[count - 1] == obj)
		{
			if (isReferenceArray == null)
			{
				isReferenceArray = new bool[4];
			}
			else if (count == isReferenceArray.Length)
			{
				Array.Resize(ref isReferenceArray, isReferenceArray.Length * 2);
			}
			isReferenceArray[count - 1] = true;
		}
	}

	internal void Pop(object obj)
	{
		if (count > 16)
		{
			_ = objectDictionary;
			objectDictionary.Remove(obj);
		}
		count--;
	}

	internal bool Contains(object obj)
	{
		int num = count;
		if (num > 16)
		{
			if (objectDictionary != null && objectDictionary.ContainsKey(obj))
			{
				return true;
			}
			num = 16;
		}
		for (int num2 = num - 1; num2 >= 0; num2--)
		{
			if (obj == objectArray[num2] && isReferenceArray != null && !isReferenceArray[num2])
			{
				return true;
			}
		}
		return false;
	}
}
