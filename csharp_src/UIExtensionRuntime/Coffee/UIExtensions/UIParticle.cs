using System.Collections;
using System.Collections.Generic;
using Coffee.UIParticleExtensions;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Coffee.UIExtensions;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasRenderer))]
public class UIParticle : MaskableGraphic
{
	public static bool ENABLE_STEP_BAKE_MESH = true;

	[Tooltip("Ignore canvas scaler")]
	[SerializeField]
	[FormerlySerializedAs("m_IgnoreParent")]
	private bool m_IgnoreCanvasScaler = true;

	[Tooltip("Particle effect scale")]
	[SerializeField]
	private float m_Scale = 100f;

	[Tooltip("Particle effect scale")]
	[SerializeField]
	private Vector3 m_Scale3D;

	[Tooltip("Animatable material properties. If you want to change the material properties of the ParticleSystem in Animation, enable it.")]
	[SerializeField]
	internal AnimatableProperty[] m_AnimatableProperties = new AnimatableProperty[0];

	[Tooltip("Particles")]
	[SerializeField]
	private List<ParticleSystem> m_Particles = new List<ParticleSystem>();

	[Tooltip("Shrink rendering by material on refresh.\nNOTE: Performance will be improved, but in some cases the rendering is not correct.")]
	[SerializeField]
	private bool m_ShrinkByMaterial;

	[Tooltip("Mesh Share Tag")]
	[SerializeField]
	private string m_shareMeshTag;

	private bool _shouldBeRemoved;

	private DrivenRectTransformTracker _tracker;

	private Mesh _bakedMesh;

	private readonly List<Material> _modifiedMaterials = new List<Material>();

	private readonly List<Material> _maskMaterials = new List<Material>();

	private readonly List<bool> _activeMeshIndices = new List<bool>();

	private Vector3 _cachedPosition;

	private static readonly List<Material> s_TempMaterials = new List<Material>(2);

	private static MaterialPropertyBlock s_Mpb;

	private static readonly List<Material> s_PrevMaskMaterials = new List<Material>();

	private static readonly List<Material> s_PrevModifiedMaterials = new List<Material>();

	private static readonly List<Component> s_Components = new List<Component>();

	private static readonly List<ParticleSystem> s_ParticleSystems = new List<ParticleSystem>();

	public string shareMeshTag => m_shareMeshTag;

	public override bool raycastTarget
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool ignoreCanvasScaler
	{
		get
		{
			return m_IgnoreCanvasScaler;
		}
		set
		{
			m_IgnoreCanvasScaler = value;
			_tracker.Clear();
			if (base.isActiveAndEnabled && m_IgnoreCanvasScaler)
			{
				_tracker.Add(this, base.rectTransform, DrivenTransformProperties.Scale);
			}
		}
	}

	public bool shrinkByMaterial
	{
		get
		{
			return m_ShrinkByMaterial;
		}
		set
		{
			if (m_ShrinkByMaterial != value)
			{
				m_ShrinkByMaterial = value;
				RefreshParticles();
			}
		}
	}

	public float scale
	{
		get
		{
			return m_Scale3D.x;
		}
		set
		{
			m_Scale = Mathf.Max(0.001f, value);
			m_Scale3D = new Vector3(m_Scale, m_Scale, m_Scale);
		}
	}

	public Vector3 scale3D
	{
		get
		{
			return m_Scale3D;
		}
		set
		{
			if (!(m_Scale3D == value))
			{
				m_Scale3D.x = Mathf.Max(0.001f, value.x);
				m_Scale3D.y = Mathf.Max(0.001f, value.y);
				m_Scale3D.z = Mathf.Max(0.001f, value.z);
			}
		}
	}

	internal Mesh bakedMesh => _bakedMesh;

	public List<ParticleSystem> particles => m_Particles;

	public IEnumerable<Material> materials => _modifiedMaterials;

	public override Material materialForRendering => base.canvasRenderer.GetMaterial(0);

	public List<bool> activeMeshIndices
	{
		get
		{
			return _activeMeshIndices;
		}
		set
		{
			if (!_activeMeshIndices.SequenceEqualFast(value))
			{
				_activeMeshIndices.Clear();
				_activeMeshIndices.AddRange(value);
				UpdateMaterial();
			}
		}
	}

	internal Vector3 cachedPosition
	{
		get
		{
			return _cachedPosition;
		}
		set
		{
			_cachedPosition = value;
		}
	}

	public void Play()
	{
		particles.Exec(delegate(ParticleSystem p)
		{
			p.Play();
		});
	}

	public void Pause()
	{
		particles.Exec(delegate(ParticleSystem p)
		{
			p.Pause();
		});
	}

	public void Stop()
	{
		particles.Exec(delegate(ParticleSystem p)
		{
			p.Stop();
		});
	}

