namespace UnityEngine.UIElements;

public class DropdownMenuSeparator : DropdownMenuItem
{
	public string subMenuPath { get; }

	public DropdownMenuSeparator(string subMenuPath)
	{
		this.subMenuPath = subMenuPath;
	}
}
