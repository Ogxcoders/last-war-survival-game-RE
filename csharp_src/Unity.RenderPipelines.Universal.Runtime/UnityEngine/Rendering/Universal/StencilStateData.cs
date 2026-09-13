using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[Serializable]
[MovedFrom("UnityEngine.Rendering.LWRP")]
public class StencilStateData
{
	public bool overrideStencilState;

	public int stencilReference;

	public CompareFunction stencilCompareFunction = CompareFunction.Always;

	public StencilOp passOperation;

	public StencilOp failOperation;

	public StencilOp zFailOperation;
}
