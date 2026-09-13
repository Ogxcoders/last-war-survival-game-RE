using System.Collections.Generic;

namespace System;

internal interface IAndroidTimeZoneDB
{
	IEnumerable<string> GetAvailableIds();

	byte[] GetTimeZoneData(string id);
}
