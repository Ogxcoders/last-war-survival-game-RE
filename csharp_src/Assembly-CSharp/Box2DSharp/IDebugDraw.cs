using Box2DSharp.Collision;
using Box2DSharp.Common;
using MiniGame.Core;

namespace Box2DSharp;

public interface IDebugDraw : IDraw
{
	void DrawAABB(Box2DSharp.Collision.AABB aabb, Color color);

	void DrawString(float x, float y, string strings);

	void DrawString(int x, int y, string strings);

	void DrawString(FloatVector2 position, string strings);
}
