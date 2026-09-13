namespace VEngine;

public class ManifestVersionFile
{
	public uint crc;

	public int version;

	public static ManifestVersionFile Load(string path)
	{
		ManifestVersionFile manifestVersionFile = new ManifestVersionFile();
		if (SyncReader.ExistFile(path))
		{
			string[] array = SyncReader.ReadAllTextByFilePath(path).Split(new char[1] { ',' });
			if (array.Length > 2)
			{
				manifestVersionFile.version = array[0].IntValue();
				manifestVersionFile.crc = array[2].UIntValue();
			}
		}
		return manifestVersionFile;
	}
}
