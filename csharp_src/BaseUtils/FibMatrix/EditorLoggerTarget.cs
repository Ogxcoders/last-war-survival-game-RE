using System.Text;
using GameFramework;
using UnityEngine;

namespace FibMatrix;

public class EditorLoggerTarget : ILoggerTarget
{
	private StringBuilder _sb = new StringBuilder(200);

	public LogTargetType targetType => LogTargetType.Runtime;

	public void Debug(string message, object context, LogImportance importance)
	{
	}

	public void Info(string message, object context, LogImportance importance)
	{
		Log.Info(message, context as Object);
	}

	public void Warning(string message, object context, LogImportance importance)
	{
		Log.Warning(message, context as Object);
	}

	public void Error(string message, string stackTrace, object context, LogImportance importance)
	{
		string format;
		lock (_sb)
		{
			_sb.Clear();
			_sb.Append("Message:");
			_sb.AppendLine(message);
			_sb.AppendLine("StackTrace:");
			_sb.AppendLine(stackTrace);
			format = _sb.ToString();
		}
		Log.Error(format, context as Object);
	}
}
