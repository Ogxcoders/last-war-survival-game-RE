using System;

namespace FibMatrix.PerfTools;

[Serializable]
public class CDOverdrawChart
{
	public string cameraName = "";

	public CDChart chart;

	public int veryHighFrameCountThresholdOverdraw = 30;
}
