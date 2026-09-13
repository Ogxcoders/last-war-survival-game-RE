using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using Sfs2XLw.Core;
using Sfs2XLw.Core.Sockets;
using Sfs2XLw.Entities.Data;
using Sfs2XLw.Logging;
using Sfs2XLw.Protocol.Serialization;
using Sfs2XLw.Util;

namespace Sfs2XLw.Bitswarm;

public class UDPManager : IUDPManager
{
	private SmartFox sfs;

	private long packetId;

	private ISocketLayer udpSocket;

	private Logger log;

	private bool locked;

	private bool initSuccess;

	private readonly int MAX_RETRY = 3;

	private readonly int RESPONSE_TIMEOUT = 3000;

	private Timer initThread;

	private int currentAttempt;

	private IPacketEncrypter packetEncrypter;

	private object initThreadLocker = new object();

	public long NextUdpPacketId => packetId++;

	public bool Inited => initSuccess;

	public UDPManager(SmartFox sfs)
	{
		this.sfs = sfs;
		packetId = 0L;
		if (sfs != null)
		{
			log = sfs.Log;
		}
		else
		{
			log = new Logger(null);
		}
		currentAttempt = 1;
		packetEncrypter = new DefaultPacketEncrypter(sfs.GetSocketEngine() as BitSwarmClient);
	}

	public void Initialize(string udpAddr, int udpPort)
	{
		if (initSuccess)
		{
			log.Warn("UDP Channel already initialized!");
		}
		else if (!locked)
		{
			locked = true;
			udpSocket = new UDPSocketLayer(sfs);
			udpSocket.OnData = OnUDPData;
			udpSocket.OnError = OnUDPError;
			udpSocket.Connect(udpAddr, udpPort);
			SendInitializationRequest();
		}
		else
		{
			log.Warn("UPD initialization is already in progress!");
		}
	}

	public void Send(ByteArray binaryData)
	{
		if (initSuccess)
		{
			try
			{
				udpSocket.Write(binaryData.Bytes);
				if (sfs.Debug)
				{
					log.Info("UDP Data written: " + DefaultObjectDumpFormatter.HexDump(binaryData));
				}
				return;
			}
			catch (Exception ex)
			{
				log.Warn("WriteUDP operation failed due to error: " + ex.Message + " " + ex.StackTrace);
				return;
			}
		}
		log.Warn("UDP protocol is not initialized yet. Please use the initUDP() method.");
	}

	public bool isConnected()
	{
		return udpSocket.IsConnected;
	}

	public void Reset()
	{
		StopTimer();
		currentAttempt = 1;
		initSuccess = false;
		locked = false;
		packetId = 0L;
	}

	private void OnUDPData(byte[] bt)
	{
		ByteArray byteArray = new ByteArray(bt);
		if (byteArray.BytesAvailable < 4)
		{
			log.Warn("Too small UDP packet. Len: " + byteArray.Length);
			return;
		}
		if (sfs.Debug)
		{
			log.Info("UDP Data Read: " + DefaultObjectDumpFormatter.HexDump(byteArray));
		}
		byte num = byteArray.ReadByte();
		bool flag = (num & 0x20) > 0;
		bool flag2 = (num & 0x40) > 0;
		short num2 = byteArray.ReadShort();
		if (num2 != byteArray.BytesAvailable)
		{
			log.Warn("Insufficient UDP data. Expected: " + num2 + ", got: " + byteArray.BytesAvailable);
			return;
		}
		ByteArray byteArray2 = new ByteArray(byteArray.ReadBytes(byteArray.BytesAvailable));
		if (flag2)
		{
			try
			{
				packetEncrypter.Decrypt(byteArray2);
			}
			catch (Exception ex)
			{
				log.Warn("UDP data decryption failed due to error: " + ex.Message + " " + ex.StackTrace);
				return;
			}
		}
		if (flag)
		{
			byteArray2.Uncompress();
		}
		ISFSObject iSFSObject = SFSObject.NewFromBinaryData(byteArray2);
		if (iSFSObject.ContainsKey("h"))
		{
			if (!initSuccess)
			{
				StopTimer();
				locked = false;
				initSuccess = true;
				Hashtable hashtable = new Hashtable();
				hashtable["success"] = true;
				sfs.DispatchEvent(new SFSEvent(SFSEvent.UDP_INIT, hashtable));
			}
		}
		else
		{
			sfs.GetSocketEngine().IoHandler.Codec.OnPacketRead(iSFSObject);
		}
	}

	private void OnUDPError(string error, SocketError se)
	{
		log.Warn("Unexpected UDP I/O Error. " + error + " [" + se.ToString() + "]");
	}

	private void SendInitializationRequest()
	{
		SFSObject sFSObject = new SFSObject();
		((ISFSObject)sFSObject).PutByte("c", (byte)1);
		((ISFSObject)sFSObject).PutByte("h", (byte)1);
		((ISFSObject)sFSObject).PutLong("i", NextUdpPacketId);
		((ISFSObject)sFSObject).PutInt("u", sfs.MySelf.Id);
		ByteArray byteArray = ((ISFSObject)sFSObject).ToBinary();
		ByteArray byteArray2 = new ByteArray();
		byteArray2.WriteByte(128);
		byteArray2.WriteShort(Convert.ToInt16(byteArray.Length));
		byteArray2.WriteBytes(byteArray.Bytes);
		udpSocket.Write(byteArray2.Bytes);
		StartTimer();
	}

	private void OnTimeout(object state)
	{
		if (initSuccess)
		{
			return;
		}
		lock (initThreadLocker)
		{
			if (initThread == null)
			{
				return;
			}
		}
		if (currentAttempt < MAX_RETRY)
		{
			currentAttempt++;
			log.Debug("UDP Init Attempt: " + currentAttempt);
			SendInitializationRequest();
			StartTimer();
		}
		else
		{
			currentAttempt = 0;
			locked = false;
			Hashtable hashtable = new Hashtable();
			hashtable["success"] = false;
			sfs.DispatchEvent(new SFSEvent(SFSEvent.UDP_INIT, hashtable));
		}
	}

	private void StartTimer()
	{
		if (initThread != null)
		{
			initThread.Dispose();
		}
		initThread = new Timer(OnTimeout, null, RESPONSE_TIMEOUT, -1);
	}

	private void StopTimer()
	{
		lock (initThreadLocker)
		{
			if (initThread != null)
			{
				initThread.Dispose();
				initThread = null;
			}
		}
	}

	public void Disconnect()
	{
		if (udpSocket != null)
		{
			udpSocket.Disconnect();
		}
		Reset();
	}
}
