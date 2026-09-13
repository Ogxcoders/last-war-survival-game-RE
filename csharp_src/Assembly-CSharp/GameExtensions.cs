using System;
using System.Text;
using UnityEngine;

public static class GameExtensions
{
	public static string GetFullPath(this Transform obj)
	{
		if (obj == null)
		{
			return string.Empty;
		}
		try
		{
			Transform parent = obj.parent;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(parent.name);
			while (parent != null)
			{
				stringBuilder.Insert(0, parent.name + "/");
				parent = parent.parent;
			}
			return stringBuilder.ToString();
		}
		catch (Exception)
		{
			return obj.name;
		}
	}
}
