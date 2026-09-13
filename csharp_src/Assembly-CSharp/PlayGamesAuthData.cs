using System;
using XLua;

[Serializable]
[LuaCallCSharp(GenFlag.No)]
public class PlayGamesAuthData
{
	public string authCode;

	public string playerId;

	public string displayName;

	public bool success;

	public int code;

	public string error;
}
