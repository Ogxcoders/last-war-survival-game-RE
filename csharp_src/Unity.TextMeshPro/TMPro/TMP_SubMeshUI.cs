using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace TMPro;

[ExecuteAlways]
public class TMP_SubMeshUI : MaskableGraphic
{
	[SerializeField]
	private TMP_FontAsset m_fontAsset;

	[SerializeField]
	private TMP_SpriteAsset m_spriteAsset;

	[SerializeField]
	private Material m_material;

	[SerializeField]
	private Material m_sharedMaterial;

	private Material m_fallbackMaterial;

	private Material m_fallbackSourceMaterial;

	[SerializeField]
	private bool m_isDefaultMaterial;

	[SerializeField]
	private float m_padding;

	private Mesh m_mesh;

	[SerializeField]
	private TextMeshProUGUI m_TextComponent;

	[NonSerialized]
	private bool m_isRegisteredForEvents;

	private bool m_materialDirty;

	[SerializeField]
	private int m_materialReferenceIndex;

	private Transform m_RootCanvasTransform;

	public TMP_FontAsset fontAsset
	{
		get
		{
			return m_fontAsset;
		}
		set
		{
			m_fontAsset = value;
		}
	}

	public TMP_SpriteAsset spriteAsset
	{
		get
		{
			return m_spriteAsset;
		}
		set
		{
			m_spriteAsset = value;
		}
	}

