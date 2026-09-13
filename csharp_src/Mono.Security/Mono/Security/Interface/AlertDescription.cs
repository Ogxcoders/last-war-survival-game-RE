namespace Mono.Security.Interface;

public enum AlertDescription : byte
{
	CloseNotify = 0,
	UnexpectedMessage = 10,
	BadRecordMAC = 20,
	DecryptionFailed_RESERVED = 21,
	RecordOverflow = 22,
	DecompressionFailure = 30,
	HandshakeFailure = 40,
	NoCertificate_RESERVED = 41,
	BadCertificate = 42,
	UnsupportedCertificate = 43,
	CertificateRevoked = 44,
	CertificateExpired = 45,
	CertificateUnknown = 46,
	IlegalParameter = 47,
	UnknownCA = 48,
	AccessDenied = 49,
	DecodeError = 50,
	DecryptError = 51,
	ExportRestriction = 60,
	ProtocolVersion = 70,
	InsuficientSecurity = 71,
	InternalError = 80,
	UserCancelled = 90,
	NoRenegotiation = 100,
	UnsupportedExtension = 110
}
