using System;

namespace ThinkingAnalytics;

[Flags]
public enum AUTO_TRACK_EVENTS
{
	NONE = 0,
	APP_START = 1,
	APP_END = 2,
	APP_CRASH = 0x10,
	APP_INSTALL = 0x20,
	APP_SCENE_LOAD = 0x40,
	APP_SCENE_UNLOAD = 0x80,
	ALL = 0xF3
}
