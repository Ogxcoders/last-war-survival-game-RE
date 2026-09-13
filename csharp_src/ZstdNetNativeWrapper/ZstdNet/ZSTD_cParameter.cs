namespace ZstdNet;

public enum ZSTD_cParameter
{
	ZSTD_c_compressionLevel = 100,
	ZSTD_c_windowLog = 101,
	ZSTD_c_hashLog = 102,
	ZSTD_c_chainLog = 103,
	ZSTD_c_searchLog = 104,
	ZSTD_c_minMatch = 105,
	ZSTD_c_targetLength = 106,
	ZSTD_c_strategy = 107,
	ZSTD_c_enableLongDistanceMatching = 160,
	ZSTD_c_ldmHashLog = 161,
	ZSTD_c_ldmMinMatch = 162,
	ZSTD_c_ldmBucketSizeLog = 163,
	ZSTD_c_ldmHashRateLog = 164,
	ZSTD_c_contentSizeFlag = 200,
	ZSTD_c_checksumFlag = 201,
	ZSTD_c_dictIDFlag = 202,
	ZSTD_c_nbWorkers = 400,
	ZSTD_c_jobSize = 401,
	ZSTD_c_overlapLog = 402
}
