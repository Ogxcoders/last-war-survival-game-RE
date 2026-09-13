using System;
using Protobuf;

public class DominatorInfo : IDisposable
{
	public int dominatorId;

	public int dominatorRank;

	public void UpdateDominator(DominatorProto proto)
	{
		dominatorId = proto.DominatorId;
		dominatorRank = proto.RankLv;
	}

	public void Dispose()
	{
	}
}
