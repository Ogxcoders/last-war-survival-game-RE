using UnityEngine.Experimental.Rendering;

namespace GameKit.Base;

public class ProfilerService : SingletonBehaviour<ProfilerService>
{
	public SRPBatcherProfiler SRPProfiler { get; private set; }

	public static void Initialize()
	{
		SingletonBehaviour<ProfilerService>.Instance.gameObject.name = "Profiler";
		SingletonBehaviour<ProfilerService>.Instance.SRPProfiler = SingletonBehaviour<ProfilerService>.Instance.gameObject.AddComponent<SRPBatcherProfiler>();
	}
}
