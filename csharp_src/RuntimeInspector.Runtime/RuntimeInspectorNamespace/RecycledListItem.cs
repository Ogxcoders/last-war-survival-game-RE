using UnityEngine;

namespace RuntimeInspectorNamespace;

[RequireComponent(typeof(RectTransform))]
public class RecycledListItem : MonoBehaviour
{
	private IListViewAdapter adapter;

	public object Tag { get; set; }

	public int Position { get; set; }

	internal void SetAdapter(IListViewAdapter adapter)
	{
		this.adapter = adapter;
	}

	public void OnClick()
	{
		adapter.OnItemClicked(this);
	}
}
