using System;

namespace UnityEngine.Profiling.Memory.Experimental;

public class MetaData
{
	[NonSerialized]
	public string content;

	[NonSerialized]
	public string platform;

	[Obsolete("Starting with version 9 of the snapshot format screenshots are no longer part of the snapshot, but separate files. Use the appropriate TakeSnapshot overload to also capture screenshots.", false)]
	public Texture2D screenshot { get; internal set; }
}
