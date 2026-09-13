namespace RGNetUtils;

public enum IcmpType
{
	EchoReply = 0,
	DestinationUnreachable = 3,
	SourceQuench = 4,
	Redirect = 5,
	EchoRequest = 8,
	TimeExceeded = 11,
	ParameterProblem = 12,
	Timestamp = 13,
	TimestampReply = 14,
	InformationRequest = 15,
	InformationReply = 16
}
