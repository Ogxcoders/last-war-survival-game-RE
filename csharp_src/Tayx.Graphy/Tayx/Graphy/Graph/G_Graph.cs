using UnityEngine;

namespace Tayx.Graphy.Graph;

public abstract class G_Graph : MonoBehaviour
{
	protected abstract void UpdateGraph();

	protected abstract void CreatePoints();
}
