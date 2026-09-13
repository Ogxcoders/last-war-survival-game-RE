using System;
using MiniGame.Core;

namespace MiniGame.Test;

public class GameTestEnv : GameSharedEnv
{
	public override IResourceLoader ResourceLoader { get; set; }

	public override object TakeSnapshot()
	{
		throw new NotImplementedException();
	}

	public override void RestoreSnapshot(object snapshot)
	{
		throw new NotImplementedException();
	}
}
