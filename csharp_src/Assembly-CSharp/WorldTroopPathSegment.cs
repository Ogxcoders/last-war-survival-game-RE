using UnityEngine;

public struct WorldTroopPathSegment
{
	public Vector3 pos;

	public Vector3 dir;

	public float dist;

	public override string ToString()
	{
		return string.Format("pos:{0},dir:{1},dist:{2:F1}", pos, dir.ToString("F2"), dist);
	}
}
