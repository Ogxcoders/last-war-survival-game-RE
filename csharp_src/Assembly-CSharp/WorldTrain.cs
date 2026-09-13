using System.Collections.Generic;
using Sfs2X.Entities.Data;

public class WorldTrain
{
	public static float Carriage_Length = 1.85f;

	public long uuid;

	public int cfgId;

	public float length;

	public TrainType type;

	public WorldTrainConfig config;

	public ISFSObject trainData;

	public List<int> stationList;

	public WorldTrain(ISFSObject q)
	{
		uuid = q.TryGetLong("uuid");
		cfgId = q.TryGetInt("cfgId");
		type = (TrainType)q.TryGetInt("type");
		config = SceneManager.MarchDataMgr.GetTrainConfig(cfgId);
		length = Carriage_Length * (float)config.carriageNum;
		trainData = q;
	}
}
