namespace BestHTTP.WebSocket;

public enum WebSocketStausCodes : ushort
{
	NormalClosure = 1000,
	GoingAway = 1001,
	ProtocolError = 1002,
	WrongDataType = 1003,
	Reserved = 1004,
	NoStatusCode = 1005,
	ClosedAbnormally = 1006,
	DataError = 1007,
	PolicyError = 1008,
	TooBigMessage = 1009,
	ExtensionExpected = 1010,
	WrongRequest = 1011,
	TLSHandshakeError = 1015
}
