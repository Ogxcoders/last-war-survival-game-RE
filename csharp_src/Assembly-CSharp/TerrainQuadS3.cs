using System.Collections.Generic;
using UnityEngine;

public class TerrainQuadS3 : TerrainQuadSeason<QuadCellS3>
{
	public const float ANIM_DURATION = 1f;

	private static QuadCellS3 zero;

	private float _transitEndTime;

	protected override QuadCellS3 DefaultStateValue()
	{
		return zero;
	}

	protected override void BeforeSetup()
	{
		base.passId = TerrainQuadLayerRenderer.RenderPass_S3;
		blockSize = 32;
		minCellSize = 1;
		_transitEndTime = Time.timeSinceLevelLoad;
	}

	public override void SetTransit()
	{
		_transitEndTime = Time.timeSinceLevelLoad + 1f;
	}

	protected override void OnChangeMipmap0(int index, ref QuadCellS3 last, ref QuadCellS3 current)
	{
		if (Mathf.Abs(current.layer0 - last.layer0) > 0.5f && current.time > 0f)
		{
			current.layer0Prev = last.layer0;
		}
	}

	protected override QuadCellS3 CalcMipmap(int xPos, int yPos, int mipmapLevel)
	{
		int num = xPos * 2;
		int num2 = yPos * 2;
		QuadCellS3 mipmapData = GetMipmapData(num, num2, mipmapLevel - 1);
		QuadCellS3 mipmapData2 = GetMipmapData(num + 1, num2, mipmapLevel - 1);
		QuadCellS3 mipmapData3 = GetMipmapData(num, num2 + 1, mipmapLevel - 1);
		QuadCellS3 mipmapData4 = GetMipmapData(num + 1, num2 + 1, mipmapLevel - 1);
		return new QuadCellS3((mipmapData.layer0 + mipmapData2.layer0 + mipmapData3.layer0 + mipmapData4.layer0) / 4f, (mipmapData.layer1 + mipmapData2.layer1 + mipmapData3.layer1 + mipmapData4.layer1) / 4f);
	}

	protected override bool InTransit()
	{
		return _transitEndTime > Time.timeSinceLevelLoad;
	}

	protected override void BuildItems()
	{
		float timeSinceLevelLoad = Time.timeSinceLevelLoad;
		_MipMap mipMap = _mipmaps[base.showMipmapLevel];
		int num = blockSize / (int)Mathf.Pow(2f, base.showMipmapLevel);
		int num2 = _lastBlockLB.x * num;
		int num3 = _lastBlockLB.y * num;
		int num4 = (_lastBlockRT.x + 1) * num - 1;
		int num5 = (_lastBlockRT.y + 1) * num - 1;
		int num6 = -1;
		for (int i = num3; i <= num5; i++)
		{
			for (int j = num2; j <= num4; j++)
			{
				int key = (j << 16) | (i & 0xFFFF);
				if (mipMap.data.TryGetValue(key, out var value))
				{
					QuadCellS3 quadCellS = value;
					int num7 = j;
					int num8 = i;
					int num9 = num7 * mipMap.quadCellWidth;
					int num10 = num8 * mipMap.quadCellWidth;
					_MatrixPoolItem matrixPoolItem = _pool[_cursor];
					ref Matrix4x4 reference = ref matrixPoolItem.matrix[++num6];
					matrixPoolItem.length++;
					reference.m03 = num9;
					reference.m13 = mipMap.yOffset;
					reference.m23 = num10;
					reference.m00 = _quadSize;
					reference.m01 = quadCellS.time;
					QuadCellS3 mipmapData = GetMipmapData(num7 - 1, num8, mipMap);
					QuadCellS3 mipmapData2 = GetMipmapData(num7, num8 + 1, mipMap);
					QuadCellS3 mipmapData3 = GetMipmapData(num7 + 1, num8, mipMap);
					QuadCellS3 mipmapData4 = GetMipmapData(num7, num8 - 1, mipMap);
					QuadCellS3 mipmapData5 = GetMipmapData(num7 - 1, num8 - 1, mipMap);
					QuadCellS3 mipmapData6 = GetMipmapData(num7 + 1, num8 - 1, mipMap);
					QuadCellS3 mipmapData7 = GetMipmapData(num7 - 1, num8 + 1, mipMap);
					QuadCellS3 mipmapData8 = GetMipmapData(num7 + 1, num8 + 1, mipMap);
					reference.m02 = (mipmapData.Layer0Value(timeSinceLevelLoad) + mipmapData4.Layer0Value(timeSinceLevelLoad) + mipmapData5.Layer0Value(timeSinceLevelLoad) + quadCellS.Layer0Value(timeSinceLevelLoad)) / 4f;
					reference.m10 = (mipmapData3.Layer0Value(timeSinceLevelLoad) + mipmapData4.Layer0Value(timeSinceLevelLoad) + mipmapData6.Layer0Value(timeSinceLevelLoad) + quadCellS.Layer0Value(timeSinceLevelLoad)) / 4f;
					reference.m11 = (mipmapData.Layer0Value(timeSinceLevelLoad) + mipmapData2.Layer0Value(timeSinceLevelLoad) + mipmapData7.Layer0Value(timeSinceLevelLoad) + quadCellS.Layer0Value(timeSinceLevelLoad)) / 4f;
					reference.m12 = (mipmapData3.Layer0Value(timeSinceLevelLoad) + mipmapData2.Layer0Value(timeSinceLevelLoad) + mipmapData8.Layer0Value(timeSinceLevelLoad) + quadCellS.Layer0Value(timeSinceLevelLoad)) / 4f;
					reference.m20 = (mipmapData.layer1 + mipmapData4.layer1 + mipmapData5.layer1 + quadCellS.layer1) / 4f;
					reference.m21 = (mipmapData3.layer1 + mipmapData4.layer1 + mipmapData6.layer1 + quadCellS.layer1) / 4f;
					reference.m22 = (mipmapData.layer1 + mipmapData2.layer1 + mipmapData7.layer1 + quadCellS.layer1) / 4f;
					reference.m30 = (mipmapData3.layer1 + mipmapData2.layer1 + mipmapData8.layer1 + quadCellS.layer1) / 4f;
					if (num6 >= 510)
					{
						SetPoolCursor(_cursor + 1);
						num6 = -1;
					}
				}
			}
		}
	}

