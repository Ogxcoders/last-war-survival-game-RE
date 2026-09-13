using UnityEngine;

public class UIBoxColliderSize : MonoBehaviour
{
	[SerializeField]
	private RectTransform rectTransform;

	[SerializeField]
	private BoxCollider2D boxCollider2D;

	[SerializeField]
	private bool isUpdateSize = true;

	private void LateUpdate()
	{
		if (isUpdateSize && !(rectTransform == null) && !(boxCollider2D == null))
		{
			boxCollider2D.offset = rectTransform.rect.center;
			boxCollider2D.size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
			isUpdateSize = false;
		}
	}
}
