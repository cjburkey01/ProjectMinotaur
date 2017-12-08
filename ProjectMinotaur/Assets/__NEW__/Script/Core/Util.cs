using System;

public static class Util {

	public static double GetMillis() {
		return DateTime.Now.TimeOfDay.TotalMilliseconds;
	}

}