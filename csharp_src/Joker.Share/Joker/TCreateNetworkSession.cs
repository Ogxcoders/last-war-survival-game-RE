using System.Net;

namespace Joker;

public delegate NetworkSession TCreateNetworkSession(long sessionId, IPEndPoint endPoint);
