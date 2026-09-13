using System;
using System.Collections.Generic;
using UnityEngine;

public class GPUSkinAnimTexData : ScriptableObject
{
	[Serializable]
	public class ClipData
	{
		public string name;

		public int startFrameRow;

		public int totalFrame;

		public bool isLooping;

		public float frameRate;

		[NonSerialized]
		public int nameHash;

		public float duration => (float)(totalFrame - 1) / frameRate;

		public ClipData Clone()
		{
			return new ClipData
			{
				name = name,
				startFrameRow = startFrameRow,
				totalFrame = totalFrame,
				isLooping = isLooping,
				nameHash = nameHash,
				frameRate = frameRate
			};
		}
	}

	public List<ClipData> ClipDatas = new List<ClipData>();

	public int texHeight;

	public Texture2D animTex;

	public List<Material> animMats = new List<Material>();

	public Mesh animMesh;

	public float totalDuration
	{
		get
		{
			float num = 0f;
			foreach (ClipData clipData in ClipDatas)
			{
				num += clipData.duration;
			}
			return num;
		}
	}
}
