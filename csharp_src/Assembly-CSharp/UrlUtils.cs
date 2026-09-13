using System.IO;
using UnityEngine;

public static class UrlUtils
{
	private const string HEAD_URL_MID = "http://app1.im.medrickgames.com:8086/fb/img/";

	private const string HEAD_URL = "https://im30-i.akamaized.net/fb/img/";

	public static bool IsUseCustomPic(int picVer)
	{
		if (picVer > 0 && picVer < 1000000)
		{
			return true;
		}
		return false;
	}

	public static bool GenCustomPicUrl(string uid, int picVer, out string url, out string key)
	{
		url = "";
		key = "";
		string md5Hash = AESHelper.GetMd5Hash(uid + $"_{picVer}");
		string text = uid;
		if (text.Length > 6)
		{
			text = text.Substring(text.Length - 6);
		}
		key = text + "/" + md5Hash + ".jpg";
		string text2 = Path.Combine(Application.persistentDataPath, "Images", "heads", key);
		if (File.Exists(text2))
		{
			url = "file://" + text2;
		}
		else if (GameEntry.GlobalData.isMiddleEast())
		{
			url = "http://app1.im.medrickgames.com:8086/fb/img/" + key;
		}
		else
		{
			url = "https://im30-i.akamaized.net/fb/img/" + key;
		}
		return true;
	}
}
