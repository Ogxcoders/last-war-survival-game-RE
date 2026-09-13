using UnityEngine;
using UnityEngine.UI;

public class UIScrollCell : MonoBehaviour
{
	public Text indexTxt;

	public void RefreshItem()
	{
		if ((bool)indexTxt)
		{
			int result = 0;
			int.TryParse(base.name, out result);
			indexTxt.text = result.ToString();
		}
	}
}
