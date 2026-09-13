using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Sec;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO.Pem;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.OpenSsl;

public class PemReader : BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO.Pem.PemReader
{
	private readonly IPasswordFinder pFinder;

	static PemReader()
	{
	}

	public PemReader(TextReader reader)
		: this(reader, null)
	{
	}

	public PemReader(TextReader reader, IPasswordFinder pFinder)
		: base(reader)
	{
		this.pFinder = pFinder;
	}

	public object ReadObject()
	{
		PemObject pemObject = ReadPemObject();
		if (pemObject == null)
		{
			return null;
		}
		if (Platform.EndsWith(pemObject.Type, "PRIVATE KEY"))
		{
			return ReadPrivateKey(pemObject);
		}
		switch (pemObject.Type)
		{
		case "PUBLIC KEY":
			return ReadPublicKey(pemObject);
		case "RSA PUBLIC KEY":
			return ReadRsaPublicKey(pemObject);
		case "CERTIFICATE REQUEST":
		case "NEW CERTIFICATE REQUEST":
			return ReadCertificateRequest(pemObject);
		case "CERTIFICATE":
		case "X509 CERTIFICATE":
			return ReadCertificate(pemObject);
		case "PKCS7":
		case "CMS":
			return ReadPkcs7(pemObject);
		case "X509 CRL":
			return ReadCrl(pemObject);
		case "ATTRIBUTE CERTIFICATE":
			return ReadAttributeCertificate(pemObject);
		default:
			throw new IOException("unrecognised object: " + pemObject.Type);
		}
	}

	private AsymmetricKeyParameter ReadRsaPublicKey(PemObject pemObject)
	{
		RsaPublicKeyStructure instance = RsaPublicKeyStructure.GetInstance(Asn1Object.FromByteArray(pemObject.Content));
		return new RsaKeyParameters(isPrivate: false, instance.Modulus, instance.PublicExponent);
	}

	private AsymmetricKeyParameter ReadPublicKey(PemObject pemObject)
	{
		return PublicKeyFactory.CreateKey(pemObject.Content);
	}

	private X509Certificate ReadCertificate(PemObject pemObject)
	{
		try
		{
			return new X509CertificateParser().ReadCertificate(pemObject.Content);
		}
		catch (Exception ex)
		{
			throw new PemException("problem parsing cert: " + ex.ToString());
		}
	}

	private X509Crl ReadCrl(PemObject pemObject)
	{
		try
		{
			return new X509CrlParser().ReadCrl(pemObject.Content);
		}
		catch (Exception ex)
		{
			throw new PemException("problem parsing cert: " + ex.ToString());
		}
	}

	private Pkcs10CertificationRequest ReadCertificateRequest(PemObject pemObject)
	{
		try
		{
			return new Pkcs10CertificationRequest(pemObject.Content);
		}
		catch (Exception ex)
		{
			throw new PemException("problem parsing cert: " + ex.ToString());
		}
	}

	private IX509AttributeCertificate ReadAttributeCertificate(PemObject pemObject)
	{
		return new X509V2AttributeCertificate(pemObject.Content);
	}

	private BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.ContentInfo ReadPkcs7(PemObject pemObject)
	{
		try
		{
			return BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.ContentInfo.GetInstance(Asn1Object.FromByteArray(pemObject.Content));
		}
		catch (Exception ex)
		{
			throw new PemException("problem parsing PKCS7 object: " + ex.ToString());
		}
	}

