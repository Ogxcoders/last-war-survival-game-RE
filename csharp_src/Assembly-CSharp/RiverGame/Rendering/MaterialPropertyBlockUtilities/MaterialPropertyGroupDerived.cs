namespace RiverGame.Rendering.MaterialPropertyBlockUtilities;

public class MaterialPropertyGroupDerived : MaterialPropertyGroup
{
	private static int ID = MaterialPropertyGroup.IDAllocator++;

	public override int id => ID;
}
