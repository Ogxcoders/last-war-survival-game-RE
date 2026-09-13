using System.Collections.Generic;
using System.IO;

public class DragonLandRangeDesc
{
	public static void Save(string savePath, List<DragonLandItem> dataList)
	{
		using BinaryWriter binaryWriter = new BinaryWriter(File.Create(savePath));
		int count = dataList.Count;
		binaryWriter.Write(count);
		for (int i = 0; i < count; i++)
		{
			dataList[i].Save(binaryWriter);
		}
	}

	public static List<DragonLandItem> Load(byte[] data)
	{
		if (data == null || data.Length == 0)
		{
			return null;
		}
		List<DragonLandItem> list = new List<DragonLandItem>();
		using BinaryReader binaryReader = new BinaryReader(new MemoryStream(data));
		int num = binaryReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			DragonLandItem dragonLandItem = new DragonLandItem();
			dragonLandItem.Load(binaryReader);
			list.Add(dragonLandItem);
		}
		return list;
	}
}
