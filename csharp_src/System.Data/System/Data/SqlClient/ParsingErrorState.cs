namespace System.Data.SqlClient;

internal enum ParsingErrorState
{
	Undefined,
	FedAuthInfoLengthTooShortForCountOfInfoIds,
	FedAuthInfoLengthTooShortForData,
	FedAuthInfoFailedToReadCountOfInfoIds,
	FedAuthInfoFailedToReadTokenStream,
	FedAuthInfoInvalidOffset,
	FedAuthInfoFailedToReadData,
	FedAuthInfoDataNotUnicode,
	FedAuthInfoDoesNotContainStsurlAndSpn,
	FedAuthInfoNotReceived,
	FedAuthNotAcknowledged,
	FedAuthFeatureAckContainsExtraData,
	FedAuthFeatureAckUnknownLibraryType,
	UnrequestedFeatureAckReceived,
	UnknownFeatureAck,
	InvalidTdsTokenReceived,
	SessionStateLengthTooShort,
	SessionStateInvalidStatus,
	CorruptedTdsStream,
	ProcessSniPacketFailed,
	FedAuthRequiredPreLoginResponseInvalidValue
}
