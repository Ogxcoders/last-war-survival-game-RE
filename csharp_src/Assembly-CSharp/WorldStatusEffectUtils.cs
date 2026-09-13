public class WorldStatusEffectUtils
{
	public static bool IsNeedShowStatusEffect()
	{
		int lodLevel = SceneManager.World.GetLodLevel();
		int num = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetCurrentDisplayLevel");
		if (lodLevel <= 2)
		{
			return num >= 0;
		}
		return false;
	}
}
