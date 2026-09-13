using System;

namespace Microsoft.Internal;

internal static class StringComparers
{
	public static StringComparer ContractName => StringComparer.Ordinal;

	public static StringComparer MetadataKeyNames => StringComparer.Ordinal;
}
