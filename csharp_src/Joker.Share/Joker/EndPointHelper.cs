using System.Net;

namespace Joker;

public static class EndPointHelper
{
	public static IPEndPoint Clone(this EndPoint endPoint)
	{
		IPEndPoint iPEndPoint = (IPEndPoint)endPoint;
		return new IPEndPoint(iPEndPoint.Address, iPEndPoint.Port);
	}
}
