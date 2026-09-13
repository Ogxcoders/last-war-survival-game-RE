using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data.ProviderBase;

internal class BasicFieldNameLookup
{
	private Dictionary<string, int> _fieldNameLookup;

	private readonly string[] _fieldNames;

	private CompareInfo _compareInfo;

	public BasicFieldNameLookup(string[] fieldNames)
	{
		if (fieldNames == null)
		{
			throw ADP.ArgumentNull("fieldNames");
		}
		_fieldNames = fieldNames;
	}

	public BasicFieldNameLookup(ReadOnlyCollection<string> columnNames)
	{
		int count = columnNames.Count;
		string[] array = new string[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = columnNames[i];
		}
		_fieldNames = array;
		GenerateLookup();
	}

	public BasicFieldNameLookup(IDataReader reader)
	{
		int fieldCount = reader.FieldCount;
		string[] array = new string[fieldCount];
		for (int i = 0; i < fieldCount; i++)
		{
			array[i] = reader.GetName(i);
		}
		_fieldNames = array;
	}

	public int GetOrdinal(string fieldName)
	{
		if (fieldName == null)
		{
			throw ADP.ArgumentNull("fieldName");
		}
		int num = IndexOf(fieldName);
		if (-1 == num)
		{
			throw ADP.IndexOutOfRange(fieldName);
		}
		return num;
	}

	public int IndexOfName(string fieldName)
	{
		if (_fieldNameLookup == null)
		{
			GenerateLookup();
		}
		if (!_fieldNameLookup.TryGetValue(fieldName, out var value))
		{
			return -1;
		}
		return value;
	}

	public int IndexOf(string fieldName)
	{
		if (_fieldNameLookup == null)
		{
			GenerateLookup();
		}
		if (!_fieldNameLookup.TryGetValue(fieldName, out var value))
		{
			value = LinearIndexOf(fieldName, CompareOptions.IgnoreCase);
			if (-1 == value)
			{
				value = LinearIndexOf(fieldName, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
			}
		}
		return value;
	}

	protected virtual CompareInfo GetCompareInfo()
	{
		return CultureInfo.InvariantCulture.CompareInfo;
	}

	private int LinearIndexOf(string fieldName, CompareOptions compareOptions)
	{
		if (_compareInfo == null)
		{
			_compareInfo = GetCompareInfo();
		}
		int num = _fieldNames.Length;
		for (int i = 0; i < num; i++)
		{
			if (_compareInfo.Compare(fieldName, _fieldNames[i], compareOptions) == 0)
			{
				_fieldNameLookup[fieldName] = i;
				return i;
			}
		}
		return -1;
	}

	private void GenerateLookup()
	{
		int num = _fieldNames.Length;
		Dictionary<string, int> dictionary = new Dictionary<string, int>(num);
		int num2 = num - 1;
		while (0 <= num2)
		{
			string key = _fieldNames[num2];
			dictionary[key] = num2;
			num2--;
		}
		_fieldNameLookup = dictionary;
	}
}
