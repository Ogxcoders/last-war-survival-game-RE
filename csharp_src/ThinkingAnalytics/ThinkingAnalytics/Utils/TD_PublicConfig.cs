using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace ThinkingAnalytics.Utils;

public class TD_PublicConfig
{
	public static bool DisableCSharpException = false;

	public static List<string> DisPresetProperties = new List<string>();

	public static readonly string LIB_VERSION = "2.5.1.4";

	public static void GetPublicConfig()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("ta_public_config");
		if (!(textAsset != null) || string.IsNullOrEmpty(textAsset.text))
		{
			return;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(textAsset.text);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("resources");
		for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
		{
			XmlNode xmlNode2 = xmlNode.ChildNodes[i];
			if (xmlNode2.NodeType != XmlNodeType.Element)
			{
				continue;
			}
			XmlElement xmlElement = xmlNode2 as XmlElement;
			if (!xmlElement.HasAttributes)
			{
				continue;
			}
			string attribute = xmlElement.GetAttribute("name");
			if (attribute == "TDDisPresetProperties" && xmlElement.HasChildNodes)
			{
				for (int j = 0; j < xmlElement.ChildNodes.Count; j++)
				{
					XmlNode xmlNode3 = xmlElement.ChildNodes[j];
					if (xmlNode3.NodeType == XmlNodeType.Element)
					{
						DisPresetProperties.Add(xmlNode3.InnerText);
					}
				}
			}
			else if (attribute == "DisableCSharpException")
			{
				DisableCSharpException = Convert.ToBoolean(xmlElement.InnerText);
			}
		}
	}
}
