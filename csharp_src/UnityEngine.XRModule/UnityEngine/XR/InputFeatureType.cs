namespace UnityEngine.XR;

internal enum InputFeatureType : uint
{
	Custom = 0u,
	Binary = 1u,
	DiscreteStates = 2u,
	Axis1D = 3u,
	Axis2D = 4u,
	Axis3D = 5u,
	Rotation = 6u,
	Hand = 7u,
	Bone = 8u,
	Eyes = 9u,
	kUnityXRInputFeatureTypeInvalid = uint.MaxValue
}
