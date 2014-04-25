using System;

namespace Scripts.Framework.Extensions
{
	/// <summary>
	/// Extentions for enums.
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// A FX 3.5 way to mimic the FX4 "HasFlag" method.
		/// </summary>
		/// <param name="variable">The tested enum.</param>
		/// <param name="value">The value to test.</param>
		/// <returns>True if the flag is set. Otherwise false.</returns>
		public static bool HasFlag(this Enum variable, Enum value)
		{
			// check if from the same type.
			if (variable.GetType() != value.GetType()) {
				throw new ArgumentException("The checked flag is not from the same type as the checked variable.");
			}

			Convert.ToUInt64(value);
			ulong num = Convert.ToUInt64(value);
			ulong num2 = Convert.ToUInt64(variable);

			return (num2 & num) == num;
		}
	}
}