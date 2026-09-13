using System.Collections.Generic;

namespace Box2DSharp.Foreign;

public interface IPlatformLogic
{
	List<int> OnPlatformIDs { get; set; }
}
