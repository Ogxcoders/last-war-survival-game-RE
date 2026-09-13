using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;

public class F2mPoint : AbstractF2mPoint
{
	public override ECFieldElement YCoord
	{
		get
		{
			int curveCoordinateSystem = CurveCoordinateSystem;
			if ((uint)(curveCoordinateSystem - 5) <= 1u)
			{
				ECFieldElement rawXCoord = base.RawXCoord;
				ECFieldElement rawYCoord = base.RawYCoord;
				if (base.IsInfinity || rawXCoord.IsZero)
				{
					return rawYCoord;
				}
				ECFieldElement eCFieldElement = rawYCoord.Add(rawXCoord).Multiply(rawXCoord);
				if (6 == curveCoordinateSystem)
				{
					ECFieldElement eCFieldElement2 = base.RawZCoords[0];
					if (!eCFieldElement2.IsOne)
					{
						eCFieldElement = eCFieldElement.Divide(eCFieldElement2);
					}
				}
				return eCFieldElement;
			}
			return base.RawYCoord;
		}
	}

	protected internal override bool CompressionYTilde
	{
		get
		{
			ECFieldElement rawXCoord = base.RawXCoord;
			if (rawXCoord.IsZero)
			{
				return false;
			}
			ECFieldElement rawYCoord = base.RawYCoord;
			int curveCoordinateSystem = CurveCoordinateSystem;
			if ((uint)(curveCoordinateSystem - 5) <= 1u)
			{
				return rawYCoord.TestBitZero() != rawXCoord.TestBitZero();
			}
			return rawYCoord.Divide(rawXCoord).TestBitZero();
		}
	}

