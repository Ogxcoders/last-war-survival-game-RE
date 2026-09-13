using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameKit.Base;

[RequireComponent(typeof(ScrollRect))]
public class UnlimitedScrollView : MonoBehaviour
{
	public delegate void DragOnHeadOrTailOfTheScrollViewDelegate(int d);

	public delegate void OnPointerDownScrollViewDelegate();

	public delegate void BeginDragDelegate(int head, int tail);

	public delegate void ItemMoveInDelegate(GameObject itemObj, object userData);

	public delegate void ItemMoveOutDelegate(GameObject itemObj, object userData);

	private ScrollRect scroll;

	private readonly List<ItemWrap> wraps = new List<ItemWrap>();

	private readonly Dictionary<ItemWrap, GameObject> items = new Dictionary<ItemWrap, GameObject>();

	private HorizontalOrVerticalLayoutGroup group;

	private float spacing;

	private bool inited;

	private bool beginDrag;

	private Vector2 beginDragPosition;

	[SerializeField]
	private int headIndex;

	[SerializeField]
	private int tailIndex = -1;

	private bool toHead;

	private bool toTail;

	public DragOnHeadOrTailOfTheScrollViewDelegate DragOnHeadOrTailOfTheScrollView;

	public OnPointerDownScrollViewDelegate OnPointerDownScrollView;

	public BeginDragDelegate OnBeginDrag;

	public ItemMoveInDelegate OnItemMoveIn;

	public ItemMoveOutDelegate OnItemMoveOut;

	public int HeadIndex => headIndex;

	public int TailIndex => tailIndex;

	public bool ToHead
	{
		set
		{
			toHead = value;
			toTail &= !toHead;
		}
	}

	public bool ToTail
	{
		set
		{
			toTail = value;
			toHead &= !toTail;
		}
	}

	public Vector2 ViewportSize => scroll.viewport.rect.size;

	public Vector2 ContentSize => scroll.content.rect.size;

	public Vector2 ContentPosition
	{
		get
		{
			return scroll.content.anchoredPosition;
		}
		set
		{
			scroll.content.anchoredPosition = value;
		}
	}

	public Vector2 Velocity
	{
		get
		{
			return scroll.velocity;
		}
		set
		{
			scroll.velocity = value;
		}
	}

	private void Update()
	{
		if (!inited)
		{
			Initialize();
			return;
		}
		if (toTail && tailIndex < wraps.Count - 1)
		{
			CreateItem(wraps[++tailIndex]);
		}
		if (toHead && headIndex > 0)
		{
			CreateItem(wraps[--headIndex])?.transform.SetAsFirstSibling();
		}
		if (toHead)
		{
			if (scroll.vertical)
			{
				scroll.verticalNormalizedPosition = 1f;
			}
			else if (scroll.horizontal)
			{
				scroll.horizontalNormalizedPosition = 0f;
			}
		}
		if (toTail)
		{
			if (scroll.vertical)
			{
				scroll.verticalNormalizedPosition = 0f;
			}
			else if (scroll.horizontal)
			{
				scroll.horizontalNormalizedPosition = 1f;
			}
		}
		if ((scroll.vertical && ContentSize.y < ViewportSize.y) || (scroll.horizontal && ContentSize.x < ViewportSize.x))
		{
			if (headIndex > 0)
			{
				CreateItem(wraps[--headIndex])?.transform.SetAsFirstSibling();
			}
			else if (tailIndex < wraps.Count - 1)
			{
				CreateItem(wraps[++tailIndex]);
			}
		}
	}

	private void Awake()
	{
		scroll = GetComponent<ScrollRect>();
		if (scroll.vertical != scroll.horizontal)
		{
			if (scroll.vertical)
			{
				group = scroll.content.GetComponent<VerticalLayoutGroup>();
			}
			else if (scroll.horizontal)
			{
				group = scroll.content.GetComponent<HorizontalLayoutGroup>();
				if (group == null)
				{
					group = scroll.content.GetComponent<BidirectionalHorizontalLayoutGroup>();
				}
			}
			if (group == null)
			{
				Debug.LogError("Only support HorizontalOrVerticalLayoutGroup in UnlimitedScrollView component, you can use UnlimitedScrollGrid or other component to support your custom style.");
				base.enabled = false;
				return;
			}
			spacing = group.spacing;
			if (scroll.horizontalScrollbar != null)
			{
				scroll.horizontalScrollbar.gameObject.SetActive(value: false);
			}
			if (scroll.verticalScrollbar != null)
			{
				scroll.verticalScrollbar.gameObject.SetActive(value: false);
			}
		}
		else
		{
			Debug.LogError("Vertical or Horizontal must be different in UnlimitedScrollView component for now");
			base.enabled = false;
		}
	}

	private void OnEnable()
	{
		scroll.onValueChanged.AddListener(OnValueChanged);
	}

	private void OnDisable()
	{
		scroll.onValueChanged.RemoveListener(OnValueChanged);
		Clear();
	}

	private void OnPointerDown()
	{
		OnPointerDownScrollView?.Invoke();
	}

