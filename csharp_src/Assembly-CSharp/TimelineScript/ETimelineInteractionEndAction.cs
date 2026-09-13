using System;

namespace TimelineScript;

[Flags]
public enum ETimelineInteractionEndAction
{
	Resume = 1,
	JumpToTime = 2
}
