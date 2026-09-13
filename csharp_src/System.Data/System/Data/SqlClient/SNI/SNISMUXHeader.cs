namespace System.Data.SqlClient.SNI;

internal class SNISMUXHeader
{
	public const int HEADER_LENGTH = 16;

	public byte SMID;

	public byte flags;

	public ushort sessionId;

	public uint length;

	public uint sequenceNumber;

	public uint highwater;
}
