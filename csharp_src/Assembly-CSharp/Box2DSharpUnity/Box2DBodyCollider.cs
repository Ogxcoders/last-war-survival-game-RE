using Box2DSharp.Common;
using UnityEngine;

namespace Box2DSharpUnity;

public class Box2DBodyCollider : MonoBehaviour
{
	public FP Density;

	public FP Friction;

	public FP Restitution;

	public int FixtureType;
}
