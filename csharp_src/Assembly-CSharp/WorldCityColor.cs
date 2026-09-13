using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using VEngine;

public class WorldCityColor
{
	public int id;

	public Color outlineColor;

	public Color innerColor;

	public Color baseColor;

	public int splashIndex;

	public string splashPath;

	private Asset _asset;

	private static Dictionary<int, Color> temperatureTextColor = new Dictionary<int, Color>(300);

	public static Color ConvertToColor(string str)
	{
		if (!str.IsNullOrEmpty())
		{
			if (str[0] != '#')
			{
				str = "#" + str;
			}
			if (ColorUtility.TryParseHtmlString(str, out var color))
			{
				return color;
			}
		}
		return Color.black;
	}

	public static Color HexToColor(string str)
	{
		if (!str.IsNullOrEmpty())
		{
			int num = 0;
			if (str.StartsWith("#"))
			{
				num = 1;
			}
			float num2 = int.Parse(str.Substring(num, 2), NumberStyles.HexNumber);
			float num3 = int.Parse(str.Substring(num + 2, 2), NumberStyles.HexNumber);
			float num4 = int.Parse(str.Substring(num + 4, 2), NumberStyles.HexNumber);
			float num5 = 255f;
			if (str.Length > 7)
			{
				num5 = int.Parse(str.Substring(num + 6, 2), NumberStyles.HexNumber);
			}
			return new Color(num2 / 255f, num3 / 255f, num4 / 255f, num5 / 255f);
		}
		return Color.black;
	}

	public static Color GetTemperatureColor(float temperature)
	{
		if (temperature >= 50f)
		{
			return HexToColor("#ff6060");
		}
		if (temperature >= 44f)
		{
			return HexToColor("#fd7442");
		}
		if (temperature >= 39f)
		{
			return HexToColor("#fb9438");
		}
		if (temperature >= 34f)
		{
			return HexToColor("#f3cb38");
		}
		if (temperature >= 29f)
		{
			return HexToColor("#fadd71");
		}
		if (temperature >= 24f)
		{
			return HexToColor("#c5db5d");
		}
		if (temperature >= 19f)
		{
			return HexToColor("#76db5d");
		}
		if (temperature >= 14f)
		{
			return HexToColor("#5bdd98");
		}
		if (temperature >= 9f)
		{
			return HexToColor("#61e9db");
		}
		if (temperature >= 4f)
		{
			return HexToColor("#4fe8ea");
		}
		if (temperature >= -1f)
		{
			return HexToColor("#4be2f8");
		}
		if (temperature >= -6f)
		{
			return HexToColor("#4bd0f8");
		}
		if (temperature >= -11f)
		{
			return HexToColor("#4bc1fe");
		}
		if (temperature >= -16f)
		{
			return HexToColor("#5cb4ff");
		}
		if (temperature >= -21f)
		{
			return HexToColor("#71aaff");
		}
		if (temperature >= -26f)
		{
			return HexToColor("#7797ff");
		}
		if (temperature >= -31f)
		{
			return HexToColor("#8798ff");
		}
		_ = -37f;
		return HexToColor("#9191ff");
	}

	public static Color GetTemperatureTextColor(float temperature)
	{
		int num = (int)Mathf.Floor(temperature);
		if (temperatureTextColor.TryGetValue(num, out var value))
		{
			return value;
		}
		Color color = HexToColor(GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetTemperatureTextColor", num));
		temperatureTextColor[num] = color;
		return color;
	}

	public WorldCityColor(string strInnerColor, string strBaseColor, string strOutlineColor)
	{
		if (strOutlineColor.IsNullOrEmpty())
		{
			outlineColor = Color.white;
		}
		else
		{
			outlineColor = ConvertToColor(strOutlineColor);
		}
		if (strInnerColor.IsNullOrEmpty())
		{
			innerColor = Color.white;
		}
		else
		{
			innerColor = ConvertToColor(strInnerColor);
		}
		if (strBaseColor.IsNullOrEmpty())
		{
			baseColor = Color.white;
		}
		else
		{
			baseColor = ConvertToColor(strBaseColor);
		}
	}

	public WorldCityColor(int xmlId, string tableName)
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData(tableName, xmlId, "icon");
		string templateData2 = GameEntry.ConfigCache.GetTemplateData(tableName, xmlId, "outlineColor");
		string templateData3 = GameEntry.ConfigCache.GetTemplateData(tableName, xmlId, "innerColor");
		string templateData4 = GameEntry.ConfigCache.GetTemplateData(tableName, xmlId, "baseColor");
		if (templateData.IsNullOrEmpty())
		{
			string templateData5 = GameEntry.ConfigCache.GetTemplateData(tableName, xmlId, "splash");
			if (templateData5.IsNullOrEmpty())
			{
				splashIndex = 0;
			}
			else if (templateData5.Length == 1)
			{
				splashIndex = templateData5.ToInt();
			}
		}
		else
		{
			splashIndex = 0;
			splashPath = $"Assets/Main/Scenes/Zone/Textures/{templateData}.png";
		}
		if (templateData2.IsNullOrEmpty())
		{
			outlineColor = Color.white;
		}
		else
		{
			outlineColor = ConvertToColor(templateData2);
		}
		if (templateData3.IsNullOrEmpty())
		{
			innerColor = Color.white;
		}
		else
		{
			innerColor = ConvertToColor(templateData3);
		}
		if (templateData4.IsNullOrEmpty())
		{
			baseColor = Color.white;
		}
		else
		{
			baseColor = ConvertToColor(templateData4);
		}
		if (splashIndex > 0)
		{
			splashIndex--;
		}
		id = xmlId;
		_asset = null;
	}

	public Texture2D GetTexture2D()
	{
		if (_asset == null && !splashPath.IsNullOrEmpty() && GameEntry.Resource != null)
		{
			_asset = GameEntry.Resource.LoadAsset(splashPath, typeof(Texture2D));
		}
		if (_asset == null)
		{
			return null;
		}
		return _asset.asset as Texture2D;
	}

	public void Release()
	{
		if (_asset != null && GameEntry.Resource != null)
		{
			GameEntry.Resource.UnloadAsset(_asset);
			_asset = null;
		}
	}
}
