using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ess;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tsp;

public class TimeStampTokenGenerator
{
	private int accuracySeconds = -1;

	private int accuracyMillis = -1;

	private int accuracyMicros = -1;

	private bool ordering;

	private GeneralName tsa;

	private string tsaPolicyOID;

	private AsymmetricKeyParameter key;

	private X509Certificate cert;

	private string digestOID;

	private BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr;

	private BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr;

	private IX509Store x509Certs;

	private IX509Store x509Crls;

	public TimeStampTokenGenerator(AsymmetricKeyParameter key, X509Certificate cert, string digestOID, string tsaPolicyOID)
		: this(key, cert, digestOID, tsaPolicyOID, null, null)
	{
	}

	public TimeStampTokenGenerator(AsymmetricKeyParameter key, X509Certificate cert, string digestOID, string tsaPolicyOID, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr)
	{
		this.key = key;
		this.cert = cert;
		this.digestOID = digestOID;
		this.tsaPolicyOID = tsaPolicyOID;
		this.unsignedAttr = unsignedAttr;
		TspUtil.ValidateCertificate(cert);
		IDictionary dictionary = ((signedAttr == null) ? Platform.CreateHashtable() : signedAttr.ToDictionary());
		try
		{
			EssCertID essCertID = new EssCertID(DigestUtilities.CalculateDigest("SHA-1", cert.GetEncoded()));
			BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.Attribute attribute = new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.Attribute(PkcsObjectIdentifiers.IdAASigningCertificate, new DerSet(new SigningCertificate(essCertID)));
			dictionary[attribute.AttrType] = attribute;
		}
		catch (CertificateEncodingException e)
		{
			throw new TspException("Exception processing certificate.", e);
		}
		catch (SecurityUtilityException e2)
		{
			throw new TspException("Can't find a SHA-1 implementation.", e2);
		}
		this.signedAttr = new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(dictionary);
	}

	public void SetCertificates(IX509Store certificates)
	{
		x509Certs = certificates;
	}

	public void SetCrls(IX509Store crls)
	{
		x509Crls = crls;
	}

	public void SetAccuracySeconds(int accuracySeconds)
	{
		this.accuracySeconds = accuracySeconds;
	}

	public void SetAccuracyMillis(int accuracyMillis)
	{
		this.accuracyMillis = accuracyMillis;
	}

	public void SetAccuracyMicros(int accuracyMicros)
	{
		this.accuracyMicros = accuracyMicros;
	}

	public void SetOrdering(bool ordering)
	{
		this.ordering = ordering;
	}

	public void SetTsa(GeneralName tsa)
	{
		this.tsa = tsa;
	}

	public TimeStampToken Generate(TimeStampRequest request, BigInteger serialNumber, DateTime genTime)
	{
		MessageImprint messageImprint = new MessageImprint(new AlgorithmIdentifier(new DerObjectIdentifier(request.MessageImprintAlgOid), DerNull.Instance), request.GetMessageImprintDigest());
		Accuracy accuracy = null;
		if (accuracySeconds > 0 || accuracyMillis > 0 || accuracyMicros > 0)
		{
			DerInteger seconds = null;
			if (accuracySeconds > 0)
			{
				seconds = new DerInteger(accuracySeconds);
			}
			DerInteger millis = null;
			if (accuracyMillis > 0)
			{
				millis = new DerInteger(accuracyMillis);
			}
			DerInteger micros = null;
			if (accuracyMicros > 0)
			{
				micros = new DerInteger(accuracyMicros);
			}
			accuracy = new Accuracy(seconds, millis, micros);
		}
		DerBoolean derBoolean = null;
		if (ordering)
		{
			derBoolean = DerBoolean.GetInstance(ordering);
		}
		DerInteger nonce = null;
		if (request.Nonce != null)
		{
			nonce = new DerInteger(request.Nonce);
		}
		DerObjectIdentifier tsaPolicyId = new DerObjectIdentifier(tsaPolicyOID);
		if (request.ReqPolicy != null)
		{
			tsaPolicyId = new DerObjectIdentifier(request.ReqPolicy);
		}
		TstInfo tstInfo = new TstInfo(tsaPolicyId, messageImprint, new DerInteger(serialNumber), new DerGeneralizedTime(genTime), accuracy, derBoolean, nonce, tsa, request.Extensions);
		try
		{
			CmsSignedDataGenerator cmsSignedDataGenerator = new CmsSignedDataGenerator();
			byte[] derEncoded = tstInfo.GetDerEncoded();
			if (request.CertReq)
			{
				cmsSignedDataGenerator.AddCertificates(x509Certs);
			}
			cmsSignedDataGenerator.AddCrls(x509Crls);
			cmsSignedDataGenerator.AddSigner(key, cert, digestOID, signedAttr, unsignedAttr);
			return new TimeStampToken(cmsSignedDataGenerator.Generate(PkcsObjectIdentifiers.IdCTTstInfo.Id, new CmsProcessableByteArray(derEncoded), encapsulate: true));
		}
		catch (CmsException e)
		{
			throw new TspException("Error generating time-stamp token", e);
		}
		catch (IOException e2)
		{
			throw new TspException("Exception encoding info", e2);
		}
		catch (X509StoreException e3)
		{
			throw new TspException("Exception handling CertStore", e3);
		}
	}
}
