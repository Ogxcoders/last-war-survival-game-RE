public interface IManualUpdator
{
	float lastUpdateTime { get; set; }

	void ManualUpdate(float delta);
}
