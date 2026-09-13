namespace System.Security.Cryptography;

public struct ECParameters
{
	public ECPoint Q;

	public byte[] D;

	public ECCurve Curve;

	public void Validate()
	{
		bool flag = false;
		if (Q.X == null || Q.Y == null || Q.X.Length != Q.Y.Length)
		{
			flag = true;
		}
		if (!flag)
		{
			if (Curve.IsExplicit)
			{
				flag = D != null && D.Length != Curve.Order.Length;
			}
			else if (Curve.IsNamed)
			{
				flag = D != null && D.Length != Q.X.Length;
			}
		}
		if (flag)
		{
			throw new CryptographicException("The specified key parameters are not valid. Q.X and Q.Y are required fields. Q.X, Q.Y must be the same length. If D is specified it must be the same length as Q.X and Q.Y for named curves or the same length as Order for explicit curves.");
		}
		Curve.Validate();
	}
}