	[Obsolete("Use ECCurve.CreatePoint to construct points")]
	public F2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y)
		: this(curve, x, y, withCompression: false)
	{
	}

	[Obsolete("Per-point compression property will be removed, see GetEncoded(bool)")]
	public F2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression)
		: base(curve, x, y, withCompression)
	{
		if (x == null != (y == null))
		{
			throw new ArgumentException("Exactly one of the field elements is null");
		}
		if (x != null)
		{
			F2mFieldElement.CheckFieldElements(x, y);
			if (curve != null)
			{
				F2mFieldElement.CheckFieldElements(x, curve.A);
			}
		}
	}

	internal F2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		: base(curve, x, y, zs, withCompression)
	{
	}

	protected override ECPoint Detach()
	{
		return new F2mPoint(null, AffineXCoord, AffineYCoord, withCompression: false);
	}

	public override ECPoint Add(ECPoint b)
	{
		if (base.IsInfinity)
		{
			return b;
		}
		if (b.IsInfinity)
		{
			return this;
		}
		ECCurve curve = Curve;
		int coordinateSystem = curve.CoordinateSystem;
		ECFieldElement rawXCoord = base.RawXCoord;
		ECFieldElement rawXCoord2 = b.RawXCoord;
		switch (coordinateSystem)
		{
		case 0:
		{
			ECFieldElement rawYCoord5 = base.RawYCoord;
			ECFieldElement rawYCoord6 = b.RawYCoord;
			ECFieldElement eCFieldElement28 = rawXCoord.Add(rawXCoord2);
			ECFieldElement eCFieldElement29 = rawYCoord5.Add(rawYCoord6);
			if (eCFieldElement28.IsZero)
			{
				if (eCFieldElement29.IsZero)
				{
					return Twice();
				}
				return curve.Infinity;
			}
			ECFieldElement eCFieldElement30 = eCFieldElement29.Divide(eCFieldElement28);
			ECFieldElement eCFieldElement31 = eCFieldElement30.Square().Add(eCFieldElement30).Add(eCFieldElement28)
				.Add(curve.A);
			ECFieldElement y3 = eCFieldElement30.Multiply(rawXCoord.Add(eCFieldElement31)).Add(eCFieldElement31).Add(rawYCoord5);
			return new F2mPoint(curve, eCFieldElement31, y3, base.IsCompressed);
		}
		case 1:
		{
			ECFieldElement rawYCoord3 = base.RawYCoord;
			ECFieldElement eCFieldElement15 = base.RawZCoords[0];
			ECFieldElement rawYCoord4 = b.RawYCoord;
			ECFieldElement eCFieldElement16 = b.RawZCoords[0];
			bool isOne3 = eCFieldElement15.IsOne;
			ECFieldElement eCFieldElement17 = rawYCoord4;
			ECFieldElement eCFieldElement18 = rawXCoord2;
			if (!isOne3)
			{
				eCFieldElement17 = eCFieldElement17.Multiply(eCFieldElement15);
				eCFieldElement18 = eCFieldElement18.Multiply(eCFieldElement15);
			}
			bool isOne4 = eCFieldElement16.IsOne;
			ECFieldElement eCFieldElement19 = rawYCoord3;
			ECFieldElement eCFieldElement20 = rawXCoord;
			if (!isOne4)
			{
				eCFieldElement19 = eCFieldElement19.Multiply(eCFieldElement16);
				eCFieldElement20 = eCFieldElement20.Multiply(eCFieldElement16);
			}
			ECFieldElement eCFieldElement21 = eCFieldElement17.Add(eCFieldElement19);
			ECFieldElement eCFieldElement22 = eCFieldElement18.Add(eCFieldElement20);
			if (eCFieldElement22.IsZero)
			{
				if (eCFieldElement21.IsZero)
				{
					return Twice();
				}
				return curve.Infinity;
			}
			ECFieldElement eCFieldElement23 = eCFieldElement22.Square();
			ECFieldElement eCFieldElement24 = eCFieldElement23.Multiply(eCFieldElement22);
			ECFieldElement b3 = (isOne3 ? eCFieldElement16 : (isOne4 ? eCFieldElement15 : eCFieldElement15.Multiply(eCFieldElement16)));
			ECFieldElement eCFieldElement25 = eCFieldElement21.Add(eCFieldElement22);
			ECFieldElement eCFieldElement26 = eCFieldElement25.MultiplyPlusProduct(eCFieldElement21, eCFieldElement23, curve.A).Multiply(b3).Add(eCFieldElement24);
			ECFieldElement x = eCFieldElement22.Multiply(eCFieldElement26);
			ECFieldElement b4 = (isOne4 ? eCFieldElement23 : eCFieldElement23.Multiply(eCFieldElement16));
			ECFieldElement y2 = eCFieldElement21.MultiplyPlusProduct(rawXCoord, eCFieldElement22, rawYCoord3).MultiplyPlusProduct(b4, eCFieldElement25, eCFieldElement26);
			ECFieldElement eCFieldElement27 = eCFieldElement24.Multiply(b3);
			return new F2mPoint(curve, x, y2, new ECFieldElement[1] { eCFieldElement27 }, base.IsCompressed);
		}
		case 6:
		{
			if (rawXCoord.IsZero)
			{
				if (rawXCoord2.IsZero)
				{
					return curve.Infinity;
				}
				return b.Add(this);
			}
			ECFieldElement rawYCoord = base.RawYCoord;
			ECFieldElement eCFieldElement = base.RawZCoords[0];
			ECFieldElement rawYCoord2 = b.RawYCoord;
			ECFieldElement eCFieldElement2 = b.RawZCoords[0];
			bool isOne = eCFieldElement.IsOne;
			ECFieldElement eCFieldElement3 = rawXCoord2;
			ECFieldElement eCFieldElement4 = rawYCoord2;
			if (!isOne)
			{
				eCFieldElement3 = eCFieldElement3.Multiply(eCFieldElement);
				eCFieldElement4 = eCFieldElement4.Multiply(eCFieldElement);
			}
			bool isOne2 = eCFieldElement2.IsOne;
			ECFieldElement eCFieldElement5 = rawXCoord;
			ECFieldElement eCFieldElement6 = rawYCoord;
			if (!isOne2)
			{
				eCFieldElement5 = eCFieldElement5.Multiply(eCFieldElement2);
				eCFieldElement6 = eCFieldElement6.Multiply(eCFieldElement2);
			}
			ECFieldElement eCFieldElement7 = eCFieldElement6.Add(eCFieldElement4);
			ECFieldElement eCFieldElement8 = eCFieldElement5.Add(eCFieldElement3);
			if (eCFieldElement8.IsZero)
			{
				if (eCFieldElement7.IsZero)
				{
					return Twice();
				}
				return curve.Infinity;
			}
			ECFieldElement eCFieldElement10;
			ECFieldElement y;
			ECFieldElement eCFieldElement11;
			if (rawXCoord2.IsZero)
			{
				ECPoint eCPoint = Normalize();
				rawXCoord = eCPoint.RawXCoord;
				ECFieldElement yCoord = eCPoint.YCoord;
				ECFieldElement b2 = rawYCoord2;
				ECFieldElement eCFieldElement9 = yCoord.Add(b2).Divide(rawXCoord);
				eCFieldElement10 = eCFieldElement9.Square().Add(eCFieldElement9).Add(rawXCoord)
					.Add(curve.A);
				if (eCFieldElement10.IsZero)
				{
					return new F2mPoint(curve, eCFieldElement10, curve.B.Sqrt(), base.IsCompressed);
				}
				y = eCFieldElement9.Multiply(rawXCoord.Add(eCFieldElement10)).Add(eCFieldElement10).Add(yCoord)
					.Divide(eCFieldElement10)
					.Add(eCFieldElement10);
				eCFieldElement11 = curve.FromBigInteger(BigInteger.One);
			}
			else
			{
				eCFieldElement8 = eCFieldElement8.Square();
				ECFieldElement eCFieldElement12 = eCFieldElement7.Multiply(eCFieldElement5);
				ECFieldElement eCFieldElement13 = eCFieldElement7.Multiply(eCFieldElement3);
				eCFieldElement10 = eCFieldElement12.Multiply(eCFieldElement13);
				if (eCFieldElement10.IsZero)
				{
					return new F2mPoint(curve, eCFieldElement10, curve.B.Sqrt(), base.IsCompressed);
				}
				ECFieldElement eCFieldElement14 = eCFieldElement7.Multiply(eCFieldElement8);
				if (!isOne2)
				{
					eCFieldElement14 = eCFieldElement14.Multiply(eCFieldElement2);
				}
				y = eCFieldElement13.Add(eCFieldElement8).SquarePlusProduct(eCFieldElement14, rawYCoord.Add(eCFieldElement));
				eCFieldElement11 = eCFieldElement14;
				if (!isOne)
				{
					eCFieldElement11 = eCFieldElement11.Multiply(eCFieldElement);
				}
			}
			return new F2mPoint(curve, eCFieldElement10, y, new ECFieldElement[1] { eCFieldElement11 }, base.IsCompressed);
		}
		default:
			throw new InvalidOperationException("unsupported coordinate system");
		}
	}

	public override ECPoint Twice()
	{
		if (base.IsInfinity)
		{
			return this;
		}
		ECCurve curve = Curve;
		ECFieldElement rawXCoord = base.RawXCoord;
		if (rawXCoord.IsZero)
		{
			return curve.Infinity;
		}
		switch (curve.CoordinateSystem)
		{
		case 0:
		{
			ECFieldElement eCFieldElement10 = base.RawYCoord.Divide(rawXCoord).Add(rawXCoord);
			ECFieldElement x = eCFieldElement10.Square().Add(eCFieldElement10).Add(curve.A);
			ECFieldElement y = rawXCoord.SquarePlusProduct(x, eCFieldElement10.AddOne());
			return new F2mPoint(curve, x, y, base.IsCompressed);
		}
		case 1:
		{
			ECFieldElement rawYCoord2 = base.RawYCoord;
			ECFieldElement eCFieldElement11 = base.RawZCoords[0];
			bool isOne2 = eCFieldElement11.IsOne;
			ECFieldElement eCFieldElement12 = (isOne2 ? rawXCoord : rawXCoord.Multiply(eCFieldElement11));
			ECFieldElement b3 = (isOne2 ? rawYCoord2 : rawYCoord2.Multiply(eCFieldElement11));
			ECFieldElement eCFieldElement13 = rawXCoord.Square();
			ECFieldElement eCFieldElement14 = eCFieldElement13.Add(b3);
			ECFieldElement eCFieldElement15 = eCFieldElement12;
			ECFieldElement eCFieldElement16 = eCFieldElement15.Square();
			ECFieldElement eCFieldElement17 = eCFieldElement14.Add(eCFieldElement15);
			ECFieldElement eCFieldElement18 = eCFieldElement17.MultiplyPlusProduct(eCFieldElement14, eCFieldElement16, curve.A);
			ECFieldElement x2 = eCFieldElement15.Multiply(eCFieldElement18);
			ECFieldElement y2 = eCFieldElement13.Square().MultiplyPlusProduct(eCFieldElement15, eCFieldElement18, eCFieldElement17);
			ECFieldElement eCFieldElement19 = eCFieldElement15.Multiply(eCFieldElement16);
			return new F2mPoint(curve, x2, y2, new ECFieldElement[1] { eCFieldElement19 }, base.IsCompressed);
		}
		case 6:
		{
			ECFieldElement rawYCoord = base.RawYCoord;
			ECFieldElement eCFieldElement = base.RawZCoords[0];
			bool isOne = eCFieldElement.IsOne;
			ECFieldElement eCFieldElement2 = (isOne ? rawYCoord : rawYCoord.Multiply(eCFieldElement));
			ECFieldElement eCFieldElement3 = (isOne ? eCFieldElement : eCFieldElement.Square());
			ECFieldElement a = curve.A;
			ECFieldElement eCFieldElement4 = (isOne ? a : a.Multiply(eCFieldElement3));
			ECFieldElement eCFieldElement5 = rawYCoord.Square().Add(eCFieldElement2).Add(eCFieldElement4);
			if (eCFieldElement5.IsZero)
			{
				return new F2mPoint(curve, eCFieldElement5, curve.B.Sqrt(), base.IsCompressed);
			}
			ECFieldElement eCFieldElement6 = eCFieldElement5.Square();
			ECFieldElement eCFieldElement7 = (isOne ? eCFieldElement5 : eCFieldElement5.Multiply(eCFieldElement3));
			ECFieldElement b = curve.B;
			ECFieldElement eCFieldElement9;
			if (b.BitLength < curve.FieldSize >> 1)
			{
				ECFieldElement eCFieldElement8 = rawYCoord.Add(rawXCoord).Square();
				ECFieldElement b2 = ((!b.IsOne) ? eCFieldElement4.SquarePlusProduct(b, eCFieldElement3.Square()) : eCFieldElement4.Add(eCFieldElement3).Square());
				eCFieldElement9 = eCFieldElement8.Add(eCFieldElement5).Add(eCFieldElement3).Multiply(eCFieldElement8)
					.Add(b2)
					.Add(eCFieldElement6);
				if (a.IsZero)
				{
					eCFieldElement9 = eCFieldElement9.Add(eCFieldElement7);
				}
				else if (!a.IsOne)
				{
					eCFieldElement9 = eCFieldElement9.Add(a.AddOne().Multiply(eCFieldElement7));
				}
			}
			else
			{
				eCFieldElement9 = (isOne ? rawXCoord : rawXCoord.Multiply(eCFieldElement)).SquarePlusProduct(eCFieldElement5, eCFieldElement2).Add(eCFieldElement6).Add(eCFieldElement7);
			}
			return new F2mPoint(curve, eCFieldElement6, eCFieldElement9, new ECFieldElement[1] { eCFieldElement7 }, base.IsCompressed);
		}
		default:
			throw new InvalidOperationException("unsupported coordinate system");
		}
	}

	public override ECPoint TwicePlus(ECPoint b)
	{
		if (base.IsInfinity)
		{
			return b;
		}
		if (b.IsInfinity)
		{
			return Twice();
		}
		ECCurve curve = Curve;
		ECFieldElement rawXCoord = base.RawXCoord;
		if (rawXCoord.IsZero)
		{
			return b;
		}
		int coordinateSystem = curve.CoordinateSystem;
		if (coordinateSystem == 6)
		{
			ECFieldElement rawXCoord2 = b.RawXCoord;
			ECFieldElement eCFieldElement = b.RawZCoords[0];
			if (rawXCoord2.IsZero || !eCFieldElement.IsOne)
			{
				return Twice().Add(b);
			}
			ECFieldElement rawYCoord = base.RawYCoord;
			ECFieldElement eCFieldElement2 = base.RawZCoords[0];
			ECFieldElement rawYCoord2 = b.RawYCoord;
			ECFieldElement x = rawXCoord.Square();
			ECFieldElement b2 = rawYCoord.Square();
			ECFieldElement eCFieldElement3 = eCFieldElement2.Square();
			ECFieldElement b3 = rawYCoord.Multiply(eCFieldElement2);
			ECFieldElement b4 = curve.A.Multiply(eCFieldElement3).Add(b2).Add(b3);
			ECFieldElement eCFieldElement4 = rawYCoord2.AddOne();
			ECFieldElement eCFieldElement5 = curve.A.Add(eCFieldElement4).Multiply(eCFieldElement3).Add(b2)
				.MultiplyPlusProduct(b4, x, eCFieldElement3);
			ECFieldElement eCFieldElement6 = rawXCoord2.Multiply(eCFieldElement3);
			ECFieldElement eCFieldElement7 = eCFieldElement6.Add(b4).Square();
			if (eCFieldElement7.IsZero)
			{
				if (eCFieldElement5.IsZero)
				{
					return b.Twice();
				}
				return curve.Infinity;
			}
			if (eCFieldElement5.IsZero)
			{
				return new F2mPoint(curve, eCFieldElement5, curve.B.Sqrt(), base.IsCompressed);
			}
			ECFieldElement x2 = eCFieldElement5.Square().Multiply(eCFieldElement6);
			ECFieldElement eCFieldElement8 = eCFieldElement5.Multiply(eCFieldElement7).Multiply(eCFieldElement3);
			ECFieldElement y = eCFieldElement5.Add(eCFieldElement7).Square().MultiplyPlusProduct(b4, eCFieldElement4, eCFieldElement8);
			return new F2mPoint(curve, x2, y, new ECFieldElement[1] { eCFieldElement8 }, base.IsCompressed);
		}
		return Twice().Add(b);
	}

	public override ECPoint Negate()
	{
		if (base.IsInfinity)
		{
			return this;
		}
		ECFieldElement rawXCoord = base.RawXCoord;
		if (rawXCoord.IsZero)
		{
			return this;
		}
		ECCurve curve = Curve;
		switch (curve.CoordinateSystem)
		{
		case 0:
		{
			ECFieldElement rawYCoord4 = base.RawYCoord;
			return new F2mPoint(curve, rawXCoord, rawYCoord4.Add(rawXCoord), base.IsCompressed);
		}
		case 1:
		{
			ECFieldElement rawYCoord3 = base.RawYCoord;
			ECFieldElement eCFieldElement2 = base.RawZCoords[0];
			return new F2mPoint(curve, rawXCoord, rawYCoord3.Add(rawXCoord), new ECFieldElement[1] { eCFieldElement2 }, base.IsCompressed);
		}
		case 5:
		{
			ECFieldElement rawYCoord2 = base.RawYCoord;
			return new F2mPoint(curve, rawXCoord, rawYCoord2.AddOne(), base.IsCompressed);
		}
		case 6:
		{
			ECFieldElement rawYCoord = base.RawYCoord;
			ECFieldElement eCFieldElement = base.RawZCoords[0];
			return new F2mPoint(curve, rawXCoord, rawYCoord.Add(eCFieldElement), new ECFieldElement[1] { eCFieldElement }, base.IsCompressed);
		}
		default:
			throw new InvalidOperationException("unsupported coordinate system");
		}
	}
}
