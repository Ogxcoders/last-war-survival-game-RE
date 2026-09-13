using Leopotam.EcsLite;
using Newtonsoft.Json;

namespace Box2DSharp.Foreign;

public class IBodyLogic
{
	public int ILayer;

	public bool CanOnPlatform;

	[JsonIgnore]
	public int ID = -1;

	[JsonIgnore]
	public int OwnerID = -1;

	[JsonIgnore]
	public bool Die;

	[JsonIgnore]
	public EcsPackedEntity EntityId;

	public virtual IBodyLogic Clone()
	{
		return (IBodyLogic)MemberwiseClone();
	}
}
