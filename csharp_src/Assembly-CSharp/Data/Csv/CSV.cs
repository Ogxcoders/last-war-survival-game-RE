using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Data.Csv;

public class CSV
{
	public Encoding CsvEncoding { get; set; } = Encoding.UTF8;

	public bool TrimColumns { get; set; }

	public char CsvSeparator { get; set; } = ',';

	public List<List<string>> Data { get; set; } = new List<List<string>>();

	public int RowCount => Data.Count;

	public int ColumnCount
	{
		get
		{
			if (Data == null || Data.Count <= 0)
			{
				return 0;
			}
			return Data[0].Count;
		}
	}

	public string this[int row, int column]
	{
		get
		{
			return GetData(row, column);
		}
		set
		{
			SetData(row, column, value);
		}
	}

	public CSV()
	{
	}

	public CSV(string filepath)
		: this(filepath, Encoding.Default)
	{
	}

	public CSV(string filepath, Encoding encoding)
	{
		CsvEncoding = encoding;
		Read(filepath);
	}

	public string GetData(int row, int column)
	{
		if (!IsInRange(row, column))
		{
			return null;
		}
		return Data[row][column];
	}

	public void AddColumn(string[] columnData = null, int columnID = int.MaxValue)
	{
		columnData = columnData ?? new string[RowCount];
		List<string> list = new List<string>();
		for (int i = 0; i < ColumnCount; i++)
		{
			list.Add((i < columnData.Length) ? columnData[i] : "");
		}
		columnID = ((columnID >= 0) ? columnID : 0);
		columnID = ((columnID >= ColumnCount) ? ColumnCount : columnID);
		for (int j = 0; j < RowCount; j++)
		{
			Data[j].Insert(columnID, columnData[j]);
		}
	}

	public void RemoveColumn(int columnId)
	{
		if (0 <= columnId && columnId < ColumnCount)
		{
			for (int i = 0; i < RowCount; i++)
			{
				Data[i].RemoveAt(columnId);
			}
		}
	}

	public void RemoveColumns(params int[] columnIds)
	{
		(from c in columnIds.Distinct()
			orderby c descending
			select c).ToList().ForEach(delegate(int c)
		{
			RemoveColumn(c);
		});
	}

	public void AddRow(string[] strs = null, int rowID = int.MaxValue)
	{
		strs = strs ?? new string[ColumnCount];
		List<string> list = new List<string>();
		for (int i = 0; i < ColumnCount; i++)
		{
			list.Add((i < strs.Length) ? strs[i] : "");
		}
		rowID = ((rowID >= 0) ? rowID : 0);
		rowID = ((rowID >= RowCount) ? RowCount : rowID);
		Data.Insert(rowID, list);
	}

	public void AddRow(int rowNo, params string[] rowdata)
	{
		AddRow(rowdata, rowNo);
	}

	public bool IsInRange(int row, int column)
	{
		if (row >= 0 && row < RowCount && column >= 0)
		{
			return column < ColumnCount;
		}
		return false;
	}

	public bool SetData(int row, int column, string value)
	{
		bool num = IsInRange(row, column);
		if (num)
		{
			Data[row][column] = value;
		}
		return num;
	}

	private void Read(string csvfile)
	{
		Data = GetListCsvData(csvfile);
	}

	public virtual List<List<string>> GetListCsv(string csvData)
	{
		using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(csvData));
		using StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
		List<List<string>> list = new List<List<string>>();
		bool flag = false;
		for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
		{
			if (flag)
			{
				List<string> list2 = ParseContinueLine(text);
				flag = list2.Count > 0 && list2[list2.Count - 1].EndsWith("\r\n");
				List<string> list3 = list[list.Count - 1];
				list3[list3.Count - 1] += list2[0];
				list2.RemoveAt(0);
				list3.AddRange(list2);
			}
			else
			{
				List<string> list2 = ParseLine(text);
				flag = list2.Count > 0 && list2[list2.Count - 1].EndsWith("\r\n");
				list.Add(list2);
			}
		}
		streamReader.Close();
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			num = ((list[i].Count > num) ? list[i].Count : num);
		}
		foreach (List<string> item in list)
		{
			while (item.Count < num)
			{
				item.Add("");
			}
		}
		return list;
	}

	protected virtual List<List<string>> GetListCsvData(string file)
	{
		StreamReader streamReader = new StreamReader(file, Encoding.UTF8);
		List<List<string>> list = new List<List<string>>();
		bool flag = false;
		for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
		{
			if (flag)
			{
				List<string> list2 = ParseContinueLine(text);
				flag = list2.Count > 0 && list2[list2.Count - 1].EndsWith("\r\n");
				List<string> list3 = list[list.Count - 1];
				list3[list3.Count - 1] += list2[0];
				list2.RemoveAt(0);
				list3.AddRange(list2);
			}
			else
			{
				List<string> list2 = ParseLine(text);
				flag = list2.Count > 0 && list2[list2.Count - 1].EndsWith("\r\n");
				list.Add(list2);
			}
		}
		streamReader.Close();
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			num = ((list[i].Count > num) ? list[i].Count : num);
		}
		foreach (List<string> item in list)
		{
			while (item.Count < num)
			{
				item.Add("");
			}
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
					Debug.LogError(line);
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

	public bool Save(string outcsvfile)
	{
		if (!File.Exists(outcsvfile))
		{
			throw new Exception("find error in your FilePath");
		}
		if (Data == null)
		{
			throw new Exception("your DataSouse is null");
		}
		using (StreamWriter streamWriter = new StreamWriter(outcsvfile, append: false, CsvEncoding))
		{
			foreach (List<string> datum in Data)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < datum.Count; i++)
				{
					stringBuilder.Append(datum[i].Contains("\"") ? string.Format("\"{0}\"", datum[i].Replace("\"", "\"\"")) : $"\"{datum[i]}\"");
					if (i < datum.Count - 1)
					{
						stringBuilder.Append(CsvSeparator);
					}
				}
				streamWriter.WriteLine(stringBuilder.ToString());
			}
		}
		return true;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < Data.Count; i++)
		{
			stringBuilder.AppendLine(string.Join("\t\t", Data[i]));
		}
		return stringBuilder.ToString();
	}
}
