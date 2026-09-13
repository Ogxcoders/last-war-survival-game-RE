using System;

namespace ParkourStage.Runtime.Manager;

public interface IStageDataEditManager : IDisposable
{
	void OnUpdate();

	void DrawView();

	bool SaveToDataLoader();
}
