namespace System.Resources;

internal class NameOrId
{
	private string name;

	private int id;

	public bool IsName => name != null;

	public string Name => name;

	public int Id => id;

	public NameOrId(string name)
	{
		this.name = name;
	}

	public NameOrId(int id)
	{
		this.id = id;
	}

	public override string ToString()
	{
		if (name != null)
		{
			return "Name(" + name + ")";
		}
		return "Id(" + id + ")";
	}
}
