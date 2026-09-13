using System;
using System.Collections.Generic;
using TMPro;

namespace UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Graphic))]
public class SoftMaskable : MonoBehaviour, IMaterialModifier, ICanvasRaycastFilter
{
	private const int kVisibleInside = 85;

	private const int kVisibleOutside = 170;

	private static readonly Hash128 k_InvalidHash = default(Hash128);

	private static int s_SoftMaskTexId;

	private static int s_StencilCompId;

	private static int s_MaskInteractionId;

	private static int s_GameVPId;

	private static int s_GameTVPId;

	private static List<SoftMaskable> s_ActiveSoftMaskables;

	private static int[] s_Interactions = new int[4];

	[SerializeField]
	[HideInInspector]
	[Obsolete]
	private bool m_Inverse;

	[SerializeField]
	[Tooltip("The interaction for each masks.")]
	[HideInInspector]
	private int m_MaskInteraction = 85;

	[SerializeField]
	[Tooltip("Use stencil to mask.")]
	private bool m_UseStencil = true;

	[SerializeField]
	[Tooltip("Use soft-masked raycast target.\n\nNote: This option is expensive.")]
	private bool m_RaycastFilter;

	private Graphic _graphic;

	private SoftMask _softMask;

	private Hash128 _effectMaterialHash;

	private Image _image;

	private bool _isGray;

	public bool inverse
	{
		get
		{
			return m_MaskInteraction == 170;
		}
		set
		{
			int num = (value ? 170 : 85);
			if (m_MaskInteraction != num)
			{
				m_MaskInteraction = num;
				graphic.SetMaterialDirtyEx();
			}
		}
	}

	public bool raycastFilter
	{
		get
		{
			return m_RaycastFilter;
		}
		set
		{
			m_RaycastFilter = value;
		}
	}

	public bool useStencil
	{
		get
		{
			return m_UseStencil;
		}
		set
		{
			if (m_UseStencil != value)
			{
				m_UseStencil = value;
				graphic.SetMaterialDirtyEx();
			}
		}
	}

	public Graphic graphic
	{
		get
		{
			if (!_graphic)
			{
				return _graphic = GetComponent<Graphic>();
			}
			return _graphic;
		}
	}

	public SoftMask softMask
	{
		get
		{
			if (!_softMask)
			{
				return _softMask = this.GetComponentInParentEx<SoftMask>();
			}
			return _softMask;
		}
	}

	public Material modifiedMaterial { get; private set; }

	public Image grayImage
	{
		get
		{
			if (!_image)
			{
				return _image = GetComponent<Image>();
			}
			return _image;
		}
	}

	Material IMaterialModifier.GetModifiedMaterial(Material baseMaterial)
	{
		_softMask = null;
		modifiedMaterial = null;
		if (!base.isActiveAndEnabled || !softMask)
		{
			MaterialCache.Unregister(_effectMaterialHash);
			_effectMaterialHash = k_InvalidHash;
			return baseMaterial;
		}
		Hash128 effectMaterialHash = _effectMaterialHash;
		int num = (m_UseStencil ? 1 : 0);
		if (_isGray)
		{
			num += 10;
		}
		_effectMaterialHash = new Hash128((uint)baseMaterial.GetInstanceID(), (uint)softMask.GetInstanceID(), (uint)m_MaskInteraction, (uint)num);
		modifiedMaterial = MaterialCache.Register(baseMaterial, _effectMaterialHash, delegate(Material mat)
		{
			if (graphic is TextMeshProUGUI || graphic is TMP_SubMeshUI)
			{
				mat.shader = softMask.materialableText.shader;
			}
			else if (_isGray)
			{
				mat.shader = softMask.materialableGray.shader;
			}
			else
			{
				mat.shader = softMask.materialable.shader;
			}
			mat.SetTexture(s_SoftMaskTexId, softMask.softMaskBuffer);
			mat.SetInt(s_StencilCompId, m_UseStencil ? 3 : 8);
			Transform stopAfter = MaskUtilities.FindRootSortOverrideCanvas(base.transform);
			int stencilDepth = MaskUtilities.GetStencilDepth(base.transform, stopAfter);
			mat.SetVector(s_MaskInteractionId, new Vector4((1 <= stencilDepth) ? (m_MaskInteraction & 3) : 0, (2 <= stencilDepth) ? ((m_MaskInteraction >> 2) & 3) : 0, (3 <= stencilDepth) ? ((m_MaskInteraction >> 4) & 3) : 0, (4 <= stencilDepth) ? ((m_MaskInteraction >> 6) & 3) : 0));
		});
		MaterialCache.Unregister(effectMaterialHash);
		return modifiedMaterial;
	}

	bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		if (!base.isActiveAndEnabled || !this.softMask)
		{
			return true;
		}
		if (!RectTransformUtility.RectangleContainsScreenPoint(base.transform as RectTransform, sp, eventCamera))
		{
			return false;
		}
		if (m_RaycastFilter)
		{
			SoftMask softMask = _softMask;
			for (int i = 0; i < 4; i++)
			{
				s_Interactions[i] = (softMask ? ((m_MaskInteraction >> i * 2) & 3) : 0);
				softMask = (softMask ? softMask.parent : null);
			}
			return _softMask.IsRaycastLocationValid(sp, eventCamera, graphic, s_Interactions);
		}
		SoftMask softMask2 = _softMask;
		for (int j = 0; j < 4; j++)
		{
			if (!softMask2)
			{
				break;
			}
			s_Interactions[j] = (softMask2 ? ((m_MaskInteraction >> j * 2) & 3) : 0);
			bool flag = s_Interactions[j] == 1;
			bool flag2 = RectTransformUtility.RectangleContainsScreenPoint(softMask2.transform as RectTransform, sp, eventCamera);
			if (!softMask2.ignoreSelfGraphic && flag != flag2)
			{
				return false;
			}
			foreach (SoftMask child in softMask2._children)
			{
				if ((bool)child)
				{
					bool flag3 = RectTransformUtility.RectangleContainsScreenPoint(child.transform as RectTransform, sp, eventCamera);
					if (!child.ignoreSelfGraphic && flag != flag3)
					{
						return false;
					}
				}
			}
			softMask2 = (softMask2 ? softMask2.parent : null);
		}
		return true;
	}

	public void SetMaskInteraction(SpriteMaskInteraction intr)
	{
		SetMaskInteraction(intr, intr, intr, intr);
	}

	public void SetMaskInteraction(SpriteMaskInteraction layer0, SpriteMaskInteraction layer1, SpriteMaskInteraction layer2, SpriteMaskInteraction layer3)
	{
		m_MaskInteraction = (int)(layer0 + ((int)layer1 << 2) + ((int)layer2 << 4) + ((int)layer3 << 6));
		graphic.SetMaterialDirtyEx();
	}

	private void OnEnable()
	{
		if (s_ActiveSoftMaskables == null)
		{
			s_ActiveSoftMaskables = new List<SoftMaskable>();
			s_SoftMaskTexId = Shader.PropertyToID("_SoftMaskTex");
			s_StencilCompId = Shader.PropertyToID("_StencilComp");
			s_MaskInteractionId = Shader.PropertyToID("_MaskInteraction");
		}
		s_ActiveSoftMaskables.Add(this);
		graphic.SetMaterialDirtyEx();
		_softMask = null;
	}

	private void OnDisable()
	{
		s_ActiveSoftMaskables.Remove(this);
		graphic.SetMaterialDirtyEx();
		_softMask = null;
		MaterialCache.Unregister(_effectMaterialHash);
		_effectMaterialHash = k_InvalidHash;
	}

	public void SetGray(bool isGray)
	{
		_isGray = isGray;
		graphic.SetMaterialDirtyEx();
	}
}
