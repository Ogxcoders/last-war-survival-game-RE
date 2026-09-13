using System;
using UnityEngine;

public class GPUSkinningAnimation : ScriptableObject
{
	public string guid;

	public new string name;

	public GPUSkinningBone[] bones;

	public int rootBoneIndex;

	public GPUSkinningClip[] clips;

	public Bounds bounds;

	public int textureWidth;

	public int textureHeight;

	public GPUSkinningBone GetBoneByTransform(Transform transform)
	{
		int num = bones.Length;
		for (int i = 0; i < num; i++)
		{
			if (bones[i].transform == transform)
			{
				return bones[i];
			}
		}
		return null;
	}

	public int GetBoneIndex(GPUSkinningBone bone)
	{
		return Array.IndexOf(bones, bone);
	}
}
