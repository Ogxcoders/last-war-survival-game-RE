using System.IO;

namespace System.Resources;

internal abstract class Win32Resource
{
	private NameOrId type;

	private NameOrId name;

	private int language;

	public Win32ResourceType ResourceType
	{
		get
		{
			if (type.IsName)
			{
				return (Win32ResourceType)(-1);
			}
			return (Win32ResourceType)type.Id;
		}
	}

	public NameOrId Name => name;

	public NameOrId Type => type;

	public int Language => language;

	internal Win32Resource(NameOrId type, NameOrId name, int language)
	{
		this.type = type;
		this.name = name;
		this.language = language;
	}

	internal Win32Resource(Win32ResourceType type, int name, int language)
	{
		this.type = new NameOrId((int)type);
		this.name = new NameOrId(name);
		this.language = language;
	}

	public abstract void WriteTo(Stream s);

	public override string ToString()
	{
		return "Win32Resource (Kind=" + ResourceType.ToString() + ", Name=" + name?.ToString() + ")";
	}
}
