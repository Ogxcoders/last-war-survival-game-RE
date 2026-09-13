using NiceJson;

namespace Mopsicus.Plugins;

public interface IPlugin
{
	string Name { get; }

	void OnData(JsonObject data);

	void OnError(JsonObject data);
}
