using Sfs2X.Entities.Data;

public static class MessageExtension
{
	public static int TryGetInt(this ISFSObject obj, string key)
	{
		return (int)obj.TryGetNumber(key);
	}

	public static float TryGetFloat(this ISFSObject obj, string key)
	{
		return (float)obj.TryGetNumber(key);
	}

	public static long TryGetLong(this ISFSObject obj, string key)
	{
		if (obj != null && obj.ContainsKey(key))
		{
			switch (obj.GetData(key).Type)
			{
			case 2:
				return obj.GetByte(key);
			case 3:
				return obj.GetShort(key);
			case 4:
				return obj.GetInt(key);
			case 5:
				return obj.GetLong(key);
			case 7:
				return (long)obj.GetDouble(key);
			case 6:
				return (long)obj.GetFloat(key);
			case 8:
			{
				long result = 0L;
				long.TryParse(obj.GetUtfString(key), out result);
				return result;
			}
			}
		}
		return 0L;
	}

	public static double TryGetDouble(this ISFSObject obj, string key)
	{
		return obj.TryGetNumber(key);
	}

	public static double TryGetNumber(this ISFSObject obj, string key)
	{
		if (obj != null && obj.ContainsKey(key))
		{
			switch (obj.GetData(key).Type)
			{
			case 2:
				return (int)obj.GetByte(key);
			case 3:
				return obj.GetShort(key);
			case 4:
				return obj.GetInt(key);
			case 5:
				return obj.GetLong(key);
			case 7:
				return obj.GetDouble(key);
			case 6:
				return obj.GetFloat(key);
			case 8:
			{
				double result = 0.0;
				double.TryParse(obj.GetUtfString(key), out result);
				return result;
			}
			}
		}
		return 0.0;
	}

	public static bool TryGetBool(this ISFSObject obj, string key)
	{
		if (obj != null && obj.ContainsKey(key))
		{
			switch (obj.GetData(key).Type)
			{
			case 1:
				return obj.GetBool(key);
			case 4:
				return obj.TryGetNumber(key) != 0.0;
			}
		}
		return false;
	}

	public static string TryGetString(this ISFSObject obj, string key)
	{
		if (obj != null && obj.ContainsKey(key))
		{
			int type = obj.GetData(key).Type;
			if (type == 8)
			{
				return obj.GetUtfString(key);
			}
			return obj.TryGetNumber(key).ToString();
		}
		return "";
	}

	public static string TryMergeString(this ISFSObject obj, string key)
	{
		if (obj.ContainsKey(key))
		{
			ISFSArray sFSArray = obj.GetSFSArray(key);
			string text = string.Empty;
			for (int i = 0; i < sFSArray.Count; i++)
			{
				string utfString = sFSArray.GetUtfString(i);
				if (!string.IsNullOrEmpty(utfString))
				{
					text += utfString;
				}
			}
			return text;
		}
		return "";
	}

	public static ISFSObject TryGetObj(this ISFSObject obj, string key)
	{
		if (obj != null && obj.ContainsKey(key) && obj.GetData(key).Type == 18)
		{
			return obj.GetSFSObject(key);
		}
		return null;
	}

	public static ISFSArray TryGetArray(this ISFSObject obj, string key)
	{
		if (obj != null && obj.ContainsKey(key))
		{
			return obj.GetSFSArray(key);
		}
		return null;
	}

	public static int[] TryGetIntArray(this ISFSObject obj, string key)
	{
		if (obj != null && obj.ContainsKey(key))
		{
			return obj.GetIntArray(key);
		}
		return null;
	}

	public static long GetLong(this ISFSObject obj, string key)
	{
		if (obj != null && obj.ContainsKey(key))
		{
			return obj.GetLong(key);
		}
		return 0L;
	}

	public static bool IsNullOrEmpty(this ISFSArray obj)
	{
		if (obj != null)
		{
			return obj.Count == 0;
		}
		return true;
	}
}
