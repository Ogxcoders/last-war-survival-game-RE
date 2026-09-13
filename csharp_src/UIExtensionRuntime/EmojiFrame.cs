using System;

[Serializable]
internal struct EmojiFrame
{
	public string filename;

	public EmojiRect frame;

	public bool rotated;

	public bool trimmed;

	public EmojiRect spriteSourceSize;

	public EmojiSize sourceSize;

	public EmojiFloat2 pivot;
}
