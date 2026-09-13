using System.Collections.Generic;
using System.IO;
using System.Xml;
using Unity.Mathematics;
using UnityEngine;

public class WorldSceneDesc
{
	public class ObjectDesc
	{
		public int id;

		public int type;

		public Vector3 localPos;

		public Vector3 rotation;

		public Vector3 scale;

		public string assetPath;

		public int assetGuid;

		public bool isStatic;

		public float distance;

		public bool useMapOuter;

		public Vector2Int worldDecorationTilePos;

		public int4 worldDecorationTileRect;

		public bool hide;

		public bool playAnim;

		public Bounds bounds;

		public void Load(BinaryReader reader)
		{
			id = reader.ReadInt16();
			type = reader.ReadInt16();
			localPos = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			rotation = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			scale = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			assetPath = reader.ReadString();
		}

		public void LoadWorldOptimize(BinaryReader reader, int extends)
		{
			id = reader.ReadInt16();
			type = 0;
			localPos = new Vector3(reader.ReadSingle(), 0f, reader.ReadSingle());
			rotation = new Vector3(0f, reader.ReadSingle(), 0f);
			scale = Vector3.one * reader.ReadSingle();
			assetGuid = reader.ReadInt16();
			int num = reader.ReadInt16();
			int num2 = reader.ReadInt16();
			worldDecorationTilePos = new Vector2Int(num, num2);
			worldDecorationTileRect = new int4(num - extends, num2 - extends, num + extends, num2 + extends);
		}

		public void LoadWorldOptimizeWithMapOuter(BinaryReader reader, int extends)
		{
			id = reader.ReadInt16();
			type = reader.ReadInt16();
			localPos = new Vector3(reader.ReadSingle(), 0f, reader.ReadSingle());
			rotation = new Vector3(0f, reader.ReadSingle(), 0f);
			scale = Vector3.one * reader.ReadSingle();
			assetGuid = reader.ReadInt16();
			int num = reader.ReadInt16();
			int num2 = reader.ReadInt16();
			worldDecorationTilePos = new Vector2Int(num, num2);
			worldDecorationTileRect = new int4(num - extends, num2 - extends, num + extends, num2 + extends);
		}

		public void LoadWorldOptimizeS5(BinaryReader reader, int extends)
		{
			id = reader.ReadInt32();
			type = reader.ReadInt16();
			localPos = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			rotation = new Vector3(0f, 0f, 0f);
			scale = Vector3.one * reader.ReadSingle();
			assetGuid = reader.ReadInt32();
			int num = reader.ReadInt16();
			int num2 = reader.ReadInt16();
			worldDecorationTilePos = new Vector2Int(num, num2);
			worldDecorationTileRect = new int4(num - extends, num2 - extends, num + extends, num2 + extends);
		}

		public void LoadPVEDecorationOptimize(BinaryReader reader)
		{
			id = reader.ReadInt16();
			type = 0;
			localPos = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			rotation = new Vector3(0f, reader.ReadSingle(), 0f);
			scale = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			assetGuid = reader.ReadInt16();
		}

		public void Save(BinaryWriter writer)
		{
			writer.Write((short)id);
			writer.Write((short)type);
			writer.Write(localPos.x);
			writer.Write(localPos.y);
			writer.Write(localPos.z);
			writer.Write(rotation.x);
			writer.Write(rotation.y);
			writer.Write(rotation.z);
			writer.Write(scale.x);
			writer.Write(scale.y);
			writer.Write(scale.z);
			writer.Write(assetPath);
		}

		public void SaveWorldOptimize(BinaryWriter writer)
		{
			writer.Write((short)id);
			writer.Write(localPos.x);
			writer.Write(localPos.z);
			writer.Write(rotation.y);
			writer.Write(scale.x);
			writer.Write((short)assetGuid);
			writer.Write((short)worldDecorationTilePos.x);
			writer.Write((short)worldDecorationTilePos.y);
		}

		public void SaveWorldOptimizeS5(BinaryWriter writer)
		{
			writer.Write(id);
			writer.Write((short)type);
			writer.Write(localPos.x);
			writer.Write(localPos.y);
			writer.Write(localPos.z);
			writer.Write(scale.x);
			writer.Write(assetGuid);
			writer.Write((short)worldDecorationTilePos.x);
			writer.Write((short)worldDecorationTilePos.y);
		}

