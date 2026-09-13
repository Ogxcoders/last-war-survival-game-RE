using UnityEngine;
using XLua;

[Hotfix(HotfixFlag.Stateless)]
public class ScrollTxtNode : MonoBehaviour
{
	[SerializeField]
	private RectTransform txtRect;

	public float target = -178f;

	public float scrollInterval = 45f;

	public float speed = 50f;

	public bool canMove;

	private float scrollTimer;

	private Vector2 originPos;

	public bool run;

	private void Start()
	{
		scrollTimer = scrollInterval;
		originPos = txtRect.anchoredPosition;
	}

	public void Move()
	{
		if (!canMove)
		{
			canMove = true;
		}
	}

	private void Update()
	{
		if (!run)
		{
			return;
		}
		if (!canMove && scrollTimer > 0f)
		{
			scrollTimer -= Time.deltaTime;
			if (scrollTimer <= 0f)
			{
				canMove = true;
			}
		}
		else if (canMove)
		{
			Vector2 anchoredPosition = txtRect.anchoredPosition;
			anchoredPosition.x -= speed * Time.deltaTime;
			txtRect.anchoredPosition = anchoredPosition;
			if (anchoredPosition.x <= target)
			{
				canMove = false;
				scrollTimer = scrollInterval;
				txtRect.anchoredPosition = originPos;
			}
		}
	}
}
