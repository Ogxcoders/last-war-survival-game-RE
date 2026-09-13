namespace Spine.Unity;

public class NoOpTextureLoader : TextureLoader
{
	public void Load(AtlasPage page, string path)
	{
	}

	public void Unload(object texture)
	{
	}
}
