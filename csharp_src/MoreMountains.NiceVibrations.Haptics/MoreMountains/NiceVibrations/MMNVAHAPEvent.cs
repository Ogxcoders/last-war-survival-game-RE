using System;
using System.Collections.Generic;

namespace MoreMountains.NiceVibrations;

[Serializable]
public class MMNVAHAPEvent
{
	public int Time;

	public string EventType;

	public double EventDuration;

	public List<MMNVAHAPEventParameter> EventParameters;
}
