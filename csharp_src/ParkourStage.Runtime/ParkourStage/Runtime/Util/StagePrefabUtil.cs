namespace ParkourStage.Runtime.Util;

public static class StagePrefabUtil
{
	private static IStagePrefabGenerator ms_StagePrefabGenerator;

	public static void RegisterPrefabGenerator(IStagePrefabGenerator prefabGenerator)
	{
		ms_StagePrefabGenerator = prefabGenerator;
	}

	public static IStagePrefabGenerator GetStagePrefabGenerator()
	{
		return ms_StagePrefabGenerator;
	}
}
