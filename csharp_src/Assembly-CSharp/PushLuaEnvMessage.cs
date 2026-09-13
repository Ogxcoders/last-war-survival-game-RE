using Sfs2X.Entities.Data;
using UnityEngine;
using XLua;

public class PushLuaEnvMessage : BaseMessage
{
	private static PushLuaEnvMessage _instance;

	public static PushLuaEnvMessage Instance => _instance ?? (_instance = MessageFactory.GetMessage<PushLuaEnvMessage>());

	public override string GetMsgId()
	{
		return "push.lua.env";
	}

	protected override void CSHandleResponse(ISFSObject message)
	{
	}

	public static void Exe(string a, string b)
	{
		LuaEnv env = GameEntry.Lua.Env;
		env.DoString("\n            function add(a, b)\n                return a .. b ..'  '.. LuaEntry.DataConfig:GetMd5()\n            end\n        ");
		object[] array = env.Global.Get<LuaFunction>("add").Call(a, b);
		Debug.Log("Result: " + array[0]);
	}
}
