using Box2DSharp.Common;
using MiniGame.Core;

namespace MiniGame.Biubiu.Client;

public class SystemClientLockStepInput : SystemClientInput
{
	protected override void CreateBulletImpl(int entity, FVector2 bodyPos, FVector2 bulletPos, FVector2 dir, int id)
	{
		GameBiuBiuPlayerBase player = ((_shared.Value.ResourceLoader as GameLoader).LoaderEnv as GameBiubiuPlayerEnv).Player;
		if (player.IsMultiPlayerGame && _shared.Value.GameState == EGameWorldState.Running)
		{
			GameBiubiuFrameSyncResp.CmdCreateBullet message = new GameBiubiuFrameSyncResp.CmdCreateBullet
			{
				EntityID = entity,
				FrameIndex = -1,
				BodyPosX = bodyPos.X.RawValue,
				BodyPosY = bodyPos.Y.RawValue,
				PositionX = bulletPos.X.RawValue,
				PositionY = bulletPos.Y.RawValue,
				DirectionX = dir.X.RawValue,
				DirectionY = dir.Y.RawValue
			};
			player.Send(message);
		}
	}
}
