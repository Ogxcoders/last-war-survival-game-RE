namespace Joker;

public static class UtilPath
{
	public static string GetRegularPath(string path)
	{
		return path?.Replace('\\', '/');
	}
}
