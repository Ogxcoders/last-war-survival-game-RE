using System;
using UnityEngine;

public class GPUSkinNodeMapping : MonoBehaviour
{
	public string[] nodeOriginPath;

	public string[] nodeNewPath;

	public Transform[] nodeNewObj;

	public Transform FindNodeByOrigPath(string path)
	{
		int num = Array.FindIndex(nodeOriginPath, (string s) => s == path);
		if (num >= 0 && num < nodeNewObj.Length)
		{
			return nodeNewObj[num];
		}
		return null;
	}
}
