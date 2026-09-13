namespace UnityEngine;

public struct TextGenerationSettings
{
	public Font font;

	public Color color;

	public int fontSize;

	public float lineSpacing;

	public bool richText;

	public float scaleFactor;

	public FontStyle fontStyle;

	public TextAnchor textAnchor;

	public bool alignByGeometry;

	public bool resizeTextForBestFit;

	public int resizeTextMinSize;

	public int resizeTextMaxSize;

	public bool updateBounds;

	public VerticalWrapMode verticalOverflow;

	public HorizontalWrapMode horizontalOverflow;

	public Vector2 generationExtents;

	public Vector2 pivot;

	public bool generateOutOfBounds;

	private bool CompareColors(Color left, Color right)
	{
		return Mathf.Approximately(left.r, right.r) && Mathf.Approximately(left.g, right.g) && Mathf.Approximately(left.b, right.b) && Mathf.Approximately(left.a, right.a);
	}

	private bool CompareVector2(Vector2 left, Vector2 right)
	{
		return Mathf.Approximately(left.x, right.x) && Mathf.Approximately(left.y, right.y);
	}

	public bool Equals(TextGenerationSettings other)
	{
		return CompareColors(color, other.color) && fontSize == other.fontSize && Mathf.Approximately(scaleFactor, other.scaleFactor) && resizeTextMinSize == other.resizeTextMinSize && resizeTextMaxSize == other.resizeTextMaxSize && Mathf.Approximately(lineSpacing, other.lineSpacing) && fontStyle == other.fontStyle && richText == other.richText && textAnchor == other.textAnchor && alignByGeometry == other.alignByGeometry && resizeTextForBestFit == other.resizeTextForBestFit && resizeTextMinSize == other.resizeTextMinSize && resizeTextMaxSize == other.resizeTextMaxSize && resizeTextForBestFit == other.resizeTextForBestFit && updateBounds == other.updateBounds && horizontalOverflow == other.horizontalOverflow && verticalOverflow == other.verticalOverflow && CompareVector2(generationExtents, other.generationExtents) && CompareVector2(pivot, other.pivot) && font == other.font;
	}
}
