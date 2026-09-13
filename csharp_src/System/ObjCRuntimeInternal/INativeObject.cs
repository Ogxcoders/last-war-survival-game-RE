using System;

namespace ObjCRuntimeInternal;

internal interface INativeObject
{
	IntPtr Handle { get; }
}
