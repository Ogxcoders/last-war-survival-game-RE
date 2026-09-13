using System;
using System.Collections.Generic;
using Joker;

namespace MiniGame.Core.Server;

public class RoomMessage : IMessage
{
	public string SID;

	public string UUID;

	public string RoomSessionID;

	public string UID;

	public string PlayerSessionID;

	public int OpCode;

	public object Data;

	public static List<Type> MessageTypes = new List<Type>
	{
		typeof(C2SGameRoomCreate),
		typeof(S2CGameRoomCreate),
		typeof(C2SGameRoomDestroy),
		typeof(S2CGameRoomDestroy),
		typeof(C2SGameRoomEnter),
		typeof(S2CGameRoomEnter),
		typeof(C2SGameRoomLeave),
		typeof(S2CGameRoomLeave),
		typeof(C2SGameRoomReady),
		typeof(S2CGameRoomReady),
		typeof(C2SGameRoomStart),
		typeof(S2CGameRoomStart),
		typeof(C2SGameRoomEnd),
		typeof(S2CGameRoomEnd),
		typeof(C2SGameRoomVerify),
		typeof(S2CGameRoomVerify),
		typeof(C2SGameRoomSync),
		typeof(S2CGameRoomSync)
	};
}
