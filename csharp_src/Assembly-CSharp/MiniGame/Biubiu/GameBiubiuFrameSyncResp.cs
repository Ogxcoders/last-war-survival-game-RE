using System.Collections.Generic;
using MiniGame.Core.Server;

namespace MiniGame.Biubiu;

public class GameBiubiuFrameSyncResp : IMessageSync
{
	public class CmdCreateBullet : IMessageSync
	{
		public int EntityID;

		public int FrameIndex;

		public long PositionX;

		public long PositionY;

		public long BodyPosX;

		public long BodyPosY;

		public long DirectionX;

		public long DirectionY;
	}

	public class ViewGunAim : IMessageSync
	{
		public int PlayerID;

		public float X;

		public float Y;

		public float Z;
	}

	public int LogicTickLockStep;

	public List<CmdCreateBullet> Commands;
}
