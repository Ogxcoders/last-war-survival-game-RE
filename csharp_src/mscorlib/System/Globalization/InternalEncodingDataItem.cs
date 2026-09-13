using System.Security;

namespace System.Globalization;

internal struct InternalEncodingDataItem
{
	[SecurityCritical]
	internal string webName;

	internal ushort codePage;
}