	public void Clear()
	{
		particles.Exec(delegate(ParticleSystem p)
		{
			p.Clear();
		});
	}

	public void Simulate(float t, bool withChildren, bool restart)
	{
		particles.Exec(delegate(ParticleSystem p)
		{
			p.Simulate(t, withChildren, restart);
		});
		SetMaterialDirty();
		Canvas.ForceUpdateCanvases();
	}

	public void SetParticleSystemInstance(GameObject instance)
	{
		SetParticleSystemInstance(instance, destroyOldParticles: true);
	}

	public void SetParticleSystemInstance(GameObject instance, bool destroyOldParticles)
	{
		if (!instance)
		{
			return;
		}
		foreach (Transform item in base.transform)
		{
			GameObject gameObject = item.gameObject;
			gameObject.SetActive(value: false);
			if (destroyOldParticles)
			{
				Object.Destroy(gameObject);
			}
		}
		Transform obj = instance.transform;
		obj.SetParent(base.transform, worldPositionStays: false);
		obj.localPosition = Vector3.zero;
		RefreshParticles(instance);
	}

	public void SetParticleSystemPrefab(GameObject prefab)
	{
		if ((bool)prefab)
		{
			SetParticleSystemInstance(Object.Instantiate(prefab.gameObject), destroyOldParticles: true);
		}
	}

	public void RefreshParticles()
	{
		RefreshParticles(base.gameObject);
	}

	public void RefreshParticles(GameObject root)
	{
		if (!root)
		{
			return;
		}
		root.GetComponentsInChildren(particles);
		particles.RemoveAll((ParticleSystem x) => x.GetComponentInParent<UIParticle>() != this);
		foreach (ParticleSystem particle in particles)
		{
			ParticleSystem.TextureSheetAnimationModule textureSheetAnimation = particle.textureSheetAnimation;
			if (textureSheetAnimation.mode == ParticleSystemAnimationMode.Sprites && textureSheetAnimation.uvChannelMask == (UVChannelFlags)0)
			{
				textureSheetAnimation.uvChannelMask = UVChannelFlags.UV0;
			}
		}
		particles.Exec(delegate(ParticleSystem p)
		{
			p.GetComponent<ParticleSystemRenderer>().enabled = !base.enabled;
		});
		particles.SortForRendering(base.transform, m_ShrinkByMaterial);
		SetMaterialDirty();
	}

	protected override void UpdateMaterial()
	{
		s_PrevMaskMaterials.AddRange(_maskMaterials);
		_maskMaterials.Clear();
		s_PrevModifiedMaterials.AddRange(_modifiedMaterials);
		_modifiedMaterials.Clear();
		if (m_ShouldRecalculateStencil)
		{
			Transform stopAfter = MaskUtilities.FindRootSortOverrideCanvas(base.transform);
			m_StencilValue = (base.maskable ? MaskUtilities.GetStencilDepth(base.transform, stopAfter) : 0);
			m_ShouldRecalculateStencil = false;
		}
		int num = activeMeshIndices.CountFast();
		if (num == 0 || !base.isActiveAndEnabled || particles.Count == 0)
		{
			base.canvasRenderer.Clear();
			ClearPreviousMaterials();
			return;
		}
		GetComponents(typeof(IMaterialModifier), s_Components);
		int num2 = Mathf.Min(8, num);
		base.canvasRenderer.materialCount = num2;
		int num3 = 0;
		for (int i = 0; i < particles.Count; i++)
		{
			if (num2 <= num3)
			{
				break;
			}
			ParticleSystem particleSystem = particles[i];
			if (!particleSystem)
			{
				continue;
			}
			ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
			component.GetSharedMaterials(s_TempMaterials);
			int num4 = i * 2;
			if (activeMeshIndices.Count <= num4)
			{
				break;
			}
			if (activeMeshIndices[num4] && 0 < s_TempMaterials.Count)
			{
				Material modifiedMaterial = GetModifiedMaterial(s_TempMaterials[0], particleSystem.GetTextureForSprite());
				for (int j = 1; j < s_Components.Count; j++)
				{
					modifiedMaterial = (s_Components[j] as IMaterialModifier).GetModifiedMaterial(modifiedMaterial);
				}
				base.canvasRenderer.SetMaterial(modifiedMaterial, num3);
				UpdateMaterialProperties(component, num3);
				num3++;
			}
			num4++;
			if (activeMeshIndices.Count <= num4 || num2 <= num3)
			{
				break;
			}
			if (activeMeshIndices[num4] && 1 < s_TempMaterials.Count)
			{
				Material modifiedMaterial2 = GetModifiedMaterial(s_TempMaterials[1], null);
				for (int k = 1; k < s_Components.Count; k++)
				{
					modifiedMaterial2 = (s_Components[k] as IMaterialModifier).GetModifiedMaterial(modifiedMaterial2);
				}
				base.canvasRenderer.SetMaterial(modifiedMaterial2, num3++);
			}
		}
		ClearPreviousMaterials();
	}

