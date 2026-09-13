using System.Diagnostics;

namespace System.Runtime.Diagnostics;

internal class DiagnosticTraceSource : TraceSource
{
	private const string PropagateActivityValue = "propagateActivity";

	internal bool PropagateActivity
	{
		get
		{
			bool result = false;
			string value = base.Attributes["propagateActivity"];
			if (!string.IsNullOrEmpty(value) && !bool.TryParse(value, out result))
			{
				result = false;
			}
			return result;
		}
		set
		{
			base.Attributes["propagateActivity"] = value.ToString();
		}
	}

	internal DiagnosticTraceSource(string name)
		: base(name)
	{
	}

	protected override string[] GetSupportedAttributes()
	{
		return new string[1] { "propagateActivity" };
	}
}
