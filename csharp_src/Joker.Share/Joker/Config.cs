using System;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Joker;

public static class Config
{
	public static void ReadConfig(object config, JObject data)
	{
		FieldInfo[] fields = config.GetType().GetFields(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		foreach (FieldInfo fieldInfo in fields)
		{
			if (!data.ContainsKey(fieldInfo.Name))
			{
				continue;
			}
			JToken jToken = data[fieldInfo.Name];
			ConfigValidatorAttribute customAttribute = fieldInfo.GetCustomAttribute<ConfigValidatorAttribute>();
			try
			{
				switch (jToken.Type)
				{
				case JTokenType.Boolean:
					fieldInfo.SetValue(config, (bool)jToken);
					customAttribute?.CheckValid((bool)jToken);
					break;
				case JTokenType.Integer:
					fieldInfo.SetValue(config, (int)jToken);
					customAttribute?.CheckValid((int)jToken);
					break;
				case JTokenType.Float:
					fieldInfo.SetValue(config, (float)(double)jToken);
					customAttribute?.CheckValid((float)(double)jToken);
					break;
				case JTokenType.String:
					fieldInfo.SetValue(config, (string?)jToken);
					customAttribute?.CheckValid((string?)jToken);
					break;
				default:
					throw new Exception($"Unsupport config key:{fieldInfo.Name} type:{jToken.Type}");
				}
			}
			catch (ConfigInvalidException ex)
			{
				throw new ConfigInvalidException("config attribute invalid:" + fieldInfo.Name + "." + ex.Message);
			}
			catch (Exception)
			{
				throw new InvalidCastException("Unsupport config key:" + fieldInfo.Name + " type:" + jToken.ToString());
			}
		}
	}
}
