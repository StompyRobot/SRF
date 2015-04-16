public static class SRUtil
{

	public static void Swap<T>(ref T one, ref T two)
	{

		var t = one;
		one = two;
		two = t;

	}

}