	private object ReadPrivateKey(PemObject pemObject)
	{
		string text = pemObject.Type.Substring(0, pemObject.Type.Length - "PRIVATE KEY".Length).Trim();
		byte[] array = pemObject.Content;
		IDictionary dictionary = Platform.CreateHashtable();
		foreach (PemHeader header in pemObject.Headers)
		{
			dictionary[header.Name] = header.Value;
		}
		if ((string)dictionary["Proc-Type"] == "4,ENCRYPTED")
		{
			if (pFinder == null)
			{
				throw new PasswordException("No password finder specified, but a password is required");
			}
			char[] password = pFinder.GetPassword();
			if (password == null)
			{
				throw new PasswordException("Password is null, but a password is required");
			}
			string[] array2 = ((string)dictionary["DEK-Info"]).Split(new char[1] { ',' });
			string dekAlgName = array2[0].Trim();
			byte[] iv = Hex.Decode(array2[1].Trim());
			array = PemUtilities.Crypt(encrypt: false, array, password, dekAlgName, iv);
		}
		try
		{
			Asn1Sequence instance = Asn1Sequence.GetInstance(array);
			AsymmetricKeyParameter publicParameter;
			AsymmetricKeyParameter asymmetricKeyParameter;
			switch (text)
			{
			default:
				if (text.Length != 0)
				{
					goto case null;
				}
				return PrivateKeyFactory.CreateKey(PrivateKeyInfo.GetInstance(instance));
			case "RSA":
			{
				if (instance.Count != 9)
				{
					throw new PemException("malformed sequence in RSA private key");
				}
				RsaPrivateKeyStructure instance3 = RsaPrivateKeyStructure.GetInstance(instance);
				publicParameter = new RsaKeyParameters(isPrivate: false, instance3.Modulus, instance3.PublicExponent);
				asymmetricKeyParameter = new RsaPrivateCrtKeyParameters(instance3.Modulus, instance3.PublicExponent, instance3.PrivateExponent, instance3.Prime1, instance3.Prime2, instance3.Exponent1, instance3.Exponent2, instance3.Coefficient);
				break;
			}
			case "DSA":
			{
				if (instance.Count != 6)
				{
					throw new PemException("malformed sequence in DSA private key");
				}
				DerInteger derInteger = (DerInteger)instance[1];
				DerInteger derInteger2 = (DerInteger)instance[2];
				DerInteger derInteger3 = (DerInteger)instance[3];
				DerInteger obj = (DerInteger)instance[4];
				DerInteger obj2 = (DerInteger)instance[5];
				DsaParameters parameters = new DsaParameters(derInteger.Value, derInteger2.Value, derInteger3.Value);
				asymmetricKeyParameter = new DsaPrivateKeyParameters(obj2.Value, parameters);
				publicParameter = new DsaPublicKeyParameters(obj.Value, parameters);
				break;
			}
			case "EC":
			{
				ECPrivateKeyStructure instance2 = ECPrivateKeyStructure.GetInstance(instance);
				AlgorithmIdentifier algorithmIdentifier = new AlgorithmIdentifier(X9ObjectIdentifiers.IdECPublicKey, instance2.GetParameters());
				asymmetricKeyParameter = PrivateKeyFactory.CreateKey(new PrivateKeyInfo(algorithmIdentifier, instance2.ToAsn1Object()));
				DerBitString publicKey = instance2.GetPublicKey();
				publicParameter = ((publicKey == null) ? ECKeyPairGenerator.GetCorrespondingPublicKey((ECPrivateKeyParameters)asymmetricKeyParameter) : PublicKeyFactory.CreateKey(new SubjectPublicKeyInfo(algorithmIdentifier, publicKey.GetBytes())));
				break;
			}
			case "ENCRYPTED":
				return PrivateKeyFactory.DecryptKey(pFinder.GetPassword() ?? throw new PasswordException("Password is null, but a password is required"), EncryptedPrivateKeyInfo.GetInstance(instance));
			case null:
				throw new ArgumentException("Unknown key type: " + text, "type");
			}
			return new AsymmetricCipherKeyPair(publicParameter, asymmetricKeyParameter);
		}
		catch (IOException ex)
		{
			throw ex;
		}
		catch (Exception ex2)
		{
			throw new PemException("problem creating " + text + " private key: " + ex2.ToString());
		}
	}

	private static X9ECParameters GetCurveParameters(string name)
	{
		X9ECParameters byName = CustomNamedCurves.GetByName(name);
		if (byName == null)
		{
			byName = ECNamedCurveTable.GetByName(name);
		}
		if (byName == null)
		{
			throw new Exception("unknown curve name: " + name);
		}
		return byName;
	}
}
