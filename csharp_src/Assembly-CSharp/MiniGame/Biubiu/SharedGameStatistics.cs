namespace MiniGame.Biubiu;

public struct SharedGameStatistics
{
	public int ExceptionCount;

	public int[] BulletNum;

	public long BattleTimeMills;

	public EPlayerID WinPlayerID;

	public int GameEndFrame;

	public int Retry_Connect_Count;

	public bool HeadShot;
}
