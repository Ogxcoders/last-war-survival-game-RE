using System.Collections;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace System.Diagnostics;

[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
public class DelimitedListTraceListener : TextWriterTraceListener
{
	private string delimiter = ";";

	private string secondaryDelim = ",";

	private bool initializedDelim;

	public string Delimiter
	{
		get
		{
			lock (this)
			{
				if (!initializedDelim)
				{
					if (base.Attributes.ContainsKey("delimiter"))
					{
						delimiter = base.Attributes["delimiter"];
					}
					initializedDelim = true;
				}
			}
			return delimiter;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("Delimiter");
			}
			if (value.Length == 0)
			{
				throw new ArgumentException(global::SR.GetString("Generic_ArgCantBeEmptyString", "Delimiter"));
			}
			lock (this)
			{
				delimiter = value;
				initializedDelim = true;
			}
			if (delimiter == ",")
			{
				secondaryDelim = ";";
			}
			else
			{
				secondaryDelim = ",";
			}
		}
	}

	public DelimitedListTraceListener(Stream stream)
		: base(stream)
	{
	}

	public DelimitedListTraceListener(Stream stream, string name)
		: base(stream, name)
	{
	}

	public DelimitedListTraceListener(TextWriter writer)
		: base(writer)
	{
	}

	public DelimitedListTraceListener(TextWriter writer, string name)
		: base(writer, name)
	{
	}

	public DelimitedListTraceListener(string fileName)
		: base(fileName)
	{
	}

	public DelimitedListTraceListener(string fileName, string name)
		: base(fileName, name)
	{
	}

	protected internal override string[] GetSupportedAttributes()
	{
		return new string[1] { "delimiter" };
	}

	public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
	{
		if (base.Filter == null || base.Filter.ShouldTrace(eventCache, source, eventType, id, format, args))
		{
			WriteHeader(source, eventType, id);
			if (args != null)
			{
				WriteEscaped(string.Format(CultureInfo.InvariantCulture, format, args));
			}
			else
			{
				WriteEscaped(format);
			}
			Write(Delimiter);
			Write(Delimiter);
			WriteFooter(eventCache);
		}
	}

	public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
	{
		if (base.Filter == null || base.Filter.ShouldTrace(eventCache, source, eventType, id, message))
		{
			WriteHeader(source, eventType, id);
			WriteEscaped(message);
			Write(Delimiter);
			Write(Delimiter);
			WriteFooter(eventCache);
		}
	}

	public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
	{
		if (base.Filter == null || base.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data))
		{
			WriteHeader(source, eventType, id);
			Write(Delimiter);
			WriteEscaped(data.ToString());
			Write(Delimiter);
			WriteFooter(eventCache);
		}
	}

	public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
	{
		if (base.Filter != null && !base.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
		{
			return;
		}
		WriteHeader(source, eventType, id);
		Write(Delimiter);
		if (data != null)
		{
			for (int i = 0; i < data.Length; i++)
			{
				if (i != 0)
				{
					Write(secondaryDelim);
				}
				WriteEscaped(data[i].ToString());
			}
		}
		Write(Delimiter);
		WriteFooter(eventCache);
	}

	private void WriteHeader(string source, TraceEventType eventType, int id)
	{
		WriteEscaped(source);
		Write(Delimiter);
		Write(eventType.ToString());
		Write(Delimiter);
		Write(id.ToString(CultureInfo.InvariantCulture));
		Write(Delimiter);
	}

	private void WriteFooter(TraceEventCache eventCache)
	{
		if (eventCache != null)
		{
			if (IsEnabled(TraceOptions.ProcessId))
			{
				Write(eventCache.ProcessId.ToString(CultureInfo.InvariantCulture));
			}
			Write(Delimiter);
			if (IsEnabled(TraceOptions.LogicalOperationStack))
			{
				WriteStackEscaped(eventCache.LogicalOperationStack);
			}
			Write(Delimiter);
			if (IsEnabled(TraceOptions.ThreadId))
			{
				WriteEscaped(eventCache.ThreadId.ToString(CultureInfo.InvariantCulture));
			}
			Write(Delimiter);
			if (IsEnabled(TraceOptions.DateTime))
			{
				WriteEscaped(eventCache.DateTime.ToString("o", CultureInfo.InvariantCulture));
			}
			Write(Delimiter);
			if (IsEnabled(TraceOptions.Timestamp))
			{
				Write(eventCache.Timestamp.ToString(CultureInfo.InvariantCulture));
			}
			Write(Delimiter);
			if (IsEnabled(TraceOptions.Callstack))
			{
				WriteEscaped(eventCache.Callstack);
			}
		}
		else
		{
			for (int i = 0; i < 5; i++)
			{
				Write(Delimiter);
			}
		}
		WriteLine("");
	}

	private void WriteEscaped(string message)
	{
		if (!string.IsNullOrEmpty(message))
		{
			StringBuilder stringBuilder = new StringBuilder("\"");
			int num = 0;
			int num2;
			while ((num2 = message.IndexOf('"', num)) != -1)
			{
				stringBuilder.Append(message, num, num2 - num);
				stringBuilder.Append("\"\"");
				num = num2 + 1;
			}
			stringBuilder.Append(message, num, message.Length - num);
			stringBuilder.Append("\"");
			Write(stringBuilder.ToString());
		}
	}

	private void WriteStackEscaped(Stack stack)
	{
		StringBuilder stringBuilder = new StringBuilder("\"");
		bool flag = true;
		foreach (object item in stack)
		{
			if (!flag)
			{
				stringBuilder.Append(", ");
			}
			else
			{
				flag = false;
			}
			string text = item.ToString();
			int num = 0;
			int num2;
			while ((num2 = text.IndexOf('"', num)) != -1)
			{
				stringBuilder.Append(text, num, num2 - num);
				stringBuilder.Append("\"\"");
				num = num2 + 1;
			}
			stringBuilder.Append(text, num, text.Length - num);
		}
		stringBuilder.Append("\"");
		Write(stringBuilder.ToString());
	}
}
