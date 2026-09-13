using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldDecorationChunkDesc
{
	public const int DefaultDataType = 1;

	public const int MapOuterDataType = 2;

	private List<WorldSceneDesc.ObjectDesc> _objectDescList;

	public Vector2Int regionCoord;

	private int _cacheListCount;

	private byte[] _cacheData;

	public int dataType = 1;

	public Vector2Int chunkCoord { get; private set; }

	public List<WorldSceneDesc.ObjectDesc> objectDescList => _objectDescList;

	public WorldDecorationChunkDesc(Vector2Int coord, int dataType)
	{
		chunkCoord = coord;
		this.dataType = dataType;
		_objectDescList = new List<WorldSceneDesc.ObjectDesc>(64);
	}

	public WorldDecorationChunkDesc(int dataType)
	{
		this.dataType = dataType;
	}

	public void Add(WorldSceneDesc.ObjectDesc objDesc)
	{
		_objectDescList.Add(objDesc);
	}

	public void Save(BinaryWriter writer, int serverIndex)
	{
		Vector2Int vector2Int = chunkCoord;
		writer.Write(vector2Int.x);
		writer.Write(vector2Int.y);
		List<WorldSceneDesc.ObjectDesc> list = _objectDescList;
		writer.Write(list.Count);
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			if (serverIndex != -1)
			{
				list[i].SaveWorldOptimizeS5(writer);
			}
			else if (dataType == 2)
			{
				list[i].SaveWorldOptimizeWithMapOuter(writer);
			}
			else
			{
				list[i].SaveWorldOptimize(writer);
			}
		}
	}

	public void Load(BinaryReader reader, int objectDescByteCount)
	{
		int x = reader.ReadInt32();
		int y = reader.ReadInt32();
		chunkCoord = new Vector2Int(x, y);
		int num = (_cacheListCount = reader.ReadInt32());
		if (_cacheListCount > 0)
		{
			_cacheData = reader.ReadBytes(num * objectDescByteCount);
		}
	}

	public List<WorldSceneDesc.ObjectDesc> GetChunkObjList(int extends, bool isS5)
	{
		if (_cacheData != null)
		{
			_objectDescList = new List<WorldSceneDesc.ObjectDesc>(_cacheListCount);
			using (BinaryReader reader = new BinaryReader(new MemoryStream(_cacheData)))
			{
				for (int i = 0; i < _cacheListCount; i++)
				{
					WorldSceneDesc.ObjectDesc objectDesc = new WorldSceneDesc.ObjectDesc();
					if (isS5)
					{
						objectDesc.LoadWorldOptimizeS5(reader, extends);
					}
					else if (dataType == 2)
					{
						objectDesc.LoadWorldOptimizeWithMapOuter(reader, extends);
					}
					else
					{
						objectDesc.LoadWorldOptimize(reader, extends);
					}
					_objectDescList.Add(objectDesc);
				}
			}
			_cacheData = null;
		}
		return _objectDescList;
	}
}
