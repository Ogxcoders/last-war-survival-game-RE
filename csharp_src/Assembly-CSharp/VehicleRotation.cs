using UnityEngine;

public class VehicleRotation
{
	public static Quaternion LookAt(Transform transform, Vector3 target)
	{
		transform.LookAt(target);
		return transform.rotation;
	}

	public static void GetAniDir(Direction dir, out string dirPrefix, out bool flipX)
	{
		switch (dir)
		{
		case Direction.N:
			dirPrefix = "N_";
			flipX = false;
			break;
		case Direction.NW:
			dirPrefix = "NW_";
			flipX = false;
			break;
		case Direction.W:
			dirPrefix = "W_";
			flipX = false;
			break;
		case Direction.SW:
			dirPrefix = "SW_";
			flipX = false;
			break;
		case Direction.S:
			dirPrefix = "S_";
			flipX = false;
			break;
		case Direction.SE:
			dirPrefix = "SW_";
			flipX = true;
			break;
		case Direction.E:
			dirPrefix = "W_";
			flipX = true;
			break;
		case Direction.NE:
			dirPrefix = "NW_";
			flipX = true;
			break;
		default:
			dirPrefix = "N_";
			flipX = false;
			break;
		}
	}

	public static Quaternion WorldLookAt(Transform transform, Vector3 target, bool is2D, string action)
	{
		if (transform == null)
		{
			return Quaternion.identity;
		}
		if (is2D)
		{
			Vector3 to = target - transform.position;
			to.z *= 0.5f;
			float num = 0f - Vector3.SignedAngle(Vector3.forward, to, Vector3.up);
			num += 22.5f;
			if (num < 0f)
			{
				num += 360f;
			}
			else if (num >= 360f)
			{
				num -= 360f;
			}
			GetAniDir((Direction)(num / 45f), out var dirPrefix, out var flipX);
			transform.GetComponent<Animator>().Play(dirPrefix + action);
			transform.GetComponent<SpriteRenderer>().flipX = flipX;
			return Quaternion.identity;
		}
		return Quaternion.LookRotation(target - transform.position, Vector3.up);
	}
}
