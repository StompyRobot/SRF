using System;

/// <summary>
/// Some extension methods for <see cref="Random"/>
/// https://bitbucket.org/Superbest/superbest-random/overview
/// </summary>
public static class RandomExtensions
{

	/// <summary>
	/// Box-Muller transform random number generation.
	/// </summary>
	/// <param name="r"></param>
	/// <param name = "mean">Mean of the distribution</param>
	/// <param name = "stdDev">Standard deviation</param>
	/// <returns></returns>
	public static double NextGaussian(this Random r, double mean = 0, double stdDev = 1)
	{
		var u1 = r.NextDouble();
		var u2 = r.NextDouble();

		var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
			 Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
		var randNormal =
					 mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)

		return randNormal;
	}

	/// <summary>
	///   Generates values from a triangular distribution.
	/// </summary>
	/// <remarks>
	/// See http://en.wikipedia.org/wiki/Triangular_distribution for a description of the triangular probability distribution and the algorithm for generating one.
	/// </remarks>
	/// <param name="r"></param>
	/// <param name = "a">Minimum</param>
	/// <param name = "b">Maximum</param>
	/// <param name = "c">Mode (most frequent value)</param>
	/// <returns></returns>
	public static double NextTriangular(this Random r, double a, double b, double c)
	{
		var u = r.NextDouble();

		return u < (c - a) / (b - a)
					? a + Math.Sqrt(u * (b - a) * (c - a))
					: b - Math.Sqrt((1 - u) * (b - a) * (b - c));
	}

	/// <summary>
	///   Equally likely to return true or false. Uses <see cref="Random.Next()"/>.
	/// </summary>
	/// <returns></returns>
	public static bool NextBoolean(this Random r)
	{
		return r.Next(2) > 0;
	}

}