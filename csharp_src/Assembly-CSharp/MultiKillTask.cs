using System;

public class MultiKillTask : IDisposable
{
	public MultiKillBubbleData data;

	public MultiKillTaskType type;

	public MultiKillTask Init(MultiKillTaskType type, MultiKillBubbleData data)
	{
		this.type = type;
		this.data = data;
		return this;
	}

	public void Dispose()
	{
		data = null;
	}

	public override string ToString()
	{
		return $"{type}:{data.marchUuid}:{data.killNum}";
	}
}
