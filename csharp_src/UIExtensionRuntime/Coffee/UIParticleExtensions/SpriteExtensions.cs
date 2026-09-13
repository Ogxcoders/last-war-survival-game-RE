using UnityEngine;

namespace Coffee.UIParticleExtensions;

internal static class SpriteExtensions
{
	internal static Texture2D GetActualTexture(this Sprite self)
	{
		if (!self)
		{
			return null;
		}
		return self.texture;
	}
}
