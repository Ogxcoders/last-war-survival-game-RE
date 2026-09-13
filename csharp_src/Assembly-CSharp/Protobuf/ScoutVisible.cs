using Google.Protobuf.Reflection;

namespace Protobuf;

public enum ScoutVisible
{
	[OriginalName("DEFAULT")]
	Default,
	[OriginalName("ENABLE")]
	Enable,
	[OriginalName("DISABLE")]
	Disable,
	[OriginalName("NOT_MATCH")]
	NotMatch,
	[OriginalName("ENABLE_EMPTY")]
	EnableEmpty
}
