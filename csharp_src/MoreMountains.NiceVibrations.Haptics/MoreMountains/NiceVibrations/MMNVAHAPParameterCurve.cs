using System;
using System.Collections.Generic;

namespace MoreMountains.NiceVibrations;

[Serializable]
public class MMNVAHAPParameterCurve
{
	public string ParameterID;

	public double Time;

	public List<MMNVAHAPParameterCurveControlPoint> ParameterCurveControlPoints;
}
