using System;
using System.Text.RegularExpressions;

public static class IOSDevicesScore
{
	public static int QueryModelScore(string query)
	{
		int result = -1;
		if (query.StartsWith("iPhone"))
		{
			result = QueryIphone(query);
		}
		else if (query.StartsWith("iPod"))
		{
			result = QueryIpod(query);
		}
		else if (query.StartsWith("iPad"))
		{
			result = QueryIpad(query);
		}
		return result;
	}

	private static int QueryIphone(string query)
	{
		switch (query)
		{
		case "iPhone17,5":
		case "iPhone17,4":
		case "iPhone17,3":
		case "iPhone17,2":
		case "iPhone17,1":
		case "iPhone16,2":
		case "iPhone16,1":
			return 140;
		case "iPhone15,5":
		case "iPhone15,4":
		case "iPhone15,3":
		case "iPhone15,2":
		case "iPhone15,1":
		case "iPhone14,8":
		case "iPhone14,7":
		case "iPhone14,6":
		case "iPhone14,5":
		case "iPhone14,4":
		case "iPhone14,3":
		case "iPhone14,2":
		case "iPhone14,1":
			return 120;
		case "iPhone13,4":
		case "iPhone13,3":
		case "iPhone13,2":
		case "iPhone13,1":
			return 100;
		case "iPhone12,8":
		case "iPhone12,5":
		case "iPhone12,3":
		case "iPhone12,1":
			return 80;
		case "iPhone11,8":
		case "iPhone11,6":
		case "iPhone11,4":
		case "iPhone11,2":
		case "iPhone10,6":
		case "iPhone10,5":
		case "iPhone10,4":
		case "iPhone10,3":
		case "iPhone10,2":
		case "iPhone10,1":
			return 60;
		case "iPhone9,4":
		case "iPhone9,3":
		case "iPhone9,2":
		case "iPhone9,1":
		case "iPhone8,4":
		case "iPhone8,2":
		case "iPhone8,1":
			return 40;
		case "iPhone7,2":
		case "iPhone7,1":
		case "iPhone6,2":
		case "iPhone6,1":
			return 20;
		default:
			try
			{
				Match match = new Regex("iPhone(\\d+),").Match(query);
				if (match.Success)
				{
					int num = int.Parse(match.Groups[1].Value);
					if (num >= 16)
					{
						return 140;
					}
					if (num >= 14)
					{
						return 120;
					}
					if (num >= 13)
					{
						return 100;
					}
					if (num >= 12)
					{
						return 80;
					}
					if (num >= 10)
					{
						return 60;
					}
					if (num >= 8)
					{
						return 40;
					}
					if (num >= 6)
					{
						return 20;
					}
					return 0;
				}
			}
			catch (Exception)
			{
				return 0;
			}
			return 0;
		}
	}

	private static int QueryIpod(string query)
	{
		return 0;
	}

	private static int QueryIpad(string query)
	{
		switch (query)
		{
		case "iPad16,6":
		case "iPad16,5":
		case "iPad16,4":
		case "iPad16,3":
		case "iPad16,2":
		case "iPad16,1":
			return 140;
		case "iPad14,9":
		case "iPad14,8":
		case "iPad14,6":
		case "iPad14,5":
		case "iPad14,4":
		case "iPad14,3":
		case "iPad14,2":
		case "iPad14,11":
		case "iPad14,10":
		case "iPad14,1":
			return 120;
		case "iPad13,9":
		case "iPad13,8":
		case "iPad13,7":
		case "iPad13,6":
		case "iPad13,5":
		case "iPad13,4":
		case "iPad13,2":
		case "iPad13,19":
		case "iPad13,18":
		case "iPad13,17":
		case "iPad13,16":
		case "iPad13,11":
		case "iPad13,10":
		case "iPad13,1":
			return 100;
		case "iPad12,2":
		case "iPad12,1":
		case "iPad11,7":
		case "iPad11,6":
		case "iPad11,4":
		case "iPad11,3":
		case "iPad11,2":
		case "iPad11,1":
			return 80;
		case "iPad8,12":
		case "iPad8,11":
		case "iPad8,10":
		case "iPad8,9":
		case "iPad8,8":
		case "iPad8,7":
		case "iPad8,6":
		case "iPad8,5":
		case "iPad8,4":
		case "iPad8,3":
		case "iPad8,2":
		case "iPad8,1":
			return 60;
		case "iPad7,6":
		case "iPad7,5":
		case "iPad7,4":
		case "iPad7,3":
		case "iPad7,2":
		case "iPad7,1":
		case "iPad7,12":
		case "iPad7,11":
		case "iPad6,8":
		case "iPad6,7":
		case "iPad6,4":
		case "iPad6,3":
		case "iPad6,12":
		case "iPad6,11":
			return 40;
		case "iPad5,4":
		case "iPad5,3":
		case "iPad5,2":
		case "iPad5,1":
		case "iPad4,5":
		case "iPad4,4":
		case "iPad4,2":
		case "iPad4,1":
			return 20;
		default:
			try
			{
				Match match = new Regex("iPad(\\d+),").Match(query);
				if (match.Success)
				{
					int num = int.Parse(match.Groups[1].Value);
					if (num >= 16)
					{
						return 140;
					}
					if (num >= 14)
					{
						return 120;
					}
					if (num >= 13)
					{
						return 100;
					}
					if (num >= 11)
					{
						return 80;
					}
					if (num >= 8)
					{
						return 60;
					}
					if (num >= 6)
					{
						return 40;
					}
					return 20;
				}
			}
			catch (Exception)
			{
				return 0;
			}
			return 0;
		}
	}
}
