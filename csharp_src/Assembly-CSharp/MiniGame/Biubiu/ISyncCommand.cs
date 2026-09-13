using Newtonsoft.Json;

namespace MiniGame.Biubiu;

public interface ISyncCommand
{
	int FrameIndex { get; }

	int TypeID { get; }

	[JsonIgnore]
	bool IsValid { get; set; }

	ISyncCommand Clone();
}
