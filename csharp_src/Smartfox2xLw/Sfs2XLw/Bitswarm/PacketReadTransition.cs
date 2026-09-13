namespace Sfs2XLw.Bitswarm;

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
