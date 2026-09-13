namespace TMPro;

public struct GlyphPairKey
{
	public uint firstGlyphIndex;

	public uint secondGlyphIndex;

	public uint key;

	public GlyphPairKey(uint firstGlyphIndex, uint secondGlyphIndex)
	{
		this.firstGlyphIndex = firstGlyphIndex;
		this.secondGlyphIndex = secondGlyphIndex;
		key = (secondGlyphIndex << 16) | firstGlyphIndex;
	}

	internal GlyphPairKey(TMP_GlyphPairAdjustmentRecord record)
	{
		firstGlyphIndex = record.firstAdjustmentRecord.glyphIndex;
		secondGlyphIndex = record.secondAdjustmentRecord.glyphIndex;
		key = (secondGlyphIndex << 16) | firstGlyphIndex;
	}
}
