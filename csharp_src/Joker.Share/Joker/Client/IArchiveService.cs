using System.IO;
using System.Threading.Tasks;

namespace Joker.Client;

public interface IArchiveService : IService
{
	bool Exists(string path);

	bool ExistsFile(string path);

	bool ExistsDirectory(string path);

	Stream Create(string path);

	Stream OpenRead(string path);

	Stream OpenWrite(string path);

	void Delete(string path);

	bool DeleteFile(string path);

	bool DeleteDirectory(string path, bool recursive = true);

	bool Move(string src, string dst);

	bool MoveFile(string src, string dst);

	bool MoveDirectory(string src, string dst);

	StreamReader OpenText(string file);

	string ReadAllText(string file);

	void WriteAllText(string file, string content);

	void MakeDir(string path);

	bool TryMakeDir(string path);

	string[] GetFiles(string dir);

	string[] GetDirectories(string dir);

	byte[] ReadAllBytes(string file);

	Task<byte[]> ReadAllBytesAsync(string file);
}
