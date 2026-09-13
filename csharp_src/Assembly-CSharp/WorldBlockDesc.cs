using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldBlockDesc
{
	public List<Vector2Int> Tiles = new List<Vector2Int>();

	public void Save(string savePath)
	{
		using BinaryWriter binaryWriter = new BinaryWriter(File.Create(savePath));
		int count = Tiles.Count;
		binaryWriter.Write(count);
		for (int i = 0; i < count; i++)
		{
			binaryWriter.Write((short)Tiles[i].x);
			binaryWriter.Write((short)Tiles[i].y);
		}
	}

	public void Load(byte[] data)
	{
		if (data == null || data.Length == 0)
		{
			return;
		}
		using BinaryReader binaryReader = new BinaryReader(new MemoryStream(data));
		int num = binaryReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			short x = binaryReader.ReadInt16();
			short y = binaryReader.ReadInt16();
			Tiles.Add(new Vector2Int(x, y));
		}
	}
}
