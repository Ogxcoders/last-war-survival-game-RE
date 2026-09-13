using System.Collections.Generic;

namespace FibMatrix.Rendering;

public class QualitySettingSwitcherCenter
{
	public static Dictionary<QualitySettingSwitcher, object> QualitySettingSwitcheres { get; } = new Dictionary<QualitySettingSwitcher, object>();

	public static void Switch(EnQualityLevel level)
	{
		foreach (KeyValuePair<QualitySettingSwitcher, object> qualitySettingSwitchere in QualitySettingSwitcheres)
		{
			qualitySettingSwitchere.Key.SwitchInternal(level);
		}
	}
}
