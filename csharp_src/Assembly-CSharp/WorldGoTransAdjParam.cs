using System;
using UnityEngine;

[Serializable]
public class WorldGoTransAdjParam
{
	public enum AdjustType
	{
		None,
		LocalScale,
		LocalPos,
		LocalPosZ
	}

	public Transform TransNode;

	public AdjustType AdjType;

	public string RefCurveName;

	public float ParamNumF;

	public Vector3 V3Param;
}
