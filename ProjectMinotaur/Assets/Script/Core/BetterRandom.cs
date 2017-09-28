//using System;
//using System.Security.Cryptography;
using UnityEngine;

public class BetterRandom /*: RandomNumberGenerator*/ {

	/*private static RandomNumberGenerator r;

	public BetterRandom() {
		r = RandomNumberGenerator.Create();
	}

	public override void GetBytes(byte[] data) {
		r.GetBytes(data);
	}

	public override void GetNonZeroBytes(byte[] data) {
		r.GetNonZeroBytes(data);
	}

	public double NextDouble() {
		byte[] b = new byte[4];
		r.GetBytes(b);
		return (double) BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
	}

	public int Next(int min, int max) {
		return (int) Math.Round(NextDouble() * (max - min)) + min;
	}

	public int Next() {
		return Next(0, Int32.MaxValue);
	}

	public int Next(int max) {
		return Next(0, max);
	}*/

	public static int Between(int min, int max, int seed) {
		Random.InitState(seed);
		return Random.Range(min, max);
	}

}