		public void SaveWorldOptimizeWithMapOuter(BinaryWriter writer)
		{
			writer.Write((short)id);
			writer.Write((short)type);
			writer.Write(localPos.x);
			writer.Write(localPos.z);
			writer.Write(rotation.y);
			writer.Write(scale.x);
			writer.Write((short)assetGuid);
			writer.Write((short)worldDecorationTilePos.x);
			writer.Write((short)worldDecorationTilePos.y);
		}

		public void SavePVEDecorationOptimize(BinaryWriter writer)
		{
			writer.Write((short)id);
			writer.Write(localPos.x);
			writer.Write(localPos.y);
			writer.Write(localPos.z);
			writer.Write(rotation.y);
			writer.Write(scale.x);
			writer.Write(scale.y);
			writer.Write(scale.z);
			writer.Write((short)assetGuid);
		}

		public void SaveXml(XmlDocument doc, XmlNode parent)
		{
			XmlElement xmlElement = doc.CreateElement("Object");
			parent.AppendChild(xmlElement);
			xmlElement.SetAttribute("id", id.ToString());
			xmlElement.SetAttribute("type", type.ToString());
			xmlElement.SetAttribute("T", $"{localPos.x} {localPos.y} {localPos.z}");
			xmlElement.SetAttribute("R", $"{rotation.x} {rotation.y} {rotation.z}");
			xmlElement.SetAttribute("S", $"{scale.x} {scale.y} {scale.z}");
			xmlElement.SetAttribute("path", assetPath);
			xmlElement.SetAttribute("assetGuid", assetGuid.ToString());
			if (assetGuid > 0)
			{
				xmlElement.SetAttribute("TilePos", $"{worldDecorationTilePos.x} {worldDecorationTilePos.y}");
			}
		}

		public void Clear()
		{
			id = 0;
			type = 0;
			localPos = Vector3.zero;
			rotation = Vector3.zero;
			scale = Vector3.zero;
			assetPath = string.Empty;
			assetGuid = 0;
			isStatic = false;
			distance = 0f;
			useMapOuter = false;
			worldDecorationTilePos = Vector2Int.zero;
			worldDecorationTileRect = int4.zero;
			hide = false;
			playAnim = false;
			bounds = default(Bounds);
		}
	}

	public class GridDesc
	{
		public int width;

		public int height;

		public byte[] grids;

		public void Load(BinaryReader reader)
		{
			width = reader.ReadInt32();
			height = reader.ReadInt32();
			int num = reader.ReadInt32();
			if (num > 0)
			{
				byte[] array = reader.ReadBytes(num);
				grids = new byte[width * height];
				for (int i = 0; i < grids.Length; i++)
				{
					int num2 = i / 8;
					int num3 = i % 8;
					grids[i] = (byte)(((array[num2] & (1 << num3)) != 0) ? 1u : 0u);
				}
			}
		}

		public void Save(BinaryWriter writer)
		{
			writer.Write(width);
			writer.Write(height);
			if (grids != null && grids.Length != 0)
			{
				int num = (grids.Length + 7) / 8;
				writer.Write(num);
				byte[] array = new byte[num];
				for (int i = 0; i < grids.Length; i++)
				{
					if (grids[i] != 0)
					{
						int num2 = i / 8;
						int num3 = i % 8;
						byte b = (byte)(1 << num3);
						array[num2] |= b;
					}
				}
				writer.Write(array);
			}
			else
			{
				writer.Write(0);
			}
		}