	protected override void DrawMipmapGizmos(_MipMap mipMap, GUIStyle style)
	{
		using Dictionary<int, QuadCellS3>.Enumerator enumerator = mipMap.data.GetEnumerator();
		while (enumerator.MoveNext())
		{
			int key = enumerator.Current.Key;
			QuadCellS3 value = enumerator.Current.Value;
			int num = key >> 16;
			short num2 = (short)(key & 0xFFFF);
			int num3 = num * mipMap.quadCellWidth;
			int num4 = num2 * mipMap.quadCellWidth;
			Matrix4x4 identity = Matrix4x4.identity;
			QuadCellS3 mipmapData = GetMipmapData(num - 1, num2, mipMap);
			QuadCellS3 mipmapData2 = GetMipmapData(num, num2 + 1, mipMap);
			QuadCellS3 mipmapData3 = GetMipmapData(num + 1, num2, mipMap);
			QuadCellS3 mipmapData4 = GetMipmapData(num, num2 - 1, mipMap);
			QuadCellS3 mipmapData5 = GetMipmapData(num - 1, num2 - 1, mipMap);
			QuadCellS3 mipmapData6 = GetMipmapData(num + 1, num2 - 1, mipMap);
			QuadCellS3 mipmapData7 = GetMipmapData(num - 1, num2 + 1, mipMap);
			QuadCellS3 mipmapData8 = GetMipmapData(num + 1, num2 + 1, mipMap);
			identity.m02 = (mipmapData.layer0 + mipmapData4.layer0 + mipmapData5.layer0 + value.layer0) / 4f;
			identity.m10 = (mipmapData3.layer0 + mipmapData4.layer0 + mipmapData6.layer0 + value.layer0) / 4f;
			identity.m11 = (mipmapData.layer0 + mipmapData2.layer0 + mipmapData7.layer0 + value.layer0) / 4f;
			identity.m12 = (mipmapData3.layer0 + mipmapData2.layer0 + mipmapData8.layer0 + value.layer0) / 4f;
			int quadCellWidth = mipMap.quadCellWidth;
			Vector3 vector = new Vector3((float)num3 + (float)quadCellWidth / 2f, mipMap.yOffset, (float)num4 + (float)quadCellWidth / 2f);
			_ = vector + new Vector3((float)(-quadCellWidth) / 4f, 0f, (float)(-quadCellWidth) / 4f);
			_ = vector + new Vector3((float)quadCellWidth / 4f, 0f, (float)(-quadCellWidth) / 4f);
			_ = vector + new Vector3((float)(-quadCellWidth) / 4f, 0f, (float)quadCellWidth / 4f);
			_ = vector + new Vector3((float)quadCellWidth / 4f, 0f, (float)quadCellWidth / 4f);
		}
	}
}
