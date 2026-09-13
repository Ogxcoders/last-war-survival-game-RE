using Box2DSharp;
using Box2DSharp.Common;
using Box2DSharp.Foreign;
using Box2DSharp.Testbed.Unity.Inspection;
using UnityEngine;

namespace Box2DSharpUnity;

public class Box2DDriveManager : MonoBehaviour
{
	private static Box2DDriveManager ms_instance;

	private Box2DGame _box2DGame;

	private float _scale = 1f;

	public static void SetupDebugWorld(Box2DGame world)
	{
		if (ms_instance == null)
		{
			ms_instance = Camera.main.gameObject.AddComponent<Box2DDriveManager>();
		}
		ms_instance._box2DGame = world;
		ms_instance._scale = 1f;
	}

	public static void SetupDebugWorld(Box2DGame world, GameObject root, float scale, bool useCustomColor = false, UnityEngine.Color color = default(UnityEngine.Color))
	{
		if (!root.TryGetComponent<Box2DDriveManager>(out var component))
		{
			component = root.AddComponent<Box2DDriveManager>();
		}
		component._box2DGame = world;
		component._scale = scale;
		component._box2DGame.World.Draw = new DebugDraw
		{
			Draw = component.GetDraw(useCustomColor, color),
			ShowUI = true
		};
	}

	private void OnRenderObject()
	{
		if (_box2DGame != null && _box2DGame.World != null)
		{
			if (_box2DGame.World.Draw == null)
			{
				_box2DGame.World.Draw = new DebugDraw
				{
					Draw = GetDraw(),
					ShowUI = true
				};
			}
			DrawFlag drawFlag = (DrawFlag)0;
			drawFlag |= DrawFlag.DrawShape;
			drawFlag |= DrawFlag.DrawJoint;
			drawFlag |= DrawFlag.DrawAABB;
			drawFlag |= DrawFlag.DrawCenterOfMass;
			drawFlag |= DrawFlag.DrawContactPoint;
			_box2DGame.World.Draw.Flags = drawFlag;
			_box2DGame.World.DebugDraw();
		}
	}

	private UnityDraw GetDraw(bool useCustomColor = false, UnityEngine.Color color = default(UnityEngine.Color))
	{
		if (ms_instance == this)
		{
			return UnityDraw.GetDraw();
		}
		return UnityDraw.GetDraw(base.gameObject, _scale, useCustomColor, color);
	}
}
