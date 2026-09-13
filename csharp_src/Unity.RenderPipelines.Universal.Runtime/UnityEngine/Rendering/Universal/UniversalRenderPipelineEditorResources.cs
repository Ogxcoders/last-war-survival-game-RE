using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[MovedFrom("UnityEngine.Rendering.LWRP")]
public class UniversalRenderPipelineEditorResources : ScriptableObject
{
	[Serializable]
	[ReloadGroup]
	public sealed class ShaderResources
	{
		[Reload("Shaders/Autodesk Interactive/Autodesk Interactive.shadergraph", ReloadAttribute.Package.Root)]
		public Shader autodeskInteractivePS;

		[Reload("Shaders/Autodesk Interactive/Autodesk Interactive Transparent.shadergraph", ReloadAttribute.Package.Root)]
		public Shader autodeskInteractiveTransparentPS;

		[Reload("Shaders/Autodesk Interactive/Autodesk Interactive Masked.shadergraph", ReloadAttribute.Package.Root)]
		public Shader autodeskInteractiveMaskedPS;

		[Reload("Shaders/Terrain/TerrainDetailLit.shader", ReloadAttribute.Package.Root)]
		public Shader terrainDetailLitPS;

		[Reload("Shaders/Terrain/WavingGrass.shader", ReloadAttribute.Package.Root)]
		public Shader terrainDetailGrassPS;

		[Reload("Shaders/Terrain/WavingGrassBillboard.shader", ReloadAttribute.Package.Root)]
		public Shader terrainDetailGrassBillboardPS;

		[Reload("Shaders/Nature/SpeedTree7.shader", ReloadAttribute.Package.Root)]
		public Shader defaultSpeedTree7PS;

		[Reload("Shaders/Nature/SpeedTree8.shader", ReloadAttribute.Package.Root)]
		public Shader defaultSpeedTree8PS;
	}

	[Serializable]
	[ReloadGroup]
	public sealed class MaterialResources
	{
		[Reload("Runtime/Materials/Lit.mat", ReloadAttribute.Package.Root)]
		public Material lit;

		[Reload("Runtime/Materials/ParticlesLit.mat", ReloadAttribute.Package.Root)]
		public Material particleLit;

		[Reload("Runtime/Materials/TerrainLit.mat", ReloadAttribute.Package.Root)]
		public Material terrainLit;
	}

	public ShaderResources shaders;

	public MaterialResources materials;
}
