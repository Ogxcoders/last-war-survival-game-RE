using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollLoopText : MonoBehaviour
{
	public Text txt;

	public List<Text> scrollTxt;

	public float offset = 40f;

	public float speed = -30f;

	public RectTransform scrollEndPos;

	private Vector2 endPos;

	public void CheckCanRun()
	{
		RectTransform component = txt.GetComponent<RectTransform>();
		RectTransform component2 = txt.transform.parent.GetComponent<RectTransform>();
		txt.gameObject.SetActive(value: true);
		if (txt.preferredWidth >= component2.sizeDelta.x - 2f)
		{
			base.gameObject.SetActive(value: true);
			txt.gameObject.SetActive(value: false);
			scrollTxt[0].text = txt.text;
			scrollTxt[0].color = txt.color;
			scrollTxt[1].text = txt.text;
			scrollTxt[1].color = txt.color;
			scrollTxt[2].text = txt.text;
			scrollTxt[2].color = txt.color;
			scrollTxt[0].rectTransform.anchoredPosition = txt.rectTransform.anchoredPosition;
			float num = component.anchoredPosition.x + txt.preferredWidth + offset;
			Vector2 anchoredPosition = component.anchoredPosition;
			anchoredPosition.x -= txt.preferredWidth + offset;
			scrollEndPos.anchoredPosition = anchoredPosition;
			Text text = scrollTxt[1];
			Vector2 anchoredPosition2 = component.anchoredPosition;
			anchoredPosition2.x = num;
			text.rectTransform.anchoredPosition = anchoredPosition2;
			text = scrollTxt[2];
			anchoredPosition2 = component.anchoredPosition;
			num += txt.preferredWidth + offset;
			anchoredPosition2.x = num;
			Vector2 vector = (text.rectTransform.anchoredPosition = anchoredPosition2);
			endPos = vector;
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		foreach (Text item in scrollTxt)
		{
			Vector2 anchoredPosition = item.rectTransform.anchoredPosition;
			anchoredPosition.x += Time.deltaTime * speed;
			item.rectTransform.anchoredPosition = anchoredPosition;
			if (anchoredPosition.x <= scrollEndPos.anchoredPosition.x)
			{
				item.rectTransform.anchoredPosition = endPos;
			}
		}
	}
}
