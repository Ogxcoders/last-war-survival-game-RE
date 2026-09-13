using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

public abstract class Integers
{
	public static int RotateLeft(int i, int distance)
	{
		return (i << distance) ^ (i >>> -distance);
	}

	[CLSCompliant(false)]
	public static uint RotateLeft(uint i, int distance)
	{
		return (i << distance) ^ (i >> -distance);
	}

	public static int RotateRight(int i, int distance)
	{
		return (i >>> distance) ^ (i << -distance);
	}

	[CLSCompliant(false)]
	public static uint RotateRight(uint i, int distance)
	{
		return (i >> distance) ^ (i << -distance);
	}
}
