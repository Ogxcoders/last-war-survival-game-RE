using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DragonLandItem
{
	public bool block;

	public string assetPath;

	public Vector3 position;

	public Vector3 localPosition;

	public Vector3 localScale;

	public Quaternion localRotation;

	public List<int> rangeList;

	public Rect rect = Rect.zero;

	public GameObject go;

	public void Save(BinaryWriter writer)
	{
		writer.Write(assetPath);
		writer.Write(block);
		writer.Write(position.x);
		writer.Write(position.z);
		writer.Write(localPosition.x);
		writer.Write(localPosition.z);
		writer.Write(localScale.x);
		writer.Write(localScale.y);
		writer.Write(localScale.z);
		writer.Write(localRotation.x);
		writer.Write(localRotation.y);
		writer.Write(localRotation.z);
		writer.Write(localRotation.w);
		if (rangeList == null || block)
		{
			writer.Write(0);
		}
		else
		{
			writer.Write(rangeList.Count);
			foreach (int range in rangeList)
			{
				writer.Write(range);
			}
		}
		writer.Write(rect.x);
		writer.Write(rect.y);
		writer.Write(rect.width);
		writer.Write(rect.height);
	}

	public void Load(BinaryReader reader)
	{
		assetPath = reader.ReadString();
		block = reader.ReadBoolean();
		position = new Vector3(reader.ReadSingle(), 0f, reader.ReadSingle());
		localPosition = new Vector3(reader.ReadSingle(), 0f, reader.ReadSingle());
		localScale = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		localRotation = new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		int num = reader.ReadInt32();
		rangeList = new List<int>(num);
		for (int i = 0; i < num; i++)
		{
			rangeList.Add(reader.ReadInt32());
		}
		float x = reader.ReadSingle();
		float y = reader.ReadSingle();
		float width = reader.ReadSingle();
		float height = reader.ReadSingle();
		rect = new Rect(x, y, width, height);
	}
}
