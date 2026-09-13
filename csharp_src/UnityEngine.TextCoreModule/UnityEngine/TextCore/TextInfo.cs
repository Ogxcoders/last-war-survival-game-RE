using System;
using UnityEngine.Bindings;

namespace UnityEngine.TextCore;

[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
internal class TextInfo
{
	private static Vector2 s_InfinityVectorPositive = new Vector2(32767f, 32767f);

	private static Vector2 s_InfinityVectorNegative = new Vector2(-32767f, -32767f);

	public int characterCount;

	public int spriteCount;

	public int spaceCount;

	public int wordCount;

	public int linkCount;

	public int lineCount;

	public int pageCount;

	public int materialCount;

	public TextElementInfo[] textElementInfo;

	public WordInfo[] wordInfo;

	public LinkInfo[] linkInfo;

	public LineInfo[] lineInfo;

	public PageInfo[] pageInfo;

	public MeshInfo[] meshInfo;

	public bool isDirty;

	public TextInfo()
	{
		textElementInfo = new TextElementInfo[8];
		wordInfo = new WordInfo[16];
		linkInfo = new LinkInfo[0];
		lineInfo = new LineInfo[2];
		pageInfo = new PageInfo[4];
		meshInfo = new MeshInfo[1];
		materialCount = 0;
		isDirty = true;
	}

	internal void Clear()
	{
		characterCount = 0;
		spaceCount = 0;
		wordCount = 0;
		linkCount = 0;
		lineCount = 0;
		pageCount = 0;
		spriteCount = 0;
		for (int i = 0; i < meshInfo.Length; i++)
		{
			meshInfo[i].vertexCount = 0;
		}
	}

	internal void ClearMeshInfo(bool updateMesh)
	{
		for (int i = 0; i < meshInfo.Length; i++)
		{
			meshInfo[i].Clear(updateMesh);
		}
	}

	internal void ClearLineInfo()
	{
		if (lineInfo == null)
		{
			lineInfo = new LineInfo[2];
		}
		for (int i = 0; i < lineInfo.Length; i++)
		{
			lineInfo[i].characterCount = 0;
			lineInfo[i].spaceCount = 0;
			lineInfo[i].wordCount = 0;
			lineInfo[i].controlCharacterCount = 0;
			lineInfo[i].width = 0f;
			lineInfo[i].ascender = s_InfinityVectorNegative.x;
			lineInfo[i].descender = s_InfinityVectorPositive.x;
			lineInfo[i].lineExtents.min = s_InfinityVectorPositive;
			lineInfo[i].lineExtents.max = s_InfinityVectorNegative;
			lineInfo[i].maxAdvance = 0f;
		}
	}

	internal static void Resize<T>(ref T[] array, int size)
	{
		int newSize = ((size > 1024) ? (size + 256) : Mathf.NextPowerOfTwo(size));
		Array.Resize(ref array, newSize);
	}

	internal static void Resize<T>(ref T[] array, int size, bool isBlockAllocated)
	{
		if (isBlockAllocated)
		{
			size = ((size > 1024) ? (size + 256) : Mathf.NextPowerOfTwo(size));
		}
		if (size != array.Length)
		{
			Array.Resize(ref array, size);
		}
	}
}
