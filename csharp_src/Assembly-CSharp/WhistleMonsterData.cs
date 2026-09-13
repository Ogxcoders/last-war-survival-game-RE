using System;
using Protobuf;

public class WhistleMonsterData : IDisposable
{
	public WhistleMonsterState state;

	public void SetData(WhistleInfo msg)
	{
		state = (WhistleMonsterState)msg.State;
	}

	public void Dispose()
	{
	}
}