	private void OnDragBegin()
	{
		beginDrag = true;
		beginDragPosition = scroll.normalizedPosition;
		toTail = false;
		toHead = false;
		OnBeginDrag?.Invoke(headIndex, tailIndex);
	}

	private void OnDragEnd()
	{
		beginDrag = false;
		if (scroll.vertical)
		{
			if (scroll.normalizedPosition.y - beginDragPosition.y > 0.1f && scroll.normalizedPosition.y > 1f && headIndex == 0)
			{
				DragOnHeadOrTailOfTheScrollView?.Invoke(1);
			}
			else if (beginDragPosition.y - scroll.normalizedPosition.y > 0.1f && scroll.normalizedPosition.y < 0f && tailIndex == wraps.Count - 1)
			{
				DragOnHeadOrTailOfTheScrollView?.Invoke(0);
			}
			else
			{
				OnValueChanged(scroll.normalizedPosition);
			}
		}
		else if (scroll.horizontal)
		{
			if (scroll.normalizedPosition.x - beginDragPosition.x > 0.1f && scroll.normalizedPosition.x > 1f && headIndex == 0)
			{
				DragOnHeadOrTailOfTheScrollView?.Invoke(1);
			}
			else if (beginDragPosition.x - scroll.normalizedPosition.x > 0.1f && scroll.normalizedPosition.x < 0f && tailIndex == wraps.Count - 1)
			{
				DragOnHeadOrTailOfTheScrollView?.Invoke(0);
			}
			else
			{
				OnValueChanged(scroll.normalizedPosition);
			}
		}
	}

	private void Initialize()
	{
		if (!(ViewportSize == Vector2.zero))
		{
			if (scroll.vertical)
			{
				InitVertical();
			}
			else if (scroll.horizontal)
			{
				InitHorizontal();
			}
			inited = true;
		}
	}

	private void InitVertical()
	{
		Vector2 vector = new Vector2(group.padding.left + group.padding.right, group.padding.top + group.padding.bottom);
		if (toTail)
		{
			tailIndex = wraps.Count - 1;
			int num = tailIndex;
			while (num >= 0 && vector.y < ViewportSize.y)
			{
				headIndex = num;
				GameObject gameObject = CreateItem(wraps[num]);
				if (gameObject != null)
				{
					gameObject.transform.SetAsFirstSibling();
					vector = new Vector2(vector.x, vector.y + gameObject.GetComponent<RectTransform>().rect.size.y);
				}
				num--;
			}
			return;
		}
		headIndex = 0;
		for (int i = 0; i < wraps.Count; i++)
		{
			if (!(vector.y <= ViewportSize.y))
			{
				break;
			}
			tailIndex = i;
			GameObject gameObject2 = CreateItem(wraps[i]);
			if (gameObject2 != null)
			{
				vector = new Vector2(vector.x, vector.y + gameObject2.GetComponent<RectTransform>().rect.size.y);
			}
		}
	}

	private void InitHorizontal()
	{
		Vector2 vector = new Vector2(group.padding.left + group.padding.right, group.padding.top + group.padding.bottom);
		if (toTail)
		{
			tailIndex = wraps.Count - 1;
			int num = tailIndex;
			while (num >= 0 && vector.x < ViewportSize.x)
			{
				headIndex = num;
				GameObject gameObject = CreateItem(wraps[num]);
				if (gameObject != null)
				{
					gameObject.transform.SetAsFirstSibling();
					vector = new Vector2(vector.x + gameObject.GetComponent<RectTransform>().rect.size.x, vector.y);
				}
				num--;
			}
			return;
		}
		headIndex = 0;
		for (int i = 0; i < wraps.Count; i++)
		{
			if (!(vector.x <= ViewportSize.x))
			{
				break;
			}
			tailIndex = i;
			GameObject gameObject2 = CreateItem(wraps[i]);
			if (gameObject2 != null)
			{
				vector = new Vector2(vector.x + gameObject2.GetComponent<RectTransform>().rect.size.x, vector.y);
			}
		}
	}

	public void Clear()
	{
		inited = false;
		beginDrag = false;
		headIndex = 0;
		tailIndex = -1;
		wraps.Clear();
		foreach (KeyValuePair<ItemWrap, GameObject> item in items)
		{
			if (!(item.Value == null))
			{
				OnItemMoveOut?.Invoke(item.Value, item.Key.userdata);
				item.Value.Recycle();
			}
		}
		items.Clear();
	}

	public int GetItemWrapCount()
	{
		return wraps.Count;
	}

	public GameObject GetItem(ItemWrap wrap)
	{
		if (items.TryGetValue(wrap, out var value))
		{
			return value;
		}
		return null;
	}

	public ItemWrap GetItemWrap(object userdata)
	{
		return wraps.Find((ItemWrap p) => p.userdata == userdata);
	}

	public ItemWrap GetItemWrap(int index)
	{
		if (index < 0 || index >= wraps.Count)
		{
			return null;
		}
		return wraps[index];
	}

