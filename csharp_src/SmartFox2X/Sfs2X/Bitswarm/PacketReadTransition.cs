namespace Sfs2X.Bitswarm;

public enum PacketReadTransition
{
	HeaderReceived,
	SizeReceived,
	IncompleteSize,
	WholeSizeReceived,
	PacketFinished,
	InvalidData,
	InvalidDataFinished
}
