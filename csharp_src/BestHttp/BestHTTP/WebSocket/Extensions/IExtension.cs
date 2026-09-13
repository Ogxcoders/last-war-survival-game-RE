using BestHTTP.WebSocket.Frames;

namespace BestHTTP.WebSocket.Extensions;

public interface IExtension
{
	void AddNegotiation(HTTPRequest request);

	bool ParseNegotiation(WebSocketResponse resp);

	byte GetFrameHeader(WebSocketFrame writer, byte inFlag);

	byte[] Encode(WebSocketFrame writer);

	byte[] Decode(byte header, byte[] data, int length);
}
