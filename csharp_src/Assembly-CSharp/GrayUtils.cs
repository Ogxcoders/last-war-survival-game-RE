using System;
using System.Security.Cryptography;
using System.Text;
using GameFramework;

public static class GrayUtils
{
	public static bool hasFileLog;

	public static bool isGrayServer => GameEntry.Setting.GetString("SERVER_ZONE", "").CompareTo("APS3") == 0;

	public static bool isGM => GameEntry.Setting.GetInt("Setting.GM_FLAG") > 0;

	public static bool InGrayServer(int start, int end)
	{
		if (end < start || start < 0 || end < 0)
		{
			return false;
		}
		if (int.TryParse(GameEntry.Setting.GetString("SERVER_ZONE", "").Replace("APS", ""), out var result))
		{
			if (result >= start)
			{
				return result <= end;
			}
			return false;
		}
		return false;
	}

	public static bool IsGrayDevice(int percent = 5, bool gmAlways = true)
	{
		if (gmAlways && isGM)
		{
			return true;
		}
		return GetPercentageFromMd5(GameEntry.Device.GetDeviceUid()) <= (double)percent;
	}

	public static double GetPercentageFromMd5(string input)
	{
		using MD5 mD = MD5.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		return BitConverter.ToUInt64(mD.ComputeHash(bytes), 0) % 100 + 1;
	}

	public static bool CheckLuaSwitchWithServerFormat(string key, string param, int serverId)
	{
		try
		{
			string[] array = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", key, param).Split(new char[1] { ';' });
			foreach (string text in array)
			{
				if (text.Contains("-"))
				{
					string[] array2 = text.Split(new char[1] { '-' });
					int num = int.Parse(array2[0]);
					int num2 = int.Parse(array2[1]);
					if (serverId >= num && serverId <= num2)
					{
						return true;
					}
				}
				else if (serverId == int.Parse(text))
				{
					return true;
				}
			}
			return false;
		}
		catch (Exception ex)
		{
			Log.Error($"CheckLuaSwitchWithServerFormat input:{key} {param} {serverId}  Exception:{ex.ToString()}");
			return false;
		}
	}
}
