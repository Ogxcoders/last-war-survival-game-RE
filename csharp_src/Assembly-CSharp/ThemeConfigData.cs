public static class ThemeConfigData
{
	public struct TextPreset
	{
		public string Name;

		public string Color;

		public string Material;
	}

	public static readonly TextPreset[] Config = new TextPreset[13]
	{
		new TextPreset
		{
			Name = "全屏界面标题",
			Color = "e6e6e6FF",
			Material = "Assets/Main/TMPFont/Main/TitleFontMat/Title-Outline_080808.mat"
		},
		new TextPreset
		{
			Name = "全屏界面分页-选中",
			Color = "e6e6e6FF",
			Material = ""
		},
		new TextPreset
		{
			Name = "全屏界面分页-非选中",
			Color = "e6e6e6FF",
			Material = ""
		},
		new TextPreset
		{
			Name = "聊天-玩家等级",
			Color = "e6e6e6FF",
			Material = ""
		},
		new TextPreset
		{
			Name = "聊天-玩家名字",
			Color = "8c8c8cFF",
			Material = ""
		},
		new TextPreset
		{
			Name = "聊天信息",
			Color = "e6e6e6FF",
			Material = ""
		},
		new TextPreset
		{
			Name = "二级文本",
			Color = "a5a5a5FF",
			Material = ""
		},
		new TextPreset
		{
			Name = "弹窗标题",
			Color = "e6e6e6FF",
			Material = "Assets/Main/TMPFont/Main/ChapterFontMat/Chapter-Outline_080808_60.mat"
		},
		new TextPreset
		{
			Name = "按钮文字",
			Color = "e6e6e6FF",
			Material = "Assets/Main/TMPFont/Main/TitleFontMat/Title-Outline_080808.mat"
		},
		new TextPreset
		{
			Name = "全屏界面分页-非选中蓝色",
			Color = "697dabFF",
			Material = ""
		},
		new TextPreset
		{
			Name = "回复文本",
			Color = "646464FF",
			Material = ""
		},
		new TextPreset
		{
			Name = "输入回复文本",
			Color = "646464FF",
			Material = ""
		},
		new TextPreset
		{
			Name = "公告标题",
			Color = "f5f5f5FF",
			Material = ""
		}
	};
}
