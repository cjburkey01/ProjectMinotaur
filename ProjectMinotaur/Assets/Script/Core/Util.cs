using System;
using System.Security.Cryptography;

public static class Util {

	private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

	// Array extenson.
	public static T[] Copy<T>(this T[] array) {
		T[] ret = new T[array.Length];
		for (int i = 0; i < array.Length; i ++) {
			ret[i] = array[i];
		}
		return ret;
	}

	public static double GetMillis() {
		return DateTime.Now.TimeOfDay.TotalMilliseconds;
	}

	// Inclusive for min and max.
	public static int NextRand(int min, int max) {
		byte[] randomNumber = new byte[1];
		_generator.GetBytes(randomNumber);
		double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);
		double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);
		int range = max - min + 1;
		double randomValueInRange = Math.Floor(multiplier * range);
		return (int) (min + randomValueInRange);
	}

}