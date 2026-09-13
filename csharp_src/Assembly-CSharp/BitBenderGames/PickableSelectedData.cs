using UnityEngine;

namespace BitBenderGames;

public class PickableSelectedData
{
	public Transform SelectedTransform { get; set; }

	public bool IsDoubleClick { get; set; }

	public bool IsLongTap { get; set; }
}
