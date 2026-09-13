using UnityEngine;

public class STMCharInfo
{
	public CharacterInfo ch;

	public Vector3 pos;

	public int line;

	public float indent;

	public float size;

	public Vector3 TopLeftVert => RelativePos(new Vector3(ch.minX, ch.maxY, 0f));

	public Vector3 TopRightVert => RelativePos(new Vector3(ch.maxX, ch.maxY, 0f));

	public Vector3 BottomRightVert => RelativePos(new Vector3(ch.maxX, ch.minY, 0f));

	public Vector3 BottomLeftVert => RelativePos(new Vector3(ch.minX, ch.minY, 0f));

	public Vector3 Middle => RelativePos(new Vector3((float)(ch.minX + ch.maxX) * 0.5f, (float)(ch.minY + ch.maxY) * 0.5f, 0f));

	public Vector3 RelativePos(Vector3 yeah)
	{
		return pos + yeah * (size / (float)ch.size);
	}

	public Vector3 RelativePos2(Vector3 yeah)
	{
		return pos + yeah * size;
	}

	public Vector3 Advance(float extraSpacing, float myQuality)
	{
		return new Vector3((float)ch.advance + extraSpacing * size, 0f, 0f) * (size / myQuality);
	}

	public STMCharInfo(SuperTextMesh stm)
	{
		Reset(stm);
	}

	public STMCharInfo(STMCharInfo clone, CharacterInfo ch)
	{
		Reset(clone, ch);
	}

	public void Reset(STMCharInfo clone, CharacterInfo ch)
	{
		this.ch = ch;
		pos = clone.pos;
		line = clone.line;
		indent = clone.indent;
		size = clone.size;
	}

	public void Reset(SuperTextMesh stm)
	{
		ch = default(CharacterInfo);
		ch.style = stm.style;
		pos = default(Vector3);
		line = 0;
		indent = 0f;
		size = stm.size;
	}
}
