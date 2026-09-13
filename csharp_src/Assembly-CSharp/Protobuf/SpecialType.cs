using Google.Protobuf.Reflection;

namespace Protobuf;

public enum SpecialType
{
	[OriginalName("NONE")]
	None = 0,
	[OriginalName("DETECT_EVENT")]
	DetectEvent = 1,
	[OriginalName("CROSS_THRONE")]
	CrossThrone = 3,
	[OriginalName("CITY_GHOST")]
	CityGhost = 4,
	[OriginalName("S5_CENTER_THRONE")]
	S5CenterThrone = 5
}
