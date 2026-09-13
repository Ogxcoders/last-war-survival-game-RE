using System;

namespace ParkourStage.Runtime.Data;

public interface IStageEditData : IDisposable
{
	void LoadShowPrefab();

	void SetSelected(bool isSelected);

	void OnUpdate();

	void DrawView();

	bool IsDestroyed();
}
