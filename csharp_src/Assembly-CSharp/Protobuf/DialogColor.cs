using Google.Protobuf.Reflection;

namespace Protobuf;

public enum DialogColor
{
	[OriginalName("DEFAULT_COLOR")]
	DefaultColor,
	[OriginalName("BLACK")]
	Black,
	[OriginalName("WHITE")]
	White,
	[OriginalName("RED")]
	Red,
	[OriginalName("GREEN")]
	Green,
	[OriginalName("YELLOW")]
	Yellow,
	[OriginalName("BLUE")]
	Blue
}
