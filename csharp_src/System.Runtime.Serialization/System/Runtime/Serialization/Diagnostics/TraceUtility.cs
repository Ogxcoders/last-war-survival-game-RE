using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Diagnostics;

namespace System.Runtime.Serialization.Diagnostics;

internal static class TraceUtility
{
	private static Dictionary<int, string> traceCodes = new Dictionary<int, string>(18)
	{
		{ 196609, "WriteObjectBegin" },
		{ 196610, "WriteObjectEnd" },
		{ 196611, "WriteObjectContentBegin" },
		{ 196612, "WriteObjectContentEnd" },
		{ 196613, "ReadObjectBegin" },
		{ 196614, "ReadObjectEnd" },
		{ 196615, "ElementIgnored" },
		{ 196616, "XsdExportBegin" },
		{ 196617, "XsdExportEnd" },
		{ 196618, "XsdImportBegin" },
		{ 196619, "XsdImportEnd" },
		{ 196620, "XsdExportError" },
		{ 196621, "XsdImportError" },
		{ 196622, "XsdExportAnnotationFailed" },
		{ 196623, "XsdImportAnnotationFailed" },
		{ 196624, "XsdExportDupItems" },
		{ 196625, "FactoryTypeNotFound" },
		{ 196626, "ObjectWithLargeDepth" }
	};

	internal static void Trace(TraceEventType severity, int traceCode, string traceDescription)
	{
		Trace(severity, traceCode, traceDescription, null);
	}

	internal static void Trace(TraceEventType severity, int traceCode, string traceDescription, TraceRecord record)
	{
		Trace(severity, traceCode, traceDescription, record, null);
	}

	internal static void Trace(TraceEventType severity, int traceCode, string traceDescription, TraceRecord record, Exception exception)
	{
		string text = "";
		DiagnosticUtility.DiagnosticTrace.TraceEvent(severity, traceCode, text, traceDescription, record, exception, null);
	}
}
