using System;
using System.Collections.Generic;

namespace RiverGame.PerformanceAnalysis;

public class EnumToName<T> where T : Enum
{
	private Dictionary<T, string> m_Names;

	public string this[T level]
	{
		get
		{
			if (m_Names == null)
			{
				m_Names = new Dictionary<T, string>();
			}
			if (!m_Names.TryGetValue(level, out var value))
			{
				value = level.ToString();
				m_Names[level] = value;
			}
			return value;
		}
	}
}
