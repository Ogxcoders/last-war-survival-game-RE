namespace FibMatrix.BaseUtils;

public class BaseUtilRuntimeCfg : FibSingletonCfgBase
{
	private static BaseUtilRuntimeCfg s_Inst;

	public string devCodeName = "";

	public EngineBIConfig biConfig;

	public static BaseUtilRuntimeCfg Instance
	{
		get
		{
			if (s_Inst == null)
			{
				s_Inst = FibSingletonCfgBase.LoadOrCreate<BaseUtilRuntimeCfg>(runtimeAsset: true);
			}
			return s_Inst;
		}
	}
}
