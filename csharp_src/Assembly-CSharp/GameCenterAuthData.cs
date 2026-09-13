using System;
using XLua;

[Serializable]
[LuaCallCSharp(GenFlag.No)]
public class GameCenterAuthData
{
	public string displayName;

	public string playerID;

	public string teamPlayerID;

	public string publicKeyUrl;

	public string signature;

	public string salt;

	public string timestamp;
}
