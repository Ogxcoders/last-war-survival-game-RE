using System;
using UnityEngine;

[Serializable]
public class GPUSkinningBone
{
	[NonSerialized]
	public Transform transform;

	public Matrix4x4 bindpose;

	public int parentBoneIndex = -1;

	public int[] childrenBonesIndices;

	[NonSerialized]
	public Matrix4x4 animationMatrix;

	public string name;

	public string guid;

	[NonSerialized]
	private bool bindposeInvInit;

	[NonSerialized]
	private Matrix4x4 bindposeInv;

	public Matrix4x4 BindposeInv
	{
		get
		{
			if (!bindposeInvInit)
			{
				bindposeInv = bindpose.inverse;
				bindposeInvInit = true;
			}
			return bindposeInv;
		}
	}
}
