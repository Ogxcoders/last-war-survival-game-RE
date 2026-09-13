using UnityEngine;
using UnityEngine.UI;

public class ItemRender : DynamicInfinityItem
{
	public Text m_TxtName;

	public Button m_Btn;

	private void Start()
	{
		m_Btn.onClick.AddListener(delegate
		{
			MonoBehaviour.print("Click " + mData.ToString());
		});
	}

	protected override void OnRenderer()
	{
		base.OnRenderer();
		m_TxtName.text = mData.ToString();
	}
}
