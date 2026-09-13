using FibMatrix.BaseUtils;
using UnityEngine;

namespace FibMatrix.PerfTools;

[HelpURL("https://rivergame.feishu.cn/wiki/wikcnJVM2odOxoIEE0DkTTi8Tye")]
internal class PerfToolsRuntimeCfg : FibSingletonCfgBase
{
	private static PerfToolsRuntimeCfg s_Inst;

	public OverdrawMonitorCfg overdrawMonitorConfig;

	public static PerfToolsRuntimeCfg Instance
	{
		get
		{
			if (s_Inst == null)
			{
				s_Inst = FibSingletonCfgBase.LoadOrCreate<PerfToolsRuntimeCfg>(runtimeAsset: true);
			}
			return s_Inst;
		}
	}
}
