using System;
using Sfs2X.Bitswarm;
using Sfs2X.Entities.Data;

namespace ProtoBufNet;

public interface INetPacket : IDisposable
{
	int bodyLengthHasRead { get; set; }

	int bodyHasRead { get; set; }

	ISFSObject info { get; }

	string logType { get; set; }

	long recvTime { get; set; }

	void headerBuffInfo(out NetPacketBufferInfo info);

	void bodyLengthBuffInfo(out NetPacketBufferInfo info);

	byte[] bodyBuffBytes();

	int bodyBuffLength();

	void onBeforeSend(IMessage msg);

	void parseHeader();

	void parseBodyLength();

	void onPackageReceiveBegin();

	void onPackageReceiveFinish();

	void onPackageSendBegin();

	void onPackageSendFinish();
}