		public void SaveXml(XmlDocument doc, XmlNode parent, int serverIndex = -1)
		{
			XmlElement xmlElement = doc.CreateElement("MapGridList");
			if (serverIndex != -1)
			{
				xmlElement.SetAttribute("width", (width / 3).ToString());
				xmlElement.SetAttribute("height", (height / 3).ToString());
			}
			else
			{
				xmlElement.SetAttribute("width", width.ToString());
				xmlElement.SetAttribute("height", height.ToString());
			}
			parent.AppendChild(xmlElement);
			if (grids == null)
			{
				return;
			}
			for (int i = 0; i < grids.Length; i++)
			{
				if (grids[i] != 0)
				{
					XmlElement xmlElement2 = doc.CreateElement("G");
					if (serverIndex != -1)
					{
						int x = i % width;
						int y = i / height;
						Vector2Int vector2Int = Tile3000ToTile1000(x, y, serverIndex);
						xmlElement2.SetAttribute("x", vector2Int.x.ToString());
						xmlElement2.SetAttribute("y", vector2Int.y.ToString());
					}
					else
					{
						xmlElement2.SetAttribute("x", (i % width).ToString());
						xmlElement2.SetAttribute("y", (i / width).ToString());
					}
					xmlElement2.SetAttribute("walkable", (grids[i] == 0) ? "1" : "0");
					xmlElement.AppendChild(xmlElement2);
				}
			}
		}

		public Vector2Int Tile3000ToTile1000(int x3000, int y3000, int serverIndex)
		{
			int num = serverIndex / 3;
			int num2 = serverIndex % 3 * 1000;
			int num3 = num * 1000;
			int x3001 = x3000 - num2;
			int y3001 = y3000 - num3;
			return new Vector2Int(x3001, y3001);
		}
	}

	public const string WorldDecorationParentPath = "Assets/Main/Scenes";

	public const string WorldDecorationXmlParentPath = "Assets/_Art_LastWar/XML";

	public const string WorldDescPath = "Assets/Main/Scenes/WorldSceneDesc.bytes";

	public const string NewWorldDescPath = "Assets/Main/Scenes/NewWorldSceneDesc{0}.bytes";

	public const string NewWorldDescPath_Season = "Assets/Main/SeasonRes/{0}/Scenes/NewWorldSceneDesc{1}_{2}.bytes";

	public const string WorldBlockDescPath = "Assets/Main/Scenes/WorldBlockDesc{0}.bytes";

	public const string WorldBlockDescPath_Season = "Assets/Main/SeasonRes/{0}/Scenes/WorldBlockDesc{0}_{1}.bytes";

	public const string WorldDescXmlPath = "Assets/_Art_LastWar/XML/WorldSceneDesc{0}.xml";

	public const string WorldDescXmlPath_S5 = "Assets/_Art_LastWar/XML/WorldSceneDesc{0}_{1}.xml";

	public const string WorldBlockDescXmlPath = "Assets/_Art_LastWar/XML/WorldBlockDesc{0}.xml";

	public const string WorldBlockDescXmlPath_S5 = "Assets/_Art_LastWar/XML/WorldBlockDesc{0}_{1}.xml";

	public const string WorldAllianceCityDescPath = "Assets/Main/Scenes/WorldSceneAllianceCityDesc{0}.bytes";

	public const string WorldAllianceCityDescPath_S5 = "Assets/Main/Scenes/WorldSceneAllianceCityDesc{0}_{1}.bytes";

	public const string WorldAllianceCityDescXmlPath = "Assets/_Art_LastWar/XML/WorldSceneAllianceCityDesc{0}.xml";

	public const string WorldAllianceCityDescXmlPath_S5 = "Assets/_Art_LastWar/XML/WorldSceneAllianceCityDesc{0}_{1}.xml";

	public const string WorldDecorationPath = "Assets/Main/Scenes/WorldDecoration";

	public const string WorldDecorationPath_Season = "Assets/Main/SeasonRes/{0}/Scenes/WorldDecoration_{1}";

	public const string CityDescPath = "Assets/Main/Scenes/CitySceneDesc.bytes";

	public const string CityDescXmlPath = "Assets/_Art_LastWar/XML/CitySceneDesc.xml";

	private List<ObjectDesc> _objectDescs = new List<ObjectDesc>();

	private GridDesc _gridDesc = new GridDesc();

	public List<ObjectDesc> objectDescs => _objectDescs;

	public GridDesc gridDesc => _gridDesc;

