using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Mono.Security.X509.Extensions;

namespace Mono.Security.X509;

public class X509Store
{
	private string _storePath;

	private X509CertificateCollection _certificates;

	private ArrayList _crls;

	private bool _crl;

	private bool _newFormat;

	private string _name;

	public X509CertificateCollection Certificates
	{
		get
		{
			if (_certificates == null)
			{
				_certificates = BuildCertificatesCollection(_storePath);
			}
			return _certificates;
		}
	}

	public ArrayList Crls
	{
		get
		{
			if (!_crl)
			{
				_crls = new ArrayList();
			}
			if (_crls == null)
			{
				_crls = BuildCrlsCollection(_storePath);
			}
			return _crls;
		}
	}

	public string Name
	{
		get
		{
			if (_name == null)
			{
				int num = _storePath.LastIndexOf(Path.DirectorySeparatorChar);
				_name = _storePath.Substring(num + 1);
			}
			return _name;
		}
	}

	internal X509Store(string path, bool crl, bool newFormat)
	{
		_storePath = path;
		_crl = crl;
		_newFormat = newFormat;
	}

	public void Clear()
	{
		ClearCertificates();
		ClearCrls();
	}

	private void ClearCertificates()
	{
		if (_certificates != null)
		{
			_certificates.Clear();
		}
		_certificates = null;
	}

	private void ClearCrls()
	{
		if (_crls != null)
		{
			_crls.Clear();
		}
		_crls = null;
	}

	public void Import(X509Certificate certificate)
	{
		CheckStore(_storePath, throwException: true);
		if (_newFormat)
		{
			ImportNewFormat(certificate);
			return;
		}
		string text = Path.Combine(_storePath, GetUniqueName(certificate));
		if (!File.Exists(text))
		{
			text = Path.Combine(_storePath, GetUniqueNameWithSerial(certificate));
			if (!File.Exists(text))
			{
				using (FileStream fileStream = File.Create(text))
				{
					byte[] rawData = certificate.RawData;
					fileStream.Write(rawData, 0, rawData.Length);
					fileStream.Close();
				}
				ClearCertificates();
			}
			return;
		}
		string path = Path.Combine(_storePath, GetUniqueNameWithSerial(certificate));
		if (GetUniqueNameWithSerial(LoadCertificate(text)) != GetUniqueNameWithSerial(certificate))
		{
			using (FileStream fileStream2 = File.Create(path))
			{
				byte[] rawData2 = certificate.RawData;
				fileStream2.Write(rawData2, 0, rawData2.Length);
				fileStream2.Close();
			}
			ClearCertificates();
		}
	}

	public void Import(X509Crl crl)
	{
		CheckStore(_storePath, throwException: true);
		if (_newFormat)
		{
			throw new NotSupportedException();
		}
		string path = Path.Combine(_storePath, GetUniqueName(crl));
		if (!File.Exists(path))
		{
			using (FileStream fileStream = File.Create(path))
			{
				byte[] rawData = crl.RawData;
				fileStream.Write(rawData, 0, rawData.Length);
			}
			ClearCrls();
		}
	}

	public void Remove(X509Certificate certificate)
	{
		if (_newFormat)
		{
			RemoveNewFormat(certificate);
			return;
		}
		string path = Path.Combine(_storePath, GetUniqueNameWithSerial(certificate));
		if (File.Exists(path))
		{
			File.Delete(path);
			ClearCertificates();
			return;
		}
		path = Path.Combine(_storePath, GetUniqueName(certificate));
		if (File.Exists(path))
		{
			File.Delete(path);
			ClearCertificates();
		}
	}

	public void Remove(X509Crl crl)
	{
		if (_newFormat)
		{
			throw new NotSupportedException();
		}
		string path = Path.Combine(_storePath, GetUniqueName(crl));
		if (File.Exists(path))
		{
			File.Delete(path);
			ClearCrls();
		}
	}

	private void ImportNewFormat(X509Certificate certificate)
	{
		using System.Security.Cryptography.X509Certificates.X509Certificate certificate2 = new System.Security.Cryptography.X509Certificates.X509Certificate(certificate.RawData);
		long subjectNameHash = X509Helper2.GetSubjectNameHash(certificate2);
		string path = Path.Combine(_storePath, $"{subjectNameHash:x8}.0");
		if (!File.Exists(path))
		{
			using (FileStream stream = File.Create(path))
			{
				X509Helper2.ExportAsPEM(certificate2, stream, includeHumanReadableForm: true);
			}
			ClearCertificates();
		}
	}

