using System.Collections.Generic;
using System.ComponentModel;

namespace System.Xml.Xsl.Runtime;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct StringConcat
{
	private string s1;

	private string s2;

	private string s3;

	private string s4;

	private string delimiter;

	private List<string> strList;

	private int idxStr;

	public string Delimiter
	{
		get
		{
			return delimiter;
		}
		set
		{
			delimiter = value;
		}
	}

	internal int Count => idxStr;

	public void Clear()
	{
		idxStr = 0;
		delimiter = null;
	}

	public void Concat(string value)
	{
		if (delimiter != null && idxStr != 0)
		{
			ConcatNoDelimiter(delimiter);
		}
		ConcatNoDelimiter(value);
	}

	public string GetResult()
	{
		return idxStr switch
		{
			0 => string.Empty, 
			1 => s1, 
			2 => s1 + s2, 
			3 => s1 + s2 + s3, 
			4 => s1 + s2 + s3 + s4, 
			_ => string.Concat(strList.ToArray()), 
		};
	}

	internal void ConcatNoDelimiter(string s)
	{
		switch (idxStr)
		{
		case 0:
			s1 = s;
			break;
		case 1:
			s2 = s;
			break;
		case 2:
			s3 = s;
			break;
		case 3:
			s4 = s;
			break;
		case 4:
		{
			int capacity = ((strList == null) ? 8 : strList.Count);
			List<string> list = (strList = new List<string>(capacity));
			list.Add(s1);
			list.Add(s2);
			list.Add(s3);
			list.Add(s4);
			goto default;
		}
		default:
			strList.Add(s);
			break;
		}
		idxStr++;
	}
}