	private void ClearPreviousMaterials()
	{
		foreach (Material s_PrevMaskMaterial in s_PrevMaskMaterials)
		{
			StencilMaterial.Remove(s_PrevMaskMaterial);
		}
		s_PrevMaskMaterials.Clear();
		foreach (Material s_PrevModifiedMaterial in s_PrevModifiedMaterials)
		{
			ModifiedMaterial.Remove(s_PrevModifiedMaterial);
		}
		s_PrevModifiedMaterials.Clear();
	}

	private Material GetModifiedMaterial(Material baseMaterial, Texture2D texture)
	{
		if (0 < m_StencilValue)
		{
			baseMaterial = StencilMaterial.Add(baseMaterial, (1 << m_StencilValue) - 1, StencilOp.Keep, CompareFunction.Equal, ColorWriteMask.All, (1 << m_StencilValue) - 1, 0);
			_maskMaterials.Add(baseMaterial);
		}
		ModifiedMaterial.DisableFog(baseMaterial);
		if (texture == null && m_AnimatableProperties.Length == 0)
		{
			return baseMaterial;
		}
		int id = ((m_AnimatableProperties.Length != 0) ? GetInstanceID() : 0);
		baseMaterial = ModifiedMaterial.Add(baseMaterial, texture, id);
		_modifiedMaterials.Add(baseMaterial);
		return baseMaterial;
	}

	internal void UpdateMaterialProperties()
	{
		if (m_AnimatableProperties.Length == 0)
		{
			return;
		}
		int b = activeMeshIndices.CountFast();
		int num = Mathf.Max(8, b);
		base.canvasRenderer.materialCount = num;
		int num2 = 0;
		for (int i = 0; i < particles.Count; i++)
		{
			if (num <= num2)
			{
				break;
			}
			ParticleSystem particleSystem = particles[i];
			if ((bool)particleSystem)
			{
				ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
				component.GetSharedMaterials(s_TempMaterials);
				if (activeMeshIndices[i * 2] && 0 < s_TempMaterials.Count)
				{
					UpdateMaterialProperties(component, num2);
					num2++;
				}
			}
		}
	}

	internal void UpdateMaterialProperties(Renderer r, int index)
	{
		if (m_AnimatableProperties.Length == 0 || base.canvasRenderer.materialCount <= index)
		{
			return;
		}
		r.GetPropertyBlock(s_Mpb ?? (s_Mpb = new MaterialPropertyBlock()));
		if (s_Mpb.isEmpty)
		{
			return;
		}
		Material material = base.canvasRenderer.GetMaterial(index);
		if ((bool)material)
		{
			AnimatableProperty[] animatableProperties = m_AnimatableProperties;
			for (int i = 0; i < animatableProperties.Length; i++)
			{
				animatableProperties[i].UpdateMaterialProperties(material, s_Mpb);
			}
			s_Mpb.Clear();
		}
	}

	protected override void OnEnable()
	{
		activeMeshIndices.Clear();
		UIParticleUpdater.Register(this);
		particles.Exec(delegate(ParticleSystem p)
		{
			p.GetComponent<ParticleSystemRenderer>().enabled = false;
		});
		if (base.isActiveAndEnabled && m_IgnoreCanvasScaler)
		{
			_tracker.Add(this, base.rectTransform, DrivenTransformProperties.Scale);
		}
		_bakedMesh = MeshPool.Rent();
		base.OnEnable();
		InitializeIfNeeded();
	}

	private new IEnumerator Start()
	{
		bool num = particles.AnyFast(delegate(ParticleSystem ps)
		{
			ps.GetComponentsInChildren(includeInactive: false, s_ParticleSystems);
			return s_ParticleSystems.AnyFast((ParticleSystem p) => p.isPlaying && (p.subEmitters.enabled || p.main.prewarm));
		});
		s_ParticleSystems.Clear();
		if (num)
		{
			Stop();
			Clear();
			yield return null;
			Play();
		}
	}

	protected override void OnDisable()
	{
		UIParticleUpdater.Unregister(this);
		if (!_shouldBeRemoved)
		{
			particles.Exec(delegate(ParticleSystem p)
			{
				p.GetComponent<ParticleSystemRenderer>().enabled = true;
			});
		}
		_tracker.Clear();
		MeshPool.Return(_bakedMesh);
		_bakedMesh = null;
		base.OnDisable();
	}

	protected override void UpdateGeometry()
	{
	}

	protected override void OnDidApplyAnimationProperties()
	{
	}

	private void InitializeIfNeeded()
	{
		if ((bool)this && !particles.AnyFast())
		{
			RefreshParticles();
		}
	}
}
