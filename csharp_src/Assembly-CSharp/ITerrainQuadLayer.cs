using UnityEngine;
using UnityEngine.Rendering;

public interface ITerrainQuadLayer
{
	string passId { get; }

	int blockSize { get; }

	int minCellSize { get; }

	float scaler { get; }

	float invScaler { get; }

	int showMipmapLevel { get; set; }

	void Setup(int quadSize, int mipmapCount);

	void Clear();

	void SetupMaterial(Material material);

	void SetViewRect(Vector2Int lb, Vector2Int rt);

	void SetTransit();

	void AddMipmap0(int x, int y);

	void ChangeMipmap0(int x, int y);

	void RemoveMipmap0(int x, int y);

	void UpdateMipmaps(Vector2Int block);

	void RemoveMipmaps(Vector2Int block);

	void ClearMipmaps();

	void Rebuild();

	bool Ready();

	bool Prepare();

	void DrawNoise(CommandBuffer cmd);

	void DrawLayers(CommandBuffer cmd);

	void DrawGizmos();
}
