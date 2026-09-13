public class WorldAssistanceHeroAsync : AsyncMono<WorldAssistanceHeroAsync>
{
	public CircleMeshInstanced meshRenderer;

	public void SetHeroHead(int heroId)
	{
		if (heroId != 0)
		{
			string spritePath = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetHeroIcon", heroId);
			meshRenderer.LoadSprite(spritePath);
		}
	}
}
