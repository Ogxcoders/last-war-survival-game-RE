using System.Threading;

namespace Joker;

public static class NetServices
{
	private static int acceptIdGenerator = int.MinValue;

	public static uint CreateConnectChannelId()
	{
		return RandomGenerator.RandUInt32();
	}

	public static uint CreateAcceptChannelId()
	{
		return (uint)Interlocked.Add(ref acceptIdGenerator, 1);
	}
}
