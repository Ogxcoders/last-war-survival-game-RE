using XLua;

public class MultiKillConfig
{
	public int id;

	public string prefab;

	public string explode;

	public string name;

	public int level;

	public string icon;

	public int size;

	public MultiKillConfig(int id, LuaTable table)
	{
		this.id = id;
		prefab = table.Get<string>("prefab");
		explode = table.Get<string>("explode");
		name = table.Get<string>("name");
		level = table.Get<int>("level");
		icon = table.Get<string>("icon");
		size = table.Get<int>("size");
	}
}
