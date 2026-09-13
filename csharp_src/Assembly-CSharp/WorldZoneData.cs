using System.Collections.Generic;
using System.IO;

public class WorldZoneData
{
	public int color = -1;

	public string AllianceId = string.Empty;

	public int ServerId;

	public List<int> edgeIdList;

	public int Rate;

	public short ZoneId { get; private set; }

	public int Level { get; private set; }

	public int PosType { get; private set; }

	public int CityPos { get; private set; }

	public int CityState { get; private set; }

	public short X { get; private set; }

	public short Y { get; private set; }

	public short W { get; private set; }

	public short H { get; private set; }

	public short Cx { get; private set; }

	public short Cy { get; private set; }

	public void Load(BinaryReader br)
	{
		ZoneId = br.ReadInt16();
		Level = br.ReadInt32();
		PosType = br.ReadInt32();
		CityPos = br.ReadInt32() + 1;
		CityState = br.ReadInt32();
		int num = br.ReadInt32();
		if (num > 0)
		{
			br.BaseStream.Seek(num * 2, SeekOrigin.Current);
		}
		X = br.ReadInt16();
		Y = br.ReadInt16();
		W = br.ReadInt16();
		H = br.ReadInt16();
		Cx = br.ReadInt16();
		Cy = br.ReadInt16();
		num = br.ReadInt32();
		if (num > 0)
		{
			edgeIdList = new List<int>(num);
			for (int i = 0; i < num; i++)
			{
				edgeIdList.Add(br.ReadInt16());
			}
		}
		else
		{
			edgeIdList = new List<int>();
		}
		num = br.ReadInt32();
		if (num > 0)
		{
			br.BaseStream.Seek(num * 2, SeekOrigin.Current);
		}
	}
}
