using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;

namespace Data.Csv;

public class CSVCacheder
{
	public string _name = string.Empty;

	private int _key;

	private readonly Dictionary<string, int> _cachedDicRow = new Dictionary<string, int>();

	private readonly Dictionary<string, int> _cachedDicClo = new Dictionary<string, int>();

	public Encoding CsvEncoding { get; set; } = Encoding.UTF8;

	public bool TrimColumns { get; set; }

	public char CsvSeparator { get; set; } = ',';

	public List<Row> Rows { get; private set; } = new List<Row>();

	public Row this[string row] => GetRow(row);

	public int RowCount => Rows.Count;

	public int ColumnCount
	{
		get
		{
			if (Rows == null || Rows.Count <= 0)
			{
				return 0;
			}
			return Rows[0].Count;
		}
	}

	public bool IsInRange(int row, int column)
	{
		if (row >= 0 && row < RowCount && column >= 0)
		{
			return column < ColumnCount;
		}
		return false;
	}

	public CSVCacheder(string tableName, string text, int keyIndex = 0)
	{
		_name = tableName;
		CsvEncoding = Encoding.Default;
		_key = keyIndex;
		Rows = GetListRow(text);
	}

	public Row GetRow(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		if (_cachedDicRow.TryGetValue(id, out var value))
		{
			if (value < Rows.Count)
			{
				return Rows[value];
			}
		}
		else
		{
			Log.Error("no id {0} in table {1}", id, _name);
		}
		return null;
	}

	public int GetColumnIndex(string key)
	{
		if (!_cachedDicClo.TryGetValue(key, out var value))
		{
			return -1;
		}
		return value;
	}

	public bool hasKey(string key)
	{
		return _cachedDicClo.ContainsKey(key);
	}

	public string GetData(string id, string key)
	{
		if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(key))
		{
			return string.Empty;
		}
		if (_cachedDicRow.TryGetValue(id, out var value))
		{
			return Rows[value].GetData(key);
		}
		return string.Empty;
	}

	public List<Row> GetListRow(string csvData)
	{
		using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(csvData));
		using StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
		List<List<string>> list = new List<List<string>>();
		List<Row> list2 = new List<Row>();
		bool flag = false;
		int num = 0;
		for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
		{
			List<string> list3;
			if (flag)
			{
				list3 = ParseContinueLine(text);
				flag = list3.Count > 0 && list3[list3.Count - 1].EndsWith("\r\n");
				List<string> list4 = list[list.Count - 1];
				list4[list4.Count - 1] += list3[0];
				list3.RemoveAt(0);
				list4.AddRange(list3);
			}
			else
			{
				list3 = ParseLine(text);
				flag = list3.Count > 0 && list3[list3.Count - 1].EndsWith("\r\n");
				list.Add(list3);
			}
			if (!flag)
			{
				if (num > 0)
				{
					List<string> list5 = list[list.Count - 1];
					list2.Add(new Row(this, list5.ToArray()));
					if (_cachedDicRow.ContainsKey(list3[_key]))
					{
						Log.Error("Table: " + _name + " ID: " + list3[_key] + " ID Repetition");
						_cachedDicRow[list3[_key]] = num - 1;
					}
					else
					{
						_cachedDicRow.Add(list3[_key], num - 1);
					}
				}
				else
				{
					if (!string.Equals(list3[0], "id"))
					{
						Log.Error("{} don't have id ", _name);
					}
					for (int i = 0; i < list3.Count; i++)
					{
						_cachedDicClo.Add(list3[i], i);
					}
				}
				num++;
			}
		}
		streamReader.Close();
		int num2 = 0;
		for (int j = 0; j < list.Count; j++)
		{
			num2 = ((list[j].Count > num2) ? list[j].Count : num2);
		}
		foreach (Row item in list2)
		{
			if (item.Count < num2)
			{
				Log.Error("Table :" + _name + " maxColumn > row.Count row : " + item.ToString());
			}
		}
		return list2;
	}

	protected List<string> ParseLine(string line)
	{
		StringBuilder stringBuilder = new StringBuilder();
		List<string> list = new List<string>();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		stringBuilder.Remove(0, stringBuilder.Length);
		if (line == "")
		{
			list.Add("");
		}
		for (int i = 0; i < line.Length; i++)
		{
			char c = line[i];
			if (!flag)
			{
				flag = true;
				if (c == '"')
				{
					flag2 = true;
					continue;
				}
			}
			if (flag2)
			{
				if (i + 1 == line.Length)
				{
					if (c == '"')
					{
						flag2 = false;
						continue;
					}
					flag3 = true;
				}
				else if (c == '"' && line[i + 1] == CsvSeparator)
				{
					flag2 = false;
					flag = false;
					i++;
				}
				else if (c == '"' && line[i + 1] == '"')
				{
					i++;
				}
				else if (c == '"')
				{
					Log.Error(line);
					throw new Exception("格式错误，错误的双引号转义");
				}
			}
			else if (c == CsvSeparator)
			{
				flag = false;
			}
			if (!flag)
			{
				list.Add(TrimColumns ? stringBuilder.ToString().Trim() : stringBuilder.ToString());
				stringBuilder.Remove(0, stringBuilder.Length);
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		if (flag)
		{
			if (flag3)
			{
				stringBuilder.Append("\r\n");
			}
			list.Add(TrimColumns ? stringBuilder.ToString().Trim() : stringBuilder.ToString());
		}
		else
		{
			list.Add("");
		}
		return list;
	}

	protected List<string> ParseContinueLine(string line)
	{
		StringBuilder stringBuilder = new StringBuilder();
		List<string> list = new List<string>();
		stringBuilder.Remove(0, stringBuilder.Length);
		if (line == "")
		{
			list.Add("\r\n");
			return list;
		}
		for (int i = 0; i < line.Length; i++)
		{
			char c = line[i];
			if (i + 1 == line.Length)
			{
				if (c == '"')
				{
					list.Add(TrimColumns ? stringBuilder.ToString().TrimEnd(Array.Empty<char>()) : stringBuilder.ToString());
					return list;
				}
				stringBuilder.Append("\r\n");
				list.Add(stringBuilder.ToString());
				return list;
			}
			if (c == '"' && line[i + 1] == CsvSeparator)
			{
				list.Add(TrimColumns ? stringBuilder.ToString().TrimEnd(Array.Empty<char>()) : stringBuilder.ToString());
				i++;
				list.AddRange(ParseLine(line.Remove(0, i + 1)));
				break;
			}
			if (c == '"' && line[i + 1] == '"')
			{
				i++;
			}
			else if (c == '"')
			{
				throw new Exception("格式错误，错误的双引号转义");
			}
			stringBuilder.Append(c);
		}
		return list;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < Rows.Count; i++)
		{
			stringBuilder.AppendLine(string.Join("\t\t", Rows[i].ToString()));
		}
		return stringBuilder.ToString();
	}
}
