using System;

namespace ThinkingSDK.PC.Main;

[Flags]
public enum AUTO_TRACK_EVENTS
{
	NONE = 0,
	APP_START = 1,
	APP_END = 2,
	APP_CRASH = 0x10,
	APP_INSTALL = 0x20,
	ALL = 0x33
}
