using System.Collections.Generic;

public class RenderSettingManager
{
	private static Stack<RenderSettingData> settingStack = new Stack<RenderSettingData>();

	public static void PushSetting(RenderSettingData settingData)
	{
		settingStack.Push(settingData);
		settingData.ApplyRenderSetting();
	}

	public static void PopSetting()
	{
		settingStack.Pop();
		if (settingStack.Count > 0)
		{
			settingStack.Peek().ApplyRenderSetting();
		}
	}
}
