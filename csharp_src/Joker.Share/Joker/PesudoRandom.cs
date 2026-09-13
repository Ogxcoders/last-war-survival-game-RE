using System;

namespace Joker;

public class PesudoRandom
{
	private const int N = 624;

	private const int M = 397;

	private const uint MatrixA = 2567483615u;

	private const uint UpperMask = 2147483648u;

	private const int LowerMask = int.MaxValue;

	private const uint SpectralTestMultiplier = 1812433253u;

	private const uint B = 2636928640u;

	private const uint C = 4022730752u;

	private static readonly uint[] Mag01 = new uint[2] { 0u, 2567483615u };

	private readonly uint[] _state = new uint[624];

	private int _index = 625;

	public PesudoRandom()
		: this((uint)Environment.TickCount)
	{
	}

	public PesudoRandom(uint seed)
	{
		Seed(seed);
	}

	public void Seed(uint seed)
	{
		_state[0] = seed;
		for (_index = 1; _index < 624; _index++)
		{
			_state[_index] = (uint)(1812433253 * (_state[_index - 1] ^ (_state[_index - 1] >> 30)) + _index);
		}
	}

	public uint Next()
	{
		if (_index >= 624)
		{
			Twist();
		}
		return Temper(_state[_index++]);
	}

	public uint Next(uint maxValue)
	{
		return (uint)((double)Next() * ((double)maxValue * 2.3283064E-10));
	}

	public uint Next(uint minValue, uint maxValue)
	{
		if (maxValue <= minValue)
		{
			throw new ArgumentException("maxValue must be greater than minValue.");
		}
		uint num = maxValue - minValue;
		return (uint)(int)((ulong)((long)Next() * (long)num) / 4294967296uL) + minValue;
	}

	public double NextDouble()
	{
		return (double)Next() * 2.3283064370807974E-10;
	}

	private void Twist()
	{
		uint num;
		for (int i = 0; i < 623; i++)
		{
			num = (_state[i] & 0x80000000u) | (_state[i + 1] & 0x7FFFFFFF);
			_state[i] = _state[(i + 397) % 624] ^ (num >> 1) ^ Mag01[num & 1];
		}
		num = (_state[623] & 0x80000000u) | (_state[0] & 0x7FFFFFFF);
		_state[623] = _state[396] ^ (num >> 1) ^ Mag01[num & 1];
		_index = 0;
	}

	private static uint Temper(uint next)
	{
		next ^= next >> 11;
		next ^= (next << 7) & 0x9D2C5680u;
		next ^= (next << 15) & 0xEFC60000u;
		next ^= next >> 18;
		return next;
	}
}
