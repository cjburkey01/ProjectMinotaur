using UnityEngine;

public class BetterRandom {

	public static int Between(int min, int max, int seed) {
		Random.InitState(seed);
		return Random.Range(min, max);
	}

}