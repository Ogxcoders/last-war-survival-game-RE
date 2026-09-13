using System;
using System.Text;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule;

internal class RenderGraphLogger
{
	private StringBuilder m_Builder = new StringBuilder();

	private int m_CurrentIndentation;

	public void Initialize()
	{
		m_Builder.Clear();
		m_CurrentIndentation = 0;
	}

	public void IncrementIndentation(int value)
	{
		m_CurrentIndentation += Math.Abs(value);
	}

	public void DecrementIndentation(int value)
	{
		m_CurrentIndentation = Math.Max(0, m_CurrentIndentation - Math.Abs(value));
	}

	public void LogLine(string format, params object[] args)
	{
		for (int i = 0; i < m_CurrentIndentation; i++)
		{
			m_Builder.Append('\t');
		}
		m_Builder.AppendFormat(format, args);
		m_Builder.AppendLine();
	}

	public string GetLog()
	{
		return m_Builder.ToString();
	}
}
