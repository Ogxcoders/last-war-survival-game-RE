using System;
using System.Collections.Generic;

namespace BestHTTP.Caching;

public sealed class UriComparer : IEqualityComparer<Uri>
{
	public bool Equals(Uri x, Uri y)
	{
		return Uri.Compare(x, y, UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped, StringComparison.Ordinal) == 0;
	}

	public int GetHashCode(Uri uri)
	{
		return uri.ToString().GetHashCode();
	}
}
