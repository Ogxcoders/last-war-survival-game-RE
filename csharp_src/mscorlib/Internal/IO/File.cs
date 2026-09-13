namespace Internal.IO;

internal static class File
{
	internal static bool InternalExists(string fullPath)
	{
		if (Interop.Sys.Stat(fullPath, out var output) < 0 && Interop.Sys.LStat(fullPath, out output) < 0)
		{
			return false;
		}
		return (output.Mode & 0xF000) != 16384;
	}
}
