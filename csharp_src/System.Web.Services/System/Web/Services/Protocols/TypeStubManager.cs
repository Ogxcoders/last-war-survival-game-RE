using System.Collections;

namespace System.Web.Services.Protocols;

internal class TypeStubManager
{
	private static Hashtable type_to_manager;

	static TypeStubManager()
	{
		type_to_manager = new Hashtable();
	}

	internal static TypeStubInfo GetTypeStub(Type t, string protocolName)
	{
		return GetLogicalTypeInfo(t).GetTypeStub(protocolName);
	}

	internal static LogicalTypeInfo GetLogicalTypeInfo(Type t)
	{
		lock (type_to_manager)
		{
			LogicalTypeInfo logicalTypeInfo = (LogicalTypeInfo)type_to_manager[t];
			if (logicalTypeInfo != null)
			{
				return logicalTypeInfo;
			}
			logicalTypeInfo = new LogicalTypeInfo(t);
			type_to_manager[t] = logicalTypeInfo;
			return logicalTypeInfo;
		}
	}
}
