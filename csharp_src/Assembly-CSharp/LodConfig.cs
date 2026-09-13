using XLua;

public class LodConfig
{
	public string path;

	public int lodStart;

	public int lodEnd;

	public bool isMain;

	public bool noFading;

	public bool standaloneScaleSpeed;

	public int minLod;

	public int maxLod;

	public float maxScale;

	public void InitByTemplate(LuaTable template)
	{
		path = template.Get<string>("path");
		lodStart = template.Get<int>("lodStart");
		lodEnd = template.Get<int>("lodEnd");
		isMain = template.Get<bool>("isMain");
		noFading = template.Get<bool>("noFading");
		standaloneScaleSpeed = template.Get<bool>("standaloneScaleSpeed");
		if (standaloneScaleSpeed)
		{
			minLod = template.Get<int>("minLod");
			maxLod = template.Get<int>("maxLod");
			maxScale = template.Get<float>("maxScale");
			if (maxLod > minLod && minLod >= 0)
			{
				standaloneScaleSpeed = true;
			}
			else
			{
				standaloneScaleSpeed = false;
			}
		}
	}
}
