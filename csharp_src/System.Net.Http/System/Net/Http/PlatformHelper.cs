using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;

namespace System.Net.Http;

internal static class PlatformHelper
{
	internal static bool IsContentHeader(string name)
	{
		if (HeaderDescriptor.TryGet(name, out var descriptor))
		{
			return descriptor.HeaderType == HttpHeaderType.Content;
		}
		return false;
	}

	internal static string GetSingleHeaderString(string name, IEnumerable<string> values)
	{
		string separator = ", ";
		if (HeaderDescriptor.TryGet(name, out var descriptor) && descriptor.Parser != null && descriptor.Parser.SupportsMultipleValues)
		{
			separator = descriptor.Parser.Separator;
		}
		return string.Join(separator, values);
	}

	internal static StreamContent CreateStreamContent(Stream stream, CancellationToken cancellationToken)
	{
		return new StreamContent(stream);
	}
}
