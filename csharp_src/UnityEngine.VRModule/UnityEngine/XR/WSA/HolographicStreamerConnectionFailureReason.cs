using UnityEngine.Bindings;

namespace UnityEngine.XR.WSA;

[NativeHeader("Modules/VR/HoloLens/PerceptionRemoting.h")]
public enum HolographicStreamerConnectionFailureReason
{
	None,
	Unknown,
	NoServerCertificate,
	HandshakePortBusy,
	HandshakeUnreachable,
	HandshakeConnectionFailed,
	AuthenticationFailed,
	RemotingVersionMismatch,
	IncompatibleTransportProtocols,
	HandshakeFailed,
	TransportPortBusy,
	TransportUnreachable,
	TransportConnectionFailed,
	ProtocolVersionMismatch,
	ProtocolError,
	VideoCodecNotAvailable,
	Canceled,
	ConnectionLost,
	DeviceLost,
	DisconnectRequest
}
