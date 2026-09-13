using System.Collections.Generic;
using System.Text;

namespace System.IO.Compression;

public class ZipArchiveEntry
{
	private sealed class DirectToArchiveWriterStream : Stream
	{
		private long _position;

		private CheckSumAndSizeWriteStream _crcSizeStream;

		private bool _everWritten;

		private bool _isDisposed;

		private ZipArchiveEntry _entry;

		private bool _usedZip64inLH;

		private bool _canWrite;

		public override long Length
		{
			get
			{
				ThrowIfDisposed();
				throw new NotSupportedException("This stream from ZipArchiveEntry does not support seeking.");
			}
		}

		public override long Position
		{
			get
			{
				ThrowIfDisposed();
				return _position;
			}
			set
			{
				ThrowIfDisposed();
				throw new NotSupportedException("This stream from ZipArchiveEntry does not support seeking.");
			}
		}

		public override bool CanRead => false;

		public override bool CanSeek => false;

		public override bool CanWrite => _canWrite;

		public DirectToArchiveWriterStream(CheckSumAndSizeWriteStream crcSizeStream, ZipArchiveEntry entry)
		{
			_position = 0L;
			_crcSizeStream = crcSizeStream;
			_everWritten = false;
			_isDisposed = false;
			_entry = entry;
			_usedZip64inLH = false;
			_canWrite = true;
		}

		private void ThrowIfDisposed()
		{
			if (_isDisposed)
			{
				throw new ObjectDisposedException(GetType().ToString(), "A stream from ZipArchiveEntry has been disposed.");
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			ThrowIfDisposed();
			throw new NotSupportedException("This stream from ZipArchiveEntry does not support reading.");
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			ThrowIfDisposed();
			throw new NotSupportedException("This stream from ZipArchiveEntry does not support seeking.");
		}

		public override void SetLength(long value)
		{
			ThrowIfDisposed();
			throw new NotSupportedException("SetLength requires a stream that supports seeking and writing.");
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "The argument must be non-negative.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "The argument must be non-negative.");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("The offset and length parameters are not valid for the array that was given.");
			}
			ThrowIfDisposed();
			if (count != 0)
			{
				if (!_everWritten)
				{
					_everWritten = true;
					_usedZip64inLH = _entry.WriteLocalFileHeader(isEmptyFile: false);
				}
				_crcSizeStream.Write(buffer, offset, count);
				_position += count;
			}
		}

