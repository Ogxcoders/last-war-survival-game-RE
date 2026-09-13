using System.Collections.Generic;

namespace UnityEngine.Rendering;

internal class DebugActionDesc
{
	public List<string[]> buttonTriggerList = new List<string[]>();

	public string axisTrigger = "";

	public List<KeyCode[]> keyTriggerList = new List<KeyCode[]>();

	public DebugActionRepeatMode repeatMode;

	public float repeatDelay;
}
