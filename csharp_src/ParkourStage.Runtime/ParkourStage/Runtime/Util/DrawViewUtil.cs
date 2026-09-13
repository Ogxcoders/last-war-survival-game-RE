namespace ParkourStage.Runtime.Util;

public static class DrawViewUtil
{
	private static IViewDrawer ms_ViewDrawer;

	public static void RegisterDrawView(IViewDrawer drawViewer)
	{
		ms_ViewDrawer = drawViewer;
	}

	public static IViewDrawer GetViewDrawer()
	{
		return ms_ViewDrawer;
	}
}
