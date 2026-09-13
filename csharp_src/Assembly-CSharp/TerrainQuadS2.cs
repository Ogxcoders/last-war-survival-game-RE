using System.Collections.Generic;
using UnityEngine;

public class TerrainQuadS2 : TerrainQuadSeason<float>
{
	private int _terrainNoiseRectNameID;

	private int _terrainNoiseVPNameID;

	protected override float DefaultStateValue()
	{
		return 1f;
	}

	protected override void BeforeSetup()
	{
		base.passId = TerrainQuadLayerRenderer.RenderPass_S2;
		blockSize = 16;
		minCellSize = 2;
		_terrainNoiseRectNameID = Shader.PropertyToID("_TerrainNoiseRect");
		_terrainNoiseVPNameID = Shader.PropertyToID("_TerrainNoiseVP");
	}

	protected override float CalcMipmap(int xPos, int yPos, int mipmapLevel)
	{
		int num = xPos * 2;
		int num2 = yPos * 2;
		float mipmapData = GetMipmapData(num, num2, mipmapLevel - 1);
		float mipmapData2 = GetMipmapData(num + 1, num2, mipmapLevel - 1);
		float mipmapData3 = GetMipmapData(num, num2 + 1, mipmapLevel - 1);
		float mipmapData4 = GetMipmapData(num + 1, num2 + 1, mipmapLevel - 1);
		return (mipmapData + mipmapData2 + mipmapData3 + mipmapData4) / 4f;
	}

	protected override void BuildItems()
	{
		_MipMap mipMap = _mipmaps[base.showMipmapLevel];
		int num = blockSize / (int)Mathf.Pow(2f, base.showMipmapLevel);
		int num2 = _lastBlockLB.x * num;
		int num3 = _lastBlockLB.y * num;
		int num4 = (_lastBlockRT.x + 1) * num - 1;
		int num5 = (_lastBlockRT.y + 1) * num - 1;
		int num6 = num2 * mipMap.quadCellWidth;
		int num7 = num3 * mipMap.quadCellWidth;
		int num8 = (num4 - num2) * mipMap.quadCellWidth;
		int num9 = (num5 - num3) * mipMap.quadCellWidth;
		float num10 = (float)num8 * 0.5f;
		float num11 = (float)num9 * 0.5f;
		Vector4 value = new Vector4(num6, num7, num8, num9);
		Vector3 vector = new Vector3((float)num6 + num10, 1f, (float)num7 + num11);
		Matrix4x4 matrix4x = Matrix4x4.LookAt(vector, vector + Vector3.down, Vector3.forward);
		Matrix4x4 matrix4x2 = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, 1f, -1f)) * matrix4x.inverse;
		Matrix4x4 value2 = GL.GetGPUProjectionMatrix(Matrix4x4.Ortho(0f - num10, num10, 0f - num11, num11, -10f, 10f), renderIntoTexture: true) * matrix4x2;
		Shader.SetGlobalVector(_terrainNoiseRectNameID, value);
		Shader.SetGlobalMatrix(_terrainNoiseVPNameID, value2);
		int num12 = -1;
		for (int i = num3; i <= num5; i++)
		{
			for (int j = num2; j <= num4; j++)
			{
				int key = (j << 16) | (i & 0xFFFF);
				if (mipMap.data.TryGetValue(key, out var value3))
				{
					float m = value3;
					int num13 = j;
					int num14 = i;
					int num15 = num13 * mipMap.quadCellWidth;
					int num16 = num14 * mipMap.quadCellWidth;
					_MatrixPoolItem matrixPoolItem = _pool[_cursor];
					ref Matrix4x4 reference = ref matrixPoolItem.matrix[++num12];
					matrixPoolItem.length++;
					reference.m03 = num15;
					reference.m13 = mipMap.yOffset;
					reference.m23 = num16;
					reference.m00 = _quadSize;
					reference.m01 = m;
					float mipmapData = GetMipmapData(num13 - 1, num14, mipMap);
					float mipmapData2 = GetMipmapData(num13, num14 + 1, mipMap);
					float mipmapData3 = GetMipmapData(num13 + 1, num14, mipMap);
					float mipmapData4 = GetMipmapData(num13, num14 - 1, mipMap);
					float mipmapData5 = GetMipmapData(num13 - 1, num14 - 1, mipMap);
					float mipmapData6 = GetMipmapData(num13 + 1, num14 - 1, mipMap);
					float mipmapData7 = GetMipmapData(num13 - 1, num14 + 1, mipMap);
					float mipmapData8 = GetMipmapData(num13 + 1, num14 + 1, mipMap);
					reference.m02 = mipmapData;
					reference.m10 = mipmapData3;
					reference.m11 = mipmapData2;
					reference.m12 = mipmapData4;
					reference.m20 = mipmapData5;
					reference.m21 = mipmapData6;
					reference.m22 = mipmapData7;
					reference.m30 = mipmapData8;
					if (num12 >= 510)
					{
						SetPoolCursor(_cursor + 1);
						num12 = -1;
					}
				}
			}
		}
	}

	protected override void DrawMipmapGizmos(_MipMap mipMap, GUIStyle style)
	{
		using Dictionary<int, float>.Enumerator enumerator = mipMap.data.GetEnumerator();
		while (enumerator.MoveNext())
		{
			int key = enumerator.Current.Key;
			float value = enumerator.Current.Value;
			int num = key >> 16;
			short num2 = (short)(key & 0xFFFF);
			int num3 = num * mipMap.quadCellWidth;
			int num4 = num2 * mipMap.quadCellWidth;
			Matrix4x4 identity = Matrix4x4.identity;
			float mipmapData = GetMipmapData(num - 1, num2, mipMap);
			float mipmapData2 = GetMipmapData(num, num2 + 1, mipMap);
			float mipmapData3 = GetMipmapData(num + 1, num2, mipMap);
			float mipmapData4 = GetMipmapData(num, num2 - 1, mipMap);
			float mipmapData5 = GetMipmapData(num - 1, num2 - 1, mipMap);
			float mipmapData6 = GetMipmapData(num + 1, num2 - 1, mipMap);
			float mipmapData7 = GetMipmapData(num - 1, num2 + 1, mipMap);
			float mipmapData8 = GetMipmapData(num + 1, num2 + 1, mipMap);
			int num5 = 4;
			identity.m02 = (mipmapData + mipmapData4 + mipmapData5 + value - (float)num5) * 1f / ((float)num5 * 6f);
			identity.m10 = (mipmapData3 + mipmapData4 + mipmapData6 + value - (float)num5) * 1f / ((float)num5 * 6f);
			identity.m11 = (mipmapData + mipmapData2 + mipmapData7 + value - (float)num5) * 1f / ((float)num5 * 6f);
			identity.m12 = (mipmapData3 + mipmapData2 + mipmapData8 + value - (float)num5) * 1f / ((float)num5 * 6f);
			int quadCellWidth = mipMap.quadCellWidth;
			Vector3 vector = new Vector3((float)num3 + (float)quadCellWidth / 2f, mipMap.yOffset, (float)num4 + (float)quadCellWidth / 2f);
			_ = vector + new Vector3((float)(-quadCellWidth) / 4f, 0f, (float)(-quadCellWidth) / 4f);
			_ = vector + new Vector3((float)quadCellWidth / 4f, 0f, (float)(-quadCellWidth) / 4f);
			_ = vector + new Vector3((float)(-quadCellWidth) / 4f, 0f, (float)quadCellWidth / 4f);
			_ = vector + new Vector3((float)quadCellWidth / 4f, 0f, (float)quadCellWidth / 4f);
		}
	}
}
