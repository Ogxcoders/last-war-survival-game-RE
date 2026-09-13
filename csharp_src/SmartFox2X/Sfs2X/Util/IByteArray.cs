using Sfs2X.Entities.Data;

namespace Sfs2X.Util;

public interface IByteArray
{
	byte[] Bytes { get; set; }

	int Length { get; }

	int Position { get; set; }

	int BytesAvailable { get; }

	void WriteByte(SFSDataType tp);

	void WriteByte(byte b);

	void WriteBytes(byte[] data);

	void WriteBytes(byte[] data, int ofs, int count);

	void WriteBool(bool b);

	void WriteInt(int i);

	void WriteUShort(ushort us);

	void WriteShort(short s);

	void WriteLong(long l);

	void WriteFloat(float f);

	void WriteDouble(double d);

	void WriteUTF(string str);

	void WriteText(string str);

	byte ReadByte();

	byte[] ReadBytes(int count);

	bool ReadBool();

	int ReadInt();

	ushort ReadUShort();

	short ReadShort();

	long ReadLong();

	float ReadFloat();

	double ReadDouble();

	string ReadUTF();

	string ReadText();
}