	private void RemoveNewFormat(X509Certificate certificate)
	{
		using System.Security.Cryptography.X509Certificates.X509Certificate certificate2 = new System.Security.Cryptography.X509Certificates.X509Certificate(certificate.RawData);
		long subjectNameHash = X509Helper2.GetSubjectNameHash(certificate2);
		string path = Path.Combine(_storePath, $"{subjectNameHash:x8}.0");
		if (File.Exists(path))
		{
			File.Delete(path);
			ClearCertificates();
		}
	}

	private string GetUniqueNameWithSerial(X509Certificate certificate)
	{
		return GetUniqueName(certificate, certificate.SerialNumber);
	}

	private string GetUniqueName(X509Certificate certificate, byte[] serial = null)
	{
		byte[] array = GetUniqueName(certificate.Extensions, serial);
		string method;
		if (array == null)
		{
			method = "tbp";
			array = certificate.Hash;
		}
		else
		{
			method = "ski";
		}
		return GetUniqueName(method, array, ".cer");
	}

	private string GetUniqueName(X509Crl crl)
	{
		byte[] array = GetUniqueName(crl.Extensions);
		string method;
		if (array == null)
		{
			method = "tbp";
			array = crl.Hash;
		}
		else
		{
			method = "ski";
		}
		return GetUniqueName(method, array, ".crl");
	}

	private byte[] GetUniqueName(X509ExtensionCollection extensions, byte[] serial = null)
	{
		X509Extension x509Extension = extensions["2.5.29.14"];
		if (x509Extension == null)
		{
			return null;
		}
		SubjectKeyIdentifierExtension subjectKeyIdentifierExtension = new SubjectKeyIdentifierExtension(x509Extension);
		if (serial == null)
		{
			return subjectKeyIdentifierExtension.Identifier;
		}
		byte[] array = new byte[subjectKeyIdentifierExtension.Identifier.Length + serial.Length];
		Buffer.BlockCopy(subjectKeyIdentifierExtension.Identifier, 0, array, 0, subjectKeyIdentifierExtension.Identifier.Length);
		Buffer.BlockCopy(serial, 0, array, subjectKeyIdentifierExtension.Identifier.Length, serial.Length);
		return array;
	}

	private string GetUniqueName(string method, byte[] name, string fileExtension)
	{
		StringBuilder stringBuilder = new StringBuilder(method);
		stringBuilder.Append("-");
		foreach (byte b in name)
		{
			stringBuilder.Append(b.ToString("X2", CultureInfo.InvariantCulture));
		}
		stringBuilder.Append(fileExtension);
		return stringBuilder.ToString();
	}

	private byte[] Load(string filename)
	{
		byte[] array = null;
		using FileStream fileStream = File.OpenRead(filename);
		array = new byte[fileStream.Length];
		fileStream.Read(array, 0, array.Length);
		fileStream.Close();
		return array;
	}

	private X509Certificate LoadCertificate(string filename)
	{
		return new X509Certificate(Load(filename));
	}

	private X509Crl LoadCrl(string filename)
	{
		return new X509Crl(Load(filename));
	}

	private bool CheckStore(string path, bool throwException)
	{
		try
		{
			if (Directory.Exists(path))
			{
				return true;
			}
			Directory.CreateDirectory(path);
			return Directory.Exists(path);
		}
		catch
		{
			if (throwException)
			{
				throw;
			}
			return false;
		}
	}

	private X509CertificateCollection BuildCertificatesCollection(string storeName)
	{
		X509CertificateCollection x509CertificateCollection = new X509CertificateCollection();
		string path = Path.Combine(_storePath, storeName);
		if (!CheckStore(path, throwException: false))
		{
			return x509CertificateCollection;
		}
		string[] files = Directory.GetFiles(path, _newFormat ? "*.0" : "*.cer");
		if (files != null && files.Length != 0)
		{
			string[] array = files;
			foreach (string filename in array)
			{
				try
				{
					X509Certificate value = LoadCertificate(filename);
					x509CertificateCollection.Add(value);
				}
				catch
				{
				}
			}
		}
		return x509CertificateCollection;
	}

	private ArrayList BuildCrlsCollection(string storeName)
	{
		ArrayList arrayList = new ArrayList();
		string path = Path.Combine(_storePath, storeName);
		if (!CheckStore(path, throwException: false))
		{
			return arrayList;
		}
		string[] files = Directory.GetFiles(path, "*.crl");
		if (files != null && files.Length != 0)
		{
			string[] array = files;
			foreach (string filename in array)
			{
				try
				{
					X509Crl value = LoadCrl(filename);
					arrayList.Add(value);
				}
				catch
				{
				}
			}
		}
		return arrayList;
	}
}