		public override void Flush()
		{
			ThrowIfDisposed();
			_crcSizeStream.Flush();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && !_isDisposed)
			{
				_crcSizeStream.Dispose();
				if (!_everWritten)
				{
					_entry.WriteLocalFileHeader(isEmptyFile: true);
				}
				else if (_entry._archive.ArchiveStream.CanSeek)
				{
					_entry.WriteCrcAndSizesInLocalHeader(_usedZip64inLH);
				}
				else
				{
					_entry.WriteDataDescriptor();
				}
				_canWrite = false;
				_isDisposed = true;
			}
			base.Dispose(disposing);
		}
	}

	[Flags]
	private enum BitFlagValues : ushort
	{
		DataDescriptor = 8,
		UnicodeFileName = 0x800
	}

	internal enum CompressionMethodValues : ushort
	{
		Stored = 0,
		Deflate = 8,
		Deflate64 = 9,
		BZip2 = 12,
		LZMA = 14
	}

	private const ushort DefaultVersionToExtract = 10;

	private const int MaxSingleBufferSize = 2147483591;

	private ZipArchive _archive;

	private readonly bool _originallyInArchive;

	private readonly int _diskNumberStart;

	private readonly ZipVersionMadeByPlatform _versionMadeByPlatform;

	private ZipVersionNeededValues _versionMadeBySpecification;

	private ZipVersionNeededValues _versionToExtract;

	private BitFlagValues _generalPurposeBitFlag;

	private CompressionMethodValues _storedCompressionMethod;

	private DateTimeOffset _lastModified;

	private long _compressedSize;

	private long _uncompressedSize;

	private long _offsetOfLocalHeader;

	private long? _storedOffsetOfCompressedData;

	private uint _crc32;

	private byte[][] _compressedBytes;

	private MemoryStream _storedUncompressedData;

	private bool _currentlyOpenForWrite;

	private bool _everOpenedForWrite;

	private Stream _outstandingWriteStream;

	private uint _externalFileAttr;

	private string _storedEntryName;

	private byte[] _storedEntryNameBytes;

	private List<ZipGenericExtraField> _cdUnknownExtraFields;

	private List<ZipGenericExtraField> _lhUnknownExtraFields;

	private byte[] _fileComment;

	private CompressionLevel? _compressionLevel;

	private static readonly bool s_allowLargeZipArchiveEntriesInUpdateMode = IntPtr.Size > 4;

	internal static readonly ZipVersionMadeByPlatform CurrentZipPlatform = ((Path.PathSeparator == '/') ? ZipVersionMadeByPlatform.Unix : ZipVersionMadeByPlatform.Windows);

	public ZipArchive Archive => _archive;

	[CLSCompliant(false)]
	public uint Crc32 => _crc32;

	public long CompressedLength
	{
		get
		{
			if (_everOpenedForWrite)
			{
				throw new InvalidOperationException("Length properties are unavailable once an entry has been opened for writing.");
			}
			return _compressedSize;
		}
	}

	public int ExternalAttributes
	{
		get
		{
			return (int)_externalFileAttr;
		}
		set
		{
			ThrowIfInvalidArchive();
			_externalFileAttr = (uint)value;
		}
	}

	public string FullName
	{
		get
		{
			return _storedEntryName;
		}
		private set
		{
			if (value == null)
			{
				throw new ArgumentNullException("FullName");
			}
			_storedEntryNameBytes = EncodeEntryName(value, out var isUTF);
			_storedEntryName = value;
			if (isUTF)
			{
				_generalPurposeBitFlag |= BitFlagValues.UnicodeFileName;
			}
			else
			{
				_generalPurposeBitFlag &= ~BitFlagValues.UnicodeFileName;
			}
			if (ParseFileName(value, _versionMadeByPlatform) == "")
			{
				VersionToExtractAtLeast(ZipVersionNeededValues.ExplicitDirectory);
			}
		}
	}

	public DateTimeOffset LastWriteTime
	{
		get
		{
			return _lastModified;
		}
		set
		{
			ThrowIfInvalidArchive();
			if (_archive.Mode == ZipArchiveMode.Read)
			{
				throw new NotSupportedException("Cannot modify read-only archive.");
			}
			if (_archive.Mode == ZipArchiveMode.Create && _everOpenedForWrite)
			{
				throw new IOException("Cannot modify entry in Create mode after entry has been opened for writing.");
			}
			if (value.DateTime.Year < 1980 || value.DateTime.Year > 2107)
			{
				throw new ArgumentOutOfRangeException("value", "The DateTimeOffset specified cannot be converted into a Zip file timestamp.");
			}
			_lastModified = value;
		}
	}

	public long Length
	{
		get
		{
			if (_everOpenedForWrite)
			{
				throw new InvalidOperationException("Length properties are unavailable once an entry has been opened for writing.");
			}
			return _uncompressedSize;
		}
	}

	public string Name => ParseFileName(FullName, _versionMadeByPlatform);

	internal bool EverOpenedForWrite => _everOpenedForWrite;

	private long OffsetOfCompressedData
	{
		get
		{
			if (!_storedOffsetOfCompressedData.HasValue)
			{
				_archive.ArchiveStream.Seek(_offsetOfLocalHeader, SeekOrigin.Begin);
				if (!ZipLocalFileHeader.TrySkipBlock(_archive.ArchiveReader))
				{
					throw new InvalidDataException("A local file header is corrupt.");
				}
				_storedOffsetOfCompressedData = _archive.ArchiveStream.Position;
			}
			return _storedOffsetOfCompressedData.Value;
		}
	}

	private MemoryStream UncompressedData
	{
		get
		{
			if (_storedUncompressedData == null)
			{
				_storedUncompressedData = new MemoryStream((int)_uncompressedSize);
				if (_originallyInArchive)
				{
					using Stream stream = OpenInReadMode(checkOpenable: false);
					try
					{
						stream.CopyTo(_storedUncompressedData);
					}
					catch (InvalidDataException)
					{
						_storedUncompressedData.Dispose();
						_storedUncompressedData = null;
						_currentlyOpenForWrite = false;
						_everOpenedForWrite = false;
						throw;
					}
				}
				CompressionMethod = CompressionMethodValues.Deflate;
			}
			return _storedUncompressedData;
		}
	}

	private CompressionMethodValues CompressionMethod
	{
		get
		{
			return _storedCompressionMethod;
		}
		set
		{
			switch (value)
			{
			case CompressionMethodValues.Deflate:
				VersionToExtractAtLeast(ZipVersionNeededValues.ExplicitDirectory);
				break;
			case CompressionMethodValues.Deflate64:
				VersionToExtractAtLeast(ZipVersionNeededValues.Deflate64);
				break;
			}
			_storedCompressionMethod = value;
		}
	}

	internal ZipArchiveEntry(ZipArchive archive, ZipCentralDirectoryFileHeader cd)
	{
		_archive = archive;
		_originallyInArchive = true;
		_diskNumberStart = cd.DiskNumberStart;
		_versionMadeByPlatform = (ZipVersionMadeByPlatform)cd.VersionMadeByCompatibility;
		_versionMadeBySpecification = (ZipVersionNeededValues)cd.VersionMadeBySpecification;
		_versionToExtract = (ZipVersionNeededValues)cd.VersionNeededToExtract;
		_generalPurposeBitFlag = (BitFlagValues)cd.GeneralPurposeBitFlag;
		CompressionMethod = (CompressionMethodValues)cd.CompressionMethod;
		_lastModified = new DateTimeOffset(ZipHelper.DosTimeToDateTime(cd.LastModified));
		_compressedSize = cd.CompressedSize;
		_uncompressedSize = cd.UncompressedSize;
		_externalFileAttr = cd.ExternalFileAttributes;
		_offsetOfLocalHeader = cd.RelativeOffsetOfLocalHeader;
		_storedOffsetOfCompressedData = null;
		_crc32 = cd.Crc32;
		_compressedBytes = null;
		_storedUncompressedData = null;
		_currentlyOpenForWrite = false;
		_everOpenedForWrite = false;
		_outstandingWriteStream = null;
		FullName = DecodeEntryName(cd.Filename);
		_lhUnknownExtraFields = null;
		_cdUnknownExtraFields = cd.ExtraFields;
		_fileComment = cd.FileComment;
		_compressionLevel = null;
	}

	internal ZipArchiveEntry(ZipArchive archive, string entryName, CompressionLevel compressionLevel)
		: this(archive, entryName)
	{
		_compressionLevel = compressionLevel;
	}

	internal ZipArchiveEntry(ZipArchive archive, string entryName)
	{
		_archive = archive;
		_originallyInArchive = false;
		_diskNumberStart = 0;
		_versionMadeByPlatform = CurrentZipPlatform;
		_versionMadeBySpecification = ZipVersionNeededValues.Default;
		_versionToExtract = ZipVersionNeededValues.Default;
		_generalPurposeBitFlag = (BitFlagValues)0;
		CompressionMethod = CompressionMethodValues.Deflate;
		_lastModified = DateTimeOffset.Now;
		_compressedSize = 0L;
		_uncompressedSize = 0L;
		_externalFileAttr = 0u;
		_offsetOfLocalHeader = 0L;
		_storedOffsetOfCompressedData = null;
		_crc32 = 0u;
		_compressedBytes = null;
		_storedUncompressedData = null;
		_currentlyOpenForWrite = false;
		_everOpenedForWrite = false;
		_outstandingWriteStream = null;
		FullName = entryName;
		_cdUnknownExtraFields = null;
		_lhUnknownExtraFields = null;
		_fileComment = null;
		_compressionLevel = null;
		if (_storedEntryNameBytes.Length > 65535)
		{
			throw new ArgumentException("Entry names cannot require more than 2^16 bits.");
		}
		if (_archive.Mode == ZipArchiveMode.Create)
		{
			_archive.AcquireArchiveStream(this);
		}
	}

	public void Delete()
	{
		if (_archive != null)
		{
			if (_currentlyOpenForWrite)
			{
				throw new IOException("Cannot delete an entry currently open for writing.");
			}
			if (_archive.Mode != ZipArchiveMode.Update)
			{
				throw new NotSupportedException("Delete can only be used when the archive is in Update mode.");
			}
			_archive.ThrowIfDisposed();
			_archive.RemoveEntry(this);
			_archive = null;
			UnloadStreams();
		}
	}

	public Stream Open()
	{
		ThrowIfInvalidArchive();
		return _archive.Mode switch
		{
			ZipArchiveMode.Read => OpenInReadMode(checkOpenable: true), 
			ZipArchiveMode.Create => OpenInWriteMode(), 
			_ => OpenInUpdateMode(), 
		};
	}

	public override string ToString()
	{
		return FullName;
	}

	private string DecodeEntryName(byte[] entryNameBytes)
	{
		Encoding encoding = (((_generalPurposeBitFlag & BitFlagValues.UnicodeFileName) != 0) ? Encoding.UTF8 : ((_archive == null) ? Encoding.UTF8 : (_archive.EntryNameEncoding ?? Encoding.UTF8)));
		return encoding.GetString(entryNameBytes);
	}

	private byte[] EncodeEntryName(string entryName, out bool isUTF8)
	{
		Encoding encoding = ((_archive == null || _archive.EntryNameEncoding == null) ? (ZipHelper.RequiresUnicode(entryName) ? Encoding.UTF8 : Encoding.ASCII) : _archive.EntryNameEncoding);
		isUTF8 = encoding.Equals(Encoding.UTF8);
		return encoding.GetBytes(entryName);
	}

	internal void WriteAndFinishLocalEntry()
	{
		CloseStreams();
		WriteLocalFileHeaderAndDataIfNeeded();
		UnloadStreams();
	}

	internal void WriteCentralDirectoryFileHeader()
	{
		BinaryWriter binaryWriter = new BinaryWriter(_archive.ArchiveStream);
		Zip64ExtraField zip64ExtraField = default(Zip64ExtraField);
		bool flag = false;
		uint value;
		uint value2;
		if (SizesTooLarge())
		{
			flag = true;
			value = uint.MaxValue;
			value2 = uint.MaxValue;
			zip64ExtraField.CompressedSize = _compressedSize;
			zip64ExtraField.UncompressedSize = _uncompressedSize;
		}
		else
		{
			value = (uint)_compressedSize;
			value2 = (uint)_uncompressedSize;
		}
		uint value3;
		if (_offsetOfLocalHeader > uint.MaxValue)
		{
			flag = true;
			value3 = uint.MaxValue;
			zip64ExtraField.LocalHeaderOffset = _offsetOfLocalHeader;
		}
		else
		{
			value3 = (uint)_offsetOfLocalHeader;
		}
		if (flag)
		{
			VersionToExtractAtLeast(ZipVersionNeededValues.Zip64);
		}
		int num = (flag ? zip64ExtraField.TotalSize : 0) + ((_cdUnknownExtraFields != null) ? ZipGenericExtraField.TotalSize(_cdUnknownExtraFields) : 0);
		ushort value4;
		if (num > 65535)
		{
			value4 = (ushort)(flag ? zip64ExtraField.TotalSize : 0);
			_cdUnknownExtraFields = null;
		}
		else
		{
			value4 = (ushort)num;
		}
		binaryWriter.Write(33639248u);
		binaryWriter.Write((byte)_versionMadeBySpecification);
		binaryWriter.Write((byte)CurrentZipPlatform);
		binaryWriter.Write((ushort)_versionToExtract);
		binaryWriter.Write((ushort)_generalPurposeBitFlag);
		binaryWriter.Write((ushort)CompressionMethod);
		binaryWriter.Write(ZipHelper.DateTimeToDosTime(_lastModified.DateTime));
		binaryWriter.Write(_crc32);
		binaryWriter.Write(value);
		binaryWriter.Write(value2);
		binaryWriter.Write((ushort)_storedEntryNameBytes.Length);
		binaryWriter.Write(value4);
		binaryWriter.Write((ushort)((_fileComment != null) ? ((ushort)_fileComment.Length) : 0));
		binaryWriter.Write((ushort)0);
		binaryWriter.Write((ushort)0);
		binaryWriter.Write(_externalFileAttr);
		binaryWriter.Write(value3);
		binaryWriter.Write(_storedEntryNameBytes);
		if (flag)
		{
			zip64ExtraField.WriteBlock(_archive.ArchiveStream);
		}
		if (_cdUnknownExtraFields != null)
		{
			ZipGenericExtraField.WriteAllBlocks(_cdUnknownExtraFields, _archive.ArchiveStream);
		}
		if (_fileComment != null)
		{
			binaryWriter.Write(_fileComment);
		}
	}

	internal bool LoadLocalHeaderExtraFieldAndCompressedBytesIfNeeded()
	{
		if (_originallyInArchive)
		{
			_archive.ArchiveStream.Seek(_offsetOfLocalHeader, SeekOrigin.Begin);
			_lhUnknownExtraFields = ZipLocalFileHeader.GetExtraFields(_archive.ArchiveReader);
		}
		if (!_everOpenedForWrite && _originallyInArchive)
		{
			_compressedBytes = new byte[_compressedSize / 2147483591 + 1][];
			for (int i = 0; i < _compressedBytes.Length - 1; i++)
			{
				_compressedBytes[i] = new byte[2147483591];
			}
			_compressedBytes[_compressedBytes.Length - 1] = new byte[_compressedSize % 2147483591];
			_archive.ArchiveStream.Seek(OffsetOfCompressedData, SeekOrigin.Begin);
			for (int j = 0; j < _compressedBytes.Length - 1; j++)
			{
				ZipHelper.ReadBytes(_archive.ArchiveStream, _compressedBytes[j], 2147483591);
			}
			ZipHelper.ReadBytes(_archive.ArchiveStream, _compressedBytes[_compressedBytes.Length - 1], (int)(_compressedSize % 2147483591));
		}
		return true;
	}

	internal void ThrowIfNotOpenable(bool needToUncompress, bool needToLoadIntoMemory)
	{
		if (!IsOpenable(needToUncompress, needToLoadIntoMemory, out var message))
		{
			throw new InvalidDataException(message);
		}
	}

	private CheckSumAndSizeWriteStream GetDataCompressor(Stream backingStream, bool leaveBackingStreamOpen, EventHandler onClose)
	{
		DeflateStream baseStream = (_compressionLevel.HasValue ? new DeflateStream(backingStream, _compressionLevel.Value, leaveBackingStreamOpen) : new DeflateStream(backingStream, CompressionMode.Compress, leaveBackingStreamOpen));
		bool flag = true;
		bool leaveOpenOnClose = leaveBackingStreamOpen && !flag;
		return new CheckSumAndSizeWriteStream(baseStream, backingStream, leaveOpenOnClose, this, onClose, delegate(long initialPosition, long currentPosition, uint checkSum, Stream backing, ZipArchiveEntry thisRef, EventHandler closeHandler)
		{
			thisRef._crc32 = checkSum;
			thisRef._uncompressedSize = currentPosition;
			thisRef._compressedSize = backing.Position - initialPosition;
			closeHandler?.Invoke(thisRef, EventArgs.Empty);
		});
	}

	private Stream GetDataDecompressor(Stream compressedStreamToRead)
	{
		Stream stream = null;
		return CompressionMethod switch
		{
			CompressionMethodValues.Deflate => new DeflateStream(compressedStreamToRead, CompressionMode.Decompress), 
			CompressionMethodValues.Deflate64 => new DeflateManagedStream(compressedStreamToRead, CompressionMethodValues.Deflate64), 
			_ => compressedStreamToRead, 
		};
	}

	private Stream OpenInReadMode(bool checkOpenable)
	{
		if (checkOpenable)
		{
			ThrowIfNotOpenable(needToUncompress: true, needToLoadIntoMemory: false);
		}
		Stream compressedStreamToRead = new SubReadStream(_archive.ArchiveStream, OffsetOfCompressedData, _compressedSize);
		return GetDataDecompressor(compressedStreamToRead);
	}

	private Stream OpenInWriteMode()
	{
		if (_everOpenedForWrite)
		{
			throw new IOException("Entries in create mode may only be written to once, and only one entry may be held open at a time.");
		}
		_everOpenedForWrite = true;
		CheckSumAndSizeWriteStream dataCompressor = GetDataCompressor(_archive.ArchiveStream, leaveBackingStreamOpen: true, delegate(object o, EventArgs e)
		{
			ZipArchiveEntry zipArchiveEntry = (ZipArchiveEntry)o;
			zipArchiveEntry._archive.ReleaseArchiveStream(zipArchiveEntry);
			zipArchiveEntry._outstandingWriteStream = null;
		});
		_outstandingWriteStream = new DirectToArchiveWriterStream(dataCompressor, this);
		return new WrappedStream(_outstandingWriteStream, closeBaseStream: true);
	}

	private Stream OpenInUpdateMode()
	{
		if (_currentlyOpenForWrite)
		{
			throw new IOException("Entries cannot be opened multiple times in Update mode.");
		}
		ThrowIfNotOpenable(needToUncompress: true, needToLoadIntoMemory: true);
		_everOpenedForWrite = true;
		_currentlyOpenForWrite = true;
		UncompressedData.Seek(0L, SeekOrigin.Begin);
		return new WrappedStream(UncompressedData, this, delegate(ZipArchiveEntry thisRef)
		{
			thisRef._currentlyOpenForWrite = false;
		});
	}

	private bool IsOpenable(bool needToUncompress, bool needToLoadIntoMemory, out string message)
	{
		message = null;
		if (_originallyInArchive)
		{
			if (needToUncompress && CompressionMethod != CompressionMethodValues.Stored && CompressionMethod != CompressionMethodValues.Deflate && CompressionMethod != CompressionMethodValues.Deflate64)
			{
				CompressionMethodValues compressionMethod = CompressionMethod;
				if (compressionMethod == CompressionMethodValues.BZip2 || compressionMethod == CompressionMethodValues.LZMA)
				{
					message = global::SR.Format("The archive entry was compressed using {0} and is not supported.", CompressionMethod.ToString());
				}
				else
				{
					message = "The archive entry was compressed using an unsupported compression method.";
				}
				return false;
			}
			if (_diskNumberStart != _archive.NumberOfThisDisk)
			{
				message = "Split or spanned archives are not supported.";
				return false;
			}
			if (_offsetOfLocalHeader > _archive.ArchiveStream.Length)
			{
				message = "A local file header is corrupt.";
				return false;
			}
			_archive.ArchiveStream.Seek(_offsetOfLocalHeader, SeekOrigin.Begin);
			if (!ZipLocalFileHeader.TrySkipBlock(_archive.ArchiveReader))
			{
				message = "A local file header is corrupt.";
				return false;
			}
			if (OffsetOfCompressedData + _compressedSize > _archive.ArchiveStream.Length)
			{
				message = "A local file header is corrupt.";
				return false;
			}
			if (needToLoadIntoMemory && _compressedSize > int.MaxValue && !s_allowLargeZipArchiveEntriesInUpdateMode)
			{
				message = "Entries larger than 4GB are not supported in Update mode.";
				return false;
			}
		}
		return true;
	}

	private bool SizesTooLarge()
	{
		if (_compressedSize <= uint.MaxValue)
		{
			return _uncompressedSize > uint.MaxValue;
		}
		return true;
	}

	private bool WriteLocalFileHeader(bool isEmptyFile)
	{
		BinaryWriter binaryWriter = new BinaryWriter(_archive.ArchiveStream);
		Zip64ExtraField zip64ExtraField = default(Zip64ExtraField);
		bool flag = false;
		uint value;
		uint value2;
		if (isEmptyFile)
		{
			CompressionMethod = CompressionMethodValues.Stored;
			value = 0u;
			value2 = 0u;
		}
		else if (_archive.Mode == ZipArchiveMode.Create && !_archive.ArchiveStream.CanSeek && !isEmptyFile)
		{
			_generalPurposeBitFlag |= BitFlagValues.DataDescriptor;
			flag = false;
			value = 0u;
			value2 = 0u;
		}
		else if (SizesTooLarge())
		{
			flag = true;
			value = uint.MaxValue;
			value2 = uint.MaxValue;
			zip64ExtraField.CompressedSize = _compressedSize;
			zip64ExtraField.UncompressedSize = _uncompressedSize;
			VersionToExtractAtLeast(ZipVersionNeededValues.Zip64);
		}
		else
		{
			flag = false;
			value = (uint)_compressedSize;
			value2 = (uint)_uncompressedSize;
		}
		_offsetOfLocalHeader = binaryWriter.BaseStream.Position;
		int num = (flag ? zip64ExtraField.TotalSize : 0) + ((_lhUnknownExtraFields != null) ? ZipGenericExtraField.TotalSize(_lhUnknownExtraFields) : 0);
		ushort value3;
		if (num > 65535)
		{
			value3 = (ushort)(flag ? zip64ExtraField.TotalSize : 0);
			_lhUnknownExtraFields = null;
		}
		else
		{
			value3 = (ushort)num;
		}
		binaryWriter.Write(67324752u);
		binaryWriter.Write((ushort)_versionToExtract);
		binaryWriter.Write((ushort)_generalPurposeBitFlag);
		binaryWriter.Write((ushort)CompressionMethod);
		binaryWriter.Write(ZipHelper.DateTimeToDosTime(_lastModified.DateTime));
		binaryWriter.Write(_crc32);
		binaryWriter.Write(value);
		binaryWriter.Write(value2);
		binaryWriter.Write((ushort)_storedEntryNameBytes.Length);
		binaryWriter.Write(value3);
		binaryWriter.Write(_storedEntryNameBytes);
		if (flag)
		{
			zip64ExtraField.WriteBlock(_archive.ArchiveStream);
		}
		if (_lhUnknownExtraFields != null)
		{
			ZipGenericExtraField.WriteAllBlocks(_lhUnknownExtraFields, _archive.ArchiveStream);
		}
		return flag;
	}

	private void WriteLocalFileHeaderAndDataIfNeeded()
	{
		if (_storedUncompressedData != null || _compressedBytes != null)
		{
			if (_storedUncompressedData != null)
			{
				_uncompressedSize = _storedUncompressedData.Length;
				using Stream destination = new DirectToArchiveWriterStream(GetDataCompressor(_archive.ArchiveStream, leaveBackingStreamOpen: true, null), this);
				_storedUncompressedData.Seek(0L, SeekOrigin.Begin);
				_storedUncompressedData.CopyTo(destination);
				_storedUncompressedData.Dispose();
				_storedUncompressedData = null;
				return;
			}
			if (_uncompressedSize == 0L)
			{
				CompressionMethod = CompressionMethodValues.Stored;
			}
			WriteLocalFileHeader(isEmptyFile: false);
			byte[][] compressedBytes = _compressedBytes;
			foreach (byte[] array in compressedBytes)
			{
				_archive.ArchiveStream.Write(array, 0, array.Length);
			}
		}
		else if (_archive.Mode == ZipArchiveMode.Update || !_everOpenedForWrite)
		{
			_everOpenedForWrite = true;
			WriteLocalFileHeader(isEmptyFile: true);
		}
	}

	private void WriteCrcAndSizesInLocalHeader(bool zip64HeaderUsed)
	{
		long position = _archive.ArchiveStream.Position;
		BinaryWriter binaryWriter = new BinaryWriter(_archive.ArchiveStream);
		bool num = SizesTooLarge();
		bool flag = num && !zip64HeaderUsed;
		uint value = (uint)(num ? uint.MaxValue : _compressedSize);
		uint value2 = (uint)(num ? uint.MaxValue : _uncompressedSize);
		if (flag)
		{
			_generalPurposeBitFlag |= BitFlagValues.DataDescriptor;
			_archive.ArchiveStream.Seek(_offsetOfLocalHeader + 6, SeekOrigin.Begin);
			binaryWriter.Write((ushort)_generalPurposeBitFlag);
		}
		_archive.ArchiveStream.Seek(_offsetOfLocalHeader + 14, SeekOrigin.Begin);
		if (!flag)
		{
			binaryWriter.Write(_crc32);
			binaryWriter.Write(value);
			binaryWriter.Write(value2);
		}
		else
		{
			binaryWriter.Write(0u);
			binaryWriter.Write(0u);
			binaryWriter.Write(0u);
		}
		if (zip64HeaderUsed)
		{
			_archive.ArchiveStream.Seek(_offsetOfLocalHeader + 30 + _storedEntryNameBytes.Length + 4, SeekOrigin.Begin);
			binaryWriter.Write(_uncompressedSize);
			binaryWriter.Write(_compressedSize);
			_archive.ArchiveStream.Seek(position, SeekOrigin.Begin);
		}
		_archive.ArchiveStream.Seek(position, SeekOrigin.Begin);
		if (flag)
		{
			binaryWriter.Write(_crc32);
			binaryWriter.Write(_compressedSize);
			binaryWriter.Write(_uncompressedSize);
		}
	}

	private void WriteDataDescriptor()
	{
		BinaryWriter binaryWriter = new BinaryWriter(_archive.ArchiveStream);
		binaryWriter.Write(134695760u);
		binaryWriter.Write(_crc32);
		if (SizesTooLarge())
		{
			binaryWriter.Write(_compressedSize);
			binaryWriter.Write(_uncompressedSize);
		}
		else
		{
			binaryWriter.Write((uint)_compressedSize);
			binaryWriter.Write((uint)_uncompressedSize);
		}
	}

	private void UnloadStreams()
	{
		if (_storedUncompressedData != null)
		{
			_storedUncompressedData.Dispose();
		}
		_compressedBytes = null;
		_outstandingWriteStream = null;
	}

	private void CloseStreams()
	{
		if (_outstandingWriteStream != null)
		{
			_outstandingWriteStream.Dispose();
		}
	}

	private void VersionToExtractAtLeast(ZipVersionNeededValues value)
	{
		if ((int)_versionToExtract < (int)value)
		{
			_versionToExtract = value;
		}
		if ((int)_versionMadeBySpecification < (int)value)
		{
			_versionMadeBySpecification = value;
		}
	}

	private void ThrowIfInvalidArchive()
	{
		if (_archive == null)
		{
			throw new InvalidOperationException("Cannot modify deleted entry.");
		}
		_archive.ThrowIfDisposed();
	}

	private static string GetFileName_Windows(string path)
	{
		int num = path.Length;
		while (--num >= 0)
		{
			char c = path[num];
			if (c == '\\' || c == '/' || c == ':')
			{
				return path.Substring(num + 1);
			}
		}
		return path;
	}

	private static string GetFileName_Unix(string path)
	{
		int num = path.Length;
		while (--num >= 0)
		{
			if (path[num] == '/')
			{
				return path.Substring(num + 1);
			}
		}
		return path;
	}

	internal static string ParseFileName(string path, ZipVersionMadeByPlatform madeByPlatform)
	{
		return madeByPlatform switch
		{
			ZipVersionMadeByPlatform.Windows => GetFileName_Windows(path), 
			ZipVersionMadeByPlatform.Unix => GetFileName_Unix(path), 
			_ => ParseFileName(path, CurrentZipPlatform), 
		};
	}
}
