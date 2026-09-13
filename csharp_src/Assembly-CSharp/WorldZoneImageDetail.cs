using System.Collections.Generic;
using System.IO;

public class WorldZoneImageDetail
{
	public uint version;

	public List<string> imageList = new List<string>();

	public Dictionary<int, int> zoneMap = new Dictionary<int, int>();

	public void Load(BinaryReader br)
	{
		version = br.ReadUInt16();
		if (version != 1)
		{
			return;
		}
		uint num = br.ReadUInt16();
		for (uint num2 = 0u; num2 < num; num2++)
		{
			imageList.Add(br.ReadString());
		}
		uint num3 = br.ReadUInt16();
		for (uint num4 = 0u; num4 < num3; num4++)
		{
			uint value = br.ReadUInt16();
			uint num5 = br.ReadUInt16();
			for (uint num6 = 0u; num6 < num5; num6++)
			{
				zoneMap.Add(br.ReadUInt16(), (int)value);
			}
		}
	}

	public string GetZoneImageName(int zoneId)
	{
		if (zoneMap.TryGetValue(zoneId, out var value) && value < imageList.Count)
		{
			return imageList[value];
		}
		return null;
	}
}
