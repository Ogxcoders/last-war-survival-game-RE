using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class ObjectReferencePicker : SkinnedWindow, IListViewAdapter
{
	public delegate void ReferenceCallback(object reference);

	public delegate string NameGetter(object reference);

	private const string SPRITE_ATLAS_PREFIX = "SpriteAtlasTexture-";

	private static ObjectReferencePicker m_instance;

	private ReferenceCallback onReferenceChanged;

	private ReferenceCallback onSelectionConfirmed;

	private NameGetter referenceNameGetter;

	private NameGetter referenceDisplayNameGetter;

	[SerializeField]
	private Image panel;

	[SerializeField]
	private Image scrollbar;

	[SerializeField]
	private InputField searchBar;

	[SerializeField]
	private Image searchIcon;

	[SerializeField]
	private Image searchBarBackground;

	[SerializeField]
	private Text selectPromptText;

	[SerializeField]
	private LayoutElement searchBarLayoutElement;

	[SerializeField]
	private LayoutElement buttonsLayoutElement;

	[SerializeField]
	private Button cancelButton;

	[SerializeField]
	private Button okButton;

	[SerializeField]
	private RecycledListView listView;

	[SerializeField]
	private Image listViewBackground;

	[SerializeField]
	private ObjectReferencePickerItem referenceItemPrefab;

	private Canvas referenceCanvas;

	private readonly List<object> references = new List<object>(64);

	private readonly List<object> filteredReferences = new List<object>(64);

	private object initialValue;

	private object currentlySelectedObject;

	private ObjectReferencePickerItem currentlySelectedItem;

	public static ObjectReferencePicker Instance
	{
		get
		{
			if (!m_instance)
			{
				m_instance = UnityEngine.Object.Instantiate(Resources.Load<ObjectReferencePicker>("RuntimeInspector/ObjectReferencePicker"));
				m_instance.gameObject.SetActive(value: false);
				RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Add(m_instance.transform);
			}
			return m_instance;
		}
	}

	int IListViewAdapter.Count => filteredReferences.Count;

	float IListViewAdapter.ItemHeight => base.Skin.LineHeight;

	protected override void Awake()
	{
		base.Awake();
		listView.SetAdapter(this);
		searchBar.onValueChanged.AddListener(OnSearchTextChanged);
		cancelButton.onClick.AddListener(Cancel);
		okButton.onClick.AddListener(delegate
		{
			try
			{
				if (onSelectionConfirmed != null)
				{
					onSelectionConfirmed(currentlySelectedObject);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			Close();
		});
	}

	public void Show(ReferenceCallback onReferenceChanged, ReferenceCallback onSelectionConfirmed, NameGetter referenceNameGetter, NameGetter referenceDisplayNameGetter, object[] references, object initialReference, bool includeNullReference, string title, Canvas referenceCanvas)
	{
		initialValue = initialReference;
		this.onReferenceChanged = onReferenceChanged;
		this.onSelectionConfirmed = onSelectionConfirmed;
		this.referenceNameGetter = referenceNameGetter ?? ((NameGetter)((object reference) => reference.GetNameWithType()));
		this.referenceDisplayNameGetter = referenceDisplayNameGetter ?? ((NameGetter)((object reference) => reference.GetNameWithType()));
		if ((bool)referenceCanvas && this.referenceCanvas != referenceCanvas)
		{
			this.referenceCanvas = referenceCanvas;
			Canvas component = GetComponent<Canvas>();
			component.CopyValuesFrom(referenceCanvas);
			component.sortingOrder = Mathf.Max(1000, referenceCanvas.sortingOrder + 100);
		}
		panel.rectTransform.anchoredPosition = Vector2.zero;
		base.gameObject.SetActive(value: true);
		selectPromptText.text = title;
		currentlySelectedObject = initialReference;
		GenerateReferenceItems(references, includeNullReference);
	}

	public void Cancel()
	{
		try
		{
			if (currentlySelectedObject != initialValue && onReferenceChanged != null)
			{
				onReferenceChanged(initialValue);
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		Close();
	}

	public void Close()
	{
		onReferenceChanged = null;
		onSelectionConfirmed = null;
		referenceNameGetter = null;
		referenceDisplayNameGetter = null;
		initialValue = null;
		currentlySelectedObject = null;
		currentlySelectedItem = null;
		references.Clear();
		filteredReferences.Clear();
		base.gameObject.SetActive(value: false);
	}

	protected override void RefreshSkin()
	{
		panel.color = base.Skin.WindowColor;
		listViewBackground.color = base.Skin.BackgroundColor;
		scrollbar.color = base.Skin.ScrollbarColor;
		selectPromptText.SetSkinText(base.Skin);
		searchBar.textComponent.SetSkinButtonText(base.Skin);
		searchBarBackground.color = base.Skin.ButtonBackgroundColor;
		searchIcon.color = base.Skin.ButtonTextColor;
		searchBarLayoutElement.SetHeight(base.Skin.LineHeight);
		buttonsLayoutElement.SetHeight(Mathf.Min(45f, (float)base.Skin.LineHeight * 1.5f));
		cancelButton.SetSkinButton(base.Skin);
		okButton.SetSkinButton(base.Skin);
		listView.ResetList();
	}

	private void GenerateReferenceItems(object[] references, bool includeNullReference)
	{
		this.references.Clear();
		filteredReferences.Clear();
		searchBar.text = string.Empty;
		if (includeNullReference)
		{
			this.references.Add(null);
		}
		Array.Sort(references, (object ref1, object ref2) => referenceNameGetter(ref1).CompareTo(referenceNameGetter(ref2)));
		for (int num = 0; num < references.Length; num++)
		{
			UnityEngine.Object obj = references[num] as UnityEngine.Object;
			if ((bool)obj)
			{
				if ((obj.hideFlags == HideFlags.None || obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideInHierarchy || obj.hideFlags == HideFlags.HideInInspector) && ((!(obj is Texture) && !(obj is Sprite)) || !obj.name.StartsWith("SpriteAtlasTexture-")))
				{
					this.references.Add(obj);
				}
			}
			else if (references[num] != null)
			{
				this.references.Add(references[num]);
			}
		}
		OnSearchTextChanged(string.Empty);
		listView.UpdateList();
	}

	RecycledListItem IListViewAdapter.CreateItem(Transform parent)
	{
		ObjectReferencePickerItem objectReferencePickerItem = UnityEngine.Object.Instantiate(referenceItemPrefab, parent, worldPositionStays: false);
		objectReferencePickerItem.Skin = base.Skin;
		return objectReferencePickerItem;
	}

	private void OnSearchTextChanged(string value)
	{
		filteredReferences.Clear();
		value = value.ToLowerInvariant();
		for (int i = 0; i < references.Count; i++)
		{
			if (referenceNameGetter(references[i]).ToLowerInvariant().Contains(value))
			{
				filteredReferences.Add(references[i]);
			}
		}
		listView.UpdateList();
	}

	void IListViewAdapter.SetItemContent(RecycledListItem item)
	{
		ObjectReferencePickerItem objectReferencePickerItem = (ObjectReferencePickerItem)item;
		objectReferencePickerItem.SetContent(filteredReferences[objectReferencePickerItem.Position], referenceDisplayNameGetter(filteredReferences[objectReferencePickerItem.Position]));
		if (objectReferencePickerItem.Reference == currentlySelectedObject)
		{
			objectReferencePickerItem.IsSelected = true;
			currentlySelectedItem = objectReferencePickerItem;
		}
		else
		{
			objectReferencePickerItem.IsSelected = false;
		}
		objectReferencePickerItem.Skin = base.Skin;
	}

	void IListViewAdapter.OnItemClicked(RecycledListItem item)
	{
		if (currentlySelectedItem != null)
		{
			currentlySelectedItem.IsSelected = false;
		}
		currentlySelectedItem = (ObjectReferencePickerItem)item;
		currentlySelectedObject = currentlySelectedItem.Reference;
		currentlySelectedItem.IsSelected = true;
		try
		{
			if (onReferenceChanged != null)
			{
				onReferenceChanged(currentlySelectedObject);
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	public static void DestroyInstance()
	{
		if ((bool)m_instance)
		{
			RuntimeInspectorUtils.IgnoredTransformsInHierarchy.Remove(m_instance.transform);
			UnityEngine.Object.Destroy(m_instance);
			m_instance = null;
		}
	}
}
