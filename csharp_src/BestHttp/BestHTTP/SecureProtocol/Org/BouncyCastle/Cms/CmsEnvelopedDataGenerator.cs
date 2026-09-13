using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;

public class CmsEnvelopedDataGenerator : CmsEnvelopedGenerator
{
	public CmsEnvelopedDataGenerator()
	{
	}

	public CmsEnvelopedDataGenerator(SecureRandom rand)
		: base(rand)
	{
	}

	private CmsEnvelopedData Generate(CmsProcessable content, string encryptionOid, CipherKeyGenerator keyGen)
	{
		AlgorithmIdentifier algorithmIdentifier = null;
		KeyParameter keyParameter;
		Asn1OctetString encryptedContent;
		try
		{
			byte[] array = keyGen.GenerateKey();
			keyParameter = ParameterUtilities.CreateKeyParameter(encryptionOid, array);
			Asn1Encodable asn1Params = GenerateAsn1Parameters(encryptionOid, array);
			algorithmIdentifier = GetAlgorithmIdentifier(encryptionOid, keyParameter, asn1Params, out var cipherParameters);
			IBufferedCipher cipher = CipherUtilities.GetCipher(encryptionOid);
			cipher.Init(forEncryption: true, new ParametersWithRandom(cipherParameters, rand));
			MemoryStream memoryStream = new MemoryStream();
			CipherStream cipherStream = new CipherStream(memoryStream, null, cipher);
			content.Write(cipherStream);
			Platform.Dispose(cipherStream);
			encryptedContent = new BerOctetString(memoryStream.ToArray());
		}
		catch (SecurityUtilityException e)
		{
			throw new CmsException("couldn't create cipher.", e);
		}
		catch (InvalidKeyException e2)
		{
			throw new CmsException("key invalid in message.", e2);
		}
		catch (IOException e3)
		{
			throw new CmsException("exception decoding algorithm parameters.", e3);
		}
		Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
		foreach (RecipientInfoGenerator recipientInfoGenerator in recipientInfoGenerators)
		{
			try
			{
				asn1EncodableVector.Add(recipientInfoGenerator.Generate(keyParameter, rand));
			}
			catch (InvalidKeyException e4)
			{
				throw new CmsException("key inappropriate for algorithm.", e4);
			}
			catch (GeneralSecurityException e5)
			{
				throw new CmsException("error making encrypted content.", e5);
			}
		}
		EncryptedContentInfo encryptedContentInfo = new EncryptedContentInfo(CmsObjectIdentifiers.Data, algorithmIdentifier, encryptedContent);
		Asn1Set unprotectedAttrs = null;
		if (unprotectedAttributeGenerator != null)
		{
			unprotectedAttrs = new BerSet(unprotectedAttributeGenerator.GetAttributes(Platform.CreateHashtable()).ToAsn1EncodableVector());
		}
		return new CmsEnvelopedData(new ContentInfo(CmsObjectIdentifiers.EnvelopedData, new EnvelopedData(null, new DerSet(asn1EncodableVector), encryptedContentInfo, unprotectedAttrs)));
	}

	public CmsEnvelopedData Generate(CmsProcessable content, string encryptionOid)
	{
		try
		{
			CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator(encryptionOid);
			keyGenerator.Init(new KeyGenerationParameters(rand, keyGenerator.DefaultStrength));
			return Generate(content, encryptionOid, keyGenerator);
		}
		catch (SecurityUtilityException e)
		{
			throw new CmsException("can't find key generation algorithm.", e);
		}
	}

	public CmsEnvelopedData Generate(CmsProcessable content, string encryptionOid, int keySize)
	{
		try
		{
			CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator(encryptionOid);
			keyGenerator.Init(new KeyGenerationParameters(rand, keySize));
			return Generate(content, encryptionOid, keyGenerator);
		}
		catch (SecurityUtilityException e)
		{
			throw new CmsException("can't find key generation algorithm.", e);
		}
	}
}