	public void AddItemWrap(GameObject prefab, object userdata)
	{
		wraps.Add(new ItemWrap
		{
			prefab = prefab,
			userdata = userdata
		});
	}

	public void InsertItemWrap(int index, GameObject prefab, object userdata)
	{
		if (index >= 0 && index < wraps.Count)
		{
			wraps.Insert(index, new ItemWrap
			{
				prefab = prefab,
				userdata = userdata
			});
			if (index <= headIndex)
			{
				headIndex++;
			}
			if (index <= tailIndex)
			{
				tailIndex++;
			}
		}
	}

	public void RemoveItemWrap(object userdata)
	{
		ItemWrap itemWrap = GetItemWrap(userdata);
		if (itemWrap == null)
		{
			return;
		}
		for (int i = 0; i < wraps.Count; i++)
		{
			if (wraps[i] == itemWrap)
			{
				RemoveItemWrap(i);
				break;
			}
		}
	}

	public void RemoveItemWrap(int index)
	{
		if (index >= 0 && index < wraps.Count)
		{
			wraps.RemoveAt(index);
			if (index < headIndex)
			{
				headIndex--;
			}
			if (index <= tailIndex)
			{
				tailIndex--;
			}
		}
	}

	private GameObject CreateItem(ItemWrap wrap)
	{
		if (wrap.prefab == null)
		{
			return null;
		}
		GameObject gameObject = wrap.prefab.Spawn(scroll.content);
		OnItemMoveIn?.Invoke(gameObject, wrap.userdata);
		items[wrap] = gameObject;
		return gameObject;
	}

	private void DeleteItem(ItemWrap wrap)
	{
		if (items.TryGetValue(wrap, out var value))
		{
			OnItemMoveOut?.Invoke(value, wrap.userdata);
			value.Recycle();
			items[wrap] = null;
		}
	}

	public void AddItemToTail(GameObject prefab, object userdata)
	{
		bool flag = tailIndex == wraps.Count - 1;
		AddItemWrap(prefab, userdata);
		if (flag)
		{
			CreateItem(wraps[++tailIndex]);
		}
		ToTail = flag;
	}

	private void OnValueChanged(Vector2 vector2)
	{
		if (!inited || scroll.content.childCount <= 0 || wraps.Count <= 0 || beginDrag)
		{
			return;
		}
		if (scroll.vertical)
		{
			float height = items[wraps[headIndex]].GetComponent<RectTransform>().rect.height;
			float height2 = items[wraps[tailIndex]].GetComponent<RectTransform>().rect.height;
			if (ContentPosition.y > ViewportSize.y + height + spacing && tailIndex < wraps.Count - 1)
			{
				DeleteItem(wraps[headIndex++]);
				ContentPosition = new Vector2(ContentPosition.x, ContentPosition.y - height - spacing);
				CreateItem(wraps[++tailIndex]);
			}
			else if (ContentPosition.y < ViewportSize.y && headIndex > 0)
			{
				if (ContentSize.y - ContentPosition.y - ViewportSize.y > height2 + spacing)
				{
					DeleteItem(wraps[tailIndex--]);
				}
				GameObject gameObject = CreateItem(wraps[--headIndex]);
				if (gameObject != null)
				{
					gameObject.transform.SetAsFirstSibling();
					float height3 = gameObject.GetComponent<RectTransform>().rect.height;
					ContentPosition = new Vector2(ContentPosition.x, ContentPosition.y + height3 + spacing);
				}
			}
			else if (ContentSize.y - ContentPosition.y - ViewportSize.y < 0f && tailIndex < wraps.Count - 1)
			{
				CreateItem(wraps[++tailIndex]);
			}
		}
		else
		{
			if (!scroll.horizontal)
			{
				return;
			}
			float width = items[wraps[headIndex]].GetComponent<RectTransform>().rect.width;
			float width2 = items[wraps[tailIndex]].GetComponent<RectTransform>().rect.width;
			if (0f - ContentPosition.x > ViewportSize.x + width + spacing && tailIndex < wraps.Count - 1)
			{
				DeleteItem(wraps[headIndex++]);
				ContentPosition = new Vector2(ContentPosition.x + width + spacing, ContentPosition.y);
				CreateItem(wraps[++tailIndex]);
			}
			else if (0f - ContentPosition.x < ViewportSize.x && headIndex > 0)
			{
				if (ContentSize.x + ContentPosition.x - ViewportSize.x > width2 + spacing)
				{
					DeleteItem(wraps[tailIndex--]);
				}
				GameObject gameObject2 = CreateItem(wraps[--headIndex]);
				if (gameObject2 != null)
				{
					gameObject2.transform.SetAsFirstSibling();
					float width3 = gameObject2.GetComponent<RectTransform>().rect.width;
					ContentPosition = new Vector2(ContentPosition.x - width3 - spacing, ContentPosition.y);
				}
			}
			else if (ContentSize.x + ContentPosition.x - ViewportSize.y < 0f && tailIndex < wraps.Count - 1)
			{
				CreateItem(wraps[++tailIndex]);
			}
		}
	}
}
