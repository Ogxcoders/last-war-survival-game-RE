using System;
using UnityEngine.Bindings;

namespace UnityEngine.TextCore;

[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
internal static class TextGeneratorUtilities
{
	public static readonly Vector2 largePositiveVector2 = new Vector2(2.1474836E+09f, 2.1474836E+09f);

	public static readonly Vector2 largeNegativeVector2 = new Vector2(-214748370f, -214748370f);

	public const float largePositiveFloat = 32767f;

	public const float largeNegativeFloat = -32767f;

	public static bool Approximately(float a, float b)
	{
		return b - 0.0001f < a && a < b + 0.0001f;
	}

	public static Color32 HexCharsToColor(char[] hexChars, int tagCount)
	{
		switch (tagCount)
		{
		case 4:
		{
			byte r8 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[1]));
			byte g8 = (byte)(HexToInt(hexChars[2]) * 16 + HexToInt(hexChars[2]));
			byte b8 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[3]));
			return new Color32(r8, g8, b8, byte.MaxValue);
		}
		case 5:
		{
			byte r7 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[1]));
			byte g7 = (byte)(HexToInt(hexChars[2]) * 16 + HexToInt(hexChars[2]));
			byte b7 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[3]));
			byte a4 = (byte)(HexToInt(hexChars[4]) * 16 + HexToInt(hexChars[4]));
			return new Color32(r7, g7, b7, a4);
		}
		case 7:
		{
			byte r6 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[2]));
			byte g6 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[4]));
			byte b6 = (byte)(HexToInt(hexChars[5]) * 16 + HexToInt(hexChars[6]));
			return new Color32(r6, g6, b6, byte.MaxValue);
		}
		case 9:
		{
			byte r5 = (byte)(HexToInt(hexChars[1]) * 16 + HexToInt(hexChars[2]));
			byte g5 = (byte)(HexToInt(hexChars[3]) * 16 + HexToInt(hexChars[4]));
			byte b5 = (byte)(HexToInt(hexChars[5]) * 16 + HexToInt(hexChars[6]));
			byte a3 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[8]));
			return new Color32(r5, g5, b5, a3);
		}
		case 10:
		{
			byte r4 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[7]));
			byte g4 = (byte)(HexToInt(hexChars[8]) * 16 + HexToInt(hexChars[8]));
			byte b4 = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[9]));
			return new Color32(r4, g4, b4, byte.MaxValue);
		}
		case 11:
		{
			byte r3 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[7]));
			byte g3 = (byte)(HexToInt(hexChars[8]) * 16 + HexToInt(hexChars[8]));
			byte b3 = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[9]));
			byte a2 = (byte)(HexToInt(hexChars[10]) * 16 + HexToInt(hexChars[10]));
			return new Color32(r3, g3, b3, a2);
		}
		case 13:
		{
			byte r2 = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[8]));
			byte g2 = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[10]));
			byte b2 = (byte)(HexToInt(hexChars[11]) * 16 + HexToInt(hexChars[12]));
			return new Color32(r2, g2, b2, byte.MaxValue);
		}
		case 15:
		{
			byte r = (byte)(HexToInt(hexChars[7]) * 16 + HexToInt(hexChars[8]));
			byte g = (byte)(HexToInt(hexChars[9]) * 16 + HexToInt(hexChars[10]));
			byte b = (byte)(HexToInt(hexChars[11]) * 16 + HexToInt(hexChars[12]));
			byte a = (byte)(HexToInt(hexChars[13]) * 16 + HexToInt(hexChars[14]));
			return new Color32(r, g, b, a);
		}
		default:
			return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
	}

	public static Color32 HexCharsToColor(char[] hexChars, int startIndex, int length)
	{
		switch (length)
		{
		case 7:
		{
			byte r2 = (byte)(HexToInt(hexChars[startIndex + 1]) * 16 + HexToInt(hexChars[startIndex + 2]));
			byte g2 = (byte)(HexToInt(hexChars[startIndex + 3]) * 16 + HexToInt(hexChars[startIndex + 4]));
			byte b2 = (byte)(HexToInt(hexChars[startIndex + 5]) * 16 + HexToInt(hexChars[startIndex + 6]));
			return new Color32(r2, g2, b2, byte.MaxValue);
		}
		case 9:
		{
			byte r = (byte)(HexToInt(hexChars[startIndex + 1]) * 16 + HexToInt(hexChars[startIndex + 2]));
			byte g = (byte)(HexToInt(hexChars[startIndex + 3]) * 16 + HexToInt(hexChars[startIndex + 4]));
			byte b = (byte)(HexToInt(hexChars[startIndex + 5]) * 16 + HexToInt(hexChars[startIndex + 6]));
			byte a = (byte)(HexToInt(hexChars[startIndex + 7]) * 16 + HexToInt(hexChars[startIndex + 8]));
			return new Color32(r, g, b, a);
		}
		default:
			return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
	}

	public static int HexToInt(char hex)
	{
		return hex switch
		{
			'0' => 0, 
			'1' => 1, 
			'2' => 2, 
			'3' => 3, 
			'4' => 4, 
			'5' => 5, 
			'6' => 6, 
			'7' => 7, 
			'8' => 8, 
			'9' => 9, 
			'A' => 10, 
			'B' => 11, 
			'C' => 12, 
			'D' => 13, 
			'E' => 14, 
			'F' => 15, 
			'a' => 10, 
			'b' => 11, 
			'c' => 12, 
			'd' => 13, 
			'e' => 14, 
			'f' => 15, 
			_ => 15, 
		};
	}

	public static float ConvertToFloat(char[] chars, int startIndex, int length)
	{
		int lastIndex;
		return ConvertToFloat(chars, startIndex, length, out lastIndex);
	}

	public static float ConvertToFloat(char[] chars, int startIndex, int length, out int lastIndex)
	{
		if (startIndex == 0)
		{
			lastIndex = 0;
			return -32767f;
		}
		int num = startIndex + length;
		bool flag = true;
		float num2 = 0f;
		int num3 = 1;
		if (chars[startIndex] == '+')
		{
			num3 = 1;
			startIndex++;
		}
		else if (chars[startIndex] == '-')
		{
			num3 = -1;
			startIndex++;
		}
		float num4 = 0f;
		for (int i = startIndex; i < num; i++)
		{
			uint num5 = chars[i];
			if ((num5 >= 48 && num5 <= 57) || num5 == 46)
			{
				if (num5 == 46)
				{
					flag = false;
					num2 = 0.1f;
				}
				else if (flag)
				{
					num4 = num4 * 10f + (float)((num5 - 48) * num3);
				}
				else
				{
					num4 += (float)(num5 - 48) * num2 * (float)num3;
					num2 *= 0.1f;
				}
			}
			else if (num5 == 44)
			{
				if (i + 1 < num && chars[i + 1] == ' ')
				{
					lastIndex = i + 1;
				}
				else
				{
					lastIndex = i;
				}
				return num4;
			}
		}
		lastIndex = num;
		return num4;
	}

	public static Vector2 PackUV(float x, float y, float scale)
	{
		Vector2 result = default(Vector2);
		result.x = (int)(x * 511f);
		result.y = (int)(y * 511f);
		result.x = result.x * 4096f + result.y;
		result.y = scale;
		return result;
	}

	public static void StringToCharArray(string sourceText, ref int[] charBuffer, ref RichTextTagStack<int> styleStack, TextGenerationSettings generationSettings)
	{
		if (sourceText == null)
		{
			charBuffer[0] = 0;
			return;
		}
		if (charBuffer == null)
		{
			charBuffer = new int[8];
		}
		styleStack.SetDefault(0);
		int writeIndex = 0;
		for (int i = 0; i < sourceText.Length; i++)
		{
			if (sourceText[i] == '\\' && sourceText.Length > i + 1)
			{
				switch (sourceText[i + 1])
				{
				case 85:
					if (sourceText.Length > i + 9)
					{
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = GetUtf32(sourceText, i + 2);
						i += 9;
						writeIndex++;
						continue;
					}
					break;
				case 92:
					if (!generationSettings.parseControlCharacters || sourceText.Length <= i + 2)
					{
						break;
					}
					if (writeIndex + 2 > charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex] = sourceText[i + 1];
					charBuffer[writeIndex + 1] = sourceText[i + 2];
					i += 2;
					writeIndex += 2;
					continue;
				case 110:
					if (!generationSettings.parseControlCharacters)
					{
						break;
					}
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex] = 10;
					i++;
					writeIndex++;
					continue;
				case 114:
					if (!generationSettings.parseControlCharacters)
					{
						break;
					}
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex] = 13;
					i++;
					writeIndex++;
					continue;
				case 116:
					if (!generationSettings.parseControlCharacters)
					{
						break;
					}
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex] = 9;
					i++;
					writeIndex++;
					continue;
				case 117:
					if (sourceText.Length > i + 5)
					{
						if (writeIndex == charBuffer.Length)
						{
							ResizeInternalArray(ref charBuffer);
						}
						charBuffer[writeIndex] = (ushort)GetUtf16(sourceText, i + 2);
						i += 5;
						writeIndex++;
						continue;
					}
					break;
				}
			}
			if (char.IsHighSurrogate(sourceText[i]) && char.IsLowSurrogate(sourceText[i + 1]))
			{
				if (writeIndex == charBuffer.Length)
				{
					ResizeInternalArray(ref charBuffer);
				}
				charBuffer[writeIndex] = char.ConvertToUtf32(sourceText[i], sourceText[i + 1]);
				i++;
				writeIndex++;
				continue;
			}
			if (sourceText[i] == '<' && generationSettings.richText)
			{
				if (IsTagName(ref sourceText, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
					continue;
				}
				if (IsTagName(ref sourceText, "<STYLE=", i))
				{
					if (ReplaceOpeningStyleTag(ref sourceText, i, out var srcOffset, ref charBuffer, ref writeIndex, ref styleStack))
					{
						i = srcOffset;
						continue;
					}
				}
				else if (IsTagName(ref sourceText, "</STYLE>", i))
				{
					ReplaceClosingStyleTag(ref charBuffer, ref writeIndex, ref styleStack);
					i += 7;
					continue;
				}
			}
			if (writeIndex == charBuffer.Length)
			{
				ResizeInternalArray(ref charBuffer);
			}
			charBuffer[writeIndex] = sourceText[i];
			writeIndex++;
		}
		if (writeIndex == charBuffer.Length)
		{
			ResizeInternalArray(ref charBuffer);
		}
		charBuffer[writeIndex] = 0;
	}

	private static void ResizeInternalArray<T>(ref T[] array)
	{
		int newSize = Mathf.NextPowerOfTwo(array.Length + 1);
		Array.Resize(ref array, newSize);
	}

	internal static void ResizeArray<T>(T[] array)
	{
		int num = array.Length * 2;
		if (num == 0)
		{
			num = 8;
		}
		Array.Resize(ref array, num);
	}

	private static bool IsTagName(ref string text, string tag, int index)
	{
		if (text.Length < index + tag.Length)
		{
			return false;
		}
		for (int i = 0; i < tag.Length; i++)
		{
			if (TextUtilities.ToUpperFast(text[index + i]) != tag[i])
			{
				return false;
			}
		}
		return true;
	}

	private static bool IsTagName(ref int[] text, string tag, int index)
	{
		if (text.Length < index + tag.Length)
		{
			return false;
		}
		for (int i = 0; i < tag.Length; i++)
		{
			if (TextUtilities.ToUpperFast((char)text[index + i]) != tag[i])
			{
				return false;
			}
		}
		return true;
	}

	private static bool ReplaceOpeningStyleTag(ref int[] sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex, ref RichTextTagStack<int> styleStack)
	{
		int tagHashCode = GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset);
		TextStyle style = TextStyleSheet.GetStyle(tagHashCode);
		if (style == null || srcOffset == 0)
		{
			return false;
		}
		styleStack.Add(style.hashCode);
		int num = style.styleOpeningTagArray.Length;
		int[] text = style.styleOpeningTagArray;
		for (int i = 0; i < num; i++)
		{
			int num2 = text[i];
			if (num2 == 60)
			{
				if (IsTagName(ref text, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
					continue;
				}
				if (IsTagName(ref text, "<STYLE=", i))
				{
					if (ReplaceOpeningStyleTag(ref text, i, out var srcOffset2, ref charBuffer, ref writeIndex, ref styleStack))
					{
						i = srcOffset2;
						continue;
					}
				}
				else if (IsTagName(ref text, "</STYLE>", i))
				{
					ReplaceClosingStyleTag(ref charBuffer, ref writeIndex, ref styleStack);
					i += 7;
					continue;
				}
			}
			if (writeIndex == charBuffer.Length)
			{
				ResizeInternalArray(ref charBuffer);
			}
			charBuffer[writeIndex] = num2;
			writeIndex++;
		}
		return true;
	}

	private static bool ReplaceOpeningStyleTag(ref string sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex, ref RichTextTagStack<int> styleStack)
	{
		int tagHashCode = GetTagHashCode(ref sourceText, srcIndex + 7, out srcOffset);
		TextStyle style = TextStyleSheet.GetStyle(tagHashCode);
		if (style == null || srcOffset == 0)
		{
			return false;
		}
		styleStack.Add(style.hashCode);
		int num = style.styleOpeningTagArray.Length;
		int[] text = style.styleOpeningTagArray;
		for (int i = 0; i < num; i++)
		{
			int num2 = text[i];
			if (num2 == 60)
			{
				if (IsTagName(ref text, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
					continue;
				}
				if (IsTagName(ref text, "<STYLE=", i))
				{
					if (ReplaceOpeningStyleTag(ref text, i, out var srcOffset2, ref charBuffer, ref writeIndex, ref styleStack))
					{
						i = srcOffset2;
						continue;
					}
				}
				else if (IsTagName(ref text, "</STYLE>", i))
				{
					ReplaceClosingStyleTag(ref charBuffer, ref writeIndex, ref styleStack);
					i += 7;
					continue;
				}
			}
			if (writeIndex == charBuffer.Length)
			{
				ResizeInternalArray(ref charBuffer);
			}
			charBuffer[writeIndex] = num2;
			writeIndex++;
		}
		return true;
	}

	private static void ReplaceClosingStyleTag(ref int[] charBuffer, ref int writeIndex, ref RichTextTagStack<int> styleStack)
	{
		int hashCode = styleStack.CurrentItem();
		TextStyle style = TextStyleSheet.GetStyle(hashCode);
		styleStack.Remove();
		if (style == null)
		{
			return;
		}
		int num = style.styleClosingTagArray.Length;
		int[] text = style.styleClosingTagArray;
		for (int i = 0; i < num; i++)
		{
			int num2 = text[i];
			if (num2 == 60)
			{
				if (IsTagName(ref text, "<BR>", i))
				{
					if (writeIndex == charBuffer.Length)
					{
						ResizeInternalArray(ref charBuffer);
					}
					charBuffer[writeIndex] = 10;
					writeIndex++;
					i += 3;
					continue;
				}
				if (IsTagName(ref text, "<STYLE=", i))
				{
					if (ReplaceOpeningStyleTag(ref text, i, out var srcOffset, ref charBuffer, ref writeIndex, ref styleStack))
					{
						i = srcOffset;
						continue;
					}
				}
				else if (IsTagName(ref text, "</STYLE>", i))
				{
					ReplaceClosingStyleTag(ref charBuffer, ref writeIndex, ref styleStack);
					i += 7;
					continue;
				}
			}
			if (writeIndex == charBuffer.Length)
			{
				ResizeInternalArray(ref charBuffer);
			}
			charBuffer[writeIndex] = num2;
			writeIndex++;
		}
	}

	private static int GetUtf32(string text, int i)
	{
		int num = 0;
		num += HexToInt(text[i]) << 30;
		num += HexToInt(text[i + 1]) << 24;
		num += HexToInt(text[i + 2]) << 20;
		num += HexToInt(text[i + 3]) << 16;
		num += HexToInt(text[i + 4]) << 12;
		num += HexToInt(text[i + 5]) << 8;
		num += HexToInt(text[i + 6]) << 4;
		return num + HexToInt(text[i + 7]);
	}

	private static int GetUtf16(string text, int i)
	{
		int num = 0;
		num += HexToInt(text[i]) << 12;
		num += HexToInt(text[i + 1]) << 8;
		num += HexToInt(text[i + 2]) << 4;
		return num + HexToInt(text[i + 3]);
	}

	private static int GetTagHashCode(ref int[] text, int index, out int closeIndex)
	{
		int num = 0;
		closeIndex = 0;
		for (int i = index; i < text.Length; i++)
		{
			if (text[i] != 34)
			{
				if (text[i] == 62)
				{
					closeIndex = i;
					break;
				}
				num = ((num << 5) + num) ^ (int)TextUtilities.ToUpperASCIIFast((ushort)text[i]);
			}
		}
		return num;
	}

	private static int GetTagHashCode(ref string text, int index, out int closeIndex)
	{
		int num = 0;
		closeIndex = 0;
		for (int i = index; i < text.Length; i++)
		{
			if (text[i] != '"')
			{
				if (text[i] == '>')
				{
					closeIndex = i;
					break;
				}
				num = ((num << 5) + num) ^ (int)TextUtilities.ToUpperASCIIFast(text[i]);
			}
		}
		return num;
	}

	public static void FillCharacterVertexBuffers(int i, TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		int materialReferenceIndex = textInfo.textElementInfo[i].materialReferenceIndex;
		int vertexCount = textInfo.meshInfo[materialReferenceIndex].vertexCount;
		TextElementInfo[] textElementInfo = textInfo.textElementInfo;
		textInfo.textElementInfo[i].vertexIndex = vertexCount;
		if (generationSettings.inverseYAxis)
		{
			Vector3 vector = default(Vector3);
			vector.x = 0f;
			vector.y = generationSettings.screenRect.y + generationSettings.screenRect.height;
			vector.z = 0f;
			Vector3 position = textElementInfo[i].vertexBottomLeft.position;
			position.y *= -1f;
			textInfo.meshInfo[materialReferenceIndex].vertices[vertexCount] = position + vector;
			position = textElementInfo[i].vertexTopLeft.position;
			position.y *= -1f;
			textInfo.meshInfo[materialReferenceIndex].vertices[1 + vertexCount] = position + vector;
			position = textElementInfo[i].vertexTopRight.position;
			position.y *= -1f;
			textInfo.meshInfo[materialReferenceIndex].vertices[2 + vertexCount] = position + vector;
			position = textElementInfo[i].vertexBottomRight.position;
			position.y *= -1f;
			textInfo.meshInfo[materialReferenceIndex].vertices[3 + vertexCount] = position + vector;
		}
		else
		{
			textInfo.meshInfo[materialReferenceIndex].vertices[vertexCount] = textElementInfo[i].vertexBottomLeft.position;
			textInfo.meshInfo[materialReferenceIndex].vertices[1 + vertexCount] = textElementInfo[i].vertexTopLeft.position;
			textInfo.meshInfo[materialReferenceIndex].vertices[2 + vertexCount] = textElementInfo[i].vertexTopRight.position;
			textInfo.meshInfo[materialReferenceIndex].vertices[3 + vertexCount] = textElementInfo[i].vertexBottomRight.position;
		}
		textInfo.meshInfo[materialReferenceIndex].uvs0[vertexCount] = textElementInfo[i].vertexBottomLeft.uv;
		textInfo.meshInfo[materialReferenceIndex].uvs0[1 + vertexCount] = textElementInfo[i].vertexTopLeft.uv;
		textInfo.meshInfo[materialReferenceIndex].uvs0[2 + vertexCount] = textElementInfo[i].vertexTopRight.uv;
		textInfo.meshInfo[materialReferenceIndex].uvs0[3 + vertexCount] = textElementInfo[i].vertexBottomRight.uv;
		textInfo.meshInfo[materialReferenceIndex].uvs2[vertexCount] = textElementInfo[i].vertexBottomLeft.uv2;
		textInfo.meshInfo[materialReferenceIndex].uvs2[1 + vertexCount] = textElementInfo[i].vertexTopLeft.uv2;
		textInfo.meshInfo[materialReferenceIndex].uvs2[2 + vertexCount] = textElementInfo[i].vertexTopRight.uv2;
		textInfo.meshInfo[materialReferenceIndex].uvs2[3 + vertexCount] = textElementInfo[i].vertexBottomRight.uv2;
		textInfo.meshInfo[materialReferenceIndex].colors32[vertexCount] = textElementInfo[i].vertexBottomLeft.color;
		textInfo.meshInfo[materialReferenceIndex].colors32[1 + vertexCount] = textElementInfo[i].vertexTopLeft.color;
		textInfo.meshInfo[materialReferenceIndex].colors32[2 + vertexCount] = textElementInfo[i].vertexTopRight.color;
		textInfo.meshInfo[materialReferenceIndex].colors32[3 + vertexCount] = textElementInfo[i].vertexBottomRight.color;
		textInfo.meshInfo[materialReferenceIndex].vertexCount = vertexCount + 4;
	}

	public static void FillSpriteVertexBuffers(int i, TextGenerationSettings generationSettings, TextInfo textInfo)
	{
		int materialReferenceIndex = textInfo.textElementInfo[i].materialReferenceIndex;
		int vertexCount = textInfo.meshInfo[materialReferenceIndex].vertexCount;
		TextElementInfo[] textElementInfo = textInfo.textElementInfo;
		textInfo.textElementInfo[i].vertexIndex = vertexCount;
		if (generationSettings.inverseYAxis)
		{
			Vector3 vector = default(Vector3);
			vector.x = 0f;
			vector.y = generationSettings.screenRect.y + generationSettings.screenRect.height;
			vector.z = 0f;
			Vector3 position = textElementInfo[i].vertexBottomLeft.position;
			position.y *= -1f;
			textInfo.meshInfo[materialReferenceIndex].vertices[vertexCount] = position + vector;
			position = textElementInfo[i].vertexTopLeft.position;
			position.y *= -1f;
			textInfo.meshInfo[materialReferenceIndex].vertices[1 + vertexCount] = position + vector;
			position = textElementInfo[i].vertexTopRight.position;
			position.y *= -1f;
			textInfo.meshInfo[materialReferenceIndex].vertices[2 + vertexCount] = position + vector;
			position = textElementInfo[i].vertexBottomRight.position;
			position.y *= -1f;
			textInfo.meshInfo[materialReferenceIndex].vertices[3 + vertexCount] = position + vector;
		}
		else
		{
			textInfo.meshInfo[materialReferenceIndex].vertices[vertexCount] = textElementInfo[i].vertexBottomLeft.position;
			textInfo.meshInfo[materialReferenceIndex].vertices[1 + vertexCount] = textElementInfo[i].vertexTopLeft.position;
			textInfo.meshInfo[materialReferenceIndex].vertices[2 + vertexCount] = textElementInfo[i].vertexTopRight.position;
			textInfo.meshInfo[materialReferenceIndex].vertices[3 + vertexCount] = textElementInfo[i].vertexBottomRight.position;
		}
		textInfo.meshInfo[materialReferenceIndex].uvs0[vertexCount] = textElementInfo[i].vertexBottomLeft.uv;
		textInfo.meshInfo[materialReferenceIndex].uvs0[1 + vertexCount] = textElementInfo[i].vertexTopLeft.uv;
		textInfo.meshInfo[materialReferenceIndex].uvs0[2 + vertexCount] = textElementInfo[i].vertexTopRight.uv;
		textInfo.meshInfo[materialReferenceIndex].uvs0[3 + vertexCount] = textElementInfo[i].vertexBottomRight.uv;
		textInfo.meshInfo[materialReferenceIndex].uvs2[vertexCount] = textElementInfo[i].vertexBottomLeft.uv2;
		textInfo.meshInfo[materialReferenceIndex].uvs2[1 + vertexCount] = textElementInfo[i].vertexTopLeft.uv2;
		textInfo.meshInfo[materialReferenceIndex].uvs2[2 + vertexCount] = textElementInfo[i].vertexTopRight.uv2;
		textInfo.meshInfo[materialReferenceIndex].uvs2[3 + vertexCount] = textElementInfo[i].vertexBottomRight.uv2;
		textInfo.meshInfo[materialReferenceIndex].colors32[vertexCount] = textElementInfo[i].vertexBottomLeft.color;
		textInfo.meshInfo[materialReferenceIndex].colors32[1 + vertexCount] = textElementInfo[i].vertexTopLeft.color;
		textInfo.meshInfo[materialReferenceIndex].colors32[2 + vertexCount] = textElementInfo[i].vertexTopRight.color;
		textInfo.meshInfo[materialReferenceIndex].colors32[3 + vertexCount] = textElementInfo[i].vertexBottomRight.color;
		textInfo.meshInfo[materialReferenceIndex].vertexCount = vertexCount + 4;
	}

	public static void AdjustLineOffset(int startIndex, int endIndex, float offset, TextInfo textInfo)
	{
		Vector3 vector = new Vector3(0f, offset, 0f);
		for (int i = startIndex; i <= endIndex; i++)
		{
			textInfo.textElementInfo[i].bottomLeft -= vector;
			textInfo.textElementInfo[i].topLeft -= vector;
			textInfo.textElementInfo[i].topRight -= vector;
			textInfo.textElementInfo[i].bottomRight -= vector;
			textInfo.textElementInfo[i].ascender -= vector.y;
			textInfo.textElementInfo[i].baseLine -= vector.y;
			textInfo.textElementInfo[i].descender -= vector.y;
			if (textInfo.textElementInfo[i].isVisible)
			{
				textInfo.textElementInfo[i].vertexBottomLeft.position -= vector;
				textInfo.textElementInfo[i].vertexTopLeft.position -= vector;
				textInfo.textElementInfo[i].vertexTopRight.position -= vector;
				textInfo.textElementInfo[i].vertexBottomRight.position -= vector;
			}
		}
	}

	public static void ResizeLineExtents(int size, TextInfo textInfo)
	{
		size = ((size > 1024) ? (size + 256) : Mathf.NextPowerOfTwo(size + 1));
		LineInfo[] array = new LineInfo[size];
		for (int i = 0; i < size; i++)
		{
			if (i < textInfo.lineInfo.Length)
			{
				array[i] = textInfo.lineInfo[i];
				continue;
			}
			array[i].lineExtents.min = largePositiveVector2;
			array[i].lineExtents.max = largeNegativeVector2;
			array[i].ascender = -32767f;
			array[i].descender = 32767f;
		}
		textInfo.lineInfo = array;
	}

	public static FontStyles LegacyStyleToNewStyle(FontStyle fontStyle)
	{
		return fontStyle switch
		{
			FontStyle.Bold => FontStyles.Bold, 
			FontStyle.Italic => FontStyles.Italic, 
			FontStyle.BoldAndItalic => FontStyles.Bold | FontStyles.Italic, 
			_ => FontStyles.Normal, 
		};
	}

	public static TextAlignment LegacyAlignmentToNewAlignment(TextAnchor anchor)
	{
		return anchor switch
		{
			TextAnchor.UpperLeft => TextAlignment.TopLeft, 
			TextAnchor.UpperCenter => TextAlignment.TopCenter, 
			TextAnchor.UpperRight => TextAlignment.TopRight, 
			TextAnchor.MiddleLeft => TextAlignment.MiddleLeft, 
			TextAnchor.MiddleCenter => TextAlignment.MiddleCenter, 
			TextAnchor.MiddleRight => TextAlignment.MiddleRight, 
			TextAnchor.LowerLeft => TextAlignment.BottomLeft, 
			TextAnchor.LowerCenter => TextAlignment.BottomCenter, 
			TextAnchor.LowerRight => TextAlignment.BottomRight, 
			_ => TextAlignment.TopLeft, 
		};
	}
}