	public void Load(byte[] data, bool worldOptimize = false)
	{
		if (data == null || data.Length == 0)
		{
			return;
		}
		using BinaryReader binaryReader = new BinaryReader(new MemoryStream(data));
		int num = binaryReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			ObjectDesc objectDesc = new ObjectDesc();
			if (worldOptimize)
			{
				objectDesc.LoadWorldOptimize(binaryReader, 1);
			}
			else
			{
				objectDesc.Load(binaryReader);
			}
			_objectDescs.Add(objectDesc);
		}
		_gridDesc.Load(binaryReader);
	}

	public Dictionary<Vector2Int, WorldDecorationChunkDesc> LoadWorldDecoration(byte[] data, int dataType)
	{
		if (data == null || data.Length == 0)
		{
			return new Dictionary<Vector2Int, WorldDecorationChunkDesc>();
		}
		using BinaryReader binaryReader = new BinaryReader(new MemoryStream(data));
		int num = binaryReader.ReadInt32();
		Dictionary<Vector2Int, WorldDecorationChunkDesc> dictionary = new Dictionary<Vector2Int, WorldDecorationChunkDesc>(num);
		int objectDescByteCount = binaryReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			WorldDecorationChunkDesc worldDecorationChunkDesc = new WorldDecorationChunkDesc(dataType);
			worldDecorationChunkDesc.Load(binaryReader, objectDescByteCount);
			dictionary.Add(worldDecorationChunkDesc.chunkCoord, worldDecorationChunkDesc);
		}
		_gridDesc.Load(binaryReader);
		return dictionary;
	}

	public List<ObjectDesc> LoadWithReturn(byte[] data)
	{
		List<ObjectDesc> list = new List<ObjectDesc>();
		if (data == null || data.Length == 0)
		{
			return list;
		}
		using BinaryReader binaryReader = new BinaryReader(new MemoryStream(data));
		int num = binaryReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			ObjectDesc objectDesc = new ObjectDesc();
			objectDesc.Load(binaryReader);
			_objectDescs.Add(objectDesc);
			list.Add(objectDesc);
		}
		_gridDesc.Load(binaryReader);
		return list;
	}

	public void LoadWithPool(byte[] data, Stack<ObjectDesc> stack, List<ObjectDesc> list)
	{
		if (data == null || data.Length == 0)
		{
			return;
		}
		using BinaryReader binaryReader = new BinaryReader(new MemoryStream(data));
		int num = binaryReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			ObjectDesc objectDesc = null;
			objectDesc = ((stack == null || stack.Count <= 0) ? new ObjectDesc() : stack.Pop());
			objectDesc.Load(binaryReader);
			_objectDescs.Add(objectDesc);
			list.Add(objectDesc);
		}
	}

	public void Save(string savePath, bool worldOptimize = false, bool pveDecorationSave = false)
	{
		using BinaryWriter binaryWriter = new BinaryWriter(File.Create(savePath));
		int count = _objectDescs.Count;
		binaryWriter.Write(count);
		for (int i = 0; i < count; i++)
		{
			if (worldOptimize)
			{
				_objectDescs[i].SaveWorldOptimize(binaryWriter);
			}
			else if (pveDecorationSave)
			{
				_objectDescs[i].SavePVEDecorationOptimize(binaryWriter);
			}
			else
			{
				_objectDescs[i].Save(binaryWriter);
			}
		}
		_gridDesc.Save(binaryWriter);
	}

	public void SaveGrid(BinaryWriter writer)
	{
		_gridDesc.Save(writer);
	}

	public void SaveXml(string savePath, int serverIndex = -1)
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "yes");
		xmlDocument.AppendChild(newChild);
		XmlElement xmlElement = xmlDocument.CreateElement("WorldSceneDesc");
		xmlDocument.AppendChild(xmlElement);
		XmlElement xmlElement2 = xmlDocument.CreateElement("ObjectList");
		xmlElement.AppendChild(xmlElement2);
		for (int i = 0; i < _objectDescs.Count; i++)
		{
			if (_objectDescs[i].type != 2)
			{
				_objectDescs[i].SaveXml(xmlDocument, xmlElement2);
			}
		}
		_gridDesc.SaveXml(xmlDocument, xmlElement, serverIndex);
		xmlDocument.Save(savePath);
	}
}