	public override Texture mainTexture
	{
		get
		{
			if (sharedMaterial != null)
			{
				return sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex);
			}
			return null;
		}
	}

	public override Material material
	{
		get
		{
			return GetMaterial(m_sharedMaterial);
		}
		set
		{
			if (!(m_sharedMaterial != null) || m_sharedMaterial.GetInstanceID() != value.GetInstanceID())
			{
				m_sharedMaterial = (m_material = value);
				m_padding = GetPaddingForMaterial();
				SetVerticesDirty();
				SetMaterialDirty();
			}
		}
	}

	public Material sharedMaterial
	{
		get
		{
			return m_sharedMaterial;
		}
		set
		{
			SetSharedMaterial(value);
		}
	}

	public Material fallbackMaterial
	{
		get
		{
			return m_fallbackMaterial;
		}
		set
		{
			if (!(m_fallbackMaterial == value))
			{
				if (m_fallbackMaterial != null && m_fallbackMaterial != value)
				{
					TMP_MaterialManager.ReleaseFallbackMaterial(m_fallbackMaterial);
				}
				m_fallbackMaterial = value;
				TMP_MaterialManager.AddFallbackMaterialReference(m_fallbackMaterial);
				SetSharedMaterial(m_fallbackMaterial);
			}
		}
	}

	public Material fallbackSourceMaterial
	{
		get
		{
			return m_fallbackSourceMaterial;
		}
		set
		{
			m_fallbackSourceMaterial = value;
		}
	}

	public override Material materialForRendering => TMP_MaterialManager.GetMaterialForRendering(this, m_sharedMaterial);

	public bool isDefaultMaterial
	{
		get
		{
			return m_isDefaultMaterial;
		}
		set
		{
			m_isDefaultMaterial = value;
		}
	}

	public float padding
	{
		get
		{
			return m_padding;
		}
		set
		{
			m_padding = value;
		}
	}

	public Mesh mesh
	{
		get
		{
			if (m_mesh == null)
			{
				m_mesh = new Mesh();
				m_mesh.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_mesh;
		}
		set
		{
			m_mesh = value;
		}
	}

	public TMP_Text textComponent
	{
		get
		{
			if (m_TextComponent == null)
			{
				m_TextComponent = GetComponentInParent<TextMeshProUGUI>();
			}
			return m_TextComponent;
		}
	}

	public static TMP_SubMeshUI AddSubTextObject(TextMeshProUGUI textComponent, MaterialReference materialReference)
	{
		GameObject obj = new GameObject("TMP UI SubObject [" + materialReference.material.name + "]", typeof(RectTransform));
		obj.hideFlags = HideFlags.DontSave;
		obj.transform.SetParent(textComponent.transform, worldPositionStays: false);
		obj.transform.SetAsFirstSibling();
		obj.layer = textComponent.gameObject.layer;
		RectTransform component = obj.GetComponent<RectTransform>();
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.sizeDelta = Vector2.zero;
		component.pivot = textComponent.rectTransform.pivot;
		obj.AddComponent<LayoutElement>().ignoreLayout = true;
		TMP_SubMeshUI tMP_SubMeshUI = obj.AddComponent<TMP_SubMeshUI>();
		tMP_SubMeshUI.m_TextComponent = textComponent;
		tMP_SubMeshUI.raycastTarget = textComponent.raycastTarget;
		tMP_SubMeshUI.m_materialReferenceIndex = materialReference.index;
		tMP_SubMeshUI.m_fontAsset = materialReference.fontAsset;
		tMP_SubMeshUI.m_spriteAsset = materialReference.spriteAsset;
		tMP_SubMeshUI.m_isDefaultMaterial = materialReference.isDefaultMaterial;
		tMP_SubMeshUI.SetSharedMaterial(materialReference.material);
		return tMP_SubMeshUI;
	}

	protected override void OnEnable()
	{
		if (!m_isRegisteredForEvents)
		{
			m_isRegisteredForEvents = true;
		}
		if (base.hideFlags != HideFlags.DontSave)
		{
			base.hideFlags = HideFlags.DontSave;
		}
		m_ShouldRecalculateStencil = true;
		RecalculateClipping();
		RecalculateMasking();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (m_fallbackMaterial != null)
		{
			TMP_MaterialManager.ReleaseFallbackMaterial(m_fallbackMaterial);
			m_fallbackMaterial = null;
		}
	}

	protected override void OnDestroy()
	{
		if (m_mesh != null)
		{
			UnityEngine.Object.DestroyImmediate(m_mesh);
		}
		if (m_MaskMaterial != null)
		{
			TMP_MaterialManager.ReleaseStencilMaterial(m_MaskMaterial);
		}
		if (m_fallbackMaterial != null)
		{
			TMP_MaterialManager.ReleaseFallbackMaterial(m_fallbackMaterial);
			m_fallbackMaterial = null;
		}
		m_isRegisteredForEvents = false;
		RecalculateClipping();
		if (m_TextComponent != null)
		{
			m_TextComponent.havePropertiesChanged = true;
			m_TextComponent.SetAllDirty();
		}
	}

	protected override void OnTransformParentChanged()
	{
		if (IsActive())
		{
			m_ShouldRecalculateStencil = true;
			RecalculateClipping();
			RecalculateMasking();
		}
	}

	public override Material GetModifiedMaterial(Material baseMaterial)
	{
		Material material = baseMaterial;
		if (m_ShouldRecalculateStencil)
		{
			Transform stopAfter = MaskUtilities.FindRootSortOverrideCanvas(base.transform);
			m_StencilValue = (base.maskable ? MaskUtilities.GetStencilDepth(base.transform, stopAfter) : 0);
			m_ShouldRecalculateStencil = false;
		}
		if (m_StencilValue > 0)
		{
			Material maskMaterial = StencilMaterial.Add(material, (1 << m_StencilValue) - 1, StencilOp.Keep, CompareFunction.Equal, ColorWriteMask.All, (1 << m_StencilValue) - 1, 0);
			StencilMaterial.Remove(m_MaskMaterial);
			m_MaskMaterial = maskMaterial;
			material = m_MaskMaterial;
		}
		return material;
	}

	public float GetPaddingForMaterial()
	{
		return ShaderUtilities.GetPadding(m_sharedMaterial, m_TextComponent.extraPadding, m_TextComponent.isUsingBold);
	}

	public float GetPaddingForMaterial(Material mat)
	{
		return ShaderUtilities.GetPadding(mat, m_TextComponent.extraPadding, m_TextComponent.isUsingBold);
	}

	public void UpdateMeshPadding(bool isExtraPadding, bool isUsingBold)
	{
		m_padding = ShaderUtilities.GetPadding(m_sharedMaterial, isExtraPadding, isUsingBold);
	}

	public override void SetAllDirty()
	{
	}

	public override void SetVerticesDirty()
	{
		if (IsActive() && m_TextComponent != null)
		{
			m_TextComponent.havePropertiesChanged = true;
			m_TextComponent.SetVerticesDirty();
		}
	}

	public override void SetLayoutDirty()
	{
	}

	public override void SetMaterialDirty()
	{
		m_materialDirty = true;
		UpdateMaterial();
		if (m_OnDirtyMaterialCallback != null)
		{
			m_OnDirtyMaterialCallback();
		}
	}

	public void SetPivotDirty()
	{
		if (IsActive())
		{
			base.rectTransform.pivot = m_TextComponent.rectTransform.pivot;
		}
	}

	private Transform GetRootCanvasTransform()
	{
		if (m_RootCanvasTransform == null)
		{
			m_RootCanvasTransform = m_TextComponent.canvas.rootCanvas.transform;
		}
		return m_RootCanvasTransform;
	}

	public override void Cull(Rect clipRect, bool validRect)
	{
	}

	protected override void UpdateGeometry()
	{
	}

	public override void Rebuild(CanvasUpdate update)
	{
		if (update == CanvasUpdate.PreRender && m_materialDirty)
		{
			UpdateMaterial();
			m_materialDirty = false;
		}
	}

	public void RefreshMaterial()
	{
		UpdateMaterial();
	}

	protected override void UpdateMaterial()
	{
		if (!(m_sharedMaterial == null))
		{
			if (m_sharedMaterial.HasProperty(ShaderUtilities.ShaderTag_CullMode) && textComponent != null && textComponent.fontSharedMaterial != null)
			{
				float value = textComponent.fontSharedMaterial.GetFloat(ShaderUtilities.ShaderTag_CullMode);
				m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_CullMode, value);
			}
			base.canvasRenderer.materialCount = 1;
			base.canvasRenderer.SetMaterial(materialForRendering, 0);
		}
	}

	public override void RecalculateClipping()
	{
		base.RecalculateClipping();
	}

	private Material GetMaterial()
	{
		return m_sharedMaterial;
	}

	private Material GetMaterial(Material mat)
	{
		if (m_material == null || m_material.GetInstanceID() != mat.GetInstanceID())
		{
			m_material = CreateMaterialInstance(mat);
		}
		m_sharedMaterial = m_material;
		m_padding = GetPaddingForMaterial();
		SetVerticesDirty();
		SetMaterialDirty();
		return m_sharedMaterial;
	}

	private Material CreateMaterialInstance(Material source)
	{
		Material obj = new Material(source)
		{
			shaderKeywords = source.shaderKeywords
		};
		obj.name += " (Instance)";
		return obj;
	}

	private Material GetSharedMaterial()
	{
		return base.canvasRenderer.GetMaterial();
	}

	private void SetSharedMaterial(Material mat)
	{
		m_sharedMaterial = mat;
		m_Material = m_sharedMaterial;
		m_padding = GetPaddingForMaterial();
		SetMaterialDirty();
	}
}
