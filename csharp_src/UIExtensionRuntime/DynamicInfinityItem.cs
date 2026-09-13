using UnityEngine;

public class DynamicInfinityItem : MonoBehaviour
{
	public delegate void OnSelect(DynamicInfinityItem item);

	public delegate void OnUpdateData(DynamicInfinityItem item);

	public OnSelect OnSelectHandler;

	public OnUpdateData OnUpdateDataHandler;

	protected DynamicRect mDRect;

	protected object mData;

	public DynamicRect DRect
	{
		get
		{
			return mDRect;
		}
		set
		{
			mDRect = value;
			base.gameObject.SetActive(value != null);
		}
	}

	private void Start()
	{
	}

	public void SetData(object data)
	{
		if (data != null)
		{
			mData = data;
			if (OnUpdateDataHandler != null)
			{
				OnUpdateDataHandler(this);
			}
			OnRenderer();
		}
	}

	protected virtual void OnRenderer()
	{
	}

	public object GetData()
	{
		return mData;
	}

	public T GetData<T>()
	{
		return (T)mData;
	}
}
