using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class GPUSkinningUtil
{
	public static string BonesHierarchyTree(GPUSkinningAnimation gpuSkinningAnimation)
	{
		if (gpuSkinningAnimation == null || gpuSkinningAnimation.bones == null)
		{
			return null;
		}
		string str = string.Empty;
		BonesHierarchy_Internal(gpuSkinningAnimation, gpuSkinningAnimation.bones[gpuSkinningAnimation.rootBoneIndex], string.Empty, ref str);
		return str;
	}

	public static void BonesHierarchy_Internal(GPUSkinningAnimation gpuSkinningAnimation, GPUSkinningBone bone, string tabs, ref string str)
	{
		str = str + tabs + bone.name + "\n";
		int num = ((bone.childrenBonesIndices != null) ? bone.childrenBonesIndices.Length : 0);
		for (int i = 0; i < num; i++)
		{
			BonesHierarchy_Internal(gpuSkinningAnimation, gpuSkinningAnimation.bones[bone.childrenBonesIndices[i]], tabs + "    ", ref str);
		}
	}

	public static string BoneHierarchyPath(GPUSkinningBone[] bones, int boneIndex)
	{
		if (bones == null || boneIndex < 0 || boneIndex >= bones.Length)
		{
			return null;
		}
		GPUSkinningBone gPUSkinningBone = bones[boneIndex];
		string text = gPUSkinningBone.name;
		while (gPUSkinningBone.parentBoneIndex != -1)
		{
			gPUSkinningBone = bones[gPUSkinningBone.parentBoneIndex];
			text = gPUSkinningBone.name + "/" + text;
		}
		return text;
	}

	public static string BoneHierarchyPath(GPUSkinningAnimation gpuSkinningAnimation, int boneIndex)
	{
		if (gpuSkinningAnimation == null)
		{
			return null;
		}
		return BoneHierarchyPath(gpuSkinningAnimation.bones, boneIndex);
	}

	public static string MD5(string input)
	{
		MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
		mD5CryptoServiceProvider.Clear();
		string text = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text += array[i].ToString("X").PadLeft(2, '0');
		}
		return text.ToLower();
	}

	public static int NormalizeTimeToFrameIndex(GPUSkinningClip clip, float normalizedTime)
	{
		if (clip == null)
		{
			return 0;
		}
		normalizedTime = Mathf.Clamp01(normalizedTime);
		return (int)(normalizedTime * (clip.length * (float)clip.fps - 1f));
	}

	public static float FrameIndexToNormalizedTime(GPUSkinningClip clip, int frameIndex)
	{
		if (clip == null)
		{
			return 0f;
		}
		int num = (int)((float)clip.fps * clip.length);
		frameIndex = Mathf.Clamp(frameIndex, 0, num - 1);
		return (float)frameIndex / (float)(num - 1);
	}
}
