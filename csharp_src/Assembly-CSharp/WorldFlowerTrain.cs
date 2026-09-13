using System.Collections.Generic;
using Sfs2X.Entities.Data;

public class WorldFlowerTrain
{
	public long uuid;

	public int preStation;

	public List<int> historyStationList;

	public List<WorldSingleFlowerTrain> flowerTrainDataList;

	public float length;

	private const int FlowerTrainSpaceTileSize = 8;

	private const int FlowerTrainSize = 5;

	public WorldFlowerTrain(ISFSObject q)
	{
		UpdateData(q);
	}

	public void UpdateData(ISFSObject q)
	{
		uuid = q.TryGetLong("uuid");
		preStation = q.TryGetInt("preStation");
		int[] array = q.TryGetIntArray("historyStation");
		historyStationList = new List<int>();
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				historyStationList.Add(array[i]);
			}
		}
		flowerTrainDataList = new List<WorldSingleFlowerTrain>();
		ISFSArray iSFSArray = q.TryGetArray("flowerTrains");
		if (iSFSArray == null)
		{
			return;
		}
		foreach (object item in iSFSArray)
		{
			WorldSingleFlowerTrain worldSingleFlowerTrain = new WorldSingleFlowerTrain(uuid);
			worldSingleFlowerTrain.ParseData(item as ISFSObject);
			flowerTrainDataList.Add(worldSingleFlowerTrain);
		}
		int count = flowerTrainDataList.Count;
		length = count * 5 + (count - 1) * 8;
	}
}
