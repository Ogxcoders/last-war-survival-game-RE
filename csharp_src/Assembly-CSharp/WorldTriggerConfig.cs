using XLua;

public class WorldTriggerConfig
{
	public int id;

	public int type;

	public string prefab;

	public string explode;

	public string name;

	public int level;

	public string icon;

	public int size;

	public int plot;

	public WorldTriggerConfig(int id, LuaTable table)
	{
		this.id = id;
		type = table.Get<int>("type");
		prefab = table.Get<string>("prefab");
		explode = table.Get<string>("explode");
		name = table.Get<string>("name");
		level = table.Get<int>("level");
		icon = table.Get<string>("icon");
		size = table.Get<int>("size");
		plot = table.Get<int>("plot");
	}
}
