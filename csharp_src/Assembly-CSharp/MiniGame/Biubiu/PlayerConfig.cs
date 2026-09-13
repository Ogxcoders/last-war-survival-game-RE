using System;
using Box2DSharp.Common;

namespace MiniGame.Biubiu;

[Serializable]
public class PlayerConfig : IConfig
{
	public FVector2 LowerBound;

	public FVector2 UpperBound;

	public FP Spine3BoneHeight;
}
